<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
    var inboxinfo = (InboxInfo)ViewData["receivelist"];
    var receivedlist = inboxinfo.maillist; %>
<div class="page-header">
	<h1>
		收件箱
	</h1>
</div><!-- /.page-header -->

<div class="row">
	<div class="col-xs-12">
		<!-- PAGE CONTENT BEGINS -->
		<div class="row">
			<div class="col-xs-12">
				<div class="tabbable">
					<ul id="inbox-tabs" class="inbox-tabs nav nav-tabs padding-16 tab-size-bigger tab-space-1">
						<li class="li-new-mail pull-right">
							<a data-toggle="tab" href="#write" data-target="write" class="btn-new-mail">
								<span class="btn btn-purple no-border">
									<i class="ace-icon fa fa-envelope bigger-130"></i>
									<span class="bigger-110">写信</span>
								</span>
							</a>
						</li><!-- /.li-new-mail -->

						<li class="active">
							<a data-toggle="tab" href="#inbox" data-target="inbox">
								<i class="blue ace-icon fa fa-inbox bigger-130"></i>
								<span class="bigger-110">收件箱</span>
							</a>
						</li>

						<li>
							<a data-toggle="tab" href="#sent" data-target="sent">
								<i class="orange ace-icon fa fa-location-arrow bigger-130"></i>
								<span class="bigger-110">已发送</span>
							</a>
						</li>
					</ul>

					<div class="tab-content no-border no-padding">
						<div id="inbox" class="tab-pane in active">
							<div class="message-container">
								<div id="id-message-list-navbar" class="message-navbar clearfix">
									<div class="message-bar">
										<div class="message-infobar" id="id-message-infobar">
											<span class="blue bigger-150">收件箱</span>
											<!--<span class="grey bigger-110">(<%= receivedlist.Count() %> 未读)</span>-->
										</div>

										<%--<div class="message-toolbar hide">
											<div class="inline position-relative align-left">
												<button type="button" class="btn-white btn-primary btn btn-xs dropdown-toggle" data-toggle="dropdown">
													<span class="bigger-110">操作</span>

													<i class="ace-icon fa fa-caret-down icon-on-right"></i>
												</button>

												<ul class="dropdown-menu dropdown-lighter dropdown-caret dropdown-125">
													<li>
														<a href="#">
															<i class="ace-icon fa fa-mail-reply blue"></i>&nbsp; 回复
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-mail-forward green"></i>&nbsp; 转发
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-folder-open orange"></i>&nbsp; 附件
														</a>
													</li>

													<li class="divider"></li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-eye blue"></i>&nbsp; 已读表示
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-eye-slash green"></i>&nbsp; 未读表示
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-flag-o red"></i>&nbsp; 红旗
														</a>
													</li>

													<li class="divider"></li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-trash-o red bigger-110"></i>&nbsp; 删除
														</a>
													</li>
												</ul>
											</div>

											<div class="inline position-relative align-left">
												<button type="button" class="btn-white btn-primary btn btn-xs dropdown-toggle" data-toggle="dropdown">
													<i class="ace-icon fa fa-folder-o bigger-110 blue"></i>
													<span class="bigger-110">移动到</span>

													<i class="ace-icon fa fa-caret-down icon-on-right"></i>
												</button>

												<ul class="dropdown-menu dropdown-lighter dropdown-caret dropdown-125">
													<li>
														<a href="#">
															<i class="ace-icon fa fa-stop pink2"></i>&nbsp; Tag#1
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-stop blue"></i>&nbsp; Family
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-stop green"></i>&nbsp; Friends
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-stop grey"></i>&nbsp; Work
														</a>
													</li>
												</ul>
											</div>

											<button type="button" class="btn btn-xs btn-white btn-primary">
												<i class="ace-icon fa fa-trash-o bigger-125 orange"></i>
												<span class="bigger-110">删除</span>
											</button>
										</div>--%>
									</div>

									<div>
										<div class="messagebar-item-left">
											<label class="inline middle">
												<input type="checkbox" id="id-toggle-all" class="ace" />
												<span class="lbl"></span>
											</label>

											&nbsp;
											<div class="inline position-relative">
												<a href="#" data-toggle="dropdown" class="dropdown-toggle">
													<i class="ace-icon fa fa-caret-down bigger-125 middle"></i>
												</a>

												<ul class="dropdown-menu dropdown-lighter dropdown-100">
													<li>
														<a id="id-select-message-all" href="#">All</a>
													</li>

													<li>
														<a id="id-select-message-none" href="#">None</a>
													</li>

													<li class="divider"></li>

													<li>
														<a id="id-select-message-unread" href="#">Unread</a>
													</li>

													<li>
														<a id="id-select-message-read" href="#">Read</a>
													</li>
												</ul>
											</div>
										</div>

										<div class="messagebar-item-right hide">
											<div class="inline position-relative">
												<a href="#" data-toggle="dropdown" class="dropdown-toggle">
													排序 &nbsp;
													<i class="ace-icon fa fa-caret-down bigger-125"></i>
												</a>

												<ul class="dropdown-menu dropdown-lighter dropdown-menu-right dropdown-100">
													<li>
														<a href="#">
															<i class="ace-icon fa fa-check green"></i>
															按日期
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-check invisible"></i>
															发件人
														</a>
													</li>

													<li>
														<a href="#">
															<i class="ace-icon fa fa-check invisible"></i>
															标题
														</a>
													</li>
												</ul>
											</div>
										</div>

										<div class="nav-search minimized">
											<form class="form-search" id="searchform">
												<span class="input-icon">
													<input type="text" autocomplete="off" id="searchkey" class="input-small nav-search-input" placeholder="请搜索邮箱..." />
													<i class="ace-icon fa fa-search nav-search-icon"></i>
												</span>
											</form>
										</div>
									</div>
								</div>

								<div id="id-message-item-navbar" class="hide message-navbar clearfix">
									<div class="message-bar">
										<div class="message-toolbar">
										</div>
									</div>

									<div>
										<div class="messagebar-item-left">
											<a href="#" class="btn-back-message-list">
												<i class="ace-icon fa fa-arrow-left blue bigger-110 middle"></i>
												<b class="bigger-110 middle">返回</b>
											</a>
										</div>
                                    </div>
								</div>

								<div id="id-message-new-navbar" class="hide message-navbar clearfix">
									<div class="message-bar">
									</div>

									<div>
										<div class="messagebar-item-left">
											<a href="#" class="btn-back-message-list">
												<i class="ace-icon fa fa-arrow-left bigger-110 middle blue"></i>
												<b class="middle bigger-110">返回</b>
											</a>
										</div>

										<div class="messagebar-item-right">
											<span class="inline btn-send-message">
												<button type="button" onclick="sendmail();" id="btnloading-btnSend" class="loading-btn btn btn-sm btn-primary no-border btn-white btn-round" data-loading-text="发送中...">
													发送
													<i class="ace-icon fa fa-arrow-right icon-on-right"></i>
												</button>
											</span>
										</div>
									</div>
								</div>

								<div class="message-list-container">
									<div class="message-list" id="message-list">
                                        <% foreach (var item in receivedlist)
                                           { %>
										<div class="message-item <% if (item.isread == 0) { %>message-unread<% } %>">
											<label class="inline">
												<input type="checkbox" class="ace" data-mailid="<%= item.uid %>" />
												<span class="lbl"></span>
											</label>

											<i class="message-star ace-icon fa fa-star-o light-grey"></i>
											<span class="sender" title="Alex John Red Smith"><%= item.sender %></span>
											<span class="time"><%= String.Format("{0:yyyy年MM月dd日 HH:mm:ss}", item.sendtime)%></span>

                                            <% if (item.attachsize > 0)
                                               { %>
											<span class="attachment">
												<i class="ace-icon fa fa-paperclip"></i>
											</span>
                                            <% } %>

											<span class="summary">
												<span class="text">
													<%= CommonModel.han_cut(item.title, 70) %>
												</span>
											</span>
										</div>
                                           <% 
                                           }
                                            %>
									</div>
								</div>

								<div class="message-footer clearfix">
									<div class="pull-left"> 共有<%= inboxinfo.totalcnt %>件  </div>

									<div class="pull-right">
										<div class="inline middle"> <label id="currpagenum"><%= inboxinfo.currpage %></label> / <label id="totalpagenum"><%= inboxinfo.totalcnt / 10 + 1 %></label> </div>

										&nbsp; &nbsp;
										<ul class="pagination middle">
											<li class="disabled" data-action="first">
												<a href="javascript:(0);" onclick="return false;">
													<i class="ace-icon fa fa-step-backward middle"></i>
												</a>
											</li>

											<li class="disabled" data-action="prev">
												<a href="javascript:(0);" onclick="return false;">
													<i class="ace-icon fa fa-caret-left bigger-140 middle"></i>
												</a>
											</li>

											<li>
												<span>
													<input value="1" maxlength="3" type="text" id="txtpagenum" />
												</span>
											</li>

											<li data-action="next">
												<a href="javascript:(0);" onclick="return false;">
													<i class="ace-icon fa fa-caret-right bigger-140 middle"></i>
												</a>
											</li>

											<li data-action="last">
												<a href="javascript:(0);" onclick="return false;">
													<i class="ace-icon fa fa-step-forward middle"></i>
												</a>
											</li>
										</ul>
									</div>
								</div>
							</div>
						</div>
					</div><!-- /.tab-content -->
				</div><!-- /.tabbable -->
			</div><!-- /.col -->
		</div><!-- /.row -->

		<form id="id-message-form" class="hide form-horizontal message-form col-xs-12">
			<div>
				<div class="form-group">
					<label class="col-sm-3 control-label no-padding-right" for="form-field-recipient">收信人:</label>

                    <div class="col-sm-1">
                        <a class="btn btn-white btn-default btn-round" onclick="AddCrew()">
	                        <i class="ace-icon fa fa-plus red2"></i>
	                        添加
                        </a>
                    </div>
					<div class="col-sm-8">
                    <div class="clearfix" style="width:100%">
					    <select multiple class="chosen-select" id="receiver" name="receiver" data-placeholder="请输入收信人">
                        <% if (ViewData["userlist"] != null) {
                               foreach (var item in (List<UserInfo>)ViewData["userlist"])
                               { %>
                            <option value="<%= item.uid %>"><%= item.realname %></option>
                               <% 
                               }
                                %>
                        <% } %>
					    </select>
                    </div>
					</div>
				</div>

				<div class="hr hr-18 dotted"></div>

				<div class="form-group">
					<label class="col-sm-3 control-label no-padding-right" for="title">标题:</label>

					<div class="col-sm-6 col-xs-12">
						<div class="input-icon block col-xs-12 no-padding">
							<input maxlength="100" type="text" class="col-xs-12" name="title" id="title" placeholder="请输入标题" />
							<i class="ace-icon fa fa-comment-o"></i>
						</div>
					</div>
				</div>

				<div class="hr hr-18 dotted"></div>

				<div class="form-group">
					<label class="col-sm-3 control-label no-padding-right">
						<span class="inline space-24 hidden-480"></span>
						内容:
					</label>

					<div class="col-sm-9">
						<div class="kind-editor">
                            <textarea class="form-control" id="contents" name="contents" style="height:300px; width:780px;"></textarea>
                        </div>
					</div>
				</div>

				<div class="hr hr-18 dotted"></div>

				<div class="form-group no-margin-bottom">
					<label class="col-sm-3 control-label no-padding-right">附件:</label>

					<div class="col-sm-9">
						<div id="form-attachments" class="col-sm-3" >
							<input type="file" name="attachfile" id="attachfile" /><%--选择附件--%>
                            <div id="attachmentslist">
                               <%-- <label id="attachfilename"></label>--%>
                                <input type="hidden" id="attachpath" name="attachpath" value="" />
                                <input type="hidden" id="attachfilesize" name="attachfilesize" value="0" />
                                <input type="hidden" id="filename" name="filename" />
                            </div>
						</div>
                        <a href="javascript:(0)" onclick="changemailtitle();return false;" class="col-sm-3 control-label">取附件名做标题</a>
					</div>
				</div>
				<div class="space"></div>
                <input type="hidden" id="esccontent" name="esccontent" />
			</div>
		</form>

        <%--邮件详情--%>
		<div class="hide message-content" id="id-message-content">
			<div class="message-header clearfix">
				<div class="pull-left">
					<span class="blue bigger-125" id="msg-title"> </span>
                    <input type="text" id="mailid" style="display:none" value="" />
					<div class="space-4"></div>

                    <div>   
					    <i class="ace-icon fa fa-star orange2"></i>

					    &nbsp;
                         <span class="time grey" >发件人:</span>
					    &nbsp;

					    <img class="middle" alt="" src="" width="32" id="msg-sender-img" />
					    &nbsp;
					    <a href="#" class="sender" id="msg-sender"></a>
                        <input type="text" value="" id="msg-senderid" style="display:none"/>
					    &nbsp;
					    <i class="ace-icon fa fa-clock-o bigger-110 orange middle"></i>
					    <span class="time grey" id="msg-time"></span>
                     </div>

                     <div id="receivers" style="position:relative;left:33px">   
                     </div>
				</div>

				<div class="pull-right action-buttons" id="action-tabs">
                
					<a data-toggle="tab" href="#relpay" data-target="relpay">
						回复<i class="ace-icon fa fa-reply green icon-only bigger-130"></i>
					</a>

					<a data-toggle="tab" href="#forward" data-target="forward" >
						转发<i class="ace-icon fa fa-mail-forward blue icon-only bigger-130"></i>
					</a>
				    <a href="javascript:(0)" onclick="sendreceipt();return false;" id="receipt"  class="hide"> 
						发送回执<i class="ace-icon fa fa-envelope red icon-only bigger-130"></i> 
					</a>
                    <a  id="alreadysent" class="hide red"> 
						已发送
					</a>
				</div>
			</div>

			<div class="hr hr-double"></div>

			<div class="message-body" id="msg-contents">
			</div>

			<div class="hr hr-double"></div>

			<div class="message-attachment clearfix">
				<div class="attachment-title">
					<span class="blue bolder bigger-110">附件</span>
					&nbsp;
					<span class="grey" id="attachment-size"></span>

					<div class="inline position-relative">
						<a href="#" data-toggle="dropdown" class="dropdown-toggle">
							&nbsp;
							<i class="ace-icon fa fa-caret-down bigger-125 middle"></i>
						</a>

						<ul class="dropdown-menu dropdown-lighter">
							<li>
								<a href="#" class="downlink">下载文件</a>
							</li>
						</ul>
					</div>
				</div>

				&nbsp;
				<ul class="attachment-list pull-left list-unstyled" id="attachment_list">
					<%--<li>
						<a href="#" class="attached-file downlink">
							<i class="ace-icon fa fa-file-o bigger-110"></i>
							<span class="attached-name" id="attached-name"></span>
						</a>

						<span class="action-buttons">
							<a href="#" class="downlink">
								<i class="ace-icon fa fa-download bigger-125 blue"></i>
							</a>
						</span>
					</li>--%>
				</ul>
                <input type="text" value="0" id="attachsize_forstore" style="display:none"/>
			</div>
		</div><!-- /.message-content -->

		<!-- PAGE CONTENT ENDS -->
	</div><!-- /.col -->
</div><!-- /.row -->

<div id="dialog-message" class="hide">
    <div class="row">
		<div class="col-sm-4">
	        <div class="tabbable">
		        <ul class="nav nav-tabs" id="myTab">
                <%if (ViewData["sectorlist"] != null) {
                      %>
                    <li class="active"><a data-toggle="tab" href="#home">干部</a></li>
			        <li><a data-toggle="tab" href="#profile">列车长</a></li>   
                    <li><a data-toggle="tab" href="#grouptab">班组成员</a></li>
                      <%}
                else{%>
                    <li class="hide"><a data-toggle="tab" href="#home">干部</a></li>
			        <li class="hide"><a data-toggle="tab" href="#profile">列车长</a></li> 
                    <li class="active"><a data-toggle="tab" href="#grouptab">班组成员</a></li>  
                  <%} %>
			       
			        
                </ul>
		        <div class="tab-content ">
                 <%if (ViewData["sectorlist"] != null) {
                      %>
			        <div id="home" class="tab-pane in active">
				        <div id="tree2" class="tree"></div>
			        </div>

			        <div id="profile" class="tab-pane">
				        <div id="tree3" class="tree"></div>
			        </div>

			        <div id="grouptab" class="tab-pane">
				        <div id="tree4" class="tree"></div>
			        </div>
                        <%}
                else{%>
                    <div id="grouptab" class="tab-pane in active">
				        <div id="tree4" class="tree"></div>
			        </div>
               <% }%>
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
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/themes/default/default.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/chosen.css" />
	<link rel="stylesheet" type="text/css" href="<%= ViewData["rootUri"] %>Content/plugins/uploadify/uploadify.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/bootstrap-tag.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/chosen.jquery.min.js"></script>

	<script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.hotkeys.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery-ui.custom.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.ui.touch-punch.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.slimscroll.min.js"></script>
    <script charset="utf-8" src="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/kindeditor-min.js"></script>
	<script charset="utf-8" src="<%= ViewData["rootUri"] %>Content/plugins/kindeditor-4.1.7/lang/zh_CN.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/uploadify/jquery.uploadify.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/fuelux/fuelux.tree.min.js"></script>
	<script src="<%= ViewData["rootUri"] %>Content/js/jquery.slimscroll.min.js"></script>

	<!-- inline scripts related to this page -->
	<script type="text/javascript">
	    var editor;
	    var chosentag;
        var currpage = 1;
        var searchkey = "";
        var currtab = "inbox";



         function replay(currentTab,title,content){
               // alert(content);
	            if ($('.message-form').is(':visible')) return;

	            var message = $('.message-list');
	            $('.message-container').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-160"></i></div>');

	            setTimeout(function () {
	                message.next().addClass('hide');
	                $('.message-container').find('.message-loading-overlay').remove();
	                $('.message-list').addClass('hide');
	                $('.message-footer').addClass('hide');
	                $('.message-form').removeClass('hide');
	                $('.message-navbar').addClass('hide');
	                $('#id-message-new-navbar').removeClass('hide');
	                //reset form??
                   
//                      var newHtml = "<select multiple class=\"chosen-select\" id=\"receiver\" name=\"receiver\" data-placeholder=\"请输入收信人\">";
//                      var combobox = document.getElementById("receiver");
//                      var sel_name = "";
//                      for (i = 0; i < combobox.options.length; i++){
//                          var itemhtml_val = combobox.options[i].value;
//                          var itemhtml_text = combobox.options[i].text;
// 
//                          if(itemhtml_text == '蔡成哲'){
//  			                newHtml += "\n<option value='" + itemhtml_val + "' selected>" + itemhtml_text + "</option>";
//  		                    sel_name = itemhtml_text;
//                          } else {
//  			                newHtml += "\n<option value='" + itemhtml_val + "'>" + itemhtml_text + "</option>";
//                          }
//                      }
//                   $("#receiver").html(newHtml);
                     var w = $('#receiver').parent().width();
 	                 $('#receiver').next().css({ 'width': w });
                     $("#attachmentslist").children("div").remove();


                    if (currentTab == 'relpay') {
                        if ( $("#title").val==null) {
                         $("#title").attr("value","回复:"+title);
                        }
                        else{
                         $("#title").val("回复:"+title);
                        }
                         var senderid= $("#msg-senderid").val();
                         $("#receiver option").removeAttr('selected');
                         $("#receiver option[value=" + senderid + "]").attr("selected", "selected");
                         $("#receiver").trigger("chosen:updated");
                         if (editor != undefined) {
                            editor.html("");
                            editor.sync();
                         }
                    }
                    //转发
                    else if (currentTab == 'forward') {
                         $("#receiver option").removeAttr('selected');
                         $("#receiver").trigger("chosen:updated");
                         if ( $("#title").val==null) {
                             $("#title").attr("value","转发:"+title);
                        }
                        else{
                             $("#title").val("转发:"+title);
                        }
                         if (editor != undefined) {
                            editor.html(content);
                            editor.sync();
                         }
                            
                            if($("#attachment_list").children().length!=0){
                                  var filename_obj = [];
                                  $("span[name^='filename_indetail']").each(function(i, o){
                                      filename_obj[i] = $(o).html();
                                  });
                                  var filehref_obj = [];
                                  $("input[name^='filehref_indetail']").each(function(i, o){
                                      filehref_obj[i]=$(o).val();
                                  });
                                  

                                    var forward_html="";
                                   for(i=0;i<$("#attachment_list").children().length;i++){
                                  
                                       forward_html += "<div id=\"SWFUpload_0_0\" class=\"uploadify-queue-item\">";					
                                       forward_html += "<div class=\"cancel\">";						
                                       forward_html += "<a onclick='removediv(this);return false;' href='javascript:(0)'>X</a>";				
                                       forward_html += "</div>";					
                                       forward_html += "<span class=\"fileName\">"+filename_obj[i]+"</span>";							
                                       forward_html += "<input type=\"hidden\"  name=\"attachpath_singel\" value='"+ filehref_obj[i] +"'/>";
                                       forward_html += "<input type=\"hidden\"  name=\"attachfilesize_singel\" value=' "+ 0 +"' />";
                                       forward_html += "<input type=\"hidden\"  name='filename_singel' value='"+ filename_obj[i] +"' />";
                                       forward_html += "</div>";
                                     }
                                      
                                      $("#attachmentslist").append(forward_html);
                                      getfileinfo();
	                                  $("#attachfilesize").val($("#attachsize_forstore").val());
                            }

	               //  $('.message-form').get(0).reset();
	           }
	           }, 300 + parseInt(Math.random() * 300));
            }


	    jQuery(function ($) {
	        chosentag = $('.chosen-select').chosen({ allow_single_deselect: true });
	        //resize the chosen on window resize
	        $(window).on('resize.chosen', function () {
	            var w = $('.chosen-select').parent().width();
	            $('.chosen-select').next().css({ 'width': w });
	        }).trigger('resize.chosen');

	        //handling tabs and loading/displaying relevant messages and forms
	        //not needed if using the alternative view, as described in docs
	        $('#inbox-tabs a[data-toggle="tab"]').on('show.bs.tab', function (e) {
	            var currentTab = $(e.target).data('target');
                currtab = currentTab;
	            if (currentTab == 'write') {
	                Inbox.show_form();
	            }
	            else if (currentTab == 'inbox' || currentTab == 'sent') {
                    $(".message-form").addClass("hide");
	                Inbox.show_list();
                }
	        })


            //easi.
            $('#action-tabs a[data-toggle="tab"]').on('show.bs.tab', function (e) {
	            var currentTab = $(e.target).data('target');
                currtab = currentTab;
                var title=$("#msg-title").text();
                var content=$("#msg-contents").text();
	            replay(currentTab,title,content);
	           
	        })

           




	        //basic initializations
	        $('.message-list .message-item input[type=checkbox]').removeAttr('checked');
	        $('.message-list').on('click', '.message-item input[type=checkbox]', function () {
	            $(this).closest('.message-item').toggleClass('selected');
	            if (this.checked) Inbox.display_bar(1); //display action toolbar when a message is selected
	            else {
	                Inbox.display_bar($('.message-list input[type=checkbox]:checked').length);
	                //determine number of selected messages and display/hide action toolbar accordingly
	            }
	        });

            $('#tree2').ace_tree({
			    dataSource: treeDataSource2 ,
			    loadingHTML:'<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
			    'open-icon' : 'ace-icon fa fa-folder-open',
			    'close-icon' : 'ace-icon fa fa-folder',
			    'selectable' : true,
			    'selected-icon' : null,
			    'unselected-icon' : null
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
			    dataSource: treeDataSource3 ,
			    loadingHTML:'<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
			    'open-icon' : 'ace-icon fa fa-folder-open',
			    'close-icon' : 'ace-icon fa fa-folder',
			    'selectable' : true,
			    'selected-icon' : null,
			    'unselected-icon' : null
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

            $('#tree4').ace_tree({
			    dataSource: treeDataSource4 ,
			    loadingHTML:'<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
			    'open-icon' : 'ace-icon fa fa-folder-open',
			    'close-icon' : 'ace-icon fa fa-folder',
			    'selectable' : true,
			    'selected-icon' : null,
			    'unselected-icon' : null
		    });

            $('#tree4').on('selected', function (evt, data) {
                var selid = $(data.info[0].name).data("id");
	            $.ajax({
	                type: "GET",
	                url: rootUri + "User/FindGroupUserList",
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

	        //check/uncheck all messages
	        $('#id-toggle-all').removeAttr('checked').on('click', function () {
	            if (this.checked) {
	                Inbox.select_all();
	            } else Inbox.select_none();
	        });

	        //select all
	        $('#id-select-message-all').on('click', function (e) {
	            e.preventDefault();
	            Inbox.select_all();
	        });

	        //select none
	        $('#id-select-message-none').on('click', function (e) {
	            e.preventDefault();
	            Inbox.select_none();
	        });

	        //select read
	        $('#id-select-message-read').on('click', function (e) {
	            e.preventDefault();
	            Inbox.select_read();
	        });

	        //select unread
	        $('#id-select-message-unread').on('click', function (e) {
	            e.preventDefault();
	            Inbox.select_unread();
	        });

	        /////////
	        //override dialog's title function to allow for HTML titles
	        $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
	            _title: function (title) {
	                var $title = this.options.title || '&nbsp;'
	                if (("title_html" in this.options) && this.options.title_html == true)
	                    title.html($title);
	                else title.text($title);
	            }
	        }));

	        //display first message in a new area
	        $('.message-list ').on('click', '.message-item .text', function () {
	            //show the loading icon
	            $('.message-container').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-160"></i></div>');

	            $('.message-inline-open').removeClass('message-inline-open').find('.message-content').remove();

	            var message_list = $(this).closest('.message-list');

	            $('#inbox-tabs a[href="#inbox"]').parent().removeClass('active');

                var mailid = $(this).closest(".message-item").first().children().find("input[type='checkbox']").eq(0).data("mailid");
                var getTimestamp=new Date().getTime();               
		        $.ajax({
		            type: "GET",
		            url: rootUri + "Mail/MailDetail/" + mailid,
		            dataType: "json",
                    date:{
                    getTimestamp:getTimestamp
                    },
		            success: function (data) {
		                if (data != null) {
	                        //hide everything that is after .message-list (which is either .message-content or .message-form)
	                        message_list.next().addClass('hide');
	                        $('.message-container').find('.message-loading-overlay').remove();

	                        //close and remove the inline opened message if any!

	                        //hide all navbars
	                        $('.message-navbar').addClass('hide');
	                        //now show the navbar for single message item
	                        $('#id-message-item-navbar').removeClass('hide');
                            $(".message-attachment").removeClass('hide');

	                        //hide all footers
	                        $('.message-footer').addClass('hide');

                            if (currtab=="inbox") {
	                        $("#receipt").removeClass('hide');
                            $("#mailid").val(data.uid);

                               //检查是否发送回执
                                $.ajax({
		                            type: "GET",
		                            url: rootUri + "Mail/GetReceipt?mailid="+$("#mailid").val()+"&getTimestamp="+getTimestamp,
		                            dataType: "json",
		                            success: function (data) {
		                                if (data == "") {
                                            $("#receipt").removeClass('hide');
                                            $("#alreadysent").addClass('hide');
		                                } 
                                        else{
                                            $("#receipt").addClass('hide');
                                            $("#alreadysent").removeClass('hide');
                                        }
		                            },
                                
		                            error: function (data) {
		                                alert("Error: " + data.status);
		                            }
		                        });
                            }
                            else{
	                        $("#receipt").addClass('hide');
                            $("#alreadysent").addClass('hide');
                            }
                            if (currtab=="sent") {
                                var html="<span class='time grey' >收件人:</span>";
                                
                                for(i=0;i<data.receiverlist.length;i++)
                                {
                                    html += "&nbsp;";
                                    html += "&nbsp;";
                                    html += "<span class='sender' >" + data.receiverlist[i].receivername + "</span>";
                                    if (data.receiverlist[i].receiptno==1) {
                                          html += "&nbsp;";
                                          html+=" <i class='ace-icon fa fa-envelope red icon-only bigger-130'></i>"
                                          html+=" <span class='time grey' >"+ data.receiverlist[i].receipttime +"</span>"
                                    }   
                                        
                                    //html +="</div>"                   
                                }
                            $("#receivers").html(html);
                            }
                            else{
                            var html="";
                            $("#receivers").html(html);
                            }
	                        $("#msg-title").html(data.title);
	                        $("#msg-sender").html(data.sender);
	                        $("#msg-sender-img").attr("src", rootUri + data.senderimg);
	                        $("#msg-time").html(data.sendtimestr);
	                        $("#msg-contents").html(unescape(data.contents));
	                        $("#msg-senderid").val(data.senderid);

                            var attachinf = "";

                            if (data.attachsize > 1024 * 1024) {
                                attachinf = "(" + data.fileamount + "个文件, " + (data.attachsize / 1024 / 1024).toFixed(1) + "MB)";
                            } else if (data.attachsize > 1024 ) {
                                attachinf = "(" + data.fileamount + "个文件, " + (data.attachsize / 1024).toFixed(1) + "KB)";
                            } else if (data.attachsize > 0 ) {
                                attachinf = "(" + data.fileamount + "个文件, " + attachsize + "B)";
                            } else {
                                $(".message-attachment").addClass("hide");
                            }

	                        $("#attachment-size").html(attachinf);
                            $("#attachsize_forstore").val(data.attachsize);
                            var html="";
                            for(i=0;i<data.fileamount;i++){
                            html += "<li>";
                            html += "<a href='#'' class='attached-file downlink'>";
                            html += "<i class='ace-icon fa fa-file-o bigger-110'></i>";
                            html += "<span class='attached-name' name='filename_indetail'>"+data.filename[i]+"</span>";
                            html += "</a>";
                            html += "<input type='text' name='filehref_indetail' value='"+ data.atachpath[i] +"' style='display:none' >";
                            html += "<span class='action-buttons'>";
                            //html += "<a href='javascript:(0)' onclick='download("+123+","+123+")' class='downlink' >";
                            html += "<a href='"+ rootUri+"Mail/Download?fileName="+ data.filename[i] + "&fileUrl=" + data.atachpath[i] + "' class='downlink' >";
                            html += "<i class='ace-icon fa fa-download bigger-125 blue'></i>";
                            html += "</a>";
                            html += "</span>";
                            html += "</li>";
                            }
                            $("#attachment_list").html(html);

// 	                        $("#attached-name").html(data.attachname);
//                             $(".downlink").attr("href", rootUri + data.attachpath);

	                        //move .message-content next to .message-list and hide .message-list
	                        $('.message-content').removeClass('hide').insertAfter(message_list.addClass('hide'));

	                        //add scrollbars to .message-body
	                        $('.message-content .message-body').ace_scroll({
	                            size: 150,
	                            mouseWheelLock: true,
	                            styleClass: 'scroll-visible'
	                        });
		                }
		            },
		            error: function (data) {
		                alert("Error: " + data.status);
		            }
		        });
	        });

	        //back to message list
	        $('.btn-back-message-list').on('click', function (e) {

	            e.preventDefault();
	            $('#inbox-tabs a[href="#inbox"]').tab('show');
                $(".message-form").addClass("hide");
	        });

	        //hide message list and display new message form
	        /**
	        $('.btn-new-mail').on('click', function(e){
	        e.preventDefault();
	        Inbox.show_form();
	        });
	        */

	        var Inbox = {
	            //displays a toolbar according to the number of selected messages
	            display_bar: function (count) {
	                if (count == 0) {
	                    $('#id-toggle-all').removeAttr('checked');
	                    $('#id-message-list-navbar .message-toolbar').addClass('hide');
	                    $('#id-message-list-navbar .message-infobar').removeClass('hide');
	                }
	                else {
	                    $('#id-message-list-navbar .message-infobar').addClass('hide');
	                    $('#id-message-list-navbar .message-toolbar').removeClass('hide');
	                }
	            }
				,
	            select_all: function () {
	                var count = 0;
	                $('.message-item input[type=checkbox]').each(function () {
	                    this.checked = true;
	                    $(this).closest('.message-item').addClass('selected');
	                    count++;
	                });

	                $('#id-toggle-all').get(0).checked = true;

	                Inbox.display_bar(count);
	            }
				,
	            select_none: function () {
	                $('.message-item input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');
	                $('#id-toggle-all').get(0).checked = false;

	                Inbox.display_bar(0);
	            }
				,
	            select_read: function () {
	                $('.message-unread input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');

	                var count = 0;
	                $('.message-item:not(.message-unread) input[type=checkbox]').each(function () {
	                    this.checked = true;
	                    $(this).closest('.message-item').addClass('selected');
	                    count++;
	                });
	                Inbox.display_bar(count);
	            }
				,
	            select_unread: function () {
	                $('.message-item:not(.message-unread) input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');

	                var count = 0;
	                $('.message-unread input[type=checkbox]').each(function () {
	                    this.checked = true;
	                    $(this).closest('.message-item').addClass('selected');
	                    count++;
	                });

	                Inbox.display_bar(count);
	            }
	        }

	        //show message list (back from writing mail or reading a message)
	        Inbox.show_list = function () {
	            $('.message-container').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-160"></i></div>');
                var getTimestamp=new Date().getTime();               
		        $.ajax({
		            type: "GET",
		            url: rootUri + "Mail/GetReceivedMailList",
		            dataType: "json",
                    data: {
                        kind: currtab,
                        page: currpage,
                        searchkey: searchkey,
                        getTimestamp:getTimestamp

                    },
		            success: function (rst) {
		                if (rst != null) {
                            $('.message-container').find('.message-loading-overlay').remove();
	                        $('.message-navbar').addClass('hide');
	                        $('#id-message-list-navbar').removeClass('hide');

	                        $('.message-footer').addClass('hide');
	                        $('.message-footer:not(.message-footer-style2)').removeClass('hide');

	                        $('.message-list').removeClass('hide').next().addClass('hide');
                            
                            var htmlstr = "";

                            var data = rst.maillist;
                            for(var i = 0; i < data.length; i++)
                            {
								htmlstr += '<div class="message-item ';
                                if (data[i].isread == 0) {
                                    htmlstr += "message-unread";
                                }
                                htmlstr += '">' + 
									'<label class="inline">' +
										'<input type="checkbox" class="ace" data-mailid="' + data[i].uid + '" />' + 
										'<span class="lbl"></span>' + 
									'</label>' +
									'<i class="message-star ace-icon fa fa-star-o light-grey"></i>' + 
									'<span class="sender" title="' + data[i].sender + '">' + data[i].sender + '</span>' + 
									'<span class="time">' + data[i].sendtimestr + '</span>';

                                    if (data[i].attachsize > 0) {
									    htmlstr += '<span class="attachment">' +
										    '<i class="ace-icon fa fa-paperclip"></i>' + 
									    '</span>';
                                    }

									htmlstr += '<span class="summary">' +
										'<span class="text">' + data[i].title + 
										'</span>' + 
									'</span>' + 
								    '</div>';
                            }
                            $("#message-list").html(htmlstr);
                            $("#txtpagenum").val(rst.currpage);
                            $("#currpagenum").html(rst.currpage);

                            var totalpage = Math.ceil(rst.totalcnt / 10);
                            $("#totalpagenum").html(totalpage);

                            if (rst.currpage == totalpage) {
                                $(".pagination li[data-action='last']").addClass("disabled");
                                $(".pagination li[data-action='next']").addClass("disabled");
                                $(".pagination li[data-action='prev']").removeClass("disabled");
                                $(".pagination li[data-action='first']").removeClass("disabled");
                            } else if(rst.currpage == 1) {
                                $(".pagination li[data-action='prev']").addClass("disabled");
                                $(".pagination li[data-action='first']").addClass("disabled");
                                $(".pagination li[data-action='last']").removeClass("disabled");
                                $(".pagination li[data-action='next']").removeClass("disabled");
                            } else {
                                $(".pagination li[data-action='prev']").removeClass("disabled");
                                $(".pagination li[data-action='first']").removeClass("disabled");
                                $(".pagination li[data-action='last']").removeClass("disabled");
                                $(".pagination li[data-action='next']").removeClass("disabled");
                            }
	                        //hide the message item / new message window and go back to list
		                }
		            },
		            error: function (data) {
		                alert("Error: " + data.status);
		            }
		        });
	        }

	        //show write mail form
	        Inbox.show_form = function () {
	            //if ($('.message-form').is(':visible')) return;
	            if (!form_initialized) {
	                initialize_form();
	            }

	            var message = $('.message-list');
	            $('.message-container').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-160"></i></div>');

	            setTimeout(function () {
	                message.next().addClass('hide');

	                $('.message-container').find('.message-loading-overlay').remove();

	                $('.message-list').addClass('hide');
	                $('.message-footer').addClass('hide');
	                $('.message-form').removeClass('hide');

	                $('.message-navbar').addClass('hide');
	                $('#id-message-new-navbar').removeClass('hide');

	                //reset form??
                    $("#attachmentslist").children("div").remove();
                    $("#title").attr("value","");

                    $("#contents").html("");
                    $("#receiver option").removeAttr('selected');
                    $("#receiver").trigger("chosen:updated");

                    if (editor != undefined) {
                        editor.html("");
                        editor.sync();
                    }

	                $('.message-form').get(0).reset();
	                var w = $('.chosen-select').parent().width();
	                $('.chosen-select').next().css({ 'width': w });
                    $("#receiver").trigger("chosen:updated");

	            }, 300 + parseInt(Math.random() * 300));
	        }

	        //intialize kind editor
	        KindEditor.ready(function (K) {
	            editor = KindEditor.create('textarea[name="contents"]', {
                    uploadJson: "<%= ViewData["rootUri"] %>Upload/UploadKindEditorImage",
                    fileManagerJson: "<%= ViewData["rootUri"] %>Upload/ProcessKindEditorRequest",
                    allowFileManager: true,
                    allowUpload: true,
                    resizeType:1,
                    afterChange:function(){
                        if (editor != null)
                        {
                            editor.sync();
                        }
                    }
	            });

	        });


	        $('#attachfile').uploadify({
	            'buttonText': '请选择附件',
	            //'queueSizeLimit': 1,  //设置上传队列中同时允许的上传文件数量，默认为999
	            'multi': false,
	            'uploadLimit': 0,   //设置允许上传的文件数量，默认为999
	            'swf': rootUri + 'Content/plugins/uploadify/uploadify.swf',
                //'auto':false,
	            //'fileTypeExts': '*.flv;*.mp4;*.mpeg;*.avi;',
	            //'fileTypeDesc': 'Video Files (.flv,.mp4,.mpeg,.avi)',
	            'fileSizeLimit': '20MB',

	            'uploader': rootUri + 'Upload/UploadFile',
	            'onUploadComplete': function (file) {   //单个文件上传完成时触发事件
	                //alert('The file ' + file.name + ' finished processing.');
	            },
	            'onQueueComplete': function (queueData) {   //队列中全部文件上传完成时触发事件
	                //alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
	            },
	            'onUploadSuccess': function (file, data, response) {    //单个文件上传成功后触发事件
	                //alert('文件 ' + file.name + ' 已经上传成功，并返回 ' + response + ' 保存文件名称为 ' + data.SaveName + "|" + data.FileSize + "|" + response.SaveName);
//                      $("#filename").val(file.name);
//  	                $("#attachpath").val(data);
//  	                $("#attachfilesize").val(file.size);
//  	                $("#attachfilename").html(file.name);


// 
//                     var html="<label >"+ file.name +"</label>";
//                         html+="<input type=\"hidden\"  name=\"attachpath\" value='"+ data +"'/>";
//                         html+= "<input type=\"hidden\"  name=\"attachfilesize\" value=' "+file.size +"' />";
//                         html+="<input type=\"hidden\"  name='"+file.name+ "' />";
                               


                           var htm  = "<div id=\"attachdiv\" class=\"uploadify-queue-item\">";					
                               htm += "<div class=\"cancel\">";						
                               htm += "<a onclick='removediv(this);return false;' href='javascript:(0)'>X</a>";				
                              // <a href="javascript:$('#attachfile').uploadify('cancel', 'SWFUpload_0_0')">X</a>
                               htm += "</div>";					
                               htm += "<span class=\"fileName\">"+file.name+"</span>";							
                               htm += "<input type=\"hidden\"  name=\"attachpath_singel\" value='"+ data +"'/>";
                               htm += "<input type=\"hidden\"  name=\"attachfilesize_singel\" value=' "+file.size +"' />";
                               htm += "<input type=\"hidden\"  name='filename_singel' value='"+ file.name +"' />";
                               htm += "</div>";

                  //  alert(htm);

                     setTimeout(function(){
                     $("#attachmentslist").css("opacity","0");//这句加在append前
                     $("#attachmentslist").animate({opacity:1},500);//这句的作用是使DIV缓慢显示
                     $("#attachmentslist").append(htm);
                     getfileinfo();
                     },2800);
                    

	            }
	        });




	        var form_initialized = false;
	        function initialize_form() {
	            if (form_initialized) return;
	            form_initialized = true;

	            //file input

	            //Add Attachment
	            //the button to add a new file input
	        } //initialize_form

	        //turn the recipient field into a tag input field!
	        /**	
	        var tag_input = $('#form-field-recipient');
	        try { 
	        tag_input.tag({placeholder:tag_input.attr('placeholder')});
	        } catch(e) {}
			
			
	        //and add form reset functionality
	        $('#id-message-form').on('reset', function(){
	        $('.message-form .message-body').empty();
					
	        $('.message-form .ace-file-input:not(:first-child)').remove();
	        $('.message-form input[type=file]').ace_file_input('reset_input_ui');
			
	        var val = tag_input.data('value');
	        tag_input.parent().find('.tag').remove();
	        $(val.split(',')).each(function(k,v){
	        tag_input.before('<span class="tag">'+v+'<button class="close" type="button">&times;</button></span>');
	        });
	        });
	        */

            $("#searchform").on("submit", function(){
                searchkey = $("#searchkey").val();
                Inbox.show_list();
            });

            $("#txtpagenum").on("change", function(){
                currpage = $("#txtpagenum").val();
                Inbox.show_list();
            });

            $(".pagination").on('click', 'li', function() {
                var disabled = $(this).hasClass("disabled");

                if (!disabled) {
                    var act = $(this).data("action");

                    if (act == "next") {
                        currpage = currpage + 1;
                        Inbox.show_list();
                    } else if (act == "prev") {
                        currpage = currpage - 1;
                        Inbox.show_list();
                    } else if (act == "first") {
                        currpage = 1;
                        Inbox.show_list();
                    } else if (act == "last") {
                        currpage = $("#totalpagenum").text();
                        Inbox.show_list();
                    }
                }
            });

	    });
        

function download(fileName,filePath){
       // alert(fileName);
        $.ajax({
            type: "GET",
            url:  rootUri+"Mail/Download?fileName="+ excape(fileName) + "&fileUrl=" + filePath,
            dataType: "json",         
            success: function (data) {
                          if (data!=""&&data.length!=0) {
                          alert(data);
                          }
                    }
        });
}
function removediv(o){
$(o).parent().parent().remove();
getfileinfo();
}
function getfileinfo() {
        var attachpath_obj = $("input[name^='attachpath_singel']");
        var attachpath_value="";
        attachpath_obj.each(function (i,o){
            attachpath_value += $(o).val()+",";
        });

        var filename_obj = $("input[name^='filename_singel']");
        var filename_value="";
        filename_obj.each(function (i,o){
            filename_value += $(o).val()+",";
        });  

        var attachfilesize_obj = $("input[name^='attachfilesize_singel']");
        var attachfilesize_value=0;
        attachfilesize_obj.each(function (i,o){
            attachfilesize_value +=  parseInt($(o).val());
        });    
      //  alert(attachpath_value);
 	    $("#attachpath").val(attachpath_value);
        if (attachfilesize_value!=0) {
	        $("#attachfilesize").val(attachfilesize_value);
        }
 	    $("#filename").val(filename_value);

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
    <% if (sectorlist != null) {%>
	'sector' : {name: '科室', type: 'folder', 'icon-class':'blue'},
    <% }%>
	'team' : {name: '车队', type: 'folder', 'icon-class':'blue'}
}
<%if (sectorlist != null) { %>
tree_data_2['sector']['additionalParameters'] = {
	'children' : [
<% 
foreach(var item in sectorlist) {
 %>
		{name: '<i data-kind="科室干部" data-id="<%= item.uid %>" class="'+ace_icon+' fa fa-bars blue"></i> <%= item.sectorname %>', type: 'item'},
<% 
} %>
	]}
<% }%>
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

var tree_data_4 = {
<% 
    foreach(var item in teamlist) {
    %>
	'team_<%= item.uid %>' : {name: '<%= item.teamname %>', type: 'folder', 'icon-class':'blue'},
    <%
    }
 %>
}

<% 
    foreach(var item in teamlist) {
    %>
        tree_data_4['team_<%= item.uid %>']['additionalParameters'] = {
	        'children' : [
        <% 
        var subgroups = grouplist.Where(m => m.teamid == item.uid).ToList();
        foreach(var sitem in subgroups) {
         %>
		        {name: '<i data-kind="" data-id="<%= sitem.uid %>" class="'+ace_icon+' fa fa-bars blue"></i> <%= sitem.groupname %>', type: 'item'},
        <% 
        } %>
	        ]
        }
<% } %>
var treeDataSource4 = new DataSourceTree({data: tree_data_4});


		function sendmail() {
            if ($("#receiver").val() == "" || $("#receiver").val() == null) {
                alert("请选择收信人");
                return;
            }
            if ($("#title").val() == "" || $("#title").val() == null) {
                alert("请输入标题");
                return;
            }
            $("#esccontent").attr("value", escape($("#contents").val()));
            $("#btnSend").button('loading');
            setTimeout(function(){
		    $.ajax({
		        type: "POST",
		        url: rootUri + "Mail/SendMail",
		        dataType: "json",
                data: $('#id-message-form').serialize(),
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
		        }
		    });},2500)
		}
        
        function redirectToListPage()
        {
	        $('#inbox-tabs a[href="#inbox"]').tab('show');
            $(".message-form").addClass("hide");
        }

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

        function addAllUser()
        {
			$('#userlist option').each(function () {
                //var selval = $(this).val();
                $("#sellist").append($(this));
			    //$("#receiver option[value=" + selval + "]").attr("selected", "selected");
			});
            //$('#receiver').trigger('chosen:updated');
        }

        function addSelectedUser()
        {
			$('#userlist option:selected').each(function () {
                //var selval = $(this).val();
			    //$("#receiver option[value=" + selval + "]").attr("selected", "selected");
                $("#sellist").append($(this));
			});
            //$('#receiver').trigger('chosen:updated');
        }

        function removeAllUser()
        {
			$('#sellist option').each(function () {
                //var selval = $(this).val();
			    //$("#receiver option[value=" + selval + "]").prop("selected", false);
                $(this).remove();
			});
            
            //$('#receiver').trigger('chosen:updated');
        }

        function removeSelectedUser()
        {
			$('#sellist option:selected').each(function () {
                //var selval = $(this).val();
			    //$("#receiver option[value=" + selval + "]").prop("selected", false);
                $(this).remove();
			});
            //$('#receiver').trigger('chosen:updated');
        }

        function sendreceipt(){
        $.ajax({
		        type: "GET",
		        url: rootUri + "Mail/SendReceipt?mailid="+$("#mailid").val(),
		        dataType: "json",
		        success: function (data) {
		            if (data == "") {
                        $("#receipt").addClass('hide');
                        $("#alreadysent").removeClass('hide');
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
		        }
		    });
        }

        function changemailtitle(){
        var title=$("#attachdiv").children("span").text();
            if (title!=""&&title!=null) {
            $("#title").val(title);
            }
        }
    </script>

</asp:Content>
