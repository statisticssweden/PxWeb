Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class to act as a service for PMatrix functionality
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PMatrixHelper
#Region "Private variables"
        Private _operationsHelper As New OperationsHelper

        Private _model As PCAxis.Paxiom.PXModel
        Private _newMeta As PCAxis.Paxiom.PXMeta

        Private _PMHeading As Integer()
        Private _NewPMHeading As Integer()

        Private _PMStub As Integer()
        Private _NewPMStub As Integer()

        Private _headVariablesValueIndex As Integer()
        Private _NewHeadVariablesValueIndex As Integer()
        Private _stubVariablesValueIndex As Integer()
        Private _NewStubVariablesValueIndex As Integer()

        Private _modWeight As Integer()

        Private _changedVariablePlacement As PCAxis.Paxiom.PlacementType
        Private _changedVariableIndex As Integer = -1

        ' Sum functionalíty related variables
        Private _sumVariablePlacement As PCAxis.Paxiom.PlacementType
        Private _sumVariableIndex As Integer
        Private _sumSelectedIndexs As New Dictionary(Of Integer, Integer)
        ' end sum related
        Private _calculatePerPartOneMatrixValue As Double
        Private _calculatePerPartValueHasBeenSet As Boolean = False
#End Region

#Region "Properties"

        ''' <summary>
        ''' The new Meta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NewMeta() As PCAxis.Paxiom.PXMeta
            Get
                Return _newMeta
            End Get
            Set(ByVal value As PCAxis.Paxiom.PXMeta)
                _newMeta = value
                InitNewMeta()
            End Set
        End Property

        ''' <summary>
        ''' PM for the newMeta heading variables
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NewPMHeading() As Integer()
            Get
                Return _NewPMHeading
            End Get
            Set(ByVal value As Integer())
                _NewPMHeading = value
            End Set
        End Property

        ''' <summary>
        ''' PM for the newMeta stub variables
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NewPMStub() As Integer()
            Get
                Return _NewPMStub
            End Get
            Set(ByVal value As Integer())
                _NewPMStub = value
            End Set
        End Property

        ''' <summary>
        ''' PM for the Meta heading variables
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PMHeading() As Integer()
            Get
                Return _PMHeading
            End Get
        End Property

        ''' <summary>
        ''' PM for the Meta stub variables
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PMStub() As Integer()
            Get
                Return _PMStub
            End Get
        End Property


        ''' <summary>
        ''' Placement of the SumVariable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Initialized in the SumVarInit function</remarks>
        Public ReadOnly Property SumVariablePlacement() As PCAxis.Paxiom.PlacementType
            Get
                Return _sumVariablePlacement
            End Get
            'Set(ByVal value As PCAxis.Paxiom.PlacementType)
            '    _sumVariablePlacement = value
            'End Set
        End Property


        ''' <summary>
        ''' Indexposition of the changed variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChangedVariableIndex() As Integer
            Get
                Return _changedVariableIndex
            End Get
            Set(ByVal value As Integer)
                _changedVariableIndex = value
            End Set
        End Property

        ''' <summary>
        ''' Placement of the changed variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChangedVariablePlacement() As PCAxis.Paxiom.PlacementType
            Get
                Return _changedVariablePlacement
            End Get
            Set(ByVal value As PCAxis.Paxiom.PlacementType)
                _changedVariablePlacement = value
            End Set
        End Property


        ''' <summary>
        ''' Mod array to be used as an index remapper
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModWeight() As Integer()
            Get
                Return _modWeight
            End Get
            Set(ByVal value As Integer())
                _modWeight = value
            End Set
        End Property


        ''' <summary>
        ''' Indexposition of values for each Stub variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StubVariablesValueIndex() As Integer()
            Get
                Return _stubVariablesValueIndex
            End Get
        End Property

        ''' <summary>
        ''' Indexposition of values for each Heading variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HeadVariablesValueIndex() As Integer()
            Get
                Return _headVariablesValueIndex
            End Get
        End Property
#End Region


#Region "Constructors / Initializers"
        'Public Sub New()

        'End Sub

        ''' <summary>
        ''' Constructor - set model to work with and initialize help matrixes
        ''' </summary>
        ''' <param name="model">PXModel</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal model As PXModel)
            _model = model

            Me.Init()
        End Sub

        ''' <summary>
        ''' Functionality to initialize the neccessary helpers
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub Init()
            _PMHeading = Me.CreatePMatrix(_model.Meta.Heading)
            _PMStub = Me.CreatePMatrix(_model.Meta.Stub)

            ' Init _weight arrays
            If Me._model.Meta.Heading.Count = 0 Then
                Me._headVariablesValueIndex = Nothing
            Else
                Me._headVariablesValueIndex = New Integer(Me._model.Meta.Heading.Count - 1) {}
            End If
            If Me._model.Meta.Stub.Count = 0 Then
                Me._stubVariablesValueIndex = Nothing
            Else
                Me._stubVariablesValueIndex = New Integer(Me._model.Meta.Stub.Count - 1) {}
            End If

        End Sub

        ''' <summary>
        ''' Initialize and setup the newMeta helpers
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitNewMeta()
            _NewPMHeading = Me.CreatePMatrix(_newMeta.Heading)
            _NewPMStub = Me.CreatePMatrix(_newMeta.Stub)

            ' Init _weight arrays
            If Me._newMeta.Heading.Count = 0 Then
                Me._NewHeadVariablesValueIndex = Nothing
            Else
                Me._NewHeadVariablesValueIndex = New Integer(Me._newMeta.Heading.Count - 1) {}
            End If
            If Me._newMeta.Stub.Count = 0 Then
                Me._NewStubVariablesValueIndex = Nothing
            Else
                Me._NewStubVariablesValueIndex = New Integer(Me._newMeta.Stub.Count - 1) {}
            End If
        End Sub

        ''' <summary>
        ''' Initialize for Sum Variable functionality
        ''' </summary>
        ''' <param name="var"></param>
        ''' <param name="valueCodes"></param>
        ''' <param name="keepValues"></param>
        ''' <remarks></remarks>
        Public Sub SumVariableInit(ByVal var As Variable, ByVal valueCodes As List(Of String), ByVal keepValues As Boolean, ByVal newMeta As PCAxis.Paxiom.PXMeta)
            Dim tmpIndex As Integer

            Me._sumVariablePlacement = var.Placement

            If var.Placement = PlacementType.Stub Then
                Me._sumVariableIndex = Me._model.Meta.Stub.IndexOf(var)
            Else
                Me._sumVariableIndex = Me._model.Meta.Heading.IndexOf(var)
            End If

            For i As Integer = 0 To valueCodes.Count - 1
                tmpIndex = var.Values.GetIndexByCode(valueCodes(i))
                If tmpIndex > -1 Then
                    If Not _sumSelectedIndexs.ContainsKey(tmpIndex) Then
                        Me._sumSelectedIndexs.Add(tmpIndex, Nothing)
                    End If
                End If
            Next

            Me.NewMeta = newMeta
        End Sub
#End Region


        ''' <summary>
        ''' Creates the helpmatrixes that are used to identify which variable values that corresponds
        ''' to a given row or column in the datamatrix
        ''' </summary>
        ''' <param name="variables">The variables (Stub or Heading)</param>
        ''' <returns>An Integer array</returns>
        ''' <remarks></remarks>
        Public Function CreatePMatrix(ByVal variables As Variables) As Integer()
            Dim size As Integer = variables.Count

            Dim pMatrix As Integer()
            'Check that there is at least one variable
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

        ''' <summary>
        ''' Identifies value position in the heading variables for the incoming columnIndex
        ''' </summary>
        ''' <param name="columnIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalcHeadingWeights(ByVal columnIndex As Integer)
            If _headVariablesValueIndex IsNot Nothing Then
                Dim size As Integer = _headVariablesValueIndex.Length
                Dim headingVariables As Variables = Me._model.Meta.Heading

                For index As Integer = 0 To size - 1
                    Me._headVariablesValueIndex(index) = Convert.ToInt32(Math.Floor((columnIndex / Me._PMHeading(index + 1)))) Mod headingVariables(index).Values.Count
                Next
            End If
        End Sub

        ''' <summary>
        ''' Identifies value position in the heading variables for the incoming columnIndex
        ''' If the changed variable is in the Heading it matches the indexposition and remaps using the _modWeight array
        ''' </summary>
        ''' <param name="columnIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalcHeadingWeightsForModWeight(ByVal columnIndex As Integer)
            ' Check properties
            If _modWeight Is Nothing Or _changedVariableIndex = -1 Then
                Throw New PXOperationException("Required properties has not been set for the ModWeight calculation to work.")
            End If

            If _headVariablesValueIndex IsNot Nothing Then
                Dim size As Integer = _headVariablesValueIndex.Length
                Dim headingVariables As Variables = Me._model.Meta.Heading

                For index As Integer = 0 To size - 1
                    If index = _changedVariableIndex And _changedVariablePlacement = PlacementType.Heading Then
                        ' Get the (possibly) remapped index from the _modWeight array
                        Me._headVariablesValueIndex(index) = _modWeight(Convert.ToInt32(Math.Floor((columnIndex / Me._PMHeading(index + 1)))) Mod headingVariables(index).Values.Count)
                    Else
                        ' No change - same index as before
                        Me._headVariablesValueIndex(index) = Convert.ToInt32(Math.Floor((columnIndex / Me._PMHeading(index + 1)))) Mod headingVariables(index).Values.Count
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Identifies value position in the stub variables for the incoming rowIndex
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalcStubWeights(ByVal rowIndex As Integer)
            If _stubVariablesValueIndex IsNot Nothing Then
                Dim size As Integer = _stubVariablesValueIndex.Length
                Dim stubVariables As Variables = Me._model.Meta.Stub

                For index As Integer = 0 To size - 1
                    Me._stubVariablesValueIndex(index) = Convert.ToInt32(Math.Floor(rowIndex / Me._PMStub(index + 1))) Mod stubVariables(index).Values.Count
                Next
            End If
        End Sub

        ''' <summary>
        ''' Identifies value position in the stub variables for the incoming rowIndex
        ''' If the changed variable is in the Heading it matches the indexposition and remaps using the _modWeight array
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalcStubWeightsForModWeight(ByVal rowIndex As Integer)
            ' Check properties
            If _modWeight Is Nothing Or _changedVariableIndex = -1 Then
                Throw New PXOperationException("Required properties has not been set for the ModWeight calculation to work.")
            End If

            If _stubVariablesValueIndex IsNot Nothing Then
                Dim size As Integer = _stubVariablesValueIndex.Length
                Dim stubVariables As Variables = Me._model.Meta.Stub

                For index As Integer = 0 To size - 1

                    If index = _changedVariableIndex And _changedVariablePlacement = PlacementType.Stub Then
                        ' Get the (possibly) remapped index from the _modWeight array
                        Me._stubVariablesValueIndex(index) = _modWeight(Convert.ToInt32(Math.Floor(rowIndex / Me._PMStub(index + 1))) Mod stubVariables(index).Values.Count)
                    Else
                        ' No change - same index as before
                        Me._stubVariablesValueIndex(index) = Convert.ToInt32(Math.Floor(rowIndex / Me._PMStub(index + 1))) Mod stubVariables(index).Values.Count
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Calculate the row and column index to use in the new matrix
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <param name="columnIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalculateIndex(ByRef rowIndex As Integer, ByRef columnIndex As Integer)
            rowIndex = 0
            columnIndex = 0

            If Me._stubVariablesValueIndex IsNot Nothing Then
                For i As Integer = 0 To Me._stubVariablesValueIndex.Length - 1
                    rowIndex += Me._PMStub(i + 1) * Me._stubVariablesValueIndex(i)
                Next
            End If
            If Me._headVariablesValueIndex IsNot Nothing Then
                For i As Integer = 0 To Me._headVariablesValueIndex.Length - 1
                    columnIndex += Me._PMHeading(i + 1) * Me._headVariablesValueIndex(i)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Calculate the row and column index using newMeta setup
        ''' </summary>
        ''' <param name="rIndex"></param>
        ''' <param name="cIndex"></param>
        ''' <remarks></remarks>
        Public Sub CalculateIndexForNewMeta(ByRef rIndex As Integer, ByRef cIndex As Integer)
            rIndex = 0
            cIndex = 0

            If Me._stubVariablesValueIndex IsNot Nothing Then
                For i As Integer = 0 To Me._stubVariablesValueIndex.Length - 1
                    rIndex += Me._NewPMStub(i + 1) * Me._stubVariablesValueIndex(i)
                Next
            End If

            If Me._headVariablesValueIndex IsNot Nothing Then
                For i As Integer = 0 To Me._headVariablesValueIndex.Length - 1
                    cIndex += Me._NewPMHeading(i + 1) * Me._headVariablesValueIndex(i)
                Next
            End If
        End Sub




        ''' <summary>
        ''' Transfer data from old model to new data using newMeta as template.
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="newSumVar"></param>
        ''' <param name="rhs"></param>
        ''' <remarks>The supported variants for summarizing data is:
        ''' * For addition n numbers of Values can be selected OR one Value and One Constant
        ''' * For all other operations: Two Values OR one Value and one Constant
        ''' </remarks>
        Public Sub SumVariableTransferData(ByRef data As PXData, ByVal newSumVar As Variable, ByVal rhs As SumDescription)
            Dim oldValueIndex As Integer
            Dim newValueIndex As Integer
            Dim sumValueIndex As Integer = 0
            Dim calculatedIndexChange As Integer
            Dim calculatedSumValueIndexChange As Integer
            Dim sumValueCounter As Integer
            Dim sumValueCounterReset As Integer
            Dim valOld As Value
            Dim valNew As Value
            Dim valueShiftInterval As Integer
            Dim writeColumn As Integer
            Dim writeRow As Integer
            Dim theValue As Double
            Dim theSavedValue As Double
            Dim writeValue As Double
            Dim keepThisCoR As Boolean
            Dim indexPositionChange As Integer

            Dim rhsKeepValues As Boolean
            Dim rhsNewValueCode As String
            Dim rhsValueCodes As List(Of String)
            Dim firstSumValue As Boolean = False ' Keep track of the selected values position to be able to do the correct calculation for subtraction and division

            Dim calculateWithConstant As Boolean = False
            Dim isValueInSumSelection As Boolean

            If TypeOf rhs Is SumDescription Then
                rhsKeepValues = rhs.KeepValues
                rhsNewValueCode = rhs.NewValueCode
                rhsValueCodes = rhs.ValueCodes
            Else
                Throw New PXOperationException("Paramater not suported")
            End If

            ' Should we calculate with a constant value? - We do not check this if we are grouping
            If rhs.ValueCodes.Count = 1 Then calculateWithConstant = True

            ' How many columns/rows does each sumvariable value span?
            If Me._sumVariablePlacement = PlacementType.Heading Then
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(Me.NewMeta.Heading.IndexOf(newSumVar), Me.NewMeta.Heading)
            Else
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(Me.NewMeta.Stub.IndexOf(newSumVar), Me.NewMeta.Stub)
            End If

            For rowIndex As Integer = 0 To Me._model.Data.MatrixRowCount - 1
                Me.CalcStubWeights(rowIndex)

                If newSumVar.Placement = PlacementType.Stub Then
                    keepThisCoR = True
                    newValueIndex = -1
                    calculatedIndexChange = 0
                    ' check the sumvariable value for this row
                    oldValueIndex = Me.StubVariablesValueIndex(_sumVariableIndex)

                    valOld = Me._model.Meta.Stub(_sumVariableIndex).Values(oldValueIndex)
                    valNew = NewMeta.Stub(_sumVariableIndex).Values.GetByCode(valOld.Code)

                    ' We need to know if this value was the first selected for the division/subtraction calculations to be correct
                    firstSumValue = (valOld.Code = rhs.ValueCodes(0))

                    valueShiftInterval = Me._PMStub(_sumVariableIndex + 1)
                    sumValueCounterReset = Me._PMStub(_sumVariableIndex + 1) * rhs.ValueCodes.Count

                    ' Reset if sumValueCounter has done a full round for the values to summarize
                    If sumValueCounter = sumValueCounterReset Then sumValueCounter = 0

                    ' Check if value is to be summarized
                    isValueInSumSelection = False
                    For Each valueCode As String In rhs.ValueCodes
                        If valOld.Code = valueCode Then
                            isValueInSumSelection = True
                            Exit For
                        End If
                    Next
                    If isValueInSumSelection Then
                        sumValueCounter = sumValueCounter + 1
                    End If

                    ' Get the sum value index
                    sumValueIndex = NewMeta.Stub(_sumVariableIndex).Values.IndexOf(NewMeta.Stub(_sumVariableIndex).Values.GetByCode(rhsNewValueCode))

                    ' Check if the value has moved in meta
                    If valNew IsNot Nothing Then
                        newValueIndex = NewMeta.Stub(_sumVariableIndex).Values.IndexOf(valNew)

                        If newValueIndex < oldValueIndex Then
                            ' calculate the position change
                            calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                        ElseIf newValueIndex > oldValueIndex Then
                            calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                        End If
                    Else
                        keepThisCoR = False
                    End If
                End If

                ' Reset if sumVar is in the heading
                If newSumVar.Placement = PlacementType.Heading Then sumValueCounter = 0
                For colIndex As Integer = 0 To Me._model.Data.MatrixColumnCount - 1
                    Me.CalcHeadingWeights(colIndex)

                    If Me._sumVariablePlacement = PlacementType.Heading Then
                        keepThisCoR = True
                        newValueIndex = -1
                        calculatedIndexChange = 0
                        ' check the sumvariable value for this col
                        oldValueIndex = Me.HeadVariablesValueIndex(_sumVariableIndex)

                        valOld = Me._model.Meta.Heading(_sumVariableIndex).Values(oldValueIndex)
                        valNew = Me.NewMeta.Heading(_sumVariableIndex).Values.GetByCode(valOld.Code)

                        ' We need to know if this value was the first one selected for the division/subtraction calculations to be correct
                        firstSumValue = (valOld.Code = rhs.ValueCodes(0))

                        valueShiftInterval = Me._PMHeading(_sumVariableIndex + 1)
                        sumValueCounterReset = Me._PMHeading(_sumVariableIndex + 1) * rhs.ValueCodes.Count

                        ' Check if value is to be summarized
                        isValueInSumSelection = False
                        For Each valueCode As String In rhs.ValueCodes
                            If valOld.Code = valueCode Then
                                isValueInSumSelection = True
                                Exit For
                            End If
                        Next

                        ' Reset?
                        If sumValueCounter = sumValueCounterReset Then sumValueCounter = 0

                        If isValueInSumSelection Then
                            sumValueCounter = sumValueCounter + 1
                        End If

                        ' Get the sum value index
                        sumValueIndex = Me.NewMeta.Heading(_sumVariableIndex).Values.IndexOf(Me.NewMeta.Heading(_sumVariableIndex).Values.GetByCode(rhsNewValueCode))

                        ' Check if the value has moved in meta
                        If valNew IsNot Nothing Then
                            newValueIndex = NewMeta.Heading(_sumVariableIndex).Values.IndexOf(valNew)

                            If newValueIndex < oldValueIndex Then
                                ' calculate the position change
                                calculatedIndexChange -= (oldValueIndex - newValueIndex) * indexPositionChange
                            ElseIf newValueIndex > oldValueIndex Then
                                calculatedIndexChange += (newValueIndex - oldValueIndex) * indexPositionChange
                            End If
                        Else
                            keepThisCoR = False
                        End If

                    End If

                    'Read from old data
                    theValue = Me._model.Data.ReadElement(rowIndex, colIndex)

                    ' Calculate index
                    Me.CalculateIndexForNewMeta(writeRow, writeColumn)

                    If keepThisCoR Then
                        ' Do we need to adjust the index position?
                        If calculatedIndexChange <> 0 Then
                            If Me._sumVariablePlacement = PlacementType.Heading Then
                                writeColumn += calculatedIndexChange
                            Else
                                writeRow += calculatedIndexChange
                            End If
                        End If

                        'Write to new data
                        data.WriteElement(writeRow, writeColumn, theValue)
                        data.WriteDataNoteCellElement(writeRow, writeColumn, Me._model.Data.ReadDataCellNoteElement(rowIndex, colIndex))
                    End If

                    If IsSumVariableValueSelected() Then
                        ' This value should be summarized

                        ' Get the index for the sum value
                        calculatedSumValueIndexChange = 0

                        ' Calculate the sum Value index change depending on if we should keep operator values or not
                        If rhsKeepValues Then
                            calculatedSumValueIndexChange = (sumValueIndex - newValueIndex) * indexPositionChange
                        Else
                            ' this is a selected value that has been removed
                            calculatedSumValueIndexChange = (sumValueIndex - oldValueIndex) * indexPositionChange
                        End If

                        If calculatedSumValueIndexChange <> 0 Then
                            ' Index position for the sum value index needs to be corrected
                            If Me._sumVariablePlacement = PlacementType.Heading Then
                                writeColumn += calculatedSumValueIndexChange
                            Else
                                writeRow += calculatedSumValueIndexChange
                            End If
                        End If

                        ' Get value that might have been saved in previous loop
                        theSavedValue = data.ReadElement(writeRow, writeColumn)

                        ' Write to new matrix
                        writeValue = 0
                        Select Case rhs.Operation
                            Case SumOperationType.Grouping
                                ' Just increment value
                                writeValue = _operationsHelper.CalculateValue(theSavedValue, theValue, SumOperationType.Addition, Me._model)
                            Case SumOperationType.Addition
                                If calculateWithConstant Then
                                    ' Calculate with constant
                                    writeValue = _operationsHelper.CalculateValue(theValue, rhs.ConstantValue, SumOperationType.Addition, Me._model)
                                ElseIf ((rhsValueCodes.Count = 2) AndAlso (rhsValueCodes(0) = rhsValueCodes(1))) Then
                                    'Special case when we want to add one value with itself
                                    writeValue = _operationsHelper.CalculateValue(theValue, theValue, SumOperationType.Addition, Me._model)
                                Else
                                    ' Just increment value
                                    writeValue = _operationsHelper.CalculateValue(theSavedValue, theValue, SumOperationType.Addition, Me._model)
                                End If
                            Case SumOperationType.Division
                                If calculateWithConstant And Not rhs.ConstantValue <> 0 Then
                                    Throw New PXOperationException("Constant value cannot be zero!")
                                End If
                                If sumValueCounter > valueShiftInterval And Not calculateWithConstant Then
                                    ' Perform division with saved value and new value
                                    If firstSumValue = False Then
                                        ' The previous value was selected first
                                        writeValue = _operationsHelper.CalculateValue(theSavedValue, theValue, SumOperationType.Division, Me._model, 2)
                                    Else
                                        ' The current value was selected first
                                        writeValue = _operationsHelper.CalculateValue(theValue, theSavedValue, SumOperationType.Division, Me._model, 2)
                                    End If
                                Else
                                    ' Set the value for use in next loop, or calculate with the constant
                                    If calculateWithConstant Then
                                        writeValue = _operationsHelper.CalculateValue(theValue, rhs.ConstantValue, SumOperationType.Division, Me._model, 2)
                                    ElseIf ((rhsValueCodes.Count = 2) AndAlso (rhsValueCodes(0) = rhsValueCodes(1))) Then
                                        'Special case when we want to divide one value with itself
                                        writeValue = 1
                                    Else
                                        ' Save for use in next loop
                                        writeValue = theValue
                                    End If
                                End If
                            Case SumOperationType.Multiplication
                                If sumValueCounter > valueShiftInterval And Not calculateWithConstant Then
                                    ' Perform multiplication with saved value and new value
                                    writeValue = _operationsHelper.CalculateValue(theSavedValue, theValue, SumOperationType.Multiplication, Me._model)
                                Else
                                    ' Set the value for use in next loop, or calculate with the constant
                                    If calculateWithConstant Then
                                        writeValue = _operationsHelper.CalculateValue(theValue, rhs.ConstantValue, SumOperationType.Multiplication, Me._model)
                                    ElseIf ((rhsValueCodes.Count = 2) AndAlso (rhsValueCodes(0) = rhsValueCodes(1))) Then
                                        'Special case when we want to multiply one value with itself
                                        writeValue = _operationsHelper.CalculateValue(theValue, theValue, SumOperationType.Multiplication, Me._model)
                                    Else
                                        ' Save for use in next loop
                                        writeValue = theValue
                                    End If
                                End If

                            Case SumOperationType.Subtraction
                                If sumValueCounter > valueShiftInterval And Not calculateWithConstant Then
                                    ' Perform subtraction with saved value and new value
                                    If firstSumValue = False Then
                                        ' The previous value was selected first
                                        writeValue = _operationsHelper.CalculateValue(theSavedValue, theValue, SumOperationType.Subtraction, Me._model)
                                    Else
                                        ' The current value was selected first
                                        writeValue = _operationsHelper.CalculateValue(theValue, theSavedValue, SumOperationType.Subtraction, Me._model)
                                    End If
                                Else
                                    ' Set the value for use in next loop, or calculate with the constant
                                    If calculateWithConstant Then
                                        writeValue = _operationsHelper.CalculateValue(theValue, rhs.ConstantValue, SumOperationType.Subtraction, Me._model)
                                    ElseIf ((rhsValueCodes.Count = 2) AndAlso (rhsValueCodes(0) = rhsValueCodes(1))) Then
                                        'Special case when we want to subtract one value with itself
                                        writeValue = 0
                                    Else
                                        ' Save for use in next loop
                                        writeValue = theValue
                                    End If
                                End If
                        End Select

                        ' Write to matrix
                        data.WriteElement(writeRow, writeColumn, writeValue)
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' Check if current Sum Variable Value is one of the selected values
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsSumVariableValueSelected() As Boolean
            If Me._sumVariablePlacement = PlacementType.Stub Then
                Return Me._sumSelectedIndexs.ContainsKey(Me._stubVariablesValueIndex(Me._sumVariableIndex))
            End If
            Return Me._sumSelectedIndexs.ContainsKey(Me._headVariablesValueIndex(Me._sumVariableIndex))
        End Function

        ''' <summary>
        ''' Transfer functionality for the CalculatePerPart Operation
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="theNewVariable"></param>
        ''' <param name="rhs"></param>
        ''' <remarks></remarks>
        Public Sub CalculatePerPartTransferData(ByRef data As PXData, ByVal theNewVariable As Variable, ByVal rhs As CalculatePerPartDescription)
            Dim calcValueIndex As Integer = 0 'Index of the value in theNewVariable that the calculation is based upon. Hardcoded now... 
            Dim calculatedIndexChange As Integer 'Offset for the calculated value
            Dim valueShiftInterval As Integer
            Dim writeColumn As Integer
            Dim writeRow As Integer
            Dim calcDataColumn As Integer
            Dim calcDataRow As Integer
            Dim theValue As Double
            Dim indexPositionChange As Integer
            Dim newVariableIndex As Integer
            Dim intervalCounter As Integer = 0
            Dim calculatedValue As Double
            Dim rhsKeepValues As Boolean

            If TypeOf rhs Is CalculatePerPartDescription Then
                rhsKeepValues = rhs.KeepValue
            Else
                Throw New PXOperationException("Paramater not suported")
            End If

            'Initiate variables
            '------------------
            If rhsKeepValues Then
                InitiateTransferData(theNewVariable, newVariableIndex, indexPositionChange, valueShiftInterval)
            End If

            For rowIndex As Integer = 0 To Me._model.Data.MatrixRowCount - 1
                Me.CalcStubWeights(rowIndex)

                'Handle row counters
                '-------------------
                If rhsKeepValues Then
                    If theNewVariable.Placement = PlacementType.Stub Then
                        calculatedIndexChange = 0

                        calculatedIndexChange = indexPositionChange * intervalCounter

                        ' increment interval counter if we have done a full round
                        If (rowIndex + 1) Mod valueShiftInterval = 0 Then intervalCounter = intervalCounter + 1
                    End If

                    ' Reset if the variable is in the heading
                    If theNewVariable.Placement = PlacementType.Heading Then
                        intervalCounter = 0
                    End If
                End If

                For colIndex As Integer = 0 To Me._model.Data.MatrixColumnCount - 1
                    Me.CalcHeadingWeights(colIndex)

                    'Handle column counters
                    '----------------------
                    If rhsKeepValues Then
                        If theNewVariable.Placement = PlacementType.Heading Then

                            calculatedIndexChange = indexPositionChange * intervalCounter

                            ' increment interval counter if we have done a full round
                            If (colIndex + 1) Mod valueShiftInterval = 0 Then intervalCounter = intervalCounter + 1
                        End If
                    End If

                    'Read from old data
                    '------------------
                    theValue = Me._model.Data.ReadElement(rowIndex, colIndex)

                    'Calculate where to write in new data
                    '------------------------------------
                    If rhsKeepValues Then
                        ' Set the positions to write in matrix
                        If theNewVariable.Placement = PlacementType.Heading Then
                            writeRow = rowIndex
                            writeColumn = colIndex + calculatedIndexChange

                            calcDataRow = writeRow
                            calcDataColumn = writeColumn + indexPositionChange * (theNewVariable.Values.Count - 1 - calcValueIndex)
                        Else
                            writeColumn = colIndex
                            writeRow = rowIndex + calculatedIndexChange

                            calcDataColumn = writeColumn
                            calcDataRow = writeRow + indexPositionChange * (theNewVariable.Values.Count - 1 - calcValueIndex)
                        End If
                    Else
                        ' Write to the same position in the matrix - replacing the original value
                        writeRow = rowIndex
                        writeColumn = colIndex
                    End If

                    'Write to new data
                    '-----------------
                    If rhsKeepValues Then
                        Dim writeCalcValue As Boolean 'True if row/column belongs to the value that the calculation is based upon

                        'Write to new data
                        data.WriteElement(writeRow, writeColumn, theValue)
                        data.WriteDataNoteCellElement(writeRow, writeColumn, Me._model.Data.ReadDataCellNoteElement(rowIndex, colIndex))
                        If theNewVariable.Values.Count > 2 Then
                            'Check if it is the value the calculation is based upon...
                            If theNewVariable.Placement = PlacementType.Heading Then
                                If calcValueIndex = _headVariablesValueIndex(newVariableIndex) Then
                                    writeCalcValue = True
                                Else
                                    writeCalcValue = False
                                End If
                            Else
                                If calcValueIndex = _stubVariablesValueIndex(newVariableIndex) Then
                                    writeCalcValue = True
                                Else
                                    writeCalcValue = False
                                End If
                            End If
                        Else
                            'Allways calculate if there are only two values in on the sortvariable
                            writeCalcValue = True
                        End If

                        If writeCalcValue Then
                            'Get and write the calculated data
                            calculatedValue = CalculatePerPartGetCalculatedValue(rowIndex, colIndex, rhs)
                            data.WriteElement(calcDataRow, calcDataColumn, calculatedValue)
                            data.WriteDataNoteCellElement(calcDataRow, calcDataColumn, Me._model.Data.ReadDataCellNoteElement(rowIndex, colIndex))
                        End If
                    Else
                        'Get and write the calculated data
                        calculatedValue = CalculatePerPartGetCalculatedValue(rowIndex, colIndex, rhs)
                        data.WriteElement(writeRow, writeColumn, calculatedValue)
                        data.WriteDataNoteCellElement(writeRow, writeColumn, Me._model.Data.ReadDataCellNoteElement(rowIndex, colIndex))
                    End If

                Next
            Next

        End Sub


        ''' <summary>
        ''' Get the calculated value for the CalculatePerPart Operation.
        ''' Calculation is dependant on the CalculationPerPartSelectionType
        ''' </summary>
        ''' <param name="row">Row index</param>
        ''' <param name="column">Column index</param>
        ''' <param name="rhs">Calculation description</param>
        ''' <returns>Calculated value</returns>
        ''' <remarks></remarks>
        Private Function CalculatePerPartGetCalculatedValue(ByVal row As Integer, ByVal column As Integer, ByVal rhs As CalculatePerPartDescription) As Double
            Dim calcValue As Double = 0
            Dim modelValue As Double = 0
            Dim returnValue As Double = 0
            Dim matrixHelper As New PMatrixHelper(Me._model)

            Select Case rhs.CalculationVariant
                Case CalculatePerPartSelectionType.OneMatrixValue
                    calcValue = GetOneMatrixValueCalculationValue(rhs, matrixHelper)

                Case CalculatePerPartSelectionType.OneVariableAllValues
                    calcValue = GetOneVariableAllValuesCalculationValue(row, column, rhs, matrixHelper)

                Case CalculatePerPartSelectionType.OneVariableOneValue
                    calcValue = GetOneVariableOneValueCalculationValue(row, column, rhs, matrixHelper)

                Case Else
                    Throw New PXOperationException("Calculation variant not supported")
            End Select

            modelValue = Me._model.Data.ReadElement(row, column)

            Dim unitValue As Integer = 0
            If rhs.OperationType = CalculatePerPartType.PerCent Then
                unitValue = 100
            Else
                unitValue = 1000
            End If

            ' return in unit format
            modelValue = _operationsHelper.CalculateValue(modelValue, unitValue, SumOperationType.Multiplication, matrixHelper._model)
            returnValue = _operationsHelper.CalculateValue(modelValue, calcValue, SumOperationType.Division, matrixHelper._model, 2)

            Return returnValue
        End Function

        ''' <summary>
        ''' Returns value to use in the OneMatrixValue calculation
        ''' </summary>
        ''' <param name="rhs">Calculation description</param>
        ''' <returns>Value to use in the OneMatrixValue calculation</returns>
        ''' <remarks></remarks>
        Private Function GetOneMatrixValueCalculationValue(ByVal rhs As CalculatePerPartDescription, _
                                                           ByVal matrixHelper As PMatrixHelper) As Double
            Dim startCheckIndex As Integer
            Dim iFoundTheValue As Boolean

            If _calculatePerPartValueHasBeenSet = False Then
                ' Get the value from oldModel
                For matrixRow As Integer = 0 To Me._model.Data.MatrixRowCount - 1
                    matrixHelper.CalcStubWeights(matrixRow)
                    For matrixCol As Integer = 0 To Me._model.Data.MatrixColumnCount - 1
                        matrixHelper.CalcHeadingWeights(matrixCol)
                        iFoundTheValue = True

                        ' Check if we are at the position in the matrix for the selected variable values
                        ' Start to check the stub variables
                        startCheckIndex = 0
                        If matrixHelper._stubVariablesValueIndex IsNot Nothing Then
                            For i As Integer = 0 To matrixHelper._model.Meta.Stub.Count - 1
                                If rhs.ValueSelection(i).ValueCodes(0) <> matrixHelper._model.Meta.Stub(i).Values(matrixHelper._stubVariablesValueIndex(i)).Code Then
                                    iFoundTheValue = False
                                    Exit For
                                End If
                            Next
                        End If

                        If iFoundTheValue = True Then
                            ' All variable values were correct in the stub
                            ' Now check the heading
                            startCheckIndex = matrixHelper._model.Meta.Stub.Count
                            If matrixHelper._headVariablesValueIndex IsNot Nothing Then
                                For i As Integer = 0 To matrixHelper._model.Meta.Heading.Count - 1
                                    If rhs.ValueSelection(startCheckIndex + i).ValueCodes(0) <> matrixHelper._model.Meta.Heading(i).Values(matrixHelper._headVariablesValueIndex(i)).Code Then
                                        iFoundTheValue = False
                                        Exit For
                                    End If
                                Next

                                ' Did we find it?
                                If iFoundTheValue Then
                                    ' Save value for next round so we do not fetch the value all the time
                                    _calculatePerPartOneMatrixValue = matrixHelper._model.Data.ReadElement(matrixRow, matrixCol)
                                    ' to know if we have fetched the matrix value
                                    _calculatePerPartValueHasBeenSet = True

                                    ' Assign value to calculate with
                                    Return _calculatePerPartOneMatrixValue
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                ' Return the fetched matrix value
                Return _calculatePerPartOneMatrixValue
            End If
        End Function

        ''' <summary>
        ''' Returns value to use in the OneVariableAllValues calculation
        ''' </summary>
        ''' <param name="row">Row index</param>
        ''' <param name="column">Column index</param>
        ''' <param name="rhs">Calculation description</param>
        ''' <returns>Value to use in the OneVariableAllValues calculation</returns>
        ''' <remarks></remarks>
        Private Function GetOneVariableAllValuesCalculationValue(ByVal row As Integer, _
                                                       ByVal column As Integer, _
                                                       ByVal rhs As CalculatePerPartDescription, _
                                                       ByVal matrixHelper As PMatrixHelper) As Double
            Dim calcValue As Double = 0
            Dim variableSelection As Selection = Nothing
            Dim theVariable As Variable = Nothing
            Dim variableIndex As Integer = 0

            variableSelection = GetVariableSelection(rhs)

            theVariable = matrixHelper._model.Meta.Variables.GetByCode(variableSelection.VariableCode)

            If theVariable.Placement = PlacementType.Heading Then
                variableIndex = matrixHelper._model.Meta.Heading.IndexOf(theVariable)

                ' Check for which column to start
                matrixHelper.CalcHeadingWeights(column)


                'Find the column of the base value
                Dim baseValueColumn As Integer = 0

                baseValueColumn = GetBaseValueIndex(variableIndex, _
                         matrixHelper._headVariablesValueIndex, _
                         matrixHelper.PMHeading)

                'Read the base value
                calcValue = 0
                For z As Integer = 0 To theVariable.Values.Count - 1
                    calcValue = PXData.AddElements(calcValue, Me._model.Data.ReadElement(row, baseValueColumn + (z * matrixHelper.PMHeading(variableIndex + 1))))
                Next
            Else
                variableIndex = matrixHelper._model.Meta.Stub.IndexOf(theVariable)

                ' Check for which row to start
                matrixHelper.CalcStubWeights(row)


                'Find the row of the base value
                Dim baseValueRow As Integer = 0

                baseValueRow = GetBaseValueIndex(variableIndex, _
                                                matrixHelper._stubVariablesValueIndex, _
                                                matrixHelper.PMStub)

                'Read the base value
                calcValue = 0
                For z As Integer = 0 To theVariable.Values.Count - 1
                    calcValue = PXData.AddElements(calcValue, Me._model.Data.ReadElement(baseValueRow + (z * matrixHelper.PMStub(variableIndex + 1)), column))
                Next
            End If

            Return calcValue
        End Function

        ''' <summary>
        ''' Returns value to use in the OneVariableOneValue calculation
        ''' </summary>
        ''' <param name="row">Row index</param>
        ''' <param name="column">Column index</param>
        ''' <param name="rhs">Calculation description</param>
        ''' <returns>Value to use in the OneVariableOneValue calculation</returns>
        ''' <remarks></remarks>
        Private Function GetOneVariableOneValueCalculationValue(ByVal row As Integer, _
                                                       ByVal column As Integer, _
                                                       ByVal rhs As CalculatePerPartDescription, _
                                                       ByVal matrixHelper As PMatrixHelper) As Double
            Dim calcValue As Double = 0
            Dim variableSelection As Selection = Nothing
            Dim theVariable As Variable = Nothing
            Dim variableIndex As Integer = 0

            variableSelection = GetVariableSelection(rhs)

            theVariable = matrixHelper._model.Meta.Variables.GetByCode(variableSelection.VariableCode)

            'Index within the variable for the base value - Only 1 in the calculation variant
            Dim baseValueIndex As Integer = theVariable.Values.GetIndexByCode(variableSelection.ValueCodes(0))

            If theVariable.Placement = PlacementType.Heading Then
                variableIndex = matrixHelper._model.Meta.Heading.IndexOf(theVariable)

                ' Check for which column to start
                matrixHelper.CalcHeadingWeights(column)


                'Find the column of the base value
                Dim baseValueColumn As Integer = 0

                baseValueColumn = GetBaseValueIndex(variableIndex, _
                         matrixHelper._headVariablesValueIndex, _
                         matrixHelper.PMHeading)

                baseValueColumn = baseValueColumn + (baseValueIndex * matrixHelper.PMHeading(variableIndex + 1))

                'Read the base value
                calcValue = Me._model.Data.ReadElement(row, baseValueColumn)

            Else
                variableIndex = matrixHelper._model.Meta.Stub.IndexOf(theVariable)

                ' Check for which row to start
                matrixHelper.CalcStubWeights(row)

                'Find the row of the base value
                Dim baseValueRow As Integer

                baseValueRow = GetBaseValueIndex(variableIndex, _
                                                 matrixHelper._stubVariablesValueIndex, _
                                                 matrixHelper.PMStub)

                baseValueRow = baseValueRow + (baseValueIndex * matrixHelper.PMStub(variableIndex + 1))

                'Read the base value
                calcValue = Me._model.Data.ReadElement(baseValueRow, column)

            End If

            Return calcValue
        End Function

        ''' <summary>
        ''' Returns the selection from the description
        ''' </summary>
        ''' <param name="rhs">Description object</param>
        ''' <returns>Selection object</returns>
        ''' <remarks></remarks>
        Private Function GetVariableSelection(ByVal rhs As CalculatePerPartDescription) As Selection
            Dim variableSelection As Selection = Nothing

            ' Get the variable/value selection
            For i As Integer = 0 To rhs.ValueSelection.Count - 1
                If rhs.ValueSelection(i).ValueCodes.Count > 0 Then
                    'assign to work with
                    variableSelection = rhs.ValueSelection(i)
                    Exit For
                End If
            Next

            If variableSelection Is Nothing Then
                Throw New PXOperationException("VariableValue selection missing!")
            End If

            Return variableSelection
        End Function

        ''' <summary>
        ''' Returns the index of the row/column of the value to base the per part calculation upon
        ''' </summary>
        ''' <param name="variableIndex">Index of the variable to base the calculation upon</param>
        ''' <param name="valueIndexes">Integer array with variable indexes</param>
        ''' <param name="valueWeights">Integer array with variable weights</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Offset for the value in CalculatePerPartSelectionType.OneVariableOneValue CalculationVariant
        ''' is not included. Must be added outside this function
        ''' </remarks>
        Private Function GetBaseValueIndex(ByVal variableIndex As Integer, _
                                           ByVal valueIndexes As Integer(), _
                                           ByVal valueWeights As Integer()) As Integer
            Dim baseValueIndex As Integer = 0

            For x As Integer = 0 To valueIndexes.Count - 1
                ' Do not check for the selected variable
                If x <> variableIndex Then
                    baseValueIndex = baseValueIndex + (valueIndexes(x) * valueWeights(x + 1))
                End If
            Next

            Return baseValueIndex
        End Function

        ''' <summary>
        ''' Initiates variables of the CalculatePerPartTransferData method
        ''' </summary>
        ''' <param name="theNewVariable">The new variable</param>
        ''' <param name="newVariableIndex">Index of the new variable</param>
        ''' <param name="indexPositionChange"></param>
        ''' <param name="valueShiftInterval"></param>
        ''' <remarks></remarks>
        Private Sub InitiateTransferData( _
                            ByVal theNewVariable As Variable, _
                            ByRef newVariableIndex As Integer, _
                            ByRef indexPositionChange As Integer, _
                            ByRef valueShiftInterval As Integer)

            ' Find the new variables index position
            If theNewVariable.Placement = PlacementType.Heading Then
                newVariableIndex = _newMeta.Heading.IndexOf(theNewVariable)
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(newVariableIndex, Me.NewMeta.Heading)
                valueShiftInterval = Me._PMHeading(newVariableIndex)
            Else
                newVariableIndex = _newMeta.Stub.IndexOf(theNewVariable)
                indexPositionChange = _operationsHelper.GetVariableIndexSpan(newVariableIndex, Me.NewMeta.Stub)
                valueShiftInterval = Me._PMStub(newVariableIndex)
            End If
        End Sub
    End Class
End Namespace