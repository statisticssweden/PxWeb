Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for handling ChangeValueOrder Operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeValueOrder
        Implements PCAxis.Paxiom.IPXOperation

#Region "Private members"
        Private _pMatrixHelper As PMatrixHelper
        'Private _weightHeading As Integer()
        'Private _weightStub As Integer()
        'Public _modWeight As Integer()
        'Private _oldPMHeading As Integer()
        'Private _oldPMStub As Integer()
        'Private _changedVariablePlacement As PCAxis.Paxiom.PlacementType
        'Private _changedVariableIndex As Integer
#End Region

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is ChangeValueOrderDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, ChangeValueOrderDescription))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PCAxis.Paxiom.PXModel, ByVal rhs As ChangeValueOrderDescription) As PCAxis.Paxiom.PXModel
            Dim newModel As PCAxis.Paxiom.PXModel = oldModel.CreateCopy()
            Dim newMeta As PCAxis.Paxiom.PXMeta = newModel.Meta
            Dim newData As PCAxis.Paxiom.PXData = newModel.Data

            Dim oldMeta As PCAxis.Paxiom.PXMeta = oldModel.Meta
            Dim oldData As PCAxis.Paxiom.PXData = oldModel.Data

            If rhs.VariableCode Is Nothing Then
                Throw New PXOperationException("Variable Code was missing.")
            ElseIf rhs.VariableCode.Length = 0 Then
                Throw New PXOperationException("Variable Code was missing.")
            End If

            If rhs.ModifiedVariableValueWeight Is Nothing Then
                Throw New PXOperationException("Modified Variable Value weight was missing.")
            End If

            _pMatrixHelper = New PMatrixHelper(oldModel)

            _pMatrixHelper.ModWeight = rhs.ModifiedVariableValueWeight

            REM Insert the variables in new Meta from old Meta and the incoming changevalueorderdescription
            Dim tmpVariable As PCAxis.Paxiom.Variable
            tmpVariable = newModel.Meta.Variables.GetByCode(rhs.VariableCode)
            ' Change the value order
            ChangeValueOrder(tmpVariable)

            'Remove hierarchy from variable if present
            If tmpVariable.Hierarchy.IsHierarchy Then
                tmpVariable.Hierarchy.Clear()
            End If

            ' Where is our variable placed - stub or head
            _pMatrixHelper.ChangedVariablePlacement = tmpVariable.Placement

            ' Keep track of changeVariables index in the stub/head collection
            If tmpVariable.Placement = PlacementType.Stub Then
                _pMatrixHelper.ChangedVariableIndex = newMeta.Stub.IndexOf(tmpVariable)
            Else
                _pMatrixHelper.ChangedVariableIndex = newMeta.Heading.IndexOf(tmpVariable)
            End If

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            ' Setup the new matrix
            SetupNewMatrix(oldModel, newData)

            'newModel = New PXModel(newMeta, newData)
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel
        End Function


#Region "Private functions/subs"
        ''' <summary>
        ''' Change value order for incoming variable based on the _modWeight array
        ''' </summary>
        ''' <param name="var"></param>
        ''' <remarks></remarks>
        Private Sub ChangeValueOrder(ByRef var As PCAxis.Paxiom.Variable)
            Dim tmpVar As PCAxis.Paxiom.Variable
            Dim valueArr() As Value = New Value(var.Values.Count - 1) {}

            ' Copy the Original Variable
            tmpVar = var.CreateCopy()
            ' Clear the original
            var.RecreateValues()

            ' Now place them in new order
            For index As Integer = 0 To _pMatrixHelper.ModWeight.Length - 1
                valueArr(_pMatrixHelper.ModWeight(index)) = tmpVar.Values(index).CreateCopy()
            Next
            ' and put them back
            For index As Integer = 0 To _pMatrixHelper.ModWeight.Length - 1
                var.Values.Add(valueArr(index))
            Next
        End Sub
        ''' <summary>
        ''' Move values from old matrix to new matrix
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="newData"></param>
        ''' <remarks></remarks>
        Private Sub SetupNewMatrix(ByRef oldModel As PCAxis.Paxiom.PXModel, ByRef newData As PCAxis.Paxiom.PXData)
            Dim row(oldModel.Data.MatrixColumnCount - 1) As Double
            Dim nrIndex As Integer
            Dim ncIndex As Integer
            Dim de As Double
            Dim newValuesInserted As Boolean = False

            ' Loop all rows/columns in the old matrix.
            ' Move value to new matrix using new matrix position where applicable
            For rowIndex As Integer = 0 To oldModel.Data.MatrixRowCount - 1
                ' Setup help array for Stub
                'CalcStubWeights(rowIndex, oldModel.Meta.Stub)
                _pMatrixHelper.CalcStubWeightsForModWeight(rowIndex)
                For columnIndex As Integer = 0 To oldModel.Data.MatrixColumnCount - 1
                    ' Setup help array for Head
                    'CalcHeadingWeights(columnIndex, oldModel.Meta.Heading)
                    _pMatrixHelper.CalcHeadingWeightsForModWeight(columnIndex)

                    ' Get value from old matrix
                    de = oldModel.Data.ReadElement(rowIndex, columnIndex)

                    ' Calculate index for the new matrix
                    'CalcXIndexes(nrIndex, ncIndex)
                    _pMatrixHelper.CalculateIndex(nrIndex, ncIndex)

                    newData.WriteElement(nrIndex, ncIndex, de)
                    newData.WriteDataNoteCellElement(nrIndex, ncIndex, oldModel.Data.ReadDataCellNoteElement(rowIndex, columnIndex))
                Next

            Next
        End Sub
#End Region
    End Class
End Namespace