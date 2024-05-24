Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' Control that displays information about how many columns, rows and resulting cells is selected
''' </summary>
''' <remarks></remarks>
Partial Public Class VariableSelectorSelectionInformation
    Inherits MarkerControlBase(Of VariableSelectorSelectionInformationCodebehind, VariableSelectorSelectionInformation)


#Region "Properties"

    Private _showSelectionLimits As Boolean = False
    ''' <summary>
    ''' 
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
    ''' Selection limit for total number of cells in selection
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


    Private _showSelectionsMade As Boolean = False
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSelectionsMade() As Boolean
        Get
            Return _showSelectionsMade
        End Get
        Set(ByVal value As Boolean)
            _showSelectionsMade = value
        End Set
    End Property



#End Region

End Class
