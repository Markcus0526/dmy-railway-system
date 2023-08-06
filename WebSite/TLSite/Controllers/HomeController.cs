using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class HomeController : Controller
    {
        UserModel userModel = new UserModel();

//         public ActionResult LogOn()
//         {
//             string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
//             ViewData["rootUri"] = rootUri;
// 
//             return View();
//         }

        [Authorize]
        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Home";
            ViewData["level2nav"] = "Home";
            var currId = CommonModel.GetSessionUserID();
            var userinfo = userModel.GetUserById(currId);
            ViewData["userinfo"] = userinfo;

            //检查是否提醒用户任务快要到期
            bool checknotifytask = false;
            TaskModel taskmodel = new TaskModel();
            var deadlinetasklist = taskmodel.MyDeadlineTask(currId);
            if (deadlinetasklist.Count > 0)
            {
                checknotifytask = true;
            }

            ViewData["notifytask"] = checknotifytask;
            //

            return View();
        }

    }
}