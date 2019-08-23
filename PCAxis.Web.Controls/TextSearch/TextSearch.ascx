<%@ control inherits="PCAxis.Web.Controls.TextSearchCodebehind" %>
<div class="textsearch">
    <asp:panel id="SearchUIPanel" runat="server">
        <p>
            <asp:textbox id="SearchTextBox" runat="server" />
            <asp:button id="SearchButton" runat="server" />
        </p>
        <p>
            <asp:requiredfieldvalidator id="SearchTextBoxRequiredFieldValidator" 
                controltovalidate="SearchTextBox"
                runat="server" display="Dynamic" 
                setfocusonerror="True" ></asp:requiredfieldvalidator></p>
        <p>
            <asp:radiobuttonlist runat="server" id="SearchOption" datatextfield="Key" datavaluefield="Value"
                repeatlayout="Flow" repeatdirection="Horizontal" />
        </p>
        <asp:listbox id="SearchLibrariesListBox" runat="server" datatextfield="Name" datavaluefield="Value" />
    </asp:panel>
    <asp:panel id="ResultUIPanel" runat="server">
        <h1>
            <asp:literal runat="server" id="ResultHeader" /></h1>
        <asp:repeater id="ResultRepeater" runat="server" enableviewstate="False">
            <headertemplate>
                <ol>
            </headertemplate>
            <itemtemplate>
                <li>
                    <p runat='server' visible='<%# ShowHeading(CInt(Eval("Size"))) %>'>
                        <%#Eval("PresentationText")%></p>
                    <p>
                        <asp:hyperlink runat='server' visible='<%# Marker.ShowSelectLink%>' navigateurl='<%#GetLink(Eval("Selection").ToString(),"Select")%>'
                            text='<%#IIf(ShowHeading(Cint(Eval("Size"))),Me.GetLocalizedString("CtrlTextSearchSelect"), Eval("PresentationText").ToString())%>'></asp:hyperlink>
                        <asp:hyperlink runat='server' visible='<%# ShowDownloadLink(CInt(Eval("Size"))) %>'
                            navigateurl='<%#GetLink(Eval("Selection").ToString(),"Download")%>' text='<%#IIf(ShowHeading(Cint(Eval("Size"))),Me.GetLocalizedString("CtrlTextSearchDownload"), Eval("PresentationText").ToString())%>'></asp:hyperlink>
                        <asp:hyperlink runat='server' visible='<%# Marker.ShowViewLink%>' navigateurl='<%#GetLink(Eval("Selection").ToString(),"View")%>'
                            text='<%#IIf(ShowHeading(Cint(Eval("Size"))),Me.GetLocalizedString("CtrlTextSearchView"), Eval("PresentationText").ToString())%>'></asp:hyperlink>
                    </p>
                    <asp:repeater id="ResultAttributesRepeater" runat="server" enableviewstate="False">
                        <headertemplate>
                            <p class="textsearch-attributes">
                        </headertemplate>
                        <separatortemplate>
                            -
                        </separatortemplate>
                        <itemtemplate>
                            <%#Eval("Key")%>
                            <%#Eval("Value")%></itemtemplate>
                        <footertemplate>
                            </p></footertemplate>
                    </asp:repeater>
                </li>
            </itemtemplate>
            <footertemplate>
                </ol>
            </footertemplate>
        </asp:repeater>
    </asp:panel>
</div>
