Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Management



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Relational
    Inherits FileTypeMarkerControlBase(Of RelationalCodebehind, Relational)

    ''' <summary>
    ''' Creates a relational file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            Dim relationalSerializer As New RelationtableFileSerializer
            relationalSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
