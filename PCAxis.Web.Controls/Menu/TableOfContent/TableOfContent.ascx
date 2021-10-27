<%@ control inherits="PCAxis.Web.Controls.TableOfContentCodebehind" %>

<asp:Panel runat="server" ID="TableOfContentPanel" EnableViewState="true">
    <div id="TableOfContent">
        <asp:button id="ActionButton" runat="server" cssclass="tableofcontent_action" />
        <asp:Panel runat="server" ID="MenuTreeviewPanel">
            <asp:TreeView ID="MenuNavigationTree" runat="server" NodeWrap="false" EnableClientScript="true" PopulateNodesFromClient="true" ExpandDepth="1" OnTreeNodePopulate="PopulateNode" >
                <NodeStyle CssClass="tableofcontent_treenode"  />
                <SelectedNodeStyle CssClass="tableofcontent_selectedtreenode" />
                <RootNodeStyle CssClass="tableofcontent_roottreenode" />
            </asp:TreeView>
        </asp:Panel>

        <asp:Panel runat="server" ID="MenulistPanel">
            <asp:Repeater ID="MenuItemList" runat="server">
                <HeaderTemplate><ul class="tableofcontent_ul"></HeaderTemplate>
                <ItemTemplate><li class="tableofcontent_li_menuitem"><a href="<%#DataBinder.Eval(Container.DataItem, "LinkURL")%>" class="tableofcontent_menuitem"><%#DataBinder.Eval(Container.DataItem, "Text")%></a></li></ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
            
            <asp:Repeater ID="LinkItemList" runat="server">
                <HeaderTemplate><ul class="tableofcontent_ul"></HeaderTemplate>
                <ItemTemplate><li class="tableofcontent_li_linkitem"><a href="<%#DataBinder.Eval(Container.DataItem, "LinkURL")%>" class="tableofcontent_linkitem"><%#DataBinder.Eval(Container.DataItem, "Text")%></a></li></ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>
</asp:Panel>

<script>

    jQuery(document).ready(function () {
        var offset = jQuery('.AspNet-TreeView-Collapse.first').offset();
        if (offset) {
            jQuery('html, body').scrollTop(offset.top);
        }
        jQuery('a[rel="_blank"]').each(function () {
            jQuery(this).attr('target', '_blank');
        });
    });

</script>
