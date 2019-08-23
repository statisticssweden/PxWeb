<%@ control inherits="PCAxis.Web.Controls.SearchValuesCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<asp:Panel ID="pnlSearchValues" runat="server" DefaultButton="cmdSearch">
<div class="searchvalues_container">
    <div class="searchvalues_title">
        <asp:Literal ID="Table" runat="server"></asp:Literal>
        <asp:Literal ID="SearchHeader" runat="server" />
    </div>
    
    <div>
        <div class="searchvalues_options_container">
<%--            <asp:Label ID="SearchForCodeLabel" runat="server" AssociatedControlID="SearchForCode"/>
            <br />
            <asp:TextBox ID="SearchForCode" runat="server"/>
            <asp:Button ID="SearchForCodeButton" runat="server" />
            <br />
            <asp:Label ID="SearchForValueLabel" runat="server" AssociatedControlID="SearchForValue"/>
            <br />
            <asp:TextBox ID="SearchForValue" runat="server"/>
            <asp:Button ID="SearchForValueButton" runat="server" />
            <br />
--%>            
            <asp:Label ID="lblSearch" runat="server"></asp:Label><br />
            <asp:RadioButton ID="rbSearchCode" runat="server" GroupName="SearchCategory" Checked="true" CssClass="variableselector_radioButtonList"/>            
            <asp:RadioButton ID="rbSearchValue" runat="server" GroupName="SearchCategory" CssClass="variableselector_radioButtonList"/>            
            <asp:RadioButton ID="rbShowAll" runat="server" GroupName="SearchCategory" CssClass="variableselector_radioButtonList"/>            
            <br />
            <asp:Label ID="lblSearchText" runat="server" AssociatedControlID="txtSearchText" ></asp:Label>
            <asp:TextBox ID="txtSearchText" runat="server"></asp:TextBox>
            <asp:Button ID="cmdSearch" CssClass="variableselector_search_button" runat="server" />
            <br />
            <asp:Label ID="lblSearchError" runat="server" Text="" CssClass="errorMessage"></asp:Label>
            <asp:RadioButton ID="SearchTypeBeginning" runat="server" GroupName="SearchType" Checked="true" CssClass="variableselector_radioButtonList" />
            <br />
            <asp:RadioButton ID="SearchTypeAnywhere" runat="server" GroupName="SearchType" CssClass="variableselector_radioButtonList" />
            <br />
<%--            <asp:Button ID="ShowAllButton" runat="server" CssClass="variableselector_showAllButton" />
            <br />
--%>            
<!--            <asp:Button ID="SearchResultSelectAll" runat="server" CssClass="variableselector_searchresultsselectall" /> -->
            <br />
            <asp:HyperLink ID="lnkSearchInformation" runat="server" CssClass="variableselector_searchinformationlink" NavigateUrl="http://www.dn.se">Information</asp:HyperLink>
            <div class="searchvalues_selectiontips">
                <asp:Literal ID="litSelectionTips" runat="server"></asp:Literal>
            </div>
        </div>
        <asp:Label ID="SearchResultLabel" class="searchvalues_searchresults" runat="server"></asp:Label>
        <asp:Label ID="SearchResultNumberOfHitsLabel" class="searchvalues_searchresultsnumberofhits" runat="server"></asp:Label>
        <br />

        <table id="grdHeaders">
            <tr><td></td></tr>
        </table>
        
        <div class="searchvalues_searchresults_container">
            <asp:GridView ID="grdSearchResult" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="50" ondatabound="grdPagedGrid_DataBound" CssClass="searchvalues_searchresults_grid">
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="paged_grid_cell select_cell" HeaderStyle-CssClass="select_cell search_results_header">
                        <ItemTemplate >
                            <asp:CheckBox runat="server" ID="chkSelected"/>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="paged_grid_cell code_cell" HeaderStyle-CssClass="code_cell search_results_header">  
                        <ItemTemplate>  
                            <asp:Label runat="server" ID="lblCode" Text='<%# DataBinder.Eval(Container.DataItem, "Code")%>'/>
                        </ItemTemplate>  
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="paged_grid_cell value_cell" HeaderStyle-CssClass="value_cell search_results_header">  
                        <ItemTemplate>  
                            <asp:Label runat="server" ID="lblValue" Text='<%# DataBinder.Eval(Container.DataItem, "Value")%>'/>
                        </ItemTemplate>  
                    </asp:TemplateField>  
                </Columns>
                <PagerSettings Position="Bottom" PageButtonCount="11" Mode="NumericFirstLast" />
                <PagerStyle CssClass="hide" />
                <PagerTemplate>
                    <asp:Panel ID="pnlPagingrow1" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="pnlPagingrow2" runat="server">
                        <asp:LinkButton ID="lnkFirstPage" runat="server" />
                        <asp:Label ID="lbNumberOfPages" runat="server"/>
                        <asp:LinkButton ID="lnkLastPage" runat="server" />
                    </asp:Panel>
                </PagerTemplate>
            </asp:GridView>
        </div>
        <div style="clear:both">
        <asp:Panel ID="pnlSecondPager" runat="server" CssClass="pagerFooter pagerFooterJavascript hide">
            <asp:Panel ID="pnlPagingrow1Second" runat="server">
            </asp:Panel>
            <asp:Panel ID="pnlPagingrow2Second" runat="server">
                <asp:LinkButton ID="lnkFirstPageSecond" runat="server" />
                <asp:Label ID="lbNumberOfPagesSecond" runat="server"/>
                <asp:LinkButton ID="lnkLastPageSecond" runat="server" />
            </asp:Panel>
        </asp:Panel> 
        <asp:Panel ID="pnlNonJavascriptPager" runat="server" CssClass="pagerFooter pagerFooterNonJavascript">
            <asp:Panel ID="pnlPagingrow1NonJavascript" runat="server">
            </asp:Panel>
            <asp:Panel ID="pnlPagingrow2NonJavascript" runat="server">
                <asp:Button ID="cmdFirstPageNonJavascript" runat="server" />
                <asp:Label ID="lblNumberOfPagesNonJavascript" runat="server"/>
                <asp:Button ID="cmdLastPageNonJavascript" runat="server"  />
            </asp:Panel>
        </asp:Panel> 
        </div>
    </div>
    <div>
        <asp:Image ID="LeftDividerImage" runat="server" CssClass="searchvalues_leftdivider" AlternateText="Divider" />
        <asp:ImageButton ID="AddButton" class="searchvalues_addbutton" runat="server"/>
        <asp:ImageButton ID="RemoveButton" class="searchvalues_removebutton" runat="server"/>
        <asp:Image ID="RightDividerImage" runat="server" CssClass="searchvalues_rightdivider" AlternateText="Divider" />
    </div>  
    <div>
        <div class="searchvalues_leftdiv">
            <asp:Button ID="SelectAllAvailableValues" CssClass="searchvalues_select_all_available_values" runat="server" Visible="false" />        
        </div>
        <div class="searchvalues_choosenvalues_container">
            <asp:Label ID="ChoosenValuesLabel" class="searchvalues_choosenvalues" runat="server"></asp:Label>
            <asp:Label ID="NumberOfChoosenValuesLabel" class="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
            <br />
            <asp:ListBox ID="SelectedVariableValues" runat="server" class="searchvalues_selectedvalues_listbox" />
            <br />
        </div>    
    </div> 
    <div class="searchvalues_buttonscontainer">
        <asp:Button ID="CancelButton" runat="server" class="searchvalues_cancelbutton"/>
        <asp:Button ID="ClearButton" runat="server" class="searchvalues_clearbutton" />
        <asp:Button ID="AddToVariableSelectorButton" runat="server" class="searchvalues_addtovariableselectorbutton" />
    </div>
</div>
</asp:Panel>


<script type="text/javascript">
    //CONSTANTS and GLOBALS
    var COLLECTION_CONTAINER_CLASS = "searchvalues_searchresults_grid";
    var SELECTED_STYLE_CLASS = "selected_row";
    var CLASS_TO_HIDE = "select_cell";
    var COLLECTION_WITH_MOUSEEVENTS_SELECTOR = "tr.pagedGridRow:not(.pagerFooter)";
    var COLLECTION_WITH_MOUSEEVENTS;
    var CONTROL_KEY_PRESSED = false;
    var SHIFT_KEY_PRESSED = false;


    jQuery(document).ready(function() {
        gw_SetHeader("searchvalues_searchresults_grid", "grdHeaders"); // Copy headers outside grid

        //Get the elements to set mouse eventhandlers on
        COLLECTION_WITH_MOUSEEVENTS = jQuery(COLLECTION_WITH_MOUSEEVENTS_SELECTOR);
        
        // Hide column with checkboxes
        jQuery("." + CLASS_TO_HIDE).hide();

        jQuery(".searchvalues_searchresults_grid tr:odd").addClass("alternating_row");

        jQuery('.pagerFooterJavascript').removeClass("hide");
        jQuery(".pagerFooterNonJavascript").addClass("hide");
        //ScrollToPosition();

        //Keep track on the ctrl-key and shift-key
        jQuery(document).keydown(function(event) {
            CONTROL_KEY_PRESSED = event.ctrlKey;
            SHIFT_KEY_PRESSED = event.shiftKey;
        });
        jQuery(document).keyup(function(event) {
            CONTROL_KEY_PRESSED = event.ctrlKey;
            SHIFT_KEY_PRESSED = event.shiftKey;
        });

        //mouseup, mousedown, mouseenter for all tablerows that not is of class pageFooter
        var stopRowIndex = 0;
        var startRowIndex = 0;
        var mouseIsDown = false;
        var lastEnteredRowIndex = 0;

        //Mousedown
        COLLECTION_WITH_MOUSEEVENTS.mousedown(function() {
            mouseIsDown = true;
            startRowIndex = pagedGridRow_MouseDown(this, startRowIndex);

            //Mouseup
        }).mouseup(function(event) {
            mouseIsDown = false;
            stopRowIndex = pagedGridRow_MouseUp(this);
            SelectRows(startRowIndex, stopRowIndex);

            //Mouseenter
        }).mouseenter(function() {
            if (mouseIsDown) {
                lastEnteredRowIndex = pagedGridRow_MouseEnter(this);
            }
        });

        //Mouseout container
        //Handle that mouse leavs the container between mousedown and mouseup
        jQuery("." + COLLECTION_CONTAINER_CLASS).mouseleave(function() {
            if (mouseIsDown) {
                mouseIsDown = false;
                ClearText();
                SelectRows(startRowIndex, lastEnteredRowIndex);
            }
        });


    });


    //-----------------------------------------------------------------------
    //Deselect previous selected rows, select current row.
    //Returns rowindex where mousebutton was pushed if shift not is pressed, else currentStartRowIndex 
    //------------------------------------------------------------------------
    function pagedGridRow_MouseDown(currentRow, currentStartRowIndex) {
        var rowAlreadySelected = false;

        if (CONTROL_KEY_PRESSED) {
            //Toggle between checked/unchecked checkboxes
            jQuery(currentRow).find("input:checkbox").each(function(i) {//Check checkboxes in current row
                rowAlreadySelected = jQuery(this).is(':checked');
                jQuery(this).prop('checked', (!rowAlreadySelected));
            });
            //Toggle between selected/not selected class
            jQuery(currentRow).find("td").each(function(i) {
                if (!rowAlreadySelected) {
                    jQuery(this).addClass(SELECTED_STYLE_CLASS);
                }
                else {
                    jQuery(this).removeClass(SELECTED_STYLE_CLASS);
                }
            });
            return COLLECTION_WITH_MOUSEEVENTS.index(currentRow);
        }
        else {
            //Deselect selected
            if (!CONTROL_KEY_PRESSED) {
                jQuery('.' + SELECTED_STYLE_CLASS).removeClass(SELECTED_STYLE_CLASS); //Deselect all
                jQuery("input:checkbox").each(function(i) {//Uncheck all checkboxes
                    jQuery(this).prop('checked', false);
                });
            }

            //Select current
            jQuery(currentRow).find("td").each(function(i) {
                jQuery(this).addClass(SELECTED_STYLE_CLASS);
            });
            jQuery(currentRow).find("input:checkbox").each(function(i) {//Check checkboxes in current row
                jQuery(this).prop('checked', true);
            });

            //Set rowindex for mousedown
            if (!SHIFT_KEY_PRESSED) {
                //return rowIndexMouseDown = COLLECTION_WITH_MOUSEEVENTS.index(currentRow);
                return COLLECTION_WITH_MOUSEEVENTS.index(currentRow);
            } else {
                return currentStartRowIndex;
            }
        }
    }

    //-----------------------------------------------------------------------
    //Clear the textselection on page, sets selected-style on current row or 
    //clears the selected-style if the row already has it.
    //Returns index of row entered
    //-----------------------------------------------------------------------
    function pagedGridRow_MouseEnter(currentRow) {
        ClearText();
        jQuery(currentRow).find("td").each(function(i) {
            if (jQuery(this).hasClass(SELECTED_STYLE_CLASS)) {
                jQuery(this).removeClass(SELECTED_STYLE_CLASS);
            }
            else {
                jQuery(this).addClass(SELECTED_STYLE_CLASS);
            }
        });
        return COLLECTION_WITH_MOUSEEVENTS.index(currentRow);
    }

    //-----------------------------------------------------------------------
    //Call SelectRows.
    //Returns rowindex where mousebutton was released.
    //-----------------------------------------------------------------------
    function pagedGridRow_MouseUp(currentRow) {
        ClearText(); //Clear selected text
        return COLLECTION_WITH_MOUSEEVENTS.index(currentRow);
    }


    //-------------------------------------------------------------------------------------------
    //Clears all selected-styles.
    //Sets selected-style on all rows of type pagedGridRow from mousedownIndex to mouseupIndex and
    //check all checkboxes in those
    //-------------------------------------------------------------------------------------------
    function SelectRows(mousedownIndex, mouseupIndex) {

        var startRow = mousedownIndex;
        var stopRow = mouseupIndex;
        if (mousedownIndex == mouseupIndex) {
            return;
        }
        if (mousedownIndex > mouseupIndex) {
            startRow = mouseupIndex;
            stopRow = mousedownIndex;
        }

        jQuery('.' + SELECTED_STYLE_CLASS).removeClass(SELECTED_STYLE_CLASS); //Deselect all
        for (var i = startRow; i <= stopRow; i++) {
            var elmt = COLLECTION_WITH_MOUSEEVENTS.get(i);
            jQuery(elmt).find("td").each(function(i) {
                jQuery(this).addClass(SELECTED_STYLE_CLASS);
            });
            jQuery(elmt).find("input:checkbox").each(function(i) {//Check checkboxes in current row
                jQuery(this).prop('checked', true);
            });
        };
    }

    //---------------------------------
    //Clear the textselection on page
    //---------------------------------
    function ClearText() {
        try {
            var userSelection;
            if (window.getSelection) {
                userSelection = window.getSelection();
                window.getSelection().removeAllRanges();
            }
            else if (document.selection) { // should come last; Opera!
                userSelection = document.selection.createRange();
                document.selection.empty();
            }
        } catch (ex) {
        }
    }


    /*originGrid = id for grid containing data. 
    headerGrid = id for grid containing headers*/
    function gw_SetHeader(originGridClass, headerGrid) {
        //jQuery("#" + originGrid + " tr th").appendTo("#" + headerGrid + " tr");
        jQuery("." + originGridClass + " tr th").appendTo("#" + headerGrid + " tr");
        //        jQuery(".gw_selectbuttonItem").hide();
        //        jQuery(".gw_selectbuttonHeader").hide();
    }

</script>
