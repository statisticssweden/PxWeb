
Namespace PCAxis.Paxiom.Operations

    Public Class CalculateTwoValues
        Implements PCAxis.Paxiom.IPXOperation

        'creates weight matrix to hold information about 
        Private _weightHeading As Integer()
        Private _weightStub As Integer()

        'Creates pMatrix
        Private _oldPMHeading As Integer()
        Private _oldPMStub As Integer()
        Private _newPMHeading As Integer()
        Private _newPMStub As Integer()
        Private _sumVariableInStub As Boolean
        Private _sumVariableIndex As Integer
        Private _newValueIndex As Integer

        Private _selectedIndexs As New Dictionary(Of Integer, Integer)

        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is SumDescription Then
                Throw New Exception("Paramater not suported")
            End If

            Return Execute(lhs, CType(rhs, SumDescription))
        End Function

        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As SumDescription) As PCAxis.Paxiom.PXModel
            Dim newModel As PCAxis.Paxiom.PXModel
            Dim newMeta As PCAxis.Paxiom.PXMeta
            Dim newData As PCAxis.Paxiom.PXData

            Dim oldMeta As PCAxis.Paxiom.PXMeta = oldModel.Meta
            Dim oldData As PCAxis.Paxiom.PXData = oldModel.Data

            newMeta = oldMeta.CreateCopy()
            Dim v As Variable
            Dim nv As Variable 'the new variable
            Dim val As Value
            For i As Integer = 0 To oldMeta.Variables.Count - 1
                v = oldMeta.Variables(i)
                If rhs.VariableCode = v.Code Then
                    nv = v.CreateCopy()
                    nv.RecreateValues()
                    For j As Integer = 0 To v.Values.Count - 1
                        val = v.Values(j)
                        'If the value is in the sum
                        If rhs.ValueCodes.Contains(val.Code) Then
                            'Only add old value if it should be keept
                            If rhs.KeepValues Then
                                nv.Values.Add(val)
                            End If
                        Else
                            nv.Values.Add(val)
                        End If
                    Next
                Else
                    nv = v
                End If

                newMeta.Variables.Add(nv)
                If nv.Placement = PlacementType.Heading Then
                    newMeta.Heading.Add(nv)
                Else
                    newMeta.Stub.Add(nv)
                End If
            Next


            MakeIndexes(oldModel.Meta, rhs)

            'creates the new value

            Dim oldSumVar As Variable = oldMeta.Variables.GetByCode(rhs.VariableCode)
            Dim newsumVar As Variable = newMeta.Variables.GetByCode(rhs.VariableCode)
            _newValueIndex = CalcNewIndex(oldSumVar, rhs)
            Dim newValue As New Value(rhs.NewValueName, oldMeta.NumberOfLanguages)
            newValue.SetCode(rhs.NewValueCode)
            newsumVar.Values.Insert(_newValueIndex, newValue)

            'Create space forr the new data
            newData = CreateData(newMeta)

            'Creates pMatrix
            _oldPMHeading = CreatePMatrix(oldMeta.Heading)
            _oldPMStub = CreatePMatrix(oldMeta.Stub)
            _newPMHeading = CreatePMatrix(newMeta.Heading)
            _newPMStub = CreatePMatrix(newMeta.Stub)

            'TODO Calc weights matrix
            If oldMeta.Heading.Count = 0 Then
                _weightHeading = Nothing
            Else
                _weightHeading = New Integer(oldMeta.Heading.Count - 1) {}
            End If
            If oldMeta.Stub.Count = 0 Then
                _weightStub = Nothing
            Else
                _weightStub = New Integer(oldMeta.Stub.Count - 1) {}
            End If


            'Calculate(datacontent)
            If _sumVariableInStub Then
                CalculateDatacontentStub(oldMeta, oldData, newData, rhs)
            Else
                CalculateDatacontentHeading(oldMeta, oldData, newData, rhs)
            End If

            newModel = New PXModel(newMeta, newData)
            newModel.Meta.CreateTitle()

            Return newModel

        End Function




        Private Sub CalculateDatacontentStub(ByRef oldMeta As PCAxis.Paxiom.PXMeta, ByRef oldData As PCAxis.Paxiom.PXData, ByRef newData As PCAxis.Paxiom.PXData, ByVal rhs As SumDescription)
            Dim row(oldData.MatrixColumnCount - 1) As Double
            Dim nrIndex As Integer
            Dim ncIndex As Integer
            Dim de As Double
            Dim newValuesInserted As Boolean = False
            Dim variableLevel As Integer
            Dim isFirst As Boolean = False

            Dim parentRowWeight As Integer = 0
            Dim parentRowIndex As Integer = oldMeta.Stub.GetIndexByCode(rhs.VariableCode) - 1

            'If parent row contains first variable in stub, then we need some special treatment
            If parentRowIndex < 0 Then
                parentRowIndex = 0
                isFirst = True
            End If

            'Rows
            For rowIndex As Integer = 0 To oldData.MatrixRowCount - 1

                'Check index for current parent stub variable  
                parentRowWeight = _weightStub(parentRowIndex)

                'Calculate stub weights
                CalcStubWeights(rowIndex, oldMeta.Stub)

                'Check if index for current parent stub variable has increased (next parent row reached) or decreased (new parent on higher level reached)
                If _weightStub(parentRowIndex) <> parentRowWeight AndAlso Not isFirst Then
                    newValuesInserted = False
                End If

                'Columns
                For columnIndex As Integer = 0 To oldData.MatrixColumnCount - 1
                    CalcHeadingWeights(columnIndex, oldMeta.Heading)
                    de = oldData.ReadElement(rowIndex, columnIndex)
                    If ValueInSum() Then
                        'Write rows with old values
                        If rhs.KeepValues Then
                            CalcIndexes(nrIndex, ncIndex)
                            newData.WriteElement(nrIndex, ncIndex, de)
                            newValuesInserted = True
                        End If
                        'Write rows with new calculated values
                        CalcXIndexes(nrIndex, ncIndex)
                        newData.IncrementElement(nrIndex, ncIndex, de)
                    Else
                        If newValuesInserted Then
                            variableLevel = oldMeta.Stub.GetIndexByCode(rhs.VariableCode) + 1
                            CalcIndexesWithNewValueInserted(nrIndex, ncIndex, variableLevel)
                        Else
                            CalcIndexes(nrIndex, ncIndex)
                        End If
                        newData.WriteElement(nrIndex, ncIndex, de)
                    End If
                Next

            Next
        End Sub

        Private Sub CalculateDatacontentHeading(ByRef oldMeta As PCAxis.Paxiom.PXMeta, ByRef oldData As PCAxis.Paxiom.PXData, ByRef newData As PCAxis.Paxiom.PXData, ByVal rhs As SumDescription)
            Dim nrIndex As Integer
            Dim ncIndex As Integer
            Dim de As Double
            Dim newValuesInserted As Boolean = False
            Dim variableLevel As Integer
            Dim isFirst As Boolean = False

            Dim parentColumnWeight As Integer = 0
            Dim parentColumnIndex As Integer = oldMeta.Heading.GetIndexByCode(rhs.VariableCode) - 1

            'If parent column contains first variable in heading, then we need some special treatment
            If parentColumnIndex < 0 Then
                parentColumnIndex = 0
                isFirst = True
            End If


            'Columns
            For columnIndex As Integer = 0 To oldData.MatrixColumnCount - 1
                'Check index for current parent stub variable  
                parentColumnWeight = _weightHeading(parentColumnIndex)

                'Calculate heading weights
                CalcHeadingWeights(columnIndex, oldMeta.Heading)

                'Check if index for current parent stub variable has increased (next parent column reached) or decreased (new parent on higher level reached)
                If _weightHeading(parentColumnIndex) <> parentColumnWeight AndAlso Not isFirst Then
                    newValuesInserted = False
                End If

                'Rows
                For rowIndex As Integer = 0 To oldData.MatrixRowCount - 1
                    CalcStubWeights(rowIndex, oldMeta.Stub)
                    de = oldData.ReadElement(rowIndex, columnIndex)
                    If ValueInSum() Then
                        'Write rows with old values
                        If rhs.KeepValues Then
                            CalcIndexes(nrIndex, ncIndex)
                            newData.WriteElement(nrIndex, ncIndex, de)
                            newValuesInserted = True
                        End If
                        'Write rows with new calculated values
                        CalcXIndexes(nrIndex, ncIndex)
                        newData.IncrementElement(nrIndex, ncIndex, de)
                    Else
                        If newValuesInserted Then
                            variableLevel = oldMeta.Heading.GetIndexByCode(rhs.VariableCode) + 1
                            CalcIndexesWithNewValueInserted(nrIndex, ncIndex, variableLevel)
                        Else
                            CalcIndexes(nrIndex, ncIndex)
                        End If

                        newData.WriteElement(nrIndex, ncIndex, de)
                    End If
                Next

            Next
        End Sub








#Region "Common with SUMWithConstant"

        ''' <summary>
        ''' Calculates indexes for data before new valuerows has been inserted
        ''' </summary>
        ''' <param name="rIndex">current rowindex</param>
        ''' <param name="cIndex">current columnindex</param>
        ''' <remarks></remarks>
        Private Sub CalcIndexes(ByRef rIndex As Integer, ByRef cIndex As Integer)
            rIndex = 0
            cIndex = 0
            For i As Integer = 0 To _weightStub.Length - 1
                rIndex += _newPMStub(i + 1) * _weightStub(i)
            Next
            For i As Integer = 0 To _weightHeading.Length - 1
                cIndex += _newPMHeading(i + 1) * _weightHeading(i)
            Next
        End Sub


        ''' <summary>
        ''' Calculates indexes for data after new valuerows has been inserted
        ''' </summary>
        ''' <param name="rIndex">current rowindex</param>
        ''' <param name="cIndex">current columnindex</param>
        ''' <param name="variableLevel">variables index in Stub/Heading</param>
        ''' <remarks></remarks>
        Private Sub CalcIndexesWithNewValueInserted(ByRef rIndex As Integer, ByRef cIndex As Integer, ByRef variableLevel As Integer)
            rIndex = 0
            cIndex = 0
            For i As Integer = 0 To _weightStub.Length - 1
                rIndex += _newPMStub(i + 1) * _weightStub(i)
            Next
            For i As Integer = 0 To _weightHeading.Length - 1
                cIndex += _newPMHeading(i + 1) * _weightHeading(i)
            Next
            'New values are inserted increase index with that space
            If _sumVariableInStub Then
                rIndex += _newPMStub(variableLevel)
            Else
                cIndex += _newPMHeading(variableLevel)
            End If

        End Sub

        Private Sub MakeIndexes(ByVal meta As PXMeta, ByVal rhs As SumDescription)
            Dim var As Variable
            var = meta.Variables.GetByCode(rhs.VariableCode)
            If var.Placement = PlacementType.Stub Then
                _sumVariableInStub = True
                _sumVariableIndex = meta.Stub.IndexOf(var)
            Else
                _sumVariableInStub = False
                _sumVariableIndex = meta.Heading.IndexOf(var)
            End If
            For i As Integer = 0 To rhs.ValueCodes.Count - 1
                _selectedIndexs.Add(var.Values.GetIndexByCode(rhs.ValueCodes(i)), Nothing)
            Next
        End Sub


        Private Shared Function CalcNewIndex(ByVal var As Variable, ByVal info As SumDescription) As Integer
            'sum all
            If info.ValueCodes.Count = var.Values.Count Then
                If info.KeepValues Then
                    Return var.Values.Count
                Else
                    Return 0
                End If
            End If

            Dim max As Integer = 0
            For Each valCode As String In info.ValueCodes
                max = Math.Max(max, var.Values.GetIndexByCode(valCode))
            Next
            If info.KeepValues Then
                Return max + 1
            End If

            Return max + 1 - (info.ValueCodes.Count - 1)
        End Function



        Public Function CreateData(ByVal meta As PXMeta) As PXData
            Dim rows As Integer = 1
            Dim columns As Integer = 1

            For i As Integer = 0 To meta.Stub.Count - 1
                rows *= meta.Stub(i).Values.Count
            Next

            For i As Integer = 0 To meta.Heading.Count - 1
                columns *= meta.Heading(i).Values.Count
            Next

            Dim data As New PXData

            data.SetMatrixSize(rows, columns)

            Return data
        End Function



        Private Function CreatePMatrix(ByVal variables As Variables) As Integer()
            Dim size As Integer = variables.Count

            Dim pMatrix As Integer()
            'Check that there is at least one variable in the Heading
            If size > 0 Then
                pMatrix = New Integer(size) {}

                For index As Integer = 0 To size - 1
                    pMatrix(index) = 1
                    For j As Integer = index To size - 1
                        pMatrix(index) *= variables(j).Values.Count
                    Next
                Next
                pMatrix(size) = 1
            Else
                pMatrix = Nothing
            End If

            Return pMatrix
        End Function

        'calculates the index for the new sum value
        Private Sub CalcXIndexes(ByRef rIndex As Integer, ByRef cIndex As Integer)
            rIndex = 0
            cIndex = 0
            If _sumVariableInStub Then
                For i As Integer = 0 To _weightStub.Length - 1
                    If i = _sumVariableIndex Then
                        rIndex += _newPMStub(i + 1) * _newValueIndex
                    Else
                        rIndex += _newPMStub(i + 1) * _weightStub(i)
                    End If
                Next

                For i As Integer = 0 To _weightHeading.Length - 1
                    cIndex += _newPMHeading(i + 1) * _weightHeading(i)
                Next
            Else
                For i As Integer = 0 To _weightStub.Length - 1
                    rIndex += _newPMStub(i + 1) * _weightStub(i)
                Next

                For i As Integer = 0 To _weightHeading.Length - 1
                    If i = _sumVariableIndex Then
                        cIndex += _newPMHeading(i + 1) * _newValueIndex
                    Else
                        cIndex += _newPMHeading(i + 1) * _weightHeading(i)
                    End If
                Next
            End If

        End Sub

        'OK
        Private Sub CalcHeadingWeights(ByVal columnIndex As Integer, ByVal headingVariables As Variables)
            Dim size As Integer = _weightHeading.Length

            For index As Integer = 0 To size - 1
                _weightHeading(index) = Convert.ToInt32(Math.Floor((columnIndex / _oldPMHeading(index + 1)))) Mod headingVariables(index).Values.Count
            Next
        End Sub

        'OK
        Private Sub CalcStubWeights(ByVal rowIndex As Integer, ByVal stubVariables As Variables)
            Dim size As Integer = _weightStub.Length

            For index As Integer = 0 To size - 1
                _weightStub(index) = Convert.ToInt32(Math.Floor(rowIndex / _oldPMStub(index + 1))) Mod stubVariables(index).Values.Count
            Next
        End Sub

        Private Function ValueInSum() As Boolean
            If _sumVariableInStub Then
                Return _selectedIndexs.ContainsKey(_weightStub(_sumVariableIndex))
            End If
            Return _selectedIndexs.ContainsKey(_weightHeading(_sumVariableIndex))
        End Function

#End Region

    End Class

End Namespace
