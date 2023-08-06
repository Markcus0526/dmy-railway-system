using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLSite.Models.Library;
using System.Collections.Specialized;

namespace TLSite.Models
{
    public class CreditModel
    {
        TieluDBDataContext db = new TieluDBDataContext();

      //获取用户列表
      public List<UserDetailInfo> GetUserList()
        {
          //  List<UserDetailInfo> retlist = new List<UserDetailInfo>();
            var rst = db.tbl_users
                .Where(m => m.deleted == 0 && m.userkind == 2)
                .Join(db.tbl_crewroles, m2 => m2.crewrole, l => l.uid, (m2, l) => new { user = m2, role = l })
                .Join(db.tbl_traingroups, m3 => m3.user.crewgroupid, l => l.uid, (m3, l) => new { user = m3, group = l })
                .Join(db.tbl_railteams, m4 => m4.group.teamid, l => l.uid, (m4, l) => new { user = m4, team = l })                
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
                    teamname=m.team.teamname,
                    //积分集合
                })
                .ToList();
            return rst;
        }

      //查询积分 条件搜索
      public List<UserDetailInfo> GetCredInfoByCondition(long secoterid,long groupid,int year,int month,string name)
      {
                List<UserDetailInfo> rst = null;
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
                   groupid=m.user.user.user.crewgroupid,
                   
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

          var cloglist = db.tbl_checklogs.Where(m => m.deleted == 0&&m.logtype.Contains("两违")).ToList();
          var cinfolist = db.tbl_checkinfos.Where(m => m.checktype==1).ToList();
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

              if (rst2.Count!=0)
              {
              
               // typeone: 段级&段以上 批评教育
                 r.typeone = rst2.Where(m => m.checkpoint <= 9)
                              .Where(m => m.checklevel == "段级" || m.checklevel=="段以上")
                              .Select(m => m.checkpoint).ToList().Sum();



                  //车队  批评教育  50%
                  var typetwoA = rst2.Where(m => m.checkpoint <= 9)
                                  .Where(m =>m.checklevel == "车队")
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
                                  .Where(m => m.checklevel == "段级"||m.checklevel =="段以上")
                                  .Select(m => m.checkpoint).ToList().Sum();


              r.typetwo = typetwoA + typetwoB ;
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
                         .Where(m=>m.checkeinfo.relpoint != null)
                         .Where(m=>double.Parse(m.checkeinfo.relpoint)!=0)
                         .Select(m => new ChecklogModel
                         {
                           checklevel=m.checklogs.checklevel,
                           relatedpoint = Math.Round(double.Parse(m.checkeinfo.relpoint), 1, MidpointRounding.AwayFromZero),
                           checkpoint=m.checkeinfo.chkpoint
                         }
                           ).ToList();


                  if (rst3.Count != 0)
               {
                //段&段以上扣的 联挂分按原分算
                   var b=rst3.Where(m => m.checklevel == "段级" || m.checklevel == "段以上").Select(m => m.relatedpoint).ToList().Sum();
                //车队扣的      1-9 批评教育，10-20连挂考核  联挂分按50%算
                   var c=rst3.Where(m => m.checklevel == "车队"&& m.checkpoint>0&&m.checkpoint<=20).Select(m => Math.Round((m.relatedpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                //车队扣的      50\60 离岗问题  联挂分按100%算
                   var d = rst3.Where(m => m.checklevel == "车队")
                       .Where(m=> m.checkpoint == 50 || m.checkpoint ==60)
                       .Select(m => Math.Round((m.relatedpoint), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                
                      
                //班级扣的      组内自查联挂分为0
                   r.typethree=a + b + c+ d;
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
      }


        /*计算总分！
         *递归函数 
         */ 
      public double conttotalCredit(long uid,int month,int year )
      {
          int typeone;
          double typetwo;
          double typethree;
          int typefour;
          int typefive;
          var checkloglist = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("两违")).ToList();
          var checkinfolist = db.tbl_checkinfos.Where(m => m.checktype == 1).ToList();
          var totaladdcreditlist = db.tbl_checkinfos.Where(m => m.checktype == 0).ToList();
          //本月加的总分
          int addcredit;
          var addcreditlist = checkloglist.Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
                           .Join(totaladdcreditlist, m => m.infoid, l => l.uid, (m, l) => new { log = m, info = l })
                           .Select(m => m.info.chkpoint).ToList();
          addcredit = addcreditlist.Sum();

          long crewroleid = db.tbl_users.Where(m => m.uid == uid).Select(m => m.crewrole).ToList()[0];

          var rst2 = checkloglist
                        .Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
                        .Join(checkinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                        .Select(m => new ChecklogModel
                        {
                            checkpoint = m.checkeinfo.chkpoint,
                            checklevel = m.checklogs.checklevel,
                       //     relatedpoint = Math.Round(float.Parse(m.checkeinfo.relpoint),1),
                        }
                        ).ToList();



              /*
               * typeone: 段级&段以上 批评教育
               * typetwo: 班组&车队   批评教育  
               * typethree:           联挂考核  
               * typefour:            直接离岗培训  
               * typefive:            调离岗位  
               */
                
               // typeone: 段级&段以上 批评教育
                  typeone = rst2.Where(m => m.checkpoint <= 9)
                              .Where(m => m.checklevel == "段级" || m.checklevel=="段以上")
                              .Select(m => m.checkpoint).ToList().Sum();



                  //车队  批评教育  50%
                  var typetwoA = rst2.Where(m => m.checkpoint <= 9)
                                  .Where(m =>m.checklevel == "车队")
                                  .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
              

                
                  //班组  批评教育  25%
                  var typetwoB = rst2.Where(m => m.checkpoint <= 9)
                                  .Where(m => m.checklevel == "班组")
                                  .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();

                  //车队  联挂考核  50%
                  var typethreeA = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                  .Where(m => m.checklevel == "车队")
                                  .Select(m => Math.Round((m.checkpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
              
                  //班组  联挂考核  25%
                  var typethreeB = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                  .Where(m => m.checklevel == "班组")
                                  .Select(m => Math.Round((m.checkpoint * 0.25), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                 
              //段级  联挂考核  100%
              var typethreeC = rst2.Where(m => m.checkpoint >= 10 && m.checkpoint <= 20)
                                  .Where(m => m.checklevel == "段级"||m.checklevel =="段以上")
                                  .Select(m => m.checkpoint).ToList().Sum();


               typetwo = typetwoA + typetwoB ;



              //检查联挂积分，
              //先判断是不是列车长，列车张的积分由两部分组成
              if (crewroleid == 2)
              {
                  //自己给自己扣的分
                  var a = typethreeA + typethreeB+typethreeC;

                //从checklog表中选择其他人给该列车长扣分的数据  
                  var rst3 = checkloglist
                         .Where(m => m.checktime.Year == year && m.checktime.Month == month && m.crewrelid == uid)
                         .Join(checkinfolist, m => m.infoid, l => l.uid, (m, l) => new { checklogs = m, checkeinfo = l })
                         .Where(m => m.checkeinfo.relpoint != null)
                         .Where(m => double.Parse(m.checkeinfo.relpoint) != 0)
                         .Select(m => new ChecklogModel
                         {
                           checklevel=m.checklogs.checklevel,
                           relatedpoint = Math.Round(double.Parse(m.checkeinfo.relpoint), 1, MidpointRounding.AwayFromZero),
                           checkpoint=m.checkeinfo.chkpoint
                         }
                           ).ToList();


                  if (rst3.Count != 0)
               {
                //段&段以上扣的 联挂分按原分算
                   var b=rst3.Where(m => m.checklevel == "段级" || m.checklevel == "段以上")
                             .Select(m => m.relatedpoint).ToList().Sum();
                //车队扣的      1-9 批评教育，10-20连挂考核  联挂分按50%算
                   var c=rst3.Where(m => m.checklevel == "车队"&& m.checkpoint>0&&m.checkpoint<=20)
                             .Select(m => Math.Round((m.relatedpoint * 0.5), 1, MidpointRounding.AwayFromZero)).ToList().Sum();
                //车队扣的      50\60 离岗问题  联挂分按100%算
                   var d = rst3.Where(m => m.checklevel == "车队")
                       .Where(m=> m.checkpoint == 50 || m.checkpoint ==60)
                       .Select(m => m.relatedpoint).ToList().Sum();
                
                      
                //班级扣的      组内自查联挂分为0
                    typethree=a + b + c+d;
               }

                  else
                  {
                       typethree = a;
                  }
              
             }
                  //不是列车长
              else
              {

                  typethree = typethreeA + typethreeB + typethreeC;

              }
        
              typefour = rst2.Where(m => m.checkpoint == 50).ToList()
                           .Select(m => m.checkpoint).ToList().Sum(); 
              typefive = rst2.Where(m => m.checkpoint == 60).ToList()
                           .Select(m => m.checkpoint).ToList().Sum();


          double CurrentCredit;

         

          if (month==1||month==4||month==7||month==10)
          {
              if (typefour != 0 || typefive != 0)
              {
                  CurrentCredit = typetwo + typethree + typefour + typefive ;
                  return CurrentCredit;
              }
              else
              {
                  CurrentCredit = typetwo + typethree + typefour + typefive - addcredit;
                  if (CurrentCredit<0)
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
                  CurrentCredit = typetwo + typethree + typefour + typefive + conttotalCredit(uid,month - 1, year);
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



      /*
       * 点击积分列表最右侧的按钮，显示详细内容
       * @uid:按钮对应的表user.uid
       * @month：月份参数
       * @year：年参数
       * 
       * 可修改联挂扣分的姓名 是显示列车张，还是显示原扣分乘务员
       * 修改请搜索： “按需求更改”
       */
      public  List<UserCreditDetail> getUserCreditByUid(long uid,int month,int year ){

        var rst = new List<UserCreditDetail>();
        var rst1 = new List<UserCreditDetail>();

        //首先判断是不是列车长
        var crewroleid= db.tbl_users
                      .Where(m=>m.uid==uid)
                      .Select(m=>m.crewrole)
                      .ToList();
        var checklogtbl = db.tbl_checklogs.Where(m => m.deleted == 0 && m.logtype.Contains("两违")).ToList();
        var checkinftbl = db.tbl_checkinfos.Where(m => m.checktype == 1).ToList();
          //列车长
       if(crewroleid[0]==2 ){
           //自己扣的分
           rst1 = checklogtbl.Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
               .Join(checkinftbl, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
               .Join(db.tbl_users,m=>m.checklog.crewid,l=>l.uid,(m,l)=>new{checkinfo=m,user=l })
               .Join(db.tbl_traingroups,m=>m.user.crewgroupid,l=>l.uid,(m,l)=>new{user=m,group=l })
               .Join(db.tbl_railteams,m=>m.group.teamid,l=>l.uid,(m,l)=>new{group=m,team=l })
               .Join(db.tbl_crewroles,m=>m.group.user.user.crewrole,l=>l.uid,(m,l)=>new{team=m,role=l })
               .OrderBy(m=>m.team.group.user.checkinfo.checklog.checklevel)
               .Select(m=> new UserCreditDetail
               {
                   teamname=m.team.team.teamname,
                   crewno=m.team.group.user.user.crewno,
                   name=m.team.group.user.user.realname,
                   groupname=m.team.group.group.groupname,
                   crewrolename=m.role.rolename,
                   checkno=m.team.group.user.checkinfo.checkinfo.checkno,
                   //分数
                   chkpoint=m.team.group.user.checkinfo.checkinfo.chkpoint,
                   content=m.team.group.user.checkinfo.checklog.contents,
                   checkexecparentname=m.team.group.user.checkinfo.checklog.checkexecparent,
                   checkername = m.team.group.user.checkinfo.checklog.checkername,
                   checktime=m.team.group.user.checkinfo.checklog.checktime,
                   month=month,
                   checklevel=m.team.group.user.checkinfo.checklog.checklevel,
               }              
               ).ToList();
           //计算分数
           foreach (var r in rst1 )
           {
               //段以上  按原分

               //班组
               if (r.checklevel=="班组")
               {
                   //批评教育&&联挂考核 25%
                   if (r.chkpoint<=20&&r.chkpoint>0)
                   {
                       r.chkpoint=Math.Round((r.chkpoint*0.25), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值                   
               }

               //车队
               if (r.checklevel=="车队")
               {
                    //批评教育&&联挂考核 50%
                   if (r.chkpoint<=20&&r.chkpoint>0)
                   {
                       r.chkpoint=Math.Round((r.chkpoint*0.5), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值      
               }
           }





            //别人扣的分
           var rst2 = checklogtbl.Where(m => m.crewrelid == uid && m.checktime.Year == year && m.checktime.Month == month)
                           .Join(checkinftbl, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })


                            //“按需求更改”：更改显示扣分者的姓名，
                            //如果显示扣分人， m.checklog.crewid,  用crewid找名字
                            //如果显示被扣分人， m.checklog.crewrelid,  用crewrelid找名字
                           .Join(db.tbl_users, m => m.checklog.crewid, l => l.uid, (m, l) => new { checkinfo = m, user = l })
                           .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                           .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                           .Join(db.tbl_crewroles, m => m.group.user.user.crewrole, l => l.uid, (m, l) => new { team = m, role = l })
                           .Where(m => m.team.group.user.checkinfo.checkinfo.relpoint != null)
                           .Where(m => m.team.group.user.checkinfo.checkinfo.relpoint != "0" )
                           .OrderBy(m => m.team.group.user.checkinfo.checklog.checklevel)
                           .Select(m => new UserCreditDetail
               {
                   teamname = m.team.team.teamname,
                   crewno = m.team.group.user.user.crewno,
                   name = m.team.group.user.user.realname,
                   groupname = m.team.group.group.groupname,
                   crewrolename = m.role.rolename,
                   checkno = m.team.group.user.checkinfo.checkinfo.checkno,
                   //分数
                   chkpoint = m.team.group.user.checkinfo.checkinfo.chkpoint,
                   //需要转换
                   relpoint = Math.Round((double.Parse(m.team.group.user.checkinfo.checkinfo.relpoint)), 1, MidpointRounding.AwayFromZero),
                   content = m.team.group.user.checkinfo.checklog.contents,
                   checkexecparentname = m.team.group.user.checkinfo.checklog.checkexecparent,
                   checkername = m.team.group.user.checkinfo.checklog.checkername,
                   checktime = m.team.group.user.checkinfo.checklog.checktime,
                   month = month,
                   checklevel = m.team.group.user.checkinfo.checklog.checklevel,
               }
               ).ToList();

           //计算分数
           foreach (var r in rst2)
           {
               //段以上  按原分
               if (r.checklevel == "段以上" || r.checklevel == "段级")
               {
                   r.chkpoint = r.relpoint;
               }
               //班组
               if (r.checklevel == "班组")
               {
                   //批评教育&&联挂考核 25%
                   if (r.chkpoint <= 20 && r.chkpoint > 0)
                   {
                       r.chkpoint = Math.Round((r.relpoint * 0.25), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值     
                   else
                   {
                       r.chkpoint = r.relpoint;

                   }
               }

               //车队
               if (r.checklevel == "车队")
               {
                   //批评教育&&联挂考核 50%
                   if (r.chkpoint <= 20 && r.chkpoint > 0)
                   {
                       r.chkpoint = Math.Round((r.relpoint * 0.5), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值  
                   else
                   {
                       r.chkpoint = r.relpoint;
                   }
               }
           }


           //激励加分 rst3（add cresit list）
           var rst3 = checklogtbl.Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
                            .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
                            .Where(m => m.checkinfo.checktype == 0)
                           .Join(db.tbl_users, m => m.checklog.crewid, l => l.uid, (m, l) => new { checkinfo = m, user = l })
                           .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                           .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                           .Join(db.tbl_crewroles, m => m.group.user.user.crewrole, l => l.uid, (m, l) => new { team = m, role = l })
                           .OrderBy(m => m.team.group.user.checkinfo.checklog.checklevel)
                           .Select(m => new UserCreditDetail
               {
                   teamname = m.team.team.teamname,
                   crewno = m.team.group.user.user.crewno,
                   name = m.team.group.user.user.realname,
                   groupname = m.team.group.group.groupname,
                   crewrolename = m.role.rolename,
                   checkno = m.team.group.user.checkinfo.checkinfo.checkno,
                   //分数
                   addpoint = m.team.group.user.checkinfo.checkinfo.chkpoint,
                   content = m.team.group.user.checkinfo.checklog.contents,
                   checkexecparentname = m.team.group.user.checkinfo.checklog.checkexecparent,
                   checkername = m.team.group.user.checkinfo.checklog.checkername,
                   checktime = m.team.group.user.checkinfo.checklog.checktime,
                   month = month,
                   checklevel = m.team.group.user.checkinfo.checklog.checklevel,
               }
               ).ToList();

           //合并3项内容
           rst = rst1.Concat(rst2).Concat(rst3).ToList();
       }
      //不是列车长
       else{
           rst1 = checklogtbl.Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
               .Join(checkinftbl, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
               .Join(db.tbl_users,m=>m.checklog.crewid,l=>l.uid,(m,l)=>new{checkinfo=m,user=l })
               .Join(db.tbl_traingroups,m=>m.user.crewgroupid,l=>l.uid,(m,l)=>new{user=m,group=l })
               .Join(db.tbl_railteams,m=>m.group.teamid,l=>l.uid,(m,l)=>new{group=m,team=l })
               .Join(db.tbl_crewroles,m=>m.group.user.user.crewrole,l=>l.uid,(m,l)=>new{team=m,role=l })
               .OrderBy(m => m.team.group.user.checkinfo.checklog.checklevel)
               .Select(m=> new UserCreditDetail
               {
                   teamname=m.team.team.teamname,
                   crewno=m.team.group.user.user.crewno,
                   name=m.team.group.user.user.realname,
                   groupname=m.team.group.group.groupname,
                   crewrolename=m.role.rolename,
                   checkno=m.team.group.user.checkinfo.checkinfo.checkno,
                   //分数
                   chkpoint=m.team.group.user.checkinfo.checkinfo.chkpoint,
                   content=m.team.group.user.checkinfo.checklog.contents,
                   checkexecparentname=m.team.group.user.checkinfo.checklog.checkexecparent,
                   checkername = m.team.group.user.checkinfo.checklog.checkername,
                   checktime=m.team.group.user.checkinfo.checklog.checktime,
                   month=month,
                   checklevel=m.team.group.user.checkinfo.checklog.checklevel,
               }              
               ).ToList();
           //计算分数
           foreach (var r in rst1 )
           {
               //段以上  按原分
             
               //班组
               if (r.checklevel=="班组")
               {
                   //批评教育&&联挂考核 25%
                   if (r.chkpoint<=20&&r.chkpoint>0)
                   {
                       r.chkpoint=Math.Round((r.chkpoint*0.25), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值                   
               }

               //车队
               if (r.checklevel=="车队")
               {
                    //批评教育&&联挂考核 50%
                   if (r.chkpoint<=20&&r.chkpoint>0)
                   {
                       r.chkpoint=Math.Round((r.chkpoint*0.5), 1, MidpointRounding.AwayFromZero);
                   }
                   //离岗问题 按原来分值      
               }
           }


           //激励加分 rst3（add cresit list）
           var rst3 = checklogtbl.Where(m => m.crewid == uid && m.checktime.Year == year && m.checktime.Month == month)
                            .Join(db.tbl_checkinfos, m => m.infoid, l => l.uid, (m, l) => new { checklog = m, checkinfo = l })
                            .Where(m => m.checkinfo.checktype == 0)
                           .Join(db.tbl_users, m => m.checklog.crewid, l => l.uid, (m, l) => new { checkinfo = m, user = l })
                           .Join(db.tbl_traingroups, m => m.user.crewgroupid, l => l.uid, (m, l) => new { user = m, group = l })
                           .Join(db.tbl_railteams, m => m.group.teamid, l => l.uid, (m, l) => new { group = m, team = l })
                           .Join(db.tbl_crewroles, m => m.group.user.user.crewrole, l => l.uid, (m, l) => new { team = m, role = l })
                           .OrderBy(m => m.team.group.user.checkinfo.checklog.checklevel)
                           .Select(m => new UserCreditDetail
                           {
                               teamname = m.team.team.teamname,
                               crewno = m.team.group.user.user.crewno,
                               name = m.team.group.user.user.realname,
                               groupname = m.team.group.group.groupname,
                               crewrolename = m.role.rolename,
                               checkno = m.team.group.user.checkinfo.checkinfo.checkno,
                               //分数
                               addpoint = m.team.group.user.checkinfo.checkinfo.chkpoint,
                               content = m.team.group.user.checkinfo.checklog.contents,
                               checkexecparentname = m.team.group.user.checkinfo.checklog.checkexecparent,
                               checkername = m.team.group.user.checkinfo.checklog.checkername,
                               checktime = m.team.group.user.checkinfo.checklog.checktime,
                               month = month,
                               checklevel = m.team.group.user.checkinfo.checklog.checklevel,
                           }
               ).ToList();
           rst = rst1.Concat(rst3).ToList();


       }

        return rst;

     

    }
    
       public class UserCreditDetail{


           public string teamname{set;get;} 
           public string crewno{set;get;} 
           public string name{set;get;} 
           public string groupname{set;get;} 
           public string crewrolename{set;get;} 
           public long checkinfoid{set;get;} 
           public double chkpoint{set;get;}
           public int addpoint { set; get; } 
           public string content{set;get;} 
           public string checkexecparentname{set;get;}
           public string checkername{set;get;}
           public DateTime checktime { set; get; } 
           public int month{set;get;}
           public string checklevel { set; get; }
           public double relpoint { set; get; } 
           public long groupid{set;get;} 
           public long crewroelid{set;get;} 
           public string checkno{set;get;} 
           
        }


        //导出积分用
       public System.Data.DataTable ExportCreditList(long secoterid,long groupid, int year, int month, string name)
       {
           var products = new System.Data.DataTable("积分查询表"); 


           var alllist = GetCredInfoByCondition(secoterid,groupid, year, month, name);

           if (products != null)
           {
               products.Columns.Add("车队", typeof(string));
               products.Columns.Add("工资号", typeof(string));
               products.Columns.Add("姓名", typeof(string));
               products.Columns.Add("班组", typeof(string));
               products.Columns.Add("职名", typeof(string));
               products.Columns.Add("段及以上批评教育积分", typeof(string));
               products.Columns.Add("班组车队检查批评教育积分", typeof(string));
               products.Columns.Add("联挂考核积分", typeof(string));
               products.Columns.Add("直接离岗培训积分", typeof(string));
               products.Columns.Add("直接调整工作岗位积分", typeof(string));
               products.Columns.Add("本月积分", typeof(string));
               products.Columns.Add("激励积分", typeof(string));
               products.Columns.Add("累计积分", typeof(string));


               int i = 1;
               foreach (var item in alllist)
               {
                   products.Rows.Add(
                       
                       item.teamname,
                       item.crewno,
                       item.realname,
                       item.crewgroupname,
                       item.crewrole,
                       item.typeone,
                       item.typetwo,
                       item.typethree,
                       item.typefour,
                       item.typefive,
                       item.typetwo + item.typethree + item.typefour + item.typefive,
                       item.addcredit,
                       conttotalCredit(item.uid,month,year)
                       );
                   i++;
               }
           }

           return products;
       }

        //导出个人详细积分用
       public System.Data.DataTable ExportCreditDetilList(long uid, int month, int year, string totlechkpoint, string totleaddpoint)
       {
           var products = new System.Data.DataTable("积分查询表");


           var alllist = getUserCreditByUid(uid, month, year );

           if (products != null)
           {
               products.Columns.Add("车队", typeof(string));
               products.Columns.Add("工资号", typeof(string));
               products.Columns.Add("姓名", typeof(string));
               products.Columns.Add("班组", typeof(string));
               products.Columns.Add("职名", typeof(string));
               products.Columns.Add("两违问题积分", typeof(string));
               products.Columns.Add("激励积分", typeof(string));
               products.Columns.Add("问题内容", typeof(string));
               products.Columns.Add("监察部门", typeof(string));
               products.Columns.Add("检查人", typeof(string));
               products.Columns.Add("检查级别", typeof(string));
               products.Columns.Add("检查时间", typeof(string));
               products.Columns.Add("所属月份", typeof(string));


               int i = 1;
               foreach (var item in alllist)
               {
                   products.Rows.Add(
                       item.teamname,
                       item.crewno,
                       item.name,
                       item.groupname,
                       item.crewrolename,
                       item.chkpoint,
                       item.addpoint,
                       item.content,
                       item.checkexecparentname,
                       item.checkername,
                       item.checklevel,
                       item.checktime,
                       month+"月"
                       );
                   i++;
               }
               string[] total = new string[] { "本月积分合计", "", "", "", "", totlechkpoint, totleaddpoint, "", "", "", "", "", "" };
               products.Rows.Add(total);
           }

           return products;
       }
    }

     
}