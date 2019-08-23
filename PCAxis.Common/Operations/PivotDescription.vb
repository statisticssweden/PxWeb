Namespace PCAxis.Paxiom.Operations

    Public Class PivotDescription
        Public VariableName As String
        Public VariablePlacement As PCAxis.Paxiom.PlacementType

        Public Sub New()

        End Sub

        Public Sub New(ByVal variableName As String, ByVal placement As PCAxis.Paxiom.PlacementType)
            Me.VariableName = variableName
            Me.VariablePlacement = placement
        End Sub
    End Class

End Namespace
