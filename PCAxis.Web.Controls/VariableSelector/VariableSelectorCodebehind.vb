Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Paxiom
Imports PCAxis.Query
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management

Public Class VariableSelectorCodebehind
    Inherits PaxiomControlBase(Of VariableSelectorCodebehind, VariableSelector)

#Region "Localized strings"
    Private Const ROW_SELECTION_LIMIT_EXCEEDED As String = "CtrlVariableSelectorSelectionLimitRowsExceeded"
    Private Const COL_SELECTION_LIMIT_EXCEEDED As String = "CtrlVariableSelectorSelectionLimitColsExceeded"
    Private Const CELL_SELECTION_LIMIT_EXCEEDED As String = "CtrlVariableSelectorSelectionLimitCellsExceeded"
#End Region


#Region " Controls "

    Protected VariableSelectorPanel As Panel
    Protected VariableSelectorMarkingTips As VariableSelectorMarkingTips
    Protected VariableSelectorEliminationInformation As VariableSelectorEliminationInformation
    Protected WithEvents VariableSelectorValueSelectRepeater As Repeater
    Protected WithEvents ButtonViewTable As Button
    Protected SelectionErrorlabel As Label
    Protected SelectionErrorlabelTextCells As Label
    Protected SelectionErrorlabelTextColumns As Label
    Protected SelectionErrorlabelTextRows As Label
    Protected WithEvents SearchVariableValues As SearchValues
    Protected VariableSelectorSelectionInformation As VariableSelectorSelectionInformation
    Protected SearchVariableValuesPanel As Panel
    Protected HierarchicalSelectPanel As Panel
    Protected SelectFromGroupPanel As Panel
    Protected WithEvents SelectHierarchichalVariable As Hierarchical
    Protected WithEvents SelectValuesFromGroup As SelectFromGroup
    Private buttonClicked As Boolean = False 'If the Continue button is clicked or not
    Protected SelectionValidationSummary As ValidationSummary

#End Region

#Region " Properties "

    ''' <summary>
    ''' Read only property for exposing the clientID of the search button
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SearchButtonClientID() As String
        Get
            Return Me.SearchVariableValues.SearchButtonClientID
        End Get
    End Property
#End Region

    ''' <summary>
    ''' Loads variableselector controls
    ''' </summary> 
    Protected Sub VariableSelector_Load() Handles Me.Load


        Dim haveSetCandidateMustSelect As Boolean = Marker.SortVariableOrder
        For Each var As Paxiom.Variable In Me.PaxiomModel.Meta.Variables
            If Not var.ExtendedProperties.ContainsKey("CandidateMustSelect") Then
                haveSetCandidateMustSelect = False
                Exit For
            End If
        Next
        Dim query As IEnumerable(Of Paxiom.Variable)
        'Make the content variable to be the first, then time, then variables that might be non eliminateable displayed in VariableSelector
        If haveSetCandidateMustSelect Then
            query = From v In Me.PaxiomModel.Meta.Variables Order By v.IsContentVariable Descending, v.IsTime Descending, v.ExtendedProperties("CandidateMustSelect") Descending Select v
        Else               'Make the content variable to be the first. rest sorted as default from parser
            query = From v In Me.PaxiomModel.Meta.Variables Order By v.IsContentVariable Descending
        End If



        'Clientsidecalidation if true, else serverside

        If Marker.ClientSideValidation Then
            ButtonViewTable.OnClientClick = "return ValidateAll()"
        Else
            ButtonViewTable.OnClientClick = ""
        End If

        'VariableSelectorValueSelectRepeater.DataSource = Me.PaxiomModel.Meta.Variables
        VariableSelectorValueSelectRepeater.DataSource = query
        VariableSelectorValueSelectRepeater.DataBind()

        SetLocalizedTexts()

        VariableSelectorMarkingTips.MarkingTipsLinkNavigateUrl = Marker.MarkingTipsLinkNavigateUrl
        SelectHierarchichalVariable.HierarchyLevelsOpen = Marker.HierarchicalSelectionLevelsOpen


        VariableSelectorSelectionInformation.ShowSelectionLimits = Marker.ShowSelectionLimits
        VariableSelectorSelectionInformation.ShowSelectionsMade = Marker.ShowSelectedRowsColumns
        VariableSelectorSelectionInformation.LimitSelectionsBy = Marker.LimitSelectionsBy
        VariableSelectorSelectionInformation.SelectedTotalCellsLimit = Marker.SelectedTotalCellsDownloadLimit
        VariableSelectorSelectionInformation.SelectedColumnsLimit = Marker.SelectedColumnsLimit
        VariableSelectorSelectionInformation.SelectedRowsLimit = Marker.SelectedRowsLimit

        If Marker.ShowMarkingTips Then
            VariableSelectorMarkingTips.Visible = True
        Else
            VariableSelectorMarkingTips.Visible = False
        End If

        Me.SearchVariableValues.ShowSearchInformationLink = Marker.ShowSearchInformationLink
        Me.SearchVariableValues.SearchInformationLinkText = Marker.SearchInformationLinkText
        Me.SearchVariableValues.SearchInformationLinkURL = Marker.SearchInformationLinkURL
        Me.SearchVariableValues.ShowTableName = Marker.ShowTableNameInSearch
        Me.SearchVariableValues.ShowAllAvailableValuesButton = Marker.ShowAllAvailableValuesSearchButton
        Me.SearchVariableValues.AlwaysShowCodeAndTextInSearchResult = Marker.AlwaysShowCodeAndTextInAdvancedSearchResult

        InitializeSelectedValuesetsAndGroupingsOnLoad()

        If Marker.ReloadGroupings OrElse _previousModel IsNot Nothing Then
            ReApplyGroupings()
        End If
    End Sub

    ''' <summary>
    ''' Handles event PreRender
    ''' </summary> 
    Protected Sub VariableSelector_PreRender() Handles Me.PreRender
        If Not buttonClicked Then
            AutomaticSelection()
        End If
    End Sub

    ''' <summary>
    ''' Handles automatic view of table with "default selection" or "All selection"
    ''' </summary> 
    ''' <remarks>Triggered by querystring action set to "selectdefaul" or "selectall" </remarks> 
    Protected Sub AutomaticSelection()
        ' --- Case querystring contains action=selectdefault, select default values for all variables and view table automatically
        If (QuerystringManager.GetQuerystringParameter("action") = "selectdefault") Then
            SaveSelections()
            ViewTable()
        End If

        ' --- Case querystring contains action=selectall, select all values for all variables and view table automatically
        If (QuerystringManager.GetQuerystringParameter("action") = "selectall") Then
            SaveSelections()
            ViewTable()
        End If

        If (QuerystringManager.GetQuerystringParameter("px_autoopen") = "true") Then
            SelectAllValues()
            ViewTable()
        End If

    End Sub

    ''' <summary>
    ''' Add VariableSelectorValueSelect controll and necessary events for each variable in RepeaterValueSelect
    ''' </summary>   
    Protected Sub RepeaterValueSelect_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles VariableSelectorValueSelectRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim var As Variable = DirectCast(e.Item.DataItem, Variable)

            Dim ValueSelectPlaceHolder As PlaceHolder = DirectCast(item.FindControl("ValueSelectPlaceHolder"), PlaceHolder)
            If ValueSelectPlaceHolder.Controls.Count = 0 Then
                Dim VariableSelect As New VariableSelectorValueSelect
                VariableSelect.ID = "VariableValueSelect"
                VariableSelect.Variable = var
                VariableSelect.LimitSelectionsBy = Marker.LimitSelectionsBy
                VariableSelect.ShowElimMark = Marker.ShowElimMark
                VariableSelect.ClientSideValidation = Marker.ClientSideValidation
                VariableSelect.ShowHierarchies = Marker.ShowHierarchies
                VariableSelect.AllowAggreg = Marker.AllowAggreg
                VariableSelect.SearchButtonMode = Marker.SearchButtonMode
                VariableSelect.MaxRowsWithoutSearch = Marker.MaxRowsWithoutSearch
                VariableSelect.AlwaysShowTimeVariableWithoutSearch = Marker.AlwaysShowTimeVariableWithoutSearch
                VariableSelect.EliminationImagePath = Marker.EliminationImagePath
                VariableSelect.ListSize = Marker.ListSize
                VariableSelect.NumberOfValuesInDefaultView = Marker.NumberOfValuesInDefaultView
                VariableSelect.JavascriptRowLimit = Marker.JavascriptRowLimit
                VariableSelect.ValuesetMustBeSelectedFirst = Marker.ValuesetMustBeSelectedFirst
                VariableSelect.SelectionFromGroupButtonMode = Marker.SelectionFromGroupButtonMode
                VariableSelect.MetadataInformationButtonMode = Marker.MetadataInformationMode
                VariableSelect.ButtonsForContentVariable = Marker.ButtonsForContentVariable
                VariableSelect.SearchValuesBeginningOfWordCheckBoxDefaultChecked = Marker.SearchValuesBeginningOfWordCheckBoxDefaultChecked
                VariableSelect.PreSelectFirstContentAndTime = Marker.PreSelectFirstContentAndTime
                VariableSelect.MetaLinkProvider = Marker.MetaLinkProvider
                ValueSelectPlaceHolder.Controls.Add(VariableSelect)
                AddHandler VariableSelect.SelectHierarchicalButtonClicked, AddressOf VariableSelector_SelectHierarchicalValueButtonClicked
                AddHandler VariableSelect.SearchLargeNumberOfValuesButtonClicked, AddressOf VariableSelector_SearchLargeNumberOfValuesButtonClicked
                AddHandler VariableSelect.SelectFromGroupButtonClicked, AddressOf VariableSelector_SelectFromGroupButtonClicked
                AddHandler VariableSelect.MetadataInformationButtonClicked, AddressOf VariableSelector_MetadataInformationClicked
                If PCAxis.Paxiom.Settings.Metadata.RemoveSingleContent AndAlso var.IsContentVariable AndAlso var.Values.Count = 1 Then
                    'Don´t show the content variable if it only contains one value
                    VariableSelect.Visible = False
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Triggered when Language is changed
    ''' Nothing is done here for the moment
    ''' </summary>    
    Private Sub VariableSelector_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        SetLocalizedTexts()
    End Sub

    ''' <summary>
    ''' Set languange dependent texts
    ''' </summary>    
    Private Sub SetLocalizedTexts()
        ButtonViewTable.Text = GetLocalizedString("CtrlVariableSelectorContinueButton")
        SelectionValidationSummary.HeaderText = "<span>" + GetLocalizedString("CtrlVariableSelectorValidationSummary") + "</span>"
        If (Marker.LimitSelectionsBy = "RowsColumns") Then
            SelectionErrorlabelTextRows.Text = String.Format(Me.GetLocalizedString(ROW_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedRowsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            SelectionErrorlabelTextColumns.Text = String.Format(Me.GetLocalizedString(COL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedColumnsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
        Else
            SelectionErrorlabelTextCells.Text = String.Format(Me.GetLocalizedString(CELL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedTotalCellsDownloadLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
        End If
    End Sub

    ''' <summary>
    ''' Save selections made in the listboxes for each variable
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveSelections()
        VariableSelector.SelectedVariableValues.Clear()

        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            Dim selection = variableSelectorValueSelect.Selection
            VariableSelector.SelectedVariableValues.Add(variableSelectorValueSelect.Variable.Code, selection)
        Next
    End Sub

    ''' <summary>
    ''' Renders selections made in variable listboxes as saved in statevariable
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub RenderSelectionsMade()
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            variableSelectorValueSelect.RenderSelection()
        Next
    End Sub

    ''' <summary>
    ''' Selects default values for all variables
    ''' </summary>
    ''' <remarks>Number of values in defaultselection is set by property NumberOfValuesInDefaultView</remarks>
    Friend Sub SelectDefaultValues()
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            variableSelectorValueSelect.ShowAllValues = True
            variableSelectorValueSelect.SelectDefaultValues()
        Next
    End Sub

    ''' <summary>
    ''' Selects all values for all variables
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub SelectAllValues()
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            variableSelectorValueSelect.ShowAllValues = True
            variableSelectorValueSelect.SelectAllValues()
        Next
    End Sub

    ''' <summary>
    ''' Handles buttonclick event in VariableSelectorValueSelect to trigger selection of values for hierarchical variable through treeview
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VariableSelector_SelectHierarchicalValueButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Variable)
        SaveSelections()
        SelectHierarchichalVariable.Variable = variable
        SelectHierarchichalVariable.RenderHierarchicalTree()
        HierarchicalSelectPanel.Visible = True
        VariableSelectorPanel.Visible = False
    End Sub

    Private Sub VariableSelector_SelectFromGroupButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Variable)
        SaveSelections()
        SelectValuesFromGroup.Variable = variable
        SelectValuesFromGroup.InitiateSearch()
        SelectFromGroupPanel.Visible = True
        VariableSelectorPanel.Visible = False
        Marker.OnLeaveVariableSelectorMain(e)
    End Sub

    Private Sub VariableSelector_MetadataInformationClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Variable)
        Dim args As New VariableSelector.MatadataInformationEventArgs
        args.Variable = variable
        args.MetaId = variable.MetaId
        Marker.OnMetadataInformationSelected(sender, args)

    End Sub

    ''' <summary>
    ''' Changes selection in listbox with values for variable according to selection done by hierarchical selection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <param name="hierarchicalVariableName">Name of hierarchical variable the selection has be made for</param>
    ''' <remarks></remarks>
    Private Sub SelectHierarchichalVariable_SelectionsDone(ByVal sender As Object, ByVal e As System.EventArgs, ByVal hierarchicalVariableName As String) Handles SelectHierarchichalVariable.SelectionsDone
        HierarchicalSelectPanel.Visible = False
        VariableSelectorPanel.Visible = True
        RenderSelectionsMade()
    End Sub

    ''' <summary>
    ''' Changes selection in listbox with values for variable according to selection done in "Selection from group"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SelectValuesFromGroup_SelectionDone(sender As Object, e As SelectFromGroupEventArgs) Handles SelectValuesFromGroup.SelectionDone
        SelectFromGroupPanel.Visible = False
        VariableSelectorPanel.Visible = True

        'Switch to the aggregation selected in the "Select values from group" dialog
        'Find the right control
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)

            If variableSelectorValueSelect.Variable.Code.Equals(e.VariableCode) Then
                variableSelectorValueSelect.ChangeSelectedGrouping(e.Aggregation, e.Includes)
                Exit For
            End If
        Next

        RenderSelectionsMade()
        Marker.OnReenterVariableSelectorMain(e)
    End Sub
    ''' <summary>
    ''' Handles buttonclick event in VariableSelectorValueSelect to trigger selection of values for variable through valuesearch
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VariableSelector_SearchLargeNumberOfValuesButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Variable)
        SaveSelections()
        SearchVariableValues.Variable = variable
        SearchVariableValues.InitiateSearch()
        VariableSelectorPanel.Visible = False
        SearchVariableValuesPanel.Visible = True
        Marker.OnLeaveVariableSelectorMain(e)
    End Sub

    ''' <summary>
    ''' Changes selection in listbox with values for variable according to selection done by search selection.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SearchVariableValues_SearchVariableValuesAdd(ByVal sender As Object, ByVal e As EventArgs) Handles SearchVariableValues.SelectionsDone 'SearchVariableValuesAdd
        VariableSelectorPanel.Visible = True
        SearchVariableValuesPanel.Visible = False
        RenderSelectionsMade()
        Marker.OnReenterVariableSelectorMain(e)
    End Sub


    '''' <summary>
    '''' Variable selection done.
    '''' </summary>
    '''' <remarks></remarks>
    Private Sub ButtonViewTable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewTable.Click
        buttonClicked = True
        Page.Validate()
        ViewTable()
    End Sub

    ''' <summary>
    ''' Check that number of selected rows and number of selected columns are within configured limits if LimitSelectionsBy = "RowsCols"
    ''' Check that number of selected cells are within configured limits if LimitSelectionsBy = "Cells"
    ''' </summary>
    ''' <param name="selectedRows">Total count of selected rows</param>
    ''' <param name="selectedColumns">Total count of selected columns</param>
    ''' <returns>True if selection are within the configured limits</returns>
    ''' <remarks></remarks>
    Private Function CheckNumberOfSelected(ByVal selectedRows As Integer, ByVal selectedColumns As Integer) As Boolean
        Dim message As String = ""
        'Reset selection error message
        SelectionErrorlabel.Text = ""
        If (Marker.LimitSelectionsBy = "RowsColumns") Then
            SelectionErrorlabelTextRows.Text = String.Format(Me.GetLocalizedString(ROW_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedRowsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            SelectionErrorlabelTextColumns.Text = String.Format(Me.GetLocalizedString(COL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedColumnsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
        Else
            SelectionErrorlabelTextCells.Text = String.Format(Me.GetLocalizedString(CELL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedTotalCellsDownloadLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
        End If

        If (Marker.LimitSelectionsBy = "RowsColumns") Then
            'Check that number of selected rows and number of selected columns are within configured limits
            If selectedRows > Marker.SelectedRowsLimit Then
                message += String.Format(Me.GetLocalizedString(ROW_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedRowsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            End If
            If selectedColumns > Marker.SelectedColumnsLimit Then
                message += String.Format(Me.GetLocalizedString(COL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedColumnsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            End If
        Else
            'Check that number of selected cells are within configured limits
            If (selectedRows * selectedColumns) > Marker.SelectedTotalCellsDownloadLimit Then
                message += String.Format(Me.GetLocalizedString(CELL_SELECTION_LIMIT_EXCEEDED), DataFormatter.NumericToString(Marker.SelectedTotalCellsDownloadLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            End If
        End If

        If message.Length > 0 Then
            'To many rows/columns or cells selected
            SelectionErrorlabel.Text = message
            Return False
        Else
            'OK
            Return True
        End If

    End Function



    ''' <summary>
    ''' Build Paxiommodel and view table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ViewTable()

        SaveSelections()
        Dim sels As New List(Of Selection)
        Dim selectedRows As Integer = 0
        Dim selectedColumns As Integer = 0
        For Each variable As Variable In Me.PaxiomModel.Meta.Variables
            Dim sel As Selection = New Selection(variable.Code)
            For Each Val As String In VariableSelector.SelectedVariableValues(variable.Code).ValueCodes
                sel.ValueCodes.Add(Val)
            Next

            sels.Add(sel)
            'Count selected rows and columns
            If sel.ValueCodes.Count > 0 Then
                If variable.Placement = PlacementType.Heading Then
                    If (selectedColumns = 0) Then
                        selectedColumns = sel.ValueCodes.Count
                    Else
                        selectedColumns *= sel.ValueCodes.Count
                    End If
                ElseIf variable.Placement = PlacementType.Stub Then
                    If (selectedRows = 0) Then
                        selectedRows = sel.ValueCodes.Count
                    Else
                        selectedRows *= sel.ValueCodes.Count
                    End If
                End If
            End If
        Next

        'Check that number of selected rows and number of selected columns are within configured limits if LimitSelectionsBy = "RowsCols"
        'Check that number of selected cells are within configured limits if LimitSelectionsBy = "Cells"
        If (Not CheckNumberOfSelected(selectedRows, selectedColumns)) Then
            Exit Sub
        End If


        Dim selections() As Paxiom.Selection
        selections = sels.ToArray()

        If (IsEliminationSelectionsDone()) Then
            Dim builder As Paxiom.IPXModelBuilder
            Dim query As New PCAxis.Query.TableQuery(PaxiomManager.PaxiomModel, selections)

            Dim contentDictionary As New Dictionary(Of String, String)()

            'Set PaxiomManager.Content if only one content is selected and RemoveSingleContent is true
            If Settings.Metadata.RemoveSingleContent = True And PaxiomManager.PaxiomModel.Meta.ContentVariable IsNot Nothing And sels.FirstOrDefault(Function(x) x.VariableCode.Equals(PaxiomManager.PaxiomModel.Meta.ContentVariable.Code)).ValueCodes.Count = 1 Then
                contentDictionary.Add(sels.FirstOrDefault(Function(x) x.VariableCode.Equals(PaxiomManager.PaxiomModel.Meta.ContentVariable.Code)).VariableCode, sels.FirstOrDefault(Function(x) x.VariableCode.Equals(PaxiomManager.PaxiomModel.Meta.ContentVariable.Code)).ValueCodes(0))
            Else
                contentDictionary = Nothing
            End If

            builder = Management.PaxiomManager.PaxiomModelBuilder
            If Not builder.BuildForPresentation(selections) Then
                'Fatal error when building for presentation - Show error message
                With builder.Errors(0)
                    If .Params IsNot Nothing Then
                        SelectionErrorlabel.Text = String.Format(PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString(.Code), .Params)
                    Else
                        SelectionErrorlabel.Text = PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString(.Code)
                    End If
                End With
                Exit Sub
            End If
            Management.PaxiomManager.PaxiomModelBuilder = Nothing
            builder.Model.IsComplete = True
            PaxiomManager.PaxiomModel = builder.Model
            PaxiomManager.QueryModel = query
            PaxiomManager.OperationsTracker = New OperationsTracker()
            PaxiomManager.SingleContentSelection = contentDictionary

            SignalAction()

            Marker.ScreenOutputMethod.Invoke(Marker.PresentationView, Me.PaxiomModel)

        Else
            Dim VariableMissingSelection As String = ""
            For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
                Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
                If Not (variableSelectorValueSelect.EliminationSelectionsDone) Then
                    VariableMissingSelection = variableSelectorValueSelect.Variable.Name
                End If
            Next
            '  SelectionErrorlabel.Text = GetLocalizedString("CtrlVariableSelectorSelectionMissingErrorMessage") + VariableMissingSelection
        End If

    End Sub

    ''' <summary>
    ''' Signals the action to listeners
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SignalAction()
        Dim args As PxActionEventArgs

        'Presentation view
        args = New PxActionEventArgs(PxActionEventType.Presentation, Me.Marker.PresentationView)
        PxActionEventHelper.SetModelProperties(args, PaxiomManager.PaxiomModel)

        Marker.OnPxActionEvent(args)
    End Sub


    ''' <summary>
    ''' Returns true if valueselections is done for all variables not possible to eliminate, else false
    ''' </summary>
    ''' <remarks></remarks>
    Friend Function IsEliminationSelectionsDone() As Boolean
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            If Not (variableSelectorValueSelect.EliminationSelectionsDone) Then
                Return False
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Returns the current view mode of the VariableSelector web control
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ViewMode() As VariableSelectorViewMode
        If VariableSelectorPanel.Visible Then
            Return VariableSelectorViewMode.Selection
        ElseIf SearchVariableValuesPanel.Visible Then
            Return VariableSelectorViewMode.Search
        ElseIf HierarchicalSelectPanel.Visible Then
            Return VariableSelectorViewMode.Hierarchical
        Else
            Return VariableSelectorViewMode.Selection
        End If
    End Function

    Friend Sub ReApplyGroupings()
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            If Not String.IsNullOrEmpty(variableSelectorValueSelect.SelectedGrouping) Then
                variableSelectorValueSelect.ApplyGrouping(variableSelectorValueSelect.SelectedGrouping, False, variableSelectorValueSelect.SelectedGroupingPresentation)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Initialize which values that shall be selected by default
    ''' </summary>
    ''' <param name="model"></param>
    ''' <remarks></remarks>
    Public Sub InitializeSelection(ByVal model As PXModel)

        If model Is Nothing Then
            Exit Sub
        End If

        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim VariableSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)

            If VariableSelect Is Nothing Then
                Exit Sub
            End If

            Dim variable As PCAxis.Paxiom.Variable = model.Meta.Variables.GetByCode(VariableSelect.Variable.Code)

            If variable Is Nothing Then
                Continue For 'Probably a variable which supports Elimination
            End If

            VariableSelect.SelectValues(variable.Values)
        Next
    End Sub

    Private _previousModel As PXModel = Nothing

    Public Sub InitializeSelectedValuesetsAndGroupings(ByVal model As PXModel)
        _previousModel = model
    End Sub

    Public Sub InitializeSelectedValuesetsAndGroupingsOnLoad()
        If _previousModel Is Nothing OrElse Core.Management.PaxiomManager.QueryModel Is Nothing OrElse Core.Management.PaxiomManager.QueryModel.Query Is Nothing Then
            Exit Sub
        End If

        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim VariableSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)

            If VariableSelect Is Nothing Then
                Exit Sub
            End If

            Dim valuesetsAndGroupingsCount As Integer = 0

            If VariableSelect.Variable.ValueSets IsNot Nothing Then
                valuesetsAndGroupingsCount += VariableSelect.Variable.ValueSets.Count
            End If

            If VariableSelect.Variable.Groupings IsNot Nothing Then
                valuesetsAndGroupingsCount += VariableSelect.Variable.Groupings.Count
            End If

            If (valuesetsAndGroupingsCount > 0) Then


                Dim queryItem = Core.Management.PaxiomManager.QueryModel.Query.FirstOrDefault(Function(x) x.Code = VariableSelect.Variable.Code)

                If queryItem IsNot Nothing Then
                    Dim selectedGroup As String = Nothing
                    Dim selectedValueset As String = Nothing
                    Dim selectedGroupOrValueset = queryItem.Selection.Filter.Split(":"c).Last()

                    If VariableSelect.Variable.Groupings IsNot Nothing Then
                        Dim selectedGroupItem = VariableSelect.Variable.Groupings.FirstOrDefault(Function(x) x.ID = selectedGroupOrValueset)

                        If selectedGroupItem IsNot Nothing Then
                            selectedGroup = selectedGroupItem.ID
                        End If
                    End If

                    If selectedGroup Is Nothing AndAlso VariableSelect.Variable.ValueSets IsNot Nothing Then
                        Dim selectedValuesetItem = VariableSelect.Variable.ValueSets.FirstOrDefault(Function(x) x.ID = selectedGroupOrValueset)

                        If selectedValuesetItem IsNot Nothing Then
                            selectedValueset = selectedValuesetItem.ID
                        End If
                    End If

                    If selectedGroup IsNot Nothing Then 'Grouping has higher precedence than valaueset
                        VariableSelect.SelectedGrouping = "gr__" + selectedGroup
                    ElseIf selectedValueset IsNot Nothing Then
                        VariableSelect.SelectedGrouping = "vs__" + selectedValueset
                    End If
                End If
            End If
        Next
    End Sub

End Class



