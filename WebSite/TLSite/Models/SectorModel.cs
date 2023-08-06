using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Collections;

namespace TLSite.Models
{
    public class SectorModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 部门


        public List<tbl_railsector> GetSectorList()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railsectors
                .Where(m => m.deleted == 0 && m.zoneid == zoneid).OrderBy(m => m.sortid)
                .ToList();
        }
        public ArrayList GetSectorListArray()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            var list= db.tbl_railsectors
                .Where(m => m.deleted == 0 && m.zoneid == zoneid).OrderBy(m => m.sortid).ToList();
            ArrayList alist = new ArrayList();
            foreach  (var l in list)
            {
                alist.Add(l);
            }
            return alist;
        }

        public List<tbl_railteam> GetTeamSectorList()
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railteams
                .Where(m => m.deleted == 0 && m.zoneid == zoneid).OrderBy(m => m.sortid)
                .ToList();
        }

        public JqDataTableInfo GetSectorDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_railsector> filteredCompanies;

            List<tbl_railsector> alllist = GetSectorList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.sectorname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_railsector, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sectorname :
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
                c.sectorname,
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

        public bool DeleteSector(long[] items)
        {
            string delSql = "UPDATE tbl_railsector SET deleted = 1 WHERE ";
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

        public tbl_railsector GetSectorById(long uid)
        {
            long zoneid = CommonModel.GetCurrZoneId();

            return db.tbl_railsectors
                .Where(m => m.deleted == 0 && m.uid == uid && m.zoneid == zoneid)
                .FirstOrDefault();
        }

        public string InsertSector(string sectorname, int sortid)
        {
            long zoneid = CommonModel.GetCurrZoneId();
            tbl_railsector newuser = new tbl_railsector();

            newuser.zoneid = zoneid;
            newuser.sectorname = sectorname;
            newuser.sortid = sortid;
            newuser.createtime = DateTime.Now;

            db.tbl_railsectors.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateSector(long uid, string sectorname, int sortid)
        {
            string rst = "";
            tbl_railsector edititem = GetSectorById(uid);

            if (edititem != null)
            {
                edititem.sectorname = sectorname;
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

        public bool CheckDuplicateName(string sectorname, long uid)
        {
            bool rst = true;
            long zoneid = CommonModel.GetCurrZoneId();

            rst = ((from m in db.tbl_railsectors
                    where m.deleted == 0 && m.sectorname == sectorname && m.uid != uid && m.zoneid == zoneid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        

        #endregion

    }
}