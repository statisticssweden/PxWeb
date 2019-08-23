Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class for supporting the CalculatePerPart Operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalculatePerPartDescription
        Public CalculationVariant As CalculatePerPartSelectionType
        Public OperationType As CalculatePerPartType
        ''' <summary>
        ''' Variable/Value selection(s)
        ''' </summary>
        ''' <remarks>The order of the Variables is important! Must be the same as for the model.</remarks>
        Public ValueSelection As Selection()
        Public ValueName As String
        Public KeepValue As Boolean
        'Public UnitVariableValueIndex As Integer = 0
    End Class
End Namespace