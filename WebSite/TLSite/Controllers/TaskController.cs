using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class TaskController : Controller
    {
        TaskModel taskModel = new TaskModel();
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        UserModel userModel = new UserModel();

        [Authorize]
        public ActionResult TaskList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Task";
            ViewData["level2nav"] = "TaskList";

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveTaskList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = taskModel.GetTaskDataTable(param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MyTaskList(int kind)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Task";
            ViewData["level2nav"] = "MyTaskList";
            ViewData["kind"] = kind;
                
            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveMyTaskList(int kind, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = taskModel.GetMyTaskDataTable(kind, param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddTask()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Task";
            ViewData["level2nav"] = "AddTask";
            ViewData["sectorlist"] = sectorModel.GetSectorList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["userlist"] = userModel.GetAllChezhangUpList();

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitTask(string title, string docno, string contents, DateTime starttime, DateTime endtime, string filename, string path, long filesize)
        {
            string rst = "";
            string receiver = "";

            try
            {
                receiver = Request.Form["receiver"].ToString();
            }
            catch (System.Exception ex)
            {

            }

            path = path.Trim(new char[] { '\"' });
            rst = taskModel.InsertTask(receiver, title, docno, contents, starttime, endtime.AddHours(23), filename, path, filesize);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TaskDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string origin = "";

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Task";
           // ViewData["level2nav"] = "TaskList";

            var taskinfo = taskModel.GetTaskInfo(id,1);
            var execlist = taskModel.GetExecutiveList(id);
            var messagelist = taskModel.GetMessageList(id);

            if (Request.QueryString["origin"] != null)
            {
                origin = Request.QueryString["origin"].ToString();
            }

            ViewData["taskinfo"] = taskinfo;
            ViewData["execlist"] = execlist;
            ViewData["messagelist"] = messagelist;
            ViewData["origin"] = origin;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult ExecuteTask(long id)
        {
            string rst = "";

            rst = taskModel.ExecuteTask(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult ExecuteFinish(long id)
        {
            string rst = "";

            rst = taskModel.ExecuteFinish(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult ProcessMessage(string text, long taskid)
        {
            string rst = "";

            rst = taskModel.SaveMessage(taskid, text);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}