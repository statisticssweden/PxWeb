<%@ control  inherits="PCAxis.Web.Controls.CommandBar.CommandBarCodebehind" %>

<asp:Panel runat="server" ID="CommandBarPanel">
    <div class="commandbar">
        <asp:panel id="DropDownPanel" runat="server" cssclass="commandbar_dropdown">
            <div>
                <asp:dropdownlist id="FunctionDropDownList" runat="server" autopostback="True" CssClass="commandbar_function_dropdownlist" ></asp:dropdownlist> 
                <asp:panel cssclass="commandbar_shortcut" id="FunctionDropDownListShortcuts" runat="server"></asp:panel>
            </div>
            <div>
                <asp:dropdownlist id="SaveAsDropDownList" runat="server" CssClass="commandbar_saveas_dropdownlist"></asp:dropdownlist>    
                <asp:panel cssclass="commandbar_shortcut" id="SaveAsDropDownListShortcuts" runat="server"></asp:panel>
            </div>
            <div>
                <asp:dropdownlist id="PresentationViewsDropDownList" runat="server" autopostback="True" CssClass="commandbar_link_dropdownlist" ></asp:dropdownlist>
                <asp:panel cssclass="commandbar_shortcut" id="LinkDropDownListShortcuts" runat="server"></asp:panel>
            </div>
                <asp:panel id="CommandBarShortcutsDropDown" runat="server" cssclass="commandbar_mainshortcut"></asp:panel>
                <asp:button id="ActionButton" runat="server" cssclass="commandbar_action" />    
        </asp:panel>
        <asp:panel id="ButtonPanel" runat="server" cssclass="commandbar_images"></asp:panel>
        <asp:panel id="CommandBarShortcutsImage" runat="server" cssclass="commandbar_mainshortcut"></asp:panel>
        <asp:panel id="PluginControlHolder" cssclass="commandbar_container" runat="server" visible="false"></asp:panel>
        <asp:Panel ID="SaveFilePanel" runat="server" CssClass="commandbar_savefilelink">
            <asp:HyperLink ID="SaveFileLink" runat="server" Target="_blank"></asp:HyperLink>
        </asp:Panel>
    </div>
    
    <div id="commandbarDownloadFileDialog" class="commandbar_download_file_dialog" style="display: none" runat="server">
        <asp:Panel ID="commandbarDownloadFileContainer" CssClass="commandbar_download_file_container" runat="server">
            <asp:Label ID="commandbarDownloadFileInformation" CssClass="commandbar_download_file_information" runat="server"></asp:Label>
            <asp:HyperLink ID="commandbarDownloadFileLink" CssClass="commandbar_download_file_link" runat="server" Target="_self"></asp:HyperLink>
        </asp:Panel>
    </div>

</asp:Panel>
