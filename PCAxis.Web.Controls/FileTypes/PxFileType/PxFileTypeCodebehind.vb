

Imports PCAxis.Web.Core

Public Class PxFileTypeCodebehind
    Inherits FileTypeControlBase(Of PxFileTypeCodebehind, PxFileType)

    Private Sub PrnFileType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

End Class
