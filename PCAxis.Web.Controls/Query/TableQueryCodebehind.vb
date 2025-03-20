

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management
Imports PCAxis.Query
Imports System.IO
Imports System.Web

<ToolboxData("<{0}:TableQuery runat=""server""></{0}:TableQuery>")>
Public Class TableQueryCodebehind
    Inherits PaxiomControlBase(Of TableQueryCodebehind, TableQuery)

    Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(TableQueryCodebehind))

#Region "fields"
    Protected pnlQueryInformation As Panel
    Protected pnlQueryInformationV2 As Panel
    Protected lblInformationText As Label
    Protected lblInformationTextV2 As Label
    Protected lblUrl As Label
    Protected lblUrlV2 As Label
    Protected txtUrl As TextBox
    Protected txtUrlV2 As TextBox
    Protected lblQuery As Label
    Protected txtQuery As TextBox
    Protected lnkMoreInfo As HyperLink
    Protected WithEvents btnSaveQuery As Button
    Protected lblTableQueryInformation As Label
    Protected btnCopyUrlV2 As Button
#End Region

#Region "Localized strings"
    Private Const LOC_TABLEQUERY_SHOW_INFORMATION As String = "CtrlTableQueryShowInformation"
    Private Const LOC_TABLEQUERY_INFORMATION_TEXT As String = "CtrlTableQueryInformationText"
    Private Const LOC_TABLEQUERY_V2_INFORMATION_TEXT As String = "CtrlTableQueryV2InformationText"
    Private Const LOC_TABLEQUERY_URL_CAPTION As String = "CtrlTableQueryUrlCaption"
    Private Const LOC_TABLEQUERY_QUERY_CAPTION As String = "CtrlTableQueryQueryCaption"
    Private Const LOC_TABLEQUERY_MORE_INFORMATION As String = "CtrlTableQueryMoreInformation"
    Private Const LOC_TABLEQUERY_SAVE_QUERY As String = "CtrlTableQuerySaveQuery"
    Private Const LOC_TABLEQUERY_COPY As String = "CtrlTableQueryCopy"
#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If VerifyDatabase() Then
                Me.Visible = True
                ShowApiURL()
                ShowApiV2URL()
                ShowApiQuery()
                ShowMoreInfoLink()
                Localize()
                btnSaveQuery.Visible = Marker.ShowSaveApiQueryButton
                pnlQueryInformationV2.Visible = Marker.EnableApiV2QueryLink
            Else
                Me.Visible = False
            End If
        End If
    End Sub

    Private Sub Page_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        If Me.Visible Then
            Localize()
            ShowApiURL()
            ShowApiV2URL()
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
        lblInformationText.Text = GetLocalizedString(LOC_TABLEQUERY_INFORMATION_TEXT)
        lblInformationTextV2.Text = GetLocalizedString(LOC_TABLEQUERY_V2_INFORMATION_TEXT)
        lblUrl.Text = GetLocalizedString(LOC_TABLEQUERY_URL_CAPTION)
        lblUrlV2.Text = GetLocalizedString(LOC_TABLEQUERY_URL_CAPTION)
        lblQuery.Text = GetLocalizedString(LOC_TABLEQUERY_QUERY_CAPTION)

        lnkMoreInfo.Text = String.Format("<span class=link-text>{0}</span>", Server.HtmlEncode(GetLocalizedString(LOC_TABLEQUERY_MORE_INFORMATION)))
        btnSaveQuery.Text = GetLocalizedString(LOC_TABLEQUERY_SAVE_QUERY)
        lblTableQueryInformation.Text = GetLocalizedString(LOC_TABLEQUERY_SHOW_INFORMATION)
        btnCopyUrlV2.Text = GetLocalizedString(LOC_TABLEQUERY_COPY)
    End Sub

    ''' <summary>
    ''' Show the API URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowApiURL()
        Dim sb As New System.Text.StringBuilder()

        If Context Is Nothing Then
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

    ''' <summary>
    ''' Show the API-V2 URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowApiV2URL()
        Dim sb As New System.Text.StringBuilder()
        Dim route As String
        If Not Marker.URLRootV2 Is Nothing Then
            route = Marker.URLRootV2.Replace("\", "/")
            sb.Append(route)
            If Not route.EndsWith("/") Then
                sb.Append("/")
            End If
        End If

        sb.Append("tables/")

        Dim model As PXModel = PaxiomManager.PaxiomModel

        If model Is Nothing Then
            txtUrlV2.Text = ""
            Return
        End If

        Dim tableId As String = model.Meta.TableID

        'If PX file the Matrix should be used as tabled identifier
        If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
            tableId = model.Meta.Matrix
        End If

        sb.Append(tableId)

        sb.Append("/data")
        Dim lang As String = LocalizationManager.CurrentCulture.Name
        lang = Util.GetLanguageForNet3_5(lang)
        sb.Append("?lang=")
        sb.Append(lang)

        If PaxiomManager.SingleContentSelection IsNot Nothing AndAlso PaxiomManager.SingleContentSelection.Count > 0 Then
            sb.Append($"&valueCodes[{PaxiomManager.SingleContentSelection.First().Key}]={PaxiomManager.SingleContentSelection.First().Value}")
        End If

        For Each var In model.Meta.Variables
            Dim values As String = GetValuesString(var.Values)
            'If var.CurrentValueSet IsNot Nothing Then
            '    sb.Append(String.Format("&codelist[{0}]={1}", var.Code, var.CurrentValueSet.ID))
            'End If
            'If var.CurrentGrouping IsNot Nothing Then
            '    sb.Append(String.Format("&outputValues[{0}]={1}", var.Code, "aggregated"))
            'End If
            sb.Append(String.Format("&valueCodes[{0}]={1}", var.Code, values))
        Next

        For Each var In PaxiomManager.QueryModel.Query
            Dim filter As String = var.Selection.Filter
            Dim prefix As String = filter.Split(":"c).First()
            Dim suffix As String = filter.Split(":"c).Last()
            If prefix.Equals("vs") Or prefix.Equals("agg") Then
                sb.Append(String.Format("&codelist[{0}]={1}_{2}", var.Code, prefix, suffix))
            End If
            If prefix.Equals("agg") Then
                sb.Append(String.Format("&outputValues[{0}]={1}", var.Code, "aggregated"))
            End If
        Next

        txtUrlV2.Text = sb.ToString()
    End Sub
    Private Function GetValuesString(var As Values) As String
        Dim valuesSB As New System.Text.StringBuilder()
        For Each val As Value In var
            valuesSB.Append(HttpUtility.UrlEncode(val.Code))
            valuesSB.Append(",")
        Next
        valuesSB.Remove(valuesSB.Length - 1, 1)
        Return valuesSB.ToString()
    End Function

    Private Function GetAppPath() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim scheme As String = context.Request.Url.Scheme
        Dim host As String = context.Request.Url.Host
        Dim port As Integer = context.Request.Url.Port
        Dim applicationPath As String = context.Request.ApplicationPath

        Dim portPart As String = If((port = 80 And scheme = "http") Or (port = 443 And scheme = "https"), String.Empty, $":{port}")

        Return $"{scheme}://{host}{portPart}{applicationPath}"
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
                Me.lnkMoreInfo.Target = "_blank"
                Me.lnkMoreInfo.NavigateUrl = Marker.MoreInfoURL
            Else
                Me.lnkMoreInfo.NavigateUrl = LinkManager.CreateLink(Marker.MoreInfoURL)
            End If
        End If
    End Sub

    Private Sub BtnSaveQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveQuery.Click
        Try
            Dim postUrl = txtUrl.Text.Trim()

            If (postUrl.EndsWith("/")) Then
                postUrl = postUrl.Remove(postUrl.Length - 1)
            End If

            Dim apiQuery = txtQuery.Text
            Dim tableId = postUrl.Split({"/"c}).Last()
            Dim jsonString = String.Concat("{""queryObj"":", apiQuery, ",""tableIdForQuery"":""", tableId, """}")

            Using stream = GenerateStreamFromString(jsonString)
                Response.Clear()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", CreateHeaderValue(tableId))
                stream.CopyTo(Response.OutputStream)
                Response.Flush()
                Response.SuppressContent = True
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        Catch ex As Exception
            Logger.Error(ex.Message)
        End Try
    End Sub

    Private Function CreateHeaderValue(ByVal tableId As String) As String
        Dim firstPartOfFilename As String

        If Not String.IsNullOrEmpty(Marker.SaveApiQueryText) Then
            firstPartOfFilename = Marker.SaveApiQueryText
        Else
            firstPartOfFilename = "px-web"
        End If

        Return String.Format("attachment; filename=""{0}api_table_{1}.json"";", firstPartOfFilename, tableId)
    End Function

    Private Function GenerateStreamFromString(ByVal queryString As String) As Stream
        Dim memoryStream As New MemoryStream()

        Try
            Dim writer As New StreamWriter(memoryStream)
            writer.Write(queryString)
            writer.Flush()
            memoryStream.Position = 0
            Return memoryStream
        Catch ex As Exception
            memoryStream.Dispose()
            Throw ex
        End Try
    End Function
#End Region

End Class
