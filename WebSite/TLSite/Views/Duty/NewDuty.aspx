<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var teaminfo = (tbl_railteam)ViewData["teaminfo"]; %>
<div class="page-header">
	<h1>
		出乘管理
		<small>
			<i class="ace-icon fa fa-angle-double-right"></i>
            <% if (ViewData["uid"] == null)
               { %>
			添加
            <% }
               else
               { %>
               编辑
            <% } %>
            出乘
		</small>
        <a class="btn btn-white btn-default btn-round" onclick="location.href='/duty/dutylist'" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>

<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="starttime">出乘日期：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
							<input class="input-large date-picker" name="starttime" id="starttime" type="text" data-date-format="yyyy-mm-dd" <% if (ViewData["starttime"] != null) { %>value="<%= ViewData["starttime"] %>"<% } %> />
							<span class="input-group-addon">
								<i class="fa fa-calendar bigger-110"></i>
							</span>
						</div>
                    </div>
				</div>


				<label class="col-sm-1 control-label no-padding-right" for="endtime">退乘日期：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
							<input class="input-large date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd" <% if (ViewData["endtime"] != null) { %>value="<%= ViewData["endtime"] %>"<% } %>/>
							<span class="input-group-addon">
								<i class="fa fa-calendar bigger-110"></i>
							</span>
						</div>
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="teamid">车队：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="teamid" name="teamid" data-placeholder="请选择车队" onchange="changeGroupList();changeRouteList();">
                            <% if (ViewData["teamlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["teamid"] != null && ViewData["teamid"].ToString() == item.uid.ToString()) { %>selected<% } %>><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>

				<label class="col-sm-1 control-label no-padding-right" for="routeid">线路：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="routeid" name="routeid" data-placeholder="请选择线路" onchange="changeTrainNoList();">
                            <% if (ViewData["routelist"] != null)
                               {
                                   foreach (var item in (List<tbl_railroute>)ViewData["routelist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["routeid"] != null && ViewData["routeid"].ToString() == item.uid.ToString()) { %>selected<% } %> ><%= item.routename %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
			</div>

			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="groupid">班组：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="groupid" name="groupid" data-placeholder="请选择班组">
                            <% if (ViewData["grouplist"] != null)
                               {
                                   foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["groupid"] != null && ViewData["groupid"].ToString() == item.uid.ToString()) { %>selected<% } %> ><%= item.groupname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="trainno">车次：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="trainno" name="trainno" data-placeholder="请选择车次">
                            <% if (ViewData["routelist"] != null)
                               {
                                   foreach (var item in (List<TrainNoInfo>)ViewData["trainnolist"])
                                   { %>
                                   <option value="<%= item.trainno %>" <% if (ViewData["trainno"] != null && ViewData["trainno"].ToString() == item.trainno) { %>selected<% } %> ><%= item.trainno %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
			</div>


            <div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for=""></label>
				<div class="col-sm-4">
                    <div class="clearfix">
                        <a class="btn btn-white btn-default btn-round" onclick="showGroupCrewList()">
	                        <i class="ace-icon fa fa-check red2"></i>
	                        生成报告
                        </a>
                    </div>
				</div>

                <%if (ViewData["editable"] != null)
                  {
                      if (ViewData["editable"].ToString() == "1")
                      {%>
                        <a class="btn btn-white btn-default btn-round" onclick="forbid(<% =ViewData["uid"].ToString()%>)">
	                    解除48小时限制
                        </a>
                    <%}
                      else
                      {%>
                         <a class="btn btn-white btn-default btn-round" onclick="forbid(<% =ViewData["uid"].ToString()%>)">
	                     恢复48小时限制
                         </a>
                    <%}
                  } %>
			</div>

            <h3 class="header smaller lighter green">下方列表显示查询结果</h3>
            <% if (ViewData["uid"] != null)
               { %>
            <div>
                <p>
                    <a class="btn btn-white btn-info btn-bold" onclick="openAddCrewDlg();">
	                    <i class="ace-icon fa fa-plus bigger-120 blue"></i>添加成员
                    </a>
                </p>
            </div>
            <% } %>

		    <div>
			    <table id="tblspec" class="table table-striped table-bordered table-hover">
				    <thead>
					    <tr>
						    <th>工次号</th>
						    <th>姓名</th>
						    <th>职名</th>
						    <th>班组</th>
					    </tr>
				    </thead>
				    <tbody>
				    </tbody>
			    </table>
		    </div>

            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
                    <button class="btn btn-sm btn-purple loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-floppy-o bigger-125"></i>
						提交
					</button>
					&nbsp; &nbsp; &nbsp;
					<button class="btn btn-sm" type="reset">
						<i class="ace-icon fa fa-undo bigger-110"></i>
						重置
					</button>
				</div>
			</div>
        </form>
	</div>
</div>

<div id="dialog-message" class="hide">
    <form class="form-horizontal" role="form" id="form_crew">
        <div class="col-xs-12">
		    <div class="form-group">
			    <label class="col-sm-1 control-label no-padding-right" style="margin:6px 0px;" for="sgroupid">班组：</label>
			    <div class="col-sm-5" style="margin:6px 0px;" >
                    <div class="clearfix">
						<select class="select2" id="sgroupid" name="sgroupid" data-placeholder="请选择班组">
                            <% if (ViewData["grouplist"] != null)
                               {
                                   foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.groupname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
			    </div>
                <a class="btn btn-white btn-default btn-round" onclick="showAbleCrewList()">
	                <i class="ace-icon fa fa-search"></i>
	                查询成员
                </a>
		    </div>
        </div>
        <input type="hidden" id="crewuid" name="crewuid" value="0" />
    </form>
    <div class="row">
        <div class="col-xs-5">
			<div class="widget-box">
				<div class="widget-header widget-header-flat">
					<h4 class="smaller">
						可选成员
					</h4>
				</div>

				<div class="widget-body">
					<div class="widget-main">
						<select id="ablelist" name="ablelist" multiple="multiple" style="height:200px;width:100%">
						</select>
					</div>
				</div>
			</div>
        </div>
        <div class="col-xs-2">
			<br/><br/>
			<button style="margin:4px;" type="button" class="btn btn-small btn-primary" id="send_right_all">>></button>
			<button style="margin:4px;" type="button" class="btn btn-small btn-primary" id="send_right">&nbsp;>&nbsp;</button>
			<button style="margin:4px;" type="button" class="btn btn-small btn-primary btn-inverse" id="send_left">&nbsp;<&nbsp;</button>
			<button style="margin:4px;" type="button" class="btn btn-small btn-primary btn-inverse" id="send_left_all"><<</button>
        </div>
        <div class="col-xs-5">
			<div class="widget-box">
				<div class="widget-header widget-header-flat">
					<h4 class="smaller">
						追加成员
					</h4>
				</div>

				<div class="widget-body">
					<div class="widget-main">
						<select id="additionlist" name="additionlist" multiple="multiple" style="height:200px; width:100%">
						</select>
					</div>
				</div>
			</div>
        </div>
    </div>
</div><!-- #dialog-message -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/jquery-ui.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>

	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>

	<script type="text/javascript">
	    var specTable;

	    jQuery(function ($) {
	        $(".select2").css('width', '250px').select2({ allowClear: true })
			.on('change', function () {
			    // change sub search options
			});

	        $('.date-picker').datepicker({
	            autoclose: true,
	            todayHighlight: true,
	            language: "zh-CN"
	        })

	        $.validator.messages.required = "必须要填写";
	        $.validator.messages.number = jQuery.validator.format("请输入一个有效的数字.");
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                starttime: {
	                    required: true
	                },
	                endtime: {
	                    required: true
	                },
	                teamid: {
	                    required: true
	                },
	                routeid: {
	                    required: true
	                },
	                groupid: {
	                    required: true
	                },
	                trainno: {
	                    required: true
	                }
	            },
	            highlight: function (e) {
	                $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
	            },

	            success: function (e) {
	                $(e).closest('.form-group').removeClass('has-error'); //.addClass('has-info');
	                $(e).remove();
	            },

	            errorPlacement: function (error, element) {
	                if (element.is(':checkbox') || element.is(':radio')) {
	                    var controls = element.closest('div[class*="col-"]');
	                    if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
	                    else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
	                }
	                else if (element.is('.select2')) {
	                    error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
	                }
	                else if (element.is('.chosen-select')) {
	                    error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
	                }
	                else error.insertAfter(element.parent());
	            },

	            submitHandler: function (form) {
	                submitform();
	                return false;
	            },
	            invalidHandler: function (form) {
	                $(".loading-btn").removeAttr("disabled");
	            }
	        });

	        //override dialog's title function to allow for HTML titles
	        $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
	            _title: function (title) {
	                var $title = this.options.title || '&nbsp;'
	                if (("title_html" in this.options) && this.options.title_html == true)
	                    title.html($title);
	                else title.text($title);
	            }
	        }));

	        specTable =
			$('#tblspec')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": false,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Duty/ShowAllCrewOfGroup/" + $("#uid").val(),
			    "oLanguage": {
			        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
			    },
			    //bAutoWidth: false,
			    "aoColumns": [
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false }
				],
			    "iDisplayLength": 10,

			    "fnDrawCallback": function (oSettings) {
			        //showBatchBtn();
			    }

			});

	        $("#send_right").click(function () {
	            $('#ablelist option:selected').each(function () {
	                $("#additionlist").append($(this));
	                $("#ablelist").remove($(this));
	            });
	        });
	        $("#send_right_all").click(function () {
	            if ($("#ablelist").html() != "") {
	                $("#additionlist").html($("#ablelist").html());
	                $("#ablelist").html("");
	            }
	        });
	        $("#send_left").click(function () {
	            $('#additionlist option:selected').each(function () {
	                $("#ablelist").append($(this));
	                $("#additionlist").remove($(this));
	            });
	        });
	        $("#send_left_all").click(function () {
	            if ($("#additionlist").html() != "") {
	                $("#ablelist").html($("#additionlist").html());
	                $("#additionlist").html("");
	            }
	        });
	    });

	    function submitform() {
	        $(".loading-btn").attr("disabled", "disabled");

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Duty/SubmitDuty",
	            dataType: "json",
	            data: $('#validation-form').serialize(),
	            success: function (data) {
	                if (data == "") {
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

	                    toastr["success"]("操作成功!", "恭喜您");
	                    window.location = rootUri + "Duty/DutyList";
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

	                    toastr["error"](data, "温馨敬告");

	                }
	            },
	            error: function (data) {
	                alert("Error: " + data.status);
	                $(".loading-btn").removeAttr("disabled");
	            }
	        });
	    }

	    function changeGroupList() {
	        var teamid = $("#teamid").val();

	        $.ajax({
	            type: "GET",
	            url: rootUri + "Duty/GetGroupListOfTeam/" + teamid,
	            dataType: "json",
	            success: function (data) {
	                var rhtml = "";
	                if (data.length > 0) {
	                    for (var i = 0; i < data.length; i++) {
	                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
	                    }
	                    $("#groupid").html(rhtml);
	                    $("#groupid").css('width', '250px').select2({ allowClear: true });

	                    $("#sgroupid").html(rhtml);
	                    $("#sgroupid").css('width', '250px').select2({ allowClear: true });
	                }
	                else {
	                    $("#groupid").html(rhtml);
	                    $("#groupid").css('width', '250px').select2({ allowClear: true });

	                    $("#sgroupid").html(rhtml);
	                    $("#sgroupid").css('width', '250px').select2({ allowClear: true });
                    }
	            }
	        });
        }

        function changeTrainNoList() {
            var routeid = $("#routeid").val();
            $.ajax({
                type: "GET",
                url: rootUri + "Duty/GetTrainNoListOfRoute/" + routeid,
                dataType: "json",
                success: function (data) {
                    var rhtml = "";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].trainno + "'>" + data[i].trainno + "</option>";
                        }
                        $("#trainno").html(rhtml);
                        $("#trainno").css('width', '250px').select2({ allowClear: true });
                    }
                }
            });
        }

        function showGroupCrewList() {
            oSettings = specTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Duty/RetrieveDutyCrewList/" + $("#uid").val() + "?act=add&groupid=" + $("#groupid").val();

            specTable.dataTable().fnDraw();

        }

        var crewdlg;
        function openAddCrewDlg() {
            var uid = $("#uid").val();
            var teamid = $("#teamid").val();

            crewdlg = $("#dialog-message").removeClass('hide').dialog({
                modal: true,
                width: 700,
                title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> 添加成员</h4></div>",
                title_html: true,
                buttons: [
					{
					    text: "确定",
					    "class": "btn btn-primary btn-xs",
					    click: function () {
					        var crewids = [];
					        $('#additionlist option').each(function (i, selected) {
					            crewids[i] = $(selected).val();
					        });

					        if (crewids.length > 0) {
					            $.ajax({
					                type: "POST",
					                url: rootUri + "Duty/SubmitAdditionCrew/" + uid + "?crewids=" + crewids,
					                dataType: "json",
					                success: function (data) {
					                    showGroupCrewList()
					                }
					            });
					        }


					        crewdlg.dialog("close");
					    }
					},
					{
					    text: "取消",
					    "class": "btn btn-xs",
					    click: function () {
					        $(this).dialog("close");
					    }
					}
				]
            });

        }

        function showAbleCrewList() {
            var sgroupid = $("#sgroupid").val();
            $.ajax({
                type: "GET",
                url: rootUri + "Duty/FindAbleCrewList/" + sgroupid,
                dataType: "json",
                success: function (data) {
                    var rhtml = "";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
                        }
                        $("#ablelist").html(rhtml);
                    }
                }
            });
        }

        function processDel(sel_id) {
            var uid = $("#uid").val();
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
                                url: rootUri + "Duty/DeleteDutyCrew/" + uid + "?crewid=" + sel_id,
                                type: "get",
                                success: function (message) {
                                    showGroupCrewList();
                                }
                            });
                        }
                    }
                }
            });
            return false;
        }
        function forbid(uid) {
            $.ajax({
                type: "GET",
                url: rootUri + "Duty/changeforbidable/?uid=" + uid,
                dataType: "json",
                success: function (data) {
                    if (data == "") {
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
                        toastr["success"]("操作成功!", "恭喜您");
                       // window.location.reload();
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
                        toastr["error"](data, "温馨敬告");
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $(".loading-btn").removeAttr("disabled");
                }
            });
        }
        function changeRouteList() {
            var teamid = $("#teamid").val();
            $.ajax({
                type: "GET",
                url: rootUri + "Duty/GetRoutList/" + teamid,
                dataType: "json",
                success: function (data) {
                    var rhtml = "";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].routename + "</option>";
                        }
                        $("#routeid").html(rhtml);
                        $("#routeid").css('width', '250px').select2({ allowClear: true });
                    }
                    else {
                        $("#routeid").html(rhtml);
                        $("#routeid").css('width', '250px').select2({ allowClear: true });  
                    }
                }
            });
            changeTrainNoList();
        }
    </script>
</asp:Content>