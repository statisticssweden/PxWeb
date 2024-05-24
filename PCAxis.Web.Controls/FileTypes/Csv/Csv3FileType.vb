Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management


''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Csv3FileType
    Inherits FileTypeMarkerControlBase(Of Csv3FileTypeCodebehind, Csv3FileType)

    ''' <summary>
    ''' Creates a csv3 file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            Dim csv3FileSerializer As New Csv3FileSerializer
            csv3FileSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
