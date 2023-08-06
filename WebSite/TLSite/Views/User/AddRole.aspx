<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var roleinfo = (tbl_adminrole)ViewData["roleinfo"]; %>
<div class="page-header">
	<h1>
		管理员
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
            管理员
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
				<label class="col-sm-3 control-label no-padding-right" for="rolename">角色名称：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="rolename" name="rolename" placeholder="请输入角色名称" class="input-large form-control" <% if (roleinfo != null) { %>value="<%= roleinfo.rolename %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="rolename">角色：</label>
				<div class="col-sm-5">
                    <div class="clearfix">
                    <table id="listTable" class="table table-bordered table-hover dataTable table-striped" style="clear:right; width:100%; margin:0px; padding:0px;">
                        <tbody>
                            <tr>
                                <td rowspan="7" style="width:200px; text-align:center;">
									<label>
										<input type="checkbox" class="ace setbtn ul-1" data-id="1" 
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Judge") && 
                                        ViewData["role"].ToString().Split(',').Contains("Credit") && 
                                        ViewData["role"].ToString().Split(',').Contains("Document") && 
                                        ViewData["role"].ToString().Split(',').Contains("Mission") && 
                                        ViewData["role"].ToString().Split(',').Contains("Rule") && 
                                        ViewData["role"].ToString().Split(',').Contains("OnlineTest")
                                        ) { %> checked <% } %> />
										<span class="lbl"> 业务管理</span>
									</label>
						        </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="Judge"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Judge")) { %> checked <% } %> />
										<span class="lbl"> 绩效考核</span>
									</label>
							    </td>
				            </tr>
                            <tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="Credit"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Credit")) { %> checked <% } %> />
										<span class="lbl"> 积分查询</span>
									</label>
							    </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="Document"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Document")) { %> checked <% } %> />
										<span class="lbl"> 公文流转</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="Mission"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Mission")) { %> checked <% } %> />
										<span class="lbl"> 任务管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="Rule"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Rule")) { %> checked <% } %> />
										<span class="lbl"> 规章管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-1" data-id="1" value="OnlineTest"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("OnlineTest")) { %> checked <% } %> />
										<span class="lbl"> 在线考试</span>
									</label>
								</td>
				            </tr>
                            <tr>
                                <td rowspan="9" style="width:200px; text-align:center;">
									<label>
										<input type="checkbox" class="ace setbtn ul-2" data-id="2" <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Sector") && 
                                        ViewData["role"].ToString().Split(',').Contains("RailTeam") && 
                                        ViewData["role"].ToString().Split(',').Contains("Route") && 
                                        ViewData["role"].ToString().Split(',').Contains("Group") && 
                                        ViewData["role"].ToString().Split(',').Contains("TrainNo") &&
                                        ViewData["role"].ToString().Split(',').Contains("Contact") &&
                                        ViewData["role"].ToString().Split(',').Contains("Duty") &&
                                        ViewData["role"].ToString().Split(',').Contains("CheckInfo")
                                        ) { %> checked <% } %> />
										<span class="lbl"> 系统数据</span>
									</label>
						        </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="Sector"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Sector")) { %> checked <% } %> />
										<span class="lbl"> 科室管理</span>
									</label>
							    </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="RailTeam"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("RailTeam")) { %> checked <% } %> />
										<span class="lbl"> 车队管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="Route"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Route")) { %> checked <% } %> />
										<span class="lbl"> 线路管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="Group"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Group")) { %> checked <% } %> />
										<span class="lbl"> 班组管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="TrainNo"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("TrainNo")) { %> checked <% } %> />
										<span class="lbl"> 车次管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="CheckInfo"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("CheckInfo")) { %> checked <% } %> />
										<span class="lbl"> 项点管理</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="Contact"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Contact")) { %> checked <% } %> />
										<span class="lbl"> 通讯录</span>
									</label>
								</td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-2" data-id="2" value="Duty"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("Duty")) { %> checked <% } %> />
										<span class="lbl"> 出乘计划</span>
									</label>
								</td>
				            </tr>
                            <tr>
                                <td rowspan="5" style="width:200px; text-align:center;">
									<label>
										<input type="checkbox" class="ace setbtn ul-3" data-id="3" <% if (ViewData["role"] != null && 
                                        ViewData["role"].ToString().Split(',').Contains("UserManage") && 
                                        ViewData["role"].ToString().Split(',').Contains("SysSetting") &&
                                        ViewData["role"].ToString().Split(',').Contains("AdminList") &&
                                        ViewData["role"].ToString().Split(',').Contains("RoleManage")
                                        ) { %> checked <% } %> />
										<span class="lbl"> 系统管理</span>
									</label>
						        </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-3" data-id="3" value="UserManage"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("UserManage")) { %> checked <% } %> />
										<span class="lbl"> 用户管理</span>
									</label>
							    </td>
				            </tr>
                            <tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-3" data-id="3" value="AdminList"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("AdminList")) { %> checked <% } %> />
										<span class="lbl"> 管理员列表</span>
									</label>
							    </td>
				            </tr>
                            <tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-3" data-id="3" value="RoleManage"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("RoleManage")) { %> checked <% } %> />
										<span class="lbl"> 角色管理</span>
									</label>
							    </td>
				            </tr>
							<tr>
							    <td>
									<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-3" data-id="3" value="SysSetting"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("SysSetting")) { %> checked <% } %> />
										<span class="lbl"> 系统设置</span>
									</label>
								</td>
				            </tr>
                            <tr>
                                <td style="text-align:center;">
                                	<label>
										<input name="configuration" type="checkbox" class="ace indbtn li-4" data-id="4" value="TeamAdmin" onclick="swift()"
                                        <% if (ViewData["role"] != null && ViewData["role"].ToString().Split(',').Contains("TeamAdmin")) { %> checked <% } %> />
										<span class="lbl"> 车队管理员</span>
									</label>
                                </td>
                                <td>
                                    <div class="teamdiv" <% if (ViewData["role"] != null && !ViewData["role"].ToString().Split(',').Contains("TeamAdmin")||(ViewData["role"]==null)) { %> style="display:none" <% } %>>
                                	    <label>车队</label>
                                        <select id="teamid" name="teamid">
                                            <option value="0">全部车队</option>
                                             <% if (ViewData["teamlist"] != null)
                                                {
                                                    foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                                    { %>
                                                        <option value="<%= item.uid %>" <% if (roleinfo!= null &&roleinfo.teamid == item.uid) { %>selected<% } %>>
                                                        <%= item.teamname %></option>
                                                <% 
                                                    }
                                                %>
                                            <% } %>
                                        </select>
                                    </div>
                                   <span class="time grey">车队管理员可以管理本车队人员，人员调入调出，添加本车队出乘计划。</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>					
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script type="text/javascript">
	    function swift() {
	        $(".teamdiv").toggle();
        }
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "User/RoleList";
	        }
	    }

	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

	        $.validator.messages.required = "必须要填写";
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        $.validator.messages.equalTo = jQuery.validator.format("密码不一致.");
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                rolename: {
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

	        $(".setbtn").change(function (obj) {
	            var group_name = $(this).data("id");
	            var that = this;
	            $('.li-' + group_name)
					.each(function () {
					    this.checked = that.checked;
					});
	        });

	        $(".indbtn").change(function (obj) {
	            var group_name = $(this).data("id");
	            var check_all = true;

	            $('.li-' + group_name).each(function () {
	                if (this.checked == false) {
	                    check_all = false;
	                }
	            });

	            $('.ul-' + group_name)
	            .each(function () {
	                this.checked = check_all;
	            });

	        });


	    });

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "User/SubmitRole",
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
    </script>
</asp:Content>