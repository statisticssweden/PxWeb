

Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Net
Imports System.Text

Imports PCAxis.Menu

Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management.LinkManager
Imports PCAxis.Web.Core.Management


''' <summary>
''' Component that displays information about the .px-files identified by the CurrentItem
''' in the Menu object property
''' 
''' The Menu object is usually set in Default.aspx Load event before TableList Load event is triggered like this:
'''  TableList.Menu = New Global.Menu.Implementations.XMLMenu(xmlFile, "sv")
'''  TableList.Menu.SetCurrentItemBySelection(Request.QueryString("selection"))
''' </summary>
''' <remarks></remarks>
Public Class TableListCodebehind
    Inherits PaxiomControlBase(Of TableListCodebehind, TableList)
    Private _showTextAsText As Boolean
    Private _showLinkURLSelect As Boolean
    Private _showShowDownloadLink As Boolean
    Private _showSelectOption_View As Boolean
    Private _showSelectOption_ViewDefaultValues As Boolean
    Private _showSelectOption_ViewWithCommandbar As Boolean
    Private _showSelectOption_ViewDefaultValuesWithCommandbar As Boolean
    Private _showShowFileSize As Boolean
    Private _showShowModifiedDate As Boolean
    Private _showShowLastUpdated As Boolean
    Private _showVariablesAndValues As Boolean

    Private _pathHandler As PathHandler
    Private _pxPath As String

#Region " Controls "

    Protected MenulistPanel As Panel
    Protected WithEvents LinkItemList As Repeater
    'Protected WithEvents DownloadHyperLink As HyperLink
#End Region

    ''' <summary>
    ''' if the Query string "selection" is passed as argument in the url, then the
    ''' table list is rendered    
    ''' 
    ''' Preconditions: 
    ''' Marker.Menu must be set before the Load event is triggered
    ''' Like this: TableList.Menu = New Global.Menu.Implementations.XMLMenu(xmlFile, "sv")
    ''' </summary>
    ''' <param name="sender">not used</param>
    ''' <param name="e">not used</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (QuerystringManager.GetQuerystringParameter("tablelist") = "true") Then

            If Not Page.RouteData.Values("px_path") Is Nothing Then
                _pxPath = ValidationManager.GetValue(CType(Page.RouteData.Values("px_path"), String))
            Else
                If Not String.IsNullOrEmpty(QuerystringManager.GetQuerystringParameter("px_path")) Then
                    _pxPath = QuerystringManager.GetQuerystringParameter("px_path")
                End If
            End If

            _pathHandler = PathHandlerFactory.Create(Marker.DatabaseType)

            If (Marker.OpenSelectInNewWindow) Then
                Marker.LinkTarget = "_blank"
            Else
                Marker.LinkTarget = "_top"
            End If

            If Not String.IsNullOrEmpty(_pxPath) Then
                Dim menu As Item = Marker.GetMenu(_pxPath)

                If TypeOf (menu) Is PxMenuItem Then
                    RenderMenuList(CType(menu, PxMenuItem))
                Else
                    Page.Response.Redirect("~/" + Marker.DefaultPageURL)
                End If
            Else
                'Page.Response.Redirect("~/" + Marker.DefaultPageURL)
            End If
        Else
            MenulistPanel.Visible = False
        End If


        '   DownloadHyperLink.Visible = False

    End Sub


    ''' <summary>
    ''' Renders the table list.
    ''' 
    ''' The method iterates all SubItems in MenuItem and uses 
    ''' information from these to create MenuLink objects.
    ''' These items are added to a list that eventually is 
    ''' data bound to the Repeatercontrol LinkItemList.
    ''' 
    ''' </summary>
    ''' <param name="MenuItem">Based on this object's SubItems, a list of MenuLink objects is created</param>
    ''' <remarks></remarks>
    Private Sub RenderMenuList(ByVal MenuItem As PxMenuItem)

        Dim MenuLinkList As New List(Of MenuLink)
        ''Dim MenuHelper As New MenuHelper(Marker.Menu)

        For Each childItem As Item In MenuItem.SubItems

            Dim MenuLinkItem As New MenuLink
            If TypeOf (childItem) Is PCAxis.Menu.Link Then
                Select Case CType(childItem, PCAxis.Menu.Link).Type
                    'CNMM
                    Case LinkType.Table
                        Marker.ShowFileSize = False
                        Marker.ShowLastUpdated = False

                        MenuLinkItem.Text = CType(childItem, TableLink).Text
                        MenuLinkItem.IsSmallFile = False

                        MenuLinkItem.LinkURLSelect = GetMenuItemURL(CType(childItem, TableLink), "Select")
                        'TODO: fixa länkarna till Download
                        MenuLinkItem.LinkURLDownload = GetMenuItemURL(CType(childItem, TableLink), "Download")

                        If (CType(childItem, TableLink).HasAttribute("modified")) Then
                            MenuLinkItem.Modified = CType(childItem, TableLink).GetAttribute("modified").ToString()
                        End If

                        MenuLinkItem.Type = LinkType.Table
                        MenuLinkList.Add(MenuLinkItem)
                        'PX-file
                    Case LinkType.PX
                        If GetVariables(CType(childItem, TableLink), MenuLinkItem.Variables) Then
                            MenuLinkItem.Text = CType(childItem, TableLink).Text
                            MenuLinkItem.IsSmallFile = False

                            MenuLinkItem.LinkURLSelect = GetMenuItemURL(CType(childItem, TableLink), "Select")
                            MenuLinkItem.LinkURLSelectPart = GetMenuItemURL(CType(childItem, TableLink), "SelectPart")

                            'TODO: fixa länkarna till Download
                            MenuLinkItem.LinkURLDownload = GetMenuItemURL(CType(childItem, TableLink), "Download")

                            If (CType(childItem, TableLink).HasAttribute("size")) Then
                                MenuLinkItem.Size = CType(childItem, TableLink).GetAttribute("size").ToString()

                            End If

                            'Determine if it a small file
                            Dim size As Integer = -1
                            If Marker.SmallFileSizeMode = SmallFileSizeModeType.FileSize Then
                                'Size in bytes...
                                If Not String.IsNullOrEmpty(MenuLinkItem.Size) Then
                                    size = CType(MenuLinkItem.Size.Split(CChar(" "))(0), Integer)
                                End If
                            Else
                                'Number of cells...
                                If (CType(childItem, TableLink).HasAttribute("cells")) Then
                                    If Not Integer.TryParse(CType(childItem, TableLink).GetAttribute("cells").ToString(), size) Then
                                        size = -1
                                    End If
                                End If
                            End If

                            If (size > 0) AndAlso (size < Marker.MaxFileziseForSmallFile) Then
                                MenuLinkItem.IsSmallFile = True
                            End If

                            Select Case Marker.SelectOption_Download
                                Case DownloadLinkVisibilityType.AlwaysHide
                                    MenuLinkItem.ShowDownloadLink = False
                                Case DownloadLinkVisibilityType.AlwaysShow
                                    MenuLinkItem.ShowDownloadLink = True
                                Case DownloadLinkVisibilityType.ShowIfSmallFile
                                    If MenuLinkItem.IsSmallFile Then
                                        MenuLinkItem.ShowDownloadLink = True
                                    Else
                                        MenuLinkItem.ShowDownloadLink = False
                                    End If
                            End Select

                            If (CType(childItem, TableLink).HasAttribute("modified")) Then
                                MenuLinkItem.Modified = CType(childItem, TableLink).GetAttribute("modified").ToString().PxDate()
                            End If

                            If (CType(childItem, TableLink).HasAttribute("updated")) Then
                                MenuLinkItem.Updated = CType(childItem, TableLink).GetAttribute("updated").ToString().PxDate()
                            End If

                            MenuLinkItem.Type = LinkType.PX
                            MenuLinkList.Add(MenuLinkItem)
                        End If
            'External link
                    Case LinkType.URL
                            MenuLinkItem.Text = CType(childItem, Url).Text
                            MenuLinkItem.LinkURLSelect = CType(childItem, PCAxis.Menu.Url).LinkUrl
                            MenuLinkItem.Type = LinkType.URL
                            MenuLinkList.Add(MenuLinkItem)
                End Select
            End If
        Next

        LinkItemList.DataSource = MenuLinkList


        'Set parameters for rendering of table list
        _showTextAsText = (Marker.SelectOption_Select Or Marker.SelectOption_View Or Marker.SelectOption_ViewDefaultValues Or Marker.SelectOption_ViewWithCommandbar Or Marker.SelectOption_ViewDefaultValuesWithCommandbar Or Marker.ShowAsListOfContent)
        _showLinkURLSelect = (Marker.SelectOption_Select And Not Marker.ShowAsListOfContent)
        _showShowDownloadLink = (Marker.ShowDownloadLink And Not Marker.ShowAsListOfContent)
        _showSelectOption_View = (Marker.SelectOption_View And Not Marker.ShowAsListOfContent)
        _showSelectOption_ViewDefaultValues = (Marker.SelectOption_ViewDefaultValues And Not Marker.ShowAsListOfContent)
        _showSelectOption_ViewWithCommandbar = (Marker.SelectOption_ViewWithCommandbar And Not Marker.ShowAsListOfContent)
        _showSelectOption_ViewDefaultValuesWithCommandbar = Marker.SelectOption_ViewDefaultValuesWithCommandbar And Not Marker.ShowAsListOfContent
        _showShowFileSize = Marker.ShowFileSize And Not Marker.ShowAsListOfContent
        _showShowModifiedDate = (Marker.ShowModifiedDate And Not Marker.ShowAsListOfContent)
        _showShowLastUpdated = (Marker.ShowLastUpdated And Not Marker.ShowAsListOfContent)
        _showVariablesAndValues = Marker.ShowVariablesAndValues

        LinkItemList.DataBind()

    End Sub


    ''' <summary>
    ''' Reads information about involved variables from the px-link´s attributes
    ''' </summary>
    ''' <param name="px">The px-link object</param>
    ''' <param name="vars">
    ''' Outparameter. 
    ''' An HTML-encoded string containing an ordered list with 
    ''' information about the variables in the file
    ''' </param>
    ''' <returns>True if variables was successfully read, else false</returns>
    ''' <remarks></remarks>
    Private Function GetVariables(ByVal px As TableLink, ByRef vars As String) As Boolean
        Dim variables As New StringBuilder()
        Dim i As Integer = 1

        'At least one variable with at least 1 value must exist
        If (Not px.HasAttribute("Var1Name")) Or _
           (Not px.HasAttribute("Var1Values")) Or _
           (Not px.HasAttribute("Var1NumberOfValues")) Then
            Return False
        End If

        variables.Append("<ol class='tablelist_variablelist_ol'>")

        While (px.HasAttribute("Var" & i.ToString & "Name")) And _
              (px.HasAttribute("Var" & i.ToString & "Values")) And _
              (px.HasAttribute("Var" & i.ToString & "NumberOfValues"))
            variables.Append("<li class='tablelist_variablelist_li'><b>" & px.GetAttribute("Var" & i.ToString() & "Name").ToString() & "</b>" + ": ")
            variables.Append(px.GetAttribute("Var" & i.ToString & "Values"))
            variables.Append(" (" & px.GetAttribute("Var" & i.ToString & "NumberOfValues").ToString() & ")</li>")
            i = i + 1
        End While

        variables.Append("</ol>")

        vars = variables.ToString()
        Return True
    End Function

    ''' <summary>
    ''' Creates a relative download url to a file
    ''' </summary>    
    Private Function CreateRelativeDownloadUrl(ByVal path As String) As String
        Dim linkURL As String = path

        linkURL = Marker.DatabasePath + linkURL
        linkURL = linkURL.Replace("\", "/").Replace("//", "/")
        Return linkURL
    End Function

    ''' <summary>
    ''' Creates an absolute download url to a file
    ''' </summary> 
    Private Function CreateAbsoluteDownloadUrl(ByVal Filename As String) As String
        Dim linkURL As String = Filename
        If Request.Url.Port <> 80 Then
            linkURL = Request.Url.Authority + "/" + linkURL
        Else
            linkURL = Request.Url.Host + "/" + Request.ApplicationPath + linkURL
        End If
        linkURL = linkURL.Replace("\", "/").Replace("//", "/")
        linkURL = "http://" + linkURL
        Return linkURL
    End Function



    ''' <summary>
    ''' create a url to the file identified in the MenuLink parameter
    ''' 
    ''' </summary>
    ''' <param name="MenuLink">The object containing information about the file</param>
    ''' <param name="SelectType">decides which .aspx pag we should link to</param>
    ''' <returns>a url as a string.</returns>
    ''' <remarks></remarks>
    Public Function GetMenuItemURL(ByVal MenuLink As PCAxis.Menu.Link, ByVal SelectType As String) As String

        Dim linkURL As String = ""
        Dim LinkType As LinkType = MenuLink.Type
        Dim autoOpen As Boolean = False

        If MenuLink.HasAttribute("autoOpen") Then
            If String.Compare(MenuLink.GetAttribute("autoOpen").ToString(), "true", StringComparison.InvariantCultureIgnoreCase) = 0 Then
                autoOpen = True
            End If
        End If
        Select Case (SelectType)
            Case "Download"
                If MenuLink.ID.Selection.EndsWith(".px") Then
                    linkURL = CreateRelativeDownloadUrl(MenuLink.ID.Selection)
                End If
            Case "Select", "SelectPart"
                'TODO: fixa länkning för övriga format
                Dim table As String = _pathHandler.GetTable(MenuLink.ID)
                Select Case (LinkType)
                    Case LinkType.PX
                        If autoOpen AndAlso SelectType <> "SelectPart" Then
                            linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SelectPageURL, False, New LinkItem("px_tableid", table), New LinkItem("px_path", _pxPath), New LinkItem("px_autoopen", "true"))
                        Else
                            linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SelectPageURL, False, New LinkItem("px_tableid", table), New LinkItem("px_path", _pxPath))
                        End If
                    Case LinkType.Table
                        linkURL = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.SelectPageURL, False, New LinkItem("px_tableid", table), New LinkItem("px_path", _pxPath))
                        'linkURL = MenuLink.Selection
                    Case Else
                        linkURL = table
                End Select
            Case Else
                linkURL = "Else"
        End Select

        Return linkURL

    End Function


    ''' <summary>
    ''' Used to present information about .px files
    '''     
    ''' In the method RenderMenuList() a list of MenuLink
    ''' items is data bound to a Repeater object defined in TableList.ascx
    ''' </summary>
    ''' <remarks></remarks>
    Private Class MenuLink

        ''' <summary>
        ''' Gets or sets the table title
        ''' </summary>
        ''' <value>the table title</value>        
        ''' <remarks></remarks>
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property
        Private _text As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Type() As LinkType
            Get
                Return _type
            End Get
            Set(ByVal value As LinkType)
                _type = value
            End Set
        End Property
        Private _type As LinkType = LinkType.PX

        ''' <summary>
        ''' Gets or Sets a File Size description
        ''' </summary>
        ''' <value>The file size as a string representation. E.g. '2 kb'</value>        
        ''' <remarks></remarks>
        Public Property Size() As String
            Get
                Return _size
            End Get
            Set(ByVal value As String)
                _size = value
            End Set
        End Property
        Private _size As String

        ''' <summary>
        ''' Gets or Sets the time the file was modified        
        ''' </summary>
        ''' <value>The time the file was modified. E.g. '20060209 15:20'</value>
        ''' <remarks></remarks>
        Public Property Modified() As String
            Get
                Return _modified
            End Get
            Set(ByVal value As String)
                _modified = value
            End Set
        End Property
        Private _modified As String

        ''' <summary>
        ''' Gets or Sets the time the file was updated
        ''' </summary>
        ''' <value>The time the file was updated</value>
        ''' <remarks></remarks>
        Public Property Updated() As String
            Get
                Return _updated
            End Get
            Set(ByVal value As String)
                _updated = value
            End Set
        End Property
        Private _updated As String

        ''' <summary>
        ''' Gets or Sets an HTML-encoded string containing an ordered list with 
        ''' information about the variables in the file
        ''' </summary>
        ''' <value>An HTML-encoded string representing an ordered list  ol</value>        
        ''' <remarks></remarks>
        Public Property Variables() As String
            Get
                Return _variables
            End Get
            Set(ByVal value As String)
                _variables = value
            End Set
        End Property
        Private _variables As String


        ''' <summary>
        ''' Gets or Sets the URL that is used to navigate to the Selection page        
        ''' </summary>
        ''' <value>
        ''' Url to the selection page including the querystring to the file
        ''' E.g. Select.aspx?...px=BE0101A1proc.px
        ''' </value>        
        ''' <remarks></remarks>
        Public Property LinkURLSelect() As String
            Get
                Return _linkurlselect
            End Get
            Set(ByVal value As String)
                _linkurlselect = value
            End Set
        End Property
        Private _linkurlselect As String

        ''' <summary>
        ''' Gets or Sets the URL that is used to the Selection page even if 
        ''' autOpen=TRUE in the PX file      
        ''' </summary>
        ''' <value>
        ''' Url to the selection page including the querystring to the file
        ''' E.g. Select.aspx?...px=BE0101A1proc.px
        ''' </value>        
        ''' <remarks></remarks>
        Public Property LinkURLSelectPart() As String
            Get
                Return _linkurlselectPart
            End Get
            Set(ByVal value As String)
                _linkurlselectPart = value
            End Set
        End Property
        Private _linkurlselectPart As String

        ''' <summary>
        ''' Gets or Sets the URL that is used to download the file        
        ''' </summary>
        ''' <value>
        ''' Url that is used to download the px file
        ''' E.g. Download.aspx?...Selection=BE0101A1proc.px
        ''' </value>            
        ''' <remarks></remarks>
        Public Property LinkURLDownload() As String
            Get
                Return _linkurldownload
            End Get
            Set(ByVal value As String)
                _linkurldownload = value
            End Set
        End Property
        Private _linkurldownload As String

        ''' <summary>
        ''' Gets or Sets if the file is a small file or not
        '''         
        ''' IsSmallFile is True if the file size is smaller than Marker.MaxFileziseForSmallFile
        ''' The default value for Marker.MaxFileziseForSmallFile is 1000kb
        ''' </summary>
        ''' <value>True if file is small, False if the file is large</value>        
        ''' <remarks></remarks>
        Public Property IsSmallFile() As Boolean
            Get
                Return _isSmallFile
            End Get
            Set(ByVal value As Boolean)
                _isSmallFile = value
            End Set
        End Property
        Private _isSmallFile As Boolean

        Public Property ShowDownloadLink() As Boolean
            Get
                Return _showDownloadLink
            End Get
            Set(ByVal value As Boolean)
                _showDownloadLink = value
            End Set
        End Property
        Private _showDownloadLink As Boolean

    End Class

    Private Sub LinkItemList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles LinkItemList.ItemDataBound
        If e.Item.DataItem IsNot Nothing Then
            Select Case CType(e.Item.DataItem, MenuLink).Type
                Case LinkType.PX, LinkType.Table
                    CType(e.Item.FindControl("pnlUrl"), Panel).Visible = False
                Case LinkType.URL
                    CType(e.Item.FindControl("pnlPx"), Panel).Visible = False
            End Select

            Dim link As HyperLink
            
            link = CType(e.Item.FindControl("lnkTableListItemText"), HyperLink)
            link.NavigateUrl = DataBinder.Eval(e.Item.DataItem, "LinkURLSelect").ToString()
            link.Text = DataBinder.Eval(e.Item.DataItem, "Text").ToString()
            link.Visible = True           

            If (_showLinkURLSelect AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLSelectPart")))) Then
                link = CType(e.Item.FindControl("lnkTableListLinkURLSelect"), HyperLink)
                link.NavigateUrl = DataBinder.Eval(e.Item.DataItem, "LinkURLSelectPart").ToString()
                link.Target = Marker.LinkTarget
                link.Text = GetLocalizedString("CtrlTableListSelectOption_Select")
                link.Visible = True
            End If

            If (_showShowDownloadLink AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLDownload")))) Then
                link = CType(e.Item.FindControl("lnkTableListShowDownloadLink"), HyperLink)
                link.NavigateUrl = DataBinder.Eval(e.Item.DataItem, "LinkURLDownload").ToString()
                link.Text = GetLocalizedString("CtrlTableListSelectOption_Download")
                link.Visible = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "ShowDownloadLink"))
            End If

            If (_showSelectOption_View AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect")))) Then
                link = CType(e.Item.FindControl("lnkTableListSelectOption_View"), HyperLink)
                link.NavigateUrl = CheckNativeUrl(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect").ToString()) & "action=selectall&commandbar=false"
                link.Target = Marker.LinkTarget
                link.Text = GetLocalizedString("CtrlTableListSelectOption_View")
                link.Visible = True
            End If

            If (_showSelectOption_ViewDefaultValues AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect")))) Then
                link = CType(e.Item.FindControl("lnkTableListSelectOption_ViewDefaultValues"), HyperLink)
                link.NavigateUrl = CheckNativeUrl(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect").ToString()) & "action=selectdefault&commandbar=false"
                link.Target = Marker.LinkTarget
                link.Text = GetLocalizedString("CtrlTableListSelectOption_ViewDefaultValues")
                link.Visible = True
            End If

            If (_showSelectOption_ViewWithCommandbar AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect")))) Then
                link = CType(e.Item.FindControl("lnkTableListSelectOption_ViewWithCommandbar"), HyperLink)
                link.NavigateUrl = CheckNativeUrl(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect").ToString()) & "action=selectall"
                link.Target = Marker.LinkTarget
                link.Text = GetLocalizedString("CtrlTableListSelectOption_ViewWithCommandbar")
                link.Visible = True
            End If

            If (_showSelectOption_ViewDefaultValuesWithCommandbar AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect")))) Then
                link = CType(e.Item.FindControl("lnkTableListSelectOption_ViewDefaultValuesWithCommandbar"), HyperLink)
                link.NavigateUrl = CheckNativeUrl(DataBinder.Eval(e.Item.DataItem, "LinkURLSelect").ToString()) & "action=selectdefault"
                link.Target = Marker.LinkTarget
                link.Text = GetLocalizedString("CtrlTableListSelectOption_ViewDefaultValuesWithCommandbar")
                link.Visible = True
            End If

            If (_showShowFileSize AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "Size")))) Then
                CType(e.Item.FindControl("ltlTableListShowFileSizeHeading"), Literal).Text = GetLocalizedString("CtrlTableListSizeHeading")
                CType(e.Item.FindControl("ltlTableListShowFileSizeHeading"), Literal).Visible = True
                CType(e.Item.FindControl("ltlTableListShowFileSizeSize"), Literal).Text = DataBinder.Eval(e.Item.DataItem, "Size").ToString()
                CType(e.Item.FindControl("ltlTableListShowFileSizeSize"), Literal).Visible = True
            End If

            If (_showShowModifiedDate AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "Modified")))) Then
                CType(e.Item.FindControl("ltlTableListShowModifiedDateHeading"), Literal).Text = GetLocalizedString("CtrlTableListModifiedHeading")
                CType(e.Item.FindControl("ltlTableListShowModifiedDateHeading"), Literal).Visible = True
                CType(e.Item.FindControl("ltlTableListShowModifiedDateModified"), Literal).Text = DataBinder.Eval(e.Item.DataItem, "Modified").ToString().PxDate()
                CType(e.Item.FindControl("ltlTableListShowModifiedDateModified"), Literal).Visible = True
            End If

            If (_showShowLastUpdated AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "Updated")))) Then
                CType(e.Item.FindControl("ltlTableListShowLastUpdatedHeading"), Literal).Text = GetLocalizedString("CtrlTableListUpdatedHeading")
                CType(e.Item.FindControl("ltlTableListShowLastUpdatedHeading"), Literal).Visible = True
                CType(e.Item.FindControl("ltlTableListShowLastUpdatedUpdated"), Literal).Text = DataBinder.Eval(e.Item.DataItem, "Updated").ToString().PxDate()
                CType(e.Item.FindControl("ltlTableListShowLastUpdatedUpdated"), Literal).Visible = True
            End If
            If (_showVariablesAndValues AndAlso (Not IsNothing(DataBinder.Eval(e.Item.DataItem, "Variables")))) Then
                CType(e.Item.FindControl("ltlTableListShowVariablesAndValues"), Literal).Text = "<p>" & DataBinder.Eval(e.Item.DataItem, "Variables").ToString() & "</p>"
                CType(e.Item.FindControl("ltlTableListShowVariablesAndValues"), Literal).Visible = True
            End If


        End If
    End Sub

    Private Function CheckNativeUrl(ByRef uriToCheck As String) As String
        If Not IsDBNull(uriToCheck) Then
            If (uriToCheck.Contains("?")) Then
                Return uriToCheck + "&"
            Else
                Return uriToCheck + "?"
            End If
        End If
        Return String.Empty
    End Function
End Class
