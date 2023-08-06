<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="page-header">
	<h1>
		人员调入
		<small>
			<i class="ace-icon fa fa-angle-double-right"></i>
            人员信息
		</small>
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
</asp:Content>
