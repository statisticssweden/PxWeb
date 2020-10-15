<%@ control  inherits="PCAxis.Web.Controls.NavigationFlowCodebehind" %>

<div class="navigationFlowArea">
      <asp:HyperLink ID="firstStepLink" CssClass="navigation-link first " runat="server">
           <asp:Image ID="firstStepImage" CssClass="nav-pic" runat="server"/>
           <asp:Label ID="firstStepLabel" CssClass="pxweb-link header nav-step first " runat="server"/>
      </asp:HyperLink> 
   
      <asp:Literal ID ="navHrLeft"  runat="server"/>

      <asp:HyperLink ID="secondStepLink" CssClass="navigation-link " runat="server">
           <asp:Image ID="secondStepImage" CssClass="nav-pic" runat="server"/>
           <asp:Label ID="secondStepLabel" CssClass="pxweb-link header nav-step second " runat="server"/>
      </asp:HyperLink>
  
      <asp:Literal ID ="navHrRight"  runat="server"/>

      <asp:HyperLink ID="thirdStepLink" CssClass="third navigation-link " runat="server">
           <asp:Image ID="thirdStepImage" CssClass="nav-pic" runat ="server"/>
           <asp:Label ID="thirdStepLabel" CssClass="nav-step third " runat="server"/>
      </asp:HyperLink>
</div>
