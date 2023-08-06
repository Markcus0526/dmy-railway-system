<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% var userinfo = (tbl_user)ViewData["userinfo"]; %>
<% var userrole = CommonModel.GetUserRoleInfo(); %>
<style>
        .featurenav .block {
            float: left;
            display: block;
            height: 162px;
            text-align: center;
            overflow: hidden;
            width: 160px;
            margin-left: 30px;
            margin-right: 30px;
        }   

        .icon-text {
            cursor: pointer;
            margin-top: 3px;
            padding-left: 10px;
            height: 20px;
            display: inline-block;
            background: url('<%= ViewData["rootUri"] %>content/img/icon_text_l.png') 0px 0px no-repeat;
        }
        
        .icon-text span{
	        display: inline-block;
	        height: 20px;
	        line-height: 20px;
	        background: url('<%= ViewData["rootUri"] %>content/img/icon_text_r.png') right center no-repeat;
	        color: #111;
	        padding: 0 10px 0 0px;
        }        
        
        li.block .img {
            height: 120px;
            width: 120px;
            line-height: 120px;
            cursor: pointer;
            position: relative;
            margin: 0px auto;
            text-align: center;
        }
        li.block .img p {
            position: static;
            top: 50%;
        }        
        li.block .img p img {
            position: static;
            top: -50%;
            left: -50%;
            vertical-align: middle;
        }        
    
</style>
<div class="row">
	<div class="col-xs-12 col-sm-9 center">
        <div class="row">
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Judge/AddJudge" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/diary.png" alt="" style="height:100px;" >
						</div>
						<h2>干部考核</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Document/MyDocList" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/workflow.png" alt="" style="height:100px;" >
						</div>
						<h2>公文流转</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Task/TaskList" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/project.png" alt="" style="height:100px;" >
						</div>
						<h2>任务管理</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Rule/Browse" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:100px;" >
						</div>
						<h2>规章查询</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Exam/SectorExam" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/test.png" alt="" style="height:100px;" >
						</div>
						<h2>在线考试</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Opinion/AddOpinion" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/sms.png" alt="" style="height:100px;" >
						</div>
						<h2>职工诉求</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Contact/List" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/address.png" alt="" style="height:100px;" >
						</div>
						<h2>通讯录</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>Mail/Inbox" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/email.png" alt="" style="height:100px;" >
						</div>
						<h2>邮箱查询</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon">
					<a href="<%= ViewData["rootUri"] %>CheckInfo/CheckCheckinfo" >
						<div class="imgBorder">
							<img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/diary.png" alt="" style="height:100px;" >
						</div>
						<h2>考核查询</h2>
					</a>
				</article>
            </div>
            <div class="col-xs-12 col-sm-4 center">
                <article class="boxIcon " >
					<a href="javascript:(0);" onclick="return false;" for="qrcode" >
						<div class="imgBorder">
							<img class=" img-responsive qrcode"  data-trigger="hover" id="qrcode" src="<%=ViewData["rootUri"]%>content/img/qrcode.png" 
                            style='width:100px;height:100px' alt="" date-style="height:150px;" data-html="true" 
                            data-content="<img src='<%=ViewData["rootUri"]%>content/img/qrcode.png'style='width:150px;height:150px;'/> <div class='text-center'>注：不支持微信内的</br>二维码扫描</div>"
                            data-original-title="预览二维码"
                            />
						</div>
						<h2>二维码下载</br>
                            手机客户端</h2>
					</a>
				</article>
            </div>
        </div>

	</div><!-- /.col -->

	<div class="col-xs-12 col-sm-3 center"><span class="profile-picture" style="margin-top:20px;">
			<img class="editable img-responsive" alt="<%= userinfo.realname %>" id="avatar2" 
            src="<%= ViewData["rootUri"] %><% if (String.IsNullOrEmpty(userinfo.imgurl)) { %>content/img/profile-pic.jpg<% } else { %><%= userinfo.imgurl %><% } %>"/>
		</span>
		<h4 class="blue">
			<span class="middle"><%= userinfo.realname %></span>
		</h4>

		<div class="profile-user-info">
			<div class="profile-info-row">
				<div class="profile-info-name"> 用户名： </div>

				<div class="profile-info-value">
					<span><%= userinfo.username %></span>
				</div>
			</div>

			<div class="profile-info-row">
				<div class="profile-info-name"> 出生日期：</div>

				<div class="profile-info-value">
					<span><%= String.Format("{0:yyyy-MM-dd}", userinfo.birthday) %></span>
				</div>
			</div>

			<div class="profile-info-row">
				<div class="profile-info-name"> 性别： </div>

				<div class="profile-info-value">
					<span><% if (userinfo.gender == 0) { %>男<% } else { %>女<% } %></span>
				</div>
			</div>

			<div class="profile-info-row">
				<div class="profile-info-name"> 邮箱地址： </div>

				<div class="profile-info-value">
					<span><%= userinfo.mailaddr %></span>
				</div>
			</div>

			<div class="profile-info-row">
				<div class="profile-info-name"> 手机号： </div>

				<div class="profile-info-value">
					<span><%= userinfo.phonenum %></span>
				</div>
			</div>
			<div class="profile-info-row">
				<div class="profile-info-name"> QQ号： </div>

				<div class="profile-info-value">
					<span><%= userinfo.qqnum %></span>
				</div>
			</div>

    <% 
        if (userrole != null && ((string)userrole).Contains("Crew"))
        {
        %>
			<div class="profile-info-row">
				<div class="profile-info-name"> 考核积分： </div>

				<div class="profile-info-value">
					<span>27</span>
				</div>
			</div>
		</div>
        <% } %>

		<div class="hr hr-8 dotted"></div>
	</div><!-- /.col -->
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
	 <script src="<%= ViewData["rootUri"] %>Content/js/tooltip.js"></script>
     <script src="<%= ViewData["rootUri"] %>Content/js/bpopover.js"></script>

<script type="text/javascript">
    jQuery(function ($) {
    var checknotify = "<%=ViewData["notifytask"]%>"; 
    if (checknotify=="True") {
    alert("您今天有需要完成的任务，请及时完成！")
    }
      $(".qrcode").popover();
    })

</script>
</asp:Content>
