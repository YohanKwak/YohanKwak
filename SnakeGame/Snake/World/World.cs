// Authors: Simon Whidden and Yohan Kwak, 11/27/2022
// Class for a world object, contains snakes, walls, and powerups.
using SnakeGame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class World
    {
        public Dictionary<int, Snake> snakes;
        public Dictionary<int, Wall> walls;
        public Dictionary<int, PowerUp> powerups;

        //Prevents drawing prematurely before the server has sent the actual worldsize.
        public int worldSize = -1;

        /// <summary>
        /// Basic constructor creating three dictionaries that contain
        /// all snakes, powerups, and walls.
        /// </summary>
        public World()
        {
            snakes = new Dictionary<int, Snake>();
            walls = new Dictionary<int, Wall>();
            powerups = new Dictionary<int, PowerUp>();
        }

    }
}
