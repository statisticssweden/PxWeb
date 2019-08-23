Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class supporting the Sum operations
    ''' </summary>
    ''' <remarks>
    ''' Usage:
    ''' For the VariableCode: Perform Operation on selected ValueCodes and place the Sum in a new value created from NewValueName/NewValueCode
    ''' If KeepValues = false the operand values will be removed.
    ''' </remarks>
    Public Class SumDescription
        Public VariableCode As String
        Public ValueCodes As New List(Of String)
        Public NewValueName As String
        Public NewValueCode As String
        Public KeepValues As Boolean
        Public ConstantValue As Double
        Public CalculateWithConstant As Boolean
        Public Operation As SumOperationType
        Public GroupedVariableIndex As Integer = 0
        Public DoEliminationForSumAll As Boolean = False ' If true the sumvariable will be removed if the result is only one value
    End Class

End Namespace
