using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSMConnectivity.Models
{
    public class Way
    {
        public string Id { get; set; }
        public string roadClass { get; set; }
        public string maxSpeed { get; set; }
        public string oneWay { get; set; }
        public string name { get; set; }
        public Node startNode { get; set; }
        public Node endNode { get; set; }
        public List<Node> nodes { get; set; }
    }
}