<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>

<div class="page-header">
	<h1>
		考试成绩历史
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
                                    <label for="form-field-select-3" for="starttime">起始日期:</label>
                                </div>
						        <div class="input-group" style="width:200px; float:left;">
                                    <input class="form-control date-picker" id="starttime" name="starttime" type="text" data-date-format="yyyy-mm-dd" />
							        <span class="input-group-addon">
								        <i class="fa fa-calendar bigger-110"></i>
							        </span>
                                </div>
                                <div style="float:left">&nbsp;&nbsp;&nbsp;</div>
                                <div style="float:left;">
                                    <label for="form-field-select-3" for="endtime">结束日期:</label>
                                </div>
						        <div class="input-group" style="width:200px; float:left;">
                                    <input class="form-control date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd" />
							        <span class="input-group-addon">
								        <i class="fa fa-calendar bigger-110"></i>
							        </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-sm-12">
				            <label class="col-sm-1 control-label no-padding-right" >考试范围<span class="red">*</span>：</label>
				            <div class="col-sm-2">
                                <div class="clearfix">
						            <div class="input-group col-xs-10 col-sm-5">
							            <select class="select2" id="examtype" onchange="changeexambooklist()">
                                            <option value="0">所有考试</option>
                                            <option value="1">段及考试</option>
                                            <option value="2">车队考试</option>
                                        </select>
						            </div>
                                </div>
				            </div>
                            <label class="col-sm-1 control-label no-padding-right" >考试题目：</label>
				            <div class="col-sm-2">
                                <div class="clearfix">
						            <div class="input-group col-xs-10 col-sm-5">
							            <select class="select2" id="exambooklist">
                                            <option value="0">全部</option>
                                        </select>
						            </div>
                                </div>
				            </div>
				            <label class="col-sm-1 control-label no-padding-right team" >车队<span class="red">*</span>：</label>
				            <div class="col-sm-2">
                                <div class="clearfix">
						            <div class="input-group col-xs-10 col-sm-5">
							            <select class="select2 team" id="teamList" onchange="changegrouplist()"  <% if (userrole != null && ((string)userrole).Contains("TeamManager")) { %> disabled<% } %> >
                                            <% if (ViewData["teamlist"] != null)
                                               {
                                                
                                                   foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                                   { %>
                                                   <option value="<%= item.uid %>"  <%  if ( item.uid== (long)ViewData["teamid"]) { %> selected<% } %> ><%= item.teamname %></option>
                                                   <% 
                                                   }
                                                    %>
                                            <% } %>
                                        </select>
							            
						            </div>
                                </div>
				            </div>
                            <label class="col-sm-1 control-label no-padding-right group" data-placeholder="请选择班组" >班组<span class="red">*</span>：</label>
				            <div class="col-sm-2">
                                <div class="clearfix">
						            <div class="input-group col-xs-10 col-sm-5">
							            <select class="select2 group" id="groupList" >
                                            <option value="0">请选择...</option>
                                            
                                        </select>
						            </div>
                                </div>
				            </div>
			            </div>
                    </div>
                    </br>
                    </br>
					
                    <hr />
						<span class="btn btn-sm btn-info" onclick="search_data();" ><i class="fa fa-search"></i> 查询</span>
						<a target="_blank" class="btn btn-sm btn-info" id="download"><i class="fa fa-download"></i> 导出Excel</a>
					
				</div>
			</div>
		</div>
		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
                        <th>考试范围</th>
						<th>考试标题</th>
						<th>工资号</th>
						<th>姓名</th>
                        <th>考试分数</th>
                        <th>参加考试日期</th>
                        <th>考试时间</th>
                        <th>答题时间</th>
						<th style="width:140px;">总题数/答对数</th>
						<th style="width:200px;">操作</th>
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
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>

	<script type="text/javascript">
        

        function changegrouplist(){
            var teamid= $("#teamList").val();
            $.ajax({
                    type: "GET",
                    url: rootUri + "Exam/GetgroupList/?teamid=" + teamid,
                    dataType: "json",
                    success: function (data) {
                        var rhtml = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                            }
                            $("#groupList").html(rhtml);
                            $("#groupList").css('width', '200px').select2({ allowClear: true });
                        }
                        else{
                        rhtml= "<option value='0'>无可用班组</option>";
                        $("#groupList").html(rhtml);
                        $("#groupList").css('width', '200px').select2({ allowClear: true });
                        }
                    },
                });
            }
	    var selected_id = "";
	    var oTable;
	    jQuery(function ($) {
	        $('.date-picker').datepicker({
	            autoclose: true,
	            todayHighlight: true,
	            language: "zh-CN"
	        });
            changegrouplist();
	        $(".select2").css('width', '200px').select2({ allowClear: true })
			.on('change', function () {
			    //$(this).closest('form').validate().element($(this));
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
            changeexambooklist()
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
        function showtable(){
        oTable =
			$('#tbldata')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": true,
			    "sAjaxSource": rootUri + "Exam/SerchTeamResultList?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val()+ "&examtype=" + $("#examtype").val()+ "&teamid=" + $("#teamList").val()+ "&groupid=" + $("#groupList").val()+"&exambook="+$("#exambooklist").val(),
			    "oLanguage": {
			        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
			    },
                "bFilter": false,
			    //bAutoWidth: false,
			    "aoColumns": [
					null,
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
				        aTargets: [0],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = "";
				            if (o.aData[0] == "0") {
				                rst = "段级考试";
				            } else {
				                rst = "车队考试";
                            }
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [6],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = o.aData[6] + '分';
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [9],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'Exam/ResultDetail/' + o.aData[9] + '">' +
                                '<i class="ace-icon fa fa-search bigger-120"></i>查看详情' +
                                '</a>&nbsp;&nbsp;';
				            return rst;
				        },
				        sClass: 'center'
				    }
                ],
			    "fnDrawCallback": function (oSettings) {

			    }

			});
        }
		function search_data() {
            if (oTable != null) {

		    oSettings = oTable.fnSettings();
		    oSettings.sAjaxSource = rootUri + "Exam/SerchTeamResultList?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val()+ "&examtype=" + $("#examtype").val()+ "&teamid=" + $("#teamList").val()+ "&groupid=" + $("#groupList").val()+"&exambook="+$("#exambooklist").val();
            oTable.fnClearTable(0);
            oTable.fnDraw();
            }
            else{
            showtable();}
            $("#download").attr("href",rootUri + "Exam/ExportTeamExamResult/?groupid=" + $("#groupList").val() + "&examtype=" + $("#examtype").val()+"&teamid="+$("#teamList").val()+"&exambook="+$("#exambooklist").val());

            }

        function changeexambooklist(){
        var examtype= $("#examtype").val();
            $.ajax({
                    type: "GET",
                    url: rootUri + "Exam/GetExambokkListByExamtype/?examtype=" + examtype,
                    dataType: "json",
                    success: function (data) {
                        var rhtml = "<option value='0'>全部</option>";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].title + "</option>";
                            }
                            $("#exambooklist").html(rhtml);
                            $("#exambooklist").css('width', '200px').select2({ allowClear: true });
                        }
                        else{
                        rhtml= "<option value='0'>无可用试题</option>";
                        $("#exambooklist").html(rhtml);
                        $("#exambooklist").css('width', '200px').select2({ allowClear: true });

                        }
                    },
                });
        }
    </script>
</asp:Content>
