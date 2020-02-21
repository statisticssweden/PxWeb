''' <summary>
''' Class for handling Menu item id:s and paths for PX-file databases
''' </summary>
''' <remarks></remarks>
Public Class PxPathHandler
    Inherits PathHandler

    Public Const DIVIDER_STRING As String = "\"

    Public Overrides Function Combine(ByVal parentId As String, ByVal item As Menu.ItemSelection) As String
        Dim index As Integer
        Dim result As New System.Text.StringBuilder()
        Dim selection As String

        If Not String.IsNullOrEmpty(parentId) Then
            result.Append(parentId)
            result.Append(NODE_DIVIDER)
        End If

        selection = item.Selection

        index = selection.LastIndexOf(DIVIDER_STRING)

        If index > -1 Then
            If selection.Length > index Then
                result.Append(selection.Substring(index + 1))
            End If
        Else
            result.Append(selection)
        End If

        Return result.ToString()
    End Function

    Public Overrides Function GetPath(ByVal itemId As String) As System.Collections.Generic.List(Of Menu.ItemSelection)
        Dim lst As New List(Of Menu.ItemSelection)
        Dim parts() As String
        Dim separator() As String = {NODE_DIVIDER}
        Dim item As Menu.ItemSelection
        Dim id As New System.Text.StringBuilder
        Dim first As Boolean = True

        If Not String.IsNullOrEmpty(itemId) Then
            parts = itemId.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)

            If parts.Length > 0 Then
                For Each part As String In parts
                    If Not first Then
                        id.Append(NODE_DIVIDER)
                    End If

                    id.Append(part)
                    item = GetSelection(id.ToString())

                    If Not item Is Nothing Then
                        lst.Add(item)
                    End If

                    first = False
                Next
            End If
        End If

        Return lst
    End Function

    Public Overrides Function GetSelection(ByVal itemId As String) As Menu.ItemSelection
        Dim item As New Menu.ItemSelection
        Dim index As Integer

        If String.IsNullOrEmpty(itemId) Then
            Return item
        End If

        'Clean ItemId of slashes
        itemId = itemId.Replace("/", NODE_DIVIDER)
        itemId = itemId.Replace("\", NODE_DIVIDER)

        item.Menu = ""
        item.Selection = ""

        index = itemId.LastIndexOf(NODE_DIVIDER)

        If index > -1 Then
            If index > 0 Then
                item.Menu = itemId.Substring(0, index)
            End If

            item.Menu = item.Menu.Replace(NODE_DIVIDER, DIVIDER_STRING)
            item.Selection = itemId.Replace(NODE_DIVIDER, DIVIDER_STRING)
        Else
            item.Menu = itemId
        End If

        Return item
    End Function

    Public Overrides Function GetPathString(ByVal menuItem As Menu.PxMenuItem) As String
        Return Combine(menuItem.ID.Menu.Replace(DIVIDER_STRING, PathHandler.NODE_DIVIDER), menuItem.ID)
    End Function

    Public Overrides Function GetPathString(node As Menu.ItemSelection) As String
        If Not node Is Nothing Then
            Return node.Selection.Replace("\", PathHandler.NODE_DIVIDER)
        Else
            Return ""
        End If
    End Function


    Public Overrides Function GetTable(ByVal item As Menu.ItemSelection) As String
        Dim index As Integer = item.Selection.LastIndexOf(DIVIDER_STRING)

        If index > -1 AndAlso (item.Selection.Length > index) Then
            Return item.Selection.Substring(index + 1)
        Else
            Return item.Selection
        End If
    End Function

    Public Overrides Function CombineTable(ByVal db As String, ByVal path As String, ByVal table As String) As String
        Dim tablePath As String

        If path.Equals("-") Then
            tablePath = db & DIVIDER_STRING & table
        Else
            tablePath = path & PathHandler.NODE_DIVIDER & table
            tablePath = tablePath.Replace(PathHandler.NODE_DIVIDER, DIVIDER_STRING)
        End If

        Return tablePath
    End Function

    Public Overrides Function GetNodeIds(itemId As String) As List(Of String)
        Dim nodes As New List(Of String)

        Dim parts() As String
        Dim separator() As String = {NODE_DIVIDER}
        Dim id As New System.Text.StringBuilder
        Dim first As Boolean = True

        If Not String.IsNullOrEmpty(itemId) Then
            parts = itemId.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)

            If parts.Length > 0 Then
                For Each part As String In parts
                    If Not first Then
                        id.Append(NODE_DIVIDER)
                    Else
                        first = False
                    End If

                    id.Append(part)
                    nodes.Add(id.ToString())

                Next
            End If
        End If

        Return nodes

    End Function

End Class
