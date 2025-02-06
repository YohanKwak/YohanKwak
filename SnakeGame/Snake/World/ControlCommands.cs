// Authors: Simon Whidden and Yohan Kwak, 11/27/2022
// Class for an object that holds given user inputs.

using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        [JsonProperty(PropertyName = "moving")]
        public string moving;

        /// <summary>
        /// Creates a new ControlCommand object that holds the given user input as long as it is
        /// w, a, s, or d.
        /// </summary>
        /// <param name="character"> Given user input character.</param>
        public ControlCommand(string character)
        {
            moving = character;
            if(character == "w")
            {
                moving = "up";
            }
            else if(character == "a")
            {
                moving = "left";
            }
            else if(character == "s")
            {
                moving = "down";
            }
            else if(character == "d")
            {
                moving = "right";
            }
            else
            {
                moving = "none";
            }
        }

    }
}
