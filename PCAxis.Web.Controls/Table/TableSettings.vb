Public Class TableSettings
    Private _zeroOption As PCAxis.Paxiom.ZeroOptionType
    Public Property ZeroOption() As PCAxis.Paxiom.ZeroOptionType
        Get
            Return _zeroOption
        End Get
        Set(ByVal value As PCAxis.Paxiom.ZeroOptionType)
            _zeroOption = value
        End Set
    End Property

    Sub New()
        _zeroOption = Paxiom.ZeroOptionType.ShowAll
    End Sub


End Class
