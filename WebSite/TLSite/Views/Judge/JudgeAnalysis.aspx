<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		结果分析
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
				                <label class="col-sm-2 control-label no-padding-right" for="starttime">检查开始时间：</label>
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
				                <label class="col-sm-2 control-label no-padding-right" for="endtime">检查结束时间：</label>
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
                             <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="teamid">车队：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                    <select class="select2" id="teamid" name="teamid" data-placeholder="请选择" onchange="changeGroupList()">
                                                <option value="0" selected>全部</option>
                                                <% if (ViewData["teamlist"] != null) { 
                                                        foreach(var item in (List<tbl_railteam>)ViewData["teamlist"]) {
                                                            %>
                                                            <option value="<%= item.uid %>"><%= item.teamname %></option>
                                                            <% 
                                                        }
                                                } %>
				                            </select>
						                </div>
                                    </div>
				                </div>
                                              <label class="col-sm-2 control-label no-padding-right" for="crewid">只看前：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
						                   <input  id="selecetop" />
						                </div>
                                    </div>
				                </div>
			                </div>
                              <div class="form-group">
				                <label class="col-sm-2 control-label no-padding-right" for="starttime">检查级别：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
                                            <select class="select2" id="checklevel">
                                                <option value="0">全部</option>
                                                <option value="段以上">段以上</option>
                                                <option value="段级">段级</option>
                                                <option value="车队">车队</option>
                                                <option value="班组">班组</option>
                                            </select>
						                </div>
                                    </div>
				                </div>
				                <label class="col-sm-2 control-label no-padding-right" for="endtime">项点编码：</label>
				                <div class="col-sm-3">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5">
							                    <input  id="checkno" />
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
                <div id="chartcontainer" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
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
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>

    <script src="<%= ViewData["rootUri"] %>Content/plugins/highchart/js/highcharts.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/highchart/js/modules/exporting.js"></script>
    
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
		    var crewid = $("#crewid").val();
		    var starttime = $("#starttime").val();
		    var endtime = $("#endtime").val();
		    var selecetop = $("#selecetop").val();
		    var checklevel = $("#checklevel").val();
		    var checkno = $("#checkno").val();

		    $.ajax({
		        type: "POST",
		        url: rootUri + "Judge/GetJudgeChartData",
		        dataType: "json",
		        data: {
                    teamid: teamid,
                    checklevel: checklevel,
                    checkno: checkno,
                    starttime: starttime,
                    endtime: endtime,
                    selecetop: selecetop
                },
		        success: function (data) {
		            showChart(data);
		        }
		    });
		}

		function showChart(data) {
		    var series = data.series;
		    var jsonseries = [];
		    for (var i = 0; i < series.length; i++) {
		        var datalist = [];
		        for (var j = 0; j < series[i].data.length; j++) {
		            datalist.push(parseInt(series[i].data[j], 10));
		        }

		        jsonseries.push({
                    name: series[i].name,
                    data: datalist
                });

		    }

            var tmp = [{
                name: 'John',
                data: [5]
            }];



		    $('#chartcontainer').highcharts({
		        chart: {
		            type: 'column'
		        },
		        title: {
		            text: '绩效考核统计图表'
		        },
		        xAxis: {
		            categories: data.categories
		        },
		        credits: {
		            enabled: false
		        },
		        series: jsonseries
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

		function changeCrewList() {
		    var groupid = $("#groupid").val();

		    $.ajax({
		        type: "GET",
		        url: rootUri + "Judge/GetCrewListOfGroup/" + groupid,
		        dataType: "json",
		        success: function (data) {
		            var rhtml = "<option value='0' >全部</option>";
		            if (data.length > 0) {
		                for (var i = 0; i < data.length; i++) {
		                    rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
		                }
		                $("#crewid").html(rhtml);
		                $("#crewid").css('width', '250px').select2({ allowClear: true });
		            }
		        }
		    });
		}
    </script>
</asp:Content>
