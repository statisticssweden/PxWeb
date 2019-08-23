Public Class HtmlSerializerCreator
    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As Paxiom.IPXModelStreamSerializer Implements Core.ISerializerCreator.Create
        Return New PCAxis.Paxiom.HtmlFileSerializer()
    End Function
End Class
