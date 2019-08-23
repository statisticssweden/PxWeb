<%@ control  inherits="PCAxis.Web.Controls.PerPartCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p><asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_perpart_titletext" /></p>
     
    <asp:Panel runat="server" ID="PerPartPanel" Visible="true">
            
            <!-- Name and KeepValue  -->
            <asp:Panel runat="server" ID="CalcOptionsPanel">
                <asp:RadioButtonList runat="server" ID="KeepValueRadio" RepeatColumns="2" CssClass="commandbar_perpart_radio_horisontal">
                    <asp:ListItem Selected="True" Text="Keep original data" Value="0" />
                    <asp:ListItem Selected="False" Text="Keep perpart only" Value="1"/>
                </asp:RadioButtonList>
                <p>
                    <asp:Label runat="server" ID="NewValueNameLabel" CssClass="commandbar_perpart_perparttext"/>
                    <asp:TextBox runat="server" ID="NewValueNameTextBox"  CssClass="commandbar_perpart_perparttextbox"/>
                    <asp:RequiredFieldValidator ID="NewVariableNameRequired" EnableClientScript="false" runat="server" ControlToValidate="NewValueNameTextBox" ErrorMessage="Name for new value is required" CssClass="commandbar_perpart_errormessage"/>
                    <asp:Label ID="lblError" runat="server" Text="" CssClass="commandbar_perpart_errormessage"></asp:Label>
                </p>
            </asp:Panel>  
            
            <!-- Select option  -->
            <asp:Panel runat="server" ID="SelectOptionPanel">
                <asp:RadioButtonList runat="server" ID="SelectOptionRadio" CssClass="commandbar_radioButtonList">
                    <asp:ListItem Selected="True" Text="OneVariableAllValues" Value="1" />
                    <asp:ListItem Text="OneVariableOneValue" Value="2"/>
                    <asp:ListItem Text="OneMatrixValue" Value="3"/>
                </asp:RadioButtonList>
                <p class="commandbar_button_row">
                    <asp:Button ID="SelectOption_ContinueButton" runat="server" CssClass="commandbar_perpart_continuebutton" />
                    <asp:Button ID="CancelButton1" runat="server" CssClass="commandbar_cancelbutton" />
                </p>
            </asp:Panel>   
            
            <!-- Option 1  -->
            <asp:Panel runat="server" ID="SelectVariablePanel" Visible="false">
                <asp:Repeater ID="SelectVariableRepeater" runat="server">      
                    <HeaderTemplate>
                        <p>
                    </HeaderTemplate>    
                    <ItemTemplate>  
                        <asp:RadioButton runat="server" ID="VariableNameRadio" GroupName="VariableSelectionGroup" CssClass="commandbar_perpart_radio commandbar_radioButtonList"/> 
                    </ItemTemplate>   
                    <FooterTemplate>
                        </p>
                    </FooterTemplate>         
                </asp:Repeater> 
                <p class="commandbar_button_row">
                    <asp:Button ID="SelectVariable_ContinueButton" runat="server" CssClass="commandbar_perpart_continuebutton" />
                    <asp:Button ID="CancelButton2" runat="server" CssClass="commandbar_cancelbutton" />
                </p>
            </asp:Panel>   

            <!-- Option 2  -->
            <asp:Panel runat="server" ID="CalculateOneVariablePanel" Visible="false" CssClass="variableselector_valuesselect_valuesselect_panel">
                <p><asp:Label runat="server" ID="SelectedVariableNameLabel" CssClass="commandbar_perpart_selectvaluetext"/></p>
                <p><asp:ListBox runat="server" ID="CalculateOneVariableListBox" CssClass="variableselector_valuesselect_valueslistbox"/></p>
                <p class="commandbar_button_row">
                    <asp:Button ID="CalculateOneVariable_ContinueButton" runat="server" CssClass="commandbar_perpart_continuebutton" />
                    <asp:Button ID="CancelButton3" runat="server" CssClass="commandbar_cancelbutton" />
                </p>
                <asp:RequiredFieldValidator ID="VariableSelectionRequired" EnableClientScript="false" runat="server" ControlToValidate="CalculateOneVariableListBox" ErrorMessage="Select a value" CssClass="commandbar_perpart_errormessage"/>
            </asp:Panel>
            
            <!-- Option 3  -->
            <asp:Panel runat="server" ID="CalculateAllVariablesPanel" Visible="false">
                <asp:Repeater ID="CalculateAllVariablesRepeater" runat="server">      
                    <HeaderTemplate>
                        <div class="variableselector_variable_box_container"> 
                    </HeaderTemplate>      
                    <ItemTemplate>  
                        <div class="variableselector_valuesselect_box">
                            <asp:Panel runat="server" ID="CalculateAllVariablesValuesSelectPanel" CssClass="variableselector_valuesselect_valuesselect_panel">
                                <p><asp:Label runat="server" ID="VariableNameLabel" CssClass="commandbar_perpart_perparttext"/></p>  
                                <asp:ListBox runat="server" ID="CalculateAllVariablesValuesListBox" CssClass="variableselector_valuesselect_valueslistbox"/>
                                 <asp:RequiredFieldValidator ID="ValueSelectionRequired" runat="server" ControlToValidate="CalculateAllVariablesValuesListBox" ErrorMessage="Select a value" CssClass="commandbar_perpart_errormessage"/>
                            </asp:Panel>
                        </div>  
                    </ItemTemplate>   
                    <FooterTemplate>
                        </div>
                        <div class="variableselector_clearboth"></div>
                    </FooterTemplate>         
                </asp:Repeater>
                <p class="commandbar_button_row">
                    <asp:Button ID="CalculateAllVariables_ContinueButton" runat="server" CssClass="commandbar_perpart_continuebutton" />                                        
                    <asp:Button ID="CancelButton4" runat="server" CssClass="commandbar_cancelbutton" />
                </p>
               
            </asp:Panel>   
     
        <!-- Errormessage  -->
         <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
            <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_perpart_errormessage" Text="" /></p>
         </asp:Panel>
        
    </asp:Panel>

</div>


