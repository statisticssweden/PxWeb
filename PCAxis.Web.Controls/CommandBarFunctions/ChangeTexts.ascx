<%@ control  inherits="PCAxis.Web.Controls.ChangeTextsCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p><asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_changetexts_titletext" /></p>
    
    <asp:Panel ID="ChangeTextPanel" runat="server" Visible="false">     
        <p><asp:Label runat="server" ID="ChangeTextTextLabel" CssClass="commandbar_changepresentation_changetext"/></p>
        <p class="commandbar_changepresentation_inputrow">
            <asp:Label runat="server" ID="ContentsTextLabel" CssClass="commandbar_changepresentation_changetext"/>
            <asp:TextBox id="ContentsTextTextBox" runat="server" CssClass="commandbar_changepresentation_textbox"/>
            <asp:Label ID="lblContentsError" runat="server" Text="" CssClass="errorMessage"></asp:Label>
        </p>
        <p class="commandbar_changepresentation_inputrow">
            <asp:Label runat="server" ID="UnitsTextLabel" CssClass="commandbar_changepresentation_changetext"/>
            <asp:TextBox id="UnitsTextTextBox" runat="server" CssClass="commandbar_changepresentation_textbox"/>
            <asp:Label ID="lblUnitsError" runat="server" Text="" CssClass="errorMessage"></asp:Label>
        </p>
        <p><asp:Label runat="server" ID="VariablesTextLabel" CssClass="commandbar_changepresentation_changetext"/></p>
         <asp:Repeater ID="ChangeTextRepeater" runat="server">      
                    <HeaderTemplate> 
                    </HeaderTemplate>      
                    <ItemTemplate>  
                        <p class="commandbar_changepresentation_inputrow">
                            <asp:Label runat="server" ID="ValueListLabel" Text="<%# Convert.ToString(Container.ItemIndex + 1) %>" CssClass="commandbar_changepresentation_changetext"/> 
                            <asp:TextBox id="ValueNameTextbox" runat="server" CssClass="commandbar_changepresentation_textbox"/>
                            <asp:Label ID="lblError" runat="server" Text="" CssClass="errorMessage"></asp:Label>
                        </p>
                        
                    </ItemTemplate>   
                    <FooterTemplate>  
                    </FooterTemplate>         
                </asp:Repeater> 
     <p class="commandbar_button_row">
        <asp:Button ID="ChangeText_ContinueButton" runat="server" CssClass="commandbar_changepresentation_continuebutton"/>
        <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton" />
    </p>
    </asp:Panel>
    

    
    <!-- Errormessage  -->
     <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
        <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_changepresentation_errormessage" Text="" /></p>
     </asp:Panel>
    
</div>

