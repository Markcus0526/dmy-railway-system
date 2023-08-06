<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html lang="en">
<head>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta charset="utf-8" />
	<title>铁路信息平台</title>
    
    <link rel="shortcut icon" href="<%= ViewData["rootUri"] %>content/favicon.ico" />

	<meta name="description" content="User login page" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />

	<!-- bootstrap & fontawesome -->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/bootstrap.min.css" />
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/font-awesome.min.css" />

	<!-- ace styles -->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace.min.css" />

	<!--[if lte IE 9]>
		<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-part2.min.css" />
	<![endif]-->
	<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-rtl.min.css" />

	<!--[if lte IE 9]>
		<link rel="stylesheet" href="<%= ViewData["rootUri"] %>content/css/ace-ie.min.css" />
	<![endif]-->

	<!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

	<!--[if lt IE 9]>
	<script src="<%= ViewData["rootUri"] %>content/js/html5shiv.js"></script>
	<script src="<%= ViewData["rootUri"] %>content/js/respond.min.js"></script>
	<![endif]-->
</head>

	<body class="login-layout blur-login">
		<div class="main-container">
			<div class="main-content">
				<div class="row">
					<div class="col-sm-10 col-sm-offset-1">
						<div class="login-container">
							<div class="center">
                                <!--<img src="<%= ViewData["rootUri"] %>content/img/logosmall.png" style="max-width:100%" />-->
                                <h1>
									<span class="red">锦客客运端</span>
									<span class="white" id="id-text2">信息平台</span>
								</h1>
							</div>

							<div class="space-6"></div>

							<div class="position-relative">
								<div id="login-box" class="login-box visible widget-box no-border">
									<div class="widget-body">
										<div class="widget-main">

											<h4 class="header blue lighter bigger">
												<i class="ace-icon fa fa-smile-o green"></i>
												欢迎来铁路信息平台!
											</h4>

											<div class="space-6"></div>

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
                                            <div class="social-or-login center">
												<span class="bigger-110">或者使用手机端扫一扫快速登录</span>
											</div>
                                            <div class="space-6"></div>
                                            <div class="social-login center">
												<img src="<%= ViewData["rootUri"] %>content/img/erwei_big.jpg" style="max-height:200px;" />
											</div>
										</div><!-- /.widget-main -->
                                        <div class="toolbar clearfix">
											<div style="width:100%; text-align:center;">
												<a href="#" class="user-signup-link" style="margin: 0 auto; text-align:center; float:none">
													2013@ XXXXXXXXX公司
												</a>
											</div>
										</div>
									</div><!-- /.widget-body -->
								</div><!-- /.login-box -->

							</div><!-- /.position-relative -->
						</div>
					</div><!-- /.col -->
				</div><!-- /.row -->
			</div><!-- /.main-content -->
		</div><!-- /.main-container -->

		<!-- basic scripts -->

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
		<script type="text/javascript">
		    if ('ontouchstart' in document.documentElement) document.write("<script src='<%= ViewData["rootUri"] %>content/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
		</script>

		<!-- inline scripts related to this page -->
		<script type="text/javascript">
		    jQuery(function ($) {
		    });
		</script>
	</body>

</html>

