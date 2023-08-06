<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="TLSite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>
    <div class="page-header">
        <h1>
            出乘管理 <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)"
                style="float: right"><i class="ace-icon fa fa-times red2"></i>返回 </a>
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
                                <div class="form-group" style="height:40px;">
                                    <label class="col-sm-1 control-label no-padding-right" for="starttime">
                                        出乘日期：</label>
                                    <div class="col-sm-3">
                                        <div class="clearfix">
                                            <div class="input-group col-xs-10 col-sm-5 ">
                                                <input class="input-large date-picker" name="starttime" id="starttime" type="text"
                                                    data-date-format="yyyy-mm-dd" <% if (ViewData["starttime"] != null) { %>value="<%= ViewData["starttime"] %>"
                                                    <% } %> />
                                                <span class="input-group-addon"><i class="fa fa-calendar bigger-110"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <label class="col-sm-1 control-label no-padding-right" for="endtime">
                                        退乘日期：</label>
                                    <div class="col-sm-3">
                                        <div class="clearfix">
                                            <div class="input-group col-xs-10 col-sm-5 ">
                                                <input class="input-large date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd"
                                                    <% if (ViewData["endtime"] != null) { %>value="<%= ViewData["endtime"] %>" <% } %> />
                                                <span class="input-group-addon"><i class="fa fa-calendar bigger-110"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-1 control-label no-padding-right" for="teamid">
                                        车队：</label>
                                    <div class="col-sm-3">
                                        <div class="clearfix">
                                            <select class="select2" id="teamid" name="teamid" data-placeholder="请选择车队" onchange="changeGroupList()">
                                                <%if (userrole != null && (((string)userrole).Contains("Duty") || ((string)userrole).Contains("Executive")))
                                                  {%>
                                                <option value="0">请选择...</option>
                                                <%} %>
                                                <% if (ViewData["teamlist"] != null)
                                                   {
                                                       foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                                       { %>
                                                            <option value="<%= item.uid %>"> <%= item.teamname %></option>
                                                    <% 
                                                        }
                                                    %>
                                                <% } %>
                                            </select>
                                        </div>
                                    </div>
                                    <label class="col-sm-1 control-label no-padding-right" for="groupid">
                                        班组：</label>
                                    <div class="col-sm-3">
                                        <div class="clearfix">
                                            <select class="select2" id="groupid" name="groupid" onchange="changeCrewList()">
                                                <option value="0">请选择...</option>
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
                                    <label class="col-sm-1 control-label no-padding-right" for="">
                                        姓名：</label>
                                    <div class="col-sm-3">
                                        <div class="clearfix">
                                            <select class="select2" id="crewlist" name="crewlist">
                                                <option value="0">请选择...</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group" style="height:30px;">
                            <!--按钮部分-->
                            <div style="float: right; margin-right: 10px;">
                                <a class="btn btn-sm btn-info" id="searchdata" onclick="showlist()"><i class="fa fa-search">
                                </i>按班组乘务信息查询</a> <a class="btn btn-sm btn-info" id="A1" onclick="showlist2()"><i
                                    class="fa fa-search red2"></i>个人乘务信息查询</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <p>
                    <a class="btn btn-white btn-info btn-bold  <%if (userrole != null && ((string)userrole).Contains("Excutive")&&!((string)userrole).Contains("TeamExec")&&!((string)userrole).Contains("TeamAdmin") ){ %>hide<% } %>" href="<%= ViewData["rootUri"] %>Duty/NewDuty">
                        <i class="ace-icon fa fa-plus bigger-120 blue"></i>添加出乘 
                    </a>
                    <a class="btn btn-white btn-warning btn-bold"style="display: none;" id="btnbatchdel" onclick="processDel();">
                        <i class="ace-icon fa fa-trash-o bigger-120 orange"></i>批量删除 
                    </a>
                </p>
            </div>
            <div class="portlet-body">
                <table id="tbldata" class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th class="center">
                                <label class="position-relative">
                                    <input type="checkbox" class="ace" />
                                    <span class="lbl"></span>
                                </label>
                            </th>
                            <th>
                                车队
                            </th>
                            <th>
                                线路
                            </th>
                            <th>
                                班组
                            </th>
                            <th>
                                车次
                            </th>
                            <th>
                                出乘日期
                            </th>
                            <th>
                                退乘日期
                            </th>
                            <th style="min-width: 80px;">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="portlet-body">
                <table id="tbldata2" class="table table-striped table-bordered table-hover" style="display: none">
                    <thead>
                        <tr>
                            <th>
                                车队
                            </th>
                            <th>
                                班组
                            </th>
                            <th>
                                工资号
                            </th>
                            <th>
                                姓名
                            </th>
                            <th>
                                职名
                            </th>
                            <th>
                                乘务车次
                            </th>
                            <th>
                                出乘日期
                            </th>
                            <th>
                                退乘日期
                            </th>
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
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>

    <script type="text/javascript">
        var selected_id = "";
        var oTable;
        jQuery(function ($) {

            $('.date-picker').datepicker({
                autoclose: true,
                todayHighlight: true,
                language: "zh-CN"
            });

		    $(".select2").css('width', '250px').select2({ allowClear: true })
		    .on('change', function () {
		    });

            function noneused() {
                oTable =
				$('#tbldata')
				.dataTable({
				    "bServerSide": true,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "Duty/RetrieveDutyList",
				    "oLanguage": {
				        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
				    },
				    //bAutoWidth: false,
				    "aoColumns": [
					  { "bSortable": false },
					  null,
					  null,
					  null,
					  null,
					  null,
					  null,
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
				                var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'Duty/EditDuty/' + o.aData[7] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
				                rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[7] + ')">' +
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
            }

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
            search();
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
                                    url: rootUri + "Duty/DeleteDuty",
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
                                            if (tbl != null) {
                                                   // tbl.fnClearTable(0);
                                                    tbl.fnDraw();
                                            }
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

        var tbl;
        var tbl2;

        function search() {
            tbl = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "Duty/RefreshTable/?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val() + "&teamid=" + $("#teamid").val() + "&groupid=" + $("#groupid").val(),
                "bFilter": false,
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
				                var rst = '<a class="btn btn-xs btn-info" href="' + rootUri + 'Duty/EditDuty/' + o.aData[7] + '">' +
                                    '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                    '</a>&nbsp;&nbsp;';
				                rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[7] + ')">' +
                                    '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
                                    '</a>';
				                return rst;
				            },
				            sClass: 'center'
				        }
                    ],

            });
        }
        function search2() {
            tbl2 = $('#tbldata2').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "Duty/RefreshTable2/?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val() + "&uid=" + $("#crewlist").val(),
                "bFilter": false,
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
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
					],
                "bPaginate": true,
                "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],
                "iDisplayLength": 10
            });
        }
        function showlist() {
            $("#tbldata2_wrapper").hide();
            $("#tbldata_wrapper").show();
            $("#tbldata").show();
            if (tbl != null) {
                var oSettings = tbl.fnSettings();
                oSettings.sAjaxSource = rootUri + "Duty/RefreshTable/?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val() + "&teamid=" + $("#teamid").val() + "&groupid=" + $("#groupid").val();
              //  tbl.fnClearTable(0);
                tbl.fnDraw();
            }
            else {

                search();
            }
        }


        function showlist2() {
            $("#tbldata_wrapper").hide();
            $("#tbldata").hide();
            $("#tbldata2_wrapper").show();
            $("#tbldata2").show();

            if (tbl2 != null) {
                var oSettings2 = tbl2.fnSettings();
                oSettings2.sAjaxSource = rootUri + "Duty/RefreshTable2/?starttime=" + $("#starttime").val() + "&endtime=" + $("#endtime").val() + "&teamid=" + $("#teamid").val() + "&groupid=" + $("#groupid").val()+ "&uid=" + $("#crewlist").val();
               // tbl2.fnClearTable(0);
                tbl2.fnDraw();
            }
            else {

                search2();
            }
        }

        function changeGroupList(){
        var teamid=$("#teamid").val();
        $.ajax({
	            type: "GET",
	            url: rootUri + "Judge/FindGroupListByTeam/" + teamid,
	            dataType: "json",
	            success: function (data) {
	                var rhtml = "";
	                if (data.length > 0) {
                            rhtml += "<option value='0'>请选择...</option>";
	                    for (var i = 0; i < data.length; i++) {
	                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
	                    }
	                }
                    else{
                        if (teamid==0) {
                                 rhtml="<option value='0'>请选择...</option>";
                        }       
                        else{    rhtml="<option value='0'>无可选班组</option>";
                            }
                    }
                        $("#groupid").html(rhtml);
	                    $("#groupid").css('width', '250px').select2({ allowClear: true });
	                
                        changeCrewList();    
	            }
	        });
        }

        function changeCrewList(){
            var groupid=$("#groupid").val();
            var teamid=$("#teamid").val();
            var rhtml=""
            if (teamid!="0"&&groupid=="0"){
            //用teamid查用户列表
                 $.ajax({
	                type: "GET",
	                url: rootUri + "Duty/GetCrewListByTeam/" + teamid,
	                dataType: "json",
	                success: function (data) {
	                    var rhtml = "";
	                    if (data.length > 0) {
                                rhtml += "<option value='0'>请选择...</option>";
	                        for (var i = 0; i < data.length; i++) {
	                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
	                        }
	                    }
                        else{
                                rhtml="<option value='0'>无可选人员</option>";
                        }
                            $("#crewlist").html(rhtml);
	                        $("#crewlist").css('width', '250px').select2({ allowClear: true });
	                }
	              });
            }
            else if (teamid!="0"&&groupid!="0") {
                 //用groupid查用户列表
                 $.ajax({
	                type: "GET",
	                url: rootUri + "Duty/GetCrewListByGroup/" + groupid,
	                dataType: "json",
	                success: function (data) {
	                    var rhtml = "";
	                    if (data.length > 0) {
                                rhtml += "<option value='0'>请选择...</option>";
	                        for (var i = 0; i < data.length; i++) {
	                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
	                        }
	                    }
                        else{
                                rhtml="<option value='0'>无可选人员</option>";
                        }
                            $("#crewlist").html(rhtml);
	                        $("#crewlist").css('width', '250px').select2({ allowClear: true });
	                }
	              });
            }
            else if(teamid=="0"){
                 rhtml="<option value='0'>请选择...</option>";
            }
       
        
        }
    </script>
</asp:Content>
