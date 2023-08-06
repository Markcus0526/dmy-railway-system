using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class RouteController : Controller
    {
        RouteModel routeModel = new RouteModel();
        TeamModel teammodel = new TeamModel();

        [Authorize]
        public ActionResult RouteList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Route";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveRouteList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = routeModel.GetRouteDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddRoute()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Route";
            
            ViewData["teamlist"] = teammodel.GetTeamList();
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        public ActionResult EditRoute(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Route";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditRoute", "", rootUri);
            ViewData["teamlist"] = teammodel.GetTeamList();

            var routeinfo = routeModel.GetRouteById(id);
            ViewData["routeinfo"] = routeinfo;
            ViewData["uid"] = routeinfo.uid;
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View("AddRoute");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteRoute(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = routeModel.DeleteRoute(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitRoute(long uid, string routename, int sortid, long teamid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = routeModel.InsertRoute(routename, sortid, teamid);
            }
            else
            {
                rst = routeModel.UpdateRoute(uid, routename, sortid, teamid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult CheckUniqueRoutename(string routename, long uid)
        {
            bool rst = routeModel.CheckDuplicateName(routename, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
