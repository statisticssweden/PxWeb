Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Menu
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Core.Management.LinkManager
''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class TableOfContentCodebehind
    Inherits PaxiomControlBase(Of TableOfContentCodebehind, TableOfContent)

#Region "Private fields"
    Private _menu As Item
    Private _pathHandler As PathHandler
#End Region

#Region " Controls "

    Protected TableOfContentPanel As Panel
    Protected ErrorLabel As Label
    Protected MenuNavigationTree As TreeView
    Protected MenuNavigationList As Label
    Protected WithEvents MenuItemList As Repeater
    Protected WithEvents LinkItemList As Repeater
    Protected MenuTreeviewPanel As Panel
    Protected WithEvents ActionButton As Button

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadScripts()

        _pathHandler = PathHandlerFactory.Create(Marker.DatabaseType)

        If Not Page.IsPostBack Then
            If Marker.ShowTreeViewMenu Then
                RenderTree()
                If Not Marker.ShowJavascriptDisabledButton Then
                    ActionButton.Visible = False
                End If
            Else
                'Show TableOfContent in List-mode
                MenuTreeviewPanel.Visible = False
                ActionButton.Visible = False 'List view does not need javascript
            End If

            SetLocalizedText()
        End If

        'Show TableList
        If (QuerystringManager.GetQuerystringParameter("tablelist") = "true") Then
            TableOfContentPanel.Visible = False
        Else
            If (Not Marker.ShowTreeViewMenu) Then
                RenderMenuList()
            End If
        End If

    End Sub

    ''' <summary>
    ''' Render the startup TableOfContent tree.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RenderTree()

        _menu = Marker.GetMenu(Marker.StartNode)

        If Not _menu Is Nothing Then
            If (Marker.ShowRootName) Then
                Dim node As New TreeNode()
                node.SelectAction = TreeNodeSelectAction.Expand
                node.PopulateOnDemand = True
                node.Text = _menu.Text
                node.Value = GetRootNodeValue()
                If IsTableListLink(_menu) Then
                    SetTableListLink(_menu, node)
                End If
                If Marker.UrlLinkMode = UrlLinkModeType.Image Then
                    If TypeOf _menu Is PxMenuItem Then
                        AddImageLinks(CType(_menu, PxMenuItem), node)
                    End If
                End If

                MenuNavigationTree.Nodes.Add(node)
            Else
                If IsTableListLink(_menu) Then
                    Dim node As New TreeNode(_menu.Text)
                    SetTableListLink(_menu, node)
                    MenuNavigationTree.Nodes.Add(node)
                Else
                    MenuNavigationTree.ExpandDepth = 0
                    If TypeOf _menu Is PxMenuItem Then
                        For Each Item As Item In CType(_menu, PxMenuItem).SubItems
                            AddTreeViewNode(Item, Nothing)
                        Next
                    End If
                End If
            End If

            ExpandNodes()
        End If


        If (Marker.ExpandAllNodes) Then
            MenuNavigationTree.ExpandDepth = -1
            MenuNavigationTree.ExpandAll()
        End If
    End Sub

    ''' <summary>
    ''' Returns the value of the root node
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRootNodeValue() As String
        Dim rootValue As String = ""

        If (Marker.ShowRootName) Then
            If Not String.IsNullOrEmpty(Marker.StartNode) Then
                rootValue = Marker.StartNode
            Else
                'rootValue = _menu.RootItem.ID.AsString()
                rootValue = _pathHandler.Combine("", _menu.ID)
            End If
        End If

        Return rootValue
    End Function

    ' ''' <summary>
    ' ''' Expands the tree according to the value of the ExpandNode property.
    ' ''' Levels in the tree are separated with the / character.
    ' ''' 
    ' ''' Example:
    ' ''' If the value of ExpandNode is BE__BE0101 it is expected that a tree node with the value
    ' ''' BE exists directly under the root node. It is also expected that the BE tree node has a 
    ' ''' childnode with the value BE0101. This method will expand the BE and the BE0101 tree nodes.
    ' ''' </summary>
    ' ''' <remarks></remarks>
    Private Sub ExpandNodes()
        Dim separator() As String = {PathHandler.NODE_DIVIDER}
        Dim foundNode As TreeNode
        Dim parentNode As TreeNode = Nothing
        Dim nodePath As New System.Text.StringBuilder
        Dim nodes() As String
        Dim index As Integer = 0
        Dim first As Boolean = True

        If Not String.IsNullOrEmpty(Marker.ExpandNode) Then

            nodes = Marker.ExpandNode.Split(separator, StringSplitOptions.RemoveEmptyEntries)

            If nodes.Length = 0 Then
                Exit Sub
            End If

            If Not Marker.ShowRootName Then
                index = 1
                If nodes.Length < 2 Then
                    Exit Sub
                End If

                nodePath.Append(nodes(0))
                nodePath.Append(PathHandler.NODE_DIVIDER)
            End If

            'Because nodes are loaded on demand we have to load and expand the childnodes in a loop
            For i As Integer = index To nodes.Length - 1
                If Not first Then
                    nodePath.Append(PathHandler.NODE_DIVIDER)
                End If

                nodePath.Append(nodes(i))

                If first Then
                    foundNode = MenuNavigationTree.FindNode(System.Web.HttpUtility.UrlDecode(nodePath.ToString()))

                    If foundNode Is Nothing Then
                        Exit Sub
                    End If

                    foundNode.Expand()
                    parentNode = foundNode
                    first = False
                Else
                    For Each node As TreeNode In parentNode.ChildNodes
                        If node.Value.Equals(nodePath.ToString()) Then
                            node.Expand()
                            parentNode = node
                        End If
                    Next
                End If
            Next

        End If
    End Sub

    Protected Sub TableOfContent_LanguageChanged() Handles Me.LanguageChanged
        SetLocalizedText()
    End Sub

    ''' <summary>
    ''' Render language strings.
    ''' </summary>
    Private Sub SetLocalizedText()
        ' --- Action button shown if script is disabled
        ActionButton.Text = GetLocalizedString("CtrlTableOfContentActionButton")
    End Sub

    ''' <summary>
    ''' The method is used to add client scripts to table of content
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadScripts()
        'Used to hide the action button if javascript is enabled
        Page.ClientScript.RegisterStartupScript(Me.GetType, "TableOfContent_HideActionButton", "PCAxis_HideElement("".tableofcontent_action"");", True)
    End Sub

    ''' <summary>
    ''' Populates a tree node on demand
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub PopulateNode(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
        _menu = Marker.GetMenu(e.Node.Value)

        If _menu Is Nothing Then
            Exit Sub
        End If

        If TypeOf (_menu) Is PxMenuItem Then
            Dim menuitem As PxMenuItem = CType(_menu, PxMenuItem)

            For Each item As PCAxis.Menu.Item In menuitem.SubItems
                'If e.Node.Value <> item.Selection Then
                AddTreeViewNode(item, e.Node)
                'End If
            Next
            For Each item As PCAxis.Menu.Item In menuitem.Urls
                'If e.Node.Value <> item.Selection Then
                AddTreeViewNode(item, e.Node)
                'End If
            Next

        End If
    End Sub

    ''' <summary>
    ''' Renders the menu as a list
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RenderMenuList()

        Dim id As String

        If (Not String.IsNullOrEmpty(Marker.ExpandNode)) AndAlso String.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_path")) Then
            id = Marker.ExpandNode
        ElseIf Not String.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_path")) Then
            id = QuerystringManager.GetQuerystringParameter("px_path")
        Else
            'First time
            id = Marker.StartNode
        End If

        id = System.Web.HttpUtility.UrlDecode(id)

        _menu = Marker.GetMenu(id)

        If TypeOf (_menu) Is PxMenuItem Then
            Dim _MenuItemList As New List(Of MenuLink)
            Dim _MenuLinkList As New List(Of MenuLink)

            For Each childItem As Item In CType(_menu, PxMenuItem).SubItems

                Dim MenuLinkItem As New MenuLink

                If TypeOf (childItem) Is PxMenuItem Then
                    MenuLinkItem.Text = CType(childItem, PxMenuItem).Text
                    MenuLinkItem.LinkURL = GetMenuItemURL(childItem)
                    _MenuItemList.Add(MenuLinkItem)

                ElseIf TypeOf (childItem) Is PCAxis.Menu.Link Then
                    Dim MenuLink As PCAxis.Menu.Link = CType(childItem, PCAxis.Menu.Link)
                    MenuLinkItem.Text = GetMenuItemText(MenuLink, True)
                    MenuLinkItem.LinkURL = GetMenuItemURL(MenuLink)
                    _MenuLinkList.Add(MenuLinkItem)
                End If

            Next
            MenuItemList.DataSource = _MenuItemList
            LinkItemList.DataSource = _MenuLinkList

            MenuItemList.DataBind()
            LinkItemList.DataBind()

        Else
            Page.Response.Redirect(Marker.DefaultPageURL)
        End If

    End Sub


    ''' <summary>
    ''' Add a node to the tree
    ''' </summary>
    ''' <param name="Item">The item to add</param>
    ''' <param name="parentNode">The parent of the item</param>
    ''' <remarks></remarks>
    Private Sub AddTreeViewNode(ByVal Item As PCAxis.Menu.Item, ByVal parentNode As TreeNode)

        Dim childNode As TreeNode

        If TypeOf (Item) Is PxMenuItem Then
            childNode = New TreeNode(Item.Text)
            childNode.Value = CreateNodeId(Item, parentNode)
            childNode.PopulateOnDemand = True
            childNode.SelectAction = TreeNodeSelectAction.Expand
            If Not parentNode Is Nothing Then
                parentNode.ChildNodes.Add(childNode)
            Else
                MenuNavigationTree.Nodes.Add(childNode)
            End If

            If IsTableListLink(Item) Then
                SetTableListLink(Item, childNode)
            End If

            If Marker.UrlLinkMode = UrlLinkModeType.Image Then
                AddImageLinks(CType(Item, PxMenuItem), childNode)
            End If

        ElseIf TypeOf (Item) Is PCAxis.Menu.Link Then
            If Marker.IncludePXFilesInTreeView Then
                If Marker.UrlLinkMode = UrlLinkModeType.Image Then
                    If CType(Item, PCAxis.Menu.Link).Type = LinkType.URL Then
                        'Don´t show link as tree node 
                        Exit Sub
                    End If
                End If

                Dim MenuLink As PCAxis.Menu.Link = CType(Item, PCAxis.Menu.Link)
                childNode = New TreeNode("<span class='tableofcontent_link'>" + GetMenuItemText(MenuLink, True) + "</span>")
                childNode.SelectAction = TreeNodeSelectAction.None
                childNode.NavigateUrl = GetMenuItemURL(MenuLink, parentNode)

                If Not parentNode Is Nothing Then
                    parentNode.ChildNodes.Add(childNode)
                Else
                    MenuNavigationTree.Nodes.Add(childNode)
                End If
            End If

        ElseIf TypeOf (Item) Is PCAxis.Menu.Headline Then
            If (Marker.IncludePXFilesInTreeView) Then
                childNode = New TreeNode
                childNode.Text = "<span>" + Item.Text + "</span>"
                childNode.SelectAction = TreeNodeSelectAction.None
                If Not parentNode Is Nothing Then
                    parentNode.ChildNodes.Add(childNode)
                Else
                    MenuNavigationTree.Nodes.Add(childNode)
                End If
            End If

        End If

    End Sub

    ''' <summary>
    ''' Create node id for the specified menu item
    ''' </summary>
    ''' <param name="Item">The menu item to create an id for</param>
    ''' <param name="parentNode">Parent node of the menu item</param>
    ''' <returns>Id string of the menu item</returns>
    ''' <remarks></remarks>
    Private Function CreateNodeId(ByVal Item As PCAxis.Menu.Item, ByVal parentNode As TreeNode) As String
        Dim parentId As String

        If Not parentNode Is Nothing Then
            parentId = parentNode.Value
        Else
            parentId = Item.ID.Menu
        End If

        Return _pathHandler.Combine(parentId, Item.ID)

    End Function

    ''' <summary>
    ''' When UrlLinkMode = Image, links shall be displayed as icons next to the folder containing the link.
    ''' This method adds these icons.
    ''' </summary>
    ''' <param name="item">The PxMenuItem representing the folder</param>
    ''' <param name="node">The tree node representing the folder</param>
    ''' <remarks></remarks>
    Private Sub AddImageLinks(ByVal item As PxMenuItem, ByVal node As TreeNode)
        If Marker.UrlLinkMode = UrlLinkModeType.Image Then


            For Each url As PCAxis.Menu.Url In item.Urls

                If url.LinkPres = LinkPres.Icon Then
                    node.Text = node.Text & "|" & GetMenuItemURL(url)
                    node.ToolTip = GetLocalizedString("CtrlTableOfContentExternalLinkTooltip")
                End If

            Next
        End If

    End Sub

    ''' <summary>
    ''' Checks if the menu item is a link to TableList. 
    ''' To be a TableList link two conditions must be fulfilled:
    ''' 1. Tables are not included in the tree
    ''' 2. The menu item only contains tables (no subfolders)
    ''' </summary>
    ''' <param name="Item">The menu item to check</param>
    ''' <returns>True if the menu item is a TableList link, else false</returns>
    ''' <remarks></remarks>
    Private Function IsTableListLink(ByVal Item As PCAxis.Menu.Item) As Boolean
        If TypeOf (Item) Is PxMenuItem Then
            If Not Marker.IncludePXFilesInTreeView Then
                If ItemContainsOnlyPxFiles(CType(Item, PxMenuItem)) Then
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    ''' <summary>
    ''' Sets the TableList-link properties on the tree node
    ''' </summary>
    ''' <param name="Item">The menu item representing the node</param>
    ''' <param name="node">The tree node to set properties for</param>
    ''' <remarks></remarks>
    Private Sub SetTableListLink(ByVal Item As PCAxis.Menu.Item, ByVal node As TreeNode)
        'node.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.MenuPageURL, False, New LinkItem("tablelist", "true"), New LinkItem("selection", node.Value))
        node.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.MenuPageURL, False, New LinkItem("tablelist", "true"), New LinkItem("px_path", node.Value))
        node.Text = "<span class='tableofcontent_tablelistlink'>" + node.Text + "</span>"
        node.SelectAction = TreeNodeSelectAction.SelectExpand
        node.PopulateOnDemand = False
    End Sub

    ''' <summary>
    ''' Checks if the current item (folder) only contains tables (no subfolders)
    ''' </summary>
    ''' <param name="Item">The item to check</param>
    ''' <returns>True if the item only contains tables, else false</returns>
    ''' <remarks></remarks>
    Private Function ItemContainsOnlyPxFiles(ByVal Item As PxMenuItem) As Boolean
        Dim px As Boolean = False
        Dim folder As Boolean = False

        Dim pHandler As PathHandler = PathHandlerFactory.Create(Marker.DatabaseType)

        Dim nodeid As String = pHandler.Combine(Item.ID.Menu, Item.ID)

        Dim i As Item = Marker.GetMenu(nodeid)

        If TypeOf i Is PxMenuItem Then
            Item = CType(i, PxMenuItem)
        End If


        For Each childItem As Item In Item.SubItems
            If TypeOf (childItem) Is PCAxis.Menu.Link Then
                'PX-file
                px = True
            Else
                'Folder
                folder = True
            End If

            If px And folder Then
                Return False
            End If
        Next

        If px And Not folder Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetMenuItemText(ByVal MenuItem As PCAxis.Menu.Item) As String
        Return GetMenuItemText(MenuItem, False)
    End Function

    Public Function GetMenuItemText(ByVal MenuItem As PCAxis.Menu.Item, ByRef ShowMetaIcons As Boolean) As String
        Dim altStr As String = ""
        Dim linkText As StringBuilder = New StringBuilder()
        linkText.Append(MenuItem.Text)

        If TypeOf (MenuItem) Is PCAxis.Menu.Link Then
            Dim MenuLink As PCAxis.Menu.Link = CType(MenuItem, PCAxis.Menu.Link)

            If ShowMetaIcons Then
                Dim imgPath As String
                If (MenuLink.HasAttribute("size")) Then
                    If Marker.ShowFileSize Then
                        If (MenuLink.GetAttribute("size").ToString().Length > 0) Then
                            Dim size As Integer

                            If Integer.TryParse(MenuLink.GetAttribute("size").ToString(), size) Then
                                size = CInt(size / 1000)
                            End If

                            altStr = GetLocalizedString("CtrlTableOfContentSizeHeading") & " " & size.ToString() & " " & GetLocalizedString("CtrlTableOfContentSizeKilobytes")
                            If Marker.MetadataAsIcons Then
                                imgPath = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, Marker.MetaIconSizeImagePath)
                                linkText.Append(GetMenuItemMetaIcon(imgPath, altStr))
                            Else
                                If Not Marker.ShowTextForMetadata Then
                                    altStr = size.ToString() & " " & GetLocalizedString("CtrlTableOfContentSizeKilobytes")
                                End If
                                linkText.Append(GetMetadataText(altStr))
                            End If
                        End If
                    End If
                End If

                If (MenuLink.HasAttribute("modified")) Then
                    If Marker.ShowModified Then
                        If (MenuLink.GetAttribute("modified").ToString().Length > 0) Then
                            altStr = GetLocalizedString("CtrlTableOfContentModifiedHeading") & " " & MenuLink.GetAttribute("modified").ToString().PxDate()
                            If Marker.MetadataAsIcons Then
                                imgPath = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, Marker.MetaIconModifiedImagePath)
                                linkText.Append(GetMenuItemMetaIcon(imgPath, altStr))
                            Else
                                If Not Marker.ShowTextForMetadata Then
                                    altStr = MenuLink.GetAttribute("modified").ToString().PxDate()
                                End If
                                linkText.Append(GetMetadataText(altStr))
                            End If
                        End If
                    End If
                End If

                If (MenuLink.HasAttribute("updated")) Then
                    If Marker.ShowLastUpdated Then
                        If (MenuLink.GetAttribute("updated").ToString().Length > 0) Then
                            altStr = GetLocalizedString("CtrlTableOfContentUpdatedHeading") & " " & MenuLink.GetAttribute("updated").ToString().PxDate()
                            If Marker.MetadataAsIcons Then
                                imgPath = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, Marker.MetaIconUpdatedImagePath)
                                linkText.Append(GetMenuItemMetaIcon(imgPath, altStr))
                            Else
                                If Not Marker.ShowTextForMetadata Then
                                    altStr = MenuLink.GetAttribute("updated").ToString().PxDate()
                                End If
                                linkText.Append(GetMetadataText(altStr))
                            End If
                        End If
                    End If
                End If
            End If

            AddTableCategory(MenuLink, linkText)
            AddTableUpdatedAfterPublish(MenuLink, linkText)

        End If

        Return linkText.ToString()

    End Function

    ''' <summary>
    ''' Add table category to table text
    ''' </summary>
    ''' <param name="menuLink">Menu.Link item for the table</param>
    ''' <param name="linkText">Stringbuilder object with table text</param>
    ''' <remarks></remarks>
    Private Sub AddTableCategory(ByVal menuLink As PCAxis.Menu.Link, ByVal linkText As StringBuilder)
        If Marker.ShowTableCategory Then
            Select Case menuLink.Category
                Case PresCategory.Internal
                    linkText.Append(" (I)")
                Case PresCategory.Official
                    linkText.Append(" (O)")
                Case PresCategory.Private
                    linkText.Append(" (P)")
                Case Else
                    linkText.Append(" (S)")
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Add image to the tables that has been updated after they where published
    ''' </summary>
    ''' <param name="menuLink">Menu.Link item for the table</param>
    ''' <param name="linkText">Stringbuilder object with table text</param>
    ''' <remarks></remarks>
    Private Sub AddTableUpdatedAfterPublish(ByVal menuLink As PCAxis.Menu.Link, ByVal linkText As StringBuilder)
        If Marker.ShowTableUpdatedAfterPublish Then
            If TypeOf (menuLink) Is PCAxis.Menu.TableLink Then
                Dim tbllnk As PCAxis.Menu.TableLink = CType(menuLink, PCAxis.Menu.TableLink)

                If tbllnk.LastUpdated > tbllnk.Published Then
                    Dim imgPath As String = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, Marker.MetaIconUpdatedAfterPublishImagePath)
                    linkText.Append(GetMenuItemMetaIcon(imgPath, ""))
                End If
            End If
        End If
    End Sub

    Public Function GetMenuItemURL(ByVal MenuItem As PCAxis.Menu.Item, Optional ByVal parentNode As TreeNode = Nothing) As String

        Dim linkURL As String = ""

        ' --- If Item is of type Link
        If TypeOf (MenuItem) Is PCAxis.Menu.Link Then
            Dim menuLink As PCAxis.Menu.Link = CType(MenuItem, PCAxis.Menu.Link)
            Dim table As String = _pathHandler.GetTable(menuLink.ID)

            Select Case (menuLink.Type)
                Case LinkType.PX, LinkType.Table
                    Dim path As String = ""
                    If Not parentNode Is Nothing AndAlso Not String.IsNullOrEmpty(parentNode.Value) Then
                        path = parentNode.Value
                    Else
                        If Not String.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_path")) Then
                            path = QuerystringManager.GetQuerystringParameter("px_path")
                        ElseIf Not String.IsNullOrEmpty(Marker.ExpandNode) Then
                            path = Marker.ExpandNode
                        Else
                            path = "-"
                        End If
                    End If

                    If (MenuItem.HasAttribute("autoOpen") AndAlso MenuItem.GetAttribute("autoOpen").ToString().Equals("true", StringComparison.OrdinalIgnoreCase)) Then
                        linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SelectPageURL, False, New LinkItem("px_tableid", table), New LinkItem("px_path", path),
                                                                                New LinkItem("px_autoopen", "true"))
                    Else
                        linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SelectPageURL, False, New LinkItem("px_tableid", table), New LinkItem("px_path", path))
                    End If

                Case LinkType.URL
                    If Not String.IsNullOrEmpty(CType(MenuItem, PCAxis.Menu.Url).LinkUrl) Then
                        linkURL = CType(MenuItem, PCAxis.Menu.Url).LinkUrl
                    Else
                        'TODO: Get base-URL from new TableOfContent property
                        linkURL = table
                    End If
                Case LinkType.PXS
                    'TODO: fixa länkar till formatet PxsFile
                    linkURL = table
                Case LinkType.XDF
                    'TODO: fixa länkar till formatet XDFFile
                    linkURL = table
                Case Else
                    linkURL = table
            End Select
        End If

        ' --- I Item is of type MenuItem
        If TypeOf (MenuItem) Is PxMenuItem Then
            Dim MenuLink As PxMenuItem = CType(MenuItem, PxMenuItem)
            Dim nodeid As String

            If String.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_path")) Then
                nodeid = _pathHandler.GetPathString(MenuLink)
            Else
                nodeid = _pathHandler.Combine(QuerystringManager.GetQuerystringParameter("px_path"), MenuLink.ID)
            End If

            nodeid = System.Web.HttpUtility.UrlPathEncode(nodeid)

            linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.MenuPageURL, False, New LinkItem("px_path", nodeid))
        End If

        Return linkURL

    End Function

    ''' <summary>
    ''' Create HTML string for the meta icon image. 
    ''' </summary>
    ''' <param name="ImageUrl">Relative path to the image</param>
    ''' <param name="ImageAlt">Alternative text for the image</param>
    ''' <returns>HTML string for the image</returns>
    ''' <remarks></remarks>
    Public Function GetMenuItemMetaIcon(ByVal ImageUrl As String, ByVal ImageAlt As String) As String
        Dim MetaIcon As String
        MetaIcon = "<img src='" + ResolveUrl(ImageUrl) + "' title='" + ImageAlt + " ' alt='" + ImageAlt + "' class='tableofcontent_metaicon' />"

        Return MetaIcon
    End Function

    ''' <summary>
    ''' Create HTML string for table metadata as text
    ''' </summary>
    ''' <param name="text">Input text</param>
    ''' <returns>HTML string</returns>
    ''' <remarks></remarks>
    Public Function GetMetadataText(ByVal text As String) As String
        Dim sb As New StringBuilder()

        sb.Append("&nbsp;<em>[" + text + "]</em>")

        Return sb.ToString()
    End Function


    Private Class MenuLink

        Private _text As String
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

        Private _linkurl As String
        Public Property LinkURL() As String
            Get
                Return _linkurl
            End Get
            Set(ByVal value As String)
                _linkurl = value
            End Set
        End Property

        Private _type As String
        Public Property Type() As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property


    End Class

    Private Sub ActionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ActionButton.Click
        MenuNavigationTree.ExpandDepth = -1
        MenuNavigationTree.ExpandAll()
        ActionButton.Visible = False
    End Sub

End Class

