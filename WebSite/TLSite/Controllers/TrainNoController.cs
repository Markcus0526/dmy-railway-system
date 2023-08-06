using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;

namespace TLSite.Controllers
{
    public class TrainNoController : Controller
    {
        TrainNoModel tnoModel = new TrainNoModel();
        RouteModel routeModel = new RouteModel();

        [Authorize]
        public ActionResult TrainNoList()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "TrainNo";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "", "", rootUri);

            return View();
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult RetrieveTrainNoList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = tnoModel.GetTrainNoDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddTrainNo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "TrainNo";
            ViewData["routelist"] = routeModel.GetRouteList();

            return View();
        }

        [Authorize]
        public ActionResult EditTrainNo(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Base";
            ViewData["level2nav"] = "TrainNo";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), ViewData["level2nav"].ToString(), "EditTrainNo", "", rootUri);

            var noinfo = tnoModel.GetTrainNoById(id);
            ViewData["noinfo"] = noinfo;
            ViewData["uid"] = noinfo.uid;
            ViewData["routelist"] = routeModel.GetRouteList();

            return View("AddTrainNo");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteTrainNo(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = tnoModel.DeleteTrainNo(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitTrainNo(long uid, long routeid, string trainno, int sortid)
        {
            string rst = "";

            if (uid == 0)
            {
                rst = tnoModel.InsertTrainNo(routeid, trainno, sortid);
            }
            else
            {
                rst = tnoModel.UpdateTrainNo(uid, routeid, trainno, sortid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult CheckUniqueTrainNo(string trainno, long uid)
        {
            bool rst = tnoModel.CheckDuplicateName(trainno, uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
