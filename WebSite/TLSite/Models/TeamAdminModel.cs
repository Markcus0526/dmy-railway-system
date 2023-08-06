using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace TLSite.Models
{
    public class transferinfo
    {
        public long uid { set; get; }
        public long crewid { set; get; }
        public string realname { set; get; }
        public string crewno{ set; get; }
        public string rolename { set; get; }
        public string contents { set; get; }

        public long   oldteamid { set; get; }
        public string oldteamname { set; get; }
        public long   newteamid { set; get; }
        public string newteamname { set; get; }
        public long   oldgroupid { set; get; }
        public string oldgroupname { set; get; }
        public long   newgroupid { set; get; }
        public string newgroupname { set; get; }

        public DateTime transfertime { set; get; }
        public string state { set; get; }

    }
    public class TeamAdminModel
    {
        TieluDBDataContext db = new TieluDBDataContext();
        UserModel usermodel = new UserModel();
        public tbl_user GetCurrentUser()
        {
            var currentid=CommonModel.GetSessionUserID();
            var currentuser=db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            return currentuser;
        }
        #region 车队人员库
        public List<UserDetailInfo> GetTeamCrewList(int usertype, long groupid, long teamid)
        {
           // List<UserDetailInfo> rstlist = new List<UserDetailInfo>();
            var ganbulist = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.TeamExec)
                // .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
               //  .Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                 .Join(db.tbl_railteams, m => m.execparentid, l => l.uid, (m, l) => new { user = m, team = l })
              //  .OrderBy(m => m.user.createtime)
                .Select(m => new UserDetailInfo
                {
                    uid = m.user.uid,
                    userkind = m.user.userkind,
                    username = m.user.username,
                    realname = m.user.realname,
                    usertype = ExecType.TeamExec,
                    crewrole = m.user.execrole,
                    crewno = m.user.crewno,
                    imgurl = m.user.imgurl,
                    policyface = m.user.policyface,
                    gender = m.user.gender,
                    birthday = m.user.birthday,
                    groupid = m.user.crewgroupid,
                   // crewgroupname = m.user.group.groupname,
                    teamid = m.user.execparentid,
                    teamname = m.team.teamname,
                })
                .ToList();


            var crewlist = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW)
                 .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                 .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                 .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { user = m, team = l })
                .OrderBy(m => m.user.user.user.createtime)
                .Select(m => new UserDetailInfo
                {
                    uid = m.user.user.user.uid,
                    userkind = m.user.user.user.userkind,
                    username = m.user.user.user.username,
                    realname = m.user.user.user.realname,
                    usertype = m.user.user.user.crewrole==2?"列车长":"列车员",
                    crewrole= m.user.user.role.rolename,
                    crewno = m.user.user.user.crewno,
                    imgurl = m.user.user.user.imgurl,
                    policyface = m.user.user.user.policyface,
                    gender = m.user.user.user.gender,
                    birthday =m.user.user.user.birthday,
                    groupid = m.user.user.user.crewgroupid,
                    crewgroupname = m.user.group.groupname,
                    teamid = m.user.group.teamid,
                    teamname=m.team.teamname,
                })
                .ToList();
            var alllist = crewlist.Concat(ganbulist).ToList();


            if (usertype != 0)
            {
                if (usertype == 2)
                {
                    alllist = alllist.Where(m => m.usertype == ExecType.TeamExec).ToList();
                }
                if (usertype == 3)//列车长
                {
                    alllist = alllist.Where(m => m.usertype == "列车长").ToList();
                }
                else if (usertype == 4)//列车员
                {
                    alllist = alllist.Where(m => m.usertype == "列车员").ToList();
                }
            }
            if (teamid != 0)
            {
                alllist = alllist.Where(m => m.teamid == teamid).ToList();
            }
            if (groupid != 0)
            {
                alllist = alllist.Where(m => m.groupid == groupid).ToList();
            }
            return alllist;
        }

        public string InsertTeamCrew(int flags,string username, string pwd, string crewno, string policyface,long parentid, long groupid,
           string realname, string imgurl, DateTime birthday, byte gender, string phonenum, long crewrole, string execrole)
        {
            try
            {
                tbl_user newuser = new tbl_user();
                long accountid = CommonModel.GetCurrAccountId();

                string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
                string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
                string targetbase = HostingEnvironment.MapPath("~/" + savepath);

                //列车员操作
                if (flags==1)
                {
                    newuser.userkind = 2;
                    if (crewrole == 2)
                    {
                        newuser.exectype = "列车长";
                        newuser.crewrole = 2;
                    }
                    else
                    {
                        newuser.crewrole = crewrole;
                        newuser.exectype = "列车员";
                    }
                    newuser.crewgroupid = groupid;
                    newuser.execparentid = 0;

                }
                else if (flags==2)
                {
                    newuser.userkind = 1;
                    newuser.exectype = "车队干部";
                    newuser.crewrole = 0;
                    newuser.execrole = execrole;
                    newuser.crewgroupid = 0;
                    newuser.execparentid = parentid;

                }


                
                newuser.opinionmanage = 0;
                newuser.teammanage = 0;
                                
                newuser.username = username;

                newuser.password = UserModel.GetMD5Hash(pwd);
                newuser.crewno = crewno;
                newuser.policyface = policyface;
                newuser.realname = realname;

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                }

                newuser.imgurl = savepath + imgurl;


                newuser.birthday = birthday;
                newuser.gender = gender;
                newuser.phonenum = phonenum;
                newuser.createtime = DateTime.Now;

                db.tbl_users.InsertOnSubmit(newuser);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        public string UpdateTeamCrew(int flags,long uid, string username, string pwd, string crewno, string policyface, long parentid, long groupid,
           string realname, string imgurl, DateTime birthday, byte gender, string phonenum, long crewrole, string execrole)
        {
            string rst = "";
            tbl_user edititem = db.tbl_users.Where(m => m.uid == uid).FirstOrDefault();
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            if (edititem != null)
            {
                if (!String.IsNullOrEmpty(pwd))
                {
                    edititem.password = UserModel.GetMD5Hash(pwd);
                }

                if (flags==1)
                {
                    edititem.userkind = 2;
                    if (crewrole == 2)
                    {
                        edititem.exectype = "列车长";
                        edititem.crewrole = 2;

                    }
                    else
                    {
                        edititem.exectype = "列车员";
                        edititem.crewrole = crewrole;
                    }
                }
                else if (flags==2)
                {
                    edititem.userkind = 1;
                    edititem.exectype = "车队干部";
                    edititem.crewrole = 0;
                    edititem.execrole = execrole;
                }

                edititem.opinionmanage = 0;
                edititem.teammanage = 0;

                edititem.execparentid = parentid;
                edititem.crewgroupid = groupid;
                
                edititem.username = username;
                edititem.crewno = crewno;
                edititem.policyface = policyface;
                edititem.realname = realname;

                if (imgurl != edititem.imgurl)
                {

                    if (File.Exists(orgbase + imgurl))
                    {
                        if (!Directory.Exists(targetbase))
                        {
                            Directory.CreateDirectory(targetbase);
                        }
                        File.Move(orgbase + imgurl, targetbase + imgurl);
                    }

                    edititem.imgurl = savepath + imgurl;
                }

                edititem.birthday = birthday;
                edititem.gender = gender;
                edititem.phonenum = phonenum;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "用户不存在";
            }

            return rst;
        }

        //导出Excel
        public System.Data.DataTable ExportTeamcrewList(int userkind, long groupid, long teamid)
        {
            var products = new System.Data.DataTable("车队人员信息表");


            List<UserDetailInfo> alllist = GetTeamCrewList(userkind, groupid, teamid);

            if (products != null)
            {
                products.Columns.Add("所属部门", typeof(string));
                products.Columns.Add("班组名称", typeof(string));
                products.Columns.Add("职务", typeof(string));
                products.Columns.Add("登录名", typeof(string));
                products.Columns.Add("工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("政治面貌", typeof(string));
                products.Columns.Add("性别", typeof(string));
                products.Columns.Add("出生日期", typeof(string));

                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(
                        item.teamname,
                        item.crewgroupname,
                        (item.userkind == (int)UserKind.EXECUTIVE) ? item.execrole : item.crewrole,
                        item.username,
                        item.crewno,
                        item.realname,
                        item.policyface,
                        item.gender == 0 ? "男" : "女",
                        String.Format("{0:yyyy/MM/dd}", item.birthday)
                        );
                    i++;
                }
            }
            return products;
        }
        #endregion


        public List<tbl_railteam> GetCurrentTeam()
        {
           List<tbl_railteam>currentteam = null;
            var currentid = CommonModel.GetSessionUserID();
            var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            if (currentuser.userkind==0)
            {
                var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole).FirstOrDefault().teamid;
                if (currentteamid!=0)
                {
                    currentteam = db.tbl_railteams.Where(m => m.uid == currentteamid).ToList();
                }
                else
                {
                    TeamModel teammodel = new TeamModel();
                    currentteam = teammodel.GetTeamList();
                }
            }
            return currentteam;
        }

        public List<transferinfo> GetTransferIn()
        {
            var rst = new List<transferinfo>();
            var currentid = CommonModel.GetSessionUserID();
            var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            if (currentuser.userkind == 0)
            {
                var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole && m.deleted == 0).FirstOrDefault().teamid;
                rst = db.tbl_crewtransfers.Where(m => m.deleted == 0 && (m.newgroupid == 0 || m.createtime.Month == DateTime.Now.Month))
                    .OrderByDescending(m => m.createtime)
                    .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { transfer = m, user = l })
                    .Join(db.tbl_railteams, m => m.transfer.newteamid, l => l.uid, (m, l) => new { transfer = m, newteam = l })
                    .Join(db.tbl_railteams, m => m.transfer.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    .Join(db.tbl_traingroups, m => m.transfer.transfer.user.crewgroupid, l => l.uid, (m, l) => new { transfer = m, group = l })
                    .Join(db.tbl_crewroles, m => m.transfer.transfer.transfer.user.crewrole, l => l.uid, (m, l) => new { transfer = m, role = l })
                    .Select(m => new transferinfo
                    {
                        uid = m.transfer.transfer.transfer.transfer.transfer.uid,
                        crewid = m.transfer.transfer.transfer.transfer.transfer.crewid,
                        realname = m.transfer.transfer.transfer.transfer.user.realname,
                        crewno = m.transfer.transfer.transfer.transfer.user.crewno,
                        rolename = m.role.rolename,

                        oldteamid = m.transfer.transfer.oldteam.uid,
                        oldteamname = m.transfer.transfer.oldteam.teamname,
                        newteamid = m.transfer.transfer.transfer.newteam.uid,
                        newteamname = m.transfer.transfer.transfer.newteam.teamname,

                        oldgroupid = m.transfer.group.uid,
                        oldgroupname = m.transfer.group.groupname,

                        transfertime = m.transfer.transfer.transfer.transfer.transfer.transfertime,
                        state = m.transfer.transfer.transfer.transfer.transfer.newgroupid == 0 ? "未分配" : "已接受"
                    })
                .ToList();
                    if (currentteamid != 0)
                    {
                        rst = rst.Where(m => m.newteamid == currentteamid).ToList();
                    }
            }
            return rst;
        }
        public List<transferinfo> GetTransferOut()
        {
            var rst = new List<transferinfo>();
            var currentid = CommonModel.GetSessionUserID();
            var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            if (currentuser.userkind == 0)
            {
                var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole && m.deleted == 0).FirstOrDefault().teamid;
                rst = db.tbl_crewtransfers.Where(m => m.deleted == 0&&(m.newgroupid==0||m.createtime.Month==DateTime.Now.Month))
                    .OrderByDescending(m => m.createtime)
                    .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { transfer = m, user = l })
                    .Join(db.tbl_railteams, m => m.transfer.newteamid, l => l.uid, (m, l) => new { transfer = m, newteam = l })
                    .Join(db.tbl_railteams, m => m.transfer.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    .Join(db.tbl_traingroups, m => m.transfer.transfer.user.crewgroupid, l => l.uid, (m, l) => new { transfer = m, group = l })
                    .Join(db.tbl_crewroles, m => m.transfer.transfer.transfer.user.crewrole, l => l.uid, (m, l) => new { transfer = m, role = l })
                    .Select(m => new transferinfo
                    {
                        uid=m.transfer.transfer.transfer.transfer.transfer.uid,
                        crewid = m.transfer.transfer.transfer.transfer.transfer.crewid,
                        realname = m.transfer.transfer.transfer.transfer.user.realname,
                        crewno = m.transfer.transfer.transfer.transfer.user.crewno,
                        rolename = m.role.rolename,

                        oldteamid = m.transfer.transfer.oldteam.uid,
                        oldteamname = m.transfer.transfer.oldteam.teamname,
                        newteamid = m.transfer.transfer.transfer.newteam.uid,
                        newteamname = m.transfer.transfer.transfer.newteam.teamname,

                        oldgroupid = m.transfer.group.uid,
                        oldgroupname = m.transfer.group.groupname,

                        transfertime = m.transfer.transfer.transfer.transfer.transfer.transfertime,
                        state = m.transfer.transfer.transfer.transfer.transfer.newgroupid == 0 ? "未分配" : "已分配"
                    })
                .ToList();
                if (currentteamid != 0)
                {
                    rst = rst.Where(m => m.oldteamid == currentteamid).ToList();
                }
            }
            return rst;
        }

        //添加调动
        public string AddTransfer(long oldteamid, long crewid, long newteamid)
        {
            var rst = "";
            var currenttime = DateTime.Now;
            try
            {
                tbl_crewtransfer newitem=new tbl_crewtransfer();
                
                newitem.oldteamid = oldteamid;
                newitem.crewid = crewid;

                newitem.newteamid = newteamid;
                newitem.transfertime = currenttime;
                newitem.createtime= currenttime;

                db.tbl_crewtransfers.InsertOnSubmit(newitem);
                db.SubmitChanges();
            }
            catch(SystemException ex)
            {
                rst = "添加失败,错误原因" + ex;
            }
            return rst;
        }
        //删除调动
        public bool DeleteTransfer(long delid)
        {
            string delSql = "UPDATE tbl_crewtransfer SET deleted=1 WHERE uid=" + delid;

            db.ExecuteCommand(delSql);

            return true;
        }

        public tbl_crewtransfer GetTransferDetail(long uid)
        {
            return db.tbl_crewtransfers.Where(m=>m.uid==uid).FirstOrDefault();
        }
        
        public bool AcceptCrew(long uid,long transferid,long groupid)
        {
            bool rst = false;
            try
            {
                tbl_user edituser = db.tbl_users.Where(m => m.uid == uid).FirstOrDefault();
                edituser.crewgroupid = groupid;
                db.SubmitChanges();
                try
                {
                    tbl_crewtransfer edittransfer = db.tbl_crewtransfers.Where(m => m.uid == transferid).FirstOrDefault();
                    edittransfer.newgroupid = groupid;
                    db.SubmitChanges();
                    rst = true;
                }
                catch (System.Exception ex)
                {
                	
                }
            }
            catch (System.Exception ex)
            {
                    	
            }

            return rst;
        }

#region 其他人员变动

        public string AddSpTransfer(long crewid, long oldteamid, String contents)
        {
            var rst = "";
            var currenttime = DateTime.Now;
            try
            {
                tbl_sptransfer newitem = new tbl_sptransfer();

                newitem.oldteamid = oldteamid;
                newitem.crewid = crewid;
                newitem.contents = contents;

                newitem.transfertime = currenttime;
                newitem.createtime = currenttime;

                db.tbl_sptransfers.InsertOnSubmit(newitem);
                db.SubmitChanges();
            }
            catch (SystemException ex)
            {
                rst = "添加失败,错误原因" + ex;
            }
            return rst;
        }
        public List<transferinfo> GetSPTransferTable()
        {
            var rst = new List<transferinfo>();
            var currentid = CommonModel.GetSessionUserID();
            var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            if (currentuser.userkind == 0)
            {
                var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole && m.deleted == 0).FirstOrDefault().teamid;
                rst = db.tbl_sptransfers.Where(m => m.deleted == 0 && (m.state == 0 || m.createtime.Month == DateTime.Now.Month))
                    .OrderByDescending(m => m.createtime)
                    .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { transfer = m, user = l })
                    .Join(db.tbl_railteams, m => m.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    //.Join(db.tbl_railteams, m => m.transfer.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    .Join(db.tbl_traingroups, m => m.transfer.user.crewgroupid, l => l.uid, (m, l) => new { transfer = m, group = l })
                    .Join(db.tbl_crewroles, m => m.transfer.transfer.user.crewrole, l => l.uid, (m, l) => new { transfer = m, role = l })
                    .Select(m => new transferinfo
                    {
                        uid = m.transfer.transfer.transfer.transfer.uid,
                        crewid = m.transfer.transfer.transfer.transfer.crewid,
                        realname = m.transfer.transfer.transfer.user.realname,
                        crewno = m.transfer.transfer.transfer.user.crewno,
                        rolename = m.role.rolename,


                        oldteamid = m.transfer.transfer.oldteam.uid,
                        oldteamname = m.transfer.transfer.oldteam.teamname,

                        oldgroupid = m.transfer.group.uid,
                        oldgroupname = m.transfer.group.groupname,
                        contents = m.transfer.transfer.transfer.transfer.contents,
                        transfertime = m.transfer.transfer.transfer.transfer.transfertime,
                        state = m.transfer.transfer.transfer.transfer.state == 0 ? "未处理" : "已处理"
                    })
                .ToList();
                if (currentteamid != 0)
                {
                    rst = rst.Where(m => m.oldteamid == currentteamid).ToList();
                }
            }
            return rst;
        }
        //删除sp调动
        public bool DeleteSPTransfer(long delid)
        {
            string delSql = "UPDATE tbl_sptransfer SET deleted=1 WHERE uid=" + delid;

            db.ExecuteCommand(delSql);

            return true;
        }
        //超级管理员显示的table
        public List<transferinfo> GetManagebleSpTable(DateTime currenttime,long teamid)
        {
            var rst = new List<transferinfo>();
           // var currentid = CommonModel.GetSessionUserID();
            //var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            
              //  var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole && m.deleted == 0).FirstOrDefault().teamid;
            rst = db.tbl_sptransfers.Where(m => m.deleted == 0 && m.transfertime.Month == currenttime.Month)
                    .OrderByDescending(m=>m.createtime)
                    .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { transfer = m, user = l })
                    .Join(db.tbl_railteams, m => m.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    //.Join(db.tbl_railteams, m => m.transfer.transfer.oldteamid, l => l.uid, (m, l) => new { transfer = m, oldteam = l })
                    .Join(db.tbl_traingroups, m => m.transfer.user.crewgroupid, l => l.uid, (m, l) => new { transfer = m, group = l })
                    .Join(db.tbl_crewroles, m => m.transfer.transfer.user.crewrole, l => l.uid, (m, l) => new { transfer = m, role = l })
                    
                    .Select(m => new transferinfo
                    {
                        uid = m.transfer.transfer.transfer.transfer.uid,
                        crewid = m.transfer.transfer.transfer.transfer.crewid,
                        realname = m.transfer.transfer.transfer.user.realname,
                        crewno = m.transfer.transfer.transfer.user.crewno,
                        rolename = m.role.rolename,


                        oldteamid = m.transfer.transfer.oldteam.uid,
                        oldteamname = m.transfer.transfer.oldteam.teamname,

                        oldgroupid = m.transfer.group.uid,
                        oldgroupname = m.transfer.group.groupname,
                        contents = m.transfer.transfer.transfer.transfer.contents,
                        transfertime = m.transfer.transfer.transfer.transfer.transfertime,
                        state = m.transfer.transfer.transfer.transfer.state == 0 ? "未处理" : "已处理"
                    })
                .ToList();
            if (teamid!=0)
            {
                rst = rst.Where(m => m.oldteamid == teamid).ToList();
            }
            return rst;
        }
#endregion
        //管理其他人员变动
        public string ExcuteSpTransfer(long crewid, long transferid, long transfertype)
        {
            var rst = "";
            //删除人员
            if (transfertype==3)
            {
                try
                {
                    tbl_user edituser = db.tbl_users.Where(m => m.uid == crewid).FirstOrDefault();
                    edituser.deleted = 0;

                    tbl_sptransfer editsp = db.tbl_sptransfers.Where(m => m.uid == transferid).FirstOrDefault();
                    editsp.state = 1;
                    db.SubmitChanges();
                }
                catch (System.Exception ex)
                {
                    rst = "处理请求失败，请刷新后尝试。";
                    return rst;
                }
            }
            if (transfertype == 1)
            {
                try
                {
                    var dailygroupid = GetDailyGroup().uid;
                        try
                        {
                            tbl_user edituser = db.tbl_users.Where(m=>m.uid==crewid).FirstOrDefault();
                            edituser.crewgroupid = dailygroupid;

                            tbl_sptransfer editsp = db.tbl_sptransfers.Where(m=>m.uid==transferid).FirstOrDefault();
                            editsp.state = 1;

                            db.SubmitChanges();
                    
                        }
                        catch (System.Exception ex)
                        {
                            rst = "处理请求失败，请刷新后尝试。";
                            return rst;
                        }
                        
                }
                catch (System.Exception ex)
                {
                    rst = "未找到日勤组，请先创建！";
                    return rst;
                }
            }
            if (transfertype == 2)
            {   
                try
                {
                    var edituser = usermodel.GetUserDetailById(crewid);

                    tbl_crewtransfer crewts = new tbl_crewtransfer();
                    crewts.crewid = crewid;
                    crewts.oldteamid = edituser.teamid;
                    crewts.newteamid = edituser.teamid;
                    crewts.transfertime = DateTime.Now;
                    crewts.createtime = DateTime.Now;
                    db.tbl_crewtransfers.InsertOnSubmit(crewts);

                    tbl_sptransfer editsp = db.tbl_sptransfers.Where(m => m.uid == transferid).FirstOrDefault();
                    editsp.state = 1;
                    db.SubmitChanges();
                }
                catch (System.Exception ex)
                {
                    rst = "处理请求失败，请刷新后尝试。";
                    return rst;
                }
            }
            return rst;
        }

        //获得日勤班组
        public tbl_traingroup GetDailyGroup()
        {
            return db.tbl_traingroups.Where(m => m.dailygroup == 1&&m.deleted==0).FirstOrDefault();
        }
    }
}