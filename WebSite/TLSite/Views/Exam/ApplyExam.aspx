<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var bookinfo = (ExamBookInfo)ViewData["bookinfo"]; %>
<% var result = (JsonExamResult)ViewData["result"]; %>
<% var examlist = (List<JsonExamInfo>)ViewData["examlist"]; %>
<style>
    .page-content
    {
        margin:0;
        padding:0;
    }
    .selansw
    {
        font-weight:bold;
        color: #ee0000;
    }
    .confanswer
    {
        font-size:14px;
        }
</style>
<% if (ViewData["showkind"].ToString() == "apply")
   { %>
<div id="examtop" style="position:fixed; top:auto; z-index:30; margin-top:-10px;">
    <table style="width:100%">
        <tr>
            <td style="background-color: #d9edf7;">
                <h1 style="text-align:center;"><%= bookinfo.title %></h1>
            </td>
            <td style="width:234px;">
                <div class="coming-soon-countdown">
                    <div id="defaultCountdown"></div>
                </div>
            </td>
        </tr>
    </table>
</div>
<% }
   else
   { %>
<div class="page-header">
	<h1 style="text-align:center;">
		<%= bookinfo.title%>
	</h1>
</div>

<% } %>

<div class="row" style="margin-top:120px;">
	<div class="col-xs-12">
		<div class="widget-body">
			<div class="widget-main">
				<div id="fuelux-wizard" data-target="#step-container">
					<ul class="wizard-steps">
						<li data-target="#step1" class="active">
							<span class="step">1</span>
							<span class="title">填写试卷的考题</span>
						</li>

						<li data-target="#step2">
							<span class="step">2</span>
							<span class="title">确认考题的答案</span>
						</li>

						<li data-target="#step3">
							<span class="step">3</span>
							<span class="title">考试成绩</span>
						</li>

						<li data-target="#step4">
							<span class="step">4</span>
							<span class="title">查看错误详情</span>
						</li>
					</ul>
				</div>
				<hr />

				<div class="step-content pos-rel" id="step-container">
					<div class="step-pane active" id="step1">
						<form class="form-horizontal" id="examform">
                        <% 
                            int i = 1;
                            if (examlist != null) {
                               
                               foreach (var item in examlist)
                               { %>
                               
							<div class="form-group">
								<label for="inputWarning" class="col-xs-12 col-sm-2 control-label no-padding-right">考题<%= i %>：</label>
                                <div class="col-xs-12 col-sm-10" >
                                    <h4 style="font-weight:bold;"><%= item.title %></h4> <% if (item.examtype == ExamType.MultiSel)
                                                                                            { %>  （多选题）<%} %>
                                </div>
							</div>

							<div class="form-group">
								<label for="inputError2" class="col-xs-12 col-sm-3 control-label no-padding-right"></label>
								<div class="col-xs-12 col-sm-9">
                                    <div class="control-group">
                                   <% if (item.examtype == ExamType.OneSel) { %>
                                    <!-- 单选题 -->
                                            <% foreach (var qitem in item.question)
                                               { %>
										<div class="radio">
											<label>
												<input name="onesel_<%= i %>" type="radio" value="<%= qitem.ind %>" class="ace">
												<span class="lbl"> <%= qitem.ind %>: <%= qitem.question %></span>
											</label>
										</div>
                                            <% } %>
                                   <% } else if (item.examtype == ExamType.MultiSel) { %>
                                        <% foreach (var qitem in item.question)
                                            { %>
										<div class="checkbox">
											<label>
												<input name="multisel_<%= i %>" type="checkbox" value="<%= qitem.ind %>" class="ace">
												<span class="lbl"> <%= qitem.ind %>: <%= qitem.question %></span>
											</label>
										</div>
                                        <% } %>
                                   <% } else if (item.examtype == ExamType.YesNo) { %>
										<div class="radio">
											<label>
												<input name="yesno_<%= i %>" type="radio" value="yes" class="ace">
												<span class="lbl"> 对</span>
											</label>
										</div>

										<div class="radio">
											<label>
												<input name="yesno_<%= i %>" type="radio" value="no" class="ace">
												<span class="lbl"> 错</span>
											</label>
										</div>

                                   <% } %>
									</div>
								</div>
							</div>
                               <% 
                                  i++;
                                  
                               }
                                %>
                        <% } %>

                            <input type="hidden" id="uid" name="uid" value="<%= ViewData["uid"] %>" />
						</form>
					</div>

					<div class="step-pane" id="step2">
                        <% if (ViewData["showkind"].ToString() == "apply")
                           { %>
						<form class="form-horizontal" id="Form1">
                            <h3 class="lighter block green center">如下红色的答案是您已选择的答案。请仔细确认您回答的考试答案。</h3>

                        <% 
                            i = 1;
                            if (examlist != null)
                            {

                                foreach (var item in examlist)
                                { %>
							<div class="form-group">
								<label for="inputWarning" class="col-xs-12 col-sm-2 control-label no-padding-right">考题<%= i %>：</label>
                                <div class="col-xs-12 col-sm-10" >
                                    <h4 style="font-weight:bold;"><%= item.title %></h4>
                                </div>
							</div>

							<div class="form-group">
								<label for="inputError2" class="col-xs-12 col-sm-3 control-label no-padding-right"></label>
								<div class="col-xs-12 col-sm-9">
                                    <div>
                                   <% if (item.examtype == ExamType.OneSel) { %>
                                    <!-- 单选题 -->
                                        <% 
                                            foreach (var qitem in item.question)
                                            { %>
                                            <p class="confanswer" id="p_onesel_<%= i %>_<%= qitem.ind %>" class="selansw"><%= qitem.ind %>: <%= qitem.question %></p>
                                        <% } %>
                                   <% } else if (item.examtype == ExamType.MultiSel) { %>
                                        <% foreach (var qitem in item.question)
                                            { %>
                                            <p class="confanswer" id="p_multisel_<%= i %>_<%= qitem.ind %>" class="selansw"><%= qitem.ind %>: <%= qitem.question %></p>
                                        <% } %>
                                   <% } else if (item.examtype == ExamType.YesNo) { %>
                                        <p class="confanswer" id="p_yesno_<%= i %>_yes" class="selansw">对</p>
                                        <p class="confanswer" id="p_yesno_<%= i %>_no">错</p>
                                   <% } %>
									</div>
								</div>
							</div>
                               <% 
                                    i++;
                                }
                            } %>
						</form>
                        <% } %>
					</div>

					<div class="step-pane" id="step3">
                        <% if (ViewData["showkind"].ToString() == "detail") { %>
						<div class="center">
							<h3 class="green lighter">恭喜您，本考试结束了，您的考试成绩是如下。</h3>
						</div>
                        <div class="center" style="margin-top:36px;">
                            <h1><b><%= result.score%></b></h1>
                        </div>
                        <div class="center" style="margin-top:24px;">
                            <style type="text/css">
                                .examrsttbl td 
                                {
                                    padding:5px;
                                    text-align:center;
                                    }
                            </style>
                            <table class="examrsttbl" style="width:50%; margin:0 auto;" border="1">
                                <tr>
                                    <td style="font-weight:bold;">总考题：<%= result.totalyesno + result.totalonesel + result.totalmultisel%></td>
                                    <td style="font-weight:bold;">总答对题：<%= result.correctmultisel + result.correctonesel + result.correctyesno%></td>
                                </tr>
                                <tr>
                                    <td>单选题：<%= result.totalonesel%></td>
                                    <td>答对题：<%= result.correctonesel%></td>
                                </tr>
                                <tr>
                                    <td>多选题：<%= result.totalmultisel%></td>
                                    <td>答对题：<%= result.correctmultisel%></td>
                                </tr>
                                <tr>
                                    <td>判断题：<%= result.totalyesno%></td>
                                    <td>答对题：<%= result.correctyesno%></td>
                                </tr>
                            </table>
                        </div>
                        <div class="center" style="margin-top:20px;">
                            <div class="row">
                            	<div class="col-xs-3">
                                </div>
                            	<div class="col-xs-3">
                                    <p>考试时间：<%= bookinfo.examtime%>分钟</p>
                                </div>
                            	<div class="col-xs-3">
                                    <p id="usedtime">实际答题时间：<%= ViewData["usedtime"]%></p>
                                   
                                </div>
                            	<div class="col-xs-3">
                                </div>
                            </div>
                        </div>
                        <% } %>
					</div>

					<div class="step-pane" id="step4">
                        <% if (ViewData["showkind"].ToString() == "detail")
                           { %>
						<div class="center">
                            <table class="examrsttbl" style="width:80%; margin:0 auto;" border="1">
                                <tr>
                                    <td style="font-weight:bold;">编号</td>
                                    <td style="font-weight:bold;">种类</td>
                                    <td style="font-weight:bold;">考题</td>
                                    <td style="font-weight:bold;">正确答案</td>
                                    <td style="font-weight:bold;">您的答案</td>
                                </tr>
                                <% 
                               i = 1;
                               if (result != null && result.result != null)
                               {
                                    foreach(var item in result.result) { %>
                                <tr>
                                    <td><%= i %></td>
                                    <td><%= item.examtype %></td>
                                    <td><%= item.title %></td>
                                    <td>
                                        <div style="text-align:left;">
                                            <% 
                                        if (item.question.Count() > 0)
                                        {
                                            foreach (var qitem in item.question)
                                            {
                                                if (item.examtype == ExamType.MultiSel)
                                                {
                                                    string[] ansarr = ((IEnumerable)item.answer).Cast<object>()
                                                     .Select(x => x.ToString())
                                                     .ToArray();
                                                       %>
                                                       <p <% if (ansarr.Contains(qitem.ind)) { %>class="selansw green"<% } %>><%= qitem.ind%>：<%= qitem.question%></p>
                                                       <% 
                                                }
                                                else if (item.examtype == ExamType.OneSel)
                                                {
                                                       %>
                                                       <p <% if ((string)item.answer == qitem.ind) { %>class="selansw green"<% } %>><%= qitem.ind%>：<%= qitem.question%></p>
                                                       <% 
                                                }
                                                   %>
                                            
                                            <% }
                                        } else { %>
                                            <p <% if ((int)item.answer == 1) { %>class="selansw green"<% } %>>对 </p>
                                            <p <% if ((int)item.answer == 0) { %>class="selansw green"<% } %>>错 </p>
                                        <% } %>
                                        </div>
                                    </td>
                                    <td>
                                        <p class="red">
                                        <% if (item.examtype == ExamType.MultiSel)
                                           {
                                               if (item.myanswer == null)
                                               { %>

                                               <% }
                                               else
                                               {
                                                   string[] ansarr = ((IEnumerable)item.myanswer).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                                                    %>
                                        <%= String.Join(",", ansarr) %>
                                               <%
                                               }
                                                %>
                                        <% }
                                           else if (item.examtype == ExamType.YesNo)
                                           { %>
                                        <%= ((string)item.myanswer == "yes") ? "对" :"错" %>
                                        <% }
                                           else
                                           { %>
                                        <%= item.myanswer %>
                                        <% } %>
                                        </p>
                                    </td>
                                </tr>
                                <% 
                                        i++;
                                    }
                                } %>
                            </table>
						</div>
                        <% } %>
					</div>
				</div>

				<hr />
				<div class="wizard-actions">
					<button class="btn btn-prev loading-btn" data-loading-text="提交中...">
						<i class="ace-icon fa fa-arrow-left"></i>
						上一步
					</button>

					<button class="btn btn-success btn-next loading-btn" data-last="查看考试成绩列表" data-loading-text="提交中...">
						下一步
						<i class="ace-icon fa fa-arrow-right icon-on-right"></i>
					</button>
				</div>
			</div><!-- /.widget-main -->
		</div><!-- /.widget-body -->
    </div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link href="<%= ViewData["rootUri"] %>Content/plugins/countdown/coming-soon.css" rel="stylesheet" type="text/css"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<% var bookinfo = (ExamBookInfo)ViewData["bookinfo"]; %>
<% var examlist = (List<JsonExamInfo>)ViewData["examlist"]; %>

	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/fuelux/fuelux.wizard.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/countdown/jquery.countdown.js"></script>

<% if (ViewData["showkind"].ToString() == "apply") { %>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/countdown/plugin/jquery.countdown-zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
<% } %>

	<script type="text/javascript">
    var examstarttime;
	    jQuery(function ($) {

	        $('#fuelux-wizard')
			.ace_wizard({
<% if (ViewData["showkind"].ToString() == "detail") { %>
			    step: 3
<% } %>
			})
			.on('change', function (e, info) {
<% if (ViewData["showkind"].ToString() == "apply") { %>
			    if (info.step == 2 && info.direction == "next") {
			        bootbox.dialog({
			            message: "谢谢参加考试! 一旦提交回答之后不能再参加此考试，您确定要提交考试答案吗？",
			            buttons: {
			                "success": {
			                    "label": "确定",
			                    "className": "btn-sm btn-primary",
			                    "callback": function () {
			                        $('#examform').submit();
			                    }

			                },
			                "cancel": {
			                    "label": "取消",
			                    "className": "btn-sm btn-danger btn-primary",
			                    "callback": function () {
			                        return true;
			                    }
			                }
			            }
			        });
			        return false;
			    } else if (info.step == 1 && info.direction == "next") {
                    $(".confanswer").removeClass("selansw");
                    var selind = "";
                    <% 
                    int i = 1;
                    foreach(var item in examlist) {
                        if (item.examtype == ExamType.OneSel) { %>
                            $("input[name=onesel_<%= i %>]:checked").each(function () {
                                selind = $(this).attr('value');
                                $("#p_onesel_<%= i %>_" + selind).addClass("selansw");
                            });
                        <%
                        } else if (item.examtype == ExamType.MultiSel) { %>
                            $("input[name=multisel_<%= i %>]:checked").each(function () {
                                selind = $(this).attr('value');
                                $("#p_multisel_<%= i %>_" + selind).addClass("selansw");
                            });
                        <%
                        } else if (item.examtype == ExamType.YesNo) {
                        %>
                            $("input[name=yesno_<%= i %>]:checked").each(function () {
                                selind = $(this).attr('value');
                                $("#p_yesno_<%= i %>_" + selind).addClass("selansw");
                            });
                        <%   
                        }
                        i++;
                    } %>

			        return true;
			    }

<% } %>
			})
			.on('finished', function (e) {
                window.location = rootUri + "Exam/ResultList";
			}).on('stepclick', function (e) {
			    //e.preventDefault();//this will prevent clicking and selecting steps
			});

	        //jump to a step
	        $('#step-jump').on('click', function () {
	            var wizard = $('#fuelux-wizard').data('wizard');
	            wizard.currentStep = 1;
	            wizard.setState();
	        });

<% if (ViewData["showkind"].ToString() == "apply") { %>
	        var endtime = new Date();
	        endtime.setMinutes(endtime.getMinutes() + <%= bookinfo.examtime %>);
            //endtime.setMinutes(endtime.getMinutes() + 1);
	        $('#defaultCountdown').countdown({ 
                until: endtime,
                onExpiry: onExpireExam,
                onTick: onTickExam
            });
            examstarttime=new Date().getTime();
	        $('#year').text(endtime.getFullYear());

	        $("#examtop").css("width", $(".page-content").css("width"));

	        $(window).bind('resize', function () {
	            $("#examtop").css("width", $(".page-content").css("width"));
	        });
<% } %>

<% if (ViewData["showkind"].ToString() == "apply") { %>

	        $.validator.messages.required = "必须要填写";
	        $.validator.messages.number = jQuery.validator.format("请输入一个有效的数字.");
	        $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
	        $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
	        $('#examform').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            ignore: [],
	            //focusInvalid: false,
	            rules: {
	                // 	                receiver: {
	                // 	                    required: true
	                // 	                },
	                // 	                title: {
	                // 	                    required: true
	                // 	                },
	                // 	                contents:
	                //                     {
	                //                         required: true
	                //                     }
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
<% } %>
	    });

<% if (ViewData["showkind"].ToString() == "apply") { %>
	    function submitform() {
            var examendtime=new Date().getTime();
            var usedtime=examendtime-examstarttime;
            //alert(parseInt(usedtime/(1000*60))+"分"+parseInt(usedtime%(1000*60)/1000)+"秒");
            var usedtimestr=parseInt(usedtime/(1000*60))+"分"+parseInt(usedtime%(1000*60)/1000)+"秒";
	        $('.loading-btn').button("loading");

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Exam/SubmitApplyExam?usedtime="+parseInt(usedtime/1000),
	            dataType: "json",
	            data: $('#examform').serialize(),
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
                      //  $("#usedtime").inserthtml(usedtime);
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

        function onExpireExam()
        {
            alert("时间到了, 您的试卷被自动提交。");
            $('#examform').submit();
        }

        var examhour;
        var examminute;
        var examsecond;

        function onTickExam(periods)
        {
            if (periods[4] == 0 && periods[5] == 4&& periods[6]==59 ) {
                $(".coming-soon-countdown").css("background-color", "#fe3920");
                alert("时间快到，请尽快填写试卷以后提交！");
            }
            
            examhour = periods[4];
            examminute = periods[5];
            examsecond = periods[6];
        }

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            window.location = rootUri + "Exam/MyResultDetail/<%= ViewData["uid"] %>";
	        }
	    }
<% } %>
    </script>
</asp:Content>
