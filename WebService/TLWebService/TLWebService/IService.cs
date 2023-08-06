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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetData(string data);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData LoginUser(string username, string pwd);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetJiDuanList();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetBuMenList();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetCheDuiListWithUid(string bmID);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetCheDuiList(string starttime);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllCheDuiList();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllBanZuListWithJiFen(string cdID);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetBanZuList(string cdID, string starttime);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllBanZuList(string cdID);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetZeRenRenList(string bzID, string starttime);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllZeRenRenList(string bzID);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetLieCheZhangList(string bzID, string starttime);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetXiangDianList(string query);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoHeJiLuList(string cdID, string bzID, string ryID, string dtstart, string dtend);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoHeJiLuDet(string uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoHeTongJi(string cdID, string bzID, string dtstart, string dtend);

        [OperationContract]
        [WebInvoke(                
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetKaoHeItem(
            string uid,
            string starttime,
            string checktime,
            string cdID,
            string bzID,
            string zrrID,
            string lczID,
            string xdID,
            string content,
            string imgdata);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllCDRenYuanList(string cdID);

         [WebGet(
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             ResponseFormat = WebMessageFormat.Json,
             RequestFormat = WebMessageFormat.Json)]
         ServiceResponseData GetAllBMRenYuanList(string bmID);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetDaiQianItem(string uid, string pageno);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetYiShouItem(string uid, string pageno);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetGongWenItem(
            string uid,
            string title,
            string docno,
            string receiver,
            string sector,
            string outdate,
            string attach,
            string attachdate,
            string pubcontent,
            string attachcontent,
            string filename,
            string filedata);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetGWFaBuList();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetGongWenList(string bumen, string startdate, string enddate, string query, long userid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoShiLiShiList(string uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoShiWenTiList(string uid, string kind);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoShiList(string pid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetKaoHeChartData(long teamid, long groupid, string starttime, string endtime);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetKaoShiJieGuo(
            string uid,
            string eid,
            string score,
            string examres,
            string examsecond);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetLiuZhuanData(string uid, string docid, string content, string receiver);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetQianShouData(string uid, string docid, string content, string receiver);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetTaskItem(
            string uid,
            string title,
            string receiver,
            string starttime,
            string endtime,
            string contents,
            string filename,
            string filedata);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetMyTaskList(string uid, string pageno);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetMySendTaskList(string uid, string pageno);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetTaskDetInfo(string uid, string taskid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetTaskRunned(string uid, string taskid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetTaskCompleted(string uid, string taskid);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetTaskLog(string uid, string taskid, string contents);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetContactList(string contactkind, string rolename);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetEmailList(string uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetSentEmailList(string uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetEmailDetInfo(string uid, string emailid);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetEmail(string uid, string receiver, string title, string content, string filename, string filedata);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetReply(string uid, string receiver, string title, string content, string filename, string filedata, string yid);

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SendReceipt(string uid, string mailid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetAllUserList(string keyword, string pageno);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetReadedState(string uid, string emailid);

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData SetOpinion(string uid, string kind, string title, string content);

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetOpinionList(long uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetRuleList();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetDetRule(string ruleid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetRoutes();

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetCreditList(string findtime, string name, string sectorid, string routeid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetSelfCheckList(string findtime, string sectorid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetCombineCheckList(string findtime, string sectorid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetNewsList(string uid);

        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponseData GetDaiQianDetail(string uid, string docid);
    }
}
