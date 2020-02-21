Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class VariableSelectorValueSelect
    Inherits MarkerControlBase(Of VariableSelectorValueSelectCodebehind, VariableSelectorValueSelect)



    Public Event SelectHierarchicalButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal hierarchicalVariable As Paxiom.Variable)
    Friend Sub OnSelectHierarchicalButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal hierarchicalVariable As Paxiom.Variable)
        RaiseEvent SelectHierarchicalButtonClicked(Me, e, hierarchicalVariable)
    End Sub

    Public Event SearchLargeNumberOfValuesButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal Variable As Paxiom.Variable)
    Friend Sub OnSearchLargeNumberOfValuesButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal Variable As Paxiom.Variable)
        RaiseEvent SearchLargeNumberOfValuesButtonClicked(Me, e, Variable)
    End Sub

    Public Event SelectFromGroupButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Paxiom.Variable)
    Friend Sub OnSelectFromGroupButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Paxiom.Variable)
        RaiseEvent SelectFromGroupButtonClicked(Me, e, variable)
    End Sub

    Public Event MetadataInformationButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Paxiom.Variable)
    Friend Sub OnMetadataInformationButtonClicked(ByVal sender As Object, ByVal e As EventArgs, ByVal variable As Paxiom.Variable)
        RaiseEvent MetadataInformationButtonClicked(Me, e, variable)
    End Sub

    Private _variable As Paxiom.Variable
    Private _variablename As String
    Private _selection As Paxiom.Selection

    Private _searchButtonMode As VariableSelectorSearchButtonViewMode
    Private _maxRowsWithoutSearch As Integer
    Private _alwaysShowTimeVariableWithoutSearch As Boolean
    Private _listSize As Integer
    Private _showElimMark As Boolean
    Private _showHierarchies As Boolean

    Private _showAllValues As Boolean = False
    Private _valuesSortDirection As SortDirection
    Private _numberOfValuesInDefaultView As Integer
    Private _showGroupingDropDown As Boolean = True
    Private _allowAggreg As Boolean = True
    Private _selectedGrouping As String = ""
    Private _selectedGroupingPresentation As GroupingIncludesType
    Private _selectionFromGroup As Boolean = False
    Private _metadataInformation As Boolean = False

#Region "Event"

#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or sets the variable this controll should present
    ''' </summary>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property Selection() As Paxiom.Selection
        Get
            Return Control.GetSelection()
        End Get
        Set(ByVal value As Paxiom.Selection)
            _selection = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the selected grouping valueset title
    ''' </summary>
    ''' <remarks></remarks>
    Private _variableTitleSecond As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property VariableTitleSecond() As String
        Get
            Return _variableTitleSecond
        End Get
        Set(ByVal value As String)
            _variableTitleSecond = value
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


    ''' <summary>
    ''' Gets or sets the variable this controll should present
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Variable() As Paxiom.Variable
        Get
            Return _variable
        End Get
        Set(ByVal value As Paxiom.Variable)
            _variable = value

        End Set

    End Property

    ''' <summary>
    ''' If set to false the variable name is displayed instead of dropdown with groupings, as if variable had no groupings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ShowGroupingDropDown() As Boolean
        Get
            Return _showGroupingDropDown
        End Get
        Set(ByVal value As Boolean)
            _showGroupingDropDown = value
        End Set
    End Property

    ''' <summary>
    ''' If aggregations (groupings are allowed or not)
    ''' </summary>
    ''' <remarks></remarks>
    Public Property AllowAggreg() As Boolean
        Get
            Return _allowAggreg
        End Get
        Set(ByVal value As Boolean)
            _allowAggreg = value
        End Set
    End Property

    ''' <summary>
    ''' Name of the variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property VariableName() As String
        Get
            Return _variablename
        End Get
        Set(ByVal value As String)
            _variablename = value

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
    ''' Select first content and time value
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


    ''' <summary>
    ''' Controls when the meta data button shall be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetadataInformationButtonMode() As Boolean
        Get
            Return _metadataInformation
        End Get
        Set(ByVal value As Boolean)
            _metadataInformation = value
        End Set
    End Property

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

    ''' <summary>
    ''' Show image indicating that eliminiation of variable selection not is valid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowAllValues() As Boolean
        Get
            Return _showAllValues
        End Get
        Set(ByVal value As Boolean)
            _showAllValues = value
        End Set
    End Property

    ''' <summary>
    ''' Number of visible items in the listboxes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Show image indicating that eliminiation of variable selection not is valid.
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

    ''' <summary>
    ''' Show/hide imagebutton for hierarchies
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

    ''' <summary>
    ''' Sets or gets sort order of the values listbox.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ValuesSortDirection() As SortDirection
        Get
            Return _valuesSortDirection
        End Get
        Set(ByVal value As SortDirection)
            _valuesSortDirection = value
        End Set
    End Property

    Private _isSortDirectionSet As Boolean = False
    ''' <summary>
    ''' Sets or gets sort order of the values listbox.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property IsSortDirectionSet() As Boolean
        Get
            Return _isSortDirectionSet
        End Get
        Set(ByVal value As Boolean)
            _isSortDirectionSet = value
        End Set
    End Property

    ''' <summary>
    ''' Show number of values selected in short or descriptive form. 0 = Descriptive form with labels, 1 = Short form without labels.
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

    Friend ReadOnly Property EliminationSelectionsDone() As Boolean
        Get
            If Not Variable.Elimination And VariableSelector.SelectedVariableValues(Variable.Code).ValueCodes.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private _hideTitlesForValueSelectionStatistics As Boolean
    ''' <summary>
    ''' Show number of values selected in short or descriptive form. 0 = Descriptive form with labels, 1 = Short form without labels.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property HideTitlesForValueSelectionStatistics() As Boolean
        Get
            Return _hideTitlesForValueSelectionStatistics
        End Get
        Set(ByVal value As Boolean)
            _hideTitlesForValueSelectionStatistics = value
        End Set
    End Property


    Private _valuesetMustBeSelectedFirst As Boolean
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

    ''' <summary>
    ''' If a grouping/valueset has been applied to the variable this property holds the code of the grouping/valueset
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedGrouping() As String
        Get
            Return _selectedGrouping
        End Get
        Set(ByVal value As String)
            _selectedGrouping = value
        End Set
    End Property

    ''' <summary>
    ''' If a grouping is selected this property holds how the grouping shall be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedGroupingPresentation() As GroupingIncludesType
        Get
            Return _selectedGroupingPresentation
        End Get
        Set(ByVal value As GroupingIncludesType)
            _selectedGroupingPresentation = value
        End Set
    End Property

#End Region

    Public Sub RenderSelection()
        If HasManyValues() AndAlso _
           VariableSelector.SelectedVariableValues.ContainsKey(Variable.Code) AndAlso _
           VariableSelector.SelectedVariableValues(Variable.Code).ValueCodes.Count > 0 Then
            Control.UpdateDisplayModeUI()
        Else
            Control.RenderSelection()
        End If
    End Sub

    Public Sub SelectDefaultValues()
        Control.SelectDefaultValues()
    End Sub

    Public Sub SelectAllValues()
        Control.SelectAllValues()
    End Sub

    ''' <summary>
    ''' Select the values specified by the values parameter
    ''' </summary>
    ''' <param name="values">Values collection</param>
    ''' <remarks></remarks>
    Public Sub SelectValues(ByVal values As PCAxis.Paxiom.Values)
        Control.SelectValues(values)
    End Sub

    Public Sub DeselectAllValues()
        Control.DeselectAllValues()
    End Sub

    ''' <summary>
    ''' Tells if the variable has many values or not.
    ''' </summary>
    ''' <returns>
    ''' Returns true if the variable has more values than the configuration setting for many values,
    ''' else false.
    ''' </returns>
    ''' <remarks></remarks>
    Friend Function HasManyValues() As Boolean
        Return Variable.Values.Count > MaxRowsWithoutSearch
    End Function

    Friend Function ApplyGrouping(ByVal code As String, Optional ByVal clearSelection As Boolean = True, Optional ByVal include As Nullable(Of GroupingIncludesType) = Nothing) As Boolean
        Return Control.ApplyGrouping(code, clearSelection, include)
    End Function

    Friend Sub ChangeSelectedGrouping(ByVal grouping As String, ByVal include As GroupingIncludesType)
        Control.ChangeSelectedGrouping(grouping, include)
    End Sub
End Class
