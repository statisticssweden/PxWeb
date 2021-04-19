<%@ control  inherits="PCAxis.Web.Controls.InformationCodebehind" %>

<asp:Repeater ID="InformationRepeater" runat="server" enableviewstate="False">
    <HeaderTemplate>
        <div class="dl information_definitionlist font-normal-text">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="NestedAccordionStart" runat="server" />
        <div class="dd information_<%#DataBinder.Eval(Container.DataItem, "InformationType").ToString().ToLower()%>_value">
            <asp:Literal ID="MainDefinition" runat="server" />            
            <asp:Repeater ID="VariableRepeater" runat="server" enableviewstate="False">
                <HeaderTemplate>
                    <dl>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="flex-column">
                      <dt class="font-heading"><asp:Literal ID="VariableTerm" runat="server" /></dt>
                      <dd><asp:Literal ID="VariableDefinition" runat="server" /></dd>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </dl>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <asp:Literal ID="NestedAccordionEnd" runat="server" />
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>


