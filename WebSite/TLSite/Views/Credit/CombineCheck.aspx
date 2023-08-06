<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		自控率查询
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
					搜索
				</h4>
			</div>

			<div class="widget-body">
				<div class="widget-main">
                    <div class="searchbar">
                        <!--以下为serchbar中内容-->
                             <div class="form-group" >  
                                <label class="col-sm-1 control-label no-padding-right" for="serchtype"  >统计类型:</label>
                                <div class="col-sm-2">
                                    <div class="clearfix">
						                <select class="select2" id="serchtype" name="serchtype" data-placeholder="请选择" onchange="javascript:location.href=this.value;">
                                            <option value="SelfCheck" >两违考核问题自控率</option>
                                            <option value="CombineCheck" selected="selected">结合部问题自控率</option>
                                  
				                        </select> 
                                    </div> 
                                </div>    
              	                <label class="col-sm-1 control-label no-padding-right" for="starttime">月份：</label>
				                <div class="col-sm-2">
                                    <div class="clearfix">
						                <div class="input-group col-xs-10 col-sm-5 ">			
                                           <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                                           <input id="date" type="hidden" value="<%= ViewData["CurrentComputerDate"] %>"/>
                                           <input type="text" id="date2" readonly=readonly  value="<%= ViewData["CurrentDate"] %>"  style="width:100px;" onfocus="WdatePicker({dateFmt:'yyyy年MM月', vel:date, isShowClear:false,  autoShowQS:false})" />
						                   <span class="input-group-addon">
						                   <i class="fa fa-calendar bigger-110"></i>
					                       </span>
						                </div>
                                    </div>
				                </div>
                                <label class="col-sm-1 control-label no-padding-right" for="sectorid" >所在部门：</label>
                                <div class="col-sm-2"> 
                                    <div class="clearfix">
						                <select class="select2" id="sectorid" name="sectorid" data-placeholder="请选择部门" onchange="">
                                            <% if (ViewData["Sectorlist"] != null)
                                               {
                                                   foreach (var item in (List<tbl_railteam>)ViewData["Sectorlist"])
                                                   { %>
                                                   <option value="<%= item.uid %>" ><%= item.teamname %></option>
                                                   <% 
                                                   }
                                                    %>
                                            <% } %>
				                        </select>
                                    </div>
                                </div>
                               </br>
                             </div>
                        </div>
                    <!--以下为serchbar结束-->
					<hr />
                    <!--按钮部分-->
                    <div style="float:right;">
						<a class="btn btn-sm btn-info" id="searchdata" ><i class="fa fa-search"></i> 开始统计</a>
						<a target="_blank" id="export" class="btn btn-sm btn-info" ><i class="fa fa-download"></i> 导出Excel</a>
                     </div>
                   <p>&nbsp</p>
                   
                   </div>
			    </div>
			</div>
            <div>
			    <table id="tbldata" class="table table-striped table-bordered table-hover">
				    <thead>
				
                        <tr>
                          <th>车队</th>
                          <th>班组</th>
                          <th>班组自查</br>问题件数</th>
                          <th>车队检查</br>问题件数</th>
                          <th>段及以上检</br>查问题件数</th>
                        </tr>
				    </thead>
				    <tbody>
				    </tbody>
			    </table>
		    </div>
    </div>
</div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>    

    
<script type="text/javascript">
    var oTable;
    function search() {
        oTable =
    $('#tbldata').dataTable({
        "bServerSide": true,
        "bProcessing": true,
        "sAjaxSource": rootUri + "Credit/RefreshCombineCheck/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val(),
        "bFilter": false,
        "oLanguage": {
            "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
        },
        //bAutoWidth: false,
        "aoColumns": [
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
					],
        "bPaginate": true,
        "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],
        "iDisplayLength": 10
    });

}

$(function () {
    $("#searchdata").click(function () {


        if (oTable == null) {
            search();
        }
        else {
            oSettings = oTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Credit/RefreshCombineCheck/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val();

            oTable.dataTable().fnDraw();
        }

        $("#export").attr("href", rootUri + "Credit/ExportCombineCheckList/?sectorid=" + $("#sectorid").val() + "&date=" + $("#date").val());

    });
});
jQuery(function ($) {
    $("#serchtype").css('width', '200px').select2({ allowClear: true });
    $("#sectorid").css('width', '160px').select2({ allowClear: true });

});
</script>

</asp:Content>
