using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;


/*
 * @auther easi
 * @version 1.0
 * 功能:党内两违比率查询
 * 
 */


namespace TLSite.Controllers
{
    public class CreditOfPartyController : Controller
    {

        CreditOfPartyModel cop = new CreditOfPartyModel();
        //
        // GET: /CreditOfParty/


        [Authorize]
        public ActionResult CheckCreditOfParty()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["level1nav"] = "Credit";
            ViewData["level2nav"] = "checkcreditofparty";
            ViewData["rootUri"] = rootUri;
            //年月日显示
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy年MM月");
            ViewData["CurrentComputerDate"] = DateTime.Now.ToString("yyyy/MM/dd");

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


               int year = 0;
               int month = 0;
               var date = Request.QueryString["date"].ToString();

               if (date != null && date.Equals("") == false)
               {
                   year = DateTime.Parse(date).Year;
                   month = DateTime.Parse(date).Month;
               }
               else
               {
               }
               var checklevel = Request.QueryString["checklevel"].ToString();
               var uid = int.Parse(Request.QueryString["uid"].ToString());

               var policy = Request.QueryString["policy"].ToString();
               var teamid = long.Parse(Request.QueryString["teamid"].ToString());

               var rst = new List<CreditOfPartyModel.forStore>();
               rst = cop.partylist(year, month, uid, checklevel, teamid, policy);


               var displayedCompanies = rst.Skip(param.iDisplayStart);
               if (param.iDisplayLength > 0)
               {
                   displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
               }


               var result = from c in displayedCompanies
                            select new[]{
                              c.teamname,
                              c.crewno,
                              c.name,
                              c.groupname,
                              c.rolename,
                              c.polityface,
                              c.trainno,
                              c.checkinfono,
                              Convert.ToString(c.credit),
                              c.content,
                              c.checkerexecparent,
                              c.checkername,
                              Convert.ToString(month)+"月",              
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


            //得到用户名的列表
            [Authorize]
            [AjaxOnly]
            public JsonResult Getuserlist( )
            {

                var policyface=Request.QueryString["policy"].ToString();
                var id = int.Parse(Request.QueryString["teamid"].ToString());
                var usermodel = new UserModel();
                var rst = usermodel.GetUserListbyTeamandPolicy(policyface, id);
                return Json(rst, JsonRequestBehavior.AllowGet);
            }


            [Authorize]
            //导出积分表
            public void ExportCreditList()
            {
                var date = Request.QueryString["date"].ToString();

                // if (date != null && date.Equals("")==false)

                int year = DateTime.Parse(date).Year;
                int month = DateTime.Parse(date).Month;

                var checklevel = Request.QueryString["checklevel"].ToString();

                var uid = long.Parse(Request.QueryString["uid"].ToString());
                var policy = Request.QueryString["policy"].ToString();
                var teamid = long.Parse(Request.QueryString["teamid"].ToString());

                var datalist = cop.ExportCreditOfPartyList(year, month, uid, checklevel, teamid,policy);
                var grid = new GridView();

                grid.DataSource = datalist;
                grid.DataBind();

                string fileName = "attachment; filename=党内两违比率查询表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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
