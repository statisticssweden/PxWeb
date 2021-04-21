<%@ Page Language="C#" MasterPageFile="~/Presentation.Master" AutoEventWireup="true" CodeBehind="DataSort.aspx.cs" Inherits="PXWeb.DataSort" Title="<%$ PxString: PxWebTitleDataSort %>" %>
<%@ MasterType VirtualPath="~/Presentation.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="cphMain" runat="server">
    <script>
            
        jQuery(document).ready(function() {
            jQuery('table').not(".savequery_rblist").hide();
            jQuery('table').not(".savequery_rblist").removeClass('table-class').addClass("tablesorter").show();
            jQuery('table').tablesorter({
                
                sortList: [[0, 0]],

                textExtraction: function (node) {
                    var sortValue = jQuery(node).metadata().sortvalue || node.innerHTML;                
                    var symbolArray = <%= GetSymbolArray() %>;
                    var symbolIndex = jQuery.inArray(sortValue, symbolArray);

                    if (symbolIndex >= 0) {
                        if(!Number.MIN_SAFE_INTEGER) {    
                            Number.MIN_SAFE_INTEGER = -(Math.pow(2, 53) - 1); // -9007199254740991
                        }
                        
                        return Number.MIN_SAFE_INTEGER + symbolIndex;
                    }                   
             
                    if(/[a-zA-Z]/.test(sortValue)) {
                        return sortValue;
                    }

                    var lastCommaIndex = sortValue.lastIndexOf(',');
                    
                    if(lastCommaIndex !== -1) {
                        var decimalNumbers = sortValue.substring(lastCommaIndex + 1);
                        var nonDecimalNumbers = sortValue.substring(0, lastCommaIndex).replace(',', '').replace(' ', '');
                        var numberSortValue = parseFloat(nonDecimalNumbers + '.' + decimalNumbers);

                        if(!isNaN(numberSortValue)) {
                            return numberSortValue;
                        }
                    }

                    return sortValue;
                },
                
                debug: false
            });
        });
    </script>
        <div id="datasort_content">
            <div id="datasort_description" class="flex-column">
                <asp:Label ID="lblDescription" runat="server" CssClass="datasort_sortdescription" />
                <asp:Label ID="lblCopyDescription" runat="server" CssClass="datasort_copydescription" />         
            </div>
            <asp:Label ID="lblTableTitle" runat="server" CssClass="datasort_tabletitle" />
            <pxc:Table id="Table" runat="server" />
        </div>	
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>
