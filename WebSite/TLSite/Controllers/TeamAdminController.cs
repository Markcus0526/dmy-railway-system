using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TLSite.Controllers
{
    public class TeamAdminController : Controller
    {
        UserModel usermodel = new UserModel();
        TeamAdminModel teamadmodel = new TeamAdminModel();
        //
        // GET: /TeamAdmin/

        public JsonResult GetGroupListWithoutDailyGroup(long teamid)
        {
            GroupModel groupModel = new GroupModel();
            var rst = groupModel.GetGroupListOfTeamWithoutDailyGroup(teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }


        #region 车队人员库
        [Authorize(Roles = "TeamAdmin")]
        public ActionResult TeamCrewList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrew";
            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["currentteam"] = currentteam;

            GroupModel groupmodel = new GroupModel();
            ViewData["grouplist"] = groupmodel.GetGroupListOfTeam(currentteam.FirstOrDefault().uid);
            return View();
        }

        [Authorize(Roles = "TeamAdmin")]
        [AjaxOnly]
        public JsonResult RetrieveTeamCrewList(int userkind, long groupid, long teamid, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = teamadmodel.GetTeamCrewList(userkind, groupid, teamid);
            
            
            IEnumerable<UserDetailInfo> filteredCompanies;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                filteredCompanies = rst
                   .Where(c => isNameSearchable && (c.username.ToLower().Contains(param.sSearch.ToLower()) || c.crewno.ToLower().Contains(param.sSearch.ToLower())));
            }

            else
            {
                filteredCompanies = rst;
            }

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.usertype,
                               c.teamname,
                               c.crewgroupname,
                               c.crewrole,
                               c.username,
                               c.crewno,
                               c.realname,
                               c.imgurl,
                               c.policyface,
                               Convert.ToString(c.gender),
                               String.Format("{0:yyyy-MM-dd}", c.birthday),
                               Convert.ToString( c.uid)
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "TeamAdmin")]
        public void ExportTeamCrewList(int userkind, long groupid, long teamid, JQueryDataTableParamModel param)
        {

            var datalist = teamadmodel.ExportTeamcrewList(userkind, groupid, teamid);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=车队人员信息表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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

        [Authorize(Roles = "TeamAdmin")]
        public ActionResult AddTeamCrew()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrew";


            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["teamlist"] = currentteam;

            GroupModel groupmodel = new GroupModel();
            ViewData["grouplist"] = groupmodel.GetGroupListOfTeam(currentteam.FirstOrDefault().uid);

            CrewRoleModel crewrole = new CrewRoleModel();
            ViewData["crewrole"] = crewrole.GetCrewRoleList();

            return View();
        }

        [Authorize(Roles = "TeamAdmin")]
        public ActionResult EditTeamCrew(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrew";

            var userinfo = usermodel.GetUserDetailById(id);
            ViewData["userinfo"] = userinfo;
            ViewData["uid"] = userinfo.uid;

            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["teamlist"] = currentteam;

            GroupModel groupmodel = new GroupModel();
            ViewData["grouplist"] = groupmodel.GetGroupListOfTeamWithoutDailyGroup(currentteam.FirstOrDefault().uid);

            CrewRoleModel crewrole = new CrewRoleModel();
            ViewData["crewrole"] = crewrole.GetCrewRoleList();

            return View("AddTeamCrew");
        }

        [Authorize(Roles = "TeamAdmin")]
        public ActionResult EditTeamExec(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrew";

            var userinfo = usermodel.GetUserDetailById(id);
            ViewData["userinfo"] = userinfo;
            ViewData["uid"] = userinfo.uid;

            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["teamlist"] = currentteam;

            return View("AddTeamExec");
        }

        [Authorize(Roles = "TeamAdmin")]
        public JsonResult SubmitTeamCrew(int exectype, long uid, string username, string userpwd, string crewno, string policyface, long parentid,
            string realname, string imgurl, DateTime birthday, byte gender, string phonenum, long crewrole, string execrole)
        {
           long groupid=0;
            if ( Request.QueryString["groupid"]!=null)
            {
                groupid = long.Parse(Request.QueryString["groupid"]);
            }
            else
            {
                if (exectype==1)
                {
                    return Json("请选择班组！", JsonRequestBehavior.AllowGet);
                }
            }
          
            string rst = "";
            //long groupid = 0;
            var flags = 1;
            if (exectype==1)
            {
                flags = 1;
            }
            else
            {
                flags = 2;
            }
            if (uid == 0)
            {
                rst = teamadmodel.InsertTeamCrew(flags, username, userpwd, crewno, policyface, parentid, groupid, realname, imgurl, birthday, gender, phonenum, crewrole, execrole);
            }
            else
            {
                rst = teamadmodel.UpdateTeamCrew(flags, uid, username, userpwd, crewno, policyface, parentid, groupid, realname, imgurl, birthday, gender, phonenum, crewrole, execrole);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "TeamAdmin")]
        public JsonResult SubmitTeamExec(long uid, string username, string userpwd, string crewno, string policyface,long parentid,
            string realname, string imgurl, DateTime birthday, byte gender, string phonenum, string execrole)
        {
            string rst = "";
            //long groupid = 0;
            int flags = 2;
            if (uid == 0)
            {
                rst = teamadmodel.InsertTeamCrew(flags, username, userpwd, crewno, policyface,parentid, 0, realname, imgurl, birthday, gender, phonenum, 0, execrole);
            }
            else
            {
                rst = teamadmodel.UpdateTeamCrew(flags,uid, username, userpwd, crewno, policyface,parentid, 0, realname, imgurl, birthday, gender, phonenum, 0, execrole);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 调入调出
        [Authorize(Roles = "TeamAdmin")]
        public ActionResult TeamCrewTransfer()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrewTransfer";

            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["currentteam"] = currentteam;
            
            TeamModel teammodel = new TeamModel();
            ViewData["teamlist"] = teammodel.GetTeamList();

            GroupModel groupmodel=new GroupModel();
            ViewData["grouplist"] = groupmodel.GetGroupListOfTeam(currentteam.FirstOrDefault().uid);

             ViewData["crewlist"] = usermodel.GetUserListByTeamid(currentteam.FirstOrDefault().uid);

            return View();
        }

        [AjaxOnly]
        [Authorize(Roles = "TeamAdmin")]
        public JsonResult SubmitExchange(long currentteam, long crewlist, long teamlist)
        {
            string rst = "";

            rst = teamadmodel.AddTransfer(currentteam, crewlist, teamlist);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [Authorize(Roles = "TeamAdmin")]
        public JsonResult DeleteTransfer(long delids)
        {
            bool rst = teamadmodel.DeleteTransfer(delids);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public JsonResult GetuserListofTeam()
        {
            var id = int.Parse(Request.QueryString["teamid"].ToString());
            var rst = usermodel.GetUserListByTeamid(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult RetrieveTableIn(JQueryDataTableParamModel param)
        {
            var rst = teamadmodel.GetTransferIn();

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.oldteamname,
                               c.crewno,
                               c.realname,
                               c.oldgroupname,
                               c.rolename,
                              Convert.ToString(c.transfertime),
                               c.newteamname,
                               c.state,
                              Convert.ToString(c.uid),
                              Convert.ToString(c.crewid)
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public JsonResult RetrieveTableOut(JQueryDataTableParamModel param)
        {
            var rst = teamadmodel.GetTransferOut();

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.oldteamname,
                               c.crewno,
                               c.realname,
                               c.oldgroupname,
                               c.rolename,
                              Convert.ToString(c.transfertime),
                               c.newteamname,
                               c.state,
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

        public JsonResult GetUsabelGroupList(long id)
        {
            var currentteam = teamadmodel.GetTransferDetail(id);
            GroupModel groupmodel=new GroupModel();
            var rst = groupmodel.GetGroupListOfTeamWithoutDailyGroup(currentteam.newteamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AcceptCrew(long crewid, long transferid,long groupid)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrewTransfer";
            bool rst = teamadmodel.AcceptCrew( crewid,  transferid, groupid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 其他人员变动
        [Authorize(Roles = "TeamAdmin")]
        public ActionResult SPCrewTransfer()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "SPCrewTransfer";

            var currentteam = teamadmodel.GetCurrentTeam();
            ViewData["currentteam"] = currentteam;

            GroupModel groupmodel = new GroupModel();
            ViewData["grouplist"] = groupmodel.GetGroupListOfTeam(currentteam.FirstOrDefault().uid);

            ViewData["crewlist"] = usermodel.GetUserListByTeamid(currentteam.FirstOrDefault().uid);

            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveSPtransferTable(JQueryDataTableParamModel param)
        {
            var rst = teamadmodel.GetSPTransferTable();

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.oldteamname,
                               c.crewno,
                               c.realname,
                              Convert.ToString(c.transfertime),
                               c.contents,
                               c.state,
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

        [AjaxOnly]
        [Authorize(Roles = "TeamAdmin")]
        public JsonResult GetUserInfo(long id)
        {

            var rst = usermodel.GetUserDetailById(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [Authorize(Roles = "TeamAdmin")]
        public JsonResult AddSpTransfer(long crewid, long oldteamid, string contents)
        {

            var rst = teamadmodel.AddSpTransfer(crewid, oldteamid, contents);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [Authorize(Roles = "TeamAdmin")]
        public JsonResult DeleteSPTransfer(long delids)
        {
            bool rst = teamadmodel.DeleteSPTransfer(delids);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 管理其他变动
        [Authorize(Roles = "UserManage")]
        public ActionResult ManageSPTransfer()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "ManageSPCrewTransfer";

            TeamModel teammodel = new TeamModel();
            ViewData["teamlist"] = teammodel.GetTeamList();

            //年月日显示
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy年MM月");
            ViewData["CurrentComputerDate"] = DateTime.Now.ToString("yyyy/MM/dd");
            return View();
        }
        [AjaxOnly]
        public JsonResult RetrieveManageSPtransferTable(JQueryDataTableParamModel param)
        {

            var teamid = int.Parse(Request.QueryString["teamid"]);
            var currenttime = DateTime.Parse(Request.QueryString["date"]);

            var rst = teamadmodel.GetManagebleSpTable(currenttime, teamid);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.oldteamname,
                               c.crewno,
                               c.realname,
                              Convert.ToString(c.transfertime),
                               c.contents,
                               c.state,
                              Convert.ToString(c.uid),
                              Convert.ToString(c.crewid)
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExcuteSpTransfer(long crewid, long transferid, string transfertype)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "TeamCrewTransfer";

            var rst = teamadmodel.ExcuteSpTransfer(crewid, transferid, long.Parse(transfertype));
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    #endregion
    }
}
