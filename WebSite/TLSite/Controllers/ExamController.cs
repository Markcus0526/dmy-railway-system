using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

namespace TLSite.Controllers
{
    public class ExamController : Controller
    {
        ExamModel examModel = new ExamModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();

        #region Exam Manage
        [Authorize(Roles = "OnlineTest")]
        public ActionResult ExamList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";
            ViewData["teamlist"] = teamModel.GetTeamList();

            return View();
        }

        [Authorize(Roles = "TeamManager")]
        public ActionResult TeamExamList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveExamList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = examModel.GetExamDataTable(0,0,param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveExamListWithCondition(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var examkind = int.Parse(Request.QueryString["examkind"]);
            var teamid =long.Parse( Request.QueryString["teamid"]);

            JqDataTableInfo rst = examModel.GetExamDataTableWithCondition(examkind, teamid,0, 0, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveTeamExamList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);

            JqDataTableInfo rst = examModel.GetExamDataTableByTeamid(0,0,param, Request.QueryString, rootUri,teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "OnlineTest")]
        public ActionResult AddExam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";
            ViewData["teamlist"] = teamModel.GetTeamList();

            return View();
        }


        [Authorize(Roles = "TeamManager")]
        public ActionResult TeamAddExam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";
            ViewData["teamlist"] = teamModel.GetTeamList();
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            ViewData["teamid"] = teamid;
            return View();
        }



        [Authorize(Roles = "OnlineTest")]
        //[Authorize(Roles = "TeamManager")]

        public ActionResult EditExam(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";

            var examinfo = examModel.GetExamInfo(id);
            ViewData["examinfo"] = examinfo;
            ViewData["uid"] = examinfo.uid;
            ViewData["teamlist"] = teamModel.GetTeamList();

            return View("AddExam");
        }

        public ActionResult TeamEditExam(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";

            var examinfo = examModel.GetExamInfo(id);
            ViewData["examinfo"] = examinfo;
            ViewData["uid"] = examinfo.uid;
            ViewData["teamlist"] = teamModel.GetTeamList();

            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            ViewData["teamid"] = teamid;
            return View("TeamAddExam");
        }

        [Authorize(Roles = "OnlineTest,TeamManager")]
        [HttpPost]
        public JsonResult DeleteExam(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = examModel.DeleteExam(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "OnlineTest,TeamManager")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitExam(long uid, byte examkind, long teamid,string title, string examtype, string answerstr,int score)
        {
            string rst = "";

            string choisestr = Request.QueryString["choisestr"].ToString();
            choisestr = choisestr.TrimEnd(new char[] {'\n'});
            string answeryesnostr = Request.QueryString["answer"];
            int answeryesno=0;
            if (answeryesnostr!=null)
            {
                answeryesno = int.Parse(answeryesnostr);
            }
            answerstr = answerstr.TrimEnd(new char[] { ',', ' ', '，' });
            try
            {
                answerstr = answerstr.Replace("，", ",");
            }
            catch (System.Exception ex)
            {

            }
            try
            {
                answerstr = answerstr.ToUpper();
            }
            catch (System.Exception ex)
            {

            }

            if (uid == 0)
            {
                rst = examModel.InsertExam(examkind, teamid, examtype, title, choisestr, answerstr, answeryesno, score);
            }
            else
            {
                rst = examModel.UpdateExam(uid, examkind, teamid, examtype, title, choisestr, answerstr, answeryesno, score);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ExamBook Manage
        [Authorize(Roles = "OnlineTest")]
        public ActionResult ExamBookList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamBookList";


            return View();
        }

        [Authorize(Roles = "TeamManager")]
        public ActionResult TeamExamBookList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamBookList";


            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveExamBookList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = examModel.GetExamBookDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveTeamExamBookList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            JqDataTableInfo rst = examModel.GetTeamExamBookDataTable(param, Request.QueryString, rootUri,teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "OnlineTest")]
        public ActionResult AddExamBook()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamBookList";
            ViewData["examids"] = "";
            ViewData["teamlist"] = teamModel.GetTeamList();
            return View();
        }

        [Authorize(Roles = "OnlineTest,TeamManager")]
        public ActionResult AddTeamExamBook()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamBookList";
            ViewData["examids"] = "";
            ViewData["teamlist"] = teamModel.GetTeamList();

            long teamid = 0;
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("TeamManager"))
            {
                var uid = CommonModel.GetSessionUserID();
                teamid = teamModel.GetExecparentIdFromUserId(uid);
            }
            ViewData["userteamid"] = teamid;
            return View();
        }
        [Authorize(Roles = "OnlineTest,TeamManager")]
        public ActionResult EditExamBook(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamBookList";

            var bookinfo = examModel.GetExamBookInfo(id);
            ViewData["bookinfo"] = bookinfo;
            ViewData["uid"] = bookinfo.uid;
            ViewData["examids"] = bookinfo.examidstrs;
            ViewData["teamlist"] = teamModel.GetTeamList();

            long teamid = 0;
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("TeamManager"))
            {
                var uid = CommonModel.GetSessionUserID();
                teamid = teamModel.GetExecparentIdFromUserId(uid);
            }
            ViewData["userteamid"] = teamid;

            return View("AddExamBook");
        }

        //删除试卷
        [Authorize(Roles = "OnlineTest,TeamManager")]
        [HttpPost]
        public JsonResult DeleteExamBook(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = examModel.DeleteExamBook(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "OnlineTest,TeamManager")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitExamBook(long uid, byte examkind, string title, int examtime, string contents, string examids,long teamid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = examModel.InsertExamBook(examkind, title, examtime, contents, examids,teamid);
            }
            else
            {
                rst = examModel.UpdateExamBook(uid, examkind, title, examtime, contents, examids, teamid);
            }
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SectorExam
        [Authorize]
        public ActionResult SectorExam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "SectorExam";

            return View();
        }

        [Authorize]
        public ActionResult TeamExam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "TeamExam";

            return View();
        }

        //考试试题列表
        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveBookList(byte examkind, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = examModel.GetBookDataTable(examkind, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [Authorize]
        public ActionResult PreviewExam(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "SectorExam";

            var bookinfo = examModel.GetExamBookInfo(id);
            ViewData["bookinfo"] = bookinfo;

            return View();
        }

        [Authorize]
        public ActionResult ApplyExam(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "SectorExam";
            ViewData["fixview"] = "1";
            ViewData["showkind"] = "apply";

            var bookinfo = examModel.GetExamBookInfo(id);
            ViewData["bookinfo"] = bookinfo;
            ViewData["uid"] = bookinfo.uid;

            var examlist = examModel.GetJsonExamList(bookinfo.examids);
            //string jsons = examModel.GetJsonSerializedString(examlist);

            ViewData["examlist"] = examlist;

            return View();
        }
        [Authorize(Roles = "OnlineTest")]
        [Authorize]
        public ActionResult ResultList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamResult";

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;
            ViewData["grouplist"] = null;
            if (teamlist != null)
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;
            }

            return View();
        }
        [Authorize(Roles = "TeamManager")]
        [Authorize]
        public ActionResult TeamResultList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamResult";
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            ViewData["teamid"] = teamid;

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            return View();
        }
        [AjaxOnly]
        public JsonResult GetgroupList()
        {
            var id = int.Parse(Request.QueryString["teamid"].ToString());
            var groupmodel = new GroupModel();
            var rst = groupmodel.GetGroupListOfTeam(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveResultList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            if (Request.QueryString["starttime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            JqDataTableInfo rst = examModel.GetExamResultDataTable(starttime, endtime, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult SerchResultList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            if (Request.QueryString["starttime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }
            var userkind = int.Parse(Request.QueryString["userkind"]);
            var examtype = int.Parse(Request.QueryString["examtype"]);
            var teamid = long.Parse(Request.QueryString["teamid"]);
            var groupid = long.Parse(Request.QueryString["groupid"]);
            var exambook = long.Parse(Request.QueryString["exambook"]);

            JqDataTableInfo rst = examModel.SearchExamResultDataTable(starttime, endtime, groupid,  teamid, userkind, examtype,exambook, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult SerchTeamResultList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            if (Request.QueryString["starttime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }
            var examtype = int.Parse(Request.QueryString["examtype"]);
            var teamid = long.Parse(Request.QueryString["teamid"]);
            var groupid = long.Parse(Request.QueryString["groupid"]);
            var exambookid = long.Parse(Request.QueryString["exambook"]);

            JqDataTableInfo rst = examModel.SearchTeamExamResultDataTable(starttime, endtime, groupid, teamid, examtype,exambookid, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ResultDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "SectorExam";
            ViewData["fixview"] = "1";
            ViewData["showkind"] = "detail";

            var bookid = examModel.GetBookidByExamresulId(id);

            var bookinfo = examModel.GetExamBookInfoIncludeDeleted(bookid);
            ViewData["bookinfo"] = bookinfo;
            ViewData["uid"] = bookinfo.uid;

            var examlist = examModel.GetJsonExamList(bookinfo.examids);
            ViewData["examlist"] = examlist;

            var result = examModel.GetResultFromBookID(id);
            ViewData["result"] = result;
            var usedtime = int.Parse(examModel.GetUsedtimeFromExamID(id).examsecond);
            ViewData["usedtime"] = Convert.ToInt32(usedtime / 60) + "分" + Convert.ToInt32(usedtime % 60) + "秒";

            return View("ApplyExam");
        }

        [Authorize]
        public ActionResult MyResultDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "SectorExam";
            ViewData["fixview"] = "1";
            ViewData["showkind"] = "detail";

            var bookinfo = examModel.GetExamBookInfoIncludeDeleted(id);
            ViewData["bookinfo"] = bookinfo;
            ViewData["uid"] = bookinfo.uid;

            var examlist = examModel.GetJsonExamList(bookinfo.examids);
            ViewData["examlist"] = examlist;

            var result = examModel.GetMyResultFromBookID(id);
            ViewData["result"] = result;
            var usedtime = int.Parse(examModel.GetMyUsedtimeFromBookID(id).examsecond);
            ViewData["usedtime"] = Convert.ToInt32(usedtime / 60) + "分" + Convert.ToInt32(usedtime % 60) + "秒";

            return View("ApplyExam");
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitApplyExam(long uid)
        {
            string rst = "";
            string usedtime = Request.QueryString["usedtime"];
            rst = examModel.SubmitExamResult(uid,usedtime, Request.Form);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ImportExam()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamList";
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("TeamManager"))
            {
                ViewData["userrole"] = "TeamManager";

            }
            else if (userrole != null && ((string)userrole).Contains("OnlineTest"))
            {
                ViewData["userrole"] = "OnlineTest";

            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitExamImport(string fileurl)
        {
            string rst = "";

            rst = examModel.ImportExamData(fileurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitTeamExamImport(string fileurl)
        {
            string rst = "";

            rst = examModel.ImportTeamExamData(fileurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult CheckUniqueBookName(string title, long uid)
        {
            bool rst = examModel.CheckDuplicateBookName(title, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveSelectedBookExam(string examids/*, string selids*/, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = examModel.GetSelectedExamDataTable(examids, /*selids, */param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "OnlineTest,TeamManager")]
        public ActionResult ExamStatistic()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamStatistic";

            var teamlist = teamModel.GetTeamList();
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            ViewData["teamid"] = teamid;
            ViewData["teamlist"] = teamlist;

            ViewData["grouplist"] = null;
            if (teamlist != null)
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;
            }

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }

        [Authorize(Roles = "TeamManager")]
        public ActionResult TeamExamStatistic()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Exam";
            ViewData["level2nav"] = "ExamStatistic";

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            ViewData["grouplist"] = null;
            if (teamlist != null)
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;
            }

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }
        [Authorize(Roles = "OnlineTest,TeamManager")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult GetExamChartData(long teamid, long groupid, string starttime, string endtime)
        {
            var rst = examModel.GetExamChartData(teamid, groupid, starttime, endtime);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public void ExportExamResult()
        {
            var datalist = examModel.ExportResultList();
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=考试成绩" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        [Authorize]
        public void ExportExamSerchResult()
        {

            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            if (Request.QueryString["starttime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }
            var userkind = int.Parse(Request.QueryString["userkind"]);
            var examtype = int.Parse(Request.QueryString["examtype"]);
            var teamid = long.Parse(Request.QueryString["teamid"]);
            var groupid = long.Parse(Request.QueryString["groupid"]);
            var exambook = long.Parse(Request.QueryString["exambook"]);

            var datalist = examModel.ExportSearchResultList(starttime, endtime, groupid, teamid, userkind, examtype, exambook);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=考试成绩" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        [Authorize]
        public void ExportTeamExamResult()
        {
            long groupid = long.Parse(Request.QueryString["groupid"].ToString());
            long teamid = long.Parse(Request.QueryString["teamid"].ToString());
            int examtype = int.Parse(Request.QueryString["examtype"].ToString());
            int exambookid = int.Parse(Request.QueryString["exambook"].ToString());


            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            if (Request.QueryString["starttime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null && !string.IsNullOrEmpty(Request.QueryString["starttime"]))
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            var datalist = examModel.ExportTeamResultList(starttime, endtime, teamid, groupid, examtype, exambookid);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=考试成绩" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult SerchExamList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var testtype = Request.QueryString["testtype"].ToString();
            int examtype = 0;
            var examkind = int.Parse(Request.QueryString["examkind"]);
            var teamid = long.Parse(Request.QueryString["teamid"]);

            if (!string.IsNullOrEmpty(testtype))
            {
                examtype = int.Parse(testtype);
            }

            JqDataTableInfo rst = examModel.GetExamDataTableWithCondition(examkind, teamid, examtype, 0, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SerchTeamExamList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var testtype = Request.QueryString["testtype"].ToString();
            int examtype = 0;
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            if (!string.IsNullOrEmpty(testtype))
            {
                examtype = int.Parse(testtype);
            }

            JqDataTableInfo rst = examModel.GetExamDataTableByTeamid(examtype, 0, param, Request.QueryString, rootUri, teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RandomGenerate(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var testtype = Request.QueryString["testtype"].ToString();
            var testamount = Request.QueryString["testamount"].ToString();

            int examtype = 0;
            if (!string.IsNullOrEmpty(testtype))
            {
                examtype = int.Parse(testtype);
            }
            int testamontt = 0;
            if (!string.IsNullOrEmpty(testtype) && int.TryParse(testamount, out testamontt))
            {
                testamontt = int.Parse(testamount);
            }
            JqDataTableInfo rst = examModel.GetExamDataTable(examtype,testamontt, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GenerateExamDataTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var testtype = Request.QueryString["testtype"].ToString();
            var testamount = Request.QueryString["testamount"].ToString();
            var examkind = int.Parse(Request.QueryString["examkind"]);
            var teamid = long.Parse(Request.QueryString["teamid"]);

            int examtype = 0;
            if (!string.IsNullOrEmpty(testtype))
            {
                examtype = int.Parse(testtype);
            }
            int testamontt = 0;
            if (!string.IsNullOrEmpty(testtype) && int.TryParse(testamount, out testamontt))
            {
                testamontt = int.Parse(testamount);
            }
            JqDataTableInfo rst = examModel.GenerateExamDataTable(examkind, teamid, examtype, testamontt, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult GenerateTeamExamDataTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var testtype = Request.QueryString["testtype"].ToString();
            var testamount = Request.QueryString["testamount"].ToString();

            int examtype = 0;
            if (!string.IsNullOrEmpty(testtype))
            {
                examtype = int.Parse(testtype);
            }
            int testamontt = 0;
            if (!string.IsNullOrEmpty(testtype) && int.TryParse(testamount, out testamontt))
            {
                testamontt = int.Parse(testamount);
            }

            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            JqDataTableInfo rst = examModel.GenerateExamDataTableByTeamid(teamid, examtype, testamontt, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize]
        [AjaxOnly]
        public JsonResult GetExambokkListByExamtype()
        {
            var uid = CommonModel.GetSessionUserID();
            var teamid = teamModel.GetExecparentIdFromUserId(uid);
            var examtype = int.Parse(Request.QueryString["examtype"].ToString());
            var rst = examModel.GetTeamExamBookListofType(examtype, teamid);
            return Json(rst,JsonRequestBehavior.AllowGet);
        }
    }
}