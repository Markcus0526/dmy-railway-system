using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TLWebService.Model;

namespace TLWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service : IService
    {
        TLModel model = new TLModel();

        public ServiceResponseData GetData(string data)
        {
             ServiceResponseData retData = new ServiceResponseData();
//             retData.Data = string.Format("You entered: {0}", data);

             //retData = SetGongWenItem("8", "Cccccc", "Bbbb", "8", "班子成员", "2014-08-09", "", "", "Aaaaa", "", "aaa.txt", "");
             retData = SetReply("5", "1", "asdfa", "asdfasdfadsfdf", "frame_0006.jpg", "asdfasdfasdfadsf", "-1");
             //byte[] bytes = Convert.FromBase64String("");

            return retData;
        }

        public ServiceResponseData LoginUser(string username, string pwd)
        {
            return model.LoginUser(username, pwd);
        }

        public ServiceResponseData GetJiDuanList()
        {
            return model.GetJiDuanList();
        }

        public ServiceResponseData GetBuMenList()
        {
            return model.GetBuMenList();
        }

        public ServiceResponseData GetCheDuiListWithUid(string bmID)
        {
            return model.GetCheDuiListWithUid(bmID);
        }

        public ServiceResponseData GetCheDuiList(string starttime)
        {
            return model.GetCheDuiList(starttime);
        }

        public ServiceResponseData GetAllCheDuiList()
        {
            return model.GetAllCheDuiList();
        }

        public ServiceResponseData GetAllBanZuListWithJiFen(string cdID)
        {
            return model.GetAllBanZuListWithJiFen(cdID);
        }

        public ServiceResponseData GetBanZuList(string cdID, string starttime)
        {
            return model.GetBanZuList(cdID, starttime);
        }

        public ServiceResponseData GetAllBanZuList(string cdID)
        {
            return model.GetAllBanZuList(cdID);
        }

        public ServiceResponseData GetZeRenRenList(string bzID, string starttime)
        {
            return model.GetZeRenRenList(bzID, starttime);
        }

        public ServiceResponseData GetAllZeRenRenList(string bzID)
        {
            return model.GetAllZeRenRenList(bzID);
        }

        public ServiceResponseData GetLieCheZhangList(string bzID, string starttime)
        {
            return model.GetLieCheZhangList(bzID, starttime);
        }

        public ServiceResponseData GetXiangDianList(string query)
        {
            return model.GetXiangDianList(query);
        }

        public ServiceResponseData GetKaoHeJiLuList(string cdID, string bzID, string ryID, string dtstart, string dtend)
        {
            return model.GetKaoHeJiLuList(cdID, bzID, ryID, dtstart, dtend);
        }

        public ServiceResponseData GetKaoHeJiLuDet(string uid)
        {
            return model.GetKaoHeJiLuDet(uid);
        }

        public ServiceResponseData GetKaoHeTongJi(string cdID, string bzID, string dtstart, string dtend)
        {
            return model.GetKaoHeTongJi(cdID, bzID, dtstart, dtend);
        }

        public ServiceResponseData SetKaoHeItem(
            string uid,
            string starttime,
            string checktime,
            string cdID,
            string bzID,
            string zrrID,
            string lczID,
            string xdID,
            string content,
            string imgdata)
        {
            return model.SetKaoHeItem(uid, starttime, checktime, cdID, bzID, zrrID, lczID, xdID, content, imgdata);
        }

        public ServiceResponseData GetAllCDRenYuanList(string cdID)
        {
            return model.GetAllCDRenYuanList(cdID);
        }

        public ServiceResponseData GetAllBMRenYuanList(string bmID)
        {
            return model.GetAllBMRenYuanList(bmID);
        }

        public ServiceResponseData GetDaiQianItem(string uid, string pageno)
        {
            return model.GetDaiQianItem(uid, pageno);
        }

        public ServiceResponseData GetYiShouItem(string uid, string pageno)
        {
            return model.GetYiShouItem(uid, pageno);
        }

        public ServiceResponseData SetGongWenItem( string uid, string title, string docno, string receiver, string sector,
                                                string outdate, string attach, string attachdate, string pubcontent,
                                                string attachcontent, string filename, string filedata)
        {
            return model.SetGongWenItem(uid, title, docno, receiver, sector, outdate, attach, attachdate, pubcontent,
                                        attachcontent, filename, filedata);
        }

        public ServiceResponseData GetGWFaBuList()
        {
            return model.GetGWFaBuList();
        }

        public ServiceResponseData GetGongWenList(string bumen, string startdate, string enddate, string query, long userid)
        {
            return model.GetGongWenList(bumen, startdate, enddate, query, userid);
        }

        public ServiceResponseData GetKaoShiLiShiList(string uid)
        {
            return model.GetKaoShiLiShiList(uid);
        }

        public ServiceResponseData GetKaoShiWenTiList(string uid, string kind)
        {
            return model.GetKaoShiWenTiList(uid, kind);
        }

        public ServiceResponseData GetKaoShiList(string pid)
        {
            return model.GetKaoShiList(pid);
        }

        public ServiceResponseData GetKaoHeChartData(long teamid, long groupid, string starttime, string endtime)
        {
            return model.GetKaoHeChartData(teamid, groupid, starttime, endtime);
        }

        public ServiceResponseData SetKaoShiJieGuo(
            string uid,
            string eid,
            string score,
            string examres,
            string examsecond)
        {
            return model.SetKaoShiJieGuo(uid, eid, score, examres, examsecond);
        }

        public ServiceResponseData SetLiuZhuanData(string uid, string docid, string content, string receiver)
        {
            return model.SetLiuZhuanData(uid, docid, content, receiver);
        }

        public ServiceResponseData SetQianShouData(string uid, string docid, string content, string receiver)
        {
            return model.SetQianShouData(uid, docid, content, receiver);
        }

        public ServiceResponseData SetTaskItem(
            string uid,
            string title,
            string receiver,
            string starttime,
            string endtime,
            string contents,
            string filename,
            string filedata)
        {
            return model.SetTaskItem(uid, title, receiver, starttime, endtime, contents, filename, filedata);
        }

        public ServiceResponseData GetMyTaskList(string uid, string pageno)
        {
            return model.GetMyTaskList(uid, pageno);
        }

        public ServiceResponseData GetMySendTaskList(string uid, string pageno)
        {
            return model.GetMySendTaskList(uid, pageno);
        }

        public ServiceResponseData GetTaskDetInfo(string uid, string taskid)
        {
            return model.GetTaskDetInfo(uid, taskid);
        }

        public ServiceResponseData SetTaskRunned(string uid, string taskid)
        {
            return model.SetTaskRunned(uid, taskid);
        }

        public ServiceResponseData SetTaskCompleted(string uid, string taskid)
        {
            return model.SetTaskCompleted(uid, taskid);
        }

        public ServiceResponseData SetTaskLog(string uid, string taskid, string contents)
        {
            return model.SetTaskLog(uid, taskid, contents);
        }

        public ServiceResponseData GetContactList(string contactkind, string rolename)
        {
            return model.GetContactList(contactkind, rolename);
        }

        public ServiceResponseData GetEmailList(string uid)
        {
            return model.GetEmailList(uid);
        }

        public ServiceResponseData GetSentEmailList(string uid)
        {
            return model.GetSentEmailList(uid);
        }

        public ServiceResponseData GetEmailDetInfo(string uid, string emailid)
        {
            return model.GetEmailDetInfo(uid, emailid);
        }

        public ServiceResponseData SetEmail(string uid, string receiver, string title, string content, string filename, string filedata)
        {
            return model.SetEmail(uid, receiver, title, content, filename, filedata);
        }

        public ServiceResponseData SetReply(string uid, string receiver, string title, string content, string filename, string filedata, string yid)
        {
            return model.SetReply(uid, receiver, title, content, filename, filedata, yid);
        }

        public ServiceResponseData SendReceipt(string uid, string mailid)
        {
            return model.SendReceipt(uid, mailid);
        }

        public ServiceResponseData GetAllUserList(string keyword, string pageno)
        {
            return model.GetAllUserList(keyword, pageno);
        }

        public ServiceResponseData SetReadedState(string uid, string emailid)
        {
            return model.SetReadedState(uid, emailid);
        }

        public ServiceResponseData SetOpinion(string uid, string kind, string title, string content)
        {
            return model.SetOpinion(uid, kind, title, content);
        }

        public ServiceResponseData GetOpinionList(long uid)
        {
            return model.GetOpinionList(uid);
        }

        public ServiceResponseData GetRuleList()
        {
            return model.GetRuleList();
        }

        public ServiceResponseData GetDetRule(string ruleid)
        {
            return model.GetDetRule(ruleid);
        }

        public ServiceResponseData GetRoutes()
        {
            return model.GetRoutes();
        }

        public ServiceResponseData GetCreditList(string findtime, string name, string sectorid, string routeid)
        {
            return model.GetCreditList(findtime, name, sectorid, routeid);
        }

        public ServiceResponseData GetSelfCheckList(string findtime, string sectorid)
        {
            return model.GetSelfCheckList(findtime, sectorid);
        }

        public ServiceResponseData GetCombineCheckList(string findtime, string sectorid)
        {
            return model.GetCombineCheckList(findtime, sectorid);
        }

        public ServiceResponseData GetNewsList(string uid)
        {
            return model.GetNewsList(uid);
        }

        public ServiceResponseData GetDaiQianDetail(string uid, string docid)
        {
            return model.GetDaiQianDetail(uid, docid);
        }
    }
}
