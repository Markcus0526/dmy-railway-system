using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace TLSite.Models
{
    public enum MailType
    {
        RECEIVED,
        SENT,
        DRAFT
    }

    public class InboxInfo
    {
        public List<ReceivedMail> maillist { get; set; }
        public int totalcnt { get; set; }
        public int currpage { get; set; }
    }

    public class ReceivedMail
    {
        public long uid { get; set; }
        public MailType mailtype { get; set; }
        public long senderid { get; set; }
        public string sender { get; set; }
        public string senderimg { get; set; }
        public string receiver { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public string attachname { get; set; }
        public string attachpath { get; set; }
        public long attachsize { get; set; }
        public int isread { get; set; }
        public DateTime sendtime { get; set; }
        public string sendtimestr { get; set; }
        public DateTime? readtime { get; set; }
        public DateTime createtime { get; set; }
        public int deleted { get; set; }
        public string[] filename { get; set; }
        public string[] atachpath { get; set; }
        public int fileamount { get; set; }
        public List<receipt> receiverlist { get; set; }
    }

    public class OutboxInfo
    {
        public List<SentMail> maillist { get; set; }
        public int totalcnt { get; set; }
        public int currpage { get; set; }
    }

    public class receipt
    {
        public int receiptno { get; set; }
        public string receipttime { get; set; }

        public long receiverid { get; set; }
        public string receivername { get; set; }
    }

    public class SentMail
    {
        public long uid { get; set; }
        public MailType mailtype { get; set; }
        public long senderid { get; set; }
        public string sender { get; set; }
        public string senderimg { get; set; }
        public string receiver { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public string attachname { get; set; }
        public string attachpath { get; set; }
        public long attachsize { get; set; }
        public int isread { get; set; }
        public DateTime sendtime { get; set; }
        public string sendtimestr { get; set; }
        public DateTime? readtime { get; set; }
        public DateTime createtime { get; set; }
        public int deleted { get; set; }
    }

    public class MailModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        public static void SendSysMail(string hostname, int hostport, string mailuser, string mailpass, string fromaddr, string toaddr, string title, string mailbody)
        {
            try
            {
                string logoimgpath = HostingEnvironment.MapPath("/content/img/logo.png");

                SmtpClient client = new SmtpClient();
                client.Host = hostname;
                //client.Port = hostport;
                //client.UseDefaultCredentials = true;
                //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //client.Credentials = new System.Net.NetworkCredential(mailuser, mailpass);
                MailMessage Message = new MailMessage();
                Message.From = new System.Net.Mail.MailAddress(fromaddr);
                Message.To.Add(toaddr);
                Message.Subject = title;
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(mailbody));
                StreamReader reader = new StreamReader(stream);
                string utf8body = reader.ReadToEnd();

                Message.Body = utf8body;
                Message.SubjectEncoding = System.Text.Encoding.UTF8;
                Message.BodyEncoding = System.Text.Encoding.UTF8;
                Message.Priority = System.Net.Mail.MailPriority.High;
                Message.IsBodyHtml = true;
                Message.Attachments.Add(new Attachment(logoimgpath));
                Message.Attachments[0].ContentType.Name = "image/png";
                Message.Attachments[0].ContentId = "wallogo";
                Message.Attachments[0].ContentDisposition.Inline = true;
                Message.Attachments[0].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                client.Send(Message);

            }
            catch (System.Exception ex)
            {
                CommonModel.WriteLogFile("SystemModel", "SendSysMail()", ex.ToString());
            }
        }

        public static int perpage = 10;
        public InboxInfo GetReceivedList(int page, string searchkey)
        {
            InboxInfo result = new InboxInfo();
            List<ReceivedMail> rst = new List<ReceivedMail>();
            long myid = CommonModel.GetSessionUserID();

            var list = db.tbl_mails.Where(m => m.deleted == 0)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new ReceivedMail
                {
                    uid = m.mail.uid,
                    mailtype = (MailType)m.mail.mailtype,
                    senderid = m.mail.senderid,
                    sender = m.sender.realname,
                    receiver = m.mail.receiver,
                    title = m.mail.title,
                    contents = m.mail.contents,
                    attachname = m.mail.attachname,
                    attachpath = m.mail.attachpath,
                    attachsize = (long)m.mail.attachsize,
                    isread = m.mail.isread,
                    sendtime = m.mail.sendtime,
                    sendtimestr = String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", m.mail.sendtime),
                    readtime = m.mail.readtime,
                    createtime = m.mail.createtime
                })
                .ToList();

            if (!String.IsNullOrWhiteSpace(searchkey))
            {
                list = list.Where(m => m.title.Contains(searchkey)).ToList();
            }

            foreach (var item in list)
            {
                string[] rids = item.receiver.Split(',');

                if (rids.Contains(myid.ToString()))
                {
                    rst.Add(item);
                }
            }

            result.totalcnt = rst.Count();
            result.currpage = page;

            rst = rst.Skip(perpage * (page - 1)).Take(perpage).ToList();

            var ids = rst.Select(m => m.uid).ToList();

            var mailloglist = db.tbl_maillogs.Where(m => m.deleted == 0 && ids.Contains(m.mailid)).ToList();

            foreach (var item in rst)
            {
                var existlog = mailloglist.Where(m => m.mailid == item.uid && m.receiverid == myid).FirstOrDefault();

                if (existlog != null)
                {
                    item.isread = 1;
                }
            }

            result.maillist = rst;

            return result;
        }

        public OutboxInfo GetSentList(int page, string searchkey)
        {
            OutboxInfo result = new OutboxInfo();
            List<SentMail> rst = new List<SentMail>();
            long myid = CommonModel.GetSessionUserID();

            rst = db.tbl_mails.Where(m => m.deleted == 0 && m.senderid == myid)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new SentMail
                {
                    uid = m.mail.uid,
                    mailtype = (MailType)m.mail.mailtype,
                    senderid = m.mail.senderid,
                    sender = m.sender.realname,
                    senderimg = m.sender.imgurl,
                    receiver = m.mail.receiver,
                    title = m.mail.title,
                    contents = m.mail.contents,
                    attachname = m.mail.attachname,
                    attachpath = m.mail.attachpath,
                    attachsize = (long)m.mail.attachsize,
                    isread = 1,
                    sendtime = m.mail.sendtime,
                    sendtimestr = String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", m.mail.sendtime),
                    readtime = m.mail.readtime,
                    createtime = m.mail.createtime
                })
                .ToList();

            if (!String.IsNullOrWhiteSpace(searchkey))
            {
                rst = rst.Where(m => m.title.Contains(searchkey)).ToList();
            }

            result.totalcnt = rst.Count();
            result.currpage = page;

            rst = rst.Skip(perpage * (page - 1)).Take(perpage).ToList();

            result.maillist = rst;

            return result;
        }

        public string InsertMail(string receiver, string title, string esccontent, string attachpath, long attachsize, string attachname)
        {
            string rst = "";
            long senderid = CommonModel.GetSessionUserID();

            try
            {
                tbl_mail newmail = new tbl_mail();

                //newmail.mailtype = (int)MailType.SENT;
                newmail.senderid = senderid;
                newmail.receiver = receiver;
                newmail.title = title;
                newmail.contents = esccontent;
                newmail.attachname = attachname;
                newmail.attachpath = attachpath;
                newmail.attachsize = attachsize;
                newmail.sendtime = DateTime.Now;
                newmail.createtime = DateTime.Now;

                db.tbl_mails.InsertOnSubmit(newmail);
                db.SubmitChanges();

                string[] receiverids = receiver.Split(',');

                foreach (string rid in receiverids)
                {
                    tbl_sysnoticelog newnoticelog = new tbl_sysnoticelog();
                    newnoticelog.logtype = (int)NoticeType.MAIL;
                    newnoticelog.title = "收到新邮箱";
                    newnoticelog.readerid = long.Parse(rid);
                    newnoticelog.createtime = DateTime.Now;
                    newnoticelog.checklogid = newmail.uid;

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

        public ReceivedMail GetMail(long mailid, int logflag)
        {
            long myid = CommonModel.GetSessionUserID();

            var maillist = db.tbl_mails.Where(m => m.deleted == 0 && m.uid == mailid).ToList();
            var rst = maillist
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new ReceivedMail
                {
                    uid = m.mail.uid,
                    mailtype = (MailType)m.mail.mailtype,
                    senderid = m.mail.senderid,
                    sender = m.sender.realname,
                    senderimg = m.sender.imgurl, 
                    receiver = m.mail.receiver,
                    title = m.mail.title,
                    contents = m.mail.contents,
                    attachname = m.mail.attachname,
                    attachpath = m.mail.attachpath,
                    attachsize = (long)m.mail.attachsize,
                    isread = m.mail.isread,
                    sendtime = m.mail.sendtime,
                    sendtimestr = String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", m.mail.sendtime),
                    readtime = m.mail.readtime,
                    createtime = m.mail.createtime,
                    filename= m.mail.attachname.Split(','),
                    atachpath = m.mail.attachpath.Split(','),
                    fileamount = m.mail.attachname.Split(',').Count()-1,
                    receiverlist = new List<receipt>()
                })
                .FirstOrDefault();

            if (logflag == 1)
            {
                var loginfo = db.tbl_maillogs.Where(m => m.deleted == 0 && m.mailid == mailid && m.receiverid == myid).FirstOrDefault();

                if (loginfo == null)
                {
                    tbl_maillog log = new tbl_maillog();
                    log.mailid = mailid;
                    log.receiverid = myid;
                    log.readtime = DateTime.Now;

                    db.tbl_maillogs.InsertOnSubmit(log);
                    db.SubmitChanges();

                    

                }
                //更新sysnoticelog表
                string[] rids = rst.receiver.Split(',');
                if (rids.Contains(myid.ToString()))
                {
                    var noticelog = db.tbl_sysnoticelogs.Where(m => m.readerid == myid && m.logtype == (int)NoticeType.MAIL && m.checklogid == mailid).FirstOrDefault();
                    if (noticelog != null && noticelog.readtime == null)
                    {
                        noticelog.readtime = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
            }

                
            return rst;
        }

        public List<ReceivedMail> GetUnreadMail()
        {
            List<ReceivedMail> rst = new List<ReceivedMail>();
            List<ReceivedMail> filterlist = new List<ReceivedMail>();
            long myid = CommonModel.GetSessionUserID();

            var list = db.tbl_mails.Where(m => m.deleted == 0)
                .Join(db.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new ReceivedMail
                {
                    uid = m.mail.uid,
                    mailtype = (MailType)m.mail.mailtype,
                    senderid = m.mail.senderid,
                    senderimg = m.sender.imgurl,
                    sender = m.sender.realname,
                    receiver = m.mail.receiver,
                    title = m.mail.title,
                    contents = m.mail.contents,
                    attachname = m.mail.attachname,
                    attachpath = m.mail.attachpath,
                    attachsize = (long)m.mail.attachsize,
                    isread = m.mail.isread,
                    sendtime = m.mail.sendtime,
                    sendtimestr = CommonModel.GetTimeDiffFromNow(m.mail.sendtime),
                    readtime = m.mail.readtime,
                    createtime = m.mail.createtime
                })
                .ToList();

            foreach (var item in list)
            {
                string[] rids = item.receiver.Split(',');

                if (rids.Contains(myid.ToString()))
                {
                    filterlist.Add(item);
                }
            }

            var ids = filterlist.Select(m => m.uid).ToList();

            var mailloglist = db.tbl_maillogs.Where(m => m.deleted == 0 && ids.Contains(m.mailid)).ToList();

            foreach (var item in filterlist)
            {
                var existlog = mailloglist.Where(m => m.mailid == item.uid && m.receiverid == myid).FirstOrDefault();

                if (existlog != null)
                {
                    item.isread = 1;
                }
                else
                {
                    rst.Add(item);
                }
            }

            return rst;
        }

        public receipt GetReceiptByid(long uid,long mailid){
             
            var receiver= db.tbl_users
                         .Where(m =>m.uid == uid)
                         .Select(m => new receipt
                         {
                             receiverid = m.uid,
                             receivername = m.realname,
                         }).FirstOrDefault();

            var receiptno=db.tbl_maillogs.Where(m=>m.deleted==0&&m.mailid==mailid&&m.receiverid==uid).FirstOrDefault();
            receiver.receiptno = receiptno == null ? 0 : receiptno.receipt;
            receiver.receipttime = receiptno == null ? null : String.Format("{0:yyyy年MM月dd日 HH:mm:ss}",receiptno.receipttime);
            return receiver;
        }

        public string sendreceipt(long mailid, long uid)
        {
            var rst="";
            var loginfo = db.tbl_maillogs.Where(m => m.deleted == 0 && m.mailid == mailid && m.receiverid == uid).FirstOrDefault();

            if (loginfo != null)
            {
                if (loginfo.receipttime == null)
                {
                    try
                    {
                        loginfo.receipt = 1;
                        loginfo.receipttime = DateTime.Now;
                        db.SubmitChanges();
                    }
                    catch (System.Exception ex)
                    {
                        rst = "发送回执失败";
                    }
                }
            }
            else{
                rst="发送回执失败";
            }
            return rst;
        }


        public string GetReceipt(long mailid, long uid)
        {
            var rst = "";
            var loginfo = db.tbl_maillogs.Where(m => m.deleted == 0 && m.mailid == mailid && m.receiverid == uid).FirstOrDefault();

            if (loginfo != null)
            {
                if (loginfo.receipttime == null)
                {
                }
                else
                {
                    rst = String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", loginfo.receipttime);
                }
            }
            else
            {
                rst = "收信失败，请刷新";
            }
            return rst;
        }
    }
}