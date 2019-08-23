Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums


''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class TableQuery
    Inherits MarkerControlBase(Of TableQueryCodebehind, TableQuery)


    Private _urlRoot As String
    ''' <summary>
    ''' The URL root to the API service. If this property is set it will be used in the URL displayed in the web control.
    ''' Example: The value of URLRoot could be http://www.myApi.com 
    ''' 
    ''' If this property is not set the URL root of the current http context will be used instead. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property URLRoot() As String
        Get
            Return _urlRoot
        End Get
        Set(ByVal value As String)
            _urlRoot = value
        End Set
    End Property

    Private _showRoutePrefix As Boolean = True
    ''' <summary>
    ''' If the route prefix should be displayed in the API url
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ShowRoutePrefix() As Boolean
        Get
            Return _showRoutePrefix
        End Get
        Set(ByVal value As Boolean)
            _showRoutePrefix = value
        End Set
    End Property

    Private _routePrefix As String
    ''' <summary>
    ''' Route prefix
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property RoutePrefix() As String
        Get
            Return _routePrefix
        End Get
        Set(ByVal value As String)
            _routePrefix = value
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

    Private _db As String
    ''' <summary>
    ''' Identifies the current database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property Database() As String
        Get
            Return _db
        End Get
        Set(ByVal value As String)
            _db = value
        End Set
    End Property

    Private _path As String
    ''' <summary>
    ''' Path to the current table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property Path() As String
        Get
            Return _path
        End Get
        Set(ByVal value As String)
            _path = value
        End Set
    End Property

    Private _table As String
    ''' <summary>
    ''' Identifies the current table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' A table is identified as database/path/table
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


    Private _moreInfoUrl As String
    ''' <summary>
    ''' URL to additional information about accessing the table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property MoreInfoURL() As String
        Get
            Return _moreInfoUrl
        End Get
        Set(ByVal value As String)
            _moreInfoUrl = value
        End Set
    End Property


    Private _moreInfoIsExternalPage As Boolean
    ''' <summary>
    ''' If "More iformation" shall be opened in a new window or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property MoreInfoIsExternalPage() As Boolean
        Get
            Return _moreInfoIsExternalPage
        End Get
        Set(ByVal value As Boolean)
            _moreInfoIsExternalPage = value
        End Set
    End Property


End Class
