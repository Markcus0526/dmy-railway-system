<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		干部管理
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
				                <label class="col-sm-2 control-label no-padding-right" for="userkind">用户类型：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="userkind" name="userkind" data-placeholder="请选择">
                                                <option value="0" selected>全部</option>
                                                <option value="1" >科室干部</option>
                                                <option value="2" >车队干部</option>
                                                <option value="3" >列车长</option>
                                                <option value="3" >列车员</option>
<!--                                                 <option value="4" >列车员</option> -->
				                            </select>
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
            <p>
                <a class="btn btn-white btn-info btn-bold" href="<%= ViewData["rootUri"] %>User/AddExecutive">
	                <i class="ace-icon fa fa-plus bigger-120 blue"></i>添加干部
                </a>
                <a class="btn btn-white btn-warning btn-bold" style="display:none;" id="btnbatchdel" onclick="processDel();">
                    <i class="ace-icon fa fa-trash-o bigger-120 orange"></i>批量删除
                </a>
            </p>
		</div>



		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th class="center">
							<label class="position-relative">
								<input type="checkbox" class="ace" />
								<span class="lbl"></span>
							</label>
						</th>
						<th>用户类型</th>
						<th>所属部门</th>
						<th>班组名称</th>
						<th>职务</th>
						<th>账号</th>
						<th>姓名</th>
						<th>照片</th>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  


		<script type="text/javascript">
		    var selected_id = "";
		    var oTable;
		    jQuery(function ($) {
		            oTable =$('#tbldata').dataTable({
				    "bServerSide": true,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "User/RetrieveUserList?userkind=0",
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
				                return '<label class="position-relative">' +
								    '<input type="checkbox" value="' + o.aData[0] + '" name="selcheckbox" class="ace" onclick="showBatchBtn()" />' +
								    '<span class="lbl"></span>' +
							        '</label>';
				            },
				            sClass: 'center'
				        },
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
				            aTargets: [8],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = "";
				                if (o.aData[8] == "0") {
				                    rst = "先生";
				                } else if (o.aData[8] == "1") {
				                    rst = "女士";
				                }
				                return rst;
				            }
				        },
				        {
				            aTargets: [10],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'User/EditExecutive/' + o.aData[10] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
				                rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[10] + ')">' +
                                    '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
                                    '</a>';
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

		    function search_data() {
		        var userkind = $("#userkind").val();

		        oSettings = oTable.fnSettings();
		        oSettings.sAjaxSource = rootUri + "User/RetrieveUserList?userkind=" + userkind;

		        oTable.dataTable().fnDraw();
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
		                                url: rootUri + "User/DeleteExecutive",
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
        </script>
</asp:Content>