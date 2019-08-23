Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for handling ChangeDecimals Operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeDecimals
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(lhs As PXModel, rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is ChangeDecimalsDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, ChangeDecimalsDescription))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(oldModel As PXModel, rhs As ChangeDecimalsDescription) As PXModel
            Dim newMeta As PCAxis.Paxiom.PXMeta = oldModel.Meta.CreateCopy()
            Dim newData As PCAxis.Paxiom.PXData = oldModel.Data.CreateCopy()
            Dim newModel As PCAxis.Paxiom.PXModel = New PCAxis.Paxiom.PXModel(newMeta, newData)

            newModel.Meta.Decimals = rhs.Decimals
            newModel.Meta.ShowDecimals = rhs.Decimals

            Return newModel
        End Function

    End Class

End Namespace
