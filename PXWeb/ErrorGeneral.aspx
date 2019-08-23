<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorGeneral.aspx.cs" Inherits="PXWeb.ErrorGeneral" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID="litTitle" EnableViewState="false" runat="server" /></title>
    <link href="Resources/Styles/PxWeb.css" rel="stylesheet" type="text/css" media="screen" />  
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrap"> 
            <div id="header" class="header">
                <div class="headerleft">
                        <img id="imgSiteLogo"  enableviewstate="false" runat="server" alt="PX-Web" class="imgSiteLogo" />
                        <span class="siteLogoText">PX-Web</span>
                </div>
                <div class="headerright">
                </div>
                <div style="clear: both;"> </div>
            </div>
            <div id="content"> 
                <div id="place-holder">
                    <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblSavedQueryErrorMessage" runat="server" Text=""></asp:Label>
                    <br /><br />
                    <asp:HyperLink ID="lnkStart" runat="server" NavigateUrl="~/Default.aspx" Text=""></asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
