Imports PCAxis.Paxiom
Imports PCAxis.Web.Core

Public Class Csv2FileSerializerCreator

    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As IPXModelStreamSerializer Implements ISerializerCreator.Create
        Return New PCAxis.Paxiom.Csv2FileSerializer()
    End Function
End Class
