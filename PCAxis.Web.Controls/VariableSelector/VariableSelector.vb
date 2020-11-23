Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Enums
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Metadata


''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class VariableSelector
    Inherits MarkerControlBase(Of VariableSelectorCodebehind, VariableSelector)

    Public Class MatadataInformationEventArgs
        Inherits System.EventArgs

        Public Variable As PCAxis.Paxiom.Variable
        Public TableId As Integer
        Public MetaId As String
    End Class
#Region " Events "

    'Public Event SelectHierarchicalButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal hierarchicalVariableName As String)
    'Friend Sub OnSelectHierarchicalButtonClicked(ByVal e As EventArgs, ByVal hierarchicalVariableName As String)
    '    RaiseEvent SelectHierarchicalButtonClicked(Me, e, hierarchicalVariableName)
    'End Sub

    'Public Event SelectFromGroupButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variableName As String)
    'Friend Sub OnSelectFromGroupButtonClicked(ByVal e As EventArgs, ByVal variableName As String)
    '    RaiseEvent SelectFromGroupButtonClicked(Me, e, variableName)
    'End Sub

    'Public Event SearchLargeNumberOfValuesButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal Variable As Variable)
    'Friend Sub OnSearchLargeNumberOfValuesButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Variable)
    '    RaiseEvent SearchLargeNumberOfValuesButtonClicked(sender, e, variable)
    'End Sub

    Public Event LeaveVariableSelectorMain(ByVal sender As Object, ByVal e As EventArgs)
    Friend Sub OnLeaveVariableSelectorMain(ByVal e As EventArgs)
        RaiseEvent LeaveVariableSelectorMain(Me, e)
    End Sub

    Public Event ReenterVariableSelectorMain(ByVal sender As Object, ByVal e As EventArgs)
    Friend Sub OnReenterVariableSelectorMain(ByVal e As EventArgs)
        RaiseEvent ReenterVariableSelectorMain(Me, e)
    End Sub

    Public Event ViewTableAutomatically(ByVal sender As Object, ByVal e As EventArgs)
    Friend Sub OnViewTableAutomatically()
        RaiseEvent ViewTableAutomatically(Me, New EventArgs())
    End Sub

    ''' <summary>
    ''' Event used to signal when a button for meta data information is clickt.
    ''' </summary>
    Public Event MetadataInformationSelected(ByVal sender As Object, ByVal e As MatadataInformationEventArgs)
    ''' <summary>
    ''' Raises an event to signal when value meta data button is clickt
    ''' </summary>
    Friend Sub OnMetadataInformationSelected(ByVal sender As Object, ByVal e As MatadataInformationEventArgs)
        RaiseEvent MetadataInformationSelected(Me, e)
    End Sub

    ''' <summary>
    ''' Signal PX action to listeners
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PxActionEvent As PxActionEventHandler
    Friend Sub OnPxActionEvent(ByVal e As PxActionEventArgs)
        RaiseEvent PxActionEvent(Me, e)
    End Sub


#End Region


#Region "Public methods"

    ''' <summary>
    ''' Initialize which values that shall be selected as default
    ''' </summary>
    ''' <param name="model">PXModel object</param>
    ''' <remarks></remarks>
    Public Sub InitializeSelection(ByVal model As PXModel)
        Control.InitializeSelection(model)
    End Sub

    Public Sub InitializeSelectedValuesetsAndGroupings(ByVal model As PXModel)
        Control.InitializeSelectedValuesetsAndGroupings(model)
    End Sub

#End Region


#Region " Properties "

    ''' <summary>
    ''' Describes the current view mode of the VariableSelector web control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Valid value is given during the PreRender part of the web page life cycle</remarks>
    Public ReadOnly Property ViewMode() As VariableSelectorViewMode
        Get
            Return Control.ViewMode()
        End Get
    End Property

    ''' <summary>
    ''' Property holding selections done for all variables in a table.
    ''' </summary>
    Public Shared ReadOnly Property SelectedVariableValues() As Dictionary(Of String, Selection)
        Get
            Const NAME_SELECTEVARIABLES As String = "SelectVariables"
            If Not StateProvider.StateProviderFactory.GetStateProvider().Contains(GetType(VariableSelector), NAME_SELECTEVARIABLES) Then
                StateProvider.StateProviderFactory.GetStateProvider.Add(GetType(VariableSelector), NAME_SELECTEVARIABLES, New Dictionary(Of String, Selection))
            End If
            Return CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(VariableSelector), NAME_SELECTEVARIABLES), Global.System.Collections.Generic.Dictionary(Of String, Global.PCAxis.Paxiom.Selection))
        End Get
    End Property

    ''' <summary>
    ''' Property holding information on if selections have been done for variables not possible to eliminate.
    ''' </summary>
    Public ReadOnly Property isEliminationSelectionsDone() As Boolean
        Get
            Return Control.IsEliminationSelectionsDone()
        End Get
    End Property

    Private _markingTipsLinkNavigateUrl As String
    ''' <summary>
    ''' Gets or sets the url for tips and help
    ''' </summary>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MarkingTipsLinkNavigateUrl() As String
        Get
            Return _markingTipsLinkNavigateUrl
        End Get
        Set(ByVal value As String)
            _markingTipsLinkNavigateUrl = value

        End Set
    End Property

    Private _searchButtonMode As VariableSelectorSearchButtonViewMode = VariableSelectorSearchButtonViewMode.ManyValues
    ''' <summary>
    ''' Controls when the search values button shall be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SearchButtonMode() As VariableSelectorSearchButtonViewMode
        Get
            Return _searchButtonMode
        End Get
        Set(ByVal value As VariableSelectorSearchButtonViewMode)
            _searchButtonMode = value
        End Set
    End Property

    Private _selectionFromGroup As Boolean
    ''' <summary>
    ''' Controls when the Selection from group button shall be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectionFromGroupButtonMode() As Boolean
        Get
            Return _selectionFromGroup
        End Get
        Set(ByVal value As Boolean)
            _selectionFromGroup = value
        End Set
    End Property

    Private _buttonsForContentVariable As Boolean
    ''' <summary>
    ''' If buttons shall be displayed for content variables or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)>
    Public Property ButtonsForContentVariable() As Boolean
        Get
            Return _buttonsForContentVariable
        End Get
        Set(ByVal value As Boolean)
            _buttonsForContentVariable = value
        End Set
    End Property

    Private _searchValuesBeginningOfWordCheckBoxDefaultChecked As Boolean
    ''' <summary>
    ''' Decides default search option. If false the search Is a inside text search
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)>
    Public Property SearchValuesBeginningOfWordCheckBoxDefaultChecked() As Boolean
        Get
            Return _searchValuesBeginningOfWordCheckBoxDefaultChecked
        End Get
        Set(ByVal value As Boolean)
            _searchValuesBeginningOfWordCheckBoxDefaultChecked = value
        End Set
    End Property

    Private _preSelectFirstContentAndTime As Boolean
    ''' <summary>
    ''' Decides if first content and time value are pre selected
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)>
    Public Property PreSelectFirstContentAndTime() As Boolean
        Get
            Return _preSelectFirstContentAndTime
        End Get
        Set(ByVal value As Boolean)
            _preSelectFirstContentAndTime = value
        End Set
    End Property


    Private _metadataInformation As Boolean
    ''' <summary>
    ''' Controls when the meta data button shall be displayed for a variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetadataInformationMode() As Boolean
        Get
            Return _metadataInformation
        End Get
        Set(ByVal value As Boolean)
            _metadataInformation = value
        End Set
    End Property

    Private _maxRowsWithoutSearch As Integer = 500
    ''' <summary>
    ''' Gets or sets the number of rows that will be shown without search
    ''' </summary>
    ''' <value>Number of rows that will be shown without search</value>
    ''' <returns>A <see cref="Integer" /> representing the number of rows that will be shown without search</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MaxRowsWithoutSearch() As Integer
        Get
            Return _maxRowsWithoutSearch
        End Get
        Set(ByVal value As Integer)
            _maxRowsWithoutSearch = value

        End Set
    End Property

    Private _alwaysShowTimeVariableWithoutSearch As Boolean = False
    ''' <summary>
    ''' Gets or sets the number of rows that will be shown without search
    ''' </summary>
    ''' <value>Number of rows that will be shown without search</value>
    ''' <returns>A <see cref="Integer" /> representing the number of rows that will be shown without search</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property AlwaysShowTimeVariableWithoutSearch() As Boolean
        Get
            Return _alwaysShowTimeVariableWithoutSearch
        End Get
        Set(ByVal value As Boolean)
            _alwaysShowTimeVariableWithoutSearch = value

        End Set
    End Property

    Private _javascriptRowLimit As Integer = 500
    ''' <summary>
    ''' Gets or sets the number of rows that will be used as limit when Javascript should be disabled 
    ''' This is used because Javascript is slower to execute than native code (Especially in IE)
    ''' 
    ''' </summary>
    ''' <value>Number of rows that will be used as Javascript limit</value>    
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property JavascriptRowLimit() As Integer
        Get
            Return _javascriptRowLimit
        End Get
        Set(ByVal value As Integer)
            _javascriptRowLimit = value

        End Set
    End Property



    Private _listSize As Integer
    ''' <summary>
    ''' Gets or sets the number of rows that will be shown without search
    ''' </summary>
    ''' <value>Number of rows that will be shown without search</value>
    ''' <returns>A <see cref="Integer" /> representing the number of rows that will be shown without search</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ListSize() As Integer
        Get
            Return _listSize
        End Get
        Set(ByVal value As Integer)
            _listSize = value

        End Set
    End Property

    Private _eliminationImagePath As String
    ''' <summary>
    ''' Gets or Sets the image URL of the elimination selection image.
    ''' 
    ''' The initial value is defined in ControlSettings.xml
    ''' E.g. "~/resources/images/dottra.gif"
    ''' </summary>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property EliminationImagePath() As String
        Get
            Return _eliminationImagePath
        End Get
        Set(ByVal value As String)
            _eliminationImagePath = value
        End Set
    End Property

    Private _clientSideValidation As Boolean = True
    ''' <summary>
    ''' Sholud validation of listbox be on client. If false then it will be serverside
    ''' 
    ''' The initial value is defined in ControlSettings.xml
    ''' E.g. True  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)>
    Public Property ClientSideValidation() As Boolean
        Get
            Return _clientSideValidation
        End Get
        Set(ByVal value As Boolean)
            _clientSideValidation = value
        End Set
    End Property


    Private _showElimMark As Boolean = True
    ''' <summary>
    ''' Show image indicating that eliminiation of variable selection not is valid.
    ''' 
    ''' The initial value is defined in ControlSettings.xml
    ''' E.g. True  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowElimMark() As Boolean
        Get
            Return _showElimMark
        End Get
        Set(ByVal value As Boolean)
            _showElimMark = value
        End Set
    End Property

    Private _showMarkingTips As Boolean = True
    ''' <summary>
    ''' Show image indicating that eliminiation of variable selection not is valid.
    ''' 
    ''' The initial value is defined in ControlSettings.xml
    ''' E.g. True  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowMarkingTips() As Boolean
        Get
            Return _showMarkingTips
        End Get
        Set(ByVal value As Boolean)
            _showMarkingTips = value
        End Set
    End Property


    Private _showSelectionLimits As Boolean
    ''' <summary>
    ''' Show/hide information about selection limits.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSelectionLimits() As Boolean
        Get
            Return _showSelectionLimits
        End Get
        Set(ByVal value As Boolean)
            _showSelectionLimits = value
        End Set
    End Property


    Private _selectedTotalCellsLimit As Integer
    ''' <summary>
    ''' Selection limit for total number of cells in selection when presentation on screen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedTotalCellsLimit() As Integer
        Get
            Return _selectedTotalCellsLimit
        End Get
        Set(ByVal value As Integer)
            _selectedTotalCellsLimit = value
        End Set
    End Property

    Private _selectedTotalCellsDownloadLimit As Integer
    ''' <summary>
    ''' Selection limit for total number of cells in selection when downloading to file
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedTotalCellsDownloadLimit() As Integer
        Get
            Return _selectedTotalCellsDownloadLimit
        End Get
        Set(ByVal value As Integer)
            _selectedTotalCellsDownloadLimit = value
        End Set
    End Property


    Private _limitSelectionsBy As String
    ''' <summary>
    ''' Sets if the variable selection is limited by total number of cells for the selection 
    ''' or by number of rows and cells for the selection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Expected values are "RowsColumns" or "Cells"</remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property LimitSelectionsBy() As String
        Get
            Return _limitSelectionsBy
        End Get
        Set(ByVal value As String)
            _limitSelectionsBy = value
        End Set
    End Property





    Private _selectedColumnsLimit As Integer
    ''' <summary>
    ''' Selection limits for columns.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedColumnsLimit() As Integer
        Get
            Return _selectedColumnsLimit
        End Get
        Set(ByVal value As Integer)
            _selectedColumnsLimit = value
        End Set
    End Property


    Private _selectedRowsLimit As Integer
    ''' <summary>
    ''' Selection limits for rows
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedRowsLimit() As Integer
        Get
            Return _selectedRowsLimit
        End Get
        Set(ByVal value As Integer)
            _selectedRowsLimit = value
        End Set
    End Property




    Private _showSelectedRowsColumns As Boolean
    ''' <summary>
    ''' Show/hide information about selections done.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSelectedRowsColumns() As Boolean
        Get
            Return _showSelectedRowsColumns
        End Get
        Set(ByVal value As Boolean)
            _showSelectedRowsColumns = value
        End Set
    End Property

    Private _showHierarchies As Boolean
    ''' <summary>
    ''' Show/Hide hierarchies
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowHierarchies() As Boolean
        Get
            Return _showHierarchies
        End Get
        Set(ByVal value As Boolean)
            _showHierarchies = value
        End Set
    End Property

    Private _allowAggreg As Boolean
    ''' <summary>
    ''' Are aggregations (groupings) allowed or not?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property AllowAggreg() As Boolean
        Get
            Return _allowAggreg
        End Get
        Set(ByVal value As Boolean)
            _allowAggreg = value
        End Set
    End Property

    Private _numberOfValuesInDefaultView As Integer
    ''' <summary>
    ''' Number of values to select for each variable if automatic default is applied.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property NumberOfValuesInDefaultView() As Integer
        Get
            Return _numberOfValuesInDefaultView
        End Get
        Set(ByVal value As Integer)
            _numberOfValuesInDefaultView = value
        End Set
    End Property

    'Private _viewPageUrl As String
    ' ''' <summary>
    ' ''' Pageurl for view table.
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<PropertyPersistState(PersistStateType.PerControlAndPage)> _
    'Public Property ViewPageUrl() As String
    '    Get
    '        Return _viewPageUrl
    '    End Get
    '    Set(ByVal value As String)
    '        _viewPageUrl = value
    '    End Set
    'End Property


    Private _hierarchicalSelectionLevelsOpen As Integer
    ''' <summary>
    ''' Filepath to menu xml-file.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property HierarchicalSelectionLevelsOpen() As Integer
        Get
            Return _hierarchicalSelectionLevelsOpen
        End Get
        Set(ByVal value As Integer)
            _hierarchicalSelectionLevelsOpen = value
        End Set
    End Property


    Private _totalDownload As Boolean
    ''' <summary>
    ''' If TotalDownload has been set to false only the output format "screen"
    ''' shall be exposed in the variable selector. If it is set to true the "screen"
    ''' and all other file formats shall be exposed in the variable selector.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property TotalDownload() As Boolean
        Get
            Return _totalDownload
        End Get
        Set(ByVal value As Boolean)
            _totalDownload = value
        End Set
    End Property

    Private _presentationView As String
    ''' <summary>
    ''' Presentation view
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage), _
    Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property PresentationView() As String
        Get
            Return _presentationView
        End Get
        Set(ByVal value As String)
            _presentationView = value
        End Set
    End Property

    Private _outputFormats As New List(Of String)
    ''' <summary>
    ''' File formats available as output
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage), _
    Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property OutputFormats() As List(Of String)
        Get
            Return _outputFormats
        End Get
        Set(ByVal value As List(Of String))
            _outputFormats = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets wheather the information link shall be displayed when VariableSelector is in serach-mode
    ''' </summary>
    ''' <remarks></remarks>
    Private _showSearchInformationLink As Boolean = False
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSearchInformationLink() As Boolean
        Get
            Return _showSearchInformationLink
        End Get
        Set(ByVal value As Boolean)
            _showSearchInformationLink = value
        End Set
    End Property

    ''' <summary>
    ''' Text of the Search information link when VariableSelector is in serach-mode
    ''' </summary>
    ''' <remarks></remarks>
    Private _searchInformationLinkText As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SearchInformationLinkText() As String
        Get
            Return _searchInformationLinkText
        End Get
        Set(ByVal value As String)
            _searchInformationLinkText = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the URL of the search information link when VariableSelector is in serach-mode
    ''' </summary>
    ''' <remarks></remarks>
    Private _searchInformationLinkURL As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SearchInformationLinkURL() As String
        Get
            Return _searchInformationLinkURL
        End Get
        Set(ByVal value As String)
            _searchInformationLinkURL = value
        End Set
    End Property


    ''' <summary>
    ''' If table name shall be displayed or not when VariableSelector is in search-mode
    ''' </summary>
    ''' <remarks></remarks>
    Private _showTableNameInSearch As Boolean = True
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTableNameInSearch() As Boolean
        Get
            Return _showTableNameInSearch
        End Get
        Set(ByVal value As Boolean)
            _showTableNameInSearch = value
        End Set
    End Property

    ''' <summary>
    ''' Read only property for exposing the ClientID of the search button
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SearchButtonClientID() As String
        Get
            Return Control.SearchButtonClientID
        End Get
    End Property


    Private _valuesetMustBeSelectedFirst As Boolean = False
    ''' <summary>
    ''' If the variable has one ore more valuesets only the grouping dropdown will be visisble to the user when this property is set to true.
    ''' The user have to select valueset (or aggregation if there are aggregations also) before the selection of values can start.
    ''' After the valueset (or aggregation) has been selected the listbox with values and the sort- and searchbuttons will be displayed to the 
    ''' user. 
    ''' If this property is set to false the listbox with values and the buttons are displayed directly togheter with the grouping dropdown.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ValuesetMustBeSelectedFirst() As Boolean
        Get
            Return _valuesetMustBeSelectedFirst
        End Get
        Set(ByVal value As Boolean)
            _valuesetMustBeSelectedFirst = value
        End Set
    End Property

    Private _showSelectAllAvailableValuesSearchButton As Boolean = False
    ''' <summary>
    ''' If the "Show all available values" button shall be visible or not in the "Search values" view
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Warning! Setting this property to true may result in extremely heavy web pages if the variable contains many values (more than 10000)</remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowAllAvailableValuesSearchButton() As Boolean
        Get
            Return _showSelectAllAvailableValuesSearchButton
        End Get
        Set(ByVal value As Boolean)
            _showSelectAllAvailableValuesSearchButton = value
        End Set
    End Property


    Private _reloadGroupings As Boolean = False
    Public Property ReloadGroupings() As Boolean
        Get
            Return _reloadGroupings
        End Get
        Set(ByVal value As Boolean)
            _reloadGroupings = value
        End Set
    End Property


    ''' <summary>
    ''' Defines signature of the ScreenMethod method
    ''' </summary>
    ''' <param name="presentationView"></param>
    ''' <remarks></remarks>
    Public Delegate Sub ScreenMethod(ByVal presentationView As String, ByVal model As PXModel)

    Private _screenMethod As ScreenMethod
    ''' <summary>
    ''' Declares the Screen method to use
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScreenOutputMethod() As ScreenMethod
        Get
            If _screenMethod Is Nothing Then
                _screenMethod = AddressOf DefaultScreenMethod
            End If
            Return _screenMethod
        End Get
        Set(ByVal value As ScreenMethod)
            _screenMethod = value
        End Set
    End Property

    Private _metaLinkProvider As IMetaIdProvider
    ''' <summary>
    ''' MetaLinkProvider to use
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MetaLinkProvider() As IMetaIdProvider
        Get
            Return _metaLinkProvider
        End Get
        Set(ByVal value As IMetaIdProvider )
            _metaLinkProvider = value
        End Set
    End Property



    ''' <summary>
    ''' Default implementation for output to screen
    ''' </summary>
    ''' <param name="presentationView"></param>
    ''' <remarks></remarks>
    Protected Sub DefaultScreenMethod(ByVal presentationView As String, ByVal model As PXModel)
        Dim pluginKey As String = presentationView
        Dim plugin As CommandBarPluginInfo = CommandBarPluginManager.Views(pluginKey)

        plugin.Invoke(model, Page.Controls)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

#End Region

End Class
