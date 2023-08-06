package com.damytech.STData;

public class STServiceData {
    public static String serviceAddr = "http://59.44.198.134:2802/Service.svc/";
    //public static String serviceAddr = "http://192.168.1.41:1030/Service.svc/";

    // Error Code
    public static final int ERR_SUCCESS = 0;
    public static final int ERR_EXIST_DATA = -4;
    public static final int ERR_SERVER_ERROR = 500;

    // Command List
    public static String cmdLoginUser = "LoginUser";
    public static String cmdGetJiDuanList = "GetJiDuanList";
    public static String cmdGetBuMenList = "GetBuMenList";
    public static String cmdGetCheDuiListWithUid = "GetCheDuiListWithUid";
    public static String cmdGetCheDuiList = "GetCheDuiList";
    public static String cmdGetAllCheDuiList = "GetAllCheDuiList";
    public static String cmdGetBanZuList = "GetBanZuList";
    public static String cmdGetAllBanZuList = "GetAllBanZuList";
    public static String cmdGetAllBanZuListWithJiFen = "GetAllBanZuListWithJiFen";
    public static String cmdGetZeRenRenList = "GetZeRenRenList";
    public static String cmdGetAllZeRenRenList = "GetAllZeRenRenList";
    public static String cmdGetLieCheZhangList = "GetLieCheZhangList";
    public static String cmdGetXiangDianList = "GetXiangDianList";
    public static String cmdGetKaoHeJiLuList = "GetKaoHeJiLuList";
    public static String cmdSetKaoHeItem = "SetKaoHeItem";
    public static String cmdGetKaoHeJiLuDet = "GetKaoHeJiLuDet";
    public static String cmdGetDaiQianItem = "GetDaiQianItem";
    public static String cmdGetDaiQianDetail = "GetDaiQianDetail";
    public static String cmdGetYiShouItem = "GetYiShouItem";
    public static String cmdGetAllCDRenYuanList = "GetAllCDRenYuanList";
    public static String cmdGetAllBMRenYuanList = "GetAllBMRenYuanList";
    public static String cmdSetGongWenItem = "SetGongWenItem";
    public static String cmdGetGWFaBuList = "GetGWFaBuList";
    public static String cmdGetGongWenList = "GetGongWenList";
    public static String cmdGetKaoShiLiShiList = "GetKaoShiLiShiList";
    public static String cmdGetKaoShiWenTiList = "GetKaoShiWenTiList";
    public static String cmdGetKaoShiList = "GetKaoShiList";
    public static String cmdGetKaoHeChartData = "GetKaoHeChartData";
    public static String cmdSetKaoShiJieGuo = "SetKaoShiJieGuo";
    public static String cmdSetLiuZhuanData = "SetLiuZhuanData";
    public static String cmdSetQianShouData = "SetQianShouData";
    public static String cmdSetTaskItem = "SetTaskItem";
    public static String cmdGetMySendTaskList = "GetMySendTaskList";
    public static String cmdGetMyTaskList = "GetMyTaskList";
    public static String cmdGetTaskDetInfo = "GetTaskDetInfo";
    public static String cmdSetTaskRunned = "SetTaskRunned";
    public static String cmdSetTaskCompleted = "SetTaskCompleted";
    public static String cmdSetTaskLog = "SetTaskLog";
    public static String cmdGetContactList = "GetContactList";
    public static String cmdGetEmailList = "GetEmailList";
    public static String cmdGetSentEmailList = "GetSentEmailList";
    public static String cmdSendReceipt = "SendReceipt";
    public static String cmdGetEmailDetInfo = "GetEmailDetInfo";
    public static String cmdSetEmail = "SetEmail";
    public static String cmdSetReply = "SetReply";
    public static String cmdGetAllUserList = "GetAllUserList";
    public static String cmdSetReadedState="SetReadedState";
    public static String cmdSetOpinion = "SetOpinion";
    public static String cmdGetRuleList = "GetRuleList";
    public static String cmdGetDetRule = "GetDetRule";
    public static String cmdGetRoutes = "GetRoutes";
    public static String cmdGetCreditList = "GetCreditList";
    public static String cmdGetSelfCheckList = "GetSelfCheckList";
    public static String cmdGetCombineCheckList = "GetCombineCheckList";
    public static String cmdGetNewsList= "GetNewsList";
    public static String cmdGetOpinionList= "GetOpinionList";

	// Connection Info
	public static int connectTimeout = 4 * 1000; // 5 Seconds
}
