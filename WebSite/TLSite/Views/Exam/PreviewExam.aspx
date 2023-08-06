<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var bookinfo = (ExamBookInfo)ViewData["bookinfo"]; %>
<div class="page-header">
	<h1 style="text-align:center;">
		考试预览
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="row">
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
	            <h3>考试标题：<%= bookinfo.title %></h3>

	            <hr>
	            <h4>考试时间：<%= bookinfo.examtime %>分钟</h4>
                <h4>总考题数：<%= bookinfo.examcount%>个题</h4>
                <hr />
                <h4>考试内容：</h4>
                <p><%= bookinfo.contents%></p>
                <hr />
                <div style="text-align:center; margin:0 auto;">
                <a href="<%= ViewData["rootUri"] %>exam/ApplyExam/<%= bookinfo.uid %>" class="btn btn-app btn-success" >
	                <i class="ace-icon fa fa-pencil-square-o bigger-230"></i>
	                开始考试
                </a>
                </div>
            </div>
            <div class="col-sm-3">
            </div>
        </div>
    </div>
</div><!-- #dialog-specdata -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
</asp:Content>