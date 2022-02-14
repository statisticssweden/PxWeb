<%@ Control Inherits="PCAxis.Web.Controls.SearchValuesCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<asp:Panel ID="pnlSearchValues" runat="server" CssClass="pxbox negative searchvalues-container flex-colunm"  DefaultButton="SearchValuesButton">
    <h2>
        <asp:Label ID="SearchHeader" CssClass="pxweb-title negative" runat="server" />
    </h2>
    <div id="pxcontent">
        <pxc:UserManualScreenReader ID="UserManualSearchValues"
            headerCode="CtrlSearchValuesUserManualScreenReaderRegion"
            textCode="CtrlSearchValuesUserManualScreenReader"
            runat="server" ClientIDMode="Static"/>
    </div>
    <div class="flex-row flex-wrap">
        <div class="searchvalues-usage-tips negative flex-column">
            <asp:Literal ID="litSelectionTips" runat="server"></asp:Literal>
        </div>
        <asp:Button ID="FetchAllButton" CssClass="fetch-all-button align-self-flex-start pxweb-buttons database-icon pxweb-btn negative icon-placement" runat="server" Visible="false"/>
     </div>
    <hr class="pxweb-divider type-light with-margin"/>
    <h3>
        <asp:Label ID="lblSearch" runat="server" Text="" CssClass="pxweb-title negative" />
    </h3>
  <asp:Panel ID="SearchRegion" role="region" CssClass="flex-column" runat="server" >
  <asp:CheckBox runat="server" ID="SearchValuesBeginningOfWordCheckBox" Visible="true" Enabled="true" CssClass="variableselector_valuesselect_search_textstart_checkbox pxweb-checkbox negative" />
      <div class="flex-row flex-wrap s-margin-top">
            <div class="pxweb-input search-panel">
                <div class="input-wrapper">
                    <asp:TextBox ID="SearchValuesTextbox" CssClass="with-icon" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="SearchValuesButton" CssClass="icon-wrapper search-icon" runat="server">
                      <span class="hidden">wave temp fix..</span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        </asp:Panel>
        <asp:Label ID="lblSearchError" runat="server" Text="" CssClass="pxweb-input-error negative"></asp:Label>
        <div class="searchvalues-listbox-container">
            <asp:Label ID="SearchResultNumberOfHitsLabel" CssClass="pxweb-info-text" runat="server"></asp:Label>
            <asp:ListBox ID="SearchResults" runat="server" CssClass="searchvalues-listbox" />
          </div>
        <div class="flex-row flex-wrap">
              <asp:Button runat="server" ID="MoveToResultButton" CssClass="pxweb-buttons arrow-down-icon pxweb-btn negative icon-placement" Enabled="false"/>
                <asp:Button runat="server" ID="DeselectAllButton" CssClass="variableselector_valuesselect_deselect_all_button pxweb-btn negative icon-placement variableselector-buttons"/>
                <asp:Button runat="server" ID="SelectAllButton" CssClass="hidden pxweb-buttons checked-icon pxweb-btn negative icon-placement variableselector-buttons" Enabled="false"/>
          </div>
        <div class="searchvalues_options_container">
             <asp:HyperLink ID="lnkSearchInformation" runat="server" CssClass="variableselector_searchinformationlink" NavigateUrl="http://www.dn.se">Information</asp:HyperLink>
        </div>
    </asp:Panel>
    <div class="s-margin-top"></div>
    <hr class="pxweb-divider type-light with-margin"/>
    <h3>
     <asp:Label ID="ChoosenValuesLabel" CssClass="pxweb-title negative" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="SelectedValuesRegion" role="region" CssClass="flex-column" runat="server">
        <div class="flex-row">
        <asp:Button ID="RemoveButton" Enabled="true" CssClass="searchvalues_removebutton pxweb-buttons arrow-up-icon pxweb-btn negative icon-placement" runat="server" />
        </div>
        <div class="searchvalues-listbox-container">
            <span class="pxweb-info-text">
                <asp:Label ID="NumberOfChoosenValuesLabelPart1" CssClass="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
                <asp:Label ID="NumberOfChoosenValuesLabelPart2" CssClass="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
                <asp:Label ID="NumberOfChoosenValuesLabelPart3" CssClass="searchvalues_numberofchoosenvalues" runat="server"></asp:Label>
            </span>
            <asp:ListBox ID="SelectedVariableValues" runat="server" CssClass="searchvalues-listbox"/>
        </div>
        <asp:Button ID="CancelButton" runat="server" CssClass="align-self-flex-end pxweb-buttons cancel-icon pxweb-btn negative icon-placement" />
   </asp:Panel>
</asp:Panel>
<div class="m-margin-top"></div>
<div class="flex-row justify-center">
    <asp:Button ID="DoneButton" runat="server" CssClass="pxweb-btn primary-btn" Enabled="false" />
</div>  

