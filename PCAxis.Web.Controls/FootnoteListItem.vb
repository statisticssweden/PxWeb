Public Class FootnoteListItem
    Private _FootnoteType As Enums.FootnoteType
    Private _Term As String
    Private _Definition As String
    Private _Mandatory As Boolean
    Private _Header As Boolean

    Public Sub New(ByVal footnoteType As Enums.FootnoteType)
        Me.FootnoteType = footnoteType
    End Sub

    Public Property FootnoteType() As Enums.FootnoteType
        Get
            Return _FootnoteType
        End Get
        Set(ByVal value As Enums.FootnoteType)
            _FootnoteType = value
        End Set
    End Property

    Public Property Term() As String
        Get
            Return _Term
        End Get
        Set(ByVal value As String)
            _Term = value
        End Set
    End Property

    Public Property Definition() As String
        Get
            Return _Definition
        End Get
        Set(ByVal value As String)
            _Definition = value
        End Set
    End Property

    Public Property Mandatory() As Boolean
        Get
            Return _Mandatory
        End Get
        Set(ByVal value As Boolean)
            _Mandatory = value
        End Set
    End Property

    Public Property Header() As Boolean
        Get
            Return _Header
        End Get
        Set(ByVal value As Boolean)
            _Header = value
        End Set
    End Property
End Class
