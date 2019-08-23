<%@ control inherits="PCAxis.Web.Controls.VariableSelectorSelectionInformationCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>
<input id="SelectionLimitation" class="variableselector_selection_limits_number" runat="server" type="hidden" />
<input id="NumberFormat" class="variableselector_selection_limits_numberformat" runat="server" type="hidden" />
<asp:PlaceHolder runat="server" ID="SelectionMadeInformationPlaceHolder">
    <p>
        <asp:PlaceHolder runat="server" ID="RowColSelectionInformationPlaceHolder">
            <asp:Label runat="server" ID="SelectedRowsLabel" CssClass="variableselector_selected_rows_label"/>
            <asp:Label runat="server" ID="SelectedRowsLabelSelected" CssClass="variableselector_selected_rows_label_selected"/>
            <asp:Label runat="server" ID="SelectedColumnsLabel" CssClass="variableselector_selected_columns_label"/>
            <asp:Label runat="server" ID="SelectedColumnsLabelSelected" CssClass="variableselector_selected_columns_label_selected"/> 
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="CellSelectionInformationPlaceHolder">
            <asp:Label runat="server" ID="SelectedCellsLabel" CssClass="variableselector_selected_cells_label"/>
            <asp:Label runat="server" ID="SelectedCellsLabelSelected" CssClass="variableselector_selected_cells_label_selected"/>
            <asp:Label runat="server" ID="SelectedCellsNumberLabel" CssClass="variableselector_selected_cells_number_label"/>
            <asp:Label runat="server" ID="SelectedCellsLimitLabel" CssClass="variableselector_selected_cells_limit_label"/>
        </asp:PlaceHolder>
    </p>
</asp:PlaceHolder>
<asp:PlaceHolder ID="SelectionLimitsInformationPlaceHolder" runat="server">
     <p><asp:Label runat="server" ID="SelectionLimitationLabel" CssClass="variableselector_selection_limits_label"/></p>
</asp:PlaceHolder>

<script type="text/javascript">
    var StubListboxes = new Array();
    var HeadingListboxes = new Array();
    var _selectedInStub = 0;
    var _selectedInHeading = 0;
    var _selectionLimit = new Array();
    var _toManySelectedCellsText;
    var _toManySelectedRowsText;
    var _toManySelectedColumnsText;

    jQuery(document).ready(function() {
        GetListboxSelections("ValuesListBox");
    });


    //Get listbox with name matching [nameregex]. Checks the Stub/Heading alignment and limitSelectionBy setting in the parameters for its onchange-event
    //Calls SelectedValueChanged with listboxid,["Stub"]/["Heading"] alignment, number of selected values and ["RowsColumns"]/["Cells"] limitation
    //nameregex: string to match in the listboxname to get a listbox
    function GetListboxSelections(nameregex) {
        var nameRegExp = new RegExp(nameregex);
        var stubRegExp = new RegExp("Stub");
        var headingRegExp = new RegExp("Heading");
        var limitSelectionByRegExp = new RegExp("RowsColumns");
        var selectionCount;
        var limitSelectionBy = "Cells";

        jQuery.each(jQuery("select"), function(index, obj) {
            if (nameRegExp.test(obj.name)) {
                if (obj.attributes["onchange"] != null) {
                    if (limitSelectionByRegExp.test(obj.attributes["onchange"].value)) {
                        limitSelectionBy = "RowsColumns";
                    }
                    selectionCount = jQuery("#" + obj.id + " option:selected").length;
                    if (stubRegExp.test(obj.attributes["onchange"].value)) {
                        SelectedValueChanged(obj.id, "Stub", selectionCount, limitSelectionBy);
                    } else if (headingRegExp.test(obj.attributes["onchange"].value)) {
                        SelectedValueChanged(obj.id, "Heading", selectionCount, limitSelectionBy);
                    }
                }
            }
        });
    }

    //VariableListBox type
    function VariableListBox(listBoxId, numberOfSelectedItems, variablePlacement) {
        this.id = listBoxId;
        this.count = numberOfSelectedItems;
        this.placement = variablePlacement;
    }

    //Makes StubListboxes and HeadingListboxes contain the listboxes with items selected
    //Updates _selectedInStub and _selectedInHeading to correct counts
    //Calls SetValuesRowColCount or SetValuesCellCount depending on value in limitSelectionBy
    //id: listbox id
    //placement: ["Heading"]/["Stub"]
    //count: Number of selected items in the listbox
    //limitSelectionBy: ["Cells"]/["RowsColumns"]
    function SelectedValueChanged(id, placement, count, limitSelectionBy) {
        var listBox = new VariableListBox(id, count, placement);
        var arr;
        //Stubvalues selected/deselected
        if (placement == "Stub") {          
            if (count > 0) {
                StubListboxes = AddVariableListBox(StubListboxes, listBox);
            } else {
                StubListboxes = RemoveVariableListBox(StubListboxes, listBox);
            }
            arr = StubListboxes;
        //Headingvalues selected/deselected
        } else {            
            if (count > 0) {
                HeadingListboxes = AddVariableListBox(HeadingListboxes, listBox);
            } else {
                HeadingListboxes = RemoveVariableListBox(HeadingListboxes, listBox);
            }
            arr = HeadingListboxes;
        }
        //Count selections       
        var selectionCount = 0;
        jQuery.each(arr, function(index, obj) {
            if (selectionCount == 0) {
                selectionCount = obj.count;
            }
            else {
                selectionCount *= obj.count;
            }
        });

        //Update counts
        if (placement == "Stub") {
            _selectedInStub = selectionCount
        }
        else {
            _selectedInHeading = selectionCount
        }

        if (limitSelectionBy == "RowsColumns") {
            SetValuesRowColCount(selectionCount, placement, limitSelectionBy);
        }
        else {
            SetValuesCellCount(selectionCount, placement, limitSelectionBy);
        }
    }


    //Print out number of selected rows and columns. Assures that if anything is seleted both counters are at least 1
    //Calls CheckNumberOfSelected
    //selectionCount: number of selected items in the current placement (Heading/Stub)
    //placement: ["Heading"]/["Stub"]
    //limitSelectionBy: ["Cells"]/["RowsColumns"]
    function SetValuesRowColCount(selectionCount, placement, limitSelectionBy) {
        var modifiedLabel, siblingCount, siblingLabel;
        if (placement == "Stub") {
            modifiedLabel = "SelectedRowsLabelSelected";
            siblingLabel = "SelectedColumnsLabelSelected";
            siblingCount = HeadingListboxes.length;
        }
        else {
            modifiedLabel = "SelectedColumnsLabelSelected";
            siblingLabel = "SelectedRowsLabelSelected";
            siblingCount = StubListboxes.length;
        }

        //If there is a selection, the minimum value is 1 for both rows and columns
        if (siblingCount < 1) {
            if (selectionCount > 0) {
                SetLabelText("1", siblingLabel,false);
            } else {
                SetLabelText("0", siblingLabel,false);
            }
        }
        
        if ((selectionCount <= 0) && (siblingCount > 0)) {
            selectionCount = 1;
        }

        //Set texts
        SetLabelText(format(GetNumberFormat(),selectionCount), modifiedLabel, false);
        CheckNumberOfSelected(limitSelectionBy);
    }

    //Print out number of selected cells. Assures that if anything is seleted the count is at least 1
    //Calls CheckNumberOfSelected
    //selectionCount: number of selected items in the current placement (Heading/Stub)
    //placement: ["Heading"]/["Stub"]
    //limitSelectionBy: ["Cells"]/["RowsColumns"]
    function SetValuesCellCount(selectionCount, placement, limitSelectionBy) {
        //If there is a selection, the minimum value is 1 for both rows and columns
        if (_selectedInStub > 0 && _selectedInHeading == 0) {
            _selectedInHeading = 1;
        }
        if (_selectedInHeading > 0 && _selectedInStub == 0) {
            _selectedInStub = 1;
        }
        //Set texts
        //SetLabelText(_selectedInHeading * _selectedInStub, "SelectedCellsLabelSelected", false);
        SetLabelText(format(GetNumberFormat(), _selectedInHeading * _selectedInStub), "SelectedCellsNumberLabel", false);

        CheckNumberOfSelected(limitSelectionBy);
    }

    //Get format of how numbers shall be displayed
    function GetNumberFormat() {
        if (jQuery('.variableselector_selection_limits_numberformat').first().val() != null) {
            return jQuery('.variableselector_selection_limits_numberformat').first().val();
        }
        else {
            return "";
        }
    }

    //Checks if selection limitation is exceeded, sets or clears message.
    //limitSelectionBy: ["Cells"]/["RowsColumns"]
    function CheckNumberOfSelected(limitSelectionBy) {
        var limitationExceeded = false;
        var errorMessage;
        var localSelectionLimit
        if (_selectionLimit.length < 1) {
            localSelectionLimit = jQuery(".variableselector_selection_limits_number").first().val();
            if (localSelectionLimit == null) {
                _selectionLimit = [0];
            }
            else {
                _selectionLimit = localSelectionLimit.split(",");
            }
        }
        if (_toManySelectedCellsText == null) {
            _toManySelectedCellsText = GetLabelText("SelectionErrorlabelTextCells", "variableselector_selectionerror_label_text");
        }
        if (_toManySelectedRowsText == null) {
            _toManySelectedRowsText = GetLabelText("SelectionErrorlabelTextRows", "variableselector_selectionerror_label_text");
        }
        if (_toManySelectedColumnsText == null) {
            _toManySelectedColumnsText = GetLabelText("SelectionErrorlabelTextColumns", "variableselector_selectionerror_label_text");
        }
        
        //Message if to many cells is selected
        if (limitSelectionBy == "Cells" && _selectionLimit.length == 1) {
            if ((_selectedInHeading * _selectedInStub) > _selectionLimit[0]) {
                errorMessage = _toManySelectedCellsText;
                limitationExceeded = true;
            }
        }
        else if (limitSelectionBy == "RowsColumns" && _selectionLimit.length == 2) {
            if (_selectedInStub > _selectionLimit[0]) {
                errorMessage = _toManySelectedRowsText;
                limitationExceeded = true;
            }
            if (_selectedInHeading > _selectionLimit[1]) {
                errorMessage = _toManySelectedColumnsText;
                limitationExceeded = true;
            }
        }
        //alert("_selectedInStub:" + _selectedInStub + ",_selectionLimit[0]:" + _selectionLimit[0] + "_selectedInHeading:" + _selectedInHeading + ",_selectionLimit[1]:" + _selectionLimit[1] + ",limitationExceeded" + limitationExceeded);
        if (limitationExceeded) {
            SetLabelText_IdAndCSS(errorMessage, "SelectionErrorlabel", "variableselector_selectionerror_label", false);
            disableFromClass("variableselector_continue_button", true);
        }
        else {
            var currentMessage = GetLabelText("SelectionErrorlabel", "variableselector_selectionerror_label");
            if ((currentMessage == _toManySelectedCellsText) || (currentMessage == _toManySelectedRowsText) || (currentMessage == _toManySelectedColumnsText)) {
                SetLabelText_IdAndCSS("", "SelectionErrorlabel", "variableselector_selectionerror_label", false);
            }
            disableFromClass("variableselector_continue_button", false);
        }
    }

    
    
</script>
    
