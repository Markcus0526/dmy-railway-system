<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userinfo = (tbl_user)ViewData["userinfo"]; %>
<div class="page-header">
	<h1>
		干部
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
            干部
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
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="username">登录名：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="username" name="username" placeholder="请输入登录名" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.username %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="userpwd">密码：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="password" id="userpwd" name="userpwd" placeholder="请输入密码" class="input-large form-control" />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="confirmpwd">确认密码：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="password" id="confirmpwd" name="confirmpwd" placeholder="请确认密码" class="input-large form-control" />
                    </div>
				</div>
			</div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="crewno">工次号：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="crewno" name="crewno" placeholder="请输入工次号" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.crewno %>"<% } %> />
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="policyface">政治面貌：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="policyface" name="policyface" placeholder="请输入政治面貌" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.policyface %>"<% } %> />
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="exectype">干部类型：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="exectype" type="radio" value="<%= ExecType.SectorExec %>" 
                                    class="ace"  <% if (userinfo == null || (userinfo != null && userinfo.exectype == ExecType.SectorExec)) { %>checked<% } %> />
							    <span class="lbl"> <%= ExecType.SectorExec %></span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="exectype" type="radio" value="<%= ExecType.TeamExec %>" class="ace" <% if (userinfo != null && userinfo.exectype == ExecType.TeamExec) { %>checked<% } %>/>
							    <span class="lbl"> <%= ExecType.TeamExec %></span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="exectype" type="radio" value="<%= ExecType.TrainCoach %>" class="ace" <% if (userinfo != null && userinfo.exectype == ExecType.TrainCoach) { %>checked<% } %>/>
							    <span class="lbl"> <%= ExecType.TrainCoach %></span>
						    </label>
					    </div>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="parentid">所属部门：</label>
			    <div class="col-sm-3" style="margin:6px 0px;">
                    <div class="clearfix">
					    <select class="select2" id="parentid" name="parentid" data-placeholder="请选择部门">
                            <% if (ViewData["parentlist"] != null) {
                                   if (userinfo == null || (userinfo != null && userinfo.exectype == ExecType.SectorExec))
                                   {
                                       foreach (var item in (List<tbl_railsector>)ViewData["parentlist"])
                                       { %>
                                        <option value="<%= item.uid %>" <% if (userinfo != null && item.uid == userinfo.execparentid) { %>selected<% } %>><%= item.sectorname %></option>
                                       <% }                                       
                                   }
                                   else if (userinfo != null && (userinfo.exectype == ExecType.TeamExec || userinfo.exectype == ExecType.TrainCoach))
                                   {
                                       foreach (var item in (List<tbl_railteam>)ViewData["parentlist"])
                                       { %>
                                        <option value="<%= item.uid %>"  <% if (userinfo != null && item.uid == userinfo.execparentid) { %>selected<% } %>><%= item.teamname %></option>
                                       <% }
                                   }
                                %>
                            <% 
                                }  %>
				        </select>
                    </div>
			    </div>
                <label class="opinionman <% if (userinfo == null || (userinfo != null && userinfo.exectype != ExecType.TeamExec)) { %>hide<% } %> col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="opinionmanage">管理诉求：</label>
			    <div class="col-sm-4 opinionman <% if (userinfo == null || (userinfo != null && userinfo.exectype != ExecType.TeamExec)) { %>hide<% } %>" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="opinionmanage" type="radio" value="1" 
                                    class="ace"  <% if (userinfo == null || (userinfo != null && userinfo.opinionmanage == 1)) { %>checked<% } %> />
							    <span class="lbl"> 是</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="opinionmanage" type="radio" value="0" 
                                    class="ace" <% if (userinfo != null && userinfo.opinionmanage == 0) { %>checked<% } %>/>
							    <span class="lbl"> 否</span>
						    </label>
					    </div>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" id="groupdiv" style="<% if (userinfo == null || (userinfo != null && userinfo.exectype != ExecType.TrainCoach)) { %>display:none;<% } %>">
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="parentid">所属班组：</label>
			    <div class="col-sm-3" style="margin:6px 0px;">
                    <div class="clearfix">
					    <select class="select2" id="groupid" name="groupid" data-placeholder="请选择班组">
                            <% if (ViewData["grouplist"] != null) {
                                   if (userinfo != null && userinfo.exectype == ExecType.TrainCoach)
                                   {
                                       var groupinfo = (tbl_traingroup)ViewData["groupinfo"];
                                       foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                       { %>
                                        <option value="<%= item.uid %>" <% if (userinfo != null && item.uid == userinfo.crewgroupid) { %>selected<% } %>><%= item.groupname %></option>
                                       <% }                                       
                                   }
                                %>
                            <% 
                                }  %>
				        </select>
                    </div>
			    </div>
            </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="execrole">职务：</label>
			    <div class="col-sm-3" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="execrole" name="execrole" placeholder="请输入姓名" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.execrole %>"<% } %> />
                    </div>
			    </div>

                                <label class="opinionman <% if (userinfo == null || (userinfo != null && userinfo.exectype != ExecType.TeamExec)) { %>hide<% } %> col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="teammanage">车队管理：</label>
			    <div class="col-sm-4 opinionman <% if (userinfo == null || (userinfo != null && userinfo.exectype != ExecType.TeamExec)) { %>hide<% } %>" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="teammanage" type="radio" value="1" 
                                    class="ace"  <% if ( userinfo != null && userinfo.teammanage == 1) { %>checked<% } %> />
							    <span class="lbl"> 是</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="teammanage" type="radio" value="0" 
                                    class="ace" <% if (userinfo == null ||(userinfo != null && userinfo.teammanage == 0)) { %>checked<% } %>/>
							    <span class="lbl"> 否</span>
						    </label>
					    </div>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="realname">真实姓名：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="realname" name="realname" placeholder="请输入姓名" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.realname %>"<% } %> />
                    </div>
			    </div>
		    </div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="company">照片<span class="red">*</span>：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <input type="button" class="btn btn-sm" id="btndetimg" value="选择本人照片" />
                        <img src="<%= ViewData["rootUri"] %>Content/img/loading.gif" style="display:none;" id="loadingimg">
                        <input type="hidden" name="imgurl" id="imgurl" <% if (userinfo != null && !String.IsNullOrEmpty(userinfo.imgurl)) { %> value="<%= userinfo.imgurl %>"<% } %> />
                    </div>
                    <div style="margin:10px 0px;" id="divimglist">
                    <% if (userinfo != null && !String.IsNullOrEmpty(userinfo.imgurl))
                       {
                            %>
                    <div style='float:left; padding:5px;'>
                        <img src="<%= ViewData["rootUri"] %><%= userinfo.imgurl %>" height='180px' onmouseover='over_img(this)' onmouseout='out_img(this)' >
                        <a href='javascript:(0);'><img src='<%= ViewData["rootUri"] %>content/img/imgdel.png' class='close_btn' onclick='removeMe(this, "<%= userinfo.imgurl %>")' onmouseover='over_close(this)' 
                        style='visibility:hidden; margin-top:-100px; margin-left:-10px; width:20px; height:20px;' onmouseout='out_close(this)'></a>
                    </div>

                    <% } %>
                    </div>
				</div>
			</div>

            <div class="form-group">
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="birthday">出生日期：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="input-group col-xs-10 col-sm-3 ">
						    <input class="input-large form-control date-picker" name="birthday" id="birthday" type="text" data-date-format="yyyy-mm-dd"  <% if (userinfo != null) { %>value="<%= String.Format("{0:yyyy-MM-dd}", userinfo.birthday) %>"<% } %>/>
						    <span class="input-group-addon">
							    <i class="fa fa-calendar bigger-110"></i>
						    </span>
					    </div>
                    </div>
			    </div>
            </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="gender">性别：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="gender" type="radio" value="0" class="ace"  <% if (userinfo == null || (userinfo != null && userinfo.gender == 0)) { %>checked<% } %> />
							    <span class="lbl"> 先生</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="gender" type="radio" value="1" class="ace" <% if (userinfo != null && userinfo.gender == 1) { %>checked<% } %>/>
							    <span class="lbl"> 女士</span>
						    </label>
					    </div>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="phonenum">电话号：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
				    <input type="text" id="phonenum" name="phonenum" placeholder="请输入电话号" class="input-large form-control" <% if (userinfo != null) { %>value="<%= userinfo.phonenum %>"<% } %>/>
                    </div>
			    </div>
		    </div>

            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
					<button class="btn btn-info loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-check bigger-110"></i>
						提交
					</button>

					&nbsp; &nbsp; &nbsp;
					<button class="btn" type="reset">
						<i class="ace-icon fa fa-undo bigger-110"></i>
						重置
					</button>
				</div>
			</div>
        </form>
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
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/ajaxupload.js"></script>

	<script type="text/javascript">
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "User/PersonnelList";
	        }
	    }
	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

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
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        $.validator.messages.equalTo = jQuery.validator.format("密码不一致.");
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                username: {
	                    required: true,
	                    uniquename: true
	                },
	                userpwd: {
	                    minlength: 6
                        <% if (ViewData["uid"] == null) { %>
	                    ,required: true
                        <% } %>
	                },
                    crewno: {
                        required: true,
                        uniquecrewno: true
                    },
                    policyface: {
                        required: true
                    },
	                confirmpwd: {
	                    equalTo: "#userpwd"
	                },
	                exectype: {
	                    required: true
	                },
	                parentid: {
	                    required: true
	                },
	                realname: {
	                    required: true
	                },
	                gender: {
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
	                submitform();
	                return false;
	            },
	            invalidHandler: function (form) {
	                $('.loading-btn').button('reset');
	            }
	        });
	        $.validator.addMethod("uniquename", function (value, element) {
	            return checkUserName();
	        }, "用户名已存在");

	        $.validator.addMethod("uniquecrewno", function (value, element) {
	            return checkCrewNo();
	        }, "工次号已存在");

            $("#parentid").change(function(){
                var teamid = $(this).val();
                $.ajax({
                    async: false,
                    type: "GET",
                    url: rootUri + "User/GetSelectedGroupList",
                    dataType: "json",
                    data: {
                        teamid: teamid
                    },
                    success: function (data) {
                        var rhtml = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                            }
                        }
                        $("#groupid").html(rhtml);
                        $("#groupid").css('width', '250px').select2({ allowClear: true });
                    }
                });
            });

	        $('input[name="exectype"]:radio').change(
                function () {
                    var exectype = $(this).val();
                    $.ajax({
                        async: false,
                        type: "GET",
                        url: rootUri + "User/GetExecutiveParentList",
                        dataType: "json",
                        data: {
                            exectype: exectype
                        },
                        success: function (data) {
                            var rhtml = "";
                            if (exectype == "科室干部" || exectype == "车队干部") 
                            {
                                if (data.length > 0) {
                                    for (var i = 0; i < data.length; i++) {
                                        if (exectype == "科室干部") {
                                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].sectorname + "</option>";
                                        } else {
                                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].teamname + "</option>";
                                        }
                                    }
                                    $("#parentid").html(rhtml);
                                    $("#parentid").css('width', '250px').select2({ allowClear: true });

                                    if (exectype == "科室干部") {
                                        $(".opinionman").addClass("hide");
                                    } else {
                                        $(".opinionman").removeClass("hide");
                                    }
                                }
                                $("#groupdiv").hide();
                            } else if (exectype == "列车长") {
                                for (var i = 0; i < data.teamlist.length; i++) {
                                    rhtml += "<option value='" + data.teamlist[i].uid + "'>" + data.teamlist[i].teamname + "</option>";
                                }
                                $("#parentid").html(rhtml);
                                $("#parentid").css('width', '250px').select2({ allowClear: true });

                                rhtml = "";
                                for (var i = 0; i < data.grouplist.length; i++) {
                                    rhtml += "<option value='" + data.grouplist[i].uid + "'>" + data.grouplist[i].groupname + "</option>";
                                }
                                $("#groupid").html(rhtml);
                                $("#groupid").css('width', '250px').select2({ allowClear: true });

                                $("#groupdiv").show();
                                $(".opinionman").addClass("hide");
                            }
                        }
                    });
                }
            );

            new AjaxUpload('#btndetimg', {
		        action: rootUri + 'Upload/UploadImage',
                onSubmit : function(file , ext){
                    $('#loadingimg').show();
                    if (! (ext && /^(JPG|PNG|JPEG|GIF)$/.test( ext.toUpperCase() ))){
                        // extensiones permitidas
		                alert('错误: 只能上传图片', '');
                        $('#loadingimg').hide();
                        return false;
                    } 
                },
                onComplete: function(file, response){
                    $('#loadingimg').hide();
                    var pic_data = "<div style='float:left; padding:5px;'>";
                    pic_data += "<img src='" + rootUri + "Content/uploads/temp/" + response + "' height='180px' onmouseover='over_img(this)' onmouseout='out_img(this)' >";
                    pic_data +=  "<a href='javascript:(0);'><img src='" + rootUri + "content/img/imgdel.png' class='close_btn' onclick='removeMe(this, \""+response+"\")' onmouseover='over_close(this)' style='visibility:hidden; margin-top:-100px; margin-left:-10px; width:20px; height:20px;' onmouseout='out_close(this)'></a>";
                    pic_data += "</div>";
                    $('#divimglist').html( pic_data );
                    $('#imgurl').attr("value", response );
                }
            });

	    });

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "User/SubmitExecutive",
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
	                $('.loading-btn').button('reset');
	            }
	        });
	    }

	    function checkUserName() {
	        var username = $("#username").val();
	        var uid = $("#uid").val();
	        var retval = false;

	        $.ajax({
	            async: false,
	            type: "GET",
	            url: rootUri + "User/CheckUniqueUsername",
	            dataType: "json",
	            data: {
	                username: username,
	                uid: uid
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


    </script>
</asp:Content>