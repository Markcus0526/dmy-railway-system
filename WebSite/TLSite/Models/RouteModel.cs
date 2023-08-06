using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class RouteModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 线路
        public List<tbl_railroute> GetRouteList()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railroutes
                .Where(m => m.deleted == 0 && m.zoneid == zoneid)
                .OrderBy(m => m.sortid)
                .ToList();
        }
        public List<tbl_railroute> GetRouteListByTeamid(long teamid)
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railroutes
                .Where(m => m.deleted == 0 && m.zoneid == zoneid&&m.teamid==teamid)
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public JqDataTableInfo GetRouteDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_railroute> filteredCompanies;

            List<tbl_railroute> alllist = GetRouteList();
           // alllist.Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { route = m, team = l }).ToList();
                   
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.routename.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_railroute, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.routename :
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
                db.tbl_railteams.Where(m=>m.uid==c.teamid).Select(m=>m.teamname).FirstOrDefault(),
                c.routename,
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

        public bool DeleteRoute(long[] items)
        {
            string delSql = "UPDATE tbl_railroute SET deleted = 1 WHERE ";
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

        public tbl_railroute GetRouteById(long uid)
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railroutes
                .Where(m => m.deleted == 0 && m.uid == uid && m.zoneid == zoneid)
                .FirstOrDefault();
        }

        public string InsertRoute(string routename, int sortid,long teamid)
        {
            long zoneid = CommonModel.GetCurrZoneId();
            tbl_railroute newuser = new tbl_railroute();

            newuser.zoneid = zoneid;
            newuser.routename = routename;
            newuser.sortid = sortid;
            newuser.createtime = DateTime.Now;
            newuser.teamid = teamid;

            db.tbl_railroutes.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateRoute(long uid, string routename, int sortid, long teamid)
        {
            string rst = "";
            tbl_railroute edititem = GetRouteById(uid);

            if (edititem != null)
            {
                edititem.routename = routename;
                edititem.sortid = sortid;
                edititem.teamid = teamid;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "此标签不存在";
            }

            return rst;
        }

        public bool CheckDuplicateName(string routename, long uid)
        {
            bool rst = true;
            long zoneid = CommonModel.GetCurrZoneId();

            rst = ((from m in db.tbl_railroutes
                    where m.deleted == 0 && m.routename == routename && m.uid != uid && m.zoneid == zoneid
                    select m).FirstOrDefault() == null);

            return rst;
        }
        #endregion
    }
}