August 2020:
SimpliFying the GUI, will something break in Sweden?:

1)
The listbox containing the desired values har a magic row when it it empty. The row just says that it is empty, but for the column below, it wonder if it also hides a bug in Epi-Server.


  ''' <summary>
    ''' Get the number of values that are selected
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Handles special case when no value is selected in Epi-Server</remarks>
    Private Function GetNumberOfChoosenValues() As Integer
        'Special case for Epi-Server when no value is selected
        If Me.SelectedVariableValues.Items.Count = 1 Then
            If String.IsNullOrEmpty(Me.SelectedVariableValues.Items(0).Value) Then
                Return 0
            End If
        End If
        Return Me.SelectedVariableValues.Items.Count
    End Function
	
2) The gridview and pager is replaced by a simple listbox
In the old code:
''If the codes are fictional, the column for codes should not be displayed. New css class is added so the row/rows would extend over the entire box
''Reqtest buggreport #188

If Not Marker.Variable Is Nothing AndAlso Marker.Variable.Values.IsCodesFictional Then
 grdSearchResult.Columns(1).Visible = False
 grdSearchResult.Columns(2).ItemStyle.CssClass = "paged_grid_cell only_value_cell"
 rdSearchResult.Columns(2).HeaderStyle.CssClass = "only_value_cell search_results_header"
Else
 grdSearchResult.Columns(1).HeaderText = Me.GetLocalizedString(LOC_CODE) + "DDD"
End If
grdSearchResult.Columns(2).HeaderText = Me.GetLocalizedString(LOC_VALUE)	

Is Fictional codes Working and Reqtest buggreport #188 still ok?

3) Left this as i found it. Is it in use?
<div class="searchvalues_options_container">
                     <asp:HyperLink ID="lnkSearchInformation" runat="server" CssClass="variableselector_searchinformationlink" NavigateUrl="http://www.dn.se">Information</asp:HyperLink>
                </div>
				

