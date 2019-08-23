Imports PCAxis.Paxiom.Extensions

Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Adapter for IPXModelBuilder suitable for file based storages
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PXFileModelBuilderAdpater
        Inherits PXModelBuilderAdapter
        Implements IDisposable

        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PXFileModelBuilderAdpater))

        'The "cursor" position i the entire data matrixx including the unselected data
        Protected m_dataIndex As Integer
        Protected m_selectedRowLength As Integer
        Protected m_selectedColumnLength As Integer

        'The real number of columns in the entire data matrix includes unselected data
        Protected m_dataColumnLength As Integer
        'The real number of rows in the entire data matrix includes unselected data
        Protected m_dataRowLength As Integer
        'The current row that the writer "cursor" is
        Protected m_currentRow As Integer
        'The current column that the writer "cursor" is
        Protected m_currentColumn As Integer
        Protected m_useFile As Boolean

        Protected m_pickleMatrixHeading As Integer()
        Protected m_pickleMatrixStub As Integer()



        Protected m_hasReadMeta As Boolean = False
        Protected m_hasReadData As Boolean = False


        Protected m_selectionHeading() As System.Collections.BitArray
        Protected m_selectionStub() As System.Collections.BitArray

        Protected m_eliminations As New List(Of Operations.EliminationDescription)


        Protected Overrides Sub BeginBuildForSelection()
            Me.m_builderState = ModelBuilderStateType.BuildingForSelection
            MyBase.BeginBuildForSelection()
            'Will only BuildMeta once data once
            If m_hasReadMeta Then
                Logger.Debug("Metadata already read")
                'TODO FIX Error code
                'Throw New PCAxis.Paxiom.PXModelParserException("Already read meta", "")
            End If
        End Sub

        Protected Overrides Sub EndBuildForSelection()
            m_hasReadMeta = True
            MyBase.EndBuildForSelection()
            Me.m_builderState = ModelBuilderStateType.BuildForSelection
        End Sub



        Public Overrides Function BuildForPresentation(ByVal selection() As Selection) As Boolean
            Me.m_builderState = ModelBuilderStateType.BuildingForPresentation
            'Can only be called once
            If m_hasReadData Then
                Logger.Warn("Data already read")
                'TODO FIX Error code
                Throw New PCAxis.Paxiom.PXModelParserException("Already read data", "")
            End If

            'Must read the meta data prior
            If Not m_hasReadMeta Then
                BuildForSelection()
            End If

            InitDataHandler(selection)

            'Set the size of the data matrix
            m_model.Data.SetMatrixSize(m_selectedRowLength, m_selectedColumnLength)
            Try
                Me.m_parser.ParseData(AddressOf Me.DataHandler, m_model.Meta.GetRowLength()) 'RowLength)
            Catch ex As Exception
                Me.Errors.Add(New BuilderMessage(ex.Message))
            End Try

            PruneMeta()

            m_model.Meta.CreateTitle()
            m_model.IsComplete = True
            m_hasReadData = True

            If Me.Errors.Count > 0 Then
                Logger.WarnFormat("Fatal error when building for presentation")
                Return False
            End If

            Me.m_builderState = ModelBuilderStateType.BuildForPresentation

            If Me.m_parser IsNot Nothing AndAlso GetType(IDisposable).IsAssignableFrom(m_parser.GetType()) Then
                CType(Me.m_parser, IDisposable).Dispose()
            End If

            Return True

        End Function


        ''' <summary>
        ''' Initialize the reading of the data
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub InitDataHandler(ByVal selection() As Selection)
            InitPickleMatrix()

            ReDim m_selectionHeading(m_model.Meta.Heading.Count - 1)
            For i As Integer = 0 To m_selectionHeading.Length - 1
                m_selectionHeading(i) = New BitArray(m_model.Meta.Heading(i).Values.Count, False)
            Next

            ReDim m_selectionStub(m_model.Meta.Stub.Count - 1)
            For i As Integer = 0 To m_selectionStub.Length - 1
                m_selectionStub(i) = New BitArray(m_model.Meta.Stub(i).Values.Count, False)
            Next

            m_selectedRowLength = 1
            m_selectedColumnLength = 1

            Dim v As Variable
            Dim s As Selection
            Dim sv As System.Collections.BitArray

            For i As Integer = 0 To selection.Length - 1
                s = selection(i)
                v = m_model.Meta.Variables.GetByCode(s.VariableCode)

                'Handles eliminations
                If s.ValueCodes.Count = 0 And v.Elimination Then
                    If v.EliminationValue Is Nothing Then
                        'TODO SUM ALL
                        For j As Integer = 0 To v.Values.Count - 1
                            s.ValueCodes.Add(v.Values(j).Code)
                        Next

                        m_eliminations.Add(New Operations.EliminationDescription(v.Code, False))
                    Else

                        Dim value As Value
                        value = FindValue(v, v.EliminationValue.Value)
                        s.ValueCodes.Add(value.Code)
                        m_eliminations.Add(New Operations.EliminationDescription(v.Code, True))


                    End If
                End If

                If v.Placement = PlacementType.Heading Then
                    m_selectedColumnLength *= s.ValueCodes.Count
                    sv = m_selectionHeading(m_model.Meta.Heading.IndexOf(v))
                Else
                    m_selectedRowLength *= s.ValueCodes.Count
                    sv = m_selectionStub(m_model.Meta.Stub.IndexOf(v))
                End If

                Dim cIndex As Integer
                For j As Integer = 0 To v.Values.Count - 1
                    cIndex = s.ValueCodes.IndexOf(v.Values(j).Code)
                    If cIndex > -1 Then
                        sv(j) = True
                        s.ValueCodes.RemoveAt(cIndex)
                    End If
                    If s.ValueCodes.Count = 0 Then
                        Exit For
                    End If
                Next
            Next

            m_currentColumn = 0
            m_currentRow = 0
            m_dataIndex = 0
            m_dataColumnLength = 1
            For Each var As Variable In m_model.Meta.Heading
                m_dataColumnLength *= var.Values.Count
            Next
            m_dataRowLength = 1
            For Each var As Variable In m_model.Meta.Stub
                m_dataRowLength *= var.Values.Count
            Next

        End Sub

        ''' <summary>
        ''' Removes unnecessary data that is no longer applicable on the cube.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub PruneMeta()
            Dim valRemoved As Boolean

            'Removes unselected values in the Stub
            For i As Integer = 0 To m_model.Meta.Stub.Count - 1
                valRemoved = False
                Dim v As Variable = m_model.Meta.Stub(i)
                Dim s As BitArray = m_selectionStub(i)
                For j As Integer = s.Count - 1 To 0 Step -1
                    If Not s(j) Then
                        v.Values.RemoveAt(j)
                        valRemoved = True
                    End If
                Next

                If valRemoved Then
                    UpdateElimination(v)
                    RemoveHierarchies(v)
                End If
            Next
            'Removes unselected values in the Heading
            For i As Integer = 0 To m_model.Meta.Heading.Count - 1
                valRemoved = False
                Dim v As Variable = m_model.Meta.Heading(i)
                Dim s As BitArray = m_selectionHeading(i)
                For j As Integer = s.Count - 1 To 0 Step -1
                    If Not s(j) Then
                        v.Values.RemoveAt(j)
                        valRemoved = True
                    End If
                Next

                If valRemoved Then
                    UpdateElimination(v)
                    RemoveHierarchies(v)
                End If
            Next

            'Timevalues
            For Each variable As Variable In m_model.Meta.Variables
                If variable.HasTimeValue Then
                    variable.BuildTimeValuesString()
                End If
            Next

            'Removes eliminated variables
            If m_eliminations.Count > 0 Then
                Dim func As New Operations.Elimination
                m_model = func.Execute(m_model, m_eliminations.ToArray)
            End If


            'TODO Petros Prio A prune Notes and other stuff.

            m_model.Meta.Prune()
            'This functionality was moved to PXMeta.PruneMeta
            ' Loop CellNotes and verify their existance
            'Dim checkVar As Variable
            'Dim doRemove As Boolean
            'For j As Integer = _model.Meta.CellNotes.Count - 1 To 0 Step -1
            '    doRemove = False
            '    For i As Integer = _model.Meta.CellNotes(j).Conditions.Count - 1 To 0 Step -1
            '        ' Check the variable for existance
            '        checkVar = _model.Meta.Variables.GetByCode(_model.Meta.CellNotes(j).Conditions(i).VariableCode)
            '        ' Check the variable
            '        If checkVar Is Nothing Then
            '            doRemove = True
            '            Exit For
            '        Else
            '            ' Check the value
            '            If checkVar.Values.GetByCode(_model.Meta.CellNotes(j).Conditions(i).ValueCode) Is Nothing Then
            '                doRemove = True
            '                Exit For
            '            End If
            '        End If
            '    Next
            '    If doRemove Then
            '        _model.Meta.CellNotes.RemoveAt(j)
            '    End If
            'Next
            '' Now do the same for datanotecells
            'For j As Integer = _model.Meta.DataNoteCells.Count - 1 To 0 Step -1
            '    doRemove = False
            '    For i As Integer = _model.Meta.DataNoteCells(j).Conditions.Count - 1 To 0 Step -1
            '        ' Check the variable for existance
            '        checkVar = _model.Meta.Variables.GetByCode(_model.Meta.DataNoteCells(j).Conditions(i).VariableCode)
            '        ' Check the variable
            '        If checkVar Is Nothing Then
            '            doRemove = True
            '            Exit For
            '        Else
            '            ' Check the value
            '            If checkVar.Values.GetByCode(_model.Meta.DataNoteCells(j).Conditions(i).ValueCode) Is Nothing Then
            '                doRemove = True
            '                Exit For
            '            End If
            '        End If
            '    Next

            '    If doRemove Then
            '        _model.Meta.DataNoteCells.RemoveAt(j)
            '    End If
            'Next

        End Sub

        ''' <summary>
        ''' Called AFTER one or more values are REMOVED from a variable to remove the hierarchies for that variable.
        ''' All values must be selected for a variable for hiearchies to exist.
        ''' </summary>
        ''' <param name="variable">The variable</param>
        ''' <remarks></remarks>
        Private Sub RemoveHierarchies(ByVal variable As Variable)
            If variable.Hierarchy.IsHierarchy Then
                variable.Hierarchy.Clear()
            End If
        End Sub

        ''' <summary>
        ''' Called AFTER one or more values are REMOVED from a variable to update the elimination of that variable
        ''' </summary>
        ''' <param name="variable">The variable to update</param>
        ''' <remarks></remarks>
        Private Sub UpdateElimination(ByVal variable As Variable)
            If variable.EliminationValue Is Nothing Then
                'If no EliminationValue exists all values must be selected
                variable.Elimination = False
            Else
                If variable.Values.GetByCode(variable.EliminationValue.Code) Is Nothing Then
                    'If EliminationValue exists that value must be selected
                    variable.Elimination = False
                    variable.EliminationValue = Nothing
                End If
            End If

        End Sub

        Private Sub DataHandler(ByVal data As Double(), ByVal startIndex As Integer, ByVal stopIndex As Integer, ByRef stopReading As Boolean)
            Dim d As Double
            Dim buffer As Double() 'Promote to member
            Dim writeIndex As Integer = 0

            ReDim buffer(stopIndex - startIndex)

            For i As Integer = startIndex To stopIndex
                d = data(i)
                If Not CancelData() Then
                    m_currentColumn += 1
                    buffer(writeIndex) = d
                    writeIndex += 1
                    If m_currentColumn = m_selectedColumnLength Then
                        m_currentColumn = 0
                        m_currentRow += 1
                    End If

                End If
                m_dataIndex += 1
            Next

            If writeIndex > 0 Then
                m_model.Data.Write(buffer, 0, writeIndex - 1)
            End If

            If m_model.Data.IsMatrixLoaded Then
                stopReading = True
            Else
                stopReading = False
            End If
        End Sub


#Region "Data reading"

        ''' <summary>
        ''' Calculates help matrixes for TODO
        ''' </summary>
        ''' <remarks>If there is no variable in the stub or heding there corsponding matrix will be set to Nothing/null</remarks>
        Private Sub InitPickleMatrix()
            Dim size As Integer = m_model.Meta.Heading.Count

            'Check that there is at least one variable in the Heading
            If size > 0 Then
                '_pickleMatrixHeading = New Integer(size - 1) {}
                m_pickleMatrixHeading = New Integer(size) {}

                For index As Integer = 0 To size - 1
                    m_pickleMatrixHeading(index) = 1
                    For j As Integer = index To size - 1
                        m_pickleMatrixHeading(index) *= m_model.Meta.Heading(j).Values.Count
                    Next
                    m_pickleMatrixHeading(size) = 1
                Next
            Else
                m_pickleMatrixHeading = Nothing
            End If

            size = m_model.Meta.Stub.Count
            'Check that there is at least one variable in the Stub
            If size > 0 Then
                '_pickleMatrixStub = New Integer(size - 1) {}
                m_pickleMatrixStub = New Integer(size) {}

                For index As Integer = 0 To size - 1
                    m_pickleMatrixStub(index) = 1
                    For j As Integer = index To size - 1
                        m_pickleMatrixStub(index) *= m_model.Meta.Stub(j).Values.Count
                    Next
                Next
                m_pickleMatrixStub(size) = 1
            Else
                m_pickleMatrixStub = Nothing
            End If

        End Sub

        ''' <summary>
        ''' Checks if all selected data has been read.
        ''' </summary>
        ''' <returns>Returns True if all selected data has been read otherwise False</returns>
        ''' <remarks></remarks>
        Private Function CancelData() As Boolean
            Dim size As Integer
            size = m_model.Meta.Heading.Count

            For index As Integer = 0 To size - 1
                If Not (index = size - 1) Then
                    If Not m_selectionHeading(index)(Convert.ToInt32(Math.Floor((m_dataIndex Mod m_pickleMatrixHeading(index)) / m_pickleMatrixHeading(index + 1)))) Then
                        Return True
                    End If
                Else
                    If Not m_selectionHeading(index)(Convert.ToInt32(Math.Floor((m_dataIndex Mod m_pickleMatrixHeading(index))))) Then
                        Return True
                    End If
                End If
            Next

            size = m_model.Meta.Stub.Count

            For index As Integer = 0 To size - 1
                If index = 0 Then
                    'If size > 1 Then
                    If Not m_selectionStub(index)(Convert.ToInt32(Math.Floor(m_dataIndex / (m_dataColumnLength * m_pickleMatrixStub(index + 1))))) Then
                        Return True
                    End If
                    'End If

                ElseIf index = (size - 1) Then
                    If Not m_selectionStub(index)(Convert.ToInt32(Math.Floor(m_dataIndex / m_dataColumnLength)) Mod m_pickleMatrixStub(index)) Then
                        Return True
                    End If
                Else
                    If Not m_selectionStub(index)(Convert.ToInt32(Math.Floor(m_dataIndex / (m_dataColumnLength * m_pickleMatrixStub(index + 1)))) Mod m_model.Meta.Stub(index).Values.Count) Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function


#End Region


        ''' <summary>
        ''' Finds variables by there names
        ''' </summary>
        ''' <param name="meta">the meta data where to find the variable</param>
        ''' <param name="findId">the search string</param>
        ''' <returns>The variable with findId as name</returns>
        ''' <remarks></remarks>
        Protected Overrides Function FindVariable(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal findId As String) As Variable
            Dim variable As Variable = Nothing

            If findId.Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                'Sortvariable - Get name from languagemanager
                findId = m_model.Meta.GetLocalizedSortVariableName(m_selectedLanguage)
            End If

            variable = meta.Variables.GetByName(findId, meta.CurrentLanguageIndex)

            If variable Is Nothing Then
                If Me.m_model.Meta.Variables.Count = 0 Then
                    'Throw New PXModelParserException(GetLocalizedString(ErrorCodes.STUB_AND_HEADING_MISSING))
                    Throw New PXModelParserException(ErrorCodes.STUB_AND_HEADING_MISSING)
                End If
            End If

            Return variable
        End Function

        ''' <summary>
        ''' Finds variables by there names and for a given language
        ''' </summary>
        ''' <param name="meta">the meta data where to find the variable</param>
        ''' <param name="findId">the search string</param>
        ''' <param name="lang">the language index to look for</param>
        ''' <returns>The variable with findId as name</returns>
        ''' <remarks></remarks>
        Protected Overrides Function FindVariable(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal findId As String, ByVal lang As Integer) As Variable
            Dim variable As Variable = Nothing

            If findId.Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                'Sortvariable - Get name from languagemanager
                findId = m_model.Meta.GetLocalizedSortVariableName(m_selectedLanguage)
            End If

            variable = meta.Variables.GetByName(findId, meta.CurrentLanguageIndex)

            If variable Is Nothing Then
                If Me.m_model.Meta.Variables.Count = 0 Then
                    Throw New PXModelParserException(GetLocalizedString(ErrorCodes.STUB_AND_HEADING_MISSING))
                End If
            End If

            Return variable
        End Function

        ''' <summary>
        ''' Finds a value in the variables value collection by matching the name
        ''' </summary>
        ''' <param name="variable">the variable containening the value</param>
        ''' <param name="findId">the name of the value</param>
        ''' <returns>The value having findId as it's Value property</returns>
        ''' <remarks></remarks>
        Protected Overrides Function FindValue(ByVal variable As PCAxis.Paxiom.Variable, ByVal findId As String) As Value
            Return variable.Values.GetByName(findId)
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                    If Me.m_parser IsNot Nothing AndAlso GetType(IDisposable).IsAssignableFrom(m_parser.GetType()) Then
                        CType(Me.m_parser, IDisposable).Dispose()
                    End If

                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
