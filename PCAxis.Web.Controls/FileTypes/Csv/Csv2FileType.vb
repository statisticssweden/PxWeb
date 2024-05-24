Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management


''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Csv2FileType
    Inherits FileTypeMarkerControlBase(Of Csv2FileTypeCodebehind, Csv2FileType)

    ''' <summary>
    ''' Creates a csv2 file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            Dim csv2FileSerializer As New Csv2FileSerializer
            csv2FileSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
