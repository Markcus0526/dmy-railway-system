using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;

namespace TLSite.Controllers
{
    public class RuleController : Controller
    {
        RuleModel ruleModel = new RuleModel();

        [Authorize(Roles = "Rule")]
        public ActionResult List()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Rule";
            ViewData["level2nav"] = "Rule";

            return View();
        }

        [Authorize(Roles = "Rule")]
        [AjaxOnly]
        public JsonResult RetrieveRuleList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = ruleModel.GetRuleDataTable(param, Request.QueryString, rootUri);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Browse()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Rule";
            ViewData["level2nav"] = "Rule";
            ViewData["rulelist"] = ruleModel.GetRuleList();

            return View();
        }

        [Authorize]
        public ActionResult Detail(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Rule";
            ViewData["level2nav"] = "Rule";
            ViewData["ruleinfo"] = ruleModel.GetRuleInfo(id);

            return View();
        }

        [Authorize(Roles = "Rule")]
        public ActionResult AddRule()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Rule";
            ViewData["level2nav"] = "Rule";

            return View();
        }

        [Authorize(Roles = "Rule")]
        public ActionResult EditRule(long id)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Rule";
            ViewData["level2nav"] = "Rule";

            var ruleinfo = ruleModel.GetRuleInfo(id);
            ViewData["ruleinfo"] = ruleinfo;
            ViewData["uid"] = ruleinfo.uid;
            ViewData["contents"] = ruleinfo.contents;

            return View("AddRule");
        }

        [Authorize(Roles = "Rule")]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitRule(long uid, string rulename, string imgurl, int sortid, string esccontent, string filename, string path, long filesize)
        {
            string rst = "";

            path = path.Trim(new char[] { '\"' });
            if (uid == 0)
            {
                rst = ruleModel.InsertRule(rulename, imgurl, sortid, esccontent, filename, path, filesize);
            }
            else
            {
                rst = ruleModel.UpdateRule(uid, rulename, imgurl, sortid, esccontent, filename, path, filesize);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Rule")]
        [HttpPost]
        public JsonResult DeleteRule(string delids)
        {
            string[] ids = delids.Split(',');
            long[] selcheckbox = ids.Where(m => !String.IsNullOrWhiteSpace(m)).Select(m => long.Parse(m)).ToArray();
            bool rst = ruleModel.DeleteRule(selcheckbox);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}