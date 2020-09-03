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
    public class FijiController : Controller
    {
        JavaScriptSerializer ser = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

        public List<IncorrectConnectionNode> incorrectConnectionsPR;

        public List<IncorrectConnectionNode> incorrectConnectionsSEC;

        public List<IncorrectConnectionNode> incorrectConnectionsTER;

        public List<IncorrectConnectionNode> whitelistNodes;

        public ActionResult Index()
        {
            // FJ Incorrect Connections
            var incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_PR_trimmed.json"))));

            var incorrectConnectionsSEC = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_SEC_trimmed.json"))));

            var incorrectConnectionsTER = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_TER_trimmed.json"))));

            // FJ Primary, Secondary and Tertiary routes 
            var primary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_PR_RNG.json"))));

            var secondary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_SEC_RNG.json"))));

            var tertiary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_TER_RNG.json"))));

            // FJ Disconnections Primary and Secondary 
            var disjointedSubTreeWaysPR = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_primary_disconnections.json"))));

            var disjointedSubTreeWaysSEC = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_disconnections_secondary.json"))));

            var ferryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_FW_RNG.json"))));

            var ferryTerminals = ser.Deserialize<List<FerryTerminals>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_FerryTerminals.json"))));

            // FJ Connectivity Fixes
            var FJ_P_viaS = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ-P_viaS.json"))));

            var FJ_PS_viaST = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ-PS_viaST.json"))));

            var FJ_PST_viaST = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ-PST_viaST.json"))));


            ViewBag.incorrectConnectionsPR = incorrectConnectionsPR;

            ViewBag.incorrectConnectionsSEC = incorrectConnectionsSEC;

            ViewBag.incorrectConnectionsTER = incorrectConnectionsTER;

            ViewBag.primary = primary;

            ViewBag.secondary = secondary;

            ViewBag.tertiary = tertiary;

            ViewBag.disjointedSubTreeWaysPR = disjointedSubTreeWaysPR;

            ViewBag.disjointedSubTreeWaysSEC = disjointedSubTreeWaysSEC;

            ViewBag.ferryRoutes = ferryRoutes;

            ViewBag.ferryTerminals = ferryTerminals;

            ViewBag.FJ_PST_viaST = FJ_PST_viaST;

            ViewBag.FJ_P_viaS = FJ_P_viaS;

            ViewBag.FJ_PS_viaST = FJ_PS_viaST;

            return View();
        }

        public string addToWhitelist(string id)
        {
            string returnMsg = "Node " + id + " has been whitelisted";

            incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_PR_trimmed.json"))));

            string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes_FJ.json"));
            if (System.IO.File.Exists(whitelistNodeFile))
            {
                whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));
            }
            else
            {
                whitelistNodes = new List<IncorrectConnectionNode>();
            }

            foreach (IncorrectConnectionNode node in incorrectConnectionsPR)
            {
                if (node.Id.Equals(id))
                {
                    whitelistNodes.Add(node);
                    incorrectConnectionsPR.Remove(node);
                    break;
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_PR_trimmed.json")), JsonConvert.SerializeObject(incorrectConnectionsPR));
            System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes_FJ.json")), JsonConvert.SerializeObject(whitelistNodes));

            return returnMsg;
        }

        public string removeFromWhitelist(string id)
        {
            string returnMsg = "Node " + id + " has been removed from the whitelist";

            incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_PR_trimmed.json"))));

            string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes_FJ.json"));
            whitelistNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(whitelistNodeFile));

            foreach (IncorrectConnectionNode node in whitelistNodes)
            {
                if (node.Id.Equals(id))
                {
                    incorrectConnectionsPR.Add(node);
                    whitelistNodes.Remove(node);
                    break;
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_IncorrectConnections_PR_trimmed.json")), JsonConvert.SerializeObject(incorrectConnectionsPR));
            System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes_FJ.json")), JsonConvert.SerializeObject(whitelistNodes));

            return returnMsg;
        }

        public JsonResult getWhiteListData()
        {
            string whitelistNodeFile = Server.MapPath(Url.Content("~/Content/json_files/WhitelistNodes_FJ.json"));
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

        public JsonResult getIncorrectConnectionsDataPR()
        {
            incorrectConnectionsPR = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_PR_trimmed.json"))));

            var json = ser.Serialize(incorrectConnectionsPR);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult getIncorrectConnectionsDataSEC()
        //{
        //    incorrectConnectionsSEC = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_SEC.json"))));

        //    var json = JsonConvert.SerializeObject(incorrectConnectionsSEC);
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult getIncorrectConnectionsDataTER()
        //{
        //    incorrectConnectionsTER = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/IncorrectConnections/FJ_IncorrectConnections_TER.json"))));

        //    var json = JsonConvert.SerializeObject(incorrectConnectionsTER);
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}
    }
}