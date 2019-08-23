
Namespace PCAxis.Paxiom.Operations

    Public Class SumWithConstantDescription
        Public Enum SumWithConstantOperation As Short
            Subtraction = 0
            Addition = 1
            Division = 2
            Multiplication = 3
        End Enum

        Public VariableCode As String
        Public ValueCodes As New List(Of String)
        Public NewValueName As String
        Public NewValueCode As String
        Public KeepValues As Boolean
        Public ConstantValue As Double
        Public ConstantOperation As SumWithConstantOperation
    End Class

End Namespace
