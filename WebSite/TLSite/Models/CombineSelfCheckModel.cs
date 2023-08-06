using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TLSite.Models
{
    public class CombineSelfCheckModel 
    {
        //
        // GET: /ConbineCheckModel/

        TieluDBDataContext db = new TieluDBDataContext();
        public class forStore
        {



            public int checkpoint { set; get; }

            public string checklevel { set; get; }

            public long groupid { set; get; }


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

        //结合部问题查询方法
        public List<CombineCheckViewTBList> CombineCheck(int year, int month, long sectorid)
        {


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
                     .Where(m => m.deleted == 0 &&m.logtype.Contains("结合部"))
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
                var two = rst.Where(m => m.checklevel == "车队" ).ToList().Count();
                //段以上
                var three = rst.Where(m => m.checklevel == "段级" || m.checklevel == "段以上").ToList().Count();

                r.one = one;
                r.two = two;
                r.three = three;
            }

            return grouplist;
        }


        //导出自控率列表
        public System.Data.DataTable ExportcombineCheckList(int month, int year, long sectorid)
        {
            var products = new System.Data.DataTable("积分查询表");


            var alllist = CombineCheck(year, month, sectorid);

            if (products != null)
            {
                products.Columns.Add("车队", typeof(string));
                products.Columns.Add("班组", typeof(string));
                products.Columns.Add("班组自查问题件数", typeof(string));
                products.Columns.Add("车队检查问题件数", typeof(string));
                products.Columns.Add("段及以上检查问题件数", typeof(string));

                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(
                        item.groupname,
                        item.teamname,
                        item.one,
                        item.two,
                        item.three
                        );
                    i++;
                }
            }

            return products;
        }

    }
}
