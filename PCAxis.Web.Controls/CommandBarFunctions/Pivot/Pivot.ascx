<%@ control  inherits="PCAxis.Web.Controls.PivotCodebehind" %>
<div class="flex-column">
    <h3 class="container_titletext">
        <asp:Literal runat="server" Text="<%$ PxString: CtrlCommandBarFunctionPivotManualTitle%>" />
    </h3>
    <div class="flex-row s-margin-top">
        <div class="commandbar-listbox-container">
            <asp:label id="StubLabel" runat="server" cssclass="commandbar_pivot_title font-heading" /><br />
            <asp:listbox  id="StubListBox" runat="server" cssclass="commandbar_pivot_listbox" selectionmode="Multiple" />
                <div class="flex-column">
                    <asp:button id="MoveRightButton" runat="server" alternatetext="move right" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-right no-margin-right" />
                    <div class="flex-row">
                        <asp:button id="StubUpButton" runat="server" alternatetext="move up" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-up" />
                        <asp:button id="StubDownButton" runat="server" alternatetext="move down" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-down no-margin-right" />
                    </div>
                </div>
        </div>
        <div class="commandbar-listbox-container m-margin-left">
            <asp:label id="HeadingLabel" runat="server" cssclass="commandbar_pivot_title font-heading" /><br />
            <asp:listbox id="HeadingListBox" runat="server"  cssclass="commandbar_pivot_listbox"  selectionmode="Multiple" />
                <div class="flex-column">
                    <asp:button id="MoveLeftButton" runat="server" alternatetext="move left" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-left no-margin-right" />
                    <div class="flex-row">
                        <asp:button id="HeadingUpButton" runat="server" alternatetext="move up" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-up" />
                        <asp:button id="HeadingDownButton" runat="server" alternatetext="move down" cssclass="pxweb-btn icon-placement pxweb-buttons arrow-down no-margin-right" />
                    </div>
                </div>
        </div>
    </div>
    <div class="container_exit_buttons_row">
        <asp:Button ID="CancelButton" runat="server" CssClass="pxweb-btn pxweb-buttons" />
        <asp:button id="ContinueButton" runat="server" CssClass="pxweb-btn primary-btn pxweb-buttons" />
    </div>
</div>
