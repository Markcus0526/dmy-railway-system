using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Collections;

namespace TLSite.Models
{
    public class TeamModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 车队
        public List<tbl_railteam> GetTeamList()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railteams
                .Where(m => m.deleted == 0 && m.zoneid == zoneid)
                .OrderBy(m => m.sortid)
                .ToList();

        }

        public ArrayList GetTeamArrayList()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            var list= db.tbl_railteams
                .Where(m => m.deleted == 0 && m.zoneid == zoneid)
                .OrderBy(m => m.sortid)
                .ToList();
            ArrayList alist=new ArrayList();
            foreach (var l in list){
                alist.Add(l);
            }
            return alist;
        }
        public List<tbl_railteam> GetJudgeTeamList(string starttime)
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
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime)
                    .ToList();

                var teamids = dutylist.Select(m => m.teamid).ToList();
                var teamlist = db.tbl_railteams
                    .Where(m => m.deleted == 0 && teamids.Contains(m.uid))
                    .ToList();

                return teamlist;
            }
            catch (System.Exception ex)
            {
            	
            }

            return null;
        }

        public JqDataTableInfo GetTeamDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_railteam> filteredCompanies;

            List<tbl_railteam> alllist = GetTeamList();

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
            Func<tbl_railteam, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.teamname :
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

        public bool DeleteTeam(long[] items)
        {
            string delSql = "UPDATE tbl_railteam SET deleted = 1 WHERE ";
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

        public tbl_railteam GetTeamById(long uid)
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railteams
                .Where(m => m.deleted == 0 && m.uid == uid && m.zoneid == zoneid)
                .FirstOrDefault();
        }

        public string InsertTeam(string teamname, int sortid)
        {
            long zoneid = CommonModel.GetCurrZoneId();
            tbl_railteam newuser = new tbl_railteam();

            newuser.zoneid = zoneid;
            newuser.teamname = teamname;
            newuser.sortid = sortid;
            newuser.createtime = DateTime.Now;

            db.tbl_railteams.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateTeam(long uid, string teamname, int sortid)
        {
            string rst = "";
            tbl_railteam edititem = GetTeamById(uid);

            if (edititem != null)
            {
                edititem.teamname = teamname;
                edititem.sortid = sortid;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "此标签不存在";
            }

            return rst;
        }

        public bool CheckDuplicateName(string teamname, long uid)
        {
            bool rst = true;
            long zoneid = CommonModel.GetCurrZoneId();

            rst = ((from m in db.tbl_railteams
                    where m.deleted == 0 && m.teamname == teamname && m.uid != uid && m.zoneid == zoneid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public long GetTeamIdFromUserId(long userid)
        {
            long rst = 0;
            try
            {
                rst = db.tbl_users.Where(m => m.deleted == 0 && m.uid == userid)
                     .Join(db.tbl_traingroups, m => m.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                     .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { user = m, team = l })
                     .Select(m => m.team.uid)
                     .FirstOrDefault();
            }
            catch (System.Exception ex)
            {
            	
            }

            return rst;
        }

        public long GetExecparentIdFromUserId(long userid)
        {
            long rst = 0;
            try
            {
                rst = db.tbl_users.Where(m => m.deleted == 0 && m.uid == userid)
                     .Join(db.tbl_railteams, m => m.execparentid, l => l.uid, (m, l) => new { user = m, team = l })
                     .Select(m => m.team.uid)
                     .FirstOrDefault();
            }
            catch (System.Exception ex)
            {

            }

            return rst;
        }

        #endregion
    }
}