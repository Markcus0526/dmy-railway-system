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
    public class ContactController : Controller
    {
        ContactModel contactModel = new ContactModel();

        [Authorize]
        public ActionResult List()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";
            var contactlist = contactModel.GetContactList();
            ViewData["contactlist"] = contactlist;

            return View();
        }

        [Authorize]
        public ActionResult Detail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";

            return View();
        }

        [Authorize(Roles = "Contact")]
        public ActionResult ContactList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";

            return View();
        }

        [Authorize(Roles = "Contact")]
        [AjaxOnly]
        public JsonResult RetrieveContactList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = contactModel.GetContactDataTable(param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Contact")]
        public ActionResult AddContact()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";

            return View();
        }

        [Authorize(Roles = "Contact")]
        public ActionResult EditContact(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";

            var contactinfo = contactModel.GetContactInfo(id);
            ViewData["contactinfo"] = contactinfo;
            ViewData["uid"] = contactinfo.uid;

            return View("AddContact");
        }

        [Authorize(Roles = "Contact")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitContact(long uid, string name, string contactkind, string partname,
            string groupname, string trainno, string phonenum1,
            string phonenum2, string shortnum1, string shortnum2, string note,
            string rolename, string rolekind, string linenum)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = contactModel.InsertContact(name, contactkind, partname, groupname, trainno, phonenum1, phonenum2, shortnum1, shortnum2, note, rolename, rolekind, linenum);
            }
            else
            {
                rst = contactModel.UpdateContact(uid, name, contactkind, partname, groupname, trainno, phonenum1, phonenum2, shortnum1, shortnum2, note, rolename, rolekind, linenum);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Contact")]
        [HttpPost]
        public JsonResult DeleteContact(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = contactModel.DeleteContact(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Contact")]
        public ActionResult ImportContact()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Contact";

            return View();
        }

        [Authorize(Roles = "Contact")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitContactImport(string fileurl)
        {
            string rst = "";

            rst = contactModel.ImportContactData(fileurl);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public void ExportContact()
        {
            var datalist = contactModel.ExportContactList();
            var grid = new GridView();

            grid.DataSource = datalist;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=通讯录" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
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
