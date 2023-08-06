<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var detailinfo = (OpinionInfo)ViewData["detailinfo"]; %>
<div class="page-header">
	<h1>
		诉求详情
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
                <div>
                <input style="display:none" name="uid" id="uid" value="<%= detailinfo.uid %>"/>
                <table border="1" class="docdettbl" style="width:100%;margin-bottom:5px" width="100%">
                	<tr>
                		<td colspan="2" style="text-align:center;">
                            <h4><%= detailinfo.title %></h4>
                            </td>
                	</tr>
                    <tr>
                        <td><b>诉求时间：</b><%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", detailinfo.createtime)%></td>
                        <td><b>诉求人：</b><%= detailinfo.sendername%></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                                <div style="min-height:200px; padding:10px;">
                                    <div>
                                        <h4><b>诉求内容:</b></h4>
                                    </div>
                                    <div style="font-size:14px; min-height:260px;">
                                        <%=detailinfo.contents%>
                                    </div>
                                    
                                </div>
                            </td>
                    </tr>   
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <div style="min-height:200px; padding:10px;">
                                <div>
                                    <h4><b>处理意见:</b></h4>
                                </div>
                                <%if (detailinfo.feedback == null) { %>
                                <div>
                                    <div style="font-size:14px; min-height:260px;width:100%;">
                                        <textarea class="form-control" id="feedback" name="feedback" style="height:260px; width:640px;margin: auto;"></textarea>
                                    </div>
                                    <button class="btn btn-info loading-btn" onclick="submitstorelist()" data-loading-text="提交中..."style ="margin-right: 85px;margin-top: 5px;margin-bottom: 10px; float:right">
					                    <i class="ace-icon fa fa-check bigger-110"></i>
					                    确认提交
				                    </button>
                                </div>
                                <%}
                                  else
                                  {%>
                                       <div style="font-size:14px; min-height:260px;">
                                        <%=detailinfo.feedback%>
                                        </div>
                                 <% }
                                  %>

                            </div>
                         </td>
                    </tr>
                </table>
                    <%--<div class="row">
                        <p>职工名称：<%= detailinfo.sendername %></p>
                    </div>
                    <div class="row">
                        <p>标题：<%= detailinfo.title %></p>
                    </div>
                    <div class="row">
                        <p>内容：<%= detailinfo.contents %></p>
                    </div>--%>
                </div>
            </div>
            <div class="col-sm-2"></div>
        </div>
    </div>
</div><!-- #dialog-specdata -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<script type="text/javascript">
    function submitstorelist() {
        var feedback = $("#feedback").val();
        if (feedback == null || feedback == "") {
            alert("请输出反馈内容。")
            return;
        }
        $.ajax({
            type: "GET",
            url: rootUri + "Opinion/SubmitFeedback/?uid="+$("#uid").val()+"&feedback="+escape(feedback) ,
            dataType: "json",
            success: function (data) {
                if (data=="") {
                    window.history.go(0);
                }
            }
        });

    }
</script>
</asp:Content>
