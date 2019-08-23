

Imports PCAxis.Web.Core
Imports PCAxis.Paxiom
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management
Public Class TextCodebehind
    Inherits FileTypeControlBase(Of TextCodebehind, Text)

    Protected WithEvents LineLengthDescription As Label
    Protected WithEvents LineLengthValue As TextBox
    Protected WithEvents PageLengthDescription As Label
    Protected WithEvents PageLengthValue As TextBox
    Protected WithEvents MarginDescription As Label
    Protected WithEvents MarginValue As TextBox
    Protected WithEvents ContinueButton As Button

    'Public Overrides Sub SerializeAndStream()
    '    Using stream As IO.MemoryStream = New IO.MemoryStream()
    '        Dim textSerializer As New TextFileSerializer
    '        textSerializer.RowLength = CInt(LineLengthValue.Text)
    '        textSerializer.PageLength = CInt(PageLengthValue.Text)
    '        textSerializer.Margin = CInt(MarginValue.Text)
    '        textSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
    '        StreamFile(stream, Marker.SelectedFileType.MimeType, Marker.SelectedFileType.FileExtension)
    '    End Using
    'End Sub

    Private Sub Text_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LineLengthDescription.Text = GetLocalizedString("CtrlSaveAsLineLengthControl")
        PageLengthDescription.Text = GetLocalizedString("CtrlSaveAsPageLengthControl")
        MarginDescription.Text = GetLocalizedString("CtrlSaveAsMarginControl")
        ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton")        
    End Sub

    Private Sub continue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        'SerializeAndStream()
        OnFinished()
        Marker.SerializeAndStream()
    End Sub
End Class
