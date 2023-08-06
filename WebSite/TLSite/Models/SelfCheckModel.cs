using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TLSite.Models.Library
{
    public class SelfCheckModel
    {

        TieluDBDataContext db = new TieluDBDataContext();


         public class forStore{

   

             public int checkpoint{set;get;}

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
             public int eight   { set; get; }

         }

        //根据年、月、部门类型 查自控率
         public List<grouplist> selfcheck(int year, int month, long sectorid)
         {
             var grouplist = db.tbl_traingroups.Where(m => m.deleted == 0 && m.teamid == sectorid)
                 .Join(db.tbl_railteams, m => m.teamid, l => l.uid, (m, l) => new {group=m,team=l })
                 .Select(m => new grouplist
                 {
                     groupid = m.group.uid,
                     groupname = m.group.groupname,
                     teamname=m.team.teamname
                 })
                 .ToList();

             foreach (var r in grouplist)
             {
                 var rst = db.tbl_checklogs
                .Where(m => m.deleted == 0  && m.logtype.Contains("两违")&&m.groupid == r.groupid && m.createtime.Year == year && m.createtime.Month == month )
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                .Where(m=>m.checkeinfo.checktype==1)
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


         //导出自控率列表
         public System.Data.DataTable ExportSelfCheckList( int month, int year, long sectorid)
         {
             var products = new System.Data.DataTable("积分查询表");


             var alllist = selfcheck(year, month, sectorid);

             if (products != null)
             {
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 products.Columns.Add("", typeof(string));
                 
                 string[] head1 = new string[] { "车队", "班组", "批评教育问题（1-6分）", "批评教育问题（7-9分）", "联挂考核问题", "离岗培训及以上问题" };
                 string[] head2 = new string[] { "班组自查", "班组以上检查", "班组自查", "班组以上检查", "班组自查", "班组以上检查", "班组自查", "班组以上检查" };
                 products.Rows.Add(head1);
                 products.Rows.Add(head2);
                 int i = 1;
                 foreach (var item in alllist)
                 {
                     products.Rows.Add(
                         item.teamname,
                         item.groupname,
                         item.one,
                         item.two,
                         item.three,
                         item.four,
                         item.five,
                         item.six,
                         item.seven,
                         item.eight
                         );
                     i++;
                 }
             }

             return products;
         }
    }
}
