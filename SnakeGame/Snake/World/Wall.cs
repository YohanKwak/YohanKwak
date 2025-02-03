// Authors: Simon Whidden and Yohan Kwak, 11/27/2022
// Class for a wall object, contains positions and IDs for segments of walls.
using Newtonsoft.Json;
using SnakeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    //[XmlRoot(ElementName = "Wall")]
    public class Wall
    {
        [JsonProperty(PropertyName = "wall")]
        [XmlElement("ID")] 
        public int wallID;
        [JsonProperty(PropertyName = "p1")]
        [XmlElement("p1")]
        public Vector2D? p1;
        [JsonProperty(PropertyName = "p2")]
        [XmlElement("p2")]
        public Vector2D? p2;

        public Wall(int wallNum, Vector2D pos1, Vector2D pos2)
        {
            wallID = wallNum;
            p1 = pos1;
            p2 = pos2;
        }

        public Wall()
        {

        }
    }
}
