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
    public class DocumentController : Controller
    {
        DocumentModel docModel = new DocumentModel();
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        UserModel userModel = new UserModel();

        [Authorize(Roles = "Document,Executive")]
        public ActionResult AddDocument()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            ViewData["level2nav"] = "AddDocument";
            ViewData["sectorlist"] = sectorModel.GetSectorList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["userlist"] = userModel.GetAllUserList();

            return View();
        }

        [Authorize]
        public ActionResult MyDocList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            int kind = (int)DocStatus.WAITSIGN;

            if (Request.QueryString["kind"] != null)
            {
                try
                {
                    kind = int.Parse(Request.QueryString["kind"].ToString());
                }
                catch (System.Exception ex)
                {
                	
                }
            }

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            if (kind == (int)DocStatus.WAITSIGN)
            {
                ViewData["level2nav"] = "MyDocList";
            }
            else if (kind == (int)DocStatus.ALREADYSIGN)
            {
                ViewData["level2nav"] = "MyRecvDocList";
            }

            ViewData["kind"] = kind;

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveMyDocList(int kind, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);

            if (Request.QueryString["starttime"] != null)
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null)
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            JqDataTableInfo rst = docModel.GetMyDocDataTable(starttime, endtime, kind, param, Request.QueryString, rootUri);
            
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Document,Executive")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitDocument(string title, string docno, string contents, string filename, string path, long filesize)
        {
            string rst = "";
            string receiver = "";

            try
            {
                receiver = Request.Form["receiver"].ToString();
            }
            catch (System.Exception ex)
            {

            }

            path = path.Trim(new char[] { '\"' });
            rst = docModel.InsertDocument(receiver, title, docno, contents, filename, path, filesize);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Detail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            ViewData["level2nav"] = "MyDocList";

            ViewData["sectorlist"] = sectorModel.GetSectorList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["userlist"] = userModel.GetAllUserList();

            var docinfo = docModel.GetMyDocDetailInfo(id);
            ViewData["docinfo"] = docinfo;
            ViewData["receiver"] = docModel.GetReceiverNameList(docinfo.receiver);
            ViewData["uid"] = docinfo.uid;

            var loglist = docModel.GetMyDocLogList(id);
            ViewData["loglist"] = loglist;

            var mylog = docModel.GetMyDocLogInfo(id);
            ViewData["mylog"] = mylog;

            return View();
        }

        [Authorize]
        public ActionResult SignDetail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            ViewData["level2nav"] = "MyDocList";

            ViewData["sectorlist"] = sectorModel.GetSectorList();
            ViewData["teamlist"] = teamModel.GetTeamList();
            ViewData["grouplist"] = groupModel.GetGroupList();
            ViewData["userlist"] = userModel.GetAllUserList();

            var docinfo = docModel.GetMyDocDetailInfo(id);
            ViewData["docinfo"] = docinfo;
            ViewData["receiverlist"] = docModel.GetSignDocReceiverList(docinfo);
            ViewData["uid"] = docinfo.uid;

            var loglist = docModel.GetMyDocLogList(id);
            ViewData["loglist"] = loglist;

            var mylog = docModel.GetMyDocLogInfo(id);
            ViewData["mylog"] = mylog;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitSignForward(long uid, string execnote, int flag)
        {
            string rst = "";
            string receiver = "";

            if (flag==1)
            {
                try
                {
                    receiver = Request.Form["receiver"].ToString();
                }
                catch (System.Exception ex)
                {
                    rst = "请选择流转人";
                    return Json(rst, JsonRequestBehavior.AllowGet);

                }
            }
            

            rst = docModel.SubmitDocSign(uid, execnote, flag, receiver);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Document,Executive")]
        public ActionResult DocSearch()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            ViewData["level2nav"] = "DocSearch";

            var teamlist = teamModel.GetTeamList();
            ViewData["teamlist"] = teamlist;

            var sectorlist = sectorModel.GetSectorList();
            ViewData["sectorlist"] = sectorlist;

            ViewData["grouplist"] = null;
            if (teamlist != null)
            {
                var grouplist = groupModel.GetGroupListOfTeam(teamlist[0].uid);
                ViewData["grouplist"] = grouplist;
            }

            string starttime = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["starttime"] = starttime;

            return View();
        }

        [Authorize(Roles = "Document,Executive")]
        [AjaxOnly]
        public JsonResult RetrieveSearchResult(long teamid, long sectorid, string starttime, string endtime, JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = docModel.GetSearchDocDataTable(teamid, sectorid, starttime, endtime, param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Document")]
        [HttpPost]
        public JsonResult DeleteDocument(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = docModel.DeleteDocument(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SentList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Document";
            ViewData["level2nav"] = "SentDocList";

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveSentDocList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);

            if (Request.QueryString["starttime"] != null)
            {
                try
                {
                    starttime = DateTime.Parse(Request.QueryString["starttime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            if (Request.QueryString["endtime"] != null)
            {
                try
                {
                    endtime = DateTime.Parse(Request.QueryString["endtime"].ToString());
                }
                catch (System.Exception ex)
                {

                }
            }

            JqDataTableInfo rst = docModel.GetSentDocDataTable(starttime, endtime, param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public void ExportDocument(int kind)
        {
            var datalist = docModel.ExportDocList(kind);
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=已收公文" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
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
        public void ExportSentDocument()
        {
            var datalist = docModel.ExportSentDocList();
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=已发公文" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
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
