using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;

namespace TLSite.Controllers
{
    public class SystemController : Controller
    {
        UserModel userModel = new UserModel();
        SystemModel sysModel = new SystemModel();
        TaskModel taskModel = new TaskModel();
        MailModel mailModel = new MailModel();

        [Authorize]
        public ActionResult NoticeDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Home";
            ViewData["level2nav"] = "Home";

            return View();
        }

        #region 个人中心
        [Authorize]
        public ActionResult Profile()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var currId = CommonModel.GetSessionUserID();
            var userinfo = userModel.GetUserById(currId);

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "System";
            ViewData["level2nav"] = "Profile";
            ViewData["userinfo"] = userinfo;

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult SubmitUserInfo(string img, string uid, string realname, string birthday, byte sex, string mailaddr, 
            string qqnum, string phonenum, string mailnotice, string newpassword)
        {
            string rst = "";
            string p_number = "";
            byte m_notice = 0;

            string[] tmp = phonenum.Split(new Char[] { '-' });
            for (int i = 0; i < tmp.Count(); i++)
                p_number += tmp[i];

            if (mailnotice == "on")
                m_notice = 1;

            rst = userModel.UpdateUserInfo(img, Convert.ToInt64(uid), realname, birthday,
                                         sex, mailaddr, qqnum, p_number, m_notice, newpassword);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 通知
        [Authorize]
        [AjaxOnly]
        public JsonResult GetSystemNotice()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var noticelist = sysModel.GetUnreadNoticeTip(10);
            var tasklist = taskModel.GetUnfinishedTask();
            var maillist = mailModel.GetUnreadMail();

            return Json(new { noticelist = noticelist, tasklist = tasklist, maillist = maillist }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetLatestNotice(string lasttime)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime lastdate = DateTime.Now;

            try
            {
                lastdate = DateTime.Parse(lasttime);
            }
            catch (System.Exception ex)
            {
            	
            }

            var noticelist = sysModel.GetLatestUnreadNotice(lastdate);

            return Json(new { nowtime = String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", DateTime.Now), rootUri = rootUri, noticelist = noticelist }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [Authorize(Roles = "SysSetting")]
        public ActionResult SysConfig()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "System";
            ViewData["level2nav"] = "SysConfig";
            ViewData["sysconfig"] = sysModel.GetSysConfig();

            return View();
        }

        [Authorize(Roles = "SysSetting")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitConfig(string notice, string imgurl)
        {
            string rst = "";

            rst = sysModel.SubmitConfig(notice, imgurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SysSetting")]
        public ActionResult SlideImg()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "System";
            ViewData["level2nav"] = "SlideImg";

            return View();
        }

        [Authorize(Roles = "SysSetting")]
        [AjaxOnly]
        public JsonResult RetrieveSlideList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = sysModel.GetSlideDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SysSetting")]
        public ActionResult AddSlide()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "System";
            ViewData["level2nav"] = "SlideImg";

            return View();
        }

        [Authorize(Roles = "SysSetting")]
        public ActionResult EditSlide(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "System";
            ViewData["level2nav"] = "SlideImg";

            var slideinfo = sysModel.GetSlideById(id);
            ViewData["slideinfo"] = slideinfo;
            ViewData["uid"] = slideinfo.uid;

            return View("AddSlide");
        }

        [Authorize(Roles = "SysSetting")]
        [HttpPost]
        public JsonResult DeleteSlide(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = sysModel.DeleteSlide(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SysSetting")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitSlide(long uid, string title, string imgurl, int sortid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = sysModel.InsertSlide(title, imgurl, sortid);
            }
            else
            {
                rst = sysModel.UpdateSlide(uid, title, imgurl, sortid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
