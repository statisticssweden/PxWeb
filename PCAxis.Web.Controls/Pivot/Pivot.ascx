<%@ register assembly="PCAxis.Web.Controls" namespace="PCAxis.Web.Controls" tagprefix="pxc" %>
<div>
    <div>
        <asp:button id="ContinueButton" runat="server" />
    </div>
    <div style="float:left;width:320px">
        <div style="float:left;width:20px;padding:5px">
            <asp:imagebutton id="StubUpButton" runat="server" alternatetext="move up"  />
            <asp:imagebutton id="StubDownButton" runat="server" alternatetext="move down"  />
        </div>
        <div style="float:left;width:80px;padding:5px">
            <asp:label id="StubLabel" runat="server" text=""></asp:label>
            <asp:listbox id="StubListBox" runat="server"></asp:listbox>
        </div>
        <div style="float:left;width:60px;padding:5px"> 
            <asp:imagebutton id="MoveRightButton" runat="server" alternatetext="move right" />
            <asp:imagebutton id="MoveLeftButton" runat="server" alternatetext="move left" />            
        </div>
        <div style="float:left;width:80px;padding:5px">
            <asp:label id="HeadingLabel" runat="server" text=""></asp:label>
            <asp:listbox id="HeadingListBox" runat="server"></asp:listbox>
        </div>
        <div style="float:left;width:20px;padding:5px">
            <asp:imagebutton id="HeadingUpButton" runat="server" alternatetext="move up" />
            <asp:imagebutton id="HeadingDownButton" runat="server" alternatetext="move down" />
        </div>
    </div>
 </div>
 