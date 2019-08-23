Imports PCAxis.Paxiom.Localization

Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Operation that splits the timevariable into two new variables: 
    ''' A year variable and a timescale variable. Timescale can be either month or quarter.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SplitTimevariable
        Implements PCAxis.Paxiom.IPXOperation

#Region "Private members"
        Private _pMatrixHelper As PMatrixHelper
        Private _oldModel As PXModel
        Private _timeVar As Variable
        Private _uniqueYears As System.Collections.Specialized.StringCollection
        Private _resMan As PxResourceManager
        Private _yearVar As Variable
        Private _TimescaleVar As Variable
        Private _newMeta As PXMeta
        Private _newData As PXData
        Private _timeVarIndex As Integer
#End Region

        Public Sub New()
            _uniqueYears = New System.Collections.Specialized.StringCollection
            _resMan = PxResourceManager.GetResourceManager
        End Sub

        ''' <summary>
        ''' Executes the "Split time variable" operation
        ''' </summary>
        ''' <param name="oldModel">The old model</param>
        ''' <param name="rhs">Not used</param>
        ''' <returns>The new model containg the year and timescale variables</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            Dim newModel As PXModel

            _oldModel = oldModel
            _newMeta = oldModel.Meta.CreateCopy()

            'Read old meta
            FindTimeVariable()
            FindUniqueYears()

            'Create new model
            CreateNewTimeVariables()
            ReplaceTimeVariable()
            CreateNewData()

            'InitializeHelpMatrixes()
            _pMatrixHelper = New PMatrixHelper(oldModel)
            _pMatrixHelper.NewMeta = _newMeta

            WriteData()

            newModel = New PXModel(_newMeta, _newData)
            newModel.IsComplete = True

            Return newModel
        End Function

#Region "Read old meta methods"
        ''' <summary>
        ''' Finds the time variable
        ''' </summary>
        ''' <remarks>
        ''' Throws PXOperationException if no time variable was found
        ''' </remarks>
        Private Sub FindTimeVariable()
            'Find the time variable
            'For Each variable As Variable In _oldMeta.Variables
            For Each variable As Variable In _newMeta.Variables
                If variable.HasTimeValue Then
                    _timeVar = variable
                    Exit For
                End If
            Next

            If _timeVar Is Nothing Then
                Throw New PXOperationException("Could not find time variable")
            End If

            If _timeVar.TimeScale <> TimeScaleType.Monthly And _timeVar.TimeScale <> TimeScaleType.Quartely Then
                Throw New PXOperationException("Can not split time when timescale is year, halfyear or week")
            End If

            'Find variable index of the time variable
            If _timeVar.Placement = PlacementType.Heading Then
                _timeVarIndex = _newMeta.Heading.GetIndexByCode(_timeVar.Code)
            Else
                _timeVarIndex = _newMeta.Stub.GetIndexByCode(_timeVar.Code)
            End If
        End Sub

        ''' <summary>
        ''' Finds the unique years. Adds them to the _uniqueYears StringCollection.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FindUniqueYears()
            _uniqueYears.Clear()

            For Each Value As Value In _timeVar.Values
                AddYears(Value.TimeValue)
            Next

            'Sort the years...
            ArrayList.Adapter(_uniqueYears).Sort()
        End Sub

        ''' <summary>
        ''' Adds one or more years to the _uniqueYears collection
        ''' </summary>
        ''' <param name="strVal">The string containing the year or the interval of years</param>
        ''' <remarks></remarks>
        Private Sub AddYears(ByVal strVal As String)
            If strVal.IndexOf("-") <> -1 Then
                'Interval
                Dim interval As String()
                Dim y1, y2 As Integer

                interval = strVal.Split("-"c)
                If interval.Length <> 2 Then
                    Exit Sub
                End If

                y1 = CInt(ReadYear(interval(0)))
                y2 = CInt(ReadYear(interval(1)))

                If y2 < y1 Then
                    Throw New PXOperationException("Illegal interval: " & strVal)
                End If

                For i As Integer = y1 To y2
                    AddYear(i.ToString)
                Next
            Else
                'Single year
                AddYear(ReadYear(strVal))
            End If
        End Sub

        ''' <summary>
        ''' Adds a single year to the _uniqueYears collection if it does not already exists in 
        ''' the collection
        ''' </summary>
        ''' <param name="year">The year to add</param>
        ''' <remarks></remarks>
        Private Sub AddYear(ByVal year As String)
            If year.Length = 4 Then
                If Not _uniqueYears.Contains(year) Then
                    _uniqueYears.Add(year)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Reads out the year part of a time variable value
        ''' </summary>
        ''' <param name="strVal">The time variable value</param>
        ''' <returns>The year string</returns>
        ''' <remarks>The year is supposed to be contained in the 4 first characters.</remarks>
        Private Function ReadYear(ByVal strVal As String) As String
            Dim year As String = ""

            If String.IsNullOrEmpty(strVal) Then
                Return year
            End If

            strVal = strVal.Trim(""""c)

            If strVal.Length < 4 Then
                'Must at least contain a year CCYY (C for Century, Y for Year)
                Return year
            End If

            year = strVal.Substring(0, 4)
            Return year
        End Function
#End Region

#Region "Create new model methods"
        ''' <summary>
        ''' Creates the new time variables that will replace the old time variable.
        ''' A year variable is allways created. The timescale variable that is created 
        ''' corresponds to the type of the time variable.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateNewTimeVariables()
            Dim langs As String() = GetLanguages()
            Dim langIndex As Integer = 0

            '----------
            ' Creation 
            '----------
            CreateYearVariable(langs.Length)
            CreateTimescaleVariable(langs.Length)

            '--------------------
            ' Set language texts
            '--------------------
            For Each lang As String In langs
                'Set language on Year variable
                PaxiomUtil.SetLanguage(_yearVar, langIndex)
                SetYearVariableLanguage(lang)

                'Set language on Timescale variable
                PaxiomUtil.SetLanguage(_TimescaleVar, langIndex)
                SetTimescaleVariableLanguage(lang)

                langIndex = langIndex + 1
            Next

            PaxiomUtil.SetLanguage(_yearVar, _newMeta.CurrentLanguageIndex)
            PaxiomUtil.SetLanguage(_TimescaleVar, _newMeta.CurrentLanguageIndex)
        End Sub

        ''' <summary>
        ''' Creates the year variable
        ''' </summary>
        ''' <param name="languages">Number of languages in the model</param>
        ''' <remarks></remarks>
        Private Sub CreateYearVariable(ByVal languages As Integer)
            _yearVar = New Variable
            For i As Integer = 0 To _uniqueYears.Count - 1
                _yearVar.Values.Add(New Value)
            Next
            _yearVar.Values.SetFictionalCodes()
            PaxiomUtil.ResizeLanguageVariables(_yearVar, languages)
            _yearVar.Placement = _timeVar.Placement
        End Sub

        ''' <summary>
        ''' Creates the timescale variable
        ''' </summary>
        ''' <param name="languages">Number of languages in the model</param>
        ''' <remarks>Throws PXOperationException if timescale is other than Monthly or Quarterly</remarks>
        Private Sub CreateTimescaleVariable(ByVal languages As Integer)
            _TimescaleVar = New Variable

            Select Case _timeVar.TimeScale
                Case TimeScaleType.Monthly
                    'Add value for each month
                    For i As Integer = 0 To 12 - 1
                        _TimescaleVar.Values.Add(New Value)
                    Next
                Case TimeScaleType.Quartely
                    'Add value for each quarter
                    For i As Integer = 0 To 4 - 1
                        _TimescaleVar.Values.Add(New Value)
                    Next
            End Select
            _TimescaleVar.Values.SetFictionalCodes()
            PaxiomUtil.ResizeLanguageVariables(_TimescaleVar, languages)
            _TimescaleVar.Placement = _timeVar.Placement
        End Sub


        ''' <summary>
        ''' Sets language texts for the year variable
        ''' </summary>
        ''' <param name="lang">The actual language</param>
        ''' <remarks></remarks>
        Private Sub SetYearVariableLanguage(ByVal lang As String)
            _yearVar.Name = _resMan.GetString("PxcOpSplitTimeVarYear", lang)

            For i As Integer = 0 To _yearVar.Values.Count - 1
                _yearVar.Values(i).Value = _uniqueYears(i)
            Next
        End Sub


        ''' <summary>
        ''' Sets language texts for the timescale variable.
        ''' </summary>
        ''' <param name="lang">The actual language</param>
        ''' <remarks></remarks>
        Private Sub SetTimescaleVariableLanguage(ByVal lang As String)
            Select Case _timeVar.TimeScale
                Case TimeScaleType.Monthly
                    _TimescaleVar.Name = _resMan.GetString("PxcOpSplitTimeVarMonth", lang)
                    _TimescaleVar.Values(0).Value = _resMan.GetString("PxcOpSplitTimeVarJanuary", lang)
                    _TimescaleVar.Values(1).Value = _resMan.GetString("PxcOpSplitTimeVarFebruary", lang)
                    _TimescaleVar.Values(2).Value = _resMan.GetString("PxcOpSplitTimeVarMarch", lang)
                    _TimescaleVar.Values(3).Value = _resMan.GetString("PxcOpSplitTimeVarApril", lang)
                    _TimescaleVar.Values(4).Value = _resMan.GetString("PxcOpSplitTimeVarMay", lang)
                    _TimescaleVar.Values(5).Value = _resMan.GetString("PxcOpSplitTimeVarJune", lang)
                    _TimescaleVar.Values(6).Value = _resMan.GetString("PxcOpSplitTimeVarJuly", lang)
                    _TimescaleVar.Values(7).Value = _resMan.GetString("PxcOpSplitTimeVarAugust", lang)
                    _TimescaleVar.Values(8).Value = _resMan.GetString("PxcOpSplitTimeVarSeptember", lang)
                    _TimescaleVar.Values(9).Value = _resMan.GetString("PxcOpSplitTimeVarOctober", lang)
                    _TimescaleVar.Values(10).Value = _resMan.GetString("PxcOpSplitTimeVarNovember", lang)
                    _TimescaleVar.Values(11).Value = _resMan.GetString("PxcOpSplitTimeVarDecember", lang)
                Case TimeScaleType.Quartely
                    _TimescaleVar.Name = _resMan.GetString("PxcOpSplitTimeVarQuarter", lang)
                    _TimescaleVar.Values(0).Value = _resMan.GetString("PxcOpSplitTimeVarQuarter1", lang)
                    _TimescaleVar.Values(1).Value = _resMan.GetString("PxcOpSplitTimeVarQuarter2", lang)
                    _TimescaleVar.Values(2).Value = _resMan.GetString("PxcOpSplitTimeVarQuarter3", lang)
                    _TimescaleVar.Values(3).Value = _resMan.GetString("PxcOpSplitTimeVarQuarter4", lang)
            End Select
        End Sub

        ''' <summary>
        ''' Get the languages in the model
        ''' </summary>
        ''' <returns>String array with all the language codes in the model</returns>
        ''' <remarks></remarks>
        Private Function GetLanguages() As String()
            Dim langs As String()

            langs = _newMeta.GetAllLanguages

            If langs Is Nothing Then
                'Only default language exists - Add it to langs
                ReDim langs(0)
                langs(0) = _newMeta.Language
            End If

            Return langs
        End Function

        ''' <summary>
        ''' Replace the timevariable with the new year- and timescale variables
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ReplaceTimeVariable()
            Dim index As Integer

            'Replace the old time variable with our new variables
            If _timeVar.Placement = PlacementType.Heading Then
                index = _newMeta.Heading.IndexOf(_timeVar)
            Else
                index = _newMeta.Stub.IndexOf(_timeVar)
            End If

            _newMeta.RemoveVariable(_timeVar)
            _newMeta.InsertVariable(index, _yearVar)
            _newMeta.InsertVariable(index + 1, _TimescaleVar)

            _newMeta.CreateTitle()
        End Sub

        ''' <summary>
        '''  Creates a new PXData object that corresponds to the new Meta containing the
        ''' Year- and timescale variables
        ''' </summary>
        ''' <remarks>All elements in the data is initilzed with "Data missing"</remarks>
        Private Sub CreateNewData()
            Dim rows As Integer = 1
            Dim columns As Integer = 1

            For i As Integer = 0 To _newMeta.Stub.Count - 1
                rows *= _newMeta.Stub(i).Values.Count
            Next

            For i As Integer = 0 To _newMeta.Heading.Count - 1
                columns *= _newMeta.Heading(i).Values.Count
            Next

            _newData = New PXData

            _newData.SetMatrixSize(rows, columns)

            For row As Integer = 0 To _newData.MatrixRowCount - 1
                For col As Integer = 0 To _newData.MatrixColumnCount - 1
                    _newData.WriteElement(row, col, PXConstant.DATASYMBOL_1)
                Next
            Next
        End Sub

        ''' <summary>
        ''' Reads from the old PXData and writes to the new PXData.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub WriteData()
            Dim theValue As Double
            Dim row As Integer
            Dim col As Integer

            For rowIndex As Integer = 0 To _oldModel.Data.MatrixRowCount - 1
                Dim valueIndex As Integer
                Dim value As String

                For colIndex As Integer = 0 To _oldModel.Data.MatrixColumnCount - 1
                    If _timeVar.Placement = PlacementType.Heading Then
                        row = rowIndex

                        '-----------------------------
                        ' Find the column to write to
                        '-----------------------------

                        'Identify heading values for this column
                        _pMatrixHelper.CalcHeadingWeights(colIndex)

                        'Identify valueindex for the time variable
                        valueIndex = _pMatrixHelper.HeadVariablesValueIndex(_timeVarIndex)

                        'Identify the value for the time variable
                        value = _timeVar.Values(valueIndex).TimeValue
                        'Find the column to write to
                        col = GetRowColumnIndex(value)
                    Else
                        col = colIndex

                        '--------------------------
                        ' Find the row to write to
                        '--------------------------

                        'Identify stub values for this row
                        _pMatrixHelper.CalcStubWeights(rowIndex)

                        'Identify valueindex for the time variable
                        valueIndex = _pMatrixHelper.StubVariablesValueIndex(_timeVarIndex)

                        'Identify the value for the time variable
                        value = _timeVar.Values(valueIndex).TimeValue
                        'Find the row to write to
                        row = GetRowColumnIndex(value)
                    End If

                    'Read from old data
                    theValue = _oldModel.Data.ReadElement(rowIndex, colIndex)

                    'Write to new data
                    _newData.WriteElement(row, col, theValue)
                Next
            Next
        End Sub



        ''' <summary>
        ''' Calculates which row or column to write to.
        ''' </summary>
        ''' <param name="value">The actual TimeValue. Used to identify valueindex for the
        ''' year- and timescale variables</param>
        ''' <returns>Row- or columnindex</returns>
        ''' <remarks></remarks>
        Private Function GetRowColumnIndex(ByVal value As String) As Integer
            Dim year As String
            Dim yearIndex As Integer
            Dim periodIndex As Integer
            Dim index As Integer = 0
            Dim j As Integer = 0

            year = ReadYear(value)
            yearIndex = _uniqueYears.IndexOf(year)

            Select Case _timeVar.TimeScale
                Case TimeScaleType.Monthly
                    periodIndex = ReadMonth(value)
                Case TimeScaleType.Quartely
                    periodIndex = ReadQuarter(value)
            End Select

            If yearIndex = -1 Or periodIndex = -1 Then
                Throw New PXOperationException("Calculation error")
            End If

            If _timeVar.Placement = PlacementType.Heading Then
                For i As Integer = 0 To _pMatrixHelper.HeadVariablesValueIndex.Length - 1
                    If i <> _timeVarIndex Then
                        index = index + _pMatrixHelper.NewPMHeading(j + 1) * _pMatrixHelper.HeadVariablesValueIndex(i)
                    Else
                        index = index + _pMatrixHelper.NewPMHeading(j + 1) * yearIndex
                        j = j + 1
                        index = index + _pMatrixHelper.NewPMHeading(j + 1) * periodIndex
                    End If
                    j = j + 1
                Next
            Else
                For i As Integer = 0 To _pMatrixHelper.StubVariablesValueIndex.Length - 1
                    If i <> _timeVarIndex Then
                        index = index + _pMatrixHelper.NewPMStub(j + 1) * _pMatrixHelper.StubVariablesValueIndex(i)
                    Else
                        index = index + _pMatrixHelper.NewPMStub(j + 1) * yearIndex
                        j = j + 1
                        index = index + _pMatrixHelper.NewPMStub(j + 1) * periodIndex
                    End If
                    j = j + 1
                Next
            End If

            Return index
        End Function

        ''' <summary>
        ''' Reads the month-part of the given value
        ''' </summary>
        ''' <param name="value">The value to read from</param>
        ''' <returns>The month index</returns>
        ''' <remarks></remarks>
        Private Function ReadMonth(ByVal value As String) As Integer
            Dim month As String = ""

            If String.IsNullOrEmpty(value) Then
                Return -1
            End If

            value = value.Trim(""""c)

            If value.Length < 6 Then
                'Must at least contain a year CCYYMM (C for Century, Y for Year, M for Month) 
                Return -1
            End If

            month = value.Substring(4, 2)
            Return CInt(month) - 1
        End Function

        ''' <summary>
        ''' Reads the quarter-part of the given value
        ''' </summary>
        ''' <param name="value">The value to read from</param>
        ''' <returns>The quarter index</returns>
        ''' <remarks></remarks>
        Private Function ReadQuarter(ByVal value As String) As Integer
            Dim quarter As String = ""

            If String.IsNullOrEmpty(value) Then
                Return -1
            End If

            value = value.Trim(""""c)

            If value.Length < 5 Then
                'Must at least contain a year CCYYQ (C for Century, Y for Year, Q for Quarter) 
                Return -1
            End If

            quarter = value.Substring(4, 1)
            Return CInt(quarter) - 1
        End Function
#End Region

    End Class

End Namespace
