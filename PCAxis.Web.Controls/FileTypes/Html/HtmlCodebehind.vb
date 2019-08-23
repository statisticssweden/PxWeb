Imports PCAxis.Web.Core
Imports PCAxis.Paxiom
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management

Public Class HtmlCodebehind
    Inherits FileTypeControlBase(Of HtmlCodebehind, Html)

    Private Sub Html_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

End Class
