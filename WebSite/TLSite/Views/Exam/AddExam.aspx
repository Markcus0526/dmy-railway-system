<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var examinfo = (ExamInfo)ViewData["examinfo"]; %>
<div class="page-header">
	<h1>
		题库
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
            试题
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
				<label class="col-sm-3 control-label no-padding-right" for="examkind">试题用途：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="examkind" type="radio" value="0" class="ace"  <% if (examinfo == null || (examinfo != null && examinfo.examkind == ExamKind.Sector)) { %>checked<% } %> />
							    <span class="lbl"> 段级考试</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="examkind" type="radio" value="1" class="ace" <% if (examinfo != null && examinfo.examkind == ExamKind.Team) { %>checked<% } %>/>
							    <span class="lbl"> 车队考试</span>
						    </label>
					    </div>
                    </div>
				</div>
			</div>
		    <div class="form-group" id="divteamid" style="<% if (examinfo == null || (examinfo != null && examinfo.examkind == ExamKind.Sector)) { %>display:none;<% } %>">
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="examtype">车队：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
					    <select class="select2" id="teamid" name="teamid" data-placeholder="请选择车队">
                        <% var teamlist = (List<tbl_railteam>)ViewData["teamlist"];
                           foreach (var item in teamlist)
                           {
                               %>
                               <option value="<%= item.uid %>"><%= item.teamname %></option>
                               <%
                           }
                            %>
				        </select>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="examtype">试题类型：</label>
			    <div class="col-sm-9" style="margin:6px 0px;">
                    <div class="clearfix">
					    <select class="select2" id="examtype" name="examtype" data-placeholder="请选择类型" onchange="changeexamtype()">
                            <option value="<%= ExamType.OneSel %>" <% if (examinfo != null && examinfo.examtype == ExamType.OneSel) { %>selected<% } %>><%= ExamType.OneSel %></option>
                            <option value="<%= ExamType.MultiSel %>" <% if (examinfo != null && examinfo.examtype == ExamType.MultiSel) { %>selected<% } %>><%= ExamType.MultiSel %></option>
                            <option value="<%= ExamType.YesNo %>" <% if (examinfo != null && examinfo.examtype == ExamType.YesNo) { %>selected<% } %>><%= ExamType.YesNo %></option>
				        </select>
                    </div>
			    </div>
		    </div>
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="title">试题标题：</label>
			    <div class="col-sm-6" style="margin:6px 0px;">
                    <div class="clearfix">
    					<input type="text" id="title" name="title" placeholder="请输入提问内容" class="form-control" <% if (examinfo != null) { %>value="<%= examinfo.title %>"<% } %> />
                    </div>
			    </div>
		    </div>
            <div id="answerofsel">
		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="title">选项：</label>
			    <div class="col-sm-5" style="margin:6px 0px;" id="qoptlist">
                    <div class="clearfix">
                        <label class="control-label no-padding-right" style="display:inline;">选项A：</label>
    					<input type="text" style="display:inline;" id="Text1" name="choise" placeholder="请输入选项A" class="input-xlarge form-control"  />
                    </div>
                    <div class="clearfix" style="margin-top:10px;">
                        <label class="control-label no-padding-right" style="display:inline;">选项B：</label>
    					<input type="text" style="display:inline;" id="Text2" name="choise" placeholder="请输入选项B" class="input-xlarge form-control" />
                    </div>
                    <div class="clearfix" style="margin-top:10px;">
                        <label class="control-label no-padding-right" style="display:inline;">选项C：</label>
    					<input type="text" style="display:inline;" id="Text3" name="choise" placeholder="请输入选项C" class="input-xlarge form-control"/>
                    </div>
                    <div class="clearfix" style="margin-top:10px;">
                        <label class="control-label no-padding-right" style="display:inline;">选项D：</label>
    					<input type="text" style="display:inline;" id="Text4" name="choise" placeholder="请输入选项D" class="input-xlarge form-control"  />
                    </div>
			    </div>
			    <div class="col-sm-1" style="margin:6px 0px;">
                    <div class="clearfix">
                        <a class="btn btn-white btn-info btn-bold" href="javascript:(0);" onclick="AddQOptList();return false;">
	                        <i class="ace-icon fa fa-plus bigger-120 blue"></i>添加选项
                        </a>
                    </div>
                </div>
		    </div>

		    <div class="form-group" >
			    <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="answer">正确答案：</label>
			    <div class="col-sm-2" style="margin:6px 0px;">
                    <div class="clearfix">
    					<input type="text" id="answer" name="answerstr" placeholder="请输入正确答案" class="input-large form-control" <% if (examinfo != null) { %>value="<%= examinfo.answerstr %>"<% } %>/>
                    </div>
			    </div>
                <span class="time grey control-label col-sm-3 no-padding-left" style="position:relative;left:-50px;top:6px;" >多选题答案请用逗号隔开,如答案：B,C,D</span>
		    </div>
          </div>
            <div class="form-group" id="answerofyesno">
				<label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="answer">正确答案：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="answer" type="radio" value="0" class="ace" <% if (examinfo != null  && examinfo.answerstr == "YES") { %>checked<% } %> />
							    <span class="lbl"> 对</span>
						    </label>
					    </div>
					    <div class="radio" style="display:inline-block">
						    <label>
							    <input name="answer" type="radio" value="1" class="ace" <% if (examinfo != null && examinfo.answerstr == "NO") { %>checked<% } %>/>
							    <span class="lbl"> 错</span>
						    </label>
					    </div>
                    </div>
				</div>
			</div>
		    <div class="form-group" >
                <label class="col-sm-3 control-label no-padding-right" style="margin:6px 0px;" for="answer">此题分值：</label>
				<div class="col-sm-1">
                    <div class="clearfix">
    					<input type="text" style="display:inline;width:100px" id="score" name="score" placeholder="请输入数字" class="input-xlarge form-control" <% if (examinfo != null) { %>value="<%= examinfo.score %>"<% } %> />					   
                    </div>
				</div>
                <span class="time grey control-label col-sm-1 no-padding-left"  >请输入数字</span>

            </div>
            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />

			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
					<button class="btn btn-info loading-btn" type="button" onclick="submitform()" data-loading-text="提交中...">
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>

	<script type="text/javascript">
            
	    var flag = 68;

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Exam/ExamList";
	        }
	    }
	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });
	        changeexamtype();
	        $("input[name=examkind]").change(function (e) {
	            var selval = $("input[name=examkind]:checked").val();
	            if (selval == "0") {
	                $("#divteamid").hide();
	            } else if (selval == "1") {
	                $("#divteamid").show();
	            }
	        });


	        $(".select2").css('width', '200px').select2({ allowClear: true })
			.on('change', function () {

			});

	        // 	        $.validator.messages.required = "必须要填写";
	        // 	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        // 	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        // 	        $.validator.messages.equalTo = jQuery.validator.format("密码不一致.");
	        $.validator.messages.required = "必须要填写";
            $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            //focusInvalid: false,
	            rules: {
	                title: {
	                    required: true
	                },
	                answer: {
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
	              //  return false;
	            },
	            invalidHandler: function (form) {
	                $('.loading-btn').button('reset');
	            }
	        });
	    });

	    function submitform() {
	        var sectint = 65;
	        var choiselist = $("input[name='choise']");
	        var choisestr = "";
	        choiselist.each(function (i, o) {
	            choisestr += String.fromCharCode(sectint + i) + "." + $(o).val()+"\n";
	        });
	      //  alert($('#validation-form').serialize());

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Exam/SubmitExam?choisestr=" + escape(choisestr),
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

//	    function checkUserName() {
//	        var username = $("#username").val();
//	        var uid = $("#uid").val();
//	        var retval = false;

//	        $.ajax({
//	            async: false,
//	            type: "GET",
//	            url: rootUri + "User/CheckUniqueUsername",
//	            dataType: "json",
//	            data: {
//	                username: username,
//	                uid: uid
//	            },
//	            success: function (data) {
//	                if (data == true) {
//	                    retval = true;
//	                } else {
//	                    retval = false;
//	                }
//	            }
//	        });

//	        return retval;
//	    }

	    function deletePardiv(thisobj) {
	        if (thisobj.attr("value") == flag) {
	            flag = flag - 1;
            }
	        thisobj.parent().remove();
	    }

	    function AddQOptList() {
	        flag += 1;
            var rhtml = '<div class="clearfix" style="margin-top:10px;">' +
                '<label class="control-label no-padding-right" style="display:inline;">选项' + String.fromCharCode(flag) + '： </label>' +
    			'<input type="text" style="display:inline;"  name="choise" placeholder="请输入选项" class="input-xlarge form-control" />' +
                '<a href="javascript:(0);" onclick="deletePardiv($(this));return false;" value="'+flag+'"><i class="ace-icon fa fa-times"></i>删除</a>' +
            '</div>';
            $("#qoptlist").append(rhtml);

            return true;
        }
        function changeexamtype() {
            var examtype = $("#examtype").val();
            if (examtype == "<%=ExamType.YesNo%>") {
                $("#answerofyesno").removeClass("hide");
                $("#answerofsel").addClass("hide");
            }
            else if (examtype == "<%=ExamType.OneSel%>" || examtype == "<%=ExamType.MultiSel%>") {
                $("#answerofyesno").addClass("hide");
                $("#answerofsel").removeClass("hide");
            }
        }
    </script>
</asp:Content>