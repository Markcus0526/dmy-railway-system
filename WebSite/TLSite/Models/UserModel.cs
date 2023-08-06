using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Hosting;
using System.IO;
using System.Data.OleDb;
using System.Data;


namespace TLSite.Models
{
    public enum UserKind
    {
        ADMIN = 0,
        EXECUTIVE,
        CREW
    }

    public enum execparent
    {
        班子成员=2,
        乘务和统计科,
        安路科,
        段各科室
    }

    public class ExecType
    {
        public const string SectorExec = "科室干部";
        public const string TeamExec = "车队干部";
        public const string TrainCoach = "列车长";
        public const string TrainCrew = "列车员";//不含列车长

    }

    public class UserInfo
    {
        public long uid { get; set; }
        public UserKind userkind { get; set; }
        public string username { get; set; }
        public string realname { get; set; }
        public string imgurl { get; set; }
        public string role { get; set; }
        public int teammanger { get; set; }
        public DateTime createtime { get; set; }
        public string exectype { get; set; }
        public string crewno { get; set; }
        
    }

    public class CrewInfo
    {
        public long uid { get; set; }
        public string username { get; set; }
        public string realname { get; set; }
        public string imgurl { get; set; }
        public DateTime? birthday { get; set; }
        public string birthdaystr { get; set; }
        public byte gender { get; set; }
        public string phonenum { get; set; }
        public long roleid { get; set; }
        public string rolename { get; set; }
        public string crewno { get; set; }
        public string policyface { get; set; }
        public long groupid { get; set; }
        public string groupname { get; set; }
        public long teamid { get; set; }

    }

    public class AdminInfo
    {
        public long uid { get; set; }
        public string username { get; set; }
        public string rolename { get; set; }
        public string role { get; set; }
        public DateTime createtime { get; set; }
    }

    public class ExecutiveInfo
    {
        public long uid { get; set; }
        public string username { get; set; }
        public string realname { get; set; }
        public DateTime? birthday { get; set; }
        public string birthdaystr { get; set; }
        public byte gender { get; set; }
        public string phonenum { get; set; }
        public string execrole { get; set; }
        public string exectype { get; set; }
        public long parentid { get; set; }
        public string parentname { get; set; }
        public string imgurl { get; set; }
    }

    public class UserDetailInfo
    {
        public long uid { get; set; }
        public int userkind { get; set; }
        public string usertype { get; set; }
        public string userkindstr { get; set; }
        public string username { get; set; }
        public string realname { get; set; }
        public DateTime? birthday { get; set; }
        public string birthdaystr { get; set; }
        public byte gender { get; set; }
        public string phonenum { get; set; }
        public string execrole { get; set; }
        public string exectype { get; set; }
        public long parentid { get; set; }
        public long teamid { get; set; }
        public string parentname { get; set; }
        public long groupid{ get; set; }
        public string policyface { get; set; }
        public int opinionmanage { get; set; }
        public int teammanage { get; set; }
        public long execparentid{ get; set; }

        //Crew Info
        public long crewroleid { get; set; }
        public string crewrole { get; set; }
        public long crewgroupid { get; set; }
        public string crewgroupname { get; set; }
        public string crewno { get; set; }
        public string imgurl { get; set; }
        public string teamname { get; set; }
        public double addcredit { get; set; }
        public double curentCredit { get; set; }
        public double curentmonthCredit { get; set; }



        //积分封装
        public List<ChecklogModel> abovezone { get; set; }
        public List<ChecklogModel> zone { get; set; }
        public List<ChecklogModel> team { get; set; }
        public List<ChecklogModel> groupa { get; set; }


        public int typeone { get; set; }
        public double typetwo { get; set; }
        public double typethree { get; set; }
        public int typefour { get; set; }
        public int typefive { get; set; }

    

    }

    //积分用
    public class PersenCrew
    {
        public string username { get; set; }
        public long uid { get; set; }
        //姓名
        public string realname { get; set; }
        //职位名
        public string execrole { get; set; }
        //工资号
        public string crewno { get; set; }
        //班组id
        public long crewgroupid { get; set; }

    }

    #region LogOnModel
    public class LogOnModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [DisplayName("用户名:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("密码:")]
        public string Password { get; set; }

        [DisplayName("下次自动登录")]
        public bool RememberMe { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "{0}至少为{1}位.";
        private readonly int _minCharacters = 6;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion


    public class UserModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();

        public tbl_user GetUserById(long uid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public UserDetailInfo GetUserDetailById(long uid)
        {
            var returnuser = db.tbl_users
                .Where(m => m.deleted == 0 && m.uid == uid)
                .Select(m => new UserDetailInfo
                {
                    uid=m.uid,
                    userkind=m.userkind,
                    username=m.username,
                    realname=m.realname,
                    birthday=m.birthday,
                    gender=m.gender,
                    imgurl=m.imgurl,
                    phonenum=m.phonenum,
                    crewroleid=m.crewrole,
                    crewno=m.crewno,
                    crewgroupid=m.crewgroupid,
                    exectype = m.exectype,
                    execrole=m.execrole,
                    execparentid=m.execparentid,
                    opinionmanage = m.opinionmanage,
                    teammanage=m.teammanage,
                    policyface=m.policyface
                }).FirstOrDefault();

            if (returnuser.userkind == (int)UserKind.EXECUTIVE)
            {
                returnuser.usertype = returnuser.exectype;
            }
            else if (returnuser.userkind == (int)UserKind.ADMIN)
            {
                returnuser.usertype = "管理员";
            }
            else
            {
                var rolelist = GetCrewRoleList();
                returnuser.crewrole = rolelist.Where(m => m.uid == returnuser.crewroleid).Select(m => m.rolename).FirstOrDefault();
                returnuser.usertype = returnuser.crewrole == "列车长" ? "列车长" : "列车员";
                returnuser.teamid = db.tbl_traingroups.Where(m => m.deleted == 0 && m.uid == returnuser.crewgroupid).Select(m => m.teamid).FirstOrDefault();
            }
            return returnuser;
        }

        public CrewInfo GetUserTeamGrouById(long uid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.uid == uid)
                .Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    uid = m.user.uid,
                    teamid = m.group.teamid,
                    groupid = m.group.uid
                })
                .FirstOrDefault();
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #region User Login
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public UserInfo ValidateUser(string username, string password)
        {
            string sha1Pswd = UserModel.GetMD5Hash(password);
            return GetUserByNamePwd(username, sha1Pswd);
        }

        public UserInfo GetUserByNamePwd(string uname, string upwd)
        {
            UserInfo rst = null;
            try
            {
                var userinfo = db.tbl_users.Where(m => m.deleted == 0 && m.username == uname && m.password == upwd).FirstOrDefault();

                rst = new UserInfo();
                rst.uid = userinfo.uid;
                rst.userkind = (UserKind)userinfo.userkind;
                rst.exectype = userinfo.exectype;
                rst.username = userinfo.username;
                rst.realname = userinfo.realname;
                rst.imgurl = userinfo.imgurl;
                rst.teammanger = userinfo.teammanage;
                

                if (rst.userkind == UserKind.ADMIN)
                {
                    var roleinfo = db.tbl_adminroles.Where(m => m.deleted == 0 && m.uid == userinfo.adminrole).FirstOrDefault();

                    if (roleinfo != null)
                    {
                        rst.role = roleinfo.role;
                    }
                }
                else if (rst.userkind == UserKind.EXECUTIVE)
                {
                    rst.role = "Executive";
                    if (rst.exectype==ExecType.TeamExec)
                    {
                        rst.role += ",TeamExec";
                    }
                    if (rst.teammanger == 1)
                    {
                        rst.role += ",TeamManager";
                    }
                }
                else if (rst.userkind == UserKind.CREW)
                {
                    rst.role = "Crew";
                    if (userinfo.crewrole==2)
                    {
                        rst.role += ",Coach";
                    }
                    
                }
            }
            catch (System.Exception ex)
            {
                return null;	
            }

            return rst;
        }


        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        #endregion

        #region User Transaction
        public List<tbl_crewrole> GetCrewRoleList()
        {
            return db.tbl_crewroles
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public bool CheckDuplicateName(string username, long uid)
        {
            bool rst = true;

            rst = ((from m in db.tbl_users
                    where m.deleted == 0 && m.username == username && m.uid != uid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public bool CheckDuplicateCrewNo(string crewno, long uid)
        {
            bool rst = true;

            rst = ((from m in db.tbl_users
                    where m.deleted == 0 && m.crewno == crewno && m.uid != uid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public CrewInfo GetGroupCrewInfo(long id, long groupid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.crewgroupid == groupid && m.userkind == (int)UserKind.CREW && m.uid == id)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    uid = m.user.user.uid,
                    username = m.user.user.username,
                    realname = m.user.user.realname,
                    imgurl = m.user.user.imgurl,
                    birthday = m.user.user.birthday,
                    birthdaystr = String.Format("{0:yyyy-MM-dd}", m.user.user.birthday),
                    gender = m.user.user.gender,
                    phonenum = m.user.user.phonenum,
                    roleid = m.user.role.uid,
                    rolename = m.user.role.rolename,
                    crewno = m.user.user.crewno,
                    policyface = m.user.user.policyface,
                    groupid = m.group.uid,
                    groupname = m.group.groupname
                })
                .FirstOrDefault();
        }

        public List<CrewInfo> GetGroupCrewList(long groupid)
        {
            var rst= db.tbl_users
                .Where(m => m.deleted == 0  && m.userkind == (int)UserKind.CREW)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo { 
                    uid = m.user.user.uid,
                   // groupid=m.user.user.crewgroupid
                    username = m.user.user.username,
                    realname = m.user.user.realname,
                    imgurl = m.user.user.imgurl,
                    birthday = m.user.user.birthday,
                    gender = m.user.user.gender,
                    phonenum = m.user.user.phonenum,
                    roleid = m.user.role.uid,
                    rolename = m.user.role.rolename,
                    crewno = m.user.user.crewno,
                    policyface = m.user.user.policyface,
                    groupid = m.group.uid,
                    groupname = m.group.groupname
                }).ToList();
                if (groupid!=0)
                {
                    rst=rst.Where(m=>m.groupid==groupid).ToList();
                }
               
            return rst;
        }

        public List<CrewInfo> GetGroupChezhangList(long groupid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.crewgroupid == groupid && m.userkind == (int)UserKind.CREW && m.crewrole == 2)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    uid = m.user.user.uid,
                    username = m.user.user.username,
                    realname = m.user.user.realname,
                    imgurl = m.user.user.imgurl,
                    birthday = m.user.user.birthday,
                    gender = m.user.user.gender,
                    phonenum = m.user.user.phonenum,
                    roleid = m.user.role.uid,
                    rolename = m.user.role.rolename,
                    crewno = m.user.user.crewno,
                    groupid = m.group.uid,
                    groupname = m.group.groupname
                })
                .ToList();
        }


        public List<CrewInfo> GetGroupUserList(long groupid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.crewgroupid == groupid && m.userkind == (int)UserKind.CREW)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    uid = m.user.user.uid,
                    username = m.user.user.username,
                    realname = m.user.user.realname,
                    imgurl = m.user.user.imgurl,
                    birthday = m.user.user.birthday,
                    gender = m.user.user.gender,
                    phonenum = m.user.user.phonenum,
                    roleid = m.user.role.uid,
                    rolename = m.user.role.rolename,
                    crewno = m.user.user.crewno,
                    groupid = m.group.uid,
                    groupname = m.group.groupname
                })
                .ToList();
        }
        public List<CrewInfo> GetCrewListByGroupidandTeamid(long groupid,long teamid)
        {
            var rst= db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { user = m, team= l })
                
                .Select(m => new CrewInfo
                {
                    uid = m.user.user.user.uid,
                    username = m.user.user.user.username,
                    realname = m.user.user.user.realname,
                    imgurl = m.user.user.user.imgurl,
                    birthday = m.user.user.user.birthday,
                    gender = m.user.user.user.gender,
                    phonenum = m.user.user.user.phonenum,
                    roleid = m.user.user.role.uid,
                    rolename = m.user.user.role.rolename,
                    crewno = m.user.user.user.crewno,
                    groupid = m.user.group.uid,
                    groupname = m.user.group.groupname,
                    teamid= m.team.uid
                })
                .ToList();
            if (groupid!=0)
            {
                rst = rst.Where(m => m.groupid == groupid).ToList();
            }
            else if (teamid != 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();

            }
            return rst;
        }
        public List<CrewInfo> GetJudgeCrewList(long groupid, string starttime)
        {
            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {
            	
            }
            //首先判断是否为日勤组
            int dailygroup=db.tbl_traingroups.Where(m => m.uid == groupid).Select(m => m.dailygroup).FirstOrDefault();
            if (dailygroup == 1)
            {
                var crewlist = GetGroupCrewList(groupid);
                return crewlist;
            }
            else
            {
                try
                {
                    var dutyinfo = db.tbl_duties
                        .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.groupid == groupid)
                        .FirstOrDefault();

                    if (dutyinfo != null)
                    {
                        var crewlist = GetDutyCrewList(dutyinfo.uid);

                        return crewlist;
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            return null;
        }


        public List<CrewInfo> GetDutyCrewList(long dutyid)
        {
            //选取crewid
            var dutycrewlist = db.tbl_dutycrews
                .Where(m => m.deleted == 0 && m.dutyid == dutyid)
                .Select(m => m.crewid)
                .ToList();

            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && dutycrewlist.Contains(m.uid))
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    uid = m.user.user.uid,
                    username = m.user.user.username,
                    realname = m.user.user.realname,
                    birthday = m.user.user.birthday,
                    gender = m.user.user.gender,
                    phonenum = m.user.user.phonenum,
                    roleid = m.user.role.uid,
                    rolename = m.user.role.rolename,
                    crewno = m.user.user.crewno == null ? "无" : m.user.user.crewno,
                    groupid = m.group.uid,
                    groupname = m.group.groupname
                })
                .ToList();
        }

        public List<ExecutiveInfo> GetSecCheckerNameList(long sectorid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.SectorExec && m.execparentid == sectorid)
                .Select(m => new ExecutiveInfo
                {
                    username = m.realname,
                    uid = m.uid,

                })
                .ToList();
        }

        public List<ExecutiveInfo> GetTeamCheckerNameList(long teamid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.TeamExec && m.execparentid == teamid)
                .Select(m => new ExecutiveInfo
                {
                    username = m.realname,
                    uid = m.uid,

                })
                .ToList();
        }

        public List<CrewInfo> GetTrainCoach(long groupid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && m.crewgroupid == groupid)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Where(m => m.role.rolename == "列车长")
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    username=m.user.user.realname,
                    uid=m.user.user.uid,

                })
                .ToList();
        }

        public List<CrewInfo> GetGroupCheckerNameList(long groupid)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && m.crewgroupid == groupid)
                .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new CrewInfo
                {
                    username = m.user.user.realname,
                    uid = m.user.user.uid,

                })
                .ToList();
        }

        public JqDataTableInfo GetDutyCrewDataTable(long dutyid, string act, long groupid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<CrewInfo> filteredCompanies;

            List<CrewInfo> alllist = new List<CrewInfo>();

            if (dutyid > 0)
            {
                alllist = GetDutyCrewList(dutyid);
            }
            else if (act == "add" && groupid > 0)
            {
                alllist = GetGroupCrewList(groupid);
            }

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.realname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CrewInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.realname :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }


            var result = from c in displayedCompanies
                         select new[] { 
                c.crewno,
                c.realname,
                c.rolename,
                c.groupname,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetCrewList(long dutyid, string act, long groupid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<CrewInfo> filteredCompanies;

            List<CrewInfo> alllist = new List<CrewInfo>();

            if (dutyid > 0)
            {
                alllist = GetDutyCrewList(dutyid);
            }
            else if (act == "add" && groupid > 0)
            {
                alllist = GetGroupCrewList(groupid);
            }

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.realname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CrewInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.realname :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }


            var result = from c in displayedCompanies
                         select new[] { 
                c.crewno,
                c.realname,
                c.rolename,
                c.groupname
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetGroupCrewDataTable(long groupid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<CrewInfo> filteredCompanies;

            List<CrewInfo> alllist = GetGroupCrewList(groupid);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.username.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CrewInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.username :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[] { 
                c.crewno, 
                c.policyface,
                c.imgurl,
                c.rolename,
                c.realname,
                c.username,
                String.Format("{0:yyyy-MM-dd}", c.birthday),
                c.gender.ToString(),
                c.phonenum,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool InsertAddtionCrew(long dutyid, string crewids)
        {
            bool rst = false;

            string[] idstr = crewids.Split(',');

            try
            {
                var existlist = GetDutyCrewList(dutyid);

                List<tbl_dutycrew> addlist = new List<tbl_dutycrew>();
                foreach (var item in idstr)
                {
                    var dupitem = existlist.Where(m => m.uid.ToString() == item).FirstOrDefault();
                    if (dupitem != null)
                    {
                        continue;
                    }

                    tbl_dutycrew additem = new tbl_dutycrew();
                    additem.dutyid = dutyid;
                    additem.crewid = long.Parse(item);

                    addlist.Add(additem);
                }

                db.tbl_dutycrews.InsertAllOnSubmit(addlist);
                db.SubmitChanges();

                rst = true;
            }
            catch (System.Exception ex)
            {
            	
            }

            return rst;
        }

        public bool DeleteDutyCrew(long dutyid, long crewid)
        {
            bool rst = false;

            try
            {
                tbl_dutycrew delitem = db.tbl_dutycrews.Where(m => m.dutyid == dutyid && m.crewid == crewid).FirstOrDefault();
                db.tbl_dutycrews.DeleteOnSubmit(delitem);
                db.SubmitChanges();
                rst = true;
            }
            catch (System.Exception ex)
            {
            	
            }

            return rst;
        }

        public string UpdateUserInfo(string imgurl, long uid, string realname, string birthday, byte sex, 
            string mailaddr, string qqnum, string phonenum, byte notifymail, string newpwd)
        {
            string rst = "";

            tbl_user edititem = GetUserById(uid);

            if (edititem != null)
            {

                edititem.realname = realname;
                if ((edititem.password != newpwd) && (newpwd != ""))
                    edititem.password = GetMD5Hash(newpwd);
                try { edititem.birthday = Convert.ToDateTime(birthday); }
                catch (Exception e) { }
                edititem.gender = sex;
                edititem.notifymail = notifymail;
                edititem.mailaddr = mailaddr;
                edititem.phonenum = phonenum;
                edititem.mailaddr = mailaddr;
                edititem.qqnum = qqnum;
                edititem.imgurl = imgurl;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "用户不存在";
            }
            
            return rst;
        }

        #endregion

        #region AdminRole CRUD
        public List<tbl_adminrole> GetRoleList()
        {
            return db.tbl_adminroles
                .Where(m => m.deleted == 0)
                .ToList();
        }

        public JqDataTableInfo GetRoleDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_adminrole> filteredCompanies;

            List<tbl_adminrole> alllist = GetRoleList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.rolename.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_adminrole, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.rolename :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.uid), 
                c.rolename,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteRole(long[] items)
        {
            try
            {
                var dellist = db.tbl_adminroles
                    .Where(m => items.Contains(m.uid))
                    .ToList();

                db.tbl_adminroles.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        public tbl_adminrole GetRoleById(long uid)
        {
            return db.tbl_adminroles
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public string InsertRole(string rolename, string role,long teamid)
        {
            tbl_adminrole newitem = new tbl_adminrole();
            long accountid = CommonModel.GetCurrAccountId();

            newitem.rolename = rolename;
            newitem.role = role;
            newitem.teamid = teamid;
            db.tbl_adminroles.InsertOnSubmit(newitem);

            db.SubmitChanges();

            return "";
        }

        public string UpdateRole(long uid, string rolename, string role,long teamid)
        {
            string rst = "";
            tbl_adminrole edititem = GetRoleById(uid);

            if (edititem != null)
            {
                edititem.rolename = rolename;
                edititem.role = role;
                edititem.teamid = teamid;
                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "数据不存在";
            }

            return rst;
        }
        #endregion

        #region Admin CRUD
        public List<AdminInfo> GetAdminList()
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.ADMIN)
                .OrderBy(m => m.createtime)
                .Join(db.tbl_adminroles, m => m.adminrole, r => r.uid, (m, r) => new { user = m, role = r })
                .Select(m => new AdminInfo
                {
                    uid = m.user.uid,
                    username = m.user.username,
                    rolename = m.role.rolename,
                    role = m.role.role,
                    createtime = m.user.createtime
                })
                .ToList();
        }

        public AdminInfo GetAdminByNamePwd(string uname, string upwd)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.username == uname && m.password == GetMD5Hash(upwd))
                .OrderBy(m => m.createtime)
                .Join(db.tbl_adminroles, m => m.adminrole, r => r.uid, (m, r) => new { user = m, role = r })
                .Select(m => new AdminInfo
                {
                    uid = m.user.uid,
                    username = m.user.username,
                    rolename = m.role.rolename,
                    createtime = m.user.createtime
                })
                .FirstOrDefault();
        }

        public JqDataTableInfo GetAdminDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<AdminInfo> filteredCompanies;

            List<AdminInfo> alllist = GetAdminList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.username.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AdminInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.username :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.uid), 
                c.username,
                c.rolename,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteAdmin(long[] items)
        {
            string delSql = "UPDATE tbl_user SET deleted = 1 WHERE ";
            string whereSql = "";
            foreach (long uid in items)
            {
                if (whereSql != "") whereSql += " OR";
                whereSql += " uid = " + uid;
            }

            delSql += whereSql;

            db.ExecuteCommand(delSql);

            return true;
        }

        public List<tbl_adminrole> GetAdminRoleList()
        {
            return db.tbl_adminroles
                .Where(m => m.deleted == 0)
                .ToList();
        }

        public string InsertAdmin(string username, string userpwd, long rolename)
        {
            tbl_user newuser = new tbl_user();
            long accountid = CommonModel.GetCurrAccountId();

            newuser.userkind = (int)UserKind.ADMIN;
            newuser.username = username;
            newuser.password = GetMD5Hash(userpwd);
            newuser.adminrole = rolename;
            newuser.createtime = DateTime.Now;
            newuser.policyface = "无";
            newuser.execparentid = db.tbl_adminroles.Where(m => m.uid == rolename).Select(m => m.teamid).FirstOrDefault();
            db.tbl_users.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateAdmin(long uid, string username, string userpwd, long rolename)
        {
            string rst = "";
            tbl_user edititem = GetUserById(uid);

            if (edititem != null)
            {
                edititem.password = GetMD5Hash(userpwd);
                edititem.adminrole = rolename;
                edititem.execparentid = db.tbl_adminroles.Where(m => m.uid == rolename).Select(m => m.teamid).FirstOrDefault();

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "用户不存在";
            }

            return rst;
        }

        public bool CheckDuplicateName(string username)
        {
            bool rst = true;

            rst = ((from m in db.tbl_users
                    where m.deleted == 0 && m.username == username
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public string UpdatePwd(long uid, string newpwd)
        {
            try
            {
                var aInfo = GetUserById(uid);
                if (aInfo != null)
                {
                    aInfo.password = UserModel.GetMD5Hash(newpwd);
                    db.SubmitChanges();

                    return "";
                }
            }
            catch (System.Exception ex)
            {
                CommonModel.WriteLogFile("MerchantModel", "UpdatePwd()", ex.ToString());
                return ex.ToString();
            }

            return "";
        }
        #endregion

        #region Executive CRUD
        public List<ExecutiveInfo> GetExecutiveList(int userkind)
        {
            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE)
                .OrderBy(m => m.createtime)
                .Select(m => new ExecutiveInfo
                {
                    uid = m.uid,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    execrole = m.execrole,
                    parentid = m.execparentid,
                    imgurl = m.imgurl
                })
                .ToList();

            var sectorlist = sectorModel.GetSectorList();
            var teamlist = teamModel.GetTeamList();

            foreach (var item in rst)
            {
                if (item.exectype == ExecType.SectorExec)
                {
                    item.parentname = sectorlist.Where(m => m.uid == item.parentid).Select(m => m.sectorname).FirstOrDefault();
                }
                else if (item.exectype == ExecType.TeamExec)
                {
                    item.parentname = teamlist.Where(m => m.uid == item.parentid).Select(m => m.teamname).FirstOrDefault();
                }
            }

            return rst;
        }

        public ExecutiveInfo GetDocExecUserInfo(long uid)
        {

            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind <= (int)UserKind.EXECUTIVE && m.uid == uid)
                .OrderBy(m => m.createtime)
                .Select(m => new ExecutiveInfo
                {
                    uid = m.uid,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    parentid = m.execparentid
                })
                .FirstOrDefault();

            var sectorlist = sectorModel.GetSectorList();
            var teamlist = teamModel.GetTeamList();

            if (rst != null)
            {
                if (rst.exectype == ExecType.SectorExec)
                {
                    rst.parentname = sectorlist.Where(m => m.uid == rst.parentid).Select(m => m.sectorname).FirstOrDefault();
                }
                else if (rst.exectype == ExecType.TeamExec)
                {
                    rst.parentname = teamlist.Where(m => m.uid == rst.parentid).Select(m => m.teamname).FirstOrDefault();
                }
                else
                {
                    rst.parentname = "系统管理员";
                }
            }

            return rst;
        }

        public ExecutiveInfo GetExecutiveByNamePwd(string uname, string upwd)
        {
            return db.tbl_users
                .Where(m => m.deleted == 0 && m.username == uname && m.password == GetMD5Hash(upwd))
                .OrderBy(m => m.createtime)
                .Select(m => new ExecutiveInfo
                {
                    uid = m.uid,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    parentid = m.execparentid

                })
                .FirstOrDefault();
        }

        public List<UserDetailInfo> GetGanbuList(int userkind)
        {
            List<UserDetailInfo> retlist = new List<UserDetailInfo>();

            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind != (int)UserKind.ADMIN)
//                 .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
//                 .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
//                 .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { user = m, team = l })
                .OrderBy(m => m.createtime)
                .Select(m => new UserDetailInfo
                {
                    uid = m.uid,
                    userkind = m.userkind,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    execrole = m.execrole,
                    parentid = m.execparentid,
                    imgurl = m.imgurl,
                    crewroleid = m.crewrole,
                    crewgroupid = m.crewgroupid,
                    crewno=m.crewno,
                    policyface=m.policyface
//                     parentname = m.team.teamname,
//                     crewrole = m.user.user.role.rolename,
//                     crewgroupname = m.user.group.groupname
                })
                .ToList();

            var sectorlist = sectorModel.GetSectorList();
            var teamlist = teamModel.GetTeamList();
            var rolelist = GetCrewRoleList();
            var grouplist = groupModel.GetGroupList();

            foreach (var item in rst)
            {
                if (item.userkind == (int)UserKind.EXECUTIVE)
                {
                    if (item.exectype == ExecType.SectorExec)
                    {
                        item.parentname = sectorlist.Where(m => m.uid == item.parentid).Select(m => m.sectorname).FirstOrDefault();
                    }
                    else if (item.exectype == ExecType.TeamExec)
                    {
                        item.parentname = teamlist.Where(m => m.uid == item.parentid).Select(m => m.teamname).FirstOrDefault();
                        item.teamid = item.parentid;
                    }
                    item.usertype = item.exectype;
                }
                else
                {
                    var groupinfo = grouplist.Where(m => m.uid == item.crewgroupid).FirstOrDefault();
                    item.usertype = item.crewrole;
                    try
                    {
                        item.parentname = teamlist.Where(m => m.uid == groupinfo.teamid).Select(m => m.teamname).FirstOrDefault();
                        item.teamid = teamlist.Where(m => m.uid == groupinfo.teamid).Select(m => m.uid).FirstOrDefault();
                        item.crewgroupname = groupinfo.groupname;
                    }
                    catch 
                    {
                        groupinfo = db.tbl_traingroups
                                        .Where(m => m.uid==item.crewgroupid)
                                        .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                                        .OrderBy(m => m.team.sortid)
                                        .Select(m => new GroupInfo
                                        {
                                            uid = m.group.uid,
                                            teamid = m.team.uid,
                                            teamname = m.team.teamname,
                                            groupname = m.group.groupname,
                                            sortid = m.group.sortid,
                                            createtime = m.group.createtime
                                        })
                                        .FirstOrDefault();
                        item.parentname = db.tbl_railteams.Where(m => m.uid == groupinfo.teamid).Select(m => m.teamname).FirstOrDefault();
                        item.teamid = db.tbl_railteams.Where(m => m.uid == groupinfo.teamid).Select(m => m.uid).FirstOrDefault();
                        item.crewgroupname = "已删除";
                    }
                    item.crewrole = rolelist.Where(m => m.uid == item.crewroleid).Select(m => m.rolename).FirstOrDefault();
                    
                    if (item.crewrole != "列车长")
                    {
                       // continue;
                    }
                    item.usertype = item.crewrole == "列车长" ? "列车长" : "列车员";
                }

                retlist.Add(item);
            }

            if (userkind == 1)  //科室干部
            {
                retlist = retlist.Where(m => m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.SectorExec).ToList();
            }
            else if (userkind == 2) //车队干部
            {
                retlist = retlist.Where(m => m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.TeamExec).ToList();
            }
            else if (userkind == 3) //列车长
            {
                retlist = retlist.Where(m => m.userkind == (int)UserKind.CREW && m.crewrole == "列车长").ToList();
            }
            else if (userkind == 4) //列车员
            {
                retlist = retlist.Where(m => m.userkind == (int)UserKind.CREW && m.crewrole != "列车长").ToList();
            }
            
            return retlist;
        }

        public JqDataTableInfo GetGanbuDataTable(int userkind, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<UserDetailInfo> filteredCompanies;

            List<UserDetailInfo> alllist = GetGanbuList(userkind);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.username.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<UserDetailInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.username :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.uid),
                c.usertype,
                //部门名称
                c.parentname,
                c.crewgroupname,
                (c.userkind == (int)UserKind.EXECUTIVE) ? c.execrole : c.crewrole,
                c.username,
                c.crewno,
                c.realname,
                c.imgurl,
                c.policyface,
                c.gender.ToString(),
                String.Format("{0:yyyy-MM-dd}", c.birthday),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetGanbuDataTable(int userkind, long groupid, long teamid,long sectorid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<UserDetailInfo> filteredCompanies;

            List<UserDetailInfo> alllist = GetGanbuList(userkind);
           
            if (userkind==1)
            {
                if(sectorid!=0){
                    alllist = alllist.Where(m => m.parentid == sectorid).ToList();
                }
            }
            else if (userkind == 2)
            {
                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
            else if (userkind == 3 || userkind == 4)
            {
                if (groupid != 0)
                {
                    alllist = alllist.Where(m => m.crewgroupid == groupid).ToList();
                }

                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
            else if (userkind == 0)
            {
                if (groupid != 0)
                {
                    alllist = alllist.Where(m => m.crewgroupid == groupid).ToList();
                }

                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && (c.realname.ToLower().Contains(param.sSearch.ToLower()) ||( c.crewno != null && c.crewno.ToLower().Contains(param.sSearch.ToLower()))));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<UserDetailInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.username :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.uid),
                c.usertype,
                //部门名称
                c.parentname,
                c.crewgroupname,
                (c.userkind == (int)UserKind.EXECUTIVE) ? c.execrole : c.crewrole,
                c.username,
                c.crewno,
                c.realname,
                c.imgurl,
                c.policyface,
                c.gender.ToString(),
                String.Format("{0:yyyy-MM-dd}", c.birthday),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteExecutive(long[] items)
        {
            string delSql = "UPDATE tbl_user SET deleted = 1 WHERE ";
            string whereSql = "";
            foreach (long uid in items)
            {
                if (whereSql != "") whereSql += " OR";
                whereSql += " uid = " + uid;
            }

            delSql += whereSql;

            db.ExecuteCommand(delSql);

            return true;
        }

        public string InsertPerson(string username, string pwd, string crewno, string policyface, string exectype, long parentid, long groupid,
            string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage, byte teammanage,long crewrole)
        {
            try
            {
                tbl_user newuser = new tbl_user();
                long accountid = CommonModel.GetCurrAccountId();

                string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
                string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
                string targetbase = HostingEnvironment.MapPath("~/" + savepath);

                if (exectype == "科室干部" || exectype == "车队干部")
                {
                    if (exectype == "车队干部")
                    {
                        newuser.opinionmanage = opinionmanage;
                        newuser.teammanage = teammanage;
                    }
                    else
                    {
                        newuser.opinionmanage = 0;
                        newuser.teammanage = 0;
                    }
                    newuser.userkind = 1;
                    newuser.crewrole = 0;
                    newuser.execparentid = parentid;
                    newuser.crewgroupid = 0;
                    newuser.execrole = execrole;
                    newuser.exectype = exectype;

                }
                else if (exectype == "列车长" || exectype == "列车员")
                {
                    if (exectype == "列车长")
                    {
                        newuser.crewrole = 2;
                        newuser.exectype = "列车长";
                    }
                    else
                    {
                        newuser.crewrole = crewrole;
                        newuser.exectype = exectype;
                    }
                    newuser.userkind = 2;
                    newuser.opinionmanage = 0;
                    newuser.teammanage = 0;
                    newuser.execparentid = 0;
                    newuser.crewgroupid = groupid;
                }

                newuser.username = username;
                newuser.password = GetMD5Hash(pwd);
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

        //public string UpdateExecutive(long uid, string username, string pwd, string crewno, string policyface, string exectype, long parentid, long groupid,
        //    string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage, byte teammanage)
        //{
        //    string rst = "";
        //    tbl_user edititem = GetUserById(uid);
        //    string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
        //    string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
        //    string targetbase = HostingEnvironment.MapPath("~/" + savepath);

        //    if (edititem != null)
        //    {
        //        if (!String.IsNullOrEmpty(pwd))
        //        {
        //            edititem.password = GetMD5Hash(pwd);
        //        }
        //        if (exectype == "科室干部" || exectype == "车队干部")
        //        {
        //            edititem.userkind = 1;
        //            edititem.crewrole = 0;
        //            edititem.execparentid = parentid;
        //            edititem.crewgroupid = 0;
        //            if (exectype == "车队干部")
        //            {
        //                edititem.opinionmanage = opinionmanage;
        //                edititem.teammanage = teammanage;
        //            }
        //            else
        //            {
        //                edititem.opinionmanage = 0;
        //                edititem.teammanage = 0;
        //            }
        //        }
        //        else if (exectype == "列车长"||exectype == "列车员")
        //        {
        //            if (exectype == "列车长")
        //            {
        //                edititem.crewrole = 2;
        //            }
        //            edititem.userkind = 2;
        //            edititem.opinionmanage = 0;
        //            edititem.teammanage = 0;
        //            edititem.execparentid = 0;
        //            edititem.execrole = execrole;
        //            edititem.crewgroupid = groupid;
        //        }
        //        edititem.username = username;
        //        edititem.exectype = exectype;
        //        edititem.crewno = crewno;
        //        edititem.policyface = policyface;
        //        edititem.realname = realname;


        //        if (imgurl != edititem.imgurl)
        //        {

        //            if (File.Exists(orgbase + imgurl))
        //            {
        //                if (!Directory.Exists(targetbase))
        //                {
        //                    Directory.CreateDirectory(targetbase);
        //                }
        //                File.Move(orgbase + imgurl, targetbase + imgurl);
        //            }

        //            edititem.imgurl = savepath + imgurl;
        //        }

        //        edititem.birthday = birthday;
        //        edititem.gender = gender;
        //        edititem.phonenum = phonenum;


        //        db.SubmitChanges();
        //        rst = "";
        //    }
        //    else
        //    {
        //        rst = "用户不存在";
        //    }

        //    return rst;
        //}
        public string UpdatePerson(long uid, string username, string pwd, string crewno, string policyface, string exectype, long parentid, long groupid,
            string execrole, string realname, string imgurl, DateTime birthday, byte gender, string phonenum, byte opinionmanage, byte teammanage,long crewrole)
        {
            string rst = "";
            tbl_user edititem = GetUserById(uid);
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            if (edititem != null)
            {
                if (!String.IsNullOrEmpty(pwd))
                {
                    edititem.password = GetMD5Hash(pwd);
                }
                if (exectype == "科室干部" || exectype == "车队干部")
                {
                    if (exectype == "车队干部")
                    {
                        edititem.opinionmanage = opinionmanage;
                        edititem.teammanage = teammanage;
                    }
                    else
                    {
                        edititem.opinionmanage = 0;
                        edititem.teammanage = 0;
                    }
                    edititem.userkind = 1;
                    edititem.crewrole = 0;
                    edititem.execparentid = parentid;
                    edititem.crewgroupid = 0;
                    edititem.execrole = execrole;
                    edititem.exectype = exectype;

                }
                else if (exectype == "列车长" || exectype == "列车员")
                {
                    if (exectype == "列车长")
                    {
                        edititem.crewrole = 2;
                        edititem.exectype = "列车长";
                    }
                    else
                    {
                        edititem.crewrole = crewrole;
                        edititem.exectype = exectype;
                    }
                    edititem.userkind = 2;
                    edititem.opinionmanage = 0;
                    edititem.teammanage = 0;
                    edititem.execparentid = 0;
                    edititem.crewgroupid = groupid;
                }
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
        public List<ExecutiveInfo> FindExecutiveList(long selid, string selkind)
        {
            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.execparentid == selid && m.exectype == selkind)
                .OrderBy(m => m.createtime)
                .Select(m => new ExecutiveInfo
                {
                    uid = m.uid,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    parentid = m.execparentid
                })
                .ToList();

            var sectorlist = sectorModel.GetSectorList();
            var teamlist = teamModel.GetTeamList();

            foreach (var item in rst)
            {
                if (item.exectype == ExecType.SectorExec)
                {
                    item.parentname = sectorlist.Where(m => m.uid == item.parentid).Select(m => m.sectorname).FirstOrDefault();
                }
                else if (item.exectype == ExecType.TeamExec)
                {
                    item.parentname = teamlist.Where(m => m.uid == item.parentid).Select(m => m.teamname).FirstOrDefault();
                }
            }

            return rst;
        }

        #endregion

        public List<UserInfo> GetAllUserList()
        {
            List<UserInfo> rst = new List<UserInfo>();

            rst = db.tbl_users
                .Where(m => m.deleted == 0)
                .Select(m => new UserInfo
                {
                    uid = m.uid,
                    realname = m.realname
                })
                .ToList();

            return rst;
        }

        public List<UserInfo> GetAllChezhangUpList()
        {
            List<UserInfo> rst = new List<UserInfo>();

            rst = db.tbl_users
                .Where(m => m.deleted == 0 && ((m.userkind == (int)UserKind.CREW && m.crewrole == 2) || m.userkind == (int)UserKind.EXECUTIVE))
                .Select(m => new UserInfo
                {
                    uid = m.uid,
                    realname = m.realname
                })
                .ToList();

            return rst;
        }

        //得到用户名列表
        public List<UserInfo> GetUserListbyTeamandPolicy(string policyface, long teamid)
        {
            List<UserInfo> rst = new List<UserInfo>();
            if (policyface == "党")
            {
                rst = db.tbl_users
               .Where(m => m.deleted == 0 && m.userkind==2)
               .Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
               .Where(m => m.user.policyface.Contains("党") && m.group.teamid == teamid)
               .Select(m => new UserInfo
               {
                   uid = m.user.uid,
                   realname = m.user.realname,
               })
               .ToList();
            }
            else
            {
                rst = db.tbl_users
                    .Where(m => m.deleted == 0 && m.userkind == 2)
                    .Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                    .Where(m => !m.user.policyface.Contains("党") && m.group.teamid == teamid)
                    .Select(m => new UserInfo
                    {
                        uid=m.user.uid,
                        realname = m.user.realname,
                    })
                    .ToList();
            }
            return rst;
        }


        public List<long> GetDutynoByGroup(string starttime, long groupid   )
        {
            DateTime startd = new DateTime();
            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {
                
            }
            var dutylist=db.tbl_duties.Where(m => m.deleted == 0);
            var trainno = dutylist.Where(m => m.starttime <= startd && m.endtime >= startd && m.groupid == groupid)
                                  .Join(db.tbl_trainnos, m => m.trainno, l => l.trainno, (m, l) => new { duty = m, train = l })
                                  .Select(m=>m.train.uid).ToList();
            return trainno;
        }
        public List<UserInfo> GetUserListByTeamidCombineTeamsec(long teamid)
        {
            var crewlist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW);
            var rst = crewlist.Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                              .Join(db.tbl_crewroles, m => m.user.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                            .Where(m => m.user.group.teamid == teamid)
                            .Select(m => new UserInfo
                            {
                                realname = m.user.user.realname,
                                uid = m.user.user.uid,
                                role = m.role.rolename,
                                crewno = m.user.user.crewno
                            })
                            .ToList();
            var teamseclist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.exectype == ExecType.TeamExec);
            var rst2 = teamseclist.Select(m => new UserInfo
                            {
                                realname = m.realname,
                                uid = m.uid,
                                role = m.execrole,
                                crewno = m.crewno
                            })
                            .ToList();
            return rst2.Concat(rst).ToList();
        }

        public List<UserInfo> GetUserListByTeamid(long teamid)
        {
            var allusers = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW);
            var rst = allusers.Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                              .Join(db.tbl_crewroles, m => m.user.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                            .Where(m => m.user.group.teamid == teamid)
                            .Select(m => new UserInfo
                            {
                                realname=m.user.user.realname,
                                uid=m.user.user.uid,
                                role=m.role.rolename,
                                crewno = m.user.user.crewno
                            })
                            .ToList();
            return rst;
        }
        public List<UserInfo> GetUserListByGroupid(long groupid)
        {
            return  db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW&&m.crewgroupid==groupid)
                            .Select(m => new UserInfo
                            {
                                realname = m.realname,
                                uid = m.uid,
                            })
                            .ToList();
    
        }
        //导出Excel
        public System.Data.DataTable ExportPersonnelList(int userkind, long groupid, long teamid,long sectorid)
        {
            var products = new System.Data.DataTable("人员信息表");


            List<UserDetailInfo> alllist = GetGanbuList(userkind);
          //  var resullist = alllist;
            if (userkind == 1)
            {
                if (sectorid != 0)
                {
                    alllist = alllist.Where(m => m.parentid == sectorid).ToList();
                }
            }
            else if (userkind == 2)
            {
                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
            else if (userkind == 3 || userkind == 4)
            {
                if (groupid != 0)
                {
                    alllist = alllist.Where(m => m.crewgroupid == groupid).ToList();
                }

                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
            else if (userkind == 0)
            {
                if (groupid != 0)
                {
                    alllist = alllist.Where(m => m.crewgroupid == groupid).ToList();
                }

                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                }
            }
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
                        item.parentname,
                        item.crewgroupname,
                        (item.userkind == (int)UserKind.EXECUTIVE) ? item.execrole : item.crewrole,
                        item.username,
                        item.crewno,
                        item.realname,
                        item.policyface,
                        item.gender == 0 ? "男" : "女",
                        String.Format("{0:yyyy/MM/dd}",item.birthday)
                        );
                    i++;
                }
            }
            return products;
        }

        //导入人员表格
        OleDbConnection oledbConn;
        public string ImportUserData(string filepath)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
            long myid = CommonModel.GetSessionUserID();

            try
            {
                string fname = orgbase + filepath;
                if (!File.Exists(fname))
                {
                    return "文件不存在";
                }

                if (Path.GetExtension(fname) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(fname) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fname + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;';");
                }
                else if (Path.GetExtension(fname) == ".csv")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties='text;'");
                }
                oledbConn.Open();
                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + Sheet1 + "]";
                var reader = cmd.ExecuteReader();

                int ind = 0;
                List<tbl_user> examlist = new List<tbl_user>();
                var teamlist = teamModel.GetTeamList();

                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        ind++;
                        continue;
                    }

                    tbl_user newitem = new tbl_user();

                    if (reader.GetValue(0).ToString() == "科室干部")
                    {
                        newitem.userkind = 1;
                        newitem.exectype = "科室干部";
                        newitem.crewgroupid = 0;
                        newitem.execparentid = changeexecparentnametoid(reader.GetValue(8).ToString());
                        newitem.crewrole=0;
                        newitem.execrole=reader.GetValue(10).ToString();
                    }
                    else if (reader.GetValue(0).ToString() == "车队干部")
                    {
                        newitem.userkind = 1;
                        newitem.exectype = "车队干部";
                        newitem.crewgroupid = 0;
                        newitem.execparentid = teamlist.Where(m=>m.deleted==0&&m.teamname==reader.GetValue(8).ToString()).Select(m=>m.uid).FirstOrDefault();
                        newitem.crewrole=0;
                        newitem.execrole=reader.GetValue(10).ToString();

                    }
                    else if (reader.GetValue(0).ToString() == "列车长")
                    {
                        newitem.userkind = 2;
                        newitem.exectype = "列车长";
                        //newitem.exectype = ;
                        try
                        {
                            var crewgroupid = changegroupnametoid(reader.GetValue(9).ToString());
                            if (crewgroupid==0)
                            {
                                rst = "班组输入错误或不存在该班组。";
                                return rst;
                            }
                            newitem.crewgroupid = crewgroupid;
                        }
                        catch (Exception e)
                        {
                            rst = "班组输入错误。";
                            return rst;

                        }
                        newitem.execparentid = 0;
                        newitem.crewrole= db.tbl_crewroles.Where(m=>m.rolename==reader.GetValue(10).ToString()).Select(m=>m.uid).FirstOrDefault();
                     //   newitem.execrole=;

                    }
                    else if (reader.GetValue(0).ToString() == "列车员")
                    {
                        newitem.userkind = 2;
                        newitem.exectype = "列车员";

                        //newitem.exectype = ;
                        try
                        {
                            var crewgroupid = changegroupnametoid(reader.GetValue(9).ToString());
                            if (crewgroupid == 0)
                            {
                                rst = "班组输入错误或不存在该班组。";
                                return rst;
                            }
                            newitem.crewgroupid = crewgroupid;

                        }
                        catch (Exception e)
                        {
                            rst = "班组输入错误。";
                            return rst;
                        }
                        newitem.execparentid =0;
                        newitem.crewrole = db.tbl_crewroles.Where(m => m.rolename == reader.GetValue(10).ToString()).Select(m => m.uid).FirstOrDefault();
                        // newitem.execrole=;
                    }
                    else
                    {
                        continue;
                    }


                    newitem.username = reader.GetValue(1).ToString();
                    newitem.password = GetMD5Hash(reader.GetValue(2).ToString());
                    newitem.crewno = reader.GetValue(4).ToString();
                    newitem.policyface = reader.GetValue(7).ToString();
                    newitem.realname = reader.GetValue(3).ToString();

                    try
                    {
                        newitem.birthday = DateTime.Parse(reader.GetValue(6).ToString());
                    }
                    catch 
                    {
                    	try
                    	{
                            DateTime dtime = DateTime.ParseExact(reader.GetValue(6).ToString(), "yyyyMMdd", null);
                            newitem.birthday=dtime;
                    	}
                    	catch (System.Exception ex)
                    	{
                            rst = "出生年月日不正确，请检查后上传。";
                            return rst;
                    	}
                    }
                    if (reader.GetValue(5).ToString()=="男")
                    {
                        newitem.gender = 0;
                    }
                    else
                    {
                        newitem.gender = 1;
                    }
                    newitem.phonenum = reader.GetValue(11).ToString();
                    newitem.createtime = DateTime.Now;

                    examlist.Add(newitem);

                    ind++;
                }
                reader.Close();

                db.tbl_users.InsertAllOnSubmit(examlist);
                db.SubmitChanges();
                rst = "";
            }

            catch (Exception ex)
            {
                rst = "请检测导入内容格式是否正确.";
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }
        public long changeexecparentnametoid(string parentname)
        {
            return db.tbl_railsectors.Where(m => m.deleted == 0 && m.sectorname == parentname).Select(m => m.uid).FirstOrDefault();
           
        }

        public long changegroupnametoid(string groupname)
        {
            return db.tbl_traingroups.Where(m => m.deleted == 0 && m.groupname == groupname).Select(m => m.uid).FirstOrDefault();
        }
    }
}