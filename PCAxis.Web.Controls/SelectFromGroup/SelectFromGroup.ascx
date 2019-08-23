<%@ control  inherits="PCAxis.Web.Controls.SelectFromGroupCodebehind" %>

<div class="selectfromgroup_container">
    <div class="selectfromgroup_left">
        <div>
            <asp:Label ID="lblHeading" runat="server" CssClass="selectfromgroup_heading"></asp:Label>
            <asp:Label ID="lblVariable" runat="server" CssClass="selectfromgroup_variable"></asp:Label>
            <asp:DropDownList ID="cboGrouping" runat="server" AutoPostBack="true" CssClass="selectfromgroup_selectgrouping"></asp:DropDownList>
        </div>
    
        <asp:Panel ID="pnlGroups" runat="server" CssClass="selectfromgroup_groupspanel">
            <asp:Label ID="lblGroups" runat="server" CssClass="selectfromgroup_groupsheading"></asp:Label>
            <asp:ListBox ID="lstGroups" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup_groupslist"></asp:ListBox>
            <asp:Button ID="btnSelectGroups" runat="server" CssClass="selectfromgroup_groupsbutton" OnClick="btnSelectGroups_Click" />
        </asp:Panel>
    
        <asp:Panel ID="pnlValues" runat="server" CssClass="selectfromgroup_valuespanel">
            <asp:Label ID="lblValues" runat="server" CssClass="selectfromgroup_valuesheading"></asp:Label>
            <asp:ListBox ID="lstValues" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup_valueslist"></asp:ListBox>
        </asp:Panel>
    
        <asp:Button ID="btnSelectionDone" runat="server" CssClass="selectfromgroup_returnbutton" Enabled="false" />
        <asp:Button ID="btnCancel" runat="server" CssClass="selectfromgroup_cancel" />
    </div>
    <div class="selectfromgroup_right">
        <asp:RadioButtonList ID="rblType" AutoPostBack="true" runat="server" RepeatDirection="Vertical" CssClass="selectfromgroup_selecttype">
        </asp:RadioButtonList>
    </div>
</div>

