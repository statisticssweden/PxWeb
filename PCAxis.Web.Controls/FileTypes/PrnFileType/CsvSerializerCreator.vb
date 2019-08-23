Public Class CsvSerializerCreator
    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As Paxiom.IPXModelStreamSerializer Implements Core.ISerializerCreator.Create

        Dim prnSerializer As New PCAxis.Paxiom.CsvFileSerializer()

        prnSerializer.ThousandSeparator = False
        prnSerializer.DecimalSeparator = "."c

        Select Case fileInfo
            Case Plugins.FileFormats.CSV_TABULATOR_WITH_HEADING
                prnSerializer.Title = True
                'prnSerializer.CanWriteDataOnly = False
                prnSerializer.Delimiter = CChar(vbTab)
            Case Plugins.FileFormats.CSV_TABULATOR_WITHOUT_HEADING
                prnSerializer.Title = False
                'prnSerializer.CanWriteDataOnly = True
                prnSerializer.Delimiter = CChar(vbTab)
            Case Plugins.FileFormats.CSV_COMMA_WITH_HEADING
                prnSerializer.Title = True
                'prnSerializer.CanWriteDataOnly = False
                prnSerializer.Delimiter = CChar(",")
            Case Plugins.FileFormats.CSV_COMMA_WITHOUT_HEADING
                prnSerializer.Title = False
                'prnSerializer.CanWriteDataOnly = True
                prnSerializer.Delimiter = CChar(",")
            Case Plugins.FileFormats.CSV_SPACE_WITH_HEADING
                prnSerializer.Title = True
                'prnSerializer.CanWriteDataOnly = False
                prnSerializer.Delimiter = CChar(" ")
            Case Plugins.FileFormats.CSV_SPACE_WITHOUT_HEADING
                prnSerializer.Title = False
                'prnSerializer.CanWriteDataOnly = True
                prnSerializer.Delimiter = CChar(" ")
            Case Plugins.FileFormats.CSV_SEMICOLON_WITH_HEADING
                prnSerializer.Title = True
                'prnSerializer.CanWriteDataOnly = False
                prnSerializer.Delimiter = CChar(";")
            Case Plugins.FileFormats.CSV_SEMICOLON_WITHOUT_HEADING
                prnSerializer.Title = False
                'prnSerializer.CanWriteDataOnly = True
                prnSerializer.Delimiter = CChar(";")
        End Select

        Return prnSerializer

    End Function
End Class
