<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		车队考试
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
		<div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>考试标题</th>
						<th style="width:140px;">总考题数</th>
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
			    "bProcessing": true,
			    "sAjaxSource": rootUri + "Exam/RetrieveBookList?examkind=<%= (int)ExamKind.Team %>",
			    "oLanguage": {
			        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
			    },
			    //bAutoWidth: false,
			    "aoColumns": [
					null,
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
				        aTargets: [1],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = o.aData[1] + '个题';
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [2],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'Exam/PreviewExam/' + o.aData[2] + '">' +
                                '<i class="ace-icon fa fa-pencil bigger-120"></i>参加考试' +
                                '</a>&nbsp;&nbsp;';
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

    </script>
</asp:Content>
