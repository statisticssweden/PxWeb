Imports PCAxis.Paxiom.Extensions

Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class for handling CalculatePerPart Operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalculatePerPart
        Implements PCAxis.Paxiom.IPXOperation
        Private _pMatrixHelper As PMatrixHelper
        Private newModel As PCAxis.Paxiom.PXModel
        Private newMeta As PCAxis.Paxiom.PXMeta
        Private newData As PCAxis.Paxiom.PXData

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is CalculatePerPartDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, CalculatePerPartDescription))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' The new model will only contain the currently selected language. All other languages will be removed
        ''' </remarks>
        Public Function Execute(ByVal oldModel As PXModel, ByVal rhs As CalculatePerPartDescription) As PXModel
            newModel = oldModel.CreateCopy()
            newMeta = newModel.Meta
            newData = newModel.Data

            'Keep only the current language
            If newModel.Meta.NumberOfLanguages > 1 Then
                newModel.Meta.DeleteAllLanguagesExceptCurrent()
            End If

            Dim newVar As Variable = Nothing

            ValidateExecutionParameters(rhs)

            _pMatrixHelper = New PMatrixHelper(oldModel)


            If rhs.KeepValue Then
                AddSortVariable(rhs)
            Else
                ModifyExistingVariable(rhs)
            End If

            ' Get a copy from new meta
            newVar = newMeta.Variables.GetByCode(PXConstant.SORTVARIABLE)

            ' Set new meta in pmatrixhelper to setup the support structure
            _pMatrixHelper.NewMeta = newMeta

            'Create space for the new data
            newData.SetMatrixSize(newMeta.GetRowCount(), newMeta.GetRowLength())

            _pMatrixHelper.CalculatePerPartTransferData(newData, newVar, rhs)

            ' Create new model, its title and set as complete.
            newModel.Meta.Prune()
            newModel.Meta.CreateTitle()
            newModel.IsComplete = True

            Return newModel
        End Function

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Validates that the calculation description has valid parameters
        ''' </summary>
        ''' <param name="rhs">The calculation description</param>
        ''' <remarks>Throws exception if invalid parameters</remarks>
        Private Sub ValidateExecutionParameters(ByVal rhs As CalculatePerPartDescription)
            If rhs.ValueName Is Nothing OrElse rhs.ValueName.Trim() = String.Empty Then
                Throw New PXOperationException("Value name is missing.")
            End If
            If rhs.ValueSelection.Count < 1 Then
                Throw New PXOperationException("Variable/Value selection is missing.")
            End If
        End Sub

        ''' <summary>
        ''' Creates a new sort variable and inserts it into the new meta
        ''' </summary>
        ''' <param name="rhs">calculation description</param>
        ''' <remarks></remarks>
        Private Sub AddSortVariable(ByVal rhs As CalculatePerPartDescription)
            Dim newVar As Variable = Nothing
            Dim newVariablePlacement As PlacementType
            Dim newVariableIndexInCollection As Integer

            'TODO: Hur skall hanteringen av code vara - som det är nu så får Variable samma kod och namn - dvs språkhanterad....
            newVar = newModel.Meta.Variables.GetByCode(PXConstant.SORTVARIABLE)

            If newVar Is Nothing Then
                FindSortVariablePlacement(rhs, newVariablePlacement, newVariableIndexInCollection)

                newVar = New Variable(newModel.Meta.GetLocalizedSortVariableName(newModel.Meta.Language), PXConstant.SORTVARIABLE, newVariablePlacement, 0)
                newVar.SortVariable = True

                newMeta.InsertVariable(newVariableIndexInCollection, newVar)

                Dim units As String
                If Not String.IsNullOrEmpty(newMeta.ContentInfo.Units) Then
                    units = newMeta.ContentInfo.Units
                Else
                    units = "number"
                End If
                Dim value1 As New Value(units)
                newVar.Values.Add(value1)
            Else
                RaiseSortVariableAlreadyExistError(rhs)
            End If

            ' Check that the value does not exist
            If newVar.Values.GetByName(rhs.ValueName) IsNot Nothing Then
                Throw New PXOperationException("New Value name is not unique.")
            End If

            Dim value2 As New Value(rhs.ValueName)
            value2.Precision = 2
            newVar.Values.Add(value2)
            ' Ensure that the created values has Codes
            newVar.Values.SetFictionalCodes()

            If newVar.Values.Count > 1 Then
                newMeta.ContentInfo.Units = ""
            End If
        End Sub

        ''' <summary>
        ''' Modifies existing variable to display per cent (or per millage)
        ''' </summary>
        ''' <param name="rhs"></param>
        ''' <remarks></remarks>
        Private Sub ModifyExistingVariable(ByVal rhs As CalculatePerPartDescription)
            Dim val As String = Nothing
            Dim var As Variable

            If newModel.Meta.Variables.GetByCode(PXConstant.SORTVARIABLE) Is Nothing Then

                'Find first variable with selected value
                For Each s As Selection In rhs.ValueSelection
                    If s.ValueCodes.Count > 0 Then
                        val = s.VariableCode
                        Exit For
                    End If
                Next

                If val Is Nothing Then
                    Throw New PXOperationException("Variable not found")
                End If

                var = newMeta.Variables.GetByCode(val)

                For Each Value As Value In var.Values
                    Value.Precision = 2
                Next

                'Set UNIT
                Select Case rhs.OperationType
                    Case CalculatePerPartType.PerCent
                        newMeta.ContentInfo.Units = GetLocalizedString("PxcPercent")
                    Case CalculatePerPartType.PerMil
                        newMeta.ContentInfo.Units = GetLocalizedString("PxcPermil")
                End Select
            End If

        End Sub

        ''' <summary>
        ''' Called if sort variable already exist to raise appropriate error
        ''' </summary>
        ''' <param name="rhs"></param>
        ''' <remarks></remarks>
        Private Sub RaiseSortVariableAlreadyExistError(ByVal rhs As CalculatePerPartDescription)
            'Not allowed to calculate per cent or per millage if sortvariable exists.
            If rhs.OperationType = CalculatePerPartType.PerCent Then
                Throw New PXOperationException(GetLocalizedString("PxcPercentUnitVarExists"))
            ElseIf rhs.OperationType = CalculatePerPartType.PerMil Then
                Throw New PXOperationException(GetLocalizedString("PxcPermilUnitVarExists"))
                'Else
                '    ' Get from newMeta
                '    If newVar.Placement = PlacementType.Heading Then
                '        newVar = newMeta.Heading.GetByCode(PXConstant.SORTVARIABLE)
                '    Else
                '        newVar = newMeta.Stub.GetByCode(PXConstant.SORTVARIABLE)
                '    End If
            End If

        End Sub

        ''' <summary>
        ''' Find out where the new sort variable shall be placed among the variables
        ''' </summary>
        ''' <param name="rhs">calculation description</param>
        ''' <param name="newVariablePlacement">Placement stub/heading</param>
        ''' <param name="newVariableIndexInCollection">Index in stub/heading</param>
        ''' <remarks></remarks>
        Private Sub FindSortVariablePlacement( _
                              ByVal rhs As CalculatePerPartDescription, _
                              ByRef newVariablePlacement As PlacementType, _
                              ByRef newVariableIndexInCollection As Integer)

            Dim singleSelection As Selection = Nothing

            Select Case rhs.CalculationVariant
                Case CalculatePerPartSelectionType.OneMatrixValue 'One value from each variable...
                    'If at least one variable is in the stub - place the new variable there. 
                    'If not - place it in the heading
                    If newModel.Meta.Stub.Count > 0 Then
                        newVariablePlacement = PlacementType.Stub
                    Else
                        newVariablePlacement = PlacementType.Heading
                    End If

                    newVariableIndexInCollection = 0
                Case CalculatePerPartSelectionType.OneVariableAllValues, CalculatePerPartSelectionType.OneVariableOneValue
                    ' Find the selection where the value is selected
                    For i As Integer = 0 To rhs.ValueSelection.Count - 1
                        If rhs.ValueSelection(i).ValueCodes.Count > 0 Then
                            singleSelection = rhs.ValueSelection(i)
                            Exit For
                        End If
                    Next

                    If singleSelection Is Nothing Then Throw New PXOperationException("Missing selection for CalculatePerPartVariant")

                    ' Find where the parent to the selected variable is at to determine the position of the new variable
                    Dim v As Variable = newModel.Meta.Variables.GetByCode(singleSelection.VariableCode)

                    If Not v Is Nothing Then
                        If v Is newModel.Meta.Variables(0) Then
                            ' This is the first variable in the stub/heading - place new variable before it
                            newVariableIndexInCollection = 0
                            newVariablePlacement = v.Placement
                        Else
                            ' Check if we got the first variable in the Heading
                            ' If so place the new variable last in the Stub
                            If v Is newModel.Meta.Heading(0) Then
                                newVariableIndexInCollection = newModel.Meta.Stub.Count
                                newVariablePlacement = PlacementType.Stub
                            Else
                                ' Place new variable in same collection as the selected one
                                newVariablePlacement = v.Placement
                                ' and get the index where to place it
                                If newVariablePlacement = PlacementType.Heading Then
                                    newVariableIndexInCollection = newModel.Meta.Heading.IndexOf(v)
                                Else
                                    newVariableIndexInCollection = newModel.Meta.Stub.IndexOf(v)
                                End If
                            End If
                        End If
                    End If

                Case Else
                    Throw New PXOperationException("Unsupported CalculatePerPartVariant")
            End Select
        End Sub
    End Class

End Namespace