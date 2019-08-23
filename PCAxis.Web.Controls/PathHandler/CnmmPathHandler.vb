''' <summary>
''' Class for handling Menu item id:s and paths for CNMM databases
''' </summary>
''' <remarks></remarks>
Public Class CnmmPathHandler
    Inherits PathHandler

    Public Overrides Function Combine(ByVal parentId As String, ByVal item As Menu.ItemSelection) As String
        Dim result As New System.Text.StringBuilder()

        If Not String.IsNullOrEmpty(parentId) Then
            result.Append(parentId)
            result.Append(NODE_DIVIDER)
        End If

        result.Append(item.Selection)

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

        If String.IsNullOrEmpty(itemId) Then
            Return item
        End If

        Dim separator As String() = {NODE_DIVIDER}
        Dim parts As String() = itemId.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)

        If parts.Length > 1 Then
            item.Menu = parts(parts.Length - 2)
            item.Selection = parts(parts.Length - 1)
        ElseIf parts.Length = 1 Then
            item.Selection = parts(0)
        End If

        Return item
    End Function

    Public Overrides Function GetPathString(ByVal menuItem As Menu.PxMenuItem) As String
        Dim pathString As New System.Text.StringBuilder
        Dim item As Menu.PxMenuItem


        pathString.Append(menuItem.ID.Selection)

        item = menuItem
        While Not item.Parent Is Nothing
            pathString.Insert(0, item.Parent.ID.Selection & PathHandler.NODE_DIVIDER)
            item = CType(item.Parent, Menu.PxMenuItem)
        End While

        If Not pathString.ToString().StartsWith("START") Then
            pathString.Insert(0, "START" & PathHandler.NODE_DIVIDER)
        End If

        Return pathString.ToString()
    End Function

    Public Overrides Function GetPathString(node As Menu.ItemSelection) As String
        If Not node Is Nothing Then
            Dim pathString As New System.Text.StringBuilder

            pathString.Append(node.Menu)
            pathString.Append(PathHandler.NODE_DIVIDER)
            pathString.Append(node.Selection)

            Return pathString.ToString()
        Else
            Return ""
        End If
    End Function

    Public Overrides Function GetTable(ByVal item As Menu.ItemSelection) As String
        Dim index As Integer = item.Selection.IndexOf(":")

        If (index > -1) AndAlso (item.Selection.Length > index) Then
            Return item.Selection.Substring(index + 1)
        End If
        Return item.Selection
    End Function

    Public Overrides Function CombineTable(ByVal db As String, ByVal path As String, ByVal table As String) As String
        If table.IndexOf(":") < 0 Then
            Return db & ":" & table
        Else
            Return table
        End If
    End Function

End Class
