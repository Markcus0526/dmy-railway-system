<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		车队人员列表
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="widget-box">
			<div class="widget-header">
				<h4 class="smaller">
					请搜索查询
				</h4>
			</div>
			<div class="widget-body">
				<div class="widget-main">
                    <div class="row">
	                    <div class="col-xs-12">
                            <form class="form-horizontal" role="form" id="validation-form">
			                <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="userkind">干部角色：</label>
				                <div class="col-sm-2">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="userkind" name="userkind" data-placeholder="请选择" onchange="">
                                                <option value="0" selected="selected">全部</option>
                                                <option value="2" >车队干部</option>
                                                <option value="3" >列车长</option>
                                                <option value="4" >列车员</option> 
				                            </select>
						                </div>
                                    </div>
				                </div>
                                 <label class="col-sm-1 control-label no-padding-right team" for="teamlist">车队：</label>
                                 <div class="col-sm-2 team">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="teamlist" name="teamlist" data-placeholder="请选择车队" onchange="changegrouplist()">
                                                 <% if (ViewData["currentteam"] != null)
                                                   {
                                                       foreach (var item in (List<tbl_railteam>)ViewData["currentteam"])
                                                       { %>
                                                <option value="<%= item.uid %>"><%= item.teamname %></option>
                                                <% 
                                                        }
                                                %>
                                                <% } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
				                <label class="col-sm-1 control-label no-padding-right group" for="grouplist">班组：</label>
                                 <div class="col-sm-2 group">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="grouplist" name="grouplist" data-placeholder="请选择班组">
                                                <option value="0">全部</option>
                                                 <% if (ViewData["grouplist"] != null)
                                                   {
                                                       foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                                       { %>
                                                <option value="<%= item.uid %>"><%= item.groupname %></option>
                                                <% 
                                                        }
                                                %>
                                                <% } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
                                <label class="col-sm-1 control-label no-padding-right sector hide" for="sectorlist">部门：</label>
                                 <div class="col-sm-2 sector hide">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="sectorlist" name="sectorlist" data-placeholder="请选择部门班组">
                                                <option value="0">全部</option>
                                                 <% if (ViewData["parentlist"] != null)
                                                   {
                                                       foreach (var item in (List<tbl_railsector>)ViewData["parentlist"])
                                                       { %>
                                                <option value="<%= item.uid %>"><%= item.sectorname%></option>
                                                <% 
                                                        }
                                                %>
                                                <% } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
			                </div>
                            </form>
                        </div>
                    </div>
					<hr />
					<div style="text-align:right;">
						<span class="btn btn-sm btn-info" onclick="retrievetbl();" ><i class="fa fa-search"></i> 查询</span>
						<a target="_blank" id="download" class="btn btn-sm btn-info" href="<% = ViewData["rootUri"]%>TeamAdmin/ExportTeamCrewList?userkind=0&groupid=0&teamid=0"><i class="fa fa-download"></i> 导出Excel</a>        
					</div>
				</div>
			</div>
		</div>



		<div>
            <p>
                <a class="btn btn-white btn-info btn-bold" href="<%= ViewData["rootUri"] %>TeamAdmin/AddTeamCrew">
	                <i class="ace-icon fa fa-plus bigger-120 blue"></i>添加人员
                </a>
                
            </p>
		</div>



		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>用户类型</th>
						<th>所属部门</th>
						<th>班组名称</th>
						<th>职务</th>
						<th>登录名</th>
						<th>工资号</th>
						<th>姓名</th>
						<th>照片</th>
						<th>政治面貌</th>
						<th>性别</th>
						<th class="hidden-480">出生日期</th>
						<th style="min-width:80px;">操作</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>
	</div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>



		<script type="text/javascript">
		    var selected_id = "";
		    var oTable;
		    jQuery(function ($) {

		        $(".select2").css('width', '200px').select2({ allowClear: true })
		        .on('change', function () {
		        });

		        oTable = $('#tbldata').dataTable({
		            "bServerSide": true,
		            "bProcessing": true,
		            "sAjaxSource": rootUri + "TeamAdmin/RetrieveTeamCrewList?userkind=0&&groupid="+$("#grouplist").val()+"&teamid="+$("#teamlist").val(),
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
					  { "bSortable": false }
					],
		            "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],
		            "iDisplayLength": 10,
		            "aoColumnDefs": [
				        {
				            aTargets: [7],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = "";
				                if (o.aData[7] == "" || o.aData[7] == null) {
				                    rst = "<img src='" + rootUri + "content/img/profile-pic.jpg' style='height:80px;' />";
				                } else {
				                    rst = "<img src='" + rootUri + o.aData[7] + "' style='height:80px;' />";
				                }
				                return rst;
				            }
				        },
				        {
				            aTargets: [9],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = "";
				                if (o.aData[9] == "0") {
				                    rst = "先生";
				                } else if (o.aData[9] == "1") {
				                    rst = "女士";
				                }
				                return rst;
				            }
				        },
				        {
				            aTargets: [11],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
                                    if (o.aData[0]!="车队干部") {
                                     var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'TeamAdmin/EditTeamCrew/' + o.aData[11] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
                                    }
                                    else{
                                    var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'TeamAdmin/EditTeamExec/' + o.aData[11] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
                                    }
				                    
				                    return rst;
				            },
				            sClass: 'center'
				        }
                    ],
		            "fnDrawCallback": function (oSettings) {
		                showBatchBtn();
		            }

		        });



		        $(document).on('click', 'th input:checkbox', function () {
		            var that = this;
		            $(this).closest('table').find('tr > td:first-child input:checkbox')
					.each(function () {
					    this.checked = that.checked;
					    $(this).closest('tr').toggleClass('selected');
					});

		            showBatchBtn();
		        });

		        $('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });
		        function tooltip_placement(context, source) {
		            var $source = $(source);
		            var $parent = $source.closest('table')
		            var off1 = $parent.offset();
		            var w1 = $parent.width();

		            var off2 = $source.offset();
		            //var w2 = $source.width();

		            if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
		            return 'left';
		        }
		        $("#download").attr("href", rootUri + "TeamAdmin/ExportTeamCrewList?userkind="+ $("#userkind").val()+"&&groupid="+$("#grouplist").val()+"&teamid="+$("#teamlist").val());

		    });

		    function showBatchBtn() {
		        selected_id = "";

		        $(':checkbox:checked').each(function () {
		            if ($(this).attr('name') == 'selcheckbox')
		                selected_id += $(this).attr('value') + ",";
		        });

		        if (selected_id != "") {
		            $("#btnbatchdel").css("display", "normal");
		        } else {
		            $("#btnbatchdel").css("display", "none");
		        }
		    }

		    function refreshTable() {
		        oSettings = oTable.fnSettings();

		        oTable.dataTable().fnDraw();
		    }
		    function redirectToListPage(status) {
		        if (status.indexOf("error") != -1) {
		        } else {
		            refreshTable();
		        }
		    }

		    function retrievetbl() {
		        var oSettings = oTable.fnSettings();
                    oSettings.sAjaxSource = rootUri + "TeamAdmin/RetrieveTeamCrewList?userkind="+ $("#userkind").val()+"&&groupid="+$("#grouplist").val()+"&teamid="+$("#teamlist").val();
                    oTable.fnClearTable(0);
                    oTable.fnDraw();
		        $("#download").attr("href", rootUri + "TeamAdmin/ExportTeamCrewList?userkind="+ $("#userkind").val()+"&&groupid="+$("#grouplist").val()+"&teamid="+$("#teamlist").val());

		    }

         function changegrouplist(){
            var teamid= $("#teamlist").val();
            $.ajax({
                    type: "GET",
                    url: rootUri + "CheckInfo/GetgroupList/?teamid=" + teamid,
                    dataType: "json",
                    success: function (data) {
                        var rhtml = "<option value='0'>全部</option>";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                            }
                            $("#grouplist").html(rhtml);
                            $("#grouplist").css('width', '194px').select2({ allowClear: true });

                        }
                        else{
                        rhtml= "<option value='0'>无可用班组</option>";
                        $("#grouplist").html(rhtml);
                        $("#grouplist").css('width', '194px').select2({ allowClear: true });

                        }
                       // changeuserlist();
                    },
                });
       }
        </script>
</asp:Content>
