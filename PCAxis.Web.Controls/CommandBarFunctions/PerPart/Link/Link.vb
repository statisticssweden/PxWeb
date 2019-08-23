Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

Public Class Link
    Implements ICommandBarNoGUIPlugin
    Private Const PROPERTY_LINK As String = "Link"
    Private Const PROPERTY_TRANSFORM As String = "Transform"


    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        If System.Web.HttpContext.Current IsNot Nothing AndAlso properties.ContainsKey(PROPERTY_LINK) Then
            If properties.ContainsKey(PROPERTY_TRANSFORM) AndAlso properties(PROPERTY_TRANSFORM).ToLower() = "true" Then
                'Copy all properties (except "Link" and "Transform") to querystring
                Dim linkItems As New List(Of LinkManager.LinkItem)
                For Each kvp As KeyValuePair(Of String, String) In properties
                    If Not kvp.Key.Equals(PROPERTY_LINK) AndAlso Not kvp.Key.Equals(PROPERTY_TRANSFORM) Then
                        linkItems.Add(New LinkManager.LinkItem(kvp.Key, kvp.Value))
                    End If
                Next
                System.Web.HttpContext.Current.Response.Redirect(Core.Management.LinkManager.CreateLink(properties(PROPERTY_LINK), linkItems.ToArray))
            Else
                System.Web.HttpContext.Current.Response.Redirect(properties(PROPERTY_LINK))
            End If
        End If
    End Sub
End Class
