Namespace PCAxis.Paxiom.Operations
    Public Class SumGrouping
        Implements PCAxis.Paxiom.IPXOperation

        Private newMeta As PXMeta
        Private indexMatrix()()() As Integer
        Private weightArray() As Integer
        Private originalModel As PXModel
        Private newData As PXData

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is SumGroupingDescription Then
                Throw New PXOperationException("Parameter not supported")
            End If

            Return Execute(lhs, CType(rhs, SumGroupingDescription))
        End Function

        ''' <summary>
        ''' Execute the grouping operation
        ''' </summary>
        ''' <param name="oldModel">PXModel object containg the "micro" data</param>
        ''' <param name="rhs">Grouping description object</param>
        ''' <returns>PXModel object that reflects the state after the grouping operation has been executed</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As SumGroupingDescription) As PCAxis.Paxiom.PXModel

            originalModel = oldModel
            ValidateGrouping(oldModel, rhs)

            newMeta = CreateMeta(oldModel, rhs)
            newData = CreateData(newMeta)
            indexMatrix = CreateIndexMatrix(oldModel, newMeta, rhs)
            weightArray = CreateWeightArray(oldModel)

            Dim cellIndex As Integer = 0
            SetData(0, New Integer(newMeta.Variables.Count - 1) {}, cellIndex)

            Dim model As New PXModel(newMeta, newData)
            Return model
        End Function

        Sub SetData(ByVal currentIndex As Integer, ByVal variableArr As Integer(), ByRef cellIndex As Integer)
            variableArr(currentIndex) = 0
            For index As Integer = 0 To newMeta.Variables(currentIndex).Values.Count - 1
                If currentIndex < newMeta.Variables.Count - 1 Then
                    SetData(currentIndex + 1, variableArr, cellIndex)
                Else
                    SetCellValue(variableArr, cellIndex)
                    cellIndex += 1
                End If
                variableArr(currentIndex) += 1
            Next
        End Sub

        Sub SetCellValue(ByVal variableValueIndexArr As Integer(), ByVal cellIndex As Integer)
            Dim data As Double = 0
            AggregateCellValue(variableValueIndexArr, 0, 0, data)
            newData.WriteElement(cellIndex, data)
        End Sub

        ''' <summary>
        ''' Aggregates all values for a specific cell in the new data
        ''' </summary>
        ''' <param name="variableValueIndexArr">Array of indecies for vurrent value per variable</param>
        ''' <param name="currentVariableIndex">Index of the current variable</param>
        ''' <param name="indexSum">sum of the index for all variables</param>
        ''' <param name="aggValue">The aggregated value</param>
        ''' <remarks></remarks>
        Sub AggregateCellValue(ByVal variableValueIndexArr As Integer(), ByVal currentVariableIndex As Integer, ByVal indexSum As Integer, ByRef aggValue As Double)
            Dim valueIndex As Integer = variableValueIndexArr(currentVariableIndex)
            Dim valueArray As Integer() = indexMatrix(currentVariableIndex)(valueIndex)
            If currentVariableIndex = variableValueIndexArr.Length - 1 Then
                'We got all values an can calculate the index for the data in original model
                For i As Integer = 0 To valueArray.Length - 1
                    If valueArray(i) = -1 Then
                        Continue For
                    End If
                    Dim dataIndex As Integer = indexSum + valueArray(i) * weightArray(currentVariableIndex)
                    aggValue = PXData.AddElements(aggValue, originalModel.Data.ReadElement(dataIndex))
                Next
            Else
                For i As Integer = 0 To valueArray.Length - 1
                    If valueArray(i) = -1 Then
                        Continue For
                    End If
                    AggregateCellValue(variableValueIndexArr, currentVariableIndex + 1, indexSum + valueArray(i) * weightArray(currentVariableIndex), aggValue)
                Next
            End If
        End Sub


        ''' <summary>
        ''' Create PXMeta object. Values will be taken from the grouped variables
        ''' </summary>
        ''' <param name="oldModel">PXModel object</param>
        ''' <param name="rhs">Grouping description object</param>
        ''' <returns>The created PXMeta object</returns>
        ''' <remarks></remarks>
        Private Function CreateMeta(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As SumGroupingDescription) As PXMeta
            Dim newMeta As PXMeta = oldModel.Meta.CreateCopy()

            'Create meta with values from grouped variables
            For Each groupVar In rhs.GroupVariables
                Dim newVar As Variable = newMeta.Variables.GetByCode(groupVar.Code)

                newVar.Values.Clear()
                For Each Val As Value In groupVar.Values
                    newVar.Values.Add(Val.CreateCopy())
                Next
            Next

            Return newMeta
        End Function

        ''' <summary>
        ''' Create the data part of the model
        ''' </summary>
        ''' <param name="newMeta">PXMeta object</param>
        ''' <returns>The created PXData object</returns>
        ''' <remarks></remarks>
        Private Function CreateData(ByVal newMeta As PXMeta) As PXData
            Dim newData As PXData = New PXData()

            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            Return newData
        End Function

        ''' <summary>
        ''' Create matix containg information about which values that participates in a variable value.
        ''' If it is a grouped more than one value may participate to a resulting value.
        ''' </summary>
        ''' <param name="oldModel">PXModel object</param>
        ''' <param name="newMeta">PXMeta object</param>
        ''' <param name="rhs">Grouping description object</param>
        ''' <returns>The created matrix</returns>
        ''' <remarks></remarks>
        Private Function CreateIndexMatrix(ByVal oldModel As PXModel, ByVal newMeta As PXMeta, ByVal rhs As SumGroupingDescription) As Integer()()()
            'Create array with one entry per variable
            Dim varArr(newMeta.Variables.Count - 1)()() As Integer

            For i As Integer = 0 To newMeta.Variables.Count - 1
                Dim var As Variable = rhs.GroupVariables.GetByCode(newMeta.Variables(i).Code)

                'Create array with one entry per variable value
                Dim valArr(newMeta.Variables(i).Values.Count - 1)() As Integer

                'Create array with one entry for each participating value for the (grouped) value 
                For j As Integer = 0 To newMeta.Variables(i).Values.Count - 1

                    If var Is Nothing Or (Not var Is Nothing AndAlso var.CurrentGrouping.GroupPres = GroupingIncludesType.SingleValues) Then
                        'This is not a grouped variable - each value has only one participating value
                        Dim partArr(0) As Integer
                        partArr(0) = j
                        valArr(j) = partArr
                    Else
                        'This is a grouped value - create array with one entry per participating value
                        Dim index As Integer = j
                        Dim group As Group = var.CurrentGrouping.Groups.SingleOrDefault(Function(g) g.GroupCode = var.Values(index).Code)
                        If group Is Nothing OrElse group.ChildCodes.Count = 0 Then
                            Throw New PXOperationException("TODO: Fix this")
                        End If
                        Dim partArr(group.ChildCodes.Count - 1) As Integer
                        For k As Integer = 0 To group.ChildCodes.Count - 1
                            If Not oldModel.Meta.Variables(i).Values.IsCodesFictional Then
                                partArr(k) = oldModel.Meta.Variables(i).Values.GetIndexByCode(group.ChildCodes(k).Code)
                            Else
                                partArr(k) = oldModel.Meta.Variables(i).Values.GetIndexByName(group.ChildCodes(k).Code)
                            End If
                        Next
                        valArr(j) = partArr
                    End If
                Next
                varArr(i) = valArr
            Next

            Return varArr
        End Function

        ''' <summary>
        ''' Create array containg weights for how often the variables are repeated within the data
        ''' </summary>
        ''' <param name="oldModel">PXModel object</param>
        ''' <returns>Array containing the weights</returns>
        ''' <remarks></remarks>
        Private Function CreateWeightArray(ByVal oldModel As PCAxis.Paxiom.PXModel) As Integer()
            Dim rowSize As Integer = oldModel.Meta.GetRowLength()
            Dim weightArr(oldModel.Meta.Variables.Count - 1) As Integer

            For i As Integer = 0 To oldModel.Meta.Stub.Count - 1
                weightArr(i) = rowSize
                For j As Integer = i + 1 To oldModel.Meta.Stub.Count - 1
                    weightArr(i) *= oldModel.Meta.Stub(j).Values.Count
                Next
            Next
            For i As Integer = 0 To oldModel.Meta.Heading.Count - 1
                weightArr(i + oldModel.Meta.Stub.Count) = 1
                For j As Integer = i + 1 To oldModel.Meta.Heading.Count - 1
                    weightArr(i + oldModel.Meta.Stub.Count) *= oldModel.Meta.Heading(j).Values.Count
                Next
            Next

            Return weightArr
        End Function

        ''' <summary>
        ''' Validate that it is possible to perform this grouping
        ''' </summary>
        ''' <param name="model">PXModel object</param>
        ''' <param name="rhs">Grouping description</param>
        ''' <remarks></remarks>
        Private Sub ValidateGrouping(ByVal model As PCAxis.Paxiom.PXModel, ByVal rhs As SumGroupingDescription)
            Dim groupVariable As Variable

            ' Ensure that we have the same number of KeepValue information as we have grouped Variables
            If rhs.GroupVariables.Count <> rhs.KeepValues.Count Then
                Throw New PXOperationException("Number of GroupVariables and number of KeepValue parameters differ.")
            End If

            ' Check that all grouping variables has values - grouping values does not neccessary match any values in the model
            For i As Integer = 0 To model.Meta.Variables.Count - 1
                If model.Meta.Variables(i).Values.Count = 0 Then
                    ' No Values in a Variable will cause problems...
                    groupVariable = rhs.GroupVariables.GetByCode(model.Meta.Variables(i).Code)
                    If groupVariable IsNot Nothing AndAlso groupVariable.CurrentGrouping IsNot Nothing Then
                        Throw New PXOperationException("Selected grouping for " + groupVariable.Name + " does not have any matching values in the PXModel")
                    Else
                        Throw New PXOperationException("Variable " + groupVariable.Name + " has no Values")
                    End If
                End If
            Next
        End Sub

    End Class
End Namespace