<%@ Page Title="" Language="C#" MasterPageFile="~/PxWeb.Master" MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="Presentation.aspx.cs" Inherits="PXWeb.Presentation" %>
<%@ MasterType VirtualPath="~/PxWeb.Master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <pxc:commandbar id="CommandBar1" runat="server"  />   
    <asp:Label ID="lblTableCropped" runat="server" CssClass="tableCropped"></asp:Label>
    <pxc:tableinformation ID="TableInformationView" runat="server" Type="TableView" /><br />
    <pxc:table id="Table1" runat="server" >    
    </pxc:table>       
    <pxc:footnote id="Footnotes" runat="server" >  
    </pxc:footnote>
    <br /><br />
    <pxc:information id="Information" runat="server">
    </pxc:information>               
        <div id="dialogModal" runat="server" visible="false" enableviewstate="false">
            <pxc:TableInformation runat="server" ID="TableInfo" TableTitleCssClass="hierarchical_tableinformation_title" TableDescriptionCssClass="hierarchical_tableinformation_description"  EnableViewState="false" Visible="true" />
            <pxc:Footnote ID="Footnote1" runat="server" EnableViewState="false" />
        </div>
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
</asp:Content>
