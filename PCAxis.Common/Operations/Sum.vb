Namespace PCAxis.Paxiom.Operations
    Public Class Sum
        Implements PCAxis.Paxiom.IPXOperation

        Private _operationsHelper As New OperationsHelper()
        Private _PMatrixHelper As PMatrixHelper

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is SumDescription Then
                Throw New Exception("Parameter not suported")
            End If

            Return Execute(lhs, CType(rhs, SumDescription))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As SumDescription) As PCAxis.Paxiom.PXModel
            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data

            ' Get sum variable from old and new meta
            Dim oldSumVar As Variable = oldModel.Meta.Variables.GetByCode(rhs.VariableCode)
            Dim newsumVar As Variable = newMeta.Variables.GetByCode(rhs.VariableCode)

            ' Check if we have a valid selection in the SumDescription
            Dim check As String = IsSumDescriptionValidForOperation(rhs, oldSumVar)
            If check.Length > 0 Then
                Throw New PXOperationException(check)
            End If

            ' If we should not keepValues, remove them from newSumVar
            If rhs.KeepValues = False Then
                For i As Integer = (newsumVar.Values.Count - 1) To 0 Step -1
                    'If the value is in the sum
                    If rhs.ValueCodes.Contains(newsumVar.Values(i).Code) Then
                        ' Remove Value
                        newsumVar.Values.RemoveAt(i)
                    End If
                Next
            End If

            ' Validate elimination info
            If rhs.KeepValues = False Then
                If newsumVar.EliminationValue Is Nothing Then
                    'If no EliminationValue exists all values must be selected
                    newsumVar.Elimination = False
                Else
                    If newsumVar.Values.GetByName(newsumVar.EliminationValue.Value) Is Nothing Then
                        'If EliminationValue exists that value must be selected
                        newsumVar.Elimination = False
                        newsumVar.EliminationValue = Nothing
                    End If
                End If
            Else
                If newsumVar.Elimination And newsumVar.EliminationValue Is Nothing Then
                    newsumVar.Elimination = False
                End If
            End If

            ' Create the sum value and add to Variable from new Meta
            Dim newValue As New Value(rhs.NewValueName, newModel.Meta.NumberOfLanguages - 1)
            PaxiomUtil.SetCode(newValue, rhs.NewValueCode)

            ' Set precision for division or if we multiply with decimal constan
            If rhs.Operation = SumOperationType.Division Then
                newValue.Precision = 2
            ElseIf (rhs.CalculateWithConstant And _operationsHelper.GetNumberOfDecimals(rhs.ConstantValue) > 0) Then
                newValue.Precision = _operationsHelper.GetNumberOfDecimals(rhs.ConstantValue)
            End If

            ' Make sure that the new valuecode is not already in the variable
            If newsumVar.Values.GetByCode(newValue.Code) IsNot Nothing Then
                Throw New PXOperationException("The new value code was not unique")
            End If

            If rhs.Operation = SumOperationType.Grouping And rhs.GroupedVariableIndex > -1 Then
                newsumVar.Values.Insert(rhs.GroupedVariableIndex, newValue)
            Else
                newsumVar.Values.Insert(_operationsHelper.SumVariableCalculateSumValueIndex(oldSumVar, rhs.ValueCodes, rhs.KeepValues), newValue)
            End If

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            ' Init pmatrix helper
            _PMatrixHelper = New PMatrixHelper(oldModel)

            ' Initialize for use with sum functionality
            _PMatrixHelper.SumVariableInit(oldSumVar, rhs.ValueCodes, rhs.KeepValues, newMeta)

            ' Transfer data to newData
            _PMatrixHelper.SumVariableTransferData(newData, newsumVar, rhs)

            'Set precision on new value to same as highest precision in the sumvariables
            Dim precision As Integer = 0
            Dim newVal As Paxiom.Value = newsumVar.Values.GetByCode(rhs.NewValueCode)
            If Not IsNothing(newVal) Then
                For Each sumValue As Paxiom.Value In newsumVar.Values
                    If sumValue.Precision > precision Then
                        precision = sumValue.Precision
                    End If
                Next
                newVal.Precision = precision
            End If



            ' If the sum funktion was for all values + to exclude original values we should remove the sumvariable
            ' This is indicated in the DoEliminationForSumAll parameter.
            If rhs.DoEliminationForSumAll And newsumVar.Values.Count = 1 Then
                ' Remove sum variable
                newMeta.RemoveVariable(newsumVar)
            End If

            'Remove hierarchy if present
            If newsumVar.Hierarchy.IsHierarchy Then
                newsumVar.Hierarchy.Clear()
            End If

            ' Create model to return
            newModel.Meta.Prune()
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel

        End Function

        ''' <summary>
        ''' Check if the SumDescription comply to the rules for using the Sum operation
        ''' </summary>
        ''' <param name="rhs"></param>
        ''' <returns>string to be used as Exception message</returns>
        ''' <remarks>Several error messages are separated with vbCrLf</remarks>
        Public Function IsSumDescriptionValidForOperation(ByVal rhs As SumDescription, ByVal oldSumVar As Variable) As String
            Dim returnValue As String = String.Empty

            For Each val As Value In oldSumVar.Values
                If Not rhs.Operation = SumOperationType.Grouping Then
                    If val.Text = rhs.NewValueName Then
                        returnValue += "Value name already exists."
                        Exit For
                    End If
                End If
                If val.Code = rhs.NewValueCode Then
                    returnValue += "Value code already exists."
                    Exit For
                End If
            Next

            If rhs.ValueCodes.Count = 0 Then
                If returnValue.Length > 0 Then returnValue += vbCrLf
                returnValue += "Select one value together with a Constant OR select two values"
            ElseIf rhs.ValueCodes.Count = 1 Then
                If rhs.Operation <> SumOperationType.Addition And rhs.Operation <> SumOperationType.Grouping Then
                    ' There should be one value and calculate with constant
                    If Not rhs.CalculateWithConstant Then
                        If returnValue.Length > 0 Then returnValue += vbCrLf
                        returnValue += "Select one value and set the Constant OR select two values"
                    End If
                End If
            Else ' Two or more values selected
                ' Only addition is possible with more than two values
                If rhs.ValueCodes.Count > 2 Then
                    If rhs.Operation <> SumOperationType.Addition And rhs.Operation <> SumOperationType.Grouping Then
                        If returnValue.Length > 0 Then returnValue += vbCrLf
                        returnValue += "This operationType supports a maximum of two values to operate on."
                    End If
                End If

                If rhs.CalculateWithConstant Then
                    If returnValue.Length > 0 Then returnValue += vbCrLf
                    returnValue += "Only one value is allowed to be selected together with a constant."
                End If
            End If

            Return returnValue
        End Function
    End Class

End Namespace
