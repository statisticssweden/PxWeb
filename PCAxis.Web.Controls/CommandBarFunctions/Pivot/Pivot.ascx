<%@ control  inherits="PCAxis.Web.Controls.PivotCodebehind" %>
<div>
    <p>
        <asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_pivot_heading" />
    </p>
</div>
<div>
    <div class="commandbar_pivot_container_buttons">
        <asp:imagebutton id="StubUpButton" runat="server" alternatetext="move up" cssclass="commandbar_pivot_button" /><br />
        <asp:imagebutton id="StubDownButton" runat="server" alternatetext="move down" cssclass="commandbar_pivot_button" />
    </div>
    <div class="commandbar_pivot_container">
        <asp:label id="StubLabel" runat="server" cssclass="commandbar_pivot_title" /><br />
        <asp:listbox id="StubListBox" runat="server"  cssclass="commandbar_pivot_listbox" selectionmode="Multiple" />
    </div>
    <div class="commandbar_pivot_container_buttons">
        <asp:imagebutton id="MoveLeftButton" runat="server" alternatetext="move left" cssclass="commandbar_pivot_button" /><br />
        <asp:imagebutton id="MoveRightButton" runat="server" alternatetext="move right" cssclass="commandbar_pivot_button" />
    </div>
    <div class="commandbar_pivot_container">
        <asp:label id="HeadingLabel" runat="server" cssclass="commandbar_pivot_title" /><br />
        <asp:listbox id="HeadingListBox" runat="server" cssclass="commandbar_pivot_listbox"  selectionmode="Multiple" />
    </div>
    <div class="commandbar_pivot_container_buttons">
        <asp:imagebutton id="HeadingUpButton" runat="server" alternatetext="move up" cssclass="commandbar_pivot_button" /><br />
        <asp:imagebutton id="HeadingDownButton" runat="server" alternatetext="move down"
            cssclass="commandbar_pivot_button" />
    </div>
</div>
<div class="commandbar_button_row">
    <asp:button id="ContinueButton" runat="server" CssClass="commandbar_continuebutton pxweb-btn primary-btn" />
    <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton pxweb-btn primary-btn" />
</div>
