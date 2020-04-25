using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSMConnectivity.Models
{
    public class IncorrectConnectionNode
    {

        public string Id { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public List<Way> roads { get; set; }


    }
}