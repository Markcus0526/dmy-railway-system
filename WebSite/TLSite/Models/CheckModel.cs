using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.IO;
using System.Data.OleDb;
using System.Data;





namespace TLSite.Models
{
    public class CheckModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        #region 项点
        public List<tbl_checkinfo> GetCheckInfoList()
        {
            return db.tbl_checkinfos
                .Where(m => m.deleted == 0)
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public JqDataTableInfo GetCheckInfoDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_checkinfo> filteredCompanies;

            List<tbl_checkinfo> alllist = GetCheckInfoList().Where(m=>m.checktype==1).ToList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => c.checkno.ToLower().Contains(param.sSearch.ToLower()) ||
                            c.category.ToLower().Contains(param.sSearch.ToLower()) ||
                            c.chkpoint.ToString().Contains(param.sSearch.ToLower()) ||
                            ((!String.IsNullOrWhiteSpace(c.relpoint)) ? c.relpoint.ToLower().Contains(param.sSearch.ToLower()) : false) ||
                            c.checkinfo.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_checkinfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.checkno :
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
                c.category,
                c.checkno,
                c.checktype.ToString(),
                c.chkpoint.ToString(),
                c.relpoint,
                c.checkinfo,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetAddscoreList(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_checkinfo> filteredCompanies;

            List<tbl_checkinfo> alllist = GetCheckInfoList().Where(m=>m.checktype==0).ToList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => c.checkno.ToLower().Contains(param.sSearch.ToLower()) ||
                            c.category.ToLower().Contains(param.sSearch.ToLower()) ||
                            c.chkpoint.ToString().Contains(param.sSearch.ToLower()) ||
                            ((!String.IsNullOrWhiteSpace(c.relpoint)) ? c.relpoint.ToLower().Contains(param.sSearch.ToLower()) : false) ||
                            c.checkinfo.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_checkinfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.checkno :
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
                c.category,
                c.checkno,
                c.checktype.ToString(),
                c.chkpoint.ToString(),
                c.relpoint,
                c.checkinfo,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteCheckInfo(long[] items)
        {
            string delSql = "UPDATE tbl_checkinfo SET deleted = 1 WHERE ";
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

        public tbl_checkinfo GetCheckInfoById(long uid)
        {
            return db.tbl_checkinfos
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public string InsertCheckInfo(string category, string checkno, int checktype, int chkpoint, string relpoint, string checkinfo, int sortid)
        {
            tbl_checkinfo newuser = new tbl_checkinfo();

            newuser.category = category;
            newuser.checkno = checkno;
            newuser.checktype = checktype;
            newuser.chkpoint = chkpoint;
            newuser.relpoint = relpoint;
            newuser.checkinfo = checkinfo;
            newuser.sortid = sortid;

            db.tbl_checkinfos.InsertOnSubmit(newuser);

            db.SubmitChanges();

            return "";
        }

        public string UpdateCheckInfo(long uid, string category, string checkno, int checktype, int chkpoint, string relpoint, string checkinfo, int sortid)
        {
            string rst = "";
            tbl_checkinfo edititem = GetCheckInfoById(uid);

            if (edititem != null)
            {
                edititem.category = category;
                edititem.checkno = checkno;
                edititem.checktype = checktype;
                edititem.chkpoint = chkpoint;
                edititem.relpoint = relpoint;
                edititem.checkinfo = checkinfo;
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
        #endregion

        //导出项点信息
        public System.Data.DataTable ExportCheckInfo()
        {
            var products = new System.Data.DataTable("项点表");


            var alllist = GetCheckInfoList();

            if (products != null)
            {
                products.Columns.Add("分类", typeof(string));
                products.Columns.Add("项点编号", typeof(string));
                products.Columns.Add("项点种类", typeof(string));
                products.Columns.Add("扣分", typeof(string));
                products.Columns.Add("联挂扣分", typeof(string));
                products.Columns.Add("项点内容", typeof(string));
               // products.Columns.Add("排序", typeof(string));
                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(
                        item.category,
                        item.checkno,
                        item.checktype==1?"扣分":"加分",
                        item.chkpoint.ToString(),
                        item.relpoint,
                        item.checkinfo
                        );
                    i++;
                }
            }

            return products;
        }

        OleDbConnection oledbConn;
        public string ImportCheckData(string filepath)
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
                List<tbl_checkinfo> checkinfolist = new List<tbl_checkinfo>();

                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        ind++;
                        continue;
                    }

                    tbl_checkinfo newitem = new tbl_checkinfo();

                    newitem.checktype = reader.GetValue(2).ToString()=="扣分"?1:0;
                    newitem.checkno = reader.GetValue(1).ToString();
                    newitem.chkpoint = int.Parse(reader.GetValue(3).ToString());
                    newitem.relpoint = reader.GetValue(4).ToString();
                    newitem.category = reader.GetValue(1).ToString();
                    newitem.checkinfo = reader.GetValue(5).ToString();
                    newitem.sortid = int.Parse(reader.GetValue(6).ToString());

                    checkinfolist.Add(newitem);

                    ind++;
                }
                reader.Close();

                db.tbl_checkinfos.InsertAllOnSubmit(checkinfolist);
                db.SubmitChanges();
                rst = "";
            }

            catch (Exception ex)
            {
                rst = ex.ToString();
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }
    }
}