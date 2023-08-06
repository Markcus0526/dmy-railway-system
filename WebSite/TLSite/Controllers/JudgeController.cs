using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace TLSite.Controllers
{
    public class JudgeController : Controller
    {
        JudgeModel judgeModel = new JudgeModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        UserModel userModel = new UserModel();
        SectorModel sectorModel = new SectorModel();

        [Authorize(Roles = "Judge,Executive,Coach")]
        public ActionResult AddJudge()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "AddJudge";

            var teamlist = teamModel.GetTeamList();
            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["starttime"] = starttime;

            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();

            return View();
        }

        [Authorize(Roles = "Judge,Executive")]
        public ActionResult AddJudge2()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "AddJudge";

            var teamlist = teamModel.GetTeamList();
            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewData["teamlist"] = teamModel.GetJudgeTeamList(starttime);
            ViewData["starttime"] = starttime;

            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();

            return View();
        }

        [Authorize(Roles = "Judge,Executive,Coach")]
        public ActionResult AddCombineJudge()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "AddCombineJudge";

            var teamlist = teamModel.GetTeamList();
            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewData["teamlist"] = teamlist;
            ViewData["starttime"] = starttime;

            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();
            return View();
        }

        [Authorize(Roles = "Judge,Executive")]
        public ActionResult AddScore()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "AddScore";

            var teamlist = teamModel.GetTeamList();
            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewData["teamlist"] = teamlist;
            ViewData["starttime"] = starttime;

            return View();
        }
        [Authorize]
        public ActionResult JudgeList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "JudgeList";

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            ViewData["grouplist"] = null;
            if (teamlist != null)   
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;

                if (grouplist != null)
                {
                    ViewData["crewlist"] = userModel.GetGroupCrewList(grouplist[0].uid);
                }
            }

            return View();
        }

        public static long teamid = 0;
        public static long groupid = 0;
        public static long crewid = 0;
        public static DateTime starttime = new DateTime(1970, 1, 1);
        public static DateTime endtime = new DateTime(2030, 1, 1);

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveJudgeList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);
            long teamid = 0;
            long groupid = 0;
            long crewid = 0;

            if (Request.QueryString["starttime"] != null)
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {
                	
                }
            }

            if (Request.QueryString["endtime"] != null)
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["teamid"] != null)
            {
                try
                {
                    teamid = long.Parse(Request.QueryString["teamid"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["groupid"] != null)
            {
                try
                {
                    groupid = long.Parse(Request.QueryString["groupid"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["crewid"] != null)
            {
                try
                {
                    crewid = long.Parse(Request.QueryString["crewid"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            JqDataTableInfo rst = judgeModel.GetJudgeDataTable(starttime, endtime, teamid, groupid, crewid, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult JudgeDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "JudgeList";
            ViewData["detailinfo"] = judgeModel.GetJudgeDetail(id);

            return View();
        }

        [Authorize(Roles = "Judge,Executive")]
        public ActionResult JudgeAnalysis()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "JudgeAnalysis";

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            ViewData["grouplist"] = null;
            if (teamlist.Count != 0)
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;

                if (grouplist.Count!=0)
                {
                    ViewData["crewlist"] = userModel.GetGroupCrewList(grouplist[0].uid);
                }
            }

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindTrainnoOntblDutyByGroup()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var groupid = Request.QueryString["groupid"];
            long groupidd=0;
            if (groupid != "" && groupid!=null)
            {
                 groupidd = long.Parse(groupid);
            }
            var starttime = Request.QueryString["starttime"];
            long result = 0;
            var rst = userModel.GetDutynoByGroup(starttime, groupidd);
            if (rst.Count>0)
            {
                result = rst[0];
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindJudgeCrewList(long id, string starttime)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = userModel.GetJudgeCrewList(id, starttime);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindJudgeCheckerList(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var checklevel = Request.QueryString["checklevel"];

           // var rst = new List<CrewInfo>();

            if (checklevel=="段以上")
            {

            //return Json(rst, JsonRequestBehavior.AllowGet);

            }

            else if (checklevel == "段级")
            {
                var rst = userModel.GetSecCheckerNameList(id);
                return Json(rst, JsonRequestBehavior.AllowGet);
            }

            else if (checklevel == "车队")
            {
                var rst = userModel.GetTeamCheckerNameList(id);
                return Json(rst, JsonRequestBehavior.AllowGet);
            }

            else if (checklevel == "班组")
           {
               var rst = userModel.GetTrainCoach(id);
             return Json(rst, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
            
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindJudgeTeamList(string starttime)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = teamModel.GetJudgeTeamList(starttime);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindAddscoreCrewList(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = userModel.GetGroupUserList(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetGroupListOfTeam(long id)
        {
            var rst = groupModel.GetGroupListOfTeam(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetCrewListOfGroup(long id)
        {
            var rst = userModel.GetGroupCrewList(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupListOfTeam(long id, string starttime)
        {
            var rst = groupModel.GetJudgeGroupList(id, starttime);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupListOfTeamAdditionDailyGroup(long id, string starttime)
        {
            var rst = groupModel.GetJudgeGroupListAdditionDailyGroup(id, starttime);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupListByTeam(long id)
        {
            var rst = groupModel.GetGroupListOfTeam(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitJudge(long uid, string starttime, string checktime, long teamid, long groupid, long crewid, long? relcrewid,
            long selcheckid, string contents, string imgurl, string trainno, string checkername, string checkersecname, string checklevel, string checkerid)

        {
            string rst = "";

            //判断是否超过48小时
            var currenttime=DateTime.Now;
            editable edit;
            DateTime starttimee = DateTime.Now;
            try
            {
                starttimee = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {
            	
            }

            edit = judgeModel.getDutyendtime(starttimee, groupid);

            if (edit.endtime.AddHours(48) <= currenttime)
            {
                if (edit.forbid==1)
                {
                    rst = "超过48小时，不能录入";
                    return Json(rst, JsonRequestBehavior.AllowGet);
                }
            }

            if (uid == 0)
            {
                rst = judgeModel.InsertJudge(starttime, checktime, teamid, groupid, crewid, relcrewid, selcheckid, contents, imgurl, trainno, checkersecname, checkername, checklevel, checkerid);
            }
            else
            {
                //rst = dutyModel.UpdateDuty(uid, starttime, endtime, teamid, routeid, groupid, trainno);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive")]
        public string SubmitJudgeList()
        {
           return judgeModel.AddJduge();
           // AddJudge2();
          //  return View("AddJudge2");
        }

        [Authorize(Roles = "Judge,Executive")]
        public void SubmitJudgeofCombine()
        {
            judgeModel.AddJdugeofCombine();
        }

        [Authorize(Roles = "Judge,Executive")]
        public void SubmitJudgeofAdd()
        {
            judgeModel.AddJdugeofAdd();
        }
        //得到临时记录上传信息列表
        [Authorize(Roles = "Judge,Executive,Coach")]
        [AjaxOnly]
        public JsonResult GetStoreList()
        {
            var test = new List<CheckLogInfo>();
            test = judgeModel.GetStoreList();
            foreach (var c in test)
            {
                if (c.relpoint == null)
                {
                    c.relcrewname = "";
                }
                if (c.checklevel=="车队"&&c.chkpoint!=50&&c.chkpoint!=60)
                {
                    c.chkpoint = c.chkpoint * 0.5;
                    if (c.relpoint != null)
                    {
                        c.relpoint = Convert.ToString(Math.Round(double.Parse(c.relpoint) * 0.5, 1, MidpointRounding.AwayFromZero));
                    }
                }

                else if (c.checklevel == "班组" )
                {
                   if (c.chkpoint != 50 && c.chkpoint != 60)
                   {
                       c.chkpoint = Math.Round(c.chkpoint * 0.25, 1, MidpointRounding.AwayFromZero);

                   }
                    c.relpoint = "";
                    c.relcrewname = "";
                }
            }
            var result = from c in test
                         select new[]{
                                    c.teamname ,
                                    c.crewno ,
                                    c.crewname ,
                                    c.groupname ,
                                    c.rolename ,
                                    c.checkno,
                                    Convert.ToString(c.chkpoint),
                                    c.relcrewno ,
                                    c.relcrewname ,
                                    c.rolename!="列车长"? c.relpoint:"",
                                    c.contents,
                                    c.checkpart,                                            
                                    c.checkername,
                                    String.Format("{0:yyyy-MM-dd}",c.checktime),
                                    Convert.ToString(c.uid),
                        };
            return Json(new
            {
                // sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive,Coach")]
        [AjaxOnly]
        public JsonResult GetStoreListofCombine()
        {
            var test = new List<CheckLogInfo>();
            test = judgeModel.GetStoreListofCombine();
            var result = from c in test
                         select new[]{
                                    c.recievepart ,
                                    c.recievename ,
                                    c.groupname ,
                                    c.checkno ,
                                    Convert.ToString(c.chkpoint),
                                    c.contents,
                                    c.checkpart,                                            
                                    c.checkername,
                                    String.Format("{0:yyyy-MM-dd}",c.checktime),
                                    Convert.ToString(c.uid)
                        };
            return Json(new
            {
                // sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive")]
        [AjaxOnly]
        public JsonResult GetStoreListofAdd()
        {
            var test = new List<CheckLogInfo>();
            test = judgeModel.GetStoreListofAdd();
            var result = from c in test
                         select new[]{
                                    c.teamname ,
                                    c.crewno ,
                                    c.crewname ,
                                    c.groupname ,
                                    c.rolename,
                                    Convert.ToString(c.chkpoint),                                            
                                    c.checkno,
                                    c.contents,
                                    String.Format("{0:yyyy-MM-dd}",c.checktime),
                                    Convert.ToString(c.uid),
                        };
            return Json(new
            {
                // sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }
        
        //删除storelist中的一条
        [Authorize(Roles = "Judge")]
        [HttpPost]
        public JsonResult DeleteStroeList(long delids)
        {
            bool rst = judgeModel.DeleteTempLog(delids);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult Test(long uid, string starttime, string checktime, long teamid, long groupid, long crewid, long? relcrewid,
            long selcheckid, string contents, string imgurl, string trainno, string checkername, string checkersecname, string checklevel, string checkerid)
        {
            string rst = "";
            //var test = new List<CheckLogInfo>();

            //判断是否超过48小时
            var currenttime = DateTime.Now;
            editable edit;
            DateTime starttimee = DateTime.Now;
            try
            {
                starttimee = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }
            if (uid == 0)
            {
                edit = judgeModel.getDutyendtime(starttimee, groupid);
                //判断是否日勤组
                if (groupModel.CheckIfDailiGroup(groupid))
                {
                    rst = judgeModel.InsertTemplog(0,starttime, checktime, teamid, groupid, crewid, relcrewid, selcheckid, contents, imgurl, trainno, checkersecname, checkername, checklevel, checkerid);
                }
                else
                {
                    try
                    {
                        if (edit.endtime.AddHours(48) <= currenttime)
                        {
                            if (edit.forbid == 1)
                            {
                                rst = "超过48小时，不能录入";
                                return Json(rst, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                rst = judgeModel.InsertTemplog(1, starttime, checktime, teamid, groupid, crewid, relcrewid, selcheckid, contents, imgurl, trainno, checkersecname, checkername, checklevel, checkerid);
                            }
                        }
                        else
                        {
                            rst = judgeModel.InsertTemplog(1,starttime, checktime, teamid, groupid, crewid, relcrewid, selcheckid, contents, imgurl, trainno, checkersecname, checkername, checklevel, checkerid);
                        }

                    }
                    catch (System.Exception ex)
                    {
                        rst = "该检查日期无出乘记录！";
                    }
                }
            }
            else
            {
                rst = judgeModel.UpdateJudge(uid, checktime, teamid, groupid, crewid, relcrewid, selcheckid, contents, imgurl, checkersecname, checkername, checklevel, checkerid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //结合部考核添加
        [Authorize(Roles = "Judge,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitCombineJudge(long uid, string starttime, string checktime, long teamid, long groupid,
            long selcheckid, string contents, string imgurl, string trainno, string checkername, string checkersecname, string checklevel, string checkerid, string crewsector, string crewname)
        {
            string rst = "";

            //判断是否超过48小时
            var currenttime = DateTime.Now;
            editable edit;
            DateTime starttimee = DateTime.Now;
            try
            {
                starttimee = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }
            if (uid == 0)
            {
                edit = judgeModel.getDutyendtime(starttimee, groupid);

                if (edit.endtime.AddHours(48) <= currenttime)
                {
                    if (edit.forbid == 1)
                    {
                        rst = "超过48小时，不能录入";
                        return Json(rst, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    rst = judgeModel.InsertCombineJudge(checktime, teamid, groupid, selcheckid, contents, imgurl, trainno, checkersecname, checkername, checklevel, checkerid, crewsector, crewname);

                }
            }
            else
            {
                rst = judgeModel.UpdateCombineJudge(uid, checktime, teamid, groupid, selcheckid, contents, imgurl, checkersecname, checkername, checklevel, checkerid, crewsector, crewname);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        //激励加分添加
        [Authorize(Roles = "Judge,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitAddScore(long uid, string checktime, long teamid, long groupid, string groupname, long crewid, long selcheckid, string contents, string imgurl)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = judgeModel.InsertAddScore(checktime, teamid, groupid, groupname, crewid, selcheckid, contents, imgurl);
            }
            else
            {
                rst = judgeModel.UpdateAddScore(uid,checktime, teamid, groupid, groupname, crewid, selcheckid, contents, imgurl);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Judge,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult GetJudgeChartData(long teamid, string checklevel, string checkno, string starttime, string endtime, string selecetop)
        {

            var rst = judgeModel.GetJudgeChartData( teamid,  checklevel,  checkno,  starttime,  endtime,  selecetop);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public void ExportJudgeList()
        {
            var datalist = judgeModel.ExportJudgeList(teamid, groupid, crewid, starttime, endtime);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=干部考核" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", fileName);
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
        
       
        //删除storelist中的一条
        [Authorize(Roles = "Judge")]
        [HttpPost]
        public JsonResult DeleteChecklog(string delids)
        {

            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();

            bool rst = judgeModel.DeleteCheckLog(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }


        #region 编辑考核信息

        [Authorize(Roles = "Judge")]
        public ActionResult EditJudge()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "EditJudge";
            ViewData["DuanSectorlist"] = sectorModel.GetSectorList();

            ViewData["Sectorlist"] = teamModel.GetTeamList();

            ViewData["crewlist"] = userModel.GetCrewListByGroupidandTeamid(0, 0);
            //   ViewData["starttime"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            //ViewData["checkerteamlist"] = teamModel.GetTeamList();
            //ViewData["checkergrouplist"] = groupModel.GetGroupList();
            //ViewData["checkersectorlist"] = sectorModel.GetSectorList();

            return View();
        }

        [AjaxOnly]
        public JsonResult GetuserList()
        {
            var teamid = int.Parse(Request.QueryString["tramid"].ToString());
            var groupid = int.Parse(Request.QueryString["groupid"].ToString());
            var rst = userModel.GetCrewListByGroupidandTeamid(groupid, teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public ActionResult ShowTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var checktype = Request.QueryString["checktype"].ToString();
            var starttime = Request.QueryString["starttime"].ToString();
            var endtime = Request.QueryString["endtime"].ToString();
            var checkexecname = Request.QueryString["checkexecname"].ToString();
            var checkername = Request.QueryString["checkername"].ToString();
           
            // var checkername = Request.QueryString["checkername"].ToString();
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
           // var groupid = 

            long groupid = 0;
            try
            {
                //userid = long.Parse(Request.QueryString["userid"].ToString());
                 groupid = long.Parse(Request.QueryString["groupid"].ToString());
            }
            catch
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                },
                 JsonRequestBehavior.AllowGet);
            }

            long userid = 0;
            try
            {
                userid = long.Parse(Request.QueryString["userid"].ToString());

            }
            catch 
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                },
                 JsonRequestBehavior.AllowGet) ;
            }

            //var checklevel = Request.QueryString["checklevel"].ToString();
            var checkno = Request.QueryString["checkno"].ToString();
            var trainno = Request.QueryString["trainno"].ToString();
            var keyword = Request.QueryString["keyword"].ToString();
            //  var chkpoint = int.Parse(Request.QueryString["chkpoint"].ToString());


            var rst = judgeModel.GetEditCheckLogList(checktype, starttime, endtime, sectorid, groupid, userid, checkno, trainno, keyword, checkername, checkexecname);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.logtype,
                               c.teamname,
                               c.crewno,
                               c.crewname,
                               c.groupname,
                               c.rolename,
                               c.trainno,
                               c.checkno,
                              Convert.ToString( c.chkpoint),
                               c.contents,
                               c.checkersector,
                               c.checkername,
                               String.Format("{0:yyyy-MM-dd}",c.checktime),
                               Convert.ToString(c.uid)
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            },
                 JsonRequestBehavior.AllowGet);
        }
       

        public ActionResult EditCheck(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "EditJudge";
            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();
            ViewData["Judge"] = judgeModel.GetEditJudgeDetail(id);
            return View();
        }

        public ActionResult EditCombine(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "EditJudge";
            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();
            ViewData["Judge"] = judgeModel.GetEditCombineJudgeDetail(id);
            return View();
        }
        public ActionResult EditAddScroe(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Judge";
            ViewData["level2nav"] = "EditJudge";
            ViewData["checkerteamlist"] = teamModel.GetTeamList();
            ViewData["checkergrouplist"] = groupModel.GetGroupList();
            ViewData["checkersectorlist"] = sectorModel.GetSectorList();
            ViewData["Judge"] = judgeModel.GetEditJudgeDetail(id);
            return View();
        }


        #endregion
    }
}