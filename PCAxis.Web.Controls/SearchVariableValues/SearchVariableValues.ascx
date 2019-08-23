<%@ control inherits="PCAxis.Web.Controls.SearchVariableValuesCodebehind" %>
<%@ Register Assembly="PCAxis.Web.Controls" Namespace="PCAxis.Web.Controls" TagPrefix="pxc" %>

<div class="searchvariablevalues_container">
    
    <div class="searchvariablevalues_title"><asp:Literal ID="SearchHeader" runat="server" /></div>
    
    <div class="searchvariabelvalues_searchmatcheslist">
        <asp:GridView ID="SearchResult" runat="server" AutoGenerateColumns="False" GridLines="None">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="IsSelected" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Code" HeaderText="" />
                <asp:BoundField DataField="Value" HeaderText="" />            
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <div class="searchvariablevalues_text" id='a<%#Eval("Code")%>'><%#Eval("Value")%></div>                    
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div onclick="javascript:ShowHideText('a<%#Eval("Code")%>');">+/-</div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    
    <asp:ListBox ID="SelectedVariableValues" runat="server" CssClass="searchvariabelvalues_selectedvalues_listbox" />
    <asp:Button ID="Add" runat="server" />

    <div>
        <asp:Label ID="SearchForCodeLabel" runat="server" AssociatedControlID="SearchForCode"/>
        <br />
        <asp:TextBox ID="SearchForCode" runat="server"/>
        <br />
        <asp:Label ID="SearchForValueLabel" runat="server" AssociatedControlID="SearchForValue"/>
        <br />
        <asp:TextBox ID="SearchForValue" runat="server"/>
        <br />
        <asp:RadioButton ID="SearchTypeBeginning" runat="server" GroupName="SearchType" Checked="true" />
        <br />
        <asp:RadioButton ID="SearchTypeAnywhere" runat="server" GroupName="SearchType" />
        <br />
        <asp:Button ID="Search" runat="server" />
        <asp:Button ID="AddToVariableSelector" runat="server" />
    </div>
</div>