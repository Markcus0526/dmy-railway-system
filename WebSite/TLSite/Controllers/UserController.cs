using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class UserController : Controller
    {
        UserModel userModel = new UserModel();
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        CrewRoleModel crewrole = new CrewRoleModel();

        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        public JsonResult CheckUniqueUserName(string username, long uid)
        {
            bool rst = userModel.CheckDuplicateName(username, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult CheckUniqueCrewNo(string crewno, long uid)
        {
            bool rst = userModel.CheckDuplicateCrewNo(crewno, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        #region AdminRole
        [Authorize(Roles="UserManage,RoleManage")]
        public ActionResult RoleList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "RoleList";

            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveRoleList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = userModel.GetRoleDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRole()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "RoleList";
            ViewData["teamlist"] = teamModel.GetTeamList();
            return View();
        }

        public ActionResult EditRole(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "RoleList";

            var roleinfo = userModel.GetRoleById(id);
            ViewData["role"] = roleinfo.role;
            ViewData["roleinfo"] = roleinfo;
            ViewData["uid"] = roleinfo.uid;
            ViewData["teamlist"] = teamModel.GetTeamList();

            return View("AddRole");
        }

        [HttpPost]
        public JsonResult DeleteRole(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = userModel.DeleteRole(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitRole(long uid, string rolename, string[] configuration,string teamid)
        {
            string rst = "";

            string role = "";

            if (configuration != null)
            {
                role = String.Join(",", configuration);
            }

            if (uid == 0)
            {
                rst = userModel.InsertRole(rolename, role, int.Parse(teamid));
            }
            else
            {
                rst = userModel.UpdateRole(uid, rolename, role, int.Parse(teamid));
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AdminManage
        [Authorize(Roles = "UserManage,AdminList")]
        public ActionResult AdminList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "AdminList";

            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveAdminList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = userModel.GetAdminDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAdmin()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "AdminList";
            ViewData["rolelist"] = userModel.GetAdminRoleList();
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        public ActionResult EditAdmin(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "AdminList";
            ViewData["rolelist"] = userModel.GetAdminRoleList();

            var userinfo = userModel.GetUserById(id);
            ViewData["userinfo"] = userinfo;
            ViewData["uid"] = userinfo.uid;
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View("AddAdmin");
        }

        [HttpPost]
        public JsonResult DeleteAdmin(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = userModel.DeleteAdmin(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitAdmin(long uid, string username, string userpwd, long userrole)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = userModel.InsertAdmin(username, userpwd, userrole);
            }
            else
            {
                rst = userModel.UpdateAdmin(uid, username, userpwd, userrole);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "UserManage")]
        public ActionResult ChangePwd()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Home";
            ViewData["level2nav"] = "Home";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize(Roles = "UserManage")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SetChangePwd(string userpwd)
        {
            string rst = "";
            long nId = 0;

            try
            {
                nId = CommonModel.GetSessionUserID();
            }
            catch (System.Exception ex)
            {
                CommonModel.WriteLogFile("Feature", "CheckUniqureKeyword()", ex.ToString());
            }
            rst = userModel.UpdatePwd(nId, userpwd);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Executive Manage
        [Authorize(Roles = "UserManage")]
        public ActionResult ExecutiveList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "ExecutiveList";

            return View();
        }

        [Authorize(Roles = "UserManage")]
        [AjaxOnly]
        public JsonResult RetrieveUserList(int userkind, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = userModel.GetGanbuDataTable(userkind, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "UserManage")]
        [AjaxOnly]
        public JsonResult RetrievePersonnelList(int userkind, long groupid, long teamid, long sectorid, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = userModel.GetGanbuDataTable(userkind,groupid,teamid,sectorid, param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "UserManage")]
        public void ExportPersonnelList(int userkind, long groupid, long teamid, long sectorid, JQueryDataTableParamModel param)
        {

            var datalist = userModel.ExportPersonnelList(userkind, groupid, teamid, sectorid);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=人员信息表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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
        //[Authorize(Roles = "UserManage")]
        //public ActionResult AddExecutive()
        //{
        //    string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

        //    ViewData["rootUri"] = rootUri;
        //    ViewData["level1nav"] = "User";
        //    ViewData["level2nav"] = "ExecutiveList";
        //    ViewData["parentlist"] = sectorModel.GetSectorList();

        //    return View();
        //}

        [Authorize(Roles = "UserManage")]
        public ActionResult AddPerson()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "PersonnelList";
            ViewData["parentlist"] = sectorModel.GetSectorList();
            ViewData["crewrole"] = crewrole.GetCrewRoleList();

            return View();
        }

        [Authorize(Roles = "UserManage")]
        public JsonResult GetExecutiveParentList(string exectype)
        {
            if (exectype == ExecType.SectorExec)
            {
                var rst = sectorModel.GetSectorList();
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
            else if (exectype == ExecType.TeamExec)
            {
                var rst = teamModel.GetTeamList();
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
            else if (exectype == ExecType.TrainCoach)
            {
                var rst = teamModel.GetTeamList();
                var grouplist = groupModel.GetGroupListOfTeam(rst.FirstOrDefault().uid);
                return Json(new { teamlist = rst,  grouplist = grouplist}, JsonRequestBehavior.AllowGet);
            }
            else if (exectype == ExecType.TrainCrew)
            {
                var uid = 0;
                if (Request.QueryString["uid"].ToString()!="undefined")
                {
                     uid = int.Parse(Request.QueryString["uid"].ToString());
                }
               // var uid = int.Parse(Request.QueryString["uid"].ToString());
                var selectinfo = userModel.GetUserTeamGrouById(uid);
                var rst = teamModel.GetTeamList();
                var grouplist = groupModel.GetGroupListOfTeam(rst.FirstOrDefault().uid);
                return Json(new { teamlist = rst, grouplist = grouplist, selectinfo = selectinfo }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "UserManage")]
        public JsonResult GetSelectedGroupList(long teamid)
        {
            var rst = groupModel.GetGroupListOfTeam(teamid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "UserManage")]
        //public ActionResult EditExecutive(long id)
        //{
        //    string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

        //    ViewData["rootUri"] = rootUri;
        //    ViewData["level1nav"] = "User";
        //    ViewData["level2nav"] = "PersonnelList";

        //    var userinfo = userModel.GetUserById(id);
        //    ViewData["userinfo"] = userinfo;
        //    ViewData["uid"] = userinfo.uid;

        //    if (userinfo.exectype == ExecType.SectorExec)
        //    {
        //        ViewData["parentlist"] = sectorModel.GetSectorList();
        //    }
        //    else if (userinfo.exectype == ExecType.TeamExec)
        //    {
        //        ViewData["parentlist"] = teamModel.GetTeamList();
        //    }
        //    else if (userinfo.exectype == ExecType.TrainCoach)
        //    {
        //        ViewData["parentlist"] = teamModel.GetTeamList();
        //        var groupinfo = groupModel.GetGroupById(userinfo.crewgroupid);
        //        ViewData["grouplist"] = groupModel.GetGroupListOfTeam(groupinfo.teamid);
        //        ViewData["groupinfo"] = groupinfo;
        //    }
            
        //    return View("AddCrew");
        //}

        [Authorize(Roles = "UserManage")]
        public ActionResult EditPerson(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "PersonnelList";

            var userinfo = userModel.GetUserDetailById(id);
            ViewData["userinfo"] = userinfo;
            ViewData["uid"] = userinfo.uid;

            if (userinfo.usertype == ExecType.SectorExec)
            {
                ViewData["parentlist"] = sectorModel.GetSectorList();
            }
            else if (userinfo.usertype == ExecType.TeamExec)
            {
                ViewData["parentlist"] = teamModel.GetTeamList();
            }
            else if (userinfo.usertype == ExecType.TrainCoach || userinfo.usertype == ExecType.TrainCrew)
            {
                ViewData["parentlist"] = teamModel.GetTeamList();
                var groupinfo = groupModel.GetGroupById(userinfo.crewgroupid);
                try
                {
                    ViewData["grouplist"] = groupModel.GetGroupListOfTeam(groupinfo.teamid);
                    ViewData["groupinfo"] = groupinfo;
                }
                catch (System.Exception ex)
                {
                    var teamid = groupModel.GetDeletedGroupteamid(userinfo.crewgroupid);
                    ViewData["grouplist"] = groupModel.GetGroupListOfTeam(teamid);
                }
            }

            ViewData["crewrole"] = crewrole.GetCrewRoleList();

            return View("AddPerson");
        }

        [Authorize(Roles = "UserManage")]
        [HttpPost]
        public JsonResult DeleteExecutive(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = userModel.DeleteExecutive(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "UserManage")]
        //[HttpPost]
        //[AjaxOnly]
        //public JsonResult SubmitExecutive(long uid, string username, string userpwd, string crewno, string policyface, string exectype, long parentid,
        //    string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage,byte teammanage)
        //{
        //    string rst = "";
        //    long groupid = 0;

        //    if (Request.Form["groupid"] != null && !String.IsNullOrEmpty(Request.Form["groupid"].ToString()))
        //    {
        //        if (exectype == ExecType.TrainCoach || exectype == ExecType.TrainCrew)
        //        {
        //            groupid = long.Parse(Request.Form["groupid"].ToString());
        //        }
        //    }

        //    if ((exectype == ExecType.TrainCoach && groupid == 0) || (exectype == ExecType.TrainCrew && groupid == 0))
        //    {
        //        rst = "请选择班组";
        //        return Json(rst, JsonRequestBehavior.AllowGet);
        //    }

        //    if (uid == 0)
        //    {
        //        rst = userModel.InsertPerson(username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage);
        //    }
        //    else
        //    {
        //        rst = userModel.UpdateExecutive(uid, username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage);
        //    }

        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}
        [Authorize(Roles = "UserManage")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitPerson(long uid, string username, string userpwd, string crewno, string policyface, string exectype, long parentid,
            string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage,byte teammanage, long crewrole)
        {
            string rst = "";
            long groupid = 0;

            if (Request.Form["groupid"] != null && !String.IsNullOrEmpty(Request.Form["groupid"].ToString()))
            {
                if (exectype == ExecType.TrainCoach || exectype == ExecType.TrainCrew)
                {
                    groupid = long.Parse(Request.Form["groupid"].ToString());
                }
            }

            if ((exectype == ExecType.TrainCoach && groupid == 0) || (exectype == ExecType.TrainCrew && groupid == 0))
            {
                rst = "请选择班组";
                return Json(rst, JsonRequestBehavior.AllowGet);
            }

            if (uid == 0)
            {
                rst = userModel.InsertPerson(username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage, crewrole);
            }
            else
            {
                rst = userModel.UpdatePerson(uid, username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage, crewrole);

                //if (exectype == ExecType.TrainCrew)
                //{
                //    rst = userModel.UpdateCrew(uid, username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage,teammanage, crewrole);
                //}
                //else
                //{
                //    rst = userModel.UpdateExecutive(uid, username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage,teammanage);
                //}
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 公文에서 인원검색에 리용함.
        /// </summary>
        /// <param name="selid">선택된 부문의 id</param>
        /// <param name="selkind">科室干部，车队干部</param>
        /// <returns></returns>
        [Authorize]
        [AjaxOnly]
        public JsonResult FindExecutiveList(long selid, string selkind)
        {
            var rst = userModel.FindExecutiveList(selid, selkind);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupCrewList(long selid)
        {
            var rst = userModel.GetGroupCrewList(selid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupCheZhangList(long selid)
        {
            var rst = userModel.GetGroupChezhangList(selid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult FindGroupUserList(long selid)
        {
            var rst = userModel.GetGroupUserList(selid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Personnel Manage
        [Authorize(Roles = "UserManage")]
        public ActionResult PersonnelList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "PersonnelList";
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["parentlist"] = sectorModel.GetSectorList();

            return View();
        }

        //[Authorize(Roles = "UserManage")]
        //public ActionResult AddPersonnel()
        //{
        //    string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

        //    ViewData["rootUri"] = rootUri;
        //    ViewData["level1nav"] = "User";
        //    ViewData["level2nav"] = "PersonnelList";
        //    ViewData["parentlist"] = sectorModel.GetSectorList();

        //    return View();
        //}

        //[Authorize(Roles = "UserManage")]
        //public ActionResult EditPersonnel(long id)
        //{
        //    string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

        //    ViewData["rootUri"] = rootUri;
        //    ViewData["level1nav"] = "User";
        //    ViewData["level2nav"] = "PersonnelList";

        //    var userinfo = userModel.GetUserById(id);
        //    ViewData["userinfo"] = userinfo;
        //    ViewData["uid"] = userinfo.uid;

        //    if (userinfo.exectype == ExecType.SectorExec)
        //    {
        //        ViewData["parentlist"] = sectorModel.GetSectorList();
        //    }
        //    else if (userinfo.exectype == ExecType.TeamExec)
        //    {
        //        ViewData["parentlist"] = teamModel.GetTeamList();
        //    }
        //    else if (userinfo.exectype == ExecType.TrainCoach)
        //    {
        //        ViewData["parentlist"] = teamModel.GetTeamList();
        //        var groupinfo = groupModel.GetGroupById(userinfo.crewgroupid);
        //        ViewData["grouplist"] = groupModel.GetGroupListOfTeam(groupinfo.teamid);
        //        ViewData["groupinfo"] = groupinfo;
        //    }

        //    return View("AddPersonnel");
        //}

        //[Authorize(Roles = "UserManage")]
        //[HttpPost]
        //public JsonResult DeletePersonnel(string delids)
        //{
        //    string[] ids = delids.Split(',');
        //    long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
        //    bool rst = userModel.DeleteExecutive(selcheckbox);

        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}

        //[Authorize(Roles = "UserManage")]
        //[HttpPost]
        //[AjaxOnly]
        //public JsonResult SubmitPersonnel(long uid, string username, string userpwd, string crewno, string policyface, string exectype, long parentid,
        //    string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage, byte teammanage)
        //{
        //    string rst = "";
        //    long groupid = 0;

        //    if (Request.Form["groupid"] != null && !String.IsNullOrEmpty(Request.Form["groupid"].ToString()))
        //    {
        //        if (exectype == ExecType.TrainCoach)
        //        {
        //            groupid = long.Parse(Request.Form["groupid"].ToString());
        //        }
        //    }

        //    if (exectype == ExecType.TrainCoach && groupid == 0)
        //    {
        //        rst = "请选择班组";
        //        return Json(rst, JsonRequestBehavior.AllowGet);
        //    }

        //    if (uid == 0)
        //    {
        //        rst = userModel.InsertPerson(username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage);
        //    }
        //    else
        //    {
        //        rst = userModel.UpdatePerson(uid, username, userpwd, crewno, policyface, exectype, parentid, groupid, execrole, realname, imgurl, birthday, gender, phonenum, opinionmanage, teammanage, crewrole);
        //    }

        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        public JsonResult SubmitUserImport(string fileurl)
        {
            string rst = "";
            if (fileurl == "" || string.IsNullOrWhiteSpace(fileurl))
            {
                rst = "请检测是否选择文件";
                return Json(rst, JsonRequestBehavior.AllowGet);

            }

            rst = userModel.ImportUserData(fileurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ImportUser()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "User";
            ViewData["level2nav"] = "PersonnelList";
            return View();
        }
    }
}