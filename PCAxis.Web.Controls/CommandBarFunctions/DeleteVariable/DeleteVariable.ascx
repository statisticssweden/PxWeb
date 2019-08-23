<%@ control  inherits="PCAxis.Web.Controls.DeleteVariableCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p><asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_deletevariable_titletext" /></p>
        
    <asp:Panel runat="server" ID="DeleteValuePanel" Visible="true">
        <p>
            <asp:Label runat="server" ID="DeleteVariableAddToTitle" CssClass="commandbar_deletevariable_deletevariabletitle" />
            <asp:CheckBox runat="server" ID="AddToTitleCheckbox" Checked="false" />
        </p>
        <p><asp:Label runat="server" ID="DeleteVariableTextLabel" CssClass="commandbar_deletevariable_deletevariabletitle" /></p>

            <asp:Repeater ID="VariableSelectorValueSelectRepeater" runat="server">      
                <HeaderTemplate>
                    <div class="variableselector_variable_box_container"> 
                </HeaderTemplate>      
                <ItemTemplate>  
                    <div class="variableselector_valuesselect_box">
                        <asp:Panel runat="server" ID="EventButtons" CssClass="variableselector_valuesselect_eventbutton_panel">
                            <p>
                                <asp:RadioButton runat="server" ID="VariableNameRadio" GroupName="VariableSelectionGroup" CssClass="commandbar_deletevariable_nametext"/>
                            </p>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ValuesSelectPanel" CssClass="variableselector_valuesselect_valuesselect_panel">  
                            <asp:ListBox runat="server" ID="ValuesListBox" CssClass="variableselector_valuesselect_valueslistbox"/>
                        </asp:Panel>
                    </div>  
                </ItemTemplate>   
                <FooterTemplate>
                    </div>
                    <div class="variableselector_clearboth"></div>
                </FooterTemplate>         
            </asp:Repeater>    

         <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
            <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_deletevariable_errormessage" Text="" /></p>
         </asp:Panel>
        <p class="commandbar_button_row">
            <asp:Button ID="ContinueButton" runat="server" CssClass="commandbar_deletevariable_continuebutton" />
            <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton" />
        </p>
    </asp:Panel>
    <script type="text/javascript">
        jQuery(document).ready(function() {
        jQuery('.variableselector_valuesselect_box').resizable({ handles: 'e', minWidth: 150 });                        
        });
    </script>   
</div>


