<%@ control  inherits="PCAxis.Web.Controls.CommandBar.CommandBarCodebehind" %>

<asp:Panel runat="server" ID="CommandBarPanel">
    <asp:Panel ID="AccordionPanel" CssClass="accordion-panels" runat="server">
        <asp:panel class="pxweb-accordion" id="ShowResultAsPanel" ClientIDMode="Static" runat="server">
            <fieldset>
                <legend>
                    <button type="button" runat="server" class="accordion-header closed" id="ShowResultAsHeader" ClientIDMode="Static" onclick="accordionToggle(ShowResultAsPanel, this)" >
                        <span class="header-text"><asp:Label ID="ShowResultLabel"  runat="server"></asp:Label></span>
                    </button>
                </legend>
                <asp:panel runat="server" class="accordion-body closed" ID="ShowResultAsBody" ClientIDMode="Static">
                    <asp:RadioButtonList id="ShowAsRadioButtonList" runat="server" RepeatLayout="Flow" autopostback="True" CssClass="commandbar_link_dropdownlist" ></asp:RadioButtonList>
                </asp:panel>
            </fieldset>
        </asp:Panel>
        <asp:panel class="pxweb-accordion" ClientIDMode="Static" id="OperationsPanel" runat="server">
            <button type="button" runat="server" class="accordion-header closed" ClientIDMode="Static" id="OperationsHeaderButton" onclick="accordionToggle(OperationsPanel, this)" >
                <span class="header-text"><asp:Label ID="OperationsLabel"  runat="server"></asp:Label></span>
            </button>
            <asp:panel runat="server" class="accordion-body closed" ClientIDMode="Static" id="OptionsBody">
                <asp:panel cssclass="operations-container flex-column" id="OperationsButtonsPanel" ClientIDMode="Static" runat="server"></asp:panel>
            </asp:panel>        
        </asp:Panel>
        <asp:panel class="pxweb-accordion"  ClientIDMode="Static" id="SaveAsPanel" runat="server">
            <fieldset>
                <legend>
                    <button type="button"  runat="server" class="accordion-header closed" ClientIDMode="Static" id="SaveAsHeaderButton" onclick="accordionToggle(SaveAsPanel, this)" >
                        <span class="header-text"><asp:Label ID="SaveAsLabel"  runat="server"></asp:Label></span>
                    </button>
                </legend>
                <div class="accordion-body closed" id="SaveAsBody">
                    <asp:RadioButtonList id="SaveAsRadioButtonList" runat="server" RepeatLayout="Flow" CssClass="commandbar_saveas_dropdownlist"></asp:RadioButtonList>
                    <asp:Button ID="SaveAsBtn" runat="server" CssClass="pxweb-btn primary-btn"/>
                </div>
            </fieldset>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="ShortcutButtonPanel" CssClass="flex-row flex-wrap"></asp:Panel>
    <asp:Panel ID="SaveFilePanel" runat="server" CssClass="commandbar_savefilelink">
        <asp:HyperLink ID="SaveFileLink" runat="server" Target="_blank"></asp:HyperLink>
    </asp:Panel>
    <asp:panel id="PluginControlHolder" cssclass="commandbar_container" runat="server" visible="false"></asp:panel>

</asp:Panel>
