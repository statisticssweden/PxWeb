<%@ control  inherits="PCAxis.Web.Controls.PerPartCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div class="per_part_rootdiv">
    
    <h3 class="container_titletext">
       <asp:Literal runat="server" Text="<%$ PxString: CtrlCommandBarFunctionPerPart%>" />
    </h3>

     
    <asp:Panel runat="server" ID="PerPartPanel" Visible="true">
            
            <!-- Name and KeepValue  -->
            <asp:Panel runat="server" ID="CalcOptionsPanel">
                <asp:Panel runat="server" ID="KeepValueRadioPanel">
                  <asp:RadioButtonList runat="server" ID="KeepValueRadio" RepeatColumns ="2" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="commandbar_perpart_radio_horisontal">
                    <asp:ListItem Selected="False" Text="Keep original data" Value="0" />
                    <asp:ListItem Selected="True" Text="Keep perpart only" Value="1"/>
                  </asp:RadioButtonList>
                </asp:Panel>
                  <div class="m-margin-top"></div>
                <asp:Label runat="server" ID="NewValueNameLabel" ClientIDMode="Static" CssClass="commandbar_perpart_perparttext font-heading"/>
                <div>
                    <asp:TextBox runat="server" ID="NewValueNameTextBox"  aria-labelledby="NewValueNameLabel" CssClass="commandbar_perpart_perparttextbox"/>
                    <asp:RequiredFieldValidator ID="NewVariableNameRequired" EnableClientScript="false" runat="server" ControlToValidate="NewValueNameTextBox" ErrorMessage="Name for new value is required" CssClass="commandbar_perpart_errormessage"/>
                    <asp:Label ID="lblError" runat="server" Text="" CssClass="commandbar_perpart_errormessage"></asp:Label>
                </div>
                <div class="s-margin-top green-rule-thin"></div>
                <div class="s-margin-top"></div>
            </asp:Panel>  

              

            
            <!-- Select option  -->
            <asp:Panel runat="server" ID="SelectOptionPanel">
                <asp:Panel runat="server" ID="SelectOptionRadioPanel">
                    <asp:RadioButtonList runat="server" ID="SelectOptionRadio" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="commandbar_radioButtonList">
                       <asp:ListItem Selected="True" Text="OneVariableAllValues" Value="1" />
                       <asp:ListItem Text="OneVariableOneValue" Value="2"/>
                       <asp:ListItem Text="OneMatrixValue" Value="3"/>
                    </asp:RadioButtonList>
                </asp:Panel>
                <div class="container_exit_buttons_row">
                       <asp:Button ID="CancelButton1" runat="server" CssClass="container_cancelbutton pxweb-btn" />
                       <asp:Button ID="SelectOption_ContinueButton" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn"/>           
                </div>
            </asp:Panel>   
            
            <!-- Option 1  -->
            <asp:Panel runat="server" ID="SelectVariablePanel" Visible="false">
                <asp:Panel ID="SelectVariableRepeaterPanel" runat="server">
                    <div class="flex-column">
                <asp:Repeater ID="SelectVariableRepeater" runat="server">       
                    <ItemTemplate>  
                        <asp:RadioButton runat="server" ID="VariableNameRadio" GroupName="VariableSelectionGroup" CssClass="commandbar_perpart_radio commandbar_radioButtonList"/> 
                    </ItemTemplate>   
                </asp:Repeater> 
                        </div> 
                </asp:Panel>
                <div class="container_exit_buttons_row">
                    <asp:Button ID="CancelButton2" runat="server" CssClass="commandbar_cancelbutton pxweb-btn" />
                    <asp:Button ID="SelectVariable_ContinueButton" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn" />
                </div>
                
            </asp:Panel>   

            <!-- Option 2  -->
            <asp:Panel runat="server" ID="CalculateOneVariablePanel" Visible="false" CssClass="variableselector_valuesselect_valuesselect_panel">
                <asp:Label runat="server" ID="SelectedVariableNameLabel" ClientIDMode="Static" CssClass="commandbar_perpart_selectvaluetext font-heading"/>
                <asp:ListBox runat="server" ID="CalculateOneVariableListBox" aria-labelledby="SelectedVariableNameLabel" CssClass="variableselector_valuesselect_valueslistbox"/>
                
                <div class="container_exit_buttons_row">
                       <asp:Button ID="CancelButton3" runat="server" CssClass="container_cancelbutton pxweb-btn" />
                       <asp:Button ID="CalculateOneVariable_ContinueButton" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn"/>           
                </div>
                <asp:RequiredFieldValidator ID="VariableSelectionRequired" EnableClientScript="false" runat="server" ControlToValidate="CalculateOneVariableListBox" ErrorMessage="Select a value" CssClass="commandbar_perpart_errormessage"/>
            </asp:Panel>
            
            <!-- Option 3  -->
            <asp:Panel runat="server" ID="CalculateAllVariablesPanel" Visible="false">
                <asp:Label runat="server" CssClass="font-heading" Text="<%$ PxString: CtrlPerPartSelectCellLabel%>"></asp:Label>
                <asp:Repeater ID="CalculateAllVariablesRepeater" runat="server">      
                    <HeaderTemplate>
                        <div class="variableselector_variable_box_container flex-row flex-wrap"> 
                    </HeaderTemplate>      
                    <ItemTemplate>
                            <asp:Panel runat="server" ID="CalculateAllVariablesValuesSelectPanel" CssClass="commandbar-listbox-container">
                                <asp:Label runat="server" ID="VariableNameLabel" CssClass="commandbar_perpart_perparttext font-heading"/>
                                <asp:ListBox runat="server" ID="CalculateAllVariablesValuesListBox" CssClass="variableselector_valuesselect_valueslistbox" SelectionMode="Single" />
                            </asp:Panel>
                    </ItemTemplate>   
                    <FooterTemplate>
                        </div>
                        <div class="variableselector_clearboth"></div>
                    </FooterTemplate>         
                </asp:Repeater>

                 <div class="container_exit_buttons_row">
                       <asp:Button ID="CancelButton4" runat="server" CssClass="container_cancelbutton pxweb-btn" />
                       <asp:Button ID="CalculateAllVariables_ContinueButton" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn"/>           
                </div>
            </asp:Panel>   
     
        <!-- Errormessage  -->
         <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
            <div class="flex-row px-messages small">
               <div class="Information-warning-box-icon small"></div>
               <asp:Label ID="InfoMessageLabel" runat="server" CssClass="xs-margin-left" Text="" />
            </div>
            <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_perpart_errormessage" Text="" />
            <div class="container_exit_buttons_row">
              <asp:Button ID="CancelButton5" runat="server" CssClass="container_continuebutton pxweb-btn primary-btn" />
            </div>
         </asp:Panel>
        
    </asp:Panel>

</div>


