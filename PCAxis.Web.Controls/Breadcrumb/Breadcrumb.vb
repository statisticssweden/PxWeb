Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums

''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Breadcrumb
    Inherits MarkerControlBase(Of BreadcrumbCodebehind, Breadcrumb)

    ''' <summary>
    ''' Delegate function for retrieving the menu
    ''' </summary>
    ''' <param name="nodeId">Id of node in the menu</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetMenuDelegate(ByVal nodeId As String) As PCAxis.Menu.Item

    Public Enum BreadcrumbMode
        Home
        Menu
        MenuSubPage
        Selection
        SelectionSubPage
        Presentation
    End Enum

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

    Private _homePage As String
    ''' <summary>
    ''' The home page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property HomePage() As String
        Get
            Return _homePage
        End Get
        Set(ByVal value As String)
            _homePage = value
        End Set
    End Property


    Private _homePageIsExternal As Boolean
    ''' <summary>
    ''' If the home page is external or not (located outside of the PX environment)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property HomePageIsExternal() As Boolean
        Get
            Return _homePageIsExternal
        End Get
        Set(ByVal value As Boolean)
            _homePageIsExternal = value
        End Set
    End Property


    Private _homePageName As String
    ''' <summary>
    ''' The home page name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property HomePageName() As String
        Get
            Return _homePageName
        End Get
        Set(ByVal value As String)
            _homePageName = value
        End Set
    End Property

    Private _homePageImage As Boolean
    ''' <summary>
    ''' If a image is used for the homepage link or not. If set to false a text link will be displayed instead.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property HomePageImage() As Boolean
        Get
            Return _homePageImage
        End Get
        Set(ByVal value As Boolean)
            _homePageImage = value
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

    Private _useTableList As Boolean
    ''' <summary>
    ''' If TableList webcontrol is used to display tables in the menu
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property UseTableList() As Boolean
        Get
            Return _useTableList
        End Get
        Set(ByVal value As Boolean)
            _useTableList = value
        End Set
    End Property


    Public Sub Update(ByVal mode As Breadcrumb.BreadcrumbMode, Optional ByVal subpage As String = "")
        Control.Update(mode, subpage)
    End Sub

End Class
