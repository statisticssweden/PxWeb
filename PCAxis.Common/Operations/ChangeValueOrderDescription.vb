Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class for supporting the ChangeValueOrder operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeValueOrderDescription
        Public VariableCode As String
        Public ModifiedVariableValueWeight As Integer()

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor initializing the VariableCode and ModifiedVariableValueWeight parameters
        ''' </summary>
        ''' <param name="variableCode"></param>
        ''' <param name="modifiedVariableValueWeight"></param>
        ''' <remarks>
        ''' variableCode --> Code for variable to change value order for
        ''' modifiedVariableValueWeight --> array holding new index positions for the values
        ''' Ex.
        ''' valueArray(0) = "region1"
        ''' valueArray(1) = "region2"
        ''' If value order then is changed to:
        ''' valueArray(0) = "region2"
        ''' valueArray(1) = "region1"
        ''' The modifiedVariableValueWeight array should look like this:
        ''' modifiedVariableValueWeight(0) = 1
        ''' modifiedVariableValueWeight(1) = 0
        ''' ie, the new index position for the first value is 1 and 0 for the second
        ''' </remarks>
        Public Sub New(ByVal variableCode As String, ByVal modifiedVariableValueWeight As Integer())
            Me.VariableCode = variableCode
            Me.ModifiedVariableValueWeight = modifiedVariableValueWeight
        End Sub
    End Class

End Namespace
