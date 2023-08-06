<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>

<div class="page-header">
	<h1>
		结合部问题查询
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
				<label class="col-sm-3 control-label no-padding-right" for="date2">所在月份：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">			
                           <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                           <input id="date" type="hidden" value="<%=ViewData["CurrentComputerDate"] %>" />
                           <input type="text" id="date2" value="<%= ViewData["CurrentDate"] %>" readonly=readonly  style="width:100px;" onfocus="WdatePicker({dateFmt:'yyyy年MM月', vel:date, isShowClear:false,  autoShowQS:false})" />
						   <span class="input-group-addon">
						   <i class="fa fa-calendar bigger-110"></i>
					       </span>
						</div>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="checklevel">检查级别：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="checklevel" name="checklevel" data-placeholder="请选择检查人" onchange="">
                            <option selected value="">请选择...</option>
                            <option value="段以上">段以上检查</option>
                            <option value="段级">段级检查</option>
                            <option value="车队">车队检查</option>
                            <option value="班组">班组检查</option>
				        </select>
                    </div>
				</div>
			</div>
            <div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="teamlist">所属车队：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="teamlist" name="teamlist"  onchange="changegrouplsit()" <% if (userrole != null &&( ((string)userrole).Contains("Crew")||((string)userrole).Contains("TeamExec"))) { %> disabled<% } %>>
                            <option selected="selected" value="0">请选择...</option>
                            <% if (ViewData["Sectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["Sectorlist"])
                                   { %>
                                   <option value="<%= item.uid %>"  <%  if ( ViewData["teamid"]!=null&&item.uid== (long)ViewData["teamid"]) { %> selected="selected"<% } %>><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="grouplist">责任班组 ：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="grouplist" name="grouplist"  onchange="">
                            <option selected value="0">请选择...</option>
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
			</div>
            <div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right"  for="receivepart">受检部门：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<input type="text" id="receivepart"/>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="crewname">责任人：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
						<input type="text" id="crewname"/>
                        </div>
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
						<th style="width:100px">受检部门</th>
						<th>责任人</th>
						<th>项点编码</th>
                        <th>问题内容</th>
                        <th>检查部门</th>
                        <th>检查人</th>
					    <th>所属车队</th>
					    <th>责任班组</th>
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
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
    <script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script type="text/javascript">

    jQuery(function ($){
        $(".select2").css('width', '157px').select2({ allowClear: true })
			.on('change', function () {});
            });
		    var tbl;
		    var alerttable;
        //table部分
        function search() {
            tbl = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "CheckInfo/ShowTable2/?date=" + $("#date").val() + "&checklevel=" + escape($("#checklevel").val()) + "&teamid=" + $("#teamlist").val() + "&groupid=" + $("#grouplist").val()+ "&crewname=" + escape($("#crewname").val())+ "&receivepart=" + escape($("#receivepart").val()) ,
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
                    oSettings.sAjaxSource = rootUri + "CheckInfo/ShowTable2/?date=" + $("#date").val() + "&checklevel=" + escape($("#checklevel").val()) + "&teamid=" + $("#teamlist").val() + "&groupid=" + $("#grouplist").val()+ "&crewname=" + escape($("#crewname").val())+ "&receivepart=" + escape($("#receivepart").val()) ;
                    tbl.fnClearTable(0);
                    tbl.fnDraw();
                }
                else {
                    search();
                }
           
            //设置导出内容
            $("#download").attr("href",rootUri + "CheckInfo/Export2/?date=" + $("#date").val() + "&checklevel=" + escap($("#checklevel").val()) + "&teamid=" + $("#teamlist").val() + "&groupid=" + $("#grouplist").val()+ "&crewname=" + escape($("#crewname").val())+ "&receivepart=" + escape($("#receivepart").val()));

            }

          function changegrouplsit(){
          var teamid = $("#teamlist").val();

            $.ajax({
                type: "GET",
                url: rootUri + "CheckInfo/Getgroup/?teamid=" + teamid,
                dataType: "json",
               success: function (data) {
                    var rhtml = "<option value='0'>请选择...</option>";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                        }
                        $("#grouplist").html(rhtml);
                        $("#grouplist").css('width', '157px').select2({ allowClear: true });

                    }
                    else{
                    rhtml= "<option value='0'>请选择...</option>";
                    $("#grouplist").html(rhtml);
                    $("#grouplist").css('width', '157px').select2({ allowClear: true });


                    }
                },
            });


          }

    </script>
</asp:Content>
