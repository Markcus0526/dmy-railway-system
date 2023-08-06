using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class NoticeTipInfo
    {
        public long uid { get; set; }
        public NoticeType logtype { get; set; }
        public string title { get; set; }
        public string shorttitle { get; set; }
        public string imgurl { get; set; }
        public string contents { get; set; }
        public int pluscnt { get; set; }
        public DateTime createtime { get; set; }
        public string beforetime { get; set; }
        public string linkurl { get; set; }

        //Judge Log
        public long checklogid { get; set; }
    }

    public enum NoticeType
    {
        JUDGE,
        DOCUMENT,
        TASK,
        OPINION,
        MAIL
    }

    public class NoticeTitle
    {
        public static string GetShortTitle(NoticeType logtype)
        {
            string rst = "";
            switch (logtype)
            {
                case NoticeType.JUDGE:
                    rst = "收到新考核了";
                    break;
                case NoticeType.DOCUMENT:
                    rst = "收到新公文了";
                    break;
                case NoticeType.TASK:
                    rst = "收到新任务了";
                    break;
                case NoticeType.MAIL:
                    rst = "收到新邮件";
                    break;
                default:
                    break;
            }
            return rst;
        }
    }

    public class SystemModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        JudgeModel judgeModel = new JudgeModel();
        DocumentModel docModel = new DocumentModel();
        TaskModel taskModel = new TaskModel();
        MailModel mailModel = new MailModel();

        #region Notification
        public List<NoticeTipInfo> GetUnreadNoticeTip(int cnt)
        {
            List<NoticeTipInfo> rst = new List<NoticeTipInfo>();
            long reader = CommonModel.GetSessionUserID();

            var noticelist = db.tbl_sysnoticelogs
                .Where(m => m.readerid == reader && m.readtime == null)
                .OrderByDescending(m => m.createtime)
                .GroupBy(m => m.logtype)
                .Select(m => new NoticeTipInfo
                {
                    uid = m.FirstOrDefault().uid,
                    logtype = (NoticeType)m.FirstOrDefault().logtype,
                    shorttitle = NoticeTitle.GetShortTitle((NoticeType)m.FirstOrDefault().logtype),
                    title = m.FirstOrDefault().title,
                    pluscnt = m.Count() - 1,
                    beforetime = CommonModel.GetTimeDiffFromNow(m.FirstOrDefault().createtime),
                    checklogid = m.FirstOrDefault().checklogid
                })
                .Skip(0)
                .Take(cnt)
                .ToList();

            if (noticelist != null && noticelist.Count() > 0)
            {
                rst = noticelist;
            }

            foreach (var item in noticelist)
            {
                if (item.logtype == NoticeType.JUDGE)
                {
                    var loginfo = judgeModel.GetJudgeDetail(item.checklogid);

                    if (loginfo != null)
                    {
                        item.imgurl = loginfo.imgurl;
                        item.title = loginfo.checkinfo;
                        item.contents = loginfo.contents;
                        item.linkurl = "Judge/JudgeDetail/" + loginfo.uid;
                    }
                }
                else if (item.logtype == NoticeType.DOCUMENT)
                {
                    item.linkurl = "Document/MyDocList";
                    var docinfo = docModel.GetDocById(item.checklogid);

                    if (docinfo != null)
                    {
                        item.imgurl = docinfo.senderimg;
                        item.title = docinfo.title;
                        item.linkurl = "Document/Detail/" + docinfo.uid;
                        //item.contents = docinfo.contents;
                    }
                }
                else if (item.logtype == NoticeType.TASK)
                {
                    item.linkurl = "Task/MyTaskList?kind=0";
                    var taskinfo = taskModel.GetTaskInfo(item.checklogid,0);

                    if (taskinfo != null)
                    {
                        item.imgurl = taskinfo.senderimg;
                        item.title = taskinfo.title;
                        item.linkurl = "Task/TaskDetail/" + taskinfo.uid + "?origin=mytask";
                    }
                }
                else if (item.logtype == NoticeType.MAIL)
                {
                    item.linkurl = "Mail/Inbox";
                    var mailinfo = mailModel.GetMail(item.checklogid, 0);

                    if (mailinfo != null)
                    {
                        item.imgurl = mailinfo.senderimg;
                        item.title = mailinfo.title;
                        item.linkurl = "Mail/Inbox";
                    }
                }
            }

            return rst;
        }

        public List<NoticeTipInfo> GetLatestUnreadNotice(DateTime lasttime)
        {
            List<NoticeTipInfo> rst = new List<NoticeTipInfo>();
            long reader = CommonModel.GetSessionUserID();

            try
            {
                var noticelist = db.tbl_sysnoticelogs
                    .Where(m => m.readerid == reader && m.noticetime == null)
                    .OrderByDescending(m => m.createtime)
                    .Select(m => new NoticeTipInfo
                    {
                        uid = m.uid,
                        logtype = (NoticeType)m.logtype,
                        shorttitle = NoticeTitle.GetShortTitle((NoticeType)m.logtype),
                        title = m.title,
                        beforetime = CommonModel.GetTimeDiffFromNow(m.createtime),
                        checklogid = m.checklogid
                    })
                    .ToList();
                /* && m.createtime >= lasttime*/
                var changelist = db.tbl_sysnoticelogs.Where(m => m.readerid == reader && m.noticetime == null).ToList();

                foreach (var item in changelist)
                {
                    item.noticetime = DateTime.Now;
                }

                db.SubmitChanges();

                if (noticelist != null && noticelist.Count() > 0)
                {
                    rst = noticelist;
                }

                foreach (var item in noticelist)
                {
                    if (item.logtype == NoticeType.JUDGE)
                    {
                        var loginfo = judgeModel.GetJudgeDetail(item.checklogid);

                        if (loginfo != null)
                        {
                            item.imgurl = loginfo.checkerimg;
                            item.title = loginfo.checkinfo;
                            item.contents = loginfo.contents;
                        }

                        item.linkurl = "Judge/JudgeDetail/" + item.checklogid;
                    }
                    else if (item.logtype == NoticeType.DOCUMENT)
                    {
                        var loginfo = docModel.GetDocById(item.checklogid);

                        if (loginfo != null)
                        {
                            item.imgurl = loginfo.senderimg;
                            item.title = "公文：" + loginfo.title;
                            item.contents = CommonModel.han_cut(loginfo.contents, 50);
                        }

                        item.linkurl = "Document/Detail/" + item.checklogid;
                    }
                    else if (item.logtype == NoticeType.TASK)
                    {
                        var loginfo = taskModel.GetTaskInfo(item.checklogid,2);

                        if (loginfo != null)
                        {
                            item.imgurl = loginfo.senderimg;
                            item.title = "任务：" + loginfo.title;
                            item.contents = CommonModel.han_cut(loginfo.contents, 50);
                        }

                        item.linkurl = "Task/TaskDetail/" + item.checklogid;
                    }

                    else if (item.logtype == NoticeType.MAIL)
                    {
                        var loginfo = mailModel.GetMail(item.checklogid,2);

                        if (loginfo != null)
                        {
                            item.imgurl = loginfo.senderimg;
                            item.title = "邮件：" + loginfo.title;
                            item.contents = CommonModel.han_cut(loginfo.contents, 50);
                        }

                        item.linkurl = "Mail/Inbox";
                    }
                }
            }
            catch (System.Exception ex)
            {
            	
            }

            return rst;
        }

        public tbl_sysconfig GetSysConfig()
        {
            return db.tbl_sysconfigs.FirstOrDefault();
        }

        public string SubmitConfig(string notice, string imgurl)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            int editmode = 0;

            try
            {
                var config = GetSysConfig();

                if (config == null)
                {
                    config = new tbl_sysconfig();
                }
                else
                {
                    editmode = 1;
                }

                config.notice = notice;

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);

                    config.backimg = savepath + imgurl;
                }

                if (editmode == 0)
                {
                    db.tbl_sysconfigs.InsertOnSubmit(config);
                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public List<tbl_slideimg> GetSlideList()
        {
            return db.tbl_slideimgs.Where(m => m.deleted == 0)
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public JqDataTableInfo GetSlideDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_slideimg> filteredCompanies;

            List<tbl_slideimg> alllist = GetSlideList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_slideimg, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                c.title,
                c.imgpath,
                c.sortid.ToString(),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteSlide(long[] items)
        {
            try
            {
                var dellist = db.tbl_slideimgs
                    .Where(m => items.Contains(m.uid))
                    .ToList();

                db.tbl_slideimgs.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        public tbl_slideimg GetSlideById(long uid)
        {
            return db.tbl_slideimgs
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }

        public string InsertSlide(string title, string imgpath, int sortid)
        {
            tbl_slideimg newitem = new tbl_slideimg();
            long accountid = CommonModel.GetCurrAccountId();
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            newitem.title = title;
            newitem.sortid = sortid;
            newitem.createtime = DateTime.Now;

            if (File.Exists(orgbase + imgpath))
            {
                if (!Directory.Exists(targetbase))
                {
                    Directory.CreateDirectory(targetbase);
                }
                File.Move(orgbase + imgpath, targetbase + imgpath);

                newitem.imgpath = savepath + imgpath;
            }

            db.tbl_slideimgs.InsertOnSubmit(newitem);

            db.SubmitChanges();

            return "";
        }

        public string UpdateSlide(long uid, string title, string imgpath, int sortid)
        {
            string rst = "";
            tbl_slideimg edititem = GetSlideById(uid);
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            if (edititem != null)
            {
                edititem.title = title;
                edititem.sortid = sortid;

                if (File.Exists(orgbase + imgpath))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgpath, targetbase + imgpath);

                    edititem.imgpath = savepath + imgpath;
                }

                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "数据不存在";
            }

            return rst;
        }

        #endregion
    }
}