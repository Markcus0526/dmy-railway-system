<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		职工诉求
        <% 
            UserModel uModel = new UserModel();
            long myid = CommonModel.GetSessionUserID();
            var userinfo = uModel.GetUserById(myid);
        
            if (userinfo.userkind == (int)UserKind.ADMIN || (userinfo.userkind == (int)UserKind.EXECUTIVE && userinfo.exectype == ExecType.TeamExec))
            {
            %>
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Opinion/OpinionList" style="float:right">
		    <i class="ace-icon fa fa-list red2"></i>
		    查看诉求
	    </a>
        <% } %>
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Opinion/MyOpinion"
        style="float:right"> <i class="ace-icon fa fa-list red2"></i>我的诉求 </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="row">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
            <p class="alert alert-info" style="font-family:'仿宋'; font-size:18px">
	            <b style="text-align:center;">职工投诉须知：</b><br />
                <b>一、</b>“锦州客运段职工诉求中心”在24小时接收职工的咨询、意见建议和举报投诉。本系统为实名制登陆进行诉求信息发布，要求职工发布诉求必须本着实事求是，不得恶意捏造或者歪曲事实，散布谣言；公开侮辱他人、捏造事实诽谤他人或者恶意攻击他人。<br />
                <b>二、</b>诉求件办理时限:<br />
                1、咨询类诉求件：自段及车队接收到职工诉求信息之日起，在3个工作日内给予回复；<br />
                2、 投诉、举报、建议及感谢类诉求件：自段及车队接收到职工诉求信息之日起，在7个工作日内给予回复。 <br />
            </p>
            </div>
            <div class="col-xs-2"></div>
        </div>
		<form class="form-horizontal" role="form" id="validation-form">
			<div class="form-group" >
				<label class="col-sm-2 control-label no-padding-right" for="receiver">提交部门：</label>
				<div class="col-sm-7">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="examkind" type="radio" value="0" class="ace"  checked />
							    <span class="lbl"> 段级</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="examkind" type="radio" value="1" class="ace" />
							    <span class="lbl"> 车队</span>
						    </label>
					    </div>
                    </div>
				</div>
			</div>

			<div class="form-group" >
				<label class="col-sm-2 control-label no-padding-right" for="title">标题：</label>
				<div class="col-sm-8">
                    <div class="clearfix" style="width:100%">
                        <input type="text" id="title" name="title" placeholder="请输入标题" class="col-xs-10 col-sm-12">
                    </div>
				</div>
			</div>

			<div class="form-group" >
				<label class="col-sm-2 control-label no-padding-right" for="contents">诉求内容：</label>
				<div class="col-sm-8">
                    <div class="clearfix" style="width:100%">
                        <textarea class="form-control" id="contents" name="contents" style="height:200px; width:100%;"><% if (ViewData["contents"] != null) { %><%= ViewData["contents"]%><% } %></textarea>
                    </div>
				</div>
			</div>

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
					<button class="btn btn-info loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-check bigger-110"></i>
						提交
					</button>

					&nbsp; &nbsp; &nbsp;
					<button class="btn" type="reset" >
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
        var editor;
        var chosentag;
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
                ignore: [],
	            //focusInvalid: false,
	            rules: {
	                receiver: {
	                    required: true
	                },
                    title: {
                        required: true
                    },
                    contents:
                    {
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
	            url: rootUri + "Opinion/SubmitOpinion",
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

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Opinion/AddOpinion";
	        }
	    }

    </script>
</asp:Content>