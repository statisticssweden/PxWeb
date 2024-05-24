Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums

''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class NavigationFlow
    Inherits MarkerControlBase(Of NavigationFlowCodebehind, NavigationFlow)

    ''' <summary>
    ''' Delegate function for retrieving the menu
    ''' </summary>
    ''' <param name="nodeId">Id of node in the menu</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetMenuDelegate(ByVal nodeId As String) As PCAxis.Menu.PxMenuBase
#Region "Enum"
    Public Enum NavigationFlowMode
        First
        Second
        Third
    End Enum
#End Region

#Region "Properties"

    Private _showNavigationFlow As Boolean

    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ShowNavigationFlow() As Boolean
        Get
            Return _showNavigationFlow
        End Get
        Set(ByVal value As Boolean)
            _showNavigationFlow = value
        End Set
    End Property
    Private _getMenu As GetMenuDelegate
    ''' <summary>
    ''' Function to retrieve the menu
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GetMenu() As GetMenuDelegate
        Get
            Return _getMenu
        End Get
        Set(ByVal value As GetMenuDelegate)
            _getMenu = value
        End Set
    End Property

    Private _databaseId As String
    ''' <summary>
    ''' Identifies the current database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property DatabaseId() As String
        Get
            Return _databaseId
        End Get
        Set(ByVal value As String)
            _databaseId = value
        End Set
    End Property

    Private _databaseName As String
    ''' <summary>
    ''' Name of the current database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property DatabaseName() As String
        Get
            Return _databaseName
        End Get
        Set(ByVal value As String)
            _databaseName = value
        End Set
    End Property

    Private _tablePath As String
    ''' <summary>
    ''' Path to the current table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property TablePath() As String
        Get
            Return _tablePath
        End Get
        Set(ByVal value As String)
            _tablePath = value
        End Set
    End Property

    Private _table As String
    ''' <summary>
    ''' The current table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property Table() As String
        Get
            Return _table
        End Get
        Set(ByVal value As String)
            _table = value
        End Set
    End Property

    Private _menuPage As String
    ''' <summary>
    ''' The menu page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property MenuPage() As String
        Get
            Return _menuPage
        End Get
        Set(ByVal value As String)
            _menuPage = value
        End Set
    End Property

    Private _selectionPage As String
    ''' <summary>
    ''' The selection page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property SelectionPage() As String
        Get
            Return _selectionPage
        End Get
        Set(ByVal value As String)
            _selectionPage = value
        End Set
    End Property

    Private _tablePathParam As String
    ''' <summary>
    ''' Querystring parameter for table path
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property TablePathParam() As String
        Get
            Return _tablePathParam
        End Get
        Set(ByVal value As String)
            _tablePathParam = value
        End Set
    End Property

    Private _layoutParam As String
    ''' <summary>
    ''' Querystring parameter for layout
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property LayoutParam() As String
        Get
            Return _layoutParam
        End Get
        Set(ByVal value As String)
            _layoutParam = value
        End Set
    End Property

    Private _dbType As PCAxis.Web.Core.Enums.DatabaseType = DatabaseType.PX
    ''' <summary>
    ''' Type of database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DatabaseType() As PCAxis.Web.Core.Enums.DatabaseType
        Get
            Return _dbType
        End Get
        Set(ByVal value As PCAxis.Web.Core.Enums.DatabaseType)
            _dbType = value
        End Set
    End Property
#End Region
    Public Sub UpdateNavigationFlowMode(ByVal mode As NavigationFlow.NavigationFlowMode)
        Control.UpdateNavigationFlowMode(mode)
    End Sub
End Class
