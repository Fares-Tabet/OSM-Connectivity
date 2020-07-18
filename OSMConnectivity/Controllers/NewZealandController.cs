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
	public class NewZealandController : Controller
	{
		JavaScriptSerializer ser = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

		public List<IncorrectConnectionNode> incorrectConnectionNodes;

		public List<IncorrectConnectionNode> whitelistNodes;

		public ActionResult Index()
		{
			var trunks = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_trunk_RNG.json"))));

			var motorways = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_motorway_RNG.json"))));

			var ferryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_FW_RNG.json"))));

			var ferryTerminals = ser.Deserialize<List<FerryTerminals>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_FerryTerminals.json"))));

			var disjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_disconnections.json"))));

			var maxSubGraph = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/MaxSubtree.json"))));

			var incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_IncorrectConnections.json"))));

			var demoLowerTrunkTrunkLinkConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-Fix_Lower.json"))));

			var demoUpperTrunkTrunkLinkConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-Fix_Upper.json"))));

			var demoTrunkTrunkLinkConnectivity = demoLowerTrunkTrunkLinkConnectivity.Concat(demoUpperTrunkTrunkLinkConnectivity);

			var demoTrunkTrunkLinkPrimaryConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-P-PL-Fix_Lower.json"))));

			ViewBag.demoTrunkTrunkLinkConnectivity = demoTrunkTrunkLinkConnectivity;

			ViewBag.demoTrunkTrunkLinkPrimaryConnectivity = demoTrunkTrunkLinkPrimaryConnectivity;

			ViewBag.trunks = trunks;

			ViewBag.ferryRoutes = ferryRoutes;

			ViewBag.ferryTerminals = ferryTerminals;

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
			incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_IncorrectConnections.json"))));
			var json = JsonConvert.SerializeObject(incorrectConnectionNodes);
			return Json(json, JsonRequestBehavior.AllowGet);
		}
	}
}