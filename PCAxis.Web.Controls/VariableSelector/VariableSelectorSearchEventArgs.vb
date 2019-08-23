Public Class VariableSelectorSearchEventArgs
    Inherits EventArgs

    Private _variableCode As String

    Private Sub New()
    End Sub

    Public Sub New(ByVal variableCode As String)
        _variableCode = variableCode
    End Sub

    Public ReadOnly Property VariableCode() As String
        Get
            Return _variableCode
        End Get
    End Property
End Class
