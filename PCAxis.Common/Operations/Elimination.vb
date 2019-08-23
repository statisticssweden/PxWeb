Namespace PCAxis.Paxiom.Operations

    Public Class Elimination
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is EliminationDescription() Then
                Throw New PXOperationException("Paramater not suported")
            End If

            Return Execute(lhs, CType(rhs, EliminationDescription()))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="eliminations"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PXModel, ByVal eliminations As EliminationDescription()) As PXModel
            Dim newModel As PXModel = oldModel.CreateCopy()
            Dim newMeta As PXMeta = newModel.Meta
            Dim newData As PXData = oldModel.Data.CreateCopy()
            Dim selectionList As New List(Of Selection)
            Dim selection As Selection
            Dim variable As Variable

            PaxiomUtil.SetData(newModel, newData)

            'Delete values so the new PXData has the right matrix size
            For Each elim As EliminationDescription In eliminations
                If elim.EliminationByValue Then
                    variable = newModel.Meta.Variables.GetByCode(elim.VariableCode)
                    If Not variable Is Nothing AndAlso Not variable.EliminationValue Is Nothing Then
                        'Create a selection with all the values that shall be deleted (all values except the eliminationvalue)
                        selection = New Selection(variable.Code)
                        For Each value As Value In variable.Values
                            If Not value.Equals(variable.EliminationValue) Then
                                selection.ValueCodes.Add(value.Code)
                            End If
                        Next
                        selectionList.Add(selection)
                    End If
                End If
            Next

            'Delete the values
            If selectionList.Count > 0 Then
                Dim delval As New Operations.DeleteValue
                newModel = delval.Execute(newModel, selectionList.ToArray)
            End If

            'Remove all variables with elimination value
            For i As Integer = 0 To eliminations.Length - 1
                If eliminations(i).EliminationByValue Then
                    Dim v As Variable = newModel.Meta.Variables.GetByCode(eliminations(i).VariableCode)
                    'If variable is a content variable it is already deleted in delval.Execute because contentvariables
                    'with only one value always is removed.
                    If Not v Is Nothing Then
                        newMeta.RemoveVariable(v)
                    End If

                End If
            Next

            'Remove all variables with no elimination value
            Dim func As Sum
            Dim sumDesc As SumDescription
            For i As Integer = 0 To eliminations.Length - 1
                If Not eliminations(i).EliminationByValue Then
                    Dim v As Variable = newModel.Meta.Variables.GetByCode(eliminations(i).VariableCode)
                    sumDesc = CreateSumDescription(v)
                    func = New Sum()
                    newModel = func.Execute(newModel, sumDesc)
                End If
            Next


            For Each ei As EliminationDescription In eliminations
                Dim v As Variable
                v = newModel.Meta.Variables.GetByCode(ei.VariableCode)
                'Removes the variable from the collections if not already removed
                If v IsNot Nothing Then
                    newModel.Meta.RemoveVariable(v)
                End If
            Next

            ' Call Prune to validate CellNotes/DataNoteCells ...
            newModel.Meta.Prune()

            newModel.Meta.CreateTitle()

            Return newModel
        End Function



        Private Shared Function CreateSumDescription(ByVal variable As Variable) As SumDescription
            Dim sd As New SumDescription
            sd.VariableCode = variable.Code
            sd.NewValueCode = "?"
            sd.NewValueName = "?"
            sd.KeepValues = False
            sd.Operation = SumOperationType.Addition

            For i As Integer = 0 To variable.Values.Count - 1
                sd.ValueCodes.Add(variable.Values(i).Code)
            Next

            Return sd
        End Function
    End Class

End Namespace
