using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.IO;
using System.Collections;

namespace TLSite.Models
{
    public class GroupInfo
    {
        public long uid { get; set; }
        public long teamid { get; set; }
        public string teamname { get; set; }
        public string groupname { get; set; }
        public int sortid { get; set; }
        public DateTime createtime { get; set; }
    }

    public class GroupModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 班组
        public List<GroupInfo> GetGroupList()
        {
            return db.tbl_traingroups
                .Where(m => m.deleted == 0)
                .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                .OrderBy(m => m.team.sortid)
                .Select(m => new GroupInfo { 
                    uid = m.group.uid,
                    teamid = m.team.uid,
                    teamname = m.team.teamname,
                    groupname = m.group.groupname,
                    sortid = m.group.sortid,
                    createtime = m.group.createtime
                })
                .ToList();
        }

        public ArrayList GetGroupArrayList()
        {
            var list= db.tbl_traingroups
                .Where(m => m.deleted == 0)
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
                .ToList();
            ArrayList alist = new ArrayList();
            foreach (var l in list) {
                alist.Add(l);
            }
            return alist;
        }
        public List<GroupInfo> GetGroupListOfTeam(long teamid)
        {
            var rst= db.tbl_traingroups
                .Where(m => m.deleted == 0)
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
                .ToList();

            if (teamid!=0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }
            return rst;
        }

        public List<GroupInfo> GetGroupListOfTeamWithoutDailyGroup(long teamid)
        {
            var rst = db.tbl_traingroups
                .Where(m => m.deleted == 0&&m.dailygroup==0)
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
                .ToList();

            if (teamid != 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }
            return rst;
        }
        public JqDataTableInfo GetGroupDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<GroupInfo> filteredCompanies;

            List<GroupInfo> alllist = GetGroupList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.groupname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<GroupInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.groupname :
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
                c.teamname,
                c.groupname,
                c.sortid.ToString(),
                String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteGroup(long[] items)
        {
            string delSql = "UPDATE tbl_traingroup SET deleted = 1 WHERE ";
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

        public tbl_traingroup GetGroupById(long uid)
        {
            return db.tbl_traingroups
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public long GetDeletedGroupteamid(long uid)
        {
            return db.tbl_traingroups
                .Where(m => m.uid == uid).Select(m=>m.teamid)
                .FirstOrDefault();
        }

        public string InsertGroup(long teamid, string groupname, int sortid, int iscont,string dailygroup)
        {
            string rst = "";
            tbl_traingroup newuser = new tbl_traingroup();

            newuser.teamid = teamid;
            newuser.groupname = groupname;
            newuser.sortid = sortid;
            newuser.createtime = DateTime.Now;
            if (dailygroup=="1")
            {
                if (CheckHasDailiGroup(teamid))
                {
                    rst = "该车队已有日勤组";
                    return rst;
                }
                newuser.dailygroup = 1;
            }
            else
            {
                newuser.dailygroup = 0;
            }
            db.tbl_traingroups.InsertOnSubmit(newuser);

            db.SubmitChanges();

            if (iscont == 0)
            {
                rst = "";
            }
            else
            {
                rst = newuser.uid.ToString();
            }

            return rst;
        }

        public string UpdateGroup(long uid, long teamid, string groupname, int sortid, int iscont,string dailygroup)
        {
            string rst = "";
            tbl_traingroup edititem = GetGroupById(uid);

            if (edititem != null)
            {
                edititem.teamid = teamid;
                edititem.groupname = groupname;
                edititem.sortid = sortid;
                if (dailygroup == "1")
                {
                    var dailygrouplist = db.tbl_traingroups.Where(m => m.deleted == 0 && m.teamid == teamid && m.dailygroup == 1 && m.uid != uid).ToList();
                    if (dailygrouplist.Count>0)
                    {
                        rst = "该车队已有日勤组";
                        return rst;
                    }
                    edititem.dailygroup = 1;
                }
                else
                {
                    edititem.dailygroup = 0;
                }
                db.SubmitChanges();

                if (iscont == 0)
                {
                    rst = "";
                }
                else
                {
                    rst = edititem.uid.ToString();
                }
            }
            else
            {
                rst = "此班组不存在";
            }

            return rst;
        }

        public bool CheckDuplicateName(string groupname, long uid)
        {
            bool rst = true;
            long zoneid = CommonModel.GetCurrZoneId();

            rst = ((from m in db.tbl_traingroups
                    where m.deleted == 0 && m.groupname == groupname && m.uid != uid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public string InsertOrUpdateCrew(long groupid, NameValueCollection request)
        {
            string rst = "";

            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            try
            {
                long crewid = long.Parse(request["crewuid"].ToString());
                long roleid = long.Parse(request["crewroleid"].ToString());
                string crewno = request["crewno"].ToString();
                string policface = request["policyface"].ToString();
                string username = request["username"].ToString();
                string userpwd = request["userpwd"].ToString();
                string realname = request["realname"].ToString();
                DateTime birthday = DateTime.Parse(request["birthday"].ToString());
                byte gender = byte.Parse(request["gender"].ToString());
                string phonenum = request["phonenum"].ToString();
                string imgurl = request["path"].ToString();

                if (crewid == 0)
                {
                    tbl_user newuser = new tbl_user();
                    newuser.userkind = (int)UserKind.CREW;
                    newuser.username = username;

                    if (String.IsNullOrWhiteSpace(userpwd))
                    {
                        newuser.password = UserModel.GetMD5Hash(username);
                    }
                    else
                    {
                        newuser.password = UserModel.GetMD5Hash(userpwd);
                    }
                    
                    newuser.realname = realname;
                    newuser.birthday = birthday;
                    newuser.gender = gender;
                    newuser.phonenum = phonenum;
                    newuser.crewrole = roleid;
                    newuser.crewno = crewno;
                    newuser.policyface = policface;
                    newuser.crewgroupid = groupid;
                    newuser.createtime = DateTime.Now;

                    if (File.Exists(orgbase + imgurl))
                    {
                        if (!Directory.Exists(targetbase))
                        {
                            Directory.CreateDirectory(targetbase);
                        }
                        File.Move(orgbase + imgurl, targetbase + imgurl);
                    }

                    newuser.imgurl = imgurl;
                    db.tbl_users.InsertOnSubmit(newuser);
                    db.SubmitChanges();
                }
                else if (crewid > 0)
                {
                    tbl_user edituser = db.tbl_users
                        .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && m.crewgroupid == groupid && m.uid == crewid)
                        .FirstOrDefault();
                    if (edituser == null)
                    {
                        rst = "该用户不存在！";
                    }
                    else
                    {
                        edituser.username = username;
                        edituser.realname = realname;
                        edituser.birthday = birthday;
                        edituser.gender = gender;
                        edituser.phonenum = phonenum;
                        edituser.crewno = crewno;
                        edituser.policyface = policface;
                        edituser.crewrole = roleid;

                        if (!String.IsNullOrWhiteSpace(userpwd))
                        {
                            edituser.password = UserModel.GetMD5Hash(userpwd);
                        }

                        if (imgurl != edituser.imgurl)
                        {
                            if (File.Exists(orgbase + imgurl))
                            {
                                if (!Directory.Exists(targetbase))
                                {
                                    Directory.CreateDirectory(targetbase);
                                }
                                File.Move(orgbase + imgurl, targetbase + imgurl);
                            }

                            edituser.imgurl = imgurl;
                        }

                    }
                    db.SubmitChanges();
                }

            }
            catch (System.Exception ex)
            {
                CommonModel.WriteLogFile("GroupModel", "InsertOrUpdateCrew()", ex.ToString());
                rst = ex.ToString();
            }


            return rst;
        }

        public bool DeleteGroupCrew(long groupid, long[] items)
        {
            string delSql = "UPDATE tbl_user SET deleted = 1 WHERE crewgroupid = " + groupid.ToString() + " and (";
            string whereSql = "";
            foreach (long uid in items)
            {
                if (whereSql != "") whereSql += " OR";
                whereSql += " uid = " + uid;
            }

            delSql += whereSql + ")";

            db.ExecuteCommand(delSql);

            return true;
        }

        public List<tbl_traingroup> GetJudgeGroupList(long teamid, string starttime)
        {
            long zoneid = CommonModel.GetCurrZoneId();
            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                var dutylist = db.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.teamid == teamid)
                    .ToList();

                var groupids = dutylist.Select(m => m.groupid).ToList();
                var grouplist = db.tbl_traingroups
                    .Where(m => m.deleted == 0 && groupids.Contains(m.uid))
                    .ToList();

                return grouplist;
            }
            catch (System.Exception ex)
            {

            }

            return null;
        }

        public List<tbl_traingroup> GetJudgeGroupListAdditionDailyGroup(long teamid, string starttime)
        {
            long zoneid = CommonModel.GetCurrZoneId();
            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                var dutylist = db.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.teamid == teamid)
                    .ToList();

                var groupids = dutylist.Select(m => m.groupid).ToList();
                var grouplist = db.tbl_traingroups
                    .Where(m => m.deleted == 0 && groupids.Contains(m.uid))
                    .ToList();

                var dailygroup = db.tbl_traingroups
                    .Where(m => m.deleted == 0 && m.dailygroup == 1&&m.teamid==teamid).FirstOrDefault();
                if (dailygroup!=null)
                {
                    grouplist.Add(dailygroup);
                }
               // grouplist = grouplist.Concat(dailygroup).ToList();
                    
                return grouplist;
            }
            catch (System.Exception ex)
            {

            }

            return null;
        }
        public long GetGroupIdByUserid(long uid)
        {
            return db.tbl_users.Where(m => m.uid == uid).Select(m => m.crewgroupid).FirstOrDefault();
        }
        public bool CheckIfDailiGroup(long groupid)
        {
            var dailygroup =db.tbl_traingroups.Where(m => m.uid == groupid&&m.deleted==0).Select(m => m.dailygroup).FirstOrDefault();
            if (dailygroup==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckHasDailiGroup(long teamid)
        {
            var dailygroup = db.tbl_traingroups.Where(m => m.deleted==0&&m.dailygroup==1&&m.teamid==teamid).ToList();
            if (dailygroup.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}