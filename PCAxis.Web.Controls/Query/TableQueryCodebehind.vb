

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

<ToolboxData("<{0}:TableQuery runat=""server""></{0}:TableQuery>")> _
Public Class TableQueryCodebehind
    Inherits PaxiomControlBase(Of TableQueryCodebehind, TableQuery)

#Region "fields"
    Protected lnkTableQueryInformation As HyperLink
    Protected lnkHideInformation As HyperLink
    Protected pnlQueryInformation As Panel
    Protected pnlQueryInformationHidden As Panel
    Protected lblInformationText As Label
    Protected lblUrl As Label
    Protected txtUrl As TextBox
    Protected lblQuery As Label
    Protected txtQuery As TextBox
    Protected lnkMoreInfo As HyperLink
#End Region

#Region "Localized strings"
    Private Const LOC_TABLEQUERY_SHOW_INFORMATION As String = "CtrlTableQueryShowInformation"
    Private Const LOC_TABLEQUERY_SHOW_INFORMATION_TOOLTIP As String = "CtrlTableQueryShowInformationTooltip"
    Private Const LOC_TABLEQUERY_HIDE_INFORMATION As String = "CtrlTableQueryHideInformation"
    Private Const LOC_TABLEQUERY_INFORMATION_TEXT As String = "CtrlTableQueryInformationText"
    Private Const LOC_TABLEQUERY_URL_CAPTION As String = "CtrlTableQueryUrlCaption"
    Private Const LOC_TABLEQUERY_QUERY_CAPTION As String = "CtrlTableQueryQueryCaption"
    Private Const LOC_TABLEQUERY_MORE_INFORMATION As String = "CtrlTableQueryMoreInformation"
#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If VerifyDatabase() Then
                Me.Visible = True
                ShowApiURL()
                ShowApiQuery()
                ShowMoreInfoLink()
                Localize()
                lnkTableQueryInformation.NavigateUrl = BuildLink(True)
                lnkHideInformation.NavigateUrl = BuildLink(False)
            Else
                Me.Visible = False
            End If
        End If

        SetDisplayMode()
    End Sub

    Private Sub Page_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        If Me.Visible Then
            Localize()
            ShowApiURL()
        End If
    End Sub

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Verifies that the current database is exposed via the API
    ''' </summary>
    ''' <returns>True if the database is exposed via the API, else false</returns>
    ''' <remarks></remarks>
    Private Function VerifyDatabase() As Boolean
        Dim lang As String = LocalizationManager.CurrentCulture.Name

        If lang Is Nothing Then
            Return False
        End If

        lang = PCAxis.Util.GetLanguageForNet3_5(lang)

        If ExposedDatabases.DatabaseConfigurations.ContainsKey(lang) Then
            Dim dict As Dictionary(Of String, DbConfig)

            dict = ExposedDatabases.DatabaseConfigurations(lang)

            If String.IsNullOrEmpty(Marker.Database) Then
                Return False
            End If

            If dict.ContainsKey(Marker.Database) Then
                Return True
            End If
        End If

        Return False
    End Function

    ''' <summary>
    ''' Create URL for the show/hide information link
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDisplayMode()
        If Me.Visible Then
            If Not QuerystringManager.GetQuerystringParameter("showtablequery") Is Nothing Then
                If QuerystringManager.GetQuerystringParameter("showtablequery").Equals("true") Then
                    pnlQueryInformation.Style.Add("display", "inline-block")
                    pnlQueryInformationHidden.Style.Add("display", "none")
                    Exit Sub
                End If
            End If

            pnlQueryInformation.Style.Add("display", "none")
            pnlQueryInformationHidden.Style.Add("display", "inline-block")
        End If
    End Sub

    ''' <summary>
    ''' Build Link for the show/hide link of API Query information
    ''' </summary>
    ''' <param name="show">If the link should hide or show the API Query information</param>
    ''' <returns>The link URL</returns>
    ''' <remarks></remarks>
    Private Function BuildLink(ByVal show As Boolean) As String
        Dim url As New System.Text.StringBuilder
        Dim first As Boolean = True

        url.Append(Request.Url.AbsolutePath)

        For Each key As String In Request.QueryString.AllKeys
            If Not key.Equals("showtablequery") Then
                If first Then
                    url.Append("?")
                    first = False
                Else
                    url.Append("&")
                End If
                url.Append(key & "=")
                url.Append(QuerystringManager.GetQuerystringParameter(key))
            End If
        Next

        If show Then
            If first Then
                url.Append("?showtablequery=true")
            Else
                url.Append("&showtablequery=true")
            End If
        End If

        url.Append("#tablequerycontrol")

        Return url.ToString
    End Function

    ''' <summary>
    ''' Display the web control in the currently selected language
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Localize()
        lnkTableQueryInformation.Text = GetLocalizedString(LOC_TABLEQUERY_SHOW_INFORMATION)
        lnkTableQueryInformation.ToolTip = GetLocalizedString(LOC_TABLEQUERY_SHOW_INFORMATION_TOOLTIP)
        lnkHideInformation.Text = GetLocalizedString(LOC_TABLEQUERY_HIDE_INFORMATION)
        lblInformationText.Text = GetLocalizedString(LOC_TABLEQUERY_INFORMATION_TEXT)
        lblUrl.Text = GetLocalizedString(LOC_TABLEQUERY_URL_CAPTION)
        lblQuery.Text = GetLocalizedString(LOC_TABLEQUERY_QUERY_CAPTION)
        lnkMoreInfo.Text = GetLocalizedString(LOC_TABLEQUERY_MORE_INFORMATION)
    End Sub

    ''' <summary>
    ''' Show the API URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowApiURL()
        Dim sb As New System.Text.StringBuilder()

        If context Is Nothing Then
            Me.Visible = False
            Exit Sub
        End If

        Dim appPath As String = String.Empty

        If Not String.IsNullOrEmpty(Marker.URLRoot) Then
            appPath = Marker.URLRoot
        Else
            appPath = GetAppPath()
        End If

        Dim route As String = ""

        If Marker.ShowRoutePrefix Then

            'route = System.Web.Configuration.WebConfigurationManager.AppSettings("routePrefix")
            route = Marker.RoutePrefix
            If String.IsNullOrEmpty(route) Then
                Me.Visible = False
                Exit Sub
            End If
            route = route.Replace("\", "/")

        End If


        Dim lang As String = LocalizationManager.CurrentCulture.Name
        lang = Util.GetLanguageForNet3_5(lang)

        If String.IsNullOrEmpty(lang) Then
            Me.Visible = False
            Exit Sub
        End If

        sb.Append(appPath)
        If Not appPath.EndsWith("/") Then
            sb.Append("/")
        End If

        If Marker.ShowRoutePrefix Then
            sb.Append(route)
            If Not route.EndsWith("/") Then
                sb.Append("/")
            End If
        End If

        sb.Append(lang)
        sb.Append("/")

        Dim tablePath As String = ""
        Dim tablePathSB As New System.Text.StringBuilder

        If Not Marker.Path Is Nothing Then
            If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
                If Marker.Path.Equals("-") Then
                    tablePathSB.Append(Marker.Database & "/" & Marker.Table)
                Else
                    tablePathSB.Append(Marker.Path & "/" & Marker.Table)
                End If
                tablePath = tablePathSB.ToString().Replace(PathHandler.NODE_DIVIDER, "/")
            Else
                tablePathSB.Append(Marker.Database & "/")
                tablePathSB.Append(Marker.Path.Replace(PathHandler.NODE_DIVIDER, "/") & "/")
                tablePathSB.Append(Marker.Table)
                tablePath = tablePathSB.ToString()
            End If
        End If

        sb.Append(tablePath)

        txtUrl.Text = sb.ToString()
    End Sub

    Private Function GetAppPath() As String
        Dim appPath As String = String.Empty
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current

        appPath = String.Format("{0}://{1}{2}{3}", _
                                    context.Request.Url.Scheme, _
                                    context.Request.Url.Host, _
                                    IIf(context.Request.Url.Port.Equals(80), String.Empty, ":" & context.Request.Url.Port), _
                                    context.Request.ApplicationPath)

        Return appPath
    End Function

    ''' <summary>
    ''' Show the query
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowApiQuery()
        If Not PaxiomManager.QueryModel Is Nothing Then
            txtQuery.Text = JsonHelper.ToJSON(PaxiomManager.QueryModel, True)
        Else
            Me.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Show link with more info
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowMoreInfoLink()
        If String.IsNullOrEmpty(Marker.MoreInfoURL) Then
            Me.lnkMoreInfo.Visible = False
        Else
            If Marker.MoreInfoIsExternalPage Then
                Me.lnkMoreInfo.Target = "_new"
                Me.lnkMoreInfo.NavigateUrl = Marker.MoreInfoURL
            Else
                Me.lnkMoreInfo.NavigateUrl = LinkManager.CreateLink(Marker.MoreInfoURL)
            End If
        End If
    End Sub
#End Region

End Class