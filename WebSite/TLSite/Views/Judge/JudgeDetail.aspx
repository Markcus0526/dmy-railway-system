<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var detailinfo = (CheckLogInfo)ViewData["detailinfo"]; %>
<div class="page-header">
	<h1>
		考核详情
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Judge/JudgeList" style="float:right;">
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
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                            <h5><b>考核部门</b>：<%= detailinfo.checkpart %></h5>
                        </div>
                        <div class="col-sm-4">
                            <h5><b>考核人员</b>：<%= detailinfo.checkername %></h5>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                            <h5><b>被考核车队</b>：<%= detailinfo.teamname %></h5>
                        </div>
                        <div class="col-sm-4">
                            <h5><b>检查时间</b>：<%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", detailinfo.checktime) %></h5>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                            <h5><b>被考核班组</b>：<%= detailinfo.groupname %></h5>
                        </div>
                        <div class="col-sm-4">
                            
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                            <h5><b>责任人</b>：<%= detailinfo.crewname %>, 分数：<%= detailinfo.chkpoint %></h5>
                        </div>
                        <div class="col-sm-4">
                            <h5><b>联挂责任人</b>：<%= detailinfo.relcrewname %>, 分数：<%= detailinfo.relpoint %></h5>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-4">
                            <h5><b>考核图片</b>：</h5>
                            <img src="<%= ViewData["rootUri"] %><%= detailinfo.imgurl %>" style="max-width:300px; width:100%;" />
                        </div>
                        <div class="col-sm-4">
                            <h5><b>问题描述</b>：</h5>
                            <p style="font-size:16px; text-align:left; border:1px solid #ccc; background-color:#eee; min-height:300px; padding:5px;">
                                <%= detailinfo.contents %>
                            </p>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
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
