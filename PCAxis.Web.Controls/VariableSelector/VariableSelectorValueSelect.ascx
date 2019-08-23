<%@ control  inherits="PCAxis.Web.Controls.VariableSelectorValueSelectCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>


<div class="variableselector_valuesselect_box">

<asp:Panel runat="server" ID="ValuesSelectContainerPanel" DefaultButton="SearchValuesButton">
    <asp:Panel runat="server" ID="Panel1" CssClass="variableselector_valuesselect_variabletitle_panel">
            <div class="variableselector_valuesselect_variabletitle_container">
                <asp:Label runat="server" ID="VariableTitle" Cssclass="variableselector_valuesselect_variabletitle"/>
                <asp:Image runat="server" ID="EliminationImage" Visible="false" CssClass="variableselector_valuesselect_elimination" />
            </div>
            <asp:DropDownList runat="server" ID="GroupingDropDown" Visible="false" AutoPostBack="true" CssClass="variableselector_valuesselect_aggregations"/>
            <asp:button id="ActionButton" runat="server" Visible="false" cssclass="variableselector_valuesselect_action" /> 
            <div><asp:Label runat="server" ID="VariableTitleSecond" CssClass="variableselector_valuesselect_variabletitlesecond"/></div>
    </asp:Panel>

    <asp:Panel runat="server" ID="EventButtons" CssClass="variableselector_valuesselect_eventbutton_panel">
        <p>
        <asp:ImageButton runat="server" ID="SearchButton" CssClass="variableselector_valuesselect_search_imagebutton" />
        <asp:ImageButton runat="server" ID="HierarchicalSelectButton" CssClass="variableselector_valuesselect_select_hierarcical_imagebutton"/>
        <asp:ImageButton runat="server" ID="SelectAllButton" CssClass="variableselector_valuesselect_select_all_imagebutton"/>
        <asp:ImageButton runat="server" ID="DeselectAllButton" CssClass="variableselector_valuesselect_deselect_all_imagebutton"/>
        <asp:ImageButton runat="server" ID="SortASCButton" CssClass="variableselector_valuesselect_sort_asc_imagebutton"/>
        <asp:ImageButton runat="server" ID="SortDescButton" CssClass="variableselector_valuesselect_sort_desc_imagebutton"/>
        <asp:ImageButton runat="server" ID="SelectionFromGroupButton" CssClass="variableselector_valuesselect_selectionFromGroup_imagebutton"/>
        <asp:ImageButton runat="server" ID="MetadataInformationButton" CssClass="variableselector_valuesselect_metadataInformation_imagebutton"/>   
        </p>
    </asp:Panel>
    <asp:Panel ID="HiddenEventButtons" runat="server" Visible="false" CssClass="variableselector_valuesselect_hiddeneventbutton_panel">
    </asp:Panel>
    <asp:Panel runat="server" ID="SelectedStatistics" CssClass="variableselector_valuesselect_statistics_panel">
        <p>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotalTitel" /></span>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesTotal"  /></span>
        <span class="variableselector_valuesselect_statistics"><asp:Literal runat="server" ID="NumberValuesSelectedTitel" /></span>
        <span class="variableselector_valuesselect_statistics"><asp:TextBox runat="server" id="NumberValuesSelected" CssClass="variableselector_valuesselect_statistics_NumberSelected_TextBox"/></span>
        </p>
    </asp:Panel>

    <asp:Panel runat="server" ID="ValuesSelectPanel" CssClass="variableselector_valuesselect_valuesselect_panel">
        <asp:ListBox runat="server" ID="ValuesListBox" CssClass="variableselector_valuesselect_valueslistbox" />
    </asp:Panel>

    <asp:Panel runat="server" ID="SearchPanel" CssClass="variableselector_valuesselect_search_panel">
        <p class="variableselector_valuesselect_searchvalues">
            <asp:Label ID="SearchValuesHeading" runat="server" />
            <asp:TextBox runat="server" ID="SearchValuesTextbox" CssClass="variableselector_valuesselect_search_textbox" />    
            <asp:ImageButton runat="server" ID="SearchValuesButton" CssClass="variableselector_valuesselect_search_button"/>
        </p>
        <p>
            <asp:CheckBox runat="server" ID="SearchValuesBeginningOfWordCheckBox" CssClass="variableselector_valuesselect_search_textstart_checkbox" />
        </p>
    </asp:Panel>
    
    <asp:Panel ID="ManyValuesPanel" runat="server" CssClass="variableselector_valuesselect_manyvalues_panel">
        <asp:Literal ID="ManyValues" runat="server"></asp:Literal>
    </asp:Panel>
   
</asp:Panel>

<!-- <asp:Panel runat="server" ID="MaxRowsWithoutSearchContainerPanel" CssClass="variableselector_valuesselect_largenumberofvalues_panel"> -->
<!--    <p class="variableselector_valuesselect_variabletitle"><asp:Literal runat="server" ID="VariableTitleMaxRows" /></p> -->
<!--    <asp:Button runat="server" ID="ShowAllValuesButton" Text="ShowAll" CssClass="" ImageUrl="~/resources/images/spacer.gif"/> -->
<!--    <asp:Button runat="server" ID="SearchLargeNumberOfValuesButton" Text="Search" CssClass="" ImageUrl="~/resources/images/spacer.gif"/> -->
<!--    <asp:LinkButton ID="ShowHierarchyLink" runat="server" CssClass="variableselector_valuesselect_showhierarchylink"></asp:LinkButton><br /> -->
<!--    <asp:LinkButton ID="SearchLargeNumberOfValuesLink" runat="server" CssClass="variableselector_valuesselect_searchlargenumberofvalueslink"></asp:LinkButton> -->
<!-- </asp:Panel> -->

</div>

<script type="text/javascript">



    jQuery(document).ready(function($) {
       window.onunload = function() { }; //prevent Firefox from caching the page in the Back-Forward Cache 
       var dd = jQuery("#<%=GroupingDropDown.ClientID%>");
       jQuery(dd).val(jQuery(dd).attr("data-value"));

    });

</script>
