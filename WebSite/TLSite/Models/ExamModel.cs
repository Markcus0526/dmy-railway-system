using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Data;
using System.Web.Hosting;
using System.IO;
using Newtonsoft;
using System.Web.Script.Serialization;

namespace TLSite.Models
{
    public enum ExamKind
    {
        Sector = 0,
        Team
    }

    public class ExamType
    {
        public const string OneSel = "单选题";
        public const string MultiSel = "多选题";
        public const string YesNo = "判断题";
    }

    public class ExamInfo
    {
        public long uid { get; set; }
        public ExamKind examkind { get; set; }
        public long teamid { get; set; }
        public string title { get; set; }
        public string examtype { get; set; }
        public string teamname { get; set; }
        public List<SelQuestion> question { get; set; }
        public string questionstr { get; set; }
        public object answer { get; set; }
        public string answerstr { get; set; }
        public DateTime createtime { get; set; }
        public int score { get; set; }
    }

    public class crewList
    {
        public long uid { get; set; }
        public List<ExamResultInfo> examresult { get; set; }
        public List<tbl_exambook> books { get; set; }
        public string realname { get; set; }
        public string crewno { get; set; }
    }

    public class JsonExamInfo
    {
        public long id { get; set; }
        public string examtype { get; set; }
        public string title { get; set; }
        public List<SelQuestion> question { get; set; }
        public object answer { get; set; }
    }

    public class ExamChartInfo
    {
        public string label { get; set; }
        public decimal data { get; set; }
    }

    /// <summary>
    /// Don't change class members as your instant mind.
    /// </summary>
    public class JsonExamResult
    {
        public int correctonesel { get; set;}
        public int correctmultisel { get; set; }
        public int correctyesno { get; set; }
        public int totalonesel { get; set; }
        public int totalmultisel { get; set; }
        public int totalyesno { get; set; }
        public int score { get; set; }

        public List<JsonExamRstDetail> result { get; set; }
    }

    public class JsonExamRstDetail
    {
        public long id { get; set; }
        public string examtype { get; set; }
        public string title { get; set; }
        public List<SelQuestion> question { get; set; }
        public object answer { get; set; }
        public object myanswer { get; set; }
    }

    public class SelQuestion
    {
        public string ind { get; set; }
        public string question { get; set; }
    }

    public class OneSelAnswer
    {
        public long answer { get; set; }
    }

    public class MultiSelAnswer
    {
        public List<long> answer { get; set; }
    }

    public class YesNoAnswer
    {
        public bool answer { get; set; }
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
        public long? teamid { get; set; }
        public string teamname { get; set; }
    }

    public class ExamResultInfo
    {
        public long uid { get; set; }
        public ExamKind examkind { get; set; }
        public long bookid { get; set; }
        public string bookname { get; set; }
        public string title { get; set; }
        public int score { get; set; }   //考试时间
        public DateTime? parttime { get; set; }   //考试日期
        public int examtime { get; set; }   //考试时间
        public string papersecond { get; set; }   //答题时间
        public int totalnum { get; set; }
        public int correctnum { get; set; }
        public string totalnumstr { get; set; }
        public string correctnumstr { get; set; }
        public string result { get; set; }
        public DateTime createtime { get; set; }
        public string name { get; set; }
        public string crewno { get; set; }
        public int userkind { get; set; }
        public string exectype { get; set; }
        public long execparentid { get; set; }
        public long groupid { get; set; }
        public long teamid { get; set; }
        public string participatetime { get; set; }   //考试日期
       
    }

    public class ExamModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);
        TeamModel teamModel = new TeamModel();

        #region Exam CRUD
        public List<ExamInfo> GetExamList()
        {
            var rst = db.tbl_exams
                .Where(m => m.deleted == 0)
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

            foreach (var n in rst)
            {
                if (n.teamid != 0)
                {
                    n.teamname = db.tbl_railteams.Where(m => m.uid == n.teamid).Select(m => m.teamname).FirstOrDefault();
                }
            }

            return rst;
        }

        public List<ExamInfo> GetExamListByCondition(int examkind, long teamid)
        {
            var rst = new List<ExamInfo>();
            var alllist = db.tbl_exams
                .Where(m => m.deleted == 0 )
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

            if (examkind == 0)
            {
                rst = alllist.Where(m => m.examkind == ExamKind.Sector).ToList();
            }
            else if (examkind == 1)
            {
                rst = alllist.Where(m => m.examkind == ExamKind.Team&&m.teamid==teamid).ToList();

            }
            

            foreach (var n in rst)
            {
                if (n.teamid != 0)
                {
                    n.teamname = db.tbl_railteams.Where(m => m.uid == n.teamid).Select(m => m.teamname).FirstOrDefault();
                }
            }

            return rst;
        }

        public List<ExamInfo> GetExamListByTeamid(long teamid)
        {
            var rst = db.tbl_exams
                .Where(m => m.deleted == 0&&m.teamid==teamid)
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

            foreach (var n in rst)
            {
                    if (n.teamid!=0)
                    {
                        n.teamname = db.tbl_railteams.Where(m => m.uid == n.teamid).Select(m => m.teamname).FirstOrDefault();
                    }
            }
            return rst;
        }

        public List<ExamInfo> GetSelectedExamList(long[] selids)
        {
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

        public ExamInfo GetExamInfo(long uid)
        {

            var rst = db.tbl_exams
                .Where(m => m.deleted == 0 && m.uid == uid)
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
                    createtime = m.createtime,
                    score=m.score
                })
                .FirstOrDefault();

            return rst;
        }

        public List<ExamInfo> RandomGenerate( List<ExamInfo> before, int amount)
        {

            List<ExamInfo> after = new List<ExamInfo>(amount);
            Random random = new Random();
            int index = 0;
            for (int i = 1; i <= amount; i++)
            {
                //从[1,container.Count + 1)中取一个随机值，保证这个值不会超过container的元素个数
                index = random.Next(1, before.Count );
                //将随机取得值的放到结果集合中
                after.Add(before[index-1]);
                //从容器集合中删除这个值，这样会导致container.Count发生变化
                before.RemoveAt(index-1);
                //注意这一句与上面一句能达到同样效果，但是没有上面一句快
                //container.Remove(value);
            }
            return after;
        }

        public JqDataTableInfo GetExamDataTableWithCondition(int examtype,long teamid,int type,int amount,JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamInfo> filteredCompanies;

            List<ExamInfo> alllist = GetExamListByCondition(examtype,teamid);
            List<ExamInfo> searchlist = alllist;
            List<ExamInfo> generatelist = alllist;

            if (type == 1)
            {
                searchlist = alllist.Where(m => m.examtype == "单选题").ToList();
            }
            else if (type == 2)
            {
                searchlist = alllist.Where(m => m.examtype == "多选题").ToList();
            }
            else if (type == 3)
            {
                searchlist = alllist.Where(m => m.examtype== "判断题").ToList();
            }
            else{
                searchlist=alllist;
            }
            if (amount != 0&&amount<=searchlist.Count)
            {
                generatelist = RandomGenerate(searchlist, amount);
            }
            else if (amount != 0 && amount > searchlist.Count)
            {
                amount = searchlist.Count;
                generatelist = RandomGenerate(searchlist, amount);
            }
            else
            {
                generatelist = searchlist;
            }
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = generatelist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = generatelist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.teamname,
                c.examtype,
                c.title,
                String.Format("{0:yyyy-MM-dd}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = generatelist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetExamDataTable(int type, int amount, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamInfo> filteredCompanies;

            List<ExamInfo> alllist = GetExamList();
            List<ExamInfo> searchlist = alllist;
            List<ExamInfo> generatelist = alllist;

            if (type == 1)
            {
                searchlist = alllist.Where(m => m.examtype == "单选题").ToList();
            }
            else if (type == 2)
            {
                searchlist = alllist.Where(m => m.examtype == "多选题").ToList();
            }
            else if (type == 3)
            {
                searchlist = alllist.Where(m => m.examtype == "判断题").ToList();
            }
            else
            {
                searchlist = alllist;
            }
            if (amount != 0 && amount <= searchlist.Count)
            {
                generatelist = RandomGenerate(searchlist, amount);
            }
            else if (amount != 0 && amount > searchlist.Count)
            {
                amount = searchlist.Count;
                generatelist = RandomGenerate(searchlist, amount);
            }
            else
            {
                generatelist = searchlist;
            }
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = generatelist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = generatelist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.teamname,
                c.examtype,
                c.title,
                String.Format("{0:yyyy-MM-dd }", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = generatelist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GenerateExamDataTable(int examkind,long teamid,int type, int amount, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();

            List<ExamInfo> alllist = GetExamListByCondition(examkind, teamid);
            List<ExamInfo> searchlist = alllist;
            List<ExamInfo> generatelist = alllist;

            if (type == 1)
            {
                searchlist = alllist.Where(m => m.examtype == "单选题").ToList();
            }
            else if (type == 2)
            {
                searchlist = alllist.Where(m => m.examtype == "多选题").ToList();
            }
            else if (type == 3)
            {
                searchlist = alllist.Where(m => m.examtype == "判断题").ToList();
            }
            else
            {
                searchlist = alllist;
            }
            if (amount != 0 && amount <= searchlist.Count)
            {
                generatelist = RandomGenerate(searchlist, amount);
            }
            else if (amount != 0 && amount > searchlist.Count)
            {
                amount = searchlist.Count;
                generatelist = RandomGenerate(searchlist, amount);
            }
            else
            {
                generatelist = searchlist;
            }
            //Check whether the companies should be filtered by keyword
            

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc


            var result = from c in generatelist
                         select new[] { 
                Convert.ToString(c.uid),
                ((int)c.examkind).ToString(),
                c.examtype,
                c.title,
                String.Format("{0:yyyy-MM-dd}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = generatelist.Count();
            rst.iTotalDisplayRecords = generatelist.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GetExamDataTableByTeamid(int type, int amount, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long teamid)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamInfo> filteredCompanies;

            List<ExamInfo> alllist = GetExamListByTeamid(teamid);
            List<ExamInfo> searchlist = alllist;
            List<ExamInfo> generatelist = alllist;

            if (type == 1)
            {
                searchlist = alllist.Where(m => m.examtype == "单选题").ToList();
            }
            else if (type == 2)
            {
                searchlist = alllist.Where(m => m.examtype == "多选题").ToList();
            }
            else if (type == 3)
            {
                searchlist = alllist.Where(m => m.examtype == "判断题").ToList();
            }
            else
            {
                searchlist = alllist;
            }
            if (amount != 0 && amount <= searchlist.Count)
            {
                generatelist = RandomGenerate(searchlist, amount);
            }
            else if (amount != 0 && amount > searchlist.Count)
            {
                amount = searchlist.Count;
                generatelist = RandomGenerate(searchlist, amount);
            }
            else
            {
                generatelist = searchlist;
            }


            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = generatelist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = generatelist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.teamname,
                c.examtype,
                c.title,
                String.Format("{0:yyyy-MM-dd}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = generatelist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public JqDataTableInfo GenerateExamDataTableByTeamid(long teamid,int type, int amount, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();

            List<ExamInfo> alllist = GetExamListByTeamid(teamid);
            List<ExamInfo> searchlist = alllist;
            List<ExamInfo> generatelist = alllist;

            if (type == 1)
            {
                searchlist = alllist.Where(m => m.examtype == "单选题").ToList();
            }
            else if (type == 2)
            {
                searchlist = alllist.Where(m => m.examtype == "多选题").ToList();
            }
            else if (type == 3)
            {
                searchlist = alllist.Where(m => m.examtype == "判断题").ToList();
            }
            else
            {
                searchlist = alllist;
            }
            if (amount != 0 && amount <= searchlist.Count)
            {
                generatelist = RandomGenerate(searchlist, amount);
            }
            else if (amount != 0 && amount > searchlist.Count)
            {
                amount = searchlist.Count;
                generatelist = RandomGenerate(searchlist, amount);
            }
            else
            {
                generatelist = searchlist;
            }
            //Check whether the companies should be filtered by keyword


            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc


            var result = from c in generatelist
                         select new[] { 
                Convert.ToString(c.uid),
                ((int)c.examkind).ToString(),
                c.examtype,
                c.title,
                String.Format("{0:yyyy-MM-dd}", c.createtime),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = generatelist.Count();
            rst.iTotalDisplayRecords = generatelist.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteExam(long[] items)
        {
            string delSql = "UPDATE tbl_exam SET deleted = 1 WHERE ";
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

        public string InsertExam(byte examkind, long teamid, string examtype, string title, string choisestr, string answerstr,int answer, int score)
        {
            tbl_exam newitem = new tbl_exam();
            long accountid = CommonModel.GetCurrAccountId();


            newitem.examkind = examkind;
            if (examkind == 0)
            {
                newitem.teamid = 0;
            }
            else
            {
                newitem.teamid = teamid;
            }
            newitem.examtype = examtype;

            newitem.title = title;
            if (examtype!="判断题")
            {
                newitem.question = choisestr;
                newitem.answer = answerstr;
            }
            else
            {
                newitem.answer = answer==0?"YES":"NO";
            }
            newitem.createtime = DateTime.Now;
            newitem.score =score;

            db.tbl_exams.InsertOnSubmit(newitem);

            db.SubmitChanges();

            return "";
        }

        public string UpdateExam(long uid, byte examkind, long teamid, string examtype, string title, string choisestr, string answerstr,int answer, int score)
        {
            string rst = "";
            tbl_exam edititem = db.tbl_exams.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();

            if (edititem != null)
            {
                edititem.examkind = examkind;
                if (examkind == 0)
                {
                    edititem.teamid = 0;
                }
                else
                {
                    edititem.teamid = teamid;
                }
                edititem.title = title;
                if (examtype != "判断题")
                {
                    edititem.question = choisestr;
                    edititem.answer = answerstr;
                }
                else
                {
                    edititem.answer = answer==0?"YES":"NO";
                }
                edititem.examtype = examtype;
                edititem.score = score;
                db.SubmitChanges();
                rst = "";
            }
            else
            {
                rst = "该试题不存在";
            }

            return rst;
        }
        #endregion

        #region ExamBook CRUD
        public List<ExamBookInfo> GetExamBookList()
        {
            var rst = db.tbl_exambooks
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    teamid=m.teamid,
                    title = m.title,
                    createtime = m.createtime,
                })
                .ToList();
            foreach (var r in rst)
            {
                if (r.teamid != null) 
                {
                    r.teamname = db.tbl_railteams.Where(m => m.uid == r.teamid).Select(m => m.teamname).FirstOrDefault();
                }

            }


            return rst;
        }

        public List<ExamBookInfo> GetTeamExamBookList(long teamid)
        {
            var rst = db.tbl_exambooks
                .Where(m => m.deleted == 0&&m.teamid==teamid)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    title = m.title,
                    createtime = m.createtime
                })
                .ToList();

            return rst;
        }

        public List<ExamBookInfo> GetExamBookList(int examkind)
        {
            long userid = CommonModel.GetSessionUserID();
            List<ExamBookInfo> filterrst = new List<ExamBookInfo>();

            var rst = db.tbl_exambooks
                .Where(m => m.deleted == 0 && m.examkind == examkind)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    examidstrs = m.examids,
                    title = m.title,
                    createtime = m.createtime,
                    teamid=m.teamid
                })
                .ToList();
            if (examkind==1)
            {
                //人员类型
                var uid = CommonModel.GetSessionUserID();
                UserModel usermodel=new UserModel();
                var userrole = usermodel.GetUserById(uid);

                //管理员或者科室干部
                if (userrole.userkind== 0 || (userrole.userkind==1&&userrole.exectype==ExecType.SectorExec))
                {

                }
                //车队干部
                else if (userrole.userkind == 1 && userrole.exectype == ExecType.TeamExec)
                {
                    var currentteamid=teamModel.GetExecparentIdFromUserId(uid);
                    rst = rst.Where(m => m.teamid == currentteamid).ToList();
                }
                //车队人员
                else if (userrole.userkind == 2)
                {
                    var currentteamid = teamModel.GetTeamIdFromUserId(uid);
                    rst = rst.Where(m => m.teamid == currentteamid).ToList();
                }
            }
            if (rst != null)
            {
                var resultlist = db.tbl_examresults.Where(m => m.deleted == 0 && m.userid == userid).ToList();
                foreach (var item in rst)
                {
                    var fitem = resultlist.Where(m => m.exambookid == item.uid).FirstOrDefault();

                    if (fitem == null) {

                        string[] examids = item.examidstrs.Split(',');
                        List<long> ids = new List<long>();
                        foreach (var id in examids)
                        {
                            if (!String.IsNullOrWhiteSpace(id))
                            {
                                ids.Add(long.Parse(id));
                            }
                        }
                        item.examids = ids;
                        filterrst.Add(item);
                    }
                }
            }

            return filterrst;
        }

        public ExamBookInfo GetExamBookInfo(long uid)
        {

            var rst = db.tbl_exambooks
                .Where(m => m.deleted == 0 && m.uid == uid)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    teamid = m.teamid,
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
        public long GetBookidByExamresulId(long id)
        {
            long bookid = db.tbl_examresults.Where(m => m.uid == id).Select(m => m.exambookid).FirstOrDefault();
            return bookid;
        }
        public ExamBookInfo GetExamBookInfoIncludeDeleted(long uid)
        {

            var rst = db.tbl_exambooks
                .Where(m => m.uid == uid)
                .OrderByDescending(m => m.createtime)
                .Select(m => new ExamBookInfo
                {
                    uid = m.uid,
                    examkind = (ExamKind)m.examkind,
                    teamid = m.teamid,
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

        public JqDataTableInfo GetExamBookDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamBookInfo> filteredCompanies;

            List<ExamBookInfo> alllist = GetExamBookList();

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamBookInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.examkind==ExamKind.Sector?"段级":c.teamname,
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

        public JqDataTableInfo GetTeamExamBookDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri,long teamid)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamBookInfo> filteredCompanies;

            List<ExamBookInfo> alllist = GetTeamExamBookList(teamid);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamBookInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
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

        public JqDataTableInfo GetBookDataTable(int examkind, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamBookInfo> filteredCompanies;

            List<ExamBookInfo> alllist = GetExamBookList(examkind);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamBookInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                //Convert.ToString(c.uid),
                c.title,
                c.examids.Count().ToString(),
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public bool DeleteExamBook(long[] items)
        {
            string delSql = "UPDATE tbl_exambook SET deleted = 1 WHERE ";
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

        public string InsertExamBook(byte examkind, string title, int examtime, string contents, string examids,long teamid)
        {
            string rst = "";
            var userrole = CommonModel.GetUserRoleInfo();
            try
            {
                tbl_exambook newitem = new tbl_exambook();

                newitem.examkind = examkind;
                newitem.title = title;
                newitem.examtime = examtime;

                List<string> eids = examids.Split(',').ToList();
                eids = eids.Select(m => m).Distinct().ToList();
                if (userrole != null && ((string)userrole).Contains("TeamManager"))
                {
                     var uid = CommonModel.GetSessionUserID();
                     teamid = teamModel.GetExecparentIdFromUserId(uid);
                     newitem.teamid = teamid;
                }
                else
                {
                    newitem.teamid = teamid;
                }
                newitem.examids = String.Join(",", eids.ToArray());
                newitem.contents = contents;
                newitem.createtime = DateTime.Now;

                db.tbl_exambooks.InsertOnSubmit(newitem);

                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public string UpdateExamBook(long uid, byte examkind, string title, int examtime, string contents, string examids,long teamid)
        {
            string rst = "";

            try
            {
                tbl_exambook edititem = db.tbl_exambooks.Where(m => m.deleted == 0 && m.uid == uid).FirstOrDefault();

                if (edititem != null)
                {
                    edititem.examkind = examkind;
                    edititem.title = title;
                    edititem.examtime = examtime;
                    edititem.contents = contents;
                    edititem.teamid = teamid;

                    List<string> eids = examids.Split(',').ToList();
                    eids = eids.Select(m => m).Distinct().ToList();

                    edititem.examids = String.Join(",", eids.ToArray());

                    db.SubmitChanges();
                    rst = "";
                }
                else
                {
                    rst = "该试卷不存在";
                }
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }
        #endregion

        #region Exam Result

        public List<ExamResultInfo> GetExamResultList(DateTime starttime, DateTime endtime)
        {
            var rst = db.tbl_examresults
                .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
                .OrderByDescending(m => m.createtime)
                .Join(db.tbl_exambooks, m => m.exambookid, l => l.uid, (m, l) => new { result = m, book = l })
                .Join(db.tbl_users, m => m.result.userid, l => l.uid, (m, l) => new { result = m, user = l })
              //  .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { result = m, group= l })

                .Select(m => new ExamResultInfo
                {
                    uid = m.result.result.uid,
                    bookid = m.result.result.exambookid,
                    examkind = (ExamKind)m.result.book.examkind,
                    title = m.result.book.title,
                    score = m.result.result.score,
                    parttime = m.result.result.participtime,
                    examtime = m.result.book.examtime,
                    papersecond = m.result.result.examsecond,
                    result = m.result.result.examresult,
                    createtime = m.result.result.createtime,
                    name = m.user.realname,
                    crewno = m.user.crewno,
                    userkind = m.user.userkind,
                    exectype = m.user.exectype,
                    execparentid = m.user.execparentid,
                    groupid = m.user.crewgroupid,
                    
                    //teamid = db.tbl_traingroups.Where(l => l.uid == m.user.crewgroupid).Select(l=>l.teamid).FirstOrDefault()
                })
                .ToList();

            if (rst != null)
            {
                foreach (var item in rst)
                {
                    if (item.groupid!=0)
                    {
                        item.teamid = db.tbl_traingroups.Where(l => l.uid == item.groupid).Select(l => l.teamid).FirstOrDefault();
                    }
                    var detinfo = GetJsonDeserializedExamResult(item.result);
                    if (detinfo != null)
                    {
                        item.totalnum = detinfo.totalyesno + detinfo.totalonesel + detinfo.totalmultisel;
                        item.totalnumstr = Convert.ToString(item.totalnum) + "道";
                        item.correctnum = detinfo.correctonesel + detinfo.correctmultisel + detinfo.correctyesno;
                        item.correctnumstr = Convert.ToString(item.correctnum) + "道";

                    }
                }
            }

            return rst;
        }

        public List<ExamResultInfo> GetExamSearchResultList(DateTime starttime, DateTime endtime, long groupid, long teamid, long userkind, long examtype, long exambook)
        {
            var rst = db.tbl_examresults
               .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
               .OrderByDescending(m => m.createtime)
               .Join(db.tbl_exambooks, m => m.exambookid, l => l.uid, (m, l) => new { result = m, book = l })
               .Join(db.tbl_users, m => m.result.userid, l => l.uid, (m, l) => new { result = m, user = l })
                //  .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { result = m, group= l })

               .Select(m => new ExamResultInfo
               {
                   uid = m.result.result.uid,
                   bookid = m.result.result.exambookid,
                   examkind = (ExamKind)m.result.book.examkind,
                   title = m.result.book.title,
                   score = m.result.result.score,
                   parttime = m.result.result.participtime,
                   examtime = m.result.book.examtime,
                   papersecond = m.result.result.examsecond,
                   result = m.result.result.examresult,
                   createtime = m.result.result.createtime,
                   name = m.user.realname,
                   crewno = m.user.crewno,
                   userkind = m.user.userkind,
                   exectype = m.user.exectype,
                   execparentid = m.user.execparentid,
                   groupid = m.user.crewgroupid,

                   //teamid = db.tbl_traingroups.Where(l => l.uid == m.user.crewgroupid).Select(l=>l.teamid).FirstOrDefault()
               })
               .ToList();

            if (rst != null)
            {
                foreach (var item in rst)
                {
                    if (item.groupid != 0)
                    {
                        item.teamid = db.tbl_traingroups.Where(l => l.uid == item.groupid).Select(l => l.teamid).FirstOrDefault();
                    }
                    var detinfo = GetJsonDeserializedExamResult(item.result);
                    if (detinfo != null)
                    {
                        item.totalnum = detinfo.totalyesno + detinfo.totalonesel + detinfo.totalmultisel;
                        item.totalnumstr = Convert.ToString(item.totalnum) + "道";
                        item.correctnum = detinfo.correctonesel + detinfo.correctmultisel + detinfo.correctyesno;
                        item.correctnumstr = Convert.ToString(item.correctnum) + "道";

                    }
                }
            }
            List<ExamResultInfo> alllist = rst;
            if (examtype != 0)
            {
                alllist = alllist.Where(m => (int)m.examkind == examtype - 1).ToList();
            }
            if (exambook != 0)
            {
                alllist = alllist.Where(m => m.bookid == exambook).ToList();
            }
            //科室干部
            if (userkind == 1)
            {
                alllist = alllist.Where(m => m.userkind == 1 && m.exectype == "科室干部").ToList();
            }
            //车队
            else if (userkind == 2)
            {
                alllist = alllist.Where(m => m.userkind == 1 && m.exectype == "车队干部").ToList();
                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.execparentid == teamid).ToList();
                }
            }
            else if (userkind == 3)
            {

                alllist = alllist.Where(m => m.userkind == 2).ToList();
                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                    if (groupid != 0)
                    {
                        alllist = alllist.Where(m => m.groupid == groupid).ToList();
                    }
                }
            }
            return alllist;
        }

        public List<ExamResultInfo> GetTeamExamResultList(DateTime starttime, DateTime endtime, long teamid, long groupid, int examtype, long exambookid)
        {

            var teambooklist = db.tbl_exambooks.Where(m => (m.teamid == teamid || m.teamid == 0) && m.deleted == 0 && m.examkind == 1).OrderBy(m => m.title)
                                                .ToList();
            var duanbooklist = db.tbl_exambooks.Where(m => m.examkind == 0 && m.deleted == 0).OrderBy(m => m.title)
                                                .ToList();
            List<tbl_exambook> booklist = new List<tbl_exambook>();
            if (examtype==0)
            {
                booklist = teambooklist.Concat(duanbooklist).OrderBy(m=>m.examkind).ToList();
            }
            else if (examtype == 1)
            {
                booklist = duanbooklist;
            }
            else if (examtype == 2)
            {
                booklist = teambooklist;
            }

            if (exambookid!=0)
            {
                booklist = booklist.Where(m => m.uid == exambookid).ToList();
            }
            var crewlist = db.tbl_users.Where(m => m.deleted == 0 && m.userkind == 2 && m.crewgroupid == groupid)
                                       .Select(m => new crewList
                                       {
                                           uid=m.uid,
                                           crewno = m.crewno,
                                           realname = m.realname,
                                       })
                                        .ToList();
            List<ExamResultInfo> templist=new List<ExamResultInfo>();
            foreach (var r in crewlist)
            {
                if (booklist.Count!=0)
                {
                    r.books = booklist;
                    //r.examresult = booklist;
                    //r.title = booklist[0].title;
                    //r.examkind = (ExamKind)booklist[0].examkind;
                    //r.examtime = booklist[0].examtime;
                    //r.bookid = booklist[0].uid;
                    //if (booklist.Count >= 2)
                    //{
                        //for (int i = 0; i < booklist.Count; i++)
                        //{
                        //    ExamResultInfo temp = new ExamResultInfo {
                        //                            uid=r.uid,
                        //                            crewno=r.crewno,
                        //                            name=r.name,
                        //                            title=booklist[i].title,
                        //                            examkind = (ExamKind)booklist[i].examkind,
                        //                            examtime = booklist[i].examtime,
                        //                            bookid=booklist[i].uid,
                        //    };
                        //    templist.Add(temp);
                        //}
                }
               // var resultofuser = db.tbl_examresults.Where(m => m.userid == r.uid).ToList();

                //r.examresult = r.books.Join(resultofuser, m => m.uid, l => l.exambookid, (m, l) => new { book = m, result = l })
                //              .Select(m => new ExamResultInfo
                //              {
                //                  uid = r.uid,
                //                  crewno = r.crewno,
                //                  name = r.realname,
                //                  bookid = m.book.uid,
                //                  title = m.book.title,
                //                  examkind = (ExamKind)m.book.examkind,
                //                  examtime = m.book.examtime,
                //                  score = m.result.score,
                //                  parttime = m.result.participtime,
                //                  papersecond = m.result.examsecond,
                //                  result = m.result.examresult,
                //                  createtime = m.result.createtime,
                //              }).ToList();

            }
            var templist2 = new List<ExamResultInfo>();
            foreach (var r in crewlist)
            {
                if (r.books!=null)
                {
                    foreach (var m in r.books)
                    {
                        var tempresult = db.tbl_examresults.Where(e => e.exambookid == m.uid && e.userid == r.uid).ToList();
                        var participatetime = "未参加";
                        if (tempresult.Count != 0)
                        {
                            participatetime = Convert.ToString(tempresult.Select(e => e.participtime).FirstOrDefault());
                        }
                        templist2.Add(new ExamResultInfo
                        {
                            uid = r.uid,
                            crewno = r.crewno,
                            name = r.realname,
                            bookid = m.uid,
                            title = m.title,
                            examkind = (ExamKind)m.examkind,
                            examtime = m.examtime,
                            score = tempresult.Select(e => e.score).FirstOrDefault(),
                            participatetime = participatetime,
                            papersecond = tempresult.Select(e => e.examsecond).FirstOrDefault(),
                            result = tempresult.Select(e => e.examresult).FirstOrDefault(),
                            createtime = tempresult.Select(e => e.createtime).FirstOrDefault(),
                        });
                    }
                }
                }
                
           // var list = crewlist.Concat(templist).ToList().OrderBy(m=>m.uid);

            //var rst = templist.Join(db.tbl_examresults, m => m.bookid, l => l.exambookid, (m, l) => new { user = m, result = l })
            //                 .Select(r => new ExamResultInfo
            //                 {
            //                     uid = r.user.uid,
            //                     crewno = r.user.crewno,
            //                     name = r.user.name,
            //                     title = r.user.title,
            //                     examkind = r.user.examkind,
            //                     examtime = r.user.examtime,
            //                     bookid = r.user.bookid,
            //                     score = r.result.score,
            //                     parttime = r.result.participtime,
            //                     papersecond = r.result.examsecond,
            //                     result = r.result.examresult,
            //                     createtime = r.result.createtime,
            //                 }).ToList();
            var rst = templist2;

            //var rst = db.tbl_examresults
            //    .Where(m => m.deleted == 0 && starttime <= m.createtime && m.createtime < endtime.AddDays(1))
            //    .OrderByDescending(m => m.createtime)
            //    .Join(db.tbl_exambooks, m => m.exambookid, l => l.uid, (m, l) => new { result = m, book = l })
            //    .Join(db.tbl_users, m => m.result.userid, l => l.uid, (m, l) => new { result = m, user = l })

            //    .Select(m => new ExamResultInfo
            //    {
            //        uid = m.result.result.uid,
            //        bookid = m.result.result.exambookid,
            //        examkind = (ExamKind)m.result.book.examkind,
            //        title = m.result.book.title,
            //        score = m.result.result.score,
            //        parttime = m.result.result.participtime,
            //        examtime = m.result.book.examtime,
            //        papersecond = m.result.result.examsecond,
            //        result = m.result.result.examresult,
            //        createtime = m.result.result.createtime,
            //        name = m.user.realname,
            //        crewno = m.user.crewno,
            //        userkind = m.user.userkind,
            //        exectype = m.user.exectype,
            //        execparentid = m.user.execparentid,
            //        groupid = m.user.crewgroupid,
            //    })
            //    .ToList();

            if (rst != null)
            {
                foreach (var item in rst)
                {
                    var detinfo = GetJsonDeserializedExamResult(item.result);
                    if (detinfo != null)
                    {
                        item.totalnum = detinfo.totalyesno + detinfo.totalonesel + detinfo.totalmultisel;
                        item.totalnumstr = Convert.ToString(detinfo.totalyesno + detinfo.totalonesel + detinfo.totalmultisel)+"道";
                        item.correctnum = detinfo.correctonesel + detinfo.correctmultisel + detinfo.correctyesno;
                        item.correctnumstr = Convert.ToString(detinfo.correctonesel + detinfo.correctmultisel + detinfo.correctyesno) + "道";
                    }
                }
            }

            return rst;
        }

        public JqDataTableInfo GetExamResultDataTable(DateTime starttime, DateTime endtime, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamResultInfo> filteredCompanies;

            List<ExamResultInfo> alllist = GetExamResultList(starttime, endtime);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamResultInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.title,
                c.crewno,
                c.name,
                c.score.ToString(),
                Convert.ToString( c.parttime),
                c.examtime.ToString(),
                Convert.ToInt32(int.Parse(c.papersecond) / 60) + "分" + Convert.ToInt32(int.Parse(c.papersecond) % 60) + "秒",
                c.totalnumstr + " / " + c.correctnumstr,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }
        public JqDataTableInfo SearchExamResultDataTable(DateTime starttime, DateTime endtime, long groupid, long teamid, long userkind, long examtype, long exambook, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamResultInfo> filteredCompanies;
            List<ExamResultInfo> alllist = GetExamResultList(starttime, endtime);
//             List<ExamResultInfo> searchlist = alllist;
//             List<ExamResultInfo> searchlist2 = searchlist;
//             List<ExamResultInfo> searchlist3 = searchlist2;

            if (examtype != 0)
            {
                alllist = alllist.Where(m => (int)m.examkind == examtype-1).ToList();
            }
            if (exambook != 0)
            {
                alllist = alllist.Where(m => m.bookid == exambook).ToList();
            } 
            //科室干部
            if (userkind==1)
            {
                alllist = alllist.Where(m => m.userkind == 1 && m.exectype == "科室干部").ToList();
            }
                //车队
            else if (userkind == 2)
            {
                alllist = alllist.Where(m => m.userkind == 1 && m.exectype == "车队干部").ToList();
                if (teamid!=0)
                {
                    alllist = alllist.Where(m => m.execparentid == teamid).ToList();
                }
            }
            else if (userkind == 3)
            {

                alllist = alllist.Where(m => m.userkind == 2).ToList();
                if (teamid != 0)
                {
                    alllist = alllist.Where(m => m.teamid == teamid).ToList();
                    if (groupid != 0)
                    {
                        alllist = alllist.Where(m => m.groupid == groupid).ToList();
                    }
                }
            }

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamResultInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.title,
                c.crewno,
                c.name,
                c.score.ToString(),
                String.Format("{0:yyyy-MM-dd}", c.parttime),
                c.examtime.ToString(),
                Convert.ToInt32(int.Parse(c.papersecond) / 60) + "分" + Convert.ToInt32(int.Parse(c.papersecond) % 60) + "秒",
                c.totalnumstr.ToString() + " / " + c.correctnumstr.ToString(),
                Convert.ToString(c.bookid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }
        public JqDataTableInfo SearchTeamExamResultDataTable(DateTime starttime, DateTime endtime, long groupid, long teamid, int examtype,long exambooid, JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamResultInfo> filteredCompanies;

            List<ExamResultInfo> alllist = GetTeamExamResultList(starttime, endtime, teamid, groupid, examtype, exambooid);

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamResultInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.title,
                c.crewno,
                c.name,
                c.score.ToString(),
                c.participatetime,
                c.examtime.ToString(),
                c.papersecond,
                c.totalnumstr + " / " + c.correctnumstr,
                Convert.ToString(c.bookid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        #endregion

     //   #region 车队试题导入
       // OleDbConnection oledbConn2;
        public string ImportTeamExamData(string filepath)
        {
            var uid = CommonModel.GetSessionUserID();
            var sessionteamid = teamModel.GetExecparentIdFromUserId(uid);
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
            long myid = CommonModel.GetSessionUserID();

            try
            {
                string fname = orgbase + filepath;
                if (!File.Exists(fname))
                {
                    return "文件不存在";
                }

                if (Path.GetExtension(fname) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(fname) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fname + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;';");
                }
                else if (Path.GetExtension(fname) == ".csv")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties='text;'");
                }
                oledbConn.Open();
                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + Sheet1 + "]";
                var reader = cmd.ExecuteReader();

                int ind = 0;
                List<tbl_exam> examlist = new List<tbl_exam>();
                var teamlist = teamModel.GetTeamList();

                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        ind++;
                        continue;
                    }

                    tbl_exam newitem = new tbl_exam();

                    newitem.examkind = reader.GetValue(0).ToString() == "段级" ? (byte)ExamKind.Sector : (byte)ExamKind.Team;
                    var teamid = teamlist.Where(m => m.teamname == reader.GetValue(1).ToString()).Select(m => m.uid).FirstOrDefault();
                    if (teamid == null)
                    {
                        newitem.teamid = 0;
                    }
                    else
                    {
                        newitem.teamid = teamid;
                    }

                    newitem.examtype = reader.GetValue(3).ToString();

                    if (newitem.examkind == (byte)ExamKind.Team)
                    {
                        newitem.teamid = teamid;
                    }

                    string[] lines = reader.GetValue(4).ToString().Split('\n');

                    newitem.title = reader.GetValue(4).ToString();

                    if (newitem.examtype == ExamType.MultiSel || newitem.examtype == ExamType.OneSel)
                    {
                        newitem.title = lines[0];

                        List<string> questions = new List<string>(lines);
                        questions.RemoveAt(0);
                        
                        string[] arrs = questions.ToArray();

                        newitem.question = string.Join("\n", arrs);
                    } 
                    else if (newitem.examtype == ExamType.YesNo)
                    {
                        newitem.question = "";
                    }
                    else
                    {
                        rst = "试题名称不对，应为单选题/多选题/判断题";
                        return rst;

                    }

                    newitem.answer = reader.GetValue(5).ToString();
                    newitem.score = 1;
                    newitem.createtime = DateTime.Now;

                    if (newitem.teamid == sessionteamid)
                    {
                        examlist.Add(newitem);
                    }
                  //  examlist.Add(newitem);
                    
                    ind++;
                }
                reader.Close();

                db.tbl_exams.InsertAllOnSubmit(examlist);
                db.SubmitChanges();
                rst = "";
            }

            catch (Exception ex)
            {
                rst = ex.ToString();
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }

        #region 试题导入
        OleDbConnection oledbConn;
        public string ImportExamData(string filepath)
        {
            string rst = "";
            string savepath = "Content/uploads/img/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string orgbase = HostingEnvironment.MapPath("~/Content/uploads/temp/");
            string targetbase = HostingEnvironment.MapPath("~/" + savepath);
            long myid = CommonModel.GetSessionUserID();

            try
            {
                string fname = orgbase + filepath;
                if (!File.Exists(fname))
                {
                    return "文件不存在";
                }

                if (Path.GetExtension(fname) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(fname) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fname + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;';");
                }
                else if (Path.GetExtension(fname) == ".csv")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fname + ";Extended Properties='text;'");
                }
                oledbConn.Open();
                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + Sheet1 + "]";
                var reader = cmd.ExecuteReader();

                int ind = 0;
                List<tbl_exam> examlist = new List<tbl_exam>();
                var teamlist = teamModel.GetTeamList();

                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        ind++;
                        continue;
                    }

                    tbl_exam newitem = new tbl_exam();

                    newitem.examkind = reader.GetValue(0).ToString() == "段级" ? (byte)ExamKind.Sector : (byte)ExamKind.Team;

                    if (reader.GetValue(0).ToString() == "段级")
                    {
                        newitem.teamid = 0;
                    }
                    else
                    {
                        try
                        {
                            newitem.teamid = teamlist.Where(m => m.teamname == reader.GetValue(1).ToString()).FirstOrDefault().uid;
                        }
                        catch 
                        {
                            rst = "车队输入错误，错误车队为: " + reader.GetValue(1).ToString();
                            return rst;
                        }
                    }
                   // var teamid = teamlist.Where(m => m.teamname == reader.GetValue(1).ToString()).Select(m => m.uid).FirstOrDefault();

                    string[] lines = reader.GetValue(4).ToString().Split('\n');

                    newitem.title = reader.GetValue(4).ToString();
                    newitem.examtype = reader.GetValue(3).ToString();

                    if (newitem.examtype == ExamType.MultiSel || newitem.examtype == ExamType.OneSel)
                    {
                        newitem.title = lines[0];

                        List<string> questions = new List<string>(lines);
                        questions.RemoveAt(0);

                        string[] arrs = questions.ToArray();

                        newitem.question = string.Join("\n", arrs);
                    }
                    else if (newitem.examtype == ExamType.YesNo)
                    {
                        newitem.question = "";
                    }
                    else
                    {
                        rst = "试题名称不对，应为单选题/多选题/判断题";
                        return rst;
                        
                    }
                    newitem.answer = reader.GetValue(5).ToString();
                    newitem.score = int.Parse(reader.GetValue(6).ToString());
                    newitem.createtime = DateTime.Now;

                    examlist.Add(newitem);

                    ind++;
                }
                reader.Close();

                db.tbl_exams.InsertAllOnSubmit(examlist);
                db.SubmitChanges();
                rst = "";
            }

            catch (Exception ex)
            {
                rst = "请检查录入内容";
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }


        #endregion

        public bool CheckDuplicateBookName(string title, long uid)
        {
            bool rst = true;

            rst = ((from m in db.tbl_exambooks
                    where m.deleted == 0 && m.title == title && m.uid != uid
                    select m).FirstOrDefault() == null);

            return rst;
        }

        public JqDataTableInfo GetSelectedExamDataTable(string examids, /*string selids, */JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ExamInfo> filteredCompanies;

            string[] examidlist = examids.Split(',');
            //string[] selidlist = selids.Split(',');

            List<long> ids = new List<long>();

            foreach (var item in examidlist)
            {
                if (!String.IsNullOrWhiteSpace(item))
                {
                    ids.Add(long.Parse(item));
                }
            }

//             foreach (var item in selidlist)
//             {
//                 if (!String.IsNullOrWhiteSpace(item))
//                 {
//                     ids.Add(long.Parse(item));
//                 }
//             }

            ids = ids.Select(m => m).Distinct().ToList();

            List<ExamInfo> alllist = GetSelectedExamList(ids.ToArray());

            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredCompanies = alllist
                   .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredCompanies = alllist;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ExamInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.title :
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
                ((int)c.examkind).ToString(),
                c.examtype,
                c.title,
                Convert.ToString(c.uid)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
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
                        int xlsanswer = (item.answerstr.ToUpper() == "YES") ? 1 : 0;
                        newitem.answer = xlsanswer;
                    }
                    else if (item.examtype == ExamType.MultiSel)
                    {
                        newitem.question = ParseMultiSelQuestion(item.questionstr);
                        newitem.answer = ParseMultiSelAnswer(item.answerstr);
                    }
                    else if (item.examtype == ExamType.OneSel)
                    {
                        newitem.question = ParseMultiSelQuestion(item.questionstr);
                        newitem.answer = item.answerstr.Trim();
                    }

                    rst.Add(newitem);
                }
            }

            return rst;
        }

        public string GetJsonSerializedString(object examlist)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string jsonstr = jsonSerializer.Serialize(examlist);

            return jsonstr;
        }

        public JsonExamResult GetJsonDeserializedExamResult(string result)
        {
            JsonExamResult rst = new JsonExamResult();

            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                rst = jsonSerializer.Deserialize<JsonExamResult>(result);
            }
            catch (System.Exception ex)
            {
            	
            }

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

        public List<string> ParseMultiSelAnswer(string answer)
        {
            List<string> rst = new List<string>();

            rst = answer.Split(',').ToList();

            return rst;
        }

        public string SubmitExamResult(long uid,string usedtime, NameValueCollection request)
        {
            string rst = "";
            int oneselcount = 0;
            int multiselcount = 0;
            int yesnocount = 0;

            int totalonesel = 0, totalmultisel = 0, totalyesno = 0;

            var bookinfo = GetExamBookInfo(uid);
            var examlist = GetJsonExamList(bookinfo.examids);


            try
            {
                tbl_examresult result = new tbl_examresult();
                JsonExamResult examresult = new JsonExamResult();
                examresult.result = new List<JsonExamRstDetail>();

                result.userid = CommonModel.GetSessionUserID();
                result.exambookid = uid;
                result.examsecond = usedtime;

                int i = 1;
                foreach (var item in examlist)
                {
                    object myanswer = "";
                    if (item.examtype == ExamType.OneSel)
                    {
                        if (request["onesel_" + i.ToString()] != null)
                        {
                            string userval = request["onesel_" + i.ToString()].ToString();
                            if (userval == (String)item.answer)
                            {
                                oneselcount++;
                            }
                            myanswer = userval;
                        }
                        totalonesel++;
                    }
                    else if (item.examtype == ExamType.MultiSel)
                    {
                        if (request["multisel_" + i.ToString()] != null)
                        {
                            string[] userval = request["multisel_" + i.ToString()].ToString().Split(',');

                            int corrcnt = 0;
                            foreach (var ans in (List<string>)item.answer)
                            {
                                if (userval.Contains(ans))
                                {
                                    corrcnt++;
                                }
                            }

                            if (corrcnt == userval.Count())
                            {
                                multiselcount++;
                            }

                            myanswer = userval;
                        }
                        totalmultisel++;
                    }
                    else if (item.examtype == ExamType.YesNo)
                    {
                        if (request["yesno_" + i.ToString()] != null)
                        {
                            string userval = request["yesno_" + i.ToString()].ToString();
                            {
                                yesnocount++;
                            }
                            myanswer = userval;
                        }
                        totalyesno++;
                    }

                    JsonExamRstDetail detinfo = new JsonExamRstDetail();
                    detinfo.id = item.id;
                    detinfo.examtype = item.examtype;
                    detinfo.title = item.title;
                    detinfo.question = item.question;
                    detinfo.answer = item.answer;
                    detinfo.myanswer = myanswer;

                    examresult.result.Add(detinfo);

                    i++;
                }

                result.score = (int)(decimal.Round(((oneselcount + multiselcount + yesnocount) / (decimal)(totalonesel + totalmultisel + totalyesno)) * 100, 0));

                examresult.correctonesel = oneselcount;
                examresult.correctmultisel = multiselcount;
                examresult.correctyesno = yesnocount;
                examresult.totalonesel = totalonesel;
                examresult.totalmultisel = totalmultisel;
                examresult.totalyesno = totalyesno;

                examresult.score = result.score;

                result.examresult = GetJsonSerializedString(examresult);
                result.participtime = DateTime.Now;
                result.createtime = DateTime.Now;

                db.tbl_examresults.InsertOnSubmit(result);
                db.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                rst = ex.ToString();
            }

            return rst;
        }

        public JsonExamResult GetMyResultFromBookID(long bookid)
        {
            JsonExamResult rst = new JsonExamResult();
            long myid = CommonModel.GetSessionUserID();

            var resultinfo = db.tbl_examresults.Where(m => m.deleted == 0 && m.exambookid == bookid && m.userid == myid)
                .FirstOrDefault();

            if (resultinfo != null)
            {
                rst = GetJsonDeserializedExamResult(resultinfo.examresult);
            }

            return rst;

        }

        public tbl_examresult GetMyUsedtimeFromBookID(long bookid)
        {
            long myid = CommonModel.GetSessionUserID();

            var resultinfo = db.tbl_examresults.Where(m => m.deleted == 0 && m.exambookid == bookid && m.userid == myid)
                .FirstOrDefault();


            return resultinfo;

        }

        public tbl_examresult GetUsedtimeFromExamID(long id)
        {

            var resultinfo = db.tbl_examresults.Where(m => m.deleted == 0 && m.uid == id )
                .FirstOrDefault();


            return resultinfo;

        }
        public JsonExamResult GetResultFromBookID(long uid)
        {
            JsonExamResult rst = new JsonExamResult();

            var resultinfo = db.tbl_examresults.Where(m => m.deleted == 0 && m.uid == uid)
                .FirstOrDefault();

            if (resultinfo != null)
            {
                rst = GetJsonDeserializedExamResult(resultinfo.examresult);
            }

            return rst;

        }
        public List<ExamChartInfo> GetExamChartData(long teamid, long groupid, string starttime, string endtime)
        {
            List<ExamChartInfo> rst = new List<ExamChartInfo>();
            DateTime startd = new DateTime(1970, 1, 1);
            DateTime endd = new DateTime(2040, 1, 1);

            try { startd = DateTime.Parse(starttime); }
            catch { }
            try { endd = DateTime.Parse(endtime); }
            catch { }

            var datalist = db.tbl_examresults
                .Where(m => m.deleted == 0 && startd <= m.participtime && m.participtime < endd.AddDays(1))
                .Join(db.tbl_exambooks, m => m.exambookid, l => l.uid, (m, l) => new { r = m, book = l })
                .Join(db.tbl_users, m => m.r.userid, l => l.uid, (m, l) => new { r = m, user = l })
                //.Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { r = m, group = l })
                //.Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { r = m, team = l })
                .Select(m => new ExamResultInfo
                {
                    uid = m.r.r.uid,
                    score = m.r.r.score,
                    bookname = m.r.book.title,
                    userkind=m.user.userkind,
                    groupid=m.user.crewgroupid,
                    exectype=m.user.exectype,
                    execparentid=m.user.execparentid
                })
                .ToList();
            foreach (var n in datalist)
            {
                if (n.userkind==2)
                {
                    n.teamid = db.tbl_traingroups.Where(m => m.uid == n.groupid).Select(m => m.teamid).FirstOrDefault();
                }
                if (n.userkind == 1)
                {
                    if (n.exectype == ExecType.TeamExec)
                    {
                        n.teamid = n.execparentid;
                    }
                }
                else
                {
                    n.teamid = 0;
                }
            }

            if (teamid!=0)
            {
                datalist = datalist.Where(m => m.teamid == teamid).ToList();
            }
            if (groupid!=0)
            {
                datalist = datalist.Where(m => m.groupid == groupid).ToList();

            }


            var titlelist = datalist.Select(m => m.bookname).Distinct();


//             for (int i = 0; i < 10; i++)
//             {
//                 ExamChartInfo newitem = new ExamChartInfo();
//                 newitem.label = i.ToString() + "分~" + (i + 1).ToString() + "分";
// 
//                 var scorecnt = datalist.Where(m => i <= m.score && m.score <= (i+1) ).Select(m => m.score).Count();
//                 newitem.data = scorecnt;
//             
//                 rst.Add(newitem);
// 
//             }

            ExamChartInfo newitem = new ExamChartInfo();
            newitem.label ="100分~" + "91分";

            var scorecnt = datalist.Where(m => 90 < m.score && m.score <= 100 ).Select(m => m.score).Count();
            newitem.data = scorecnt;
            
            rst.Add(newitem);

            ExamChartInfo newitem2 = new ExamChartInfo();
            newitem2.label = "90分~" + "81分";

            var scorecnt2 = datalist.Where(m => 80 < m.score && m.score <= 90).Select(m => m.score).Count();
            newitem2.data = scorecnt2;

            rst.Add(newitem2);

            ExamChartInfo newitem3 = new ExamChartInfo();
            newitem3.label = "80分~" + "71分";

            var scorecnt3 = datalist.Where(m => 70 < m.score && m.score <= 80).Select(m => m.score).Count();
            newitem3.data = scorecnt3;

            rst.Add(newitem3);

            ExamChartInfo newitem4 = new ExamChartInfo();
            newitem4.label = "70分以下";

            var scorecnt4 = datalist.Where(m => 0 <= m.score && m.score <= 70).Select(m => m.score).Count();
            newitem4.data = scorecnt4;

            rst.Add(newitem4);

            return rst;
        }

        public System.Data.DataTable ExportResultList()
        {
            var products = new System.Data.DataTable("");
            DateTime starttime = new DateTime(1970, 1, 1);
            DateTime endtime = new DateTime(2030, 1, 1);

            List<ExamResultInfo> alllist = GetExamResultList(starttime, endtime);

            if (products != null)
            {
                products.Columns.Add("考试范围", typeof(string));
                products.Columns.Add("考试标题", typeof(string));
                products.Columns.Add("工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("考试分数", typeof(string));
                products.Columns.Add("参加考试日期", typeof(string));
                products.Columns.Add("考试时间", typeof(string));
                products.Columns.Add("答题时间", typeof(string));
                products.Columns.Add("总题数/答对数", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        (item.examkind == ExamKind.Sector) ? "段级考试" : "车队考试",
                        item.title,
                        item.crewno,
                        item.name,
                        item.score.ToString(),
                        String.Format("{0:yyyy-MM-dd}", item.parttime),
                        item.examtime.ToString(),
                        item.papersecond,
                        item.totalnumstr + " / " + item.correctnumstr
                    );
                }
            }

            return products;
        }
        public System.Data.DataTable ExportSearchResultList(DateTime starttime, DateTime endtime, long groupid, long teamid, long userkind, long examtype, long exambook)
        {
            var products = new System.Data.DataTable("");

            List<ExamResultInfo> alllist = GetExamSearchResultList( starttime,  endtime,  groupid,  teamid,  userkind,  examtype,  exambook);

            if (products != null)
            {
                products.Columns.Add("考试范围", typeof(string));
                products.Columns.Add("考试标题", typeof(string));
                products.Columns.Add("工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("考试分数", typeof(string));
                products.Columns.Add("参加考试日期", typeof(string));
                products.Columns.Add("考试时间", typeof(string));
                products.Columns.Add("答题时间", typeof(string));
                products.Columns.Add("总题数/答对数", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        (item.examkind == ExamKind.Sector) ? "段级考试" : "车队考试",
                        item.title,
                        item.crewno,
                        item.name,
                        item.score.ToString(),
                        String.Format("{0:yyyy-MM-dd}", item.parttime),
                        item.examtime.ToString(),
                        item.papersecond,
                        item.totalnumstr + " / " + item.correctnumstr
                    );
                }
            }

            return products;
        }
        public System.Data.DataTable ExportTeamResultList(DateTime starttime, DateTime endtime, long teamid, long groupid, int examtype, long exambookid)
        {
            var products = new System.Data.DataTable("");

            List<ExamResultInfo> alllist = GetTeamExamResultList( starttime,  endtime, teamid, groupid, examtype,exambookid);

            if (products != null)
            {
                products.Columns.Add("考试范围", typeof(string));
                products.Columns.Add("考试标题", typeof(string));
                products.Columns.Add("工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("考试分数", typeof(string));
                products.Columns.Add("参加考试日期", typeof(string));
                products.Columns.Add("考试时间", typeof(string));
                products.Columns.Add("答题时间", typeof(string));
                products.Columns.Add("总题数/答对数", typeof(string));

                foreach (var item in alllist)
                {

                    products.Rows.Add(
                        (item.examkind == ExamKind.Sector) ? "段级考试" : "车队考试",
                        item.title,
                        item.crewno,
                        item.name,
                        item.score.ToString(),
                        String.Format("{0:yyyy-MM-dd}", item.parttime),
                        item.examtime.ToString(),
                        item.papersecond,
                        item.totalnumstr + " / " + item.correctnumstr
                    );
                }
            }

            return products;
        }
        public List<ExamBookInfo> GetTeamExamBookListofType(int exambookid,long teamid)
        {
             var rst=new List<ExamBookInfo>();
             var teambooklist = new List<ExamBookInfo>();
             var duanbooklist=db.tbl_exambooks.Where(m => m.deleted==0&&m.examkind==0)
                                  .Select(m => new ExamBookInfo
                                  {
                                     examkind=(ExamKind)m.examkind,
                                     teamid=m.teamid,
                                     title=m.title,
                                     uid=m.uid
                                  }).ToList();
            if (teamid!=0)
            {
                teambooklist = db.tbl_exambooks.Where(m => m.deleted == 0 && m.examkind == 1 && (m.teamid == teamid || m.teamid == 0))
                                  .Select(m => new ExamBookInfo
                                  {
                                      examkind = (ExamKind)m.examkind,
                                      teamid = m.teamid,
                                      title = m.title,
                                      uid = m.uid
                                  }).ToList();
            }
            else
            {
                teambooklist = db.tbl_exambooks.Where(m => m.deleted == 0 && m.examkind == 1 )
                                 .Select(m => new ExamBookInfo
                                 {
                                     examkind = (ExamKind)m.examkind,
                                     teamid = m.teamid,
                                     title = m.title,
                                     uid = m.uid
                                 }).ToList();
            }
            
            if (exambookid ==0)
            {
                rst=teambooklist.Concat(duanbooklist).OrderBy(m=>m.examkind).ToList();
            }
            else if (exambookid==1)
            {
                rst = duanbooklist;
            }
            else if (exambookid==2)
            {
                rst = teambooklist;
            }
            return rst;
        }

    }
}