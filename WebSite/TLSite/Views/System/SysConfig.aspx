<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var configinfo = (tbl_sysconfig)ViewData["sysconfig"]; %>
<div class="page-header">
	<h1>
		站点配置
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
			<div class="form-group" <% if (ViewData["uid"] != null) { %>style="display:none"<% } %>>
				<label class="col-sm-3 control-label no-padding-right" for="username">系统公告：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<textarea class="form-control" id="notice" name="notice" style="height:260px; width:540px;"><% if (configinfo != null) { %><%= configinfo.notice %> <% } %></textarea>
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="company">首页背景图片：<span class="red">*</span>：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <input type="button" class="btn btn-sm" id="btndetimg" value="选择图片" />
                        <img src="<%= ViewData["rootUri"] %>Content/img/loading.gif" style="display:none;" id="loadingimg">
                        <input type="hidden" name="imgurl" id="imgurl" <% if (configinfo != null) { %> value="<%= configinfo.backimg %>"<% } %> />
                    </div>
                    <div style="margin:10px 0px;" id="divimglist">
                        <% if (configinfo != null)
                           { %>
	                    <div style='float:left; padding:5px;'>
	                    <img src='<%= ViewData["rootUri"] %><%= configinfo.backimg %>' height='180px' >
                        </div>
                        <% } %>
                    </div>
				</div>
			</div>

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
					<button class="btn btn-info loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-check bigger-110"></i>
						提交
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
    <script src="<%= ViewData["rootUri"] %>Content/js/ajaxupload.js"></script>

	<script type="text/javascript">
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location.reload();
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

	        new AjaxUpload('#btndetimg', {
	            action: rootUri + 'Upload/UploadImage',
	            onSubmit: function (file, ext) {
	                $('#loadingimg').show();
	                if (!(ext && /^(JPG|PNG|JPEG|GIF)$/.test(ext.toUpperCase()))) {
	                    // extensiones permitidas
	                    alert('错误: 只能上传图片', '');
	                    $('#loadingimg').hide();
	                    return false;
	                }
	            },
	            onComplete: function (file, response) {
	                $('#loadingimg').hide();
	                var pic_data = "<div style='float:left; padding:5px;'>";
	                pic_data += "<img src='" + rootUri + "Content/uploads/temp/" + response + "' height='180px' onmouseover='over_img(this)' onmouseout='out_img(this)' >";
	                pic_data += "<a href='javascript:(0);'><img src='" + rootUri + "content/img/imgdel.png' class='close_btn' onclick='removeMe(this, \"" + response + "\")' onmouseover='over_close(this)' style='visibility:hidden; margin-top:-100px; margin-left:-10px; width:20px; height:20px;' onmouseout='out_close(this)'></a>";
	                pic_data += "</div>";
	                $('#divimglist').html(pic_data);
	                $('#imgurl').attr("value", response);
	            }
	        });

	    });

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "System/SubmitConfig",
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