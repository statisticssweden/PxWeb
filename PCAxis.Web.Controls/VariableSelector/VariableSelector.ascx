<%@ control inherits="PCAxis.Web.Controls.VariableSelectorCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>
<%@ Import Namespace="PCAxis.Paxiom"%>

<asp:Panel ID="VariableSelectorPanel" runat="server" >
    
    <pxc:VariableSelectorMarkingTips runat="server" ID="VariableSelectorMarkingTips"  />
    <pxc:VariableSelectorEliminationInformation runat="server" ID="VariableSelectorEliminationInformation" />           
    
    <asp:Repeater ID="VariableSelectorValueSelectRepeater" runat="server" EnableViewState="false">      
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
    <br />
    
    <pxc:VariableSelectorSelectionInformation runat="server" ID="VariableSelectorSelectionInformation" />
    <br />
    
    <pxc:VariableSelectorOutputFormats runat="server" ID="OutputFormats" CssClass="variableselector_outputformats" />
    <asp:Button ID="ButtonViewTable" runat="server" CssClass="variableselector_continue_button"/>
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

<script type="text/javascript" >

    // if the elements in the group collection differs in height
    // then all gets the height of the tallest element
    function equalHeight(group) {
        var tallest = 0;
        var i = 0;
        var prevHeight = 0;
        var allHeightsAreEqual = true;
        for (i=0; i < group.length; i++) {                
            var height = group.eq(i).height();
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
        jQuery(".variableselector_valuesselect_box").resizable({ handles: 'e', minWidth: 150 });
        var group = jQuery(".variableselector_valuesselect_box");
        delayedEqualHeight(group);

        //Prevent resize to propagate down to option-tags
        jQuery("select").mousedown(function(event) {
            event.stopPropagation();
        });
        PCAxis_HideElement(".variableselector_valuesselect_action");
    });


</script>    
