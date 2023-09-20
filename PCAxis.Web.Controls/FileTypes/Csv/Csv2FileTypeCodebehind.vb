Imports PCAxis.Web.Core


Public Class Csv2FileTypeCodebehind
    Inherits FileTypeControlBase(Of Csv2FileTypeCodebehind, Csv2FileType)

    Private Sub Csv2FileType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

End Class
