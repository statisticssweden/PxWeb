Imports System.Text
Imports PCAxis.Paxiom.Localization
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Enums
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Controls
Imports PCAxis.Web.Core.Management

''' <summary>
''' Control that presents information on elimination during variable select.
''' </summary>
''' <remarks></remarks>
<ToolboxData("<{0}:VariableSelectorValueSelect runat=""server""></{0}:VariableSelectorValueSelect>")>
Public Class VariableSelectorValueSelectCodebehind
    Inherits PaxiomControlBase(Of VariableSelectorValueSelectCodebehind, VariableSelectorValueSelect)

#Region "Local enums"
    ''' <summary>
    ''' Describes the display mode of the VariableSelectorValueSelect web control
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum DisplayModeType
        ''' <summary>
        ''' Standard mode
        ''' </summary>
        ''' <remarks></remarks>
        Standard
        ''' <summary>
        ''' Large valueset - no values have been selected
        ''' </summary>
        ''' <remarks></remarks>
        ManyValuesText
        ''' <summary>
        ''' Large valueset - Values have been selected
        ''' </summary>
        ''' <remarks></remarks>
        ManyValuesListbox
        ''' <summary>
        ''' Valueset exists and no valueset (or aggregation) has been selected. 
        ''' Only the dropdownlist for valuesets and aggregations shall be displayed
        ''' </summary>
        ''' <remarks></remarks>
        SelectValueset
    End Enum
#End Region

#Region "Local variables"

    Protected _dicSortDirections As Dictionary(Of String, SortDirection) = Nothing
    Private _displayMode As DisplayModeType
#End Region

#Region "Localized strings"
    Public AriaLabelMetadata As String
#End Region

#Region "Controls"

    Protected VariableTitleSecond As Label
    Protected VariableTitle As Label
    Protected VariableTitleMetadata As Label
    Protected WithEvents GroupingDropDown As DropDownList
    Protected ValuesListBox As ListBox
    Protected MustSelect As RequiredFieldValidator
    Protected MustSelectCustom As CustomValidator


    Protected WithEvents SearchButton As LinkButton
    Protected WithEvents HierarchicalSelectButton As ImageButton
    Protected WithEvents SelectAllButton As Button
    Protected WithEvents DeselectAllButton As Button
    Protected WithEvents SelectionFromGroupButton As LinkButton

    Protected SearchValuesHeading As Label
    'Protected WithEvents SearchValuesButton As ImageButton
    Protected WithEvents SearchValuesButton As LinkButton
    Protected SearchValuesTextbox As TextBox
    Protected SearchValuesBeginningOfWordCheckBox As CheckBox

    'Protected WithEvents ShowAllValuesButton As Button
    'Protected WithEvents SearchLargeNumberOfValuesButton As Button
    'Protected WithEvents ShowHierarchyLink As LinkButton
    'Protected WithEvents SearchLargeNumberOfValuesLink As LinkButton

    Protected ValuesSelectContainerPanel As Panel
    Protected ValuesSelectPanel As Panel
    Protected EventButtons As Panel
    Protected HiddenEventButtons As Panel
    Protected SearchPanel As Panel
    Protected ManyValuesPanel As Panel
    Protected ManyValues As Literal
    Protected ContentVariable As Panel
    Protected SelectedStatistics As Panel
    Protected OptionalVariablePanel As Panel
    Protected MetadataPanel As Panel
    Protected VariableTitlePanel As Panel


    'Protected VariableTitleMaxRows As Literal
    'Protected MaxRowsWithoutSearchContainerPanel As Panel

    Protected NumberValuesTotalTitel As Literal
    Protected NumberValuesTotal As Literal
    Protected NumberValuesSelectedTitel As Literal
    Protected NumberValuesSelected As Label
    Protected MandatoryText As Label
    'Protected MandatoryStar As Label
    Protected OptionalVariableText As Label
    Protected MetadataCloseLabel As Label
    Protected SearchTip As Label
    Protected Searchguide As HtmlGenericControl

    Protected ActionButton As Button

    Protected WithEvents VariableValueLinksRepeater As Repeater
    Protected WithEvents VariableValueRepeater As Repeater
#End Region

#Region "Properties"

    ''' <summary>
    ''' Get current selection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property GetSelection() As Selection
        Get
            Dim selection As New Selection(Marker.Variable.Code)
            If PCAxis.Paxiom.Settings.Metadata.RemoveSingleContent AndAlso Marker.Variable.IsContentVariable AndAlso Marker.Variable.Values.Count = 1 Then
                selection.ValueCodes.Add(ValuesListBox.Items(0).Value)
            Else
                For Each item As ListItem In ValuesListBox.Items
                    If item.Selected Then
                        selection.ValueCodes.Add(item.Value)
                    End If
                Next
            End If

            ' Time variable shall be sorted ascending
            If Marker.Variable.IsTime Then
                ArrayList.Adapter(selection.ValueCodes).Sort()
            End If

            Return selection
        End Get
    End Property

#End Region

    Private Sub VariableSelectorValueSelect_Load() Handles Me.Load

        If Not Me.IsPostBack Then
            SearchValuesBeginningOfWordCheckBox.Checked = Marker.SearchValuesBeginningOfWordCheckBoxDefaultChecked
        End If

        If (Marker.Variable IsNot Nothing) Then
            FillControls()
            If (Marker.MetaLinkProvider IsNot Nothing) Then
                FillMetaData()
            End If
        Else
            Visible = False
        End If
        LoadScripts()

    End Sub


    Private Sub LoadScripts()
        ' Add Javascript to buttons click event
        If Me.ValuesListBox.Items.Count <= Marker.MaxRowsWithoutSearch Then 'Marker.JavascriptRowLimit is obsolete
            SelectAllButton.OnClientClick = String.Format("return VariableSelector_SelectAllAndUpdateNrSelected('{0}','{1}','{2}','{3}')", ValuesListBox.ClientID, NumberValuesSelected.ClientID, Marker.Variable.Placement.ToString(), Marker.LimitSelectionsBy)
            DeselectAllButton.OnClientClick = String.Format("return VariableSelector_DeselectAllAndUpdateNrSelected('{0}','{1}','{2}','{3}')", ValuesListBox.ClientID, NumberValuesSelected.ClientID, Marker.Variable.Placement.ToString(), Marker.LimitSelectionsBy)
            SearchValuesButton.OnClientClick = String.Format("return VariableSelector_SearchValues('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", ValuesListBox.ClientID, SearchValuesTextbox.ClientID, SearchValuesBeginningOfWordCheckBox.ClientID, NumberValuesSelected.ClientID, Marker.Variable.Placement.ToString(), Marker.LimitSelectionsBy, GetLocalizedSearchResultTextForScreenReader())
        End If

        'Used to hide the action button if javascripts is enabled
        'Page.ClientScript.RegisterStartupScript(Me.GetType, "ValueSelectorValueSelect_HideActionButton", "PCAxis_HideElement("".variableselector_valuesselect_action"");", True)
    End Sub


    Private Sub FillControls()
        Dim li As ListItem

        ' If querystring action is set to slectall or selectdefault automatic slection and view of table is triggered, render all values for autmatic value selection
        If (QuerystringManager.GetQuerystringParameter("action") = "selectdefault") Then
            Marker.ShowAllValues = True
        ElseIf (QuerystringManager.GetQuerystringParameter("action") = "selectall") Then
            Marker.ShowAllValues = True
        End If

        ' --- Handle localized language
        SetLocalizedText()

        'Set ImageUrl for buttons from embedded resource
        Dim imgurl As String = Page.ClientScript.GetWebResourceUrl(GetType(VariableSelectorValueSelectCodebehind), "PCAxis.Web.Controls.spacer.gif")
        HierarchicalSelectButton.ImageUrl = imgurl
        'Add CSSClass for checkbox label
        SearchValuesBeginningOfWordCheckBox.LabelAttributes.Add("class", "checkbox-label")
        SearchValuesBeginningOfWordCheckBox.InputAttributes.Add("aria-label", GetLocalizedString("CtrlVariableSelectorSearchguide"))

        ' --- Visibility for select hierarcical button
        If (Not Marker.Variable.Hierarchy.IsHierarchy) Or (Not Marker.ShowHierarchies) Then
            HierarchicalSelectButton.Visible = False
        Else
            HierarchicalSelectButton.Visible = True
        End If

        UpdateDisplayModeUI()

        ' --- Statistics on value selection
        If Not (Marker.HideTitlesForValueSelectionStatistics) Then
            NumberValuesTotalTitel.Text = GetLocalizedString("CtrlVariableSelectorSummaryTotalLabel")
            NumberValuesSelectedTitel.Text = GetLocalizedString("CtrlVariableSelectorSummarySelectedLabel")
        End If

        Select Case _displayMode
            Case DisplayModeType.Standard
                NumberValuesTotal.Text = DataFormatter.NumericToString(ValuesListBox.Items.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
            Case Else
                NumberValuesTotal.Text = DataFormatter.NumericToString(Marker.Variable.Values.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
        End Select

        If (Marker.PreSelectFirstContentAndTime) Then
            SelectFirstContentAndTimeValue()
        End If

        'If Me.ValuesListBox.Items.Count < Marker.JavascriptRowLimit Then
        ValuesListBox.Attributes.Add("onchange", "UpdateNumberSelected('" + ValuesListBox.ClientID + "', '" + NumberValuesSelected.ClientID + "', '" + Marker.Variable.Placement.ToString() + "','" + Marker.LimitSelectionsBy + "')")
        'End If
        UpdateSelectedStats()

        ' MustSelect.Enabled = False
        MustSelectCustom.Enabled = False
        SetElimination()

        If Marker.AllowAggreg Then
            '---- Handle variables with grouping of valuesets
            If Marker.Variable.HasGroupings() Or Marker.Variable.HasValuesets() Then
                'If Marker.Variable.CurrentGrouping Is Nothing And Marker.Variable.CurrentValueSet Is Nothing Then
                'If RedrawGroupingValues() Then

                li = New ListItem(Me.GetLocalizedString("CtrlVariableSelectorSelectValues"), "")
                If Not GroupingDropDown.Items.Contains(li) Then
                    GroupingDropDown.Items.Add(li)
                End If

                'Add valuesets
                If Marker.Variable.HasValuesets() Then
                    For Each vsInfo As PCAxis.Paxiom.ValueSetInfo In Marker.Variable.ValueSets
                        li = New ListItem(vsInfo.Name.Trim(), "vs__" & vsInfo.ID)
                        If Not GroupingDropDown.Items.Contains(li) Then
                            'The _ALL_ valueset are always delivered from the PXSQLBuilder. It contains all values. 
                            'This _ALL_ valueset will NOT be added if the ValuesetMustBeSelectedFirst setting is set to True
                            If Not (Marker.ValuesetMustBeSelectedFirst And vsInfo.ID.Equals("_ALL_")) Then
                                GroupingDropDown.Items.Add(li)
                            End If
                        End If
                    Next
                End If

                'Add groupings
                If Marker.Variable.HasGroupings() Then
                    For Each grpInfo As PCAxis.Paxiom.GroupingInfo In Marker.Variable.Groupings
                        li = New ListItem(grpInfo.Name.Trim(), "gr__" & grpInfo.ID)
                        If Not GroupingDropDown.Items.Contains(li) Then
                            GroupingDropDown.Items.Add(li)
                        End If
                    Next
                End If

                'Change layout
                If Marker.ShowGroupingDropDown Then
                    GroupingDropDown.Visible = True
                    ActionButton.Visible = True
                End If

                'End If

                'Sometimes we need to "catch up" in the graphical user interface with the Paxiom model
                'Catch up selected grouping/valueset from the model if we have somehow lost it...
                If String.IsNullOrEmpty(Marker.SelectedGrouping) Then
                    If Not Marker.Variable.CurrentGrouping Is Nothing Then
                        Marker.SelectedGrouping = "gr__" & Marker.Variable.CurrentGrouping.Name
                    ElseIf Not Marker.Variable.CurrentValueSet Is Nothing Then
                        Marker.SelectedGrouping = "vs__" & Marker.Variable.CurrentValueSet.ID
                    End If
                End If

                'Update values to be sure they match the selected grouping(fix for use of back-button)
                If (Not Marker.Variable.CurrentGrouping Is Nothing) AndAlso (Marker.SelectedGrouping.Length > 4) Then
                    'Fix for many-values problem after use of back-button from view-page
                    'If VariableSelector.SelectedVariableValues.ContainsKey(Marker.Variable.Code) Then
                    '    VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Clear()
                    'End If
                    'End Fix for many-values
                    If ApplyGrouping(Marker.SelectedGrouping, False, Marker.SelectedGroupingPresentation) Then
                        UpdateDisplayModeUI()
                    End If
                End If

                'Refresh Marker.SelectedGrouping to value from interface (fix for use of back-button)
                'Add html5-valid custom attribute to keep a selected value not changed in dom via user-actions and therefore persistent when back-button is used.
                If Not String.IsNullOrEmpty(Marker.SelectedGrouping) Then
                    Me.GroupingDropDown.SelectedValue = Marker.SelectedGrouping
                    GroupingDropDown.Attributes.Add("data-value", GroupingDropDown.SelectedValue.ToString())

                    If Not Page.IsPostBack AndAlso (Marker.Variable.CurrentValueSet IsNot Nothing OrElse Marker.Variable.CurrentGrouping IsNot Nothing) Then
                        ApplyCurrentValuesetOrGrouping()
                    End If
                End If
            End If
        End If

        If Marker.Variable.IsContentVariable AndAlso (Marker.ButtonsForContentVariable = False) Then
            EventButtons.Visible = False
            SearchPanel.Visible = False
            HiddenEventButtons.Visible = True
        End If
    End Sub


    Protected Sub ValidateListBox_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim is_valid As Boolean = ValuesListBox.SelectedIndex > -1
        If TypeOf (source) Is CustomValidator Then
            Dim myCustom As CustomValidator = CType(source, CustomValidator)
            myCustom.ValidationGroup = ""
        End If
        args.IsValid = is_valid
        If (Not is_valid) Then
            If Not Page.ClientScript.IsClientScriptBlockRegistered(Me.GetType(), "CreateResetScrollPosition") Then
                'Create the ResetScrollPosition() function
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CreateResetScrollPosition",
                                 "function ResetScrollPosition() {" & vbCrLf &
                                 " var scrollX = document.getElementById('__SCROLLPOSITIONX');" & vbCrLf &
                                 " var scrollY = document.getElementById('__SCROLLPOSITIONY');" & vbCrLf &
                                 " if (scrollX && scrollY) {" & vbCrLf &
                                 "    scrollX.value = 0;" & vbCrLf &
                                 "    scrollY.value = 0;" & vbCrLf &
                                 " }" & vbCrLf &
                                 "}", True)

                'Add the call to the ResetScrollPosition() function
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "CallResetScrollPosition", "ResetScrollPosition();", True)
            End If
        End If
    End Sub


    ''' <summary>
    ''' Mark images for which at least one value must be selected
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetElimination()
        If Marker.ShowElimMark And Not Marker.Variable.Elimination Then
            MandatoryText.Visible = True
            'MandatoryStar.Visible = True
            Me.MustSelectCustom.ErrorMessage = Marker.Variable.Name
            Me.MustSelectCustom.Enabled = True
            If (Marker.ClientSideValidation) Then
                Me.MustSelectCustom.EnableClientScript = True 'set to true for clientsidevalidation
            Else
                Me.MustSelectCustom.EnableClientScript = False 'set to false for serversidevalidation
            End If
            Me.MustSelectCustom.ValidationGroup = ValuesListBox.ClientID
            'Me.MustSelectCustom.EnableClientScript = True ' If we want to validate on select/deselect in listbox. Should maybe be possible to configurate.
            OptionalVariablePanel.Visible = False
        Else
            MandatoryText.Visible = False
            'MandatoryStar.Visible = False
            Me.MustSelectCustom.Enabled = False
            OptionalVariablePanel.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Updates the User Interface to reflect changes in the display mode
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateDisplayModeUI()
        GetDisplayMode()
        SetDisplayModeUI()
    End Sub

    'Private Function ShowValues() As Boolean
    '    If Marker.Variable.HasValuesets AndAlso Marker.Variable.CurrentValueSet Is Nothing Then
    '        If Marker.Variable.HasGroupings AndAlso Marker.Variable.CurrentGrouping IsNot Nothing Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End If

    '    Return True
    'End Function

    ''' <summary>
    ''' Render ListBox with variable values for selection.
    ''' </summary>
    Private Sub RenderValuesListbox()
        'If ShowValues() Then

        'Match grouping selected in the interface. Use of backbutton may have caused missmatches
        'between model on server and sclections sent by the interface.
        If GroupingDropDown.SelectedValue.Length > 0 Then
            Marker.SelectedGrouping = GroupingDropDown.SelectedValue
        End If
        'If (Not Marker.Variable.CurrentGrouping Is Nothing) AndAlso (Marker.SelectedGrouping.Length > 4) Then
        '    If Not (Marker.Variable.CurrentGrouping.Name).Equals(Marker.SelectedGrouping.Substring(4)) Then
        '        If ApplyGrouping(Marker.SelectedGrouping) Then
        '            UpdateDisplayModeUI()
        '        End If
        '    End If
        'End If




        ValuesListBox.SelectionMode = ListSelectionMode.Multiple
        ValuesListBox.Rows = Marker.ListSize


        Dim valuesToShow As Values = Nothing

        Select Case _displayMode
            Case DisplayModeType.ManyValuesText
                Exit Sub
            Case DisplayModeType.Standard
                valuesToShow = New Values(Marker.Variable)
                For Each value As Value In Marker.Variable.Values
                    valuesToShow.Add(value)
                Next
            Case DisplayModeType.ManyValuesListbox
                Dim valueByCode As New Dictionary(Of String, Value)

                For Each valueItem As Value In Marker.Variable.Values
                    valueByCode(valueItem.Code) = valueItem
                Next

                valuesToShow = New Values(Marker.Variable)
                For Each val As String In VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes
                    If valueByCode.ContainsKey(val) Then
                        valuesToShow.Add(valueByCode(val))
                    End If
                Next
        End Select
        If (Marker.IsSortDirectionSet) Then
            If IsNumericalTimeVariable(valuesToShow) Then
                'Time variable
                If Marker.ValuesSortDirection = SortDirection.Ascending Then
                    valuesToShow.Sort(Function(emp1, emp2) Integer.Parse(emp1.Code).CompareTo(Integer.Parse(emp2.Code)))
                Else
                    valuesToShow.Sort(Function(emp1, emp2) Integer.Parse(emp2.Code).CompareTo(Integer.Parse(emp1.Code)))
                End If
            Else
                'All other variables
                If Marker.ValuesSortDirection = SortDirection.Ascending Then
                    'valuesToShow.Sort(Function(emp1, emp2) emp1.Code.CompareTo(emp2.Code))
                    valuesToShow.Sort(Function(emp1, emp2) emp1.Text.CompareTo(emp2.Text))
                Else
                    'valuesToShow.Sort(Function(emp1, emp2) emp2.Code.CompareTo(emp1.Code))
                    valuesToShow.Sort(Function(emp1, emp2) emp2.Text.CompareTo(emp1.Text))
                End If
            End If
        Else
            If Marker.Variable.IsTime Then
                'Time variable is ordered descending by default
                If IsNumericalTimeVariable(valuesToShow) Then
                    valuesToShow.Sort(Function(emp1, emp2) Integer.Parse(emp2.Code).CompareTo(Integer.Parse(emp1.Code)))
                Else
                    valuesToShow.Sort(Function(emp1, emp2) emp2.Code.CompareTo(emp1.Code))
                End If
            End If
        End If

        ValuesListBox.DataTextField = "Text"
        ValuesListBox.DataValueField = "Code"

        ValuesListBox.DataSource = valuesToShow
        ValuesListBox.DataBind()
        If ValuesListBox.Items.Count >= ValuesListBox.Rows Then
            ValuesListBox.Style.Add("overflow-y", "scroll")
        End If
        RenderSelection()
        'End If

    End Sub


    ''' <summary>
    ''' Check if it is a legal time variable
    ''' </summary>
    ''' <param name="vals"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsNumericalTimeVariable(ByVal vals As Values) As Boolean
        Dim res As Integer

        If Marker.Variable.IsTime Then
            For i As Integer = 0 To vals.Count - 1
                If Not Integer.TryParse(vals.Item(i).Code, res) Then
                    Return False
                End If
            Next
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Updates language strings.
    ''' </summary>
    Private Sub VariableSelectorValueSelect_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        SetLocalizedText()
    End Sub

    ''' <summary>
    ''' Render language strings.
    ''' </summary>
    Private Sub SetLocalizedText()
        AriaLabelMetadata = String.Format(GetLocalizedString("CtrlVariableSelectorMetadataAriaLabel"), Marker.Variable.Name)

        ' --- Search values link button
        SearchButton.Text = String.Format("<span class='link-text'>{0}</span>", GetLocalizedString("CtrlVariableSelectorSearchLabel"))

        ' --- Hierarchical selection button
        HierarchicalSelectButton.ToolTip = GetLocalizedString("CtrlVariableSelectorHierarchicalTooltip")
        HierarchicalSelectButton.AlternateText = GetLocalizedString("CtrlVariableSelectorHierarchicalTooltip")

        ' --- Select all button
        SelectAllButton.ToolTip = GetLocalizedString("CtrlVariableSelectorSelectAllTooltip")
        SelectAllButton.Text = GetLocalizedString("CtrlVariableSelectorSelectAllButton")
        SelectAllButton.Attributes.Add("aria-label", SelectAllButton.ToolTip)
        SelectAllButton.Attributes.Add("aria-description", "")

        ' --- Deselect all button
        DeselectAllButton.ToolTip = GetLocalizedString("CtrlVariableSelectorDeSelectAllTooltip")
        DeselectAllButton.Text = GetLocalizedString("CtrlVariableSelectorDeSelectAllButton")
        DeselectAllButton.Attributes.Add("aria-label", DeselectAllButton.ToolTip)
        DeselectAllButton.Attributes.Add("aria-description", "")


        ' --- Select from groups link button
        SelectionFromGroupButton.Text = String.Format("<span class='link-text'>{0}</span>", GetLocalizedString("CtrlVariableSelectorSelectFromGroupTooltip"))

        ' --- Search panel
        'SearchValuesHeading.Text = GetLocalizedString("CtrlVariableSelectorSearchValuesHeading")
        SearchValuesBeginningOfWordCheckBox.Text = GetLocalizedString("CtrlVariableSelectorSearchValuesBeginningOfWordCheckBox")

        'wcag says: label element or title attribute  
        'SearchValuesTextbox.Attributes.Add("aria-label", GetLocalizedString("CtrlVariableSelectorSearchValuesTextboxScreenReader"))
        SearchValuesTextbox.Attributes.Add("aria-labelledby", SearchTip.ClientID)


        ValuesListBox.Attributes.Add("aria-label", GetLocalizedString("CtrlVariableSelectorSelectValuesListboxScreenReader"))

        SearchValuesTextbox.Attributes.Add("placeholder", GetLocalizedString("CtrlVariableSelectorSearchValuesTextbox"))

        ' --- Search values button
        SearchValuesButton.ToolTip = GetLocalizedString("CtrlVariableSelectorSearchValuesTooltip")
        'SearchValuesButton.AlternateText = GetLocalizedString("CtrlVariableSelectorSearchValuesTooltip")

        '--- Dropdownlist for grouping
        GroupingDropDown.Attributes.Add("aria-label", GetLocalizedString("CtrlVariableSelectorSelectDropdownGroupingLabel"))


        ' --- Many values text
        ManyValues.Text = String.Format(GetLocalizedString("CtrlVariableSelectorManyValues"), Marker.MaxRowsWithoutSearch)

        ' --- Action button shown if script is disabled
        ActionButton.Text = String.Format(GetLocalizedString("CtrlVariableSelectorActionButton"), Marker.MaxRowsWithoutSearch)

        ' --- Validator message
        MustSelectCustom.Text = GetLocalizedString("CtrlVariableSelectorValidationMessage")

        ' --- Statistics on value selection
        If Not (Marker.HideTitlesForValueSelectionStatistics) Then
            NumberValuesTotalTitel.Text = GetLocalizedString("CtrlVariableSelectorSummaryTotalLabel")
            NumberValuesSelectedTitel.Text = GetLocalizedString("CtrlVariableSelectorSummarySelectedLabel")
        End If

        Select Case _displayMode
            Case DisplayModeType.Standard
                NumberValuesTotal.Text = DataFormatter.NumericToString(ValuesListBox.Items.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
            Case Else
                NumberValuesTotal.Text = DataFormatter.NumericToString(Marker.Variable.Values.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
        End Select

        ' --- MandatoryText
        MandatoryText.Text = GetLocalizedString("CtrlVariableSelectorMandatoryText")

        OptionalVariableText.Text = GetLocalizedString("CtrlVariableSelectorOptionalVariableText")

        MetadataCloseLabel.Text = GetLocalizedString("CtrlVariableSelectorMetadataCloseButton")

        ValuesSelectContainerPanel.Attributes.Add("aria-label", GetLocalizedString("CtrlVariableSelectorSelectScreenReaderRegion") + Marker.Variable.Name)
    End Sub

    ''' <summary>
    ''' Set current selection of values for variable.
    ''' </summary>
    ''' <remarks>If querysting action="selectdefault" selection is set to default values for variable (number of values in defaultview is set by property NumberOfValuesInDefaultView in VariableSelector control.
    ''' If querysting action="selectall" slection is set to all values for variable</remarks>
    Friend Sub RenderSelection()
        If (QuerystringManager.GetQuerystringParameter("action") = "selectdefault") Then
            SelectDefaultValues()
        ElseIf (QuerystringManager.GetQuerystringParameter("action") = "selectall") Then
            SelectAllValues()
        Else
            If VariableSelector.SelectedVariableValues.ContainsKey(Marker.Variable.Code) Then
                Dim valueCodesForVariable As New HashSet(Of String)

                For Each valueCode In VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes
                    valueCodesForVariable.Add(valueCode) 'We assume valueCodes are unique for a variable
                Next

                For Each item As ListItem In ValuesListBox.Items
                    item.Selected = valueCodesForVariable.Contains(item.Value)
                Next
            End If
            UpdateSelectedStats()
        End If
    End Sub

    ''' <summary>
    ''' Do the user have to select valueset before the listbox with values are displayed?
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValuesetMustBeSelected() As Boolean
        If Marker.ValuesetMustBeSelectedFirst Then
            If Marker.Variable.HasValuesets AndAlso Marker.Variable.CurrentValueSet Is Nothing Then
                If Marker.Variable.HasGroupings AndAlso Marker.Variable.CurrentGrouping IsNot Nothing Then
                    Return False
                Else
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    ''' <summary>
    ''' Determines which display mode to use
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDisplayMode()
        Dim alwaysShowTimeVariableWithoutSearch As Boolean = Marker.AlwaysShowTimeVariableWithoutSearch AndAlso Marker.Variable.IsTime
        If ValuesetMustBeSelected() Then
            _displayMode = DisplayModeType.SelectValueset
        ElseIf Marker.HasManyValues() And Not Marker.ShowAllValues And Not alwaysShowTimeVariableWithoutSearch Then 'Marker.HasManyValues() And Not Marker.ShowAllValues And Not Marker.Variable.IsTime Then
            If VariableSelector.SelectedVariableValues.ContainsKey(Marker.Variable.Code) AndAlso
               VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Count > 0 Then
                _displayMode = DisplayModeType.ManyValuesListbox
            Else
                _displayMode = DisplayModeType.ManyValuesText
            End If
        Else
            _displayMode = DisplayModeType.Standard
        End If
    End Sub

    ''' <summary>
    ''' Update user interface elements to match the given display mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDisplayModeUI()
        Select Case _displayMode
            Case DisplayModeType.Standard
                ShowListBox()
                'ValuesSelectPanel.Visible = True
                'ValuesSelectPanel.Enabled = True
                'ValuesListBox.Attributes.CssStyle.Add("visibility", "visible")
                'ValuesListBox.Attributes.CssStyle.Clear()
                SetSearchPanelVisibility()
                ManyValuesPanel.Visible = False

                SearchButton.Visible = False
                SelectAllButton.Visible = True
                DeselectAllButton.Visible = True
                SetSelectionFromGroupVisibility()
                SelectedStatistics.Visible = True
                VariableTitle.Text = Marker.Variable.Name
                VariableTitleMetadata.Text = Marker.Variable.Name
            Case DisplayModeType.ManyValuesListbox
                ShowListBox()
                ' ValuesSelectPanel.Visible = True
                'ValuesSelectPanel.Enabled = True
                'ValuesListBox.Attributes.CssStyle.Add("visibility", "visible")
                'ValuesListBox.Attributes.CssStyle.Clear()
                SetSearchPanelVisibility()
                ManyValuesPanel.Visible = False
                SearchButton.Visible = True
                SelectAllButton.Visible = True
                DeselectAllButton.Visible = True
                SetSelectionFromGroupVisibility()
                SelectedStatistics.Visible = True
                VariableTitle.Text = Marker.Variable.Name & " " & Me.GetLocalizedString("CtrlVariableSelectorLargeValueset")
                VariableTitleMetadata.Text = Marker.Variable.Name & " " & Me.GetLocalizedString("CtrlVariableSelectorLargeValueset")
            Case DisplayModeType.ManyValuesText
                HideListBox()
                SearchPanel.Visible = False
                ManyValuesPanel.Visible = True
                SearchButton.Visible = True
                SelectAllButton.Visible = False
                DeselectAllButton.Visible = False
                SetSelectionFromGroupVisibility()
                SelectedStatistics.Visible = True
                VariableTitle.Text = Marker.Variable.Name & " " & Me.GetLocalizedString("CtrlVariableSelectorLargeValueset")
                VariableTitleMetadata.Text = Marker.Variable.Name & " " & Me.GetLocalizedString("CtrlVariableSelectorLargeValueset")
            Case DisplayModeType.SelectValueset
                HideListBox()
                SearchPanel.Visible = False
                ManyValuesPanel.Visible = False
                SearchButton.Visible = False
                SelectAllButton.Visible = False
                DeselectAllButton.Visible = False
                SelectionFromGroupButton.Visible = False
                SelectedStatistics.Visible = False
                VariableTitle.Text = Marker.Variable.Name
                VariableTitleMetadata.Text = Marker.Variable.Name
        End Select

        SetMetaDataInformationVisibility()

        If Marker.SearchButtonMode = VariableSelectorSearchButtonViewMode.Always Then
            SearchButton.Visible = True
        End If

        '' --- Variable name
        'If Marker.HasManyValues Then
        '    VariableTitle.Text = Marker.Variable.Name & " " & Me.GetLocalizedString("CtrlVariableSelectorLargeValueset")
        'Else
        '    VariableTitle.Text = Marker.Variable.Name
        'End If

        VariableTitleSecond.Text = Marker.VariableTitleSecond

        If _displayMode <> DisplayModeType.ManyValuesText Then
            RenderValuesListbox()
        End If

        Select Case _displayMode
            Case DisplayModeType.Standard
                'NumberValuesTotal.Text = ValuesListBox.Items.Count.ToString
                NumberValuesTotal.Text = DataFormatter.NumericToString(ValuesListBox.Items.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
            Case Else
                'NumberValuesTotal.Text = Marker.Variable.Values.Count.ToString()
                NumberValuesTotal.Text = DataFormatter.NumericToString(Marker.Variable.Values.Count, 0, LocalizationManager.GetTwoLetterLanguageCode())
        End Select

    End Sub

    ''' <summary>
    ''' Set the visibility of the "Selection from group" button
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSelectionFromGroupVisibility()
        If Not Marker.Variable.Groupings Is Nothing AndAlso Marker.SelectionFromGroupButtonMode AndAlso Marker.Variable.Groupings.Count > 0 Then
            SelectionFromGroupButton.Visible = True
        Else
            SelectionFromGroupButton.Visible = False
        End If
    End Sub


    ''' <summary>
    ''' Set the visibility of the "Information Meta data" button
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetMetaDataInformationVisibility()
        Dim metadata As Boolean = False

        If Marker.MetadataInformationButtonMode Then
            If Not Marker.Variable.MetaId Is Nothing Or Marker.Variable.HasValueMetaId Then
                metadata = True
            End If
        End If

        MetadataPanel.Visible = metadata
        VariableTitlePanel.Visible = Not metadata
    End Sub

    ''' <summary>
    ''' Select all values for variable.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub SelectAllValues()
        For Each item As ListItem In ValuesListBox.Items
            item.Selected = True
        Next
    End Sub

    ''' <summary>
    ''' Select the values specified by the values parameter
    ''' </summary>
    ''' <param name="values">Values collection</param>
    ''' <remarks></remarks>
    Friend Sub SelectValues(ByVal values As PCAxis.Paxiom.Values)
        For Each item As ListItem In ValuesListBox.Items
            If Not values.GetByCode(item.Value) Is Nothing Then
                item.Selected = True
            End If
        Next
    End Sub

    ''' <summary>
    ''' Deselect all values for variable.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub DeselectAllValues()
        For Each item As ListItem In ValuesListBox.Items
            item.Selected = False
        Next
        NumberValuesSelected.Text = "0"
    End Sub

    ''' <summary>
    ''' Select default values for variable.
    ''' </summary>
    ''' <remarks>Number of values in defaultselection is set by property NumberOfValuesInDefaultView i VariableSelector</remarks>
    Friend Sub SelectDefaultValues()
        Dim i = 1
        For Each item As ListItem In ValuesListBox.Items
            item.Selected = True
            i = i + 1
            If (i > Marker.NumberOfValuesInDefaultView) Then
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Update information on number of selected values for variable.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub UpdateSelectedStats()
        Dim selected As Integer = 0
        For Each item As ListItem In ValuesListBox.Items
            If item.Selected Then
                selected = selected + 1
            End If
        Next

        NumberValuesSelected.Text = selected.ToString

    End Sub

    ''' <summary>
    ''' Select all items in a ListBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SelectAllButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectAllButton.Click
        SelectAllValues()
        NumberValuesSelected.Text = ValuesListBox.Items.Count.ToString()
    End Sub

    ''' <summary>
    ''' Unselects all items in a ListBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DeselectAllButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeselectAllButton.Click
        'For Each item As ListItem In ValuesListBox.Items
        '    item.Selected = False
        'Next
        'NumberValuesSelected.Text = "0"
        DeselectAllValues()
    End Sub


    ''' <summary>
    ''' Present dialog for selecting values from group
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub hierarchicalSelectImageButton_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles HierarchicalSelectButton.Click

        Marker.OnSelectHierarchicalButtonClicked(sender, e, Marker.Variable)

    End Sub

    ''' <summary>
    ''' Present dialog for selecting values from group
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub selectionFromGroupButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectionFromGroupButton.Click

        Marker.OnSelectFromGroupButtonClicked(sender, e, Marker.Variable)

    End Sub

    ''' <summary>
    ''' Search the list of values for matching substring.
    ''' searchTextStartCheckBox.Checked determins if the search only should target the start of the strings in the list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SearchValuesButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchValuesButton.Click

        'Check for illegal characters
        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(SearchValuesTextbox.Text) Then
            Return
        End If

        'Remove leading and trailing blanks
        SearchValuesTextbox.Text = SearchValuesTextbox.Text.Trim()

        If SearchValuesTextbox.Text.Length > 0 Then
            For Each li As ListItem In ValuesListBox.Items
                If SearchValuesBeginningOfWordCheckBox.Checked Then
                    'Search start each word
                    If li.Text.ToLower().StartsWith(SearchValuesTextbox.Text, StringComparison.InvariantCultureIgnoreCase) OrElse li.Text.IndexOf(" " + SearchValuesTextbox.Text, StringComparison.InvariantCultureIgnoreCase) > -1 Then
                        li.Selected = True
                    End If
                Else
                    'Search in any position
                    If li.Text.IndexOf(SearchValuesTextbox.Text, StringComparison.InvariantCultureIgnoreCase) > -1 Then
                        li.Selected = True
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub GroupingDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GroupingDropDown.SelectedIndexChanged
        'Dim ok As Boolean = False
        If Not GroupingDropDown.SelectedValue.Equals(Marker.SelectedGrouping) Then

            'Bug 273
            ' Lagra vilken gruppering/valueset som gjorts i en property...

            'If GroupingDropDown.SelectedItem.Attributes("opinfo").Equals("grouping") Then
            'If GroupingDropDown.SelectedValue.StartsWith("gr__") Then
            '    'Apply grouping
            '    Dim grpInfo As PCAxis.Paxiom.GroupingInfo
            '    grpInfo = Marker.Variable.GetGroupingInfoById(GroupingDropDown.SelectedValue.Replace("gr__", "")) 'Remove grouping prefix

            '    If Not grpInfo Is Nothing Then
            '        Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyGrouping(Marker.Variable.Code, grpInfo, GroupingIncludesType.AggregatedValues)
            '        ok = True
            '    End If
            '    'ElseIf GroupingDropDown.SelectedItem.Attributes("opinfo").Equals("valueset") Then
            'ElseIf GroupingDropDown.SelectedValue.StartsWith("vs__") Then
            '    'Apply valueset
            '    Dim vsInfo As PCAxis.Paxiom.ValueSetInfo
            '    vsInfo = Marker.Variable.GetValuesetById(GroupingDropDown.SelectedValue.Replace("vs__", "")) 'Remove valuset prefix

            '    If Not vsInfo Is Nothing Then
            '        Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyValueSet(Marker.Variable.Code, vsInfo)
            '        ok = True
            '    End If
            'End If



            'Refresh Marker.SelectedGrouping to value from interface (fix for use of back-button)
            'Add html5-valid custom attribute to keep a selected value not changed in dom via user-actions and therefore persistent when back-button is used.
            If GroupingDropDown.SelectedValue.Length > 0 Then
                Marker.SelectedGrouping = GroupingDropDown.SelectedValue
                'Else
                '    Marker.SelectedGrouping = ""
            End If
            GroupingDropDown.Attributes.Add("data-value", GroupingDropDown.SelectedValue.ToString())

            'Show new values
            If ApplyGrouping(GroupingDropDown.SelectedValue) Then
                'RenderValuesListbox()
                UpdateDisplayModeUI()

                ' --- Visibility for select hierarcical button
                If Not (Marker.Variable.Hierarchy.IsHierarchy) Then
                    HierarchicalSelectButton.Visible = False
                End If

                'Marker.VariableTitleSecond = GroupingDropDown.Items(GroupingDropDown.SelectedIndex).Text
                'VariableTitleSecond.Text = Marker.VariableTitleSecond
                'VariableTitleSecond.Visible = True
                VariableTitle.Visible = Not MetadataPanel.Visible
                'GroupingDropDown.Visible = False
                ActionButton.Visible = False
            End If

            'Retain focus on dropdownlist after selected index changed
            GroupingDropDown.Focus()

        End If
    End Sub

    Private Sub ApplyCurrentValuesetOrGrouping()
        'Show new values
        If ApplyGrouping(GroupingDropDown.SelectedValue) Then
            UpdateDisplayModeUI()

            ' --- Visibility for select hierarcical button
            If Not (Marker.Variable.Hierarchy.IsHierarchy) Then
                HierarchicalSelectButton.Visible = False
            End If

            VariableTitle.Visible = Not MetadataPanel.Visible
            ActionButton.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Change selected grouping programmatically
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub ChangeSelectedGrouping(ByVal grouping As String, ByVal include As GroupingIncludesType)
        grouping = "gr__" & grouping

        For i As Integer = 0 To GroupingDropDown.Items.Count - 1
            If GroupingDropDown.Items(i).Value.Equals(grouping) Then
                GroupingDropDown.SelectedIndex = i
                Exit For
            End If
        Next
        'Refresh Marker.SelectedGrouping to value from interface (fix for use of back-button)
        'Add html5-valid custom attribute to keep a selected value not changed in dom via user-actions and therefore persistent when back-button is used.
        If GroupingDropDown.SelectedValue.Length > 0 Then
            Marker.SelectedGrouping = GroupingDropDown.SelectedValue
        End If
        GroupingDropDown.Attributes.Add("data-value", GroupingDropDown.SelectedValue.ToString())

        'Show new values
        If ApplyGrouping(GroupingDropDown.SelectedValue, False, include) Then
            UpdateDisplayModeUI()

            ' --- Visibility for select hierarcical button
            If Not (Marker.Variable.Hierarchy.IsHierarchy) Then
                HierarchicalSelectButton.Visible = False
            End If

            VariableTitle.Visible = Not MetadataPanel.Visible
            ActionButton.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Apply the grouping/valueset with the given code on the variable
    ''' </summary>
    ''' <param name="code">Code of grouping/valueset</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ApplyGrouping(ByVal code As String, Optional ByVal clearSelection As Boolean = True, Optional ByVal include As Nullable(Of GroupingIncludesType) = Nothing) As Boolean
        Dim ok As Boolean = False

        If (code.Equals("") And Not (Marker.ValuesetMustBeSelectedFirst)) Then
            Dim vsInfo As New PCAxis.Paxiom.ValueSetInfo
            vsInfo.ID = "_ALL_"
            Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyValueSet(Marker.Variable.Code, vsInfo)
            Marker.SelectedGroupingPresentation = GroupingIncludesType.SingleValues
            ok = True
        ElseIf code.StartsWith("gr__") Then
            'Apply grouping
            Dim grpInfo As PCAxis.Paxiom.GroupingInfo
            grpInfo = Marker.Variable.GetGroupingInfoById(code.Replace("gr__", "")) 'Remove grouping prefix

            Dim grpType As GroupingIncludesType
            If include Is Nothing Then
                grpType = grpInfo.GroupPres
            Else
                grpType = CType(include, GroupingIncludesType)
            End If

            If Not grpInfo Is Nothing Then
                Marker.SelectedGroupingPresentation = grpType
                Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyGrouping(Marker.Variable.Code, grpInfo, grpType)

                ok = True
            End If
        ElseIf code.StartsWith("vs__") Then
            'Apply valueset
            Dim vsInfo As PCAxis.Paxiom.ValueSetInfo
            vsInfo = Marker.Variable.GetValuesetById(code.Replace("vs__", "")) 'Remove valuset prefix

            If Not vsInfo Is Nothing Then
                Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyValueSet(Marker.Variable.Code, vsInfo)
                Marker.SelectedGroupingPresentation = GroupingIncludesType.SingleValues
                ok = True
            End If
        ElseIf String.IsNullOrEmpty(code) Then
            Marker.Variable.CurrentGrouping = Nothing
            Marker.Variable.CurrentValueSet = Nothing
            clearSelection = True
        End If

        'If grouping or valueset has been applied it might not longer be necessary to select at least one value for the variable, so elimination must be updated
        SetElimination()

        If clearSelection Then
            'Remove any previously selected values for this variable
            If VariableSelector.SelectedVariableValues.ContainsKey(Marker.Variable.Code) Then
                VariableSelector.SelectedVariableValues(Marker.Variable.Code).ValueCodes.Clear()
            End If

            ValuesListBox.Items.Clear()
        End If

        UpdateSelectedStats()

        Return ok
    End Function

    ''' <summary>
    ''' Open dialog for searching values 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SearchButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchButton.Click
        'Marker.ShowAllValues = True
        Marker.OnSearchLargeNumberOfValuesButtonClicked(sender, e, Marker.Variable)
    End Sub

    ''' <summary>
    ''' Handles event PreRender
    ''' </summary> 
    Protected Sub VariableSelectorValueSelectC_PreRender() Handles Me.PreRender
        If Marker.ValuesetMustBeSelectedFirst And Marker.Variable.HasValuesets() Then
            If (Marker.Variable.HasGroupings() OrElse Marker.Variable.HasValuesets()) AndAlso Marker.Variable.CurrentValueSet Is Nothing AndAlso Marker.Variable.CurrentGrouping Is Nothing Then
                SearchButton.Visible = False
                SelectAllButton.Visible = False
                DeselectAllButton.Visible = False
                SelectionFromGroupButton.Visible = False
                SelectedStatistics.Visible = False
                If ValuesListBox.Items.Count = 0 Then
                    HideListBox()
                    SearchPanel.Visible = False
                End If
            End If
            ''  SelectAllButton.ValidationGroup = ValuesListBox.ClientID

            'Should not be able to get statistics data for aggregated items when aggregation is not allowed
            If Not Marker.Variable.Meta.AggregAllowed And Marker.SelectedGroupingPresentation = GroupingIncludesType.AggregatedValues Then
                ValuesListBox.Enabled = False

                If Marker.SearchButtonMode = VariableSelectorSearchButtonViewMode.Always Then
                    SearchButton.Visible = False
                End If
            Else
                ValuesListBox.Enabled = True
            End If

        End If
        UpdateSelectedStats()
    End Sub

    ''' <summary>
    ''' Select first content and time value in listbox
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SelectFirstContentAndTimeValue()
        If Marker.Variable.IsContentVariable Or Marker.Variable.IsTime Then
            ValuesListBox.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Set visibility for search panel
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSearchPanelVisibility()
        If Marker.Variable.Values.Count > Marker.ListSize Then
            SearchPanel.Visible = True
            SetSearchTipText()
        Else
            SearchPanel.Visible = False
        End If
    End Sub

    Private Sub SetSearchTipText()
        Dim searchTipSb as New StringBuilder()
        searchTipSb.Append(GetLocalizedString("CtrlVariableSelectorSearchValuesTextboxScreenReader"))
        Dim counter As Integer = 0
        For Each value As Value In Marker.Variable.Values
            searchTipSb.Append(value.Text + " , ")
            counter += 1
            If counter = 3
                Exit For
            End If
        Next

        SearchTip.Text = searchTipSb.ToString()
    End Sub

    Protected Sub VariableValueLinksRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        CreateLink(e,"divVarValLink")
    End Sub

    private Sub CreateLink(Byval e As RepeaterItemEventArgs , byval placeholder As String)
        Dim itm As RepeaterItem = e.Item

        Dim currentItem As Metadata.MetaLink = TryCast(e.Item.DataItem, Metadata.MetaLink)

        Dim ph As HtmlGenericControl = TryCast(itm.FindControl(placeholder), HtmlGenericControl)
        if (currentItem IsNot Nothing)
            Dim lnk As HyperLink = New HyperLink()
            lnk.Text = String.Format("<span class='link-text'>{0}</span>", currentItem.LinkText)
            lnk.NavigateUrl = currentItem.Link
            lnk.Target = currentItem.Target
            lnk.CssClass = "external-link-pxbox-icon " + currentItem.CssClass
            lnk.Attributes.Add("rel", "noopener")
            ph.Controls.Add(lnk)
        End If
    End Sub

    protected Sub VariableValueRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)

        Dim itm As RepeaterItem = e.Item
        Dim currentItem As MetaItem = TryCast(e.Item.DataItem, MetaItem)

        Dim lbl As Label = TryCast(itm.FindControl("lblVariableValueName"),Label)
        If String.IsNullOrWhiteSpace(currentItem.Name) Then
            lbl.Visible = False
        Else
            lbl.Text = currentItem.Name
        End If

        Dim rep As Repeater = TryCast(itm.FindControl("VariableValueLinksRepeater"), Repeater)
        rep.DataSource = currentItem.Links
        rep.DataBind()
    End Sub

    Private Class MetaItem
        ''' <summary>
        ''' Variable/Value name
        ''' </summary>
        public Name As String
        ''' <summary>
        ''' List of links
        ''' </summary>
        public Links As List(Of PCAxis.Metadata.MetaLink)

        ''' <summary>
        ''' Constructor
        ''' </summary>
        Public Sub MetaItem()
            Links = New List(of PCAxis.Metadata.Metalink)
        End Sub
    End Class

    Private Function GetVariableLinks() As List(Of MetaItem)
        Dim lst = New List(Of MetaItem)
        Dim itm = New MetaItem()
        itm.Name = "" ' Marker.Variable.Name

        If Not String.IsNullOrWhiteSpace(Marker.Variable.MetaId) Then
            itm.Links = Marker.MetaLinkProvider.GetVariableLinks(Marker.Variable.MetaId, LocalizationManager.CurrentCulture.Name).ToList()
        End If

        If itm.Links IsNot Nothing Then
            lst.Add(itm)
        End If

        Return lst
    End Function

    Private Function GetValueLinks() As List(Of MetaItem)
        Dim myOut = New List(Of MetaItem)
        For Each value As Value In Marker.Variable.Values
            If Not String.IsNullOrWhiteSpace(value.MetaId) Then
                Dim itm = New MetaItem()
                itm.Name = value.Text
                itm.Links = Marker.MetaLinkProvider.GetValueLinks(value.MetaId, LocalizationManager.CurrentCulture.Name).ToList()
                ' Only display value if it has metadata links
                If itm.Links.Count > 0 Then
                    If itm.Links.Count = 1 Then
                        'Moves the value.Text from the "link heading" to the link text
                        itm.Links(0).LinkText = itm.Links(0).LinkText + " " + value.Text
                        itm.Name = ""
                    End If
                    myOut.Add(itm)
                End If
            End If
        Next

        Return myOut
    End Function

    Private Sub FillMetaData()
        Dim lstVariableValueLinks As List(Of MetaItem) = GetVariableLinks()
        lstVariableValueLinks.AddRange(GetValueLinks())

        If  lstVariableValueLinks.Count > 0
            VariableValueRepeater.DataSource = lstVariableValueLinks
            VariableValueRepeater.DataBind()
            VariableValueRepeater.Visible = true
        Else
            VariableValueRepeater.Visible = false
        End If
    End Sub

    Private Function GetLocalizedSearchResultTextForScreenReader() As String
        Dim searchList As New List(Of String) From {
            GetLocalizedString("CtrlVariableSelectorSearchResultSearchWordScreenReader"),
            GetLocalizedString("CtrlVariableSelectorSearchResultHitsScreenReader1"),
            GetLocalizedString("CtrlVariableSelectorSearchResultHitsScreenReader2"),
            GetLocalizedString("CtrlVariableSelectorSearchResultHitsScreenReader3"),
            GetLocalizedString("CtrlVariableSelectorSearchResultHitsScreenReader4")
        }

        Dim jsJoinedString As String
        jsJoinedString = string.Format(string.Join("|", searchList))
        Return jsJoinedString
    End Function



    Private Sub HideListBox()
        ValuesSelectPanel.Enabled = False
        ValuesListBox.Attributes.CssStyle.Add("min-height", "0px")
        ValuesListBox.Attributes.CssStyle.Add("height", "0px")
        ValuesListBox.Attributes.CssStyle.Add("visibility", "hidden")
        'HiddenEventButtons.Visible = False

    End Sub

    Private Sub ShowListBox()
        ValuesSelectPanel.Enabled = True
        ValuesListBox.Attributes.CssStyle.Add("visibility", "visible")
        ValuesListBox.Attributes.CssStyle.Clear()
    End Sub


End Class
