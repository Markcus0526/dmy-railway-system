using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;


/*
 * @auther easi
 * @version 1.0
 * 功能，查询积分
 * 
 */

namespace TLSite.Controllers
{
    public class CreditController : Controller
    {
        CreditModel creditModel = new CreditModel();
        SelfCheckModel scm = new SelfCheckModel();
        CombineSelfCheckModel ccm = new CombineSelfCheckModel();
        //
        // GET: /Credit/


        //积分
        [Authorize]//需要登陆
        public ActionResult CheckCredit()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["level1nav"] = "Credit";
            ViewData["level2nav"] = "CheckCredit";
          
            ViewData["rootUri"] = rootUri;

            //年月日显示
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy年MM月");
            ViewData["CurrentComputerDate"] = DateTime.Now.ToString("yyyy/MM/dd");
            //下为select元素用
            //线路列表
            RouteModel routeModel = new RouteModel();
            var routelist = routeModel.GetRouteList();
            ViewData["routelist"] = routelist;

            //部门列表
            SectorModel sectormodel = new SectorModel();
            ViewData["Sectorlist"] = sectormodel.GetTeamSectorList();
            return View();
        }
        [AjaxOnly]
        public ActionResult RefreshTable(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            var date = Request.QueryString["date"].ToString();

            // if (date != null && date.Equals("")==false)

            int year = DateTime.Parse(date).Year;
            int month = DateTime.Parse(date).Month;

            var name = Request.QueryString["name"].ToString();
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
           // var routeid = long.Parse(Request.QueryString["routeid"].ToString());
            var groupid = long.Parse(Request.QueryString["groupid"].ToString());

            //返回datatale需要的数据
            CreditModel cm = new CreditModel();
            var GetCredInfo = cm.GetCredInfoByCondition(sectorid,groupid, year, month, name);
            IEnumerable<UserDetailInfo> filteredCompanies;

            var isCrewNoSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isTypeOneSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var isTypeTwoSortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isTypeThreeSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var isTypeFourSortable = Convert.ToBoolean(Request["bSortable_8"]);
            var isTypeFiveSortable = Convert.ToBoolean(Request["bSortable_9"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<UserDetailInfo, double> orderingFunction = (c => 
                                                                  sortColumnIndex == 5 && isTypeOneSortable ? c.typeone :
                                                                  sortColumnIndex == 6 && isTypeTwoSortable ? c.typetwo :
                                                                  sortColumnIndex == 7 && isTypeThreeSortable ? c.typethree :
                                                                  sortColumnIndex == 8 && isTypeFourSortable ? c.typefour :
                                                                  sortColumnIndex == 9 && isTypeFiveSortable ? c.typefive :
                                                                  sortColumnIndex == 10 && isTypeFiveSortable ? c.typetwo+c.typethree+c.typefour+c.typefive :
                                                                  sortColumnIndex == 11 && isTypeFiveSortable ? c.addcredit :
                                                                  sortColumnIndex == 12 && isTypeFiveSortable ? cm.conttotalCredit(c.uid, month, year) :

                                                           0);

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = GetCredInfo.OrderBy(orderingFunction);
            else
                filteredCompanies = GetCredInfo.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }



            var result = from c in displayedCompanies
                         select new[] { c.teamname,
                                        c.crewno,            
                                        c.realname, 
                                        c.crewgroupname,
                                        c.crewrole,                                      
                                        Convert.ToString(c.typeone),
                                        Convert.ToString(c.typetwo),
                                        Convert.ToString(c.typethree),
                                        Convert.ToString(c.typefour),
                                        Convert.ToString(c.typefive),                                      
                                        Convert.ToString(c.typetwo+c.typethree+c.typefour+c.typefive),
                                        Convert.ToString(c.addcredit),                                      
                                        //  激励积分为double，考虑到会有小数
                                       Convert.ToString( cm.conttotalCredit(c.uid,month,year) ),
                                         Convert.ToString(c.uid)
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = GetCredInfo.Count(),
                iTotalDisplayRecords = GetCredInfo.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }




        //自控率查询
        [Authorize]
        public ActionResult SelfCheck()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["level1nav"] = "Credit";
            ViewData["level2nav"] = "SelfCheck";
            ViewData["rootUri"] = rootUri;

            SectorModel sectormodel = new SectorModel();
            ViewData["Sectorlist"] = sectormodel.GetTeamSectorList();
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy年MM月");
            ViewData["CurrentComputerDate"] = DateTime.Now.ToString();
            return View();
        }

        [AjaxOnly]

        public ActionResult RefreshSelfCheck(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;



            var date = Request.QueryString["date"].ToString();
            // if (date != null && date.Equals("")==false)
            int year = DateTime.Parse(date).Year;
            int month = DateTime.Parse(date).Month;
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());




            SelfCheckModel sf = new SelfCheckModel();

            var rst = sf.selfcheck(year, month, sectorid);
            var displayedCompanies = rst.Skip(param.iDisplayStart);


            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.teamname,
                               c.groupname,
                              Convert.ToString(c.one),
                              Convert.ToString(c.two),
                              Convert.ToString(c.three),
                              Convert.ToString(c.four),
                              Convert.ToString(c.five),
                              Convert.ToString(c.six),
                              Convert.ToString(c.seven),
                              Convert.ToString(c.eight),
                              
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            },
                 JsonRequestBehavior.AllowGet);
        }



        //结合部问题查询

        [Authorize]
        public ActionResult CombineCheck()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Credit";
            ViewData["level2nav"] = "SelfCheck";

            SectorModel sectormodel = new SectorModel();
            ViewData["Sectorlist"] = sectormodel.GetTeamSectorList();
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy年MM月");
            ViewData["CurrentComputerDate"] = DateTime.Now.ToString();
            return View();
        }

        [AjaxOnly]
        public ActionResult RefreshCombineCheck(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;


            int year = 0;
            int month = 0;

            var date = Request.QueryString["date"].ToString();


            if (date != null && date.Equals("") == false)
            {
                year = DateTime.Parse(date).Year;
                month = DateTime.Parse(date).Month;
            }

            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());

         

            var rst = ccm.CombineCheck(year, month, sectorid);

            var displayedCompanies = rst.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            var result = from c in displayedCompanies
                         select new[]{
                               c.teamname,
                               c.groupname,
                              Convert.ToString(c.one),
                              Convert.ToString(c.two),
                              Convert.ToString(c.three),
                              
                           };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = rst.Count(),
                iTotalDisplayRecords = rst.Count(),
                aaData = result
            },
                 JsonRequestBehavior.AllowGet);
        }



        //对话框用
        [AjaxOnly]
        public ActionResult GetAlertcontent(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            var uid = long.Parse(Request.QueryString["uid"].ToString());
            var totlechkpoint = Request.QueryString["totlechkpoint"].ToString();
            var totleaddpoint = Request.QueryString["totleaddpoint"].ToString();

            var date = Request.QueryString["date"].ToString();
            int year =2014 ;
            int month =10 ;
            try
            {
                 year = DateTime.Parse(date).Year;
                 month = DateTime.Parse(date).Month;
            }
            catch (System.Exception ex)
            {
            	
            }
//             var month = int.Parse(Request.QueryString["month"].ToString());
//             var year = int.Parse(Request.QueryString["year"].ToString());

            CreditModel cm = new CreditModel();
            var rst = cm.getUserCreditByUid(uid, month, year);

            List<String[]> test = new List<String[]>();
            foreach (var c in rst)
            {
                string[] aa = new string[]{  c.teamname,
                               c.crewno,
                               c.name,
                               c.groupname,
                               c.crewrolename,
                               c.checkno,
                                Convert.ToString(c.chkpoint),
                                Convert.ToString(c.addpoint),
                               c.content,
                               c.checkexecparentname,
                               c.checkername,
                               c.checklevel,
                               c.checktime.ToLongDateString().ToString(),                               
                               Convert.ToString(c.month)+"月" };
                test.Add(aa);
            }
            string[] total = new string[] { "本月积分合计", "", "", "", "", "", totlechkpoint, totleaddpoint, "", "", "", "", "", "" };
            test.Add(total);


            //var result = from c in rst
            //             select new[]{
            //                    c.teamname,
            //                    c.crewno,
            //                    c.name,
            //                    c.groupname,
            //                    c.crewrolename,
            //                     Convert.ToString(c.checkinfoid),
            //                     Convert.ToString(c.chkpoint),
            //                     Convert.ToString(c.addpoint),
            //                    c.content,
            //                    c.checkexecparentname,
            //                    c.checkername,
            //                    c.checklevel,
            //                    c.checktime.ToLongDateString().ToString(),                               
            //                    Convert.ToString(c.month),
            //                };



            //list<string[]> aaa = new list<string[]>();
            //string[] a1 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" };
            //string[] a2 = new string[] { "2", "3", "3", "4", "5", "6", "8", "9", "10", "11", "12", "13", "14" };
            //aaa.add(a1);
            //aaa.add(a2);



            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = test.Count(),
                iTotalDisplayRecords = test.Count(),
                aaData = test
            },
                 JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        //导出积分表
        public void ExportCreditList()
        {
            var date = Request.QueryString["date"].ToString();

            // if (date != null && date.Equals("")==false)

            int year = DateTime.Parse(date).Year;
            int month = DateTime.Parse(date).Month;

            var name = Request.QueryString["name"].ToString();
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());
           // var routeid = long.Parse(Request.QueryString["routeid"].ToString());
            var groupid = long.Parse(Request.QueryString["groupid"].ToString());

            var datalist = creditModel.ExportCreditList(sectorid,groupid, year, month, name);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=积分查询表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        [Authorize]
        //导出积分明细
        public void ExportCreditDetilList()
        {

            var date = Request.QueryString["date"].ToString();
            int year = 2014;
            int month = 10;
            try
            {
                year = DateTime.Parse(date).Year;
                month = DateTime.Parse(date).Month;
            }
            catch (System.Exception ex)
            {

            }
            var uid = long.Parse(Request.QueryString["uid"].ToString());
            var totlechkpoint = Request.QueryString["totlechkpoint"].ToString();
            var totleaddpoint = Request.QueryString["totleaddpoint"].ToString();

            var datalist = creditModel.ExportCreditDetilList(uid, month, year, totlechkpoint, totleaddpoint);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=个人积分明细" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }




        [Authorize]
        //导出‘两违’自控率
        public void ExportSelfCheckList()
        {

            var date = Request.QueryString["date"].ToString();
            int year = 2014;
            int month = 10;
            try
            {
                year = DateTime.Parse(date).Year;
                month = DateTime.Parse(date).Month;
            }
            catch (System.Exception ex)
            {

            }
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());

            //datatable
            var datatable = scm.ExportSelfCheckList(month, year, sectorid);
            var grid = new GridView();
            grid.DataSource = datatable;
            grid.DataBind();
            //grid.Columns[0].HeaderStyle.
            grid.Rows[0].Cells[0].RowSpan = 2;
            grid.Rows[0].Cells[1].RowSpan = 2;
            grid.Rows[0].Cells[2].ColumnSpan = 2;
            grid.Rows[0].Cells[3].ColumnSpan = 2;
            grid.Rows[0].Cells[4].ColumnSpan = 2;
            grid.Rows[0].Cells[5].ColumnSpan = 2;
            grid.Rows[0].Cells[9].Visible = false;
            grid.Rows[0].Cells[8].Visible = false;
            grid.Rows[0].Cells[6].Visible = false;
            grid.Rows[0].Cells[7].Visible = false;
            grid.Rows[1].Cells[8].Visible = false;
            grid.Rows[1].Cells[9].Visible = false;

//             grid.Rows[0].Cells[11].Visible = false;
//             grid.Rows[0].Cells[12].Visible = false;
//             grid.Rows[0].Cells[13].Visible = false;
            
           // grid.Dele
                
            

            string fileName = "attachment; filename=两违自控率表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        [Authorize]

        //导出结合部问题自控率
        public void ExportCombineCheckList()
        {

            var date = Request.QueryString["date"].ToString();
            // if (date != null && date.Equals("")==false)
            int year = DateTime.Parse(date).Year;
            int month = DateTime.Parse(date).Month;
            var sectorid = long.Parse(Request.QueryString["sectorid"].ToString());

            var datalist = ccm.ExportcombineCheckList(month, year,sectorid);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            string fileName = "attachment; filename=结合部自控率表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Response.ContentEncoding = Encoding.UTF8;

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }



}

