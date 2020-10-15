<%@ control  inherits="PCAxis.Web.Controls.VariableSelectorValueSelectCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>


<div class="variableselector_valuesselect_box">

<asp:Panel runat="server" ID="ValuesSelectContainerPanel" DefaultButton="SearchValuesButton">
    <asp:Panel runat="server" ID="Panel1" CssClass="variableselector_valuesselect_variabletitle_panel">
            <div class="variableselector_valuesselect_variabletitle_container">
                <asp:Panel runat="server" ID="MetadataPanel" class="metadata-container" Visible="False">
                    <div role="button" class="metadata-open-btn" tabindex="0" aria-pressed="false" onclick="metadataToggle('<%=metadataPanelLinks.ClientID%>', this)" onkeydown="handleBtnKeyDown(event, '<%=metadataPanelLinks.ClientID%>')">
                        <div class="metadata-toggle">
                            <asp:Label runat="server" ID="VariableTitleMetadata" Cssclass="variableselector_valuesselect_variabletitle metadata-text-wrap"/>        
                            <svg focusable="false" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="metadata-logo"><path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"></path><path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"></path></svg>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="metadataPanelLinks" class="metadata-popup">
                        <div class="metadata-content">
                            <asp:Repeater ID="VariableValueRepeater" runat="server" OnItemDataBound="VariableValueRepeater_ItemDataBound">
                                <ItemTemplate>
                                    <div class="metadata-row">
                                        <div class="metadata-variable">
                                            <asp:Label ID="lblVariableValueName" runat="server" CssClass="metadata-itemname"></asp:Label>
                                        </div>
                                        <div class="metadata-links">
                                            <asp:Repeater ID="VariableValueLinksRepeater" runat="server" OnItemDataBound="VariableValueLinksRepeater_ItemDataBound">
                                                <ItemTemplate>
                                                    <div id="divVarValLink" runat="server" class="pxweb-link">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <button type="button" class="metadata-close-btn" onclick="metadataToggle('<%=metadataPanelLinks.ClientID%>')">
                                <svg focusable="false" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon"><circle cx="12" cy="12" r="10"></circle><line x1="15" y1="9" x2="9" y2="15"></line><line x1="9" y1="9" x2="15" y2="15"></line></svg>
                                <asp:Label ID="MetadataCloseLabel" CssClass="metadata-close-text" runat="server"/>                               
                            </button>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <asp:Label runat="server" ID="VariableTitle" Cssclass="variableselector_valuesselect_variabletitle"/>
                <div class="mandatory_container">
                <asp:Label runat="server" ID="MandatoryText" CssClass="variableselector_valuesselect_mandatory_text" Visible="False"></asp:Label>
                <asp:Label runat="server" ID="MandatoryStar" CssClass="variableselector_valuesselect_mandatory_star" Visible="False">*</asp:Label>
                </div>
            </div>
            <asp:DropDownList runat="server" ID="GroupingDropDown" Visible="false" AutoPostBack="true" CssClass="variableselector_valuesselect_aggregations"/>
            <asp:button id="ActionButton" runat="server" Visible="false" cssclass="variableselector_valuesselect_action" /> 
            <div><asp:Label runat="server" ID="VariableTitleSecond" CssClass="variableselector_valuesselect_variabletitlesecond"/></div>
    </asp:Panel>

    <asp:Panel runat="server" ID="EventButtons" CssClass="variableselector_valuesselect_eventbutton_panel">
        <asp:ImageButton runat="server" ID="HierarchicalSelectButton" CssClass="variableselector_valuesselect_select_hierarcical_imagebutton"/>
        <asp:Button runat="server" ID="SelectAllButton" CssClass="variableselector_valuesselect_select_all_button pxweb-btn negative icon-placement variableselector-buttons"/>
        <asp:Button runat="server" ID="DeselectAllButton" CssClass="variableselector_valuesselect_deselect_all_button pxweb-btn negative icon-placement variableselector-buttons"/>

        <div class="link-buttons-container">
            <asp:LinkButton runat="server" ID="SearchButton" CssClass="pxweb-link negative with-icon variableselector-search" CausesValidation="False"></asp:LinkButton>
            <asp:LinkButton runat="server" ID="SelectionFromGroupButton" CssClass="pxweb-link negative with-icon" CausesValidation="False"></asp:LinkButton>
        </div>
        <asp:Panel runat="server" ID="SearchPanel" CssClass="variableselector_valuesselect_search_panel">
            <div class="pxweb-input search-panel">
                <div class="input-wrapper">
                    <asp:TextBox ID="SearchValuesTextbox" CssClass="with-icon" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="SearchValuesButton" CssClass="icon-wrapper search-icon" runat="server">
                        <svg focusable="false" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="search-icon"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                        <span class="hidden">wave temp fix..</span>
                    </asp:LinkButton>
                </div>
            </div>
            <asp:CheckBox runat="server" ID="SearchValuesBeginningOfWordCheckBox" CssClass="variableselector_valuesselect_search_textstart_checkbox pxweb-checkbox negative" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="HiddenEventButtons" runat="server" Visible="false" CssClass="variableselector_valuesselect_hiddeneventbutton_panel">
    </asp:Panel>
    <asp:Panel runat="server" ID="SelectedStatistics" CssClass="variableselector_valuesselect_statistics_panel">
        <hr class="box-divider-line"/>
        <p>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesSelectedTitel" /></span>
        <asp:Label runat="server" id="NumberValuesSelected" CssClass="variableselector_valuesselect_statistics"/>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotalTitel" /></span>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotal"  /></span>
        </p>
    </asp:Panel>
    <asp:Panel ID="OptionalVariablePanel" runat="server" CssClass="optional-variable-panel">
        <div class="optional-variable-icon-panel">
            <svg focusable="false" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="1 1 24 24" fill="none" stroke="#FFFFFF" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon">
            <circle cx="12" cy="12" r="10"></circle><line x1="12" y1="16" x2="12" y2="12"></line><line x1="12" y1="8" x2="12" y2="8"></line>
        </svg>
        </div>
        <asp:Label runat="server" ID="OptionalVariableText" CssClass="optional-variable-text"></asp:Label>
    </asp:Panel>
    <asp:Panel runat="server" ID="ValuesSelectPanel" CssClass="variableselector_valuesselect_valuesselect_panel">
        <asp:ListBox runat="server" ID="ValuesListBox" CssClass="variableselector_valuesselect_valueslistbox" />
    </asp:Panel>
    <asp:Panel ID="ManyValuesPanel" runat="server" CssClass="variableselector_valuesselect_manyvalues_panel">
        <asp:Literal ID="ManyValues" runat="server"></asp:Literal>
    </asp:Panel>

<%--      <asp:CustomValidator ID="MustSelectCustom" runat="server" ErrorMessage=""  CssClass="variableselector_error pxweb-input-error negative"
    ControlToValidate="ValuesListBox" ClientValidationFunction="ValidateListBox" SetFocusOnError="false" 
    OnServerValidate="ValidateListBox_ServerValidate" ForeColor=""  
    ValidateEmptyText="True"  ValidationGroup="ChangeStatus" EnableClientScript="true"  Display="Dynamic"  Enabled="false" ></asp:CustomValidator>--%>


             <asp:CustomValidator ID="MustSelectCustom" runat="server" ErrorMessage=""  CssClass="variableselector_error pxweb-input-error negative"
    ControlToValidate="ValuesListBox" SetFocusOnError="false"  Display="Dynamic"
    OnServerValidate="ValidateListBox_ServerValidate" ClientValidationFunction="ValidateListBox"  ForeColor=""  
    ValidateEmptyText="True"  ValidationGroup="ChangeStatus"></asp:CustomValidator>


</asp:Panel>




</div>

<script>
    jQuery(document).ready(function($) {
       window.onunload = function() { }; //prevent Firefox from caching the page in the Back-Forward Cache 
       var dd = jQuery("#<%=GroupingDropDown.ClientID%>");
       jQuery(dd).val(jQuery(dd).attr("data-value"));

    });

    function ValidateListBox(source, args) {
        var lb = document.getElementById(source.controltovalidate);
        var is_valid = lb.selectedIndex > -1
        if (!is_valid) {
            $(lb).addClass("variableselector_valuesselect_box_error")
        } else {
            $(lb).removeClass("variableselector_valuesselect_box_error")
        }
        if (!is_valid) {
        }
        args.IsValid = is_valid;
    }

    function metadataToggle(metadataPanelLinksId, element) {
        var metadataContainer = document.getElementById(metadataPanelLinksId.replace(/\$/gi, "_"));
        metadataContainer.classList.toggle("open");
        // Check to see if the button is pressed
        var pressed = (element.getAttribute("aria-pressed") === "true");
        // Change aria-pressed to the opposite state
        element.setAttribute("aria-pressed", !pressed);
    }

    function handleBtnKeyDown(event, metadataPanelLinksId) {
        if (event.key === " " || event.key === "Enter" || event.key === "Spacebar") { // "Spacebar" for IE11 support
            // Prevent the default action to stop scrolling when space is pressed
            event.preventDefault();
            metadataToggle(metadataPanelLinksId, event.target);
        }
    }

</script>
