<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Settings-General-Global.aspx.cs" Inherits="PXWeb.Admin.Settings_General_Global" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <div class="setting-field">
        <asp:Label ID="lblSecrecyOption" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSecrecyOption %>"></asp:Label>
        <asp:DropDownList ID="cboSecrecyOption" runat="server">
            <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsGlobalSecrecyOptionNone %>"></asp:ListItem>
            <asp:ListItem Value="Simple" Text="<%$ PxString: PxWebAdminSettingsGlobalSecrecyOptionSimple %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgSecrecyOption" runat="server" onclick="SecrecyOptionInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblRoundingRule" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalRoundingRule %>"></asp:Label>
        <asp:DropDownList ID="cboRoundingRule" runat="server">
            <asp:ListItem Value="BankersRounding" Text="<%$ PxString: PxWebAdminSettingsGlobalRoundingRuleBankersRounding %>"></asp:ListItem>
            <asp:ListItem Value="RoundUp" Text="<%$ PxString: PxWebAdminSettingsGlobalRoundingRuleRoundUp %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgRoundingRule" runat="server" onclick="RoundingRuleInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol1" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol1 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol1" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol1" runat="server" onclick="Symbol1Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol1" runat="server" 
        ControlToValidate="txtSymbol1" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol2" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol2 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol2" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol2" runat="server" onclick="Symbol2Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol2" runat="server" 
        ControlToValidate="txtSymbol2" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol3" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol3 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol3" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol3" runat="server" onclick="Symbol3Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol3" runat="server" 
        ControlToValidate="txtSymbol3" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol4" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol4 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol4" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol4" runat="server" onclick="Symbol4Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol4" runat="server" 
        ControlToValidate="txtSymbol4" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol5" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol5 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol5" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol5" runat="server" onclick="Symbol5Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol5" runat="server" 
        ControlToValidate="txtSymbol5" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblSymbol6" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalSymbol6 %>"></asp:Label>
        <asp:TextBox ID="txtSymbol6" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgSymbol6" runat="server" onclick="Symbol6Info" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorSymbol6" runat="server" 
        ControlToValidate="txtSymbol6" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDataSymbolNil" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalDataSymbolNil %>"></asp:Label>
        <asp:TextBox ID="txtDataSymbolNil" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgDataSymbolNil" runat="server" onclick="DataSymbolNilInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorDataSymbolNil" runat="server" 
        ControlToValidate="txtDataSymbolNil" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDataSymbolSum" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalDataSymbolSum %>"></asp:Label>
        <asp:TextBox ID="txtDataSymbolSum" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgDataSymbolSum" runat="server" onclick="DataSymbolSumInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        <asp:CustomValidator ID="validatorDataSymbolSum" runat="server" 
        ControlToValidate="txtDataSymbolSum" OnServerValidate="ValidateSymbol"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    
<!--    
    <div class="setting-field">
        <asp:Label ID="lblDecimalSeparator" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalDecimalSeparator %>"></asp:Label>
        <asp:DropDownList ID="cboDecimalSeparator" runat="server" >
            <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsGlobalDecimalSeparatorPoint %>"></asp:ListItem>
            <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsGlobalDecimalSeparatorComma %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDecimalSeparator" runat="server" onclick="DecimalSeparatorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblThousandSeparator" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalThousandSeparator %>"></asp:Label>
        <asp:DropDownList ID="cboThousandSeparator" runat="server" >
            <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsGlobalThousandSeparatorNone %>"></asp:ListItem>
            <asp:ListItem Value="Space" Text="<%$ PxString: PxWebAdminSettingsGlobalThousandSeparatorSpace %>"></asp:ListItem>
            <asp:ListItem Value="Point" Text="<%$ PxString: PxWebAdminSettingsGlobalThousandSeparatorPoint %>"></asp:ListItem>
            <asp:ListItem Value="Comma" Text="<%$ PxString: PxWebAdminSettingsGlobalThousandSeparatorComma %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgThousandSeparator" runat="server" onclick="ThousandSeparatorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    
-->
        <div class="setting-field">
            <asp:Label ID="lblShowSourceDescription" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalShowSourceDescription %>"></asp:Label>
            <asp:DropDownList ID="cboShowSourceDescription" runat="server">
                <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
                <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:ImageButton ID="imgShowSourceDescription" runat="server" onclick="ShowSourceDescriptionInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
        </div>
    <div class="setting-field">
        <asp:Label ID="lblTableInformationLevel" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalTableInformationLevel %>"></asp:Label>
        <asp:DropDownList ID="cboTableInformationLevel" runat="server" >
            <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminSettingsGlobalTableInformationLevelNone %>"></asp:ListItem>
            <asp:ListItem Value="MandantoryFootnotesOnly" Text="<%$ PxString: PxWebAdminSettingsGlobalTableInformationLevelMandantoryFootnotesOnly %>"></asp:ListItem>
            <asp:ListItem Value="AllFootnotes" Text="<%$ PxString: PxWebAdminSettingsGlobalTableInformationLevelAllFootnotes %>"></asp:ListItem>
            <asp:ListItem Value="AllInformation" Text="<%$ PxString: PxWebAdminSettingsGlobalTableInformationLevelAllInformation %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgTableInformationLevel" runat="server" onclick="TableInformationLevelInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblUpperCase" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalUpperCase %>"></asp:Label>
        <asp:DropDownList ID="cboUpperCase" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgUpperCase" runat="server" onclick="UpperCaseInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblDataNotePlacement" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalDataNotePlacement %>"></asp:Label>
        <asp:DropDownList ID="cboDataNotePlacement" runat="server">
            <asp:ListItem Value="Before" Text="<%$ PxString: PxWebAdminSettingsGlobalDataNotePlacementBeforeValue %>"></asp:ListItem>
            <asp:ListItem Value="After" Text="<%$ PxString: PxWebAdminSettingsGlobalDataNotePlacementAfterValue %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgDataNotePlacement" runat="server" onclick="DataNotePlacementInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblRemoveSingleContent" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalRemoveSingleContent %>"></asp:Label>
        <asp:DropDownList ID="cboRemoveSingleContent" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgRemoveSingleContent" runat="server" onclick="RemoveSingleContentInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblStrictAggregationCheck" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalStrictAggregationCheck %>"></asp:Label>
        <asp:DropDownList ID="cboStrictAggregationCheck" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgStrictAggregationCheck" runat="server" onclick="StrictAggregationCheckInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <p>
        <asp:Label ID="lblShowInformationTypes" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalShowInformationTypes %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgShowInformationTypes" runat="server" 
            onclick="ShowInformationTypesInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/> <br />
        <asp:Repeater ID="rptInformationTypes" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HiddenField ID="hidSetting" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                <asp:CheckBox ID="chkInfoType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' Checked='<%# DataBinder.Eval(Container.DataItem,"Selected") %>' />
                <br />
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>       
    </p>
    
        <asp:Label ID="lblShowInfoFile" runat="server" Text="<%$ PxString: PxWebDetailedInformation %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgShowInfoFile" runat="server" 
            onclick="ShowInformationInfoFile"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/> <br />
        <asp:CheckBox ID="chkInfoFile" runat="server" Style="padding-right:0px;" />
        <asp:Label ID="lblInfoFile" runat="server" Text="<%$ PxString: PxWebAdminSettingsGlobalShowInfoFile %>" Style="padding-left:0px;" ></asp:Label>
    
        
</asp:Content>
