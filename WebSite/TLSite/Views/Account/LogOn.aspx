<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="TLSite.Models" %>

<% SystemModel sysModel = new SystemModel();
   var configinfo = sysModel.GetSysConfig();
    %>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta charset="utf-8" />
	<title>铁路信息平台</title>

	<meta name="description" content="" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />

	<!-- bootstrap & fontawesome -->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/bootstrap.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/font-awesome.min.css" />

    <!-- ----------------------------------------------------- -->

	<!-- ace styles -->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/style.css" />

	<!--[if lte IE 9]>
		<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-part2.min.css" />
	<![endif]-->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-skins.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-rtl.min.css" />

	<!--[if lte IE 9]>
		<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-ie.min.css" />
	<![endif]-->

	<!-- inline styles related to this page -->
    <link href="<%= ViewData["rootUri"] %>content/plugins/allinone-carousel/allinone_carousel.css" rel="stylesheet" type="text/css">

	<!-- ace settings handler -->
	<script src="<%= ViewData["rootUri"] %>content/js/ace-extra.min.js"></script>

	<!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

	<!--[if lte IE 8]>
	<script src="<%= ViewData["rootUri"] %>content/js/html5shiv.js"></script>
	<script src="<%= ViewData["rootUri"] %>content/js/respond.min.js"></script>
	<![endif]-->

		<!--[if !IE]> -->
		<script src="<%= ViewData["rootUri"] %>content/js/jquery.min.js"></script>
		<!-- <![endif]-->

		<!--[if IE]>
        <script src="<%= ViewData["rootUri"] %>content/js/jquery.minie.js"></script>
        <![endif]-->

		<!--[if !IE]> -->
		<script type="text/javascript">
		    window.jQuery || document.write("<script src='<%= ViewData["rootUri"] %>content/js/jquery.min.js'>" + "<" + "/script>");
		</script>
		<!-- <![endif]-->

		<!--[if IE]>
        <script type="text/javascript">
         window.jQuery || document.write("<script src='<%= ViewData["rootUri"] %>content/js/jquery1x.min.js'>"+"<"+"/script>");
        </script>
        <![endif]-->

        <script src="<%= ViewData["rootUri"] %>content/js/jquery-ui.min.js"></script>

    <script type="text/javascript">
        var rootUri = "<%= ViewData["rootUri"] %>";
    </script>
    <style type="text/css">
        .navbar 
        {
            background: url('<%= ViewData["rootUri"] %>content/img/banner.jpg')
        }
        .ace-nav>li
        {
            /*height:80px;*/
            }
        .ace-nav>li.light-blue>a
        {
            background-color:none;
            }
        .main-content
        {
            margin-left:0px;
            }
        .footer .footer-inner
        {
            left:0px;
            }
        body
        {
            background: url("<%= ViewData["rootUri"] %>content/img/portal_bg.jpg") no-repeat scroll left top transparent;
            background-color: #D2D8DD;
        }
            
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
	        color: #fff;
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
        
        body
        {
            background: url("<%= ViewData["rootUri"] %><%= configinfo.backimg %>") no-repeat scroll left top transparent;
        }
    </style>
</head>

	<body class="login-layout">
		<div id="navbar" class="navbar navbar-default navbar-collapse h-navbar">
			<script type="text/javascript">
			    try { ace.settings.check('navbar', 'fixed') } catch (e) { }
			</script>

			<div class="navbar-container" id="navbar-container">
				<div class="navbar-header pull-left">
				</div>
                <p style="font-size:18px; color:#ddd; font-weight:bold;text-align:right; top:20px; right:10px; position:absolute;">
                <%= String.Format("{0:yyyy年MM月dd日}", DateTime.Now) %>, 欢迎您！
                </p>
			</div><!-- /.navbar-container -->
		</div>

		<div class="main-container" id="main-container">
			<script type="text/javascript">
			    try { ace.settings.check('main-container', 'fixed') } catch (e) { }
			</script>

			<div class="main-content">
				<div class="page-content" style="height:100%;">
                    <div class="row">
                        <div class="col-xs-8">
                            <div style="padding: 40px 0px 0 70px;height: 100%;" class="featurenav">
                                <li class="block" id="block_1" title="干部考核" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/diary.png"></p><div class="count" id="count_1"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>干部考核</span></a>
                                </li>
                                <li class="block" id="Li1" title="公文流转" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/workflow.png"></p><div class="count" id="Div1"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>公文流转</span></a>
                                </li>
                                <li class="block" id="Li2" title="任务管理" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/project.png"></p><div class="count" id="Div2"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>任务管理</span></a>
                                </li>
                                <li class="block" id="Li3" title="规章查询" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/guide.png"></p><div class="count" id="Div3"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>规章查询</span></a>
                                </li>
                                <li class="block" id="Li4" title="在线考试" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/test.png"></p><div class="count" id="Div4"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>在线考试</span></a>
                                </li>
                                <li class="block" id="Li5" title="职工诉求" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/sms.png"></p><div class="count" id="Div5"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>职工诉求</span></a>
                                </li>
                                <li class="block" id="Li6" title="通讯录" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/address.png"></p><div class="count" id="Div6"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>通讯录</span></a>
                                </li>
                                <li class="block" id="Li7" title="邮箱查询" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/email.png"></p><div class="count" id="Div7"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>邮箱查询</span></a>
                                </li>
                                <li class="block" id="Li8" title="考核查询" index="1">
                                    <div class="img"><p><img style="height:120px;" src="<%= ViewData["rootUri"] %>content/img/diary.png"></p><div class="count" id="Div8"></div></div>
                                    <a class="icon-text" href="javascript: void(0)"><span>考核查询</span></a>
                                </li>
                            </div>
                        </div>

                        <div class="col-xs-4">
						    <div class="login-container">
							    <div class="position-relative">
								    <div id="login-box" class="login-box visible widget-box no-border">
									    <div class="widget-body">
										    <div class="widget-main">
											    <form id="formlogon" autocomplete="off" method="post" action="<%= ViewData["rootUri"] %>Account/LogOn">
												    <fieldset>
													    <label class="block clearfix">
														    <span class="block input-icon input-icon-right">
															    <input type="text" class="form-control" placeholder="请输入账号" id="username" name="username" />
															    <i class="ace-icon fa fa-user"></i>
														    </span>
													    </label>

													    <label class="block clearfix">
														    <span class="block input-icon input-icon-right">
															    <input type="password" class="form-control" placeholder="请输入密码" id="userpwd" name="userpwd" />
															    <i class="ace-icon fa fa-lock"></i>
														    </span>
													    </label>

													    <div class="space"></div>

													    <div class="clearfix">
														    <label class="inline">
															    <input type="checkbox" class="ace" />
															    <span class="lbl"> 记住账号</span>
														    </label>

														    <button type="submit" class="width-35 pull-right btn btn-sm btn-primary">
															    <i class="ace-icon fa fa-key"></i>
															    <span class="bigger-110">登&nbsp;&nbsp;录</span>
														    </button>
													    </div>

													    <div class="space-4"></div>
												    </fieldset>
											    </form>

                                                <% if (!ViewData.ModelState.IsValid) { %>
			                                    <div class="alert alert-danger aler-dismissable">
				                                    <button class="close" data-dismiss="alert"></button>
				                                    <span><%= Html.ValidationMessage("modelerror")%></span>
			                                    </div>
                                                <% } %>
                                                <!--
                                                <div class="social-or-login center">
												    <span class="bigger-110">或者使用手机端扫一扫快速登录</span>
											    </div>
                                                <div class="space-6"></div>
                                                <div class="social-login center">
												    <img src="<%= ViewData["rootUri"] %>content/img/erwei_big.jpg" style="max-height:200px;" />
											    </div>
                                                -->
										    </div><!-- /.widget-main -->
									    </div><!-- /.widget-body -->
								    </div><!-- /.login-box -->

							    </div><!-- /.position-relative -->
						    </div>

                            <div>
                                <div style="text-align:center;">
                                    <h1>公告信息</h1>
                                </div>
                                <p class="lead" style="text-indent: 2em;" ><% if (configinfo != null)
                                                                              { %><%= configinfo.notice %><% } %>
								</p>
                            </div>
                            <div>

                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <div style="width:100%; margin: 0 auto; text-align:center; ">
                                <div id="allinone_carousel_powerful" style="width:100%; margin: 0 auto; text-align:center;">
	                                <div class="myloader"></div>
	                                <!-- CONTENT -->
	                                <ul class="allinone_carousel_list">
                                    <% var slidelist = sysModel.GetSlideList();
                                       foreach (var item in slidelist)
                                       {
                                        %>
		                                <li data-title="<%= item.title %>"><img src="<%= ViewData["rootUri"] %><%= item.imgpath %>" alt="" /></li>
                                        <% } %>
	                                </ul>
                                </div>
                            </div>
                        </div>
                    </div>
				</div><!-- /.page-content -->
			</div><!-- /.main-content -->

			<div class="footer">
				<div class="footer-inner">
					<div class="footer-content">
						<span class="bigger-120">
							<span class="blue bolder">德铭源科技有限公司</span>
							&copy; 2013-2014
						</span>

						&nbsp; &nbsp;
					</div>
				</div>
			</div>

			<a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
				<i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
			</a>
		</div><!-- /.main-container -->

		<!-- basic scripts -->

		<script type="text/javascript">
		    if ('ontouchstart' in document.documentElement) document.write("<script src='<%= ViewData["rootUri"] %>content/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
		</script>
		<script src="<%= ViewData["rootUri"] %>content/js/bootstrap.min.js"></script>

        <script src="<%= ViewData["rootUri"] %>content/plugins/allinone-carousel/js/allinone_carousel.js" type="text/javascript"></script>
        <!--[if IE]><script src="<%= ViewData["rootUri"] %>content/plugins/allinone-carousel/js/excanvas.compiled.js" type="text/javascript"></script><![endif]-->

		<!-- ace scripts -->
		<script src="<%= ViewData["rootUri"] %>content/js/ace-elements.min.js"></script>
		<script src="<%= ViewData["rootUri"] %>content/js/ace.min.js"></script>

		<!-- inline scripts related to this page -->
	<script>
	    jQuery(function () {

	        jQuery('#allinone_carousel_powerful').allinone_carousel({
	            skin: 'charming',
	            width: 980,
	            height: 500,
	            autoPlay: 1,
	            resizeImages: true,
	            autoHideBottomNav: false,
	            showElementTitle: false,
	            numberOfVisibleItems: 5,
	            //elementsHorizontalSpacing: 110,
	            //elementsVerticalSpacing: 25,
	            verticalAdjustment: 130,
	            animationTime: 0.6,
	            showPreviewThumbs: false,
	            showCircleTimer: false,
	            //nextPrevMarginTop: -33,
	            playMovieMarginTop: 0
	            //bottomNavMarginBottom: -10
	        });


	    });
    </script>

	</body>
</html>