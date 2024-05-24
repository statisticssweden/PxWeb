Imports PCAxis.Web.Core



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Text
    Inherits FileTypeMarkerControlBase(Of TextCodebehind, Text)

    Public Overrides Sub SerializeAndStream()
        'Using stream As IO.MemoryStream = New IO.MemoryStream()
        '    Dim textSerializer As New TextFileSerializer
        '    textSerializer.RowLength = CInt(LineLengthValue.Text)
        '    textSerializer.PageLength = CInt(PageLengthValue.Text)
        '    textSerializer.Margin = CInt(MarginValue.Text)
        '    textSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
        '    StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        'End Using
    End Sub

End Class
