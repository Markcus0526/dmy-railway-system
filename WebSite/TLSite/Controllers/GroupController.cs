using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class GroupController : Controller
    {
        GroupModel groupModel = new GroupModel();
        TeamModel teamModel = new TeamModel();
        UserModel userModel = new UserModel();

        [Authorize]
        public ActionResult GroupList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Group";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveGroupList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = groupModel.GetGroupDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddGroup()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Group";
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["crewrolelist"] = userModel.GetCrewRoleList();

            return View();
        }

        [Authorize]
        public ActionResult EditGroup(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Group";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditGroup", "", rootUri);

            var groupinfo = groupModel.GetGroupById(id);
            ViewData["groupinfo"] = groupinfo;
            ViewData["uid"] = groupinfo.uid;
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["crewrolelist"] = userModel.GetCrewRoleList();

            return View("AddGroup");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteGroup(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = groupModel.DeleteGroup(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitGroup(long uid, long teamid, string groupname, int sortid, int iscont,String dailygroup)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = groupModel.InsertGroup(teamid, groupname, sortid, iscont,dailygroup);
            }
            else
            {
                rst = groupModel.UpdateGroup(uid, teamid, groupname, sortid, iscont,dailygroup);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult InsertOrUpdateCrew(long id)
        {
            string rst = "";

            rst = groupModel.InsertOrUpdateCrew(id, Request.Form);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult CheckUniqueGroupname(string groupname, long uid)
        {
            bool rst = groupModel.CheckDuplicateName(groupname, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveGroupCrewList(long id, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = userModel.GetGroupCrewDataTable(id, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetGroupCrewInfo(long id, long groupid)
        {
            var rst = userModel.GetGroupCrewInfo(id, groupid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult DeleteGroupCrew(long id, string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = groupModel.DeleteGroupCrew(id, selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
