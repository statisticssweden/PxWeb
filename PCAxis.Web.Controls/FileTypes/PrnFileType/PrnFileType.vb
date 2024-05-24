Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management

''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class PrnFileType
    Inherits FileTypeMarkerControlBase(Of PrnFileTypeCodebehind, PrnFileType)

    ''' <summary>
    ''' Creates a prn file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            'Dim prnSerializer As New PrnFileSerializer
            Dim prnSerializer As New CsvFileSerializer

            prnSerializer.ThousandSeparator = False

            Select Case Me.SelectedFormat
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

            prnSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
