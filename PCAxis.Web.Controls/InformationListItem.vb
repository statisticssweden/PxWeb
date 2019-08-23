Public Class InformationListItem
    Private _InformationType As Enums.InformationType
    Private _Definition As String
    Private _InformationVariables As New List(Of InformationVariable)

    Public Sub New(ByVal informationType As Enums.InformationType)
        Me.InformationType = informationType
    End Sub

    Public Property InformationType() As Enums.InformationType
        Get
            Return _InformationType
        End Get
        Set(ByVal value As Enums.InformationType)
            _InformationType = value
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

    Public Property InformationVariables() As List(Of InformationVariable)
        Get
            Return _InformationVariables
        End Get
        Set(ByVal value As List(Of InformationVariable))
            _InformationVariables = value
        End Set
    End Property
End Class
