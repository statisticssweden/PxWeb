<%@ control  inherits="PCAxis.Web.Controls.DeleteValueCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p class="container_titletext"><asp:Label runat="server" ID="TitleLabel"  /></p>
        
    <asp:Panel runat="server" ID="DeleteValuePanel" Visible="true">
        <p><asp:Label runat="server" ID="DeleteValueTextLabel" CssClass="commandbar_deletevalue_deletevaluetitle" /></p>
        <asp:Repeater ID="VariableSelectorValueSelectRepeater" runat="server">      
            <HeaderTemplate>
                <div class="variableselector_variable_box_container">
            </HeaderTemplate>      
            <ItemTemplate>  
                    <asp:PlaceHolder ID="ValueSelectPlaceHolder" runat="server"></asp:PlaceHolder>            
            </ItemTemplate>   
            <FooterTemplate>
                </div>
                <div class="variableselector_clearboth"></div>
            </FooterTemplate>         
        </asp:Repeater>    
        <p class="commandbar_button_row">
            <asp:Button ID="ContinueButton" runat="server" CssClass="commandbar_deletevalue_continuebutton" />
            <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton" />
        </p>
        <p>
            <asp:Label runat="server" ID="lblError" Visible="false" CssClass="commandbar_deletevalue_errordescription" />
        </p>
    </asp:Panel>
    <asp:HiddenField ID="hidInit" Value="" runat="server" />
<script>
    jQuery(document).ready(function() {
    jQuery('.variableselector_valuesselect_box').resizable({ handles: 'e', minWidth: 150 });                        
    });
</script>    

</div>


