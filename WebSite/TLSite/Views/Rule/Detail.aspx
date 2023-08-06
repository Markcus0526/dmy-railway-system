<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var ruleinfo = (tbl_rule)ViewData["ruleinfo"]; %>
<div class="page-header">
	<h1 style="text-align:center">
		<%= ruleinfo.title %>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="row">
            <div class="col-sm-1"></div>
            <div class="col-sm-10">
                <div style="height:100%">
                    <iframe src="<%= ViewData["rootUri"] %><%= ruleinfo.pdfpath %>" frameborder="0" width="99%" height="700px"> </iframe>
                </div>
            </div>
            <div class="col-sm-1"></div>
        </div>
    </div>
</div><!-- #dialog-specdata -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
</asp:Content>