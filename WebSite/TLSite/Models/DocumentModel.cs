using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Web.Hosting;

namespace TLSite.Models
{
    public class DocumentInfo
    {
        public long uid { get; set; }
        public long senderid { get; set; }
        public string sendername { get; set; }
        public string senderimg { get; set; }
        public string sendpart { get; set; }
        public string receiver { get; set; }
        public string receivernames { get; set; }
        public string receiverange { get; set; }
        public string title { get; set; }
        public string docno { get; set; }
        public string contents { get; set; }
        public string attachname { get; set; }
        public string attachpath { get; set; }
        public long? attachsize { get; set; }
        public DateTime createtime { get; set; }
    }

    public class DocumentLog
    {
        public long uid { get; set; }
        public long userid { get; set; }
        public string username { get; set; }
        public long docid { get; set; }
        public string doctitle { get; set; }
        public int acttype { get; set; }
        public string execnote { get; set; }
        public string receiver { get; set; }
        public DateTime createtime { get; set; }
    }

    public enum DocStatus
    {
        WAITSIGN = 0,
        ALREADYSIGN
    }

    public class DocumentModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        UserModel userModel = new UserModel();

        #region 公文

        public string InsertDocument(string receiver, string title, string docno, string contents, string filename, string path, long filesize)
        {
            string rst = "";
            long senderid = CommonModel.GetSessionUserID();

            try
            {
                var senderinfo = userModel.GetDocExecUserInfo(senderid);

                if (senderinfo == null)
                {
                    return "非法操作";
                }

                tbl_document newitem = new tbl_document();

                newitem.sender = senderid;
                newitem.sendpart = senderinfo.parentname;
                newitem.receiver = receiver;
                newitem.title = title;
                newitem.docno = docno;
                newitem.contents = contents;
                newitem.attachpath = path;
                newitem.attachname = filename;
                newitem.attachsize = filesize;
                newitem.createtime = DateTime.Now;

                db.tbl_documents.InsertOnSubmit(newitem);
                db.SubmitChanges();

                string[] receiverids = receiver.Split(',');

                foreach(string rid in receiverids)
                {
                    tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                    newnoticelog.logtype = (int)NoticeType.DOCUMENT;
                    newnoticelog.title = "收到新公文了";
                    newnoticelog.readerid = long.Parse(rid);
                    newnoticelog.createtime = DateTime.Now;
                    newnoticelog.checklogid = newitem.uid;

                    db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public List<DocumentInfo> GetMyDocList(int kind)
        {
            List<DocumentInfo> rst = new List<DocumentInfo>();
            long myid = CommonModel.GetSessionUserID();

            var filterlist = db.tbl_documents
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath= m.doc.attachpath,
                    attachsize= m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .ToList();

            foreach (var item in filterlist)
            {
                var receiverids = item.receiver.Split(',');

                if (receiverids.Contains(myid.ToString()))
                {
                    rst.Add(item);
                }
            }

            if (kind == (int)DocStatus.WAITSIGN)
            {
                var loglist = db.tbl_documentlogs.Where(m => m.deleted == 0).ToList();

                List<long> forwdocids = new List<long>();
                foreach (var item in loglist)
                {
                    var receiverids = item.receiver.Split(',');
                    if (receiverids.Contains(myid.ToString()))
                    {
                        forwdocids.Add(item.docid);
                    }
                }

                if (forwdocids.Count() > 0)
                {
                    var forwardlist = db.tbl_documents
                        .Where(m => m.deleted == 0 && forwdocids.Contains(m.uid))
                        .OrderByDescending(m => m.createtime)
                        .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Select(m => new DocumentInfo
                        {
                            uid = m.doc.uid,
                            senderid = m.doc.sender,
                            sendername = m.sender.realname,
                            sendpart = m.doc.sendpart,
                            receiver = m.doc.receiver,
                            receiverange = m.doc.receiverange,
                            title = m.doc.title,
                            docno = m.doc.docno,
                            contents = m.doc.contents,
                            attachname = m.doc.attachname,
                            attachpath = m.doc.attachpath,
                            attachsize = m.doc.attachsize,
                            createtime = m.doc.createtime
                        })
                        .ToList();

                    rst.AddRange(forwardlist);
                    rst = rst.OrderByDescending(m => m.createtime).Distinct().ToList();
                }

                var signlist = loglist.Where(m => m.deleted == 0 && m.userid == myid).ToList();
                var docids = signlist.Select(m => m.docid).ToList();

                rst = rst.Where(m => !docids.Contains(m.uid)).ToList();
            }
            else if (kind == (int)DocStatus.ALREADYSIGN)
            {
                var loglist = db.tbl_documentlogs.Where(m => m.deleted == 0 && m.userid == myid).ToList();
                var docids = loglist.Select(m => m.docid).ToList();

                rst = rst.Where(m => docids.Contains(m.uid)).ToList();
            }
            
            return rst;
        }

        public List<DocumentInfo> GetMyDocList(DateTime starttime, DateTime endtime, int kind)
        {
            List<DocumentInfo> rst = new List<DocumentInfo>();
            long myid = CommonModel.GetSessionUserID();

            var filterlist = db.tbl_documents
                .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .ToList();

            foreach (var item in filterlist)
            {
                var receiverids = item.receiver.Split(',');

                if (receiverids.Contains(myid.ToString()))
                {
                    rst.Add(item);
                }
            }

            if (kind == (int)DocStatus.WAITSIGN)
            {
                var loglist = db.tbl_documentlogs.Where(m => m.deleted == 0).ToList();

                List<long> forwdocids = new List<long>();
                foreach (var item in loglist)
                {
                    var receiverids = item.receiver.Split(',');
                    if (receiverids.Contains(myid.ToString()))
                    {
                        forwdocids.Add(item.docid);
                    }
                }

                if (forwdocids.Count() > 0)
                {
                    var forwardlist = db.tbl_documents
                        .Where(m => m.deleted == 0 && forwdocids.Contains(m.uid))
                        .OrderByDescending(m => m.createtime)
                        .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Select(m => new DocumentInfo
                        {
                            uid = m.doc.uid,
                            senderid = m.doc.sender,
                            sendername = m.sender.realname,
                            sendpart = m.doc.sendpart,
                            receiver = m.doc.receiver,
                            receiverange = m.doc.receiverange,
                            title = m.doc.title,
                            docno = m.doc.docno,
                            contents = m.doc.contents,
                            attachname = m.doc.attachname,
                            attachpath = m.doc.attachpath,
                            attachsize = m.doc.attachsize,
                            createtime = m.doc.createtime
                        })
                        .ToList();

                    rst.AddRange(forwardlist);
                    rst = rst.OrderByDescending(m => m.createtime).Distinct().ToList();
                }

                var signlist = loglist.Where(m => m.deleted == 0 && m.userid == myid).ToList();
                var docids = signlist.Select(m => m.docid).ToList();

                rst = rst.Where(m => !docids.Contains(m.uid)).ToList();
            }
            else if (kind == (int)DocStatus.ALREADYSIGN)
            {
                var loglist = db.tbl_documentlogs.Where(m => m.deleted == 0 && m.userid == myid).ToList();
                var docids = loglist.Select(m => m.docid).ToList();

                rst = rst.Where(m => docids.Contains(m.uid)).ToList();
            }

            return rst;
        }

        public JqDataTableInfo GetMyDocDataTable(DateTime starttime, DateTime endtime, int kind, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<DocumentInfo> filteredCompanies;

            //List<DocumentInfo> alllist = GetMyDocList(kind);
            List<DocumentInfo> alllist = GetMyDocList(starttime, endtime, kind);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.sendername.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DocumentInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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
                c.docno,
                c.title,
                c.sendername,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
                GetReceiverNameList(c.receiver),
                c.attachname,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public List<string> GetSignDocReceiverList(DocumentInfo docinfo)
        {
            List<string> rst = new List<string>();

            string[] rids = docinfo.receiver.Split(',');

            rst.AddRange(rids);

            var lzlist = db.tbl_documentlogs.Where(m => m.deleted == 0 && m.docid == docinfo.uid && m.acttype == 1).ToList();

            foreach (var item in lzlist)
            {
                string[] receivers = item.receiver.Split(',');
                rst.AddRange(receivers);
            }

            return rst;
        }

        public string GetReceiverName(string receiver)
        {
            return db.tbl_users.Where(m => m.deleted == 0 && m.uid.ToString() == receiver.Trim()).Select(m => m.realname).FirstOrDefault();
        }

        public string GetReceiverNameList(string receivers)
        {
            string rst = "";

            if (!String.IsNullOrEmpty(receivers))
            {
                string[] receiverids = receivers.Split(',');

                var receiverlist = db.tbl_users.Where(m => m.deleted == 0 && receiverids.Contains(m.uid.ToString())).Select(m => m.realname).ToList();

                rst = String.Join("，", receiverlist);
            }

            return rst;
        }

        public DocumentInfo GetDocById(long id)
        {
            var rst = db.tbl_documents
                .Where(m => m.deleted == 0 && m.uid == id)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    senderimg = m.sender.imgurl,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .FirstOrDefault();

            return rst;
        }

        public DocumentInfo GetMyDocDetailInfo(long id)
        {
            long myid = CommonModel.GetSessionUserID();

            var rst = db.tbl_documents
                .Where(m => m.deleted == 0 && m.uid == id)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    senderimg = m.sender.imgurl,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .FirstOrDefault();

            string[] rids = rst.receiver.Split(',');
            if (rids.Contains(myid.ToString()))
            {
                var noticelog = db.tbl_sysnoticelogs.Where(m => m.readerid == myid && m.logtype == (int)NoticeType.DOCUMENT && m.checklogid == id).FirstOrDefault();
                if (noticelog != null && noticelog.readtime == null)
                {
                    noticelog.readtime = DateTime.Now;
                    db.SubmitChanges();
                }
            }

            return rst;
        }
        #endregion

        public string SubmitDocSign(long uid, string execnote, int flag, string receiver)
        {
            string rst = "";

            long userid = CommonModel.GetSessionUserID();
            try
            {
                tbl_documentlog newitem = new tbl_documentlog();
                newitem.acttype = (byte)flag;
                newitem.userid = userid;
                newitem.createtime = DateTime.Now;
                newitem.docid = uid;
                newitem.receiver = receiver;
                newitem.execnote = execnote;

                db.tbl_documentlogs.InsertOnSubmit(newitem);
                db.SubmitChanges();

                if (flag == 1)  //流转
                {
                    string[] receiverids = receiver.Split(',');
                    var noticeloglist = db.tbl_sysnoticelogs.Where(m => m.deleted == 0 && m.logtype == (int)NoticeType.DOCUMENT && m.checklogid == uid && receiverids.Contains(m.readerid.ToString())).ToList();

                    foreach (string rid in receiverids)
                    {
                        var oldlog = noticeloglist.Where(m => m.readerid.ToString() == rid).FirstOrDefault();

                        if (oldlog == null)
                        {
                            tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                            newnoticelog.logtype = (int)NoticeType.DOCUMENT;
                            newnoticelog.title = "收到新公文了";
                            newnoticelog.readerid = long.Parse(rid);
                            newnoticelog.createtime = DateTime.Now;
                            newnoticelog.checklogid = uid;

                            db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                        }
                        else
                        {
                            oldlog.readtime = null;
                        }
                    }

                    db.SubmitChanges();
                }

            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public tbl_documentlog GetMyDocLogInfo(long uid)
        {
            long userid = CommonModel.GetSessionUserID();
            return db.tbl_documentlogs.Where(m => m.deleted == 0 && m.docid == uid && m.userid == userid).FirstOrDefault();
        }

        public List<DocumentLog> GetMyDocLogList(long uid)
        {
            return db.tbl_documentlogs
                .Where(m => m.deleted == 0 && m.docid == uid)
                .Join(db.tbl_users, m => m.userid, l => l.uid, (m, l) => new { doc = m, user = l })
                .Select(m => new DocumentLog {
                    uid = m.doc.uid,
                    userid = m.doc.userid,
                    username = m.user.realname,
                    docid = m.doc.docid,
                    acttype = m.doc.acttype,
                    execnote = m.doc.execnote,
                    receiver = m.doc.receiver,
                    createtime = m.doc.createtime
                })
                .ToList();
        }

        public List<DocumentInfo> GetSearchDocList(long teamid, long sectorid, DateTime starttime, DateTime endtime)
        {
            var teaminfo = db.tbl_railteams.Where(m => m.deleted == 0 && m.uid == teamid).FirstOrDefault();
            var sectorinfo = db.tbl_railsectors.Where(m => m.deleted == 0 && m.uid == sectorid).FirstOrDefault();
            long myid = CommonModel.GetSessionUserID();
            string roleinfo = CommonModel.GetUserRoleInfo();
            var userinfo = userModel.GetUserById(myid);
            long partid = 0;

            var filterlist = db.tbl_documents
                .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .ToList();

            if (partid > 0)
            {
                var partinfo = db.tbl_railteams.Where(m => m.deleted == 0 && m.uid == partid).FirstOrDefault();

                if (partinfo != null)
                {
                    filterlist = filterlist.Where(m => m.sendpart == partinfo.teamname).ToList();
                }
            }

            if (teaminfo != null && sectorinfo == null)
            {
                filterlist = filterlist.Where(m => m.sendpart == teaminfo.teamname).ToList();
            }
            else if (teaminfo == null && sectorinfo != null)
            {
                filterlist = filterlist.Where(m => m.sendpart == sectorinfo.sectorname).ToList();
            }
            else if (teaminfo != null && sectorinfo != null)
            {
                filterlist = filterlist.Where(m => m.sendpart == sectorinfo.sectorname).ToList();
            }

            if (userinfo.userkind != (int)UserKind.ADMIN)
            {
                filterlist = filterlist.Where(m => m.senderid == myid || m.receiver.Split(',').Contains(myid.ToString())).ToList();
                var lzlist = db.tbl_documentlogs.Where(m => m.deleted == 0 && m.acttype == 1 && m.receiver.Split(',').Contains(myid.ToString())).ToList();
                List<long> docids = new List<long>();

                foreach (var item in lzlist)
                {
                    if (filterlist.Where(m => m.uid == item.docid).FirstOrDefault() == null)
                    {
                        docids.Add(item.docid);
                    }
                }

                var lzdoclist = db.tbl_documents
                    .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
                    .OrderByDescending(m => m.createtime)
                    .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                    .Select(m => new DocumentInfo
                    {
                        uid = m.doc.uid,
                        senderid = m.doc.sender,
                        sendername = m.sender.realname,
                        sendpart = m.doc.sendpart,
                        receiver = m.doc.receiver,
                        receiverange = m.doc.receiverange,
                        title = m.doc.title,
                        docno = m.doc.docno,
                        contents = m.doc.contents,
                        attachname = m.doc.attachname,
                        attachpath = m.doc.attachpath,
                        attachsize = m.doc.attachsize,
                        createtime = m.doc.createtime
                    })
                    .ToList();

                filterlist.AddRange(lzdoclist);
            }

            return filterlist;
        }

        public JqDataTableInfo GetSearchDocDataTable(long teamid, long sectorid, string starttime, string endtime, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<DocumentInfo> filteredCompanies;

            DateTime startd = new DateTime(1970, 1, 1);
            DateTime endd = new DateTime(2040, 1, 1);

            try { startd = DateTime.Parse(starttime); }
            catch { }
            try { endd = DateTime.Parse(endtime); }
            catch { }

            List<DocumentInfo> alllist = GetSearchDocList(teamid, sectorid, startd, endd);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.sendername.ToLower().Contains(param.sSearch.ToLower()) || 
                       c.docno.ToLower().Contains(param.sSearch.ToLower()) ||
                       c.title.ToLower().Contains(param.sSearch.ToLower()) ||
                       c.sendpart.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DocumentInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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

            var userrole = CommonModel.GetUserRoleInfo();

            var result = from c in displayedCompanies
                         select new[] { 
                c.docno,
                c.title,
                c.sendpart,
                c.sendername,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
            };

            if (userrole != null && ((string)userrole).Contains("Document"))
            {
                result = from c in displayedCompanies
                             select new[] { 
                        c.docno,
                        c.title,
                        c.sendpart,
                        c.sendername,
                        String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
                        c.uid.ToString()
                    };
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteDocument(long[] items)
        {
            try
            {
                long myid = CommonModel.GetSessionUserID();
                string roleinfo = CommonModel.GetUserRoleInfo();
                var userinfo = userModel.GetUserById(myid);
                DateTime pasttime = DateTime.Now.AddMinutes(-30);

                List<tbl_document> dellist = new List<tbl_document>();

                if (userinfo.userkind != (int)UserKind.ADMIN)
                {
                    if (roleinfo.Contains("Document"))
                    {
                        dellist = db.tbl_documents
                        .Where(m => items.Contains(m.uid))
                        .ToList();
                    }
                    else
                    {
                        dellist = db.tbl_documents.Where(m => m.deleted == 0 && m.sender == myid && m.createtime > pasttime).ToList();
                    }
                }
                else
                {
                    dellist = db.tbl_documents.Where(m => m.deleted == 0 && m.sender == myid && m.createtime > pasttime).ToList();
                }

                db.tbl_documents.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        #region 已发公文
        public List<DocumentInfo> GetSentDocList(DateTime starttime, DateTime endtime)
        {
            List<DocumentInfo> rst = new List<DocumentInfo>();
            long myid = CommonModel.GetSessionUserID();

            var filterlist = db.tbl_documents
                .Where(m => m.deleted == 0 && m.sender == myid && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new DocumentInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.sender,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    receiverange = m.doc.receiverange,
                    title = m.doc.title,
                    docno = m.doc.docno,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime
                })
                .ToList();

            return filterlist;
        }

        public JqDataTableInfo GetSentDocDataTable(DateTime starttime, DateTime endtime, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<DocumentInfo> filteredCompanies;

            List<DocumentInfo> alllist = GetSentDocList(starttime, endtime);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.sendername.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DocumentInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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
                c.docno,
                c.title,
                c.sendername,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.createtime),
                GetReceiverNameList(c.receiver),
                c.attachname,
                Convert.ToString(c.uid),
                GetDeletableSentItem(c.createtime).ToString()
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public int GetDeletableSentItem(DateTime createtime)
        {
            int rst = 0;
            DateTime now = DateTime.Now;

            TimeSpan timeDiff = DateTime.Now - createtime;

            if (timeDiff.TotalMinutes <= 30)
            {
                rst = 1;
            }

            return rst;
        }

//         public DocumentInfo GetMyDocDetailInfo(long id)
//         {
//             var rst = db.tbl_documents
//                 .Where(m => m.deleted == 0 && m.uid == id)
//                 .Join(db.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
//                 .Select(m => new DocumentInfo
//                 {
//                     uid = m.doc.uid,
//                     senderid = m.doc.sender,
//                     sendername = m.sender.realname,
//                     sendpart = m.doc.sendpart,
//                     receiver = m.doc.receiver,
//                     receiverange = m.doc.receiverange,
//                     title = m.doc.title,
//                     docno = m.doc.docno,
//                     contents = m.doc.contents,
//                     attachname = m.doc.attachname,
//                     attachpath = m.doc.attachpath,
//                     attachsize = m.doc.attachsize,
//                     createtime = m.doc.createtime
//                 })
//                 .FirstOrDefault();
// 
//             return rst;
//         }

        public System.Data.DataTable ExportDocList(int kind)
        {
            var products = new System.Data.DataTable("公文");

            List<DocumentInfo> alllist = GetMyDocList(kind);

            if (products != null)
            {
                products.Columns.Add("通知号", typeof(string));
                products.Columns.Add("公文标题", typeof(string));
                products.Columns.Add("发布人", typeof(string));
                products.Columns.Add("发布时间", typeof(string));
                products.Columns.Add("收件人", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        item.docno,
                        item.title,
                        item.sendername,
                        String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.createtime),
                        item.receivernames
                    );
                }
            }

            return products;
        }

        public System.Data.DataTable ExportSentDocList()
        {
            var products = new System.Data.DataTable("已发公文");
            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);

            List<DocumentInfo> alllist = GetSentDocList(starttime, endtime);

            if (products != null)
            {
                products.Columns.Add("通知号", typeof(string));
                products.Columns.Add("公文标题", typeof(string));
                products.Columns.Add("发布人", typeof(string));
                products.Columns.Add("发布时间", typeof(string));
                products.Columns.Add("收件人", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        item.docno,
                        item.title,
                        item.sendername,
                        String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.createtime),
                        item.receivernames
                    );
                }
            }

            return products;
        }
        #endregion
    }
}