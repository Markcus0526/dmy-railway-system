package com.damytech.commservice;

import com.damytech.HttpConn.AsyncHttpResponseHandler;
import com.damytech.STData.*;
import org.json.JSONObject;

import java.util.ArrayList;

public class CommMgr {
	
	public static CommMgr commService = new CommMgr();
    public UserSvcMgr userMgr = new UserSvcMgr();

	public CommMgr() {}

    public void LoginUser(AsyncHttpResponseHandler handler, String username, String pwd)
    {
        userMgr.LoginUser(handler, username, pwd);
    }

    public long parseLoginUser(JSONObject jsonObject, STUserInfo userInfo)
    {
        return userMgr.parseLoginUser(jsonObject, userInfo);
    }

    public void GetJiDuanList(AsyncHttpResponseHandler handler)
    {
        userMgr.GetJiDuanList(handler);
    }

    public long parseGetJiDuanList(JSONObject jsonObject, ArrayList<STJiDuan> arrData)
    {
        return userMgr.parseGetJiDuanList(jsonObject, arrData);
    }

    public void GetBuMenList(AsyncHttpResponseHandler handler)
    {
        userMgr.GetBuMenList(handler);
    }

    public long parseGetBuMenList(JSONObject jsonObject, ArrayList<STBuMen> arrData)
    {
        return userMgr.parseGetBuMenList(jsonObject, arrData);
    }

    public void GetCheDuiListWithUid(AsyncHttpResponseHandler handler, String bmID)
    {
        userMgr.GetCheDuiListWithUid(handler, bmID);
    }

    public long parseGetCheDuiListWithUid(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        return userMgr.parseGetCheDuiListWithUid(jsonObject, arrData);
    }

    public void GetCheDuiList(AsyncHttpResponseHandler handler, String starttime)
    {
        userMgr.GetCheDuiList(handler, starttime);
    }

    public long parseGetCheDuiList(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        return userMgr.parseGetCheDuiList(jsonObject, arrData);
    }

    public void GetAllCheDuiList(AsyncHttpResponseHandler handler)
    {
        userMgr.GetAllCheDuiList(handler);
    }

    public long parseGetAllCheDuiList(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        return userMgr.parseGetAllCheDuiList(jsonObject, arrData);
    }

    public void GetBanZuList(AsyncHttpResponseHandler handler, String cdID, String starttime)
    {
        userMgr.GetBanZuList(handler, cdID, starttime);
    }

    public long parseGetBanZuList(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        return userMgr.parseGetBanZuList(jsonObject, arrData);
    }

    public void GetAllBanZuList(AsyncHttpResponseHandler handler, String cdID)
    {
        userMgr.GetAllBanZuList(handler, cdID);
    }

    public long parseGetAllBanZuList(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        return userMgr.parseGetAllBanZuList(jsonObject, arrData);
    }

    public void GetAllBanZuListWithJiFen(AsyncHttpResponseHandler handler, String cdID)
    {
        userMgr.GetAllBanZuListWithJiFen(handler, cdID);
    }

    public long parseGetAllBanZuListWithJiFen(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        return userMgr.parseGetAllBanZuList(jsonObject, arrData);
    }

    public void GetZeRenRenList(AsyncHttpResponseHandler handler, String bzID, String starttime)
    {
        userMgr.GetZeRenRenList(handler, bzID, starttime);
    }

    public long parseGetZeRenRenList(JSONObject jsonObject, ArrayList<STZeRenRen> arrData)
    {
        return userMgr.parseGetZeRenRenList(jsonObject, arrData);
    }

    public void GetAllZeRenRenList(AsyncHttpResponseHandler handler, String bzID)
    {
        userMgr.GetAllZeRenRenList(handler, bzID);
    }

    public long parseGetAllZeRenRenList(JSONObject jsonObject, ArrayList<STZeRenRen> arrData)
    {
        return userMgr.parseGetAllZeRenRenList(jsonObject, arrData);
    }

    public void GetLieCheZhangList(AsyncHttpResponseHandler handler, String bzID, String starttime)
    {
        userMgr.GetLieCheZhangList(handler, bzID, starttime);
    }

    public long parseGetLieCheZhangList(JSONObject jsonObject, ArrayList<STLieCheZhang> arrData)
    {
        return userMgr.parseGetLieCheZhangList(jsonObject, arrData);
    }

    public void GetXiangDianList(AsyncHttpResponseHandler handler, String query)
    {
        userMgr.GetXiangDianList(handler, query);
    }

    public long parseGetXiangDianList(JSONObject jsonObject, ArrayList<STXiangDian> arrData)
    {
        return userMgr.parseGetXiangDianList(jsonObject, arrData);
    }

    public void GetKaoHeJiLuList(AsyncHttpResponseHandler handler, String cdID, String bzID, String ryID, String dtstart, String dtend)
    {
        userMgr.GetKaoHeJiLuList(handler, cdID, bzID, ryID, dtstart, dtend);
    }

    public long parseGetKaoHeJiLuList(JSONObject jsonObject, ArrayList<STKaoHeJiLu> arrData)
    {
        return userMgr.parseGetKaoHeJiLuList(jsonObject, arrData);
    }

    public void SetKaoHeItem(AsyncHttpResponseHandler handler, String uid, String starttime, String checktime,
                             String cdID, String bzID, String zrrID, String lczID, String xdID, String content, String imgdata)
    {
        userMgr.SetKaoHeItem(handler, uid, starttime, checktime, cdID, bzID, zrrID, lczID, xdID, content, imgdata);
    }

    public long parseSetKaoHeItem(JSONObject jsonObject)
    {
        return userMgr.parseSetKaoHeItem(jsonObject);
    }

    public void GetKaoHeJiLuDet(AsyncHttpResponseHandler handler, String uid)
    {
        userMgr.GetKaoHeJiLuDet(handler, uid);
    }

    public long parseGetKaoHeJiLuDet(JSONObject jsonObject, STKaoHeJiLuDet objData)
    {
        return userMgr.parseGetKaoHeJiLuDet(jsonObject, objData);
    }

    public void GetDaiQianItem(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        userMgr.GetDaiQianItem(handler, uid, pageno);
    }

    public long parseGetDaiQianItem(JSONObject jsonObject, STDaiQianItem objData)
    {
        return userMgr.parseGetDaiQianItem(jsonObject, objData);
    }

    public void GetYiShouItem(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        userMgr.GetYiShouItem(handler, uid, pageno);
    }

    public long parseGetYiShouItem(JSONObject jsonObject, STDaiQianItem objData)
    {
        return userMgr.parseGetYiShouItem(jsonObject, objData);
    }

    public void GetAllCDRenYuanList(AsyncHttpResponseHandler handler, String cdID)
    {
        userMgr.GetAllCDRenYuanList(handler, cdID);
    }

    public long parseGetAllCDRenYuanList(JSONObject jsonObject, ArrayList<STRenYuan> arrData)
    {
        return userMgr.parseGetAllCDRenYuanList(jsonObject, arrData);
    }

    public void GetAllBMRenYuanList(AsyncHttpResponseHandler handler, String bmID)
    {
        userMgr.GetAllBMRenYuanList(handler, bmID);
    }

    public long parseGetAllBMRenYuanList(JSONObject jsonObject, ArrayList<STRenYuan> arrData)
    {
        return userMgr.parseGetAllBMRenYuanList(jsonObject, arrData);
    }

    public void SetGongWenItem(AsyncHttpResponseHandler handler, String uid, String title, String docno,
                               String receiver, String sector, String outdate, String attach, String attachdate,
                               String pubcontent, String attachcontent, String filename, String filedata)
    {
        userMgr.SetGongWenItem(handler, uid, title, docno, receiver, sector, outdate, attach, attachdate,
                                pubcontent, attachcontent, filename, filedata);
    }

    public long parseSetGongWenItem(JSONObject jsonObject)
    {
        return userMgr.parseSetGongWenItem(jsonObject);
    }

    public void GetGWFaBuList(AsyncHttpResponseHandler handler)
    {
        userMgr.GetGWFaBuList(handler);
    }

    public long parseGetGWFaBuList(JSONObject jsonObject, ArrayList<STGWFaBu> arrData)
    {
        return userMgr.parseGetGWFaBuList(jsonObject, arrData);
    }

    public void GetGongWenList(AsyncHttpResponseHandler handler, String bumen, String startdate, String enddate, String query, long userid)
    {
        userMgr.GetGongWenList(handler, bumen, startdate, enddate, query, userid);
    }

    public long parseGetGongWenList(JSONObject jsonObject, ArrayList<STDaiQianItem> arrData)
    {
        return userMgr.parseGetGongWenList(jsonObject, arrData);
    }

    public void GetKaoShiList(AsyncHttpResponseHandler handler, String pid)
    {
        userMgr.GetKaoShiList(handler, pid);
    }

    public long parseGetKaoShiList(JSONObject jsonObject, ArrayList<STKaoShi> arrData)
    {
        return userMgr.parseGetKaoShiList(jsonObject, arrData);
    }

    public void GetKaoShiLiShiList(AsyncHttpResponseHandler handler, String uid)
    {
        userMgr.GetKaoShiLiShiList(handler, uid);
    }

    public long parseGetKaoShiLiShiList(JSONObject jsonObject, ArrayList<STKaoShiLiShi> arrData)
    {
        return userMgr.parseGetKaoShiLiShiList(jsonObject, arrData);
    }

    public void GetKaoShiWenTiList(AsyncHttpResponseHandler handler, String uid, String kind)
    {
        userMgr.GetKaoShiWenTiList(handler, uid, kind);
    }

    public long parseGetKaoShiWenTiList(JSONObject jsonObject, ArrayList<STKaoShiItem> arrData)
    {
        return userMgr.parseGetKaoShiWenTiList(jsonObject, arrData);
    }

    public void GetKaoHeChartData(AsyncHttpResponseHandler handler, String teamid, String groupid, String starttime, String endtime)
    {
        userMgr.GetKaoHeChartData(handler, teamid, groupid, starttime, endtime);
    }

    public long parseGetKaoHeChartData(JSONObject jsonObject, ArrayList<STKaoHeTongJi> arrData)
    {
        return userMgr.parseGetKaoHeChartData(jsonObject, arrData);
    }

    public void SetKaoShiJieGuo(AsyncHttpResponseHandler handler, String uid, String eid, String score,
                                String examres, String examsecond)
    {
        userMgr.SetKaoShiJieGuo(handler, uid, eid, score, examres, examsecond);
    }

    public long parseSetKaoShiJieGuo(JSONObject jsonObject)
    {
        return userMgr.parseSetKaoShiJieGuo(jsonObject);
    }

    public void SetLiuZhuanData(AsyncHttpResponseHandler handler, String uid, String docid, String content, String receiver)
    {
        userMgr.SetLiuZhuanData(handler, uid, docid, content, receiver);
    }

    public long parseSetLiuZhuanData(JSONObject jsonObject)
    {
        return userMgr.parseSetLiuZhuanData(jsonObject);
    }

    public void SetQianShouData(AsyncHttpResponseHandler handler, String uid, String docid, String content, String receiver)
    {
        userMgr.SetQianShouData(handler, uid, docid, content, receiver);
    }

    public long parseSetQianShouData(JSONObject jsonObject)
    {
        return userMgr.parseSetQianShouData(jsonObject);
    }

    public void SetTaskItem(AsyncHttpResponseHandler handler, String uid, String title, String receiver, String starttime,
                            String endtime, String contents, String filename, String filedata)
    {
        userMgr.SetTaskItem(handler, uid, title, receiver, starttime, endtime, contents, filename, filedata);
    }

    public long parseSetTaskItem(JSONObject jsonObject)
    {
        return userMgr.parseSetTaskItem(jsonObject);
    }

    public void GetMySendTaskList(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        userMgr.GetMySendTaskList(handler, uid, pageno);
    }

    public long parseGetMySendTaskList(JSONObject jsonObject, ArrayList<STRenWuItem> arrData)
    {
        return userMgr.parseGetMySendTaskList(jsonObject, arrData);
    }

    public void GetMyTaskList(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        userMgr.GetMyTaskList(handler, uid, pageno);
    }

    public long parseGetMyTaskList(JSONObject jsonObject, ArrayList<STRenWuJieDaoItem> arrData)
    {
        return userMgr.parseGetMyTaskList(jsonObject, arrData);
    }

    public void GetTaskDetInfo(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        userMgr.GetTaskDetInfo(handler, uid, taskid);
    }

    public long parseGetTaskDetInfo(JSONObject jsonObject, STTaskDetInfo objData)
    {
        return userMgr.parseGetTaskDetInfo(jsonObject, objData);
    }

    public void SetTaskRunned(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        userMgr.SetTaskRunned(handler, uid, taskid);
    }

    public long parseSetTaskRunned(JSONObject jsonObject)
    {
        return userMgr.parseSetTaskRunned(jsonObject);
    }

    public void SetTaskCompleted(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        userMgr.SetTaskCompleted(handler, uid, taskid);
    }

    public long parseSetTaskCompleted(JSONObject jsonObject)
    {
        return userMgr.parseSetTaskCompleted(jsonObject);
    }

    public void SetTaskLog(AsyncHttpResponseHandler handler, String uid, String taskid, String contents)
    {
        userMgr.SetTaskLog(handler, uid, taskid, contents);
    }

    public long parseSetTaskLog(JSONObject jsonObject)
    {
        return userMgr.parseSetTaskLog(jsonObject);
    }

    public void GetContactList(AsyncHttpResponseHandler handler, String contactkind, String rolename)
    {
        userMgr.GetContactList(handler, contactkind, rolename);
    }

    public long parseGetContactList(JSONObject jsonObject, ArrayList<STContactInfo> arrData)
    {
        return userMgr.parseGetContactList(jsonObject, arrData);
    }

    public void GetEmailList(AsyncHttpResponseHandler handler, String uid)
    {
        userMgr.GetEmailList(handler, uid);
    }

    public long parseGetEmailList(JSONObject jsonObject, ArrayList<STEmailInfo> arrData)
    {
        return userMgr.parseGetEmailList(jsonObject, arrData);
    }

    public void GetSentEmailList(AsyncHttpResponseHandler handler, String uid)
    {
        userMgr.GetSentEmailList(handler, uid);
    }

    public long parseGetSentEmailList(JSONObject jsonObject, ArrayList<STEmailInfo> arrData)
    {
        return userMgr.parseGetSentEmailList(jsonObject, arrData);
    }

    public void GetEmailDetInfo(AsyncHttpResponseHandler handler, String uid, String emailid)
    {
        userMgr.GetEmailDetInfo(handler, uid, emailid);
    }

    public long parseGetEmailDetInfo(JSONObject jsonObject, STEmailInfo arrData)
    {
        return userMgr.parseGetEmailDetInfo(jsonObject, arrData);
    }

    public void SendReceipt(AsyncHttpResponseHandler handler, String uid, String mailid)
    {
        userMgr.SendReceipt(handler, uid, mailid);
    }

    public long parseSendReceipt(JSONObject jsonObject)
    {
        return userMgr.parseSendReceipt(jsonObject);
    }

    public void GetAllUserList(AsyncHttpResponseHandler handler, String keyword, String pageno)
    {
        userMgr.GetAllUserList(handler, keyword, pageno);
    }

    public long parseGetAllUserList(JSONObject jsonObject, ArrayList<STEmailUser> arrData)
    {
        return userMgr.parseGetAllUserList(jsonObject, arrData);
    }

    public void SetEmail(AsyncHttpResponseHandler handler, String uid, String receiver, String title,
                         String content, String filename, String filedata)
    {
        userMgr.SetEmail(handler, uid, receiver, title, content, filename, filedata);
    }

    public long parseSetEmail(JSONObject jsonObject)
    {
        return userMgr.parseSetEmail(jsonObject);
    }

    public void SetReply(AsyncHttpResponseHandler handler, String uid, String receiver, String title,
                         String content, String filename, String filedata, String yid)
    {
        userMgr.SetReply(handler, uid, receiver, title, content, filename, filedata, yid);
    }

    public long parseSetReply(JSONObject jsonObject)
    {
        return userMgr.parseSetReply(jsonObject);
    }

    public void SetReadedState(AsyncHttpResponseHandler handler, String uid, String emailid)
    {
        userMgr.SetReadedState(handler, uid, emailid);
    }

    public long parseSetReadedState(JSONObject jsonObject)
    {
        return userMgr.parseSetReadedState(jsonObject);
    }

    public void SetOpinion(AsyncHttpResponseHandler handler, String uid, String kind, String title, String content)
    {
        userMgr.SetOpinion(handler, uid, kind, title, content);
    }

    public long parseSetOpinion(JSONObject jsonObject)
    {
        return userMgr.parseSetOpinion(jsonObject);
    }

    public void GetOpinionList(AsyncHttpResponseHandler handler, long uid)
    {
        userMgr.GetOpinionList(handler, uid);
    }

    public long parseGetOpinionList(JSONObject jsonObject, ArrayList<STOpinionData> arrData)
    {
        return userMgr.parseGetOpinionList(jsonObject, arrData);
    }

    public void GetRuleList(AsyncHttpResponseHandler handler)
    {
        userMgr.GetRuleList(handler);
    }

    public long parseGetRuleList(JSONObject jsonObject, ArrayList<STPdfFileData> arrData)
    {
        return userMgr.parseGetRuleList(jsonObject, arrData);
    }

    public void GetDetRule(AsyncHttpResponseHandler handler, String ruleid)
    {
        userMgr.GetDetRule(handler, ruleid);
    }

    public long parseGetDetRule(JSONObject jsonObject, STDetRule arrData)
    {
        return userMgr.parseGetDetRule(jsonObject, arrData);
    }

    public void GetRoutes(AsyncHttpResponseHandler handler)
    {
        userMgr.GetRoutes(handler);
    }

    public long parseGetRoutes(JSONObject jsonObject, ArrayList<STRoute> arrData)
    {
        return userMgr.parseGetRoutes(jsonObject, arrData);
    }

    public void GetCreditList(AsyncHttpResponseHandler handler, String findtime, String name, String sectorid, String routeid)
    {
        userMgr.GetCreditList(handler, findtime, name, sectorid, routeid);
    }

    public long parseGetCreditList(JSONObject jsonObject, ArrayList<STCreditInfo> arrData)
    {
        return userMgr.parseGetCreditList(jsonObject, arrData);
    }

    public void GetSelfCheckList(AsyncHttpResponseHandler handler, String findtime,String sectorid)
    {
        userMgr.GetSelfCheckList(handler, findtime, sectorid);
    }

    public long parseGetSelfCheckList(JSONObject jsonObject, ArrayList<STSelfCheck> arrData)
    {
        return userMgr.parseGetSelfCheckList(jsonObject, arrData);
    }

    public void GetCombineCheckList(AsyncHttpResponseHandler handler, String findtime, String sectorid)
    {
        userMgr.GetCombineCheckList(handler, findtime, sectorid);
    }

    public long parseGetCombineCheckList(JSONObject jsonObject, ArrayList<STCombineCheckInfo> arrData)
    {
        return userMgr.parseGetCombineCheckList(jsonObject, arrData);
    }

    public void GetNewsList(AsyncHttpResponseHandler handler, long uid)
    {
        userMgr.GetNewsList(handler , uid);
    }

    public long parseGetNewsList(JSONObject jsonObject, STNewsList listData)
    {
        return userMgr.parseGetNewsList(jsonObject, listData);
    }

    public void GetDaiQianDetail(AsyncHttpResponseHandler handler, long uid, long docid)
    {
        userMgr.GetDaiQianDetail(handler, uid, docid);
    }

    public long parseGetDaiQianDetail(JSONObject jsonObject, STDaiQianItem objData)
    {
        return userMgr.parseGetDaiQianDetail(jsonObject, objData);
    }
}
