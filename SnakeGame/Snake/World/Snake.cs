// Authors: Simon Whidden and Yohan Kwak, 11/27/2022
// Class for a snake object containing a variety of information related to 
// drawing snakes. Includes given username, unique snake ID, a list of 2D body vectors
// current direction, amount of powerups consumed, whether the snake should be drawn or not,
// and whether or not they disconnected or joined.
using Newtonsoft.Json;
using System;
using SnakeGame;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        [JsonProperty(PropertyName = "snake")]
        public int snakeID;

        [JsonProperty(PropertyName = "name")]
        public string? name;

        [JsonProperty(PropertyName = "body")]
        public List<Vector2D>? body;

        [JsonProperty(PropertyName = "dir")]
        public Vector2D? orientation;

        [JsonProperty(PropertyName = "score")]
        public int powerUpsEaten;

        [JsonProperty(PropertyName = "died")]
        public bool died;

        [JsonProperty(PropertyName = "alive")]
        public bool alive;

        [JsonProperty(PropertyName = "dc")]
        public bool disconnected;

        [JsonProperty(PropertyName = "join")]
        public bool join;


        public Snake(String name, int ID, List<Vector2D>? position)
        {
            this.snakeID = ID;
            this.name = name;
            this.body = position;
            this.orientation = new Vector2D();
            this.powerUpsEaten = 0;
            this.disconnected = false;
            this.join = false;
            this.alive = true;

        }
    }
}
