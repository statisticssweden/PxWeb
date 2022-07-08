<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Features-Charts-General.aspx.cs" Inherits="PXWeb.Admin.Features_Charts_General" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdminHead" runat="server">
    <link href="../Resources/Colorpicker/PXWeb_colorpicker.css" rel="stylesheet" type="text/css" /> 
    <script src="../Resources/Colorpicker/PXWeb_colorpicker.js" type="text/javascript"></script>
    <script src="../Resources/Colorpicker/eye.js" type="text/javascript"></script>
    <script src="../Resources/Colorpicker/utils.js" type="text/javascript"></script>    
    <script type="text/javascript">

        $(document).ready(function () {
            BindColorPicker('BackgroundColorGraphs', '#FFFFFF');
            
            BindColorPicker('LineColorPhrame', '#AAAAAA');
            BindColorPicker('Guidelines', '#AAAAAA');

            BindColorPicker('AddColor', '#000000');

            BindColorPicker('BackgroundColor', '#FFFFFF');

            $('.customWidget>div').css('z-index', '1000');


            $('.customWidgetSideControls>input:text').blur(function (event) {
                var id = this.tabIndex;
                switch (id) {
                    case 10000:
                        id = "BackgroundColor";
                        break;
                    case 10001:
                        id = "Guidelines";
                        break;
                    case 10002:
                        id = "BackgroundColorGraphs";
                        break;
                    case 10003:
                        id = "LineColorPhrame";
                        break;
                    case 10005:
                        id = "AddColor";
                        break;

                }

                Eventhandler_ColorInput_Blur(event, id);
            });
            $.each($(".customWidgetSideControls input:text"), function (index, obj) {
                (this).focus();
            });
        }); 

    </script>


         
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="server">
    <p>        
        <asp:Label ID="lblChartSetting" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralChartSettings %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblFontName" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralFontName %>"></asp:Label>
        <asp:DropDownList ID="cboFontName" runat="server">
        </asp:DropDownList>
        <asp:ImageButton ID="imgFontName" runat="server" onclick="FontNameInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblFontSizeTitle" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralFontSizeTitle %>"></asp:Label>
        <asp:TextBox ID="txtFontSizeTitle" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgFontSizeTitle" runat="server" onclick="FontSizeTitleInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorFontSizeTitle" runat="server" 
        ControlToValidate="txtFontSizeTitle" OnServerValidate="ValidateFontSize"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblFontSizeAxis" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralFontSizeAxis %>"></asp:Label>
        <asp:TextBox ID="txtFontSizeAxis" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgFontSizeAxis" runat="server" onclick="FontSizeAxisInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorFontSizeAxis" runat="server" 
        ControlToValidate="txtFontSizeAxis" OnServerValidate="ValidateFontSize"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblLegendFontSize" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLegendFontSize %>"></asp:Label>
        <asp:TextBox ID="txtLegendFontSize" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgLegendFontSize" runat="server" onclick="LegendFontSizeInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLegendFontSize" runat="server" 
        ControlToValidate="txtLegendFontSize" OnServerValidate="ValidateFontSize"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblMaxHeight" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralMaxHeight %>"></asp:Label>
        <asp:TextBox ID="txtMaxHeight" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxHeight" runat="server" onclick="MaxHeightInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxHeight" runat="server" 
        ControlToValidate="txtMaxHeight" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>
    <div class="setting-field">
        <asp:Label ID="lblMaxWidth" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralMaxWidth %>"></asp:Label>
        <asp:TextBox ID="txtMaxWidth" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxWidth" runat="server" onclick="MaxWidthInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="CustomValidator1" runat="server" 
        ControlToValidate="txtMaxWidth" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblMaxLineThickness" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralMaxLineThickness %>"></asp:Label>
        <asp:TextBox ID="txtMaxLineThickness" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxLineThickness" runat="server" onclick="MaxLineThicknessInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxLineThickness" runat="server" 
        ControlToValidate="txtMaxLineThickness" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>   
    <div class="setting-field">
        <asp:Label ID="lblMaxValues" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralMaxValues %>"></asp:Label>
        <asp:TextBox ID="txtMaxValues" runat="server" CssClass="mediuminput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgMaxValues" runat="server" onclick="MaxValuesInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorMaxValues" runat="server" 
        ControlToValidate="txtMaxValues" OnServerValidate="ValidateMandatoryInteger"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
        <!-- maria-->
    <div class="setting-field">
        <asp:Label ID="lblShowSourse" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralShowSourse %>"></asp:Label>
        <asp:DropDownList ID="cboShowSourse" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowSourse" runat="server" onclick="ShowSourseInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowLogo" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralShowLogo %>"></asp:Label>
        <asp:DropDownList ID="cboShowLogo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboShowLogo_SelectedIndexChanged" >
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowLogo" runat="server" onclick="ShowLogoInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <asp:Panel ID="pnlLogotypeInGraphs" runat="server" Visible="false">
        <div class="setting-field">
            <asp:Label ID="lblImgLogo" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLogoImg %>"></asp:Label>
            <asp:TextBox ID="txtPathImgLogo" runat="server"   ></asp:TextBox>
            <asp:ImageButton ID="imgImgLogo" runat="server" onclick="ImgLogoInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            <asp:CustomValidator ID="CustomValidator2" runat="server" 
            ControlToValidate="txtPathImgLogo" OnServerValidate="ValidateLogoPath"
            ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
        </div>
    </asp:Panel>

        <div class="setting-field">
        <asp:Label ID="lblBackgroundColor" runat="server" CssClass="settingFieldText" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralBackgroundColor %>"></asp:Label>         
        <div class="settingFieldControls">
            <div id="customWidgetBackgroundColor" class="customWidget">
                <div id="selectorBackgroundColor" class="colorpickerselector">
                    <div style="background-color: <%# BackgroundColor %>"></div>
                </div>
                <div id="pickerBackgroundColor" class="colorpickercontainer"></div>
            </div>
            <div id="deleteButtonBackgroundColor" class="customWidgetSideControls">
                <asp:TextBox runat="server" TabIndex="10000" MaxLength="7"  ID="txtBackgroundColor" CssClass="selectedColorBackgroundColor colorTextField" />
                <asp:ImageButton ID="imgBackgroundColor" runat="server" onclick="BackgroundColorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            </div>    
        </div>
         <asp:CustomValidator ID="validatorBackgroundColor" runat="server" 
        ControlToValidate="txtBackgroundColor" OnServerValidate="ValidateColor"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>

        <asp:Label ID="lblBackgroundAlpha" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralBackgroundAlpha %>"></asp:Label>
        <asp:TextBox ID="txtBackgroundAlpha" runat="server" CssClass="mediuminput" MaxLength="3"></asp:TextBox>
        <asp:ImageButton ID="imgBackgroundAlpha" runat="server" onclick="BackgroundAlphaInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="BackgroundAlphaValidator" runat="server" 
        ControlToValidate="txtBackgroundAlpha" OnServerValidate="ValidateAlpha"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>

    <div class="setting-field">
        <asp:Label ID="lblGuidelinesColor" runat="server" CssClass="settingFieldText" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralGuidelinesColor %>"></asp:Label>         
        <div class="settingFieldControls">
            <div id="customWidgetGuidelines" class="customWidget">
                <div id="selectorGuidelines" class="colorpickerselector">
                    <div style="background-color: <%# GuidelinesColor %>"></div>
                </div>
                <div id="pickerGuidelines" class="colorpickercontainer"></div>
            </div>
            <div id="deleteButtonGuidelines" class="customWidgetSideControls">
                <asp:TextBox runat="server" TabIndex="10001" MaxLength="7"  ID="txtGuidelinesColor" CssClass="selectedColorGuidelines colorTextField" />
                <asp:ImageButton ID="imgGuidelinesColor" runat="server" onclick="GuidelinesColorInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            </div>    
        </div>
         <asp:CustomValidator ID="validatorGuidelinesColor" runat="server" 
        ControlToValidate="txtGuidelinesColor" OnServerValidate="ValidateColor"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblBackgroundColorGraphs" runat="server" CssClass="settingFieldText" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralBackgroundColorGraphs %>"></asp:Label>         
        <div class="settingFieldControls">
            <div id="customWidgetBackgroundColorGraphs" class="customWidget">
                <div id="selectorBackgroundColorGraphs" class="colorpickerselector">
                    <div style="background-color: <%# BackgroundColorGraphs %>"></div>
                </div>
                <div id="pickerBackgroundColorGraphs" class="colorpickercontainer"></div>
            </div>
            <div id="deleteButtonBackgroundColorGraphs" class="customWidgetSideControls">
                <asp:TextBox runat="server" TabIndex="10002" MaxLength="7"  ID="txtBackgroundColorGraphs" CssClass="selectedColorBackgroundColorGraphs colorTextField" />
                <asp:ImageButton ID="imgBackgroundColorGraphs" runat="server" onclick="BackgroundColorGraphsInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            </div>    
        </div>
         <asp:CustomValidator ID="CustomValidator3" runat="server" 
        ControlToValidate="txtBackgroundColorGraphs" OnServerValidate="ValidateColor"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>

        <asp:Label ID="lblChartAlpha" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralChartAlpha %>"></asp:Label>
        <asp:TextBox ID="txtChartAlpha" runat="server" CssClass="mediuminput" MaxLength="3"></asp:TextBox>
        <asp:ImageButton ID="imgChartAlpha" runat="server" onclick="ChartAlphaInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorChartAlpha" runat="server" 
        ControlToValidate="txtChartAlpha" OnServerValidate="ValidateAlpha"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblLineColorPhrame" runat="server" CssClass="settingFieldText" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLineColorPhrame %>"></asp:Label>         
        <div class="settingFieldControls">
            <div id="customWidgetLineColorPhrame" class="customWidget">
                <div id="selectorLineColorPhrame" class="colorpickerselector">
                    <div style="background-color: <%# LineColorPhrame %>"></div>
                </div>
                <div id="pickerLineColorPhrame" class="colorpickercontainer"></div>
            </div>
            <div id="deleteLineColorPhrame" class="customWidgetSideControls">
                <asp:TextBox runat="server" TabIndex="10003" MaxLength="7"  ID="txtLineColorPhrame" CssClass="selectedColorLineColorPhrame colorTextField" />
                <asp:ImageButton ID="imgLineColorPhrame" runat="server" onclick="LineColorPhrameInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
            </div>    
        </div>
         <asp:CustomValidator ID="CustomValidator4" runat="server" 
        ControlToValidate="txtLineColorPhrame" OnServerValidate="ValidateColor"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblLineThicknessPhrame" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLineThicknessPhrame %>"></asp:Label>
        <asp:TextBox ID="txtLineThicknessPhrame" runat="server" CssClass="smallinput" MaxLength="10" TabIndex="10004"></asp:TextBox>
        <asp:ImageButton ID="imgLineThicknessPhrame" runat="server" onclick="LineThicknessPhrameInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="CustomValidatorPhrame" runat="server" 
        ControlToValidate="txtLineThicknessPhrame" OnServerValidate="ValidateLineThicknessPhrame"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div> 

    <p>
        <asp:Label ID="lblColors" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralColors %>" CssClass="setting_keyword"></asp:Label>
        <asp:ImageButton ID="imgColors" runat="server" 
            onclick="ColorsInfo"  Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>"/> <br />
        <asp:Repeater ID="rptColors" runat="server" 
            onitemcommand="rptColors_ItemCommand" onitemdatabound="rptColors_ItemDataBound">
            <HeaderTemplate>
                <div class="setting-field">
            </HeaderTemplate>
            <ItemTemplate>
                    <div id="customWidget<%# Container.ItemIndex%>" class="customWidget">
			            <div id="selector<%# Container.ItemIndex%>" class="colorpickerselector"><div style="background-color: <%# GetString(Container.DataItem) %>"></div></div>
	                    <div id="picker<%# Container.ItemIndex%>" class="colorpickercontainer"></div>
		            </div>
		            <div id="deleteButton<%# Container.ItemIndex%>" class="customWidgetSideControls">
                        <asp:TextBox runat="server" TabIndex="<%# Container.ItemIndex%>" MaxLength="7" ID="tbSelectedColor" CssClass="colorTextField" /> 
                        <asp:LinkButton ID="lnkDelete" runat="server" Text='<%$ PxString: PxWebAdminFeaturesChartsGeneralDeleteColor %>' CommandName="DeleteColor" CommandArgument='<%# GetString(Container.DataItem) %>'></asp:LinkButton>
                    </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </p>
    
    <div id="customWidgetAddColor" class="customWidget">
        <div id="selectorAddColor" class="colorpickerselector"><div style="background-color: #FFFFFF"></div></div>
        <div id="pickerAddColor" class="colorpickercontainer"></div>
    </div>
    <div id="deleteButtonAddColor" class="customWidgetSideControls">
        <asp:TextBox runat="server" TabIndex="10005" MaxLength="7"  ID="txtAddSelectedColor" CssClass="selectedColorAddColor" ></asp:TextBox>
        <asp:LinkButton ID="lnkAddColor" runat="server" Text='<%$ PxString: PxWebAdminFeaturesChartsGeneralAddColor %>' OnClick="lnkAddColor_Click"></asp:LinkButton>
    </div>
    <asp:CustomValidator ID="validatorAddColor" runat="server" 
    ControlToValidate="txtAddSelectedColor" OnServerValidate="ValidateColor"
    ErrorMessage="*" ValidateEmptyText="False" CssClass="setting-field-validator" >
    </asp:CustomValidator>

    <br /><br /><br />
    <p>        
        <asp:Label ID="Label1" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralUserSettings %>" CssClass="setting_keyword"></asp:Label>
    </p>
    <div class="setting-field">
        <asp:Label ID="lblHeight" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralHeight %>"></asp:Label>
        <asp:TextBox ID="txtHeight" runat="server" CssClass="smallinput" MaxLength="10" TabIndex="10006"></asp:TextBox>
        <asp:ImageButton ID="imgHeight" runat="server" onclick="HeightInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorHeight" runat="server" 
        ControlToValidate="txtHeight" OnServerValidate="ValidateHeight"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblWidth" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralWidth %>"></asp:Label>
        <asp:TextBox ID="txtWidth" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgWidth" runat="server" onclick="WidthInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorWidth" runat="server" 
        ControlToValidate="txtWidth" OnServerValidate="ValidateWidth"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblLineThickness" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLineThickness %>"></asp:Label>
        <asp:TextBox ID="txtLineThickness" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgLineThickness" runat="server" onclick="LineThicknessInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLineThickness" runat="server" 
        ControlToValidate="txtLineThickness" OnServerValidate="ValidateLineThickness"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
     </div>     
    <div class="setting-field">
        <asp:Label ID="lblLabelOrientation" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLabelOrientation %>"></asp:Label>
        <asp:DropDownList ID="cboLabelOrientation" runat="server">
            <asp:ListItem Value="Horizontal" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLabelOrientationHorizontal %>"></asp:ListItem>
            <asp:ListItem Value="Vertical" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLabelOrientationVertical %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgLabelOrientation" runat="server" onclick="LabelOrientationInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblTimeSortOrder" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralTimeSortOrder %>"></asp:Label>
        <asp:DropDownList ID="cboTimeSortOrder" runat="server">
            <asp:ListItem Value="Ascending" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralTimeSortOrderAscending %>"></asp:ListItem>
            <asp:ListItem Value="Descending" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralTimeSortOrderDescending %>"></asp:ListItem>
            <asp:ListItem Value="None" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralTimeSortOrderNone %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgTimeSortOrder" runat="server" onclick="TimeSortOrderInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblShowLegend" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralShowLegend %>"></asp:Label>
        <asp:DropDownList ID="cboShowLegend" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgShowLegend" runat="server" onclick="ShowLegendInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblLegendHeight" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralLegendHeight %>"></asp:Label>
        <asp:TextBox ID="txtLegendHeight" runat="server" CssClass="smallinput" MaxLength="10"></asp:TextBox>
        <asp:ImageButton ID="imgLegendHeight" runat="server" onclick="LegendHeightInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
         <asp:CustomValidator ID="validatorLegendHeight" runat="server" 
        ControlToValidate="txtLegendHeight" OnServerValidate="ValidateLegendHeight"
        ErrorMessage="*" ValidateEmptyText="True" CssClass="setting-field-validator" ></asp:CustomValidator>
    </div>
    <div class="setting-field">
        <asp:Label ID="lblGuidelinesHorizontal" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralGuidelinesHorizontal %>"></asp:Label>
        <asp:DropDownList ID="cboGuidelinesHorizontal" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgGuidelinesHorizontal" runat="server" onclick="GuidelinesHorizontalInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
    <div class="setting-field">
        <asp:Label ID="lblGuidelinesVertical" runat="server" Text="<%$ PxString: PxWebAdminFeaturesChartsGeneralGuidelinesVertical %>"></asp:Label>
        <asp:DropDownList ID="cboGuidelinesVertical" runat="server">
            <asp:ListItem Value="True" Text="<%$ PxString: PxWebAdminYes %>"></asp:ListItem>
            <asp:ListItem Value="False" Text="<%$ PxString: PxWebAdminNo %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:ImageButton ID="imgGuidelinesVertical" runat="server" onclick="GuidelinesVerticalInfo" Height="15px" Width="15px" ImageUrl="<%$ PxImage: questionmark.gif %>" />
    </div>
       
</asp:Content>
