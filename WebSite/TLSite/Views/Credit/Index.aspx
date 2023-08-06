<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="page-header">
	<h1>
		积分查询
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

                        <div class="form-group" >





                        <!--以下为serchbar中内容-->
                        <br />
                                <label for="form-field-select-3" for="">月份:</label>
                                <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                                <input type="text" style="width:100px" onfocus="WdatePicker({dateFmt:'yyyy年MM月'})" />




                <label class="col-sm-1 control-label no-padding-right" for="routeid">部门：</label>
				<div class="col-sm-3">
                   <%-- <div class="clearfix">
						<select class="select2" id="Select1" name="routeid" data-placeholder="请选择线路" >
                            <% if (ViewData["Sectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railsector>)ViewData["Sectorlist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["Sectorlist"] != null && ViewData["Sectorlist"].ToString() == item.uid.ToString()) { %>selected<% } %> ><%= item.sectorname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>--%>
				</div>




                <label class="col-sm-1 control-label no-padding-right" for="routeid">乘务线路：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="routeid" name="routeid" data-placeholder="请选择线路" >
                            <% if (ViewData["routelist"] != null)
                               {
                                   foreach (var item in (List<tbl_railroute>)ViewData["routelist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["routeid"] != null && ViewData["routeid"].ToString() == item.uid.ToString()) { %>selected<% } %> ><%= item.routename %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>

                                <!--按姓名查询-->
                                <label>按姓名查询:</label>
                                <input type="text" />

                            </div>
                        </div>
                    </div><!--以下为serchbar结束-->
					<hr />
                    
                   

                    <!--按钮部分-->
					
                    <div style="float:right; margin-right:10px;">                   
						<a class="btn btn-sm btn-info" onclick="search_data();" ><i class="fa fa-search"></i> 统计全部</a>
                        <a class="btn btn-sm btn-info"  href="#modal-table" role="button" data-toggle="modal" ><i class="fa fa-search red2"></i> 查看详细</a>
						<a target="_blank" class="btn btn-sm btn-info" href="<%= ViewData["rootUri"] %>judge/ExportJudgeList"><i class="fa fa-download"></i> 导出Excel</a>        
                     </div>
                   <h2>&nbsp</h2>

				</div>


			</div>
		</div>
        <div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>车队</th>
						<th>工资号</th>
						<th>姓名</th>
						<th>班组</th>
						<th>职名</th>
						<th>段及以上<br/>
                            批评教育积分</th>
						<th>班组车队检查<br/>
                            批评教育积分</th>
                        <th>联挂考核积分</th>
                        <th>直接离岗<br/>
                            培训积分</th>
                        <th>直接调整工作
                        <br/>岗位积分</th>
                        <th>本月积分</th>
                        <th>激励积分</th>
                        <th>累计积分</th>
					
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>
</div>


 <!--对话框 显示详情-->
<div id="modal-table" class="modal fade" tabindex="-1">
	<div class="modal-dialog" style="width:90%">
		<div class="modal-content">
			<div class="modal-header no-padding">
				<div class="table-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">
						<span class="white">&times;</span>
					</button>
					查看个人月积分明细
				</div>
			</div>
			<div class="modal-body no-padding">
				<table id="tblspec" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top">
					<thead>
						<tr>
						    <th>车队</th>
						    <th>工资号</th>
						    <th>姓名</th>
						    <th>班组</th>
						    <th>职名</th>
						    <th>项点编码</th>
						    <th>两违问题积分</th>
						    <th>激励积分</th>
						    <th>问题内容</th>
						    <th>检查部门</th>
						    <th>检查人</th>
						    <th>检查时间</th>
						    <th>所属月份</th>
						</tr>
					</thead>
					<tbody>
						
					</tbody>
				</table>
			</div>

			<div class="modal-footer no-margin-top">
            <div style="float:right">
           
				<button class="btn btn-sm btn-danger pull-left" onclick=";">
					<i class="fa fa-download"></i>
					导出个人信息
				</button>
                <button class="btn btn-sm btn-danger pull-left" data-dismiss="modal">
					<i class="ace-icon fa fa-times "></i>
					关闭窗口
				</button>
                 </div>
			</div>

            <input type="hidden" id="dataid" value=""/>

		</div><!-- /.modal-content -->
	</div><!-- /.modal-dialog -->
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<!-- 载入年月js -->
    <script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/bootbox.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>  
    


		<script type="text/javascript">
//		    var selected_id = "";
//		    var oTable;
		    function search_data() {
		        oTable = $('#tbldata').dataTable({
				    "bServerSide": true,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "Credit/AjaxHandler",
				    "oLanguage": {
				        "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
				    },
				    //bAutoWidth: false,
				    "aoColumns": [
					  { "bSortable": false },
					  null,
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false },
					  { "bSortable": false }
					],

				    "aLengthMenu": [
                        [10, 20, 50, -1],
                        [10, 20, 50, "All"] // change per page values here
                    ],

				    "iDisplayLength": 10,

				    "fnDrawCallback": function (oSettings) {
//				        showBatchBtn();
				    }

				});
                }



// 		        $(document).on('click', 'th input:checkbox', function () {
// 		            var that = this;
// 		            $(this).closest('table').find('tr > td:first-child input:checkbox')
// 					.each(function () {
// 					    this.checked = that.checked;
// 					    $(this).closest('tr').toggleClass('selected');
// 					});
// 
// 		            //showBatchBtn();
// 		        });
// 
// 		        $('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });
// 		        function tooltip_placement(context, source) {
// 		            var $source = $(source);
// 		            var $parent = $source.closest('table')
// 		            var off1 = $parent.offset();
// 		            var w1 = $parent.width();
// 
// 		            var off2 = $source.offset();
// 		            //var w2 = $source.width();
// 
// 		            if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
// 		            return 'left';
// 		        }

		 
        </script>


</asp:Content>
