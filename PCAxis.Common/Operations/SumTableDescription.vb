Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class supporting the SumTable operations
    ''' </summary>
    ''' <remarks>
    ''' If KeepValues = false the operand table will be removed.
    ''' </remarks>
    Public Class SumTableDescription
        Public SumModel As PXModel
        Public NewVariable As String
        Public NewValue1Name As String
        Public NewValue1Code As String
        Public NewValue2Name As String
        Public NewValue2Code As String
        Public NewValue3Name As String
        Public NewValue3Code As String
        Public KeepValues As Boolean
        Public ConstantValue As Double
        Public CalculateWithConstant As Boolean
        Public Operation As SumOperationType
        Public Unit As String
    End Class
End Namespace
