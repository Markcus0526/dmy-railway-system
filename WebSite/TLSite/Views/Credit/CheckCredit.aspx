<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="page-header">
	<h1>
		积分查询
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
			    <label class="col-sm-3 control-label no-padding-right" for="starttime">月份：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">			
                           <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                           <input id="date" type="hidden" value="<%=ViewData["CurrentComputerDate"] %>" />
                           <input type="text" id="date2" class="input-large" value="<%= ViewData["CurrentDate"] %>" readonly=readonly  style="width:100px;" onfocus="WdatePicker({dateFmt:'yyyy年MM月', vel:date, isShowClear:false,  autoShowQS:false})" />
						   <span class="input-group-addon">
						   <i class="fa fa-calendar bigger-110"></i>
					       </span>
						</div>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="endtime">按姓名查询：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
							<input class="input-large" id="name" name="name" type="text" />
							
						</div>
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="teamid">所在部门：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" class="form-control select" id="sectorid" name="sectorid" style="width:139px" data-placeholder="请选择部门" onchange="changegrouplist();">
                            <% if (ViewData["Sectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["Sectorlist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
                <label class="col-sm-2 control-label no-padding-right" >班组：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" class="form-control select" id="grouplist" name="grouplist" style="width:139px" data-placeholder="请选择班组" onchange="">
				            <option value="0">全部</option>
                        </select>
                    </div>
				</div>

				<%--<label class="col-sm-1 control-label no-padding-right" for="routeid">线路：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="routeid" name="routeid" style="width:210px" data-placeholder="请选择线路" onchange="">
                            <% if (ViewData["routelist"] != null)
                               {
                                   foreach (var item in (List<tbl_railroute>)ViewData["routelist"])
                                   { %>
                                   <option value="<%= item.uid %>"  ><%= item.routename %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>--%>
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
						<th style="width:75px;text-align:center">车队</th>
						<th>工资号</th>
						<th style="width:66px;text-align:center">姓名</th>
						<th>班组</th>
						<th style="width:66px;text-align:center">职名</th>
						<th>段及以上批<br/>
                            评教育积分</th>
						<th>班组车队检查<br/>
                            批评教育积分</th>
                        <th>联挂考<br/>核积分</th>
                        <th>直接离岗<br/>
                            培训积分</th>
                        <th style="width:110px;">直接调整工作
                        <br/>岗位积分</th>
                        <th>本月积分</th>
                        <th>激励积分</th>
                        <th>累计积分</th>
					    <th>操作</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>           
        </form>
	</div>
</div>


 <!--对话框 显示详情-->
<div id="modal-table" class="modal fade" tabindex="-1">
	<div class="modal-dialog" style="width:90%">
		<div class="modal-content">
			<div class="modal-header no-padding">
				<div class="table-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">
						<span class="white">&times;</span>
					</button>
					查看个人月积分明细
				</div>
			</div>

			<div class="modal-body no-padding">
				<table id="tblspec" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top">
					<thead>
						<tr>
						    <th style="width:70px">车队</th>
						    <th style="width:70px">工资号</th>
						    <th style="width:60px">姓名</th>
						    <th style="width:70px">班组</th>
						    <th style="width:60px">职名</th>
						    <th style="width:72px">项点编码</th>
						    <th style="width:72px">两违问题</br>积分</th>
						    <th style="width:72px">激励积分</th>
						    <th>问题内容</th>
						    <th style="width:72px">检查部门</th>
						    <th style="width:60px">检查人</th>
                            <th style="width:72px">检查级别</th>
						    <th style="width:105px">检查时间</th>
						    <th style="width:45px">所属月份</th>
						</tr>
					</thead>
					<tbody>
						
					</tbody>
				</table>
			</div>

			<div class="modal-footer no-margin-top">
            <div style="float:right">
           
				<a class="btn btn-sm btn-danger pull-left" id="exportdetil">
					<i class="fa fa-download"  ></i>
					导出个人信息
				</a>
                <button class="btn btn-sm btn-danger pull-left" data-dismiss="modal">
					<i class="ace-icon fa fa-times "></i>
					关闭窗口
				</button>
                 </div>
			</div>

            <input type="hidden" id="dataid" value=""/>

		</div><!-- /.modal-content -->
	</div><!-- /.modal-dialog -->
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<!-- 载入年月js -->
    <script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script type="text/javascript"  src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>    


		<script type="text/javascript">
        jQuery(function ($){
            $(".select2").select2({ allowClear: true });
            $("#grouplist").css('width', '210px');
			
            changegrouplist();
           // ChangeRoutelist();
        });
		    var tbl;
		    var alerttable;
        //table部分
        function search() {
            tbl = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "Credit/RefreshTable/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val() + "&name=" + escape($("#name").val())+ "&groupid=" + $("#grouplist").val(),
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
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": true },
					  { "bSortable": false }
					],
                "bPaginate": true,
                "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],
                "iDisplayLength": 10,

                 "aoColumnDefs": [
                          {
                            aTargets: [13],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                
                                var rst = '<a class="btn btn-xs btn-info"  href="#modal-table" data-toggle="modal" onclick="test('+ o.aData[13] +','+o.aData[10]+','+o.aData[11]+')">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
				              
				                return rst;
				            },
                            }]
               
            });
        }

        function showlist() {
                if (tbl != null) {
                    var oSettings = tbl.fnSettings();
                    oSettings.sAjaxSource = rootUri + "Credit/RefreshTable/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val() + "&name=" + escape($("#name").val()) + "&groupid=" + $("#grouplist").val();
                    tbl.fnClearTable(0);
                    tbl.fnDraw();
                }
                else {
                    search();
                }
            //设置导出内容
            $("#download").attr("href",rootUri + "Credit/ExportCreditList/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val() + "&name=" + escape($("#name").val())+ "&groupid=" + $("#grouplist").val());

            }

            //对话框内容部分
            function test(uid,totlechkpoint,totleaddpoint) { 
            var date = $('#date').val();      
                if (alerttable != null) {
                    var oSettings = alerttable.fnSettings();
                    oSettings.sAjaxSource = rootUri + "Credit/GetAlertcontent/?uid="+uid+"&date="+date+"&totlechkpoint="+totlechkpoint+"&totleaddpoint="+totleaddpoint;
                    alerttable.fnClearTable(0);
                    alerttable.fnDraw();
                }

                else {
                 alerttable = $('#tblspec').dataTable({
                    "bServerSide": true,
                    "bProcessing": true,
                    "sAjaxSource": rootUri + "Credit/GetAlertcontent/?uid="+uid+"&date="+date+"&totlechkpoint="+totlechkpoint+"&totleaddpoint="+totleaddpoint,
                    "bFilter": false,
                    "oLanguage": {
                        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
                    },
                    "bAutoWidth": false,
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
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
					],
                    "bPaginate": false
                });
                }

                //设置导出内容
                $("#exportdetil").attr("href",rootUri + "Credit/ExportCreditDetilList/?uid="+uid+"&date="+date+"&totlechkpoint="+totlechkpoint+"&totleaddpoint="+totleaddpoint);
            }
           function changegrouplist(){
                var teamid = $("#sectorid").val();
                $.ajax({
                    type: "GET",
                    url: rootUri + "CheckInfo/Getgroup/?teamid=" + teamid,
                    dataType: "json",
                   success: function (data) {
                        var rhtml = "<option value='0'>全部</option>";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                            }
                            $("#grouplist").html(rhtml);
                            $("#grouplist").select2({ allowClear: true });

                        }
                        else{
                        rhtml= "<option value='-1'>无可用班组</option>";
                        $("#grouplist").html(rhtml);
                        $("#grouplist").select2({ allowClear: true });

                        }
                    },
                });
            }
//             function ChangeRoutelist(){
//                 var teamid = $("#sectorid").val();
//                 $.ajax({
//                     type: "GET",
//                     url: rootUri + "CheckInfo/GetRouteList/?teamid=" + teamid,
//                     dataType: "json",
//                    success: function (data) {
//                         var rhtml = "<option value='0'>请选择...</option>";
//                         if (data.length > 0) {
//                             for (var i = 0; i < data.length; i++) {
//                                 rhtml += "<option value='" + data[i].uid + "'>" + data[i].routename + "</option>";
//                             }
//                             $("#routeid").html(rhtml);
//                             $("#routeid").select2({ allowClear: true });
// 
//                         }
//                         else{
//                         rhtml= "<option value='0'>请选择...</option>";
//                         $("#routeid").html(rhtml);
//                         $("#routeid").select2({ allowClear: true });
// 
//                         }
//                     },
//                 });
//             }
        </script>


</asp:Content>
