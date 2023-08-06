<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		批量导入
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<input id="userrole" value="<%= ViewData["userrole"]%>" style="display:none"/>
<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
            <div class="form-group">
                <div class="col-sm-12">
                <div class="btn-group" style="float:right">
				    <a class="btn btn-info btn-white" href="<%= ViewData["rootUri"] %>Content/files/single_multiple.xls">下载xls导入格式</a>	
				    <button data-toggle="dropdown" class="btn btn-info btn-white dropdown-toggle">
					    <span class="ace-icon fa fa-caret-down icon-only"></span>
				    </button>

				    <ul class="dropdown-menu dropdown-info dropdown-menu-right">
					    <li>
						    <a href="<%= ViewData["rootUri"] %>Content/files/single_multiple.xls">单选、多选题</a>
					    </li>
					    <li>
						    <a href="<%= ViewData["rootUri"] %>Content/files/yesno.xls">判断题</a>
					    </li>
				    </ul>
			    </div>
                </div>
            </div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="company">导入文件<span class="red">*</span>：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <input type="button" class="btn btn-sm" id="btndetimg" value="选择文件" />
                        <img src="<%= ViewData["rootUri"] %>Content/img/loading.gif" style="display:none;" id="loadingimg">
                        <input type="hidden" name="fileurl" id="fileurl" />
                    </div>
                    <div style="margin:10px 0px;" id="divimglist"></div>
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
    <script src="<%= ViewData["rootUri"] %>Content/js/ajaxupload.js"></script>

	<script type="text/javascript">
	    var userrole = $("#userrole").val();
	    var specTable;
	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

		      $.validator.messages.required = "必须要填写";
		      $('#validation-form').validate({
		          errorElement: 'span',
		          errorClass: 'help-block',
		          rules: {
		              starttime: {
		                  required: true
		              },
		              checktime: {
		                  required: true
		              },
		              teamid: {
		                  required: true
		              },
		              groupid: {
		                  required: true
		              },
		              crewid: {
		                  required: true
		              },
		              selcheckid: {
		                  required: true
		              }
		          },
		          ignore: [],
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
// 	                if (!(ext && /^(xls|xlsx|csv)$/.test(ext.toUpperCase()))) {
// 	                    // extensiones permitidas
// 	                    alert('错误: 只能上传xls、xlsx文件。', '');
// 	                    $('#loadingimg').hide();
// 	                    return false;
// 	                }
	            },
	            onComplete: function (file, response) {
	                $('#loadingimg').hide();
	                $('#divimglist').html(file);
	                $('#fileurl').attr("value", response);
	            }
	        });
	    });

	    function submitform() {
	        var url;
	        if (userrole == "TeamManager") {
	            url = "Exam/SubmitTeamExamImport";
            }
            else if (userrole == "OnlineTest") {
                url="Exam/SubmitExamImport";
            }
	        $.ajax({
	            type: "POST",
	            url: rootUri + url,
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

	            if (userrole == "TeamManager") {
	                window.location = rootUri + "Exam/TeamExamList";
	            }
	            else if (userrole == "OnlineTest") {
	                window.location = rootUri + "Exam/ExamList";
	            }
	        }
	    }
    </script>

</asp:Content>
