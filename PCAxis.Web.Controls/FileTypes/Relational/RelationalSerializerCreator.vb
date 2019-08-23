Public Class RelationalSerializerCreator
    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As Paxiom.IPXModelStreamSerializer Implements Core.ISerializerCreator.Create
        Return New PCAxis.Paxiom.RelationtableFileSerializer()
    End Function

End Class
