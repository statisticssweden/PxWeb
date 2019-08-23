Namespace PCAxis.Paxiom.Operations
    Public Class SumTable
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
            If Not TypeOf rhs Is SumTableDescription Then
                Throw New Exception("Parameter not suported")
            End If

            Return Execute(lhs, CType(rhs, SumTableDescription))

        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As SumTableDescription) As PCAxis.Paxiom.PXModel
            Dim sumModel As PXModel
            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data
            Dim variableCodesToChangeValueOrder As String()
            Dim newVar As Variable
            Dim value1 As Value
            Dim value2 As Value
            Dim value3 As Value

            If rhs.SumModel Is Nothing And rhs.CalculateWithConstant = False Then
                Throw New PXOperationException("Invalid operand data.")
            End If

            ' If exclude and use constant we do not create a new variable
            ' Otherwize we validate variable/value data
            If (rhs.CalculateWithConstant And rhs.KeepValues = False) = False Then
                If rhs.NewVariable Is Nothing Then
                    Throw New PXOperationException("Variable name is missing.")
                End If
                If rhs.NewValue1Code Is Nothing Or rhs.NewValue1Name Is Nothing Then
                    Throw New PXOperationException("New Variable Value 1 Code or Name is missing.")
                End If
                If rhs.NewValue2Code Is Nothing Or rhs.NewValue2Name Is Nothing Then
                    Throw New PXOperationException("New Variable Value 2 Code or Name is missing.")
                End If
            End If

            sumModel = rhs.SumModel

            If sumModel IsNot Nothing Then
                ' Verify that we have the same variables and values in both models
                variableCodesToChangeValueOrder = _operationsHelper.EnsureSameVariablesAndValues(oldModel, sumModel)

                ' Change value order for any variable code present in the array
                sumModel = _operationsHelper.ChangeValueOrders(oldModel, sumModel, variableCodesToChangeValueOrder)

                ' Pivot the model to match oldmodel
                sumModel = _operationsHelper.PivotModelToMatch(oldModel, sumModel)
            End If

            ' If we are calculating with a constant and are not to keep the original
            ' we should not create a new variable.
            If rhs.KeepValues Then
                ' Create new Variable and its values
                newVar = New Variable(rhs.NewVariable, PlacementType.Stub)

                value1 = New Value(rhs.NewValue1Name)
                PaxiomUtil.SetCode(value1, rhs.NewValue1Code)
                newVar.Values.Add(value1)

                value2 = New Value(rhs.NewValue2Name)
                PaxiomUtil.SetCode(value2, rhs.NewValue2Code)

                newVar.Values.Add(value2)

                ' Add third value if we are to keep the original table and we are not working with a constant value
                If rhs.CalculateWithConstant = False Then
                    value3 = New Value(rhs.NewValue3Name)
                    PaxiomUtil.SetCode(value3, rhs.NewValue3Code)
                    newVar.Values.Add(value3)
                End If

                ' Insert new Variable first
                newMeta.InsertVariable(0, newVar)
            Else
                ' Set UNITS
                If String.IsNullOrEmpty(rhs.Unit) Then
                    Throw New PXOperationException("Unit was expected but missing.")
                End If

                If newMeta.ContentInfo IsNot Nothing Then
                    newMeta.ContentInfo.Units = rhs.Unit
                End If
            End If

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            ' Loop oldModel and write data for the new values
            Dim tmpRow As Integer
            Dim originalValue As Double = 0
            Dim sumModelValue As Double = 0
            Dim calculatedValue As Double = 0
            For rowIndex As Integer = 0 To oldModel.Data.MatrixRowCount - 1
                For columnIndex As Integer = 0 To oldModel.Data.MatrixColumnCount - 1
                    ' Read original value
                    originalValue = oldModel.Data.ReadElement(rowIndex, columnIndex)

                    ' Write the original data?
                    If rhs.KeepValues Then
                        ' Value1
                        newData.WriteElement(rowIndex, columnIndex, originalValue)
                    End If

                    ' Write the selected table data?
                    If sumModel IsNot Nothing Then
                        sumModelValue = sumModel.Data.ReadElement(rowIndex, columnIndex)

                        If rhs.KeepValues Then
                            ' Value2
                            tmpRow = rowIndex + oldModel.Data.MatrixRowCount
                            newData.WriteElement(tmpRow, columnIndex, sumModelValue)
                        End If

                        calculatedValue = _operationsHelper.CalculateValue(originalValue, sumModelValue, rhs.Operation, oldModel)
                    Else
                        ' Work with a constant value
                        calculatedValue = _operationsHelper.CalculateValue(originalValue, rhs.ConstantValue, rhs.Operation, oldModel)
                    End If

                    ' Where do we write the calculated value?
                    If rhs.KeepValues Then
                        If sumModel IsNot Nothing Then
                            ' Value3
                            tmpRow = rowIndex + (oldModel.Data.MatrixRowCount * 2)
                        Else
                            ' Value2
                            ' We are working with a constant
                            tmpRow = rowIndex + oldModel.Data.MatrixRowCount
                        End If
                    Else
                        ' Value1
                        tmpRow = rowIndex
                    End If

                    ' Write the calculated value
                    newData.WriteElement(tmpRow, columnIndex, calculatedValue)
                Next
            Next

            ' Create new model, its title and set as complete.
            'newModel = New PXModel(newMeta, newData)
            newModel.Meta.Prune()
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel
        End Function
    End Class
End Namespace