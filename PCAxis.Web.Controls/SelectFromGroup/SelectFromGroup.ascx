<%@ control  inherits="PCAxis.Web.Controls.SelectFromGroupCodebehind" %>

<div class="selectfromgroup-container">
    <h2>
        <asp:Label ID="lblHeading" runat="server" CssClass="pxweb-title negative"></asp:Label>
    </h2>
    <div class="selectfromgroup-variable-container">
        <h3>
            <asp:Label ID="lblVariableDesc" runat="server" CssClass="pxweb-title negative"></asp:Label>
        </h3>
        <h3>
            <asp:Label ID="lblVariable" runat="server" CssClass="selectfromgroup-variable pxweb-title negative"></asp:Label>
        </h3>
    </div>
    <asp:DropDownList ID="cboGrouping" runat="server" AutoPostBack="true" CssClass="selectfromgroup-selectgrouping"></asp:DropDownList>
    <asp:Panel ID="pnlGroups" runat="server">
        <asp:Label ID="lblGroups" runat="server"></asp:Label>
        <asp:ListBox ID="lstGroups" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup-groupslist"></asp:ListBox>
    </asp:Panel>
    <asp:Panel ID="pnlForRbl" CssClass="selectfromgroup-radiobuttons" runat="server">
        <asp:RadioButtonList ID="rblType" AutoPostBack="true" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="selectfromgroup-selecttype">
        </asp:RadioButtonList>
    </asp:Panel>
    <asp:Button ID="btnSelectGroups" runat="server" CssClass="selectfromgroup-groupsbutton pxweb-btn negative icon-placement" OnClick="btnSelectGroups_Click" />
    <asp:Panel ID="pnlValues" runat="server" CssClass="selectfromgroup-valuespanel">
        <asp:Label ID="lblValues" runat="server"></asp:Label>
        <asp:ListBox ID="lstValues" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup-valueslist"></asp:ListBox>
    </asp:Panel>
    <asp:Button ID="btnCancel" runat="server" CssClass="selectfromgroup-cancel pxweb-btn negative icon-placement" />
</div>
<div class="selectfromgroup-button-container">
    <asp:Button ID="btnSelectionDone" runat="server" CssClass="pxweb-btn primary-btn" Enabled="false" />
</div>

