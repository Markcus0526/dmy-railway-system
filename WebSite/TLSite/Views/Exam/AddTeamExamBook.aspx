<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var bookinfo = (ExamBookInfo)ViewData["bookinfo"]; %>
<% var userrole = CommonModel.GetUserRoleInfo(); %>
<style type="text/css">
.dataTables_filter
{
    text-align:left;
    }
</style>
<div class="page-header">
	<h1>
		试卷管理
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
            试卷
		</small>
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
				<label class="col-sm-3 control-label no-padding-right" for="title">试卷名称<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
    					<input type="text" id="title" name="title" placeholder="请输入名称" class="input-large form-control" <% if (bookinfo != null) { %>value="<%= bookinfo.title %>"<% } %> />
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="examtime">考试时间：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
    					<input type="text" id="examtime" name="examtime" placeholder="请输入数字（分钟）" class="input-large form-control" <% if (bookinfo != null) { %>value="<%= bookinfo.examtime.ToString() %>"<% } %> />
                    </div>
                    <span class="help-inline col-xs-12 col-sm-7">
						<span class="middle">请输入分钟单位的数字。</span>
					</span>
				</div>
            </div>
			<div class="form-group" >
                <div class="col-sm-6">
				    <label class="col-sm-6 control-label no-padding-right" for="examkind">试卷用途<span class="red">*</span>：</label>
				    <div class="col-sm-6">
                        <div class="clearfix">
						    <select class="select2" id="examkind" name="examkind" data-placeholder="请选择..."  <% if (userrole != null && ((string)userrole).Contains("TeamManager")) { %> disabled<% } %> >
                                <option value="0" <% if (bookinfo == null || (bookinfo != null && bookinfo.examkind == ExamKind.Sector)) { %> selected<% } %> >段级考试</option>
                                <option value="1" <% if ((userrole != null && ((string)userrole).Contains("TeamManager"))||(bookinfo != null && bookinfo.examkind == ExamKind.Team)) { %> selected<% } %>>车队考试</option>
				            </select>
                        </div>
				    </div>
                     </br>
                    </br>
                    </br>
                    </br>
                    <label class="col-sm-6 control-label no-padding-right hide" for="examkind">车队<span class="red">*</span>：</label>
				    <div class="col-sm-6">
                        <div class="clearfix">
						    <input class="hide" id="teamid" name="teamid" value="<%=ViewData["userteamid"] %>" data-placeholder="请选择..." />

                        </div>
				    </div>
                </div>
				<label class="col-sm-1 control-label no-padding-right" for="examkind">考试内容<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
                        <textarea style="width:100%; height:130px;" id="contents" name="contents"><% if (bookinfo != null) { %><%= bookinfo.contents %><% } %></textarea>
                    </div>
				</div>
            </div>

            <div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for=""></label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <a class="btn btn-white btn-default btn-round" href="#modal-table" role="button" data-toggle="modal" onclick="return AddExam()">
	                        <i class="ace-icon fa fa-plus red2"></i>
	                        添加考题
                        </a>
                    </div>
				</div>
			</div>

            <h3 class="header smaller lighter green">下方列表显示考题</h3>
		    <div>
			    <table id="tblspec" class="table table-striped table-bordered table-hover">
				    <thead>
					    <tr>
						    <th style="min-width:90px;">试题用途</th>
                            <th style="min-width:90px;">类型</th>
						    <th>试题提问</th>
						    <th style="min-width:80px;">操作</th>
					    </tr>
				    </thead>
				    <tbody>
				    </tbody>
			    </table>
		    </div>

            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />
            <input type="hidden" id="iscont" name="iscont" value="0" />

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
                    <button class="btn btn-sm btn-purple loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-floppy-o bigger-125"></i>
						提交
					</button>
					&nbsp; &nbsp; &nbsp;
                    <button class="btn btn-sm loading-btn btn-purple" type="button" onclick="return submutandedit()">
						<i class="ace-icon fa fa-pencil-square-o bigger-125"></i>
						保存及继续编辑
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

<div id="modal-table" class="modal fade" tabindex="-1">
	<div class="modal-dialog" style="width:90%">
		<div class="modal-content">
			<div class="modal-header no-padding">
				<div class="table-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">
						<span class="white">&times;</span>
					</button>
					请选择考题
				</div>
			</div>
			<div class="modal-body no-padding">
				<table id="tblexamlib" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top">
					<thead>
						<tr>
						    <th class="center">
							    <label class="position-relative">
								    <input type="checkbox" class="ace" />
								    <span class="lbl"></span>
							    </label>
						    </th>
						    <th style="min-width:90px;">试题用途</th>
                            <th style="min-width:90px;">类型</th>
						    <th>试题提问</th>
						    <th style="min-width:90px;">创造日期</th>
						</tr>
					</thead>
					<tbody>
						
					</tbody>
				</table>
			</div>

			<div class="modal-footer no-margin-top">
				<button class="btn btn-sm btn-danger pull-left" data-dismiss="modal" onclick="submit_dataitem();">
					<i class="ace-icon fa fa-save"></i>
					确认
				</button>
			</div>

            <input type="hidden" id="dataid" value=""/>

		</div><!-- /.modal-content -->
	</div><!-- /.modal-dialog -->
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/jquery-ui.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/uploadify/uploadify.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>

	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/uploadify/jquery.uploadify.min.js"></script>  

	<script type="text/javascript">
	    var specTable;
        var tblexamlib;
        var examids = "<%= ViewData["examids"] %>";

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
                $(".loading-btn").removeAttr("disabled");
	        } else {
	            window.location = rootUri + "Exam/TeamExamBookList";
	        }
	    }
	    jQuery(function ($) {
	        $(".select2").css('width', '200px').select2({ allowClear: true })
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
	        $.validator.messages.equalTo = jQuery.validator.format("密码不一致.");

	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                title: {
	                    required: true
	                },
	                examtime: {
	                    required: true,
	                    number: true
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
	                //$('.loading-btn').button('reset');
                    $(".loading-btn").removeAttr("disabled");
	            }
	        });
	        $.validator.addMethod("uniquename", function (value, element) {
	            return checkBookName();
	        }, "用户名已存在");

	        specTable =
			$('#tblspec')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": false,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Exam/RetrieveSelectedBookExam?examids=" + examids,
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
                //"bPaginate":false,
			    "iDisplayLength": 10,
			    "aoColumnDefs": [
				    {
				        aTargets: [0],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rst = "";
                            if(o.aData[0] == "0") {
                                rst = "段级考试";
                            } else {
                                rst = "车队考试";
                            }
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [3],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[3] + ')">' +
                                '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
                                '</a>';
				            return rst;
				        },
				        sClass: 'center'
				    }
                ],
			    "fnDrawCallback": function (oSettings) {
			        //showBatchBtn();
			    }
			});


            
	        tblexamlib =
			$('#tblexamlib')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": true,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Exam/RetrieveTeamExamList",
			    "oLanguage": {
			        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
			    },
			    //bAutoWidth: false,
			    "aoColumns": [
					  { "bSortable": false },
					  null,
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
				],
			    "iDisplayLength": 10,
			    "aoColumnDefs": [
				    {
				        aTargets: [0],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            return '<label class="position-relative">' +
								'<input type="checkbox" value="' + o.aData[0] + '" name="selcheckbox" class="ace" />' +
								'<span class="lbl"></span>' +
							    '</label>';
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [1],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rst = "";
                            if(o.aData[1] == "0") {
                                rst = "段级考试";
                            } else {
                                rst = "车队考试";
                            }
				            return rst;
				        },
				        sClass: 'center'
				    }
                ],
//                "fnDrawCallback": function () { $("#tblexamlib_wrapper").children("div.col-xs-6").append("<select> 试题种类</select>");},
			    "fnInitComplete": function (oSettings) {
                     $("#tblexamlib_wrapper").children(".row:first").children(".col-xs-6:first").append(
                     "试题类型"+
                     "<select id='testtype' onchange='changetesttype()'>"+
                     "<option value='0'>所有试题</value>"+
                     "<option value='1'>单选题</value>"+
                     "<option value='2'>多选题</value>"+
                     "<option value='3'>判断题</value>"+
                     "</select>"
                     );
                     $("#tblexamlib_wrapper").children(".row:first").children(".col-xs-6:first").append(
                     "试题数量"+
                     "<input type='text' id='testamount'/>"
                     );

                     $("#tblexamlib_wrapper").children(".row:first").children(".col-xs-6:first").append(
                     "<a class='btn btn-white btn-default btn-round' onclick='randomgenerate()'>"+
	                 "随机选择"+
                     "</a>"
                     );
			    }
			});

		    $(document).on('click', '#tblexamlib th input:checkbox', function () {
		        var that = this;
		        $(this).closest('table').find('tr > td:first-child input:checkbox')
				.each(function () {
					this.checked = that.checked;
					$(this).closest('tr').toggleClass('selected');
				});

		        showBatchBtn();
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
	    });

        function changetesttype(){
              if(tblexamlib!=null){
                 var oSettings = tblexamlib.fnSettings();                 
                 oSettings.sAjaxSource = rootUri + "Exam/SerchTeamExamList?testtype="+$("#testtype").val();
                 tblexamlib.fnClearTable(0);
                 tblexamlib.fnDraw();
              }
        }
        function randomgenerate(){
            var testamount=$("#testamount").val();
            if(tblexamlib!=null){
                 var oSettings = tblexamlib.fnSettings();                 
                 oSettings.sAjaxSource = rootUri + "Exam/GenerateTeamExamDataTable?testtype="+$("#testtype").val()+"&testamount="+testamount;
                // oSettings.bPaginate=testamount;
                 tblexamlib.fnClearTable(0);
                 tblexamlib.fnDraw();
              }
        }
		function showBatchBtn() {
		}

	    function submitform() {
            $(".loading-btn").attr("disabled", "disabled");
	        $.ajax({
	            type: "POST",
	            url: rootUri + "Exam/SubmitExamBook?examkind=" + $("#examkind").val() + "&examids=" + examids,
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
	                } else {
	                    if ($("#iscont").val() == "0") {
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
	                    } else {
	                        window.location.href = rootUri + "Exam/EditExam/" + data;
	                    }

	                }
	            },
	            error: function (data) {
	                alert("Error: " + data.status);
                    $(".loading-btn").removeAttr("disabled");
	            }
	        });
	    }

	    function checkBookName() {
	        var title = $("#title").val();
	        var retval = false;

	        $.ajax({
	            async: false,
	            type: "GET",
	            url: rootUri + "Exam/CheckUniqueBookName",
	            dataType: "json",
	            data: {
	                title: title,
	                uid: $("#uid").val()
	            },
	            success: function (data) {
	                if (data == true) {
	                    retval = true;
	                } else {
	                    retval = false;
	                }
	            }
	        });

	        return retval;
	    }

	    function checkUserName() {
	        var username = $("#username").val();
	        var retval = false;

	        $.ajax({
	            async: false,
	            type: "GET",
	            url: rootUri + "User/CheckUniqueUserName",
	            dataType: "json",
	            data: {
	                username: username,
	                uid: $("#crewuid").val()
	            },
	            success: function (data) {
	                if (data == true) {
	                    retval = true;
	                } else {
	                    retval = false;
	                }
	            }
	        });

	        return retval;
	    }

	    var crewdlg;
	    function AddExam() {

	        crewdlg = $("#dialog-message").removeClass('hide').dialog({
	            modal: true,
	            width: 500,
	            title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> 添加成员</h4></div>",
	            title_html: true,
	            buttons: [
					{
					    text: "确定",
					    "class": "btn btn-primary btn-xs",
					    click: function () {
					        $("#form_crew").submit();
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

        function submutandedit() {
            $("#iscont").attr("value", "1");
            $("#validation-form").submit();
        }

		function processDel(sel_id) {
		    bootbox.dialog({
		        message: "您确定要删除该考题吗？",
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
                            var idlist = examids.split(',');
                            var exids = "";
                            for (var i = 0; i < idlist.length; i++)
                            {
                                if (idlist[i] != sel_id)
                                {
                                    if (exids != "") {
                                        exids += ",";
                                    }
                                    exids += idlist[i];
                                }
                            }
    
                            examids = exids;
                            showSelectedExamList();

		                }
		            }
		        }
		    });
		    return false;
		}

        function submit_dataitem()
        {
            var selected_id = "";
		    $(':checkbox:checked').each(function () {
		        if ($(this).attr('name') == 'selcheckbox') {
                    if (selected_id != "") {
                        selected_id += ",";
                    }
		            selected_id += $(this).attr('value');
                }
		    });

            examids = examids + "," + selected_id;
            showSelectedExamList();
        }

        function showSelectedExamList() {
            oSettings = specTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Exam/RetrieveSelectedBookExam?examids=" + examids;

            specTable.dataTable().fnDraw();

        }


    </script>
</asp:Content>