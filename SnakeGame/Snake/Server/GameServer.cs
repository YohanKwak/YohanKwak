//Authors: Yohan Kwak and Simon Whidden, December 8th, 2022.
//This class handles server information for the SnakeGame project.
//This includes dealing with multiple client connections, various game object collisions,
//as well as other game logic.
using NetworkUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SnakeGame;

/// <summary>
/// A complex server for receiving snake game information from multiple clients.
/// Sends messages each frame, updating all of the clients.
/// </summary>
public class GameServer
{
    // Fields
    private Dictionary<long, SocketState> clients;
    private World theWorld;
    private Dictionary<long, string> dirChanged;
    private Dictionary<long, Snake> dirSnakeGrowth;
    private int snakeGrowCounter;
    public GameSettings? gameSettings;
    private int MSPerFrame;
    private int FramePerShot;
    private int UniverseSize;
    private int RespawnRate;
    private List<Wall> Walls;
    private List<long> activated;
    private Dictionary<long, int> deathCounter;
    private List<long> wrapAround;
    private int pSpawnCounter;
    private int PowerUpSpawnRate = 200;
    private int powerupCount;
    private bool extraGameMode;

    /// <summary>
    /// Loads server settings from an external file, starts the server,
    /// and continuously updates the clients at given intervals.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        GameServer server = new GameServer();

        string filePath = "settings.txt";
        server.DeserializeToObject(filePath);

        //Loads settings from external file. If the file is not present, informs the user.
        if (server.gameSettings is not null)
        {
            server.MSPerFrame = server.gameSettings.MSPerFrame;
            server.FramePerShot = server.gameSettings.FramesPerShot;
            server.UniverseSize = server.gameSettings.UniverseSize;
            server.RespawnRate = server.gameSettings.RespawnRate;
            server.extraGameMode = server.gameSettings.ExtraGameMode;
            server.Walls = server.gameSettings.Walls;

            if (server.MSPerFrame == 0 || server.FramePerShot == 0 || server.UniverseSize == 0)
            {
                Console.WriteLine("Failed to set server settings.");
            }
            else
            {
                //Starts the server, updating at intervals determined by MSPerFrame,
                //utilizing the stopwatch.
                server.StartServer();

                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    while (watch.ElapsedMilliseconds < server.MSPerFrame)
                    {
                        //nothing
                    }
                    server.Update();
                    watch.Restart();
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to read the server's settings.xml file.");
        }
    }

    /// <summary>
    /// Initialized the server's state.
    /// </summary>
    public GameServer()
    {
        clients = new Dictionary<long, SocketState>();
        dirChanged = new Dictionary<long, string>();
        theWorld = new World();
        Walls = new List<Wall>();
        activated = new List<long>();
        deathCounter = new Dictionary<long, int>();
        dirSnakeGrowth = new Dictionary<long, Snake>();
        wrapAround = new List<long>();
    }

    /// <summary>
    /// Start accepting Tcp socket connections from clients.
    /// Additionally, it initializes the world.
    /// </summary>
    public void StartServer()
    {
        // This begins an "event loop"
        Networking.StartServer(NewClientConnected, 11000);

        theWorld = new World();

        theWorld.worldSize = UniverseSize;

        lock (clients)
        {
            foreach (Wall w in Walls)
            {
                theWorld.walls.Add(w.wallID, w);
            }
        }
        Console.WriteLine("Server is running");
    }

    /// <summary>
    /// Method to be invoked by the networking library when a new client connects.
    /// Adds the client information to the clients dictionary, giving them a unique ID based
    /// on the SocketState's ID.
    /// </summary>
    /// <param name="state">The SocketState representing the new client</param>
    private void NewClientConnected(SocketState state)
    {
        if (state.ErrorOccurred)
            return;

        lock (clients)
        {
            clients[state.ID] = state;
            Console.WriteLine("Accepted new connection");
        }

        state.OnNetworkAction = ReceiveMessage;

        Networking.GetData(state);
    }

    /// <summary>
    /// Method to be invoked by the networking library when a network action occurs.
    /// </summary>
    /// <param name="state">Given SocketState</param>
    private void ReceiveMessage(SocketState state)
    {
        if (state.ErrorOccurred)
        {
            return;
        }

        ProcessMessage(state);

        Networking.GetData(state);
    }


    /// <summary>
    /// Processes client movement information, deserializing it from JSON. When a new client join,
    /// sends them the starting world information. It also generates a new snake for the client.
    /// </summary>
    /// <param name="sender">The SocketState that represents the client</param>
    private void ProcessMessage(SocketState state)
    {

        string totalData = state.GetData();

        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        foreach (string p in parts)
        {
            // Ignore empty strings added by the regex splitter.
            if (p.Length == 0)
                continue;

            // The regex splitter will include the last string even if it doesn't end with a '\n',
            // this prevents such a message from getting through.
            if (p[p.Length - 1] != '\n')
                break;

            Console.WriteLine("received message from client " + state.ID + ": \"" + p.Substring(0, p.Length - 1) + "\"");

            state.RemoveData(0, p.Length);

            //Checks given client movement information, and if it is not movement information,
            //generates new snake for the client and sends world's initial information.
            try
            {
                JObject obj = JObject.Parse(p);
                JToken token = obj["moving"]!;

                lock (clients)
                {
                    if (token.ToString().Equals("up"))
                    {
                        dirChanged.Remove(state.ID);
                        dirChanged[state.ID] = "up";
                    }
                    else if (token.ToString().Equals("down"))
                    {
                        dirChanged.Remove(state.ID);
                        dirChanged[state.ID] = "down";
                    }
                    else if (token.ToString().Equals("left"))
                    {
                        dirChanged.Remove(state.ID);
                        dirChanged[state.ID] = "left";
                    }
                    else if (token.ToString().Equals("right"))
                    {
                        dirChanged.Remove(state.ID);
                        dirChanged[state.ID] = "right";
                    }
                }
            }
            catch
            {
                lock (clients)
                {

                    Networking.Send(state.TheSocket, state.ID + "\n" + theWorld.worldSize + "\n");

                    foreach (Wall w in Walls)
                    {
                        Networking.Send(state.TheSocket, JsonConvert.SerializeObject(w) + "\n");
                    }

                    List<Vector2D> list = new List<Vector2D>();

                    Snake s = new Snake(p, Convert.ToInt32(state.ID), list);

                    RespawnSnake(s);

                    Networking.Send(state.TheSocket, JsonConvert.SerializeObject(s) + "\n");

                    theWorld.snakes.Add(Convert.ToInt32(state.ID), s);

                    activated.Add(state.ID);
                }
            }

        }
    }

    /// <summary>
    /// Sends each client the updated world information. Also removes disconnected clients.
    /// </summary>
    private void Update()
    {
        lock (clients)
        {
            //Removes collected powerups from the world.
            foreach (PowerUp p in theWorld.powerups.Values)
            {
                if (p.died)
                {
                    theWorld.powerups.Remove(p.powerID);
                }
            }

            //Generates powerups every given interval as long as there are less than 20.
            if (theWorld.powerups.Count < 20 && pSpawnCounter >= PowerUpSpawnRate)
            {
                GeneratePowerUp();
                pSpawnCounter = 0;
            }
            else
            {
                pSpawnCounter++;
            }

            HashSet<long> disconnectedCLients = new HashSet<long>();

            //Updates the snakes' movement, checks for collisions, and handles snake death and respawning.
            foreach (Snake s in theWorld.snakes.Values)
            {
                if (!deathCounter.ContainsKey(s.snakeID) && !s.disconnected)
                {
                    MoveSnake(s);
                    CheckSnakePowerupCollision(s);
                    CheckSnakeWallCollision(s);
                    CheckSnakeOnSnakeCollision(s);
                }
                else
                {
                    if (deathCounter[s.snakeID] == RespawnRate)
                    {
                        RespawnSnake(s);
                        deathCounter.Remove(s.snakeID);
                    }
                    else
                    {
                        s.died = false;
                        deathCounter[s.snakeID]++;
                    }
                }
            }

            //Removes disconnected clients from the game.
            foreach (SocketState client in clients.Values)
            {
                if (activated.Contains(client.ID))
                {
                    foreach (Snake s in theWorld.snakes.Values)
                    {
                        if (!Networking.Send(client.TheSocket, JsonConvert.SerializeObject(s) + "\n"))
                        {
                            disconnectedCLients.Add(client.ID);
                        }
                    }
                    foreach (PowerUp p in theWorld.powerups.Values)
                    {
                        Networking.Send(client.TheSocket, JsonConvert.SerializeObject(p) + "\n");
                    }
                }
            }

            //When tested, snakes did not disappear upon disconnection. Connecting a chat client showed that snakes had properly disconnected,
            //as their coordinates were no longer being received. We believe this issue is related to the given snake client, NOT our server code.
            foreach (long id in disconnectedCLients)
            {
                RemoveClient(id);
            }
            foreach (long id in disconnectedCLients)
            {
                theWorld.snakes[Convert.ToInt32(id)].died = true;
                theWorld.snakes[Convert.ToInt32(id)].alive = false;
                theWorld.snakes[Convert.ToInt32(id)].disconnected = true;

                foreach (SocketState c in clients.Values)
                {
                    Networking.Send(c.TheSocket, JsonConvert.SerializeObject(theWorld.snakes[Convert.ToInt32(id)] + "\n"));
                }

                theWorld.snakes.Remove(Convert.ToInt32(id));
            }
        }
    }

    /// <summary>
    /// Checks collisions between snakes and walls.
    /// </summary>
    /// <param name="s">Given snake</param>
    private void CheckSnakeWallCollision(Snake s)
    {
        double headX, headY;

        headX = s.body!.Last().X;
        headY = s.body!.Last().Y;

        //Handles possible cases for collisions between walls and a snake's head, accounting for all possible wall orientations.
        foreach (Wall w in theWorld.walls.Values)
        {
            if (w.p1!.X.Equals(w.p2!.X))
            {
                if (w.p1.Y < w.p2.Y)
                {
                    if (w.p1.X - 30 <= headX && headX <= w.p1.X + 30 && headY >= w.p1.Y - 30 && headY <= w.p2.Y + 30)
                    {
                        theWorld.snakes[s.snakeID].died = true;
                        theWorld.snakes[s.snakeID].alive = false;
                        theWorld.snakes[s.snakeID].powerUpsEaten = 0;
                        deathCounter.Add(s.snakeID, 0);
                        break;
                    }
                }
                else
                {
                    if (w.p1.X - 30 <= headX && headX <= w.p1.X + 30 && headY >= w.p2.Y - 30 && headY <= w.p1.Y + 30)
                    {
                        theWorld.snakes[s.snakeID].died = true;
                        theWorld.snakes[s.snakeID].alive = false;
                        theWorld.snakes[s.snakeID].powerUpsEaten = 0;
                        deathCounter.Add(s.snakeID, 0);
                        break;
                    }
                }
            }
            else
            {
                if (w.p1.X < w.p2.X)
                {
                    if (headX >= w.p1.X - 30 && headX <= w.p2.X + 30 && headY >= w.p1.Y - 30 && headY <= w.p1.Y + 30)
                    {
                        theWorld.snakes[s.snakeID].died = true;
                        theWorld.snakes[s.snakeID].alive = false;
                        theWorld.snakes[s.snakeID].powerUpsEaten = 0;
                        deathCounter.Add(s.snakeID, 0);
                        break;
                    }
                }
                else
                {
                    if (headX >= w.p2.X - 30 && headX <= w.p1.X + 30 && headY >= w.p1.Y - 30 && headY <= w.p1.Y + 30)
                    {
                        theWorld.snakes[s.snakeID].died = true;
                        theWorld.snakes[s.snakeID].alive = false;
                        theWorld.snakes[s.snakeID].powerUpsEaten = 0;
                        deathCounter.Add(s.snakeID, 0);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks collision between snakes and powerups.
    /// </summary>
    /// <param name="s">Given snake</param>
    private void CheckSnakePowerupCollision(Snake s)
    {
        double powerupX, powerupY, headX, headY;

        headX = s.body!.Last().X;
        headY = s.body!.Last().Y;

        foreach (PowerUp p in theWorld.powerups.Values)
        {
            bool collided = false;

            powerupX = p.location!.GetX();
            powerupY = p.location.GetY();
            if (powerupX + 6 >= headX - 4 && powerupY - 6 <= headY + 4 && powerupX - 6 <= headX + 4 && powerupY + 6 >= headY - 4)
            {
                collided = true;
                theWorld.powerups[p.powerID].died = true;
            }
            if (collided)
            {
                snakeGrowCounter = 0;
                dirSnakeGrowth[s.snakeID] = s;
                s.powerUpsEaten++;
            }
        }
    }

    /// <summary>
    /// Checks snake on snake collision, including self-collisions.
    /// </summary>
    /// <param name="curSnake"></param>
    private void CheckSnakeOnSnakeCollision(Snake curSnake)
    {
        Vector2D p1;
        Vector2D p2;

        //gets two points to check for collision, which are from the snake's head.
        if (curSnake.orientation!.Y == 0)
        {
            if (curSnake.orientation.X > 0)
            {
                p1 = new Vector2D(curSnake.body!.Last().X + 5, curSnake.body!.Last().Y + 5);
                p2 = new Vector2D(curSnake.body!.Last().X + 5, curSnake.body!.Last().Y - 5);
            }
            else
            {
                p1 = new Vector2D(curSnake.body!.Last().X - 5, curSnake.body!.Last().Y + 5);
                p2 = new Vector2D(curSnake.body!.Last().X - 5, curSnake.body!.Last().Y - 5);
            }
        }
        else
        {
            if (curSnake.orientation.Y > 0)
            {
                p1 = new Vector2D(curSnake.body!.Last().X + 5, curSnake.body!.Last().Y + 5);
                p2 = new Vector2D(curSnake.body!.Last().X - 5, curSnake.body!.Last().Y + 5);
            }
            else
            {
                p1 = new Vector2D(curSnake.body!.Last().X + 5, curSnake.body!.Last().Y - 5);
                p2 = new Vector2D(curSnake.body!.Last().X - 5, curSnake.body!.Last().Y - 5);
            }
        }

        // Checking for all snakes in the world
        foreach (Snake s in theWorld.snakes.Values)
        {
            if (s.alive)
            {
                //Case for self collision
                if (s.snakeID == curSnake.snakeID)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        double x;
                        double y;

                        if (i == 0)
                        {
                            x = p1.X;
                            y = p1.Y;
                        }
                        else
                        {
                            x = p2.X;
                            y = p2.Y;
                        }

                        double x1 = s.body!.First().X;
                        double y1 = s.body!.First().Y;
                        double x2 = 0;
                        double y2 = 0;

                        bool oppositeLineMet = false;

                        // It searches from the head to tail, and until it finds the line that is going against the head's vector,
                        // it does not check for collision.
                        for (int j = s.body!.Count - 1; j >= 0; j--)
                        {
                            Vector2D v = s.body[j];

                            if (j == s.body.Count - 1)
                            {
                                //nothing
                            }
                            else
                            {

                                x2 = v.X;
                                y2 = v.Y;

                                //Vertical
                                if (x1.Equals(x2))
                                {
                                    if (y1 < y2) // segment going up
                                    {
                                        if (oppositeLineMet)
                                        {
                                            if (x >= x1 - 5 && x <= x1 + 5 && y >= y1 - 5 && y <= y2 + 5)
                                            {
                                                curSnake.died = true;
                                                curSnake.alive = false;
                                                curSnake.powerUpsEaten = 0;
                                                deathCounter.Add(curSnake.snakeID, 0);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (curSnake.orientation.Y > 0)
                                            {
                                                oppositeLineMet = true;
                                            }
                                        }
                                    }
                                    else // segment going down
                                    {
                                        if (oppositeLineMet)
                                        {
                                            if (x >= x1 - 5 && x <= x1 + 5 && y >= y2 - 5 && y <= y1 + 5)
                                            {
                                                curSnake.died = true;
                                                curSnake.alive = false;
                                                curSnake.powerUpsEaten = 0;
                                                deathCounter.Add(curSnake.snakeID, 0);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (curSnake.orientation.Y < 0)
                                            {
                                                oppositeLineMet = true;
                                            }
                                        }
                                    }
                                }
                                else // Horizontal segment
                                {
                                    if (x1 < x2) // segment going left
                                    {
                                        if (oppositeLineMet)
                                        {
                                            if (x >= x1 - 5 && x <= x2 + 5 && y >= y1 - 5 && y <= y1 + 5)
                                            {
                                                curSnake.died = true;
                                                curSnake.alive = false;
                                                curSnake.powerUpsEaten = 0;
                                                deathCounter.Add(curSnake.snakeID, 0);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (curSnake.orientation.X > 0)
                                            {
                                                oppositeLineMet = true;
                                            }
                                        }
                                    }
                                    else // segment going right
                                    {
                                        if (oppositeLineMet)
                                        {
                                            if (x >= x2 - 5 && x <= x1 + 5 && y >= y1 - 5 && y <= y1 + 5)
                                            {
                                                curSnake.died = true;
                                                curSnake.alive = false;
                                                curSnake.powerUpsEaten = 0;
                                                deathCounter.Add(curSnake.snakeID, 0);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (curSnake.orientation.X < 0)
                                            {
                                                oppositeLineMet = true;
                                            }
                                        }
                                    }
                                }
                                x1 = x2;
                                y1 = y2;
                            }
                        }
                    }
                }
                else // different snakes collision, same logic except for looking for segment going opposite direction.
                {
                    for (int i = 0; i < 2; i++)
                    {
                        double x;
                        double y;

                        if (i == 0)
                        {
                            x = p1.X;
                            y = p1.Y;
                        }
                        else
                        {
                            x = p2.X;
                            y = p2.Y;
                        }

                        double x1 = s.body!.First().X;
                        double y1 = s.body!.First().Y;
                        double x2 = 0;
                        double y2 = 0;
                        foreach (Vector2D v in s.body!)
                        {
                            if (x1.Equals(v.X) && y1.Equals(v.Y))
                            {
                                //nothing
                            }
                            else
                            {
                                x2 = v.X;
                                y2 = v.Y;

                                if (x1.Equals(x2)) // Vertical segment
                                {
                                    if (y1 < y2) // segment going up
                                    {
                                        if (x >= x1 - 5 && x <= x1 + 5 && y >= y1 - 5 && y <= y2 + 5)
                                        {
                                            curSnake.died = true;
                                            curSnake.alive = false;
                                            curSnake.powerUpsEaten = 0;
                                            deathCounter.Add(curSnake.snakeID, 0);
                                            return;
                                        }
                                    }
                                    else // segment going down
                                    {
                                        if (x >= x1 - 5 && x <= x1 + 5 && y >= y2 - 5 && y <= y1 + 5)
                                        {
                                            curSnake.died = true;
                                            curSnake.alive = false;
                                            curSnake.powerUpsEaten = 0;
                                            deathCounter.Add(curSnake.snakeID, 0);
                                            return;
                                        }
                                    }
                                }
                                else // Horizontal segment
                                {
                                    if (x1 < x2) // segment going left
                                    {
                                        if (x >= x1 - 5 && x <= x2 + 5 && y >= y1 - 5 && y <= y1 + 5)
                                        {
                                            curSnake.died = true;
                                            curSnake.alive = false;
                                            curSnake.powerUpsEaten = 0;
                                            deathCounter.Add(curSnake.snakeID, 0);
                                            return;
                                        }
                                    }
                                    else // segment going right
                                    {
                                        if (x >= x2 - 5 && x <= x1 + 5 && y >= y1 - 5 && y <= y1 + 5)
                                        {
                                            curSnake.died = true;
                                            curSnake.alive = false;
                                            curSnake.powerUpsEaten = 0;
                                            deathCounter.Add(curSnake.snakeID, 0);
                                            return;
                                        }
                                    }
                                }
                                x1 = x2;
                                y1 = y2;
                            }
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// Checks collisions to determine the position to spawn object.
    /// </summary>
    /// <param name="x"> x to check</param>
    /// <param name="y"> y to check</param>
    /// <returns></returns>
    private bool CheckSpawnCollision(double x, double y)
    {
        bool collided = false;

        //checks with wall if the given position is within walls
        foreach (Wall w in theWorld.walls.Values)
        {
            if (w.p1!.X.Equals(w.p2!.X))
            {
                if (w.p1.Y < w.p2.Y)
                {
                    if (w.p1.X - 35 <= x && x <= w.p1.X + 35 && y >= w.p1.Y - 35 && y <= w.p2.Y + 35)
                    {
                        collided = true;
                        break;
                    }
                }
                else
                {
                    if (w.p1.X - 35 <= x && x <= w.p1.X + 35 && y >= w.p2.Y - 35 && y <= w.p1.Y + 35)
                    {
                        collided = true;
                        break;
                    }
                }
            }
            else
            {
                if (w.p1.X < w.p2.X)
                {
                    if (x >= w.p1.X - 35 && x <= w.p2.X + 35 && y >= w.p1.Y - 35 && y <= w.p1.Y + 35)
                    {
                        collided = true;
                        break;
                    }
                }
                else
                {
                    if (x >= w.p2.X - 35 && x <= w.p1.X + 35 && y >= w.p1.Y - 35 && y <= w.p1.Y + 35)
                    {
                        collided = true;
                        break;
                    }
                }
            }
        }

        //checks with snakes if the given position is within snakes
        if (!collided)
        {
            foreach (Snake s in theWorld.snakes.Values)
            {
                double x1 = s.body!.First().X;
                double y1 = s.body!.First().Y;
                double x2 = 0;
                double y2 = 0;
                foreach (Vector2D v in s.body!)
                {
                    if (x1.Equals(v.X) && y1.Equals(v.Y))
                    {
                        //nothing
                    }
                    else
                    {
                        x2 = v.X;
                        y2 = v.Y;

                        if (x1.Equals(x2))
                        {
                            if (y1 < y2)
                            {
                                if (x + 8 >= x1 - 5 && x - 8 <= x1 + 5 && y + 8 >= y1 - 5 && y - 8 <= y2 + 5)
                                {
                                    collided = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (x + 8 >= x1 - 5 && x - 8 <= x1 + 5 && y + 8 >= y2 - 5 && y - 8 <= y1 + 5)
                                {
                                    collided = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (x1 < x2)
                            {
                                if (x + 8 >= x1 - 5 && x - 8 <= x2 + 5 && y + 8 >= y1 - 5 && y - 8 <= y1 + 5)
                                {
                                    collided = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (x + 8 >= x2 - 5 && x - 8 <= x1 + 5 && y + 8 >= y1 - 5 && y - 8 <= y1 + 5)
                                {
                                    collided = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        //checks with Power Ups if the given position is within Power Ups
        if (!collided)
        {
            foreach (PowerUp p in theWorld.powerups.Values)
            {
                if (x >= p.location!.X - 16 && x <= p.location.X + 16 && y >= p.location!.Y - 16 && y <= p.location.Y + 16)
                {
                    collided = true;
                    break;
                }
            }
        }
        return collided;
    }

    /// <summary>
    /// Generates new Powerups
    /// </summary>
    private void GeneratePowerUp()
    {
        Random random = new Random();

        bool legitPosition = false;

        double x = 0;
        double y = 0;

        while (!legitPosition)
        {
            x = random.Next(theWorld.worldSize - 150) - ((theWorld.worldSize - 150) / 2);
            y = random.Next(theWorld.worldSize - 150) - ((theWorld.worldSize - 150) / 2);

            legitPosition = !CheckSpawnCollision(x, y);
        }
        PowerUp power = new PowerUp(powerupCount, (new Vector2D(x, y)));
        theWorld.powerups[power.powerID] = power;
        powerupCount++;

    }

    /// <summary>
    /// Moves the snake, according to the given client movement command and current snake head orientation.
    /// If the ExtraGameMode is true, snake's speed increases as it takes more powerup.
    /// </summary>
    /// <param name="s"> given snake to move</param>
    private void MoveSnake(Snake s)
    {
        bool turned = false;

        double snakeSpeed = 3;

        if (extraGameMode)
        {
            snakeSpeed = 3 + (s.powerUpsEaten * 0.1);
            if (snakeSpeed > 6.0)
                snakeSpeed = 6;
        }

        Vector2D tail = s.body![0];
        Vector2D secEnd = s.body![1];
        Vector2D head = s.body[s.body.Count - 1];
        Vector2D secHead = s.body[s.body.Count - 2];

        //When there is given movement command
        if (dirChanged.ContainsKey(s.snakeID) && dirChanged[s.snakeID] is not null)
        {
            if (dirChanged[s.snakeID].Equals("up"))
            {
                if (s.orientation!.GetY() == 0.0)
                {
                    if (s.body!.Count == 2) //snake is a straight line
                    {
                        if (tail.X < head.X) //going right
                        {
                            tail.X += snakeSpeed;
                        }
                        else // going left
                        {
                            tail.X -= snakeSpeed;
                        }
                        if (head.Y - snakeSpeed <= -UniverseSize / 2)
                        {
                            Vector2D toEnd = new Vector2D(head.X, -UniverseSize / 2);
                            Vector2D newstart = new Vector2D(head.X, UniverseSize / 2);
                            Vector2D newHead = new Vector2D(head.X, (UniverseSize / 2) - snakeSpeed);
                            s.body.Add(toEnd);
                            s.body.Add(newstart);
                            s.body.Add(newHead);
                            wrapAround.Add(s.snakeID);
                        }
                        else
                        {
                            Vector2D newVector = new Vector2D(head.X, head.Y - snakeSpeed);
                            s.body.Add(newVector);
                        }
                        turned = true;
                        s.orientation = new Vector2D(0, -1);
                        if (wrapAround.Contains(s.snakeID))
                        {
                            wrapAround.Remove(s.snakeID);
                        }
                    }
                    else // snake has 3+ Vertices
                    {
                        if ((s.body[s.body.Count - 2].Y - s.body[s.body.Count - 3].Y) > 0)
                        {
                            //previous turn was from up as well
                            if (Math.Abs(head.X - secHead.X) >= 10)
                            {
                                ShortenTail(s, tail, secEnd, snakeSpeed);
                                if (head.Y - snakeSpeed <= -UniverseSize / 2)
                                {
                                    Vector2D toEnd = new Vector2D(head.X, -UniverseSize / 2);
                                    Vector2D newstart = new Vector2D(head.X, UniverseSize / 2);
                                    Vector2D newHead = new Vector2D(head.X, (UniverseSize / 2) - snakeSpeed);
                                    s.body.Add(toEnd);
                                    s.body.Add(newstart);
                                    s.body.Add(newHead);
                                    wrapAround.Add(s.snakeID);
                                }
                                else
                                {
                                    Vector2D newVector = new Vector2D(head.X, head.Y - snakeSpeed);
                                    s.body.Add(newVector);
                                }
                                turned = true;
                                s.orientation = new Vector2D(0, -1);
                                if (wrapAround.Contains(s.snakeID))
                                {
                                    wrapAround.Remove(s.snakeID);
                                }
                            }
                        }
                        else
                        {
                            ShortenTail(s, tail, secEnd, snakeSpeed);
                            if (head.Y - snakeSpeed <= -UniverseSize / 2)
                            {
                                Vector2D toEnd = new Vector2D(head.X, -UniverseSize / 2);
                                Vector2D newstart = new Vector2D(head.X, UniverseSize / 2);
                                Vector2D newHead = new Vector2D(head.X, (UniverseSize / 2) - snakeSpeed);
                                s.body.Add(toEnd);
                                s.body.Add(newstart);
                                s.body.Add(newHead);
                                wrapAround.Add(s.snakeID);
                            }
                            else
                            {
                                Vector2D newVector = new Vector2D(head.X, head.Y - snakeSpeed);
                                s.body.Add(newVector);
                            }
                            turned = true;
                            s.orientation = new Vector2D(0, -1);
                            if (wrapAround.Contains(s.snakeID))
                            {
                                wrapAround.Remove(s.snakeID);
                            }
                        }
                    }
                }
                else
                {
                    //nothing, already moving Vertical.
                }
            }
            else if (dirChanged[s.snakeID].Equals("down"))
            {
                if (s.orientation!.GetY() == 0.0)
                {
                    if (s.body!.Count == 2) //snake is a straight line
                    {
                        if (tail.X < head.X) //going right
                        {
                            tail.X += snakeSpeed;
                        }
                        else // going left
                        {
                            tail.X -= snakeSpeed;
                        }
                        if (head.Y + snakeSpeed >= UniverseSize / 2)
                        {
                            Vector2D toEnd = new Vector2D(head.X, UniverseSize / 2);
                            Vector2D newstart = new Vector2D(head.X, -UniverseSize / 2);
                            Vector2D newHead = new Vector2D(head.X, (-UniverseSize / 2) + snakeSpeed);
                            s.body.Add(toEnd);
                            s.body.Add(newstart);
                            s.body.Add(newHead);
                            wrapAround.Add(s.snakeID);
                        }
                        else
                        {
                            Vector2D newVector = new Vector2D(head.X, head.Y + snakeSpeed);
                            s.body.Add(newVector);
                        }
                        turned = true;
                        s.orientation = new Vector2D(0, 1);
                        if (wrapAround.Contains(s.snakeID))
                        {
                            wrapAround.Remove(s.snakeID);
                        }
                    }
                    else // snake has 3+ Vertices
                    {
                        if ((s.body[s.body.Count - 2].Y - s.body[s.body.Count - 3].Y) > 0)
                        {
                            ShortenTail(s, tail, secEnd, snakeSpeed);
                            if (head.Y + snakeSpeed >= UniverseSize / 2)
                            {
                                Vector2D toEnd = new Vector2D(head.X, UniverseSize / 2);
                                Vector2D newstart = new Vector2D(head.X, -UniverseSize / 2);
                                Vector2D newHead = new Vector2D(head.X, (-UniverseSize / 2) + snakeSpeed);
                                s.body.Add(toEnd);
                                s.body.Add(newstart);
                                s.body.Add(newHead);
                                wrapAround.Add(s.snakeID);
                            }
                            else
                            {
                                Vector2D newVector = new Vector2D(head.X, head.Y + snakeSpeed);
                                s.body.Add(newVector);
                            }
                            turned = true;
                            s.orientation = new Vector2D(0, 1);
                            if (wrapAround.Contains(s.snakeID))
                            {
                                wrapAround.Remove(s.snakeID);
                            }
                        }
                        else
                        {
                            if (Math.Abs(head.X - secHead.X) >= 10)
                            {
                                ShortenTail(s, tail, secEnd, snakeSpeed);
                                if (head.Y + snakeSpeed >= UniverseSize / 2)
                                {
                                    Vector2D toEnd = new Vector2D(head.X, UniverseSize / 2);
                                    Vector2D newstart = new Vector2D(head.X, -UniverseSize / 2);
                                    Vector2D newHead = new Vector2D(head.X, (-UniverseSize / 2) + snakeSpeed);
                                    s.body.Add(toEnd);
                                    s.body.Add(newstart);
                                    s.body.Add(newHead);
                                    wrapAround.Add(s.snakeID);
                                }
                                else
                                {
                                    Vector2D newVector = new Vector2D(head.X, head.Y + snakeSpeed);
                                    s.body.Add(newVector);
                                }
                                turned = true;
                                s.orientation = new Vector2D(0, 1);
                                if (wrapAround.Contains(s.snakeID))
                                {
                                    wrapAround.Remove(s.snakeID);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //nothing, already moving Vertical.
                }
            }
            else if (dirChanged[s.snakeID].Equals("left"))
            {
                if (s.orientation!.GetX() == 0.0)
                {
                    if (s.body!.Count == 2) //snake is a straight line
                    {
                        if (tail.Y < head.Y) //going down
                        {
                            tail.Y += snakeSpeed;
                        }
                        else // going up
                        {
                            tail.Y -= snakeSpeed;
                        }

                        if (head.X - snakeSpeed <= -UniverseSize / 2)
                        {
                            Vector2D toEnd = new Vector2D(-UniverseSize / 2, head.Y);
                            Vector2D newstart = new Vector2D(UniverseSize / 2, head.Y);
                            Vector2D newHead = new Vector2D((UniverseSize / 2) - snakeSpeed, head.Y);
                            s.body.Add(toEnd);
                            s.body.Add(newstart);
                            s.body.Add(newHead);
                            wrapAround.Add(s.snakeID);
                        }
                        else
                        {
                            Vector2D newVector = new Vector2D(head.X - snakeSpeed, head.Y);
                            s.body.Add(newVector);
                        }
                        turned = true;
                        s.orientation = new Vector2D(-1, 0);
                        if (wrapAround.Contains(s.snakeID))
                        {
                            wrapAround.Remove(s.snakeID);
                        }
                    }
                    else // snake has 3+ Vertices
                    {
                        if ((s.body[s.body.Count - 2].X - s.body[s.body.Count - 3].X) > 0)
                        {
                            if (Math.Abs(head.Y - secHead.Y) >= 10)
                            {
                                ShortenTail(s, tail, secEnd, snakeSpeed);
                                if (head.X - snakeSpeed <= -UniverseSize / 2)
                                {
                                    Vector2D toEnd = new Vector2D(-UniverseSize / 2, head.Y);
                                    Vector2D newstart = new Vector2D(UniverseSize / 2, head.Y);
                                    Vector2D newHead = new Vector2D((UniverseSize / 2) - snakeSpeed, head.Y);
                                    s.body.Add(toEnd);
                                    s.body.Add(newstart);
                                    s.body.Add(newHead);
                                    wrapAround.Add(s.snakeID);
                                }
                                else
                                {
                                    Vector2D newVector = new Vector2D(head.X - snakeSpeed, head.Y);
                                    s.body.Add(newVector);
                                }
                                turned = true;
                                s.orientation = new Vector2D(-1, 0);
                                if (wrapAround.Contains(s.snakeID))
                                {
                                    wrapAround.Remove(s.snakeID);
                                }
                            }
                        }
                        else
                        {
                            ShortenTail(s, tail, secEnd, snakeSpeed);
                            if (head.X - snakeSpeed <= -UniverseSize / 2)
                            {
                                Vector2D toEnd = new Vector2D(-UniverseSize / 2, head.Y);
                                Vector2D newstart = new Vector2D(UniverseSize / 2, head.Y);
                                Vector2D newHead = new Vector2D((UniverseSize / 2) - snakeSpeed, head.Y);
                                s.body.Add(toEnd);
                                s.body.Add(newstart);
                                s.body.Add(newHead);
                                wrapAround.Add(s.snakeID);
                            }
                            else
                            {
                                Vector2D newVector = new Vector2D(head.X - snakeSpeed, head.Y);
                                s.body.Add(newVector);
                            }
                            turned = true;
                            s.orientation = new Vector2D(-1, 0);
                            if (wrapAround.Contains(s.snakeID))
                            {
                                wrapAround.Remove(s.snakeID);
                            }
                        }
                    }
                }
                else
                {
                    //nothing, already moving Horizontal.
                }
            }
            else if (dirChanged[s.snakeID].Equals("right"))
            {
                if (s.orientation!.GetX() == 0.0)
                {
                    if (s.body!.Count == 2) //snake is a straight line
                    {
                        if (tail.Y < s.body[1].Y) //going down
                        {
                            tail.Y += snakeSpeed;
                        }
                        else // going up
                        {
                            tail.Y -= snakeSpeed;
                        }
                        if (head.X + snakeSpeed >= UniverseSize / 2)
                        {
                            Vector2D toEnd = new Vector2D(UniverseSize / 2, head.Y);
                            Vector2D newstart = new Vector2D(-UniverseSize / 2, head.Y);
                            Vector2D newHead = new Vector2D((-UniverseSize / 2) + snakeSpeed, head.Y);
                            s.body.Add(toEnd);
                            s.body.Add(newstart);
                            s.body.Add(newHead);
                            wrapAround.Add(s.snakeID);
                        }
                        else
                        {
                            Vector2D newVector = new Vector2D(s.body[1].X + snakeSpeed, s.body[1].Y);
                            s.body.Add(newVector);
                        }
                        turned = true;
                        s.orientation = new Vector2D(1, 0);
                        if (wrapAround.Contains(s.snakeID))
                        {
                            wrapAround.Remove(s.snakeID);
                        }
                    }
                    else // snake has 3+ Vertices
                    {
                        if ((s.body[s.body.Count - 2].X - s.body[s.body.Count - 3].X) > 0)
                        {
                            ShortenTail(s, tail, secEnd, snakeSpeed);
                            if (head.X + snakeSpeed >= UniverseSize / 2)
                            {
                                Vector2D toEnd = new Vector2D(UniverseSize / 2, head.Y);
                                Vector2D newstart = new Vector2D(-UniverseSize / 2, head.Y);
                                Vector2D newHead = new Vector2D((-UniverseSize / 2) + snakeSpeed, head.Y);
                                s.body.Add(toEnd);
                                s.body.Add(newstart);
                                s.body.Add(newHead);
                                wrapAround.Add(s.snakeID);
                            }
                            else
                            {
                                Vector2D newVector = new Vector2D(head.X + snakeSpeed, head.Y);
                                s.body.Add(newVector);
                            }
                            turned = true;
                            s.orientation = new Vector2D(1, 0);
                            if (wrapAround.Contains(s.snakeID))
                            {
                                wrapAround.Remove(s.snakeID);
                            }
                        }
                        else
                        {
                            if (Math.Abs(head.Y - secHead.Y) >= 10)
                            {
                                ShortenTail(s, tail, secEnd, snakeSpeed);
                                if (head.X + snakeSpeed >= UniverseSize / 2)
                                {
                                    Vector2D toEnd = new Vector2D(UniverseSize / 2, head.Y);
                                    Vector2D newstart = new Vector2D(-UniverseSize / 2, head.Y);
                                    Vector2D newHead = new Vector2D((-UniverseSize / 2) + snakeSpeed, head.Y);
                                    s.body.Add(toEnd);
                                    s.body.Add(newstart);
                                    s.body.Add(newHead);
                                    wrapAround.Add(s.snakeID);
                                }
                                else
                                {
                                    Vector2D newVector = new Vector2D(head.X + snakeSpeed, head.Y);
                                    s.body.Add(newVector);
                                }
                                turned = true;
                                s.orientation = new Vector2D(1, 0);
                                if (wrapAround.Contains(s.snakeID))
                                {
                                    wrapAround.Remove(s.snakeID);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //nothing, already moving Horizontal.
                }
            }
        }

        dirChanged.Remove(s.snakeID);

        //When no turning has been done.
        if (!turned)
        {
            ShortenTail(s, tail, secEnd, snakeSpeed);

            //lengthen the head part of the body.
            if (head.X == secHead.X)
            {
                if (head.Y < secHead.Y)
                {
                    if (head.Y - snakeSpeed <= -UniverseSize / 2)
                    {
                        Vector2D toEnd = new Vector2D(head.X, -UniverseSize / 2);
                        Vector2D newstart = new Vector2D(head.X, UniverseSize / 2);
                        Vector2D newHead = new Vector2D(head.X, (UniverseSize / 2) - snakeSpeed);
                        s.body.Add(toEnd);
                        s.body.Add(newstart);
                        s.body.Add(newHead);
                        wrapAround.Add(s.snakeID);
                    }
                    else
                    {
                        if (wrapAround.Contains(s.snakeID))
                        {
                            Vector2D newHead = new Vector2D(head.X, head.Y - snakeSpeed);
                            s.body.Add(newHead);
                            wrapAround.Remove(s.snakeID);
                        }
                        else
                        {
                            head.Y -= snakeSpeed;
                        }
                    }
                }
                else
                {
                    if (head.Y + snakeSpeed >= UniverseSize / 2)
                    {
                        Vector2D toEnd = new Vector2D(head.X, UniverseSize / 2);
                        Vector2D newstart = new Vector2D(head.X, -UniverseSize / 2);
                        Vector2D newHead = new Vector2D(head.X, (-UniverseSize / 2) + snakeSpeed);
                        s.body.Add(toEnd);
                        s.body.Add(newstart);
                        s.body.Add(newHead);
                        wrapAround.Add(s.snakeID);
                    }
                    else
                    {
                        if (wrapAround.Contains(s.snakeID))
                        {
                            Vector2D newHead = new Vector2D(head.X, head.Y + snakeSpeed);
                            s.body.Add(newHead);
                            wrapAround.Remove(s.snakeID);
                        }
                        else
                        {
                            head.Y += snakeSpeed;
                        }
                    }
                }
            }
            else
            {
                if (head.X < secHead.X)
                {
                    if (head.X - snakeSpeed <= -UniverseSize / 2)
                    {
                        Vector2D toEnd = new Vector2D(-UniverseSize / 2, head.Y);
                        Vector2D newstart = new Vector2D(UniverseSize / 2, head.Y);
                        Vector2D newHead = new Vector2D((UniverseSize / 2) - snakeSpeed, head.Y);
                        s.body.Add(toEnd);
                        s.body.Add(newstart);
                        s.body.Add(newHead);
                        wrapAround.Add(s.snakeID);
                    }
                    else
                    {
                        if (wrapAround.Contains(s.snakeID))
                        {
                            Vector2D newHead = new Vector2D(head.X - snakeSpeed, head.Y);
                            s.body.Add(newHead);
                            wrapAround.Remove(s.snakeID);
                        }
                        else
                        {
                            head.X -= snakeSpeed;
                        }
                    }
                }
                else
                {
                    if (head.X + snakeSpeed >= UniverseSize / 2)
                    {
                        Vector2D toEnd = new Vector2D(UniverseSize / 2, head.Y);
                        Vector2D newstart = new Vector2D(-UniverseSize / 2, head.Y);
                        Vector2D newHead = new Vector2D((-UniverseSize / 2) + snakeSpeed, head.Y);

                        s.body.Add(toEnd);
                        s.body.Add(newstart);
                        s.body.Add(newHead);
                        wrapAround.Add(s.snakeID);
                    }
                    else
                    {
                        if (wrapAround.Contains(s.snakeID))
                        {
                            Vector2D newHead = new Vector2D(head.X + snakeSpeed, head.Y);
                            s.body.Add(newHead);
                            wrapAround.Remove(s.snakeID);
                        }
                        else
                        {
                            head.X += snakeSpeed;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Shortens the tail of the snake, using the first two body's vector2D, accoring to given snakeSpeed.
    /// </summary>
    /// <param name="s"> snake to shorten tail</param>
    /// <param name="tail"> tail vector</param>
    /// <param name="secEnd"> second vector from tail</param>
    /// <param name="snakeSpeed"> snake velocity</param>
    private void ShortenTail(Snake s, Vector2D tail, Vector2D secEnd, double snakeSpeed)
    {
        //does not shorten tail if snake recently collected a powerup.
        if (dirSnakeGrowth.ContainsKey(s.snakeID) && snakeGrowCounter < 12)
        {
            snakeGrowCounter++;
            return;
        }

        //shortening tail of snake
        if (tail.X == secEnd.X)
        {
            if (tail.Y < secEnd.Y)
            {
                tail.Y += snakeSpeed;

                if (tail.Y >= secEnd.Y)
                {
                    if (secEnd.Y >= UniverseSize / 2)
                    {
                        s.body!.RemoveAt(0);
                        s.body!.RemoveAt(0);
                    }
                    s.body!.RemoveAt(0);
                }
            }
            else
            {
                tail.Y -= snakeSpeed;

                if (tail.Y <= secEnd.Y)
                {
                    if (secEnd.Y <= -UniverseSize / 2)
                    {
                        s.body!.RemoveAt(0);
                        s.body!.RemoveAt(0);
                    }
                    s.body!.RemoveAt(0);
                }
            }
        }
        else
        {
            if (tail.X < secEnd.X)
            {
                tail.X += snakeSpeed;

                if (tail.X >= secEnd.X)
                {
                    if (secEnd.X >= UniverseSize / 2)
                    {
                        s.body!.RemoveAt(0);
                        s.body!.RemoveAt(0);
                    }
                    s.body!.RemoveAt(0);
                }
            }
            else
            {
                tail.X -= snakeSpeed;

                if (tail.X <= secEnd.X)
                {
                    if (secEnd.X <= -UniverseSize / 2)
                    {
                        s.body!.RemoveAt(0);
                        s.body!.RemoveAt(0);
                    }
                    s.body!.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// respawns the snake.
    /// </summary>
    /// <param name="s">snake to respawn</param>
    private void RespawnSnake(Snake s)
    {
        //get random 2points
        //check with wall and other snakes and power ups

        double x1, x2, y1, y2;
        Random r = new Random();
        while (true)
        {

            double differenceX = (r.NextDouble() * (UniverseSize - 300)) - ((UniverseSize - 300) / 2);
            double differenceY = (r.NextDouble() * (UniverseSize - 300)) - ((UniverseSize - 300) / 2);

            if (powerupCount % 2 == 0)
            {
                //horizontal;
                if ((int)differenceX % 3 == 0)
                {
                    x1 = differenceX;
                    x2 = differenceX - 120;
                    s.orientation = new Vector2D(-1, 0);
                }
                else
                {
                    x1 = differenceX - 120;
                    x2 = differenceX;
                    s.orientation = new Vector2D(1, 0);
                }
                y1 = differenceY;
                y2 = differenceY;
            }
            else
            {
                //Vertical
                if ((int)differenceY % 3 == 0)
                {
                    y1 = differenceY - 120;
                    y2 = differenceY;
                    s.orientation = new Vector2D(0, 1);
                }
                else
                {
                    y1 = differenceY;
                    y2 = differenceY - 120;
                    s.orientation = new Vector2D(0, -1);
                }
                x1 = differenceX;
                x2 = differenceX;
            }
            if (!CheckSpawnCollision(x1, y1) && !CheckSpawnCollision(x2, y2) && !CheckSpawnCollision((x1 + x2) / 2, (y1 + y2) / 2))
            {
                break;
            }
        }

        // assign approved randomly generated vectos to snake
        Vector2D p1 = new Vector2D();
        p1.X = x1;
        p1.Y = y1;
        Vector2D p2 = new Vector2D();
        p2.X = x2;
        p2.Y = y2;
        if (s.body is not null)
        {
            s.body.Clear();
            s.body.Add(p1);
            s.body.Add(p2);
        }
        else
        {
            s.body = new List<Vector2D>();
            s.body.Add(p1);
            s.body.Add(p2);
        }
        s.alive = true;
    }



    /// <summary>
    /// Removes a client from the clients dictionary
    /// </summary>
    /// <param name="id">The ID of the client</param>
    private void RemoveClient(long id)
    {
        Console.WriteLine("Client " + id + " disconnected");
        lock (clients)
        {
            clients.Remove(id);
            activated.Remove(id);
        }
    }

    /// <summary>
    /// Deserializes given external setting file.
    /// </summary>
    /// <param name="filepath"> path to the external settings file</param>
    public void DeserializeToObject(string filepath)
    {
        XmlSerializer ser = new XmlSerializer(typeof(GameSettings));

        try
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                gameSettings = (GameSettings?)ser.Deserialize(sr);
            }
        }
        catch
        {
            gameSettings = default(GameSettings);
        }
    }
}

/// <summary>
/// A class to hold game settings for the snake game.
/// </summary>
[XmlRoot(ElementName = "GameSettings")]
public class GameSettings
{
    [XmlElement("MSPerFrame")]
    public int MSPerFrame { get; set; }

    [XmlElement("FramesPerShot")]
    public int FramesPerShot { get; set; }

    [XmlElement("RespawnRate")]
    public int RespawnRate { get; set; }

    [XmlElement("UniverseSize")]
    public int UniverseSize { get; set; }

    [XmlElement("ExtraGameMode")]
    public bool ExtraGameMode { get; set; }

    [XmlArrayAttribute("Walls")]
    public List<Wall> Walls { get; set; }

    public GameSettings()
    {
        Walls = new List<Wall>();

    }
}

