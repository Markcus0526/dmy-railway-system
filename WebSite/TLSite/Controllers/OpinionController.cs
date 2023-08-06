using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;

namespace TLSite.Controllers
{
    public class OpinionController : Controller
    {
        OpinionModel opinionModel = new OpinionModel();
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        UserModel userModel = new UserModel();

        [Authorize]
        public ActionResult AddOpinion()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Opinion";
            ViewData["level2nav"] = "AddOpinion";
            ViewData["sectorlist"] = sectorModel.GetSectorList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["userlist"] = userModel.GetAllUserList();

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitOpinion(int examkind, string title, string contents)
        {
            string rst = "";

            rst = opinionModel.InsertOpinion(examkind, title, contents);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult OpinionList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Opinion";
            ViewData["level2nav"] = "AddOpinion";

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveOpinionList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = opinionModel.GetOpinionDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteOpinion(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = opinionModel.DeleteOpinion(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Detail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Opinion";
            ViewData["level2nav"] = "AddOpinion";

            ViewData["detailinfo"] = opinionModel.GetOpinionInfo(id);

            return View();
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult SubmitFeedback()
        {
            var uid = long.Parse(Request.QueryString["uid"].ToString());
            var feedback=Request.QueryString["feedback"].ToString();
            var rst=opinionModel.UpdateFeedback(uid,feedback);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
         [Authorize]
        public ActionResult MyOpinion()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Opinion";
            ViewData["level2nav"] = "AddOpinion";
            return View();
        }
        
        [Authorize]
        [AjaxOnly]
         public JsonResult RetrieveMyOpinionList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var currentuserid = CommonModel.GetSessionUserID();
            var rst = opinionModel.GetMyOpinionList(currentuserid);


            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.title,
                               string.Format("{0:yyyy-MM-dd}", c.createtime),
                               c.contents,
                               c.feedback,
                               Convert.ToString(c.uid)
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
    }
}