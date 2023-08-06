<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-header">
	<h1>
		考核录入
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
		    <i class="ace-icon fa fa-times red2"></i>
		    返回
	    </a>
	</h1>
</div>

<div class="row">
	<div class="col-xs-12">
		<form class="form-horizontal" role="form" id="validation-form">
			<div class="form-group col-sm-8">
				<label class="col-sm-2 control-label no-padding-right" for="starttime">出乘日期<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5">
							<input class="input-large date-picker" id="starttime" name="starttime" type="text" data-date-format="yyyy-mm-dd" value="<%= ViewData["starttime"] %>" />
							<span class="input-group-addon">
								<i class="fa fa-calendar bigger-110"></i>
							</span>
						</div>
                    </div>
				</div>
				<label class="col-sm-2 control-label no-padding-right" for="checktime">检查日期<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<div class="input-group col-xs-10 col-sm-5">
							<input class="input-large date-picker" id="checktime" name="checktime" type="text" data-date-format="yyyy-mm-dd" value="<%= ViewData["starttime"] %>" />
							<span class="input-group-addon">
								<i class="fa fa-calendar bigger-110"></i>
							</span>
						</div>
                    </div>
                    <input value=""  id="trainno" name="trainno" style="display:none"/>
				</div>
			</div>
            <div class="form-group col-sm-4">
				<label class="col-sm-4 control-label no-padding-left" style="float:right" for="company">问题描述：<span class="red">*</span>：</label>
            </div>
			<div class="form-group col-sm-8">
				<label class="col-sm-2 control-label no-padding-right" for="teamid">车队<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="teamid" name="teamid" data-placeholder="请选择车队" onchange="changeGroupList()">
                            <% if (ViewData["teamlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["teamlist"])
                                   { %>
                                   <option value="<%= item.uid %>" <% if (ViewData["teamid"] != null && ViewData["teamid"].ToString() == item.uid.ToString()) { %>selected<% } %>><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>
                    </div>
				</div>
				<label class="col-sm-2 control-label no-padding-right" for="groupid">班组<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="groupid" name="groupid" data-placeholder="请选择班组" onchange="changeCrewList()">
				        </select>
                    </div>
				</div>
			</div>
            <div class="form-group col-sm-4" style="float:right" >
                <div class="clearfix">
                    <textarea class="form-control" id="Textarea1" name="contents" style="height:260px; width:240px;"><% if (ViewData["contents"] != null) { %><%= ViewData["contents"] %> <% } %></textarea>
                </div>
            </div>
			<div class="form-group col-sm-8" >
				<label class="col-sm-2 control-label no-padding-right" for="crewid">责任人：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2" id="crewid" name="crewid" data-placeholder="请选择...">
				        </select>
                    </div>
				</div>
				<label class="col-sm-2 control-label no-padding-right" for="relcrewid">联挂责任人：</label>
				<div class="col-sm-3">
                    <div class="clearfix" id="relcrewdiv">
						<select class="select2" id="relcrewid" name="relcrewid" data-placeholder="请选择...">
				        </select>
                    </div>
				</div>
			</div>
            <div class="form-group col-sm-8" >
				<label class="col-sm-2 control-label no-padding-right" >检查级别：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
						<select class="select2 col-sm-1 " id="checklevel" name="checklevel" data-placeholder="请选择..." onchange="changeinput()">
                        <option value="段以上" >段以上</option>
                        <option value="段级" >段级检查</option>
                        <option value="车队" >车队检查</option>
                        <option value="班组" >班组自查</option>
				        </select>
                    </div>
				</div>
                <label class="col-sm-2 control-label no-padding-right" >检查部门：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
                   		<select class="duan sec" id="checkersectorlist" name="checkersectorlist" data-placeholder="请选择..."  style="display:none">
                        <% if (ViewData["checkersectorlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railsector>)ViewData["checkersectorlist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.sectorname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>

                        <select class="team sec" id="checkerteamlist" name="checkerteamlist" data-placeholder="请选择..."  style="display:none"  >
                        <% if (ViewData["checkerteamlist"] != null)
                               {
                                   foreach (var item in (List<tbl_railteam>)ViewData["checkerteamlist"])
                                   { %>
                                   <option value="<%= item.uid %>" ><%= item.teamname %></option>
                                   <% 
                                   }
                                    %>
                            <% } %>
				        </select>

                        <input  class="groupsec" id="checkergrouplist" name="checkergrouplist" data-placeholder="请选择..." readonly  style="display:none" />
                        <input class="" id="checkersec" name="checkersec" value="" style="display:none"/>
                        <input class="duanup" id="checkersecname" name="checkersecname" value=""/>

                    </div>
				</div>
			</div>

            <div class="form-group col-sm-8">
                <div class="col-sm-10"></div>   
                 <input id="handwritebuttone" type="button" value="手写切换" style="display:none" onclick="handwrite()"/>
            </div>
            <div class="form-group col-sm-8">
               <label class="col-sm-2 control-label no-padding-right" for="relcrewid">检查人：</label>
				<div class="col-sm-3">
                    <div class="clearfix" id="">
						<select class="" id="checkernameList" name="checkernameList" data-placeholder="请选择..." style="width:200px;display:none" >
				        </select>
                        <input id="checkerid" name="checkerid" value="" style="display:none"/>
                        <input id="checkername" name="checkername" value="" />

                    </div>
				</div>
                <label class="col-sm-2 control-label no-padding-right" for="company">项点<span class="red">*</span>：</label>
				<div class="col-sm-3">
                    <div class="clearfix">
                    <a class="btn btn-sm btn-info" href="#modal-table" role="button" data-toggle="modal" style="display:inline" >选择项点</a>
                    <input type="hidden" id="selcheckid" name="selcheckid" />
                    <p id="checkinfo" style="margin-left:18px; border:2px solid #333;display:inline; padding:6px;"></p>
                    </div>
				</div>
            </div>
			<div class="form-group">
				
			</div>
			<div class="form-group">
				<label class="col-sm-3 control-label no-padding-right" for="company">图片<span class="red">*</span>：</label>
				<div class="col-sm-9">
                    <div class="clearfix">
                        <input type="button" class="btn btn-sm" id="btndetimg" value="选择图片" />
                        <img src="<%= ViewData["rootUri"] %>Content/img/loading.gif" style="display:none;" id="loadingimg">
                        <input type="hidden" name="imgurl" id="imgurl" />
                    </div>
                    <div style="margin:10px 0px;" id="divimglist"></div>
                    <button class="btn btn-info loading-btn"   type="submit" data-loading-text="提交中...">
				    <i class="ace-icon fa fa-check bigger-110"></i>
				    添加
				    </button>
				</div>
			</div>

            <input type="hidden" id="uid" name="uid" value="<% if (ViewData["uid"] != null) { %><%= ViewData["uid"] %><% } else { %>0<% } %>" />
            <div>
			    <table id="tbldata" class="table table-striped table-bordered table-hover">
				    <thead>
					    <tr>
						    <th>受检部门</th>
						    <th>责任人</br>工资号</th>
						    <th>姓名</th>
						    <th>班组</th>
						    <th>职名</th>
						    <th>积分</th>
						    <th>责任车长</br>工资号</th>
                            <th>车长姓名</th>
                            <th>项点编码</th>
                            <th>检查部门</th>
                            <th>检查人</th>
					        <th>检查时间</th>
                            <th>操作</th>
					    </tr>
				    </thead>
				    <tbody>
				    </tbody>
			    </table>
		    </div>
            </form>
			<div class="clearfix form-actions">
				<div class="col-md-offset-3 col-md-9">
					<button class="btn btn-info loading-btn" onclick="submitstorelist()" data-loading-text="提交中...">
						<i class="ace-icon fa fa-check bigger-110"></i>
						确认提交
					</button>

					&nbsp; &nbsp; &nbsp;
					<button class="btn" type="reset" >
						<i class="ace-icon fa fa-undo bigger-110"></i>
						重置
					</button>
				</div>
			</div>
	</div>
</div>

<div id="modal-table" class="modal fade" tabindex="-1">
	<div class="modal-dialog" style="width:90%">
		<div class="modal-content">
			<div class="modal-header no-padding">
				<div class="table-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">
						<span class="white">&times;</span>
					</button>
					请选择项点
				</div>
			</div>
			<div class="modal-body no-padding">
				<table id="tblspec" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top">
					<thead>
						<tr>
						    <th class="center">
						    </th>
						    <th>分类</th>
						    <th>编号</th>
						    <th>项点种类</th>
						    <th>扣分</th>
						    <th>联挂扣分</th>
						    <th>项点内容</th>
						</tr>
					</thead>
					<tbody>
						
					</tbody>
				</table>
			</div>

			<div class="modal-footer no-margin-top">
				<button class="btn btn-sm btn-danger pull-left" data-dismiss="modal" onclick="submit_dataitem();">
					<i class="ace-icon fa fa-save"></i>
					确认
				</button>
			</div>

            <input type="hidden" id="dataid" value=""/>

		</div><!-- /.modal-content -->
	</div><!-- /.modal-dialog -->
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/chosen.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/select2.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/datepicker.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/bootstrap-datepicker.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/date-time/locales/bootstrap-datepicker.zh-CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/chosen.jquery.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/select2.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.dataTables.bootstrap.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/js/ajaxupload.js"></script>

	<script type="text/javascript">
	    function handwrite() {
	        $("#checkername").toggle();
	        $("#checkernameList").toggle();
        }


	    function changeinput() {
	        if ($("#checklevel").val() == "段以上") {
	            $(".team").hide();
	            $(".groupsec").hide();
	            $(".duan").hide();
	            $("#handwritebuttone").hide();
	            $("#checkername").show();
	            $("#checkernameList").hide();
	            $(".duanup").show();
	            $("#checkerid").val("");
	            $("#checkersec").val("");
	            $("#checkersecname").val("");
	            $("#checkername").val("");
	       }
	       else if ($("#checklevel").val() == "段级") {
	           $(".duanup").hide();
	           $(".groupsec").hide();
	           $(".team").hide();
	           $("#handwritebuttone").show();
	           $("#checkername").hide();
	           $("#checkernameList").show();  

	           $(".duan").show();
	           $("#checkersec").val($(".duan").val());
	           $("#checkersecname").val($(".duan").children('option:selected').text());
	           ChangeCheckernameList();
            }
	       else if ($("#checklevel").val() == "车队") {
	           $(".duanup").hide();
	           $(".groupsec").hide();
	           $(".duan").hide();
	           $("#handwritebuttone").hide();

	           $("#checkername").hide(); 
	           $("#checkernameList").show();              
	           $(".team").show();
	           $("#checkersec").val($(".team").val());
	           $("#checkersecname").val($(".team").children('option:selected').text());
	           ChangeCheckernameList();
	       }
	       else if ($("#checklevel").val() == "班组") {
	           $(".duanup").hide();
	           $(".team").hide();
	           $(".duan").hide();
	           $("#handwritebuttone").hide();
	           $("#checkername").hide();

	           $("#checkernameList").show();
	           $(".groupsec").show();

	           $("#checkersec").val($("#groupid").val());
	           $("#checkersecname").val($("#groupid").children('option:selected').text());
	           ChangeCheckernameList();
	       }
	     
	   }

        $(document).ready(function () {
            $('#checkernameList').change(function name() {
                $("#checkerid").val($(this).children('option:selected').val());
                $("#checkername").val($(this).children('option:selected').text());

            });
            $('.sec').change(function () {
                $("#checkersec").val($(this).children('option:selected').val());
                $("#checkersecname").val($(this).children('option:selected').text());
                ChangeCheckernameList();

            });
        })
        function ChangeCheckernameList() {
        var checklevel = $("#checklevel").val();
                var id = $("#checkersec").val();
                $.ajax({
                    type: "GET",
                    url: rootUri + "Judge/FindJudgeCheckerList/" + id,
                    dataType: "json",
                    data: {
                        checklevel: checklevel
                    },
                    success: function (data) {
                        var rhtml = "", relhtml = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].username + "</option>";
                            }
                        }
                        $("#checkernameList").html(rhtml);
                    //    $("#checkernameList").css('width', '250px').select2({ allowClear: true });
                        $("#checkerid").val($("#checkernameList").val());
                        $("#checkername").val($("#checkernameList").children('option:selected').text());
                    }
                });

        }

	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
	            $('.loading-btn').button('reset');
	        } else {
	            $('.loading-btn').button('reset');
	         //   window.location = rootUri + "Judge/AddJudge";
	        }
	    }

        var specTable;
	    var editor;
        var tbl;
	    jQuery(function ($) {
	        $('.loading-btn')
		      .click(function () {
		          var btn = $(this)
		          btn.button('loading')
		      });
	         
             tbl = $('#tbldata').dataTable({
                             "bServerSide": true,
                             "bProcessing": true,
                             "sAjaxSource": rootUri + "Judge/GetStoreList",
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
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					              { "bSortable": false },
 					            ],
                             "bPaginate": false,
                             "aLengthMenu": [
                                     [10, 20, 50, -1],
                                     [10, 20, 50, "All"] // change per page values here
                                 ],
                             "iDisplayLength": 10,
                              "aoColumnDefs" : [ {
				                                aTargets: [12],    // Column number which needs to be modified
				                                fnRender: function (o, v) {   // o, v contains the object and value for the column
				                                    var rst = '<a class="btn btn-xs btn-danger" onclick="processDel(' + o.aData[12] + ')">' +
                                                        '<i class="ace-icon fa fa-trash-o bigger-120"></i>' +
                                                        '</a>';
				                                    return rst;
				                                },
				                                sClass: 'center'
				                                 }]
                         });
	        $(".select2").css('width', '250px').select2({ allowClear: true })
			.on('change', function () {
			    //$(this).closest('form').validate().element($(this));
			});

	        $('#checktime').datepicker({
	            autoclose: true,
	            todayHighlight: true,
	            language: "zh-CN"
	        });
	        $('#starttime').datepicker({ autoclose: true, todayHighlight: true, language: "zh-CN" })
			.on('change', function () {
			    changeTeamList();
			    //$(this).closest('form').validate().element($(this));
			});

	        $.validator.messages.required = "必须要填写";
	        $('#validation-form').validate({
	            errorElement: 'span',
	            errorClass: 'help-block',
	            rules: {
	                starttime: {
	                    required: true
	                },
	                checktime: {
	                    required: true
	                },
	                teamid: {
	                    required: true
	                },
	                groupid: {
	                    required: true
	                },
	                crewid: {
	                    required: true
	                },
	                selcheckid: {
	                    required: true
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
	                    //add();
	                    return false;
	            },
	            invalidHandler: function (form) {
	                $('.loading-btn').button('reset');
	            }
	        });

	        specTable =
			$('#tblspec')
			.dataTable({
			    "bServerSide": true,
			    "bProcessing": false,
			    "bFilter": true,
			    "bLengthChange": false,
			    "sAjaxSource": rootUri + "Check/RetrieveCheckInfoList",
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
			    "iDisplayLength": 10,
			    "aoColumnDefs": [
				    {
				        aTargets: [0],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = '<td class="center">' +
							    '<label class="position-relative">' +
								    '<input type="radio" class="ace" name="selcheckbox" value="' + o.aData[0] + '" />' +
								    '<span class="lbl"></span>' +
							    '</label>' +
						    '</td>';
				            return rst;
				        },
				        sClass: 'center'
				    },
				    {
				        aTargets: [3],    // Column number which needs to be modified
				        fnRender: function (o, v) {   // o, v contains the object and value for the column
				            var rst = "";
				            if (o.aData[3] == "0") {
				                rst = "加分";
				            } else if (o.aData[3] === "1") {
				                rst = "扣分";
				            }
				            return rst;
				        },
				        sClass: 'center'
				    }
                ],
			    "fnDrawCallback": function (oSettings) {
			        //showBatchBtn();
			    }
			});

	        new AjaxUpload('#btndetimg', {
	            action: rootUri + 'Upload/UploadImage',
	            onSubmit: function (file, ext) {
	                $('#loadingimg').show();
	                if (!(ext && /^(JPG|PNG|JPEG|GIF)$/.test(ext.toUpperCase()))) {
	                    // extensiones permitidas
	                    alert('错误: 只能上传图片', '');
	                    $('#loadingimg').hide();
	                    return false;
	                }
	            },
	            onComplete: function (file, response) {
	                $('#loadingimg').hide();
	                var pic_data = "<div style='float:left; padding:5px;'>";
	                pic_data += "<img src='" + rootUri + "Content/uploads/temp/" + response + "' height='180px' onmouseover='over_img(this)' onmouseout='out_img(this)' >";
	                pic_data += "<a href='javascript:(0);'><img src='" + rootUri + "content/img/imgdel.png' class='close_btn' onclick='removeMe(this, \"" + response + "\")' onmouseover='over_close(this)' style='visibility:hidden; margin-top:-100px; margin-left:-10px; width:20px; height:20px;' onmouseout='out_close(this)'></a>";
	                pic_data += "</div>";
	                $('#divimglist').html(pic_data);
	                $('#imgurl').attr("value", response);
	            }
	        });

	        changeTeamList();
	    });

       

        function processDel(sel_id) {
            $.ajax({
                url: rootUri + "Judge/DeleteStroeList",
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
		                tbl.dataTable().fnDraw();
                        }
                    }
                }
            });
        }
        function submitstorelist(){
            if (confirm("请确认输入的内容，一旦提交信息不能再来修改！")) {
                location.href="<%= ViewData["rootUri"]%>Judge/SubmitJudge2"
                $('.loading-btn').button('reset');
            } else {
                $('.loading-btn').button('reset');
            }
        }

	    function submitform() {
	        $.ajax({
	            type: "POST",
	            url: rootUri + "Judge/Test",
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

	                    toastr["success"]("添加成功!", "恭喜您");
	                    if (tbl != null) {
                            tbl.dataTable().fnDraw();
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

        function submit_dataitem()
        {
            var selid = $("input[name=selcheckbox]:checked").val();
            $("#selcheckid").val(selid);

            var checkinfo = "";
            var sinfo = "";

            var seltag = $("input[name=selcheckbox]:checked").closest("tr").find("td");

            var lgflg = 0;
            $.each(seltag, function (index, obj) {
                //                 if (index == 1) {
                //                     sinfo += "分类：" + obj.innerHTML;
                //                 }
                if (index == 2) {
                    sinfo += "项点：" + obj.innerHTML;
                }
                if (index == 3) {
                    sinfo += "，项点种类：" + obj.innerHTML;
                }
                if (index == 4) {
                    sinfo += "，分数：" + obj.innerHTML;
                    if (parseInt(obj.innerHTML, 10) > 10) {
                        lgflg = 1;
                    }
                }
                if (index == 5 && lgflg == 1) {
                    if (obj.innerHTML != "") {
                        sinfo += "，联挂分数：" + obj.innerHTML;
                    } else {
                        lgflg = 0;
                    }
                }

                if (index == 6) {
                    checkinfo += obj.innerHTML;
                }
            });

            if (lgflg == 0) {
                $("#relcrewdiv").css("display", "none");
            } else {
                $("#relcrewdiv").css("display", "block");
            }

            $("#checkinfo").html(sinfo);
            $("#contents").html(checkinfo);
        }

	    function changeGroupList() {
	        var teamid = $("#teamid").val();
            var starttime = $("#starttime").val();
            
            if (teamid == "" || teamid == null) {
	            $("#groupid").html("");
	            $("#groupid").css('width', '250px').select2({ allowClear: true });
            }

	        $.ajax({
	            type: "GET",
	            url: rootUri + "Judge/FindGroupListOfTeam/" + teamid,
	            dataType: "json",
                data: {
                    starttime: starttime
                },
	            success: function (data) {
	                var rhtml = "";
	                if (data.length > 0) {
	                    for (var i = 0; i < data.length; i++) {
	                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].groupname + "</option>";
	                    }
	                    $("#groupid").html(rhtml);
	                    $("#groupid").css('width', '250px').select2({ allowClear: true });
	                }
                    changeCrewList();
                   
	            }
	        });
        }

	    function changeCrewList() {
	        var groupid = $("#groupid").val();
            var starttime = $("#starttime").val();

            if (groupid == "" || groupid == null) {
	            $("#crewid").html("");
	            $("#crewid").css('width', '250px').select2({ allowClear: true });
	            $("#relcrewid").html("");
	            $("#relcrewid").css('width', '250px').select2({ allowClear: true });
            }

            $.ajax({
                type: "GET",
                url: rootUri + "Judge/FindJudgeCrewList/" + groupid,
                dataType: "json",
                data: {
                    starttime: starttime
                },
                success: function (data) {
                    var rhtml = "", relhtml = "";
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + " (" + data[i].rolename + ")</option>";

                            if (data[i].rolename == "列车长") {
                                relhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + " (" + data[i].rolename + ")</option>";
                            }
                        }
                    }
                    $("#crewid").html(rhtml);
                    $("#crewid").css('width', '250px').select2({ allowClear: true });
                    $("#relcrewid").html(relhtml);
                    $("#relcrewid").css('width', '250px').select2({ allowClear: true });
                    lockTrainno();

                    $("#checkergrouplist").val($("#groupid").children('option:selected').text());

                    if ($("#checklevel").val() == "班组") {
                        $("#checkersec").val($("#groupid").val());
                        ChangeCheckernameList();
                        $("#checkersecname").val($("#groupid").children('option:selected').text());
                    }

                }
            });
        }

        

        function changeTeamList()
        {
            var starttime = $("#starttime").val();
	        $.ajax({
	            type: "GET",
	            url: rootUri + "Judge/FindJudgeTeamList",
	            dataType: "json",
                data: {
                    starttime: starttime
                },
	            success: function (data) {
	                var rhtml = "";
	                if (data.length > 0) {
	                    for (var i = 0; i < data.length; i++) {
	                        rhtml += "<option value='" + data[i].uid + "'>" + data[i].teamname + "</option>";
	                    }
	                }
	                $("#teamid").html(rhtml);
	                $("#teamid").css('width', '250px').select2({ allowClear: true });
                    changeGroupList();
	            }
	        });
        }


        function lockTrainno() {
            var groupid = $("#groupid").val();
            var starttime = $("#starttime").val();

            $.ajax({
                type: "GET",
                url: rootUri + "Judge/FindTrainnoOntblDutyByGroup/",
                dataType: "json",
                data: {
                    groupid: groupid,
                    starttime: starttime
                },
                success: function (data) {
                        var rhtml = "";
                        $("#trainno").attr('value', data);
                }
            });
        }

    </script>
</asp:Content>