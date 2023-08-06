<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>

<div class="page-header">
	<h1>
		"两违"考核问题查询
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>

<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
            <div class="form-group" >
				<label class="col-sm-2 control-label no-padding-right" for="starttime">检查起始日期：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
					    <input class="input-large date-picker" name="starttime" id="starttime" type="text"
                        data-date-format="yyyy-mm-dd" <% if (ViewData["starttime"] != null) { %>value="<%= ViewData["starttime"] %>"
                        <% } %> />
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="endtime">结束日期:</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<input class="input-large date-picker" name="endtime" id="endtime" type="text"
                        data-date-format="yyyy-mm-dd" <% if (ViewData["starttime"] != null) { %>value="<%= ViewData["starttime"] %>"
                        <% } %> />
                    </div>
				</div>
			</div>


			<div class="form-group" >
                <label class="col-sm-2 control-label no-padding-right" for="checklevel">检查级别：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
						    <select class="select2" id="checklevel" name="checklevel" data-placeholder="请选择级别" onchange="changecheckersectlist()">
                                <option selected value="">请选择...</option>
                                <option value="段以上">段以上检查</option>
                                <option value="段级">段级检查</option>
                                <option value="车队">车队检查</option>
                                <option value="班组">班组检查</option>
				            </select>
                        </div>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right"  for="checkersector">检查部门：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="checkersector" name="checkersector" data-placeholder="请选择部门" onchange="ChangeCheckernameList()">
                            <option selected value="">请选择...</option>
                           
				        </select>
                    </div>
				</div>
                <label class="col-sm-1 control-label no-padding-right" for="checkernameList">检查人：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="checkernameList" name="checkernameList">
                            <option selected value="">请选择...</option>
                            <% if (ViewData["CheckerName"] != null)
                               {
                                   foreach (var item in (List<String>)ViewData["CheckerName"])
                                   { %>
                                   <option value="<%= item %>" ><%= item %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
			</div>
            <div class="form-group" >
				<label class="col-sm-2 control-label no-padding-right" for="sectorid">受检部门：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="sectorid" name="sectorid" data-placeholder="请选择部门" onchange="changegrouplist()" <% if (userrole != null &&( ((string)userrole).Contains("Crew")||((string)userrole).Contains("TeamExec"))) { %> disabled<% } %> >
                            <option selected value="0">请选择...</option>
                            <% if (ViewData["Sectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["Sectorlist"])
                                   { %>
                                   <option value="<%= item.uid %>"   <%  if ( ViewData["teamid"]!=null&&item.uid== (long)ViewData["teamid"]) { %> selected="selected"<% } %> ><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
                <label class="col-sm-1 control-label no-padding-right" for="groupid">受检班组：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="groupid" name="groupid" data-placeholder="请选择部门" onchange="changeuserlist()">
                            <option selected value="">请选择...</option>
                            <% if (ViewData["Grouplist"] != null)
                               {
                                   foreach (var item in (List<GroupInfo>)ViewData["Grouplist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.groupname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
                <label class="col-sm-1 control-label no-padding-right" for="userlist">受检人：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="userlist" name="userlist" data-placeholder="请选择部门" onchange="">
                            <option selected value="0">请选择...</option>
                            <% if (ViewData["Userlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["Userlist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
			</div>

			<div class="form-group" >
                <label class="col-sm-1 control-label no-padding-right" for="chkpoint">按问题类型：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
							<select class="select2" id="chkpoint">
                            <option selected="selected" value="0">请选择问题类型</option>
                            <option value="60">直接调整岗位问题</option>
                            <option value="50">直接离岗培训问题</option>
                            <option value="2">联挂考核问题</option>
                            <option value="1">批评教育问题</option>
                            </select>
						</div>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="checkno">问题编码：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<input class="input-large " id="checkno" name="checkno" type="text" />	
                    </div>
				</div>

				<label class="col-sm-1 control-label no-padding-right" for="trainno">乘务车次：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<input class="input-large " id="trainno" name="trainno" type="text" />
                    </div>
				</div>
				<label class="col-sm-1  control-label no-padding-right" for="keyword" >问题关键字：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<input class="input-large" id="keyword" name="keyword" type="text" />
                    </div>
				</div>
			</div>
			
            <hr />

              <!--按钮部分-->
            <div style="float:right; margin-right:10px;">                   
					<a class="btn btn-sm btn-info" id="searchdata" onclick="showlist()"><i class="fa fa-search"></i> 统计全部</a>
					<a target="_blank" id="download" class="btn btn-sm btn-info" ><i class="fa fa-download"></i> 导出Excel</a>        
             </div>
             <p>&nbsp</p>
            <h3 class="header smaller lighter green">下方列表显示查询结果</h3>
            

		   <div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th style="width:70px">车队</th>
						<th style="width:80px">责任人</br>工资号</th>
						<th>姓名</th>
						<th>班组</th>
						<th>职名</th>
						<th>乘务车次</th>
						<th>项点编码</th>
                        <th>积分</th>
                        <th style="width:270px">问题内容</th>
                        <th>检查部门</th>
                        <th>检查人</th>
					    <th>检查时间</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>           
        </form>
	</div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
    <script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
    


		<script type="text/javascript">
         jQuery(function ($) {

              $(".select2").css('width', '194px').select2({ allowClear: true })
		        .on('change', function () {
		        });
                
              $(".input-large").css('width','194px');
            $('.date-picker').datepicker({
                autoclose: true,
                todayHighlight: true,
                language: "zh-CN"
            });})

		    var tbl;
		    var alerttable;
        //table部分
        function search() {
            tbl = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "CheckInfo/ShowTable/?sectorid=" + $("#sectorid").val() + "&checkersector=" + escape($("select[name=checkersector]").find("option:selected").text()) + "&checkername=" + escape($("#checkernameList").val()) + "&checklevel=" + escape($("#checklevel").val())+ "&checkno=" + $("#checkno").val()+ "&trainno=" + $("#trainno").val()+ "&keyword=" + escape($("#keyword").val())+ "&chkpoint=" + $("#chkpoint").val() +"&starttime="+$("#starttime").val()+"&endtime="+$("#endtime").val()+"&userid="+$("#userlist").val()+"&groupid="+$("#groupid").val(),
                "bFilter": false,
                "oLanguage": {
                    "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
                },
                bAutoWidth: false,
                "aoColumns": [
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
					],
                "bPaginate": true,
                "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],
                "iDisplayLength": 10,
            });
        }

        function showlist() {
                if (tbl != null) {
                    var oSettings = tbl.fnSettings();
                    oSettings.sAjaxSource = rootUri + "CheckInfo/ShowTable/?sectorid=" + $("#sectorid").val() + "&checkersector=" + escape($("select[name=checkersector]").find("option:selected").text()) + "&checkername=" + escape($("#checkernameList").val()) + "&checklevel=" + escape($("#checklevel").val())+ "&checkno=" + $("#checkno").val()+ "&trainno=" + $("#trainno").val()+ "&keyword=" +escape( $("#keyword").val())+ "&chkpoint=" + $("#chkpoint").val() +"&starttime="+$("#starttime").val()+"&endtime="+$("#endtime").val()+"&userid="+$("#userlist").val()+"&groupid="+$("#groupid").val();
                    tbl.fnClearTable(0);
                    tbl.fnDraw();
                }
                else {
                    search();
                }
           
            //设置导出内容
            $("#download").attr("href",rootUri + "CheckInfo/Export/?sectorid=" + $("#sectorid").val() + "&checkersector=" + escape($("select[name=checkersector]").find("option:selected").text()) + "&checkername=" + escape($("#checkernameList").val()) + "&checklevel=" + escape($("#checklevel").val())+ "&checkno=" + $("#checkno").val()+ "&trainno=" + $("#trainno").val()+ "&keyword=" + escape($("#keyword").val())+ "&chkpoint=" + $("#chkpoint").val()+"&starttime="+$("#starttime").val()+"&endtime="+$("#endtime").val()+"&userid="+$("#userlist").val()+"&groupid="+$("#groupid").val());
       }

     function changegrouplist(){
                  var teamid= $("#sectorid").val();
                  $.ajax({
                            type: "GET",
                            url: rootUri + "CheckInfo/GetgroupList/?teamid=" + teamid,
                            dataType: "json",
                           success: function (data) {
                                var rhtml = "<option value=''>请选择...</option>";
                                if (data.length > 0) {
                                    for (var i = 0; i < data.length; i++) {
                                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                                    }
                                    $("#groupid").html(rhtml);
                                    $("#groupid").css('width', '194px').select2({ allowClear: true });

                                }
                                else{
                                rhtml= "<option value='0'>无可用班组</option>";
                                $("#groupid").html(rhtml);
                                $("#groupid").css('width', '194px').select2({ allowClear: true });

                                }
                                changeuserlist();
                            },
                        });
       }
       function changeuserlist(){
                var groupid= $("#groupid").val();
                if (groupid=="") {
                     var rhtml = "<option value='0'>请选择...</option>";
                     $("#userlist").html(rhtml);
                     $("#userlist").css('width', '194px').select2({ allowClear: true });
                }
                else{
                    $.ajax({
                            type: "GET",
                            url: rootUri + "CheckInfo/GetuserList/?groupid=" + groupid,
                            dataType: "json",
                            success: function (data) {
                                var rhtml = "<option value='0'>请选择...</option>";
                                if (data.length > 0) {
                                    for (var i = 0; i < data.length; i++) {
                                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
                                    }
                                    $("#userlist").html(rhtml);
                                    $("#userlist").css('width', '194px').select2({ allowClear: true });

                                }
                                else{
                                rhtml= "<option value='0'>无可用用户</option>";
                                $("#userlist").html(rhtml);
                                $("#userlist").css('width', '194px').select2({ allowClear: true });

                                }
                            },
                        });
                    }
        }
          function changecheckersectlist(){
          var checklevel=$("#checklevel").val();
          $.ajax({
                    type:"GET",
                    url:rootUri + "CheckInfo/GetCheckersecList/?checklevel=" + escape(checklevel),
                    dataType:"json",
                    success: function (data) {
                            var rhtml = "<option value=''>请选择...</option>";
                            if (data.length > 0) {
                                if (checklevel=="段级") {
                                     for (var i = 0; i < data.length; i++) {
                                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].sectorname + "</option>";
                                        }
                                        $("#checkersector").html(rhtml);
                                        $("#checkersector").css('width', '194px').select2({ allowClear: true });

                                }
                                else if(checklevel=="车队"){
                                    for (var i = 0; i < data.length; i++) {
                                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].teamname + "</option>";
                                        }
                                        $("#checkersector").html(rhtml);
                                        $("#checkersector").css('width', '194px').select2({ allowClear: true });

                                }
                                else if(checklevel=="班组"){
                                    for (var i = 0; i < data.length; i++) {
                                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                                        }
                                        $("#checkersector").html(rhtml);
                                        $("#checkersector").css('width', '194px').select2({ allowClear: true });
                                }
                                else if(checklevel=="段以上"){
                                        var rhtml = "<option selected value=''>请选择...</option>";
                                        $("#checkersector").html(rhtml);
                                        $("#checkersector").css('width', '194px').select2({ allowClear: true });
                                }
                            }
                            else{
                                        var rhtml = "<option selected value=''>请选择...</option>";
                                        $("#checkersector").html(rhtml);
                                        $("#checkersector").css('width', '194px').select2({ allowClear: true });
                            }
                        $("#checkernameList").html("<option selected value=''>请选择...</option>");
                        $("#checkernameList").css('width', '194px').select2({ allowClear: true });

                             },
                 });
          }

//           function changecheckernamelist(){
//           var checklevel=$("#checklevel").val();
//           var sectorid=$("#checkersector").val();
//           $.ajax({
//                     type:"GET",
//                     url:rootUri + "Judge/FindJudgeCheckerList/" + sectorid,
//                     dataType:"json",
//                     data: {
//                         checklevel: checklevel
//                     },
//                     success: function (data) {
//                              var rhtml = "";
//                         if (data.length > 0) {
//                             for (var i = 0; i < data.length; i++) {
//                                 rhtml += "<option value='" + data[i].uid + "'>" + data[i].username + "</option>";
//                             }
//                         }
//                         $("#checkernameList").html(rhtml);
//                              }
//                  });
//           }

        function ChangeCheckernameList() {
                var checklevel = $("#checklevel").val();
                var id = $("#checkersector").val();
                if (id=="") {
                        var rhtml = "<option value=''>请选择...</option>";
                        $("#checkernameList").html(rhtml);
                        $("#checkernameList").css('width', '194px').select2({ allowClear: true });
                }
                else{
                    $.ajax({
                        type: "GET",
                        url: rootUri + "Judge/FindJudgeCheckerList/" + id,
                        dataType: "json",
                        data: {
                            checklevel: checklevel
                        },
                        success: function (data) {
                            var rhtml = "<option  value=''>请选择...</option>";
                            if (data.length > 0) {
                                for (var i = 0; i < data.length; i++) {
                                    rhtml += "<option value='" + data[i].username + "'>" + data[i].username + "</option>";
                                }
                            }
                            $("#checkernameList").html(rhtml);
                            $("#checkernameList").css('width', '194px').select2({ allowClear: true });
                        }
                    });
                }
        }
        </script>
</asp:Content>
