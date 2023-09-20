Imports PCAxis.Web.Core

Public Class Csv3FileTypeCodebehind
    Inherits FileTypeControlBase(Of Csv3FileTypeCodebehind, Csv3FileType)

    Private Sub Csv3FiltType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

End Class
