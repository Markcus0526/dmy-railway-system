<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var ruleinfo = (tbl_rule)ViewData["ruleinfo"]; %>

<div class="page-header">
	<h1>
		规章管理
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
            规章
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
				<label class="col-sm-3 control-label no-padding-right" for="rulename">规章题目：</label>
				<div class="col-sm-6">
                    <div class="clearfix">
					<input type="text" id="rulename" name="rulename" placeholder="请输入规章题目" class="form-control" <% if (ruleinfo != null) { %>value="<%= ruleinfo.title %>"<% } %> />
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="company">封面图片<span class="red">*</span>：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <input type="button" class="btn btn-sm" id="btndetimg" value="选择图片" />
                        <img src="<%= ViewData["rootUri"] %>Content/img/loading.gif" style="display:none;" id="loadingimg">
                        <input type="hidden" name="imgurl" id="imgurl" <% if (ruleinfo != null) { %>value="<%= ruleinfo.imgurl %>"<% } %> />
                    </div>
                    <div style="margin:10px 0px;" id="divimglist">
                    <% if (ruleinfo != null) { %>
                    <div style='float:left; padding:5px;'>
                    <img src="<%= ViewData["rootUri"] %><%= ruleinfo.imgurl %>" height='120px'>
                    </div>
                    <% } %>
                    </div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="sortid">排序：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					<input type="text" id="sortid" name="sortid" placeholder="请输入数字" class="input-large form-control col-xs-10 col-sm-5"  <% if (ruleinfo != null) { %>value="<%= ruleinfo.sortid %>"<% } else {%> value="0"<% } %> />
                    <span class="help-inline col-xs-12 col-sm-7">
						<span class="middle">数字越小越靠前</span>
					</span>
                    </div>
				</div>
			</div>

			<div class="form-group" >
				<label class="col-sm-3 control-label no-padding-right" for="contents">规章PDF文件：</label>
				<div class="col-sm-9">
                    <div class="clearfix" style="width:100%">
                        <input class="" type="file" id="input_videofile" />
                        <label id="filename1"><% if (ruleinfo != null) { %><%= ruleinfo.pdfname %><% } %></label>
                        <input type="hidden" id="path" name="path" value="" />
                        <input type="hidden" id="filesize" name="filesize" value="0" />
                        <input type="hidden" id="filename" name="filename" />
                    </div>
				</div>
			</div>

<!-- 			<div class="form-group"> -->
<!-- 				<label class="col-sm-3 control-label no-padding-right" for="contents">规章内容：</label> -->
<!-- 				<div class="col-sm-9"> -->
<!--                     <div class="clearfix"> -->
<!--                         <textarea class="form-control" id="contents" name="contents" style="height:300px; width:780px;"></textarea> -->
<!--                     </div> -->
<!-- 				</div> -->
<!-- 			</div> -->

            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />
            <input type="hidden" id="esccontent" name="esccontent"  />

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
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/themes/default/default.css" />
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/uploadify/uploadify.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/ajaxupload.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/uploadify/jquery.uploadify.min.js"></script>  
	<script type="text/javascript">
	    //var editor;
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Rule/List";
	        }
	    }

// 	    KindEditor.ready(function (K) {
// 	        editor = K.create('textarea[name="contents"]', {
//                 uploadJson: "<%= ViewData["rootUri"] %>Upload/UploadKindEditorImage",
//                 fileManagerJson: "<%= ViewData["rootUri"] %>Upload/ProcessKindEditorRequest",
//                 allowFileManager: true,
//                 allowUpload: true,
//                 resizeType:1,
//                 afterChange:function(){
//                     if (editor != null)
//                     {
//                         editor.sync();
//                     }
//                 }
// 	        });
// 
// 	    });

	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });

//             var recvcontents = "<%= ViewData["contents"] %>";
//             var conthtml = unescape(recvcontents);
//             $("#contents").html(conthtml);
//             if (editor != undefined) {
//                 editor.html(conthtml);
//                 editor.sync();
//             }

		      $('#input_videofile').uploadify({
		          'buttonText': '请选择附件',
		          //'queueSizeLimit': 1,  //设置上传队列中同时允许的上传文件数量，默认为999
		          'multi': false,
		          'uploadLimit': 0,   //设置允许上传的文件数量，默认为999
		          'swf': rootUri + 'Content/plugins/uploadify/uploadify.swf',

		          'fileTypeExts': '*.pdf;',
		          'fileTypeDesc': 'PDF Files (.pdf)',
		          'fileSizeLimit': '20MB',
                   
		          'uploader': rootUri + 'Upload/UploadFile',
		          'onUploadComplete': function (file) {   //单个文件上传完成时触发事件
		              //alert('The file ' + file.name + ' finished processing.');
		          },
		          'onQueueComplete': function (queueData) {   //队列中全部文件上传完成时触发事件
		              //alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
		          },
		          'onUploadSuccess': function (file, data, response) {    //单个文件上传成功后触发事件
		              //alert('文件 ' + file.name + ' 已经上传成功，并返回 ' + response + ' 保存文件名称为 ' + data.SaveName + "|" + data.FileSize + "|" + response.SaveName);
		              $("#filename").val(file.name);
		              $("#path").val(data);
		              $("#filesize").val(file.size);
		              $("#filename1").html(file.name);
		          }
		      });


	        $.validator.messages.required = "必须要填写";
	        $.validator.messages.number = jQuery.validator.format("请输入一个有效的数字.");
	        $.validator.messages.url = jQuery.validator.format("请输入有效的地址");
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                rulename: {
	                    required: true
	                },
	                input_videofile: {
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

            new AjaxUpload('#btndetimg', {
                action: rootUri + 'Upload/UploadRulesImage',
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
                    pic_data += "<img src='" + rootUri + "Content/uploads/temp/" + response + "' height='120px' onmouseover='over_img(this)' onmouseout='out_img(this)' >";
                    pic_data +=  "<a href='javascript:(0);'><img src='" + rootUri + "content/img/imgdel.png' class='close_btn' onclick='removeMe(this, \""+response+"\")' onmouseover='over_close(this)' style='visibility:hidden; margin-top:-100px; margin-left:-10px; width:20px; height:20px;' onmouseout='out_close(this)'></a>";
                    pic_data += "</div>";
                    $('#divimglist').html( pic_data );
                    $('#imgurl').attr("value", response );
                }
            });
	    });

	    function submitform() {
            //$("#esccontent").attr("value", escape($("#contents").val()));

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Rule/SubmitRule",
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