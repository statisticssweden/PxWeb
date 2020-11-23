<%@ control  inherits="PCAxis.Web.Controls.SelectFromGroupCodebehind" %>

<div class="pxbox negative selectfromgroup-container flex-column">
    <h2>
        <asp:Label ID="lblHeading" runat="server" CssClass="pxweb-title negative"></asp:Label>
    </h2>

    <div id="pxcontent">
      <asp:Panel ID="UserManualGroupingRegion" role="region" runat="server">
        <asp:Panel ID="UserManualGrouping"  CssClass="screenreader-only" runat="server" />
      </asp:Panel>
    </div>

    <asp:Panel ID="GroupingRegion" role="region" runat="server">
        <h3>
            <asp:Label ID="lblGrouping" runat="server" CssClass="pxweb-title negative"></asp:Label>
        </h3>
         <asp:DropDownList ID="cboGrouping" runat="server" AutoPostBack="true" CssClass="selectfromgroup-selectgrouping"></asp:DropDownList>
    </asp:Panel>

    <asp:Panel ID="GroupRegion" role="region" runat="server">
      <asp:Panel ID="pnlGroups" runat="server">
        <h3>
        <asp:Label ID="lblGroups" CssClass="pxweb-title negative" runat="server"></asp:Label>
        </h3>
        <asp:ListBox ID="lstGroups" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup-groupslist"></asp:ListBox>
      </asp:Panel>
      <asp:Panel ID="pnlForRbl" CssClass="selectfromgroup-radiobuttons" runat="server">
        <asp:RadioButtonList ID="rblType" AutoPostBack="true" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="selectfromgroup-selecttype">
        </asp:RadioButtonList>
      </asp:Panel>
      <asp:Button ID="btnSelectGroups" runat="server" CssClass="selectfromgroup-groupsbutton align-self-flex-start pxweb-buttons pxweb-btn negative icon-placement" OnClick="btnSelectGroups_Click" />
    </asp:Panel>

     <asp:Panel ID="ValuesRegion" role="region" runat="server">
       <asp:Panel ID="pnlValues" runat="server" CssClass="selectfromgroup-valuespanel">
        <h3> <asp:Label ID="lblValues"  CssClass="pxweb-title negative" runat="server"></asp:Label> </h3>
        <asp:ListBox ID="lstValues" SelectionMode="Multiple" runat="server" CssClass="selectfromgroup-valueslist"></asp:ListBox>
       </asp:Panel>
     </asp:Panel>

    <asp:Button ID="btnCancel" runat="server" CssClass="selectfromgroup-cancel pxweb-buttons align-self-flex-end pxweb-btn negative icon-placement" />
</div>
<div class="selectfromgroup-button-container flex-row flex-wrap align-center justify-center">
    <asp:Button ID="btnSelectionDone" runat="server" CssClass="pxweb-btn primary-btn" Enabled="false" />
</div>

