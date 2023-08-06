using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using TLSite.Models.Library;
using System.Net.Mail;
using System.Net;

namespace TLSite.Controllers
{
    public class MailController : Controller
    {
        MailModel mailModel = new MailModel();
        SectorModel sectorModel = new SectorModel();
        TeamModel teamModel = new TeamModel();
        GroupModel groupModel = new GroupModel();
        UserModel userModel = new UserModel();

        [Authorize]
        public ActionResult Inbox()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Mail";
            ViewData["level2nav"] = "Inbox";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), "", "", "", rootUri);

            var sendlist = mailModel.GetSentList(1, "");
            var receivelist = mailModel.GetReceivedList(1, "");

            ViewData["sendlist"] = sendlist;
            ViewData["receivelist"] = receivelist;

            //判断发件人权限
            var userid=CommonModel.GetSessionUserID();
            var userInfo= userModel.GetUserDetailById(userid);

            if (userInfo.usertype==ExecType.TrainCrew)
            {
                ViewData["userlist"] = userModel.GetUserListByTeamid(userInfo.teamid);
                var teamlist = new List<tbl_railteam>();
                teamlist.Add(teamModel.GetTeamById(userInfo.teamid));
                ViewData["teamlist"] = teamlist;
            }
            else 
            {
                ViewData["userlist"] = userModel.GetAllUserList();
                ViewData["sectorlist"] = sectorModel.GetSectorList();
                ViewData["teamlist"] = teamModel.GetTeamList();
            }
           
            ViewData["grouplist"] = groupModel.GetGroupList();

            return View();
        }

        [Authorize]
        public ActionResult NewMail()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;
            ViewData["level1nav"] = "Mail";
            ViewData["level2nav"] = "Inbox";
            ViewData["navinfo"] = CommonModel.GetTopNavInfo(ViewData["level1nav"].ToString(), "", "", "", rootUri);
            ViewData["userlist"] = userModel.GetAllUserList();

            return View();
        }

        [Authorize]
        [HttpPost]
        [AjaxOnly]
        public JsonResult SendMail(string title, string esccontent, string attachpath, long attachfilesize, string filename)
        {
            string rst = "";
            string receiver = "";
            string attachpathh = "";
            //return  Json(rst, JsonRequestBehavior.AllowGet);
            try
            {
                receiver = Request.Form["receiver"].ToString();
            }
            catch (System.Exception ex)
            {

            }

            string[] path = attachpath.Split(',');
            foreach(var i in path){
                if (i.Length!=0)
                {
                    attachpathh += i.Trim(new char[] { '\"', ',' }) + ",";
                }
            }
            //attachpath = attachpath.Trim(new char[] { '\"' });
            rst = mailModel.InsertMail(receiver, title, esccontent, attachpathh, attachfilesize, filename);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult MailDetail(long id)
        {
            var rst = mailModel.GetMail(id, 1);
            string[] receiverid = rst.receiver.Split(',');

            foreach (var n in receiverid)
            {
                var receiverobj = mailModel.GetReceiptByid(long.Parse(n),id);
                rst.receiverlist.Add(receiverobj);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AjaxOnly]
        public JsonResult GetReceivedMailList(int page, string searchkey, string kind)
        {
            if (kind == "inbox")
            {
                var rst = mailModel.GetReceivedList(page, searchkey);
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
            else if (kind == "sent")
            {
                var rst = mailModel.GetSentList(page, searchkey);
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
            
            return Json(null, JsonRequestBehavior.AllowGet);
        }

     
        public string Download()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
           
            string rst="";
            //文件下载
            string fileName=Request.QueryString["fileName"];
            string fileUrl = Request.QueryString["fileUrl"];

            if (Request.UserAgent != null)
            {
                string userAgent = Request.UserAgent.ToUpper();
                if (userAgent.IndexOf("FIREFOX", StringComparison.Ordinal) <= 0)
                {
                    CommonModel commonmodel = new CommonModel();
                    fileName = commonmodel.ToUtf8String(fileName);

                }
            }

            string fn = Request.Url.ToString();
            string filePath = Server.MapPath("../") + fileUrl;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
           // Response.AppendHeader("Content-Length", fInfo.Length.ToString());
            try
            {
                Response.WriteFile(filePath);

            }
            catch (System.Exception ex)
            {
                rst = "该文件不存在，可能已经被删除。"; 
            }
            return rst;
        }
        [AjaxOnly]
        [Authorize]
        public JsonResult SendReceipt()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = "";
            long mailid = long.Parse(Request.QueryString["mailid"]);
            long userid = CommonModel.GetSessionUserID();

            rst=mailModel.sendreceipt(mailid,userid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        [Authorize]
        public JsonResult GetReceipt()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst = "";
            long mailid = long.Parse(Request.QueryString["mailid"]);
            long userid = CommonModel.GetSessionUserID();

            rst = mailModel.GetReceipt(mailid, userid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
