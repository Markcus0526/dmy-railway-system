<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var groupinfo = (tbl_traingroup)ViewData["groupinfo"]; %>
<div class="page-header">
	<h1>
		班组管理
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
            班组
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
				<label class="col-sm-3 control-label no-padding-right" for="teamid">车队<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="teamid" name="teamid" data-placeholder="请选择车队...">
                            <% 
                                if (ViewData["teamlist"] != null)
                                {
                                    foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                    { %>
                            <option value="<%= item.uid %>"  <%if(groupinfo!=null &&groupinfo.teamid== item.uid){%>selected<%} %> ><%= item.teamname%></option>
                            <% }
                                } %>
				        </select>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="groupname">班组名称：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
					<input type="text" id="groupname" name="groupname" placeholder="请输入班组" class="input-large form-control" <% if (groupinfo != null) { %>value="<%= groupinfo.groupname %>"<% } %> />
                    </div>
				</div>
            </div>
			<div class="form-group" style="display:none;">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">排序：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="sortid" name="sortid" placeholder="请输入数字" class="input-large form-control col-xs-10 col-sm-5" <% if (groupinfo != null) { %>value="<%= groupinfo.sortid %>"<% } else { %>value="1"<% } %>  />
                    <span class="help-inline col-xs-12 col-sm-7">
						<span class="middle">数字越小越靠前</span>
					</span>
                    </div>
				</div>
			</div>

            <div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for=""></label>
				<div class="col-sm-2">
                    <div class="clearfix">
                        <a class="btn btn-white btn-default btn-round" onclick="AddCrew()">
	                        <i class="ace-icon fa fa-plus red2"></i>
	                        添加成员
                        </a>
                    </div>
				</div>
                <label class="col-sm-3 control-label no-padding-right">
				    <input name="dailygroup" type="checkbox" class="ace indbtn "  value="1"
                    <% if (groupinfo!= null && groupinfo.dailygroup==1) { %> checked <% } %> />
				    <span class="lbl"> 是否为日勤</span>
			    </label>
			</div>

            <h3 class="header smaller lighter green">下方列表显示班组成员</h3>
		    <div>
			    <table id="tblspec" class="table table-striped table-bordered table-hover">
				    <thead>
					    <tr>
						    <th>工次号</th>
						    <th>政治面貌</th>
						    <th>照片</th>
						    <th>角色</th>
						    <th>姓名</th>
						    <th>账号</th>
						    <th>出生日期</th>
						    <th>性别</th>
						    <th>电话号</th>
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

<div id="dialog-message" class="hide">
    <form class="form-horizontal" role="form" id="form_crew">
        <div class="col-xs-12">
		    <div class="form-group">
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="crewroleid">角色：</label>
			    <div class="col-sm-8" style="margin:6px 0px;" >
                    <div class="clearfix">
					    <select class="select2" id="crewroleid" name="crewroleid" data-placeholder="请选择角色">
                            <% if (ViewData["crewrolelist"] != null) {
                                    foreach (var item in (List<tbl_crewrole>)ViewData["crewrolelist"])
                                    {
                                %>
                                <option value="<%= item.uid %>"><%= item.rolename %></option>
                            <% 
                                    }
                                }  %>
				        </select>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="crewno">工次号：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
                        <input type="text" id="crewno" name="crewno" placeholder="请输入工次号" class="input-large form-control" />
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="policyface">政治面貌：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
                        <input type="text" id="policyface" name="policyface" placeholder="请输入政治面貌" class="input-large form-control" />
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="username">账号：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="username" name="username" placeholder="请输入登录名" class="input-large form-control" />
                    <span class="help-inline col-xs-12 col-sm-12">
					    <span class="middle">注意：默认密码设为跟账号一样！</span>
				    </span>
                    </div>
			    </div>
		    </div>
			<div class="form-group">
				<label class="col-sm-4 control-label no-padding-right" for="userpwd">密码：</label>
				<div class="col-sm-8">
                    <div class="clearfix">
					<input type="password" id="userpwd" name="userpwd" placeholder="请输入密码" class="input-large form-control" />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-4 control-label no-padding-right" for="confirmpwd">确认密码：</label>
				<div class="col-sm-8">
                    <div class="clearfix">
					<input type="password" id="confirmpwd" name="confirmpwd" placeholder="请确认密码" class="input-large form-control" />
                    </div>
				</div>
			</div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="realname">真实姓名：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="realname" name="realname" placeholder="请输入姓名" class="input-large form-control" />
                    </div>
			    </div>
		    </div>
			<div class="form-group">
				<label class="col-sm-4 control-label no-padding-right" for="company">照片<span class="red">*</span>：</label>
				<div class="col-sm-8">
                    <div class="clearfix">
                        <input class="" type="file" id="input_videofile" />
                        <div id="filename1"></div>
                        <input type="hidden" id="path" name="path" value="" />
                        <input type="hidden" id="filesize" name="filesize" value="0" />
                        <input type="hidden" id="filename" name="filename" />
                    </div>
				</div>
			</div>

            <div class="form-group">
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="birthday">出生日期：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="input-group col-xs-10 col-sm-4 ">
						    <input class="date-picker" name="birthday" id="birthday" type="text" data-date-format="yyyy-mm-dd" />
						    <span class="input-group-addon">
							    <i class="fa fa-calendar bigger-110"></i>
						    </span>
					    </div>
                    </div>
			    </div>
            </div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="gender">性别：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="gender" type="radio" value="0" class="ace" checked />
							    <span class="lbl"> 先生</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="gender" type="radio" value="1" class="ace" />
							    <span class="lbl"> 女士</span>
						    </label>
					    </div>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-4 control-label no-padding-right" style="margin:6px 0px;" for="phonenum">电话号：</label>
			    <div class="col-sm-8" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="phonenum" name="phonenum" placeholder="请输入电话号" class="input-large form-control" />
                    </div>
			    </div>
		    </div>
        </div>
        <input type="hidden" id="crewuid" name="crewuid" value="0" />
    </form>
</div><!-- #dialog-message -->

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
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
                $(".loading-btn").removeAttr("disabled");
	        } else {
	            window.location = rootUri + "Group/GroupList";
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
	                teamid: {
	                    required: true,
	                    number: true
	                },
	                groupname: {
	                    required: true,
	                    uniquename: true
	                },
	                sortid: {
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
	            return checkGroupName();
	        }, "用户名已存在");

	        specTable =
			$('#tblspec')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": false,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Group/RetrieveGroupCrewList/" + $("#uid").val(),
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
					{ "bSortable": false },
					{ "bSortable": false },
					{ "bSortable": false }
				],
			    "iDisplayLength": 10,
			    "aoColumnDefs": [
				    {
				        aTargets: [2],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = "";
				            if (o.aData[2] == "" || o.aData[2] == null) {
				                rst = "<img src='" + rootUri + "content/img/profile-pic.jpg' style='height:80px;' />";
				            } else {
				                rst = "<img src='" + rootUri + o.aData[2] + "' style='height:80px;' />";
                            }
				            return rst;
				        }
				    },
				    {
				        aTargets: [7],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = "";
				            if (o.aData[7] == "0") {
				                rst = "先生";
				            } else if (o.aData[7] == "1") {
				                rst = "女士";
				            }
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [9],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '<a class="btn btn-xs btn-info" onclick="processEdit(' + o.aData[9] + ')">' +
                                '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                '</a>&nbsp;&nbsp;';
				            rst += '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[9] + ')">' +
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

	        //override dialog's title function to allow for HTML titles
	        $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
	            _title: function (title) {
	                var $title = this.options.title || '&nbsp;'
	                if (("title_html" in this.options) && this.options.title_html == true)
	                    title.html($title);
	                else title.text($title);
	            }
	        }));

	        $('#form_crew').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            ignore: [],
	            //focusInvalid: false,
	            rules: {
	                crewroleid: {
	                    required: true
	                },
	                crewno: {
	                    required: true,
                        uniquecrewno: true
	                },
	                policyface: {
	                    required: true
	                },
	                username: {
	                    required: true,
	                    uniquecrew: true
	                },
	                userpwd: {
	                    minlength: 6
	                },
	                confirmpwd: {
	                    equalTo: "#userpwd"
	                },
	                realname: {
	                    required: true
	                },
	                phonenum: {
	                    required: true,
	                    minlength: 11,
	                    maxlength: 11
	                },
	                birthday: {
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
	                $.ajax({
	                    type: "POST",
	                    url: rootUri + "Group/InsertOrUpdateCrew/" + $("#uid").val(),
	                    dataType: "json",
	                    data: $('#form_crew').serialize(),
	                    success: function (data) {
	                        specTable.dataTable().fnDraw();
	                        crewdlg.dialog("close");
	                    },
	                    error: function (data) {
	                        alert("Error: " + data);
	                    }
	                });
	                return false;
	            },
	            invalidHandler: function (form) {
	            }
	        });
	        $.validator.addMethod("uniquecrew", function (value, element) {
	            return checkUserName();
	        }, "用户名已存在");

	        $.validator.addMethod("uniquecrewno", function (value, element) {
	            return checkCrewNo();
	        }, "工次号已存在");

	        $('#input_videofile').uploadify({
	            'buttonText': '选择照片',
	            //'queueSizeLimit': 1,  //设置上传队列中同时允许的上传文件数量，默认为999
	            'multi': false,
	            'uploadLimit': 0,   //设置允许上传的文件数量，默认为999
	            'swf': rootUri + 'Content/plugins/uploadify/uploadify.swf',

	            //'fileTypeExts': '*.flv;*.mp4;*.mpeg;*.avi;',
	            //'fileTypeDesc': 'Video Files (.flv,.mp4,.mpeg,.avi)',
	            'fileSizeLimit': '20MB',

	            'uploader': rootUri + 'Upload/UploadPortrait',
	            'onUploadComplete': function (file) {   //单个文件上传完成时触发事件
	                //alert('The file ' + file.name + ' finished processing.');
	            },
	            'onQueueComplete': function (queueData) {   //队列中全部文件上传完成时触发事件
	                //alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
	            },
	            'onUploadSuccess': function (file, data, response) {    //单个文件上传成功后触发事件
                    var rstinfo = $.parseJSON(data);
	                $("#filename").val(file.name);
	                $("#path").val(rstinfo.imgpath);
	                $("#filesize").val(file.size);

                    var pic_data = "<div style='float:left; padding:5px;'>";
                    pic_data += "<img src='" + rootUri + rstinfo.imgpath + "' height='180px' >";
                    pic_data += "</div>";

                    $("#filename1").html(pic_data);
	            }
	        });

	    });

	    function submitform() {
            $(".loading-btn").attr("disabled", "disabled");

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Group/SubmitGroup",
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
	                    } else  {
                            if (data=="该车队已有日勤组") {
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
                            else{
	                        window.location.href = rootUri + "Group/EditGroup/" + data;
                            }
	                    }
	                }
	            },
	            error: function (data) {
	                alert("Error: " + data.status);
                    $(".loading-btn").removeAttr("disabled");
	            }
	        });
	    }

	    function checkGroupName() {
	        var groupname = $("#groupname").val();
	        var retval = false;

	        $.ajax({
	            async: false,
	            type: "GET",
	            url: rootUri + "Group/CheckUniqueGroupname",
	            dataType: "json",
	            data: {
	                groupname: groupname,
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

	    function checkCrewNo() {
	        var crewno = $("#crewno").val();
	        var retval = false;

	        $.ajax({
	            async: false,
	            type: "GET",
	            url: rootUri + "User/CheckUniqueCrewNo",
	            dataType: "json",
	            data: {
	                crewno: crewno,
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
	    function AddCrew() {
	        var uid = $("#uid").val();
	        if (uid == 0) {
	            alert("请先保存，保存以后才能添加。");
	            return false;
	        }

	        $("#crewuid").val(0);
	        $("#crewroleid").select2();
	        $("#crewno").val("");
	        $("#policface").val("");
	        $("#username").val("");
	        $("#realname").val("");
	        $("#birthday").datepicker('setDate', new Date());
	        $("#phonenum").val("");
            $("#path").val("");

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

        function processEdit(editid) {
            $.ajax({
                async: false,
                type: "GET",
                url: rootUri + "Group/GetGroupCrewInfo/" + editid,
                dataType: "json",
                data: {
                    groupid: $("#uid").val()
                },
                success: function (data) {
                    $("#crewuid").val(data.uid);
                    $("#crewroleid").select2("val", data.roleid);
                    $("#crewno").val(data.crewno);
                    $("#policyface").val(data.policyface);
                    $("#username").val(data.username);
                    $("#realname").val(data.realname);
                    $("#birthday").datepicker('setDate', data.birthdaystr);
                    var $radios = $("input:radio[name=gender]");
                    $radios.filter('[value=' + data.gender + ']').prop('checked', true);
                    $("#phonenum").val(data.phonenum);

	                $("#path").val(data.imgurl);

                    var pic_data = "<div style='float:left; padding:5px;'>";
                    pic_data += "<img src='" + rootUri + data.imgurl + "' height='180px' >";
                    pic_data += "</div>";

                    $("#filename1").html(pic_data);


                    crewdlg = $("#dialog-message").removeClass('hide').dialog({
                        modal: true,
                        width: 500,
                        title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> 编辑成员</h4></div>",
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
            });
        }

        function submutandedit() {
            $("#iscont").attr("value", "1");
            $("#validation-form").submit();
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
		            message: "您确定要删除该成员吗？",
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
		                            url: rootUri + "Group/DeleteGroupCrew/<%= ViewData["uid"] %>",
		                            data: {
		                                "delids": 
                                        _id
		                            },
		                            type: "post",
		                            success: function (message) {
                                        specTable.dataTable().fnDraw();
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