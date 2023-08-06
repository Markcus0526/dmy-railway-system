using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace TLSite.Controllers
{
    public class CheckController : Controller
    {
        CheckModel checkModel = new CheckModel();

        [Authorize]
        public ActionResult CheckInfoList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "CheckInfo";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveCheckInfoList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = checkModel.GetCheckInfoDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveAddscorList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = checkModel.GetAddscoreList(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddCheckInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "CheckInfo";

            return View();
        }

        [Authorize]
        public ActionResult EditCheckInfo(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "CheckInfo";

            var checkinfo = checkModel.GetCheckInfoById(id);
            ViewData["checkinfo"] = checkinfo;
            ViewData["uid"] = checkinfo.uid;

            return View("AddCheckInfo");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCheckInfo(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = checkModel.DeleteCheckInfo(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitCheckInfo(long uid, string category, string checkno, int checktype, int chkpoint, string relpoint, string checkinfo, int sortid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = checkModel.InsertCheckInfo(category, checkno, checktype, chkpoint, relpoint, checkinfo, sortid);
            }
            else
            {
                rst = checkModel.UpdateCheckInfo(uid, category, checkno, checktype, chkpoint, relpoint, checkinfo, sortid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public void ExpotCheckInfo()
        {
          //  var datalist = combine.ExportCreditList(date, checklevel, teamid, groupid, crewname, receivepart);
            var datalist = checkModel.ExportCheckInfo();

            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();


            string fileName = "attachment; filename=项点信息表" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
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
        public ActionResult ImportCheck()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "CheckInfo";
            return View();
        }

        public JsonResult SubmitCheckImport(string fileurl)
        {
            string rst = "";
            if (fileurl == "" || string.IsNullOrWhiteSpace(fileurl))
            {
                rst = "请检测是否选择文件";
                return Json(rst, JsonRequestBehavior.AllowGet);

            }

            rst = checkModel.ImportCheckData(fileurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
