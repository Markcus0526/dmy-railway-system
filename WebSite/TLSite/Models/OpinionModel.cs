using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class OpinionInfo
    {
        public long uid { get; set; }
        public long senderid { get; set; }
        public string sendername { get; set; }
        public string sendpart { get; set; }
        public long receiverid { get; set; }
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
        public string feedback { get; set; }
         public DateTime createtime { get; set; }
    }

    public class OpinionModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        UserModel userModel = new UserModel();

        #region 任务

        public string InsertOpinion(int examkind, string title, string contents)
        {
            string rst = "";
            long senderid = CommonModel.GetSessionUserID();

            try
            {
                var senderinfo = userModel.GetUserById(senderid);

                if (senderinfo == null)
                {
                    return "非法操作";
                }

                tbl_opinion newitem = new tbl_opinion();

                if (examkind == 0)  //段级
                {
                    newitem.receivepartid = 0;
                }
                else if (examkind == 1) //车队
                {
                    if (senderinfo.userkind == (int)UserKind.EXECUTIVE && senderinfo.exectype == ExecType.SectorExec)
                    {
                        return "科室干部无法提交车队诉求";
                    } else if (senderinfo.userkind == (int)UserKind.EXECUTIVE && senderinfo.exectype == ExecType.TeamExec)
                    {
                        newitem.receivepartid = senderinfo.execparentid;
                    }
                    else
                    {
                        GroupModel gmodel = new GroupModel();
                        var groupinfo = gmodel.GetGroupById(senderinfo.crewgroupid);
                        newitem.receivepartid = groupinfo.teamid;
                    }
                }

                newitem.title = title;
                newitem.senderid = senderid;
                newitem.contents = contents;
                newitem.createtime = DateTime.Now;

                db.tbl_opinions.InsertOnSubmit(newitem);
                db.SubmitChanges();

//                 tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
//                 newnoticelog.logtype = (int)NoticeType.TASK;
//                 newnoticelog.title = "收到新职工诉求了";
//                 newnoticelog.readerid = receivepartid;
//                 newnoticelog.createtime = DateTime.Now;
//                 newnoticelog.checklogid = newitem.uid;
// 
//                 db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);


//                 db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public List<OpinionInfo> GetOpinionList()
        {
            List<OpinionInfo> rst = new List<OpinionInfo>();
            long myid = CommonModel.GetSessionUserID();

            var userinfo = userModel.GetUserById(myid);
            long partid = 0;

//             if (userinfo.opinionmanage == 0)
//             {
//                 return rst;
//             }

            if (userinfo.userkind == (int)UserKind.ADMIN)
            {
                partid = 0;
            }
            else if (userinfo.userkind == (int)UserKind.EXECUTIVE && userinfo.exectype == ExecType.TeamExec)
            {
                if (userinfo.opinionmanage == 0)
                {
                    return rst;
                }
                else
                {
                    partid = userinfo.execparentid;
                }
            }
            else
            {
                partid = -1;
            }

            rst = db.tbl_opinions
                .Where(m => m.deleted == 0 && m.receivepartid == userinfo.execparentid)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new OpinionInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    receiverid = m.doc.receivepartid,
                    sendername = m.sender.realname,
                    title = m.doc.title,
                    createtime = m.doc.createtime
                })
                .ToList();

            return rst;
        }

        public List<OpinionInfo> GetMyOpinionList(long myid)
        {
            List<OpinionInfo> rst = new List<OpinionInfo>();

            rst = db.tbl_opinions
                .Where(m => m.deleted == 0 && m.senderid==myid)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new OpinionInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    contents=m.doc.contents,
                    title = m.doc.title,
                    feedback=m.doc.feedback,
                    createtime = m.doc.createtime
                })
                .ToList();

            return rst;
        }

        public JqDataTableInfo GetOpinionDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<OpinionInfo> filteredCompanies;

            List<OpinionInfo> alllist = GetOpinionList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable &&(c.sendername.ToLower().Contains(param.sSearch.ToLower())||c.title.ToLower().Contains(param.sSearch.ToLower())));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<OpinionInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.sendername :
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
                c.sendername,
                c.receiverid==0?"段级":"车队",
                c.title,
                String.Format("{0:yyyy-MM-dd}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteOpinion(long[] items)
        {
            try
            {
                var dellist = db.tbl_opinions
                    .Where(m => items.Contains(m.uid))
                    .ToList();

                db.tbl_opinions.DeleteAllOnSubmit(dellist);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        public OpinionInfo GetOpinionInfo(long id)
        {

            var rst = db.tbl_opinions
                .Where(m => m.deleted == 0 && m.uid == id)
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
                .Select(m => new OpinionInfo
                {
                    uid = m.doc.uid,
                    senderid = m.doc.senderid,
                    sendername = m.sender.realname,
                    title = m.doc.title,
                    contents=m.doc.contents,
                    feedback=m.doc.feedback,
                    createtime = m.doc.createtime
                })
                .FirstOrDefault();

            return rst;
        }
        public string UpdateFeedback(long uid,string feedback)
        {
            var rst = "";
            var opinion = db.tbl_opinions.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();
            if (opinion == null)
            {
                rst = "该诉求已被删除，不需要处理！";
                return rst;
            }
            else
            {
                opinion.feedback = feedback;

                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    rst = "处理失败";
                }
            }

            return rst;

        }
        #endregion
    }
}