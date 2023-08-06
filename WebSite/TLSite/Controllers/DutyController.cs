using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;

namespace TLSite.Controllers
{
    public class DutyController : Controller
    {
        DutyModel dutyModel = new DutyModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        RouteModel routeModel = new RouteModel();
        TrainNoModel trainNoModel = new TrainNoModel();
        UserModel userModel = new UserModel();

        CreditOfPartyModel cop = new CreditOfPartyModel();


        [Authorize(Roles = "Duty,Executive,TeamAdmin")]

        public ActionResult DutyList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Duty";
            ViewData["level2nav"] = "Duty";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            var userrole = CommonModel.GetUserRoleInfo();

            var teamlist = teamModel.GetTeamList();

            if (((string)userrole).Contains("TeamAdmin"))
            {
                TeamAdminModel teamadmodel = new TeamAdminModel();
                var currentteam = teamadmodel.GetCurrentTeam();
                ViewData["teamlist"] = currentteam;
                if (currentteam == null)
                {
                    ViewData["grouplist"] = null;
                }
                else if (teamlist.Count() > 0)
                {
                    ViewData["grouplist"] = groupModel.GetGroupListOfTeam(currentteam[0].uid);
                }
            }
            else
            {
                ViewData["teamlist"] = teamlist;
                if (teamlist == null)
                {
                    ViewData["grouplist"] = null;
                }
                else if (teamlist.Count() > 0)
                {
                    ViewData["grouplist"] = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                }
            }

            return View();
        }
        [Authorize]
        [AjaxOnly]
        public JsonResult GetCrewListByGroup(long id)
        {
            var rst = userModel.GetUserListByGroupid(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetCrewListByTeam(long id)
        {
            var rst = userModel.GetUserListByTeamid(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveDutyList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = dutyModel.GetDutyDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }




        [Authorize(Roles = "Duty,TeamExec,TeamAdmin")]
        public ActionResult AddDuty()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Duty";
            ViewData["level2nav"] = "Duty";
            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            if (teamlist == null)
            {
                ViewData["grouplist"] = null;
            }
            else if (teamlist.Count() > 0)
            {
                ViewData["grouplist"] = groupModel.GetGroupListOfTeam(teamlist[0].uid);
            }

            var routelist = routeModel.GetRouteList();
            ViewData["routelist"] = routelist;

            if (routelist == null)
            {
                ViewData["trainnolist"] = null;
            }
            else
            {
                ViewData["trainnolist"] = trainNoModel.GetTrainNoListOfRoute(routelist[0].uid);
            }

            return View();
        }

        [Authorize(Roles = "Duty,TeamExec,TeamAdmin")]
        public ActionResult NewDuty()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Duty";
            ViewData["level2nav"] = "Duty";

            var userrole = CommonModel.GetUserRoleInfo();

            var teamlist = teamModel.GetTeamList();

            if (((string)userrole).Contains("TeamAdmin"))
            {
                TeamAdminModel teamadmodel = new TeamAdminModel();
                var currentteam = teamadmodel.GetCurrentTeam();
                ViewData["teamlist"] = currentteam; 
            }
            else
            {
                ViewData["teamlist"] = teamlist;                
            }

            if (teamlist == null)
            {
                ViewData["grouplist"] = null;
            }
            else if (teamlist.Count() > 0)
            {
                ViewData["grouplist"] = groupModel.GetGroupListOfTeam(teamlist[0].uid);
            }

            var routelist = routeModel.GetRouteList();
            ViewData["routelist"] = routelist;

            if (routelist == null)
            {
                ViewData["trainnolist"] = null;
            }
            else
            {
                ViewData["trainnolist"] = trainNoModel.GetTrainNoListOfRoute(routelist[0].uid);
            }

            return View();
        }

        [Authorize(Roles = "Duty,TeamExec,TeamAdmin")]
        public ActionResult EditDuty(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Duty";
            ViewData["level2nav"] = "Duty";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditDuty", "", rootUri);

            var dutyinfo = dutyModel.GetDutyById(id);
            ViewData["dutyinfo"] = dutyinfo;
            ViewData["uid"] = dutyinfo.uid;
            ViewData["starttime"] = String.Format("{0:yyyy-MM-dd}", dutyinfo.starttime);
            ViewData["endtime"] = String.Format("{0:yyyy-MM-dd}", dutyinfo.endtime);
            ViewData["teamid"] = dutyinfo.teamid;
            ViewData["routeid"] = dutyinfo.routeid;
            ViewData["groupid"] = dutyinfo.groupid;
            ViewData["trainno"] = dutyinfo.trainno;
            ViewData["editable"] = dutyinfo.forbide;

            var userrole = CommonModel.GetUserRoleInfo();

            var teamlist = teamModel.GetTeamList();

            if (((string)userrole).Contains("TeamAdmin"))
            {
                TeamAdminModel teamadmodel = new TeamAdminModel();
                var currentteam = teamadmodel.GetCurrentTeam();
                ViewData["teamlist"] = currentteam;
            }
            else
            {
                ViewData["teamlist"] = teamlist;
            }

            ViewData["grouplist"] = groupModel.GetGroupListOfTeam(dutyinfo.teamid);
            ViewData["routelist"] = routeModel.GetRouteList();
            ViewData["trainnolist"] = trainNoModel.GetTrainNoListOfRoute(dutyinfo.routeid);

            return View("NewDuty");
        }

        [Authorize(Roles = "Duty,TeamExec,TeamAdmin")]
        [HttpPost]
        public JsonResult DeleteDuty(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = dutyModel.DeleteDuty(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Duty,TeamExec,TeamAdmin")]
        [HttpPost]
        [AjaxOnly]


        public JsonResult SubmitDuty(long uid, string starttime, string endtime, long teamid, long routeid, long groupid, string trainno)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = dutyModel.InsertDuty(starttime, endtime, teamid, routeid, groupid, trainno);
            }
            else
            {
                rst = dutyModel.UpdateDuty(uid, starttime, endtime, teamid, routeid, groupid, trainno);
            }

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
        public JsonResult GetTrainNoListOfRoute(long id)
        {
            var rst = trainNoModel.GetTrainNoListOfRoute(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //下方列表显示查询结果
        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveDutyCrewList(long id, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string act = "edit";
            long groupid = 0;

            if (Request.QueryString["act"] != null)
            {
                act = Request.QueryString["act"].ToString();
            }
            if (Request.QueryString["groupid"] != null)
            {
                groupid = long.Parse(Request.QueryString["groupid"].ToString());
            }

            JqDataTableInfo rst = userModel.GetDutyCrewDataTable(id, act, groupid, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult ShowAllCrewOfGroup(long id, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string act = "edit";
            long groupid = 0;

            if (Request.QueryString["act"] != null)
            {
                act = Request.QueryString["act"].ToString();
            }
            if (Request.QueryString["groupid"] != null)
            {
                groupid = long.Parse(Request.QueryString["groupid"].ToString());
            }

            JqDataTableInfo rst = userModel.GetCrewList(id, act, groupid, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindAbleCrewList(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = userModel.GetGroupCrewList(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Duty")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitAdditionCrew(long id, string crewids)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = userModel.InsertAddtionCrew(id, crewids);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Duty,TeamExec")]
        [AjaxOnly]
        public JsonResult DeleteDutyCrew(long id, long crewid)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = userModel.DeleteDutyCrew(id, crewid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }



        [AjaxOnly]
        public ActionResult RefreshTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var starttime = Request.QueryString["starttime"].ToString();
            var endtime = Request.QueryString["endtime"].ToString();

            var teamid = long.Parse(Request.QueryString["teamid"].ToString());
            var groupid = long.Parse(Request.QueryString["groupid"].ToString());

            var rst = dutyModel.Serch(groupid, teamid, starttime, endtime);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }


            var result = from c in displayedCompanies
                         select new[]{
                                    Convert.ToString(c.uid), 
                                     c.teamname,
                                     c.routename,
                                     c.groupname,
                                     c.trainno,
                                     String.Format("{0:yyyy-MM-dd}", c.starttime),
                                     String.Format("{0:yyyy-MM-dd}", c.endtime),
                                      Convert.ToString(c.uid), 
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

        //个人乘务信息查询
        [AjaxOnly]
        public ActionResult RefreshTable2(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var starttime = Request.QueryString["starttime"].ToString();
            var endtime = Request.QueryString["endtime"].ToString();

//             var teamid = long.Parse(Request.QueryString["teamid"].ToString());
//             var groupid = long.Parse(Request.QueryString["groupid"].ToString());
            var uid = long.Parse(Request.QueryString["uid"].ToString());

            var rst = dutyModel.Serch2( starttime, endtime,uid);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }


            var result = from c in displayedCompanies
                         select new[]{
                                     c.teamname,
                                     c.groupname,
                                     c.crewno,
                                     c.name,
                                     c.rolename,
                                     c.trainno,
                                     String.Format("{0:yyyy-MM-dd}", c.starttime),
                                     String.Format("{0:yyyy-MM-dd}", c.endtime),
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

        //改变操作权限
        [AjaxOnly]
        public JsonResult changeforbidable(string uid)
        {
           // int uid = int.Parse(Request.QueryString["uid"].ToString());
            var rst=dutyModel.changeforbid(int.Parse(uid));

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetRoutList(long id)
        {
            // int uid = int.Parse(Request.QueryString["uid"].ToString());
            var rst = routeModel.GetRouteListByTeamid(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
