<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Users-General.aspx.cs" Inherits="PXWeb.Admin.Users_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
    <style type="text/css">
        label { display: block;margin: 4px 0 1px 0; }
        .admin-new-user { margin: 30px 0; }
        .admin-checkbox { display: block;
                          margin: 5px 0; }
        .admin-checkbox label { display: inline; }        
        table { width: 100%;}    
    .GridPager tr {
        display: inherit;
    }
    .textbox-small {
            width: 37px;      
    }
    .textbox-medium {
            width: 89px;      
    }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <asp:GridView runat="server" ID="grvUsers" CellPadding="3" BackColor="White" AllowPaging="True" PageSize="10" AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersUsername %>">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("UserName") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersPassword %>">
                <EditItemTemplate>
                    <asp:TextBox ID="tbPassword" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblPassword" runat="server" Text="***" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersMail %>">
                <EditItemTemplate>
                    <asp:TextBox ID="tbMail" runat="server" Text='<%# Bind("Email") %>' />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblMail" runat="server" Text='<%# Bind("Email") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersAdmin %>">
                <EditItemTemplate>
                    <asp:CheckBox runat="server" ID="cbAdmin" Checked='<%# HasAdminAccess(((MembershipUser)Container.DataItem).UserName)%>' />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" Enabled="False" Checked='<%# HasAdminAccess(((MembershipUser)Container.DataItem).UserName)%>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersLocked %>">
                <EditItemTemplate>
                    <asp:CheckBox runat="server" ID="cbLocked" Enabled="<%#UserIslockedOut((MembershipUser)Container.DataItem) %>" Checked="<%#UserIslockedOut((MembershipUser)Container.DataItem) %>" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" Enabled="False" Checked="<%#UserIslockedOut((MembershipUser)Container.DataItem) %>" />
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersExpires %>">
                <EditItemTemplate>                   
                    <asp:CheckBox runat="server" Text="<%$ PxString: PxWebAdminUsersRenew %>" ID="cbUpdateLicense"/>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text="<%#LicenseExpires(((MembershipUser)Container.DataItem).UserName) %>"/>                  
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="<%$ PxString: PxWebAdminUsersLicense %>">
                <EditItemTemplate>                 
                    <asp:TextBox runat="server" CssClass="textbox-small" Text="<%#GetLicenseNumber(((MembershipUser)Container.DataItem).UserName) %>" ID="tbLicenseNumber"/>
                    <asp:RegularExpressionValidator EnableClientScript="False" ID="val6" runat="server" ErrorMessage="<%$ PxString: PxWebAdminUsersLicenseMustBeNumber %>"
            ControlToValidate="tbLicenseNumber" Style="Color: Red" Display="Dynamic" ValidationExpression="^\d+$" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text="<%#GetLicenseNumber(((MembershipUser)Container.DataItem).UserName) %>"/>                    
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField  ShowEditButton="True" UpdateText="<%$ PxString: PxWebAdminUsersUpdate %>" CancelText="<%$ PxString: PxWebAdminUsersCancel %>" EditText="<%$ PxString: PxWebAdminUsersEdit %>" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ PxString: PxWebAdminUsersDelete %>" />
        </Columns>
        <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
    </asp:GridView>
    <br />
    <asp:PlaceHolder runat="server" ID="plcNewUser">
        <div class="admin-new-user">
            <span class="setting_keyword"><asp:Literal runat="server" Text="<%$ PxString: PxWebAdminUsersCreateNewUser %>"/></span>
            <asp:Label runat="server" Text="<%$ PxString: PxWebAdminUsersUsername %>" AssociatedControlID="tbNewName" />
            <asp:TextBox runat="server" ID="tbNewName" />
            <asp:RequiredFieldValidator EnableClientScript="False" ID="val1" ValidationGroup="grpNewUser" ControlToValidate="tbNewName" runat="server" Style="Color: Red" Display="Dynamic" ErrorMessage="*" />
            <asp:Label ID="Label1" runat="server" Text="<%$ PxString: PxWebAdminUsersPassword %>" AssociatedControlID="tbNewPassword" />
            <asp:TextBox runat="server" ID="tbNewPassword" />       
            <asp:RequiredFieldValidator EnableClientScript="False" ID="val2" ValidationGroup="grpNewUser" ControlToValidate="tbNewPassword" runat="server" Style="Color: Red" Display="Dynamic" ErrorMessage="*" />
            <asp:Label runat="server" Text="<%$ PxString: PxWebAdminUsersMail %>" AssociatedControlID="tbNewMail" />
            <asp:TextBox runat="server" ID="tbNewMail" />
            <asp:RequiredFieldValidator EnableClientScript="False" ID="val3" ValidationGroup="grpNewUser" ControlToValidate="tbNewMail" runat="server" Style="Color: Red" Display="Dynamic" ErrorMessage="*" />
            <asp:RegularExpressionValidator EnableClientScript="False" ID="val4" ValidationGroup="grpNewUser" runat="server" ErrorMessage="<%$ PxString: PxWebAdminUsersErrorMail %>"
                ControlToValidate="tbNewMail" Style="Color: Red" Display="Dynamic" ValidationExpression="[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+(?:[A-Za-z]{2}|com|org|net|gov|biz|info|name|aero|biz|info|jobs|museum)\b" />
            <asp:Label runat="server" Text="<%$ PxString: PxWebAdminUsersLicense %>" AssociatedControlID="tbNewLicenseNumber"/>
            <asp:TextBox runat="server" ID="tbNewLicenseNumber" />
            <asp:RegularExpressionValidator EnableClientScript="False" ID="val5" ValidationGroup="grpNewUser" runat="server" ErrorMessage="<%$ PxString: PxWebAdminUsersLicenseMustBeNumber %>"
                ControlToValidate="tbNewLicenseNumber" Style="Color: Red" Display="Dynamic" ValidationExpression="^\d+$" />
            <asp:CheckBox runat="server" Text="<%$ PxString: PxWebAdminUsersAdmin %>" ID="cbNewAdmin" CssClass="admin-checkbox" />
            <asp:Button runat="server" ID="btnNewUser" ValidationGroup="grpNewUser" Text="<%$ PxString: PxWebAdminUsersCreateNew %>" OnClick="btnNewUser_Click" />
        </div> 
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="plcNewPassword">
        <div class="setting-field">
            <asp:Label ID="lblOldPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordOld %>"></asp:Label>
            <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:CustomValidator ID="validatorOldPassword" runat="server" 
            ControlToValidate="txtOldPassword" OnServerValidate="ValidateOldPassword" 
            ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
        </div>
        <div class="setting-field">
            <asp:Label ID="lblNewPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordNew %>"></asp:Label>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:CustomValidator ID="validatorNewPassword" runat="server" 
            ControlToValidate="txtNewPassword" OnServerValidate="ValidateNewPassword" 
            ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
        </div>
        <div class="setting-field">
            <asp:Label ID="lblVerifyPassword" runat="server" Text="<%$ PxString: PxWebAdminToolsChangePasswordVerify %>"></asp:Label>
            <asp:TextBox ID="txtVerifyPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:CustomValidator ID="validatorVerifyPassword" runat="server" 
            ControlToValidate="txtVerifyPassword" OnServerValidate="ValidateVerifyPassword" 
            ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
        </div>
        <div class="setting-field">
            <asp:Button ID="btnChange" runat="server" CssClass="toolbutton" 
                Text="<%$ PxString: PxWebAdminToolsChangePasswordChange %>" 
                onclick="btnChange_Click" />
        </div>
    </asp:PlaceHolder>

    <asp:Label runat="server" ID="lblOutput" Visible="false" />    
</asp:Content>

