<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="page-header">
	<h1>
		其他人员变动
            <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
            </br>
			<div class="form-group" >
                <label class="col-sm-3 control-label no-padding-right" for="starttime">选择月份：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">			
                           <!-- 月份选择器使用http://www.my97.net/dp/demo/resource/2.2.asp#m224 -->
                           <input id="date" type="hidden" value="<%=ViewData["CurrentComputerDate"] %>" />
                           <input type="text" id="date2" class="input-large" value="<%= ViewData["CurrentDate"] %>" readonly=readonly  style="width:100px;" onfocus="WdatePicker({dateFmt:'yyyy年MM月', vel:date, isShowClear:false,  autoShowQS:false})" />
						   <span class="input-group-addon">
						   <i class="fa fa-calendar bigger-110"></i>
					       </span>
						</div>
                    </div>
				</div>
                <label class="col-sm-2 control-label no-padding-right" for="currentteam">所在部门<span class="red">*</span>：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
						    <select class="select2" id="teamlist" name="teamlist" data-placeholder="请选择级别" onchange="">
                               <%if (ViewData["teamlist"] != null)
                                 {
                                     foreach (var n in (List<tbl_railteam>)ViewData["teamlist"])
                                      {
                                     %><option value="<%=n.uid %>"><%=n.teamname%></option> <%
                                     }
                                 }%>
				            </select>
                        </div>
                    </div>
				</div>
			</div>
            <hr />

              <!--按钮部分-->
            <div style="float:right; margin-right:10px;">                   
					<a id="execute" class="btn btn-info loading-btn" onclick="retrievetbl()" data-loading-text="提交中...">
						<i class="ace-icon fa fa-search bigger-110"></i>
						开始查询
					</a>
             </div>
             <p>&nbsp</p>
            <h3 class="header smaller lighter green">下方列表显示人员变动信息</h3>
		   <div>
			<table id="tbldata" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>所在部门</th>
						<th>工资号</th>
						<th>姓名</th>
						<th>上报时间</th>
						<th>具体原因</th>
						<th>信息状态</th>
						<th>操作</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>                  
        </form>
	</div>
</div>

<div id="dialog-message" class="hide">
    <form class="form-horizontal" role="form" id="form_crew">
        <div class="col-xs-12">
            <div class="form-group">

			    <label class="col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="sgroupid">工资号：</label>
			    <div class="col-sm-3" style="margin:6px 0px;" >
                    <div class="clearfix">
						<input id="crewno" disabled="disabled" value=""/>
                    </div>
			    </div>
               <label class="col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="sgroupid"> 人员姓名：</label>
			    <div class="col-sm-3" style="margin:6px 0px;" >
                    <div class="clearfix">
						<input id="crewname" disabled="disabled" value=""/>
                    </div>
			    </div>
		    </div>
		    <div class="form-group">
			    <label class="col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="excutetype">变动类型：</label>
			    <div class="col-sm-3" style="margin:6px 0px;" >
                    <div class="clearfix">
						<select class="select2" id="excutetype" name="excutetype" >
                            <option value="1">日勤</option>
                            <option value="2">未分配</option>
                            <option value="3">删除</option>
				        </select>
                    </div>
			    </div>
		    </div>
        </div>
    </form>
</div><!-- #dialog-message -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/jquery-ui.min.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
    <script language="javascript" type="text/javascript" src="<%= ViewData["rootUri"] %>Content/My97DatePicker/WdatePicker.js"></script>



<script type="text/javascript">
    var tbl;
    jQuery(function ($) {
        $(".select2").css('width', '194px').select2({ allowClear: true })
		.on('change', function () {
		});
           tbl=$("#tbldata").dataTable({
                "bServerSide": true,
				"bProcessing": true,
				"sAjaxSource": rootUri + "TeamAdmin/RetrieveManageSPtransferTable?date=" + $("#date").val() + "&teamid=" + $("#teamlist").val(),
				"oLanguage": {
				    "sUrl": rootUri + "Content/i18n/dataTables.chinese.txt"
				},
				//bAutoWidth: false,
				"aoColumns": [
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
                "aoColumnDefs": [{
		              aTargets: [6],    // Column number which needs to be modified
		              fnRender: function (o, v) {   // o, v contains the object and value for the column
                            if (o.aData[5]=="已处理") {
                                var rst="";
                            }
                            else{
                                var rst = '<a class="btn btn-xs btn-info" onclick="EditCrew(' + o.aData[6] + ','+ o.aData[7]+',this);">' +
                                          '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                          '</a>';
                            }
    		                    return rst;
		              },
		              sClass: 'center'
		          }]
        });   
        
        
	        $.validator.messages.required = "必须要填写";
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            rules: {
	                crewlist: {
	                    required: true,
                       // minlength:1
	                }
	            },
                messages: {
                    crewlist:{
	                    required: "请选择人员",
                        minlength:"请选择人员"
                    }
                },
                ignore: [],
	            highlight: function (e) {
	                $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
	            },

	            success: function (e) {
	                $(e).closest('.form-group').removeClass('has-error'); //.addClass('has-info');
	                $(e).remove();
	            },

	            errorPlacement: function (error, element) {
	                if (element.is(':checkbox') || element.is(':radio')) {
	                    var controls = element.closest('div[class*="col-"]');
	                    if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
	                    else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
	                }
	                else if (element.is('.select2')) {
	                    error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
	                }
	                else if (element.is('.chosen-select')) {
	                    error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
	                }
	                else error.insertAfter(element.parent());
	            },

	            submitHandler: function (form) {
                    submitform();
	                return false;
	            },
	            invalidHandler: function (form) {
	                $('.loading-btn').button('reset');
	            }
	        });
             
              $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
	            _title: function (title) {
	                var $title = this.options.title || '&nbsp;'
	                if (("title_html" in this.options) && this.options.title_html == true)
	                    title.html($title);
	                else title.text($title);
	            }
	        }));
       })

         function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "TeamAdmin/SubmitExchange",
	            dataType: "json",
	            data: $('#validation-form').serialize(),
	            success: function (data) {
	                if (data == "") {
	                    toastr.options = {
	                        "closeButton": false,
	                        "debug": true,
	                        "positionClass": "toast-bottom-right",
	                        "onclick": null,
	                        "showDuration": "3",
	                        "hideDuration": "3",
	                        "timeOut": "1500",
	                        "extendedTimeOut": "1000",
	                        "showEasing": "swing",
	                        "hideEasing": "linear",
	                        "showMethod": "fadeIn",
	                        "hideMethod": "fadeOut"
	                    };

	                    toastr["success"]("操作成功!", "恭喜您");
                        if (tblout != null) {
                            //tblout.fnClearTable(0);
                            tblout.fnDraw();
	                    }
	                } else {
	                    toastr.options = {
	                        "closeButton": false,
	                        "debug": true,
	                        "positionClass": "toast-bottom-right",
	                        "onclick": null,
	                        "showDuration": "3",
	                        "hideDuration": "3",
	                        "timeOut": "1500",
	                        "extendedTimeOut": "1000",
	                        "showEasing": "swing",
	                        "hideEasing": "linear",
	                        "showMethod": "fadeIn",
	                        "hideMethod": "fadeOut"
	                    };
	                    toastr["error"](data, "温馨敬告");
	                }
	            },
	            error: function (data) {
	                alert("Error: " + data.status);
	                $('.loading-btn').button('reset');
	            }
	        });
	    }
         function processDel(sel_id) {
            $.ajax({
                url: rootUri + "TeamAdmin/DeleteSPTransfer",
                data: {
                    "delids": sel_id
                },
                type: "post",
                success: function (message) {
                    if (message == true) {
                        toastr.options = {
                            "closeButton": false,
                            "debug": true,
                            "positionClass": "toast-bottom-right",
                            "onclick": null,
                            "showDuration": "3",
                            "hideDuration": "3",
                            "timeOut": "1500",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };
                        toastr["success"]("删除成功！", "恭喜您");
                        if (tbl != null) {
                            //tbl.fnClearTable(0);
                            tbl.fnDraw();
                        }
                    }
                }
            });
        }
        function retrievetbl() {
             var oSettings = tbl.fnSettings();
                    oSettings.sAjaxSource = rootUri + "TeamAdmin/RetrieveManageSPtransferTable/?date=" + $("#date").val() + "&teamid=" + $("#teamlist").val();
                    //tbl.fnClearTable(0);
                    tbl.fnDraw();
        }
            var crewdlg;
           function EditCrew(id,crewid,o) {
               
                var crewinfo=$(o).closest("tr").find("td");
                $("#crewno").val(crewinfo[1].innerHTML);                
                $("#crewname").val(crewinfo[2].innerHTML);
                
                //getusabelgroup(id);

                crewdlg = $("#dialog-message").removeClass('hide').dialog({
                modal: true,
                width: 700,
                title: "<div class='widget-header widget-header-small'><h4 class='smaller'>分配人员</h4></div>",
                title_html: true,
                buttons: [
					{
					    text: "确定",
					    "class": "btn btn-primary btn-xs",
					    click: function () {
					            $.ajax({
					                type: "POST",
					                url: rootUri + "TeamAdmin/ExcuteSpTransfer/",
                                    data:{
                                        crewid : crewid,
                                        transferid : id,
                                        transfertype: $("#excutetype").val()                             
                                    },
					                dataType: "json",
					                success: function (message) {
					                   if (message == "") {
                                            toastr.options = {
                                            "closeButton": false,
                                            "debug": true,
                                            "positionClass": "toast-bottom-right",
                                            "onclick": null,
                                            "showDuration": "3",
                                            "hideDuration": "3",
                                            "timeOut": "1500",
                                            "extendedTimeOut": "1000",
                                            "showEasing": "swing",
                                            "hideEasing": "linear",
                                            "showMethod": "fadeIn",
                                            "hideMethod": "fadeOut"
                                            };
                                        toastr["success"]("人员接受成功！", "恭喜您");
                                            if (tbl != null) {
                                                //tbl.fnClearTable(0);
                                                tbl.fnDraw();
                                            }                                                   
                                        }
                                        else {
	                                        toastr.options = {
	                                            "closeButton": false,
	                                            "debug": true,
	                                            "positionClass": "toast-bottom-right",
	                                            "onclick": null,
	                                            "showDuration": "3",
	                                            "hideDuration": "3",
	                                            "timeOut": "1500",
	                                            "extendedTimeOut": "1000",
	                                            "showEasing": "swing",
	                                            "hideEasing": "linear",
	                                            "showMethod": "fadeIn",
	                                            "hideMethod": "fadeOut"
	                                        };
	                                        toastr["error"](message, "温馨敬告");
	                                    }
					                }
					            });
					        
					        crewdlg.dialog("close");

					    }
					},
					{
					    text: "取消",
					    "class": "btn btn-xs",
					    click: function () {
					        $(this).dialog("close");
					    }
					}
				]
            });

        }
</script>
</asp:Content>
