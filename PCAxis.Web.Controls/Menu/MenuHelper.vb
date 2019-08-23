'Imports System.Globalization

'Public NotInheritable Class MenuHelper

'    Private _menu As PxMenu.PxMenuBase = Nothing

'    Private Sub New()

'    End Sub

'    Public Sub New(ByVal menu As PxMenu.PxMenuBase)
'        _menu = menu
'    End Sub

'    Public Function GetRelativePath(ByVal link As PxMenu.Link) As String
'        Dim path = String.Concat((From breadCrumb In link.Breadcrumbs _
'                Where Not breadCrumb.Equals(_menu.RootItem) _
'                Select breadCrumb.Description.Trim + "/").ToArray())

'        Return String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", PCAxis.Web.Controls.Configuration.ConfigurationHelper.MenuSection.PXVirtualRoot, path, link.Selection)
'    End Function
'    Public Function GetRelativePath(ByVal selection As String) As String
'        Dim path As String = String.Empty

'        If _menu.SetCurrentItemBySelection(selection) AndAlso TypeOf (_menu.CurrentItem) Is PxMenu.Link Then
'            Return GetRelativePath(CType(_menu.CurrentItem, PxMenu.Link))
'        End If

'        Return Nothing
'    End Function


'    Public Function GetFullPath(ByVal selection As String) As String
'        Try
'            Return System.Web.HttpContext.Current.Request.MapPath(GetRelativePath(selection))
'        Catch ex As Exception

'        End Try

'        Return Nothing
'    End Function

'    Public Function GetFullPath(ByVal link As PxMenu.Link) As String

'        Try
'            Return System.Web.HttpContext.Current.Request.MapPath(GetRelativePath(link))
'        Catch ex As Exception

'        End Try

'        Return Nothing

'    End Function


'End Class
