<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TLSite.Models" %>

<% var userrole = CommonModel.GetUserRoleInfo(); %>

<script type="text/javascript">
    try { ace.settings.check('sidebar', 'fixed') } catch (e) { }
</script>

<!--
<div class="sidebar-shortcuts" id="sidebar-shortcuts">
	<div class="sidebar-shortcuts-large" id="sidebar-shortcuts-large">
		<button class="btn btn-success">
			<i class="ace-icon fa fa-signal"></i>
		</button>

		<button class="btn btn-info">
			<i class="ace-icon fa fa-pencil"></i>
		</button>

		<button class="btn btn-warning">
			<i class="ace-icon fa fa-users"></i>
		</button>

		<button class="btn btn-danger">
			<i class="ace-icon fa fa-cogs"></i>
		</button>
	</div>

	<div class="sidebar-shortcuts-mini" id="sidebar-shortcuts-mini">
		<span class="btn btn-success"></span>

		<span class="btn btn-info"></span>

		<span class="btn btn-warning"></span>

		<span class="btn btn-danger"></span>
	</div>
</div>
-->

<ul class="nav nav-list">
	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Home") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Home/Index">
			<i class="menu-icon fa fa-institution"></i>
			<span class="menu-text"> 首页 </span>
		</a>

		<b class="arrow"></b>
	</li>

    <% 
        if (userrole != null && (((string)userrole).Contains("Judge")) || ((string)userrole).Contains("Executive") || ((string)userrole).Contains("Coach"))
        {
    %>
	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Judge") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa  fa-pencil-square-o"></i>
			<span class="menu-text"> 绩效考核 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>
		<b class="arrow"></b>
		<ul class="submenu">
            <% if (userrole != null && (((string)userrole).Contains("Judge") || ((string)userrole).Contains("Executive")) || ((string)userrole).Contains("Coach"))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AddJudge") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Judge/AddJudge">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/document.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">“两违”考核录入</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>

            <% if (userrole != null && (((string)userrole).Contains("Judge") || ((string)userrole).Contains("Executive")) || ((string)userrole).Contains("Coach"))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AddCombineJudge") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Judge/AddCombineJudge">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/document.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">结合部考核录入</p>
				</a>
				<b class="arrow"></b>
			</li>
            <% } %>
            <% if (userrole != null && ((string)userrole).Contains("Judge"))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AddScore") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Judge/AddScore">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/document.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">激励积分录入</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
            <% if (userrole != null && (((string)userrole).Contains("Judge") || ((string)userrole).Contains("Executive")))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "JudgeAnalysis") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Judge/JudgeAnalysis">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/graph.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">考核分析</p>
				</a>
				<b class="arrow"></b>
			</li>
            <% } %>
            <% if (userrole != null && (((string)userrole).Contains("Judge")))
               { %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "EditJudge") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Judge/EditJudge">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/attendance1.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">修改考核信息</p>
				</a>
				<b class="arrow"></b>
			</li>
            <%} %>
		</ul>
	</li>
    <%} %>
    
        <!-- 考核查询 -->
    	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "CheckInfo") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa  fa-pencil-square-o"></i>
			<span class="menu-text"> 考核查询 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>
		<b class="arrow"></b>
		<ul class="submenu">
            
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "CheckCheckinfo") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>CheckInfo/CheckCheckinfo">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;"/>
					<p style="width:100%; text-align:center; margin:0 auto;">“两违”考核问题</p>
				</a>

				<b class="arrow"></b>
			</li>
           
           	<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "CombinCheck") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>CheckInfo/CombinCheck">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;"/>
					<p style="width:100%; text-align:center; margin:0 auto;">结合部问题</p>
				</a>

				<b class="arrow"></b>
			</li>
		</ul>
	</li>

     <!-- 新增积分 -->
      <% if (userrole != null && (((string)userrole).Contains("Credit") || ((string)userrole).Contains("Executive") || ((string)userrole).Contains("Coach")))
               { %>
    	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Credit") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa  fa-pencil-square-o"></i>
			<span class="menu-text"> 积分查询 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>
		<b class="arrow"></b>
		<ul class="submenu">
            
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "CheckCredit") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Credit/CheckCredit">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;"/>
					<p style="width:100%; text-align:center; margin:0 auto;">查询积分</p>
				</a>

				<b class="arrow"></b>
			</li>
           
           	<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SelfCheck") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Credit/SelfCheck">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;"/>
					<p style="width:100%; text-align:center; margin:0 auto;">自控率查询</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% if (userrole != null && (((string)userrole).Contains("Credit") || ((string)userrole).Contains("Executive")))
               { %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "checkcreditofparty") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Creditofparty/checkcreditofparty">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;"/>
					<p style="width:100%; text-align:center; margin:0 auto;">党内两违</br>比率查询</p>
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
		</ul>
	</li>
            <%} %>

     <% 
         if (userrole != null && (((string)userrole).Contains("Sector") ||
             ((string)userrole).Contains("RailTeam")  ||
             ((string)userrole).Contains("Route") ||
             ((string)userrole).Contains("Group") ||
             ((string)userrole).Contains("TrainNo") ||
             ((string)userrole).Contains("CheckInfo")))
        {
        %>
	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Base") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-bars"></i>
			<span class="menu-text"> 基础数据 </span>

			<b class="arrow fa fa-angle-down"></b>
		</a>

		<b class="arrow"></b>

		<ul class="submenu">
              <% 
            if (userrole != null && ((string)userrole).Contains("Sector"))
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "Sector") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>sector/sectorlist">
					<i class="menu-icon fa fa-caret-right"></i>
					科室管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (((string)userrole).Contains("RailTeam") )
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "Team") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>team/teamlist">
					<i class="menu-icon fa fa-caret-right"></i>
					车队管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
                        <% 
            if (userrole != null && ((string)userrole).Contains("Route")) 
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "Route") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>route/routelist">
					<i class="menu-icon fa fa-caret-right"></i>
					线路管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("Group")) 
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "Group") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Group/Grouplist">
					<i class="menu-icon fa fa-caret-right"></i>
					班组管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("TrainNo")) 
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "TrainNo") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>TrainNo/TrainNoList">
					<i class="menu-icon fa fa-caret-right"></i>
					车次管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("CheckInfo")) 
            {
             %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "CheckInfo") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Check/CheckInfoList">
					<i class="menu-icon fa fa-caret-right"></i>
					项点管理
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
		</ul>
	</li>
    <% } %>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Document") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-external-link"></i>
			<span class="menu-text"> 公文流转 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>

		<b class="arrow"></b>

		<ul class="submenu">
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "MyDocList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Document/MyDocList?kind=<%= (int)DocStatus.WAITSIGN %>">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/attendance1.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">公文待签</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% if (userrole != null && (((string)userrole).Contains("Document") || ((string)userrole).Contains("Executive")))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AddDocument") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Document/AddDocument">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/forward.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">公文发布</p>
				</a>

				<b class="arrow"></b>
			</li>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SentDocList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Document/SentList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/forward.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">已发公文</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "MyRecvDocList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Document/MyDocList?kind=<%= (int)DocStatus.ALREADYSIGN %>">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/hr.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">已收公文</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% if (userrole != null && (((string)userrole).Contains("Document") || ((string)userrole).Contains("Executive")))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "DocSearch") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Document/DocSearch">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">公文查询</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
		</ul>
	</li>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Task") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-tasks"></i>
			<span class="menu-text"> 任务管理 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>

		<b class="arrow"></b>

		<ul class="submenu">
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "MyTaskList" && ViewData["kind"].ToString() == "0") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Task/MyTaskList?kind=0">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">待接收</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% if (userrole != null && (((string)userrole).Contains("Mission") || ((string)userrole).Contains("Executive")))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AddTask") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Task/AddTask">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/hr.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">发布任务</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "TaskList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Task/TaskList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">已发的任务</p>
				</a>

				<b class="arrow"></b>
			</li>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "MyTaskList" && ViewData["kind"].ToString() == "1") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Task/MyTaskList?kind=1">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">已接收</p>
				</a>

				<b class="arrow"></b>
			</li>
		</ul>
	</li>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Rule") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Rule/Browse">
			<i class="menu-icon fa fa-book"></i>
			<span class="menu-text"> 规章查询 </span>
		</a>
	</li>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Exam") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-check-square-o"></i>
			<span class="menu-text"> 在线考试 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>

		<b class="arrow"></b>

		<ul class="submenu">
            <% if (userrole != null && ((string)userrole).Contains("OnlineTest") )
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/ExamList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/menubook.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">题库管理</p>
				</a>

				<b class="arrow"></b>
			</li>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamBookList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/ExamBookList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/menubook.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">试卷管理</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>

            <% if (userrole != null && ((string)userrole).Contains("TeamManager"))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/TeamExamList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/menubook.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">题库管理(车队)</p>
				</a>

				<b class="arrow"></b>
			</li>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamBookList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/TeamExamBookList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/menubook.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">试卷管理(车队)</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SectorExam") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/SectorExam">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/exam_manage.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">段级考试</p>
				</a>
				<b class="arrow"></b>
			</li>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "TeamExam") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/TeamExam">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/exam_manage.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">车队考试</p>
				</a>

				<b class="arrow"></b>
			</li>

            <% if (userrole != null && ((string)userrole).Contains("OnlineTest") )
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamResult") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/ResultList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">考试历史记录</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
            <% if (userrole != null && ((string)userrole).Contains("TeamManager"))
               { %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamResult") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/TeamResultList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/work_plan.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">车队考试记录</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
            <% if (userrole != null && (((string)userrole).Contains("OnlineTest")) || userrole != null && (((string)userrole).Contains("TeamManager")))
               { %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExamStatistic") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>Exam/ExamStatistic">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/chart-icon.gif" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">考试分析</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
		</ul>
	</li>


	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Opinion") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Opinion/AddOpinion">
			<i class="menu-icon fa fa-comment-o"></i>
			<span class="menu-text"> 职工诉求 </span>
		</a>
	</li>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Contact") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Contact/List">
			<i class="menu-icon fa fa-file-o"></i>
			<span class="menu-text"> 通讯录 </span>
		</a>
		<b class="arrow"></b>
	</li>
    <% if (userrole != null && (((string)userrole).Contains("Duty") || ((string)userrole).Contains("Executive") || ((string)userrole).Contains("TeamAdmin")))
       { %>
	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Duty") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Duty/DutyList">
			<i class="menu-icon fa fa-history"></i>
			<span class="menu-text"> 出乘计划 </span>
		</a>

		<b class="arrow"></b>
	</li>
    <% } %>

    <% 
        if ((userrole != null && ((string)userrole).Contains("UserManage")) || (userrole != null && ((string)userrole).Contains("TeamAdmin")) || (userrole != null && ((string)userrole).Contains("AdminList")) || (userrole != null && ((string)userrole).Contains("RoleManage")))
        {
        %>
	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "User") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-user"></i>
			<span class="menu-text"> 用户管理 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>

		<b class="arrow"></b>

		<ul class="submenu">
			<%--<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ExecutiveList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>User/ExecutiveList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">干部列表</p>
				</a>

				<b class="arrow"></b>
			</li>--%>
            <% 
            if (userrole != null && ((string)userrole).Contains("UserManage"))
            {
            %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "PersonnelList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>User/PersonnelList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">人员库</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% }
            if (userrole != null && ((string)userrole).Contains("TeamAdmin"))
            {
            %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "TeamCrew") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>TeamAdmin/TeamCrewList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">车队人员</p>
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("TeamAdmin"))
            {
            %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "TeamCrewTransfer") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>TeamAdmin/TeamCrewTransfer">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">调入调出</p>
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("TeamAdmin"))
            {
            %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SPCrewTransfer") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>TeamAdmin/SPCrewTransfer">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">其他人员变动</p>
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
             <% 
            if (userrole != null && ((string)userrole).Contains("UserManage"))
            {
            %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "ManageSPCrewTransfer") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>TeamAdmin/ManageSPTransfer">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/guide.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">管理其他人员变动</p>
				</a>

				<b class="arrow"></b>
			</li>
            <%} %>
            <% 
            if (userrole != null && ((string)userrole).Contains("AdminList"))
            {
            %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "AdminList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>User/AdminList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/meeting.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">管理员列表</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
            <% 
            if (userrole != null && ((string)userrole).Contains("RoleManage"))
            {
            %>
			<li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "RoleList") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>User/RoleList">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/hr.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">角色管理</p>
				</a>

				<b class="arrow"></b>
			</li>
            <% } %>
		</ul>
	</li>
    <% } %>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "System") { %>active open hsub <% } %>">
		<a href="#" class="dropdown-toggle">
			<i class="menu-icon fa fa-gears"></i>
			<span class="menu-text"> 系统设置 </span>
			<b class="arrow fa fa-angle-down"></b>
		</a>
		<b class="arrow"></b>

		<ul class="submenu">
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "Profile") { %>active <% } %>">
				<a href="<%= ViewData["rootUri"] %>System/Profile">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/hr.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">个人信息</p>
				</a>
				<b class="arrow"></b>
			</li>

    <% 
        if (userrole != null && ((string)userrole).Contains("SysSetting"))
        {
        %>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SysConfig") { %>active <% } %>" >
				<a href="<%= ViewData["rootUri"] %>System/SysConfig">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/setting.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">系统设置</p>
				</a>
				<b class="arrow"></b>
			</li>
            <li class="highlight hover <% if (ViewData["level2nav"] != null && ViewData["level2nav"] == "SlideImg") { %>active <% } %>" >
				<a href="<%= ViewData["rootUri"] %>System/SlideImg">
                    <img class=" img-responsive" src="<%= ViewData["rootUri"] %>content/img/menubook.png" alt="" style="height:50px; margin:0 auto;">
					<p style="width:100%; text-align:center; margin:0 auto;">首页滚动图片</p>
				</a>
				<b class="arrow"></b>
			</li>
            <% } %>
		</ul>
	</li>

	<li class="highlight hover <% if (ViewData["level1nav"] != null && ViewData["level1nav"] == "Mail") { %>active <% } %>">
		<a href="<%= ViewData["rootUri"] %>Mail/Inbox">
			<i class="menu-icon fa fa-envelope"></i>
			<span class="menu-text"> 电子邮箱 </span>
		</a>
	</li>
</ul>

<div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
	<i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
</div>