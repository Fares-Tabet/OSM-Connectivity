using OSMConnectivity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OSMConnectivity.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {

            JavaScriptSerializer ser = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

            var trunks = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/trunk_NZ.json"))));

            var motorways = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/motorway_NZ.json"))));

            //var disconnections = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json"))));

            ViewBag.trunks = trunks;
            ViewBag.motorways = motorways;
            
            //ViewBag.json1 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/motorway_NZ_partial.json")));

            //ViewBag.json2 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/trunk .json")));

            ViewBag.disconnections = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json")));


            return View(); 

        }


    }
}