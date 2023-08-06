<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var contactinfo = (tbl_contact)ViewData["contactinfo"]; %>
<div class="page-header">
	<h1>
		通讯录
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
            通讯录
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
				<label class="col-sm-3 control-label no-padding-right" for="name">姓名：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="name" name="name" placeholder="请输入姓名" class="input-large form-control" <% if (contactinfo != null) { %>value="<%= contactinfo.contactname %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">类型：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                    <select class="form-control input-large" id="contactkind" name="contactkind">
                        <option value="科室" <% if (contactinfo != null && contactinfo.contactkind == "科室") { %>checked<% } %>>科室</option>
                        <option value="车队" <% if (contactinfo != null && contactinfo.contactkind == "车队") { %>checked<% } %>>车队</option>
                    </select>
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">部门名称：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="partname" name="partname" placeholder="请输入部门名称" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.partname %>"<% } %>/>
                    <span class="help-inline col-xs-12 col-sm-7">
						<span class="middle">例子：“车队1”或者“科室1”</span>
					</span>
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">班组名称：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="groupname" name="groupname" placeholder="请输入班组名称" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.groupname %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="trainno">车次：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="trainno" name="trainno" placeholder="请输入车次" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.trainno %>"<% } %>/>
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="phonenum1">手机号1：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="phonenum1" name="phonenum1" placeholder="请输入手机号1" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.phonenum1 %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="phonenum2">手机号2：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="phonenum2" name="phonenum2" placeholder="请输入手机号2" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.phonenum2 %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="shortnum1">小号1：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="shortnum1" name="shortnum1" placeholder="请输入小号1" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.shortpnum1 %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="shortnum2">小号2：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="shortnum2" name="shortnum2" placeholder="请输入小号2" class="input-large form-control" <% if (contactinfo != null) { %>value="<%= contactinfo.shortpnum2 %>"<% } %> />
                    </div>
				</div>
			</div>

			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="note">备注：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="note" name="note" placeholder="请输入备注" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.note %>"<% } %> />
                    </div>
				</div>
			</div>

			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="rolename">职务：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="rolename" name="rolename" placeholder="请输入职务" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.rolename %>"<% } %> />
                    </div>
				</div>
			</div>

			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="rolekind">包保分工：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="rolekind" name="rolekind" placeholder="请输入包保分工" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.rolekind %>"<% } %> />
                    </div>
				</div>
			</div>

			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="linenum">办公电话：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="linenum" name="linenum" placeholder="请输入办公电话" class="input-large form-control"  <% if (contactinfo != null) { %>value="<%= contactinfo.linenum %>"<% } %> />
                    </div>
				</div>
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/chosen.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>

	<script type="text/javascript">
	    var editor;
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Contact/ContactList";
	        }
	    }

	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

	        $.validator.messages.required = "必须要填写";
	        $.validator.messages.number = jQuery.validator.format("请输入一个有效的数字.");
	        $.validator.messages.url = jQuery.validator.format("请输入有效的地址");
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");

	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                name: {
	                    required: true
	                },
	                contactkind: {
	                    required: true
	                },
	                partname: {
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
	    });

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "Contact/SubmitContact",
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