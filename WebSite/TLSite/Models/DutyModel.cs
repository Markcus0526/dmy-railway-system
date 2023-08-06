using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class DutyInfo
    {
        public long uid { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public long teamid { get; set; }
        public long routeid { get; set; }
        public long groupid { get; set; }
        public string teamname { get; set; }
        public string routename { get; set; }
        public string groupname { get; set; }
        public string trainno { get; set; }
        public DateTime createtime { get; set; }
        public string name { get; set; }
        public string crewno { get; set; }
        public string rolename { get; set; }
    }



    public class DutyModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        UserModel userModel = new UserModel();

        #region 出乘
        public List<DutyInfo> GetDutyList()
        {
            return db.tbl_duties
                .Where(m => m.deleted == 0)
                .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { duty = m, team = l })
                .Join(db.tbl_railroutes, m => m.duty.routeid, l => l.uid, (m, l) => new { duty = m, teamroute = l })
                .Join(db.tbl_traingroups, m => m.duty.duty.groupid, l => l.uid, (m, l) => new { duty = m, teamroutegroup = l })
                .OrderBy(m => m.duty.duty.duty.createtime)
                .Select(m => new DutyInfo
                {
                    uid = m.duty.duty.duty.uid,
                    starttime = m.duty.duty.duty.starttime,
                    endtime = m.duty.duty.duty.endtime,
                    teamid = m.duty.duty.duty.teamid,
                    routeid = m.duty.duty.duty.routeid,
                    groupid = m.duty.duty.duty.groupid,
                    teamname = m.duty.duty.team.teamname,
                    routename = m.duty.teamroute.routename,
                    groupname = m.teamroutegroup.groupname,
                    trainno = m.duty.duty.duty.trainno,
                    createtime = m.duty.duty.duty.createtime
                })
                .ToList();
        }
        public List<DutyInfo> GetTeamAdminDutyList()
        {
            var currentid = CommonModel.GetSessionUserID();
            var currentuser = db.tbl_users.Where(m => m.uid == currentid).FirstOrDefault();
            var currentteamid = db.tbl_adminroles.Where(m => m.uid == currentuser.adminrole).FirstOrDefault().teamid;
            var currentdutylist = db.tbl_duties.Where(m =>m.deleted==0&& m.teamid == currentteamid).ToList();
            return currentdutylist 
                .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { duty = m, team = l })
                .Join(db.tbl_railroutes, m => m.duty.routeid, l => l.uid, (m, l) => new { duty = m, teamroute = l })
                .Join(db.tbl_traingroups, m => m.duty.duty.groupid, l => l.uid, (m, l) => new { duty = m, teamroutegroup = l })
                .OrderBy(m => m.duty.duty.duty.createtime)
                .Select(m => new DutyInfo
                {
                    uid = m.duty.duty.duty.uid,
                    starttime = m.duty.duty.duty.starttime,
                    endtime = m.duty.duty.duty.endtime,
                    teamid = m.duty.duty.duty.teamid,
                    routeid = m.duty.duty.duty.routeid,
                    groupid = m.duty.duty.duty.groupid,
                    teamname = m.duty.duty.team.teamname,
                    routename = m.duty.teamroute.routename,
                    groupname = m.teamroutegroup.groupname,
                    trainno = m.duty.duty.duty.trainno,
                    createtime = m.duty.duty.duty.createtime
                })
                .ToList();
        }


        public JqDataTableInfo GetDutyDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<DutyInfo> filteredCompanies;


            List<DutyInfo> alllist = new List<DutyInfo>();
                //GetDutyList();

            var userrole = CommonModel.GetUserRoleInfo();
            if (((string)userrole).Contains("TeamAdmin"))
            {
                alllist = GetTeamAdminDutyList();
            }
            else
            {
                alllist = GetDutyList();     
            }
           

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.teamname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DutyInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.teamname :
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
                c.routename,
                c.groupname,
                c.trainno,
                String.Format("{0:yyyy-MM-dd}", c.starttime),
                String.Format("{0:yyyy-MM-dd}", c.endtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteDuty(long[] items)
        {
            string delSql = "UPDATE tbl_duty SET deleted = 1 WHERE ";
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

        public tbl_duty GetDutyById(long uid)
        {
            return db.tbl_duties
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }
        //改变48小时限制
        public string changeforbid(long uid)
        {

            var rst = "";
            //获得原来权利
            tbl_duty duty = db.tbl_duties.Where(m => m.deleted == 0 && m.uid == uid)                   
                                      .FirstOrDefault();
            try
            {
                if (duty.forbide == 1)
                {
                    duty.forbide = 0;
                    db.SubmitChanges();

                }
                else if (duty.forbide == 0)
                {
                    duty.forbide = 1;
                    db.SubmitChanges();
                }
            }
            catch (System.Exception ex)
            {
                return ("修改权限出错");
            }

            return rst;
        }

        public string InsertDuty(string starttime, string endtime, long teamid, long routeid, long groupid, string trainno)
        {
            string rst = "";
            long zoneid = CommonModel.GetCurrZoneId();
            tbl_duty newuser = new tbl_duty();
            DateTime startd = new DateTime();
            DateTime endd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {
                return "出乘日期不能为空";
            }

            try
            {
                endd = DateTime.Parse(endtime).AddHours(23);
            }
            catch (System.Exception ex)
            {
                return "退乘日期不能为空";
            }

            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                newuser.starttime = startd;
                newuser.endtime = endd;
                newuser.teamid = teamid;
                newuser.routeid = routeid;
                newuser.groupid = groupid;
                newuser.trainno = trainno;
                newuser.createtime = DateTime.Now;
                newuser.forbide = 1;
                db.tbl_duties.InsertOnSubmit(newuser);
                db.SubmitChanges();

                var groupcrewlist = userModel.GetGroupCrewList(groupid);
                List<tbl_dutycrew> crewlist = new List<tbl_dutycrew>();
                foreach (var item in groupcrewlist)
                {
                    tbl_dutycrew newitem = new tbl_dutycrew();
                    newitem.dutyid = newuser.uid;
                    newitem.crewid = item.uid;
                    crewlist.Add(newitem);
                }
                db.tbl_dutycrews.InsertAllOnSubmit(crewlist);
                db.SubmitChanges();

                db.Transaction.Commit();
            }
            catch (System.Exception ex)
            {
                db.Transaction.Rollback();
                rst = ex.ToString();
            }

            return rst;
        }

        public string UpdateDuty(long uid, string starttime, string endtime, long teamid, long routeid, long groupid, string trainno)
        {
            string rst = "";
            tbl_duty edititem = GetDutyById(uid);
            DateTime startd = new DateTime();
            DateTime endd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {
                return "出乘日期不能为空";
            }

            try
            {
                endd = DateTime.Parse(endtime);
            }
            catch (System.Exception ex)
            {
                return "退乘日期不能为空";
            }

            if (edititem != null)
            {
                edititem.starttime = startd;
                edititem.endtime = endd;
                edititem.teamid = teamid;
                edititem.routeid = routeid;
                edititem.groupid = groupid;
                edititem.trainno = trainno;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "此出乘不存在";
            }

            return rst;
        }


        public List<DutyInfo> Serch(long groupid, long teamid, String starttime, String endtime)
        {
            var rst = db.tbl_duties
                .Where(m=>m.deleted==0)
                       // .Where(m => m.groupid == groupid && m.teamid == teamid && m.starttime >= starttime && m.starttime <= endtime)
                        .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { duty = m, team = l })
                        .Join(db.tbl_traingroups, m => m.duty.groupid, l => l.uid, (m, l) => new { team = m, group = l })
                        .Join(db.tbl_railroutes, m => m.team.duty.routeid, l => l.uid, (m, l) => new { group = m, route = l })
                        .OrderByDescending(m=>m.group.team.duty.starttime)
                        .Select(m => new DutyInfo
                        {
                            uid= m.group.team.duty.uid,
                            teamname = m.group.team.team.teamname,
                            groupname = m.group.group.groupname,
                            routename=m.route.routename,
                            trainno = m.group.team.duty.trainno,
                            starttime = m.group.team.duty.starttime,
                            endtime = m.group.team.duty.endtime,
                            groupid=m.group.group.uid,
                            teamid=m.group.team.team.uid,
                        })
                        .ToList();
            if (groupid!=0)
            {
                rst=rst.Where(m => m.groupid == groupid).ToList();
            }
            if (teamid != 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }
            if (starttime != null && starttime!="")
            {
                var time = DateTime.Parse(starttime);
                rst = rst.Where(m=>m.starttime >= time).ToList();
            }

            if (endtime != null && endtime != "")
            {
                var time = DateTime.Parse(endtime);
                rst = rst.Where(m => m.starttime <= time).ToList();
            }
            return rst;

        }

        public List<DutyInfo> Serch2(String starttime, String endtime,long uid)
        {
            var rst = db.tbl_dutycrews.Where(m => m.deleted == 0)
                                      .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { duty = m, user = l })
                                      .Join(db.tbl_crewroles, m => m.user.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                                      .Join(db.tbl_duties, m => m.user.duty.dutyid, l => l.uid, (m, l) => new { user = m, duty = l })
                                      .Join(db.tbl_railteams, m => m.duty.teamid, l => l.uid, (m, l) => new { duty = m, team = l })
                                      .Join(db.tbl_traingroups, m => m.duty.duty.groupid, l => l.uid, (m, l) => new { team = m, group = l })
                                      .Join(db.tbl_railroutes, m => m.team.duty.duty.routeid, l => l.uid, (m, l) => new { group = m, route = l })
                                      .Select(m => new DutyInfo
                                      {
                                          uid=m.group.team.duty.user.user.user.uid,
                                          teamid=m.group.team.team.uid,
                                          groupid = m.group.group.uid,
                                          teamname = m.group.team.team.teamname,
                                          groupname = m.group.group.groupname,
                                          crewno = m.group.team.duty.user.user.user.crewno,
                                          name = m.group.team.duty.user.user.user.realname,
                                          rolename = m.group.team.duty.user.role.rolename,
                                          trainno = m.group.team.duty.duty.trainno,
                                          starttime = m.group.team.duty.duty.starttime,
                                          endtime = m.group.team.duty.duty.endtime,
                                      }

                                      ).ToList();


            //var rst = db.tbl_duties
            //    // .Where(m => m.groupid == groupid && m.teamid == teamid && m.starttime >= starttime && m.starttime <= endtime)
            //            .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { duty = m, team = l })
            //            .Join(db.tbl_traingroups, m => m.duty.groupid, l => l.uid, (m, l) => new { team = m, group = l })
            //            .Join(db.tbl_railroutes, m => m.team.duty.routeid, l => l.uid, (m, l) => new { group = m, route = l })
            //            .Select(m => new DutyInfo
            //            {
            //                uid = m.group.team.duty.uid,
            //                teamname = m.group.team.team.teamname,
            //                groupname = m.group.group.groupname,
            //                routename = m.route.routename,
            //                trainno = m.group.team.duty.trainno,
            //                starttime = m.group.team.duty.starttime,
            //                endtime = m.group.team.duty.endtime,
            //                groupid = m.group.group.uid,
            //                teamid = m.group.team.team.uid,
            //            })
            //            .ToList();

           
            if (starttime != null && starttime != "")
            {
                var time = DateTime.Parse(starttime);
                rst = rst.Where(m => m.starttime >= time).ToList();
            }

            if (endtime != null && endtime != "")
            {
                var time = DateTime.Parse(endtime);
                rst = rst.Where(m => m.starttime <= time).ToList();
            }
            if (uid!=0)
            {
                rst = rst.Where(m => m.uid==uid).ToList();

            }
            return rst;

        }
        #endregion
    }
}