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

		public List<IncorrectConnectionNode> incorrectConnectionsMW;

		public List<IncorrectConnectionNode> incorrectConnectionsPR;

		public List<IncorrectConnectionNode> incorrectConnectionsSEC;

		public List<IncorrectConnectionNode> incorrectConnectionsTER;

		public List<IncorrectConnectionNode> incorrectConnectionsTR;

		public List<IncorrectConnectionNode> whitelistNodes;

		public ActionResult Index()
		{
			// NZ Incorrect Connections
			var incorrectConnectionsMW = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json"))));

			//var incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_PR_trimmed.json"))));

			//var incorrectConnectionsSEC = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_SEC_trimmed.json"))));

			//var incorrectConnectionsTER = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_TER_trimmed.json"))));

			//var incorrectConnectionsTR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_TR_trimmed.json"))));

			var primary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_PR_RNG.json"))));
			
			var secondary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_SEC_RNG.json"))));

			var tertiary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_TER_RNG.json"))));

			var motorways = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_MW_RNG.json"))));

			var trunks = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_TR_RNG.json"))));

			var ferryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_FW_RNG.json"))));

			var ferryTerminals = ser.Deserialize<List<FerryTerminals>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_FerryTerminals.json"))));

			//var disjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_disconnections.json"))));
			
			var disjointedSubTreeWaysMW = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_motorway_disconnections.json"))));
			
			var disjointedSubTreeWaysFW = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_disconnections_Ferry.json"))));

			//var maxSubGraph = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/MaxSubtree.json"))));

			var demoLowerTrunkTrunkLinkConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-Fix_Lower.json"))));

			var demoUpperTrunkTrunkLinkConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-Fix_upperMerged.json"))));

			// NZ Connectivity Fixes
			var demoTrunkTrunkLinkConnectivity = demoLowerTrunkTrunkLinkConnectivity.Concat(demoUpperTrunkTrunkLinkConnectivity);

			var demoTrunkTrunkLinkPrimaryConnectivity = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/NZ_T-TL-P-PL-Fix_Lower.json"))));

			var demoFRTrailLower = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/ViewFRTrialLower.json"))));

            ViewBag.incorrectConnectionsMW = incorrectConnectionsMW;

			ViewBag.incorrectConnectionsPR = incorrectConnectionsPR;

			ViewBag.incorrectConnectionsSEC = incorrectConnectionsSEC;

			ViewBag.incorrectConnectionsTER = incorrectConnectionsTER;

			ViewBag.incorrectConnectionsTR = incorrectConnectionsTR;

            ViewBag.primary = primary;

            ViewBag.secondary = secondary;

            ViewBag.tertiary = tertiary;

            ViewBag.motorways = motorways;

            ViewBag.trunks = trunks;

            ViewBag.ferryRoutes = ferryRoutes;

            ViewBag.ferryTerminals = ferryTerminals;

            ViewBag.disjointedSubTreeWaysMW = disjointedSubTreeWaysMW;

            ViewBag.disjointedSubTreeWaysFW = disjointedSubTreeWaysFW;

            ViewBag.demoTrunkTrunkLinkConnectivity = demoTrunkTrunkLinkConnectivity;

            ViewBag.demoTrunkTrunkLinkPrimaryConnectivity = demoTrunkTrunkLinkPrimaryConnectivity;

            ViewBag.demoFRTrailLower = demoFRTrailLower;

            //ViewBag.disjointedSubTreeWays = disjointedSubTreeWays;

            //ViewBag.maxSubGraph = maxSubGraph;

            return View();
		}

		public string addToWhitelist(string id)
		{
			string returnMsg = "Node " + id + " has been whitelisted";

			incorrectConnectionsMW = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json"))));

			string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"));
			if (System.IO.File.Exists(whitelistNodeFile))
			{
				whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));
			}
			else
			{
				whitelistNodes = new List<IncorrectConnectionNode>();
			}

			foreach (IncorrectConnectionNode node in incorrectConnectionsMW)
			{
				if (node.Id.Equals(id))
				{
					whitelistNodes.Add(node);
					incorrectConnectionsMW.Remove(node);
					break;
				}
			}
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json")), JsonConvert.SerializeObject(incorrectConnectionsMW));
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json")), JsonConvert.SerializeObject(whitelistNodes));

			return returnMsg;
		}

		public string removeFromWhitelist(string id)
		{
			string returnMsg = "Node " + id + " has been removed from the whitelist";

			incorrectConnectionsMW = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json"))));

			string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes.json"));
			whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));

			foreach (IncorrectConnectionNode node in whitelistNodes)
			{
				if (node.Id.Equals(id))
				{
					incorrectConnectionsMW.Add(node);
					whitelistNodes.Remove(node);
					break;
				}
			}
			System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json")), JsonConvert.SerializeObject(incorrectConnectionsMW));
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

		public JsonResult getIncorrectConnectionsDataMW()
		{
			incorrectConnectionsMW = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_MW.json"))));
			var json = JsonConvert.SerializeObject(incorrectConnectionsMW);
			return Json(json, JsonRequestBehavior.AllowGet);
		}

		public JsonResult getIncorrectConnectionsDataPR()
		{
			incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_PR_trimmed.json"))));

			var json = ser.Serialize(incorrectConnectionsPR);

			return Json(json, JsonRequestBehavior.AllowGet);
		}

		//public JsonResult getIncorrectConnectionsDataSEC()
		//{
		//	incorrectConnectionsSEC = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_SEC_trimmed.json"))));

		//	var json = JsonConvert.SerializeObject(incorrectConnectionsSEC);
		//	return Json(json, JsonRequestBehavior.AllowGet);
		//}

		//public JsonResult getIncorrectConnectionsDataTER()
		//{
		//	incorrectConnectionsTER = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_TER_trimmed.json"))));

		//	var json = JsonConvert.SerializeObject(incorrectConnectionsTER);
		//	return Json(json, JsonRequestBehavior.AllowGet);
		//}

		//public JsonResult getIncorrectConnectionsDataTR()
		//{
		//	incorrectConnectionsTR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/NZ/IncorrectConnections/NZ_IncorrectConnections_TR_trimmed.json"))));

		//	var json = JsonConvert.SerializeObject(incorrectConnectionsTR);
		//	return Json(json, JsonRequestBehavior.AllowGet);
		//}
	}
}