using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class TeamController : Controller
    {
        TeamModel teamModel = new TeamModel();

        [Authorize]
        public ActionResult TeamList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Team";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveTeamList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = teamModel.GetTeamDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddTeam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Team";
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        public ActionResult EditTeam(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Team";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditTeam", "", rootUri);

            var teaminfo = teamModel.GetTeamById(id);
            ViewData["teaminfo"] = teaminfo;
            ViewData["uid"] = teaminfo.uid;
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View("AddTeam");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteTeam(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = teamModel.DeleteTeam(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitTeam(long uid, string teamname, int sortid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = teamModel.InsertTeam(teamname, sortid);
            }
            else
            {
                rst = teamModel.UpdateTeam(uid, teamname, sortid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult CheckUniqueTeamname(string teamname, long uid)
        {
            bool rst = teamModel.CheckDuplicateName(teamname, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
