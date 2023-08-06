<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
    var taskinfo = (TaskInfo)ViewData["taskinfo"];
    var execlist = (List<TaskExecutive>)ViewData["execlist"];
    var messagelist = (List<TaskMessage>)ViewData["messagelist"]; 
    %>
<div class="page-header">
	<h1 style="text-align:center">
		<%= taskinfo.title %>
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="row">
            <div class="col-sm-2"></div>
            <div class="col-sm-8">
                <div style="margin-bottom:20px;">
                    <style type="text/css">
                        #tbldetinfo td 
                        {
                            padding:8px;
                            border: 1px solid #333;
                        }
                    </style>
                    <table id="tbldetinfo" style="width:100%; ">
                        <tr>
                            <td style="width:100px;">发布部门：</td>
                            <td><%= taskinfo.sendpart %></td>
                        </tr>
                        <tr>
                            <td style="width:100px;">发布人：</td>
                            <td><%= taskinfo.sendername %></td>
                        </tr>
                        <tr>
                            <td style="width:100px;">发布时间：</td>
                            <td><%= String.Format("{0:yyyy-MM-dd}", taskinfo.publishtime)%></td>
                        </tr>
                        <tr>
                            <td style="width:100px;">任务执行期间：</td>
                            <td><p><%= String.Format("{0:yyyy-MM-dd}", taskinfo.starttime) %> ~ <%= String.Format("{0:yyyy-MM-dd}", taskinfo.endtime) %></p></td>
                        </tr>
                        <tr>
                            <td style="width:100px;">内容：</td>
                            <td><p><%=taskinfo.contents %></p></td>
                        </tr>
                        <tr>
                            <td>附件：</td>
                            <td><a target="_blank" href="<%= ViewData["rootUri"] %><%= taskinfo.attachpath %>"><%=taskinfo.attachname %> </a></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="col-sm-2"></div>
        </div>
        <div class="row">
            <div class="col-sm-2"></div>
            <div class="col-sm-8">
		        <div>
			        <table id="tbldata" class="table table-striped table-bordered table-hover">
				        <thead>
					        <tr>
						        <th>执行人</th>
						        <th>接受时间</th>
						        <th>完成时间</th>
						        <th>状态</th>
					        </tr>
				        </thead>
				        <tbody>
                        <% foreach (var item in execlist)
                           { %>
                            <tr>
                                <td><%= item.receivername %></td>
                                <td><%= item.accepttime %></td>
                                <td><%= item.finishtime %></td>
                                <td>
                                <% if (item.status == TaskStatus.NOTACCPET)
                                   { %>
                                   <span class='label label-large label-important arrowed'>待接收</span>
                                   <% }
                                   else if (item.status == TaskStatus.EXECUTING)
                                   { %>
                                   <span class='label label-large label-info arrowed-right arrowed-in'>执行中</span>
                                   <% }
                                   else if (item.status == TaskStatus.FINISH)
                                   { %>
                                   <span class='label label-large label-success arrowed-in arrowed-in-right'>已完成</span>
                                   <% }
                                   else if (item.status == TaskStatus.NOTFINISH)
                                   { %>
                                   <span class='label label-large label-default arrowed-in arrowed-in-right'>未完成</span>
                                   <% }
                                        %>
                                </td>
                            </tr>
                        <% } %>
				        </tbody>
			        </table>
		        </div>
            </div>
            <div class="col-sm-2"></div>
        </div>
        <div class="row">
            <div class="col-sm-2"></div>
            <div class="col-sm-8">
                <div class="dialogs" style="overflow: hidden; width: auto; ">
                    <% foreach (var item in messagelist)
                       { %>
				    <div class="itemdiv dialogdiv">
					    <div class="user">
						    <img alt="<%= item.username %>" src="<%= ViewData["rootUri"] %><%= item.imgurl %>">
					    </div>

					    <div class="body">
						    <div class="time">
							    <i class="icon-time"></i>
							    <span class="green"><%= CommonModel.GetTimeDiffFromNow(item.createtime) %></span>
						    </div>

						    <div class="name">
							    <a href="#"><%= item.username %></a>
						    </div>
						    <div class="text"><%= item.contents %></div>
					    </div>
				    </div>
                    <% } %>
			    </div>
                <% if (ViewData["origin"] != null && ViewData["origin"].ToString() == "mytask")
                   { %>
                <div>
                    <div class="form-actions">
					    <div class="input-group">
						    <input placeholder="请输入留言 ..." type="text" class="form-control" name="message" id="txtmessage">
						    <span class="input-group-btn">
							    <button class="btn btn-sm btn-info no-radius" type="button" onclick="processMessage()">
								    <i class="icon-share-alt"></i>
								    提交
							    </button>
						    </span>
					    </div>
				    </div>
                </div>
                <% } %>
            </div>
            <div class="col-sm-2"></div>
        </div>
    </div>
</div><!-- #dialog-specdata -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
    <style type="text/css">
        .itemdiv .body 
        {
            margin-left:50px;
            }
    </style>
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<% 
    var taskinfo = (TaskInfo)ViewData["taskinfo"];
%>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
    <script type="text/javascript">
        function processMessage() {
            if (confirm("您确定要提交吗?")) {
                $.ajax({
                    url: rootUri + "Task/ProcessMessage",
                    data: {
                        text: $("#txtmessage").val(),
                        taskid: "<%= taskinfo.uid %>"
                    },
                    type: "post",
                    success: function (message) {
                        if (message == "") {
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
                            toastr["success"]("操作成功！", "恭喜您");
                        }
                    }
                });
            }
        }

        function redirectToListPage(status) {
            window.location.reload();
        }

    </script>
</asp:Content>