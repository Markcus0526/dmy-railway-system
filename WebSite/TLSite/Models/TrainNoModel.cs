using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class TrainNoInfo
    {
        public long uid { get; set; }
        public long routeid { get; set; }
        public string routename { get; set; }
        public string trainno { get; set; }
        public int sortid { get; set; }
        public DateTime createtime { get; set; }
    }

    public class TrainNoModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 线路
        public List<TrainNoInfo> GetTrainNoList()
        {
            return db.tbl_trainnos
                .Where(m => m.deleted == 0)
                .Join(db.tbl_railroutes, m => m.routeid, l => l.uid, (m, l) => new { no = m, route = l })
                .OrderBy(m => m.no.sortid)
                .Select(m => new TrainNoInfo
                {
                    uid = m.no.uid,
                    routeid = m.route.uid,
                    routename = m.route.routename,
                    trainno = m.no.trainno,
                    sortid = m.no.sortid,
                    createtime = m.no.createtime
                })
                .ToList();
        }

        public List<TrainNoInfo> GetTrainNoListOfRoute(long routeid)
        {
            return db.tbl_trainnos
                .Where(m => m.deleted == 0 && m.routeid == routeid)
                .Join(db.tbl_railroutes, m => m.routeid, l => l.uid, (m, l) => new { no = m, route = l })
                .OrderBy(m => m.no.sortid)
                .Select(m => new TrainNoInfo
                {
                    uid = m.no.uid,
                    routeid = m.route.uid,
                    routename = m.route.routename,
                    trainno = m.no.trainno,
                    sortid = m.no.sortid,
                    createtime = m.no.createtime
                })
                .ToList();
        }

        public JqDataTableInfo GetTrainNoDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<TrainNoInfo> filteredCompanies;

            List<TrainNoInfo> alllist = GetTrainNoList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.trainno.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<TrainNoInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.routename :
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
                c.routename,
                c.trainno,
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

        public bool DeleteTrainNo(long[] items)
        {
            string delSql = "UPDATE tbl_trainno SET deleted = 1 WHERE ";
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

        public tbl_trainno GetTrainNoById(long uid)
        {
            return db.tbl_trainnos
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public string InsertTrainNo(long routeid, string trainno, int sortid)
        {
            tbl_trainno newuser = new tbl_trainno();

            newuser.routeid = routeid;
            newuser.trainno = trainno;
            newuser.sortid = sortid;
            newuser.createtime = DateTime.Now;

            db.tbl_trainnos.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateTrainNo(long uid, long routeid, string trainno, int sortid)
        {
            string rst = "";
            tbl_trainno edititem = GetTrainNoById(uid);

            if (edititem != null)
            {
                edititem.routeid = routeid;
                edititem.trainno = trainno;
                edititem.sortid = sortid;

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "此班组不存在";
            }

            return rst;
        }

        public bool CheckDuplicateName(string trainno, long uid)
        {
            bool rst = true;

            rst = ((from m in db.tbl_trainnos
                    where m.deleted == 0 && m.trainno == trainno && m.uid != uid
                    select m).FirstOrDefault() == null);

            return rst;
        }
        #endregion
    }
}