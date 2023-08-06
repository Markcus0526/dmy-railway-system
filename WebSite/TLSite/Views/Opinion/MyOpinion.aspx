<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		我的诉求
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
		<div>
<%--            <p>
                <a class="btn btn-white btn-warning btn-bold" style="display:none;" id="btnbatchdel" onclick="processDel();">
                    <i class="ace-icon fa fa-trash-o bigger-120 orange"></i>批量删除
                </a>
            </p>--%>
		</div>

		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>标题</th>
						<th>提出时间</th>
						<th>诉求内容</th>
						<th>处理结果</th>
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
		        oTable =
				$('#tbldata')
				.dataTable({
				    "bServerSide": true,
		            "bFilter": false,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "Opinion/RetrieveMyOpinionList",
				    "oLanguage": {
				        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
				    },
				    //bAutoWidth: false,
				    "aoColumns": [
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
//				        {
//				            aTargets: [0],    // Column number which needs to be modified
//				            fnRender: function (o, v) {   // o, v contains the object and value for the column
//				                return '<label class="position-relative">' +
//								    '<input type="checkbox" value="' + o.aData[0] + '" name="selcheckbox" class="ace" onclick="showBatchBtn()" />' +
//								    '<span class="lbl"></span>' +
//							        '</label>';
//				            },
//				            sClass: 'center'
//				        },
				        {
				            aTargets: [4],    // Column number which needs to be modified
				            fnRender: function (o, v) {   // o, v contains the object and value for the column
				                var rst = "";
//				                rst += '<a class="btn btn-xs btn-white" href="' + rootUri + 'Opinion/Detail/' + o.aData[3] + '">' +
//                                    '查看详情' +
//                                    '</a>&nbsp;&nbsp;';
				                rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[4] + ')">' +
                                    '<i class="ace-icon fa fa-trash-o bigger-120"></i>删除' +
                                    '</a>';
				                return rst;
				            },
				            sClass: 'center'
				        }
                    ],
				    "fnDrawCallback": function (oSettings) {
				    }

				});

//		        $(document).on('click', 'th input:checkbox', function () {
//		            var that = this;
//		            $(this).closest('table').find('tr > td:first-child input:checkbox')
//					.each(function () {
//					    this.checked = that.checked;
//					    $(this).closest('tr').toggleClass('selected');
//					});

//		            showBatchBtn();
//		        });

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

//		    function showBatchBtn() {
//		        selected_id = "";

//		        $(':checkbox:checked').each(function () {
//		            if ($(this).attr('name') == 'selcheckbox')
//		                selected_id += $(this).attr('value') + ",";
//		        });

//		        if (selected_id != "") {
//		            $("#btnbatchdel").css("display", "normal");
//		        } else {
//		            $("#btnbatchdel").css("display", "none");
//		        }
	//	    }

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
		                                url: rootUri + "Opinion/DeleteOpinion",
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
		                                    } else {
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

		                                        toastr["error"]("请确认有没有在用该角色的管理员账号", "温馨敬告");
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
