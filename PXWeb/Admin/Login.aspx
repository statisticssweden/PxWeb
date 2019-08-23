<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PXWeb.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PX-Web Login</title>
    <link href="../Resources/Styles/PxWeb.css" rel="stylesheet" type="text/css" media="screen" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div class="login_container">
        <div class="login_caption">
            <asp:Label ID="lblLogin" runat="server" Text="<%$ PxString: PxWebPxLogin %>"></asp:Label>
        </div>
        <div class="login_left">
            <asp:Label ID="lblUsername" runat="server" CssClass="login_label" Text="<%$ PxString: PxWebUsername %>"></asp:Label><br />
            <asp:Label ID="lblPwd" runat="server" CssClass="login_label" Text="<%$ PxString: PxWebPassword %>"></asp:Label>
        </div>
        <div class="login_right">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="login_textbox"></asp:TextBox><br />
            <asp:TextBox ID="txtPwd" runat="server" CssClass="login_textbox" TextMode="Password"></asp:TextBox>
        </div>
        <div class="login_error">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
        <div class="login_buttoncontainer">
            <asp:Button ID="btnLogin" runat="server" CssClass="login_button" 
                Text="<%$ PxString: PxWebLogin %>" onclick="btnLogin_Click" />
        </div>
    </div>
    </form>

<script type="text/javascript" language="JavaScript">
<!--

document.form1.txtUsername.focus();

//-->
</script>


</body>
</html>
