// Authors: Simon Whidden and Yohan Kwak, 11/27/2022
// Class for a PowerUp object, contains location information for the powerup in addition to
// a unique ID.
using System;
using Newtonsoft.Json;
using SnakeGame;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeGame{

    [JsonObject(MemberSerialization.OptIn)]
    public class PowerUp
    {
        [JsonProperty(PropertyName = "power")]
        public int powerID;

        [JsonProperty(PropertyName = "loc")]
        public Vector2D? location;

        [JsonProperty(PropertyName = "died")]
        public bool died { get; set;}
        public PowerUp(int ID, Vector2D loc)
        {
            location = loc;
            powerID = ID;
        }
    }

}
