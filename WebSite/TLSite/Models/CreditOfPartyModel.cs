using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TLSite.Models
{
    public class CreditOfPartyModel
    {
        //
        // GET: /ConbineCheckModel/

        TieluDBDataContext db = new TieluDBDataContext();


        public class forStore
        {
            public string teamname { set; get; }
            public string crewno { set; get; }
            public long uid { set; get; }
            public string name { set; get; }
            public string groupname { set; get; }
            public string rolename { set; get; }
            public string polityface { set; get; }
            public string trainno { set; get; }
            public long checkinfoid { set; get; }
            public double credit { set; get; }
            public string content { set; get; }
            public string checkinfo { set; get; }
            public string checkername { set; get; }
            public string checklevel { set; get; }
            public long checkerid { set; get; }
            public double relpoint { set; get; }
            public int month { set; get; }
            public string checkinfono { set; get; }
            public string checkerexecparent { set; get; }
        }



        public List<forStore> partylist(int year, int month, long uid, string checklevel,long teamid,string policy)
        {

           // var crewrole = db.tbl_users.Where(m => m.uid == uid).Select(m => m.crewrole).ToList()[0];
           // var rst = new List<forStore>();
            var  list= 
             db.tbl_checklogs.Where(m => m.deleted == 0　&&m.logtype.Contains("两违") )
                             .Join(db.tbl_users, m => m.crewid, l => l.uid, (m, l) => new { checklog = m, user = l })
                             .Join(db.tbl_crewroles, m => m.user.crewrole, l => l.uid, (m, l) => new { checklog = m, crewrole = l })
                                             .Join(db.tbl_trainnos, m => m.checklog.checklog.trainno, l => l.uid, (m, l) => new { crewrole = m, trainno = l })
                                             .Join(db.tbl_traingroups, m => m.crewrole.checklog.checklog.groupid, l => l.uid, (m, l) => new { trainno = m, group = l })
                                             .Join(db.tbl_checkinfos, m => m.trainno.crewrole.checklog.checklog.infoid, l => l.uid, (m, l) => new { group = m, info = l })
                                             .Where(m=>m.info.checktype==1)
                                             .Join(db.tbl_railteams, m => m.group.trainno.crewrole.checklog.checklog.teamid, l => l.uid, (m, l) => new { info = m, team = l })
                                             .OrderBy(m => m.info.group.trainno.crewrole.checklog.checklog.createtime)
                                             .Where(m =>  m.info.group.trainno.crewrole.checklog.checklog.createtime.Year == year && m.info.group.trainno.crewrole.checklog.checklog.createtime.Month == month&&m.team.uid==teamid)
                                             .Select(m => new forStore
                                             {

                                                 //  groupid = m.checklog.checklog.checklog.checklog.groupid,  
                                                 uid=m.info.group.trainno.crewrole.checklog.user.uid,
                                                 teamname = m.team.teamname,
                                                 name = m.info.group.trainno.crewrole.checklog.user.realname,
                                                 groupname = m.info.group.group.groupname,
                                                 rolename = m.info.group.trainno.crewrole.crewrole.rolename,
                                                 polityface = m.info.group.trainno.crewrole.checklog.user.policyface,
                                                 trainno = m.info.group.trainno.trainno.trainno,
                                                 checkinfono = m.info.info.checkno,
                                                 credit = m.info.info.chkpoint,
                                                 content = m.info.group.trainno.crewrole.checklog.checklog.contents,
                                                 checkinfo = m.info.info.checkinfo,
                                                 checkerexecparent = m.info.group.trainno.crewrole.checklog.checklog.checkexecparent,
                                                 checkername = m.info.group.trainno.crewrole.checklog.checklog.checkername,
                                                 checklevel = m.info.group.trainno.crewrole.checklog.checklog.checklevel,
                                                 crewno = m.info.group.trainno.crewrole.checklog.user.crewno
                                                 
                                             })
                                             .ToList();
            if (checklevel!="无")
            {
                list = list.Where(m => m.checklevel == checklevel).ToList();
            } 
            if (uid!=0)
            {
                list = list.Where(m => m.uid == uid).ToList();

            }
            if (policy=="党")
            {
                list = list.Where(m => m.polityface.Contains("党")).ToList();
            }
            if (policy!="党")
            {
                list = list.Where(m => !m.polityface.Contains("党")).ToList();
            }
            //计算分数
            foreach (var r in list)
            {
                //段以上  按原分

                //班组
                if (checklevel == "班组")
                {
                    //批评教育&&联挂考核 25%
                    if (r.credit <= 20 && r.credit > 0)
                    {
                        r.credit = Math.Round((r.credit * 0.25), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值                   
                }

                //车队
                if (checklevel == "车队")
                {
                    //批评教育&&联挂考核 50%
                    if (r.credit <= 20 && r.credit > 0)
                    {
                        r.credit = Math.Round((r.credit * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值      
                }
            }
            //列车长
//             if (crewrole==2)
//             {
//                 //别人扣的list2
//                 var list2=db.tbl_checklogs.Where(m=>m.deleted==0&&m.crewrelid==uid&&m.createtime.Year==year&&m.createtime.Month==month&&m.logtype.Contains("两违"))
//                            .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
//                            .Where(m=>m.checkinfo.checktype==1)
//                             //“按需求更改”：更改显示扣分者的姓名，
//                             //如果显示扣分人， m.checklog.crewid,  用rewid找名字
//                             //如果显示被扣分人， m.checklog.crewrelid,  用rewrelid找名字
//                            .Join(db.tbl_users, m => m.checklog.crewrelid, l => l.uid, (m, l) => new { checkinfo = m, user = l })
//                            .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
//                            .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { group = m, team = l })
//                            .Join(db.tbl_crewroles, m => m.group.user.user.crewrole, l => l.uid, (m, l) => new { team = m, role = l })
//                            .Join(db.tbl_trainnos,m=>m.team.group.user.checkinfo.checklog.trainno,l => l.uid, (m, l) => new { role = m, trainno = l })
//                            .Where(m => m.role.team.group.user.checkinfo.checkinfo.relpoint != "0" )
//                            .OrderBy(m => m.role.team.group.user.checkinfo.checklog.checklevel)
//                            .Select(m => new forStore
//                                        {
//                                            teamname = m.role.team.team.teamname,
//                                            name = m.role.team.group.user.user.realname,
//                                            groupname = m.role.team.group.group.groupname,
//                                            rolename = m.role.role.rolename,
//                                            polityface = m.role.team.group.user.user.policyface,
//                                            trainno=m.trainno.trainno,
//                                            checkinfoid = m.role.team.group.user.checkinfo.checkinfo.uid,
//                                            //分数
//                                            credit = m.role.team.group.user.checkinfo.checkinfo.chkpoint,
//                                            relpoint = Math.Round((double.Parse(m.role.team.group.user.checkinfo.checkinfo.relpoint)), 1, MidpointRounding.AwayFromZero),
//                                            content = m.role.team.group.user.checkinfo.checklog.contents,
//                                            checkinfo = m.role.team.group.user.checkinfo.checkinfo.checkinfo,
//                                            checkername =m.role.team.group.user.checkinfo.checklog.checkername,
//                   
//                                        }
//                             ).ToList();
// 
//                 //计算分数
//                 foreach (var r in list2)
//                 {
//                     //段以上  按原分
//                     if (checklevel == "段以上" || checklevel == "段级")
//                     {
//                         r.credit = r.relpoint;
//                     }
//                     //班组
//                     if (checklevel == "班组")
//                     {
//                         //批评教育&&联挂考核 25%
//                         if (r.credit <= 20 && r.credit > 0)
//                         {
//                             r.credit = Math.Round((r.relpoint * 0.25), 1, MidpointRounding.AwayFromZero);
//                         }
//                         //离岗问题 按原来分值     
//                         else
//                         {
//                             r.credit = r.relpoint;
// 
//                         }
//                     }
// 
//                     //车队
//                     if (checklevel == "车队")
//                     {
//                         //批评教育&&联挂考核 50%
//                         if (r.credit <= 20 && r.credit > 0)
//                         {
//                             r.credit = Math.Round((r.relpoint * 0.5), 1, MidpointRounding.AwayFromZero);
//                         }
//                         //离岗问题 按原来分值  
//                         else
//                         {
//                             r.credit = r.relpoint;
//                         }
//                     }
//                 }
//                  rst = list.Concat(list2).ToList();
// 
//             }
// 
//             else
//             {
//                  rst = list;
//             }

            return list;
        }



        //导出用
        public System.Data.DataTable ExportCreditOfPartyList(int year, int month, long uid,string checklevel, long teamid,string policy)
        {
            var products = new System.Data.DataTable("党内两违比率查询");


            var alllist = partylist(year, month, uid, checklevel,teamid,policy);

            if (products != null)
            {
                products.Columns.Add("车队", typeof(string));
                products.Columns.Add("工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("班组", typeof(string));
                products.Columns.Add("职名", typeof(string));
                products.Columns.Add("政治面貌", typeof(string));
                products.Columns.Add("车次", typeof(string));
                products.Columns.Add("项点编号", typeof(string));
                products.Columns.Add("积分", typeof(string));
                products.Columns.Add("问题内容", typeof(string));
                products.Columns.Add("检查部门", typeof(string));
                products.Columns.Add("检查人", typeof(string));
                products.Columns.Add("所属月份", typeof(string));


                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(

                        item.teamname,
                        item.crewno,
                        item.name,
                        item.groupname,
                        item.rolename,
                        item.polityface,
                        item.trainno,
                        item.checkinfono,
                        item.credit,
                        item.content,
                        item.checkerexecparent,
                        item.checkername,
                        month+"月"
                        );
                    i++;
                }
            }

            return products;
        }
    }
}
