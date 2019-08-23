Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for handling PivotTimeToHeading Operation. After the operation has been executed the time variable will be in 
    ''' the heading and all other variables in the stub
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PivotTimeToHeading
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(lhs As PXModel, rhs As Object) As PXModel Implements IPXOperation.Execute
            Dim piv As PCAxis.Paxiom.Operations.Pivot = New PCAxis.Paxiom.Operations.Pivot()

            Dim q = From v In lhs.Meta.Variables _
                Select New PivotDescription(v.Name, CType(IIf(v.IsTime, PlacementType.Heading, PlacementType.Stub), PlacementType))

            Return piv.Execute(lhs, q.ToArray())

        End Function
    End Class

End Namespace
