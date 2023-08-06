<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var docinfo = (DocumentInfo)ViewData["docinfo"]; %>
<% var loglist = (List<DocumentLog>)ViewData["loglist"]; %>
<% var mylog = (tbl_documentlog)ViewData["mylog"]; %>
<% if (mylog != null)
   {
        %>
   <style>
       .editabletr
       {
           display:none;
           }
   </style>
<% } %>
<div class="page-header">
	<h1 style="text-align:center;">
        <a class="btn btn-white btn-default btn-round" href="<%= ViewData["rootUri"] %>Document/SignDetail/<%= ViewData["uid"] %>" style="float:left;">
		    <i class="ace-icon fa fa-list"></i>
		    待签情况
	    </a>
		锦州客运段网传公文
        <a class="btn btn-white btn-default btn-round" onclick="window.history.go(-1)" style="float:right">
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
                    <style>
                        .docdettbl td
                        {
                            padding:8px;
                        }
                    </style>
                    <form class="form-horizontal" role="form" id="validation-form">
                    <table class="docdettbl" style="width:100%" border="1">
                        <tr>
                            <td colspan="4" style="text-align:center;">
                            <h4><%= docinfo.title %></h4>
                            </td>
                        </tr>
                        <tr>
                            <td><b>通知号：</b><%= docinfo.docno %></td>
                            <td><b>发布时间：</b><%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", docinfo.createtime) %></td>
                            <td><b>发布部门：</b><%= docinfo.sendpart %></td>
                            <td><b>发布人：</b><%= docinfo.sendername %></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <b>收件人：</b>
                                <%= ViewData["receiver"] %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div style="min-height:200px; padding:10px;">
                                    <div>
                                        <h4>公文内容:</h4>
                                    </div>
                                    <div style="font-size:14px; min-height:260px;">
                                        <%= docinfo.contents %>
                                    </div>
                                    <div style="">
                                        <b>附件：</b>
                                        <a target="_blank" href="<%= ViewData["rootUri"] %><%= docinfo.attachpath %>"><%= docinfo.attachname %><% if (!String.IsNullOrEmpty(docinfo.attachpath)) { %>（<%= decimal.Round(((decimal)docinfo.attachsize / 1024/1024), 1) %>MB）<% } %></a>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div style="padding:10px;">
                                    <div>
                                        <h4>领导批示:</h4>
                                    </div>
                                    <div style="font-size:14px;">
                                        <% if (loglist != null) {
                                               foreach (var item in loglist)
                                               { %>
                                               <div class="row">
                                                <div class="col-sm-10">
                                                    <p><%= item.execnote %> 【<%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.createtime)%>】</p>
                                                </div>
                                                <div class="col-sm-2">
                                                    <p><%= item.username %></p>
                                                </div>
                                               </div>
                                               <% 
                                               }
                                                %>
                                        <% } %>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="editabletr">
                            <td colspan="4">
                                <div class="row">
                                    <div class="col-sm-1">
                                        <b>领导批示</b>
                                    </div>
                                    <div class="col-sm-11">
                                        <textarea id="execnote" name="execnote" style="height:140px; width:100%;"></textarea>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="editabletr">
                            <td colspan="4">
                                <div class="row">
                                    <div class="col-sm-1">
                                        <b>流转人</b>
                                    </div>
                                    <div class="col-sm-2">
                                        <a class="btn btn-white btn-default btn-round" onclick="AddCrew()">
	                                        <i class="ace-icon fa fa-plus red2"></i>
	                                        添加
                                        </a>
                                    </div>
                                    <div class="col-sm-9">
                                        <div class="clearfix" style="width:100%">
					                        <select multiple class="chosen-select" id="receiver" name="receiver" data-placeholder="请选择收信人">
                                            <% if (ViewData["userlist"] != null) {
                                                   foreach (var item in (List<UserInfo>)ViewData["userlist"])
                                                   { %>
                                                <option value="<%= item.uid %>" ><%= item.realname %></option>
                                                   <% 
                                                   }
                                                    %>
                                            <% } %>
					                        </select>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="editabletr">
                            <td colspan="4">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div style="float:right;">
                                            <button class="btn btn-white btn-default btn-round loading-btn" type="submit" onclick="setSignFlag(0);" data-loading-text="提交中...">
	                                            <i class="ace-icon fa fa-pencil red2"></i>
	                                            签收
                                            </button>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <button class="btn btn-white btn-default btn-round loading-btn" type="submit" onclick="setSignFlag(1);" data-loading-text="提交中...">
	                                            <i class="ace-icon fa fa-share green"></i>
	                                            流转
                                            </button>
                                            <input type="hidden" id="uid" name="uid" value="<%= ViewData["uid"] %>" />
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
            </div>
            <div class="col-sm-2"></div>
        </div>
    </div>
</div><!-- #dialog-specdata -->

<div id="dialog-message" class="hide">
    <div class="row">
		<div class="col-sm-4">
	        <div class="tabbable">
		        <ul class="nav nav-tabs" id="myTab">
			        <li class="active"><a data-toggle="tab" href="#home">干部</a></li>
			        <li><a data-toggle="tab" href="#profile">列车长</a></li>
                </ul>
		        <div class="tab-content ">
			        <div id="home" class="tab-pane in active">
				        <div id="tree2" class="tree"></div>
			        </div>

			        <div id="profile" class="tab-pane">
				        <div id="tree3" class="tree"></div>
			        </div>
                </div>
            </div>
		</div>
		<div class="col-sm-4">
            <div class="widget-box">
				<div class="widget-body">
					<div class="widget-main padding-8">
						<select id="userlist" name="userlist" multiple="multiple" style="height:300px; width:100%">
						</select>
					</div>
				</div>
            </div>
            <a class="btn btn-white btn-default btn-round" onclick="addAllUser()">
	            <i class="ace-icon fa fa-plus red2"></i>
	            全部添加
            </a>
            <a class="btn btn-white btn-default btn-round" onclick="addSelectedUser()">
	            <i class="ace-icon fa fa-plus red2"></i>
	            添加选择
            </a>
        </div>

		<div class="col-sm-4">
            <div class="widget-box">
				<div class="widget-body">
					<div class="widget-main padding-8">
						<select id="sellist" name="sellist" multiple="multiple" style="height:300px; width:100%">
						</select>
					</div>
				</div>
            </div>
            <a class="btn btn-white btn-default btn-round" onclick="removeAllUser()">
	            <i class="ace-icon fa fa-trash-o red2"></i>
	            全部删除
            </a>
            <a class="btn btn-white btn-default btn-round" onclick="removeSelectedUser()">
	            <i class="ace-icon fa fa-trash-o red2"></i>
	            删除选择
            </a>
        </div>

    </div>
</div><!-- #dialog-message -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/jquery-ui.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/chosen.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>

	<script src="<%= ViewData["rootUri"] %>Content/js/chosen.jquery.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/fuelux/fuelux.tree.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.validate.min.js"></script>

	<script type="text/javascript">
        var chosentag;
        jQuery(function ($) {

            chosentag = $('.chosen-select').chosen({ allow_single_deselect: true });
            //resize the chosen on window resize
            $(window).on('resize.chosen', function () {
                var w = $('.chosen-select').parent().width();
                $('.chosen-select').next().css({ 'width': w });
            }).trigger('resize.chosen');

            //override dialog's title function to allow for HTML titles
            $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                _title: function (title) {
                    var $title = this.options.title || '&nbsp;'
                    if (("title_html" in this.options) && this.options.title_html == true)
                        title.html($title);
                    else title.text($title);
                }
            }));

            $.validator.messages.required = "必须要填写";
            $.validator.messages.number = jQuery.validator.format("请输入一个有效的数字.");
            $.validator.messages.minlength = jQuery.validator.format("必须由至少{0}个字符组成.");
            $.validator.messages.maxlength = jQuery.validator.format("必须由最多{0}个字符组成");
            $('#validation-form').validate({
                errorElement: 'span',
                errorClass: 'help-block',
                rules: {
                    
                },
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
                    $(".loading-btn").removeAttr("disabled");
                }
            });

            $('#tree2').ace_tree({
                dataSource: treeDataSource2,
                loadingHTML: '<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
                'open-icon': 'ace-icon fa fa-folder-open',
                'close-icon': 'ace-icon fa fa-folder',
                'selectable': true,
                'selected-icon': null,
                'unselected-icon': null
            });
            $('#tree2').on('selected', function (evt, data) {
                var selid = $(data.info[0].name).data("id");
                var selkind = $(data.info[0].name).data("kind");
                $.ajax({
                    type: "GET",
                    url: rootUri + "User/FindExecutiveList",
                    dataType: "json",
                    data: {
                        selid: selid,
                        selkind: selkind
                    },
                    success: function (data) {
                        var rhtml = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
                            }
                        }
                        $("#userlist").html(rhtml);
                    }
                });
            });

            $('#tree3').ace_tree({
                dataSource: treeDataSource3,
                loadingHTML: '<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
                'open-icon': 'ace-icon fa fa-folder-open',
                'close-icon': 'ace-icon fa fa-folder',
                'selectable': true,
                'selected-icon': null,
                'unselected-icon': null
            });
            $('#tree3').on('selected', function (evt, data) {
                var selid = $(data.info[0].name).data("id");
                $.ajax({
                    type: "GET",
                    url: rootUri + "User/FindGroupCheZhangList",
                    dataType: "json",
                    data: {
                        selid: selid
                    },
                    success: function (data) {
                        var rhtml = "";
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                rhtml += "<option value='" + data[i].uid + "'>" + data[i].realname + "</option>";
                            }
                        }
                        $("#userlist").html(rhtml);
                    }
                });
            });

        });

        function addAllUser() {
            $('#userlist option').each(function () {
                //var selval = $(this).val();
                $("#sellist").append($(this));
                //$("#receiver option[value=" + selval + "]").attr("selected", "selected");
            });
            //$('#receiver').trigger('chosen:updated');
        }

        function addSelectedUser() {
            $('#userlist option:selected').each(function () {
                //var selval = $(this).val();
                //$("#receiver option[value=" + selval + "]").attr("selected", "selected");
                $("#sellist").append($(this));
            });
            //$('#receiver').trigger('chosen:updated');
        }

        function removeAllUser() {
            $('#sellist option').each(function () {
                //var selval = $(this).val();
                //$("#receiver option[value=" + selval + "]").prop("selected", false);
                $(this).remove();
            });

            //$('#receiver').trigger('chosen:updated');
        }

        function removeSelectedUser() {
            $('#sellist option:selected').each(function () {
                //var selval = $(this).val();
                //$("#receiver option[value=" + selval + "]").prop("selected", false);
                $(this).remove();
            });
            //$('#receiver').trigger('chosen:updated');
        }

var DataSourceTree = function(options) {
	this._data 	= options.data;
	this._delay = options.delay;
}

DataSourceTree.prototype.data = function(options, callback) {
	var self = this;
	var $data = null;

	if(!("name" in options) && !("type" in options)){
		$data = this._data;//the root tree
		callback({ data: $data });
		return;
	}
	else if("type" in options && options.type == "folder") {
		if("additionalParameters" in options && "children" in options.additionalParameters)
			$data = options.additionalParameters.children;
		else $data = {}//no data
	}
	
	if($data != null)//this setTimeout is only for mimicking some random delay
        callback({ data: $data });
		//setTimeout(function(){callback({ data: $data });} , parseInt(Math.random() * 500) + 200);

	//we have used static data here
	//but you can retrieve your data dynamically from a server using ajax call
	//checkout examples/treeview.html and examples/treeview.js for more info
};

<% var sectorlist = (List<tbl_railsector>)(ViewData["sectorlist"]);
var teamlist = (List<tbl_railteam>)(ViewData["teamlist"]);
var grouplist = (List<GroupInfo>)(ViewData["grouplist"]);
 %>

var ace_icon = ace.vars['icon'];
//class="'+ace_icon+' fa fa-file-text grey"
//becomes
//class="ace-icon fa fa-file-text grey"
var tree_data_2 = {
	'sector' : {name: '科室', type: 'folder', 'icon-class':'blue'},
	'team' : {name: '车队', type: 'folder', 'icon-class':'blue'}
}
tree_data_2['sector']['additionalParameters'] = {
	'children' : [
<% if (sectorlist != null) {
foreach(var item in sectorlist) {
 %>
		{name: '<i data-kind="科室干部" data-id="<%= item.uid %>" class="'+ace_icon+' fa fa-bars blue"></i> <%= item.sectorname %>', type: 'item'},
<% 
}
} %>
	]
}
tree_data_2['team']['additionalParameters'] = {
	'children' : {
<% if (teamlist != null) {
foreach(var item in teamlist) {
 %>
    'team_<%= item.uid %>' : {name: '<i data-kind="车队干部" data-id="<%= item.uid %>" class="'+ace_icon+' fa fa-bars blue"></i> <%= item.teamname %>', type: 'item'},
<% 
}
} %>
	}
}

<% 
if (teamlist != null) {
    foreach(var item in teamlist) {
        var subgroups = grouplist.Where(m => m.teamid == item.uid).ToList();
        if (subgroups.Count() > 0) {
        %>
tree_data_2['team']['additionalParameters']['children']['team_<%= item.uid %>']['additionalParameters'] = {
	'children' : [
    <% foreach(var subitem in subgroups) { %>
		{name: '<i class="'+ace_icon+' fa fa-bars blue"></i> <%= subitem.groupname %>', type: 'item'},
    <% } %>
	]
}
        <% }
    }
}
 %>

var treeDataSource2 = new DataSourceTree({data: tree_data_2});

var tree_data_3 = {
	'team' : {name: '车队', type: 'folder', 'icon-class':'blue'}
}
tree_data_3['team']['additionalParameters'] = {
	'children' : {
<% if (teamlist != null) {
foreach(var item in teamlist) {
    var subgroups = grouplist.Where(m => m.teamid == item.uid).ToList();
 %>
 <% if (subgroups.Count() > 0) { %>
    'team_<%= item.uid %>' : {name: '<%= item.teamname %>', type: 'folder', 'icon-class':''},
 <% } else { %>
 <% } %>
<% 
}
} %>
	}
}

<% 
if (teamlist != null) {
    foreach(var item in teamlist) {
        var subgroups = grouplist.Where(m => m.teamid == item.uid).ToList();
        if (subgroups.Count() > 0) {
        %>
tree_data_3['team']['additionalParameters']['children']['team_<%= item.uid %>']['additionalParameters'] = {
	'children' : [
    <% foreach(var subitem in subgroups) { %>
		{name: '<i data-kind="group" data-id="<%= subitem.uid %>" class="'+ace_icon+' fa fa-bars blue"></i> <%= subitem.groupname %>', type: 'item'},
    <% } %>
	]
}
        <% }
    }
}
 %>

var treeDataSource3 = new DataSourceTree({data: tree_data_3});

	    var crewdlg;
	    function AddCrew() {

	        crewdlg = $("#dialog-message").removeClass('hide').dialog({
	            modal: true,
	            width: 800,
                height:500,
	            title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> 添加收信人</h4></div>",
	            title_html: true,
	            buttons: [
					{
					    text: "确定",
					    "class": "btn btn-primary btn-xs",
					    click: function () {
			                $('#sellist option').each(function () {
                                var selval = $(this).val();
			                    $("#receiver option[value=" + selval + "]").attr("selected", "selected");
			                });
                            $('#receiver').trigger('chosen:updated');
					        $(this).dialog("close");
					    }
					},
					{
					    text: "取消",
					    "class": "btn btn-default btn-xs",
					    click: function () {
					        $(this).dialog("close");
					    }
					}
				]
	        });
        }

	    function submitform() {
            $(".loading-btn").attr("disabled", "disabled");

	        $.ajax({
	            type: "POST",
	            url: rootUri + "Document/SubmitSignForward?flag=" + signflag,
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
	                //$('.loading-btn').button('reset');
                    $(".loading-btn").removeAttr("disabled");
	            }
	        });
	    }

        var signflag = 0;
	    function redirectToListPage(status) {
	        if (status.indexOf("error") != -1) {
                $(".loading-btn").removeAttr("disabled");
	        } else {
	            window.location = rootUri + "Document/MyDocList";
	        }
	    }

        function setSignFlag(flag)
        {
            signflag = flag;
        }
    </script>
</asp:Content>