<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PXWeb.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<!--[if lt IE 7 ]> <html xmlns="http://www.w3.org/1999/xhtml" class="ie6"> <![endif]-->
<!--[if IE 7 ]> <html xmlns="http://www.w3.org/1999/xhtml" class="ie7"> <![endif]-->
<!--[if IE 8 ]> <html xmlns="http://www.w3.org/1999/xhtml" class="ie8"> <![endif]-->
<!--[if IE 9 ]> <html xmlns="http://www.w3.org/1999/xhtml" class="ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!-->
<html xmlns="http://www.w3.org/1999/xhtml">
<!--<![endif]-->
<head id="Head1" runat="server">
	<title>
		<asp:Literal ID="litTitle" EnableViewState="false" runat="server" /></title>
<%--	<link href="Resources/Styles/reset.css" rel="stylesheet" type="text/css" media="screen" />
	<link href="Resources/Styles/PxWeb.css" rel="stylesheet" type="text/css" media="screen" />
	<link href="Resources/Styles/Custom.css" rel="stylesheet" type="text/css" media="screen" />--%>
	<link href="<%= ResolveUrl("~/Resources/Styles/reset.css")  %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%= ResolveUrl("~/Resources/Styles/main-common.css") %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%= ResolveUrl("~/Resources/Styles/main-pxweb.css") %>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%= ResolveUrl("~/Resources/Styles/main-custom.css") %>" rel="stylesheet" type="text/css" media="screen" />

</head>

<body>
	<div id="pxwebcontent" role="main">
	<form id="form1" runat="server">
		<div id="wrap">
			<div id="header" class="header">
				<div class="headerleft">
					<img id="imgSiteLogo" enableviewstate="false" runat="server" src="" alt="" class="imgSiteLogo" />
					<span class="siteLogoText">
						<asp:Literal ID="litAppName" EnableViewState="false" runat="server" /></span>
				</div>
				<div class="headerright">
				</div>
				<div style="clear: both;"></div>
			</div>

			<div id="content">
				<div id="place-holder">
					<asp:Login runat="server" ID="LoginControl" OnAuthenticate="LogIn" RenderOuterTable="false">
						<LayoutTemplate>
							<fieldset class="login-form">
								<asp:Label ID="FailureText" runat="server" CssClass="login-form-failure"></asp:Label>
								<div>
									<asp:Label AssociatedControlID="UserName" runat="server" Text="<%$ PxString: PxWebAdminUsersUsername %>" />
									<asp:TextBox ID="UserName" runat="server"></asp:TextBox>
								</div>
								<div>
									<asp:Label AssociatedControlID="Password" runat="server" Text="<%$ PxString: PxWebAdminUsersPassword %>" />
									<asp:TextBox ID="Password" runat="server" TextMode="Password" />
								</div>
								<div class="login-form-buttons">
									<asp:Button ID="Login" runat="server" Text="<%$ PxString: PxWebAdminUsersLogin %>" CommandName="Login" />
								</div>
							</fieldset>
						</LayoutTemplate>
						
					</asp:Login>
				</div>
			</div>
		</div>
	</form>
		</div>
</body>
