<%@ control inherits="PCAxis.Web.Controls.ChangeValueOrderCodebehind" %>
<div class="flex-column">
    <h3 class="container_titletext">
        <asp:Literal runat="server" Text="<%$ PxString: CtrlCommandBarFunctionChangeValueOrder%>" />
    </h3>
    <asp:Panel ID="ChooseVariablePanel" CssClass="s-margin-top" runat="server">
        <asp:RadioButtonList ID="VariableList" CssClass="commandbar_changevalueorder_variablelist" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </asp:Panel>
    <asp:Panel ID="ChangeOrderPanel" CssClass="flex-colunm commandbar_changevalueorder_container" runat="server" Visible="false" >
        <div class="commandbar_changevalueorder_instructions">
            <asp:Literal ID="Instructions" runat="server"></asp:Literal>
        </div>
        <div class="flex-column commandbar-listbox-container">
            <asp:Label runat="server" ID="ListBoxLabel" CssClass="font-heading" AssociatedControlID="ValuesOrder"></asp:Label>
            <asp:ListBox ID="ValuesOrder" runat="server" CssClass="commandbar_changevalueorder_listbox" ></asp:ListBox>
            <div class="flex-row justify-center ">
                <asp:Button id="MoveUpButton" runat="server" alternatetext="move up" CssClass="pxweb-btn icon-placement pxweb-buttons arrow-up" />
                <asp:Button id="MoveDownButton" runat="server" alternatetext="move down" CssClass="pxweb-btn icon-placement pxweb-buttons arrow-down no-margin-right" />
            </div>
        </div>
    </asp:Panel>
    <div class="container_exit_buttons_row">
        <asp:Button ID="CancelButton" CssClass="pxweb-btn pxweb-buttons" runat="server" />
        <asp:Button ID="ContinueButton" CssClass="pxweb-btn primary-btn pxweb-buttons container_continuebutton" runat="server" />
    </div>
</div>