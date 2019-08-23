<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetadataSystemControl.ascx.cs" Inherits="PXWeb.UserControls.MetadataSystemControl" %>

<div >
    <asp:DropDownList runat="server" ID="cboVariables" Visible="true" AutoPostBack="true" CssClass="metadata_selectVariable" OnSelectedIndexChanged="cboVariables_SelectedIndexChanged">
    </asp:DropDownList>

    <asp:Panel ID="pnlVariable" runat="server" Visible="false">
        <div class="meta_header">
            <asp:Label ID="lblVariableHeader" runat="server" CssClass="metadata_header"></asp:Label>
        </div><asp:Repeater ID="repVariable" runat="server" OnItemDataBound="repVariable_ItemDataBound">
            <ItemTemplate>
                <div class="meta_row">
                    <div class="meta_left">
                        <asp:Label ID="lblVariableName" runat="server" CssClass="metadata_itemname"></asp:Label>
                    </div>
                    <div class="meta_right">
                        <asp:Repeater ID="repVariableLinks" runat="server" OnItemDataBound="repVariableLinks_ItemDataBound">
                            <ItemTemplate>
                                <div id="divVarLink" runat="server" class="metadata_itemlink"></div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoMetaVariable" runat="server" Visible="false" CssClass="metadata_no_meta"></asp:Label>

        <div class="meta_header">
            <asp:Label ID="lblValuesHeader" runat="server" CssClass="metadata_header"></asp:Label>
        </div>
        <asp:Repeater ID="repValue" runat="server" OnItemDataBound="repValue_ItemDataBound">
            <ItemTemplate>
                <div class="meta_row">
                    <div class="meta_left">
                        <asp:Label ID="lblValueName" runat="server" CssClass="metadata_itemname"></asp:Label>
                    </div>
                    <div class="meta_right">
                        <asp:Repeater ID="repValueLinks" runat="server" OnItemDataBound="repValueLinks_ItemDataBound">
                            <ItemTemplate>
                                <div id="divValueLink" runat="server" class="metadata_itemlink"></div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoMetaValues" runat="server" CssClass="metadata_no_meta" Visible="false"></asp:Label>
    </asp:Panel>
</div>
