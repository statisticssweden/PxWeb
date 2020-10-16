

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Core.Management.LinkManager
Imports PCAxis.Query
Imports PCAxis.Menu

<ToolboxData("<{0}:Breadcrumb runat=""server""></{0}:Breadcrumb>")> _
Public Class BreadcrumbCodebehind
    Inherits PaxiomControlBase(Of BreadcrumbCodebehind, Breadcrumb)

#Region "fields"
    Protected imgHome As Image
    Protected lnkHome As HyperLink
    Protected lnkDb As HyperLink
    Protected lnkPath1 As HyperLink
    Protected lnkPath2 As HyperLink
    Protected lnkPath3 As HyperLink
    Protected lnkPath4 As HyperLink
    Protected lnkPath5 As HyperLink
    Protected lnkTable As HyperLink
    Protected lblSep1 As Label
    Protected lblSep2 As Label
    Protected lblSep3 As Label
    Protected lblSep4 As Label
    Protected lblSep5 As Label
    Protected lblSep6 As Label
    Protected lblSepBeforeTable As Label
    Protected lblSepBeforeSubPage As Label
    Protected lblSubPage As Label
#End Region

#Region "Localized strings"
#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SetHomepageLink()
        End If
    End Sub

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Update the breadcrumb
    ''' </summary>
    ''' <param name="mode">The breadcrumb mode</param>
    ''' <param name="subpage">Optional parameter subpage name</param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal mode As Breadcrumb.BreadcrumbMode, Optional ByVal subpage As String = "")
        Dim linkItems As List(Of LinkManager.LinkItem)
        Dim menu As Item = Nothing
        Dim pHandler As PathHandler = PathHandlerFactory.Create(Marker.DatabaseType)

        'If mode = Breadcrumb.BreadcrumbMode.Menu Then
        '    lnkDb.CssClass = "breadcrumb_text_nolink"
        'End If

        If mode >= Breadcrumb.BreadcrumbMode.Menu Then
            lblSep1.Visible = True
            lnkDb.Text = Marker.DatabaseName

            If Not String.IsNullOrEmpty(Marker.TablePath) Then
                menu = DisplayPath(menu, pHandler)
            End If
        End If

        If mode = Breadcrumb.BreadcrumbMode.Menu Then
            If String.IsNullOrEmpty(Marker.TablePath) Then
                lnkDb.CssClass = "breadcrumb_text_nolink"
            Else
                FixMenuPath()
            End If
        End If

        If mode = Breadcrumb.BreadcrumbMode.MenuSubPage Then
            linkItems = New List(Of LinkManager.LinkItem)
            linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, ""))
            lnkDb.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())

            If Not String.IsNullOrEmpty(subpage) Then
                lblSubPage.Text = subpage
                lblSubPage.Visible = True
                lblSepBeforeSubPage.Visible = True
            End If
        End If

        If mode = Breadcrumb.BreadcrumbMode.Selection Then
            lnkTable.CssClass = "breadcrumb_text_nolink"
        End If

        If mode >= Breadcrumb.BreadcrumbMode.Selection Then


            If menu Is Nothing Then
                menu = DisplayPath(menu, pHandler)
            End If

            If Not menu Is Nothing Then
                Dim tablePath As String
                Dim cid As ItemSelection

                Dim table As String = Marker.Table

                Dim index As Integer = table.IndexOf(":"c)
                If index >= 0 Then
                    table = table.Substring(index + 1)
                End If

                tablePath = Marker.TablePath & PathHandler.NODE_DIVIDER & table
                cid = pHandler.GetSelection(tablePath)
                menu = Marker.GetMenu(tablePath)
                If menu Is Nothing Then
                    Exit Sub
                End If

                lblSepBeforeTable.Visible = True
                lnkTable.Text = menu.Text
                lnkTable.Visible = True
            End If

        End If

        If mode >= Breadcrumb.BreadcrumbMode.SelectionSubPage Then
            If Not menu Is Nothing Then
                lnkTable.Text = menu.Text
                linkItems = New List(Of LinkManager.LinkItem)
                linkItems.Add(New LinkManager.LinkItem(Marker.LayoutParam, ""))
                lnkTable.NavigateUrl = LinkManager.CreateLink(Marker.SelectionPage, linkItems.ToArray())
            End If
        End If

        If mode = Breadcrumb.BreadcrumbMode.SelectionSubPage Then
            If Not String.IsNullOrEmpty(subpage) Then
                lblSubPage.Text = subpage
                lblSubPage.Visible = True
                lblSepBeforeSubPage.Visible = True
            End If
        End If

    End Sub



#End Region

#Region "Private methods"

    ''' <summary>
    ''' Display the path part of the breadcrumb
    ''' </summary>
    ''' <param name="menu"></param>
    ''' <param name="pHandler"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayPath(ByVal menu As Item, ByVal pHandler As PathHandler) As Item
        Dim path As List(Of ItemSelection)
        Dim pathParam As New System.Text.StringBuilder
        Dim linkItems As List(Of LinkManager.LinkItem)
        Dim nodeId As String

        linkItems = New List(Of LinkManager.LinkItem)
        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, ""))
        lnkDb.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())

        If Marker.TablePath.Equals("-") Then
            Marker.TablePath = Marker.DatabaseId
        End If

        Dim nodes As List(Of String) = pHandler.GetNodeIds(Marker.TablePath)
        path = pHandler.GetPath(Marker.TablePath)

        If nodes Is Nothing Then
            Return Nothing
        End If


        If path.Count <> nodes.Count Then
            Return Nothing
        End If

        nodeId = pHandler.Combine("", path(0))

        If path.Count > 0 Then
            menu = Marker.GetMenu(nodeId)
        End If

        If menu Is Nothing Then
            Return Nothing
        End If

        Dim lnkPaths As HyperLink() = {lnkPath1, lnkPath2, lnkPath3, lnkPath4, lnkPath5}
        Dim lblSeps As Label() = {lblSep2, lblSep3, lblSep4, lblSep5, lblSep6}

        pathParam.Append(path(0).Selection)

        For index = 1 To Math.Min(path.Count - 1, 5)

            menu = Marker.GetMenu(nodes(index))
            If menu Is Nothing Then
                Return Nothing
            End If

            lnkPaths(index - 1).Text = menu.Text
            lnkPaths(index - 1).Visible = True

            linkItems = New List(Of LinkManager.LinkItem)
            If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
                linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(index).Selection)))
            Else
                pathParam.Append(PathHandler.NODE_DIVIDER)
                pathParam.Append(path(index).Selection)
                linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
            End If
            If (Marker.UseTableList AndAlso path.Count = index - 1) Then
                linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
            End If

            lnkPaths(index - 1).NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
            lblSeps(index - 1).Visible = True

        Next

        'If path.Count > 1 Then
        '    nodeId = pHandler.Combine(nodeId, path(1))
        '    menu = Marker.GetMenu(nodeId)
        '    If menu Is Nothing Then
        '        Return Nothing
        '    End If

        '    lblSep2.Visible = True

        '    If Not menu Is Nothing Then
        '        lnkPath1.Text = menu.Text
        '        lnkPath1.Visible = True
        '    End If
        '    linkItems = New List(Of LinkManager.LinkItem)
        '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(1).Selection)))
        '    Else
        '        pathParam.Append(path(0).Selection)
        '        pathParam.Append(PathHandler.NODE_DIVIDER)
        '        pathParam.Append(path(1).Selection)
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
        '    End If
        '    If (Marker.UseTableList AndAlso path.Count = 2) Then
        '        linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
        '    End If
        '    lnkPath1.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
        'End If

        'If path.Count > 2 Then
        '    If Not menu Is Nothing Then
        '        nodeId = pHandler.Combine(nodeId, path(2))
        '        menu = Marker.GetMenu(nodeId)
        '        If menu Is Nothing Then
        '            Return Nothing
        '        End If

        '        lnkPath2.Text = menu.Text
        '        lnkPath2.Visible = True
        '    End If
        '    linkItems = New List(Of LinkManager.LinkItem)
        '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(2).Selection)))
        '    Else
        '        pathParam.Append(PathHandler.NODE_DIVIDER)
        '        pathParam.Append(path(2).Selection)
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
        '    End If
        '    If (Marker.UseTableList AndAlso path.Count = 3) Then
        '        linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
        '    End If

        '    lnkPath2.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
        '    lblSep3.Visible = True
        'End If

        'If path.Count > 3 Then
        '    nodeId = pHandler.Combine(nodeId, path(3))
        '    menu = Marker.GetMenu(nodeId)
        '    If Not menu Is Nothing Then
        '        If menu Is Nothing Then
        '            Return Nothing
        '        End If

        '        lnkPath3.Text = menu.Text
        '        lnkPath3.Visible = True
        '    End If
        '    linkItems = New List(Of LinkManager.LinkItem)
        '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(3).Selection)))
        '    Else
        '        pathParam.Append(PathHandler.NODE_DIVIDER)
        '        pathParam.Append(path(3).Selection)
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
        '    End If
        '    If (Marker.UseTableList) Then
        '        linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
        '    End If
        '    lnkPath3.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
        '    lblSep4.Visible = True
        'End If

        'If path.Count > 4 Then
        '    If Not menu Is Nothing Then
        '        nodeId = pHandler.Combine(nodeId, path(4))
        '        menu = Marker.GetMenu(nodeId)
        '        If menu Is Nothing Then
        '            Return Nothing
        '        End If

        '        lnkPath4.Text = menu.Text
        '        lnkPath4.Visible = True
        '    End If
        '    linkItems = New List(Of LinkManager.LinkItem)
        '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(4).Selection)))
        '    Else
        '        pathParam.Append(PathHandler.NODE_DIVIDER)
        '        pathParam.Append(path(4).Selection)
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
        '    End If
        '    If (Marker.UseTableList) Then
        '        linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
        '    End If
        '    lnkPath4.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
        '    lblSep5.Visible = True
        'End If

        'If path.Count > 5 Then
        '    If Not menu Is Nothing Then
        '        nodeId = pHandler.Combine(nodeId, path(5))
        '        menu = Marker.GetMenu(nodeId)
        '        If menu Is Nothing Then
        '            Return Nothing
        '        End If

        '        lnkPath5.Text = menu.Text
        '        lnkPath5.Visible = True
        '    End If
        '    linkItems = New List(Of LinkManager.LinkItem)
        '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(path(5).Selection)))
        '    Else
        '        pathParam.Append(PathHandler.NODE_DIVIDER)
        '        pathParam.Append(path(5).Selection)
        '        linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, UrlPath(pathParam.ToString())))
        '    End If
        '    If (Marker.UseTableList) Then
        '        linkItems.Add(New LinkManager.LinkItem("tablelist", "true"))
        '    End If
        '    lnkPath5.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())
        '    lblSep6.Visible = True
        'End If

        Return menu
    End Function

    ''' <summary>
    ''' If BreadcrumbMode is menu and TablePath exists the last part of the path shall not be clickable
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FixMenuPath()
        If lnkPath5.Visible Then
            lnkPath5.NavigateUrl = ""
            lnkPath5.CssClass = "breadcrumb_text_nolink"
        ElseIf lnkPath4.Visible Then
            lnkPath4.NavigateUrl = ""
            lnkPath4.CssClass = "breadcrumb_text_nolink"
        ElseIf lnkPath3.Visible Then
            lnkPath3.NavigateUrl = ""
            lnkPath3.CssClass = "breadcrumb_text_nolink"
        ElseIf lnkPath2.Visible Then
            lnkPath2.NavigateUrl = ""
            lnkPath2.CssClass = "breadcrumb_text_nolink"
        ElseIf lnkPath1.Visible Then
            lnkPath1.NavigateUrl = ""
            lnkPath1.CssClass = "breadcrumb_text_nolink"
        End If
    End Sub

    ''' <summary>
    ''' Return path to table in the base format
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UrlPath(ByVal path As String) As String
        If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
            path = path.Replace(PxPathHandler.DIVIDER_STRING, PathHandler.NODE_DIVIDER)
        End If
        Return path
    End Function

    ''' <summary>
    ''' Set up the homepage link
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHomepageLink()
        If Marker.HomePageImage Then
            imgHome.ImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif")
            imgHome.AlternateText = Marker.HomePageName
            imgHome.ToolTip = Marker.HomePageName
        Else
            imgHome.Visible = False
            lnkHome.Text = Marker.HomePageName
            lnkHome.CssClass = "breadcrumb_text"
        End If

        If Marker.HomePageIsExternal Then
            lnkHome.NavigateUrl = Marker.HomePage
        Else
            lnkHome.NavigateUrl = LinkManager.CreateLink(Marker.HomePage)
        End If

    End Sub
#End Region

End Class