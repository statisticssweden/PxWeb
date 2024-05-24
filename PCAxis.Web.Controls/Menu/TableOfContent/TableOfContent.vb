Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums


''' <summary>
''' Table of content
''' </summary>
''' <remarks></remarks>
Public Partial Class TableOfContent
    Inherits MarkerControlBase(Of TableOfContentCodebehind, TableOfContent)

    ''' <summary>
    ''' Delegate function for retrieving the menu
    ''' </summary>
    ''' <param name="nodeId">Id of node in the menu</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetMenuDelegate(ByVal nodeId As String) As PCAxis.Menu.Item

#Region " Properties "

    'Private _menu As PxMenuBase
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

    'Private _downloadPageURL As String
    'Public Property DownloadPageURL() As String
    '    Get
    '        Return _downloadPageURL
    '    End Get
    '    Set(ByVal value As String)
    '        _downloadPageURL = value
    '    End Set
    'End Property


    Private _startNode As String
    ''' <summary>
    ''' Start rendering the tree from this node. Will become the root node of the tree. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StartNode() As String
        Get
            Return _startNode
        End Get
        Set(ByVal value As String)
            _startNode = value
        End Set
    End Property


    Private _expandNode As String
    ''' <summary>
    ''' Display the three with this node expanded. This setting is only valid when table of content is 
    ''' displayed as a tree.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Levels in the tree are separated with the __ characters (double underscore).
    ''' 
    ''' Example 1:
    ''' If the value of ExpandNode is BE__BE0101 it is expected that a tree node with the value
    ''' BE exists directly under the root node. It is also expected that the BE tree node has a 
    ''' childnode with the value BE0101. This method will expand the BE and the BE0101 tree nodes.
    ''' 
    ''' Example 2:
    ''' If the StartNode setting is set to BE the ExpandNode setting shall be set to BE0101 to
    ''' expand the same node as in example 1.
    ''' </remarks>
    Public Property ExpandNode() As String
        Get
            Return _expandNode
        End Get
        Set(ByVal value As String)
            _expandNode = value
        End Set
    End Property


    Private _urlLinkMode As UrlLinkModeType
    ''' <summary>
    ''' Specifies how URL links will be displayed when Table of content is displayed as a tree
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property UrlLinkMode() As UrlLinkModeType
        Get
            Return _urlLinkMode
        End Get
        Set(ByVal value As UrlLinkModeType)
            _urlLinkMode = value
        End Set
    End Property


    Private _defaultPageURL As String
    ''' <summary>
    ''' Gets or Sets the URL to the Default page
    ''' </summary>
    ''' <value>URL to the default page. E.g. 'Default.aspx'</value>        
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
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
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
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
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectPageURL() As String
        Get
            Return _selectPageURL
        End Get
        Set(ByVal value As String)
            _selectPageURL = value
        End Set
    End Property


    Private _metaIconModifiedImagePath As String = ""
    ''' <summary>
    ''' Name of the image to use as an icon for showing information about when the table was last modified
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetaIconModifiedImagePath() As String
        Get
            Return _metaIconModifiedImagePath
        End Get
        Set(ByVal value As String)
            _metaIconModifiedImagePath = value
        End Set
    End Property

    Private _metaIconUpdatedImagePath As String = ""
    ''' <summary>
    ''' Name of the image to use as an icon for showing information about when the table was last updated
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetaIconUpdatedImagePath() As String
        Get
            Return _metaIconUpdatedImagePath
        End Get
        Set(ByVal value As String)
            _metaIconUpdatedImagePath = value
        End Set
    End Property

    Private _metaIconUpdatedAfterPublishImagePath As String = ""
    ''' <summary>
    ''' Name of the image to use as an icon for showing information about that the table was updated after it was published
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetaIconUpdatedAfterPublishImagePath() As String
        Get
            Return _metaIconUpdatedAfterPublishImagePath
        End Get
        Set(ByVal value As String)
            _metaIconUpdatedAfterPublishImagePath = value
        End Set
    End Property

    Private _metaIconSizeImagePath As String = ""
    ''' <summary>
    ''' Name of the image to use as an icon for showing information about the table file size
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetaIconSizeImagePath() As String
        Get
            Return _metaIconSizeImagePath
        End Get
        Set(ByVal value As String)
            _metaIconSizeImagePath = value
        End Set
    End Property

    Private _showLastUpdated As Boolean
    ''' <summary>
    ''' If information about when the file was last updated (when PX-files are included in Table of content)
    ''' shall be shown or not. Last updated is the last time the file was written to and is extracted from the file information
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowLastUpdated() As Boolean
        Get
            Return _showLastUpdated
        End Get
        Set(ByVal value As Boolean)
            _showLastUpdated = value
        End Set
    End Property

    Private _showModified As Boolean
    ''' <summary>
    ''' If information about when the file was modified (when PX-files are included in Table of content)
    ''' shall be shown or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowModified() As Boolean
        Get
            Return _showModified
        End Get
        Set(ByVal value As Boolean)
            _showModified = value
        End Set
    End Property

    Private _showFileSize As Boolean
    ''' <summary>
    ''' If information about the file size (when PX-files are included in Table of content)
    ''' shall be shown or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowFileSize() As Boolean
        Get
            Return _showFileSize
        End Get
        Set(ByVal value As Boolean)
            _showFileSize = value
        End Set
    End Property

    Private _showTableCategory As Boolean
    ''' <summary>
    ''' If table category shall be displayed or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTableCategory() As Boolean
        Get
            Return _showTableCategory
        End Get
        Set(ByVal value As Boolean)
            _showTableCategory = value
        End Set
    End Property

    Private _showTableUpdatedAfterPublish As Boolean
    ''' <summary>
    ''' If table that have been updated after they was published shall be marked or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTableUpdatedAfterPublish() As Boolean
        Get
            Return _showTableUpdatedAfterPublish
        End Get
        Set(ByVal value As Boolean)
            _showTableUpdatedAfterPublish = value
        End Set
    End Property

    Private _metadataAsIcons As Boolean
    ''' <summary>
    ''' If table metadata shall be displayed as icons or as text
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MetadataAsIcons() As Boolean
        Get
            Return _metadataAsIcons
        End Get
        Set(ByVal value As Boolean)
            _metadataAsIcons = value
        End Set
    End Property

    Private _showTextForMetadata As Boolean
    ''' <summary>
    ''' If text shall be displayd next to the table metadata 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTextForMetadata() As Boolean
        Get
            Return _showTextForMetadata
        End Get
        Set(ByVal value As Boolean)
            _showTextForMetadata = value
        End Set
    End Property



    Private _showtreeviewmenu As Boolean
    ''' <summary>
    ''' If Table of content shall be displayed as a treeview or as a list 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTreeViewMenu() As Boolean
        Get
            Return _showtreeviewmenu
        End Get
        Set(ByVal value As Boolean)
            _showtreeviewmenu = value
        End Set
    End Property

    Private _sortbyalias As Boolean
    ''' <summary>
    ''' If the nodes within the Table of content web control shall be sorted by their alias names or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SortByAlias() As Boolean
        Get
            Return _sortbyalias
        End Get
        Set(ByVal value As Boolean)
            _sortbyalias = value
        End Set
    End Property

    Private _expandallnodes As Boolean
    ''' <summary>
    ''' If the treeview shall be totally expanded or not. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ExpandAllNodes() As Boolean
        Get
            Return _expandallnodes
        End Get
        Set(ByVal value As Boolean)
            _expandallnodes = value
        End Set
    End Property

    Private _showrootname As Boolean
    ''' <summary>
    ''' If the name of the root node shall be displayed or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowRootName() As Boolean
        Get
            Return _showrootname
        End Get
        Set(ByVal value As Boolean)
            _showrootname = value
        End Set
    End Property

    Private _includepxfile As Boolean
    ''' <summary>
    ''' If Table of content is displayed as a treeview (ShowTreeViewMenu = true) this setting 
    ''' controls if the data tables shall be displayed within the treeview or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property IncludePXFilesInTreeView() As Boolean
        Get
            Return _includepxfile
        End Get
        Set(ByVal value As Boolean)
            _includepxfile = value
        End Set
    End Property


    Private _showJavascriptDisabledButton As Boolean = True
    ''' <summary>
    ''' Controls if button that loads the whole tree shall be displayed when javascript is disabled 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowJavascriptDisabledButton() As Boolean
        Get
            Return _showJavascriptDisabledButton
        End Get
        Set(ByVal value As Boolean)
            _showJavascriptDisabledButton = value
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
