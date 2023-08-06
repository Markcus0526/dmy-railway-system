using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class RuleModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        public string InsertRule(string rulename, string imgurl, int sortid, string esccontent, string filename, string path, long filesize)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            try
            {
                tbl_rule newrule = new tbl_rule();

                newrule.title = rulename;
                newrule.sortid = sortid;
                newrule.contents = esccontent;
                newrule.pdfname = filename;
                newrule.pdfpath = path;
                newrule.pdfsize = filesize;

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                }

                newrule.imgurl = savepath + imgurl;

                db.tbl_rules.InsertOnSubmit(newrule);
                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
            	
            }

            return rst;
        }

        public string UpdateRule(long uid, string rulename, string imgurl, int sortid, string esccontent, string filename, string path, long filesize)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            try
            {
                tbl_rule editrule = db.tbl_rules.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();

                if (editrule != null)
                {
                    editrule.title = rulename;
                    editrule.sortid = sortid;
                    editrule.contents = esccontent;
                    if (!String.IsNullOrEmpty(filename))
                    {
                        editrule.pdfname = filename;
                        editrule.pdfpath = path;
                        editrule.pdfsize = filesize;
                    }
                }

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);

                    editrule.imgurl = savepath + imgurl;
                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {

            }

            return rst;
        }

        public List<tbl_rule> GetRuleList()
        {
            return db.tbl_rules.Where(m => m.deleted == 0).ToList();
        }

        public tbl_rule GetRuleInfo(long uid)
        {
            return db.tbl_rules.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();
        }

        public JqDataTableInfo GetRuleDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_rule> filteredCompanies;

            List<tbl_rule> alllist = GetRuleList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_rule, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                c.title,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteRule(long[] items)
        {
            try
            {
                var dellist = db.tbl_rules
                    .Where(m => items.Contains(m.uid))
                    .ToList();

                db.tbl_rules.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}