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
using System.Collections;

namespace TLSite.Controllers
{
    public class CheckInfoController : Controller
    {

        CheckInfoModel chceckinfomodel = new CheckInfoModel();
        CombineCheckModel combine = new CombineCheckModel();
        //
        // GET: /CheckInfo/

        [Authorize]
        [AjaxOnly]
        public JsonResult Getgroup()
        {
            var teamid = long.Parse(Request.QueryString["teamid"].ToString());
            GroupModel group = new GroupModel();
            var rst = group.GetGroupListOfTeam(teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        public ActionResult CombinCheck()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["level1nav"] = "CheckInfo";
            ViewData["level2nav"] = "CombinCheck";
            ViewData["rootUri"] = rootUri;
            GroupModel gorupmodel = new GroupModel();
            //受检班组
            ViewData["Grouplist"] = gorupmodel.GetGroupList();
            
            //所属车队
            SectorModel sectormodel = new SectorModel();
            ViewData["Sectorlist"] = sectormodel.GetTeamSectorList();

            //当前部门
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("TeamExec"))
            {
                var uid = CommonModel.GetSessionUserID();
                var teamModel = new TeamModel();
                var teamid = teamModel.GetExecparentIdFromUserId(uid);
                ViewData["teamid"] = teamid;
                ViewData["Grouplist"] = gorupmodel.GetGroupListOfTeam(teamid);
            }

            else if (userrole != null && ((string)userrole).Contains("Crew"))
            {
                var uid = CommonModel.GetSessionUserID();
                var teamModel = new TeamModel();
                var teamid = teamModel.GetTeamIdFromUserId(uid);
                ViewData["teamid"] = teamid;
                ViewData["Grouplist"] = gorupmodel.GetGroupListOfTeam(teamid);

            }
            return View();
        }

        [Authorize]
        public ActionResult CheckCheckinfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["level1nav"] = "CheckInfo";
            ViewData["level2nav"] = "CheckCheckinfo";
            ViewData["rootUri"] = rootUri;
            GroupModel gorupmodel = new GroupModel();
            ViewData["Grouplist"] = gorupmodel.GetGroupList();
            
            
          
            //部门列表
            SectorModel sectormodel = new SectorModel();
            ViewData["Sectorlist"] = sectormodel.GetTeamSectorList();

            //检查部门
            ViewData["CheckerSector"] = chceckinfomodel.getCheckerSector();

            //检查人
            ViewData["CheckerName"] = chceckinfomodel.getCheckerName();

            //当前部门
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("TeamExec"))
            {
                var uid = CommonModel.GetSessionUserID();
                var teamModel = new TeamModel();
                var teamid = teamModel.GetExecparentIdFromUserId(uid);
                ViewData["teamid"] = teamid;
                ViewData["Grouplist"] = gorupmodel.GetGroupListOfTeam(teamid);
            }

            else if (userrole != null && ((string)userrole).Contains("Crew"))
            {
                var uid = CommonModel.GetSessionUserID();
                var teamModel = new TeamModel();
                var teamid = teamModel.GetTeamIdFromUserId(uid);
                ViewData["teamid"] = teamid;
                ViewData["Grouplist"] = gorupmodel.GetGroupListOfTeam(teamid);

            }
            return View();
        }


        //CheckInfo/CheckCheckinfo的table
        [AjaxOnly]
        public ActionResult ShowTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var checkersector = Request.QueryString["checkersector"].ToString();
            var checkername = Request.QueryString["checkername"].ToString();
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
            var checklevel = Request.QueryString["checklevel"].ToString();
            var checkno = Request.QueryString["checkno"].ToString();
            var trainno = Request.QueryString["trainno"].ToString();
            var keyword = Request.QueryString["keyword"].ToString();
            var chkpoint =int.Parse( Request.QueryString["chkpoint"].ToString());
            var groupid = Request.QueryString["groupid"].ToString();
            var userid = long.Parse(Request.QueryString["userid"].ToString());
            var starttime = Request.QueryString["starttime"].ToString();
            var endtime = Request.QueryString["endtime"].ToString();



            var rst = chceckinfomodel.serchbycondition(checkername, checkersector, sectorid, checklevel, checkno, trainno, keyword, chkpoint, groupid, userid, starttime, endtime);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.teamname,
                               c.crewno,
                               c.name,
                               c.groupname,
                               c.rolename,
                               c.trainno,
                               c.checkno,
                              Convert.ToString( c.chkpoint),
                               c.content,
                               c.checkersector,
                               c.checkername,
                               String.Format("{0:yyyy-MM-dd}",c.checktime),
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


        // CheckInfo/CombinCheck的table
        [AjaxOnly]
        public ActionResult ShowTable2(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var date = Request.QueryString["date"].ToString();
            var checklevel = Request.QueryString["checklevel"].ToString();
            var teamid = long.Parse(Request.QueryString["teamid"].ToString());
            var groupid = long.Parse(Request.QueryString["groupid"].ToString());
            var crewname = Request.QueryString["crewname"].ToString();
            var receivepart = Request.QueryString["receivepart"].ToString();

            var rst = combine.serchbycondition(date, checklevel, teamid, groupid, crewname, receivepart);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.receivepart,
                               c.crewname,
                               c.checkno,
                               c.content,
                               c.checkersector,
                               c.checkername,
                               c.teamname,
                               c.groupname,
                               String.Format("{0:yyyy-MM-dd}", c.checktime),
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            },
                 JsonRequestBehavior.AllowGet);
        }


             [Authorize]
        //导出积分表
        public void Export()
        {

            var checkersector = Request.QueryString["checkersector"].ToString();
            var checkername = Request.QueryString["checkername"].ToString();
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
            var checklevel = Request.QueryString["checklevel"].ToString();
            var checkno = Request.QueryString["checkno"].ToString();
            var trainno = Request.QueryString["trainno"].ToString();
            var keyword = Request.QueryString["keyword"].ToString();
            var chkpoint = int.Parse(Request.QueryString["chkpoint"].ToString());
            var groupid = Request.QueryString["groupid"].ToString();
            var userid = long.Parse(Request.QueryString["userid"].ToString());
            var starttime = Request.QueryString["starttime"].ToString();
            var endtime = Request.QueryString["endtime"].ToString();

            var datalist = chceckinfomodel.ExportCreditList(checkername, checkersector, sectorid, checklevel, checkno, trainno, keyword, chkpoint, groupid, userid, starttime, endtime);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=两违考核问题表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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

             public void Export2()
             {

                 var date = Request.QueryString["date"].ToString();
                 var checklevel = Request.QueryString["checklevel"].ToString();
                 var teamid = long.Parse(Request.QueryString["teamid"].ToString());
                 var groupid = long.Parse(Request.QueryString["groupid"].ToString());
                 var crewname = Request.QueryString["crewname"].ToString();
                 var receivepart = Request.QueryString["receivepart"].ToString();


                 var datalist = combine.ExportCreditList(date, checklevel, teamid, groupid, crewname, receivepart);
                 var grid = new GridView();

                 grid.DataSource = datalist;
                 grid.DataBind();


                 string fileName = "attachment; filename=结合部考核问题表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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

             public JsonResult GetgroupList()
             {

                 var id = int.Parse(Request.QueryString["teamid"].ToString());
                 var groupmodel = new GroupModel();
                 var rst = groupmodel.GetGroupListOfTeam(id);
                 return Json(rst, JsonRequestBehavior.AllowGet);
             }
             [AjaxOnly]
             public JsonResult GetuserList()
             {

                 var id = int.Parse(Request.QueryString["groupid"].ToString());
                 var usermodel = new UserModel();
                 var rst = usermodel.GetGroupUserList(id);
                 return Json(rst, JsonRequestBehavior.AllowGet);
             }
             [AjaxOnly]
             public JsonResult GetRouteList()
             {
                 var id = long.Parse(Request.QueryString["teamid"].ToString());
                 var usermodel = new UserModel();
                 RouteModel routemodel = new RouteModel();
                 var rst = routemodel.GetRouteListByTeamid(id);
                 return Json(rst, JsonRequestBehavior.AllowGet);
             }
             [AjaxOnly]
             public JsonResult GetCheckersecList()
             {

                 var checklevel = Request.QueryString["checklevel"].ToString();
                 //ArrayList rst=new ArrayList();
                 if (checklevel == "段以上")
                 {
                     var rst="";
                     return Json(rst, JsonRequestBehavior.AllowGet);
                 }
                 else if (checklevel == "段级")
                 {
                     SectorModel sectorModel=new SectorModel();
                     var rst = sectorModel.GetSectorList();
                     return Json(rst, JsonRequestBehavior.AllowGet);
                 }
                 else if (checklevel == "车队")
                 {
                     TeamModel teamModel = new TeamModel();
                     var rst = teamModel.GetTeamList();
                     return Json(rst, JsonRequestBehavior.AllowGet);
                 }
                 else if (checklevel == "班组")
                 {
                     GroupModel groupModel = new GroupModel();
                     var rst = groupModel.GetGroupList();
                     return Json(rst, JsonRequestBehavior.AllowGet);

                 }
                 return Json("", JsonRequestBehavior.AllowGet);
             }
             public JsonResult GetCheckernameList()
             {

                 var checklevel = Request.QueryString["checklevel"].ToString();
                 var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
                 UserModel userModel = new UserModel();

                // ArrayList rst = new ArrayList();
                 if (checklevel == "段以上")
                 {

                 }
                 else if (checklevel == "段级")
                 {
                     var rst = userModel.GetSecCheckerNameList(sectorid);
                     return Json(rst, JsonRequestBehavior.AllowGet);
                 }
                 else if (checklevel == "车队")
                 {
                     var rst = userModel.GetTeamCheckerNameList(sectorid);
                     return Json(rst, JsonRequestBehavior.AllowGet);
                 }
                 else if (checklevel == "班组")
                 {
                     var rst = userModel.GetTrainCoach(sectorid);
                     return Json(rst, JsonRequestBehavior.AllowGet);

                 }
                 return Json(null, JsonRequestBehavior.AllowGet);
             }
    }
}
