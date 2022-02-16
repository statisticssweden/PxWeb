<%@ control  inherits="PCAxis.Web.Controls.CommandBar.CommandBarCodebehind" %>

<asp:Panel runat="server" ID="CommandBarPanel">
    <asp:Panel ID="AccordionPanel" CssClass="accordion-panels" runat="server">
        <asp:panel class="pxweb-accordion" id="ShowResultAsPanel" ClientIDMode="Static" runat="server">
            <asp:Label runat="server" CssClass="screenreader-only" ID="emptyLabel" aria-hidden="true" ClientIDMode="Static"></asp:Label>
            <button type="button" runat="server" class="accordion-header closed" id="ShowResultAsHeader" ClientIDMode="Static" aria-expanded="false" onclick="accordionToggle(ShowResultAsPanel, this)" >
                <span class="header-text"><asp:Label ID="ShowResultLabel"  runat="server"></asp:Label></span>
            </button>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblLegendShow" runat="server" CssClass="screenreader-only"></asp:Label>
                    </legend>
                    <asp:panel runat="server" class="accordion-body closed" ID="ShowResultAsBody" ClientIDMode="Static">
                        <asp:RadioButtonList id="ShowAsRadioButtonList" runat="server" RepeatLayout="Flow" CssClass="commandbar_link_dropdownlist" ></asp:RadioButtonList>
                        <asp:Button ID="ShowAsBtn" runat="server" aria-describedby="emptyLabel" CssClass="pxweb-btn primary-btn"/>
                    </asp:panel>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:panel class="pxweb-accordion" ClientIDMode="Static" id="OperationsPanel" runat="server">
            <button type="button" runat="server" class="accordion-header closed" ClientIDMode="Static" id="OperationsHeaderButton" aria-expanded="false" onclick="accordionToggle(OperationsPanel, this)" >
                <span class="header-text"><asp:Label ID="OperationsLabel"  runat="server"></asp:Label></span>
            </button>
            <asp:panel runat="server" class="accordion-body closed" ClientIDMode="Static" id="OptionsBody">
                <asp:panel cssclass="operations-container flex-column" id="OperationsButtonsPanel" ClientIDMode="Static" runat="server"></asp:panel>
            </asp:panel>        
            <asp:panel id="PluginControlHolder" cssclass="commandbar_container" runat="server" visible="false"></asp:panel>
        </asp:Panel>
        <asp:panel class="pxweb-accordion"  ClientIDMode="Static" id="SaveAsPanel" runat="server">
            <button type="button"  runat="server" class="accordion-header closed" ClientIDMode="Static" id="SaveAsHeaderButton" aria-expanded="false" onclick="accordionToggle(SaveAsPanel, this)" >
                <span class="header-text"><asp:Label ID="SaveAsLabel"  runat="server"></asp:Label></span>
            </button>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblLegendSave" runat="server" CssClass="screenreader-only"></asp:Label>
                    </legend>
                    <div class="accordion-body closed" id="SaveAsBody">
                        <asp:RadioButtonList id="SaveAsRadioButtonList" runat="server" RepeatLayout="Flow" CssClass="commandbar_saveas_dropdownlist"></asp:RadioButtonList>
                        <asp:Button ID="SaveAsBtn" runat="server" aria-describedby="emptyLabel" CssClass="pxweb-btn primary-btn"/>
                    </div>
                </fieldset>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="ShortcutButtonPanel" CssClass="flex-row flex-wrap"></asp:Panel>
    <asp:Panel ID="SaveFilePanel" runat="server" CssClass="commandbar_savefilelink">
        <asp:HyperLink ID="SaveFileLink" runat="server" Target="_blank"></asp:HyperLink>
    </asp:Panel>
    <asp:HiddenField runat="server" ID="PluginButtonUsed" ClientIDMode="Static"/>
    <asp:HiddenField runat="server" ID="AccordionState" ClientIDMode="Static"/>
</asp:Panel>

<script>
    jQuery(document).ready(function () {

        var ariaLabelBaseShowAs = jQuery("#<%=ShowAsBtn.ClientID%>").attr("aria-label");
        var ariaLabelBaseSaveAs = jQuery("#<%=SaveAsBtn.ClientID%>").attr("aria-label");

        if (jQuery("#<%=ShowAsRadioButtonList.ClientID%>").length !== 0) {
            setOnLoadRadioLabelForButton(<%=ShowAsRadioButtonList.ClientID%>, <%=ShowAsBtn.ClientID%>, ariaLabelBaseShowAs);

            jQuery("#<%=ShowAsRadioButtonList.ClientID%>").find("input[type='radio']").change(function () {
                setUpdatedRadioLabelForButton(this, <%=ShowAsBtn.ClientID%>, ariaLabelBaseShowAs);
            });
        }

        if (jQuery("#<%=SaveAsRadioButtonList.ClientID%>").length !== 0) {
            setOnLoadRadioLabelForButton(<%=SaveAsRadioButtonList.ClientID%>, <%=SaveAsBtn.ClientID%>, ariaLabelBaseSaveAs)

            jQuery("#<%=SaveAsRadioButtonList.ClientID%>").find("input[type='radio']").change(function () {
                setUpdatedRadioLabelForButton(this, <%=SaveAsBtn.ClientID%>, ariaLabelBaseSaveAs);
            });
        }
    });
</script>