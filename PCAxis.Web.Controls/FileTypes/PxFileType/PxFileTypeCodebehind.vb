

Imports PCAxis.Web.Core
Imports PCAxis.Paxiom
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management

Public Class PxFileTypeCodebehind
    Inherits FileTypeControlBase(Of PxFileTypeCodebehind, PxFileType)

    Private Sub PrnFileType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

End Class
