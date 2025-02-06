//Authors: Simon Whidden and Yohan Kwak, 11/27/2022
//This method handles connecting the server,
//parsing messages from the server, and updating the world where appropriate.
using NetworkUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameController
    {
        //Fields

        private SocketState? theSocketState = null;
        private World theWorld;
        public int worldSize;
        private bool initialSetup;
        public int clientID;

        private string userName = "";

        public delegate void GameUpdateHandler();
        public event GameUpdateHandler? UpdateArrived;

        public delegate void FailConnectedHandler();
        public event FailConnectedHandler? FailedToConnect;

        public delegate void ConnectedHandler();
        public event ConnectedHandler? Connected;

        public delegate void ConnectionBrokenHandler();
        public event ConnectionBrokenHandler? ConnectBroke;

        /// <summary>
        /// Creats a new GameController with an empty world.
        /// </summary>
        public GameController()
        {
            theWorld = new World();
            initialSetup = false;
        }

        /// <summary>
        /// Gets theWorld.
        /// </summary>
        /// <returns>A world object.</returns>
        public World GetWorld()
        {
            return theWorld;
        }

        /// <summary>
        /// Connects to the server.
        /// </summary>
        /// <param name="address">User input server address.</param>
        /// <param name="username">User input username.</param>
        public void ConnectToServer(String address, string username)
        {
            userName = username;
            Networking.ConnectToServer(OnConnect, address, 11000);
        }

        /// <summary>
        /// Callback for ConnectToServer method.
        /// </summary>
        /// <param name="state">SocketState given by ConnectToServer</param>
        private void OnConnect(SocketState state)
        {
            //If failed to connect, informs the view.
            if (state.ErrorOccurred)
            {
                FailedToConnect?.Invoke();
                return;
            }

            Connected?.Invoke();

            theSocketState = state;

            Networking.Send(theSocketState.TheSocket, userName + "\n");

            state.OnNetworkAction = ReceiveMessage;
            Networking.GetData(state);
        }

        /// <summary>
        /// Receives messages and then calls ProcessMessage.
        /// </summary>
        /// <param name="state">SocketState of the current connection.</param>
        private void ReceiveMessage(SocketState state)
        {
            //If error occurs, invoke Error delegate to inform the view.
            if (state.ErrorOccurred)
            {
                ConnectBroke?.Invoke();
                return;
            }
            lock (theWorld)
            {
                ProcessMessage(state);
            }
            Networking.GetData(state);
        }

        /// <summary>
        /// Processes given message and updates world objects appropriately.
        /// </summary>
        /// <param name="state">SocketState of the current connection.</param>
        private void ProcessMessage(SocketState state)
        {
            string totalData = state.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");


            // Loop until we have processed all messages.
            // We may have received more than one.

            List<string> newMessages = new List<string>();

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                // build a list of messages to send to the view
                newMessages.Add(p);

                // Then remove it from the SocketState's growable buffer
                state.RemoveData(0, p.Length);
            }

            //If first message sent, updates client ID and gets proper worldSize.
            if (!initialSetup)
            {
                int.TryParse(newMessages[0], out clientID);
                int.TryParse(newMessages[1], out worldSize);
                newMessages.RemoveAt(0);
                newMessages.RemoveAt(0);
                theWorld.worldSize = worldSize;
                initialSetup = true;

            }

            // inform the view
            Update(newMessages);

        }

        /// <summary>
        /// Updates the world where appropriate.
        /// </summary>
        /// <param name="data">List of strings for JSON-Deserialization.</param>
        public void Update(List<string> data)
        {
            if (data.Count <= 2)
                return;
            foreach (string message in data)
            {
                if (message[2] == 's')
                {
                    // Update snakes. Checks if active/alive > check if is new > update the snake.
                    Snake snakeData = JsonConvert.DeserializeObject<Snake>(message)!;

                    if (snakeData.disconnected)
                    {
                        theWorld.snakes.Remove(snakeData.snakeID);
                    }
                    else
                    {
                        theWorld.snakes[snakeData.snakeID] = snakeData;
                    }
                }
                else if (message[2] == 'p')
                {
                    // Update powerups. Checks if active/alive > if not, remove it.
                    PowerUp powerUpData = JsonConvert.DeserializeObject<PowerUp>(message)!;
                    if (powerUpData.died)
                    {
                        theWorld.powerups.Remove(powerUpData.powerID);
                    }
                    else
                    {
                        theWorld.powerups[powerUpData.powerID] = powerUpData;
                    }
                }
                else if (message[2] == 'w')
                {
                    Wall wallData = JsonConvert.DeserializeObject<Wall>(message)!;
                    theWorld.walls[wallData.wallID] = wallData;
                }
            }

            UpdateArrived?.Invoke();
        }

        /// <summary>
        /// Sends movement commands to the server.
        /// </summary>
        /// <param name="movement">User input movement command to be sent to the server.</param>
        public void SendMovement(string movement)
        {
            if (theSocketState is not null)
                Networking.Send(theSocketState.TheSocket, movement + "\n");
        }
    }
}
