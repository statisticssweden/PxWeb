Imports PCAxis.Web.Controls.Table
Imports PCAxis.Web.Core.StateProvider
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Management

Public Class SortedTableView
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequireControls
    Private Const KEY_COMMANDBAR_SORT_TRANSFORMATION As String = "CommandBarSortTransformation"

    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        Dim strPage As String
        Dim linkItems As New List(Of LinkManager.LinkItem)

        ' Find web page
        strPage = Configuration.ConfigurationHelper.GetViewPage(Configuration.ConfigurationHelper.CONFIG_VIEW_SORTEDTABLE)

        If String.IsNullOrEmpty(strPage) Then
            strPage = "DataSort.aspx"
        End If

        'strPage = Core.Management.LinkManager.CreateLink(strPage)

        'System.Web.HttpContext.Current.Response.Redirect(strPage)

        ' Create URL and redirect
        If (properties.Count > 0) Then
            For Each kvp As KeyValuePair(Of String, String) In properties
                linkItems.Add(New LinkManager.LinkItem(kvp.Key, kvp.Value))
            Next

            System.Web.HttpContext.Current.Response.Redirect(Core.Management.LinkManager.CreateLink(strPage, linkItems.ToArray))
        Else
            System.Web.HttpContext.Current.Response.Redirect(Core.Management.LinkManager.CreateLink(strPage))
        End If

    End Sub

    ''' <summary>
    ''' Checks if the Browser supports JavaScript. Note that the function returns True if we have discabled Javascript
    ''' but Javascript is supported by the browser. 
    ''' A better solution could be to use the JavascriptTester control but the drawback is a postback in the load event.    
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsJavascriptEnabled() As Boolean
        Return System.Web.HttpContext.Current.Request.Browser.EcmaScriptVersion.Major >= 1
    End Function


    ''' <summary>
    ''' Finds the Table control on the current page recursively 
    ''' </summary>
    ''' <param name="controls">The controlcollection to search through</param>
    ''' <returns>The Table control if found, otherwise Nothing(Null)</returns>
    ''' <remarks></remarks>
    Private Function FindTableControl(ByVal controls As System.Web.UI.ControlCollection) As Table
        Dim table As Table = Nothing
        For Each control As System.Web.UI.Control In controls
            If TypeOf (control) Is Table Then
                table = CType(control, Table)
                Exit For
            End If
            table = FindTableControl(control.Controls)
            If table IsNot Nothing Then
                Exit For
            End If
        Next
        Return table
    End Function

    Private _pageControls As System.Web.UI.ControlCollection
    Public Property PageControls() As System.Web.UI.ControlCollection Implements ICommandBarRequireControls.PageControls
        Get
            Return _pageControls
        End Get
        Set(ByVal value As System.Web.UI.ControlCollection)
            _pageControls = value
        End Set
    End Property
End Class
