<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>

<div class="page-header">
	<h1 style="text-align:center;">
		规章列表
        <% if (userrole != null && ((string)userrole).Contains("Rule"))
            { %>
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Rule/List" style="float:left">
		    管理规章
	    </a>
        <% } %>
	</h1>
</div>

<div class="row">
	
            <div class="col-sm-1"></div>
            <div class="col-sm-10">
                <div class="u-colslist">
                <ul>

                <% if (ViewData["rulelist"] != null)
                   {
                       foreach (var item in (List<tbl_rule>)ViewData["rulelist"])
                       { %>
                <li class="u-bookitm0 j-bookitm">
	                <div class="book">
		                <a href="<%= ViewData["rootUri"] %>rule/detail/<%= item.uid %>" class="cover" hidefocus="hidefocus" target="_new">
		                <img src="<%= ViewData["rootUri"] %><%= item.imgurl %>" alt="<%= item.title %>" ondragstart="return false;" oncontextmenu="return false;" style="display: block;">
		                </a>
		                <a href="<%= ViewData["rootUri"] %>rule/detail/<%= item.uid %>" class="title" hidefocus="hidefocus" target="_new"><%= item.title %></a>
	                </div>
                </li>
                       <% }
                        %>
                <% } %>
                </ul>
                </div>
            </div>
            <div class="col-sm-1"></div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
<style>

    a {
    color: #613f23;
    text-decoration: none;
    }

    .u-colslist {
    position: relative;
    padding-top:30px;
    width: 100%;
    height: auto;
    overflow: hidden;
    padding-left: 10px;
    }
    
    ul, menu, dir {
    display: block;
    list-style-type: disc;
    -webkit-margin-before: 1em;
    -webkit-margin-after: 1em;
    -webkit-margin-start: 0px;
    -webkit-margin-end: 0px;
    -webkit-padding-start: 40px;
    }

    .u-colslist li {
    padding: 0 15px 0 16px;
    margin-bottom: 20px;
    }

    .u-colslist li {
    float: left;
    }

    .u-bookitm0 {
    width: 159px;
    }

    li {
    list-style: none;
    }

    .u-bookitm0 .book, .u-bookitm1 .book {
    position: relative;
    zoom: 1;
    display: block;
    width: 148px;
    }

    *[hidefocus], input, textarea, a {
    outline: 0;
    }

    .u-bookitm0 .cover, .u-bookitm1 .cover, .u-aimg a, .m-activity .list .cover.u-txtlst .img, .u-commitm .cover {
    /*background: url(<%= ViewData["rootUri"] %>content/img/cover.png?201406301757) no-repeat;*/
    }

    .u-bookitm0 .cover, .u-bookitm1 .cover, .u-commitm .cover{
    display: block;
    _overflow: hidden;
    width: 144px;
    height: 194px;
    }
    
    .u-bookitm0 .cover img, .u-bookitm1 .cover img, .u-commitm .cover img 
    {
    width: 128px;
    height: 180px;
    border:2px solid #ccc;
        }

    .u-bookitm0 .cover, .u-bookitm1 .cover, .u-commitm .cover {
    padding: 4px 10px 16px;
    margin-bottom: -4px;
    background-position: 0 0;
    }


    .u-bookitm0 .title {
    display: block;
    height: 36px;
    padding: 0 0 0 10px;
    overflow: hidden;
    line-height: 18px;
    font-size:16px;
    font-weight:bold;
    text-align:center;
    }

</style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
</asp:Content>
