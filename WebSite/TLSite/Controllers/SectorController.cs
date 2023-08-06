using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models.Library;
using TLSite.Models;

namespace TLSite.Controllers
{
    public class SectorController : Controller
    {
        SectorModel sectorModel = new SectorModel();

        [Authorize]
        public ActionResult SectorList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Sector";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveSectorList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = sectorModel.GetSectorDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddSector()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Sector";
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View();
        }

        [Authorize]
        public ActionResult EditSector(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "Sector";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditSector", "", rootUri);

            var sectorinfo = sectorModel.GetSectorById(id);
            ViewData["sectorinfo"] = sectorinfo;
            ViewData["uid"] = sectorinfo.uid;
            //ViewData["config"] = SystemModel.GetMailConfig();

            return View("AddSector");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSector(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = sectorModel.DeleteSector(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitSector(long uid, string sectorname, int sortid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = sectorModel.InsertSector(sectorname, sortid);
            }
            else
            {
                rst = sectorModel.UpdateSector(uid, sectorname, sortid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult CheckUniqueSectorname(string sectorname, long uid)
        {
            bool rst = sectorModel.CheckDuplicateName(sectorname, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
