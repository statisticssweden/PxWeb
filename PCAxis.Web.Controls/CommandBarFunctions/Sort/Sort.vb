'Imports PCAxis.Web.Controls.Table
'Imports PCAxis.Web.Core.StateProvider
'Imports PCAxis.Web.Controls.CommandBar.Plugin
'Imports System.Web.UI.WebControls


'Public Class Sort
'    Implements ICommandBarNoGUIPlugin
'    Implements ICommandBarRequireControls
'    Private Const KEY_COMMANDBAR_SORT_TRANSFORMATION As String = "CommandBarSortTransformation"

'    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute        
'        Dim StrLink As String
'        If (Not Configuration.ConfigurationHelper.SortSection Is Nothing) AndAlso (Not String.IsNullOrEmpty(Configuration.ConfigurationHelper.SortSection.SortPage)) Then
'            StrLink = Core.Management.LinkManager.CreateLink(Configuration.ConfigurationHelper.SortSection.SortPage)
'        Else
'            StrLink = Core.Management.LinkManager.CreateLink("DataSort.aspx")
'        End If

'        'If IsJavascriptEnabled() Then
'        '    ' used if we want to show the popup in the center of the screen... but doesn't handle dual screens yet
'        '    'Dim strOpenWindowScript As String = "function openCenteredWindow(url, width, height) { "
'        '    'strOpenWindowScript += "var left = parseInt((screen.availWidth / 2) - (width / 2));"
'        '    'strOpenWindowScript += "var top = parseInt((screen.availHeight / 2) - (height / 2));"
'        '    'strOpenWindowScript += "var windowFeatures = ""width="" + width + "",height="" + height + "",status,resizable=yes,scrollbars=yes,left="" + left + "",top="" + top + ""screenX="" + left + "",screenY="" + top; "
'        '    'strOpenWindowScript += "window.open(url, '_blank', windowFeatures); }"
'        '    'Dim strScript As String = "<script type='text/javascript'>" + strOpenWindowScript + "  openCenteredWindow('" + StrLink + "', 640, 480);</script>"
'        '    'System.Web.HttpContext.Current.Response.Write(strScript)


'        '    'Ta även bort scriptet som nyss kördes
'        '    'System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>detailedresults=window.open('" + StrLink + "','_blank','width=640,height=480, resizable=yes, scrollbars=yes'); jQuery('script:eq(0)').parent().children(':last').remove(); </script>")

'        '    'Ursprunglig
'        '    'System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>detailedresults=window.open('" + StrLink + "','_blank','width=640,height=480, resizable=yes, scrollbars=yes');</script>")

'        '    'Bättre för då läggs inte Javascriptet högst upp
'        '    'Dim thePage As System.Web.UI.Page = DirectCast(System.Web.HttpContext.Current.Handler, System.Web.UI.Page)
'        '    'thePage.ClientScript.RegisterStartupScript(Me.GetType(), "OpenWin", "<script type='text/javascript'>detailedresults=window.open('" + StrLink + "','_blank','width=640,height=480, resizable=yes, scrollbars=yes');</script>")            


'        '    System.Web.HttpContext.Current.Response.Redirect(StrLink)
'        'Else
'        System.Web.HttpContext.Current.Response.Redirect(StrLink)
'        ' End If

'    End Sub

'    ''' <summary>
'    ''' Checks if the Browser supports JavaScript. Note that the function returns True if we have discabled Javascript
'    ''' but Javascript is supported by the browser. 
'    ''' A better solution could be to use the JavascriptTester control but the drawback is a postback in the load event.    
'    ''' </summary>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Private Function IsJavascriptEnabled() As Boolean
'        Return System.Web.HttpContext.Current.Request.Browser.EcmaScriptVersion.Major >= 1
'    End Function


'    ''' <summary>
'    ''' Finds the Table control on the current page recursively 
'    ''' </summary>
'    ''' <param name="controls">The controlcollection to search through</param>
'    ''' <returns>The Table control if found, otherwise Nothing(Null)</returns>
'    ''' <remarks></remarks>
'    Private Function FindTableControl(ByVal controls As System.Web.UI.ControlCollection) As Table
'        Dim table As Table = Nothing
'        For Each control As System.Web.UI.Control In controls
'            If TypeOf (control) Is Table Then
'                table = CType(control, Table)
'                Exit For
'            End If
'            table = FindTableControl(control.Controls)
'            If table IsNot Nothing Then
'                Exit For
'            End If
'        Next
'        Return table
'    End Function

'    Private _pageControls As System.Web.UI.ControlCollection
'    Public Property PageControls() As System.Web.UI.ControlCollection Implements ICommandBarRequireControls.PageControls
'        Get
'            Return _pageControls
'        End Get
'        Set(ByVal value As System.Web.UI.ControlCollection)
'            _pageControls = value
'        End Set
'    End Property
'End Class
