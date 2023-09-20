Imports PCAxis.Paxiom
Imports PCAxis.Web.Core

Public Class Csv3FileSerializerCreator

    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As IPXModelStreamSerializer Implements ISerializerCreator.Create
        Return New PCAxis.Paxiom.Csv3FileSerializer()
    End Function
End Class
