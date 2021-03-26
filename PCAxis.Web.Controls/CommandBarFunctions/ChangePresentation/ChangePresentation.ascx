<%@ control  inherits="PCAxis.Web.Controls.ChangePresentationCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div class="changepresentation_rootdiv">
    <h3 class="container_titletext">
       <asp:Literal runat="server" Text="<%$ PxString: CtrlCommandBarFunctionChangeCodeTextHeading%>" />
    </h3>
    
    <asp:Panel ID="ChangeCodeTextPanel" runat="server" Visible="false">     
        <asp:Label runat="server" ID="ChangeCodeTextTextLabel" CssClass="commandbar_changepresentation_changetext"/>
        <asp:Repeater ID="ChangeCodeTextRepeater" runat="server">          
                    <ItemTemplate>  
                         <asp:Panel ID="ChangeCodeTextOneItemPanel" runat="server">
                           <asp:RadioButtonList runat="server" ID="CodeTextRadio" RepeatColumns="3" CssClass="commandbar_changepresentation_radio"  RepeatLayout="Flow" RepeatDirection="Horizontal" />
                        </asp:Panel>
                    </ItemTemplate>    
        </asp:Repeater> 
            <!-- Errormessage  -->
    <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false" CssClass="m-margin-top">
        <div class="flex-row px-messages small">
            <div class="Information-warning-box-icon small"></div>
            <asp:Label ID="InfoMessageLabel" runat="server" CssClass="xs-margin-left" Text="" />
        </div>
        <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_changepresentation_errormessage" Text="" />
    </asp:Panel>

    <div class="container_exit_buttons_row">
        <asp:Button ID="CancelButton" runat="server" CssClass="pxweb-btn pxweb-buttons" />
        <asp:Button ID="ChangeCodeText_CompleteButton" runat="server" CssClass="pxweb-btn primary-btn pxweb-buttons" />
    </div>

    </asp:Panel>
    

    
</div>

