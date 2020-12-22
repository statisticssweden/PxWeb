<%@ control  inherits="PCAxis.Web.Controls.ChangePresentationCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p><asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_changepresentation_titletext" /></p>

     <asp:Panel ID="ChangeCodeTextPanel" runat="server" Visible="false">     
        <p><asp:Label runat="server" ID="ChangeCodeTextTextLabel" CssClass="commandbar_changepresentation_changetext"/></p>
         <asp:Repeater ID="ChangeCodeTextRepeater" runat="server">      
                    <HeaderTemplate> 
                        <p>&nbsp;</p>
                    </HeaderTemplate>      
                    <ItemTemplate>  
                        <p>
                           <asp:Label runat="server" ID="VariableNameTextLabel" CssClass="commandbar_changepresentation_headertext"/>
                           <asp:RadioButtonList runat="server" ID="CodeTextRadio" RepeatColumns="3" CssClass="commandbar_changepresentation_radio"/>
                        </p>
                    </ItemTemplate>   
                    <FooterTemplate>  
                    </FooterTemplate>         
                </asp:Repeater> 
        <div class="commandbar_button_row">
            <asp:Button ID="ChangeCodeText_ContinueButton" runat="server" CssClass="commandbar_changepresentation_continuebutton pxweb-btn primary-btn"/>
            <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton pxweb-btn primary-btn" />
        </div>
    </asp:Panel>
    
    <!-- Errormessage  -->
     <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
        <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_changepresentation_errormessage" Text="" /></p>
     </asp:Panel>
    
</div>

