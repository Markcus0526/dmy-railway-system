<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userrole = CommonModel.GetUserRoleInfo(); %>
<style>
.demo-container {
	box-sizing: border-box;
	width: 850px;
	height: 450px;
	padding: 20px 15px 15px 15px;
	margin: 15px auto 30px auto;
	border: 1px solid #ddd;
	background: #fff;
	background: linear-gradient(#f6f6f6 0, #fff 50px);
	background: -o-linear-gradient(#f6f6f6 0, #fff 50px);
	background: -ms-linear-gradient(#f6f6f6 0, #fff 50px);
	background: -moz-linear-gradient(#f6f6f6 0, #fff 50px);
	background: -webkit-linear-gradient(#f6f6f6 0, #fff 50px);
	box-shadow: 0 3px 10px rgba(0,0,0,0.15);
	-o-box-shadow: 0 3px 10px rgba(0,0,0,0.1);
	-ms-box-shadow: 0 3px 10px rgba(0,0,0,0.1);
	-moz-box-shadow: 0 3px 10px rgba(0,0,0,0.1);
	-webkit-box-shadow: 0 3px 10px rgba(0,0,0,0.1);
}

.demo-placeholder {
	width: 100%;
	height: 100%;
	font-size: 14px;
	line-height: 1.2em;
}

.legend table {
	border-spacing: 5px;
}    
</style>
<div class="page-header">
	<h1>
		考试分析
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
        <div class="widget-box">
			<div class="widget-header">
				<h4 class="smaller">
					统计条件
				</h4>
			</div>

			<div class="widget-body">
				<div class="widget-main">
                    <div class="row">
	                    <div class="col-xs-12">
                            <form class="form-horizontal" role="form" id="validation-form">
			                <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="teamid">车队：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                   
                                            <select class="select2" id="teamid" name="teamid" data-placeholder="请选择" onchange="changeGroupList()" <% if (userrole != null && ((string)userrole).Contains("TeamManager")) { %> disabled<% } %>>
                                                <option value="0" selected>全部</option>
                                                <% if (ViewData["teamlist"] != null) { 
                                                        foreach(var item in (List<tbl_railteam>)ViewData["teamlist"]) {
                                                            %>
                                                            <option value="<%= item.uid %>"  <%  if ( item.uid== (long)ViewData["teamid"]) { %> selected<% } %>><%= item.teamname %></option>
                                                            <% 
                                                        }
                                                } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
				                <label class="col-sm-2 control-label no-padding-right" for="groupid">班组：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="groupid" name="groupid" data-placeholder="请选择">
                                                <option value="0" selected>全部</option>
                                                <% if (ViewData["grouplist"] != null) {
                                                       foreach (var item in (List<GroupInfo>)ViewData["grouplist"])
                                                       {
                                                           %>
                                                           <option value="<%= item.uid %>"><%= item.groupname %></option>
                                                           <% 
                                                       }
                                                } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
			                </div>
                            <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="starttime">检查开始时间：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
							                <input class="input-large date-picker" id="starttime" name="starttime" type="text" data-date-format="yyyy-mm-dd" value="" />
							                <span class="input-group-addon">
								                <i class="fa fa-calendar bigger-110"></i>
							                </span>
						                </div>
                                    </div>
				                </div>
				                <label class="col-sm-2 control-label no-padding-right" for="endtime">检查结束时间：<span class="red">*</span>：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
							                <input class="input-large date-picker" id="endtime" name="endtime" type="text" data-date-format="yyyy-mm-dd" value="" />
							                <span class="input-group-addon">
								                <i class="fa fa-calendar bigger-110"></i>
							                </span>
						                </div>
                                    </div>
				                </div>                                
                            </div>
                            </form>
                        </div>
                    </div>
					<hr />
					<div style="text-align:center;">
						<span class="btn btn-sm btn-info" onclick="search_data();" ><i class="fa fa-search"></i> 查询</span>               
					</div>
				</div>
			</div>
		</div>
		<div>
			<div class="portlet-body">
                <div class="row" style="margin-top:30px;">
                    <div class="col-xs-2">
                    </div>
                    <div class="col-xs-8">
		                <div class="demo-container">
			                <div id="placeholder" class="demo-placeholder"></div>
		                </div>                        
                    </div>
                    <div class="col-xs-2">
                    </div>
                </div>
                
			</div>
		</div>
	</div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<!--[if lte IE 8]>
		<script src="<%= ViewData["rootUri"] %>Content/js/excanvas.min.js"></script>
	<![endif]-->

	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>

	<script src="<%= ViewData["rootUri"] %>Content/js/flot/jquery.flot.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/flot/jquery.flot.pie.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/flot/jquery.flot.resize.min.js"></script>

    
	<script type="text/javascript">

	    //var specselect;
	    jQuery(function ($) {
	        $(".select2").css('width', '250px').select2({ allowClear: true })
			.on('change', function () {

			});

	        $('.date-picker').datepicker({
	            autoclose: true,
	            todayHighlight: true,
	            language: "zh-CN"
	        });

	        changeGroupList();
	        search_data();
	    });

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	        } else {

	        }
	    }

	    function search_data() {
	        var teamid = $("#teamid").val();
	        var groupid = $("#groupid").val();
	        var starttime = $("#starttime").val();
	        var endtime = $("#endtime").val();

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Exam/GetExamChartData",
	            dataType: "json",
	            data: {
	                teamid: teamid,
	                groupid: groupid,
	                starttime: starttime,
	                endtime: endtime
	            },
	            success: function (data) {
	                showChart(data);
	            }
	        });
	    }

	    function showChart(data) {
	        var placeholder = $("#placeholder");
	        placeholder.unbind();

	        $.plot(placeholder, data, {
	            series: {
	                pie: {
	                    show: true
	                }
	            }
	        });

	    }

	    function changeGroupList() {
	        var teamid = $("#teamid").val();

	        $.ajax({
	            type: "GET",
	            url: rootUri + "Judge/GetGroupListOfTeam/" + teamid,
	            dataType: "json",
	            success: function (data) {
	                var rhtml = "<option value='0' >全部</option>";
	                if (data.length > 0) {
	                    for (var i = 0; i < data.length; i++) {
	                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
	                    }
	                    $("#groupid").html(rhtml);
	                    $("#groupid").css('width', '250px').select2({ allowClear: true });
	                }
	            }
	        });
	    }

    </script>
</asp:Content>
