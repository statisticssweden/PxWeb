Imports PCAxis.Menu

''' <summary>
''' Base class for handling Menu item id:s and paths
''' </summary>
''' <remarks></remarks>
Public MustInherit Class PathHandler
    Implements IPathHandler

    ''' <summary>
    ''' Divider string for the parts in the id/path
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NODE_DIVIDER As String = "__"

    Public MustOverride Function Combine(ByVal parentId As String, ByVal item As Menu.ItemSelection) As String Implements IPathHandler.Combine
    Public MustOverride Function GetPath(ByVal itemId As String) As System.Collections.Generic.List(Of Menu.ItemSelection) Implements IPathHandler.GetPath
    Public MustOverride Function GetSelection(ByVal itemId As String) As Menu.ItemSelection Implements IPathHandler.GetSelection
    Public MustOverride Function GetPathString(ByVal menuItem As PxMenuItem) As String Implements IPathHandler.GetPathString
    Public MustOverride Function GetPathString(ByVal menuItem As ItemSelection) As String Implements IPathHandler.GetPathString
    Public MustOverride Function GetTable(ByVal item As ItemSelection) As String Implements IPathHandler.GetTable
    Public MustOverride Function CombineTable(ByVal db As String, ByVal path As String, ByVal table As String) As String Implements IPathHandler.CombineTable

End Class
