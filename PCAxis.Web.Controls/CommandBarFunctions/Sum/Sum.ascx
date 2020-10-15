<%@ control  inherits="PCAxis.Web.Controls.SumCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <p><asp:Label runat="server" ID="TitleLabel" CssClass="commandbar_sum_titletext" /></p>
        
    <!-- Select variable -->
    <asp:Panel runat="server" ID="SelectVariableOptionsPanel">
        <p><asp:Label runat="server" ID="SelectVariabelsLabel" CssClass="commandbar_sum_sumallvaluestext"/></p>
        <asp:RadioButtonList runat="server" ID="VariableToSumRadioButtonList" CssClass="commandbar_sum_radiobuttonlist"/>
        <asp:RadioButtonList runat="server" ID="SumOptionsRadioButtonList" CssClass="commandbar_sum_radiobuttonlist" RepeatDirection="Horizontal">
            <asp:ListItem Text="Summera alla värden" Value="SumAll" Selected="True" />
            <asp:ListItem Text="Summera utvalda värden" Value="SumSelected" />
        </asp:RadioButtonList>
        <div class="commandbar_button_row">
            <asp:Button runat="server" ID="ContinueButtonSelectVariables" CssClass="pxweb-btn primary-btn"/>
            <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton pxweb-btn primary-btn" />
        </div>
    </asp:Panel>
    
    <!-- Select values -->
    <asp:Panel runat="server" ID="SelectValuesOptionsPanel" Visible="false">
        <p><asp:Label runat="server" ID="SelectVariableValuesLabel" CssClass="commandbar_sum_sumallvaluestitle" /></p>
        <p><asp:ListBox runat="server" ID="ValuesToSumListBox" Visible="false" CssClass="commandbar_sum_valuestosum_listbox"  /></p>
        <asp:Panel runat="server" ID="SelectOperandsPanel" Visible="false">
            <p class="commandbar_sum_operandspanel_row">
            <asp:Label runat="server" ID="FirstOperandLabel" CssClass="commandbar_sum_sumallvaluestext"/>
            <asp:DropDownList runat="server" ID="FirstOperandDropDown" CssClass="commandbar_sum_sumallvaluesnewvariabelname_dropdown" />
            <asp:Label runat="server" ID="SecondOperandLabel" CssClass="commandbar_sum_sumallvaluestext"/>
            <asp:DropDownList runat="server" ID="SecondOperandDropDown" CssClass="commandbar_sum_sumallvaluesnewvariabelname_dropdown" />
            </p>
            <p>
            <asp:CompareValidator runat="server" EnableClientScript="false" ID="OperandsValidator" ControlToValidate="FirstOperandDropDown" ControlToCompare="SecondOperandDropDown" Operator="NotEqual" ErrorMessage="Operation requires two different operands." CssClass="commandbar_sum_errormessage"/>
            </p>       
        </asp:Panel>
        <div class="commandbar_button_row">
            <asp:Button ID="ContinueButtonSelectValues" runat="server" CssClass="commandbar_sum_continuebutton pxweb-btn primary-btn" />            
            <asp:Button ID="CancelButton2" runat="server" CssClass="commandbar_cancelbutton pxweb-btn primary-btn" />
        </div>
    </asp:Panel>
    
    <!-- Name on new variable -->
    <asp:Panel ID="TotalVariableNamePanel" runat="server" Visible="false"> 
        <asp:Panel ID="KeepValuesPanel" runat="server" Visible="false"> 
            <p>
                <asp:Label runat="server" ID="KeepValuesLabel" CssClass="commandbar_sum_sumallvaluestext"/>
                <asp:CheckBox runat="server" id="KeepValuesCheckBox" Checked="true"  CssClass="commandbar_sum_sumallvaluesnewvariabelname_checkbox"/>
            </p> 
        </asp:Panel>   
        <p>
            <asp:Label runat="server" ID="TotalVariableNameLabel" CssClass="commandbar_sum_sumallvaluestext"/>
            <asp:TextBox  runat="server" id="TotalVariableName" CssClass="commandbar_sum_sumallvaluesnewvariabelname_textbox"/>
        </p>
        
        
        <div class="commandbar_button_row">
            <asp:Button ID="ContinueButton" runat="server" CssClass="commandbar_sum_continuebutton pxweb-btn primary-btn"/>            
            <asp:Button ID="CancelButton3" runat="server" CssClass="commandbar_cancelbutton pxweb-btn primary-btn" />
        </div>
        <asp:RequiredFieldValidator ID="TotalVariableNameRequired" EnableClientScript="false" runat="server" ControlToValidate="TotalVariableName" ErrorMessage="Name for new value is required" CssClass="commandbar_sum_errormessage"/>
    </asp:Panel>
    
    <!-- Errormessage -->
     <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
        <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_sum_errormessage" Text="" /></p>
     </asp:Panel>
    
</div>

