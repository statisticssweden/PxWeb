Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

Public Class InformationView
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequireControls

    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        Dim strPage As String
        Dim linkItems As New List(Of LinkManager.LinkItem)

        ' Find web page
        strPage = Configuration.ConfigurationHelper.GetViewPage(Configuration.ConfigurationHelper.CONFIG_VIEW_INFORMATION)

        If String.IsNullOrEmpty(strPage) Then
            strPage = "Information.aspx"
        End If

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
