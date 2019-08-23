<%@ control inherits="PCAxis.Web.Controls.TextCodebehind" %>
<div class="excelcontainer">
    
        <div class="description">
            <asp:Label ID="LineLengthDescription" runat="server" AssociatedControlID="LineLengthValue" Text="Label" />
        </div>
        <div class="value">
            <asp:TextBox ID="LineLengthValue" runat="server" />
        </div>
   
        <div class="description">
            <asp:Label ID="PageLengthDescription" runat="server" AssociatedControlID="PageLengthValue" Text="Label" />
        </div>
        <div class="value">
            <asp:TextBox ID="PageLengthValue" runat="server" />
        </div>
   
        <div class="description">
            <asp:Label ID="MarginDescription" runat="server" AssociatedControlID="MarginValue" Text="Label" />
        </div>
        <div class="value">
            <asp:TextBox ID="MarginValue" runat="server" />
        </div>
    
</div>
<asp:Button ID="ContinueButton" runat="server" CssClass="excelcontinue" />