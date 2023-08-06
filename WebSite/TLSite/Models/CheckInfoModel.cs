using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLSite.Models
{

    public class CheckCheckInfoTable{

        public string teamname{get;set;}
        public string crewno{get;set;}
        public string name{get;set;}
        public string groupname{get;set;}
        public string rolename{get;set;}
        public string trainno{get;set;}
        public string checkno{get;set;}
        public double chkpoint { get; set; }
        public string content{get;set;}
        public string checkersector{get;set;}
        public string checkername{get;set;}
        public DateTime checktime { get; set; }
        public long sectorid { get; set; }
        public string checklevel { get; set; }
        public long uid { get; set; }
        public long groupid { get; set; }
        public long trainnoid { get; set; }

    }


    public class CheckInfoModel
    {

        TieluDBDataContext db = new TieluDBDataContext(CommonModel.ConnectionString);

        //在checklog表中得到检查部门信息，让前台显示
        public List<string> getCheckerSector()
        {
            var rst =
            db.tbl_checklogs.Where(m => m.deleted ==0&&m.logtype=="两违考核"&&m.checklevel!="激励加分").Select(m => m.checkexecparent).ToList();

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

        //在checklog表中得到检查人信息，让前台显示
        public List<string> getCheckerName()
        {
            var rst =
            db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype == "两违考核" && m.checklevel != "激励加分").Select(m => m.checkername).ToList();

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
        public List<CheckCheckInfoTable> serchbycondition(string checkername, string checkersector, long sectorid, string checklevel, string checkno, string trainno, string keyword, int chkpoint,string groupid, long uid, string starttime,string endtime)
        {
            var cloglist = db.tbl_checklogs.Where(m => m.deleted == 0&&m.logtype.Contains("两违")).ToList();
            var allchecklog=cloglist
                .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklog=m,checkinfo=l})
                .Join(db.tbl_traingroups, m => m.checklog.groupid, l => l.uid, (m, l) => new { checkinfo = m, group = l })  
                .Join(db.tbl_railteams, m => m.checkinfo.checklog.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                .Join(db.tbl_users, m => m.group.checkinfo.checklog.crewid, l => l.uid, (m, l) => new { team = m, user = l })
                .Join(db.tbl_crewroles, m => m.user.crewrole, l => l.uid, (m, l) => new { user = m, role = l })
               // .Join(db.tbl_trainnos, m => m.user.team.group.checkinfo.checklog.trainno, l => l.uid, (m, l) => new { role = m, trianno = l })
                .Where(m=>m.user.team.group.checkinfo.checkinfo.checktype==1)
                .Select(m => new CheckCheckInfoTable
                {
                    teamname=m.user.team.team.teamname,
                    crewno = m.user.user.crewno,
                    name = m.user.user.realname,
                    groupname=m.user.team.group.group.groupname,
                    rolename=m.role.rolename,
                    trainnoid=m.user.team.group.checkinfo.checklog.trainno,
                    //trainno=m.trianno.trainno,
                    checkno=m.user.team.group.checkinfo.checkinfo.checkno,
                    chkpoint=m.user.team.group.checkinfo.checkinfo.chkpoint,
                    content=m.user.team.group.checkinfo.checklog.contents,
                    checkername = m.user.team.group.checkinfo.checklog.checkername,
                    checkersector = m.user.team.group.checkinfo.checklog.checkexecparent,
                    sectorid=m.user.team.group.checkinfo.checklog.teamid,
                    checklevel=m.user.team.group.checkinfo.checklog.checklevel,
                    checktime =m.user.team.group.checkinfo.checklog.checktime,
                    uid = m.user.user.uid,
                    groupid = m.user.team.group.group.uid,
                }).ToList();
            foreach (var m in allchecklog)
            {
                if (m.trainnoid != null)
                {
                    m.trainno = db.tbl_trainnos.Where(l=> l.uid == m.trainnoid).Select(l=>l.trainno).FirstOrDefault();
                }
            }


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

            if (checkersector != "" && checkersector.Length != 0 && checkersector!="请选择...")
            {
                allchecklog=allchecklog.Where(m => m.checkersector == checkersector).ToList();
            }

            if (checkername != "" && checkername.Length != 0)
            {
                allchecklog = allchecklog.Where(m => m.checkername == checkername).ToList();
            }

            if ( sectorid != 0)
            {
                allchecklog = allchecklog.Where(m => m.sectorid == sectorid).ToList();
            }

            if (checklevel != "" && checklevel.Length != 0)
            {
                
                allchecklog = allchecklog.Where(m => m.checklevel == checklevel).ToList();
            }
            if (checkno != "" && !string.IsNullOrWhiteSpace(checkno))
            {

                allchecklog = allchecklog.Where(m => m.checkno.Contains(checkno.ToUpper())).ToList();
            }

            if (trainno != "" && !string.IsNullOrWhiteSpace(trainno))
            {
                allchecklog = allchecklog.Where(m => m.trainno.Contains(trainno)).ToList();
            }

            if (keyword != "" && !string.IsNullOrWhiteSpace(keyword))
            {
                allchecklog = allchecklog.Where(m => m.content.Contains(keyword) ).ToList();
            }

            if (chkpoint == 1)
            {
                allchecklog = allchecklog.Where(m => m.chkpoint >= 0 && m.chkpoint < 10).ToList();
            }
            else if (chkpoint==2)
            {
                allchecklog = allchecklog.Where(m => m.chkpoint >= 10 && m.chkpoint <= 20).ToList();

            }
            else if (chkpoint == 50)
            {
                allchecklog = allchecklog.Where(m => m.chkpoint==50).ToList();
            }
            else if (chkpoint==60)
            {
                allchecklog = allchecklog.Where(m => m.chkpoint == 60).ToList();
            }

            //计算分数
            foreach (var r in allchecklog)
            {
                //段以上  按原分

                //班组
                if (r.checklevel == "班组")
                {
                    //批评教育&&联挂考核 25%
                    if (r.chkpoint <= 20 && r.chkpoint > 0)
                    {
                        r.chkpoint = Math.Round((r.chkpoint * 0.25), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值                   
                }

                //车队
                if (r.checklevel == "车队")
                {
                    //批评教育&&联挂考核 50%
                    if (r.chkpoint <= 20 && r.chkpoint > 0)
                    {
                        r.chkpoint = Math.Round((r.chkpoint * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                    //离岗问题 按原来分值      
                }
            }
            if (!string.IsNullOrWhiteSpace(starttime))
            {
                try
                {
                   var starttimeF= DateTime.Parse(starttime);
                   allchecklog = allchecklog.Where(m => m.checktime >= starttimeF).ToList();
                }
                catch (System.Exception ex)
                {
                }
            }
            if (!string.IsNullOrWhiteSpace(endtime))
            {
                try
                {
                    var endtimeF = DateTime.Parse(endtime);
                    allchecklog = allchecklog.Where(m => m.checktime <= endtimeF).ToList();
                }
                catch (System.Exception ex)
                {
                }
            }

            if (uid!=0)
            {
                allchecklog = allchecklog.Where(m => m.uid == uid).ToList();
            }

            if (groupid != "" && groupid.Length != 0&&groupid!="请选择...")
            {
                allchecklog = allchecklog.Where(m => m.groupid == int.Parse(groupid)).ToList();
            }
            return allchecklog;
        }


        //导出模型
        //导出积分用
        public System.Data.DataTable ExportCreditList(string checkername, string checkersector, long sectorid, string checklevel, string checkno, string trainno, string keyword, int chkpoint, string groupid, long uid, string starttime, string endtime)
        {
            var products = new System.Data.DataTable("积分查询表");


            var alllist = serchbycondition(checkername, checkersector, sectorid, checklevel, checkno, trainno, keyword, chkpoint, groupid,  uid,  starttime, endtime);

            if (products != null)
            {
                products.Columns.Add("车队", typeof(string));
                products.Columns.Add("责任人工资号", typeof(string));
                products.Columns.Add("姓名", typeof(string));
                products.Columns.Add("班组", typeof(string));
                products.Columns.Add("职名", typeof(string));
                products.Columns.Add("乘务车次", typeof(string));
                products.Columns.Add("项点编码", typeof(string));
                products.Columns.Add("积分", typeof(string));
                products.Columns.Add("问题内容", typeof(string));
                products.Columns.Add("检查部门", typeof(string));
                products.Columns.Add("检查人", typeof(string));
                products.Columns.Add("检查时间", typeof(string));


                int i = 1;
                foreach (var item in alllist)
                {
                    products.Rows.Add(

                        item.teamname,
                        item.crewno,
                        item.name,
                        item.groupname,
                        item.rolename,
                        item.trainno,
                        item.checkno,
                        item.chkpoint,
                        item.content,
                        item.checkersector,
                        item.checkername,
                        item.checktime
                        );
                    i++;
                }
            }

            return products;
        }
    }
}