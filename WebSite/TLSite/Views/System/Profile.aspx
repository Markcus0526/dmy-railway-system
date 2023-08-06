<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% tbl_user userinfo = (tbl_user)ViewData["userinfo"]; %>

<div class="page-header">
	<h1>
		个人信息
	</h1>
</div>

<div class="row">
	<div class="col-xs-12">
        <div class="row">
        <!-- ----------------- -->
		<div id="user-profile-3" class="user-profile row">
			<div class="col-sm-offset-1 col-sm-10">

				<div class="space"></div>

				<form class="form-horizontal" id="validation-form">
					<div class="tabbable">
						<ul class="nav nav-tabs padding-16">
							<li class="active">
								<a data-toggle="tab" href="#edit-basic">
									<i class="green ace-icon fa fa-pencil-square-o bigger-125"></i>
									基本信息
								</a>
							</li>

							<li>
								<a data-toggle="tab" href="#edit-settings">
									<i class="purple ace-icon fa fa-cog bigger-125"></i>
									设置
								</a>
							</li>

							<li>
								<a data-toggle="tab" href="#edit-password">
									<i class="blue ace-icon fa fa-key bigger-125"></i>
									修改密码
								</a>
							</li>
						</ul>

						<div class="tab-content profile-edit-tab-content">
							<div id="edit-basic" class="tab-pane in active">
								<h4 class="header blue bolder smaller">账号信息</h4>

								<div class="row">
									<div class="col-xs-12 col-sm-4">                                        
                                        <img src="<% if ((userinfo != null)&&(userinfo.imgurl != null)) { %><%=ViewData["rootUri"] %><%= userinfo.imgurl %> <% } else {%><%=ViewData["rootUri"] %>Content/img/no-picture.png <% } %>" id = "picture" height="200px"/>	  
                                        <input type="file"  id="input_imagefile" />
                                        <input type="hidden" id="img" name="img" value="<% if (userinfo != null) { %><%= userinfo.imgurl %><% } %>" />                                        
									</div>
                                    
									<div class="vspace-12-sm"></div>

									<div class="col-xs-12 col-sm-8">
										<div class="form-group">
											<label class="col-sm-4 control-label no-padding-right" for="realname">姓名</label>
                                             
											<div class="col-sm-4">
                                                <input type="hidden" name="uid" id="text" <% if (userinfo != null) { %> value="<%=userinfo.uid %>"<% } %> />
                                                                                 
											    <input class="col-sm-4"  type="text" style="width:150px;" name="realname" id="realname" placeholder="请输入真实姓名" <% if (userinfo != null) { %> value="<%=userinfo.realname %>"<% } %> />
                                                
											</div>
										</div>

										<div class="space-4"></div>
									</div>
								</div>

								<hr />
								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="birthday">生年日期</label>

									<div class="col-sm-9">
										<div class="input-medium">
											<div class="input-group">
												<input class="input-medium date-picker" name = "birthday" id="birthday" type="text" data-date-format="yyyy-mm-dd" placeholder="yyyy-mm-dd" <% if (userinfo != null) { %> value="<% =String.Format("{0:yyyy-MM-dd}", userinfo.birthday) %>"<% } %> />
												<span class="input-group-addon">
													<i class="ace-icon fa fa-calendar"></i>
												</span>
											</div>
										</div>
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right">性别</label>

									<div class="col-sm-9">
										<label class="inline">
											<input name="sex" id= "sex_man"type="radio" class="ace" <% if (userinfo != null) { if (userinfo.gender == 0) {%>  checked  <%} } %> value="0" />
											<span class="lbl middle"> 男（Male）</span>
										</label>

										&nbsp; &nbsp; &nbsp;
										<label class="inline">
											<input name="sex" id = "sex_woman" type="radio" class="ace" <% if (userinfo != null) { if (userinfo.gender == 1) {%>  checked  <%} } %> value="1" />
											<span class="lbl middle"> 女（Female）</span>
										</label>
									</div>
								</div>

								<div class="space-4"></div>

								<div class="space"></div>
								<h4 class="header blue bolder smaller">联系方式</h4>

								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="mailaddr">邮箱</label>

									<div class="col-sm-9">
										<span class="input-icon input-icon-right">
											<input type="email" name="mailaddr" id="mailaddr" <% if (userinfo != null) { %> value="<%=userinfo.mailaddr %>"<% } %> />
											<i class="ace-icon fa fa-envelope"></i>
										</span>
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="qqnum">QQ</label>

									<div class="col-sm-9">
										<span class="input-icon input-icon-right">
											<input type="text" name="qqnum" id="qqnum" <% if (userinfo != null) { %> value="<%=userinfo.qqnum %>"<% } %> />
											<i class="ace-icon fa fa-qq"></i>
										</span>
									</div>
								</div>
								<div class="space-4"></div>
								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="form-field-phone">手机号码</label>

									<div class="col-sm-9">
										<span class="input-icon input-icon-right">
											<input class="input-medium input-mask-phone" type="text" name = "phonenum" id="form-field-phone" <% if (userinfo != null) { %> value="<%=userinfo.phonenum %>"<% } %> />
											<i class="ace-icon fa fa-phone fa-flip-horizontal"></i>
										</span>
									</div>
								</div>
							</div>

							<div id="edit-settings" class="tab-pane">
								<div class="space-10"></div>

								<div>
									<label class="inline">
										<input type="checkbox" name="mailnotice" id = "mailnotice" class="ace" <% if (userinfo != null && userinfo.notifymail == 1) { %> checked<% } %> />
										<span class="lbl"> 发送电子邮件新的通知</span>
									</label>
								</div>

							</div>

							<div id="edit-password" class="tab-pane">
								<div class="space-10"></div>

								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="newpassword">新密码</label>
									<div class="col-sm-9">
                                        <div class="input-group"> 
										<input type="password" name = "newpassword" id="newpassword" />
                                        </div>
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-3 control-label no-padding-right" for="rnewpassword">确认密码</label>
									<div class="col-sm-9">
                                    <div class="input-group"> 
										<input type="password" name = "rnewpassword" id="rnewpassword" />
                                        </div>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="clearfix form-actions">
						<div class="col-md-offset-3 col-md-9">
							<button class="btn btn-info loading-btn" type="submit" data-loading-text="保存中..." >
								<i class="ace-icon fa fa-check bigger-110"></i>
								保存
							</button>

							&nbsp; &nbsp;
							<button class="btn" type="reset">
								<i class="ace-icon fa fa-undo bigger-110"></i>
								重置
							</button>
						</div>
					</div>
				</form>
			</div><!-- /.span -->
		</div><!-- /.user-profile -->
        <!-- ----------------- -->

	</div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" /> 
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/uploadify/uploadify.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<!--[if lte IE 8]>
		<script src="<%= ViewData["rootUri"] %>Content/js/excanvas.min.js"></script>
	<![endif]-->    

	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.sparkline.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.maskedinput.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
   	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
    <script charset="utf-8" src="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/kindeditor-min.js"></script>
	<script charset="utf-8" src="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/lang/zh_CN.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/validate.messages_zh.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/uploadify/jquery.uploadify.min.js"></script>  
    <script type="text/javascript">
     
	    function redirectToListPage(status) {
//	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
//	        } else {
//                history.go(-1);
//	            //window.location = rootUri + "Lobby/LBrief/LVideoList";
//	        }
	    }
          

        jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

            $('#input_imagefile').uploadify({
                'buttonText': "上传头像",
                //'queueSizeLimit': 1,  //设置上传队列中同时允许的上传文件数量，默认为999
                'uploadLimit': 1,   //设置允许上传的文件数量，默认为999
                'swf': rootUri + 'Content/plugins/uploadify/uploadify.swf',

	            'fileTypeExts': '*.jpg;*.png;*.jpeg',
	            'fileTypeDesc': 'Image Files (.jpg,.png,*.jpeg)',
	            'fileSizeLimit': '4MB',

                'uploader': rootUri + 'Upload/UploadifyImage',
                'onUploadComplete': function (file) {   //单个文件上传完成时触发事件
                    //alert('The file ' + file.name + ' finished processing.');
                },
                'onQueueComplete': function (queueData) {   //队列中全部文件上传完成时触发事件
                    //   alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
                },
                'onUploadSuccess': function (file, data, response) {    //单个文件上传成功后触发事件                                     
                    //alert('文件 ' + file.name + ' 已经上传成功，并返回 ' + response + ' 保存文件名称为 ' + data.SaveName);
                    data = data.replace(/\"/,"");
                    data = data.replace(/\"/,"");
                    $("#img").val(data);
                    $("#picture").attr("src", "<%= ViewData["rootUri"] %>"+ data);
                }
            });

	        $.validator.messages.required = "必须要填写";
            $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                realname: {
	                    required: true,
	                },
                    birthday: {
	                    required: true
	                },
                    newpassword: {
	                    required: true
	                },
	                rnewpassword: {
                        required: true,
                        equalTo: "#newpassword"
	                },
                    qqnum: {
                        digits: true
                    },                  
                    mailaddr: {
                        email: true
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
	        
			$('.date-picker').datepicker({
			    autoclose: true,
		        todayHighlight: true,
		        language: "zh-CN"
			});

            $('.input-mask-phone').mask('999-9999-9999');
        });
            
        function submitform() {
			$.ajax({
				type: "POST",
				url: rootUri + "System/SubmitUserInfo",
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
				        $('#newpassword').val("");
				        $('#rnewpassword').val("");

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
