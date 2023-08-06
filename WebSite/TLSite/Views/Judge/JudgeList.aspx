<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		考核查询
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="widget-box">
			<div class="widget-header">
				<h4 class="smaller">
					搜索
				</h4>
			</div>

			<div class="widget-body">
				<div class="widget-main">
                    <div class="searchbar">
                        <div>
                            <div style="height:46px;">
                                <div style="float:left;">
                                    <label for="form-field-select-3" for="starttime">检查起始日期:</label>
                                </div>
						        <div class="input-group" style="width:200px; float:left;">
                                    <input class="form-control date-picker" id="starttime" name="starttime" type="text" data-date-format="yyyy-mm-dd" />
							        <span class="input-group-addon">
								        <i class="fa fa-calendar bigger-110"></i>
							        </span>
                                </div>
                                <div style="float:left">&nbsp;&nbsp;&nbsp;</div>
                                <div style="float:left;">
                                    <label for="form-field-select-3" for="endtime">检查结束日期:</label>
                                </div>
						        <div class="input-group" style="width:200px; float:left;">
                                    <input class="form-control date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd" />
							        <span class="input-group-addon">
								        <i class="fa fa-calendar bigger-110"></i>
							        </span>
                                </div>
                            </div>

                            <div>
                                <label for="form-field-select-3" for="teamid">车队:</label>
						        <select class="select2" id="teamid" name="teamid" data-placeholder="请选择" onchange="changeGroupList()">
                                    <option value="0" selected>全部</option>
                                    <% if (ViewData["teamlist"] != null) { 
                                           foreach(var item in (List<tbl_railteam>)ViewData["teamlist"]) {
                                               %>
                                               <option value="<%= item.uid %>"><%= item.teamname %></option>
                                               <% 
                                           }
                                    } %>
				                </select>&nbsp;&nbsp;
                                <label for="form-field-select-3" for="groupid">班组:</label>
						        <select class="select2" id="groupid" name="groupid" data-placeholder="请选择" onchange="changeCrewList();">
                                    <option value="0" selected>全部</option>
                                    <% if (ViewData["grouplist"] != null) {
                                           foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                           {
                                               %>
                                               <option value="<%= item.uid %>"><%= item.groupname %></option>
                                               <% 
                                           }
                                    } %>
				                </select>

                                <label for="form-field-select-3" for="groupid">被考核人员:</label>
						        <select class="select2" id="crewid" name="crewid" data-placeholder="请选择">
                                    <option value="0" selected>全部</option>
                                    <% if (ViewData["grouplist"] != null) {
                                            foreach (var item in (List<CrewInfo>)ViewData["crewlist"])
                                            {
                                                %>
                                                <option value="<%= item.uid %>"><%= item.realname %></option>
                                                <% 
                                            }
                                    } %>
				                </select>

                            </div>
                        </div>
                    </div>
					<hr />
					<p>
						<span class="btn btn-sm btn-info" onclick="search_data();" ><i class="fa fa-search"></i> 开始统计</span>
						<a target="_blank" class="btn btn-sm btn-info" href="<%= ViewData["rootUri"] %>judge/ExportJudgeList"><i class="fa fa-download"></i> 导出Excel</a>
					</p>
				</div>
			</div>
		</div>

		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th style="min-width:120px;">问题</th>
						<th>车队</th>
						<th>班组</th>
						<th>责任人</th>
						<th>责任列车长</th>
						<th>图片</th>
						<th>检测日期</th>
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
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/chosen.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/chosen.jquery.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
		<script type="text/javascript">
		    var selected_id = "";
		    var oTable;
		    jQuery(function ($) {

		        $('.date-picker').datepicker({
		            autoclose: true,
		            todayHighlight: true,
		            language: "zh-CN"
		        })

		        $(".select2").css('width', '250px').select2({ allowClear: true })
			    .on('change', function () {
			    
			    });

		        oTable =
				$('#tbldata')
				.dataTable({
				    "bServerSide": true,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "Judge/RetrieveJudgeList",
				    "oLanguage": {
				        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
				    },
				    //bAutoWidth: false,
				    "aoColumns": [
					  { "bSortable": false },
					  null,
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
				    "bFilter": false,
				    "aoColumnDefs": [
				        {
				            aTargets: [5],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                return '<img src="' + rootUri + o.aData[5] + '" style="max-height:70px;" />';
				            },
				            sClass: 'center'
				        },
				        {
				            aTargets: [7],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = '<a target="_new" class="btn btn-xs btn-info" href="' + rootUri + 'Judge/JudgeDetail/' + o.aData[7] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>查看详情' +
                                    '</a>';
				                return rst;
				            },
				            sClass: 'center'
				        }
                    ],
				    "fnDrawCallback": function (oSettings) {
				        
				    }

				});

		        $(document).on('click', 'th input:checkbox', function () {
		            var that = this;
		            $(this).closest('table').find('tr > td:first-child input:checkbox')
					.each(function () {
					    this.checked = that.checked;
					    $(this).closest('tr').toggleClass('selected');
					});

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

		    });

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

		    function changeGroupList() {
		        var teamid = $("#teamid").val();

		        $.ajax({
		            type: "GET",
		            url: rootUri + "Judge/GetGroupListOfTeam/" + teamid,
		            dataType: "json",
		            success: function (data) {
		                var rhtml = "<option value='0' >全部</option>";
		                if (data.length > 0) {
		                    for (var i = 0; i < data.length; i++) {
		                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
		                    }
		                    $("#groupid").html(rhtml);
		                    $("#groupid").css('width', '250px').select2({ allowClear: true });
		                }
		            }
		        });
		    }

		    function search_data() {
                var starttime = $("#starttime").val();
                var endtime = $("#endtime").val();
                var teamid = $("#teamid").val();
                var groupid = $("#groupid").val();
                var crewid = $("#crewid").val();

		        oSettings = oTable.fnSettings();
		        oSettings.sAjaxSource = rootUri + "Judge/RetrieveJudgeList?starttime=" + starttime + "&endtime=" + endtime + "&teamid=" + teamid + "&groupid=" + groupid + "&crewid=" + crewid;
                
		        oTable.dataTable().fnDraw();

		    }

		    function changeCrewList() {
		        var groupid = $("#groupid").val();

		        $.ajax({
		            type: "GET",
		            url: rootUri + "Judge/GetCrewListOfGroup/" + groupid,
		            dataType: "json",
		            success: function (data) {
		                var rhtml = "<option value='0' >全部</option>";
		                if (data.length > 0) {
		                    for (var i = 0; i < data.length; i++) {
		                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
		                    }
		                    $("#crewid").html(rhtml);
		                    $("#crewid").css('width', '250px').select2({ allowClear: true });
		                }
		            }
		        });
		    }

        </script>
</asp:Content>
