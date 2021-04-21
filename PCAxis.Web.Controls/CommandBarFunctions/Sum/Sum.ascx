<%@ control  inherits="PCAxis.Web.Controls.SumCodebehind" %>
<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>

<div>
    <h3 class="container_titletext">
        <asp:Literal runat="server" ID="TitleLiteral"/>
    </h3>

    <!-- Select variable -->
    <asp:Panel runat="server" CssClass="flex-column" ID="SelectVariableOptionsPanel">
        <asp:Panel ID="ChooseVariablePanel" runat="server">
            <asp:RadioButtonList runat="server" ID="VariableToSumRadioButtonList" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="commandbar_sum_radiobuttonlist"/>
        </asp:Panel>
        <asp:Panel ID="SumOptionPanel" runat="server">
            <asp:RadioButtonList runat="server" ID="SumOptionsRadioButtonList" CssClass="commandbar_sum_radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Vertical">
                <asp:ListItem Text="Summera alla värden" Value="SumAll" Selected="True" />
                <asp:ListItem Text="Summera utvalda värden" Value="SumSelected" />
            </asp:RadioButtonList>
        </asp:Panel>
        <div class="container_exit_buttons_row">
            <asp:Button ID="CancelButton" runat="server" CssClass="commandbar_cancelbutton pxweb-btn " />
            <asp:Button runat="server" ID="ContinueButtonSelectVariables" CssClass="pxweb-btn primary-btn container_continuebutton"/>
        </div>
    </asp:Panel>
    
    <!-- Select values -->
    <asp:Panel runat="server" CssClass="flex-column" ID="SelectValuesOptionsPanel" Visible="false">
        <asp:Label runat="server" ID="SelectVariableValuesLabel" CssClass="commandbar_sum_sumallvaluestitle font-heading s-margin-top" />
        <asp:ListBox runat="server" ID="ValuesToSumListBox" Visible="false" CssClass="commandbar_sum_valuestosum_listbox"  />
        <asp:Panel runat="server" ID="SelectOperandsPanel" CssClass="commandbar_sum_operandspanel_row flex-row" Visible="false">
            <asp:Label runat="server" ID="FirstOperandLabel" CssClass="commandbar_sum_sumallvaluestext font-heading"/>
            <asp:DropDownList runat="server" ID="FirstOperandDropDown" CssClass="commandbar_sum_sumallvaluesnewvariabelname_dropdown" />
            <asp:Label runat="server" ID="SecondOperandLabel" CssClass="commandbar_sum_sumallvaluestext font-heading"/>
            <asp:DropDownList runat="server" ID="SecondOperandDropDown" CssClass="commandbar_sum_sumallvaluesnewvariabelname_dropdown" />
            <asp:CompareValidator runat="server" EnableClientScript="false" ID="OperandsValidator" ControlToValidate="FirstOperandDropDown" ControlToCompare="SecondOperandDropDown" Operator="NotEqual" ErrorMessage="Operation requires two different operands." CssClass="commandbar_sum_errormessage"/>
        </asp:Panel>
        <div class="container_exit_buttons_row">
            <asp:Button ID="CancelButton2" runat="server" CssClass="commandbar_cancelbutton pxweb-btn" />
            <asp:Button ID="ContinueButtonSelectValues" runat="server" CssClass="commandbar_sum_continuebutton pxweb-btn primary-btn container_continuebutton" />
        </div>
    </asp:Panel>
    
    <!-- Name on new variable -->
    <asp:Panel ID="TotalVariableNamePanel" CssClass="flex-column" runat="server" Visible="false"> 
        <asp:Panel ID="KeepValuesPanel" CssClass="flex-row s-margin-top" runat="server" Visible="false">
            <asp:Label runat="server" ID="KeepValuesLabel" AssociatedControlID="KeepValuesCheckBox" CssClass="commandbar_sum_sumallvaluestext font-heading"/>
            <asp:CheckBox runat="server" id="KeepValuesCheckBox" Checked="true"  CssClass="commandbar_sum_sumallvaluesnewvariabelname_checkbox"/>
        </asp:Panel>   
        <div class="flex-row s-margin-top">
            <asp:Label runat="server" ID="TotalVariableNameLabel" AssociatedControlID="TotalVariableName" CssClass="commandbar_sum_sumallvaluestext font-heading"/>
            <asp:TextBox  runat="server" id="TotalVariableName" CssClass="commandbar_sum_sumallvaluesnewvariabelname_textbox"/>
        </div>
        <div class="container_exit_buttons_row">
            <asp:Button ID="CancelButton3" runat="server" CssClass="commandbar_cancelbutton pxweb-btn" />
            <asp:Button ID="ContinueButton" runat="server" CssClass="commandbar_sum_continuebutton pxweb-btn primary-btn container_continuebutton"/>
        </div>
        <asp:RequiredFieldValidator ID="TotalVariableNameRequired" EnableClientScript="false" runat="server" ControlToValidate="TotalVariableName" ErrorMessage="Name for new value is required" CssClass="commandbar_sum_errormessage"/>
    </asp:Panel>
    <!-- Errormessage -->
     <asp:Panel runat="server" ID="ErrorMessagePanel" Visible="false">
        <p><asp:Label ID="ErrorMessageLabel" runat="server" CssClass="commandbar_sum_errormessage" Text="" /></p>
     </asp:Panel>
    
</div>

