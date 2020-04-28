using Newtonsoft.Json;
using OSMConnectivity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace OSMConnectivity.Controllers
{
    public class HomeController : Controller
    {
        JavaScriptSerializer ser = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

        public List<IncorrectConnectionNode> incorrectConnectionNodes;

        public List<IncorrectConnectionNode> whitelistNodes;

        public ActionResult Index()
        {
            var trunks = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/trunk_NZ.json"))));

            var motorways = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/motorway_NZ.json"))));

            var maxSubGraph = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/MaxSubtree.json"))));

            var disjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/DisjointedSubTreeWays.json"))));

            incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json"))));

            ViewBag.trunks = trunks;

            ViewBag.motorways = motorways;

            ViewBag.disjointedSubTreeWays = disjointedSubTreeWays;

            ViewBag.maxSubGraph = maxSubGraph;

            ViewBag.incorrectConnectionNodes = incorrectConnectionNodes;

            return View();

        }

		public string addToWhitelist(string id)
		{
			string returnMsg = "Node " + id + " has been whitelisted";

			incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json"))));

			string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"));
			if (System.IO.File.Exists(whitelistNodeFile))
			{
				whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));
			}
			else
			{
				whitelistNodes = new List<IncorrectConnectionNode>();
			}

			foreach (IncorrectConnectionNode node in incorrectConnectionNodes)
			{
				if (node.Id.Equals(id))
				{
					whitelistNodes.Add(node);
					incorrectConnectionNodes.Remove(node);
					break;
				}
			}
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json")), JsonConvert.SerializeObject(incorrectConnectionNodes));
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json")), JsonConvert.SerializeObject(whitelistNodes));
			
			return returnMsg;
		}

		public string removeFromWhitelist(string id)
		{
			string returnMsg = "Node " + id + " has been removed from the whitelist";

			incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json"))));

			string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"));
			whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));
			
			foreach (IncorrectConnectionNode node in whitelistNodes)
			{
				if (node.Id.Equals(id))
				{
					incorrectConnectionNodes.Add(node);
					whitelistNodes.Remove(node);
					break;
				}
			}
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json")), JsonConvert.SerializeObject(incorrectConnectionNodes));
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json")), JsonConvert.SerializeObject(whitelistNodes));
			
			return returnMsg;
		}

		public JsonResult getWhiteListData()
		{
			whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"))));
			var json = JsonConvert.SerializeObject(whitelistNodes);
			return Json(json, JsonRequestBehavior.AllowGet);
		}

		public JsonResult getIncorrectConnectionsData()
        {
			incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/disconnections_NZ.json"))));
			var json = JsonConvert.SerializeObject(incorrectConnectionNodes);
			return Json(json, JsonRequestBehavior.AllowGet);
		}
	}
}