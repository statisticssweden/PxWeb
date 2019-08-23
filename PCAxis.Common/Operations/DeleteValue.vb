Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Operation that deletes values from a PXModel object
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DeleteValue
        Implements PCAxis.Paxiom.IPXOperation

#Region "Private members"
        Private _operationsHelper As New OperationsHelper()
        Private _pMatrixHelper As PMatrixHelper
#End Region

#Region "Public methods"
        ''' <summary>
        ''' Executes the Delete Value operation
        ''' </summary>
        ''' <param name="lhs">The PXModel object to delete values from</param>
        ''' <param name="rhs">Object containing the selection matrix</param>
        ''' <returns>A new PXModel object with the selected values deleted</returns>
        ''' <remarks>
        ''' Throws a PXOperationException if the type of the rhs parameter is others than Selection()
        ''' </remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is Selection() Then
                Throw New PXOperationException("Paramater not suported")
            End If

            Return Execute(lhs, CType(rhs, Selection()))
        End Function

        ''' <summary>
        ''' Executes the Delete Value operation
        ''' </summary>
        ''' <param name="oldModel">The PXModel object to delete values from</param>
        ''' <param name="selection">Selection matrix containing the values to delete</param>
        ''' <returns>A new PXModel object with the selected values deleted</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal selection As PCAxis.Paxiom.Selection()) As PCAxis.Paxiom.PXModel
            Dim newModel As PCAxis.Paxiom.PXModel
            Dim newMeta As PCAxis.Paxiom.PXMeta
            Dim newData As PCAxis.Paxiom.PXData
            Dim var As Variable
            Dim val As Value
            Dim s As Selection
            Dim ok As Boolean
            Dim valIndex As Integer
            Dim valOld As Value
            Dim valNew As Value
            Dim row As Integer = 0
            Dim col As Integer = 0
            Dim theValue As Double

            If Not VerifyDelete(selection) Then
                Return oldModel
            End If

            'Copy Meta 
            newMeta = oldModel.Meta.CreateCopy
            For Each variable As Variable In newMeta.Variables
                VerifyVariableDelete(variable, selection)
            Next

            'Delete selected values from Meta
            For index As Integer = 0 To selection.Length - 1
                s = selection(index)
                If s.ValueCodes.Count > 0 Then
                    var = newMeta.Variables.GetByCode(s.VariableCode)
                    For i As Integer = 0 To s.ValueCodes.Count - 1
                        Val = var.Values.GetByCode(s.ValueCodes(i))
                        If Not val Is Nothing Then
                            'Check if it is the elimination value
                            If var.EliminationValue IsNot Nothing Then
                                If var.EliminationValue.Equals(val) Then
                                    var.EliminationValue = Nothing
                                    var.Elimination = False
                                End If
                            Else
                                'If Eliminationvalue is nothing then elimination is always false when
                                'a value Is deleted
                                var.Elimination = False
                            End If
                            'Remove the value
                            var.Values.Remove(val)

                            'Remove hierarchy from variable if present
                            If var.Hierarchy.IsHierarchy Then
                                var.Hierarchy.Clear()
                            End If
                        End If
                    Next
                    'Rebuild the Timevaluestring to match the values
                    var.BuildTimeValuesString()
                End If
            Next

            'Removed this prune because it can remove content variables before we are ready
            'newMeta.Prune()

            'Create matrix for new Data
            newData = _operationsHelper.CreateData(newMeta)

            'InitializeHelpMatrixes(oldModel)
            _pMatrixHelper = New PMatrixHelper(oldModel)

            'Read from old Data, write to new Data
            For rowIndex As Integer = 0 To oldModel.Data.MatrixRowCount - 1
                ok = True
                'Identify variable values for this row
                'IdentifyStubValues(rowIndex, oldModel.Meta.Stub)
                _pMatrixHelper.CalcStubWeights(rowIndex)

                'Verify that all variable values in the Stub for this row in the old meta
                'exists in the new meta. If not, the data from this datarow shold not be copied to the
                'new data.
                For i As Integer = 0 To oldModel.Meta.Stub.Count - 1
                    'valIndex = _stubValues(i)
                    valIndex = _pMatrixHelper.StubVariablesValueIndex(i)

                    valOld = oldModel.Meta.Stub(i).Values(valIndex)
                    valNew = newMeta.Stub(i).Values.GetByCode(valOld.Code)
                    If valNew Is Nothing Then
                        ok = False
                        Exit For
                    End If
                Next

                If ok Then
                    'All stubvalues for this row exists in the new meta!
                    col = 0
                    For colIndex As Integer = 0 To oldModel.Data.MatrixColumnCount - 1
                        ok = True
                        'Identify variable values for this column
                        'IdentifyHeadingValues(colIndex, oldModel.Meta.Heading)
                        _pMatrixHelper.CalcHeadingWeights(colIndex)

                        'Verify that all variable values in the heading for this column in the old meta
                        'exists in the new meta. If not, the data from this datacolumn should not be
                        'copied to the new data.
                        For i As Integer = 0 To oldModel.Meta.Heading.Count - 1
                            'valIndex = _headingValues(i)
                            valIndex = _pMatrixHelper.HeadVariablesValueIndex(i)

                            valOld = oldModel.Meta.Heading(i).Values(valIndex)
                            valNew = newMeta.Heading(i).Values.GetByCode(valOld.Code)
                            If valNew Is Nothing Then
                                ok = False
                                Exit For
                            End If
                        Next

                        If ok Then
                            'All Stubvalues for this row and all Headingvalues for this column exist in new meta.
                            '- The value shall be copied to the new data!

                            'Read from old data
                            theValue = oldModel.Data.ReadElement(rowIndex, colIndex)
                            'Write to new data
                            newData.WriteElement(row, col, theValue)
                            'write new datanotecell
                            newData.WriteDataNoteCellElement(row, col, oldModel.Data.ReadDataCellNoteElement(rowIndex, colIndex))
                            col = col + 1 'Increment which column to write to
                        End If
                    Next
                    row = row + 1 'Increment which row to write to
                End If
            Next

            'Create new model to return
            newModel = New PXModel(newMeta, newData)
            newModel.Meta.Prune()
            newModel.IsComplete = True
            newData.UseDataCellMatrix = oldModel.Data.UseDataCellMatrix
            Return newModel
        End Function
#End Region

#Region "Private methods"
        ''' <summary>
        ''' Verifies that at least one value is selected for deletion
        ''' </summary>
        ''' <param name="selection">The selection matrix containing the values to delete</param>
        ''' <returns>True if at least one value is selected for deletion, else false</returns>
        ''' <remarks></remarks>
        Private Function VerifyDelete(ByVal selection As Selection()) As Boolean
            For Each sel As Selection In selection
                If sel.ValueCodes.Count > 0 Then
                    Return True
                End If
            Next

            Return False
        End Function

        ''' <summary>
        ''' Check that not all values for a variable are selected for removal.
        ''' Every variable must have at least one value
        ''' </summary>
        ''' <param name="variable">Variable in old Meta to verify deletion for</param>
        ''' <param name="selection">The selection matix containing the values to delete</param>
        ''' <remarks>
        ''' Throws a PXOperationException if all values for the variable are selected for removal.
        ''' </remarks>
        Private Sub VerifyVariableDelete(ByVal variable As Variable, ByVal selection As Selection())
            For Each sel As Selection In selection
                If variable.Code.Equals(sel.VariableCode) Then
                    If variable.Values.Count = sel.ValueCodes.Count Then
                        Throw New PXOperationException("Every variable must have at least one value")
                    End If
                    Exit For
                End If
            Next
        End Sub
#End Region

    End Class

End Namespace
