<%@ control  inherits="PCAxis.Web.Controls.VariableSelectorValueSelectCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>


<div class="pxbox negative variableselector_valuesselect_box m-margin-top">

<asp:Panel runat="server" ID="ValuesSelectContainerPanel" DefaultButton="SearchValuesButton" role="region">
    <asp:Panel runat="server" ID="Panel1" CssClass="variableselector_valuesselect_variabletitle_panel">
            <div class="flex-row flex-wrap-reverse">
                <asp:Panel runat="server" ID="MetadataPanel" class="metadata-container" Visible="False">
                    <div role="button" class="metadata-open-btn" aria-label="<%= AriaLabelMetadata %>" tabindex="0" aria-pressed="false" aria-expanded="false" onclick="metadataToggle('<%=metadataPanelLinks.ClientID%>', this)" onkeydown="handleBtnKeyDown(event, '<%=metadataPanelLinks.ClientID%>')">
                        <div class="metadata-toggle">
                            <asp:Label runat="server" ID="VariableTitleMetadata" Cssclass="variableselector_valuesselect_variabletitle metadata-text-wrap"/>
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
                                                    <div id="divVarValLink" runat="server" class="pxweb-link negative with-icon">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <button type="button" class="metadata-close-btn" onclick="metadataToggle('<%=metadataPanelLinks.ClientID%>')">
                                <asp:Label ID="MetadataCloseLabel" CssClass="metadata-close-text" runat="server"/>                               
                            </button>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="VariableTitlePanel" CssClass="variableselector_valuesselect_variabletitle">
                    <asp:Label runat="server" ID="VariableTitle" AssociatedControlID="ValuesListBox" />
                    <asp:Label runat="server" ID="MandatoryText" CssClass="variableselector_valuesselect_mandatory_text" Visible="False"></asp:Label>
                    <!--<asp:Label runat="server" ID="MandatoryStar" CssClass="variableselector_valuesselect_mandatory_star" Visible="False"></asp:Label>-->
                </asp:Panel>
            </div>
            <asp:DropDownList runat="server" ID="GroupingDropDown" Visible="false" AutoPostBack="true" CssClass="variableselector_valuesselect_aggregations s-margin-top" onchange="Remove_BlockSubmit()" />
            <asp:button id="ActionButton" runat="server" Visible="false" cssclass="variableselector_valuesselect_action" /> 
            <div><asp:Label runat="server" ID="VariableTitleSecond" CssClass="variableselector_valuesselect_variabletitlesecond"/></div>
    </asp:Panel>

    <asp:Panel runat="server" ID="EventButtons" CssClass="flex-row flex-wrap align-center">
        <asp:ImageButton runat="server" ID="HierarchicalSelectButton" CssClass="variableselector_valuesselect_select_hierarcical_imagebutton"/>
        <asp:Button runat="server" ID="SelectAllButton" CssClass="variableselector_valuesselect_select_all_button pxweb-btn negative icon-placement variableselector-buttons" CausesValidation="true"/>
        <asp:Button runat="server" ID="DeselectAllButton" CssClass="variableselector_valuesselect_deselect_all_button pxweb-btn negative icon-placement variableselector-buttons" CausesValidation="true"/>

        <div class="link-buttons-container flex-column s-margin-top">
            <div class="pxweb-link negative with-icon">
                 <asp:LinkButton runat="server" ID="SearchButton" CssClass="arrow-right-pxbox-icon go-to-advanced-search" CausesValidation="False" OnClientClick="Remove_BlockSubmit()"></asp:LinkButton>
            </div>
            <div class="pxweb-link negative with-icon"> 
               <asp:LinkButton runat="server" ID="SelectionFromGroupButton" CssClass="arrow-right-pxbox-icon" CausesValidation="False" OnClientClick="Remove_BlockSubmit()"></asp:LinkButton>
            </div>
        </div>
        <asp:Panel runat="server" ID="SearchPanel" CssClass="flex-row flex-wrap s-margin-top">
          <asp:CheckBox runat="server" ID="SearchValuesBeginningOfWordCheckBox" CssClass="variableselector_valuesselect_search_textstart_checkbox pxweb-checkbox negative" />
            <div class="pxweb-input search-panel">
                <asp:Label runat="server" ID="SearchTip" CssClass="screenreader-only"></asp:Label>
                <div class="input-wrapper">
                    <asp:TextBox ID="SearchValuesTextbox" CssClass="with-icon" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="SearchValuesButton" CssClass="icon-wrapper search-icon" runat="server">
                        <span class="hidden">wave temp fix..</span>
                    </asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="HiddenEventButtons" runat="server" Visible="false" CssClass="variableselector_valuesselect_hiddeneventbutton_panel">
    </asp:Panel>
    <!--<hr class="pxweb-divider type-light with-margin"/>-->
    <asp:Panel runat="server" ID="SelectedStatistics" CssClass="variableselector_valuesselect_statistics_panel">
        <div role="region" id="SelectedStatisticsynotifyscreenreader" aria-live="polite" aria-atomic="true">
            <p>
            <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesSelectedTitel" /></span>
            <asp:Label runat="server" id="NumberValuesSelected" CssClass="variableselector_valuesselect_statistics"/>
            <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotalTitel" /></span>
            <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotal"  /></span>
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="OptionalVariablePanel" runat="server" CssClass="optional-variable-panel flex-row align-center">
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
    <asp:Panel ID="ManyValuesPanel" runat="server" CssClass="variableselector_valuesselect_manyvalues_panel s-margin-top">
        <asp:Literal ID="ManyValues" runat="server"></asp:Literal>
    </asp:Panel>

<%--      <asp:CustomValidator ID="MustSelectCustom" runat="server" ErrorMessage=""  CssClass="variableselector_error pxweb-input-error negative"
    ControlToValidate="ValuesListBox" ClientValidationFunction="ValidateListBox" SetFocusOnError="false" 
    OnServerValidate="ValidateListBox_ServerValidate" ForeColor=""  
    ValidateEmptyText="True"  ValidationGroup="ChangeStatus" EnableClientScript="true"  Display="Dynamic"  Enabled="false" ></asp:CustomValidator>--%>

    <div role="region" id="errornotifyscreenreader" aria-live="assertive" aria-atomic="true">
        <asp:CustomValidator ID="MustSelectCustom" runat="server" ErrorMessage="" Role="alert" CssClass="flex-row pxweb-input-error negative"
        ControlToValidate="ValuesListBox" SetFocusOnError="false"  Display="Dynamic"
        OnServerValidate="ValidateListBox_ServerValidate" ClientValidationFunction="ValidateListBox"  ForeColor=""  
        ValidateEmptyText="True"></asp:CustomValidator>
    </div>

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
            jQuery(lb).addClass("variableselector_valuesselect_box_error")
        } else {
            jQuery(lb).removeClass("variableselector_valuesselect_box_error")
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
        var expanded = (element.getAttribute("aria-expanded") === "true");
        // Change aria-pressed/aria-expanded to the opposite state
        element.setAttribute("aria-pressed", !pressed);
        element.setAttribute("aria-expanded", !expanded);
    }

    function handleBtnKeyDown(event, metadataPanelLinksId) {
        if (event.key === " " || event.key === "Enter" || event.key === "Spacebar") { // "Spacebar" for IE11 support
            // Prevent the default action to stop scrolling when space is pressed
            event.preventDefault();
            metadataToggle(metadataPanelLinksId, event.target);
        }
    }
    //remove blockSubmit when change Valueset/group after validationerror
    function Remove_BlockSubmit() {
        Page_BlockSubmit = false;
    }

    // override standard asp funtion
    function ValidatorUpdateDisplay(val) {
        if (typeof (val.display) == "string") {
            if (val.display == "None") {
                return;
            }
            if (val.display == "Dynamic") {
                val.style.display = val.isvalid ? "none" : "flex";
                return;
            }
        }
        if ((navigator.userAgent.indexOf("Mac") > -1) &&
            (navigator.userAgent.indexOf("MSIE") > -1)) {
            val.style.display = "inline";
        }
        val.style.visibility = val.isvalid ? "hidden" : "visible";
    }

</script>
