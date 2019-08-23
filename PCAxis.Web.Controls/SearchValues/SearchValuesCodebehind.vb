

Imports PCAxis.Web.Core
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Enums
Imports PCAxis.Web.Core.Management

Public Class SearchValuesCodebehind
    Inherits PaxiomControlBase(Of SearchValuesCodebehind, SearchValues)

#Region "Localized strings"
    Private Const LOC_SEARCH_HEADER As String = "CtrlSearchValuesHeader"
    Private Const LOC_SEARCH_SEARCH_FOR_CODE As String = "CtrlSearchValuesSearchCode"
    Private Const LOC_SEARCH_SEARCH_FOR_VALUE As String = "CtrlSearchValuesSearchValue"
    Private Const LOC_SEARCH_SEARCH_FOR_VALUE_OR_CODE As String = "CtrlSearchValuesSearchValueOrCode"
    Private Const LOC_SEARCH_TYPE_BEGINNING As String = "CtrlSearchValuesSearchTypeBeginning"
    Private Const LOC_SEARCH_TYPE_ANYWHERE As String = "CtrlSearchValuesSearchTypeAnywhere"
    Private Const LOC_SEARCH_SHOW_ALL_VALUES As String = "CtrlSearchValuesShowAllValues"
    Private Const LOC_SEARCH As String = "CtrlSearchValuesSearch"
    Private Const LOC_SEARCH_ILLEGAL_CHARACTERS_ERROR As String = "PxWebIllegalCharactersErrorMessage"
    Private Const LOC_SEARCH_SELECTION_TIPS As String = "CtrlSearchValuesSelectionTips"
    Private Const LOC_SEARCH_RESULTS_HEADER As String = "CtrlSearchValuesSearchResultHeader"
    Private Const LOC_SEARCH_RESULTS_NUMBER_OF_HITS As String = "CtrlSearchValuesSearchResultNumberOfHits"
    Private Const LOC_SEARCH_RESULTS_SELECT_ALL As String = "CtrlSearchValuesSearchResultSelectAll"
    Private Const LOC_SEARCH_ADD_TO_CHOOSEN_VALUES As String = "CtrlSearchValuesAddToChoosenValues"
    Private Const LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES As String = "CtrlSearchValuesRemoveFromChoosenValues"
    Private Const LOC_SEARCH_CHOOSEN_VALUES As String = "CtrlSearchValuesChoosenValues"
    Private Const LOC_SEARCH_NUMBER_OF_CHOOSEN_VALUES As String = "CtrlSearchValuesNumberOfChoosenValues"
    Private Const LOC_SEARCH_NO_VALUES_FOUND As String = "CtrlSearchValuesNoValuesFound"
    Private Const LOC_SEARCH_NO_VALUES_SELECTED As String = "CtrlSearchValuesNoValuesSelected"
    Private Const LOC_SEARCH_CANCEL As String = "CtrlSearchValuesCancel"
    Private Const LOC_SEARCH_CLEAR As String = "CtrlSearchValuesClear"
    Private Const LOC_SEARCH_ADD_TO_VARIABLESELECTOR As String = "CtrlSearchValuesAddToVariableSelector"
    Private Const LOC_SEARCH_SELECT_ALL_AVAILABLE_VALUES As String = "CtrlSearchValuesSelectAllAvailableValues"


    Private Const LOC_SEARCH_LABEL As String = "CtrlSearchValuesSearchLabel"
    Private Const LOC_CODE As String = "CtrlSearchValuesCode"
    Private Const LOC_VALUE As String = "CtrlSearchValuesValue"
    Private Const LOC_SHOW_ALL_VALUES As String = "CtrlSearchValuesAllValues"
    Private Const LOC_SEARCH_TEXT As String = "CtrlSearchValuesSearchText"
    Private Const LOC_SEARCH_BUTTON As String = "CtrlSearchValuesSearchButton"
    Private Const LOC_PAGER_FIRSTPAGE As String = "CtrlSearchValuesPagerFirstPage"
    Private Const LOC_PAGER_NUMBEROFPAGES As String = "CtrlSearchValuesPagerNumberOfPages"
    Private Const LOC_PAGER_LASTPAGE As String = "CtrlSearchValuesPagerLastPage"
    Private Const LOC_PAGER_FORWARD As String = "CtrlSearchValuesPagerForward"
    Private Const LOC_PAGER_BACKWARD As String = "CtrlSearchValuesPagerBackward"

#End Region

#Region "Fields"
    Protected Table As Literal
    Protected SearchHeader As Literal
    Protected lblSearch As Label
    Protected lblSearchText As Label
    Protected rbSearchCode As RadioButton
    Protected rbSearchValue As RadioButton
    Protected rbShowAll As RadioButton
    Protected txtSearchText As TextBox
    Protected WithEvents cmdSearch As Button
    Protected lblSearchError As Label
    Protected SearchTypeBeginning As RadioButton
    Protected SearchTypeAnywhere As RadioButton
    Protected SelectedVariableValues As ListBox
    Protected WithEvents CancelButton As Button
    Protected WithEvents ClearButton As Button
    Protected WithEvents AddToVariableSelectorButton As Button
    Protected litSelectionTips As Literal
    Protected SearchResultLabel As Label
    Protected SearchResultNumberOfHitsLabel As Label
    Protected ChoosenValuesLabel As Label
    Protected NumberOfChoosenValuesLabel As Label
    Protected LeftDividerImage As Image
    Protected RightDividerImage As Image
    Protected WithEvents RemoveButton As ImageButton
    Protected WithEvents AddButton As ImageButton
    Protected lnkSearchInformation As HyperLink
    Protected WithEvents grdSearchResult As GridView
    Protected pnlSecondPager As Panel
    Protected pnlPagingrow1Second As Panel
    Protected pnlPagingrow2Second As Panel
    Protected WithEvents lnkFirstPageSecond As LinkButton
    Protected lbNumberOfPagesSecond As Label
    Protected WithEvents lnkLastPageSecond As LinkButton

    Protected pnlNonJavascriptPager As Panel
    Protected pnlPagingrow1NonJavascript As Panel
    Protected pnlPagingrow2NonJavascript As Panel
    Protected WithEvents cmdFirstPageNonJavascript As Button
    Protected lblNumberOfPagesNonJavascript As Label
    Protected WithEvents cmdLastPageNonJavascript As Button

    Protected WithEvents SelectAllAvailableValues As Button
#End Region

    ''' <summary>
    ''' Gets all the selected values.
    ''' </summary>
    ''' <value>All selected values as list of strings</value>
    ''' <returns>List of strings</returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property SelectedValues() As List(Of String)
        Get
            Dim values As New List(Of String)
            For Each li As ListItem In SelectedVariableValues.Items
                values.Add(li.Text)
            Next
            Return values
        End Get
    End Property


    ''' <summary>
    ''' Initializes the control.
    ''' </summary>
    Private Sub SearchVariableValues_Load() Handles Me.Load
        If Not IsPostBack Then
            SetLocalizedText()
            HandleButtons()
            lnkSearchInformation.Visible = Marker.ShowSearchInformationLink
            lnkSearchInformation.Text = Marker.SearchInformationLinkText
            lnkSearchInformation.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SearchInformationLinkURL)
            pnlNonJavascriptPager.Visible = False
            Table.Visible = Marker.ShowTableName
            If Marker.ShowAllAvailableValuesButton Then
                SelectAllAvailableValues.Visible = True
            End If
        Else
            For Each ctr As String In Page.Request.Form
                If ctr.EndsWith("AddButton.x") Or ctr.EndsWith("AddButton.y") Then
                    Exit Sub
                End If
            Next

            BindGrid()
        End If

    End Sub


    ''' <summary>
    ''' Sets up the buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HandleButtons()
        'Set ImageUrl for buttons from embedded resource
        Dim imgurl As String = Page.ClientScript.GetWebResourceUrl(GetType(VariableSelectorValueSelectCodebehind), "PCAxis.Web.Controls.spacer.gif")

        Me.LeftDividerImage.ImageUrl = imgurl
        Me.RightDividerImage.ImageUrl = imgurl
        Me.AddButton.ImageUrl = imgurl
        Me.RemoveButton.ImageUrl = imgurl
    End Sub

    ''' <summary>
    ''' Initiation of the search control
    ''' </summary>
    ''' <remarks>Consider previous selections done for variable if saved in state-variabel</remarks>
    Public Sub InitiateSearch()
        ''If the user chooses to stop the search the search text shall be removed from the textbox
        ClearSearchTxtBox()

        HideAndClearSearchError()
        grdSearchResult.DataSource = Nothing
        grdSearchResult.DataBind()

        SelectedVariableValues.SelectionMode = ListSelectionMode.Multiple

        'Add previously selected values to the "Selected values" listbox
        Dim valuesToShow As New Values(Marker.Variable)
        Dim value As Value

        For Each val As String In VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes
            value = Marker.Variable.Values.GetByCode(val)
            If Not value Is Nothing Then
                valuesToShow.Add(value)
            End If
        Next


        SelectedVariableValues.DataTextField = "Text"
        SelectedVariableValues.DataValueField = "Code"

        SelectedVariableValues.DataSource = valuesToShow
        SelectedVariableValues.DataBind()

        Table.Text = Marker.Variable.Meta.Title & "<br />"
        SearchHeader.Text = Me.GetLocalizedString(LOC_SEARCH_HEADER) & " " & Marker.Variable.Name
        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), Me.grdSearchResult.Rows.Count.ToString)
        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()

        ''if the codes are fictional, the radio button for Codes should not be visible or selectable or default set to selected = true
        ''Reqtest buggreport #188
        If Marker.Variable.Values.IsCodesFictional Then
            rbSearchCode.Visible = False
            rbSearchValue.Checked = True
            rbSearchCode.Checked = False
        Else
            rbSearchCode.Visible = True
            rbSearchCode.Checked = True
        End If
        FixEmptyListboxes()
    End Sub

    ''' <summary>
    ''' Get the string telling how many values are selected
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNumberOfChoosenValuesText() As String
        Dim number As Integer

        number = GetNumberOfChoosenValues()
        Return String.Format(Me.GetLocalizedString(LOC_SEARCH_NUMBER_OF_CHOOSEN_VALUES), DataFormatter.NumericToString(number, 0, LocalizationManager.GetTwoLetterLanguageCode()))
    End Function

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

    ''' <summary>
    ''' Gets new language strings.
    ''' </summary>
    Private Sub SearchValues_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        SetLocalizedText()
    End Sub

    ''' <summary>
    ''' Sets up all localized content.
    ''' </summary>
    Private Sub SetLocalizedText()
        SearchHeader.Text = Me.GetLocalizedString(LOC_SEARCH_HEADER)
        SearchTypeBeginning.Text = Me.GetLocalizedString(LOC_SEARCH_TYPE_BEGINNING)
        SearchTypeAnywhere.Text = Me.GetLocalizedString(LOC_SEARCH_TYPE_ANYWHERE)
        litSelectionTips.Text = Me.GetLocalizedString(LOC_SEARCH_SELECTION_TIPS)
        SearchResultLabel.Text = Me.GetLocalizedString(LOC_SEARCH_RESULTS_HEADER)
        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), Me.grdSearchResult.Rows.Count.ToString)
        AddButton.ToolTip = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_CHOOSEN_VALUES)
        AddButton.AlternateText = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_CHOOSEN_VALUES)
        RemoveButton.ToolTip = Me.GetLocalizedString(LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES)
        RemoveButton.AlternateText = Me.GetLocalizedString(LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES)
        ChoosenValuesLabel.Text = Me.GetLocalizedString(LOC_SEARCH_CHOOSEN_VALUES)
        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()
        CancelButton.Text = Me.GetLocalizedString(LOC_SEARCH_CANCEL)
        ClearButton.Text = Me.GetLocalizedString(LOC_SEARCH_CLEAR)
        AddToVariableSelectorButton.Text = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_VARIABLESELECTOR)

        lblSearch.Text = Me.GetLocalizedString(LOC_SEARCH_LABEL)
        rbSearchCode.Text = Me.GetLocalizedString(LOC_CODE)
        rbSearchValue.Text = Me.GetLocalizedString(LOC_VALUE)
        rbShowAll.Text = Me.GetLocalizedString(LOC_SHOW_ALL_VALUES)
        lblSearchText.Text = Me.GetLocalizedString(LOC_SEARCH_TEXT)
        cmdSearch.Text = Me.GetLocalizedString(LOC_SEARCH_BUTTON)
        lnkFirstPageSecond.Text = Me.GetLocalizedString(LOC_PAGER_FIRSTPAGE)
        lbNumberOfPagesSecond.Text = Me.GetLocalizedString(LOC_PAGER_NUMBEROFPAGES)
        lnkLastPageSecond.Text = Me.GetLocalizedString(LOC_PAGER_LASTPAGE)
        cmdFirstPageNonJavascript.Text = Me.GetLocalizedString(LOC_PAGER_FIRSTPAGE)
        lblNumberOfPagesNonJavascript.Text = Me.GetLocalizedString(LOC_PAGER_NUMBEROFPAGES)
        cmdLastPageNonJavascript.Text = Me.GetLocalizedString(LOC_PAGER_LASTPAGE)

        SelectAllAvailableValues.Text = Me.GetLocalizedString(LOC_SEARCH_SELECT_ALL_AVAILABLE_VALUES)
    End Sub


    ''' <summary>
    ''' Raises an event to signal when value selection is finished.
    ''' </summary>
    Protected Sub AddToVariableSelector_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddToVariableSelectorButton.Click
        Dim selection As New Selection(Marker.Variable.Code)
        For Each item As ListItem In SelectedVariableValues.Items
            If Not String.IsNullOrEmpty(item.Value) Then
                selection.ValueCodes.Add(item.Value)
            End If
        Next
        VariableSelector.SelectedVariableValues(Marker.Variable.Code) = selection

        Marker.OnSearchVariableValuesAdd(New EventArgs())
    End Sub

    ''' <summary>
    ''' Close the search control without adding selected values to selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        ''If the user chooses to stop the search the search text shall be removed from the textbox
        ClearSearchTxtBox()
        Marker.OnSearchVariableValuesAdd(New EventArgs())
    End Sub

    ''' <summary>
    ''' Clear values in the selected values list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearButton.Click
        VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Clear()
        SelectedVariableValues.Items.Clear()
        FixEmptyListboxes()
        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()
    End Sub

    ''' <summary>
    ''' Add selected values to the choosen values listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AddButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AddButton.Click
        Dim liS As ListItem
        Dim v As Value
        Dim chk As CheckBox
        Dim lbl As Label

        If SelectedVariableValues.Items.Count = 1 AndAlso String.IsNullOrEmpty(SelectedVariableValues.Items(0).Value) Then
            SelectedVariableValues.Items.Clear()
        End If

        For Each row As GridViewRow In grdSearchResult.Rows
            chk = CType(row.FindControl("chkSelected"), CheckBox)
            If Not chk Is Nothing Then
                If chk.Checked Then
                    lbl = CType(row.FindControl("lblCode"), Label)
                    If Not lbl Is Nothing AndAlso Not String.IsNullOrEmpty(lbl.Text) Then
                        v = Me.Marker.Variable.Values.GetByCode(lbl.Text)
                        If Not v Is Nothing Then
                            Select Case Me.Marker.Variable.PresentationText
                                Case 0
                                    liS = New ListItem(v.Code, v.Code)
                                Case Else
                                    liS = New ListItem(v.Text, v.Code)
                            End Select
                            If Not SelectedVariableValues.Items.Contains(liS) Then
                                SelectedVariableValues.Items.Add(liS)
                            End If
                        End If
                    End If
                End If
            End If
        Next

        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()
        BindGrid()
    End Sub

    ''' <summary>
    ''' Remove selected values from the choosen values listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RemoveButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RemoveButton.Click
        For i As Integer = SelectedVariableValues.Items.Count - 1 To 0 Step -1
            If SelectedVariableValues.Items(i).Selected Then
                SelectedVariableValues.Items.Remove(SelectedVariableValues.Items(i))
            End If
        Next
        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()
        FixEmptyListboxes()
    End Sub


    ''' <summary>
    ''' Enable/disable SearchResultSelectAll and AddToVariableSelectorButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        AddToVariableSelectorButton.Enabled = (SelectedVariableValues.Items.Count > 0)
    End Sub

    Private Sub BindGrid()
        Dim v As New List(Of Value)
        Dim strSearch As String

        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(txtSearchText.Text) Then
            lblSearchError.Visible = True
            lblSearchError.Text = Me.GetLocalizedString(LOC_SEARCH_ILLEGAL_CHARACTERS_ERROR)
        Else
            HideAndClearSearchError()
        End If

        strSearch = txtSearchText.Text.Trim

        If (Not String.IsNullOrEmpty(strSearch)) Or rbShowAll.Checked Then
            If rbSearchCode.Checked Then
                If SearchTypeBeginning.Checked Then
                    v.AddRange(Marker.Variable.Values.SearchInBeginningOfCode(strSearch))
                Else
                    v.AddRange(Marker.Variable.Values.SearchInCode(strSearch))
                End If
            ElseIf rbSearchValue.Checked Then
                If SearchTypeBeginning.Checked Then
                    v.AddRange(Marker.Variable.Values.SearchInBeginningOfValue(strSearch))
                Else
                    v.AddRange(Marker.Variable.Values.SearchInValue(strSearch))
                End If
            ElseIf rbShowAll.Checked Then
                v = Marker.Variable.Values
            End If
        End If


        grdSearchResult.DataSource = v
        grdSearchResult.DataBind()
        grdSearchResult.RowStyle.CssClass = "pagedGridRow"

        If grdSearchResult.PageCount < 2 Then
            pnlSecondPager.Visible = False
        Else
            pnlSecondPager.Visible = True
        End If

        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), DataFormatter.NumericToString(v.Count, 0, LocalizationManager.GetTwoLetterLanguageCode()))
        ''If the codes are fictional, the column for codes should not be displayed. New css class is added so the row/rows would extend over the entire box
        ''Reqtest buggreport #188
        If Not Marker.Variable Is Nothing AndAlso Marker.Variable.Values.IsCodesFictional Then
            grdSearchResult.Columns(1).Visible = False
            grdSearchResult.Columns(2).ItemStyle.CssClass = "paged_grid_cell only_value_cell"
            grdSearchResult.Columns(2).HeaderStyle.CssClass = "only_value_cell search_results_header"
        Else
            grdSearchResult.Columns(1).HeaderText = Me.GetLocalizedString(LOC_CODE)
        End If
        grdSearchResult.Columns(2).HeaderText = Me.GetLocalizedString(LOC_VALUE)
        FixEmptyListboxes()
    End Sub

    Private Sub grdSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSearchResult.PageIndexChanging
        grdSearchResult.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        grdSearchResult.PageIndex = 0
        BindGrid()
        pnlNonJavascriptPager.Visible = True

    End Sub



    Protected Sub grdPagedGrid_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        SetSecondPagingLinks(CType(sender, GridView))
        SetNonJavascriptPagingLinks(CType(sender, GridView))
    End Sub

    ''' <summary>
    ''' Used for paging in the grid if that is used
    ''' </summary>
    ''' <param name="grid"></param>
    ''' <remarks></remarks>
    Protected Sub SetPagingLinks(ByVal grid As GridView)


        Dim pagingButton As LinkButton
        Dim pagingLabel As Label

        'Retrieve the pager row.
        Dim pagerRow As GridViewRow = grid.BottomPagerRow

        'Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As Panel = CType(pagerRow.Cells(0).FindControl("pnlPagingrow1"), Panel)

        Dim linksBesideCurrent As Integer = CInt(grid.PagerSettings.PageButtonCount / 2)
        Dim startVal As Integer = grid.PageIndex - linksBesideCurrent
        Dim stopVal As Integer = grid.PageIndex + linksBesideCurrent


        'Print out back-link
        If (startVal > 0) Then
            pagingButton = New LinkButton()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_BACKWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (startVal - 1).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerBack"
            pageList.Controls.Add(pagingButton)
        End If

        'Print out paginglinks
        For i As Integer = 0 To grid.PageCount - 1 Step 1

            If i >= startVal AndAlso i <= stopVal Then

                If i <> grid.PageIndex Then

                    pagingButton = New LinkButton()
                    pagingButton.Text = String.Format("{0}&nbsp;", i + 1)
                    pagingButton.CommandName = "Page"
                    pagingButton.CommandArgument = i.ToString()
                    AddHandler pagingButton.Command, AddressOf PagingButton_Click
                    pagingButton.ID = "Page" & i.ToString()
                    pageList.Controls.Add(pagingButton)

                Else

                    pagingLabel = New Label()
                    pagingLabel.Text = String.Format("{0}&nbsp;", i + 1)
                    pagingLabel.ID = "Page" & i.ToString()
                    pageList.Controls.Add(pagingLabel)
                End If
            End If

        Next

        'Print out forward-link
        If (stopVal + 1 < grid.PageCount) Then
            pagingButton = New LinkButton()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_FORWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (stopVal + linksBesideCurrent).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerForward"
            pageList.Controls.Add(pagingButton)
        End If


        'Second row
        Dim lnkFirstPage As LinkButton = CType(pagerRow.Cells(0).FindControl("lnkFirstPage"), LinkButton)
        lnkFirstPage.CommandName = "Page"
        lnkFirstPage.CommandArgument = "0"
        AddHandler lnkFirstPage.Command, AddressOf PagingButton_Click
        Dim lbNumberOfPages As Label = CType(pagerRow.Cells(0).FindControl("lbNumberOfPages"), Label)
        lbNumberOfPages.Text = String.Format("|| {0} {1} ||", Me.GetLocalizedString(LOC_PAGER_NUMBEROFPAGES), grid.PageCount.ToString())
        Dim lnkLastPage As LinkButton = CType(pagerRow.Cells(0).FindControl("lnkLastPage"), LinkButton)
        lnkLastPage.CommandName = "Page"
        lnkLastPage.CommandArgument = grid.PageCount.ToString()
        AddHandler lnkLastPage.Command, AddressOf PagingButton_Click

        SetSecondPagingLinks(grid)
        SetNonJavascriptPagingLinks(grid)
    End Sub


    Protected Sub SetSecondPagingLinks(ByVal grid As GridView)


        Dim pagingButton As LinkButton
        Dim pagingLabel As Label

        'Retrieve the pager row.
        Dim pagerRow As GridViewRow = grid.BottomPagerRow

        'Retrieve the PageDropDownList DropDownList from the bottom pager row.
        pnlPagingrow1Second.Controls.Clear()
        Dim pageList As Panel = pnlPagingrow1Second

        Dim linksBesideCurrent As Integer = CInt(grid.PagerSettings.PageButtonCount / 2)
        Dim startVal As Integer = grid.PageIndex - linksBesideCurrent
        Dim stopVal As Integer = grid.PageIndex + linksBesideCurrent


        'Print out back-link
        If (startVal > 0) Then
            pagingButton = New LinkButton()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_BACKWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (startVal - 1).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerBackNJ"
            pageList.Controls.Add(pagingButton)
        End If

        'Print out paginglinks
        For i As Integer = 0 To grid.PageCount - 1 Step 1

            If i >= startVal AndAlso i <= stopVal Then

                If i <> grid.PageIndex Then

                    pagingButton = New LinkButton()
                    pagingButton.Text = String.Format("{0}&nbsp;", i + 1)
                    pagingButton.CommandName = "Page"
                    pagingButton.CommandArgument = i.ToString()
                    AddHandler pagingButton.Command, AddressOf PagingButton_Click
                    pagingButton.ID = "PageNJ" & i.ToString()
                    pageList.Controls.Add(pagingButton)

                Else

                    pagingLabel = New Label()
                    pagingLabel.Text = String.Format("{0}&nbsp;", i + 1)
                    pagingLabel.ID = "PageNJ" & i.ToString()
                    pageList.Controls.Add(pagingLabel)
                End If
            End If

        Next

        'Print out forward-link
        If (stopVal + 1 < grid.PageCount) Then
            pagingButton = New LinkButton()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_FORWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (stopVal + linksBesideCurrent).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerForwardNJ"
            pageList.Controls.Add(pagingButton)
        End If


        'Second row
        Dim lnkFirstPage As LinkButton = lnkFirstPageSecond
        lnkFirstPage.CommandName = "Page"
        lnkFirstPage.CommandArgument = "0"
        AddHandler lnkFirstPage.Command, AddressOf PagingButton_Click
        Dim lbNumberOfPages As Label = lbNumberOfPagesSecond
        lbNumberOfPages.Text = String.Format("|| {0} {1} ||", Me.GetLocalizedString(LOC_PAGER_NUMBEROFPAGES), grid.PageCount.ToString())
        Dim lnkLastPage As LinkButton = lnkLastPageSecond
        lnkLastPage.CommandName = "Page"
        lnkLastPage.CommandArgument = grid.PageCount.ToString()
        AddHandler lnkLastPage.Command, AddressOf PagingButton_Click

    End Sub

    Protected Sub SetNonJavascriptPagingLinks(ByVal grid As GridView)


        Dim pagingButton As Button
        Dim pagingLabel As Label

        'Retrieve the pager row.
        Dim pagerRow As GridViewRow = grid.BottomPagerRow

        'Retrieve the PageDropDownList DropDownList from the bottom pager row.
        pnlPagingrow1NonJavascript.Controls.Clear()
        Dim pageList As Panel = pnlPagingrow1NonJavascript

        Dim linksBesideCurrent As Integer = CInt(grid.PagerSettings.PageButtonCount / 2)
        Dim startVal As Integer = grid.PageIndex - linksBesideCurrent
        Dim stopVal As Integer = grid.PageIndex + linksBesideCurrent


        'Print out back-link
        If (startVal > 0) Then
            pagingButton = New Button()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_BACKWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (startVal - 1).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerBack"
            pageList.Controls.Add(pagingButton)
        End If

        'Print out paginglinks
        For i As Integer = 0 To grid.PageCount - 1 Step 1

            If i >= startVal AndAlso i <= stopVal Then

                If i <> grid.PageIndex Then

                    pagingButton = New Button()
                    pagingButton.Text = String.Format("{0}", i + 1)
                    pagingButton.CommandName = "Page"
                    pagingButton.CommandArgument = i.ToString()
                    AddHandler pagingButton.Command, AddressOf PagingButton_Click
                    pagingButton.ID = "Page" & i.ToString()
                    pageList.Controls.Add(pagingButton)

                Else

                    pagingLabel = New Label()
                    pagingLabel.Text = String.Format("{0}", i + 1)
                    pagingLabel.ID = "Page" & i.ToString()
                    pageList.Controls.Add(pagingLabel)
                End If
            End If

        Next

        'Print out forward-link
        If (stopVal + 1 < grid.PageCount) Then
            pagingButton = New Button()
            pagingButton.Text = Me.GetLocalizedString(LOC_PAGER_FORWARD)
            pagingButton.CommandName = "Page"
            pagingButton.CommandArgument = (stopVal + linksBesideCurrent).ToString()
            AddHandler pagingButton.Command, AddressOf PagingButton_Click
            pagingButton.ID = "pagerForward"
            pageList.Controls.Add(pagingButton)
        End If


        'Second row
        Dim lnkFirstPage As Button = cmdFirstPageNonJavascript
        lnkFirstPage.CommandName = "Page"
        lnkFirstPage.CommandArgument = "0"
        AddHandler lnkFirstPage.Command, AddressOf PagingButton_Click
        Dim lbNumberOfPages As Label = lblNumberOfPagesNonJavascript
        lbNumberOfPages.Text = String.Format("|| {0} {1} ||", Me.GetLocalizedString(LOC_PAGER_NUMBEROFPAGES), grid.PageCount.ToString())
        Dim lnkLastPage As Button = cmdLastPageNonJavascript
        lnkLastPage.CommandName = "Page"
        lnkLastPage.CommandArgument = grid.PageCount.ToString()
        AddHandler lnkLastPage.Command, AddressOf PagingButton_Click

    End Sub


    Protected Sub PagingButton_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        'Set pageindex
        grdSearchResult.PageIndex = Convert.ToInt32(e.CommandArgument)
        ' Re-bind the data to refresh the DataGrid control. 
        BindGrid()
    End Sub

    ''' <summary>
    ''' Adds "empty"-rows to the listboxes if they contains no items
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FixEmptyListboxes()
        Dim li As ListItem

        If SelectedVariableValues.Items.Count = 0 Then
            li = New ListItem(Me.GetLocalizedString(LOC_SEARCH_NO_VALUES_SELECTED), "")
            'li.Enabled = False
            SelectedVariableValues.Items.Add(li)
        End If
    End Sub

    ''' <summary>
    ''' When called, removes text in textbox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSearchTxtBox()
        txtSearchText.Text = ""
    End Sub

    ''' <summary>
    ''' When called, set error lable to visible = false and remove text from lable
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HideAndClearSearchError()
        lblSearchError.Text = ""
        lblSearchError.Visible = False
    End Sub

    ''' <summary>
    ''' Read only property for exposing the ClientID of the Search button
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SearchButtonClientID() As String
        Get
            Return Me.cmdSearch.ClientID
        End Get
    End Property

    Private Sub SelectAllAvailableValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAllAvailableValues.Click
        Dim liS As ListItem

        SelectedVariableValues.Items.Clear()

        For Each v As Value In Marker.Variable.Values
            Select Case Me.Marker.Variable.PresentationText
                Case 0
                    liS = New ListItem(v.Code, v.Code)
                Case Else
                    liS = New ListItem(v.Text, v.Code)
            End Select
            If Not SelectedVariableValues.Items.Contains(liS) Then
                SelectedVariableValues.Items.Add(liS)
            End If
        Next

        NumberOfChoosenValuesLabel.Text = GetNumberOfChoosenValuesText()
    End Sub

End Class
