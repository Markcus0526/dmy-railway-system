using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLSite.Models
{

    public class CombineCheckTable
    {
        public string receivepart { get; set; }
        public string crewname { get; set; }
        public string checkno { get; set; }
        public string content { get; set; }
        public string checkersector { get; set; }
        public string checkername { get; set; }
        public string teamname { get; set; }
        public string groupname { get; set; }
        public DateTime checktime { get; set; }

        public long groupid { get; set; }

        public long teamid { get; set; }
        public long sectorid { get; set; }
        public string checklevel { get; set; }
    }

    public class CombineCheckModel
    {
        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        //在checklog表中得到检查部门信息，让前台显示
        public List<string> getCheckerSector()
        {
            var rst =
            db.tbl_checklogs.Select(m => m.checkexecparent).ToList();

            for (int i = 0; i < rst.Count; )
            {
                if (rst.LastIndexOf(rst[i]) == i)
                {
                    i++;
                }
                else
                {
                    rst.RemoveAt(i);
                }
            }
            return rst;
        }

        /*查询操作
       * @allchecklog 为所有checklog信息
       * 在allchecklog选择用户输入条件的信息
       * 
       */
        public List<CombineCheckTable> serchbycondition(string date, string checklevel, long teamid, long groupid, String crewname, String receivepart)
        {
            var cloglist = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("结合部")).ToList();

            var allchecklog = cloglist
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
                .Join(db.tbl_traingroups, m => m.checklog.groupid, l => l.uid, (m, l) => new { checkinfo = m, group = l })
                .Join(db.tbl_railteams, m => m.checkinfo.checklog.teamid, l => l.uid, (m, l) => new { group = m, team = l })
              //  .Join(db.tbl_users, m => m.group.checkinfo.checklog.crewid, l => l.uid, (m, l) => new { team = m, user = l })
                .Select(m => new CombineCheckTable
                {

                    receivepart=m.group.checkinfo.checklog.receivepart,
                    crewname=m.group.checkinfo.checklog.recievename,
                    checkno = m.group.checkinfo.checkinfo.checkno,
                    content = m.group.checkinfo.checklog.contents,
                    checkername = m.group.checkinfo.checklog.checkername,
                    checkersector = m.group.checkinfo.checklog.checkexecparent,
                    teamname = m.team.teamname,
                    groupname = m.group.group.groupname,
                    checktime = m.group.checkinfo.checklog.checktime,

                    groupid= m.group.group.uid,
                    teamid = m.team.uid,
                    checklevel = m.group.checkinfo.checklog.checklevel
                }).ToList();

            /*过滤内容
             * checkersector
             * checkername
             * sectorid
             * checklevel
             * checkno
             * trainno
             * keyword
             * checkpoint
             * 
             */

            if (date != "" && date.Length != 0)
            {
                var year = DateTime.Parse(date).Year;
                var month = DateTime.Parse(date).Month;
                allchecklog = allchecklog.Where(m => m.checktime.Year==year&&m.checktime.Month==month).ToList();
            }
            if (checklevel != "" && checklevel.Length != 0)
            {

                allchecklog = allchecklog.Where(m => m.checklevel == checklevel).ToList();
            }

            if ( teamid!= 0)
            {
                allchecklog = allchecklog.Where(m => m.teamid == teamid).ToList();
            }

            if (groupid != 0)
            {
                allchecklog = allchecklog.Where(m => m.groupid == groupid).ToList();
            }

            if (crewname != "" && !string.IsNullOrWhiteSpace(crewname))
            {
                allchecklog = allchecklog.Where(m => m.crewname.Contains(crewname)).ToList();
            }

            if (receivepart != "" && !string.IsNullOrWhiteSpace(receivepart))
            {
                allchecklog = allchecklog.Where(m => m.receivepart.Contains(receivepart)).ToList();
            }


            return allchecklog;
        }

        //导出模型
        //导出积分用
        public System.Data.DataTable ExportCreditList(string date, string checklevel, long teamid, long groupid, String crewname, String receivepart)
        {
            var products = new System.Data.DataTable("积分查询表");


            var alllist = serchbycondition( date,  checklevel,  teamid,  groupid,  crewname,  receivepart);

            if (products != null)
            {
                products.Columns.Add("受检部门", typeof(string));
                products.Columns.Add("责任人", typeof(string));
                products.Columns.Add("项点编码", typeof(string));
                products.Columns.Add("问题内容", typeof(string));
                products.Columns.Add("检查部门", typeof(string));
                products.Columns.Add("检查人", typeof(string));
                products.Columns.Add("所属车队", typeof(string));
                products.Columns.Add("责任班组", typeof(string));
                products.Columns.Add("检查时间", typeof(string));

                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(

                        item.receivepart,
                        item.crewname,
                        item.checkno,
                        item.content,
                        item.checkersector,
                        item.checkername,
                        item.teamname,
                        item.groupname,
                        item.checktime
                        );
                    i++;
                }
            }

            return products;
        }
    }
}