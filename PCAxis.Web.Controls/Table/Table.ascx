<%@ control inherits="PCAxis.Web.Controls.TableCodeBehind" enableviewstate="False" %>
<%-- Denna div kan användas för att få scrollbar kring tabellen --%>
<%--<div style="position:relative;width:600px;height:100%; overflow:scroll;display: block; clear:both;">--%>
<div id="pcaxis_tablediv">
    <asp:table id="DataTable" runat="server"   enableviewstate="false" cssclass="table-class">
    </asp:table>
</div>

<div id="pcaxis_table_defaultattributes" runat="server">
    <asp:Label ID="lblDefaultAttributes" runat="server" CssClass="table_defaultattributes_label"></asp:Label><br />
    <asp:Repeater ID="rptDefaultAttributes" runat="server" >
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem,"key") %>&nbsp;=&nbsp;<%#DataBinder.Eval(Container.DataItem, "value")%><br />
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
</div>


<div id="pxtableCellInformationDialog" style="display: none" runat="server"></div>
<asp:HiddenField ID="pxtableCellInformationDialogCloseText" runat="server" />

<%--<br clear="both" />--%>
<script type="text/javascript">
    jQuery(document).ready(function () {
        // Remove links for attribute cell values (we only want them when we have no javascript)
        jQuery('.attribute-cell a').each(function () {
            jQuery(jQuery(this).parent()).html(jQuery(this).text());
        })
        // Add click event for all attribute cells
        jQuery('.attribute-cell').click(function () {
            displayCellInformation(jQuery(this).attr("data-value"), jQuery("[id$=pxtableCellInformationDialogCloseText]").val());
            return false;
        });
    });  
</script> 

