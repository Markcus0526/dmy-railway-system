<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var currentteam = (List<tbl_railteam>)ViewData["currentteam"]; %>

<div class="page-header">
	<h1>
		调入调出
            <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>
<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
			<div class="form-group" >
                <label class="col-sm-2 control-label no-padding-right" for="currentteam">当前部门<span class="red">*</span>：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
						    <select class="select2" id="currentteam" name="currentteam" data-placeholder="请选择级别" onchange="changegrouplist()">
                               <%if (currentteam != null) {
                                     foreach (var n in currentteam)
                                      {
                                     %><option value="<%=n.uid %>"><%=n.teamname%></option> <%
                                     }
                                 }%>
				            </select>
                        </div>
                    </div>
				</div>
                <label class="col-sm-1 control-label no-padding-right" for="grouplist">班组：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5 ">
						    <select class="select2" id="grouplist" name="grouplist" data-placeholder="请选择级别" onchange="changeuserlist()">
                                <option selected value="">请选择...</option>
                                <%if (ViewData["grouplist"] != null) {
                                      foreach (var n in (List<GroupInfo>)ViewData["grouplist"])
                                      {
                                          %>
                                          <option value="<%=n.uid %>"><%=n.groupname %></option>
                                          <%
                                      }
                                  } %>
				            </select>
                        </div>
                    </div>
				</div>
                <label class="col-sm-1 control-label no-padding-right" for="crewlist">人员姓名<span class="red">*</span>：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="crewlist" name="crewlist" data-placeholder="请选择部门" onchange="">
                            <option value="">请选择...</option>
                            <%if (ViewData["crewlist"] != null)
                             {
                                 foreach (var n in (List<UserInfo>)ViewData["crewlist"])
                                      {
                                          %>
                                          <option value="<%=n.uid %>">(<%=n.crewno %>)<%=n.realname %></option>
                                          <%
                                      }
                              } %>
				        </select>
                    </div>
				</div>
			</div>
            <div class="form-group" >
            	<label class="col-sm-2 control-label no-padding-right"  for="checkersector">变动类型<span class="red">*</span>：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="transfertype" name="transfertype" data-placeholder="请选择部门" onchange="changetable()">
                            <option  value="out">人员调出</option>
                            <option  value="in">人员调入</option>
                           
				        </select>
                    </div>
				</div>
				<label class="col-sm-1 control-label no-padding-right" for="teamlist">调往部门<span class="red">*</span>：</label>
				<div class="col-sm-2">
                    <div class="clearfix">
						<select class="select2" id="teamlist" name="teamlist" data-placeholder="请选择部门"   >
                             <%if (ViewData["teamlist"] != null)
                             {
                                 foreach (var n in (List<tbl_railteam>)ViewData["teamlist"])
                                      {
                                          %>
                                          <option value="<%=n.uid %>"><%=n.teamname %></option>
                                          <%
                                      }
                              } %>
				        </select>
                    </div>
				</div>
			</div>
            <hr />

              <!--按钮部分-->
            <div style="float:right; margin-right:10px;">                   
					<button id="execute" class="btn btn-info loading-btn" type="submit" data-loading-text="提交中...">
						<i class="ace-icon fa fa-check bigger-110"></i>
						执行变动
					</button>
             </div>
             <p>&nbsp</p>
            <h3 class="header smaller lighter green">下方列表显示人员变动信息</h3>
		   <div>
			<table id="tbldata_out" class="table table-striped table-bordered table-hover">
				<thead>
					<tr>
						<th>原部门</th>
						<th>工资号</th>
						<th>姓名</th>
						<th>原班组</th>
						<th>原职名</th>
						<th>调离时间</th>
                        <th>调往部门</th>
					    <th>状态</th>
					    <th>操作</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>    
        <div>
			<table id="tbldata_in" class="table table-striped table-bordered table-hover" style="display: none">
				<thead>
					<tr>
						<th>原部门</th>
						<th>工资号</th>
						<th>姓名</th>
						<th>原班组</th>
						<th>原职名</th>
						<th>调离时间</th>
                        <th>调往部门</th>
					    <th>状态</th>
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
			    <label class="col-sm-2 control-label no-padding-right" style="margin:6px 0px;" for="sgroupid">调入班组：</label>
			    <div class="col-sm-3" style="margin:6px 0px;" >
                    <div class="clearfix">
						<select class="select2" id="newgroup" name="newgroup" data-placeholder="请选择班组">
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



<script type="text/javascript">
    var tblin;
    var tblout;
    jQuery(function ($) {
        $(".select2").css('width', '194px').select2({ allowClear: true })
		.on('change', function () {
		});
           tblout=$("#tbldata_out").dataTable({
                "bServerSide": true,
				"bProcessing": true,
				"sAjaxSource": rootUri + "TeamAdmin/RetrieveTableOut",
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
		              aTargets: [8],    // Column number which needs to be modified
		              fnRender: function (o, v) {   // o, v contains the object and value for the column
                         if (o.aData[7]=="已分配") {
                                 var rst="";
                         }
                         else{
                                var rst = '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[8] + ')">' +
                                          '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
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


          function showtblin(){
             tblin=$("#tbldata_in").dataTable({
                    "bServerSide": true,
				    "bProcessing": true,
				    "sAjaxSource": rootUri + "TeamAdmin/RetrieveTableIn",
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
		                      aTargets: [8],    // Column number which needs to be modified
		                      fnRender: function (o, v) {   // o, v contains the object and value for the column
                                 if (o.aData[7]=="已接受") {
                                    var rst="";
                                 }
                                 else{
                                    var rst = '<a class="btn btn-xs btn-info" onclick="AccepetCrew(' + o.aData[8] + ','+ o.aData[9]+',this);">' +
                                             '<i class="ace-icon fa fa-pencil bigger-120"></i>' +
                                             '</a>';
                                 }
		                          return rst;
		                      },
		                      sClass: 'center'
		          }]
            });
        }

         function changegrouplist(){
            var teamid= $("#currentteam").val();
            $.ajax({
                    type: "GET",
                    url: rootUri + "CheckInfo/GetgroupList/?teamid=" + teamid,
                    dataType: "json",
                    success: function (data) {
                        var rhtml = "<option value=''>请选择...</option>";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                            }
                            $("#grouplist").html(rhtml);
                            $("#grouplist").css('width', '194px').select2({ allowClear: true });

                        }
                        else{
                        rhtml= "<option value='0'>无可用班组</option>";
                        $("#grouplist").html(rhtml);
                        $("#grouplist").css('width', '194px').select2({ allowClear: true });

                        }
                        changeuserlist();
                    },
                });
       }
        function changeuserlist(){
            var groupid= $("#grouplist").val();
            if (groupid=="") {
                var teamid=$("#currentteam").val();
                 $.ajax({
                        type: "GET",
                        url: rootUri + "TeamAdmin/GetuserListofTeam/?teamid=" + teamid,
                        dataType: "json",
                        success: function (data) {
                            var rhtml = "<option value=''>请选择...</option>";
                            if (data.length > 0) {
                                for (var i = 0; i < data.length; i++) {
                                    rhtml += "<option value='" + data[i].uid + "'>("+data[i].crewno+")" + data[i].realname + "</option>";
                                }
                                $("#crewlist").html(rhtml);
                                $("#crewlist").css('width', '194px').select2({ allowClear: true });

                            }
                            else{
                            rhtml= "<option value=''>无可用用户</option>";
                            $("#crewlist").html(rhtml);
                            $("#crewlist").css('width', '194px').select2({ allowClear: true });

                            }
                        },
                    });
            }
            else{
                $.ajax({
                        type: "GET",
                        url: rootUri + "CheckInfo/GetuserList/?groupid=" + groupid,
                        dataType: "json",
                        success: function (data) {
                            var rhtml = "<option value=''>请选择...</option>";
                            if (data.length > 0) {
                                for (var i = 0; i < data.length; i++) {
                                    rhtml += "<option value='" + data[i].uid + "'>("+ data[i].crewno +")" + data[i].realname + "</option>";
                                }
                                $("#crewlist").html(rhtml);
                                $("#crewlist").css('width', '194px').select2({ allowClear: true });

                            }
                            else{
                            rhtml= "<option value=''>无可用用户</option>";
                            $("#crewlist").html(rhtml);
                            $("#crewlist").css('width', '194px').select2({ allowClear: true });

                            }
                        },
                    });
                }
        }
        function changetable() {
            var state=$("#transfertype").val()
            if (state=="in") {
                if (tblin==null) {
                    showtblin();
                }
                else{
                    //tblin.fnClearTable(0);
                   tblin.fnDraw();
                }
                $("#execute").hide();
                $("#tbldata_out_wrapper").hide();
                $("#tbldata_out").hide();
                $("#tbldata_in_wrapper").show();
                $("#tbldata_in").show();
            }
            else {
                if (tblout!=null) {
                   // tblout.fnClearTable(0);
                     tblout.fnDraw();
                }
                $("#tbldata_in_wrapper").hide();
                $("#tbldata_in").hide();
                $("#execute").show();
                $("#tbldata_out_wrapper").show();
                $("#tbldata_out").show();
            }
        }
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
                url: rootUri + "TeamAdmin/DeleteTransfer",
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
                        if (tblout != null) {
                          //  tblout.fnClearTable(0);
                            tblout.fnDraw();
                        }
                    }
                }
            });
        }
          var crewdlg;
          function AccepetCrew(id,crewid,o) {
               
                var crewinfo=$(o).closest("tr").find("td");
                $("#crewno").val(crewinfo[1].innerHTML);                
                $("#crewname").val(crewinfo[2].innerHTML);
                
                getusabelgroup(id);

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
                            var groupid=$("#newgroup").val();
					        if (groupid!=null&&groupid!="") {
					            $.ajax({
					                type: "POST",
					                url: rootUri + "TeamAdmin/AcceptCrew/",
                                    data:{
                                        crewid:crewid,
                                        transferid : id,
                                        groupid: groupid                              
                                    },
					                dataType: "json",
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
                                        toastr["success"]("人员接受成功！", "恭喜您");
                                            if (tblin != null) {
                                               // tblin.fnClearTable(0);
                                                tblin.fnDraw();
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
	                                        toastr["error"]("操作失败", "温馨敬告");
	                                    }
					                }
					            });
					        }
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
        function getusabelgroup(id){
            $.ajax({
			    type: "POST",
			    url: rootUri + "TeamAdmin/GetUsabelGroupList/"+id,
			    dataType: "json",
			    success: function (data) {
                    var rhtml = "";
				    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
                        }
				    }
                    $("#newgroup").html(rhtml);
                    $("#newgroup").css('width', '194px').select2({ allowClear: true });
			    }
		    }); 
        }
</script>
</asp:Content>
