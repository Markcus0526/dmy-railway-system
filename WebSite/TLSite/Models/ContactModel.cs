using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Data;

namespace TLSite.Models
{
    public class ContactInfo
    {
        public long uid { get; set; }
        public string contactkind { get; set; }
        public string partname { get; set; }
        public string name { get; set; }
        public string groupname { get; set; }
        public string trainno { get; set; }
        public string phonenum1 { get; set; }
        public string phonenum2 { get; set; }
        public string shortpnum1 { get; set; }
        public string shortpnum2 { get; set; }
        public string note { get; set; }
        public string rolename { get; set; }
        public string rolekind { get; set; }
        public string linenum { get; set; }
    }

    public class ContactModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        public List<ContactInfo> GetContactList()
        {
            var rst = db.tbl_contacts
                .Where(m => m.deleted == 0)
                .Select(m => new ContactInfo
                {
                    uid = m.uid,
                    contactkind = m.contactkind,
                    partname = m.partname,
                    name = m.contactname,
                    groupname = m.groupname,
                    trainno = m.trainno,
                    phonenum1 = m.phonenum1,
                    phonenum2 = m.phonenum2,
                    shortpnum1 = m.shortpnum1,
                    shortpnum2 = m.shortpnum2,
                    note = m.note,
                    rolekind = m.rolekind,
                    rolename = m.rolename,
                    linenum = m.linenum
                })
                .ToList();

            return rst;
        }

        public string InsertContact(string name, string contactkind, string partname, 
            string groupname, string trainno, string phonenum1, 
            string phonenum2, string shortnum1, string shortnum2, string note,
            string rolename, string rolekind, string linenum)
        {
            string rst = "";

            try
            {
                tbl_contact newcontact = new tbl_contact();

                newcontact.contactname = name;
                newcontact.contactkind = contactkind;
                newcontact.partname = partname;
                newcontact.groupname = groupname;
                newcontact.trainno = trainno;
                newcontact.phonenum1 = phonenum1;
                newcontact.phonenum2 = phonenum2;
                newcontact.shortpnum1 = shortnum1;
                newcontact.shortpnum2 = shortnum2;
                newcontact.note = note;
                newcontact.rolename = rolename;
                newcontact.rolekind = rolekind;
                newcontact.linenum = linenum;

                db.tbl_contacts.InsertOnSubmit(newcontact);
                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = "登记失败，请检查登陆信息！";
            }

            return rst;
        }

        public string UpdateContact(long uid, string name, string contactkind, string partname,
            string groupname, string trainno, string phonenum1,
            string phonenum2, string shortnum1, string shortnum2, string note,
            string rolename, string rolekind, string linenum)
        {
            string rst = "";

            try
            {
                tbl_contact editcontact = db.tbl_contacts.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();

                if (editcontact != null)
                {
                    editcontact.contactname = name;
                    editcontact.contactkind = contactkind;
                    editcontact.partname = partname;
                    editcontact.groupname = groupname;
                    editcontact.trainno = trainno;
                    editcontact.phonenum1 = phonenum1;
                    editcontact.phonenum2 = phonenum2;
                    editcontact.shortpnum1 = shortnum1;
                    editcontact.shortpnum2 = shortnum2;
                    editcontact.note = note;
                    editcontact.rolename = rolename;
                    editcontact.rolekind = rolekind;
                    editcontact.linenum = linenum;

                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {

            }

            return rst;
        }

        public tbl_contact GetContactInfo(long uid)
        {
            return db.tbl_contacts.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();
        }

        public JqDataTableInfo GetContactDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ContactInfo> filteredCompanies;

            List<ContactInfo> alllist = GetContactList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.name.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ContactInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.name :
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
                c.name,
                c.contactkind,
                c.partname,
                c.groupname,
                c.trainno,
                c.phonenum1 + ", " + c.phonenum2,
                c.shortpnum1 + ", " + c.shortpnum2,
                c.note,
                c.rolename,
                c.rolekind,
                c.linenum,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteContact(long[] items)
        {
            try
            {
                var dellist = db.tbl_contacts
                    .Where(m => items.Contains(m.uid))
                    .ToList();

                db.tbl_contacts.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        OleDbConnection oledbConn;
        public string ImportContactData(string filepath)
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
                List<tbl_contact> contactlist = new List<tbl_contact>();

                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        ind++;
                        continue;
                    }

                    tbl_contact newitem = new tbl_contact();

                    newitem.contactname = reader.GetValue(1).ToString();
                    newitem.contactkind = reader.GetValue(2).ToString();
                    newitem.partname = reader.GetValue(3).ToString();
                    newitem.groupname = reader.GetValue(4).ToString();
                    newitem.trainno = reader.GetValue(5).ToString();
                    newitem.phonenum1 = reader.GetValue(6).ToString();
                    newitem.phonenum2 = reader.GetValue(7).ToString();
                    newitem.shortpnum1 = reader.GetValue(8).ToString();
                    newitem.shortpnum2 = reader.GetValue(9).ToString();
                    newitem.rolename = reader.GetValue(10).ToString();
                    newitem.rolekind = reader.GetValue(11).ToString();
                    newitem.linenum = reader.GetValue(12).ToString();
                    newitem.note = reader.GetValue(13).ToString();

                    contactlist.Add(newitem);

                    ind++;
                }
                reader.Close();

                db.tbl_contacts.InsertAllOnSubmit(contactlist);
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

        public System.Data.DataTable ExportContactList()
        {
            var products = new System.Data.DataTable("通讯录");
            var alllist = GetContactList();

            if (products != null)
            {
                products.Columns.Add("序号", typeof(string));
                products.Columns.Add("类型", typeof(string));
                products.Columns.Add("部门名称", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("班组名称", typeof(string));
                products.Columns.Add("车次", typeof(string));
                products.Columns.Add("手机号1", typeof(string));
                products.Columns.Add("手机号2", typeof(string));
                products.Columns.Add("小号1", typeof(string));
                products.Columns.Add("小号2", typeof(string));
                products.Columns.Add("备注", typeof(string));
                products.Columns.Add("职务", typeof(string));
                products.Columns.Add("包保分工", typeof(string));
                products.Columns.Add("办公电话", typeof(string));

                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(
                        i.ToString(),
                        item.contactkind,
                        item.partname,
                        item.name,
                        item.groupname,
                        item.trainno,
                        item.phonenum1,
                        item.phonenum2,
                        item.shortpnum1,
                        item.shortpnum2,
                        item.note,
                        item.rolename,
                        item.rolekind,
                        item.linenum
                        );
                    i++;
                }
            }

            return products;
        }

    }
}