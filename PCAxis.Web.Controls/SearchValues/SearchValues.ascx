<%@ Control Inherits="PCAxis.Web.Controls.SearchValuesCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<asp:Panel ID="pnlSearchValues" runat="server" CssClass="searchvalues-container"  DefaultButton="SearchValuesButton">
            <h2>
                <asp:Label ID="SearchHeader" CssClass="pxweb-title negative" runat="server" />
            </h2>
            <div class="flex-row-wrap">
                <div class="searchvalues-usage-tips negative flex-column">
                    <asp:Literal ID="litSelectionTips" runat="server"></asp:Literal>
                </div>
                <asp:Button ID="FetchAllButton" CssClass="fetch-all-button pxweb-buttons database-icon pxweb-btn negative icon-placement" runat="server" Visible="false"/>
             </div>
           
            <hr class="green-rule"/>
            <h3>
                <asp:Label ID="lblSearch" runat="server" Text="" class="pxweb-title negative" />
            </h3>
 
            <div class="flex-column">
                    <div class="variableselector_valuesselect_search_panel flex-row">
                        <div class="pxweb-input search-panel">
                            <div class="input-wrapper">
                                <asp:TextBox ID="SearchValuesTextbox" CssClass="with-icon" runat="server"></asp:TextBox>
                                <asp:LinkButton ID="SearchValuesButton" CssClass="icon-wrapper search-icon" runat="server">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="search-icon"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                  <span class="hidden">wave temp fix..</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <asp:CheckBox runat="server" ID="SearchValuesBeginningOfWordCheckBox" Visible="true" Enabled="true" CssClass="variableselector_valuesselect_search_textstart_checkbox pxweb-checkbox negative" />
                    </div>
                    <asp:Label ID="lblSearchError" runat="server" Text="" CssClass="pxweb-input-error negative"></asp:Label>
                 

                 <div class="searchvalues-listbox-container">
                    <asp:Label ID="SearchResultNumberOfHitsLabel" runat="server"></asp:Label>
                    <asp:ListBox ID="SearchResults" runat="server" class="searchvalues-listbox" />
                  </div>

                  <div class="flex-row-wrap">
                        
                        <asp:Button runat="server" ID="MoveToResultButton" CssClass="pxweb-buttons arrow-down-icon pxweb-btn negative icon-placement" Enabled="false"/>
                        <asp:Button runat="server" ID="DeselectAllButton" CssClass="variableselector_valuesselect_deselect_all_button pxweb-btn negative icon-placement variableselector-buttons"/>
                        <asp:Button runat="server" ID="SelectAllButton" CssClass="hidden pxweb-buttons checked-icon pxweb-btn negative icon-placement variableselector-buttons" Enabled="false"/>
                  </div>
                <div class="searchvalues_options_container">
                     <asp:HyperLink ID="lnkSearchInformation" runat="server" CssClass="variableselector_searchinformationlink" NavigateUrl="http://www.dn.se">Information</asp:HyperLink>
                </div>
            </div> 
            <div class="some-vertical-space1"></div>
            <hr class="green-rule"/>
          
                <h3>
                 <asp:Label ID="ChoosenValuesLabel" class="pxweb-title negative" runat="server"></asp:Label>
                </h3>
               <div class="flex-column">
                <div class="flex-row">
                <asp:Button ID="RemoveButton" Enabled="true" class="searchvalues_removebutton pxweb-buttons arrow-up-icon pxweb-btn negative icon-placement" runat="server" />
                </div>
                <div class="searchvalues-listbox-container">
                    <asp:Label ID="NumberOfChoosenValuesLabelPart1" class="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
                     <asp:Label ID="NumberOfChoosenValuesLabelPart2" class="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
                     <asp:Label ID="NumberOfChoosenValuesLabelPart3" class="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
                    <asp:ListBox ID="SelectedVariableValues" runat="server" class="searchvalues-listbox"/>
                </div>
                
                <asp:Button ID="CancelButton" runat="server" class="searchvalues-cancel pxweb-buttons cancel-icon pxweb-btn negative icon-placement" />
          </div>
 
</asp:Panel>
<div class="some-vertical-space2"></div>
<div class="flex-row done-container">
    <asp:Button ID="DoneButton" runat="server" class="pxweb-btn primary-btn" Enabled="false" />
</div>  

