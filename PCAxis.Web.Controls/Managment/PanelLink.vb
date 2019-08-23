Imports System.Web
Imports PCAxis.Web.Core.Management

''' <summary>
''' Class for handling links to setting panels
''' </summary>
''' <remarks></remarks>
Public Class PanelLink

    ''' <summary>
    ''' Querystring key to determine which setting panel that will be displayed
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DISPLAY_PANEL As String = "displaypanel"

    ''' <summary>
    ''' Method for building links to show/hide i setting panels
    ''' </summary>
    ''' <param name="panelId">Id of panel to show. If empty all setting panels will be closed</param>
    ''' <param name="bookmark">Bookmark to add to the URL. Can be empty if no bookmark is wanted</param>
    ''' <returns>Url</returns>
    ''' <remarks>
    ''' Which setting panel that will be displayed is determined by the 'displaypanel' querystring parameter. If no 'displaypanel' is specified 
    ''' in the querystring no setting panel will be displayed.
    ''' </remarks>
    Public Shared Function BuildLink(ByVal panelId As String, Optional ByVal bookmark As String = "") As String
        Dim url As New System.Text.StringBuilder
        Dim first As Boolean = True

        'Add base URL
        url.Append(HttpContext.Current.Request.Url.AbsolutePath)

        'Add all existing querystring parameters except DISPLAY_PANEL
        For Each key As String In HttpContext.Current.Request.QueryString.AllKeys
            If Not key.Equals(DISPLAY_PANEL) Then
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

        'Add DISPLAY_PANEL to the querystring parameters
        If Not String.IsNullOrEmpty(panelId) Then
            If first Then
                url.Append("?")
            Else
                url.Append("&")
            End If

            url.Append(DISPLAY_PANEL + "=" + panelId)
        End If

        'Add bookmark
        If Not String.IsNullOrEmpty(bookmark) Then
            url.Append("#" + bookmark)
        End If

        Return url.ToString
    End Function

End Class
