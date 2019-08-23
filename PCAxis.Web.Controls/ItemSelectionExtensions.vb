Public Module ItemSelectionExtansions

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function AsString(ByVal obj As PCAxis.Menu.ItemSelection) As String
        Return ItemSelectionHelper.AsString(obj)
    End Function

End Module


Public Class ItemSelectionHelper
    Public Shared Function CreateFromString(str As String) As PCAxis.Menu.ItemSelection
        If str Is Nothing Then
            Return New PCAxis.Menu.ItemSelection()
        End If

        Dim index As Integer = str.IndexOf("#"c)
        Return New PCAxis.Menu.ItemSelection(str.Substring(0, index), str.Substring(index + 1))

    End Function

    Public Shared Function AsString(ByVal obj As PCAxis.Menu.ItemSelection) As String
        Return String.Format("{0}#{1}", obj.Menu, obj.Selection)
    End Function
End Class