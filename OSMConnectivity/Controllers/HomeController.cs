using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSMConnectivity.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {

            //ViewBag.json1 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/wayDetails.json")));
            ViewBag.json1 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/motorway.json")));

            ViewBag.json2 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/trunk .json")));

            ViewBag.json3 = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/motorwayDisconnections.json")));

            return View(); 

        }


    }
}