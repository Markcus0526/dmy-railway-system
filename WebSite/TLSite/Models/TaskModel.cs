using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public enum TaskStatus
    {
        NOTACCPET = 0,
        EXECUTING,
        FINISH,
        NOTFINISH
    }

    public class TaskInfo
    {
        public long uid { get; set; }
        public long senderid { get; set; }
        public string sendername { get; set; }
        public string senderimg { get; set; }
        public string sendpart { get; set; }
        public string receiver { get; set; }
        public string receivernames { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public string attachname { get; set; }
        public string attachpath { get; set; }
        public long? attachsize { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public DateTime publishtime { get; set; }
        public DateTime createtime { get; set; }
        public DateTime? accepttime { get; set; }
        public DateTime? finishtime { get; set; }
        public TaskStatus status { get; set; }
        public int progress { get; set; }
        public string statusstr { get; set; }
    }

    public class TaskExecutive
    {
        public long uid { get; set; }
        public long taskid { get; set; }
        public string taskname { get; set; }
        public string receivername { get; set; }
        public TaskStatus status { get; set; }
        public DateTime? accepttime { get; set; }
        public DateTime? finishtime { get; set; }
        public DateTime createtime { get; set; }
    }

    public class TaskMessage
    {
        public long uid { get; set; }
        public long taskid { get; set; }
        public long userid { get; set; }
        public string username { get; set; }
        public string imgurl { get; set; }
        public string contents { get; set; }
        public DateTime createtime { get; set; }
    }

    public class TaskModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        UserModel userModel = new UserModel();

        #region 任务

        public string InsertTask(string receiver, string title, string docno, string contents, DateTime starttime, DateTime endtime, string filename, string path, long filesize)
        {
            string rst = "";
            long senderid = CommonModel.GetSessionUserID();

            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                var senderinfo = userModel.GetDocExecUserInfo(senderid);

                if (senderinfo == null)
                {
                    return "非法操作";
                }

                tbl_task newitem = new tbl_task();

                newitem.title = title;
                newitem.sendpart = senderinfo.parentname;
                newitem.senderid = senderid;
                newitem.receiver = receiver;
                newitem.starttime = starttime;
                newitem.endtime = endtime;
                newitem.contents = contents;
                newitem.attachname = filename;
                newitem.attachpath = path;
                newitem.attachsize = filesize;
                newitem.createtime = DateTime.Now;
                newitem.publishtime = DateTime.Now;

                db.tbl_tasks.InsertOnSubmit(newitem);
                db.SubmitChanges();

                string[] receiverids = receiver.Split(',');

                foreach (string rid in receiverids)
                {
                    tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                    newnoticelog.logtype = (int)NoticeType.TASK;
                    newnoticelog.title = "收到新任务了";
                    newnoticelog.readerid = long.Parse(rid);
                    newnoticelog.createtime = DateTime.Now;
                    newnoticelog.checklogid = newitem.uid;

                    db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                }

                db.SubmitChanges();
                db.Transaction.Commit();

            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
                db.Transaction.Rollback();
            }

            return rst;
        }

        public List<TaskInfo> GetTaskList()
        {
            List<TaskInfo> rst = new List<TaskInfo>();
            long myid = CommonModel.GetSessionUserID();
            var nowtime = DateTime.Now;
            var nowday = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0).AddDays(1);

            var filterlist = db.tbl_tasks
                .Where(m => m.deleted == 0 && m.senderid == myid)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .ToList();

            foreach (var item in filterlist)
            {
                string[] rids = item.receiver.Split(',');
                var statuslist = db.tbl_taskstatus
                    .Where(m => m.deleted == 0 && rids.Contains(m.receiverid.ToString()) && m.taskid == item.uid)
                    .ToList();

                var acceptcnt = statuslist.Where(m => m.status == (int)(TaskStatus.EXECUTING)).Count();
                var finishcnt = statuslist.Where(m => m.status == (int)(TaskStatus.FINISH)).Count();

                if (acceptcnt > 0 )
                {
                    if (item.endtime<nowtime)
                    {
                        item.statusstr = "<span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>";

                    }
                    else {
                        item.statusstr = "<span class='label label-large label-info arrowed-right arrowed-in'>执行中</span>";
                    }
                }
                else if (finishcnt == rids.Count())
                {
                    item.statusstr = "<span class='label label-large label-success arrowed-in arrowed-in-right'>已完成</span>";
                }
                else if (item.endtime < nowtime)
                {
                    item.statusstr = "<span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>";
                }
                else
                {
                    item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                }

                rst.Add(item);
            }

            return rst;
        }

        public List<TaskInfo> GetMyTaskList(int kind)
        {
            List<TaskInfo> rst = new List<TaskInfo>();
            long myid = CommonModel.GetSessionUserID();
            var nowtime = DateTime.Now;
            var nowday = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0).AddDays(1);

            var filterlist = db.tbl_tasks
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .ToList();

            var statuslist = db.tbl_taskstatus
                .Where(m => m.deleted == 0 &&
                    m.receiverid == myid)
                .ToList();

            foreach (var item in filterlist)
            {
                string[] receiverids = item.receiver.Split(',');

                if (receiverids.Contains(myid.ToString()))
                {
                    TaskStatus i = TaskStatus.NOTACCPET;

                    var status = statuslist.Where(m => m.taskid == item.uid && m.receiverid == myid).FirstOrDefault();

                    if (status != null)
                    {
                        
                        i = (TaskStatus)status.status;
                        if ((i == TaskStatus.EXECUTING||i==TaskStatus.NOTACCPET)&&item.endtime<nowtime)
                        {
                            tbl_taskstatus updatestatus = db.tbl_taskstatus.Where(m => m.taskid == item.uid && m.receiverid == myid).FirstOrDefault();
                            updatestatus.status = (int)TaskStatus.NOTFINISH;
                        }
                        item.accepttime = status.accepttime;
                        item.finishtime = status.finishtime;
                        item.status = (TaskStatus)status.status;
                    }
                    else
                    {
                        if (item.endtime < nowtime)
                        {
                            i = TaskStatus.NOTFINISH;

                            tbl_taskstatus newstatus = new tbl_taskstatus();
                            newstatus.taskid = item.uid;
                            newstatus.receiverid = myid;
                            newstatus.status = (int)TaskStatus.NOTFINISH;
                            newstatus.createtime = DateTime.Now;
                            db.tbl_taskstatus.InsertOnSubmit(newstatus);
                        }
                        else
                        {
                            i = TaskStatus.NOTACCPET;

                            tbl_taskstatus newstatus = new tbl_taskstatus();
                            newstatus.taskid = item.uid;
                            newstatus.receiverid = myid;
                            newstatus.status = (int)TaskStatus.NOTACCPET;
                            newstatus.createtime = DateTime.Now;
                            db.tbl_taskstatus.InsertOnSubmit(newstatus);
                        }
                    }

                    if (i == TaskStatus.NOTACCPET)
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                    }
                    else if (i == TaskStatus.EXECUTING)
                    {
                        item.statusstr = "<span class='label label-large label-info arrowed-right arrowed-in'>执行中</span>";
                    }
                    else if (i == TaskStatus.FINISH)
                    {
                        item.statusstr = "<span class='label label-large label-success arrowed-in arrowed-in-right'>已完成</span>";
                    }
                    else if (i == TaskStatus.NOTFINISH)
                    {
                        item.statusstr = "<span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>";
                    }
                    else
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                    }

                    rst.Add(item);

                }
            }

            if (kind == 0)
            {
                rst = rst.Where(m => m.status == TaskStatus.NOTACCPET).ToList();
            }
            else
            {
                rst = rst.Where(m => m.status != TaskStatus.NOTACCPET).ToList();
            }

            db.SubmitChanges();

            return rst;
        }

        public JqDataTableInfo GetTaskDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<TaskInfo> filteredCompanies;

            List<TaskInfo> alllist = GetTaskList();

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
            Func<TaskInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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
                String.Format("{0:yyyy-MM-dd}", c.publishtime),
                c.sendpart,
                c.sendername,
                String.Format("{0:yyyy-MM-dd}", c.starttime),
                String.Format("{0:yyyy-MM-dd}", c.endtime),
                c.statusstr,
                c.attachname,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

//         public bool UpdateTaskStatus(List<TaskInfo> alllist)
//         {
//             bool rst = false;
//             List<long> updateids = new List<long>();
// 
//             foreach (var item in alllist)
//             {
//                 if (item.status == TaskStatus.NOTACCPET || item.status == TaskStatus.EXECUTING)
//                 {
// 
//                 }
//             }
// 
//             return rst;
//         }

        public JqDataTableInfo GetMyTaskDataTable(int kind, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<TaskInfo> filteredCompanies;

            List<TaskInfo> alllist = GetMyTaskList(kind);

            //UpdateTaskStatus(alllist);

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
            Func<TaskInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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
                String.Format("{0:yyyy-MM-dd}", c.publishtime),
                c.sendpart,
                c.sendername,
                String.Format("{0:yyyy-MM-dd}", c.starttime) + " ~ " + String.Format("{0:yyyy-MM-dd}", c.endtime), 
                c.accepttime == null ? "" : String.Format("{0:yyyy-MM-dd}", (DateTime)c.accepttime),
                c.finishtime == null ? "" : String.Format("{0:yyyy-MM-dd}", (DateTime)c.finishtime),
                c.statusstr,
                c.attachname,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        #endregion

        public TaskInfo GetTaskInfo(long taskid,int flag)
        {
            long myid = CommonModel.GetSessionUserID();

            var rst = db.tbl_tasks
                .Where(m => m.deleted == 0 && m.uid == taskid)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    senderimg = m.sender.imgurl,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .FirstOrDefault();
            if (flag == 1)
            {
                string[] rids = rst.receiver.Split(',');
                if (rids.Contains(myid.ToString()))
                {
                    var noticelog = db.tbl_sysnoticelogs.Where(m => m.readerid == myid && m.logtype == (int)NoticeType.TASK && m.checklogid == taskid).FirstOrDefault();
                    if (noticelog != null && noticelog.readtime == null)
                    {
                        noticelog.readtime = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
            }

            return rst;
        }

        public List<TaskExecutive> GetExecutiveList(long taskid)
        {
            List<TaskExecutive> rst = new List<TaskExecutive>();
            var task = db.tbl_tasks.Where(m => m.deleted == 0 && m.uid == taskid).FirstOrDefault();
            var nowtime = DateTime.Now;
            var nowday = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0).AddDays(1);

            if (task != null)
            {
                string[] receiverids = task.receiver.Split(',');

                var execlist = db.tbl_users.Where(m => m.deleted == 0 && receiverids.Contains(m.uid.ToString())).ToList();
                var statuslist = db.tbl_taskstatus
                    .Where(m => m.deleted == 0 &&
                        m.taskid == taskid &&
                        receiverids.Contains(m.receiverid.ToString()))
                    .ToList();

                foreach (var item in execlist)
                {
                    TaskExecutive newitem = new TaskExecutive();

                    newitem.uid = item.uid;
                    newitem.taskid = taskid;
                    newitem.taskname = task.title;
                    newitem.receivername = item.realname;
                    newitem.status = TaskStatus.NOTACCPET;

                    var stitem = statuslist.Where(m => m.receiverid == item.uid).FirstOrDefault();

                    if (stitem != null)
                    {
                        newitem.status = (TaskStatus)stitem.status;
                        newitem.accepttime = stitem.accepttime;
                        newitem.finishtime = stitem.finishtime;
                    }

                    if (task.endtime < nowtime)
                    {
                        newitem.status = TaskStatus.NOTFINISH;
                    }

                    rst.Add(newitem);
                }
            }

            return rst;
        }

        public List<TaskMessage> GetMessageList(long taskid)
        {
            return db.tbl_taskmessages
                .Where(m => m.deleted == 0 && m.taskid == taskid)
                .Join(db.tbl_users, m => m.userid, l => l.uid, (m, l) => new { message = m, user = l })
                .OrderBy(m => m.message.createtime)
                .Select(m => new TaskMessage
                {
                    uid = m.message.uid,
                    taskid = m.message.taskid,
                    userid = m.message.userid,
                    username = m.user.realname,
                    imgurl = m.user.imgurl,
                    contents = m.message.contents,
                    createtime = m.message.createtime
                })
                .ToList();
        }

        public string ExecuteTask(long taskid)
        {
            string rst = "";
            long myid = CommonModel.GetSessionUserID();

            try
            {
                var status = db.tbl_taskstatus.Where(m => m.deleted == 0 && m.taskid == taskid && m.receiverid == myid).FirstOrDefault();

                if (status != null)
                {
                    status.accepttime = DateTime.Now;
                    status.status = (int)TaskStatus.EXECUTING;
                }
                else
                {
                    tbl_taskstatus newitem = new tbl_taskstatus();
                    newitem.taskid = taskid;
                    newitem.receiverid = myid;
                    newitem.accepttime = DateTime.Now;
                    newitem.status = (int)TaskStatus.EXECUTING;
                    newitem.createtime = DateTime.Now;
                    db.tbl_taskstatus.InsertOnSubmit(newitem);
                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public string ExecuteFinish(long taskid)
        {
            string rst = "";
            long myid = CommonModel.GetSessionUserID();

            try
            {
                var status = db.tbl_taskstatus
                    .Where(m => m.deleted == 0 && m.taskid == taskid && m.receiverid == myid)
                    .FirstOrDefault();

                if (status != null)
                {
                    status.finishtime = DateTime.Now;
                    status.status = (int)TaskStatus.FINISH;
                }
                else
                {
                    tbl_taskstatus newitem = new tbl_taskstatus();
                    newitem.taskid = taskid;
                    newitem.receiverid = myid;
                    newitem.finishtime = DateTime.Now;
                    newitem.status = (int)TaskStatus.FINISH;
                    newitem.createtime = DateTime.Now;
                    db.tbl_taskstatus.InsertOnSubmit(newitem);
                }

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public string SaveMessage(long taskid, string text)
        {
            string rst = "";
            long myid = CommonModel.GetSessionUserID();

            try
            {
                tbl_taskmessage newitem = new tbl_taskmessage();
                newitem.taskid = taskid;
                newitem.userid = myid;
                newitem.contents = text;
                newitem.createtime = DateTime.Now;
                db.tbl_taskmessages.InsertOnSubmit(newitem);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public List<TaskInfo> GetUnfinishedTask()
        {
            List<TaskInfo> rst = new List<TaskInfo>();
            long myid = CommonModel.GetSessionUserID();

            var filterlist = db.tbl_tasks
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .ToList();

            var statuslist = db.tbl_taskstatus
                .Where(m => m.deleted == 0 && m.receiverid == myid)
                .ToList();

            foreach (var item in filterlist)
            {
                string[] rids = item.receiver.Split(',');

                if (rids.Contains(myid.ToString())) {
                    TaskStatus i = TaskStatus.NOTACCPET;
                    var status = statuslist.Where(m => m.taskid == item.uid && m.receiverid == myid).FirstOrDefault();

                    if (status != null)
                    {
                        i = (TaskStatus)status.status;
                        item.accepttime = status.accepttime;
                        item.finishtime = status.finishtime;
                        item.status = (TaskStatus)status.status;
                    }

                    if (i == TaskStatus.NOTACCPET)
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                        item.progress = 0;
                    }
                    else if (i == TaskStatus.EXECUTING)
                    {
                        item.progress = 50;
                        item.statusstr = "<span class='label label-large label-info arrowed-right arrowed-in'>执行中</span>";
                    }
                    else if (i == TaskStatus.FINISH)
                    {
                        item.statusstr = "<span class='label label-large label-success arrowed-in arrowed-in-right'>已完成</span>";
                        item.progress = 100;
                    }
                    else if (i == TaskStatus.NOTFINISH)
                    {
                        item.statusstr = "<span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>";
                    }
                    else
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                        item.progress = 0;
                    }

                    if (i == TaskStatus.EXECUTING)
                    {
                        rst.Add(item);
                    }
                }
            }

            return rst;
        }
        public List<TaskInfo> GetUnacceptTask()
        {
            List<TaskInfo> rst = new List<TaskInfo>();
            long myid = CommonModel.GetSessionUserID();

            var filterlist = db.tbl_tasks
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .ToList();

            var statuslist = db.tbl_taskstatus
                .Where(m => m.deleted == 0 && m.receiverid == myid)
                .ToList();

            foreach (var item in filterlist)
            {
                string[] rids = item.receiver.Split(',');

                if (rids.Contains(myid.ToString()))
                {
                    TaskStatus i = TaskStatus.NOTACCPET;
                    var status = statuslist.Where(m => m.taskid == item.uid && m.receiverid == myid).FirstOrDefault();

                    if (status != null)
                    {
                        i = (TaskStatus)status.status;
                        item.accepttime = status.accepttime;
                        item.finishtime = status.finishtime;
                        item.status = (TaskStatus)status.status;
                    }

                    if (i == TaskStatus.NOTACCPET)
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                        item.progress = 0;
                    }
                    else if (i == TaskStatus.EXECUTING)
                    {
                        item.progress = 50;
                        item.statusstr = "<span class='label label-large label-info arrowed-right arrowed-in'>执行中</span>";
                    }
                    else if (i == TaskStatus.FINISH)
                    {
                        item.statusstr = "<span class='label label-large label-success arrowed-in arrowed-in-right'>已完成</span>";
                        item.progress = 100;
                    }
                    else if (i == TaskStatus.NOTFINISH)
                    {
                        item.statusstr = "<span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>";
                    }
                    else
                    {
                        item.statusstr = "<span class='label label-large label-important arrowed'>待接收</span>";
                        item.progress = 0;
                    }

                    if (i == TaskStatus.NOTACCPET)
                    {
                        rst.Add(item);
                    }
                }
            }

            return rst;
        }

        public List<TaskInfo> MyDeadlineTask(long myid)
        {
            List<TaskInfo> rst = new List<TaskInfo>();
           // long myid = CommonModel.GetSessionUserID();
            var nowtime = DateTime.Now;
            var nowday = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0).AddDays(1);

            var filterlist = db.tbl_tasks
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new TaskInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    sendpart = m.doc.sendpart,
                    receiver = m.doc.receiver,
                    title = m.doc.title,
                    contents = m.doc.contents,
                    attachname = m.doc.attachname,
                    attachpath = m.doc.attachpath,
                    attachsize = m.doc.attachsize,
                    createtime = m.doc.createtime,
                    publishtime = m.doc.publishtime,
                    starttime = m.doc.starttime,
                    endtime = m.doc.endtime
                })
                .ToList();

            var statuslist = db.tbl_taskstatus
                .Where(m => m.deleted == 0 &&
                    m.receiverid == myid)
                .ToList();

            foreach (var item in filterlist)
            {
                string[] receiverids = item.receiver.Split(',');

                if (receiverids.Contains(myid.ToString()))
                {
                    

                    var status = statuslist.Where(m => m.taskid == item.uid && m.receiverid == myid).FirstOrDefault();

                    if (status != null)
                    {

                        if ((status.status == (int)TaskStatus.EXECUTING || status.status == (int)TaskStatus.NOTACCPET) && item.endtime.Day == nowtime.Day)
                        {
                            rst.Add(item);
                        }
     
                    }
                    else
                    {
                        if (item.endtime.Day == nowtime.Day)
                        {
                            rst.Add(item);
                        }
                    }
                }
            }
            return rst;
        }
    }
}