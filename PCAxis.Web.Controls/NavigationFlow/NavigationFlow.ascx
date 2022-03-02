<%@ control  inherits="PCAxis.Web.Controls.NavigationFlowCodebehind" %>
 <section aria-label="<%= SectionAriaLabel %>">
   <asp:Label ID ="ExplainAriaLabel" runat="server"/>
  
   <div id="navigationFlow"> 
    <div class="navigationFlowArea flex-row justify-center align-flex-start">
      <asp:HyperLink ID="firstStepLink" CssClass="flex-column justify-space-between align-center navigation-link first " runat="server">
           <asp:Image aria-hidden="true" alt="" ID="firstStepImage" CssClass="nav-pic" runat="server"/>
           <asp:Label ID="firstStepLabel" CssClass="pxweb-link header nav-step first " runat="server"/>
      </asp:HyperLink> 
   
      <asp:Literal ID ="navHrLeft" runat="server"/>

      <asp:HyperLink ID="secondStepLink" CssClass="flex-column justify-space-between align-center navigation-link " runat="server">
           <asp:Image aria-hidden="true" alt="" ID="secondStepImage" CssClass="nav-pic" runat="server"/>
           <asp:Label ID="secondStepLabel" CssClass="pxweb-link header nav-step second " runat="server"/>
      </asp:HyperLink>
  
      <asp:Literal ID ="navHrRight" runat="server"/>

      <asp:HyperLink ID="thirdStepLink" CssClass="flex-column justify-space-between align-center third navigation-link " runat="server">
           <asp:Image aria-hidden="true" alt="" ID="thirdStepImage" CssClass="nav-pic" runat ="server"/>
           <asp:Label ID="thirdStepLabel" CssClass="nav-step third " runat="server"/>
      </asp:HyperLink>
    </div>
   </div>      
 </section>
