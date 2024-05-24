Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management


''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Html
    Inherits FileTypeMarkerControlBase(Of HtmlCodebehind, Html)

    ''' <summary>
    ''' Creates a html file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            Dim htmlSerializer As New HtmlFileSerializer
            htmlSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
