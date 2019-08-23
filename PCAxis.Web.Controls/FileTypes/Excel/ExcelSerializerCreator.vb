Imports PCAxis.Paxiom

Public Class ExcelSerializerCreator
    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As Paxiom.IPXModelStreamSerializer Implements Core.ISerializerCreator.Create
        Dim excelSerializer As New ExcelFileSerializer

        If fileInfo.Equals(Plugins.FileFormats.XLS_DOUBLE_COLUMN) Then
            excelSerializer.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns
        Else
            'Get doublecolumn from application setting
            excelSerializer.DoubleColumn = Settings.Files.DoubleColumnFile
        End If

        excelSerializer.InformationLevel = Settings.Files.CompleteInfoFile

        Return excelSerializer
    End Function
End Class
