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

        public List<IncorrectConnectionNode> incorrectConnectionNodes;

        public List<IncorrectConnectionNode> whitelistNodes;

        public List<FerryTerminals> ferryTerminals;

        public ActionResult Index()
        {
            var FJ_P_viaS = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ-P_viaS.json"))));

            var FJ_PS_viaST = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ-PS_viaST.json"))));

            var FJ_Primary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_primary_RNG.json"))));

            var FJ_Secondary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_secondary_RNG.json"))));

            var ferryRoutes = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_FW_RNG.json"))));

            var ferryTerminals = ser.Deserialize<List<FerryTerminals>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_FerryTerminals.json"))));

            var disjointedSubTreeWays = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_disconnections.json"))));

            var disjointedSubTreeWays_secondary = ser.Deserialize<List<Way>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_disconnections_secondary.json"))));

            var incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_Incorrect_PR_Connections.json"))));

            ViewBag.FJ_P_viaS = FJ_P_viaS;

            ViewBag.FJ_PS_viaST = FJ_PS_viaST;

            ViewBag.FJ_Primary = FJ_Primary;

            ViewBag.FJ_Secondary = FJ_Secondary;

            ViewBag.ferryRoutes = ferryRoutes;

            ViewBag.ferryTerminals = ferryTerminals;

            ViewBag.disjointedSubTreeWays = disjointedSubTreeWays;

            ViewBag.disjointedSubTreeWays_secondary = disjointedSubTreeWays_secondary;

            ViewBag.incorrectConnectionNodes = incorrectConnectionNodes;

            return View();
        }

        public string addToWhitelist(string id)
        {
            string returnMsg = "Node " + id + " has been whitelisted";

            incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_Incorrect_All_Connections.json"))));

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

            incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_Incorrect_PR_All_Connections.json"))));

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
            System.IO.File.WriteAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ/FJ_disconnections.json")), JsonConvert.SerializeObject(incorrectConnectionNodes));
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
            incorrectConnectionNodes = ser.Deserialize<List<IncorrectConnectionNode>>(System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/json_files/FJ_Incorrect_PR_Connections.json"))));

            var json = JsonConvert.SerializeObject(incorrectConnectionNodes);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}