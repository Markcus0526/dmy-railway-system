<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var contactlist = (List<ContactInfo>)ViewData["contactlist"];
   List<ContactInfo> execlist = new List<ContactInfo>();
   List<ContactInfo> changlist = new List<ContactInfo>();
   List<ContactInfo> masterlist = new List<ContactInfo>();

   if (contactlist != null)
   {
       execlist = contactlist.Where(m => m.contactkind == "科室").ToList();

       changlist = contactlist.Where(m => m.contactkind == "车队").ToList();

       masterlist = contactlist.Where(m => m.contactkind == "车队"&&m.rolename=="列车长").ToList();
   }
    %>
<div class="page-header">
	<h1 style="text-align:center;">
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Contact/ExportContact" style="float:left">
		    <i class="ace-icon fa fa-download red2"></i>
		    导出通讯录
	    </a>
		通讯录
        <%
            var userrole = CommonModel.GetUserRoleInfo();
            if (userrole != null && ((string)userrole).Contains("Contact"))
            {
            %>
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Contact/ContactList" style="float:right">
		    <i class="ace-icon fa fa-list red2"></i>
		    管理通讯录
	    </a>
        <% } %>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="row">
            <div class="col-sm-2">
            </div>
            <div class="col-sm-8">
	            <div class="tabbable">
		            <ul class="nav nav-tabs" id="myTab">
			            <li class="active"><a data-toggle="tab" href="#home">科室</a></li>
			            <li><a data-toggle="tab" href="#profile">车队</a></li>
			            <li><a data-toggle="tab" href="#profile2">车队列车长</a></li>
                    </ul>
		            <div class="tab-content">
			            <div id="home" class="tab-pane in active">
                            <style type="text/css">
                                table td 
                                {
                                    padding:5px;
                                    text-align:center;
                                    }
                            </style>

                            <%--科室干部--%>
							<div id="accordion" class="accordion-style1 panel-group">
                                <% if (execlist != null) {
                                       var parts = execlist.GroupBy(m => m.partname).Select(m => m.Key).ToList();
                                       int i = 1;
                                       foreach (var part in parts)
                                       {
                                           var subexeclist = execlist.Where(m => m.partname == part).ToList();
                                           %>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle <% if (i > 1) { %>collapsed<% } %>" data-toggle="collapse" data-parent="#accordion" href="#collapseOne<%= i %>">
                                                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down"
                                                            data-icon-show="ace-icon fa fa-angle-right"></i>&nbsp;<%= part %> </a>
                                                </h4>
                                            </div>
                                            <div class="panel-collapse collapse <% if (i == 1) { %>in<% } %>" id="collapseOne<%= i %>">
                                                <div class="panel-body">
                                                    <table style="width: 100%" border="1">
                                                        <tr>
                                                            <td style="font-weight: bold;"> 序号 </td>
                                                            <td style="font-weight: bold;">姓名</td>
                                                            <td style="font-weight: bold;"> 职务</td>
                                                            <td style="font-weight: bold;">包保分工</td>
                                                            <td style="font-weight: bold;">手机号码</td>
                                                            <td style="font-weight: bold;">办公电话</td>
                                                            <td style="font-weight: bold;">小号</td>
                                                            <td style="font-weight: bold;">备注</td>
                                                        </tr>

                                           <%
                                           int j = 1;
                                           foreach (var item in subexeclist)
                                           { %>
                                                        <tr>
                                                            <td>
                                                                <%= j %>
                                                            </td>
                                                            <td>
                                                                <%= item.name %>
                                                            </td>
                                                            <td>
                                                                <%= item.rolename %>
                                                            </td>
                                                            <td>
                                                                <%= item.rolekind %>
                                                            </td>
                                                            <td>
                                                                <%= item.phonenum1 %><br />
                                                                <%= item.phonenum2 %>
                                                            </td>
                                                            <td>
                                                                <%= item.linenum %>
                                                            </td>
                                                            <td>
                                                                <%= item.shortpnum1 %><br />
                                                                <%= item.shortpnum2 %>
                                                            </td>
                                                            <td>
                                                                <%= item.note %>
                                                            </td>
                                                        </tr>

                                            <% 
                                               j++;
                                            }
                                            %>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                       <% 
                                           i++;
                                       }
                                        %>
                                <% } %>
							</div>
			            </div>

                        <%--车队干部--%>
			            <div id="profile" class="tab-pane">
							<div id="accordion2" class="accordion-style1 panel-group">
                                <% if (changlist != null)
                                   {
                                       var parts = changlist.GroupBy(m => m.partname).Select(m => m.Key).ToList();
                                       int i = 1;
                                       foreach (var part in parts)
                                       {
                                           var subexeclist = changlist.Where(m => m.partname == part).ToList();
                                           %>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle <% if (i > 1) { %>collapsed<% } %>" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo<%= i %>">
                                                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down"
                                                            data-icon-show="ace-icon fa fa-angle-right"></i>&nbsp;<%= part %> </a>
                                                </h4>
                                            </div>
                                            <div class="panel-collapse collapse <% if (i == 1) { %>in<% } %>" id="collapseTwo<%= i %>">
                                                <div class="panel-body">
                                                    <table style="width:100%" border="1">
                                                        <tr>
                                                            <td style="font-weight:bold;">序号</td>
                                                            <td style="font-weight:bold;">姓名</td>
                                                            <td style="font-weight: bold;">职务</td>
                                                            <td style="font-weight: bold;">包保分工</td>
                                                            <td style="font-weight:bold;">手机号码</td>
                                                            <td style="font-weight:bold;">小号</td>
                                                            <td style="font-weight:bold;">备注</td>
                                                        </tr>

                                           <%
                                           int j = 1;
                                           foreach (var item in subexeclist)
                                           { %>
                                <tr>
                                    <td><%= j %></td>
                                    <td><%= item.name %></td>
                                    <td><%= item.rolename %></td>
                                    <td><%= item.rolekind %></td>
                                    <td><%= item.phonenum1 %><br /><%= item.phonenum2 %></td>
                                    <td><%= item.shortpnum1 %><br /><%= item.shortpnum2 %></td>
                                    <td><%= item.note %></td>
                                </tr>

                                            <% 
                                               j++;
                                            }
                                            %>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                       <% 
                                           i++;
                                       }
                                        %>
                                <% } %>
							</div>
			            </div>

                        <%--列车长--%>
                        <div id="profile2" class="tab-pane">
							<div id="accordion3" class="accordion-style1 panel-group">
                                <% if (masterlist != null)
                                   {
                                       var parts = masterlist.GroupBy(m => m.partname).Select(m => m.Key).ToList();
                                       int i = 1;
                                       foreach (var part in parts)
                                       {
                                           var subexeclist = masterlist.Where(m => m.partname == part).ToList();
                                           %>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle <% if (i > 1) { %>collapsed<% } %>" data-toggle="collapse" data-parent="#accordion3" href="#collapseThree<%= i %>">
                                                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down"
                                                            data-icon-show="ace-icon fa fa-angle-right"></i>&nbsp;<%= part %> </a>
                                                </h4>
                                            </div>
                                            <div class="panel-collapse collapse <% if (i == 1) { %>in<% } %>" id="collapseThree<%= i %>">
                                                <div class="panel-body">
                                                    <table style="width:100%" border="1">
                                                        <tr>
                                                            <td style="font-weight:bold;">序号</td>
                                                            <td style="font-weight:bold;">姓名</td>
                                                            <td style="font-weight: bold;">职务</td>
                                                            <td style="font-weight: bold;">乘务车次</td>
                                                            <td style="font-weight: bold;">班组</td>
                                                            <td style="font-weight:bold;">手机号码</td>
                                                            <td style="font-weight:bold;">小号</td>
                                                        </tr>

                                           <%
                                           int j = 1;
                                           foreach (var item in subexeclist)
                                           { %>
                                <tr>
                                    <td><%= j %></td>
                                    <td><%= item.name %></td>
                                    <td><%= item.rolename %></td>
                                    <td><%= item.trainno %></td>
                                    <td><%= item.groupname %></td>
                                    <td><%= item.phonenum1 %><br /><%= item.phonenum2 %></td>
                                    <td><%= item.shortpnum1 %><br /><%= item.shortpnum2 %></td>
                                </tr>

                                            <% 
                                               j++;
                                            }
                                            %>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                       <% 
                                           i++;
                                       }
                                        %>
                                <% } %>
							</div>
			            </div>


                    </div>
                </div>

            </div>
            <div class="col-sm-2">
            </div>
        </div>
    </div>
</div><!-- #dialog-specdata -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
</asp:Content>