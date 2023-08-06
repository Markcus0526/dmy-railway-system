<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
    DocumentModel docModel = new DocumentModel();
    var docinfo = (DocumentInfo)ViewData["docinfo"]; %>
<% var loglist = (List<DocumentLog>)ViewData["loglist"]; %>
<% var mylog = (tbl_documentlog)ViewData["mylog"]; %>
<% if (mylog != null)
   {
        %>
   <style>
       .editabletr
       {
           display:none;
           }
   </style>
<% } %>
<div class="page-header">
	<h1 style="text-align:center;">
		待签情况
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right;">
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
                    <style>
                        .docdettbl td
                        {
                            padding:8px;
                        }
                    </style>
                    <form class="form-horizontal" role="form" id="validation-form">
                    <table class="docdettbl" style="width:100%" border="1">
                        <tr>
                            <td colspan="3" style="text-align:center;">
                            <h4><%= docinfo.title %></h4>
                            </td>
                        </tr>
                        <tr>
                            <td><b>发布部门：</b><%= docinfo.sendpart %></td>
                            <td><b>发布人：</b><%= docinfo.sendername %></td>
                            <td><b>发布时间：</b><%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", docinfo.createtime) %></td>
                        </tr>
                        <tr>
                            <td><b>通知号：</b><%= docinfo.docno %></td>
                            <td colspan="2">
                                <b>附件：</b>
                                <a target="_blank" href="<%= ViewData["rootUri"] %><%= docinfo.attachpath %>"><%= docinfo.attachname %><% if (!String.IsNullOrEmpty(docinfo.attachpath)) { %>（<%= decimal.Round(((decimal)docinfo.attachsize / 1024/1024), 1) %>MB）<% } %></a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div>
                                <style type="text/css">
                                    .receivertbl td, .receivertbl th
                                    {
                                        border: 1px solid #ccc;
                                        padding:6px;
                                        }
                                </style>
                                    <table class="receivertbl" style="width:100%;">
                                        <thead>
                                            <th>收报人</th>
                                            <th>签收情况</th>
                                            <th>签收时间</th>
                                            <th>前流转人</th>
                                            <th>流转时间</th>
                                        </thead>
                                        <tbody>
                                            <% var receiverlist = (List<string>)ViewData["receiverlist"];
                                               foreach (var rid in receiverlist)
                                               {
                                                   string signstr = "未签收";
                                                   string logtime = "";
                                                   var loginfo = loglist.Where(m => m.userid.ToString() == rid).FirstOrDefault();
                                                   if (loginfo != null)
                                                   {
                                                       signstr = "已签收";
                                                       logtime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", loginfo.createtime);
                                                   }

                                                   string lzstr = "";
                                                   string lztime = "";
                                                   var lzlog = loglist.Where(m => m.acttype == 1 && m.receiver.Split(',').Contains(rid)).FirstOrDefault();
                                                   if (lzlog != null) {
                                                       lzstr = docModel.GetReceiverName(lzlog.userid.ToString());
                                                       lztime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", lzlog.createtime);
                                                   }
                                                   
                                                    %>
                                               <tr>
                                                    <td><%= docModel.GetReceiverName(rid) %></td>
                                                    <td><%= signstr %></td>
                                                    <td><%= logtime%></td>
                                                    <td><%= lzstr %></td>
                                                    <td><%= lztime %></td>
                                               </tr>
                                               <% 
                                               }
                                                %>
                                            
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </form>
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

</asp:Content>