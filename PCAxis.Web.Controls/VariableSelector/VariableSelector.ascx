<%@ control inherits="PCAxis.Web.Controls.VariableSelectorCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>
<%@ Import Namespace="PCAxis.Paxiom"%>
<asp:Panel ID="VariableSelectorPanel" runat="server" >
    
    <pxc:VariableSelectorMarkingTips runat="server" ID="VariableSelectorMarkingTips"  />    
    <asp:ValidationSummary ID="SelectionValidationSummary" runat="server" DisplayMode="BulletList"  ShowValidationErrors="true"  ShowMessageBox="false"  ShowSummary="true" CssClass="variableselector_error_summary"  ForeColor=""  ValidationGroup="ChangeStatus"  />
<asp:Repeater ID="VariableSelectorValueSelectRepeater" runat="server" EnableViewState="true">      
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
 
   
    <div class ="variableselector_show_table_container">
        <asp:Button ID="ButtonViewTable" runat="server" CssClass="pxweb-btn primary-btn variableselector_continue_button" ValidationGroup="ChangeStatus"/>
            </div>
    <div class ="variableselector_info_selected_cells">
     <pxc:VariableSelectorSelectionInformation runat="server" ID="VariableSelectorSelectionInformation" />
                        </div>
 
        <asp:Label ID="SelectionErrorlabel" runat="server" visible="true" CssClass="variableselector_selectionerror_label"/>
        <asp:Label ID="SelectionErrorlabelTextCells" runat="server" CssClass="variableselector_selectionerror_label_text" />
        <asp:Label ID="SelectionErrorlabelTextColumns" runat="server" CssClass="variableselector_selectionerror_label_text" />
        <asp:Label ID="SelectionErrorlabelTextRows" runat="server" CssClass="variableselector_selectionerror_label_text" />

     
 

</asp:Panel>

<asp:Panel ID="SearchVariableValuesPanel" runat="server" Visible="false">
    <pxc:SearchValues ID="SearchVariableValues" runat="server" EnableViewState="true" />     
</asp:Panel>

<asp:Panel runat="server" ID="HierarchicalSelectPanel" Visible="false">   
        <pxc:Hierarchical runat="server" ID="SelectHierarchichalVariable" ShowButtonLabels="true" EnableViewState="true" />
</asp:Panel>

<asp:Panel ID="SelectFromGroupPanel" runat="server" Visible="false">
    <pxc:SelectFromGroup ID="SelectValuesFromGroup" runat="server" />
</asp:Panel>
<script>

    // if the elements in the group collection differs in height
    // then all gets the height of the tallest element
    function equalHeight(group) {
        var tallest = 0;
        var i = 0;
        var prevHeight = 0;
        var allHeightsAreEqual = true;
        for (i=0; i < group.length; i++) {                
            var height = group.eq(i).height() + 30;
            if (height > tallest) 
                tallest = height;
            if (i > 0 && prevHeight != height)
                allHeightsAreEqual = false;                       
            prevHeight = height;
        }

        if (!allHeightsAreEqual) {
            for (i = 0; i < group.length; i++) {
                group.eq(i).height(tallest);
            }
        }
    }

    var delayedEqualHeight = function (group) {
        setTimeout(function () { equalHeight(group); }, 350);
    }

    jQuery(document).ready(function () {
        var containerclass = document.getElementsByClassName('variableselector_variable_box_container');
        var boxelement = document.getElementsByClassName('variableselector_valuesselect_box');
    if(containerclass.length > 0 && boxelement.length > 0)
    {
            if (isSelectionLayoutCompact()) {
                containerclass[0].classList.add('variableselector_variable_box_container_compact');
                for (index = 0; index < boxelement.length; ++index) {
                    boxelement[index].classList.add('variableselector_valuesselect_box_compact');
                }  
                jQuery(".variableselector_valuesselect_box").resizable({ handles: 'e', minWidth: 150 });
                var group = jQuery(".variableselector_valuesselect_box");
                delayedEqualHeight(group);
            }
            else {
                containerclass[0].classList.add('variableselector_variable_box_container_list');
                for (index = 0; index < boxelement.length; ++index) {
                    boxelement[index].classList.add('variableselector_valuesselect_box_list');
                }   
            }
    }

        //Prevent resize to propagate down to option-tags
        jQuery("select").mousedown(function(event) {
            event.stopPropagation();
        });
        PCAxis_HideElement(".variableselector_valuesselect_action");
    });




//    (function () {
//  var originalValidationSummaryOnSubmit = window.ValidationSummaryOnSubmit;
//  window.ValidationSummaryOnSubmit = function (validationGroup) {
//    var originalScrollTo = window.scrollTo;
//    window.scrollTo = function() { };
//    originalValidationSummaryOnSubmit(validationGroup);
//    window.scrollTo = originalScrollTo;
//  }
//}());



//window.scrollTo = function () { };
  
//validationSummary.onpropertychange = function () {
//     if (this.style.display != 'none') {
//          validationSummary.scrollIntoView();
//     }
//}

</script>    
