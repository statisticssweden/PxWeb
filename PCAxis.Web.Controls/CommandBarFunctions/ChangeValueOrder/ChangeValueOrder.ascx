<%@ control inherits="PCAxis.Web.Controls.ChangeValueOrderCodebehind" %>
<div class="commandbar_changevalueorder_container_main">
    <div class="commandbar_changevalueorder_caption">
        <asp:Label ID="CaptionLabel" runat="server"></asp:Label>
    </div>
    <div>
        <div class="commandbar_changevalueorder_instructions">
            <asp:Literal ID="Instructions" runat="server"></asp:Literal>
        </div>
        <div class="commandbar_changevalueorder_mainbuttons">
            <asp:Button ID="CancelButton" runat="server" />
            <asp:Button ID="ContinueButton" runat="server" />
        </div>
    </div>
    
    <asp:Panel ID="ChooseVariablePanel" runat="server">
        <div class="commandbar_changevalueorder_variablelist">
            <asp:RadioButtonList ID="VariableList" runat="server">
            </asp:RadioButtonList>
        </div>
    </asp:Panel>
    <asp:Panel ID="ChangeOrderPanel" runat="server" Visible="false" >
        <div class="commandbar_changevalueorder_fromorder_container" >
            <div class="commandbar_changevalueorder_fromorder_downbutton"> 
                <p>&nbsp;</p>
                <asp:ImageButton ID="FromOrderDownButton" runat="server" />
            </div>
            <div class="commandbar_changevalueorder_fromorder_controls" >
                <div class="commandbar_changevalueorder_fromorder_list">
                    <p><asp:Label ID="FromOrderLabel" runat="server"></asp:Label></p>
                    <asp:ListBox ID="ValuesFromOrder" runat="server" class="commandbar_changevalueorder_fromorder_listbox" ></asp:ListBox>
                </div>
                <div class="commandbar_changevalueorder_fromorder_movebuttons">   
                    <p>&nbsp;</p>
                    <p>
                        <asp:ImageButton ID="MoveRightButton" runat="server" />
                    </p>
                    <p>
                        <asp:ImageButton ID="MoveLeftButton" runat="server" />
                    </p>                
                </div>
            </div>
        </div> 
        <div class="commandbar_changevalueorder_toorder_container">
            <div class="commandbar_changevalueorder_toorder_list">
                <p><asp:Label ID="ToOrderLabel" runat="server"></asp:Label></p>
                <asp:ListBox ID="ValuesToOrder" runat="server" class="commandbar_changevalueorder_toorder_listbox"></asp:ListBox>
            </div>
            <div class="commandbar_changevalueorder_toorder_downbutton">
                <p>&nbsp;</p>
                <asp:ImageButton ID="ToOrderDownButton" runat="server" />
            </div>
        </div>
    </asp:Panel>
</div>