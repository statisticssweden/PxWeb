<%@ control  inherits="PCAxis.Web.Controls.InformationCodebehind" %>

<asp:Repeater ID="InformationRepeater" runat="server" enableviewstate="False">
    <HeaderTemplate>
     <%--   <h2 class="information_definitionlist_heading"><asp:Literal ID="Header" runat="server" /></h2>--%>
        <dl class="information_definitionlist">
    </HeaderTemplate>
    <ItemTemplate>
        <dt class="information_<%#DataBinder.Eval(Container.DataItem, "InformationType").ToString().ToLower()%>_heading">
            <asp:Literal ID="MainTerm" runat="server" />            
        </dt>
        <dd class="information_<%#DataBinder.Eval(Container.DataItem, "InformationType").ToString().ToLower()%>_value">
            <asp:Literal ID="MainDefinition" runat="server" />            
            <asp:Repeater ID="VariableRepeater" runat="server" enableviewstate="False">
                <HeaderTemplate>
                    <dl>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="information_atom">
                      <dt><asp:Literal ID="VariableTerm" runat="server" /></dt>
                      <dd><asp:Literal ID="VariableDefinition" runat="server" /></dd>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </dl>
                </FooterTemplate>
            </asp:Repeater>
        </dd>

    </ItemTemplate>
    <FooterTemplate>
        </dl>
    </FooterTemplate>
</asp:Repeater>


