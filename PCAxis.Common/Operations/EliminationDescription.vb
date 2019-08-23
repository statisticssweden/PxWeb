Namespace PCAxis.Paxiom.Operations

    Public Class EliminationDescription

        Private _variableCode As String
        Public ReadOnly Property VariableCode() As String
            Get
                Return _variableCode
            End Get
        End Property

        Private _eliminationByValue As Boolean
        Public ReadOnly Property EliminationByValue() As Boolean
            Get
                Return _eliminationByValue
            End Get
        End Property

        Public Sub New(ByVal variabelCode As String, ByVal eliminationByValue As Boolean)
            _variableCode = variabelCode
            _eliminationByValue = eliminationByValue
        End Sub

    End Class

End Namespace
