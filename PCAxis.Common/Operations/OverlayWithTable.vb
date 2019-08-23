Namespace PCAxis.Paxiom.Operations
    Public Class OverlayWithTable
        Implements PCAxis.Paxiom.IPXOperation

        Private _operationsHelper As New OperationsHelper()

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is OverlayWithTableDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, OverlayWithTableDescription))
        End Function

        ''' <summary>
        ''' Execute function handling the typecasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As OverlayWithTableDescription) As PCAxis.Paxiom.PXModel
            Dim overlayModel As PXModel

            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data

            Dim variableCodesToChangeValueOrder As String()

            If rhs.OverlayModel Is Nothing Then
                Throw New PXOperationException("Overlay Model is missing.")
            End If
            If rhs.OverlayVariable Is Nothing Then
                Throw New PXOperationException("Overlay Variable is missing.")
            End If
            If rhs.OverlayCode1 Is Nothing Or rhs.OverlayCode2 Is Nothing Then
                Throw New PXOperationException("Overlay variable Code is missing.")
            End If
            If rhs.OverlayValue1 Is Nothing Or rhs.OverlayValue2 Is Nothing Then
                Throw New PXOperationException("Overlay variable Value is missing.")
            End If

            ' Codes and values should be different
            If (rhs.OverlayCode1 = rhs.OverlayCode2) Or (rhs.OverlayValue1 = rhs.OverlayValue2) Then
                Throw New PXOperationException("The new Values cannot have the same codes or values.")
            End If

            overlayModel = rhs.OverlayModel

            ' Verify that we have the same variables and values in both models
            variableCodesToChangeValueOrder = _operationsHelper.EnsureSameVariablesAndValues(oldModel, overlayModel)

            ' Change value order for any variable code present in the array
            overlayModel = _operationsHelper.ChangeValueOrders(oldModel, overlayModel, variableCodesToChangeValueOrder)
            
            ' Pivot the overlaymodel to match oldmodel
            overlayModel = _operationsHelper.PivotModelToMatch(oldModel, overlayModel)

            REM Insert new variable in meta - first in Stub
            Dim newVar As New Variable(rhs.OverlayVariable, PlacementType.Stub)
            Dim value1 As New Value(rhs.OverlayValue1)
            Dim value2 As New Value(rhs.OverlayValue2)
            PaxiomUtil.SetCode(value1, rhs.OverlayCode1)
            PaxiomUtil.SetCode(value2, rhs.OverlayCode2)
            newVar.Values.Add(value1)
            newVar.Values.Add(value2)
            newMeta.InsertVariable(0, newVar)

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            ' Insert data from oldModel in new matrix
            Dim newMatrixRowIndex As Integer = 0
            For rowIndex As Integer = 0 To oldModel.Data.MatrixRowCount - 1
                For columnIndex As Integer = 0 To oldModel.Data.MatrixColumnCount - 1
                    newData.WriteElement(newMatrixRowIndex, columnIndex, oldModel.Data.ReadElement(rowIndex, columnIndex))
                Next
                newMatrixRowIndex += 1
            Next
            ' And now insert data from the overlay model
            For rowIndex As Integer = 0 To overlayModel.Data.MatrixRowCount - 1
                For columnIndex As Integer = 0 To overlayModel.Data.MatrixColumnCount - 1
                    newData.WriteElement(newMatrixRowIndex, columnIndex, overlayModel.Data.ReadElement(rowIndex, columnIndex))
                Next
                newMatrixRowIndex += 1
            Next

            ' Create new model, its title and set as complete.
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel
        End Function
    End Class
End Namespace
