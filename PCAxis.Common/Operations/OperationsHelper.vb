Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Functionality shared by operations
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OperationsHelper
        ''' <summary>
        ''' Pivots a model to be in the same order as the templateModel
        ''' The model to change MUST have the same variables as the current model
        ''' </summary>
        ''' <param name="templateModel"></param>
        ''' <param name="modelToChange"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PivotModelToMatch(ByVal templateModel As PXModel, ByVal modelToChange As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel
            Dim pivotOperation As New PCAxis.Paxiom.Operations.Pivot()
            Dim pivotDescriptions As PCAxis.Paxiom.Operations.PivotDescription()
            Dim pv As PCAxis.Paxiom.Operations.PivotDescription
            Dim pivotedModel As PCAxis.Paxiom.PXModel

            pivotDescriptions = New PCAxis.Paxiom.Operations.PivotDescription(templateModel.Meta.Variables.Count - 1) {}

            REM Loop templateModel variables and add to pivotDescription
            Dim pivotDescriptionIndex As Integer = -1
            For i As Integer = 0 To templateModel.Meta.Variables.Count - 1
                pivotDescriptionIndex += 1
                pv = New PCAxis.Paxiom.Operations.PivotDescription()
                pv.VariableName = templateModel.Meta.Variables(i).Name
                pv.VariablePlacement = templateModel.Meta.Variables(i).Placement

                pivotDescriptions(pivotDescriptionIndex) = pv
            Next

            pivotedModel = pivotOperation.Execute(modelToChange, pivotDescriptions)

            Return pivotedModel
        End Function

        ''' <summary>
        ''' Use templateModel for changing incoming Models valueorder for the variable codes in the variableCodes array
        ''' </summary>
        ''' <param name="templateModel">PXModel to change valueorder for</param>
        ''' <param name="changeModel">PXModel to change valueorder for</param>
        ''' <param name="variableCodes">String array with Variable codes to change value order for</param>
        ''' <returns>Changed PXModel</returns>
        ''' <remarks></remarks>
        Public Function ChangeValueOrders(ByVal templateModel As PXModel, ByVal changeModel As PXModel, ByVal variableCodes As String()) As PXModel
            Dim changedModel As PXModel
            Dim changeValueOrderDescription As ChangeValueOrderDescription = New ChangeValueOrderDescription()
            Dim changeValueOrderOperation As ChangeValueOrder = New ChangeValueOrder()
            Dim modWeight As Integer()

            changedModel = changeModel
            ' Loop variables and compare the value order of the models
            Dim oldVar As Variable
            Dim newVar As Variable
            For i As Integer = 0 To variableCodes.Length - 1
                If (variableCodes(i) Is Nothing) = False Then
                    oldVar = templateModel.Meta.Variables.GetByCode(variableCodes(i))
                    newVar = changeModel.Meta.Variables.GetByCode(variableCodes(i))

                    modWeight = GetModWeight(oldVar, newVar)
                    changeValueOrderDescription.ModifiedVariableValueWeight = modWeight
                    changeValueOrderDescription.VariableCode = variableCodes(i)

                    changedModel = changeValueOrderOperation.Execute(changedModel, changeValueOrderDescription)
                End If
            Next

            Return changedModel
        End Function

        ''' <summary>
        ''' Initializes an array to act as a translation service for the value index
        ''' </summary>
        ''' <param name="templateVariable"></param>
        ''' <param name="newVariable"></param>
        ''' <remarks></remarks>
        Private Function GetModWeight(ByVal templateVariable As PCAxis.Paxiom.Variable, ByVal newVariable As PCAxis.Paxiom.Variable) As Integer()
            Dim modWeight As Integer()
            modWeight = New Integer(newVariable.Values.Count - 1) {}

            For i As Integer = 0 To newVariable.Values.Count - 1
                modWeight(i) = templateVariable.Values.GetIndexByCode(newVariable.Values(i).Code)
            Next

            Return modWeight
        End Function

        ''' <summary>
        ''' Check if checkModel has same variables as the templateModel
        ''' </summary>
        ''' <param name="templateModel"></param>
        ''' <param name="checkModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckModelVariables(ByVal templateModel As PXModel, ByVal checkModel As PXModel) As Boolean
            ' Same number of variables?
            If templateModel.Meta.Variables.Count <> checkModel.Meta.Variables.Count Then
                Return False
            End If

            ' Verify that all variables in model1 is available in checkModel
            Dim tmpVar As Variable
            Dim returnValue As Boolean = True
            For Each var As Variable In templateModel.Meta.Variables
                tmpVar = checkModel.Meta.Variables.GetByCode(var.Code)
                If tmpVar Is Nothing Then
                    returnValue = False
                End If
            Next

            Return returnValue
        End Function

        ''' <summary>
        ''' Get the first Variable where the Value collection differs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="changeModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetChangedVariable(ByVal oldModel As PXModel, ByVal changeModel As PXModel) As Variable
            Dim checkVar As Variable
            Dim returnVar As Variable = Nothing

            For Each var As Variable In oldModel.Meta.Variables
                checkVar = changeModel.Meta.Variables.GetByCode(var.Code)
                'If var.Values.Count <> checkVar.Values.Count Then
                '    ' Number of values differ - this is the one
                '    returnVar = var
                '    Exit For
                'End If

                For Each val As Value In checkVar.Values
                    'If var.Values.GetByCode(val.Code) Is Nothing Then
                    '    ' Value in changeModel variable was not found in oldModel variable
                    '    ' this is the changed variable
                    '    returnVar = var
                    '    Exit For
                    'End If
                    If var.Values.GetByName(val.Value) Is Nothing Then
                        ' Value in changeModel variable was not found in oldModel variable
                        ' this is the changed variable
                        returnVar = var
                        Exit For
                    End If
                Next
            Next

            Return returnVar
        End Function

        '''' <summary>
        '''' Returns a value by code or value .... TODO...
        '''' </summary>
        '''' <param name="variable"></param>
        '''' <param name="value"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Shared Function GetValue(ByVal variable As Variable, ByVal value As Value) As Value
        '    If variable.Values.IsCodesFictional Then
        '        Return variable.Values.GetByName(value.Value)
        '    Else
        '        Return variable.Values.GetByCode(value.Code)
        '    End If
        'End Function

        ''' <summary>
        ''' Creates a new PXData object
        ''' </summary>
        ''' <param name="meta">PXMeta object for the model</param>
        ''' <returns>
        ''' A new PXData object with a datamatrix that corresponds to the number of variables and
        ''' values in the PXMeta object
        ''' </returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Get the Value index position for the Sum value
        ''' </summary>
        ''' <param name="oldVar"></param>
        ''' <param name="valueCodes"></param>
        ''' <param name="keepValues"></param>
        ''' <returns></returns>
        ''' <remarks>The value position will be after the last selected item</remarks>
        Public Function SumVariableCalculateSumValueIndex(ByVal oldVar As Variable, ByVal valueCodes As List(Of String), ByVal keepValues As Boolean) As Integer
            Dim result As Integer
            Dim tmpIndex As Integer
            Dim numberOfExistingCodes As Integer = 0

            'sum all
            If valueCodes.Count = oldVar.Values.Count Then
                If keepValues Then
                    Return oldVar.Values.Count
                Else
                    Return 0
                End If
            End If

            Dim max As Integer = 0
            For Each valCode As String In valueCodes
                tmpIndex = oldVar.Values.GetIndexByCode(valCode)
                max = Math.Max(max, tmpIndex)
                ' When grouping - not all values might be present. We need to keep track of number of existing values
                If tmpIndex > -1 Then numberOfExistingCodes = numberOfExistingCodes + 1
            Next
            If keepValues Then
                Return max + 1
            End If

            'result = max - valueCodes.Count + 1
            If numberOfExistingCodes = 0 Then
                ' Special case for grouping - put sum value last
                result = oldVar.Values.Count
            Else
                result = max - numberOfExistingCodes + 1
            End If

            Return result
        End Function

        ''' <summary>
        ''' Check that the models has the same variables and values.
        ''' Returns an array, emtpy or with variable codes that need to change value order
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="checkModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EnsureSameVariablesAndValues(ByRef oldModel As PXModel, ByRef checkModel As PXModel) As String()
            Dim newVar As Variable
            Dim newValue As Value
            Dim variableCodesWithDifferentValueIndexPosition As String()
            Dim arrayIndex As Integer = 0
            Dim variableAdded As Boolean

            variableCodesWithDifferentValueIndexPosition = New String(oldModel.Meta.Variables.Count - 1) {}

            ' Loop variables in oldModel and verify that they are present in the checkModel
            For Each oldVar As Variable In oldModel.Meta.Variables
                newVar = checkModel.Meta.Variables.GetByCode(oldVar.Code)
                If newVar Is Nothing Then
                    Throw New PXOperationException("Variable (" + oldVar.Name + ") was missing in the overlay model.")
                End If

                If oldVar.Values.Count <> newVar.Values.Count Then
                    Throw New PXOperationException("Variable (" + oldVar.Name + ") did not have the same values in the overlay model.")
                End If

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
            Next

            Return variableCodesWithDifferentValueIndexPosition
        End Function

        ''' <summary>
        ''' Perform operation on firstValue and secondValue
        ''' </summary>
        ''' <param name="firstValue"></param>
        ''' <param name="secondValue"></param>
        ''' <param name="operation">+, -, /, *</param>
        ''' <param name="model"></param>
        ''' <returns>double</returns>
        ''' <remarks>If firstValue OR secondValue is equal to PXConstant.DATASYMBOL_1 than PXConstant.DATASYMBOL_1 is returned</remarks>
        Public Function CalculateValue(ByVal firstValue As Double, ByVal secondValue As Double, ByVal operation As SumOperationType, ByVal model As PXModel) As Double
            Return CalculateValue(firstValue, secondValue, operation, model, 0)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="firstValue"></param>
        ''' <param name="secondValue"></param>
        ''' <param name="operation">+, -, /, *</param>
        ''' <param name="model"></param>
        ''' <param name="precision">Number of decimals</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateValue(ByVal firstValue As Double, ByVal secondValue As Double, ByVal operation As SumOperationType, ByVal model As PXModel, ByVal precision As Integer) As Double
            Dim returnValue As Double = 0

            ' Handle special constant values
            If Array.IndexOf(PXConstant.ProtectedValues, firstValue) > -1 And Array.IndexOf(PXConstant.ProtectedValues, secondValue) > -1 Then
                If firstValue = secondValue Then
                    Return firstValue ' calculation with same symbol gives the symbol as result
                Else
                    Return PXConstant.DATASYMBOL_7 ' calculation with 2 different datasymbols gives the datasymbolsum as result
                End If
            ElseIf Array.IndexOf(PXConstant.ProtectedValues, firstValue) > -1 Then
                ' No calculation done - the result is the DataSymbol
                If firstValue = PXConstant.DATASYMBOL_NIL Then
                    ' Calculate with NIL value as zero
                    firstValue = 0
                Else
                    Return firstValue
                End If
            ElseIf Array.IndexOf(PXConstant.ProtectedValues, secondValue) > -1 Then
                ' No calculation done - the result is the DataSymbol
                If secondValue = PXConstant.DATASYMBOL_NIL Then
                    ' Calculate with NIL value as zero
                    secondValue = 0
                Else
                    Return secondValue
                End If
            End If

            Select Case operation
                Case SumOperationType.Subtraction
                    returnValue = firstValue - secondValue
                Case SumOperationType.Addition
                    returnValue = firstValue + secondValue
                Case SumOperationType.Division
                    If secondValue = 0 Then
                        returnValue = PXConstant.DATASYMBOL_1
                    Else
                        'returnValue = Math.Round(firstValue / secondValue)
                        Dim dataFormatter As New DataFormatter(model)
                        'TODO: blir fel DecimalPrecision
                        returnValue = Math.Round(firstValue / secondValue, precision, dataFormatter.RoundingRule)
                    End If
                Case SumOperationType.Multiplication
                    returnValue = firstValue * secondValue
            End Select

            Return returnValue
        End Function

        ''' <summary>
        ''' Get the number of decimals to use for the Variable (precision)
        ''' </summary>
        ''' <param name="var"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetVariablePrecision(ByVal var As Variable) As Integer
            Dim dec As Integer = 0

            Dim val As Value
            For j As Integer = 0 To var.Values.Count - 1
                val = var.Values(j)
                dec = Math.Max(val.Precision, dec)
            Next

            Return dec
        End Function

        ''' <summary>
        ''' Get number of decimals for the value
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNumberOfDecimals(ByVal value As Double) As Integer
            Dim returnValue As Integer

            If value.ToString().IndexOf(".") < 0 And value.ToString().IndexOf(",") < 0 Then
                returnValue = 0 ' No decimals
            Else
                ' Get number of decimals
                returnValue = value.ToString("R").Substring(value.ToString().Replace(",", ".").IndexOf(".") + 1).Length
            End If

            Return returnValue
        End Function

        ''' <summary>
        ''' How many indexpositions does the Variable at sumVariableIndex span?
        ''' </summary>
        ''' <param name="variableIndex"></param>
        ''' <param name="variables"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetVariableIndexSpan(ByVal variableIndex As Integer, ByVal variables As Variables) As Integer
            Dim sum As Integer = 1
            ' Start loop at first variable below the sumvariable
            Dim loopStart As Integer = variableIndex + 1

            If loopStart <= variables.Count - 1 Then
                For i As Integer = loopStart To variables.Count - 1
                    sum *= variables(i).Values.Count
                Next
            End If

            Return sum
        End Function
    End Class
End Namespace