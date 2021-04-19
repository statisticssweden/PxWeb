<%@ control  inherits="PCAxis.Web.Controls.FootnoteCodebehind" %>

<div class="footnotes_container">

    <asp:Repeater ID="FootnoteRepeater" runat="server" enableviewstate="False">
    <HeaderTemplate>
        <div role="region" aria-label="footnotes">
        <h2 class="footnote_heading"><asp:Literal ID="Header" runat="server" /></h2>
        <dl class="footnote_definitionlist">
    </HeaderTemplate>    
    <ItemTemplate>
        <asp:Literal ID="MainTermTagAndCssClass" runat="server" />
            <asp:Literal ID="MainTerm" runat="server" />            
        </dt>
        <asp:Literal ID="MainDefinitionTagAndCssClass" runat="server" />
            <asp:Literal ID="MainDefinition" runat="server" />
        </dd>
    </ItemTemplate>    
    <FooterTemplate>
        </dl>
        </div>
    </FooterTemplate>
    </asp:Repeater>


    <asp:Repeater ID="MandatoryFootnoteRepeater" runat="server" enableviewstate="False">
     <HeaderTemplate>
        <asp:Literal ID="HeaderMandatory" runat="server" />
      </HeaderTemplate>    
      <ItemTemplate>
          <asp:Literal ID="MandatoryFootnoteItem" runat="server" />            
     </ItemTemplate>    
     <FooterTemplate>
        <asp:Literal ID="FooterMandatory" runat="server" />
     </FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater ID="NonMandatoryFootnoteRepeater" runat="server" enableviewstate="False" >
      <HeaderTemplate>
        <asp:Literal ID="FootnoteAccordionStart" runat ="server" />
      </HeaderTemplate>    
      <ItemTemplate>
        <asp:Literal ID="NonMandatoryFootnoteItem" runat="server" />            
      </ItemTemplate>    
      <FooterTemplate>
          <asp:Literal ID="FootnoteAccordionEnd" runat="server" />
      </FooterTemplate>
    </asp:Repeater>


    <asp:Label ID="NoFootnotesExist" class="footnote_no_footnotes" runat="server" Text="" Visible="false"></asp:Label>
 </div>   
