Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Menu
Imports PCAxis.Web.Core.Enums


''' <summary>
''' Component that displays information about the .px-files identified by the CurrentItem
''' in the Menu object property
''' 
''' The Menu object is usually set in Default.aspx Load event before TableList Load event is triggered like this:
''' </summary>
''' <remarks></remarks>
Public Partial Class TableList
    Inherits MarkerControlBase(Of TableListCodebehind, TableList)

    ''' <summary>
    ''' Delegate function for retrieving the menu
    ''' </summary>
    ''' <param name="nodeId">Id of node in the menu</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetMenuDelegate(ByVal nodeId As String) As PCAxis.Menu.Item


#Region " Properties "

    'Private _menu As PxMenuBase
    '''' <summary>
    '''' Gets or Sets the Menu. The menu is defined in an xml file.
    '''' E.g. initialization:
    '''' TableList.Menu = new XMLMenu(Server.MapPath("~/resources/Menu/menu.xml"), "sv")
    '''' </summary>
    '''' <value>The Menu object</value>    
    '''' <remarks></remarks>
    'Public Property Menu() As PxMenuBase
    '    Get
    '        Return _menu
    '    End Get
    '    Set(ByVal value As PxMenuBase)
    '        _menu = value
    '    End Set
    'End Property

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

    Private _menuStartNode As Item
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MenuStartNode() As Item
        Get
            Return _menuStartNode
        End Get
        Set(ByVal value As Item)
            _menuStartNode = value
        End Set
    End Property

    Private _defaultPageURL As String
    ''' <summary>
    ''' Gets or Sets the URL to the Default page
    ''' </summary>
    ''' <value>URL to the default page. E.g. 'Default.aspx'</value>        
    ''' <remarks></remarks>
    Public Property DefaultPageURL() As String
        Get
            Return _defaultPageURL
        End Get
        Set(ByVal value As String)
            _defaultPageURL = value
        End Set
    End Property

    Private _menuPageURL As String
    ''' <summary>
    ''' Gets or Sets the URL to the Menu page
    ''' </summary>
    ''' <value>URL to the Menu page. E.g. 'Menu.aspx'</value>    
    ''' <remarks></remarks>
    Public Property MenuPageURL() As String
        Get
            Return _menuPageURL
        End Get
        Set(ByVal value As String)
            _menuPageURL = value
        End Set
    End Property

    Private _selectPageURL As String
    ''' <summary>
    ''' Gets or Sets the URL to the Select page
    ''' </summary>
    ''' <value>URL to the select page. E.g. 'Select.aspx'</value>    
    ''' <remarks></remarks>
    Public Property SelectPageURL() As String
        Get
            Return _selectPageURL
        End Get
        Set(ByVal value As String)
            _selectPageURL = value
        End Set
    End Property

    Private _viewPageURL As String
    ''' <summary>
    ''' Gets or Sets the URL to the View page
    ''' </summary>
    ''' <value>URL to the View page. E.g. 'view.aspx'</value>    
    ''' <remarks></remarks>
    Public Property ViewPageURL() As String
        Get
            Return _viewPageURL
        End Get
        Set(ByVal value As String)
            _viewPageURL = value
        End Set
    End Property


    Private _showAsListOfContent As Boolean
    ''' <summary>
    ''' Gets or sets wheter the row with links shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowAsListOfContent() As Boolean
        Get
            Return _showAsListOfContent
        End Get
        Set(ByVal value As Boolean)
            _showAsListOfContent = value
        End Set
    End Property

    Private _selectOption_Select As Boolean
    ''' <summary>
    ''' Gets or sets whether the select link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_Select() As Boolean
        Get
            Return _selectOption_Select
        End Get
        Set(ByVal value As Boolean)
            _selectOption_Select = value
        End Set
    End Property

    Private _selectOption_Download As DownloadLinkVisibilityType = DownloadLinkVisibilityType.ShowIfSmallFile

    ''' <summary>
    ''' Gets or sets whether the download link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_Download() As DownloadLinkVisibilityType
        Get
            Return _selectOption_Download
        End Get
        Set(ByVal value As DownloadLinkVisibilityType)
            _selectOption_Download = value
        End Set
    End Property


    Private _selectOption_View As Boolean
    ''' <summary>
    ''' Gets or sets whether the View link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_View() As Boolean
        Get
            Return _selectOption_View
        End Get
        Set(ByVal value As Boolean)
            _selectOption_View = value
        End Set
    End Property

    Private _selectOption_ViewDefaultValues As Boolean
    ''' <summary>
    ''' Gets or sets whether the "View with default values" link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_ViewDefaultValues() As Boolean
        Get
            Return _selectOption_ViewDefaultValues
        End Get
        Set(ByVal value As Boolean)
            _selectOption_ViewDefaultValues = value
        End Set
    End Property

    Private _selectOption_ViewDefaultValuesWithCommandbar As Boolean
    ''' <summary>
    ''' Gets or sets whether the "View with default values and command bar" link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_ViewDefaultValuesWithCommandbar() As Boolean
        Get
            Return _selectOption_ViewDefaultValuesWithCommandbar
        End Get
        Set(ByVal value As Boolean)
            _selectOption_ViewDefaultValuesWithCommandbar = value
        End Set
    End Property

    Private _selectOption_ViewWithCommandbar As Boolean
    ''' <summary>
    ''' Gets or sets whether the "View with command bar" link shall be visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectOption_ViewWithCommandbar() As Boolean
        Get
            Return _selectOption_ViewWithCommandbar
        End Get
        Set(ByVal value As Boolean)
            _selectOption_ViewWithCommandbar = value
        End Set
    End Property

    Private _smallFileSizeMode As SmallFileSizeModeType = SmallFileSizeModeType.FileSize
    ''' <summary>
    ''' Specifies how it is determined if a file is small or not (by the filesize in bytes or by the number of cells)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SmallFileSizeMode() As SmallFileSizeModeType
        Get
            Return _smallFileSizeMode
        End Get
        Set(ByVal value As SmallFileSizeModeType)
            _smallFileSizeMode = value
        End Set
    End Property

    Private _maxFileziseForSmallFile As Integer
    '''<summary>
    ''' Gets or Sets the file size limit of small files.
    ''' </summary>    
    ''' <value>File size limit</value>
    ''' <remarks>
    ''' When SmallFileSizeMode = FileSize this limit is given in bytes.
    ''' When SmallFileSizeMode = NumberOfCells this limit is given in number of cells.
    ''' </remarks>
    Public Property MaxFileziseForSmallFile() As Integer
        Get
            Return _maxFileziseForSmallFile
        End Get
        Set(ByVal value As Integer)
            _maxFileziseForSmallFile = value
        End Set
    End Property

    Private _openSelectInNewWindow As Boolean
    ''' <summary>
    ''' Gets or Sets if the Select page will open in a new window
    ''' Initialized from ControlSettings.xml
    ''' </summary>
    ''' <value>True=>'LinkTarget="_blank", False=>LinkTarget="_top"</value>    
    ''' <remarks></remarks>
    Public Property OpenSelectInNewWindow() As Boolean
        Get
            Return _openSelectInNewWindow
        End Get
        Set(ByVal value As Boolean)
            _openSelectInNewWindow = value
        End Set
    End Property


    Private _linkTarget As String
    ''' <summary>
    ''' Gets or Sets if the Select page will open in a new window
    ''' Initialized by the value in the property OpenSelectInNewWindow
    ''' </summary>
    ''' <value>_blank=New window, _top=Same window</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LinkTarget() As String
        Get
            Return _linkTarget
        End Get
        Set(ByVal value As String)
            _linkTarget = value
        End Set
    End Property

    Private _showModifiedDate As Boolean
    ''' <summary>
    ''' Gets or sets whether the modified date shall be visible.
    ''' </summary>    
    Public Property ShowModifiedDate() As Boolean
        Get
            Return _showModifiedDate
        End Get
        Set(ByVal value As Boolean)
            _showModifiedDate = value
        End Set
    End Property

    Private _showLastUpdated As Boolean
    ''' <summary>
    ''' Gets or sets whether last updated information shall be visible
    ''' </summary>
    Public Property ShowLastUpdated() As Boolean
        Get
            Return _showLastUpdated
        End Get
        Set(ByVal value As Boolean)
            _showLastUpdated = value
        End Set
    End Property

    Private _showFileSize As Boolean
    ''' <summary>
    ''' Gets or sets whether the file size shall be visible.
    ''' </summary>
    Public Property ShowFileSize() As Boolean
        Get
            Return _showFileSize
        End Get
        Set(ByVal value As Boolean)
            _showFileSize = value
        End Set
    End Property

    Private _showVariablesAndValues As Boolean
    ''' <summary>
    ''' Gets or sets if the variables shall be visible
    ''' </summary>
    ''' <value>True=Show variables, False=Hide variables</value>    
    Public Property ShowVariablesAndValues() As Boolean
        Get
            Return _showVariablesAndValues
        End Get
        Set(ByVal value As Boolean)
            _showVariablesAndValues = value
        End Set
    End Property

    ''' <summary>
    ''' Decides if the download link could be visible or not.
    ''' </summary>    
    Public ReadOnly Property ShowDownloadLink() As Boolean
        Get
            If SelectOption_Download = DownloadLinkVisibilityType.AlwaysHide Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    ''' <summary>
    ''' Realative path to PX-database directory
    ''' </summary>
    ''' <remarks></remarks>
    Private mDatabasePath As String
    Public Property DatabasePath() As String
        Get
            Return mDatabasePath
        End Get
        Set(ByVal value As String)
            mDatabasePath = value
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

End Class
