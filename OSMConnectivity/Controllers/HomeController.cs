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
			// FJ Files
			var FJ_P_viaS = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ-P_viaS.json"))));

			var FJ_PS_viaST = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ-PS_viaST.json"))));

			var FJ_Primary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_primary_RNG.json"))));

            var FJ_Secondary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_secondary_RNG.json"))));

			var FJ_FerryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_FW_RNG.json"))));

			var FJ_DisjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_disconnections.json"))));

			var FJ_IncorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_Incorrect_PR_All_Connections.json"))));

			// NZ Files
			var trunks = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_trunk_RNG.json"))));

            var motorways = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_motorway_RNG.json"))));
            
			var NZ_FerryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_FW_RNG.json"))));

			var NZ_DisjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_disconnections.json"))));

			var maxSubGraph = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/MaxSubtree.json"))));

			var NZ_IncorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_IncorrectConnections.json"))));

			var demoTrunkTrunkLinkConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/Demo_trunkonly.json"))));

            var demoTrunkTrunkLinkPrimaryConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/Demo_trunk_primary.json"))));

			// Concat NZ and FJ data
			var ferryRoutes = NZ_FerryRoutes.Concat(FJ_FerryRoutes);

			var disjointedSubTreeWays = NZ_DisjointedSubTreeWays.Concat(FJ_DisjointedSubTreeWays);

			incorrectConnectionNodes = NZ_IncorrectConnectionNodes.Concat(FJ_IncorrectConnectionNodes).ToList();

			// Assign ViewBag data
			ViewBag.FJ_P_viaS = FJ_P_viaS;

			ViewBag.FJ_PS_viaST = FJ_PS_viaST;

			ViewBag.FJ_Primary = FJ_Primary;

			ViewBag.demoTrunkTrunkLinkConnectivity = demoTrunkTrunkLinkConnectivity;

			ViewBag.FJ_Secondary = FJ_Secondary;

			ViewBag.demoTrunkTrunkLinkPrimaryConnectivity = demoTrunkTrunkLinkPrimaryConnectivity;

            ViewBag.trunks = trunks;

            ViewBag.ferryRoutes = ferryRoutes;

            ViewBag.motorways = motorways;

            ViewBag.disjointedSubTreeWays = disjointedSubTreeWays;

            ViewBag.maxSubGraph = maxSubGraph;

            ViewBag.incorrectConnectionNodes = NZ_IncorrectConnectionNodes;

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
			string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"));
			if (System.IO.File.Exists(whitelistNodeFile))
			{
				whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));
			}
			else
			{
				whitelistNodes = new List<IncorrectConnectionNode>();
			}
			var json = JsonConvert.SerializeObject(whitelistNodes);
			return Json(json, JsonRequestBehavior.AllowGet);
		}

		public JsonResult getIncorrectConnectionsData()
        {
			var FJ_IncorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_Incorrect_PR_All_Connections.json"))));
			var NZ_IncorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ_IncorrectConnections.json"))));
			incorrectConnectionNodes = NZ_IncorrectConnectionNodes.Concat(FJ_IncorrectConnectionNodes).ToList();

			var json = JsonConvert.SerializeObject(incorrectConnectionNodes);
			return Json(json, JsonRequestBehavior.AllowGet);
		}
	}
}