
Namespace PCAxis.Paxiom.Operations

    Public Class CalculateTwoValuesDescription
        Public Enum CalculateTwoValuesOperation As Short
            Subtraction = 0
            Addition = 1
            Division = 2
            Multiplication = 3
        End Enum

        Public VariableCode As String
        Public ValueCodeValue1 As String
        Public ValueCodeValue2 As String
        Public NewValueName As String
        Public NewValueCode As String
        Public KeepValues As Boolean
        Public CalcOperation As CalculateTwoValuesOperation
    End Class

End Namespace

