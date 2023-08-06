using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Configuration;
using TLWebService.ServiceModel;
using System.Web.Hosting;
using System.IO;

namespace TLWebService.Model
{
    public enum ExamKind
    {
        Sector = 0,
        Team
    }

    public class ExecType
    {
        public const string SectorExec = "科室干部";
        public const string TeamExec = "车队干部";
    }

    public class ExamType
    {
        public const string OneSel = "单选题";
        public const string MultiSel = "多选题";
        public const string YesNo = "判断题";
    }

    public enum UserKind
    {
        ADMIN = 0,
        EXECUTIVE,
        CREW
    }

    public enum CrewKind
    {
        ZERENREN = 3,
        LIECHEZHANG = 2
    }

    public enum ServiceError
    {
        ERR_SUCCESS = 0, //Success
        ERR_PARAM_INVALID = -1,
        ERR_NOEXIST_USER = -2,
        ERR_NOEXIST_DATA = -3,
        ERR_EXIST_DATA = -4,
        ERR_PERMISSION = 100,
        ERR_INTERNAL_EXCEPTION = 500
    }

    [DataContract]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class ServiceResponseData
    {
        [DataMember(Name = "SVCC_RETVAL", Order = 1)]
        public ServiceError RetVal { get; set; }

        object retdata = new object();
        [DataMember(Name = "SVCC_DATA", Order = 2), Newtonsoft.Json.JsonProperty]
        public object Data
        {
            get { return retdata; }
            set { retdata = value; }
        }

        String baseurl = ConfigurationManager.AppSettings["ServerRootUri"];
        [DataMember(Name = "SVCC_BASEURL", Order = 3)]
        public String BaseUrl
        {
            get { return baseurl; }
            set { baseurl = value; }
        }
    }

    #region DataContract

    public class CheckLogInfo
    {
        public long uid { get; set; }

        //Check Info
        public long infoid { get; set; }
        public string checkinfo { get; set; }
        public string checkno { get; set; }
        public int checktype { get; set; }
        public int chkpoint { get; set; }
        public string relpoint { get; set; }
        public string category { get; set; }

        public long teamid { get; set; }
        public string teamname { get; set; }
        public long groupid { get; set; }
        public string groupname { get; set; }
        public long crewid { get; set; }
        public string crewname { get; set; }
        public string crewimg { get; set; }
        public long? relcrewid { get; set; }
        public string relcrewname { get; set; }
        public long checkerid { get; set; }
        public string checkername { get; set; }
        public string checkerimg { get; set; }
        public DateTime checktime { get; set; }
        public DateTime dutytime { get; set; }
        public string contents { get; set; }
        public string imgurl { get; set; }
    }

    public class ExamBookInfo
    {
        public long uid { get; set; }
        public ExamKind examkind { get; set; }
        public string title { get; set; }
        public int examtime { get; set; }
        public int examcount { get; set; }
        public List<long> examids { get; set; }
        public string examidstrs { get; set; }
        public string contents { get; set; }
        public DateTime createtime { get; set; }
    }

    public class ExamInfo
    {
        public long uid { get; set; }
        public ExamKind examkind { get; set; }
        public long teamid { get; set; }
        public string title { get; set; }
        public string examtype { get; set; }
        public List<SelQuestion> question { get; set; }
        public string questionstr { get; set; }
        public object answer { get; set; }
        public string answerstr { get; set; }
        public DateTime createtime { get; set; }
    }

    [DataContract]
    public class STJiDuan
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STBuMen
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STCheDui
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STBanZu
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STZeRenRen
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STLieCheZhang
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STRenYuan
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STXiangDian
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string checkno = string.Empty;
        [DataMember(Name = "CheckNo", Order = 2)]
        public string CheckNo
        {
            get { return checkno; }
            set { checkno = value; }
        }

        int chkpoint = 0;
        [DataMember(Name = "ChkPoint", Order = 3)]
        public int ChkPoint
        {
            get { return chkpoint; }
            set { chkpoint = value; }
        }

        string relpoint = string.Empty;
        [DataMember(Name = "RelPoint", Order = 4)]
        public string RelPoint
        {
            get { return relpoint; }
            set { relpoint = value; }
        }

        string category = string.Empty;
        [DataMember(Name = "Category", Order = 5)]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        string info = string.Empty;
        [DataMember(Name = "Info", Order = 6)]
        public string Info
        {
            get { return info; }
            set { info = value; }
        }
    }

    [DataContract]
    public class STKaoHeJiLu
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string chktitle = string.Empty;
        [DataMember(Name = "ChkTitle", Order = 2)]
        public string ChkTitle
        {
            get { return chktitle; }
            set { chktitle = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 3)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string chkdate = string.Empty;
        [DataMember(Name = "ChkDate", Order = 4)]
        public string ChkDate
        {
            get { return chkdate; }
            set { chkdate = value; }
        }
    }

    [DataContract]
    public class STKaoHeJiLuDet
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string chktitle = string.Empty;
        [DataMember(Name = "ChkTitle", Order = 2)]
        public string ChkTitle
        {
            get { return chktitle; }
            set { chktitle = value; }
        }

        string chedui = string.Empty;
        [DataMember(Name = "CheDui", Order = 3)]
        public string CheDui
        {
            get { return chedui; }
            set { chedui = value; }
        }

        string banzu = string.Empty;
        [DataMember(Name = "BanZu", Order = 4)]
        public string BanZu
        {
            get { return banzu; }
            set { banzu = value; }
        }

        string zerenren = string.Empty;
        [DataMember(Name = "ZeRenRen", Order = 5)]
        public string ZeRenRen
        {
            get { return zerenren; }
            set { zerenren = value; }
        }

        string liechezhang = string.Empty;
        [DataMember(Name = "LieCheZhang", Order = 6)]
        public string LieCheZhang
        {
            get { return liechezhang; }
            set { liechezhang = value; }
        }

        string imgpath = string.Empty;
        [DataMember(Name = "ImgPath", Order = 7)]
        public string ImgPath
        {
            get { return imgpath; }
            set { imgpath = value; }
        }

        string chkdata = string.Empty;
        [DataMember(Name = "ChkData", Order = 8)]
        public string ChkData
        {
            get { return chkdata; }
            set { chkdata = value; }
        }

        long chkpoint = 0;
        [DataMember(Name = "ChkPoint", Order = 9)]
        public long ChkPoint
        {
            get { return chkpoint; }
            set { chkpoint = value; }
        }

        String relpoint = string.Empty;
        [DataMember(Name = "RelPoint", Order = 10)]
        public String RelPoint
        {
            get { return relpoint; }
            set { relpoint = value; }
        }

        long relid = 0;
        [DataMember(Name = "RelID", Order = 11)]
        public long? RelID { get; set; }
    }

    [DataContract]
    public class STDaiQianItem
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string title = string.Empty;
        [DataMember(Name = "Title", Order = 2)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        string bumen = string.Empty;
        [DataMember(Name = "BuMen", Order = 3)]
        public string BuMen
        {
            get { return bumen; }
            set { bumen = value; }
        }

        string fbdate = string.Empty;
        [DataMember(Name = "FBDate", Order = 4)]
        public string FBDate
        {
            get { return fbdate; }
            set { fbdate = value; }
        }

        string faburen = string.Empty;
        [DataMember(Name = "FaBuRen", Order = 5)]
        public string FaBuRen
        {
            get { return faburen; }
            set { faburen = value; }
        }

        string tongzhihao = string.Empty;
        [DataMember(Name = "TongZhiHao", Order = 6)]
        public string TongZhiHao
        {
            get { return tongzhihao; }
            set { tongzhihao = value; }
        }

        string filepath = string.Empty;
        [DataMember(Name = "FilePath", Order = 7)]
        public string FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        string filename = string.Empty;
        [DataMember(Name = "FileName", Order = 8)]
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        long total = 0;
        [DataMember(Name = "Total", Order = 9)]
        public long Total
        {
            get { return total; }
            set { total = value; }
        }

        string content = string.Empty;
        [DataMember(Name = "Content", Order = 10)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        string liuzhuanren = string.Empty;
        [DataMember(Name = "LiuZhuanRen", Order = 11)]
        public string LiuZhuanRen
        {
            get { return liuzhuanren; }
            set { liuzhuanren = value; }
        }

        string shouxinren = string.Empty;
        [DataMember(Name = "ShouXinRen", Order = 12)]
        public string ShouXinRen
        {
            get { return shouxinren; }
            set { shouxinren = value; }
        }

        string shouxinbumen = string.Empty;
        [DataMember(Name = "ShouXinBuMen", Order = 13)]
        public string ShouXinBuMen
        {
            get { return shouxinbumen; }
            set { shouxinbumen = value; }
        }

        List<TLWebService.Model.TLModel.DocumentLog> liuzhuanrenpishi = new List<TLWebService.Model.TLModel.DocumentLog>();
        [DataMember(Name = "LiuZhuanRenPiShi", Order = 13)]
        public List<TLWebService.Model.TLModel.DocumentLog> LiuZhuanRenPiShi
        {
            get { return liuzhuanrenpishi; }
            set { liuzhuanrenpishi = value; }
        }
    }

    [DataContract]
    public class STGWFaBu
    {
        string name = string.Empty;
        [DataMember(Name = "Name", Order = 1)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    [DataContract]
    public class STKaoShiLiShiItem
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string title = string.Empty;
        [DataMember(Name = "Title", Order = 2)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        string examdate = string.Empty;
        [DataMember(Name = "ExamDate", Order = 3)]
        public string ExamDate
        {
            get { return examdate; }
            set { examdate = value; }
        }

        long total = 0;
        [DataMember(Name = "Total", Order = 4)]
        public long Total
        {
            get { return total; }
            set { total = value; }
        }

        long rightans = 0;
        [DataMember(Name = "RightAns", Order = 5)]
        public long RightAns
        {
            get { return rightans; }
            set { rightans = value; }
        }

        double mark = 0;
        [DataMember(Name = "Mark", Order = 6)]
        public double Mark
        {
            get { return mark; }
            set { mark = value; }
        }
    }

    [DataContract]
    public class STKaoShiWenTiItem
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string title = string.Empty;
        [DataMember(Name = "Title", Order = 2)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        int examtime = 0;
        [DataMember(Name = "ExamTime", Order = 3)]
        public int ExamTime
        {
            get { return examtime; }
            set { examtime = value; }
        }

        long problems = 0;
        [DataMember(Name = "Problems", Order = 4)]
        public long Problems
        {
            get { return problems; }
            set { problems = value; }
        }

        string content = string.Empty;
        [DataMember(Name = "Content", Order = 5)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        int isexam = 0;
        [DataMember(Name = "IsExam", Order = 6)]
        public int IsExam
        {
            get { return isexam; }
            set { isexam = value; }
        }
    }

    public class JsonExamInfo
    {
        public long id { get; set; }
        public string examtype { get; set; }
        public string title { get; set; }
        public List<SelQuestion> question { get; set; }
        public object answer { get; set; }
    }

    public class SelQuestion
    {
        public string ind { get; set; }
        public string question { get; set; }
    }

    public class UserInfo
    {
        public long uid { get; set; }
        public long grade { get; set; }
        public string gradename { get; set; }
    }

    [DataContract]
    public class STRoute
    {
        long uid = 0;
        [DataMember(Name = "Uid", Order = 1)]
        public long Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        string name = string.Empty;
        [DataMember(Name = "Name", Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public class CreditInfo
    {
        public long Uid = 0;
        public string CheDui = string.Empty;
        public string GongZiHao = string.Empty;
        public string Name = string.Empty;
        public string BanZu = string.Empty;
        public double DuanJiYiShang = 0.0f;
        public double BanZuCheDui = 0.0f;
        public double LianGua = 0.0f;
        public double LiGang = 0.0f;
        public double DiaoZheng = 0.0f;
        public double BenYue = 0.0f;
        public double JiLi = 0.0f;
        public double LeiJi = 0.0f;
    }

    #endregion DataContract

    public class TLModel
    {
        string SECTOR = "科室干部";
        string TEAM = "车队干部";

        /*
         * usertype 0   :   干部
         *          1   :   列车长
         *          2   :   列车员
        */
        public ServiceResponseData LoginUser(string username, string pwd)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            UserInfo userInfo = new UserInfo();
            userInfo.uid = -1;
            userInfo.grade = -1;

            /*
            DateTime dateTime = new DateTime(2014, 09, 30);
            if (DateTime.Now > dateTime)
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                userInfo.uid = (long)ServiceError.ERR_NOEXIST_USER;
                userInfo.grade = -1;
                userInfo.gradename = "测试使用日期已过了";
                retData.Data = userInfo;

                return retData;
            }
            */

            string password = Common.GetMD5Hash(pwd);

            try
            {
                tbl_user user = (from m in context.tbl_users
                                 where m.username.Equals(username) == true && m.password.Equals(password) && m.deleted == 0
                                 select m).FirstOrDefault();
                if (user == null)
                {
                    retData.RetVal = ServiceError.ERR_NOEXIST_USER;
                    userInfo.uid = (long)ServiceError.ERR_NOEXIST_USER;
                    userInfo.grade = -1;
                    userInfo.gradename = Common.ErrToMessage((int)ServiceError.ERR_NOEXIST_USER);
                    retData.Data = userInfo;
                }
                else
                {
                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    userInfo.uid = user.uid;
                    if (user.userkind == 1)
                    {
                        userInfo.grade = 0;
                        userInfo.gradename = user.exectype;
                    }
                    else if (user.userkind == 2)
                    {
                        long nCrewID = user.crewrole;
                        tbl_crewrole crewInfo = (from n in context.tbl_crewroles
                                                 where n.uid == nCrewID
                                                 select n).FirstOrDefault();
                        if (crewInfo.rolename.Equals("列车长") == true)
                        {
                            userInfo.grade = 1;
                            userInfo.gradename = "列车长";
                        }
                        else
                        {
                            userInfo.grade = 2;
                            userInfo.gradename = crewInfo.rolename;
                        }
                    }
                    retData.Data = userInfo;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("LoginUser", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                userInfo.uid = (long)ServiceError.ERR_INTERNAL_EXCEPTION;
                userInfo.grade = -1;
                userInfo.gradename = Common.ErrToMessage((int)ServiceError.ERR_INTERNAL_EXCEPTION);
                retData.Data = userInfo;
            }

            return retData;
        }


        public string GetUserLevel(long uid)
        {
            string strLevel = string.Empty;
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            tbl_user user = (from m in context.tbl_users
                             where m.uid == uid && m.deleted == 0
                             select m).FirstOrDefault();

            if (user != null)
            {
                if (user.userkind == 1)
                {
                    strLevel = user.exectype;
                }
                else if (user.userkind == 2)
                {
                    long nCrewID = user.crewrole;
                    tbl_crewrole crewInfo = (from n in context.tbl_crewroles
                                             where n.uid == nCrewID
                                             select n).FirstOrDefault();
                    if (crewInfo.rolename.Equals("列车长") == true)
                    {
                        strLevel = "列车长";
                    }
                }
            }

            return strLevel;
        }

        public ServiceResponseData GetJiDuanList()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STJiDuan> arrList = (from m in context.tbl_railsectors
                                          where m.deleted == 0
                                          select new STJiDuan
                                          {
                                              Uid = m.uid,
                                              Name = m.sectorname
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetJiDuanList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetBuMenList()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STBuMen> arrList = (from m in context.tbl_railzones
                                         where m.deleted == 0
                                         select new STBuMen
                                         {
                                             Uid = m.uid,
                                             Name = m.zonename
                                         }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetBuMenList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetCheDuiListWithUid(string bmID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nBuMenID = long.Parse(bmID);

                List<STCheDui> arrList = (from m in context.tbl_railteams
                                          where m.deleted == 0 && m.zoneid == nBuMenID
                                          select new STCheDui
                                          {
                                              Uid = m.uid,
                                              Name = m.teamname
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetCheDuiListWithUid", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetCheDuiList(string starttime)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch { }

            try
            {
                var dutylist = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime)
                    .ToList();

                var teamids = dutylist.Select(m => m.teamid).ToList();
                List<STCheDui> arrList = (from m in context.tbl_railteams
                                          where m.deleted == 0 && teamids.Contains(m.uid)
                                          select new STCheDui
                                          {
                                              Uid = m.uid,
                                              Name = m.teamname
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetCheDuiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetAllCheDuiList()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STCheDui> arrList = (from m in context.tbl_railteams
                                          where m.deleted == 0
                                          select new STCheDui
                                          {
                                              Uid = m.uid,
                                              Name = m.teamname
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetCheDuiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetAllBanZuListWithJiFen(string cdID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nBuMenID = long.Parse(cdID);

                List<STBanZu> arrList = (from m in context.tbl_traingroups
                                          where m.deleted == 0 && m.teamid == nBuMenID
                                         select new STBanZu
                                          {
                                              Uid = m.uid,
                                              Name = m.groupname
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetAllBanZuListWithJiFen", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetBanZuList(string cdID, string starttime)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            DateTime startd = new DateTime();
            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch { }

            try
            {
                long nCheDuiID = long.Parse(cdID);

                var dutylist = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.teamid == nCheDuiID)
                    .ToList();

                var groupids = dutylist.Select(m => m.groupid).ToList();

                List<STBanZu> arrList = (from m in context.tbl_traingroups
                                         where m.deleted == 0 && groupids.Contains(m.uid)
                                         select new STBanZu
                                         {
                                             Uid = m.uid,
                                             Name = m.groupname
                                         }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetCheDuiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetAllBanZuList(string cdID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nCheDuiID = long.Parse(cdID);

                var dutylist = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.teamid == nCheDuiID)
                    .ToList();

                var groupids = dutylist.Select(m => m.groupid).ToList();

                List<STBanZu> arrList = (from m in context.tbl_traingroups
                                         where m.deleted == 0 && groupids.Contains(m.uid)
                                         select new STBanZu
                                         {
                                             Uid = m.uid,
                                             Name = m.groupname
                                         }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("GetCheDuiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }


            return retData;
        }

        public ServiceResponseData GetZeRenRenList(string bzID, string starttime)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch { }

            try
            {
                long nBanZuID = long.Parse(bzID);

                var dutyinfo = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.groupid == nBanZuID)
                    .FirstOrDefault();

                if (dutyinfo != null)
                {
                    List<STZeRenRen> arrList = GetDutyZeRenRenList(dutyinfo.uid);

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = arrList;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetZeRenRenList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetAllZeRenRenList(string bzID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nBanZuID = long.Parse(bzID);

                var dutyinfo = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.groupid == nBanZuID)
                    .FirstOrDefault();

                if (dutyinfo != null)
                {
                    List<STZeRenRen> arrList = GetDutyZeRenRenList(dutyinfo.uid);

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = arrList;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetZeRenRenList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public List<STZeRenRen> GetDutyZeRenRenList(long dutyid)
        {
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            var dutycrewlist = context.tbl_dutycrews
                .Where(m => m.deleted == 0 && m.dutyid == dutyid)
                .Select(m => m.crewid)
                .ToList();

            return context.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == (int)UserKind.CREW && dutycrewlist.Contains(m.uid))
                .Join(context.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(context.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new STZeRenRen
                {
                    Uid = m.user.user.uid,
                    Name = m.user.user.realname
                })
                .ToList();
        }

        public ServiceResponseData GetLieCheZhangList(string bzID, string starttime)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            DateTime startd = new DateTime();

            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch { }

            try
            {
                long nBanZuID = long.Parse(bzID);

                var dutyinfo = context.tbl_duties
                    .Where(m => m.deleted == 0 && m.starttime <= startd && startd <= m.endtime && m.groupid == nBanZuID)
                    .FirstOrDefault();

                if (dutyinfo != null)
                {
                    List<STLieCheZhang> arrList = GetDutyLieCheZhangList(dutyinfo.uid);

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = arrList;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetZeRenRenList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public List<STLieCheZhang> GetDutyLieCheZhangList(long dutyid)
        {
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            var dutycrewlist = context.tbl_dutycrews
                .Where(m => m.deleted == 0 && m.dutyid == dutyid)
                .Select(m => m.crewid)
                .ToList();

            return context.tbl_users
                .Where(m => m.deleted == 0 && m.crewrole == (int)CrewKind.LIECHEZHANG && m.userkind == (int)UserKind.CREW && dutycrewlist.Contains(m.uid))
                .Join(context.tbl_crewroles, m => m.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
                .Join(context.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                .Select(m => new STLieCheZhang
                {
                    Uid = m.user.user.uid,
                    Name = m.user.user.realname
                })
                .ToList();
        }

        public ServiceResponseData GetXiangDianList(string query)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STXiangDian> arrList = null;

                if (query.Length == 0)
                {
                    arrList = (from m in context.tbl_checkinfos
                               where m.deleted == 0
                               orderby m.sortid ascending
                               select new STXiangDian
                               {
                                   Uid = m.uid,
                                   CheckNo = m.checkno,
                                   ChkPoint = m.chkpoint,
                                   RelPoint = m.relpoint,
                                   Category = m.category,
                                   Info = m.checkinfo
                               }).ToList();
                }
                else
                {
                    arrList = (from m in context.tbl_checkinfos
                               where m.deleted == 0 && (m.checkno.Contains(query) || m.relpoint.Contains(query) || m.category.Contains(query) || m.checkinfo.Contains(query))
                               orderby m.sortid ascending
                               select new STXiangDian
                               {
                                   Uid = m.uid,
                                   CheckNo = m.checkno,
                                   ChkPoint = m.chkpoint,
                                   RelPoint = m.relpoint,
                                   Category = m.category,
                                   Info = m.checkinfo
                               }).ToList();
                }

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrList;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetXiangDianList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetKaoHeJiLuList(string cdID, string bzID, string ryID, string dtstart, string dtend)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nCheDuiID = long.Parse(cdID);
                long nBanZuID = long.Parse(bzID);
                long nRenYuanID = long.Parse(ryID);
                DateTime startDate = Convert.ToDateTime(dtstart);
                DateTime endDate = Convert.ToDateTime(dtend);

                List<STKaoHeJiLu> arrData = context.tbl_checklogs
                                            .Where(m => m.deleted == 0 && startDate <= m.checktime && m.checktime < endDate.AddDays(1) && m.teamid == nCheDuiID && m.groupid == nBanZuID && m.crewid == nRenYuanID)
                                            .Join(context.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                                            .Join(context.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                                            .Join(context.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                                            .Join(context.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                                            .Join(context.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                                            .Select(m => new STKaoHeJiLu
                                            {
                                                Uid = m.log.log.log.log.log.uid,
                                                ChkTitle = m.log.log.log.log.chkinfo.checkinfo,
                                                Name = m.log.crew.realname,
                                                ChkDate = String.Format("{0:yyyy.MM.dd}", m.log.log.log.log.log.checktime)
                                            }).ToList();
                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoHeJiLuList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetKaoHeJiLuDet(string uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nUID = long.Parse(uid);

                STKaoHeJiLuDet objData = context.tbl_checklogs
                                            .Where(m => m.deleted == 0 && m.uid == nUID)
                                            .Join(context.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { log = m, chkinfo = l })
                                            .Join(context.tbl_railteams, m => m.log.teamid, l => l.uid, (m, l) => new { log = m, team = l })
                                            .Join(context.tbl_traingroups, m => m.log.log.groupid, l => l.uid, (m, l) => new { log = m, group = l })
                                            .Join(context.tbl_users, m => m.log.log.log.crewid, l => l.uid, (m, l) => new { log = m, crew = l })
                                            .Join(context.tbl_users, m => m.log.log.log.log.checkerid, l => l.uid, (m, l) => new { log = m, checker = l })
                                            .Select(m => new STKaoHeJiLuDet
                                            {
                                                Uid = nUID,
                                                ChkTitle = m.log.log.log.log.chkinfo.checkinfo,
                                                CheDui = m.log.log.log.team.teamname,
                                                BanZu = m.log.log.group.groupname,
                                                ZeRenRen = m.log.crew.realname,
                                                LieCheZhang = "",
                                                RelID = m.log.log.log.log.log.crewrelid,
                                                ImgPath = m.log.log.log.log.log.imgurl,
                                                ChkData = m.log.log.log.log.log.contents,
                                                ChkPoint = m.log.log.log.log.chkinfo.chkpoint,
                                                RelPoint = m.log.log.log.log.chkinfo.relpoint
                                            }).FirstOrDefault();

                if (objData != null)
                {
                    if (objData.RelID != null)
                    {
                        tbl_user userData = (from m in context.tbl_users
                                             where m.deleted == 0 && m.uid == objData.RelID && m.userkind == (int)UserKind.CREW
                                             select m).FirstOrDefault();
                        if (userData != null)
                        {
                            objData.LieCheZhang = (userData.realname == null) ? "" : userData.realname;
                        }
                    }
                }

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = objData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoHeJiLuDet", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetKaoHeTongJi(string cdID, string bzID, string dtstart, string dtend)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nCheDuiID = long.Parse(cdID);
                long nBanZuID = long.Parse(bzID);
                DateTime startDate = Convert.ToDateTime(dtstart);
                DateTime endDate = Convert.ToDateTime(dtend);
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoHeTongJi", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public List<long> GetDutynoByGroup(string starttime, long groupid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            DateTime startd = new DateTime();
            try
            {
                startd = DateTime.Parse(starttime);
            }
            catch (System.Exception ex)
            {

            }
            var dutylist = db.tbl_duties.Where(m => m.deleted == 0);
            var trainno = dutylist.Where(m => m.starttime <= startd && m.endtime >= startd && m.groupid == groupid)
                                  .Join(db.tbl_trainnos, m => m.trainno, l => l.trainno, (m, l) => new { duty = m, train = l })
                                  .Select(m => m.train.uid).ToList();
            return trainno;
        }

        public long FindTrainnoOntblDutyByGroup(long bzID, string startDate)
        {
            long result = 0;
            var rst = GetDutynoByGroup(startDate, bzID);
            if (rst.Count > 0)
            {
                result = rst[0];
            }

            return result;
        }

        public ServiceResponseData SetKaoHeItem(string uid, string starttime, string checktime, string cdID, string bzID,
                                                 string zrrID, string lczID, string xdID, string content, string imgdata)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            Common.WriteLogFile("SetKaoHeItem", "is called!");

            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string filename = String.Format("{0:yyyyMMddhhmmss}", DateTime.Now) + ".jpg";
            string targetbase = ConfigurationManager.AppSettings["DataSavePath"] + savepath;

            DateTime startd = new DateTime();
            DateTime checkd = new DateTime();

            long nUID = 0;
            long nCheDuiID = 0, nBanZuID = 0, nZeRenRenID = 0, nLieCheZhangID = 0, nXiangDianID = 0;

            long routid = 0;
            long trainnoo = 0;

            try
            {
                startd = DateTime.Parse(starttime);
                checkd = DateTime.Parse(checktime);
                nUID = long.Parse(uid);
                nCheDuiID = long.Parse(cdID);
                nBanZuID = long.Parse(bzID);
                nZeRenRenID = long.Parse(zrrID);
                nLieCheZhangID = long.Parse(lczID);
                nXiangDianID = long.Parse(xdID);

                trainnoo = FindTrainnoOntblDutyByGroup(nBanZuID, starttime);

                if (trainnoo != 0)
                {
                    routid = context.tbl_trainnos.Where(m => m.uid == trainnoo).Select(m => m.routeid).ToList()[0];
                }


                byte[] bytes = Convert.FromBase64String(imgdata);
                if (!Directory.Exists(targetbase))
                {
                    Directory.CreateDirectory(targetbase);
                }

                File.WriteAllBytes(targetbase + filename, bytes);

            }
            catch (Exception ex)
            {
                Common.WriteLogFile("SetKaoHeItem", ex.Message);
                string str = ex.Message;
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;
                retData.Data = null;
            }

            try
            {
                tbl_checklog newitem = new tbl_checklog();

                string strLevel = GetUserLevel(nUID);
                if (strLevel.Equals(ExecType.SectorExec))
                {
                    strLevel = "段级";
                }
                else if (strLevel.Equals(ExecType.TeamExec))
                {
                    strLevel = "车队";
                }
                else if (strLevel.Equals("列车长"))
                {
                    strLevel = "班组";
                }

                string groupname = "";
                if (nBanZuID != 0)
                {
                    groupname = context.tbl_traingroups.Where(m => m.uid == nBanZuID).Select(m => m.groupname).ToList()[0];
                }
                var recivename = "";
                if (nZeRenRenID != 0)
                {
                    recivename = context.tbl_users.Where(m => m.uid == nZeRenRenID).Select(m => m.realname).ToList()[0];
                }
                
                newitem.infoid = nXiangDianID;
                newitem.teamid = nCheDuiID;
                newitem.groupid = nBanZuID;
                newitem.crewid = nZeRenRenID;
                newitem.crewrelid = nLieCheZhangID;
                newitem.checkerid = nUID;
                newitem.checktime = checkd;
                newitem.dutytime = startd;
                newitem.contents = content;
                newitem.routid = routid;
                newitem.imgurl = savepath + filename;
                newitem.logtype = "两违考核";
                newitem.checklevel = strLevel;
                newitem.trainno = trainnoo;
                newitem.receivepart = groupname;
                newitem.recievename = recivename;

                newitem.createtime = DateTime.Now;

                string szExcetype = "";
                string szCheckSector = "";
                szExcetype = (from m in context.tbl_users
                              where m.deleted == 0 && m.uid == nUID
                              select m.exectype).FirstOrDefault();
                if (szExcetype != null)
                {
                    if (szExcetype.Equals(ExecType.SectorExec))
                    {
                        szCheckSector = (from m in context.tbl_railsectors
                                         join n in context.tbl_users on m.uid equals n.execparentid
                                         where m.deleted == 0 && n.deleted == 0 && n.uid == nUID
                                         select m.sectorname).FirstOrDefault();
                    }
                    else if (szExcetype.Equals(ExecType.TeamExec))
                    {
                        szCheckSector = (from m in context.tbl_railteams
                                         join n in context.tbl_users on m.uid equals n.execparentid
                                         where m.deleted == 0 && n.deleted == 0 && n.uid == nUID
                                         select m.teamname).FirstOrDefault();
                    }
                }
                
                newitem.checkexecparent = (szCheckSector == null)?"":szCheckSector;
                newitem.checkername = getRealNameWithUID(nUID);

                context.tbl_checklogs.InsertOnSubmit(newitem);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("SetKaoHeItem", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetAllCDRenYuanList(string cdID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nCheDuiID = long.Parse(cdID);

                List<STRenYuan> arrData = (from m in context.tbl_users
                                           where m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.execparentid == nCheDuiID && m.exectype == TEAM
                                           select new STRenYuan
                                           {
                                               Uid = m.uid,
                                               Name = m.realname
                                           }).ToList();

                retData.Data = arrData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetAllCDRenYuanList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetAllBMRenYuanList(string bmID)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nBuMenID = long.Parse(bmID);

                List<STRenYuan> arrData = (from m in context.tbl_users
                                           where m.deleted == 0 && m.userkind == (int)UserKind.EXECUTIVE && m.execparentid == nBuMenID && m.exectype == SECTOR
                                           select new STRenYuan
                                           {
                                               Uid = m.uid,
                                               Name = m.realname
                                           }).ToList();

                retData.Data = arrData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetAllBMRenYuanList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetDaiQianItem(string uid, string pageno)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<STDaiQianItem> rst = new List<STDaiQianItem>();

            try
            {
                int nPageNo = int.Parse(pageno);
                int nUID = int.Parse(uid);

                var filterlist = context.tbl_documents
                    .OrderByDescending(m => m.createtime)
                    .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                    .Where(m => m.doc.deleted == 0)
                    .Select(m => new STDaiQianItem
                    {
                        Uid = m.doc.uid,
                        Title = m.doc.title,
                        BuMen = m.doc.sendpart,
                        FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                        FaBuRen = m.sender.realname,
                        TongZhiHao = m.doc.docno,
                        FileName = m.doc.attachname,
                        FilePath = m.doc.attachpath,
                        Total = 0,
                        Content = m.doc.contents,
                        ShouXinRen = m.doc.receiver,
                        ShouXinBuMen = m.doc.receiverange
                    }).ToList();

                foreach (var item in filterlist)
                {
                    var receiverids = item.ShouXinRen.Split(',');

                    if (receiverids.Contains(nUID.ToString()))
                    {
                        rst.Add(item);
                    }
                }

                var loglist = context.tbl_documentlogs.Where(m => m.deleted == 0).ToList();

                List<long> forwdocids = new List<long>();
                foreach (var item in loglist)
                {
                    var receiverids = item.receiver.Split(',');
                    if (receiverids.Contains(nUID.ToString()))
                    {
                        forwdocids.Add(item.docid);
                    }
                }

                if (forwdocids.Count() > 0)
                {
                    var forwardlist = context.tbl_documents
                        .Where(m => m.deleted == 0 && forwdocids.Contains(m.uid))
                        .OrderByDescending(m => m.createtime)
                        .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Select(m => new STDaiQianItem
                        {
                            Uid = m.doc.uid,
                            Title = m.doc.title,
                            BuMen = m.doc.sendpart,
                            FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                            FaBuRen = m.sender.realname,
                            TongZhiHao = m.doc.docno,
                            FileName = m.doc.attachname,
                            FilePath = m.doc.attachpath,
                            Total = 0,
                            Content = m.doc.contents,
                            ShouXinRen = m.doc.receiver,
                            ShouXinBuMen = m.doc.receiverange
                        })
                        .ToList();

                    rst.AddRange(forwardlist);
                }

                var signlist = loglist.Where(m => m.deleted == 0 && m.userid == nUID).ToList();
                var docids = signlist.Select(m => m.docid).ToList();

                rst = rst.Where(m => !docids.Contains(m.Uid)).ToList();
                if (rst != null && rst.Count >= (nPageNo + 1))
                {
                    rst[nPageNo].Total = rst.Count;
                    retData.Data = rst[nPageNo];
                }
                else
                {
                    retData.Data = null;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetDaiQianList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetYiShouItem(string uid, string pageno)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<STDaiQianItem> rst = new List<STDaiQianItem>();

            try
            {
                int nPageNo = int.Parse(pageno);
                int nUID = int.Parse(uid);

                var filterlist = context.tbl_documents
                    .OrderByDescending(m => m.createtime)
                    .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                    .Where(m => m.doc.deleted == 0)
                    .Select(m => new STDaiQianItem
                    {
                        Uid = m.doc.uid,
                        Title = m.doc.title,
                        BuMen = m.doc.sendpart,
                        FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                        FaBuRen = m.sender.realname,
                        TongZhiHao = m.doc.docno,
                        FileName = m.doc.attachname,
                        FilePath = m.doc.attachpath,
                        Total = 0,
                        Content = m.doc.contents,
                        ShouXinRen = m.doc.receiver,
                        ShouXinBuMen = m.doc.receiverange,
                        LiuZhuanRenPiShi = GetMyDocLogList(m.doc.uid)
                    }).ToList();

                foreach (var item in filterlist)
                {
                    var receiverids = item.ShouXinRen.Split(',');

                    if (receiverids.Contains(nUID.ToString()))
                    {
                        rst.Add(item);
                    }
                }

                var loglist = context.tbl_documentlogs.Where(m => m.deleted == 0 && m.userid == nUID).ToList();
                var docids = loglist.Select(m => m.docid).ToList();

                rst = rst.Where(m => docids.Contains(m.Uid)).ToList();

                if (rst != null && rst.Count >= (nPageNo + 1))
                {
                    rst[nPageNo].Total = rst.Count;
                    retData.Data = rst[nPageNo];
                }
                else
                {
                    retData.Data = null;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetYiShouItem", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData SetGongWenItem(string uid, string title, string docno, string receiver, string sector,
                                                string outdate, string attach, string attachdate, string pubcontent,
                                                string attachcontent, string filename, string filedata)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;
            DateTime outDate = new DateTime();

            long nLength = 0;
            string savepath = "Content/uploads/attachment/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string targetbase = ConfigurationManager.AppSettings["DataSavePath"] + savepath;

            try
            {
                nUID = long.Parse(uid);
                outDate = DateTime.Parse(outdate);

                if (filename.Length != 0)
                {
                    byte[] bytes = Convert.FromBase64String(filedata);
                    nLength = bytes.Length;
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }

                    File.WriteAllBytes(targetbase + filename, bytes);
                }
            }
            catch (Exception ex)
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;
                retData.Data = ex.Message;

                return retData;
            }

            try
            {
                tbl_document newitem = new tbl_document();
                newitem.sender = nUID;
                newitem.sendpart = sector;
                newitem.receiver = receiver;
                newitem.title = title;
                newitem.docno = docno;
                newitem.contents = pubcontent;
                newitem.attachname = filename;
                if (filename.Length == 0)
                    newitem.attachpath = string.Empty;
                else
                    newitem.attachpath = savepath + filename;
                newitem.attachsize = nLength;
                newitem.createtime = DateTime.Now;

                context.tbl_documents.InsertOnSubmit(newitem);
                context.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("SetGongWenItem", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetGWFaBuList()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STGWFaBu> arrData = (from m in context.tbl_documents
                                          where m.deleted == 0
                                          group m by m.sendpart into g
                                          select new STGWFaBu
                                          {
                                              Name = g.Key.ToString()
                                          }
                ).ToList();

                retData.Data = arrData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetGWFaBuList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetGongWenList(string bumen, string startdate, string enddate, string query, long userid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<STDaiQianItem> rst = new List<STDaiQianItem>();

            try
            {
                if (userid > 0)
                {
                    rst = context.tbl_documents
                        .OrderByDescending(m => m.createtime)
                        .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Where(m => m.doc.deleted == 0 && m.doc.sender == userid)
                        .Select(m => new STDaiQianItem
                        {
                            Uid = m.doc.uid,
                            Title = m.doc.title,
                            BuMen = m.doc.sendpart,
                            FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                            FaBuRen = m.sender.realname,
                            TongZhiHao = m.doc.docno,
                            FileName = m.doc.attachname,
                            FilePath = m.doc.attachpath,
                            Total = 0,
                            Content = m.doc.contents
                        }).ToList();
                }
                else
                {
                    DateTime startd = new DateTime();
                    DateTime endd = new DateTime();
                    startd = DateTime.Parse(startdate);
                    endd = DateTime.Parse(enddate);

                    rst = context.tbl_documents
                        .OrderByDescending(m => m.createtime)
                        .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Where(m => m.doc.deleted == 0 && startd <= m.doc.createtime && m.doc.createtime < endd.AddDays(1)
                                 && m.doc.sendpart == bumen && (m.doc.title.Contains(query) || (m.doc.docno.Contains(query))))
                        .Select(m => new STDaiQianItem
                        {
                            Uid = m.doc.uid,
                            Title = m.doc.title,
                            BuMen = m.doc.sendpart,
                            FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                            FaBuRen = m.sender.realname,
                            TongZhiHao = m.doc.docno,
                            FileName = m.doc.attachname,
                            FilePath = m.doc.attachpath,
                            Total = 0,
                            Content = m.doc.contents
                        }).ToList();
                }

                retData.Data = rst;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetGongWenList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null; ;
            }

            return retData;
        }

        public long GetProblemCount(long bookid)
        {
            try { 
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            tbl_exambook examBook = (from m in context.tbl_exambooks
                                     where m.deleted == 0 && m.uid == bookid
                                     select m).FirstOrDefault();

            if (examBook != null)
                return examBook.examids.Count(x => x == ',');
            }
            catch { return 0; }

            return 0;
        }

        public ServiceResponseData GetKaoShiLiShiList(string uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;

            try
            {
                nUID = long.Parse(uid);
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;

                return retData;
            }

            try
            {
                List<STKaoShiLiShiItem> arrData = (from m in context.tbl_examresults
                                                   join n in context.tbl_exambooks on m.exambookid equals n.uid
                                                   where m.deleted == 0 && m.userid == nUID
                                                   select new STKaoShiLiShiItem
                                                   {
                                                       Uid = m.uid,
                                                       Title = n.title + ((n.examkind == 0) ? "(段级考试)" : "(车队考试)"),
                                                       ExamDate = String.Format("{0:yyyy年MM月dd日}", m.createtime),
                                                       Total = GetProblemCount(n.uid),
                                                       Mark = Decimal.ToDouble(m.score)
                                                   }).ToList();

                foreach (STKaoShiLiShiItem item in arrData)
                {
                    item.RightAns = (long)((item.Mark * item.Total) / 100.0f) + 1;
                }

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoShiLiShiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetKaoShiWenTiList(string uid, string kind)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;
            int nType = 0;

            try
            {
                nUID = long.Parse(uid);
                nType = int.Parse(kind);
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;

                return retData;
            }

            try
            {
                List<tbl_exambook> arrList = (from m in context.tbl_exambooks
                                              where m.deleted == 0 && m.examkind == nType
                                              select m).ToList();

                List<STKaoShiWenTiItem> arrData = new List<STKaoShiWenTiItem>();

                if (arrList == null)
                {
                    retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                    retData.Data = null;
                }
                else
                {
                    for (int i = 0; i < arrList.Count; i++)
                    {
                        STKaoShiWenTiItem newItem = new STKaoShiWenTiItem();

                        newItem.Uid = arrList[i].uid;
                        newItem.Title = arrList[i].title;
                        newItem.ExamTime = arrList[i].examtime;
                        newItem.Content = arrList[i].contents;
                        newItem.Problems = arrList[i].examids.Count(x => x == ',');

                        tbl_examresult itemResult = (from m in context.tbl_examresults
                                                     where m.deleted == 0 && m.exambookid == newItem.Uid && m.userid == nUID
                                                     select m).SingleOrDefault();
                        if (itemResult == null)
                            newItem.IsExam = 0;
                        else
                            newItem.IsExam = 1;

                        arrData.Add(newItem);
                    }

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = arrData;
                }
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoShiWenTiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public List<string> ParseMultiSelAnswer(string answer)
        {
            List<string> rst = new List<string>();

            rst = answer.Split(',').ToList();

            return rst;
        }

        public List<SelQuestion> ParseMultiSelQuestion(string question)
        {
            List<SelQuestion> rst = new List<SelQuestion>();

            string[] lines = question.Split('\n');

            foreach (var item in lines)
            {
                SelQuestion newitem = new SelQuestion();
                string[] litem = item.Split('.');
                if (litem.Length > 1)
                {
                    newitem.ind = litem[0];
                    newitem.question = item.Substring(item.IndexOf('.') + 1);

                    rst.Add(newitem);
                }
            }

            return rst;
        }

        public List<ExamInfo> GetSelectedExamList(long[] selids)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            var rst = db.tbl_exams
                .Where(m => m.deleted == 0 && selids.Contains(m.uid))
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    teamid = m.teamid,
                    title = m.title,
                    examtype = m.examtype,
                    questionstr = m.question,
                    answerstr = m.answer,
                    createtime = m.createtime
                })
                .ToList();

            return rst;
        }

        public ExamBookInfo GetExamBookInfo(long uid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            var rst = db.tbl_exambooks
                .Where(m => m.deleted == 0 && m.uid == uid)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    title = m.title,
                    examtime = m.examtime,
                    //examids = m.examids,
                    examidstrs = m.examids,
                    contents = m.contents,
                    createtime = m.createtime
                })
                .FirstOrDefault();

            if (rst != null)
            {
                List<long> examids = new List<long>();
                string[] ids = rst.examidstrs.Split(',');
                foreach (var item in ids)
                {
                    if (!String.IsNullOrWhiteSpace(item))
                    {
                        examids.Add(long.Parse(item));
                    }
                }
                rst.examids = examids;
                rst.examcount = examids.Count();
            }

            return rst;
        }

        public ServiceResponseData GetKaoShiList(string pid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nPID = 0;

            try
            {
                nPID = long.Parse(pid);
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;

                return retData;
            }

            try
            {
                var bookinfo = GetExamBookInfo(nPID);
                var examlist = GetJsonExamList(bookinfo.examids);

                retData.Data = examlist;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetKaoShiList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public List<JsonExamInfo> GetJsonExamList(List<long> ids)
        {
            List<JsonExamInfo> rst = new List<JsonExamInfo>();

            var examlist = GetSelectedExamList(ids.ToArray());

            if (examlist != null)
            {
                foreach (var item in examlist)
                {
                    JsonExamInfo newitem = new JsonExamInfo();
                    newitem.id = item.uid;
                    newitem.examtype = item.examtype;
                    newitem.title = item.title;

                    newitem.question = new List<SelQuestion>();

                    if (item.examtype == ExamType.YesNo)
                    {
                        string xlsanswer = (item.answerstr.ToUpper() == "YES") ? "1" : "0";
                        List<string> arrAnswer = new List<string>();
                        arrAnswer.Add(xlsanswer);
                        newitem.answer = arrAnswer;
                    }
                    else if (item.examtype == ExamType.MultiSel)
                    {
                        newitem.question = ParseMultiSelQuestion(item.questionstr);
                        newitem.answer = ParseMultiSelAnswer(item.answerstr);
                    }
                    else if (item.examtype == ExamType.OneSel)
                    {
                        newitem.question = ParseMultiSelQuestion(item.questionstr);
                        List<string> arrAnswer = new List<string>();
                        arrAnswer.Add(item.answerstr.Trim());
                        newitem.answer = arrAnswer;
                    }

                    rst.Add(newitem);
                }
            }

            return rst;
        }

        public class CategoryInfo
        {
            public long uid { get; set; }
            public string category { get; set; }
        }

        public class CategoryStatistics
        {
            public string category { get; set; }
            public long count { get; set; }
        }

        public ServiceResponseData GetKaoHeChartData(long teamid, long groupid, string starttime, string endtime)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            DateTime startd = new DateTime(1970, 1, 1);
            DateTime endd = new DateTime(2040, 1, 1);

            try { startd = DateTime.Parse(starttime); }
            catch { }
            try { endd = DateTime.Parse(endtime); }
            catch { }

            var listData = (from m in context.tbl_checklogs
                            join n in context.tbl_checkinfos on m.infoid equals n.uid
                            where m.deleted == 0 && n.deleted == 0 && m.teamid == teamid && m.groupid == groupid && startd <= m.checktime && m.checktime < endd.AddDays(1)
                            select new CategoryInfo
                            {
                                uid = n.uid,
                                category = n.category
                            }).ToList();

            List<tbl_checklog> arrLog = (from m in context.tbl_checklogs
                                         where m.deleted == 0 && m.teamid == teamid && m.groupid == groupid && startd <= m.checktime && m.checktime < endd.AddDays(1)
                                         select m).ToList();

            List<String> listCategory = listData.Select(m => m.category).Distinct().ToList();
            var listUid = listData.Select(m => m.uid).Distinct().ToList();

            int index = 0;
            List<CategoryStatistics> arrRet = new List<CategoryStatistics>();
            foreach (var item in listUid)
            {
                int nCount = arrLog.Select(m => m).Where(m => m.infoid == item).Count();
                CategoryStatistics obj = new CategoryStatistics();
                obj.category = listCategory[index];
                obj.count = nCount;

                arrRet.Add(obj);
                index++;
            }

            retData.Data = arrRet;

            return retData;
        }

        public ServiceResponseData SetKaoShiJieGuo(
            string uid,
            string eid,
            string score,
            string examres,
            string examsecond)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nEID = 0;
            int nScore = 0;

            try
            {
                nUID = long.Parse(uid);
                nEID = long.Parse(eid);
                nScore = int.Parse(score);
            }
            catch (Exception ex)
            {
                Common.WriteLogFile("SetKaoShiJieGuo", ex.Message);
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;

                return retData;
            }

            try
            {
                tbl_examresult newItem = new tbl_examresult();
                newItem.userid = nUID;
                newItem.exambookid = nEID;
                newItem.score = nScore;
                newItem.examsecond = examsecond;
                newItem.participtime = DateTime.Now;
                newItem.createtime = DateTime.Now;
                newItem.deleted = 0;
                newItem.examresult = string.Empty;

                context.tbl_examresults.InsertOnSubmit(newItem);
                context.SubmitChanges();

                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("SetKaoShiJieGuo", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public ServiceResponseData SetLiuZhuanData(string uid, string docid, string content, string receiver)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nDocID = 0;

            try
            {
                nUID = long.Parse(uid);
                nDocID = long.Parse(docid);
            }
            catch { }

            try
            {                
                tbl_documentlog logItem = context.tbl_documentlogs
                        .Where(m => m.docid == nDocID && m.deleted == 0)
                        .FirstOrDefault();

                if (logItem != null)
                {
                    retData.RetVal = ServiceError.ERR_EXIST_DATA;                    
                    return retData;
                }

                tbl_documentlog newitem = new tbl_documentlog();
                newitem.acttype = 1;
                newitem.userid = nUID;
                newitem.createtime = DateTime.Now;
                newitem.docid = nDocID;
                newitem.receiver = receiver;
                newitem.execnote = content;

                context.tbl_documentlogs.InsertOnSubmit(newitem);
                context.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("SetLiuZhuanData", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public ServiceResponseData SetQianShouData(string uid, string docid, string content, string receiver)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nDocID = 0;

            try
            {
                nUID = long.Parse(uid);
                nDocID = long.Parse(docid);
            }
            catch { }

            try
            {
                tbl_documentlog newitem = new tbl_documentlog();
                newitem.acttype = 0;
                newitem.userid = nUID;
                newitem.createtime = DateTime.Now;
                newitem.docid = nDocID;
                newitem.receiver = receiver;
                newitem.execnote = content;

                context.tbl_documentlogs.InsertOnSubmit(newitem);
                context.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("SetLiuZhuanData", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public class ExecutiveInfo
        {
            public long uid { get; set; }
            public string username { get; set; }
            public string realname { get; set; }
            public DateTime? birthday { get; set; }
            public string birthdaystr { get; set; }
            public byte gender { get; set; }
            public string phonenum { get; set; }
            public string execrole { get; set; }
            public string exectype { get; set; }
            public long parentid { get; set; }
            public string parentname { get; set; }
            public string imgurl { get; set; }
        }

        public long GetCurrZoneId()
        {
            long rst = 0;
            return rst;
        }

        public List<tbl_railsector> GetSectorList()
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            long zoneid = GetCurrZoneId();

            return db.tbl_railsectors
                .Where(m => m.deleted == 0 && m.zoneid == zoneid)
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public List<tbl_railteam> GetTeamList()
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            long zoneid = GetCurrZoneId();

            return db.tbl_railteams
                .Where(m => m.deleted == 0 && m.zoneid == zoneid)
                .OrderBy(m => m.sortid)
                .ToList();
        }

        public ExecutiveInfo GetDocExecUserInfo(long uid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind <= (int)UserKind.EXECUTIVE && m.uid == uid)
                .OrderBy(m => m.createtime)
                .Select(m => new ExecutiveInfo
                {
                    uid = m.uid,
                    username = m.username,
                    realname = m.realname,
                    birthday = m.birthday,
                    birthdaystr = m.birthday.ToString(),
                    gender = m.gender,
                    phonenum = m.phonenum,
                    exectype = m.exectype,
                    parentid = m.execparentid
                })
                .FirstOrDefault();

            var sectorlist = GetSectorList();
            var teamlist = GetTeamList();

            if (rst != null)
            {
                if (rst.exectype == TLWebService.ServiceModel.Common.ExecType.SectorExec)
                {
                    rst.parentname = sectorlist.Where(m => m.uid == rst.parentid).Select(m => m.sectorname).FirstOrDefault();
                }
                else if (rst.exectype == TLWebService.ServiceModel.Common.ExecType.TeamExec)
                {
                    rst.parentname = teamlist.Where(m => m.uid == rst.parentid).Select(m => m.teamname).FirstOrDefault();
                }
                else
                {
                    rst.parentname = "系统管理员";
                }
            }

            return rst;
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
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;

            try
            {
                nUID = long.Parse(uid);
            }
            catch { }

            DateTime startd = new DateTime(1970, 1, 1);
            DateTime endd = new DateTime(2040, 1, 1);

            try { startd = DateTime.Parse(starttime); }
            catch { }
            try { endd = DateTime.Parse(endtime); }
            catch { }

            long nLength = 0;
            string savepath = "Content/uploads/attachment/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string targetbase = ConfigurationManager.AppSettings["DataSavePath"] + savepath;

            try
            {
                if (filename.Length != 0)
                {
                    byte[] bytes = Convert.FromBase64String(filedata);
                    nLength = bytes.Length;
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }

                    File.WriteAllBytes(targetbase + filename, bytes);
                }
            }
            catch (Exception ex)
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;
                retData.Data = ex.Message;

                return retData;
            }

            try
            {
                var senderinfo = GetDocExecUserInfo(nUID);

                tbl_task newitem = new tbl_task();

                newitem.title = title;
                newitem.sendpart = senderinfo.parentname;
                newitem.senderid = nUID;
                newitem.receiver = receiver;
                newitem.starttime = startd;
                newitem.endtime = endd;
                newitem.contents = contents;
                newitem.attachname = filename;
                if (filename.Length == 0)
                    newitem.attachpath = string.Empty;
                else
                    newitem.attachpath = savepath + filename;
                newitem.attachsize = nLength;
                newitem.createtime = DateTime.Now;
                newitem.publishtime = DateTime.Now;
                newitem.deleted = 0;

                context.tbl_tasks.InsertOnSubmit(newitem);
                context.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("SetLiuZhuanData", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public enum TaskStatus
        {
            NOTACCPET = 0,
            EXECUTING,
            FINISH
        }

        public class TaskInfo
        {
            public long uid { get; set; }
            public long senderid { get; set; }
            public string sendername { get; set; }
            public string sendpart { get; set; }
            public string receiver { get; set; }
            public string receivernames { get; set; }
            public string title { get; set; }
            public string contents { get; set; }
            public string attachname { get; set; }
            public string attachpath { get; set; }
            public long? attachsize { get; set; }
            public DateTime starttime { get; set; }
            public string starttime_desc { get; set; }
            public DateTime endtime { get; set; }
            public string endtime_desc { get; set; }
            public DateTime publishtime { get; set; }
            public DateTime createtime { get; set; }
            public DateTime? accepttime { get; set; }
            public DateTime? finishtime { get; set; }
            public TaskStatus status { get; set; }
            public string statusstr { get; set; }
        }

        public ServiceResponseData GetMyTaskList(string uid, string pageno)
        {
            TLDatabaseDataContext context = new TLDatabaseDataContext();
            ServiceResponseData retData = new ServiceResponseData();

            long nUID = 0;
            int nPageNo = 0;

            try
            {
                nUID = long.Parse(uid);
                nPageNo = int.Parse(pageno);
            }
            catch { }

            try
            {
                List<TaskInfo> rst = new List<TaskInfo>();

                var filterlist = context.tbl_tasks
                    .Where(m => m.deleted == 0)
                    .OrderByDescending(m => m.createtime)
                    .Join(context.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
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
                        starttime_desc = string.Format("{0:yyyy-MM-dd hh:mm}", m.doc.starttime),
                        endtime = m.doc.endtime,
                        endtime_desc = string.Format("{0:yyyy-MM-dd hh:mm}", m.doc.endtime)
                    })
                    .ToList();

                foreach (var item in filterlist)
                {
                    string[] receiverids = item.receiver.Trim().Split(',');

                    if (receiverids.Contains(uid))
                    {
                        TaskStatus i = TaskStatus.NOTACCPET;

                        var status = context.tbl_taskstatus.Where(m => m.taskid == item.uid).FirstOrDefault();

                        if (status != null)
                        {
                            i = (TaskStatus)status.status;
                            item.accepttime = status.accepttime;
                            item.finishtime = status.finishtime;
                        }
                        else
                        {
                            i = TaskStatus.NOTACCPET;
                        }

                        if (i == TaskStatus.NOTACCPET)
                        {
                            item.statusstr = "待接收";
                        }
                        else if (i == TaskStatus.EXECUTING)
                        {
                            item.statusstr = "执行中";
                        }
                        else if (i == TaskStatus.FINISH)
                        {
                            item.statusstr = "已完成";
                        }
                        else
                        {
                            item.statusstr = "待接收";
                        }
                        
                        rst.Add(item);

                    }
                }

                retData.Data = rst.Skip(3 * nPageNo).Take(3).ToArray();
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public string getRealNameWithUID(long uid)
        {
            string strName = string.Empty;
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            strName = (from m in context.tbl_users
                       where m.deleted == 0 && m.uid == uid
                       select m.realname).FirstOrDefault();

            if (strName == null)
                strName = string.Empty;

            return strName;
        }

        public class STSendTaskItem
        {
            public long uid;
            public string title;
            public string executor;
            public int status;
            public string status_desc;

        }

        public ServiceResponseData GetMySendTaskList(string uid, string pageno)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();
            List<TaskInfo> rst = new List<TaskInfo>();

            long nUID = 0;
            int nPageNo = 0;

            try
            {
                nUID = long.Parse(uid);
                nPageNo = int.Parse(pageno);
            }
            catch { }

            try
            {
                var filterlist = context.tbl_tasks
                .Where(m => m.deleted == 0 && m.senderid == nUID)
                .OrderByDescending(m => m.createtime)
                .Join(context.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
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

                    string strNames = string.Empty;
                    if (rids != null && rids.Length > 0)
                    {
                        for (int j = 0; j < rids.Length; j++)
                        {
                            string strName = getRealNameWithUID(long.Parse(rids[j]));
                            if (strName.Length > 0)
                            {
                                if (j == 0)
                                    strNames = strName;
                                else
                                    strNames = strNames + ", " + strName;
                            }
                        }
                    }

                    var statuslist = context.tbl_taskstatus
                        .Where(m => m.deleted == 0 && rids.Contains(m.receiverid.ToString()) && m.taskid == item.uid)
                        .ToList();

                    var acceptcnt = statuslist.Where(m => m.status == (int)(TaskStatus.EXECUTING)).Count();
                    var finishcnt = statuslist.Where(m => m.status == (int)(TaskStatus.FINISH)).Count();

                    if (acceptcnt > 0)
                    {
                        item.statusstr = "执行中";
                    }
                    else if (finishcnt == rids.Count())
                    {
                        item.statusstr = "已完成";
                    }
                    else
                    {
                        item.statusstr = "待接收";
                    }

                    item.receivernames = strNames;

                    rst.Add(item);
                }

                retData.Data = rst.Skip(5 * nPageNo).Take(5).ToArray();
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData = null;
            }

            return retData;
        }
        
        public class TaskLog
        {
            public string postdate = "";
            public string name = "";
            public string content = "";
        }
        public class TaskDetInfo
        {
            public long uid = 0;
            public long taskstatusid = 0;
            public string title = "";
            public string startdate = "";
            public string enddate = "";
            public int state = 0;
            public string content = "";
            public string filename = "";
            public string filepath = "";
            public List<TaskLog> tasklog = new List<TaskLog>();
        }
        public ServiceResponseData GetTaskDetInfo(string uid, string taskid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            TaskDetInfo detInfo = new TaskDetInfo();

            long nUID = 0, nTaskID = 0;
            try
            {
                nUID = long.Parse(uid);
                nTaskID = long.Parse(taskid);
            }
            catch { }

            try
            {
                tbl_task taskInfo = (from m in context.tbl_tasks
                                         where m.deleted == 0 && m.uid == nTaskID
                                         select m).FirstOrDefault();
                if (taskInfo == null)
                {
                    retData.RetVal = ServiceError.ERR_NOEXIST_DATA;
                    retData.Data = null;

                    return retData;
                }
                else
                {
                    detInfo.uid = taskInfo.uid;
                    detInfo.title = taskInfo.title;
                    detInfo.startdate = string.Format("{0:yyyy-MM-dd}", taskInfo.starttime);
                    detInfo.enddate = string.Format("{0:yyyy-MM-dd}", taskInfo.endtime);
                    detInfo.state = 0;
                    detInfo.content = taskInfo.contents;
                    detInfo.filename = taskInfo.attachname;
                    detInfo.filepath = taskInfo.attachpath;

                    tbl_taskstatus taskStatus = (from m in context.tbl_taskstatus
                                                 where m.deleted == 0 && m.taskid == nTaskID
                                                 select m).FirstOrDefault();
                    if (taskStatus != null)
                    {
                        detInfo.state = taskStatus.status;
                        detInfo.taskstatusid = taskStatus.uid;
                    }

                    List<tbl_taskmessage> arrList = (from m in context.tbl_taskmessages
                                                     where m.deleted == 0 && m.taskid == nTaskID
                                                     select m).ToList();
                    if (arrList != null)
                    {
                        for (int i = 0; i < arrList.Count; i++)
                        {
                            TaskLog taskLog = new TaskLog();
                            taskLog.postdate = string.Format("{0:yyyy-MM-dd}", arrList[i].createtime);
                            taskLog.content = arrList[i].contents;
                            taskLog.name = getRealNameWithUID(arrList[i].userid);

                            detInfo.tasklog.Add(taskLog);
                        }
                    }

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = detInfo;
                }
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData SetTaskRunned(string uid, string taskid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nTaskID = 0;
            try
            {
                nUID = long.Parse(uid);
                nTaskID = long.Parse(taskid);
            }
            catch { }

            try
            {
                tbl_taskstatus taskStatus = (from m in context.tbl_taskstatus
                                             where m.deleted == 0 && m.taskid == nTaskID && m.receiverid == nUID
                                             select m).FirstOrDefault();

                if (taskStatus == null)
                {
                    tbl_taskstatus newItem = new tbl_taskstatus();
                    newItem.taskid = nTaskID;
                    newItem.receiverid = nUID;
                    newItem.status = 1;
                    newItem.accepttime = DateTime.Now;
                    newItem.createtime = DateTime.Now;

                    context.tbl_taskstatus.InsertOnSubmit(newItem);
                    context.SubmitChanges();

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                }
                else
                    retData.RetVal = ServiceError.ERR_NOEXIST_DATA;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData SetTaskCompleted(string uid, string taskid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nTaskID = 0;
            try
            {
                nUID = long.Parse(uid);
                nTaskID = long.Parse(taskid);
            }
            catch { }

            try
            {
                tbl_taskstatus taskStatus = (from m in context.tbl_taskstatus
                                             where m.deleted == 0 && m.taskid == nTaskID && m.status == 1
                                             select m).FirstOrDefault();

                if (taskStatus != null)
                {
                    if (taskStatus.status != 2)
                    {
                        taskStatus.taskid = nTaskID;
                        taskStatus.receiverid = nUID;
                        taskStatus.status = 2;
                        taskStatus.finishtime = DateTime.Now;

                        context.SubmitChanges();

                        retData.RetVal = ServiceError.ERR_SUCCESS;
                    }
                    else
                    {
                        retData.RetVal = ServiceError.ERR_EXIST_DATA;
                    }
                }
                else
                {
                    retData.RetVal = ServiceError.ERR_NOEXIST_DATA;                    
                }
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public ServiceResponseData SetTaskLog(string uid, string taskid, string contents)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0, nTaskID = 0;
            try
            {
                nUID = long.Parse(uid);
                nTaskID = long.Parse(taskid);
            }
            catch { }

            try
            {
                tbl_taskmessage newItem = new tbl_taskmessage();
                newItem.taskid = nTaskID;
                newItem.userid = nUID;
                newItem.contents = contents;
                newItem.createtime = DateTime.Now;

                context.tbl_taskmessages.InsertOnSubmit(newItem);
                context.SubmitChanges();

                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class ContactInfo
        {
            public string name = "";
            public string partname = "";
            public string contactkind = "";
            public string groupname = "";
            public string rolename = "";
            public string rolekind = "";
            public string trainno = "";
            public string phonenum1 = "";
            public string phonenum2 = "";
            public string sortnum1 = "";
            public string sortnum2 = "";
            public string linenum = "";
        }
        public ServiceResponseData GetContactList(string contactkind, string rolename)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<ContactInfo> arrData;
                if (rolename == null || rolename.Length <= 0)
                {
                    arrData = (from m in context.tbl_contacts
                        where m.deleted == 0 && m.contactkind.Equals(contactkind)
                        select new ContactInfo
                        {
                            name = m.contactname,
                            partname = m.partname,
                            contactkind = m.contactkind,
                            groupname = m.groupname,
                            rolename = m.rolename,
                            rolekind = m.rolekind,
                            trainno = m.trainno,
                            phonenum1 = m.phonenum1,
                            phonenum2 = m.phonenum2,
                            sortnum1 = m.shortpnum1,
                            sortnum2 = m.shortpnum2,
                            linenum = m.linenum
                        }).ToList();
                }
                else
                {
                    arrData = (from m in context.tbl_contacts
                        where m.deleted == 0 && m.contactkind.Equals(contactkind) && m.rolename.Equals(rolename)
                        select new ContactInfo
                        {
                            name = m.contactname,
                            partname = m.partname,
                            contactkind = m.contactkind,
                            groupname = m.groupname,
                            rolename = m.rolename,
                            rolekind = m.rolekind,
                            trainno = m.trainno,
                            phonenum1 = m.phonenum1,
                            phonenum2 = m.phonenum2,
                            sortnum1 = m.shortpnum1,
                            sortnum2 = m.shortpnum2,
                            linenum = m.linenum
                        }).ToList();
                }                


                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;

            }
            catch (Exception ex)
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = ex.ToString();
            }

            return retData;
        }

        public class EmailInfo
        {
            public long uid = 0;
            public string title = "";
            public int isread = 0;
            public int isreceipt = 0;
            public string postdate = "";
            public string content = "";
            public string filename = "";
            public string filepath = "";
            public string sender = string.Empty;
            public long senderid = 0;
            public string receiver = string.Empty;            
        }

        private int IsReceiptEmail(long emailid, long userid)
        {
            int nIsReceipt = 0;
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            tbl_maillog logInfo = (from m in context.tbl_maillogs
                          where m.deleted == 0 && m.mailid == emailid && m.receiverid == userid
                          select m).FirstOrDefault();
            if (logInfo == null)
                nIsReceipt = 0;
            else
            {
                nIsReceipt = (int)logInfo.receipt;
            }

            return nIsReceipt;
        }

        public ServiceResponseData GetEmailList(string uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();
            List<EmailInfo> rst = new List<EmailInfo>();

            long nUID = 0;
            try
            {
                nUID = long.Parse(uid);
            }
            catch { }

            try
            {
                var list = context.tbl_mails.Where(m => m.deleted == 0)
                .Join(context.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new EmailInfo
                {
                    uid = m.mail.uid,
                    title = m.mail.title,
                    isread = m.mail.isread,
                    postdate = string.Format("{0:MM月dd日}", m.mail.sendtime),
                    content = m.mail.contents,
                    filename = m.mail.attachname,
                    filepath = m.mail.attachpath,
                    sender = getRealNameWithUID(m.mail.senderid),
                    receiver = m.mail.receiver
                })
                .ToList();

                foreach (var item in list)
                {
                    string[] rids = item.receiver.Split(',');

                    if (rids.Contains(uid))
                    {
                        rst.Add(item);
                    }
                }

                var ids = rst.Select(m => m.uid).ToList();

                var mailloglist = context.tbl_maillogs.Where(m => m.deleted == 0 && ids.Contains(m.mailid)).ToList();

                foreach (var item in rst)
                {
                    var existlog = mailloglist.Where(m => m.mailid == item.uid && m.receiverid == nUID).FirstOrDefault();

                    if (existlog != null)
                    {
                        item.isread = 1;
                    }
                }

                retData.Data = rst;
                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetSentEmailList(string uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();
            List<EmailInfo> rst = new List<EmailInfo>();

            long nUID = 0;
            try
            {
                nUID = long.Parse(uid);
            }
            catch { }

            try
            {
                rst = (from m in context.tbl_mails
                       where m.deleted == 0 && m.senderid == nUID
                       orderby m.sendtime descending
                       select new EmailInfo
                       {
                           uid = m.uid,
                           title = m.title,
                           isread = m.isread,
                           postdate = string.Format("{0:MM月dd日}", m.sendtime),
                           content = m.contents,
                           filename = m.attachname,
                           filepath = m.attachpath,
                           sender = getRealNameWithUID(m.senderid),
                           receiver = m.receiver
                       }).ToList();

                retData.Data = rst;
                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetEmailDetInfo(string uid, string emailid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();
            EmailInfo emailInfo = new EmailInfo();

            long nEmailID = 0;
            long nUserId = 0;
            try
            {
                nEmailID = long.Parse(emailid);
                nUserId = long.Parse(uid);
            }
            catch { }

            try
            {
                emailInfo = (from m in context.tbl_mails
                             where m.deleted == 0 && m.uid == nEmailID
                             join n in context.tbl_users on m.senderid equals n.uid
                             select new EmailInfo
                             {
                                 uid = m.uid,
                                 title =m.title,
                                 isread = m.isread,
                                 isreceipt = IsReceiptEmail(nEmailID, nUserId),
                                 postdate = string.Format("{0:yyyy年MM月dd日}", m.sendtime),
                                 content = m.contents,
                                 filename = m.attachname,
                                 filepath = m.attachpath,
                                 receiver = m.receiver,
                                 sender = n.username,
                                 senderid = m.senderid
                             }).FirstOrDefault();

                retData.Data = emailInfo;
                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData SetEmail(string uid, string receiver, string title, string content, string filename, string filedata)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;
            try
            {
                nUID = long.Parse(uid);
            }
            catch { }

            long nLength = 0;
            string savepath = "Content/uploads/attachment/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string targetbase = ConfigurationManager.AppSettings["DataSavePath"] + savepath;

            try
            {
                if (filename.Length != 0)
                {
                    byte[] bytes = Convert.FromBase64String(filedata);
                    nLength = bytes.Length;
                    if (!Directory.Exists(targetbase))
                    {
                        Directory.CreateDirectory(targetbase);
                    }

                    File.WriteAllBytes(targetbase + filename, bytes);
                }
            }
            catch (Exception ex)
            {
                retData.RetVal = ServiceError.ERR_PARAM_INVALID;
                retData.Data = ex.Message;

                return retData;
            }

            try
            {
                tbl_mail newItem = new tbl_mail();
                newItem.mailtype = 0;
                newItem.senderid = nUID;
                newItem.receiver = receiver;
                newItem.title = title;
                newItem.contents = content;
                newItem.attachname = filename;
                if (filename.Length == 0)
                    newItem.attachpath = string.Empty;
                else
                    newItem.attachpath = savepath + filename;
                newItem.attachsize = nLength;
                newItem.isread = 0;
                newItem.sendtime = DateTime.Now;
                newItem.createtime = DateTime.Now;
                newItem.deleted = 0;

                context.tbl_mails.InsertOnSubmit(newItem);
                context.SubmitChanges();

                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData SetReply(string uid, string receiver, string title, string content, string filename, string filedata, string yid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = 0;
            long nYID = 0;
            try
            {
                nUID = long.Parse(uid);
                nYID = long.Parse(yid);
            }
            catch { }

            if (nYID < 0)
            {
                long nLength = 0;
                string savepath = "Content/uploads/attachment/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
                string targetbase = ConfigurationManager.AppSettings["DataSavePath"] + savepath;

                try
                {
                    if (filename.Length != 0)
                    {
                        byte[] bytes = Convert.FromBase64String(filedata);
                        nLength = bytes.Length;
                        if (!Directory.Exists(targetbase))
                        {
                            Directory.CreateDirectory(targetbase);
                        }

                        File.WriteAllBytes(targetbase + filename, bytes);
                    }
                }
                catch (Exception ex)
                {
                    retData.RetVal = ServiceError.ERR_PARAM_INVALID;
                    retData.Data = ex.Message;

                    return retData;
                }

                try
                {
                    tbl_mail newItem = new tbl_mail();
                    newItem.mailtype = 0;
                    newItem.senderid = nUID;
                    newItem.receiver = receiver;
                    newItem.title = title;
                    newItem.contents = content;
                    newItem.attachname = filename;
                    if (filename.Length == 0)
                        newItem.attachpath = string.Empty;
                    else
                        newItem.attachpath = savepath + filename;
                    newItem.attachsize = nLength;
                    newItem.isread = 0;
                    newItem.sendtime = DateTime.Now;
                    newItem.createtime = DateTime.Now;
                    newItem.deleted = 0;

                    context.tbl_mails.InsertOnSubmit(newItem);
                    context.SubmitChanges();

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                }
                catch
                {
                    retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                    retData.Data = null;
                }

                return retData;
            }
            else
            {
                tbl_mail oldMail = (from m in context.tbl_mails
                                        where m.deleted == 0 && m.uid == nYID
                                        select m).FirstOrDefault();

                if (oldMail == null)
                {
                    retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                    retData.Data = null;
                }
                else
                {
                    tbl_mail newItem = new tbl_mail();
                    newItem.mailtype = 0;
                    newItem.senderid = nUID;
                    newItem.receiver = receiver;
                    newItem.title = title;
                    newItem.contents = content;
                    newItem.attachname = oldMail.attachname;
                    newItem.attachpath = oldMail.attachpath;
                    newItem.attachsize = oldMail.attachsize;
                    newItem.isread = 0;
                    newItem.sendtime = DateTime.Now;
                    newItem.createtime = DateTime.Now;
                    newItem.deleted = 0;

                    context.tbl_mails.InsertOnSubmit(newItem);
                    context.SubmitChanges();

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                }

                return retData;
            }
        }

        public ServiceResponseData SendReceipt(string uid, string mailid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nUID = long.Parse(uid);
            long nMailID = long.Parse(mailid);

            try
            {
                tbl_maillog logInfo = (from m in context.tbl_maillogs
                                       where m.deleted == 0 && m.mailid == nMailID && m.receiverid == nUID
                                       select m).FirstOrDefault();

                if (logInfo == null)
                {
                    tbl_maillog mailLog = new tbl_maillog();
                    mailLog.mailid = nMailID;
                    mailLog.receiverid = nUID;
                    mailLog.receipt = 1;
                    mailLog.readtime = DateTime.Now;
                    mailLog.deleted = 0;

                    context.tbl_maillogs.InsertOnSubmit(mailLog);
                    context.SubmitChanges();

                    tbl_mail mailInfo = (from m in context.tbl_mails
                                         where m.deleted == 0 && m.uid == nMailID && m.receiver.Contains(uid) == true
                                         select m).FirstOrDefault();
                    if (mailInfo != null)
                    {
                        mailInfo.isread = 1;
                        context.SubmitChanges();
                    }
                }
                else
                {
                    logInfo.receipt = 1;
                    logInfo.receipttime = DateTime.Now;
                    context.SubmitChanges();

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                }                
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
        }

        public class EmailUser
        {
            public long uid { get; set; }
            public string realname { get; set; }
        }
        public ServiceResponseData GetAllUserList(string keyword, string pageno)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            int nPageNo = 0;
            try { nPageNo = int.Parse(pageno); }
            catch { }

            List<EmailUser> rst = new List<EmailUser>();

            rst = context.tbl_users
                .Where(m => m.deleted == 0 && m.realname.Contains(keyword) == true)
                .Select(m => new EmailUser
                {
                    uid = m.uid,
                    realname = m.realname
                })
                .ToList();

            retData.Data = rst.Skip(10 * nPageNo).Take(10).ToList();
            retData.RetVal = ServiceError.ERR_SUCCESS;

            return retData;
        }

        public ServiceResponseData SetReadedState(string uid, string emailid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nEmaidID = 0;
            long nUID = 0;
            try { nEmaidID = long.Parse(emailid); nUID = long.Parse(uid); }
            catch { }

            try
            {
                tbl_maillog mailLog = (from m in context.tbl_maillogs
                                       where m.deleted == 0 && m.mailid == nEmaidID && m.receiverid == nUID
                                       select m).FirstOrDefault();

                if (mailLog == null)
                {
                    mailLog = new tbl_maillog();
                    mailLog.mailid = nEmaidID;
                    mailLog.receiverid = nUID;
                    mailLog.receipt = 0;
                    mailLog.readtime = DateTime.Now;
                    mailLog.deleted = 0;

                    context.tbl_maillogs.InsertOnSubmit(mailLog);
                    context.SubmitChanges();
                }

                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public tbl_traingroup GetGroupById(long uid)
        {
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            return context.tbl_traingroups
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }
        public tbl_user GetUserById(long uid)
        {
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            return context.tbl_users
                .Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();
        }
        public ServiceResponseData SetOpinion(string uid, string kind, string title, string content)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nKind = 0;
            long nUID = 0;
            try { nKind = long.Parse(kind); nUID = long.Parse(uid); }
            catch { }

            try
            {
                tbl_opinion newitem = new tbl_opinion();

                if (nKind == 0)  //段级
                {
                    newitem.receivepartid = 0;
                }
                else if (nKind == 1) //车队
                {
                    var senderinfo = GetUserById(nUID);
                    if (senderinfo == null)
                    {
                        retData.RetVal = ServiceError.ERR_NOEXIST_USER;

                        return retData;
                    }

                    if (senderinfo.userkind == (int)UserKind.EXECUTIVE && senderinfo.exectype == ExecType.SectorExec)
                    {
                        retData.RetVal = ServiceError.ERR_PERMISSION;
                        retData.Data = "科室干部无法提交车队诉求";

                        return retData;
                    }
                    else if (senderinfo.userkind == (int)UserKind.EXECUTIVE && senderinfo.exectype == ExecType.TeamExec)
                    {
                        newitem.receivepartid = senderinfo.execparentid;
                    }
                    else
                    {                        
                        var groupinfo = GetGroupById(senderinfo.crewgroupid);
                        newitem.receivepartid = groupinfo.teamid;
                    }
                }

                newitem.title = title;
                newitem.senderid = nUID;
                newitem.contents = content;
                newitem.createtime = DateTime.Now;

                context.tbl_opinions.InsertOnSubmit(newitem);
                context.SubmitChanges();

                retData.RetVal = ServiceError.ERR_SUCCESS;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = "参数错误";
            }

            return retData;
        }

        public class OpinionData
        {
            public long uid;
            public string title;
            public string postdate;
            public string content;
            public string feedback; 
        }

        public ServiceResponseData GetOpinionList(long uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<OpinionData> arrData = (from m in context.tbl_opinions
                                             where m.deleted == 0 && m.senderid == uid
                                             orderby m.createtime descending
                                             select new OpinionData
                                             {
                                                 uid = m.uid,
                                                 title = m.title,
                                                 postdate = string.Format("{0:yyyy-MM-dd}", m.createtime),
                                                 content = m.contents,
                                                 feedback = (m.feedback == null)?"":m.feedback
                                             }).ToList();
                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class RuleData
        {
            public long uid;
            public string path;
            public string pdfpath;
        }
        public ServiceResponseData GetRuleList()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<RuleData> arrData = (from m in context.tbl_rules
                                          where m.deleted == 0
                                          select new RuleData
                                          {
                                              uid = m.uid,
                                              path = m.imgurl,
                                              pdfpath = m.pdfpath
                                          }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class RuleDetInfo
        {
            public long uid;
            public string title;
            public string content;
        }
        public ServiceResponseData GetDetRule(string ruleid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            long nRuleID = 0;
            try { nRuleID = long.Parse(ruleid); }catch { }

            try
            {
                RuleDetInfo arrData = (from m in context.tbl_rules
                                          where m.deleted == 0 && m.uid == nRuleID
                                             select new RuleDetInfo
                                          {
                                              uid = m.uid,
                                              title = m.title,
                                              content = m.contents
                                          }).FirstOrDefault();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;

            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public ServiceResponseData GetRoutes()
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                List<STRoute> arrData = (from m in context.tbl_railroutes
                                         where m.deleted == 0 && m.zoneid == 0
                                         orderby m.sortid ascending
                                         select new STRoute
                                         {
                                             Uid = m.uid,
                                             Name = m.routename
                                         }).ToList();

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class ChecklogModel
        {
            public long uid { get; set; }
            public string content { get; set; }
            public long crewid { get; set; }
            public long crewrelid { get; set; }
            public long infoid { get; set; }
            public int checkpoint { get; set; }
            public double relatedpoint { get; set; }
            public DateTime checktime { get; set; }
            public string checklevel { get; set; }
            public string relatedpointstring { get; set; }

        }
        public class UserDetailInfo
        {
            public long uid { get; set; }
            public int userkind { get; set; }
            public string usertype { get; set; }
            public string userkindstr { get; set; }
            public string username { get; set; }
            public string realname { get; set; }
            public DateTime? birthday { get; set; }
            public string birthdaystr { get; set; }
            public byte gender { get; set; }
            public string phonenum { get; set; }
            public string execrole { get; set; }
            public string exectype { get; set; }
            public long parentid { get; set; }
            public long teamid { get; set; }
            public string parentname { get; set; }
            public long groupid { get; set; }
            public string policyface { get; set; }
            public int opinionmanage { get; set; }
            public int teammanage { get; set; }
            public long execparentid { get; set; }

            //Crew Info
            public long crewroleid { get; set; }
            public string crewrole { get; set; }
            public long crewgroupid { get; set; }
            public string crewgroupname { get; set; }
            public string crewno { get; set; }
            public string imgurl { get; set; }
            public string teamname { get; set; }
            public double addcredit { get; set; }
            public double curentCredit { get; set; }
            public double curentmonthCredit { get; set; }



            //积分封装
            public List<ChecklogModel> abovezone { get; set; }
            public List<ChecklogModel> zone { get; set; }
            public List<ChecklogModel> team { get; set; }
            public List<ChecklogModel> groupa { get; set; }


            public int typeone { get; set; }
            public double typetwo { get; set; }
            public double typethree { get; set; }
            public int typefour { get; set; }
            public int typefive { get; set; }
//             public string parentname { get; set; }
//             public long crewroleid { get; set; }
//             public string crewrole { get; set; }
//             public long crewgroupid { get; set; }
//             public string crewgroupname { get; set; }
//             public string crewno { get; set; }
//             public string imgurl { get; set; }
//             public string teamname { get; set; }
//             public double addcredit { get; set; }
//             public double curentCredit { get; set; }
//             public List<ChecklogModel> abovezone { get; set; }
//             public List<ChecklogModel> zone { get; set; }
//             public List<ChecklogModel> team { get; set; }
//             public List<ChecklogModel> groupa { get; set; }
//             public int typeone { get; set; }
//             public double typetwo { get; set; }
//             public double typethree { get; set; }
//             public int typefour { get; set; }
//             public int typefive { get; set; }
        }
        public List<UserDetailInfo> GetCredInfoByCondition(long secoterid, long groupid, int year, int month, string name)
        {
            List<UserDetailInfo> rst = null;
            TLDatabaseDataContext db = new TLDatabaseDataContext();
            //  List<UserDetailInfo> groupuser = null;

            var alluser = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == 2)
                .Join(db.tbl_crewroles, m2 => m2.crewrole, l => l.uid, (m2, l) => new { user = m2, role = l })
                .Join(db.tbl_traingroups, m3 => m3.user.crewgroupid, l => l.uid, (m3, l) => new { user = m3, group = l })
                .Join(db.tbl_railteams, m4 => m4.group.teamid, l => l.uid, (m4, l) => new { user = m4, team = l })
                .Where(m => m.team.uid == secoterid)
                .OrderBy(m => m.user.user.user.createtime)
                .Select(m => new UserDetailInfo
                {
                    uid = m.user.user.user.uid,
                    //姓名
                    realname = m.user.user.user.realname,
                    //职位名
                    crewrole = m.user.user.role.rolename,
                    //工资号
                    crewno = m.user.user.user.crewno,
                    //班组名
                    crewgroupname = m.user.group.groupname,
                    //车队名
                    teamname = m.team.teamname,
                    crewroleid = m.user.user.user.crewrole,
                    groupid = m.user.user.user.crewgroupid,

                })
                .ToList();

            if (groupid != 0)
            {
                alluser = alluser.Where(m => m.groupid == groupid).ToList();
            }
            if (name != "" && !string.IsNullOrWhiteSpace(name))
            {
                rst = alluser.Where(m => m.realname.Contains(name)).ToList();
            }

            else
            {
                rst = alluser;
            }
            /*
             * 在List<UserDetailInfo> rst 中添加 对应的 List<ChecklogModel> rst2
             * 1.遍历rst,对于每个UserDetailInfo，使用UserDetailInfo.uid 作为条件查询tbl_checklogs表;
             * 2.内敛查询 checkeinfo表（获得checkeinfo.chkpoint列）;
             * 3.每次遍历获得的为List<ChecklogModel> rst2，此rst2.crewid=rst.uid;
             * 4.每次遍历添加rst2（因为每次查询条件不同，每个rst2集合也不同.
             */

            var cloglist = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("两违")).ToList();
            var cinfolist = db.tbl_checkinfos.Where(m => m.checktype == 1).ToList();
            var addcreditlist = db.tbl_checkinfos.Where(m => m.checktype == 0).ToList();

            foreach (var r in rst)
            {
                //计算激励积分的和
                var addcredit = cloglist.Where(m => m.crewid == r.uid && m.checktime.Year == year && m.checktime.Month == month)
                                           .Join(addcreditlist, m => m.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                                           .Select(m => m.info.chkpoint).ToList();
                r.addcredit = addcredit.Sum();


                //用户checklog表中相关
                var rst2 = cloglist.Where(m => m.crewid == r.uid && m.checktime.Year == year && m.checktime.Month == month)
                          .Join(cinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                          .Select(m => new ChecklogModel
                          {
                              checkpoint = m.checkeinfo.chkpoint,
                              checklevel = m.checklogs.checklevel,
                          }
                          ).ToList();


                /*
                 * typeone: 段级&段以上 批评教育
                 * typetwo: 班组&车队   批评教育  
                 * typethree:           联挂考核  
                 * typefour:            直接离岗培训  
                 * typefive:            调离岗位  
                 */
                double typethreeA = 0;
                double typethreeB = 0;
                double typethreeC = 0;

                if (rst2.Count != 0)
                {

                    // typeone: 段级&段以上 批评教育
                    r.typeone = rst2.Where(m => m.checkpoint <= 9)
                                 .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                                 .Select(m => m.checkpoint).ToList().Sum();



                    //车队  批评教育  50%
                    var typetwoA = rst2.Where(m => m.checkpoint <= 9)
                                    .Where(m => m.checklevel == "车队")
                                    .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();



                    //班组  批评教育  25%
                    var typetwoB = rst2.Where(m => m.checkpoint <= 9)
                                    .Where(m => m.checklevel == "班组")
                                    .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

                    //车队  联挂考核  50%
                    typethreeA = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                   .Where(m => m.checklevel == "车队")
                                   .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

                    //班组  联挂考核  25%
                    typethreeB = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                   .Where(m => m.checklevel == "班组")
                                   .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

                    //段级  联挂考核  100%
                    typethreeC = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                   .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                                   .Select(m => m.checkpoint).ToList().Sum();


                    r.typetwo = typetwoA + typetwoB;
                }


                //检查联挂积分，
                //先判断是不是列车长，列车长的积分由两部分组成
                if (r.crewroleid == 2)
                {
                    //自己给自己扣的分
                    var a = typethreeA + typethreeB + typethreeC;

                    //从checklog表中选择其他人给该列车长扣分的数据  
                    var rst3 = cloglist
                             .Where(m => m.checktime.Year == year && m.checktime.Month == month && m.crewrelid == r.uid)
                             .Join(cinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                             .Where(m => m.checkeinfo.relpoint != null)
                             .Where(m => double.Parse(m.checkeinfo.relpoint) != 0)
                             .Select(m => new ChecklogModel
                             {
                                 checklevel = m.checklogs.checklevel,
                                 relatedpoint = Math.Round(double.Parse(m.checkeinfo.relpoint), 1, MidpointRounding.AwayFromZero),
                                 checkpoint = m.checkeinfo.chkpoint
                             }
                               ).ToList();


                    if (rst3.Count != 0)
                    {
                        //段&段以上扣的 联挂分按原分算
                        var b = rst3.Where(m => m.checklevel == "段级" || m.checklevel == "段以上").Select(m => m.relatedpoint).ToList().Sum();
                        //车队扣的      1-9 批评教育，10-20连挂考核  联挂分按50%算
                        var c = rst3.Where(m => m.checklevel == "车队" && m.checkpoint > 0 && m.checkpoint <= 20).Select(m => Math.Round((m.relatedpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                        //车队扣的      50\60 离岗问题  联挂分按100%算
                        var d = rst3.Where(m => m.checklevel == "车队")
                            .Where(m => m.checkpoint == 50 || m.checkpoint == 60)
                            .Select(m => Math.Round((m.relatedpoint), 1, MidpointRounding.AwayFromZero)).ToList().Sum();


                        //班级扣的      组内自查联挂分为0
                        r.typethree = a + b + c + d;
                    }

                    else
                    {
                        r.typethree = a;
                    }

                }

                else
                {

                    r.typethree = typethreeA + typethreeB + typethreeC;

                }

                r.typefour = rst2.Where(m => m.checkpoint == 50).ToList()
                             .Select(m => m.checkpoint).ToList().Sum();
                r.typefive = rst2.Where(m => m.checkpoint == 60).ToList()
                             .Select(m => m.checkpoint).ToList().Sum();
                //               r.curentmonthCredit = r.typetwo + r.typethree + r.typefour + r.typefive;
                //               r.curentCredit = conttotalCredit(r.uid, month,year);

            }
            return rst;

//             List<UserDetailInfo> rst = null;
//             TLDatabaseDataContext db = new TLDatabaseDataContext();
// 
//             var alluser = db.tbl_users
//              .Where(m => m.deleted == 0 && m.userkind == 2)
//              .Join(db.tbl_crewroles, m2 => m2.crewrole, l => l.uid, (m2, l) => new { user = m2, role = l })
//              .Join(db.tbl_traingroups, m3 => m3.user.crewgroupid, l => l.uid, (m3, l) => new { user = m3, group = l })
//              .Join(db.tbl_railteams, m4 => m4.group.teamid, l => l.uid, (m4, l) => new { user = m4, team = l })
//              .Where(m => m.team.uid == secoterid && m.team.routeids == routeid)
//              .OrderBy(m => m.user.user.user.createtime)
//              .Select(m => new UserDetailInfo
//              {
//                  uid = m.user.user.user.uid,
//                  realname = m.user.user.user.realname,
//                  crewrole = m.user.user.role.rolename,
//                  crewno = m.user.user.user.crewno,
//                  crewgroupname = m.user.group.groupname,
//                  teamname = m.team.teamname,
//                  crewroleid = m.user.user.user.crewrole,
//              })
//              .ToList();
// 
//             if (name != "" && !string.IsNullOrWhiteSpace(name))
//             {
//                 rst = alluser.Where(m => m.realname.Contains(name)).ToList();
//             }
// 
//             else
//             {
//                 rst = alluser;
//             }
// 
//             var cloglist = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("两违")).ToList();
//             var cinfolist = db.tbl_checkinfos.Where(m => m.checktype == 1).ToList();
//             var addcreditlist = db.tbl_checkinfos.Where(m => m.checktype == 2).ToList();
// 
//             foreach (var r in rst)
//             {
//                 var addcredit = cloglist.Where(m => m.crewid == r.uid && m.createtime.Year == year && m.createtime.Month == month)
//                                            .Join(addcreditlist, m => m.infoid, l => l.uid, (m, l) => new { log = m, info = l })
//                                            .Select(m => m.info.chkpoint).ToList();
//                 r.addcredit = addcredit.Sum();
// 
//                 var rst2 = cloglist.Where(m => m.crewid == r.uid && m.createtime.Year == year && m.createtime.Month == month)
//                           .Join(cinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
//                           .Select(m => new ChecklogModel
//                           {
//                               checkpoint = m.checkeinfo.chkpoint,
//                               checklevel = m.checklogs.checklevel,
//                           }
//                           ).ToList();
// 
//                 double typethreeA = 0;
//                 double typethreeB = 0;
//                 double typethreeC = 0;
// 
//                 if (rst2.Count != 0)
//                 {
//                     r.typeone = rst2.Where(m => m.checkpoint <= 9)
//                                  .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
//                                  .Select(m => m.checkpoint).ToList().Sum();
// 
//                     var typetwoA = rst2.Where(m => m.checkpoint <= 9)
//                                     .Where(m => m.checklevel == "车队")
//                                     .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
//                     
//                     var typetwoB = rst2.Where(m => m.checkpoint <= 9)
//                                     .Where(m => m.checklevel == "班组")
//                                     .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
// 
//                     typethreeA = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
//                                    .Where(m => m.checklevel == "车队")
//                                    .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
// 
//                     typethreeB = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
//                                    .Where(m => m.checklevel == "班组")
//                                    .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
// 
//                     typethreeC = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
//                                    .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
//                                    .Select(m => m.checkpoint).ToList().Sum();
// 
//                     r.typetwo = typetwoA + typetwoB;
//                 }
// 
//                 if (r.crewroleid == 2)
//                 {
//                     var a = typethreeA + typethreeB + typethreeC;
// 
//                     var rst3 = cloglist
//                              .Where(m => m.createtime.Year == year && m.createtime.Month == month && m.crewrelid == r.uid)
//                              .Join(cinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
//                              .Where(m => m.checkeinfo.relpoint != null)
//                              .Where(m => double.Parse(m.checkeinfo.relpoint) != 0)
//                              .Select(m => new ChecklogModel
//                              {
//                                  checklevel = m.checklogs.checklevel,
//                                  relatedpoint = Math.Round(double.Parse(m.checkeinfo.relpoint), 1, MidpointRounding.AwayFromZero),
//                                  checkpoint = m.checkeinfo.chkpoint
//                              }
//                                ).ToList();
// 
// 
//                     if (rst3.Count != 0)
//                     {
//                         var b = rst3.Where(m => m.checklevel == "段级" || m.checklevel == "段以上").Select(m => m.relatedpoint).ToList().Sum();
//                         var c = rst3.Where(m => m.checklevel == "车队" && m.checkpoint > 0 && m.checkpoint <= 20).Select(m => Math.Round((m.relatedpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
//                         var d = rst3.Where(m => m.checklevel == "车队")
//                             .Where(m => m.checkpoint == 50 || m.checkpoint == 60)
//                             .Select(m => Math.Round((m.relatedpoint), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
//                         
//                         r.typethree = a + b + c + d;
//                     }
// 
//                     else
//                     {
//                         r.typethree = a;
//                     }
//                 }
//                 else
//                 {
//                     r.typethree = typethreeA + typethreeB + typethreeC;
//                 }
// 
//                 r.typefour = rst2.Where(m => m.checkpoint == 50).ToList()
//                              .Select(m => m.checkpoint).ToList().Sum();
//                 r.typefive = rst2.Where(m => m.checkpoint == 60).ToList()
//                              .Select(m => m.checkpoint).ToList().Sum();
//             }
//             return rst;
        }

        public double conttotalCredit(long uid, int month, int year)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            int typeone;
            double typetwo;
            double typethree;
            int typefour;
            int typefive;
            var checkloglist = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("两违")).ToList();
            var checkinfolist = db.tbl_checkinfos.Where(m => m.checktype == 1).ToList();
            var totaladdcreditlist = db.tbl_checkinfos.Where(m => m.checktype == 2).ToList();
            int addcredit;
            var addcreditlist = checkloglist.Where(m => m.crewid == uid && m.createtime.Year == year && m.createtime.Month == month)
                             .Join(totaladdcreditlist, m => m.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                             .Select(m => m.info.chkpoint).ToList();
            addcredit = addcreditlist.Sum();

            long crewroleid = db.tbl_users.Where(m => m.uid == uid).Select(m => m.crewrole).ToList()[0];

            var rst2 = checkloglist
                          .Where(m => m.crewid == uid && m.createtime.Year == year && m.createtime.Month == month)
                          .Join(checkinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                          .Select(m => new ChecklogModel
                          {
                              checkpoint = m.checkeinfo.chkpoint,
                              checklevel = m.checklogs.checklevel,
                          }
                          ).ToList();

            typeone = rst2.Where(m => m.checkpoint <= 9)
                        .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                        .Select(m => m.checkpoint).ToList().Sum();

            var typetwoA = rst2.Where(m => m.checkpoint <= 9)
                            .Where(m => m.checklevel == "车队")
                            .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

            var typetwoB = rst2.Where(m => m.checkpoint <= 9)
                            .Where(m => m.checklevel == "班组")
                            .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

            var typethreeA = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                            .Where(m => m.checklevel == "车队")
                            .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

            var typethreeB = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                            .Where(m => m.checklevel == "班组")
                            .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

            var typethreeC = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                .Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                                .Select(m => m.checkpoint).ToList().Sum();
            
            typetwo = typetwoA + typetwoB;

            if (crewroleid == 2)
            {
                var a = typethreeA + typethreeB + typethreeC;
                var rst3 = checkloglist
                       .Where(m => m.createtime.Year == year && m.createtime.Month == month && m.crewrelid == uid)
                       .Join(checkinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                       .Where(m => m.checkeinfo.relpoint != null)
                       .Where(m => double.Parse(m.checkeinfo.relpoint) != 0)
                       .Select(m => new ChecklogModel
                       {
                           checklevel = m.checklogs.checklevel,
                           relatedpoint = Math.Round(double.Parse(m.checkeinfo.relpoint), 1, MidpointRounding.AwayFromZero),
                           checkpoint = m.checkeinfo.chkpoint
                       }
                         ).ToList();


                if (rst3.Count != 0)
                {
                    var b = rst3.Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                              .Select(m => m.relatedpoint).ToList().Sum();
                    var c = rst3.Where(m => m.checklevel == "车队" && m.checkpoint > 0 && m.checkpoint <= 20)
                              .Select(m => Math.Round((m.relatedpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                    var d = rst3.Where(m => m.checklevel == "车队")
                        .Where(m => m.checkpoint == 50 || m.checkpoint == 60)
                        .Select(m => m.relatedpoint).ToList().Sum();
                    typethree = a + b + c + d;
                }
                else
                {
                    typethree = a;
                }
            }
            else
            {
                typethree = typethreeA + typethreeB + typethreeC;
            }

            typefour = rst2.Where(m => m.checkpoint == 50).ToList()
                         .Select(m => m.checkpoint).ToList().Sum();
            typefive = rst2.Where(m => m.checkpoint == 60).ToList()
                         .Select(m => m.checkpoint).ToList().Sum();

            double CurrentCredit;
            if (month == 1 || month == 4 || month == 7 || month == 10)
            {
                if (typefour != 0 || typefive != 0)
                {
                    CurrentCredit = typetwo + typethree + typefour + typefive;
                    return CurrentCredit;
                }
                else
                {
                    CurrentCredit = typetwo + typethree + typefour + typefive - addcredit;
                    if (CurrentCredit < 0)
                    {
                        CurrentCredit = 0;
                    }
                    return CurrentCredit;
                }
            }

            else
            {
                if (typefour != 0 || typefive != 0)
                {
                    CurrentCredit = typetwo + typethree + typefour + typefive + conttotalCredit(uid, month - 1, year);
                    return CurrentCredit;
                }
                else
                {
                    CurrentCredit = typetwo + typethree + typefour + typefive - addcredit + conttotalCredit(uid, month - 1, year);
                    if (CurrentCredit < 0)
                    {
                        CurrentCredit = 0;
                    }
                    return CurrentCredit;
                }
            }
        }

        public ServiceResponseData GetCreditList(string findtime, string name, string sectorid, string routeid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<CreditInfo> arrData = new List<CreditInfo>();

            long nSectorid = 0;
            long nRouteid = 0;

            try
            {
                int year = DateTime.Parse(findtime).Year;
                int month = DateTime.Parse(findtime).Month;
                nSectorid = long.Parse(sectorid);
                nRouteid = long.Parse(routeid);

                var arrCredInfo = GetCredInfoByCondition(nSectorid, nRouteid, year, month, name);

                foreach (UserDetailInfo info in arrCredInfo)
                {
                    CreditInfo newInfo = new CreditInfo();

                    newInfo.Uid = info.uid;
                    newInfo.CheDui = info.teamname;
                    newInfo.GongZiHao = info.crewno;
                    newInfo.Name = info.realname;
                    newInfo.BanZu = info.crewgroupname;
                    newInfo.DuanJiYiShang = info.typeone;
                    newInfo.BanZuCheDui = info.typetwo;
                    newInfo.LianGua = info.typethree;
                    newInfo.LiGang = info.typefour;
                    newInfo.DiaoZheng = info.typefive;
                    newInfo.BenYue = (info.typetwo + info.typethree + info.typefour + info.typefive);
                    newInfo.JiLi = info.addcredit;
                    newInfo.LeiJi = conttotalCredit(info.uid, month, year);

                    arrData.Add(newInfo);
                }

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class forStore
        {
            public int checkpoint { set; get; }
            public string checklevel { set; get; }
            public long groupid { set; get; }
        }

        public class grouplist
        {
            public long groupid { set; get; }
            public string groupname { set; get; }
            public string teamname { set; get; }
            public int one { set; get; }
            public int two { set; get; }
            public int three { set; get; }
            public int four { set; get; }
            public int five { set; get; }
            public int six { set; get; }
            public int seven { set; get; }
            public int eight { set; get; }

        }

        //根据年、月、部门类型 查自控率
        public List<grouplist> selfcheck(int year, int month, long sectorid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            var grouplist = db.tbl_traingroups.Where(m => m.deleted == 0 && m.teamid == sectorid)
                .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                .Select(m => new grouplist
                {
                    groupid = m.group.uid,
                    groupname = m.group.groupname,
                    teamname = m.team.teamname
                })
                .ToList();

            foreach (var r in grouplist)
            {
                var rst = db.tbl_checklogs
               .Where(m => m.deleted == 0 && m.logtype.Contains("两违") && m.groupid == r.groupid && m.createtime.Year == year && m.createtime.Month == month)
               .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
               .Where(m => m.checkeinfo.checktype == 1)
               .Select(m => new forStore
               {
                   groupid = m.checklogs.groupid,
                   checkpoint = m.checkeinfo.chkpoint,
                   checklevel = m.checklogs.checklevel
               })
               .ToList();


                //1-6 班组
                var one = rst.Where(m => m.checkpoint > 0 && m.checkpoint < 7).Where(m => m.checklevel == "班组").ToList().Count();
                //1-6 班组以上
                var two = rst.Where(m => m.checkpoint > 0 && m.checkpoint < 7).Where(m => m.checklevel == "车队" || m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();

                //7-9 班组
                var three = rst.Where(m => m.checkpoint >= 7 && m.checkpoint < 10).Where(m => m.checklevel == "班组").ToList().Count();
                //7-9 班组以上
                var four = rst.Where(m => m.checkpoint >= 7 && m.checkpoint < 10).Where(m => m.checklevel == "车队" || m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();

                //10-20 班组
                var five = rst.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20).Where(m => m.checklevel == "班组").ToList().Count();
                //10-20 班组以上
                var six = rst.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20).Where(m => m.checklevel == "车队" || m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();

                //50and60 班组
                var seven = rst.Where(m => m.checkpoint == 50 || m.checkpoint == 60).Where(m => m.checklevel == "班组").ToList().Count();
                //50and60 班组以上
                var eight = rst.Where(m => m.checkpoint == 50 || m.checkpoint == 60).Where(m => m.checklevel == "车队" || m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();


                r.one = one;
                r.two = two;
                r.three = three;
                r.four = four;
                r.five = five;
                r.six = six;
                r.seven = seven;
                r.eight = eight;
            }
            return grouplist;

        }

        public ServiceResponseData GetSelfCheckList(string findtime, string sectorid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            try
            {
                long nSectorid = 0;

                int year = DateTime.Parse(findtime).Year;
                int month = DateTime.Parse(findtime).Month;
                nSectorid = long.Parse(sectorid);

                List<grouplist> arrData = new List<grouplist>();
                arrData  = selfcheck(year, month, nSectorid);

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = arrData;
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class CombineCheckViewTBList
        {
            public long groupid { set; get; }
            public string groupname { set; get; }
            public string teamname { set; get; }
            public int one { set; get; }
            public int two { set; get; }
            public int three { set; get; }
        }

        public List<CombineCheckViewTBList> CombineCheck(int year, int month, long sectorid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            var grouplist = db.tbl_traingroups.Where(m => m.deleted == 0 && m.teamid == sectorid)
                 .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                 .Select(m => new CombineCheckViewTBList
                 {
                     groupid = m.group.uid,
                     groupname = m.group.groupname,
                     teamname = m.team.teamname
                 })
                 .ToList();


            foreach (var r in grouplist)
            {
                var rst = db.tbl_checklogs
                     .Where(m => m.deleted == 0 && m.logtype.Contains("结合部"))
                      .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                       .Where(m => m.checklogs.groupid == r.groupid && m.checklogs.createtime.Year == year && m.checklogs.createtime.Month == month)
                       .Select(m => new forStore
                       {
                           groupid = m.checklogs.groupid,

                           checklevel = m.checklogs.checklevel
                       })
                .ToList();


                //班组
                var one = rst.Where(m => m.checklevel == "班组").ToList().Count();
                //车队
                var two = rst.Where(m => m.checklevel == "车队").ToList().Count();
                //段以上
                var three = rst.Where(m => m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();

                r.one = one;
                r.two = two;
                r.three = three;
            }

            return grouplist;
        }

        public class CombineCheckInfo
        {
            public string CheDui = string.Empty;
            public string BanZu = string.Empty;
            public int oneval = 0;
            public int twoval = 0;
            public int threeval = 0;
        }

        public ServiceResponseData GetCombineCheckList(string findtime, string sectorid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<CombineCheckInfo> arrData = new List<CombineCheckInfo>();

            try
            {
                long nSectorid = 0;

                int year = DateTime.Parse(findtime).Year;
                int month = DateTime.Parse(findtime).Month;
                nSectorid = long.Parse(sectorid);

                List<CombineCheckViewTBList> tempData = new List<CombineCheckViewTBList>();
                tempData = CombineCheck(year, month, nSectorid);

                if (tempData != null)
                {
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        CombineCheckInfo newInfo = new CombineCheckInfo();
                        newInfo.CheDui = tempData[i].teamname;
                        newInfo.BanZu = tempData[i].groupname;
                        newInfo.oneval = tempData[i].one;
                        newInfo.twoval = tempData[i].two;
                        newInfo.threeval = tempData[i].three;

                        arrData.Add(newInfo);
                    }

                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = arrData;
                }
                else
                {
                    retData.RetVal = ServiceError.ERR_SUCCESS;
                    retData.Data = null;
                }
            }
            catch
            {
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }

        public class NewsItem
        {
            public long uid = 0;
            public int mode = 0;   // 0 : 公文    1 : 邮箱   2 : 任务
            public string title = string.Empty;
        }
        public class NewsList
        {
            public List<NewsItem> gongwen = new List<NewsItem>();
            public List<NewsItem> email = new List<NewsItem>();
            public List<NewsItem> task = new List<NewsItem>();
        }
        public ServiceResponseData GetNewsList(string uid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            List<NewsItem> arrGongWen = new List<NewsItem>();
            List<NewsItem> arrEmail = new List<NewsItem>();
            List<NewsItem> arrTask = new List<NewsItem>();

            List<STDaiQianItem> rstGongWen = new List<STDaiQianItem>();
            List<EmailInfo> rstEmail = new List<EmailInfo>();
            List<TaskInfo> rstTask = new List<TaskInfo>();

            try
            {
                long nUID = long.Parse(uid);

                #region GongWenNewsList

                var filterlist = context.tbl_documents
                 .OrderByDescending(m => m.createtime)
                 .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                 .Where(m => m.doc.deleted == 0)
                 .Select(m => new STDaiQianItem
                 {
                     Uid = m.doc.uid,
                     Title = m.doc.title,
                     BuMen = m.doc.sendpart,
                     FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                     FaBuRen = m.sender.realname,
                     TongZhiHao = m.doc.docno,
                     FileName = m.doc.attachname,
                     FilePath = m.doc.attachpath,
                     Total = 0,
                     Content = m.doc.contents,
                     ShouXinRen = m.doc.receiver,
                     ShouXinBuMen = m.doc.receiverange
                 }).ToList();

                foreach (var item in filterlist)
                {
                    var receiverids = item.ShouXinRen.Split(',');

                    if (receiverids.Contains(nUID.ToString()))
                    {
                        rstGongWen.Add(item);
                    }
                }

                var loglist = context.tbl_documentlogs.Where(m => m.deleted == 0).ToList();

                List<long> forwdocids = new List<long>();
                foreach (var item in loglist)
                {
                    var receiverids = item.receiver.Split(',');
                    if (receiverids.Contains(nUID.ToString()))
                    {
                        forwdocids.Add(item.docid);
                    }
                }

                if (forwdocids.Count() > 0)
                {
                    var forwardlist = context.tbl_documents
                        .Where(m => m.deleted == 0 && forwdocids.Contains(m.uid))
                        .OrderByDescending(m => m.createtime)
                        .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })
                        .Select(m => new STDaiQianItem
                        {
                            Uid = m.doc.uid,
                            Title = m.doc.title,
                            BuMen = m.doc.sendpart,
                            FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                            FaBuRen = m.sender.realname,
                            TongZhiHao = m.doc.docno,
                            FileName = m.doc.attachname,
                            FilePath = m.doc.attachpath,
                            Total = 0,
                            Content = m.doc.contents,
                            ShouXinRen = m.doc.receiver,
                            ShouXinBuMen = m.doc.receiverange
                        })
                        .ToList();

                    rstGongWen.AddRange(forwardlist);
                }

                var signlist = loglist.Where(m => m.deleted == 0 && m.userid == nUID).ToList();
                var docids = signlist.Select(m => m.docid).ToList();

                rstGongWen = rstGongWen.Where(m => !docids.Contains(m.Uid)).ToList();

                if (rstGongWen != null)
                {
                    for (int i = 0; i < rstGongWen.Count; i++)
                    {
                        NewsItem newItem = new NewsItem();

                        newItem.uid = rstGongWen[i].Uid;
                        newItem.mode = 0;
                        newItem.title = rstGongWen[i].Title;

                        arrGongWen.Add(newItem);
                    }
                }

                #endregion

                #region EmailNewsList

                var list = context.tbl_mails.Where(m => m.deleted == 0)
                .Join(context.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { mail = m, sender = l })
                .OrderByDescending(m => m.mail.sendtime)
                .Select(m => new EmailInfo
                {
                    uid = m.mail.uid,
                    title = m.mail.title,
                    isread = m.mail.isread,
                    postdate = string.Format("{0:MM月dd日}", m.mail.sendtime),
                    content = m.mail.contents,
                    filename = m.mail.attachname,
                    filepath = m.mail.attachpath,
                    sender = getRealNameWithUID(m.mail.senderid),
                    receiver = m.mail.receiver
                })
                .ToList();

                foreach (var item in list)
                {
                    string[] rids = item.receiver.Split(',');

                    if (rids.Contains(uid))
                    {
                        rstEmail.Add(item);
                    }
                }

                var ids = rstEmail.Select(m => m.uid).ToList();

                var mailloglist = context.tbl_maillogs.Where(m => m.deleted == 0 && ids.Contains(m.mailid)).ToList();

                foreach (var item in rstEmail)
                {
                    var existlog = mailloglist.Where(m => m.mailid == item.uid && m.receiverid == nUID).FirstOrDefault();

                    if (existlog != null)
                    {
                        item.isread = 1;
                    }
                }

                if (rstEmail != null)
                {
                    for (int i = 0; i < rstEmail.Count; i++)
                    {
                        if (rstEmail[i].isread == 0)
                        {
                            NewsItem newItem = new NewsItem();

                            newItem.uid = rstEmail[i].uid;
                            newItem.mode = 1;
                            newItem.title = rstEmail[i].title;

                            arrEmail.Add(newItem);
                        }
                    }
                }

                #endregion

                #region TaskNewsList

                var filterTasklist = context.tbl_tasks
                    .Where(m => m.deleted == 0)
                    .OrderByDescending(m => m.createtime)
                    .Join(context.tbl_users, m => m.senderid, l => l.uid, (m, l) => new { doc = m, sender = l })
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
                        starttime_desc = string.Format("{0:yyyy-MM-dd hh:mm}", m.doc.starttime),
                        endtime = m.doc.endtime,
                        endtime_desc = string.Format("{0:yyyy-MM-dd hh:mm}", m.doc.endtime)
                    })
                    .ToList();

                var statuslist = context.tbl_taskstatus
                    .Where(m => m.deleted == 0 &&
                        m.receiverid == nUID)
                    .ToList();

                foreach (var item in filterTasklist)
                {
                    string[] receiverids = item.receiver.Split(',');

                    if (receiverids.Contains(nUID.ToString()))
                    {
                        TaskStatus i = TaskStatus.NOTACCPET;

                        var status = statuslist.Where(m => m.taskid == item.uid).FirstOrDefault();

                        if (status != null)
                        {
                            i = (TaskStatus)status.status;
                            item.accepttime = status.accepttime;
                            item.finishtime = status.finishtime;
                        }

                        if (i == TaskStatus.NOTACCPET)
                        {
                            item.status = TaskStatus.NOTACCPET;
                            item.statusstr = "待接收";
                        }
                        else if (i == TaskStatus.EXECUTING)
                        {
                            item.status = TaskStatus.EXECUTING;
                            item.statusstr = "执行中";
                        }
                        else if (i == TaskStatus.FINISH)
                        {
                            item.status = TaskStatus.FINISH;
                            item.statusstr = "已完成";
                        }
                        else
                        {
                            item.status = TaskStatus.NOTACCPET;
                            item.statusstr = "待接收";
                        }

                        rstTask.Add(item);
                    }
                }

                if (rstTask != null)
                {
                    for (int i = 0; i < rstTask.Count; i++)
                    {
                        if (rstTask[i].status == TaskStatus.NOTACCPET)
                        {
                            NewsItem newItem = new NewsItem();

                            newItem.uid = rstTask[i].uid;
                            newItem.mode = 2;
                            newItem.title = rstTask[i].title;

                            arrTask.Add(newItem);
                        }
                    }
                }

                #endregion

                NewsList newsList = new NewsList();
                newsList.gongwen = arrGongWen;
                newsList.email = arrEmail;
                newsList.task = arrTask;

                retData.RetVal = ServiceError.ERR_SUCCESS;
                retData.Data = newsList;
            }
            catch
            {
                retData.Data = null;
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
            }

            return retData;
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
            public string createtime { get; set; }
        }
        public List<DocumentLog> GetMyDocLogList(long uid)
        {
            TLDatabaseDataContext db = new TLDatabaseDataContext();

            List<DocumentLog> retData = new List<DocumentLog>();

            retData = db.tbl_documentlogs
                .Where(m => m.deleted == 0 && m.docid == uid)
                .Join(db.tbl_users, m => m.userid, l => l.uid, (m, l) => new { doc = m, user = l })
                .Select(m => new DocumentLog
                {
                    uid = m.doc.uid,
                    userid = m.doc.userid,
                    username = m.user.realname,
                    docid = m.doc.docid,
                    acttype = m.doc.acttype,
                    execnote = m.doc.execnote,
                    receiver = m.doc.receiver,
                    createtime = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime)
                })
                .ToList();

            return retData;
        }

        public ServiceResponseData GetDaiQianDetail(string uid, string docid)
        {
            ServiceResponseData retData = new ServiceResponseData();
            TLDatabaseDataContext context = new TLDatabaseDataContext();

            STDaiQianItem rst = new STDaiQianItem();

            try
            {                
                int nUID = int.Parse(uid);
                long nDocID = int.Parse(docid);

                rst = context.tbl_documents
                    .Where(m => m.deleted == 0 && m.uid == nDocID)
                    .Join(context.tbl_users, m => m.sender, l => l.uid, (m, l) => new { doc = m, sender = l })                    
                    .Select(m => new STDaiQianItem
                    {
                        Uid = m.doc.uid,
                        Title = m.doc.title,
                        BuMen = m.doc.sendpart,
                        FBDate = String.Format("{0:yyyy年MM月dd日}", m.doc.createtime),
                        FaBuRen = m.sender.realname,
                        TongZhiHao = m.doc.docno,
                        FileName = m.doc.attachname,
                        FilePath = m.doc.attachpath,
                        Total = 0,
                        Content = m.doc.contents,
                        ShouXinRen = m.doc.receiver,
                        ShouXinBuMen = m.doc.receiverange,
                        LiuZhuanRenPiShi = GetMyDocLogList(nDocID)                     
                    }).FirstOrDefault();

                string[] rids = rst.ShouXinRen.Split(',');
                if (rids.Contains(nUID.ToString()))
                {
                    var noticelog = context.tbl_sysnoticelogs.Where(m => m.readerid == nUID && m.logtype == 1 && m.checklogid == nDocID).FirstOrDefault();
                    if (noticelog != null && noticelog.readtime == null)
                    {
                        noticelog.readtime = DateTime.Now;
                        context.SubmitChanges();
                    }
                }

                retData.Data = rst;
            }
            catch (System.Exception ex)
            {
                Common.WriteLogFile("GetDaiQianList", ex.Message);
                retData.RetVal = ServiceError.ERR_INTERNAL_EXCEPTION;
                retData.Data = null;
            }

            return retData;
        }
    }
}