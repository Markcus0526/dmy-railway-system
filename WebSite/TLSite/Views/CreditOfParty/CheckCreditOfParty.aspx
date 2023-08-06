<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="TLSite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1>
            党内两违比率查询 <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)"
                style="float: right"><i class="ace-icon fa fa-times red2"></i>返回 </a>
        </h1>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <form class="form-horizontal" role="form" id="validation-form">
            <div class="form-group">
                <label class="col-sm-2 control-label no-padding-right" for="starttime">
                    月份：</label>
                <div class="col-sm-1">
                    <div class="clearfix">
                        <div class="input-group col-xs-10 col-sm-5 ">
                            <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                            <input id="date" type="hidden" value="<%=ViewData["CurrentComputerDate"] %>" />
                            <input type="text" id="date2" value="<%= ViewData["CurrentDate"] %>" readonly="readonly" class="input-large"
                                style="width: 100px;" onfocus="WdatePicker({dateFmt:'yyyy年MM月', vel:date, isShowClear:false,  autoShowQS:false})" />
                            <span class="input-group-addon"><i class="fa fa-calendar bigger-110"></i></span>
                        </div>
                    </div>
                </div>
                <label class="col-sm-2 control-label no-padding-right" for="checklevel">
                    检查级别：</label>
                <div class="col-sm-2">
                    <div class="clearfix"> 
                        <select class="select2" id="checklevel" name="checklevel" data-placeholder="请选择检查级别">
                            <option value="无">请选择...</option>                                
                            <option value="段以上">段级以上检查</option>
                            <option value="段级">段级检查</option>
                            <option value="车队">车队检查</option>
                            <option value="班组">班组自查</option>
                        </select>
                    </div>
                </div>
                <label class="col-sm-1 control-label no-padding-right" for="teamid">
                    所在部门：</label>
                <div class="col-sm-2">
                     <div class="clearfix"> 
                        <select class="select2" id="teamid" name="teamid" data-placeholder="请选择部门" onchange="changeuserList()">
                            <% if (ViewData["Sectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["Sectorlist"])
                                   { %>
                            <option value="<%= item.uid %>">
                                <%= item.teamname %></option>
                            <% 
                                           }
                            %>
                            <% } %>
                        </select>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right" for="teamid">
                    统计类型：</label>
                <div class="col-sm-3">
                    <div class="clearfix">
                        <select class="select2" id="policy" name="policy" data-placeholder="请选择" onchange="changeuserList()">
                            <option value="党">统计党员发生问题件数</option>
                            <option value="非党员">统计非党员发生问题件数</option>
                        </select>
                    </div>
                </div>
                <label class="col-sm-1 control-label no-padding-right" for="">
                    按姓名查询：</label>
                <div class="col-sm-3">
                    <div class="clearfix">
                        <div class="input-group col-xs-10 col-sm-5 ">
                            <select class="select2" id="userlist" name="userlist" data-placeholder="请选择姓名">
                                <option value="0">请选择...</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div style="float: right; margin-right: 10px;">
                <a class="btn btn-sm btn-info" id="searchdata" onclick="showlist()"><i class="fa fa-search">
                </i>开始统计</a>
                 <a target="_blank" class="btn btn-sm btn-info" id="export">
                 <i class="fa fa-download"></i>导出Excel</a>
            </div>
            <p>
                &nbsp</p>
            <h3 class="header smaller lighter green">
                下方列表显示查询结果</h3>
            <% if (ViewData["uid"] != null)
               { %>
            <div>
                <p>
                    <a class="btn btn-white btn-info btn-bold" onclick="openAddCrewDlg();"><i class="ace-icon fa fa-plus bigger-120 blue">
                    </i>添加成员 </a>
                </p>
            </div>
            <% } %>
            <div>
                <table id="tbldata" class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th style="width:80px;text-align:center">
                                车队
                            </th>
                            <th     style="width:70px;text-align:center">
                                工资号
                            </th>
                            <th style="width:60px;text-align:center">
                                姓名
                            </th>
                            <th style="text-align:center">
                                班组
                            </th>
                            <th style="text-align:center">
                                职名
                            </th>
                            <th style="text-align:center">
                                政治面貌
                            </th>
                            <th style="text-align:center">
                                车次
                            </th>
                            <th style="text-align:center">
                                项点编号
                            </th>
                            <th style="text-align:center">
                                积分
                            </th>
                            <th style="width:250px;text-align:center">
                                问题内容
                            </th>
                            <th style="text-align:center">
                                检查部门
                            </th>
                            <th style="text-align:center">
                                检查人
                            </th>
                            <th style="text-align:center">
                                所属月份
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
    <!-- 载入年月js -->
    <script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>    

    <script type="text/javascript">
		    var tbl;
		    var alerttable;

  
         jQuery(function ($){
            $(".select2").css('width', '194px').select2({ allowClear: true })
			.on('change', function () {});
           // changegrouplist();
           changeuserList();
          });
        
        function search() {
            tbl = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": rootUri + "CreditOfParty/RefreshTable/?checklevel=" + escape($("#checklevel").val()) + "&date=" + $("#date").val() + "&uid=" + $("#userlist").val()+ "&policy=" + escape($("#policy").val()) + "&teamid=" + $("#teamid").val(),
                "bFilter": false,
                "oLanguage": {
                    "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
                },
                "bAutoWidth":false,
                //bAutoWidth: false,
                "aoColumns": [
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false },
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"},
					  { "bSortable": false , sClass: "center"}
					],
                "bPaginate": true,
            });
        }

        function showlist() {
        if ($("#userlist").val()=="null") {
        alert("暂无用户");
        }
        else{

                if (tbl != null) {
                    var oSettings = tbl.fnSettings();
                    oSettings.sAjaxSource = rootUri + "CreditOfParty/RefreshTable/?checklevel=" + escape($("#checklevel").val()) + "&date=" + $("#date").val() + "&uid=" + $("#userlist").val()+"&policy=" + escape($("#policy").val())+"&teamid=" + $("#teamid").val();
                    tbl.fnClearTable(0);
                    tbl.fnDraw();
                }
                else {
                    search();
                }
            }
                $("#export").attr("href",rootUri + "CreditOfParty/ExportCreditList/?checklevel=" + escape($("#checklevel").val()) + "&date=" + $("#date").val() + "&uid=" + $("#userlist").val()+"&policy=" + escape($("#policy").val())+"&teamid=" + $("#teamid").val());          
            }


         function changeuserList() {
            var teamid = $("#teamid").val();
            var policy = $("#policy ").val();

            $.ajax({
                type: "GET",
                url: rootUri + "CreditOfParty/Getuserlist/?teamid=" + teamid+"&policy="+ escape(policy),
                dataType: "json",
               success: function (data) {
                    var rhtml = "<option value='0'>全部</option>";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
                        }
                        $("#userlist").html(rhtml);
                        $("#userlist").css('width', '194px').select2({ allowClear: true });
                    }
                    else{
                    rhtml= "<option value='null'>无可用用户</option>";
                    $("#userlist").html(rhtml);
                    $("#userlist").css('width', '194px').select2({ allowClear: true });
                    }
                },
            });
        }
    </script>
</asp:Content>
