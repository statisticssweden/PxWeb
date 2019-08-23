Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class for handling LinkWithTable Operation
    ''' </summary>
    ''' <remarks>
    ''' The linkWithTable Operation identifies the values by name.
    ''' As a result of this the two models that participates in the operation must contain
    ''' exactly the same languages.
    ''' </remarks>
    Public Class LinkWithTable
        Implements PCAxis.Paxiom.IPXOperation
#Region "Private variables"
        Private _PMatrixHelperOldModel As PMatrixHelper
        Private _PMatrixHelperLinkModel As PMatrixHelper
        Private _operationsHelper As New OperationsHelper()
        Private _changedVariable As Variable
        Private _changeVariablePlacement As PlacementType
#End Region

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is LinkWithTableDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, LinkWithTableDescription))
        End Function

        ''' <summary>
        ''' Execute function handling the typecasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As LinkWithTableDescription) As PCAxis.Paxiom.PXModel
            Dim linkModel As PXModel

            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data

            Dim oldMeta As PCAxis.Paxiom.PXMeta = oldModel.Meta
            Dim oldData As PCAxis.Paxiom.PXData = oldModel.Data

            Dim changedVariableIndex As Integer
            Dim indexPositionChange As Integer

            Dim newValueIndex As Integer
            Dim oldValueIndex As Integer
            Dim valOld As Value
            Dim valNew As Value
            Dim calculatedIndexChange As Integer
            'Dim valueShiftInterval As Integer
            Dim matrixValue As Double
            Dim writeRow As Integer
            Dim writeColumn As Integer


            If rhs.LinkWithTableModel Is Nothing Then
                Throw New PXOperationException("Model for table to link in was missing.")
            End If

            linkModel = CType(rhs.LinkWithTableModel, PXModel)

            If Not CheckLanguages(oldModel, linkModel) Then
                Throw New PXOperationException("The models must have the same language(s)")
            End If

            ' Create a helper for the oldModel
            _PMatrixHelperOldModel = New PMatrixHelper(oldModel)

            ' Check that variable selection is the same in both models
            If _operationsHelper.CheckModelVariables(oldModel, linkModel) = False Then
                Throw New PXOperationException("The models has different variable selections.")
            End If

            ' Get the variable with different values
            _changedVariable = _operationsHelper.GetChangedVariable(oldModel, linkModel)

            If _changedVariable Is Nothing Then
                ' Nothing new - return the linkmodel pivoted to match the oldModel
                Return _operationsHelper.PivotModelToMatch(oldModel, linkModel)
            End If

            ' Prepare the linkModel
            linkModel = PrepareLinkModel(oldModel, linkModel)

            _changeVariablePlacement = _changedVariable.Placement

            ' Add values that are not present in the oldModel
            Dim tmpVariable As PCAxis.Paxiom.Variable
            For i As Integer = 0 To newModel.Meta.Variables.Count - 1
                tmpVariable = newModel.Meta.Variables(i)

                If tmpVariable.Code = _changedVariable.Code Then
                    ' Add value from the linkModel to the new Model --> expanding the matrix in meta
                    For Each v As Value In linkModel.Meta.Variables.GetByCode(tmpVariable.Code).Values
                        ' Only add values we do not have
                        'If tmpVariable.Values.GetByCode(v.Code) Is Nothing Then
                        '    tmpVariable.Values.Add(v.CreateCopy())
                        'End If
                        If tmpVariable.Values.GetByName(v.Value) Is Nothing Then
                            If tmpVariable.Values.IsCodesFictional Then
                                'Values may have the same fictional codes but different values...
                                Dim tmpValue As Value = v.CreateCopy
                                PaxiomUtil.SetCode(tmpValue, Nothing)
                                tmpVariable.Values.Add(tmpValue)
                                tmpVariable.Values.SetFictionalCodes()
                            Else
                                tmpVariable.Values.Add(v.CreateCopy())
                            End If
                        End If
                    Next
                End If
            Next

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            ' Pivot linkmodel to be same as oldModel
            linkModel = _operationsHelper.PivotModelToMatch(oldModel, linkModel)

            ' How many columns/rows does the changed variable span?
            If _changeVariablePlacement = PlacementType.Heading Then
                changedVariableIndex = newMeta.Heading.IndexOf(newMeta.Heading.GetByCode(_changedVariable.Code))
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(changedVariableIndex, newMeta.Heading)
            Else
                changedVariableIndex = newMeta.Stub.IndexOf(newMeta.Stub.GetByCode(_changedVariable.Code))
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(changedVariableIndex, newMeta.Stub)
            End If

            ' Set newmeta to the helper to be able to calculate the new position
            _PMatrixHelperOldModel.NewMeta = newMeta


            ' Loop original model and transfer data
            For rowIndex As Integer = 0 To oldData.MatrixRowCount - 1
                'Calculate stub weights
                _PMatrixHelperOldModel.CalcStubWeights(rowIndex)

                ' Calculate the indexpositionchange if changed variable is in the stub
                If _changeVariablePlacement = PlacementType.Stub Then
                    newValueIndex = -1
                    calculatedIndexChange = 0
                    ' check the sumvariable value for this row
                    oldValueIndex = _PMatrixHelperOldModel.StubVariablesValueIndex(changedVariableIndex)

                    valOld = oldModel.Meta.Stub(changedVariableIndex).Values(oldValueIndex)
                    valNew = newMeta.Stub(changedVariableIndex).Values.GetByCode(valOld.Code)

                    'valueShiftInterval = _PMatrixHelperOldModel.PMStub(changedVariableIndex + 1)

                    ' Check if the value has moved in meta
                    If valNew IsNot Nothing Then
                        newValueIndex = newMeta.Stub(changedVariableIndex).Values.IndexOf(valNew)

                        If newValueIndex < oldValueIndex Then
                            ' calculate the position change
                            calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                        ElseIf newValueIndex > oldValueIndex Then
                            calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                        End If
                    End If
                End If

                'Columns
                For columnIndex As Integer = 0 To oldData.MatrixColumnCount - 1
                    ' Calculate heading weights
                    _PMatrixHelperOldModel.CalcHeadingWeights(columnIndex)

                    ' Calculate the indexpositionchange if changed variable is in the heading
                    If _changeVariablePlacement = PlacementType.Heading Then
                        newValueIndex = -1
                        calculatedIndexChange = 0
                        ' check the sumvariable value for this col
                        oldValueIndex = _PMatrixHelperOldModel.HeadVariablesValueIndex(changedVariableIndex)

                        valOld = oldModel.Meta.Heading(changedVariableIndex).Values(oldValueIndex)
                        valNew = newMeta.Heading(changedVariableIndex).Values.GetByCode(valOld.Code)

                        'valueShiftInterval = _PMatrixHelperOldModel.PMHeading(changedVariableIndex + 1)

                        ' Check if the value has moved in meta
                        If valNew IsNot Nothing Then
                            newValueIndex = newMeta.Heading(changedVariableIndex).Values.IndexOf(valNew)

                            If newValueIndex < oldValueIndex Then
                                ' calculate the position change
                                calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                            ElseIf newValueIndex > oldValueIndex Then
                                calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                            End If
                        End If
                    End If

                    ' Read from old matrix
                    matrixValue = oldData.ReadElement(rowIndex, columnIndex)

                    ' Calculate index
                    _PMatrixHelperOldModel.CalculateIndexForNewMeta(writeRow, writeColumn)

                    ' Do we need to adjust the index position?
                    If calculatedIndexChange <> 0 Then
                        If _changeVariablePlacement = PlacementType.Heading Then
                            writeColumn += calculatedIndexChange
                        Else
                            writeRow += calculatedIndexChange
                        End If
                    End If

                    newData.WriteElement(writeRow, writeColumn, matrixValue)
                Next

            Next

            ' Create PMatrix helper for the linkmodel
            _PMatrixHelperLinkModel = New PMatrixHelper(linkModel)
            ' And set the newmeta to be able to calculate the new posision
            _PMatrixHelperLinkModel.NewMeta = newMeta

            ' Loop linkmodel and transfer data not available in oldmodel
            For rowIndex As Integer = 0 To linkModel.Data.MatrixRowCount - 1
                'Calculate stub weights
                _PMatrixHelperLinkModel.CalcStubWeights(rowIndex)

                ' Calculate the indexpositionchange if changed variable is in the stub
                If _changeVariablePlacement = PlacementType.Stub Then
                    newValueIndex = -1
                    calculatedIndexChange = 0
                    ' check the sumvariable value for this row
                    oldValueIndex = _PMatrixHelperLinkModel.StubVariablesValueIndex(changedVariableIndex)

                    valOld = linkModel.Meta.Stub(changedVariableIndex).Values(oldValueIndex)
                    'valNew = newMeta.Stub(changedVariableIndex).Values.GetByCode(valOld.Code)
                    valNew = newMeta.Stub(changedVariableIndex).Values.GetByName(valOld.Value)

                    'valueShiftInterval = _PMatrixHelperLinkModel.PMStub(changedVariableIndex + 1)

                    ' Check if the value has moved in meta
                    If valNew IsNot Nothing Then
                        newValueIndex = newMeta.Stub(changedVariableIndex).Values.IndexOf(valNew)

                        If newValueIndex < oldValueIndex Then
                            ' calculate the position change
                            calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                        ElseIf newValueIndex > oldValueIndex Then
                            calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                        End If
                    End If
                End If

                'Columns
                For columnIndex As Integer = 0 To linkModel.Data.MatrixColumnCount - 1
                    ' Calculate heading weights
                    _PMatrixHelperLinkModel.CalcHeadingWeights(columnIndex)

                    ' Calculate the indexpositionchange if changed variable is in the heading
                    If _changeVariablePlacement = PlacementType.Heading Then
                        newValueIndex = -1
                        calculatedIndexChange = 0
                        ' check the sumvariable value for this col
                        oldValueIndex = _PMatrixHelperLinkModel.HeadVariablesValueIndex(changedVariableIndex)

                        valOld = linkModel.Meta.Heading(changedVariableIndex).Values(oldValueIndex)
                        'valNew = newMeta.Heading(changedVariableIndex).Values.GetByCode(valOld.Code)
                        valNew = newMeta.Heading(changedVariableIndex).Values.GetByName(valOld.Value)

                        'valueShiftInterval = _PMatrixHelperLinkModel.PMHeading(changedVariableIndex + 1)

                        ' Check if the value has moved in meta
                        If valNew IsNot Nothing Then
                            newValueIndex = newMeta.Heading(changedVariableIndex).Values.IndexOf(valNew)

                            If newValueIndex < oldValueIndex Then
                                ' calculate the position change
                                calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                            ElseIf newValueIndex > oldValueIndex Then
                                calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                            End If
                        End If
                    End If


                    ' Read from old matrix
                    matrixValue = linkModel.Data.ReadElement(rowIndex, columnIndex)

                    ' Calculate index
                    _PMatrixHelperLinkModel.CalculateIndexForNewMeta(writeRow, writeColumn)

                    ' Do we need to adjust the index position?
                    If calculatedIndexChange <> 0 Then
                        If _changeVariablePlacement = PlacementType.Heading Then
                            writeColumn += calculatedIndexChange
                        Else
                            writeRow += calculatedIndexChange
                        End If
                    End If

                    newData.WriteElement(writeRow, writeColumn, matrixValue)
                Next
            Next

            'Copy notes
            CopyNotes(newMeta, linkModel)

            newModel = New PXModel(newMeta, newData)
            newModel.Meta.CreateTitle()

            Return newModel
        End Function

        ''' <summary>
        ''' Check that the models has the same variables and values.
        ''' Returns an array, emtpy or with variable codes that need to change value order
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="checkModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function PrepareLinkModel(ByRef oldModel As PXModel, ByRef checkModel As PXModel) As PXModel
            Dim newVar As Variable
            Dim newValue As Value
            Dim variableCodesWithDifferentValueIndexPosition As String()
            Dim arrayIndex As Integer = 0
            Dim variableAdded As Boolean
            Dim valueSelection As Selection = Nothing

            variableCodesWithDifferentValueIndexPosition = New String(oldModel.Meta.Variables.Count - 1) {}

            ' Loop variables in oldModel and verify that they are present in the checkModel
            For Each oldVar As Variable In oldModel.Meta.Variables
                newVar = checkModel.Meta.Variables.GetByCode(oldVar.Code)

                'Check all variables except the changed variable
                If oldVar.Code <> _changedVariable.Code Then
                    ' Loop oldModel variable values and verify that they are present in checkModel variable values
                    variableAdded = False
                    For Each oldVal As Value In oldVar.Values
                        newValue = newVar.Values.GetByCode(oldVal.Code)
                        If newValue Is Nothing Then
                            Throw New PXOperationException("Variable (" + oldVar.Name + ") did not have the same values in the overlay model.")
                        End If

                        ' Check indexposition of the value
                        If oldVar.Values.IndexOf(oldVal) <> newVar.Values.IndexOf(newValue) And variableAdded = False Then
                            variableAdded = True
                            variableCodesWithDifferentValueIndexPosition(arrayIndex) = oldVar.Code
                            arrayIndex += 1
                        End If
                    Next
                Else
                    ' Get valuecollection for changed variable.
                    ' Used to remove values from checkModel that exist in the oldModel
                    valueSelection = New Selection(newVar.Code)
                    For Each v As Value In newVar.Values
                        'If (oldVar.Values.GetByCode(v.Code) Is Nothing) = False Then
                        '    ' Value exist in oldModel variable - add to selection for removal from checkModel
                        '    valueSelection.ValueCodes.Add(v.Code)
                        'End If
                        If (oldVar.Values.GetByName(v.Value) Is Nothing) = False Then
                            ' Value exist in oldModel variable - add to selection for removal from checkModel
                            valueSelection.ValueCodes.Add(v.Code)
                        End If
                    Next
                End If
            Next

            ' Remove the values that were present in both oldModel and checkModel for the changedVariable
            If valueSelection.ValueCodes.Count > 0 Then
                Dim s(0) As PCAxis.Paxiom.Selection
                s(0) = valueSelection

                Dim deleteValueOperation As New DeleteValue()
                checkModel = deleteValueOperation.Execute(checkModel, s)
            End If

            ' Change value order for any variable code present in the array
            checkModel = _operationsHelper.ChangeValueOrders(oldModel, checkModel, variableCodesWithDifferentValueIndexPosition)

            Return checkModel
        End Function

        ''' <summary>
        ''' The linkWithTable operation identifies values in the old model and ín the linkmodel by name.
        ''' This means the both models must be in the same language. This function compares the languages in 
        ''' the two models.
        ''' </summary>
        ''' <param name="oldModel">The old model</param>
        ''' <param name="linkModel">The link model</param>
        ''' <returns>True if the old model and the link model contains the same languages, else false</returns>
        ''' <remarks></remarks>
        Private Function CheckLanguages(ByVal oldModel As PXModel, ByVal linkModel As PXModel) As Boolean
            Dim oldLangs() As String
            Dim linkLangs() As String

            oldLangs = oldModel.Meta.GetAllLanguages
            linkLangs = linkModel.Meta.GetAllLanguages

            'Only one of the models is multilingual
            If Not oldLangs Is Nothing And linkLangs Is Nothing Then
                Return False
            End If
            If oldLangs Is Nothing And Not linkLangs Is Nothing Then
                Return False
            End If

            'None of the models as multilingual
            If oldLangs Is Nothing And linkLangs Is Nothing Then
                If oldModel.Meta.Language.Equals(linkModel.Meta.Language) Then
                    Return True
                Else
                    Return False
                End If
            End If

            'Both models are multilingual
            If oldLangs.Length <> linkLangs.Length Then
                Return False
            End If

            For i As Integer = 0 To oldLangs.Length - 1
                If Not oldLangs(i).Equals(linkLangs(i)) Then
                    Return False
                End If
            Next

            If oldModel.Meta.CurrentLanguageIndex <> linkModel.Meta.CurrentLanguageIndex Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Copies notes from the linkmodel to the new Meta. 
        ''' </summary>
        ''' <param name="newMeta">The new meta to copy the notes to</param>
        ''' <param name="linkModel">The model to copy notes from</param>
        ''' <remarks></remarks>
        Private Sub CopyNotes(ByVal newMeta As PXMeta, ByVal linkModel As PXModel)
            Dim newNote As CellNote
            Dim variable As Variable
            Dim linkValue As Value
            Dim newValue As Value

            For Each note As CellNote In linkModel.Meta.CellNotes
                newNote = note.CreateCopy

                For Each vvp As VariableValuePair In newNote.Conditions
                    'Find value in linkmodel
                    variable = linkModel.Meta.Variables.GetByCode(vvp.VariableCode)

                    If variable.Values.IsCodesFictional Then
                        'If codes are fictional the code may have been changed - translate to the new one!
                        linkValue = variable.Values.GetByCode(vvp.ValueCode)

                        'Find value in new meta by name
                        variable = newMeta.Variables.GetByCode(vvp.VariableCode)
                        newValue = variable.Values.GetByName(linkValue.Value)

                        'Set new value code in condition
                        vvp.ValueCode = newValue.Code
                    End If
                Next

                'Add to new meta
                newMeta.CellNotes.Add(newNote)
            Next
        End Sub

    End Class



End Namespace