Imports System.Web.UI.WebControls
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management

Public Class SearchValuesCodebehind
    Inherits PaxiomControlBase(Of SearchValuesCodebehind, SearchValues)

#Region "Localized strings"
    Private Const LOC_SEARCH_HEADER As String = "CtrlSearchValuesHeader2"
    Private Const LOC_SEARCH_SELECTION_TIPS As String = "CtrlSearchValuesSelectionTips"
    Private Const LOC_SEARCH_SELECT_ALL_AVAILABLE_VALUES As String = "CtrlSearchValuesSelectAllAvailableValues2"

    Private Const LOC_SEARCH_LABEL As String = "CtrlSearchValuesSearchLabel"
    Private Const LOC_SEARCH_RESULTS_NUMBER_OF_HITS As String = "CtrlSearchValuesSearchResultNumberOfHits"
    Private Const LOC_SEARCH_RESULTS_LISTBOX_ARIA_LABEL As String = "CtrlSearchValuesSearchResultListboxScreenReader"
    Private Const LOC_SEARCH_ILLEGAL_CHARACTERS_ERROR As String = "PxWebIllegalCharactersErrorMessage"
    Private Const LOC_SEARCH_RESULTS_SELECT_ALL As String = "CtrlSearchValuesSearchResultSelectAll"
    Private Const LOC_SEARCH_ADD_TO_CHOOSEN_VALUES As String = "CtrlSearchValuesAddToChoosenValues"
    Private Const LOC_SEARCH_ADD_TO_CHOOSEN_VALUES_TOOLTIP As String = "CtrlSearchValuesAddToChoosenValuesToolTip"

    Private Const LOC_SEARCH_CHOOSEN_VALUES As String = "CtrlSearchValuesChoosenValues"
    Private Const LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES As String = "CtrlSearchValuesRemoveFromChoosenValues"
    Private Const LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES_TOOLTIP As String = "CtrlSearchValuesRemoveFromChoosenValuesToolTip"
    Private Const LOC_SEARCH_NUMBER_OF_CHOOSEN_VALUES As String = "CtrlSearchValuesNumberOfChoosenValues2"
    Private Const LOC_SEARCH_CHOOSEN_VALUES_LISTBOX_ARIA_LABEL As String = "CtrlSearchValuesChoosenValuesListboxScreenReader"
    Private Const LOC_SEARCH_CANCEL As String = "CtrlSearchValuesCancel"
    Private Const LOC_SEARCH_ADD_TO_VARIABLESELECTOR As String = "CtrlSearchValuesAddToVariableSelector"

    'These are also used:
    ' "aria-label", GetLocalizedString("CtrlVariableSelectorSearchValuesTextboxScreenReader"))
    ' SearchValuesTextbox     ... ("placeholder", GetLocalizedString("CtrlVariableSelectorSearchValuesTextbox"))
    ' GetLocalizedString("CtrlVariableSelectorSearchValuesBeginningOfWordCheckBox")
#End Region

#Region "Fields"
    Protected SearchHeader As Label
    Protected litSelectionTips As Literal
    Protected WithEvents FetchAllButton As Button

    'first ruler 
    Protected lblSearch As Label
    Protected SearchValuesTextbox As TextBox
    Protected WithEvents SearchValuesButton As LinkButton
    Protected SearchValuesBeginningOfWordCheckBox As CheckBox
    Protected lblSearchError As Label
    Protected SearchResultNumberOfHitsLabel As Label
    Protected WithEvents SearchResults As ListBox
    Protected WithEvents SelectAllButton As Button
    Protected WithEvents DeselectAllButton As Button
    Protected WithEvents MoveToResultButton As Button
    Protected lnkSearchInformation As HyperLink

    'Second ruler
    Protected ChoosenValuesLabel As Label
    Protected WithEvents RemoveButton As Button

    Protected NumberOfChoosenValuesLabelPart1 As Label
    Protected NumberOfChoosenValuesLabelPart2 As Label
    Protected NumberOfChoosenValuesLabelPart3 As Label
    Protected SelectedVariableValues As ListBox
    Protected WithEvents CancelButton As Button
    Protected WithEvents DoneButton As Button

    'regions




    Protected SearchRegion As Panel
    Protected SelectedValuesRegion As Panel
#End Region




    ''' <summary>
    ''' Initializes the control.
    ''' </summary>
    Private Sub SearchVariableValues_Load() Handles Me.Load
        If Not IsPostBack Then
            'is called when main selectionpage is loaded, so we do not know the variable yet.

            'One could perhaps reset this via Marker (or something) in InitiateSearch(), but why :-).
            SearchValuesBeginningOfWordCheckBox.Checked = True

            SetLocalizedText()
            lnkSearchInformation.Visible = Marker.ShowSearchInformationLink
            lnkSearchInformation.Text = Marker.SearchInformationLinkText
            lnkSearchInformation.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SearchInformationLinkURL)

            If Marker.ShowAllAvailableValuesButton Then
                FetchAllButton.Visible = True
            End If

            SearchResults.SelectionMode = ListSelectionMode.Multiple
            SelectedVariableValues.SelectionMode = ListSelectionMode.Multiple

            SearchResults.Attributes.Add("onchange", "SetButtonEnablePropertyToHasSelected('" + SearchResults.ClientID + "', '" + MoveToResultButton.ClientID + "'); SetButtonEnablePropertyToHasSelected('" + SearchResults.ClientID + "', '" + DeselectAllButton.ClientID + "')")
            SelectedVariableValues.Attributes.Add("onchange", "SetButtonEnablePropertyToHasDeselected('" + SelectedVariableValues.ClientID + "', '" + RemoveButton.ClientID + "'); SetButtonEnablePropertyToHasSelected('" + SelectedVariableValues.ClientID + "', '" + DoneButton.ClientID + "'); SetNumberSelected('" + SelectedVariableValues.ClientID + "', '" + NumberOfChoosenValuesLabelPart2.ClientID + "')")

        End If

        'Bug: partly disapears if not added each time, it seems:
        SearchValuesBeginningOfWordCheckBox.LabelAttributes.Add("class", "checkbox-label")

        SetBottonsEnabled()
    End Sub


    ''' <summary>
    ''' Sets up the content that does NOT depend on variable (or properties set in Marker.variable )  , i.e. stuff that only depend on language.
    ''' </summary>
    Private Sub SetLocalizedText()
        litSelectionTips.Text = "<span>" + Me.GetLocalizedString(LOC_SEARCH_SELECTION_TIPS).Replace("\n", "</span><span>") + "</span>"

        'These 3 are from the "main selection" page, so they have no LOC_ constant.
        SearchValuesTextbox.Attributes.Add("placeholder", GetLocalizedString("CtrlVariableSelectorSearchValuesTextbox"))
        SearchValuesBeginningOfWordCheckBox.Text = GetLocalizedString("CtrlVariableSelectorSearchValuesBeginningOfWordCheckBox")
        SearchValuesButton.ToolTip = GetLocalizedString("CtrlVariableSelectorSearchValuesTooltip")



        MoveToResultButton.ToolTip = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_CHOOSEN_VALUES_TOOLTIP)
        MoveToResultButton.Text = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_CHOOSEN_VALUES)

        RemoveButton.ToolTip = Me.GetLocalizedString(LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES_TOOLTIP)
        RemoveButton.Text = Me.GetLocalizedString(LOC_SEARCH_REMOVE_FROM_CHOOSEN_VALUES)

        ChoosenValuesLabel.Text = Me.GetLocalizedString(LOC_SEARCH_CHOOSEN_VALUES)

        CancelButton.Text = Me.GetLocalizedString(LOC_SEARCH_CANCEL)

        DoneButton.Text = Me.GetLocalizedString(LOC_SEARCH_ADD_TO_VARIABLESELECTOR)

        lblSearch.Text = Me.GetLocalizedString(LOC_SEARCH_LABEL)

        SelectAllButton.Text = GetLocalizedString(LOC_SEARCH_RESULTS_SELECT_ALL)

        ' --- Deselect all button
        DeselectAllButton.ToolTip = GetLocalizedString("CtrlVariableSelectorDeSelectAllTooltip")
        DeselectAllButton.Text = GetLocalizedString("CtrlVariableSelectorDeSelectAllButton")

        setWCAG()
    End Sub

    Protected Sub setWCAG()
        SearchResults.Attributes.Add("aria-label", GetLocalizedString(LOC_SEARCH_RESULTS_LISTBOX_ARIA_LABEL))
        SelectedVariableValues.Attributes.Add("aria-label", GetLocalizedString(LOC_SEARCH_CHOOSEN_VALUES_LISTBOX_ARIA_LABEL))

        SearchRegion.Attributes.Add("aria-label", GetLocalizedString("CtrlSearchValuesSearchScreenReaderRegion"))
        SelectedValuesRegion.Attributes.Add("aria-label", GetLocalizedString("CtrlSearchValuesSelectedValuesScreenReaderRegion"))
        SearchValuesTextbox.Attributes.Add("aria-label", GetLocalizedString("CtrlSearchValuesSearchFieldScreenReader"))
    End Sub



    ''' <summary>
    ''' Initiation of the search control for a given/known variable. This is called via in button click in main.  
    ''' </summary>
    ''' <remarks>Consider previous selections done for variable if saved in state-variabel</remarks>
    Public Sub InitiateSearch()
        ClearSearch()
        HideAndClearSearchError()

        'Add ("imort") previously selected values to the "Selected values" listbox
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
        For Each Item As ListItem In SelectedVariableValues.Items
            Item.Selected = True
        Next



        SearchHeader.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_HEADER), Marker.Variable.Name)
        FetchAllButton.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_SELECT_ALL_AVAILABLE_VALUES), GetNumberOfPossibleValuesText())
        SetNumberOfChoosenValuesText()

        SetBottonsEnabled()
    End Sub


    Private Sub SetNumberOfChoosenValuesText()
        'some stringformat with 2 placeholders:  "bla {1} bla {0} bla" , or "{0} bla {1} bla " or ...
        ' where {1} is the placeholder for the number of possible values and {0} is the placeholder for the number of currently selected values(which needs to be "exposed" to javascript).

        Dim glue() As String = {"{0}"}
        Dim orgFormatingString As String
        orgFormatingString = Me.GetLocalizedString(LOC_SEARCH_NUMBER_OF_CHOOSEN_VALUES)

        Dim splitText() As String
        splitText = orgFormatingString.Split(glue, StringSplitOptions.None)
        'parts(0) or parts(1) contains {1} , the other is a normal string. None of them contains {0}
        '  {1} shall be replaced by the value of GetNumberOfPossibleValuesText()

        Dim firstPart As String = String.Format(splitText(0), "Not used", GetNumberOfPossibleValuesText())
        Dim lastPart As String = String.Format(splitText(1), "Not used", GetNumberOfPossibleValuesText())


        NumberOfChoosenValuesLabelPart1.Text = firstPart

        Dim numberOfSelected As Integer
        numberOfSelected = Me.SelectedVariableValues.GetSelectedIndices.Count
        NumberOfChoosenValuesLabelPart2.Text = DataFormatter.NumericToString(numberOfSelected, 0, LocalizationManager.GetTwoLetterLanguageCode())

        NumberOfChoosenValuesLabelPart3.Text = lastPart


    End Sub

    ''' <summary>
    ''' Get the formated string with the count of values in the codelist.
    ''' So it needs Marker.Variable to be set. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNumberOfPossibleValuesText() As String
        Dim number As Integer
        If Marker.Variable Is Nothing Then
            Return ""
        End If
        If Marker.Variable.Values Is Nothing Then
            Return ""
        End If

        number = Marker.Variable.Values.Count
        Return DataFormatter.NumericToString(number, 0, LocalizationManager.GetTwoLetterLanguageCode())
    End Function


    ''' <summary>
    ''' Gets new language strings.
    ''' </summary>
    Private Sub SearchValues_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), "0")
    End Sub

    ''' <summary>
    ''' Select all items in the Search-results ListBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SelectAllButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectAllButton.Click
        For Each item As ListItem In SearchResults.Items
            item.Selected = True
        Next
        SetMoveToResultButtonEnabled()
    End Sub

    Protected Sub DeselectAllButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeselectAllButton.Click
        For Each item As ListItem In SearchResults.Items
            item.Selected = False
        Next
        SetMoveToResultButtonEnabled()
        SetDeselectAllButtonEnabled()
    End Sub

    ''' <summary>
    ''' Raises an event to signal when value selection is finished. The Done Button is clicked, returning to the main selection page/gui/controll/thing.
    ''' </summary>
    Protected Sub AddToVariableSelector_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DoneButton.Click
        Dim selection As New Selection(Marker.Variable.Code)
        For Each item_index As Integer In SelectedVariableValues.GetSelectedIndices()
            Dim item As ListItem
            item = SelectedVariableValues.Items(item_index)
            If Not String.IsNullOrEmpty(item.Value) Then
                selection.ValueCodes.Add(item.Value)
            End If
        Next
        VariableSelector.SelectedVariableValues(Marker.Variable.Code) = selection
        ClearSearch()
        Marker.OnSearchVariableValuesAdd(New EventArgs())
    End Sub

    ''' <summary>
    ''' Close the search control without adding selected values to selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        ''If the user cancels and then "back" in the browser, things get inconsistent
        ClearSearch()
        Marker.OnSearchVariableValuesAdd(New EventArgs())
    End Sub

    ''' <summary>
    ''' Adds selected values from hitlist to results
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MoveToResultButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MoveToResultButton.Click
        Dim liS As ListItem

        For Each item As ListItem In SearchResults.Items
            If item.Selected Then
                liS = New ListItem(item.Text, item.Value)
                liS.Selected = True
                If Not SelectedVariableValues.Items.Contains(liS) Then
                    SelectedVariableValues.Items.Add(liS)
                End If
            End If
        Next

        SetNumberOfChoosenValuesText()
        SetSelectedVariableValuesEnabled()
        SetDoneBottonEnabled()
    End Sub

    ''' <summary>
    ''' Remove selected values from the choosen values listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RemoveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveButton.Click
        For i As Integer = SelectedVariableValues.Items.Count - 1 To 0 Step -1
            If Not SelectedVariableValues.Items(i).Selected Then
                SelectedVariableValues.Items.Remove(SelectedVariableValues.Items(i))
            End If
        Next
        SetNumberOfChoosenValuesText()
        SetRemoveBottonEnabled()
        SetDoneBottonEnabled()
        SetSelectedVariableValuesEnabled()
    End Sub


    Private Sub SearchValuesButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchValuesButton.Click
        DoSearch()
        SetDeselectAllButtonEnabled()
        SetSelectAllButtonEnabled()
        SetMoveToResultButtonEnabled()
        SetSearchResultsEnabled()

    End Sub


    Private Sub DoSearch()
        Dim liS As ListItem
        Dim v1 As New List(Of Value)
        Dim v2 As New List(Of Value)
        Dim v3 As New List(Of Value)
        Dim v4 As New List(Of Value)
        Dim v_final As New List(Of Value)
        Dim strSearch As String

        'Check for illegal characters
        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(SearchValuesTextbox.Text) Then
            lblSearchError.Visible = True
            lblSearchError.Text = Me.GetLocalizedString(LOC_SEARCH_ILLEGAL_CHARACTERS_ERROR)
        Else
            HideAndClearSearchError()
        End If

        'Remove leading and trailing blanks
        strSearch = SearchValuesTextbox.Text.Trim()

        If (Not String.IsNullOrEmpty(strSearch)) Then


            If SearchValuesBeginningOfWordCheckBox.Checked Then
                v1.AddRange(Marker.Variable.Values.SearchInBeginningOfCode(strSearch))
                v2.AddRange(Marker.Variable.Values.SearchInBeginningOfValue(strSearch))

                v3.AddRange(Marker.Variable.Values.SearchInCode(" " + strSearch))
                v4.AddRange(Marker.Variable.Values.SearchInValue(" " + strSearch))
                v_final = v1.Union(v2).Union(v3).Union(v4).ToList()
            Else
                v1.AddRange(Marker.Variable.Values.SearchInCode(strSearch))
                v2.AddRange(Marker.Variable.Values.SearchInValue(strSearch))
                v_final = v1.Union(v2).ToList()
            End If


        End If

        SearchResults.Items.Clear()

        For Each v As Value In v_final
            If Marker.AlwaysShowCodeAndTextInSearchResult Then
                liS = New ListItem(v.CodeAndValue, v.Code)
            Else
                liS = New ListItem(v.Text, v.Code)
            End If

            liS.Selected = True
            SearchResults.Items.Add(liS)
        Next

        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), DataFormatter.NumericToString(v_final.Count, 0, LocalizationManager.GetTwoLetterLanguageCode()))

    End Sub


    Private Sub FetchAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FetchAllButton.Click
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
                liS.Selected = True
                SelectedVariableValues.Items.Add(liS)
            End If
        Next

        SetNumberOfChoosenValuesText()
        SetRemoveBottonEnabled()
        SetSelectedVariableValuesEnabled()
        SetDoneBottonEnabled()
    End Sub



    Private Sub SetBottonsEnabled()
        SetSearchResultsEnabled()
        SetMoveToResultButtonEnabled()
        SetSelectAllButtonEnabled()
        SetDeselectAllButtonEnabled()

        SetRemoveBottonEnabled()
        SetSelectedVariableValuesEnabled()

        SetDoneBottonEnabled()
    End Sub

    Private Sub SetRemoveBottonEnabled()
        RemoveButton.Enabled = SelectedVariableValues.Items.Count - SelectedVariableValues.GetSelectedIndices.Count > 0
    End Sub

    Private Sub SetDoneBottonEnabled()
        DoneButton.Enabled = SelectedVariableValues.SelectedIndex > -1
    End Sub

    Private Sub SetSelectedVariableValuesEnabled()
        SelectedVariableValues.Enabled = SelectedVariableValues.Items.Count > 0
    End Sub

    Private Sub SetMoveToResultButtonEnabled()
        MoveToResultButton.Enabled = SearchResults.SelectedIndex > -1
    End Sub
    Private Sub SetDeselectAllButtonEnabled()
        DeselectAllButton.Enabled = SearchResults.SelectedIndex > -1
    End Sub

    Private Sub SetSearchResultsEnabled()
        SearchResults.Enabled = SearchResults.Items.Count > 0
    End Sub

    Private Sub SetSelectAllButtonEnabled()
        SelectAllButton.Enabled = SearchResults.Items.Count > 0
    End Sub


    ''' <summary>
    ''' When called, removes text in textbox 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSearch()
        SearchValuesTextbox.Text = ""
        SearchResults.Items.Clear()
        SearchResultNumberOfHitsLabel.Text = String.Format(Me.GetLocalizedString(LOC_SEARCH_RESULTS_NUMBER_OF_HITS), "0")
    End Sub

    ''' <summary>
    ''' When called, set error label to visible = false and remove text from label
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
            Return Me.SearchValuesButton.ClientID
        End Get
    End Property

End Class
