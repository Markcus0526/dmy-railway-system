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
    public class CheckLogInfo
    {
        public long uid { get; set; }

        //Check Info
        public long infoid { get; set; }

        public string checkinfo { get; set; }
        public string checkno { get; set; }
        public int checktype { get; set; }
        public string logtype { get; set; }
        public double chkpoint { get; set; }
        public string relpoint { get; set; }
        public string category { get; set; }

        public long teamid { get; set; }
        public string teamname { get; set; }
        public long groupid { get; set; }
        public string groupname { get; set; }
        public long crewid { get; set; }
        public string crewname { get; set; }
        public string crewno { get; set; }
        public string rolename { get; set; }
        public string crewimg { get; set; }
        public long? relcrewid { get; set; }
        public string relcrewname { get; set; }
        public string relcrewno { get; set; }
        public long checkerid { get; set; }
        public string checkersector { get; set; }
        public string checkername { get; set; }
        public string checkerimg { get; set; }
        public string checkpart { get; set; }
        public DateTime checktime { get; set; }
        public DateTime dutytime { get; set; }
        public string contents { get; set; }
        public string imgurl { get; set; }
        public string checklevel { get; set; }
        public string recievename { get; set; }
        public string recievepart { get; set; }
        public string trainno { get; set; }
        public long trainid { get; set; }
    }


    public class editable
    {

        public DateTime endtime { get; set; }
        public int forbid { get; set; }
    }
    public class JudgeChartInfo
    {
        public JudgeChartInfo()
        {
            series = new List<JudgeChartSeriesInfo>();
        }

        public List<string> categories { get; set; }
        public List<JudgeChartSeriesInfo> series { get; set; }
    }

    public class JudgeChartSeriesInfo
    {
        public JudgeChartSeriesInfo()
        {
            data = new List<string>();
        }

        public string name { get; set; }
        public List<string> data { get; set; }
        public int totlecount { get; set; }
    }

    public class JudgeModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        UserModel userModel = new UserModel();

        public string InsertJudge(string starttime, string checktime, long teamid, long groupid, long crewid, long? relcrewid,
            long selcheckid, string esccontent, string imgurl, string trainno, string checkersec, string checkername, string checklevel, string checkerid)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
            long routid = 0;
            long trainnoo = 0;
            
            if (trainno != "0" && trainno!=null)
            {
                trainnoo = long.Parse(trainno);
                routid = db.tbl_trainnos.Where(m => m.uid == trainnoo).Select(m => m.routeid).ToList()[0];
            }
            
            long checkeridd;
            string groupname="";
            if (groupid!=0)
            {
                groupname=db.tbl_traingroups.Where(m=>m.uid==groupid).Select(m=>m.groupname).ToList()[0];
            }
            var recivename="";
            if (crewid!=0)
            {
                recivename = db.tbl_users.Where(m => m.uid == crewid).Select(m => m.realname).ToList()[0];
            }

            if (checkerid!=""&&checkerid!=null)
            {
                checkeridd = long.Parse(checkerid);
            }
            else
            {
                checkeridd = 0;
            }

            DateTime checkd = new DateTime();
            DateTime startd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                tbl_checklog newitem = new tbl_checklog();
                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
                newitem.crewid = crewid;
                newitem.crewrelid = relcrewid;
                newitem.checkerid = checkeridd;
                newitem.checktime = checkd;
                newitem.dutytime = startd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;
                newitem.routid = routid;

                newitem.logtype = "两违考核";
                newitem.checklevel = checklevel;
                newitem.checkexecparent = checkersec;
                newitem.checkername =checkername;
                newitem.trainno = trainnoo;
                newitem.receivepart = groupname;
                newitem.recievename =recivename ;
                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                }

                newitem.imgurl = savepath + imgurl;

                db.tbl_checklogs.InsertOnSubmit(newitem);
                db.SubmitChanges();

                tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                newnoticelog.logtype = (int)NoticeType.JUDGE;
                newnoticelog.title = "收到新考核了";
                newnoticelog.readerid = crewid;
                newnoticelog.createtime = DateTime.Now;
                newnoticelog.checklogid = newitem.uid;

                db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public string AddJduge()
        {
            long createid = CommonModel.GetSessionUserID();
            var list = db.templogs.Where(m => m.createid == createid&&m.logtype.Contains("两违")&&m.checklevel!="激励加分").ToList();
            List<tbl_checklog> stroelist = list.Select(m=>new tbl_checklog
                                             {
                                                 logtype = m.logtype,
                                                 infoid=m.infoid,
                                                 teamid=m.teamid,
                                                 groupid=m.groupid,
                                                 crewid=m.crewid,
                                                 crewrelid=m.crewrelid,
                                                 checktime=m.checktime, 
                                                 dutytime=m.dutytime,
                                                 contents=m.contents,
                                                 imgurl=m.imgurl,
                                                 createtime=m.createtime,
                                                 routid=m.routid,
                                                 checklevel=m.checklevel,
                                                 checkexecparent=m.checkexecparent,
                                                 trainno=m.trainno,
                                                 checkername=m.checkername,
                                                 receivepart = m.receivepart,
                                                 recievename=m.recievename,
                                                 deleted=m.deleted,
                                             }).ToList();
           // List<tbl_checklog> addlist = new List<tbl_checklog>();
            try
            {
                db.tbl_checklogs.InsertAllOnSubmit(stroelist);
                db.SubmitChanges();
                foreach (var n in stroelist)
                {
                    tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                    newnoticelog.logtype = (int)NoticeType.JUDGE;
                    newnoticelog.title = "收到新考核了";
                    newnoticelog.createtime = DateTime.Now;
                    newnoticelog.checklogid = n.uid;
                    newnoticelog.readerid = n.crewid;
                    db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                    db.SubmitChanges();
                }
                var delete = list;
                db.templogs.DeleteAllOnSubmit(delete);
                db.SubmitChanges();
                return "success";
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }

        public string AddJdugeofCombine()
        {
            long createid = CommonModel.GetSessionUserID();
            var list = db.templogs.Where(m => m.createid == createid && m.logtype.Contains("结合部") && m.checklevel != "激励加分").ToList();
            List<tbl_checklog> stroelist = list.Select(m => new tbl_checklog
            {
                logtype = m.logtype,
                infoid = m.infoid,
                teamid = m.teamid,
                groupid = m.groupid,
                crewid = m.crewid,
                crewrelid = m.crewrelid,
                checktime = m.checktime,
                dutytime = m.dutytime,
                contents = m.contents,
                imgurl = m.imgurl,
                createtime = m.createtime,
                routid = m.routid,
                checklevel = m.checklevel,
                checkexecparent = m.checkexecparent,
                trainno = m.trainno,
                checkername = m.checkername,
                receivepart = m.receivepart,
                recievename = m.recievename,
                deleted = m.deleted,
            }).ToList();
            // List<tbl_checklog> addlist = new List<tbl_checklog>();
            try
            {
                db.tbl_checklogs.InsertAllOnSubmit(stroelist);
                db.SubmitChanges();
                foreach(var n in stroelist ){
                   tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                   newnoticelog.logtype = (int)NoticeType.JUDGE;
                   newnoticelog.title = "收到新考核了";
                   newnoticelog.createtime = DateTime.Now;
                   newnoticelog.checklogid = n.uid;
                   db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                   db.SubmitChanges();
                }
                var delete = list;
                db.templogs.DeleteAllOnSubmit(delete);
                db.SubmitChanges();
                return "success";
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }

        public string AddJdugeofAdd()
        {
            long createid = CommonModel.GetSessionUserID();
            var list = db.templogs.Where(m => m.createid == createid &&  m.checklevel == "激励加分").ToList();
            List<tbl_checklog> stroelist = list.Select(m => new tbl_checklog
            {
                logtype = m.logtype,
                infoid = m.infoid,
                teamid = m.teamid,
                groupid = m.groupid,
                crewid = m.crewid,
                crewrelid = m.crewrelid,
                checktime = m.checktime,
                dutytime = m.dutytime,
                contents = m.contents,
                imgurl = m.imgurl,
                createtime = m.createtime,
                routid = m.routid,
                checklevel = m.checklevel,
                checkexecparent = m.checkexecparent,
                trainno = m.trainno,
                checkername = m.checkername,
                receivepart = m.receivepart,
                recievename = m.recievename,
                deleted = m.deleted,
            }).ToList();
            // List<tbl_checklog> addlist = new List<tbl_checklog>();
            try
            {
                db.tbl_checklogs.InsertAllOnSubmit(stroelist);
                db.SubmitChanges();
                foreach (var n in stroelist)
                {
                    tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                    newnoticelog.logtype = (int)NoticeType.JUDGE;
                    newnoticelog.title = "收到新考核了";
                    newnoticelog.createtime = DateTime.Now;
                    newnoticelog.readerid=n.crewid;
                    newnoticelog.checklogid = n.uid;
                    db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                    db.SubmitChanges();
                }
                var delete = list;
                db.templogs.DeleteAllOnSubmit(delete);
                db.SubmitChanges();
                return "success";
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        //flag标示是否联挂扣分，0为不联挂
        public string InsertTemplog(int flag,string starttime, string checktime, long teamid, long groupid, long crewid, long? relcrewid,
          long selcheckid, string esccontent, string imgurl, string trainno, string checkersec, string checkername, string checklevel, string checkerid)
        {
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
            long routid = 0;
            long trainnoo = 0;
            var rst = "";

            long createid= CommonModel.GetSessionUserID();
            if (trainno != "0" && trainno != null)
            {
                trainnoo = long.Parse(trainno);
                routid = db.tbl_trainnos.Where(m => m.uid == trainnoo).Select(m => m.routeid).ToList()[0];
            }

            long checkeridd;
            string groupname = "";
            if (groupid != 0)
            {
                groupname = db.tbl_traingroups.Where(m => m.uid == groupid).Select(m => m.groupname).ToList()[0];
            }
            var recivename = "";
            if (crewid != 0)
            {
                recivename = db.tbl_users.Where(m => m.uid == crewid).Select(m => m.realname).ToList()[0];
            }

            if (checkerid != "" && checkerid != null)
            {
                checkeridd = long.Parse(checkerid);
            }
            else
            {
                checkeridd = 0;
            }

            DateTime checkd = new DateTime();
            DateTime startd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }

            if (db.tbl_users.Where(m=>m.uid==crewid).Select(m=>m.crewrole).FirstOrDefault()==db.tbl_crewroles.Where(m=>m.rolename=="列车长").Select(m=>m.uid).FirstOrDefault())
            {
                relcrewid = 0;
            }
            try
            {
                templog newitem = new templog();
                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
                newitem.crewid = crewid;
                if (flag == 0)
                {
                    newitem.crewrelid = 0;
                }
                else
                {
                    newitem.crewrelid = checklevel == "班组" ? 0 : relcrewid;
                }
                newitem.checkerid = checkeridd;
                newitem.checktime = checkd;
                newitem.dutytime = startd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;
                newitem.routid = routid;
                newitem.createid = createid;
                newitem.logtype = "两违考核";
                newitem.checklevel = checklevel;
                newitem.checkexecparent = checkersec;
                newitem.checkername = checkername;
                newitem.trainno = trainnoo;
                newitem.receivepart = groupname;
                newitem.recievename = recivename;
                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    newitem.imgurl = savepath + imgurl;
                }


                db.templogs.InsertOnSubmit(newitem);
                db.SubmitChanges();

//                   db.templogs.Join(db.tbl_traingroups, m => m.groupid, l => l.uid, (m, l) => new { log = m, group = l })
//                                  .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { log = m, team = l })
//                                  .Join(db.tbl_users, m => m.log.log.crewid, l => l.uid, (m, l) => new { log = m, user = l })
//                                  .Join(db.tbl_checkinfos, m => m.log.log.log.infoid, l => l.uid, (m, l) => new { log = m, info = l })
//                                  .Select(m => new CheckLogInfo
//                                  {
//                                      teamname = m.log.log.team.teamname,
//                                      crewno = m.log.user.crewno,
//                                      crewname = m.log.user.realname,
//                                      groupname = m.log.log.log.group.groupname,
//                                      rolename = db.tbl_crewroles.Where(l => l.uid == m.log.user.crewrole).Select(l => l.rolename).FirstOrDefault(),
//                                      chkpoint = m.info.chkpoint,
//                                      relcrewid = m.log.log.log.log.crewrelid,
//                                      relcrewname = db.tbl_users.Where(l => l.uid == m.log.log.log.log.crewrelid).Select(l => l.realname).FirstOrDefault(),
//                                      relcrewno = db.tbl_users.Where(l => l.uid == m.log.log.log.log.crewrelid).Select(l => l.crewno).FirstOrDefault(),
//                                      checkno=m.info.checkno,
//                                      checkpart=m.log.log.log.log.checkexecparent,                                            
//                                      checkername=m.log.log.log.log.checkername,
//                                      checktime=m.log.log.log.log.checktime,
//                                  }
//                                  ).ToList();

            }
            catch (System.Exception ex)
            {
                rst = "添加失败,错误原因"+ex;
            }

            return rst;
        }
        //修改两违考核
        public string UpdateJudge(long uid, string checktime, long teamid, long groupid, long crewid, long? relcrewid,
     long selcheckid, string esccontent, string imgurl, string checkersec, string checkername, string checklevel, string checkerid)
        {
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
           
            var rst = "";

            long createid = CommonModel.GetSessionUserID();
            

            long checkeridd;
            string groupname = "";
            if (groupid != 0)
            {
                groupname = db.tbl_traingroups.Where(m => m.uid == groupid).Select(m => m.groupname).ToList()[0];
            }
            var recivename = "";
            if (crewid != 0)
            {
                recivename = db.tbl_users.Where(m => m.uid == crewid).Select(m => m.realname).ToList()[0];
            }

            if (checkerid != "" && checkerid != null)
            {
                checkeridd = long.Parse(checkerid);
            }
            else
            {
                checkeridd = 0;
            }

            DateTime checkd = new DateTime();
          

            try
            {
                checkd = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }

            if (db.tbl_users.Where(m => m.uid == crewid).Select(m => m.crewrole).FirstOrDefault() == db.tbl_crewroles.Where(m => m.rolename == "列车长").Select(m => m.uid).FirstOrDefault())
            {
                relcrewid = 0;
            }
            try
            {
                tbl_checklog newitem = db.tbl_checklogs.Where(m => m.uid == uid).FirstOrDefault();
                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
                newitem.crewid = crewid;
                GroupModel groupmodel = new GroupModel();

                if (groupmodel.CheckIfDailiGroup(groupid))
                {
                    newitem.crewrelid=0;
                }
                else
                {
                    newitem.crewrelid = checklevel == "班组" ? 0 : relcrewid;                    
                }
                newitem.checkerid = checkeridd;
                newitem.checktime = checkd;
                //newitem.dutytime = startd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;
               
                
               
                newitem.checklevel = checklevel;
                newitem.checkexecparent = checkersec;
                newitem.checkername = checkername;
               
                newitem.receivepart = groupname;
                newitem.recievename = recivename;
                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    newitem.imgurl = savepath + imgurl;
                }
                db.SubmitChanges();

            }
            catch (System.Exception ex)
            {
            }

            return rst;
        }
        public List<CheckLogInfo> GetStoreList()
        {
            long creatid = CommonModel.GetSessionUserID();

            var rst = db.templogs.Join(db.tbl_traingroups, m => m.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                                 .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                                 .Join(db.tbl_users, m => m.log.log.crewid, l => l.uid, (m, l) => new { log = m, user = l })
                                 .Join(db.tbl_checkinfos, m => m.log.log.log.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                                 .Where(m => m.log.log.log.log.createid == creatid && m.log.log.log.log.logtype.Contains("两违") && m.log.log.log.log.checklevel != "激励加分")
                                 .Select(m => new CheckLogInfo
                                 {   
                                     uid=m.log.log.log.log.uid,
                                     checklevel = m.log.log.log.log.checklevel,
                                     teamname = m.log.log.team.teamname,
                                     crewno = m.log.user.crewno,
                                     crewname = m.log.user.realname,
                                     groupname = m.log.log.log.group.groupname,
                                     rolename = db.tbl_crewroles.Where(l => l.uid == m.log.user.crewrole).Select(l => l.rolename).FirstOrDefault(),
                                     chkpoint = m.info.chkpoint,
                                     relcrewid = m.log.log.log.log.crewrelid,
                                     relcrewname = db.tbl_users.Where(l => l.uid == m.log.log.log.log.crewrelid).Select(l => l.realname).FirstOrDefault(),
                                     relcrewno = db.tbl_users.Where(l => l.uid == m.log.log.log.log.crewrelid).Select(l => l.crewno).FirstOrDefault(),
                                     relpoint = m.info.relpoint,
                                     checkno=m.info.checkno,
                                     checkpart=m.log.log.log.log.checkexecparent,                                            
                                     checkername=m.log.log.log.log.checkername,
                                     checktime=m.log.log.log.log.checktime,
                                     contents = m.log.log.log.log.contents
                                 }
                                 ).ToList();
            return rst;
        }
        public List<CheckLogInfo> GetStoreListofCombine()
        {
            long creatid = CommonModel.GetSessionUserID();
           // var combinelist = db.templogs.Where(m => m.logtype.Contains("结合部") && m.createid == creatid).ToList();
            var rst = db.templogs.Join(db.tbl_traingroups, m => m.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                                 .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                                 .Join(db.tbl_checkinfos, m => m.log.log.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                                 .Where(m => m.log.log.log.createid == creatid && m.log.log.log.logtype.Contains("结合部"))
                                 .Select(m => new CheckLogInfo
                                 {
                                     uid = m.log.log.log.uid,
                                     teamname = m.log.team.teamname,
                                     recievename=m.log.log.log.recievename,
                                     recievepart = m.log.log.log.receivepart,
                                     contents=m.log.log.log.contents,
                                     groupname = m.log.log.group.groupname,
                                     chkpoint = m.info.chkpoint,
                                     checkno = m.info.checkno,
                                     checkpart = m.log.log.log.checkexecparent,
                                     checkername = m.log.log.log.checkername,
                                     checktime = m.log.log.log.checktime,
                                 }
                                 ).ToList();
            return rst;
        }

        public List<CheckLogInfo> GetStoreListofAdd()
        {
            long creatid = CommonModel.GetSessionUserID();
            // var combinelist = db.templogs.Where(m => m.logtype.Contains("结合部") && m.createid == creatid).ToList();
            var rst = db.templogs.Join(db.tbl_traingroups, m => m.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                                 .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                                 .Join(db.tbl_users, m => m.log.log.crewid, l => l.uid, (m, l) => new { log = m, user = l })
                                 .Join(db.tbl_checkinfos, m => m.log.log.log.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                                 .Where(m => m.log.log.log.log.createid == creatid &&  m.log.log.log.log.checklevel == "激励加分")
                                 .Select(m => new CheckLogInfo
                                 {
                                     uid = m.log.log.log.log.uid,
                                     teamname = m.log.log.team.teamname,
                                     crewno = m.log.user.crewno,
                                     crewname = m.log.user.realname,
                                     groupname = m.log.log.log.group.groupname,
                                     rolename = db.tbl_crewroles.Where(l => l.uid == m.log.user.crewrole).Select(l => l.rolename).FirstOrDefault(),
                                     chkpoint = m.info.chkpoint,
                                     checkno = m.info.checkno,
                                     checkpart = m.log.log.log.log.checkexecparent,
                                     checkername = m.log.log.log.log.checkername,
                                     checktime = m.log.log.log.log.checktime,
                                     contents = m.log.log.log.log.contents,
                                 }
                                 ).ToList();
            return rst;
        }

        public editable getDutyendtime(DateTime createtime, long groupid)
        {
            var endtime = db.tbl_duties.Where(m => m.starttime <= createtime && m.endtime >= createtime && m.groupid == groupid)
                                     .Select(m => new editable
                                     {
                                         endtime = m.endtime,
                                         forbid = m.forbide
                                     }).FirstOrDefault();
                                     

            return endtime;
        }
        //结合部考核添加
        public string InsertCombineJudge(string checktime, long teamid, long groupid,
           long selcheckid, string esccontent, string imgurl, string trainno, string checkersec, string checkername, string checklevel, string checkerid, string crewsector, string crewname)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            long createid = CommonModel.GetSessionUserID();
            long routid = 0;
            long trainnoo = 0;
            
            if (trainno != "0" && trainno!="")
            {
                trainnoo = long.Parse(trainno);
                routid = db.tbl_trainnos.Where(m => m.uid == trainnoo).Select(m => m.routeid).ToList()[0];
            }
            
            long checkeridd;

            if (checkerid != "" && checkerid != null)
            {
                checkeridd = long.Parse(checkerid);
            }
            else
            {
                checkeridd = 0;
            }
            DateTime checkd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                templog newitem = new templog();

                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
                newitem.crewid = 0;
                newitem.checkerid = checkeridd;
                newitem.checktime = checkd;
                newitem.dutytime = checkd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;
                newitem.routid = routid;
                newitem.createid = createid;
                newitem.logtype = "结合部考核";
                newitem.checklevel = checklevel;
                newitem.checkexecparent = checkersec;
                newitem.checkername = checkername;
                newitem.trainno = trainnoo;
                newitem.receivepart = crewsector;
                newitem.recievename = crewname;

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    newitem.imgurl = savepath + imgurl;

                }


                db.templogs.InsertOnSubmit(newitem);
                db.SubmitChanges();

//                 tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
//                 newnoticelog.logtype = (int)NoticeType.JUDGE;
//                 newnoticelog.title = "收到新考核了";
//                 newnoticelog.createtime = DateTime.Now;
//                 newnoticelog.checklogid = newitem.uid;
// 
//                 db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
// 
//                 db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }
        //修改结合部考核
        public string UpdateCombineJudge(long uid, string checktime, long teamid, long groupid,
           long selcheckid, string esccontent, string imgurl , string checkersec, string checkername, string checklevel, string checkerid, string crewsector, string crewname)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            long createid = CommonModel.GetSessionUserID();
           // long routid = 0;
           // long trainnoo = 0;

            //if (trainno != "0" && trainno != "")
            //{
            //    trainnoo = long.Parse(trainno);
            //    routid = db.tbl_trainnos.Where(m => m.uid == trainnoo).Select(m => m.routeid).ToList()[0];
            //}

            long checkeridd;

            if (checkerid != "" && checkerid != null)
            {
                checkeridd = long.Parse(checkerid);
            }
            else
            {
                checkeridd = 0;
            }
            DateTime checkd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                tbl_checklog newitem = db.tbl_checklogs.Where(m => m.uid == uid).FirstOrDefault() ;

                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
              
                newitem.checkerid = checkeridd;
                newitem.checktime = checkd;
                newitem.dutytime = checkd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;

                // newitem.crewid = 0;
                //newitem.routid = routid;
                //newitem.createid = createid;
              //  newitem.logtype = "结合部考核";
                newitem.checklevel = checklevel;
                newitem.checkexecparent = checkersec;
                newitem.checkername = checkername;
               // newitem.trainno = trainnoo;
                newitem.receivepart = crewsector;
                newitem.recievename = crewname;

                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    newitem.imgurl = savepath + imgurl;

                }


               // db.templogs.InsertOnSubmit(newitem);
                db.SubmitChanges();

                //                 tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                //                 newnoticelog.logtype = (int)NoticeType.JUDGE;
                //                 newnoticelog.title = "收到新考核了";
                //                 newnoticelog.createtime = DateTime.Now;
                //                 newnoticelog.checklogid = newitem.uid;
                // 
                //                 db.tbl_sysnoticelogs.InsertOnSubmit(newnoticelog);
                // 
                //                 db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }
        //激励积分录入
        public string InsertAddScore(string checktime, long teamid,long groupid,string groupname, long crewid,long selcheckid, string esccontent, string imgurl)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

            long routid = 0;
            long trainno = 0;
            var checkerid= CommonModel.GetSessionUserID();
            string checkexecparent = "";
            var checker=db.tbl_users.Where(m=>m.uid==checkerid).FirstOrDefault();
            var checkername=checker.realname;
            if (checker.userkind==0)
            {
                checkexecparent = "管理员";
            }
            string crewname=db.tbl_users.Where(m=>m.uid==crewid).Select(m=>m.realname).ToList()[0];
            DateTime checkd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                templog newitem = new templog();

                newitem.infoid = selcheckid;
                newitem.teamid = teamid;
                newitem.groupid = groupid;
                newitem.crewid = crewid;
                newitem.checkerid = checkerid;
                newitem.checkexecparent = checkexecparent;
                newitem.checktime = checkd;
                newitem.dutytime = checkd;
                newitem.createtime = DateTime.Now;
                newitem.contents = esccontent;
                newitem.routid = routid;
                newitem.createid = checkerid;

                newitem.logtype = "两违考核";
                newitem.checklevel = "激励加分";
                newitem.checkername = checkername;
                newitem.trainno = trainno;
                newitem.receivepart = groupname;
                newitem.recievename = crewname;
                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    newitem.imgurl = savepath + imgurl;

                }


                db.templogs.InsertOnSubmit(newitem);
                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }
        //激励积分修改
        public string UpdateAddScore(long uid,string checktime, long teamid, long groupid, string groupname, long crewid, long selcheckid, string esccontent, string imgurl)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);

           // long routid = 0;
            //long trainno = 0;
            var checkerid = CommonModel.GetSessionUserID();
            //string checkexecparent = "";
           // var checkername = db.tbl_users.Where(m => m.uid == checkerid).Select(m => m.realname).ToList()[0];
            string checkexecparent = "";
            var checker = db.tbl_users.Where(m => m.uid == checkerid).FirstOrDefault();
            var checkername = checker.realname;
            if (checker.userkind == 0)
            {
                checkexecparent = "管理员";
            }
            string crewname = db.tbl_users.Where(m => m.uid == crewid).Select(m => m.realname).ToList()[0];
            DateTime checkd = new DateTime();

            try
            {
                checkd = DateTime.Parse(checktime);
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                tbl_checklog edititem = db.tbl_checklogs.Where(m => m.uid == uid).FirstOrDefault();


                edititem.checktime = checkd;
                edititem.teamid = teamid;
                edititem.groupid = groupid;
                edititem.receivepart = groupname;
                edititem.recievename = crewname; 

                edititem.crewid = crewid;
                edititem.infoid = selcheckid;
                edititem.contents = esccontent;

                edititem.checkerid = checkerid;
                edititem.checkername = checkername;
                edititem.checkexecparent = checkexecparent;
                
                //edititem.checktime = checkd;
               // edititem.dutytime = checkd;

                edititem.createtime = DateTime.Now;
                
               // edititem.routid = routid;
              

              
                if (File.Exists(orgbase + imgurl))
                {
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }
                    File.Move(orgbase + imgurl, targetbase + imgurl);
                    edititem.imgurl = savepath + imgurl;

                }


                //db.templogs.InsertOnSubmit(edititem);
                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }
        public List<CheckLogInfo> GetCheckLogList(DateTime starttime, DateTime endtime, long teamid, long groupid, long crewid)
        {
            List<CheckLogInfo> rst = new List<CheckLogInfo>();

            rst = db.tbl_checklogs
                .Where(m => m.deleted == 0 && starttime <= m.checktime && m.checktime < endtime.AddDays(1))
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                .Join(db.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                .OrderByDescending(m => m.log.log.log.log.log.checktime)
                .Select(m => new CheckLogInfo
                {
                    uid = m.log.log.log.log.log.uid,
                    infoid = m.log.log.log.log.log.infoid,
                    checkinfo = m.log.log.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.log.log.chkinfo.checkno,
                    checktype = m.log.log.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.log.log.chkinfo.relpoint,
                    category = m.log.log.log.log.chkinfo.category,
                    teamid = m.log.log.log.team.uid,
                    teamname = m.log.log.log.team.teamname,
                    groupid = m.log.log.log.log.log.groupid,
                    groupname = m.log.log.group.groupname,
                    crewid = m.log.crew.uid,
                    crewname = m.log.crew.realname,
                    relcrewid = m.log.log.log.log.log.crewrelid,
                    relcrewname = "无",
                    checkerid = m.log.log.log.log.log.checkerid,
                    checkername = m.checker.realname,
                    checktime = m.log.log.log.log.log.checktime,
                    dutytime = m.log.log.log.log.log.dutytime,
                    imgurl = m.log.log.log.log.log.imgurl
                })
                .ToList();

            if (teamid > 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }

            if (groupid > 0)
            {
                rst = rst.Where(m => m.groupid == groupid).ToList();
            }

            if (crewid > 0)
            {
                rst = rst.Where(m => m.crewid == crewid).ToList();
            }

            var relids = rst.Where(m => m.relcrewid != null).Select(m => (long)m.relcrewid).ToList();

            if (relids.Count() > 0) 
            {
                var relcrewlist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && relids.Contains(m.uid)).ToList();

                foreach (var item in rst)
                {
                    if (item.relcrewid != null)
                    {
                        var relinfo = relcrewlist.Where(m => m.uid == (long)item.relcrewid).FirstOrDefault();
                        if (relinfo != null)
                        {
                            item.relcrewname = relinfo.realname;
                        }
                    }
                }
            }

            return rst;
        }

        public List<CheckLogInfo> newGetCheckLogList(DateTime starttime, DateTime endtime, long teamid, string checklevel, String checkno)
        {
            List<CheckLogInfo> rst = new List<CheckLogInfo>();

            rst = db.tbl_checklogs
                .Where(m => m.deleted == 0 && starttime <= m.checktime && m.checktime < endtime.AddDays(1))
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                .Join(db.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                .OrderByDescending(m => m.log.log.log.log.log.checktime)
                .Select(m => new CheckLogInfo
                {
                    uid = m.log.log.log.log.log.uid,
                    infoid = m.log.log.log.log.log.infoid,
                    checkinfo = m.log.log.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.log.log.chkinfo.checkno,
                    checktype = m.log.log.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.log.log.chkinfo.relpoint,
                    category = m.log.log.log.log.chkinfo.category,
                    teamid = m.log.log.log.team.uid,
                    teamname = m.log.log.log.team.teamname,
                    groupid = m.log.log.log.log.log.groupid,
                    groupname = m.log.log.group.groupname,
                    crewid = m.log.crew.uid,
                    crewname = m.log.crew.realname,
                    relcrewid = m.log.log.log.log.log.crewrelid,
                    relcrewname = "无",
                    checkerid = m.log.log.log.log.log.checkerid,
                    checkername = m.checker.realname,
                    checktime = m.log.log.log.log.log.checktime,
                    dutytime = m.log.log.log.log.log.dutytime,
                    imgurl = m.log.log.log.log.log.imgurl,
                    checklevel=m.log.log.log.log.log.checklevel
                })
                .ToList();

            if (teamid > 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }

            if (checklevel!="0")
            {
                rst = rst.Where(m => m.checklevel == checklevel).ToList();
            }

            if (!String.IsNullOrWhiteSpace(checkno))
            {
                rst = rst.Where(m => m.checkno.Contains(checkno)).ToList();
            }

            var relids = rst.Where(m => m.relcrewid != null).Select(m => (long)m.relcrewid).ToList();

            if (relids.Count() > 0)
            {
                var relcrewlist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && relids.Contains(m.uid)).ToList();

                foreach (var item in rst)
                {
                    if (item.relcrewid != null)
                    {
                        var relinfo = relcrewlist.Where(m => m.uid == (long)item.relcrewid).FirstOrDefault();
                        if (relinfo != null)
                        {
                            item.relcrewname = relinfo.realname;
                        }
                    }
                }
            }

            return rst;
        }

        public JqDataTableInfo GetJudgeDataTable(DateTime starttime, DateTime endtime, long teamid, long groupid, long crewid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<CheckLogInfo> filteredCompanies;

            List<CheckLogInfo> alllist = GetCheckLogList(starttime, endtime, teamid, groupid, crewid);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.teamname.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CheckLogInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.teamname :
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
                c.checkinfo,
                c.teamname,
                c.groupname,
                c.crewname,
                c.relcrewname,
                c.imgurl,
                String.Format("{0:yyyy-MM-dd}", c.checktime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public CheckLogInfo GetJudgeDetail(long uid)
        {
            long myid = CommonModel.GetSessionUserID();
            
            CheckLogInfo rst = db.tbl_checklogs
                .Where(m => m.deleted == 0 && m.uid == uid)
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                .Join(db.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                .Select(m => new CheckLogInfo
                {
                    uid = m.log.log.log.log.log.uid,
                    infoid = m.log.log.log.log.log.infoid,
                    checkinfo = m.log.log.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.log.log.chkinfo.checkno,
                    checktype = m.log.log.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.log.log.chkinfo.relpoint,
                    category = m.log.log.log.log.chkinfo.category,
                    contents = m.log.log.log.log.log.contents,
                    teamid = m.log.log.log.team.uid,
                    teamname = m.log.log.log.team.teamname,
                    groupid = m.log.log.log.log.log.groupid,
                    groupname = m.log.log.group.groupname,
                    crewid = m.log.crew.uid,
                    crewname = m.log.crew.realname,
                    crewimg = m.log.crew.imgurl,
                    relcrewid = m.log.log.log.log.log.crewrelid,
                    checkerid = m.log.log.log.log.log.checkerid,
                    checkername = m.checker.realname,
                    checkerimg = m.checker.imgurl,
                    checktime = m.log.log.log.log.log.checktime,
                    dutytime = m.log.log.log.log.log.dutytime,
                    checklevel = m.log.log.log.log.log.checklevel,
                    imgurl = m.log.log.log.log.log.imgurl
                })
                .FirstOrDefault();
                
            if (rst != null)
            {
                var userinfo = userModel.GetUserById(rst.checkerid);
                if (userinfo.userkind == (int)UserKind.EXECUTIVE)
                {
                    if (userinfo.execrole == "科室干部")
                    {
                        var sectorinfo = db.tbl_railsectors.Where(m => m.deleted == 0 && m.uid == userinfo.execparentid).FirstOrDefault();
                        rst.checkpart = sectorinfo.sectorname;

                    }
                    else if (userinfo.execrole == "车队干部")
                    {
                        var teaminfo = db.tbl_railteams.Where(m => m.deleted == 0 && m.uid == userinfo.execparentid).FirstOrDefault();
                        rst.checkpart = teaminfo.teamname;
                    }
                }
                else if (userinfo.userkind == (int)UserKind.ADMIN)
                {
                    rst.checkpart = "管理员";
                }
                else if (userinfo.userkind == (int)UserKind.CREW)
                {
                    rst.checkpart = "列车长";
                }

                if (rst.relcrewid != null)
                {
                    var reluser = userModel.GetUserById((long)rst.relcrewid);
                    rst.relcrewname = reluser.realname;
                }
                if (rst.crewid == myid)
                {
                    var noticelog = db.tbl_sysnoticelogs.Where(m => m.readerid == myid && m.logtype == (int)NoticeType.JUDGE && m.checklogid == uid).FirstOrDefault();
                    if (noticelog != null && noticelog.readtime == null)
                    {
                        noticelog.readtime = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
            }

           

            return rst;
        }

        public JudgeChartInfo GetJudgeChartData(long teamid, string checklevel, string checkno, string starttime, string endtime, string selecetop)
        {
            JudgeChartInfo rst = new JudgeChartInfo();

            DateTime startd = new DateTime(1970, 1, 1);
            DateTime endd = new DateTime(2040, 1, 1);
            int selecetopp = 0;

            try { startd = DateTime.Parse(starttime); }catch { }
            try { endd = DateTime.Parse(endtime); }catch { }
            try { selecetopp = int.Parse(selecetop); }catch { }


            var loglist = newGetCheckLogList(startd, endd, teamid, checklevel, checkno);

            var catelist = loglist.Select(m => m.category).Distinct().OrderBy(m => m).ToList();
            rst.categories = catelist;

            var serieslist = loglist.Select(m => m.checkno).Distinct().OrderBy(m => m);

            foreach (var item in serieslist)
            {
                JudgeChartSeriesInfo seriesinfo = new JudgeChartSeriesInfo();
                seriesinfo.name = item;
                List<int> cont = new List<int>();
                foreach (var ci in catelist)
                {
                    int cnt = loglist.Where(m => m.checkno == item && m.category == ci).Count();

//                     if (cnt > 0)
//                     {
                        seriesinfo.data.Add(cnt.ToString());
                        cont.Add(cnt);
                        // seriesinfo.totlecount=
//                     }
//                     else
//                     {
//                         seriesinfo.data.Add(null);
//                     }
                }
                seriesinfo.totlecount = cont.Sum();
                rst.series.Add(seriesinfo);

                
            }
            if (selecetopp!=0)
            {
                rst.series = rst.series.OrderByDescending(m => m.totlecount).Take(selecetopp).ToList();
            }
            return rst;
        }

        public System.Data.DataTable ExportJudgeList(long teamid, long groupid, long crewid, DateTime starttime, DateTime endtime)
        {
            var products = new System.Data.DataTable("消费记录");
            var alllist = GetCheckLogList(starttime, endtime, teamid, groupid, crewid);

            //TableCell[] header = new TableCell[29];  
            if (products != null)
            {
                
                products.Columns.Add("问题", typeof(string));
                products.Columns.Add("车队", typeof(string));
                products.Columns.Add("班组", typeof(string));
                products.Columns.Add("责任人", typeof(string));
                products.Columns.Add("连挂列车长", typeof(string));
                products.Columns.Add("检测时间", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        item.checkinfo,
                        item.teamname,
                        item.groupname,
                        item.crewname,
                        item.relcrewname,
                        String.Format("{0:yyyy-MM-dd}", item.checktime));
                }
            }

            return products;
        }
        public bool DeleteTempLog(long delid)
        {
            string delSql = "DELETE from templog WHERE uid=" + delid;

            db.ExecuteCommand(delSql);

            return true;
        }


        public List<CheckLogInfo> GetEditCheckLogList(string checktype, string starttime, string endtime, long teamid, long groupid, long crewid, string checkno, string trainno, string keyword,string checkername,string checkexecname)
        {
            List<CheckLogInfo> rst = new List<CheckLogInfo>();
            var loglist = db.tbl_checklogs.Where(m => m.deleted == 0).ToList();
            rst = loglist
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
               // .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                //.Join(db.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                //.Join(db.tbl_trainnos, m => m.log.log.log.log.trainno, l => l.uid, (m, l) => new { log=m, trainno=l})
                .OrderByDescending(m => m.log.log.log.checktime)
                .Select(m => new CheckLogInfo
                {
                    logtype = m.log.log.log.logtype,
                    checklevel = m.log.log.log.checklevel,
                    uid = m.log.log.log.uid,
                    infoid = m.log.log.log.infoid,
                    checkinfo = m.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.chkinfo.checkno,
                    checktype = m.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.chkinfo.relpoint,
                    category = m.log.log.chkinfo.category,

                    teamid = m.log.log.log.teamid,
                    groupid = m.log.log.log.groupid,
                    crewid = m.log.log.log.crewid,

                    groupname = m.group.groupname,
                    teamname = m.log.team.teamname,
                    crewname = m.log.log.log.recievename,
                   // relcrewid = m.log.log.log.crewrelid,
                   // relcrewname = "无",
                   // checkerid = m.log.log.log.log.log.checkerid,

                    checkersector = m.log.log.log.checkexecparent,
                    checkername = m.log.log.log.checkername,
                    checktime = m.log.log.log.checktime,
                    dutytime = m.log.log.log.dutytime,
                    imgurl = m.log.log.log.imgurl,
                    trainid = m.log.log.log.trainno,
                    contents = m.log.log.log.contents
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(starttime))
            {
                try
                {
                    var starttimeF = DateTime.Parse(starttime);
                    rst = rst.Where(m => m.checktime >= starttimeF).ToList();
                }
                catch (System.Exception ex)
                {
                }
            }
            if (!string.IsNullOrWhiteSpace(endtime))
            {
                try
                {
                    var endtimeF = DateTime.Parse(endtime);
                    rst = rst.Where(m => m.checktime <= endtimeF).ToList();
                }
                catch (System.Exception ex)
                {
                }
            }
            if (crewid != 0)
            {
                rst = rst.Where(m => m.crewid == crewid).ToList();
            }
            else if (groupid != 0)
            {
                rst = rst.Where(m => m.groupid == groupid).ToList();
            }
            else if (teamid != 0)
            {
                rst = rst.Where(m => m.teamid == teamid).ToList();
            }

            if (checkno != "" && !string.IsNullOrWhiteSpace(checkno))
            {

                rst = rst.Where(m => m.checkno.Contains(checkno.ToUpper())).ToList();
            }
            if (keyword != "" && !string.IsNullOrWhiteSpace(keyword))
            {
                rst = rst.Where(m => m.contents.Contains(keyword)).ToList();
            }
            if (checkexecname != "" && !string.IsNullOrWhiteSpace(checkexecname))
            {
                rst = rst.Where(m => m.checkersector.Contains(checkexecname)).ToList();
            }
            if (checkername != "" && !string.IsNullOrWhiteSpace(checkername))
            {
                rst = rst.Where(m => m.checkername.Contains(checkername)).ToList();
            }
            //var relids = rst.Where(m => m.relcrewid != null).Select(m => (long)m.relcrewid).ToList();

            //if (relids.Count() > 0)
            //{
            //    var relcrewlist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && relids.Contains(m.uid)).ToList();

            //    foreach (var item in rst)
            //    {
            //        if (item.relcrewid != null)
            //        {
            //            var relinfo = relcrewlist.Where(m => m.uid == (long)item.relcrewid).FirstOrDefault();
            //            if (relinfo != null)
            //            {
            //                item.relcrewname = relinfo.realname;
            //            }
            //        }
                    
            //    }
            //}

            foreach (var r in rst)
            {
                //段以上  按原分

                //班组
                if (r.checklevel == "班组")
                {
                    //批评教育&&联挂考核 25%
                    if (r.chkpoint <= 20 && r.chkpoint > 0)
                    {
                        r.chkpoint = Math.Round((r.chkpoint * 0.25), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值                   
                }

                //车队
                if (r.checklevel == "车队")
                {
                    //批评教育&&联挂考核 50%
                    if (r.chkpoint <= 20 && r.chkpoint > 0)
                    {
                        r.chkpoint = Math.Round((r.chkpoint * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值      
                }

                if (r.trainid==0)
                {
                    r.trainno = "";
                }
                else
                {
                    r.trainno = db.tbl_trainnos.Where(m => m.uid == r.trainid).Select(m => m.trainno).FirstOrDefault();
                }

                if (r.checktype == 0)
                {
                    r.logtype = "激励加分";
                }

                if (r.crewid!=0)
                {
                    r.crewno = db.tbl_users.Where(m => m.uid == r.crewid).Select(m => m.crewno).FirstOrDefault();
                    r.rolename = db.tbl_users.Where(m => m.uid == r.crewid)
                                             .Join(db.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new {rst=m,user=l })
                                             .Select(m => m.user.rolename).FirstOrDefault();
                }
            }
            if (checktype != "" && !string.IsNullOrWhiteSpace(checktype))
            {
                rst = rst.Where(m => m.logtype == checktype).ToList();
            }
            if (trainno != "" && !string.IsNullOrWhiteSpace(trainno))
            {
                rst = rst.Where(m => m.trainno.Contains(trainno)).ToList();
            }
            return rst;
        }


        public bool DeleteCheckLog(long[] items)
        {
            string delSql = "UPDATE tbl_checklog SET deleted = 1 WHERE ";
            string whereSql = "";
            foreach (long uid in items)
            {
                if (whereSql != "") whereSql += " OR";
                whereSql += " uid = " + uid;
            }

            delSql += whereSql;

            db.ExecuteCommand(delSql);

            return true;
        }
#region 编辑考核记录
        public CheckLogInfo GetEditJudgeDetail(long uid)
        {
            long myid = CommonModel.GetSessionUserID();

            CheckLogInfo rst = db.tbl_checklogs
                .Where(m => m.deleted == 0 && m.uid == uid)
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                .Select(m => new CheckLogInfo
                {
                    uid = m.log.log.log.log.uid,
                    infoid = m.log.log.log.log.infoid,
                    checkinfo = m.log.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.log.chkinfo.checkno,
                    checktype = m.log.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.log.chkinfo.relpoint,
                    category = m.log.log.log.chkinfo.category,
                    contents = m.log.log.log.log.contents,
                    teamid = m.log.log.team.uid,
                    teamname = m.log.log.team.teamname,
                    groupid = m.log.log.log.log.groupid,
                    groupname = m.log.group.groupname,
                    crewid = m.crew.uid,
                    crewname = m.crew.realname,
                    crewimg = m.crew.imgurl,
                    relcrewid = m.log.log.log.log.crewrelid,
                    checkerid = m.log.log.log.log.checkerid,
                    checkername = m.log.log.log.log.checkername,
                    checkersector = m.log.log.log.log.checkexecparent,
                    recievepart = m.log.log.log.log.receivepart,
                    recievename = m.log.log.log.log.recievename,
                    checktime = m.log.log.log.log.checktime,
                    dutytime = m.log.log.log.log.dutytime,
                    checklevel = m.log.log.log.log.checklevel,
                    imgurl = m.log.log.log.log.imgurl
                })
                .FirstOrDefault();

            if (rst != null)
            {
                if (rst.relcrewid != null && rst.relcrewid != 0)
                {
                    var reluser = userModel.GetUserById((long)rst.relcrewid);
                    rst.relcrewname = reluser.realname;
                }
            }
            return rst;
        }
        public CheckLogInfo GetEditCombineJudgeDetail(long uid)
        {
            long myid = CommonModel.GetSessionUserID();

            CheckLogInfo rst = db.tbl_checklogs
                .Where(m => m.deleted == 0 && m.uid == uid)
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                .Join(db.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                .Join(db.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
               // .Join(db.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                .Select(m => new CheckLogInfo
                {
                    uid = m.log.log.log.uid,
                    infoid = m.log.log.log.infoid,
                    checkinfo = m.log.log.chkinfo.checkinfo,
                    checkno = m.log.log.chkinfo.checkno,
                    checktype = m.log.log.chkinfo.checktype,
                    chkpoint = m.log.log.chkinfo.chkpoint,
                    relpoint = m.log.log.chkinfo.relpoint,
                    category = m.log.log.chkinfo.category,
                    contents = m.log.log.log.contents,
                    teamid = m.log.team.uid,
                    teamname = m.log.team.teamname,
                    groupid = m.log.log.log.groupid,
                    groupname = m.group.groupname,
                   // crewid = m.crew.uid,
                   // crewname = m.crew.realname,
                   // crewimg = m.crew.imgurl,
                    relcrewid = m.log.log.log.crewrelid,
                    checkerid = m.log.log.log.checkerid,
                    checkername = m.log.log.log.checkername,
                    checkersector = m.log.log.log.checkexecparent,
                    recievepart = m.log.log.log.receivepart,
                    recievename = m.log.log.log.recievename,
                    checktime = m.log.log.log.checktime,
                    dutytime = m.log.log.log.dutytime,
                    checklevel = m.log.log.log.checklevel,
                    imgurl = m.log.log.log.imgurl
                })
                .FirstOrDefault();
            return rst;
        }
#endregion
        
    }
}