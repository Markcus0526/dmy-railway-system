<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		公文查询
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
				                <label class="col-sm-2 control-label no-padding-right" for="teamid">发布车队：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="teamid" name="teamid" data-placeholder="请选择">
                                                <option value="0" selected>全部</option>
                                                <% if (ViewData["teamlist"] != null) { 
                                                        foreach(var item in (List<tbl_railteam>)ViewData["teamlist"]) {
                                                            %>
                                                            <option value="<%= item.uid %>"><%= item.teamname %></option>
                                                            <% 
                                                        }
                                                } %>
				                            </select>
						                </div>
                                    </div>
				                </div>

				                <label class="col-sm-2 control-label no-padding-right" for="sectorid">发布科室：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="sectorid" name="sectorid" data-placeholder="请选择">
                                                <option value="0" selected>全部</option>
                                                <% if (ViewData["sectorlist"] != null) { 
                                                        foreach(var item in (List<tbl_railsector>)ViewData["sectorlist"]) {
                                                            %>
                                                            <option value="<%= item.uid %>"><%= item.sectorname %></option>
                                                            <% 
                                                        }
                                                } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
			                </div>
                            <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="starttime">发布开始时间：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
							                <input class="input-large date-picker" id="starttime" name="starttime" type="text" data-date-format="yyyy-mm-dd" value="" />
							                <span class="input-group-addon">
								                <i class="fa fa-calendar bigger-110"></i>
							                </span>
						                </div>
                                    </div>
				                </div>
				                <label class="col-sm-2 control-label no-padding-right" for="endtime">发布结束时间：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
							                <input class="input-large date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd" value="" />
							                <span class="input-group-addon">
								                <i class="fa fa-calendar bigger-110"></i>
							                </span>
						                </div>
                                    </div>
				                </div>
                            </div>
                            </form>
                        </div>
                    </div>
					<hr />
					<div style="text-align:center;">
						<span class="btn btn-sm btn-info" onclick="search_data();" ><i class="fa fa-search"></i> 查询</span>
					</div>
				</div>
			</div>
		</div>
		<div>
			<div class="portlet-body">
			    <table id="tblspec" class="table table-striped table-bordered table-hover">
				    <thead>
					    <tr>
						    <th>通知号</th>
						    <th>公文标题</th>
						    <th>发布部门</th>
						    <th>发布人</th>
						    <th>发布时间</th>
        <%
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("Document"))
            {
            %>
						    <th>操作</th>
            <% } %>
					    </tr>
				    </thead>
				    <tbody>
				    </tbody>
			    </table>
			</div>
		</div>
	</div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
        <%
            var userrole = CommonModel.GetUserRoleInfo();
            %>

	<script type="text/javascript">
	    var specTable;

	    jQuery(function ($) {
	        $(".select2").css('width', '250px').select2({ allowClear: true })
			.on('change', function () {

			});

	        $('.date-picker').datepicker({
	            autoclose: true,
	            todayHighlight: true,
	            language: "zh-CN"
	        });

	        specTable =
			$('#tblspec')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": true,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Document/RetrieveSearchResult?teamid=0&sectorid=0",
			    "oLanguage": {
			        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
			    },
			    //bAutoWidth: false,
			    "aoColumns": [
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false },
                    <% 
                    if (userrole != null && ((string)userrole).Contains("Document"))
                    {
                     %>
					{ "bSortable": false }
                    <% } %>
				],
			    "iDisplayLength": 10,
				"aoColumnDefs": [
                    <% 
                    if (userrole != null && ((string)userrole).Contains("Document"))
                    {
                     %>
				    {
				        aTargets: [5],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '';
				            rst += '<a class="btn btn-xs btn-blue" href="' + rootUri + 'Document/Detail/' + o.aData[5] + '">' +
                                '查看详情' +
                                '</a>&nbsp;&nbsp;';
				            rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[5] + ')">' +
                                '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
                                '</a>';
				            return rst;
				        },
				        sClass: 'center'
				    }
                    <% } %>
                ],
			    "fnDrawCallback": function (oSettings) {
			        //showBatchBtn();
			    }

			});

	    });

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	        } else {

	        }
	    }

	    function search_data() {
	        var teamid = $("#teamid").val();
	        var sectorid = $("#sectorid").val();
	        var starttime = $("#starttime").val();
	        var endtime = $("#endtime").val();

	        oSettings = specTable.fnSettings();
	        oSettings.sAjaxSource = rootUri + "Document/RetrieveSearchResult?teamid=" + teamid + "&sectorid=" + sectorid + "&starttime=" + starttime + "&endtime=" + endtime;

	        specTable.dataTable().fnDraw();
	    }

		function processDel(sel_id) {
		    var selected_id = "";

		    if (sel_id != null && sel_id.length != "") {
		        selected_id = sel_id;
		    } else {
		        $(':checkbox:checked').each(function () {
		            if ($(this).attr('name') == 'selcheckbox')
		                selected_id += $(this).attr('value') + ",";
		        });
		    }

		    if (selected_id != "") {
		        bootbox.dialog({
		            message: "您确定要删除吗？",
		            buttons: {
		                danger: {
		                    label: "取消",
		                    className: "btn-danger",
		                    callback: function () {
		                        return true;
		                    }
		                },
		                main: {
		                    label: "确定",
		                    className: "btn-primary",
		                    callback: function () {
		                        $.ajax({
		                            url: rootUri + "Document/DeleteDocument",
		                            data: {
		                                "delids": selected_id
		                            },
		                            type: "post",
		                            success: function (message) {
		                                if (message == true) {
		                                    toastr.options = {
		                                        "closeButton": false,
		                                        "debug": true,
		                                        "positionClass": "toast-bottom-right",
		                                        "onclick": null,
		                                        "showDuration": "3",
		                                        "hideDuration": "3",
		                                        "timeOut": "1500",
		                                        "extendedTimeOut": "1000",
		                                        "showEasing": "swing",
		                                        "hideEasing": "linear",
		                                        "showMethod": "fadeIn",
		                                        "hideMethod": "fadeOut"
		                                    };
		                                    toastr["success"]("批量删除成功！", "恭喜您");
		                                }
		                            }
		                        });
		                    }
		                }
		            }
		        });
		    }
		    else {
		        //
		    }
		    return false;
		}

		function redirectToListPage(status) {
		    if (status.indexOf("error") != -1) {
		    } else {
		        refreshTable();
		    }
		}
		function refreshTable() {
		    var oSettings = specTable.fnSettings();

		    specTable.dataTable().fnDraw();
		}

    </script>
</asp:Content>
