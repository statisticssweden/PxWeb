<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveQueryCreate.ascx.cs" Inherits="PXWeb.UserControls.SaveQueryCreate" %>

<div class="savequery_container" >

    <a id="tableSavequerycontrol"></a>         
        <asp:Panel ID="pnl2_SaveQuerySelection"  CssClass="settingpanel savequery" runat="server">            
            <asp:Panel ID="pnlSettings" runat="server" CssClass="px-settings">
                <div id="divTimeWarning" runat="server" class="time_warning_panel">
                    <asp:Image ID="imgTimeWarning" CssClass="alertimage" runat="server" />
                    <asp:Label ID="lblTimeWarning" runat="server" Text="<%$ PxString: CtrlSaveQueryTimeWarning %>"></asp:Label>
                </div>
                <asp:Panel ID="pnlForRbl" runat="server">
                <asp:RadioButtonList runat="server" id="rblTimePeriod" RepeatLayout="Flow" CssClass="savequery_rblist"    >
                    <asp:ListItem Selected="True" Text="<%$ PxString: CtrlSaveQueryFloatingTimePeriod %>" Value="top"></asp:ListItem>  
                    <asp:ListItem Text="<%$ PxString: CtrlSaveQueryExtendedTimePeriod %>" Value="from"></asp:ListItem>                                
                    <asp:ListItem Text="<%$ PxString: CtrlSaveQuerySelectedTimePeriod %>" Value="item"></asp:ListItem>      
                </asp:RadioButtonList>
                </asp:Panel>
                <div id="divSaveAs" class="flex-column s-margin-top">
                    <asp:Label ID="lblResultAs" runat="server" AssociatedControlID="ddlOutputFormats" Text="Result as" CssClass="font-heading"></asp:Label>
                    <asp:dropdownlist id="ddlOutputFormats" runat="server" CssClass="commandbar_saveas_dropdownlist saveas_dropdownlist xs-margin-top">
                    </asp:dropdownlist>
                    <asp:Label ID="lblFormatError" role="alert" runat="server" cssClass="savequery_error"></asp:Label>
                </div>
                <div class="container_exit_buttons_row">
                    <asp:Button ID="btnCancelSaveQuery" runat="server" CssClass="pxweb-btn" Text="<%$ PxString: CtrlSaveQueryCancelSaveQuery %>" OnClientClick="cancelSavequery(); return false;" />
                    <asp:Button ID="btnCreateSaveQuery" runat="server" CssClass="pxweb-btn primary-btn container_continuebutton " Text="<%$ PxString: CtrlSaveQuerybtnCreateQuery %>"  OnClick="CreateSavedQueryUrl"   />                
                    <asp:Label ID="lblError"  runat="server" role="alert" cssClass="savequery_error" Text="<%$ PxString: CtrlSaveQueryFailedSave %>" Visible="false"></asp:Label>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnl3_ShowSaveQueryUrl" CssClass="settingpanel" runat="server">  
            <asp:Panel ID="pnlUrl" runat="server" CssClass="px-settings">
                <div id="divQuerySummary" class="querySummary flex-column">
                    <div class="flex-row flex-wrap">
                        <asp:Label ID="lblUpdateSummaryHeading" runat="server" Text="<%$ PxString: CtrlSaveQueryUpdateSummaryHeading %>" CssClass="querySummaryItem font-heading"></asp:Label>
                        <asp:Label ID="lblUpdateSummaryValue" runat="server" Text="" CssClass=" querySummaryItem querysummary-value"></asp:Label>
                    </div>
                    <div class="flex-row">  
                        <asp:Label ID="lblOutputSummaryHeading" runat="server" Text="<%$ PxString: CtrlSaveQueryOutputSummaryHeading %>" CssClass="querySummaryItem font-heading"></asp:Label>
                        <asp:Label ID="lblOutputSummaryValue" runat="server" Text="" CssClass=" querySummaryItem querysummary-value"></asp:Label>
                    </div>
                </div>
                <asp:Label ID="lblWithdrawInformation" runat="server" Text="<%$ PxString: CtrlSaveQueryURLInfo %>" CssClass="font-heading"></asp:Label>                        
                <asp:TextBox runat="server" ID="txtSaveQueryUrl" aria-label="<%$ PxString: CtrlSaveQueryUrlScreenReader %>" cssclass="savequery_url s-margin-top" ReadOnly="true"  ></asp:TextBox> 
                <div class="container_exit_buttons_row">
                    <asp:Button ID="btnCancelSaveQueryEnd" runat="server" CssClass="pxweb-btn" Text="<%$ PxString: CtrlSaveQueryCancelSaveQuery %>" OnClientClick="cancelSavequeryEnd(); return false;" />
                    <asp:Button ID="btnbookMark" cssclass="pxweb-btn savequery_novisibility" onclientclick="Bookmark(); return false;" text="<%$ PxString: CtrlSaveQueryBookmark%>"  runat="server" />
                    <asp:Button ID="btnCopyToClipboard" cssclass="pxweb-btn savequery_novisibility" text="<%$ PxString: CtrlSaveQueryCopyLink %>" OnClientClick="CopyToClipboard();return false" CausesValidation="false"  runat="server" />           
                    <asp:Button ID="btnMailSaveQuery" CssClass="pxweb-btn no-margin-right savequery_novisibility" text="<%$ PxString: CtrlSaveQueryMailQuery %>"  OnClientClick="document.location = sendMail(); return false;" runat="server"   /> 
                </div>
            </asp:Panel>
             
            
       </asp:Panel>  

 </div>

<script>

        jQuery(document).ready(function () {

            jQuery('.saveas_dropdownlist').change(function () {
                if (jQuery('option:selected', this).val() != "selectFormat") {
                    jQuery('#<%=lblFormatError.ClientID %>').hide(0);
                }
                else {
                    jQuery('#<%=lblFormatError.ClientID %>').show(0);
                }
            });

            if ((jQuery('[id$=lblFormatError]').is(':visible'))) {
               
            }

            jQuery('[id$=btnMailSaveQuery]').css('display', 'inline-block');

            if (typeof window.external == "object" && ('AddFavorite' in window.external)) {

                jQuery('[id$=btnbookMark]').css('display', 'inline-block');

            }

            jQuery('[id$=btnCopyToClipboard]').css('display', 'inline-block');            

        });

    function cancelSavequery() {
        jQuery('#<%=lblFormatError.ClientID %>').hide(0);
        closeAccordion('SaveQueryHeader', 'SaveQueryBody');
    }

    function cancelSavequeryEnd() {
        jQuery('#<%=lblFormatError.ClientID %>').hide(0);
        closeAccordion('SaveQueryHeader', 'SaveQueryBody');
        jQuery('[id$=pnl2_SaveQuerySelection]').show(0);
        jQuery('[id$=pnl3_ShowSaveQueryUrl]').hide(0);
    }
    
    function sendMail() {
        var link = "mailto:" +
                   "?subject=" + "<%=Subject%>" +
                   "&body=" + escape("<%=txtSaveQueryUrl.Text%>");
        return link;
    }
<%--    function CopyToClipboard() 
    { 
        window.clipboardData.setData('Text', "<%=txtSaveQueryUrl.Text%>");
        ShowPanel();
        return false;
    }--%>

    function CopyToClipboard() {
        var copyBtn = document.getElementById('<%=btnCopyToClipboard.ClientID %>')
        var queryLink = document.getElementById('<%=txtSaveQueryUrl.ClientID %>')
        queryLink.focus();
        queryLink.select(); // for mark as copied
        navigator.clipboard.writeText("<%=txtSaveQueryUrl.Text%>");
        copyBtn.value = ('<%= CopyLinkCopied %>');
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
