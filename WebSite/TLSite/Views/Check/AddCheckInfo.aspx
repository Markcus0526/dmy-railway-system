<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var checkinfo = (tbl_checkinfo)ViewData["checkinfo"]; %>
<div class="page-header">
	<h1>
		添加项点
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
            项点
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
				<label class="col-sm-3 control-label no-padding-right" for="category">分类：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="category" name="category" placeholder="请输入分类" class="input-large form-control" <% if (checkinfo != null) { %>value="<%= checkinfo.category %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="checkno">项点编号：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="checkno" name="checkno" placeholder="请输入项点编号" class="input-large form-control" <% if (checkinfo != null) { %>value="<%= checkinfo.checkno %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="checktype">类型：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="checktype" type="radio" value="0" class="ace" <% if (checkinfo == null || (checkinfo != null && checkinfo.checktype == 0)) { %>checked<% } %> />
							    <span class="lbl"> 加分</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="checktype" type="radio" value="1" class="ace" <% if (checkinfo != null && checkinfo.checktype == 1) { %>checked<% } %>/>
							    <span class="lbl"> 扣分</span>
						    </label>
					    </div>
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="chkpoint">考核分数：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="chkpoint" name="chkpoint" placeholder="请输入分数" class="input-large form-control" <% if (checkinfo != null) { %>value="<%= checkinfo.chkpoint %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="relpoint">连挂考核分数：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="relpoint" name="relpoint" placeholder="请输入分数" class="input-large form-control" <% if (checkinfo != null) { %>value="<%= checkinfo.relpoint %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="checkinfo">项点内容：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					    <textarea id="checkinfo" name="checkinfo" style="width:400px; height:200px;"><% if (checkinfo != null) { %><%= checkinfo.checkinfo %><% } %></textarea>
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">排序：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="sortid" name="sortid" placeholder="请输入数字" class="input-large form-control col-xs-10 col-sm-5" <% if (checkinfo != null) { %>value="<%= checkinfo.sortid %>"<% } else { %>value="1"<% } %>  />
                    <span class="help-inline col-xs-12 col-sm-7">
						<span class="middle">数字越小越靠前</span>
					</span>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script type="text/javascript">
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Check/CheckInfoList";
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
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                category: {
	                    required: true
	                },
	                checkno: {
	                    required: true
	                },
	                checktype: {
	                    required: true
	                },
	                chkpoint: {
	                    required: true,
                        number: true
	                },
                    checkinfo: {
                        required: true
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
	                $('.loading-btn').button('reset');
	            }
	        });
	    });

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "Check/SubmitCheckInfo",
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