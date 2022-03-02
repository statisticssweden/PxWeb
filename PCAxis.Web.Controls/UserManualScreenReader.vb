Public Class UserManualScreenReader
    Inherits System.Web.UI.WebControls.Literal

    Private _manualFor As String
    Private _headerCode As String
    Private _textCode As String

    Public Property manualForm() As String
        Get
            Return _manualFor
        End Get
        Set(ByVal value As String)
            _manualFor = value
        End Set
    End Property

    Public Property headerCode() As String
        Get
            Return _headerCode
        End Get
        Set(ByVal value As String)
            _headerCode = value
        End Set
    End Property

    Public Property textCode() As String
        Get
            Return _textCode
        End Get
        Set(ByVal value As String)
            _textCode = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        Me.Text = "<span>Hello</span>"


    End Sub

    Protected Sub The_Load() Handles Me.Load
        Localize()
    End Sub


    Private Sub Localize()
        Dim headerKey As String
        Dim textKey As String

        If _manualFor Is Nothing Then
            headerKey = _headerCode
            textKey = _textCode
        Else
            headerKey = String.Format("UserManualScreenReader_{0}_Header", _manualFor)
            textKey = String.Format("UserManualScreenReader_{0}_Text", _manualFor)
        End If

        Dim heading = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(headerKey)
        Dim longText = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(textKey)

        Dim builder As New System.Text.StringBuilder
        builder.Append("<section aria-label=""")

        builder.Append(Page.Server.HtmlEncode(heading))
        builder.Append("""><span class=""screenreader-only"">")
        builder.Append(Page.Server.HtmlEncode(longText))
        builder.Append("</span></section>")
        Me.Text = builder.ToString
    End Sub

End Class
