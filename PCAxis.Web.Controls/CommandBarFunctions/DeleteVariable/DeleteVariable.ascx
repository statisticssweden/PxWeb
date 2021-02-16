<%@ control  inherits="PCAxis.Web.Controls.DeleteVariableCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div class="per_part_rootdiv">
    <h3 class="container_titletext"><asp:Label runat="server" ID="TitleLabel" /></h3>
        
    <asp:Panel runat="server" ID="DeleteValuePanel" Visible="true">
        <p><asp:Label runat="server" ID="DeleteVariableTextLabel" CssClass="commandbar_deletevariable_deletevariabletitle font-normal-text" /></p>
        <p> 
            <asp:CheckBox runat="server" ID="AddToTitleCheckbox" Checked="true" TextAlign="Left" CssClass="font-normal-text"/> 
        </p>
       

            <asp:Repeater ID="VariableSelectorValueSelectRepeater" runat="server">      
                <HeaderTemplate>
                    <div class="variableselector_variable_box_container flex-row flex-wrap"> 
                </HeaderTemplate>      
                <ItemTemplate>  
<%--                    <div class="variableselector_valuesselect_box_commandbar">--%>
                    <div class="commandbar-listbox-container">                        
                        <asp:Panel runat="server" ID="EventButtons" CssClass="variableselector_valuesselect_eventbutton_panel_commandbar">
                            <p>
                                <asp:RadioButton runat="server" ID="VariableNameRadio" GroupName="VariableSelectionGroup" CssClass="commandbar_deletevariable_nametext"/>
                            </p>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ValuesSelectPanel" CssClass="variableselector_valuesselect_valuesselect_panel_commandbar">  
                            <asp:ListBox runat="server" ID="ValuesListBox" CssClass="variableselector_valuesselect_valueslistbox" ar />
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
        <div class="container_exit_buttons_row">
            <asp:Button ID="CancelButton" runat="server" CssClass="container_cancelbutton pxweb-btn" />
            <asp:Button ID="ContinueButton" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn" />
        </div>
    </asp:Panel>
    <script>
        jQuery(document).ready(function() {
            jQuery('.variableselector_valuesselect_box_commandbar').resizable({ handles: 'e', minWidth: 150 });                        
        });
    </script>   
</div>


