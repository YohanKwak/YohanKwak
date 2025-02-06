//Authors: Yohan Kwak and Simon Whidden, 11/27/2022
//This method creates a WorldPanel, loads images, and draws powerups, walls, and snakes on the created WorldPanel.
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using IImage = Microsoft.Maui.Graphics.IImage;
#if MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#else
using Microsoft.Maui.Graphics.Win2D;
#endif
using Color = Microsoft.Maui.Graphics.Color;
using System.Reflection;
using Microsoft.Maui;
using System.Net;
using Font = Microsoft.Maui.Graphics.Font;
using SizeF = Microsoft.Maui.Graphics.SizeF;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;
using Image = Microsoft.Maui.Controls.Image;

namespace SnakeGame;
public class WorldPanel : ScrollView, IDrawable
{
    //Fields
    private IImage wall;
    private IImage background;
    private IImage explosion;
    private IImage rat;

    private World theWorld;
    private GraphicsView graphicsView = new();
    private int viewSize = 900;
    private GameController gc;

    private int currentPlayerID;

    private bool initializedForDrawing = false;

    private Dictionary<int, int> diedCounter = new Dictionary<int, int>();

    public delegate void ObjectDrawer(object o, ICanvas canvas);


#if MACCATALYST
    private IImage loadImage(string name)
    {
        Assembly assembly = GetType().GetTypeInfo().Assembly;
        string path = "SnakeGame.Resources.Images";
        return PlatformImage.FromStream(assembly.GetManifestResourceStream($"{path}.{name}"));
    }
#else
    /// <summary>
    /// Loads images from the resources image file for later drawing.
    /// </summary>
    /// <param name="name">Name of the image to load.</param>
    /// <returns></returns>
    private IImage loadImage(string name)
    {
        Assembly assembly = GetType().GetTypeInfo().Assembly;
        string path = "SnakeGame.Resources.Images";
        var service = new W2DImageLoadingService();
        return service.FromStream(assembly.GetManifestResourceStream($"{path}.{name}"));
    }
#endif

    /// <summary>
    /// Constructor for the worldPanel.
    /// </summary>
    public WorldPanel()
    {
        gc = new GameController();
        graphicsView.Drawable = this;
        graphicsView.HeightRequest = viewSize;
        graphicsView.WidthRequest = viewSize;
        this.Content = graphicsView;
    }

    /// <summary>
    /// Updates the panel for redrawing.
    /// </summary>
    public void Invalidate()
    {
        Dispatcher.Dispatch(() => graphicsView.Invalidate());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gController"></param>
    public void SetWorldAndController(GameController gController)
    {
        gc = gController;
        theWorld = gc.GetWorld();
    }

    /// <summary>
    /// This method updates the panel accordingly based on the current world.
    /// </summary>
    /// <param name="canvas">Canvas for drawing.</param>
    /// <param name="dirtyRect"></param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (theWorld.worldSize != -1)
        {
            if (!initializedForDrawing)
            {
                InitializeDrawing();
                return;
            }

            this.currentPlayerID = gc.clientID;

            // undo any leftover transformations from last frame
            canvas.ResetState();

            // draw the objects in the world
            lock (theWorld)
            {
                // center the view on the middle of the world
                canvas.Translate(Convert.ToSingle(-theWorld.snakes[currentPlayerID].body.Last().GetX() + (viewSize / 2)), Convert.ToSingle(-theWorld.snakes[currentPlayerID].body.Last().GetY() + (viewSize / 2)));


                canvas.DrawImage(background, -gc.worldSize / 2, -gc.worldSize / 2, gc.worldSize, gc.worldSize);

                wallDraw(canvas, dirtyRect);

                foreach (var p in theWorld.powerups.Values)
                    powerupDraw(p, canvas);

                foreach (var p in theWorld.snakes.Values)
                    snakeDraw(p, canvas);
            }
        }

    }

    /// <summary>
    /// Draws the walls on the WorldPanel.
    /// </summary>
    /// <param name="canvas">Canvas for drawing.</param>
    /// <param name="dirtyRect"></param>
    private void wallDraw(ICanvas canvas, RectF dirtyRect)
    {
        if (!initializedForDrawing)
        {
            InitializeDrawing();
        }
        foreach (Wall w in theWorld.walls.Values)
        {
            if (w.p1.GetX().Equals(w.p2.GetX()))
            {
                //Vertical Walls
                if (w.p1.GetY() < (w.p2.GetY()))
                {
                    // p1 is the top point
                    double curYposition = w.p1.GetY();
                    while (curYposition <= w.p2.GetY())
                    {
                        canvas.DrawImage(wall, Convert.ToSingle(w.p1.GetX() - 25), Convert.ToSingle(curYposition - 25)
                            , 50, 50);
                        curYposition += 50;
                    }
                }
                else
                {
                    //p2 is the top point -OR- The Y posion in p1 & p2 are equal as well
                    double curYposition = w.p2.GetY();
                    while (curYposition <= w.p1.GetY())
                    {
                        canvas.DrawImage(wall, Convert.ToSingle(w.p1.GetX() - 25), Convert.ToSingle(curYposition - 25)
                            , 50, 50);
                        curYposition += 50;
                    }
                }
            }
            else
            {
                // Horizontal Walls
                if (w.p1.GetX() < (w.p2.GetX()))
                {
                    // p1 is the leftmost point
                    double curXposition = w.p1.GetX();
                    while (curXposition <= w.p2.GetX())
                    {
                        canvas.DrawImage(wall, Convert.ToSingle(curXposition - 25), Convert.ToSingle(w.p1.GetY() - 25)
                            , 50, 50);
                        curXposition += 50;
                    }
                }
                else
                {
                    //p2 is the leftmost point
                    double curXposition = w.p2.GetX();
                    while (curXposition <= w.p1.GetX())
                    {
                        canvas.DrawImage(wall, Convert.ToSingle(curXposition - 25), Convert.ToSingle(w.p1.GetY() - 25)
                            , 50, 50);
                        curXposition += 50;
                    }
                }
            }
        }


    }

    /// <summary>
    /// Draws the snake on the WorldPanel.
    /// </summary>
    /// <param name="o">Given snake object.</param>
    /// <param name="canvas">Canvas to be drawn on.</param>
    private void snakeDraw(Object o, ICanvas canvas)
    {
        Snake snake = o as Snake;
        List<Vector2D> snakeVertexList = snake.body;

        // If snake died on that frame,
        // add to the dead snake dictionary.
        if (snake.died)
        {
            if (!diedCounter.ContainsKey(snake.snakeID))
                diedCounter.Add(snake.snakeID, 0);
        }

        // Draws explosion if snake is dead.
        if (diedCounter.ContainsKey(snake.snakeID))
        {
            if (snake.alive)
            {
                diedCounter.Remove(snake.snakeID);
            }
            else
            {
                if ((diedCounter[snake.snakeID] / 5) % 2 == 0)
                {
                    canvas.DrawImage(explosion, Convert.ToSingle(snake.body.Last().GetX() - 22), Convert.ToSingle(snake.body.Last().GetY() - 22), 44, 44);
                }
                else if ((diedCounter[snake.snakeID] / 5) % 2 == 1)
                {
                    canvas.DrawImage(explosion, Convert.ToSingle(snake.body.Last().GetX() - 15), Convert.ToSingle(snake.body.Last().GetY() - 15), 30, 30);
                }
                diedCounter[snake.snakeID]++;
            }

        }

        //Draws the snake, accounting for wrapping at the world border.
        if (!diedCounter.ContainsKey(snake.snakeID))
        {

            bool isFirst = true;
            bool hitWall = false;
            float x1 = Convert.ToSingle(snakeVertexList[0].GetX());
            float y1 = Convert.ToSingle(snakeVertexList[0].GetY());


            float x2 = 0;
            float y2 = 0;

            foreach (Vector2D bodyVertex in snakeVertexList)
            {
                if (hitWall)
                {
                    x1 = Convert.ToSingle(bodyVertex.GetX());
                    y1 = Convert.ToSingle(bodyVertex.GetY());
                    hitWall = false;
                }
                else
                {
                    if (!isFirst)
                    {
                        //Chooses snake color.
                        canvas.StrokeSize = 10;
                        canvas.StrokeLineCap = LineCap.Round;

                        if ((snake.snakeID % 9) == 0)
                        {
                            canvas.StrokeColor = Colors.Red;
                        }
                        else if ((snake.snakeID % 9) == 1)
                        {
                            canvas.StrokeColor = Colors.Orange;
                        }
                        else if ((snake.snakeID % 9) == 2)
                        {
                            canvas.StrokeColor = Colors.Yellow;
                        }
                        else if ((snake.snakeID % 9) == 3)
                        {
                            canvas.StrokeColor = Colors.Green;
                        }
                        else if ((snake.snakeID % 9) == 4)
                        {
                            canvas.StrokeColor = Colors.Blue;
                        }
                        else if ((snake.snakeID % 9) == 5)
                        {
                            canvas.StrokeColor = Colors.DarkGray;
                        }
                        else if ((snake.snakeID % 9) == 6)
                        {
                            canvas.StrokeColor = Colors.Violet;
                        }
                        else if ((snake.snakeID % 9) == 7)
                        {
                            canvas.StrokeColor = Colors.White;
                        }
                        else if ((snake.snakeID % 9) == 8)
                        {
                            canvas.StrokeColor = Colors.Black;
                        }


                        x2 = Convert.ToSingle(bodyVertex.GetX());
                        y2 = Convert.ToSingle(bodyVertex.GetY());

                        canvas.DrawLine(x1, y1, x2, y2);
                        x1 = x2;
                        y1 = y2;

                        if (Math.Abs(x1) >= ((theWorld.worldSize / 2)) || Math.Abs(y1) >= ((theWorld.worldSize) / 2))
                        {
                            hitWall = true;
                        }
                    }
                    else
                        isFirst = false;

                }
            }

            //Displays username and snake score.
            canvas.DrawString(snake.name + ": " + snake.powerUpsEaten, Convert.ToSingle(snake.body.Last().GetX()), Convert.ToSingle(snake.body.Last().GetY()) + 20, HorizontalAlignment.Center);

        }
    }

    /// <summary>
    /// Draws the powerup on the WorldPanel.
    /// </summary>
    /// <param name="o">Powerup to be drawn.</param>
    /// <param name="canvas">Canvas to be drawn on.</param>
    private void powerupDraw(object o, ICanvas canvas)
    {
        PowerUp p = (PowerUp)o;

        float x1 = Convert.ToSingle(p.location.GetX());
        float y1 = Convert.ToSingle(p.location.GetY());

        canvas.DrawImage(rat, x1 - 8, y1 - 8, 16, 16);
    }

    /// <summary>
    /// Calls load image to load images for later use.
    /// </summary>
    private void InitializeDrawing()
    {
        wall = loadImage("WallSprite.png");
        background = loadImage("Background.png");
        explosion = loadImage("Explosion.png");
        rat = loadImage("Rat2.png");
        initializedForDrawing = true;
    }
}

