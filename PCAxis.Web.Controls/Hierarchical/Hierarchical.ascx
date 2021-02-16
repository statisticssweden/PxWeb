<%@ control inherits="PCAxis.Web.Controls.HierarchicalCodebehind" %>
<asp:Panel runat="server" ID="ErrorPanel" Visible="false">
    <asp:Label runat="server" ID="ErrorMessage"></asp:Label>
</asp:Panel>

<asp:Panel runat="server" ID="HierarchicalVariableSelectPanel">
    <div class="hierarchical_selectionbuttonregion">
        <asp:Label runat="server" ID="SelectAllLabel" CssClass="hierarchical_selectionlabel"/>
        <asp:ImageButton runat="server" ID="SelectAllButton" CssClass="hierarchical_selectall_button"/>

        <asp:Label runat="server" ID="UnselectAllLabel" CssClass="hierarchical_selectionlabel"/>
        <asp:ImageButton runat="server" ID="UnSelectAllButton" CssClass="hierarchical_unselectall_button"/>

        <asp:Label runat="server" ID="OpenAllNodesLabel" CssClass="hierarchical_selectionlabel"/>
        <asp:ImageButton runat="server" ID="OpenAllNodesButton" CssClass="hierarchical_openall_button"/>

        <asp:Label runat="server" ID="CloseAllNodesLabel" CssClass="hierarchical_selectionlabel"/>
        <asp:ImageButton runat="server" ID="CloseAllNodesButton" CssClass="hierarchical_closeall_button"/>
    </div>

    <p><asp:Label runat="server" ID="VariableNameLabel" CssClass="hierarchical_variabletitle"/></p>
    <asp:treeview id="VariableTreeView" runat="server" ShowCheckBoxes="All" CssClass="hierarchical_treeview" />
    <asp:Button runat="server" ID="ContinueButton" CssClass="hierarchical_continuebutton"  />
</asp:Panel>

        <asp:panel runat="server" id="CollapseTreeScriptPanel" >
                <script>                
                    //Level is registered from codebehind
                    jQuery(document).ready(function() {
                        if (level > 0) {
                            CollapseChildren(level);
                        } else {
                            CollapseTreeView();
                        }
                    });
                </script>
        </asp:panel>
