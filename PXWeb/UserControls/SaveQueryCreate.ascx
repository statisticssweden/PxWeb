<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveQueryCreate.ascx.cs" Inherits="PXWeb.UserControls.SaveQueryCreate" %>

<div class="savequery_container" >

    <a id="tableSavequerycontrol"></a>         
        <asp:Panel ID="pnl2_SaveQuerySelection"  CssClass="savequery_informationpanel savequery_informationpanelwidth settingpanel savequery" runat="server">            
            <asp:Panel ID="pnlSettings" runat="server" CssClass="px-settings">
                <div id="divTimePeriod" class="savequery_timeperioddiv"  >
                    <div id="divTimeWarning" runat="server" class="time_warning_panel">
                        <asp:Image ID="imgTimeWarning" CssClass="alertimage" runat="server" />
                        <asp:Label ID="lblTimeWarning" runat="server" Text="<%$ PxString: CtrlSaveQueryTimeWarning %>"></asp:Label>
                    </div>
                    <h2><asp:Label ID="lblTimePeriod" runat="server" Text="<%$ PxString: CtrlSaveQueryChoosetimeperiod %>" CssClass="savequery_informationtext savequery_heading"></asp:Label></h2>
                    <asp:Label ID="lblTimePeriodInformation" runat="server" Text="<%$ PxString: CtrlSaveQueryChoosetimeperiodInformation %>" CssClass="savequery_informationtext"></asp:Label>
                    <asp:RadioButtonList runat="server" id="rblTimePeriod"  CssClass="savequery_rblist"    >
                        <asp:ListItem Selected="True" Text="<%$ PxString: CtrlSaveQueryExtendedTimePeriod %>" Value="from"></asp:ListItem>              
                        <asp:ListItem Text="<%$ PxString: CtrlSaveQueryFloatingTimePeriod %>" Value="top"></asp:ListItem>                    
                        <asp:ListItem Text="<%$ PxString: CtrlSaveQuerySelectedTimePeriod %>" Value="item"></asp:ListItem>      
                    </asp:RadioButtonList>
                    <div id="divSaveAs" class="savequery_saveasdiv"  >
                        <asp:Label ID="lblResultAs" runat="server" Text="Result as" CssClass="savequery_informationtext savequery_heading"></asp:Label>
                        <asp:dropdownlist id="ddlOutputFormats" runat="server" CssClass="commandbar_saveas_dropdownlist saveas_dropdownlist">
                        </asp:dropdownlist>
                        <asp:Label ID="lblFormatError"  runat="server" cssClass="savequery_error"></asp:Label>
                    </div>
                    <div class="savequery_functionbtn">                     
                        <asp:HyperLink ID="lnkCancelSaveQuery" runat="server" CssClass="savequery_cancel">
                            <asp:Label ID="lblCancelSaveQuery" runat="server" Text="<%$ PxString: CtrlSaveQueryCancelSaveQuery %>"></asp:Label>
                        </asp:HyperLink> 
                        <asp:Button ID="btnCreateSaveQuery" runat="server" CssClass="savequery_create" Text="<%$ PxString: CtrlSaveQuerybtnCreateQuery %>"  OnClick="CreateSavedQueryUrl"   />                
                        <asp:Label ID="lblError"  runat="server"  cssClass="savequery_error" Text="<%$ PxString: CtrlSaveQueryFailedSave %>" Visible="false"></asp:Label>
                    </div>
                </div>
                <div id="divHelp">
                    <asp:Label ID="lblHeadingInformationText" runat="server" Text="<%$ PxString: CtrlSaveQueryHeadingExplanationText %>" CssClass="savequery_informationtext savequery_heading"></asp:Label>
                    <asp:Label ID="lblInformationText" runat="server" Text="<%$ PxString: CtrlSaveQueryExplanationText %>" CssClass="savequery_informationtext"></asp:Label>                
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnl3_ShowSaveQueryUrl" CssClass="savequery_informationpanel savequery_informationpanelwidth settingpanel" runat="server">  
            <asp:Panel ID="pnlUrl" runat="server" CssClass="px-settings">
                <div id="divQuerySummary" class="querySummary">
                    <asp:Label ID="lblUpdateSummaryHeading" runat="server" Text="<%$ PxString: CtrlSaveQueryUpdateSummaryHeading %>" CssClass="savequery_informationtext savequery_heading querySummaryItem"></asp:Label>
                    <asp:Label ID="lblUpdateSummaryValue" runat="server" Text="" CssClass="savequery_informationtext querySummaryItem"></asp:Label>
                    <asp:Label ID="lblOutputSummaryHeading" runat="server" Text="<%$ PxString: CtrlSaveQueryOutputSummaryHeading %>" CssClass="savequery_informationtext savequery_heading querySummaryItem"></asp:Label>
                    <asp:Label ID="lblOutputSummaryValue" runat="server" Text="" CssClass="savequery_informationtext querySummaryItem"></asp:Label>
                </div>
                <asp:Label ID="lblWithdrawInformation" runat="server" Text="<%$ PxString: CtrlSaveQueryURLInfo %>" CssClass="savequery_informationtext"></asp:Label>                        
                <asp:TextBox runat="server" ID="txtSaveQueryUrl" cssclass="savequery_url" ReadOnly="true"  ></asp:TextBox> 
                <div class="savequery_functionbtn">    
                    <asp:HyperLink ID="lnkBack" runat="server" CssClass="savequery_back savequery_function">
                        <asp:Label ID="lblBack" runat="server" Text="<%$ PxString: CtrlSaveQuerybtnBack %>"></asp:Label>
                    </asp:HyperLink>
                    <asp:Button  id="btnbookMark" cssclass="savequery_btnbookmark savequery_function savequery_novisibility" onclientclick="Bookmark(); return false;" text="<%$ PxString: CtrlSaveQueryBookmark%>"  runat="server" />
                    <asp:Button  id="btnCopyToClipboard" cssclass="savequery_btncopy savequery_function savequery_novisibility" text="<%$ PxString: CtrlSaveQueryCopyLink %>" OnClientClick="CopyToClipboard(); return false;" CausesValidation="false"  runat="server" />           
                    <asp:Button  id="btnMailSaveQuery" CssClass="savequery_btnmail savequery_function savequery_novisibility" text="<%$ PxString: CtrlSaveQueryMailQuery %>"  OnClientClick="document.location = sendMail(); return false;" runat="server"   /> 
                </div>
            </asp:Panel>
             
            
       </asp:Panel>  

 </div>

<script type="text/javascript" >

        jQuery(document).ready(function () {

            jQuery('.saveas_dropdownlist').change(function () {
                if (jQuery('option:selected', this).val() != "selectFormat") {
                    jQuery('#<%=lblFormatError.ClientID %>').hide(0);
                }
                else {
                    jQuery('#<%=lblFormatError.ClientID %>').show(0);
                }
            });
            
            jQuery('.savequery_cancel').click(function () {
                // Reset selected value in radiobuttonlist
                jQuery('#<%=rblTimePeriod.ClientID %>').find("input[value='from']").prop('checked', true);
                // Reset selected value in dropdownlist
                jQuery('#<%=ddlOutputFormats.ClientID %>').val("selectFormat");
                // Hide errormessage
                jQuery('#<%=lblFormatError.ClientID %>').hide(0);

                //Hide any currently displayed setting panel
                jQuery('.settingpanel').hide(0);

                //Change expand image on all links
                var col = jQuery('.px-settings-collapseimage');
                col.removeClass('px-settings-collapseimage');
                col.addClass('px-settings-expandimage');

                // Manipulation on the Save query link on the Presentation.Master page:
                // - Remove the expanded class for the Save query link
                jQuery('[id$=lnkSaveQueryInformation]').removeClass('settingpanelexpanded');

                return false;
            });

            if ((jQuery('[id$=pnl3_ShowSaveQueryUrl]').is(':visible')) || (jQuery('[id$=lblFormatError]').is(':visible'))) {
                // The URL-panel is displayed
                // Do some manipulation on the Save query link on the Presentation.Master page:
                // 1. Set the correct image to the Save query expander/collapser
                jQuery('[id$=imgShowSaveQueryExpander]').removeClass('px-settings-expandimage');
                jQuery('[id$=imgShowSaveQueryExpander]').addClass('px-settings-collapseimage');
                // 2. Set the correct class on the Save query link
                jQuery('[id$=lnkSaveQueryInformation]').addClass('settingpanelexpanded');
            }

            jQuery('[id$=btnMailSaveQuery]').css('display', 'inline-block');

            if (typeof window.external == "object" && ('AddFavorite' in window.external)) {

                jQuery('[id$=btnbookMark]').css('display', 'inline-block');

            }
            
            if (window.clipboardData && clipboardData.setData) {

                jQuery('[id$=btnCopyToClipboard]').css('display', 'inline-block');
            }

            jQuery('[id$=lnkBack]').click(function () {
                jQuery('[id$=pnl2_SaveQuerySelection]').show(0);
                jQuery('[id$=pnl3_ShowSaveQueryUrl]').hide(0);
                return false;
            });

    });
    function sendMail() {
        var link = "mailto:" +
                   "?subject=" + "<%=Subject%>" +
                   "&body=" + escape("<%=txtSaveQueryUrl.Text%>");
        return link;
    }
    function CopyToClipboard() 
    { 
        window.clipboardData.setData('Text', "<%=txtSaveQueryUrl.Text%>");
        ShowPanel();
        return false;
    }
    
    function Bookmark(e)
    {
        if (typeof window.external == "object" )
        {
            window.external.AddFavorite("<%=txtSaveQueryUrl.Text%>", "<%=Subject%>");
        }

    }
    function ShowPanel()
    {
        jQuery('[id$=pnl3_ShowSaveQueryUrl]').show(0);
        jQuery('[id$=pnl2_SaveQuerySelection]').hide(0);
        jQuery('[id$=pnl1_SaveQueryInformationHidden]').hide(0);

    }


</script>   
