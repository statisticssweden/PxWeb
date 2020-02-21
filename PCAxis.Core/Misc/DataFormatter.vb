Namespace PCAxis.Paxiom

    Public Class DataFormatter

#Region "Private fields"

        Private _model As PXModel
        Private _roundingRule As MidpointRounding
        Private _dataNotePlacement As DataNotePlacementType
        Private _zeroOption As ZeroOptionType = ZeroOptionType.ShowAll
        Private _infoLevel As InformationLevelType = InformationLevelType.AllFootnotes
        Private _dataSymbols() As String = {Settings.DataSymbols.SymbolNIL,
                     Settings.DataSymbols.Symbol(1),
                     Settings.DataSymbols.Symbol(2),
                     Settings.DataSymbols.Symbol(3),
                     Settings.DataSymbols.Symbol(4),
                     Settings.DataSymbols.Symbol(5),
                     Settings.DataSymbols.Symbol(6),
                     Settings.DataSymbols.Symbol(7)}


        Private _haveSubPrecisions As Boolean
        Private _subPrecisions As New Dictionary(Of String, Dictionary(Of String, Integer))
        Private _indexer As DataIndexer
        Private _secrecy As SecrecyOptionType = SecrecyOptionType.None
        Private _showDataNotes As Boolean = True

#End Region

#Region "Public properties"

        ''' <summary>
        ''' The placemnt of the datanote
        ''' </summary>
        ''' <value>The placemnt of the datanote</value>
        ''' <returns>The placemnt of the datanote</returns>
        ''' <remarks></remarks>
        Public Property DataNotePlacment() As DataNotePlacementType
            Get
                Return _dataNotePlacement
            End Get
            Set(ByVal value As DataNotePlacementType)
                _dataNotePlacement = value
            End Set
        End Property

        ''' <summary>
        ''' how rows of zeros should be treated
        ''' </summary>
        ''' <value>how rows of zeros should be treated</value>
        ''' <returns>how rows of zeros should be treated</returns>
        ''' <remarks></remarks>
        Public Property ZeroOption() As ZeroOptionType
            Get
                Return _zeroOption
            End Get
            Set(ByVal value As ZeroOptionType)
                _zeroOption = value
            End Set
        End Property

        ''' <summary>
        ''' How much metadata information that should be included
        ''' </summary>
        ''' <value>How much metadata information that should be included</value>
        ''' <returns>How much metadata information that should be included</returns>
        ''' <remarks></remarks>
        Public Property InformationLevel() As InformationLevelType
            Get
                Return _infoLevel
            End Get
            Set(ByVal value As InformationLevelType)
                _infoLevel = value
            End Set
        End Property

        ''' <summary>
        ''' What type of data symbols that should be used when displaying the cube
        ''' </summary>
        ''' <param name="index">index of the datasymbol</param>
        ''' <value>the symbol to use</value>
        ''' <returns>the symbol to use</returns>
        ''' <remarks>Use index 1 for DataSymbol1</remarks>
        Public Property DataSymbols(ByVal index As Integer) As String
            Get
                If index > 0 AndAlso index < 8 Then
                    Return _dataSymbols(index)
                Else
                    'TODO grive proper error code
                    Throw New PCAxis.Paxiom.PXException()
                End If
            End Get
            Set(ByVal value As String)
                If index > 0 AndAlso index < 8 Then
                    _dataSymbols(index) = value
                Else
                    'TODO grive proper error code
                    Throw New PCAxis.Paxiom.PXException()
                End If
            End Set
        End Property


        ''' <summary>
        ''' Rounding rule to be used for calculations
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RoundingRule() As MidpointRounding
            Get
                Return _roundingRule
            End Get
        End Property

        ''' <summary>
        ''' Symbol for DataSymbolNil
        ''' </summary>
        ''' <value>Symbol for DataSymbolNil</value>
        ''' <returns>Symbol for DataSymbolNil</returns>
        ''' <remarks></remarks>
        Public Property DataSymbolNIL() As String
            Get
                Return _dataSymbols(0)
            End Get
            Set(ByVal value As String)
                _dataSymbols(0) = value
            End Set
        End Property

        Public Property Secrecy() As SecrecyOptionType
            Get
                Return _secrecy
            End Get
            Set(ByVal value As SecrecyOptionType)
                _secrecy = value
            End Set
        End Property

        Public Property ShowDataNotes() As Boolean
            Get
                Return _showDataNotes
            End Get
            Set(ByVal value As Boolean)
                _showDataNotes = value
            End Set
        End Property


#Region "NumberFormat properties"

        'Uses the default numberformat for the thread
        Private _defaultNumberFormat As System.Globalization.NumberFormatInfo = CType(System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.Clone(), System.Globalization.NumberFormatInfo)
        Private _otherNumberFormat As System.Globalization.NumberFormatInfo
        Private _dataFormat As String = "N"

        Public Property DecimalSeparator() As String
            Get
                Return _defaultNumberFormat.NumberDecimalSeparator
            End Get
            Set(ByVal value As String)
                _defaultNumberFormat.NumberDecimalSeparator = value
            End Set
        End Property


        Public Property ThousandSeparator() As String
            Get
                Return _defaultNumberFormat.NumberGroupSeparator
            End Get
            Set(ByVal value As String)
                _defaultNumberFormat.NumberGroupSeparator = value
            End Set
        End Property

        Public Property DecimalPrecision() As Integer
            Get
                Return _defaultNumberFormat.NumberDecimalDigits
            End Get
            Set(ByVal value As Integer)
                _defaultNumberFormat.NumberDecimalDigits = value
            End Set
        End Property

        Public Property DataFormat() As String
            Get
                Return _dataFormat
            End Get
            Set(ByVal value As String)
                _dataFormat = value
            End Set
        End Property
#End Region

#End Region

#Region "Constructor and initialization"


        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="model">the model containing the data</param>
        ''' <remarks></remarks>
        Sub New(ByVal model As PXModel)
            _secrecy = Settings.Numbers.SecrecyOption
            _model = model
            Init()
        End Sub

        Private Sub Init()
            ' Sets the default number separators
            _defaultNumberFormat.NumberGroupSeparator = Settings.GetLocale(_model.Meta.GetPreferredLanguage()).ThousandSeparator
            _defaultNumberFormat.NumberDecimalSeparator = Settings.GetLocale(_model.Meta.GetPreferredLanguage()).DecimalSeparator

            'Overrides the default values if datasymbols is specified in the model
            If Not String.IsNullOrEmpty(_model.Meta.DataSymbolNIL) Then
                _dataSymbols(0) = _model.Meta.DataSymbolNIL
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol1) Then
                _dataSymbols(1) = _model.Meta.DataSymbol1
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol2) Then
                _dataSymbols(2) = _model.Meta.DataSymbol2
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol3) Then
                _dataSymbols(3) = _model.Meta.DataSymbol3
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol4) Then
                _dataSymbols(4) = _model.Meta.DataSymbol4
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol5) Then
                _dataSymbols(5) = _model.Meta.DataSymbol5
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbol6) Then
                _dataSymbols(6) = _model.Meta.DataSymbol6
            End If

            If Not String.IsNullOrEmpty(_model.Meta.DataSymbolSum) Then
                _dataSymbols(7) = _model.Meta.DataSymbolSum
            End If

            ' Checks if there are any diffrent precision
            If _model.Meta.ContentVariable IsNot Nothing AndAlso _model.Meta.ContentVariable.Values(0).HasPrecision Then
                _haveSubPrecisions = True
            Else
                _haveSubPrecisions = False
            End If

            If _model.Meta.ShowDecimals >= 0 Then
                If _model.Meta.ShowDecimals < _model.Meta.Decimals Then
                    DecimalPrecision = _model.Meta.ShowDecimals
                Else
                    'TODO Check with LOUISE
                    DecimalPrecision = _model.Meta.Decimals
                End If
            Else
                DecimalPrecision = _model.Meta.Decimals
            End If

            Dim var As Variable

            For i As Integer = 0 To _model.Meta.Variables.Count - 1
                var = _model.Meta.Variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    'If value has a precision and it greater than ShowDecimal
                    If var.Values(j).Precision >= 0 And var.Values(j).Precision > DecimalPrecision Then
                        _haveSubPrecisions = True
                        If Not _subPrecisions.ContainsKey(var.Code) Then
                            _subPrecisions.Add(var.Code, New Dictionary(Of String, Integer))
                        End If
                        _subPrecisions(var.Code).Add(var.Values(j).Code, var.Values(j).Precision)
                    End If
                Next
            Next

            'Sets the rounding
            Select Case _model.Meta.Rounding
                Case RoundingType.BankersRounding
                    _roundingRule = MidpointRounding.ToEven
                Case RoundingType.RoundUp
                    _roundingRule = MidpointRounding.AwayFromZero
                Case Else
                    _roundingRule = Settings.Numbers.RoundingRule
            End Select

            'Sets default value for placment of the datanote
            _dataNotePlacement = Settings.DataNotes.Placment

            If _model.Meta.Confidential <> 0 Then
                _secrecy = SecrecyOptionType.Simple
            End If

            _indexer = New DataIndexer(_model.Meta)
        End Sub

#End Region

#Region "Notes functions"


        ''' <summary>
        ''' Gets all cellnotes for the current cell of the indexer as one string
        ''' according to the selected InformationLevel
        ''' </summary>
        ''' <returns>All cellnotes</returns>
        ''' <remarks></remarks>
        Private Function GetCellNote() As String
            If Not _infoLevel > InformationLevelType.None Then
                Return String.Empty
            End If
            If _model.Meta.CellNotes.Count = 0 Then
                Return String.Empty
            End If

            Dim note As CellNote

            Dim sb As System.Text.StringBuilder = Nothing
            Dim ok As Boolean
            For i As Integer = 0 To _model.Meta.CellNotes.Count - 1
                note = _model.Meta.CellNotes(i)
                If _infoLevel = InformationLevelType.MandantoryFootnotesOnly And (Not note.Mandatory) Then
                    Continue For
                End If

                ok = True
                Dim var As Variable
                Dim varIndex As Integer
                Dim valIndex As Integer
                For j As Integer = 0 To note.Conditions.Count - 1
                    var = _model.Meta.Variables.GetByCode(note.Conditions(j).VariableCode)

                    If var Is Nothing Then
                        ok = False
                        Exit For
                    End If

                    valIndex = var.Values.GetIndexByCode(note.Conditions(j).ValueCode)
                    If var.Placement = PlacementType.Heading Then
                        varIndex = _model.Meta.Heading.GetIndexByCode(note.Conditions(j).VariableCode)
                        If _indexer.HeadingIndecies(varIndex) <> valIndex Then
                            ok = False
                            Exit For
                        End If
                    Else
                        varIndex = _model.Meta.Stub.GetIndexByCode(note.Conditions(j).VariableCode)
                        If _indexer.StubIndecies(varIndex) <> valIndex Then
                            ok = False
                            Exit For
                        End If
                    End If
                Next
                If ok Then
                    If sb Is Nothing Then
                        sb = New System.Text.StringBuilder
                    Else
                        sb.Append(ControlChars.CrLf)
                    End If
                    sb.Append(note.Text)
                End If
            Next
            If sb Is Nothing Then
                Return String.Empty
            End If

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Gets all datacellnotes for the current cell of the indexer as one string
        ''' according to the selected InformationLevel
        ''' </summary>
        ''' <returns>All datacellnotes</returns>
        ''' <remarks></remarks>
        Private Function GetDataNote() As String
            If _model.Meta.DataNoteCells.Count = 0 Then
                Return String.Empty
            End If

            Dim note As DataNoteCell
            Dim sb As System.Text.StringBuilder = Nothing
            Dim ok As Boolean
            For i As Integer = 0 To _model.Meta.DataNoteCells.Count - 1
                note = _model.Meta.DataNoteCells(i)

                ok = True
                Dim var As Variable
                Dim varIndex As Integer
                Dim valIndex As Integer
                For j As Integer = 0 To note.Conditions.Count - 1
                    var = _model.Meta.Variables.GetByCode(note.Conditions(j).VariableCode)

                    If var Is Nothing Then
                        ok = False
                        Exit For
                    End If

                    valIndex = var.Values.GetIndexByCode(note.Conditions(j).ValueCode)
                    If var.Placement = PlacementType.Heading Then
                        varIndex = _model.Meta.Heading.GetIndexByCode(note.Conditions(j).VariableCode)
                        If _indexer.HeadingIndecies(varIndex) <> valIndex Then
                            ok = False
                            Exit For
                        End If
                    Else
                        varIndex = _model.Meta.Stub.GetIndexByCode(note.Conditions(j).VariableCode)
                        If _indexer.StubIndecies(varIndex) <> valIndex Then
                            ok = False
                            Exit For
                        End If
                    End If
                Next
                If ok Then
                    If sb Is Nothing Then
                        sb = New System.Text.StringBuilder
                    Else
                        sb.Append(ControlChars.CrLf)
                    End If
                    sb.Append(note.Text)
                End If
            Next
            If sb Is Nothing Then
                Return String.Empty
            End If

            Return sb.ToString()
        End Function

#End Region


        ''' <summary>
        ''' Gets the rigth numberformat for a specific measure in the cube
        ''' </summary>
        ''' <param name="rowIndex">the row index of the measure</param>
        ''' <param name="columnIndex">the column index of the measure</param>
        ''' <returns>the NumberFormat for the measure</returns>
        ''' <remarks></remarks>
        Private Function GetFormat(ByVal rowIndex As Integer, ByVal columnIndex As Integer) As System.Globalization.NumberFormatInfo
            If Not _haveSubPrecisions Then
                Return _defaultNumberFormat
            End If

            If _otherNumberFormat Is Nothing Then
                _otherNumberFormat = CType(_defaultNumberFormat.Clone(), System.Globalization.NumberFormatInfo)
            End If

            _indexer.SetContext(rowIndex, columnIndex)

            Dim prec As Integer
            Dim variableCode As String
			Dim valueCode As String
			prec = _defaultNumberFormat.NumberDecimalDigits
			If _indexer.HeadingIndecies IsNot Nothing Then
				For i As Integer = 0 To _indexer.HeadingIndecies.Length - 1
					variableCode = _model.Meta.Heading(i).Code
					If _subPrecisions.ContainsKey(variableCode) Then
						valueCode = _model.Meta.Heading(i).Values(_indexer.HeadingIndecies(i)).Code
						If _subPrecisions(variableCode).ContainsKey(valueCode) Then
							prec = Math.Max(prec, _subPrecisions(variableCode)(valueCode))
						End If
					End If
				Next
			End If

			If _indexer.StubIndecies IsNot Nothing Then
                For i As Integer = 0 To _indexer.StubIndecies.Length - 1
                    variableCode = _model.Meta.Stub(i).Code
                    If _subPrecisions.ContainsKey(variableCode) Then
                        valueCode = _model.Meta.Stub(i).Values(_indexer.StubIndecies(i)).Code
                        If _subPrecisions(variableCode).ContainsKey(valueCode) Then
                            prec = Math.Max(prec, _subPrecisions(variableCode)(valueCode))
                        End If
                    End If
                Next
            End If

            _otherNumberFormat.NumberDecimalDigits = prec

            Return _otherNumberFormat
        End Function

        ''' <summary>
        ''' Creates a String buffer to hold a row
        ''' </summary>
        ''' <returns>creates a string buffer that can hold a row of measures</returns>
        ''' <remarks></remarks>
        Public Function CreateRowBuffer() As String()
            Dim buffer(_model.Data.MatrixColumnCount - 1) As String
            Return buffer
        End Function

        ''' <summary>
        ''' Gets as measure from the data matrix with the rigth format
        ''' </summary>
        ''' <param name="row">row index of the measure</param>
        ''' <param name="column">column index for the measure</param>
        ''' <returns>the measure</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal row As Integer, ByVal column As Integer) As String
            Dim value As Double
            Dim data As String

            value = _model.Data.ReadElement(row, column)

            If _secrecy = SecrecyOptionType.Simple Then
                If value = 1.0 Then
                    value = 0.0
                ElseIf value = 2.0 Then
                    value = 3.0
                End If
            End If

            'Check if it is a reserved value
            Dim index As Integer
            index = Array.IndexOf(PXConstant.ProtectedValues, value)
            If index >= 0 Then
                data = _dataSymbols(index)
            Else
                'If not reserved value convert the float value to string
                Dim nfi As Globalization.NumberFormatInfo
                If _haveSubPrecisions Then
                    nfi = GetFormat(row, column)
                Else
                    nfi = _defaultNumberFormat
                End If

                data = Math.Round(value, nfi.NumberDecimalDigits, _roundingRule).ToString(_dataFormat, nfi)
            End If

            ' Only calc the indexes if there are datanotecells
            If _model.Meta.DataNoteCells.Count > 0 Then
                _indexer.SetContext(row, column)
            End If
            If ShowDataNotes Then
                Dim dataNote As String = ""
                If Not _dataNotePlacement = DataNotePlacementType.None Then
                        If _model.Data.UseDataCellMatrix AndAlso _model.Data.DataCellMatrixIsFilled Then
                            dataNote = _model.Data.ReadDataCellNoteElement(row, column)
                        Else
                            dataNote = GetDataNote()
                        End If
                    End If
                    'Adds the datanote
                    Select Case _dataNotePlacement
                    Case DataNotePlacementType.After
                        data = data & dataNote
                    Case DataNotePlacementType.Before
                        data = dataNote & data
                End Select
            End If
            Return data
        End Function

        ''' <summary>
        ''' Gets as measure from the data matrix with the rigth format
        ''' </summary>
        ''' <param name="row">row index of the measure</param>
        ''' <param name="column">column index of the measure</param>
        ''' <param name="note">notes that are associated to the measure</param>
        ''' <returns>the string display of the measure</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal row As Integer, ByVal column As Integer, ByRef note As String) As String
            Dim value As Double
            Dim data As String

            value = _model.Data.ReadElement(row, column)

            If _secrecy = SecrecyOptionType.Simple Then
                If value = 1.0 Then
                    value = 0.0
                ElseIf value = 2.0 Then
                    value = 3.0
                End If
            End If

            'check if NPM
            Dim index As Integer
            index = Array.IndexOf(PXConstant.ProtectedValues, value)
            If index >= 0 Then
                data = _dataSymbols(index)
            Else
                'Convert floeat value to string
                Dim nfi As Globalization.NumberFormatInfo
                If _haveSubPrecisions Then
                    nfi = GetFormat(row, column)
                Else
                    nfi = _defaultNumberFormat

                End If

                data = Math.Round(value, nfi.NumberDecimalDigits, _roundingRule).ToString(_dataFormat, nfi)

            End If

            ' Only calc index if there are cellnotes or datacellnotes
            If _model.Meta.DataNoteCells.Count > 0 OrElse _model.Meta.CellNotes.Count > 0 Then
                _indexer.SetContext(row, column)
            End If

            'Gets cellnotes
            If _infoLevel > InformationLevelType.None Then
                note = GetCellNote()
            Else
                note = String.Empty
            End If

            If ShowDataNotes Then

                Dim dataNote As String = ""

                    If Not _dataNotePlacement = DataNotePlacementType.None Then
                        If _model.Data.UseDataCellMatrix AndAlso _model.Data.DataCellMatrixIsFilled Then
                            dataNote = _model.Data.ReadDataCellNoteElement(row, column)
                        Else
                            dataNote = GetDataNote()
                        End If
                    End If

                'Adds the datanote
                Select Case _dataNotePlacement
                    Case DataNotePlacementType.After
                        data = data & dataNote
                    Case DataNotePlacementType.Before
                        data = dataNote & data
                End Select
            End If
            Return data
        End Function

        ''' <summary>
        ''' Returns element data only, note and dataNote reference variables are set
        ''' </summary>
        ''' <param name="row">row index of the measure</param>
        ''' <param name="column">column index of the measure</param>
        ''' <param name="note">notes associated to the measure</param>
        ''' <param name="dataNote">datanote for the measure</param>
        ''' <returns>the measure</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal row As Integer, ByVal column As Integer, ByRef note As String, ByRef dataNote As String) As String
            Dim value As Double
            Dim data As String

            value = _model.Data.ReadElement(row, column)

            If _secrecy = SecrecyOptionType.Simple Then
                If value = 1.0 Then
                    value = 0.0
                ElseIf value = 2.0 Then
                    value = 3.0
                End If
            End If

            'check if NPM
            Dim index As Integer
            index = Array.IndexOf(PXConstant.ProtectedValues, value)
            If index >= 0 Then
                data = _dataSymbols(index)
            Else
                'Convert float to string
                Dim nfi As Globalization.NumberFormatInfo
                If _haveSubPrecisions Then
                    nfi = GetFormat(row, column)
                Else
                    nfi = _defaultNumberFormat

                End If
                data = Math.Round(value, nfi.NumberDecimalDigits, _roundingRule).ToString(_dataFormat, nfi)
            End If

            ' Only calc index if there are cellnotes or datacellnotes
            If _model.Meta.DataNoteCells.Count > 0 OrElse _model.Meta.CellNotes.Count > 0 Then
                _indexer.SetContext(row, column)
            End If

            'Get notes
            If _infoLevel > InformationLevelType.None Then
                note = GetCellNote()
            Else
                note = String.Empty
            End If

            'Set the datanote depending on datanoteplacementtype

            dataNote = String.Empty
            Select Case _dataNotePlacement
                Case DataNotePlacementType.After, DataNotePlacementType.Before
                    If _dataNotePlacement <> DataNotePlacementType.None Then
                        If _model.Data.UseDataCellMatrix AndAlso _model.Data.DataCellMatrixIsFilled Then
                            dataNote = _model.Data.ReadDataCellNoteElement(row, column)
                        Else
                            dataNote = GetDataNote()
                        End If
                    End If
            End Select
            Return data
        End Function

        ''' <summary>
        ''' Returns element data only, note, dataNote and numberformatinfo reference variables are set
        ''' </summary>
        ''' <param name="row">row index of the measure</param>
        ''' <param name="column">column index of the measure</param>
        ''' <param name="note">notes associated to the measure</param>
        ''' <param name="dataNote">datanote for the measure</param>
        ''' <param name="numberFormatInfo">numberformatinfo for the measure</param>
        ''' <returns>the measure</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal row As Integer, ByVal column As Integer, ByRef note As String, ByRef dataNote As String, ByRef numberFormatInfo As Globalization.NumberFormatInfo) As String
            Dim value As Double
            Dim data As String

            value = _model.Data.ReadElement(row, column)

            If _secrecy = SecrecyOptionType.Simple Then
                If value = 1.0 Then
                    value = 0.0
                ElseIf value = 2.0 Then
                    value = 3.0
                End If
            End If

            'check if NPM
            Dim index As Integer
            index = Array.IndexOf(PXConstant.ProtectedValues, value)
            If index >= 0 Then
                data = _dataSymbols(index)
            Else
                'Convert float to string
                Dim nfi As Globalization.NumberFormatInfo
                If _haveSubPrecisions Then
                    nfi = GetFormat(row, column)
                Else
                    nfi = _defaultNumberFormat

                End If
                data = Math.Round(value, nfi.NumberDecimalDigits, _roundingRule).ToString(_dataFormat, nfi)
                numberFormatInfo = nfi
            End If

            ' Only calc index if there are cellnotes or datacellnotes
            If _model.Meta.DataNoteCells.Count > 0 OrElse _model.Meta.CellNotes.Count > 0 Then
                _indexer.SetContext(row, column)
            End If

            'Get notes
            If _infoLevel > InformationLevelType.None Then
                note = GetCellNote()
            Else
                note = String.Empty
            End If

            'Set the datanote depending on datanoteplacementtype
            dataNote = String.Empty
            Select Case _dataNotePlacement
                Case DataNotePlacementType.After, DataNotePlacementType.Before
                    If _dataNotePlacement <> DataNotePlacementType.None Then
                        If _model.Data.UseDataCellMatrix AndAlso _model.Data.DataCellMatrixIsFilled Then
                            dataNote = _model.Data.ReadDataCellNoteElement(row, column)
                        Else
                            dataNote = GetDataNote()
                        End If
                    End If
            End Select

            Return data
        End Function

        ''' <summary>
        ''' Check if cell has attributes or notes
        ''' </summary>
        ''' <param name="row">Cell row</param>
        ''' <param name="column">Cell column</param>
        ''' <returns>True if the cell has attributes or notes, else false</returns>
        ''' <remarks></remarks>
        Public Function HasCellAttributesOrNotes(ByVal row As Integer, ByVal column As Integer) As Boolean
            Dim indexCalculated As Boolean = False

            'Check if cell has attributes at cell level
            If _model.Meta.Attributes.CellAttributes.Count > 0 Then
                Dim key As VariableValuePairs = GetAttributesCellKey(row, column)

                'So we don´t calculate the index again when checking for cellnotes...
                indexCalculated = True

                If _model.Meta.Attributes.CellAttributes.ContainsKey(key) Then
                    Return True
                End If
            End If

            'Check if cell has notes
            If _model.Meta.CellNotes.Count > 0 Then
                If Not indexCalculated Then
                    _indexer.SetContext(row, column)
                End If

                If _infoLevel > InformationLevelType.None Then
                    If GetCellNote().Length > 0 Then
                        Return True
                    End If
                End If
            End If

            Return False
        End Function

        ''' <summary>
        ''' Get the attributes for the given cell
        ''' </summary>
        ''' <param name="row">Cell row</param>
        ''' <param name="column">Cell column</param>
        ''' <param name="attr">String array containing the attributes of the cell after method call has finished</param>
        ''' <returns>True if the cell has attributes, else false</returns>
        ''' <remarks></remarks>
        Public Function GetCellAttributes(ByVal row As Integer, ByVal column As Integer, ByRef attr As String()) As Boolean
            Dim key As VariableValuePairs = GetAttributesCellKey(row, column)

            If _model.Meta.Attributes.CellAttributes.ContainsKey(key) Then
                attr = _model.Meta.Attributes.CellAttributes(key)
                Return True
            End If

            Return False
        End Function

        Public Function GetCellNotes(ByVal row As Integer, ByVal column As Integer) As List(Of String)
            Dim separator As String() = {ControlChars.CrLf}

            _indexer.SetContext(row, column)

            Return GetCellNote().Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList
        End Function

        ''' <summary>
        ''' Get a list of variable name - value name pairs for the given cell
        ''' </summary>
        ''' <param name="row">Cell row index</param>
        ''' <param name="column">Cell column index</param>
        ''' <returns>List with variable name - value name pairs</returns>
        ''' <remarks></remarks>
        Public Function GetCellVariableValues(ByVal row As Integer, ByVal column As Integer) As List(Of KeyValuePair(Of String, String))
            Dim var As Variable
            Dim val As Value
            Dim lst As New List(Of KeyValuePair(Of String, String))
            Dim varval As KeyValuePair(Of String, String)

            _indexer.SetContext(row, column)

            'Run through all stub variables
            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                'Get the variable
                var = _model.Meta.Stub(i)
                'Get the value connected to the row/column from the indexer
                val = var.Values(_indexer.StubIndecies(i))
                varval = New KeyValuePair(Of String, String)(var.Name, val.Value)
                lst.Add(varval)
            Next

            'Run through all heading variables
            For i As Integer = 0 To _model.Meta.Heading.Count - 1
                'Get the variable
                var = _model.Meta.Heading(i)
                'Get the value connected to the row/column from the indexer
                val = var.Values(_indexer.HeadingIndecies(i))
                varval = New KeyValuePair(Of String, String)(var.Name, val.Value)
                lst.Add(varval)
            Next

            Return lst

        End Function

        ''' <summary>
        ''' Creates the key for identifying the attributes of the given cell
        ''' </summary>
        ''' <param name="row">Cell row</param>
        ''' <param name="column">Cell column</param>
        ''' <returns>Key identifying the attributes of the cell. The key is a VariableValuePairs object</returns>
        ''' <remarks></remarks>
        Private Function GetAttributesCellKey(ByVal row As Integer, ByVal column As Integer) As VariableValuePairs
            Dim var As Variable
            Dim val As Value
            Dim pair As VariableValuePair
            Dim pairs As New VariableValuePairs

            _indexer.SetContext(row, column)

            'Run through all stub variables
            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                'Get the variable
                var = _model.Meta.Stub(i)
                'Get the value connected to the row/column from the indexer
                val = var.Values(_indexer.StubIndecies(i))
                'Create VariableValuePair for this value
                pair = New VariableValuePair(var.Code, val.Code)
                pairs.Add(pair)
            Next

            'Run through all heading variables
            For i As Integer = 0 To _model.Meta.Heading.Count - 1
                'Get the variable
                var = _model.Meta.Heading(i)
                'Get the value connected to the row/column from the indexer
                val = var.Values(_indexer.HeadingIndecies(i))
                'Create VariableValuePair for this value
                pair = New VariableValuePair(var.Code, val.Code)
                pairs.Add(pair)
            Next

            Return pairs
        End Function

        ''' <summary>
        ''' Checks if a specific row only contains zeros
        ''' </summary>
        ''' <param name="rowNumber">the row number in the data matrix</param>
        ''' <returns>False if at least one of the data elements in the row is not zero otherwise True</returns>
        ''' <remarks>If the rowNumber is outside the bounds of the data matrix true is returned</remarks>
        Public Function IsZeroRow(ByVal rowNumber As Integer) As Boolean
            If rowNumber < 0 OrElse rowNumber > _model.Data.MatrixRowCount Then
                Return True
            End If

            Dim row As Double() = _model.Data.CreateRowBuffer()
            _model.Data.ReadLine(rowNumber, row)
            Dim isZero As Boolean = True

            Select Case _zeroOption
                Case ZeroOptionType.ShowAll
                    isZero = False
                Case ZeroOptionType.NoZero
                    isZero = CheckNoZero(row)
                Case ZeroOptionType.NoZeroAndNil
                    isZero = CheckNoZeroAndNil(row)
                Case ZeroOptionType.NoSymbols
                    isZero = CheckNoSymbols(row)
                Case ZeroOptionType.NoZeroNilAndSymbol
                    isZero = CheckNoZeroNilAndSymbols(row)
            End Select

            Return isZero

        End Function

        Private Function CheckNoZero(ByVal row As Double()) As Boolean
            Dim isZero As Boolean = True
            For i As Integer = 0 To row.Length - 1
                If row(i) <> 0.0 Then
                    If Not (_secrecy = SecrecyOptionType.Simple And row(i) = 1.0) Then
                        isZero = False
                        Exit For
                    End If
                End If
            Next
            Return isZero
        End Function

        Private Function CheckNoZeroAndNil(ByVal row As Double()) As Boolean
            Dim isZero As Boolean = True
            For i As Integer = 0 To row.Length - 1
                If row(i) <> 0.0 AndAlso row(i) <> PXConstant.DATASYMBOL_NIL Then
                    If Not (_secrecy = SecrecyOptionType.Simple And row(i) = 1.0) Then
                        isZero = False
                        Exit For
                    End If
                End If
            Next
            Return isZero
        End Function

        Private Function CheckNoSymbols(ByVal row As Double()) As Boolean
            Dim isZero As Boolean = True
            For i As Integer = 0 To row.Length - 1
                If row(i) <> PXConstant.DATASYMBOL_1 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_2 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_3 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_4 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_5 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_6 Then

                    If Not (_secrecy = SecrecyOptionType.Simple And row(i) = 1.0) Then

                        isZero = False
                        Exit For
                    End If

                End If
            Next
            Return isZero
        End Function

        Private Function CheckNoZeroNilAndSymbols(ByVal row As Double()) As Boolean
            Dim isZero As Boolean = True
            For i As Integer = 0 To row.Length - 1
                If row(i) <> PXConstant.DATASYMBOL_1 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_2 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_3 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_4 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_5 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_6 _
                    AndAlso row(i) <> PXConstant.DATASYMBOL_NIL _
                    AndAlso row(i) <> 0.0 Then

                    If Not (_secrecy = SecrecyOptionType.Simple And row(i) = 1.0) Then
                        isZero = False
                        Exit For
                    End If
                End If
            Next
            Return isZero
        End Function

        ''' <summary>
        ''' Utility function to get a numeric value formatted correctly (according to system settings) as a string.
        ''' The returned string has the correct thousand- and decimal-separator.
        ''' </summary>
        ''' <param name="value">The numeric value</param>
        ''' <param name="precision">Number of desired decimals</param>
        ''' <returns>The numeric value formatted as a string</returns>
        ''' <remarks></remarks>
        Public Shared Function NumericToString(ByVal value As Decimal, ByVal precision As Integer, ByVal language As String) As String
            Dim numberFormat As System.Globalization.NumberFormatInfo = CType(System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.Clone(), System.Globalization.NumberFormatInfo)
            Dim dataFormat As String = "N"
            Dim roundingRule As MidpointRounding = Settings.Numbers.RoundingRule

            numberFormat.NumberGroupSeparator = Settings.GetLocale(language).ThousandSeparator
            numberFormat.NumberDecimalSeparator = Settings.GetLocale(language).DecimalSeparator
            numberFormat.NumberDecimalDigits = precision

            Return Math.Round(value, numberFormat.NumberDecimalDigits, roundingRule).ToString(dataFormat, numberFormat)
        End Function
    End Class

    ''' <summary>
    ''' Specifies the type of rows to remove when getting data
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ZeroOptionType
        ''' <summary>
        ''' Don't remove any rows
        ''' </summary>
        ''' <remarks></remarks>
        ShowAll = 0
        ''' <summary>
        ''' Remove rows that contain only zeros
        ''' </summary>
        ''' <remarks></remarks>
        NoZero = 1
        ''' <summary>
        ''' Remove rows that contain only zeros or nils
        ''' </summary>
        ''' <remarks></remarks>
        NoZeroAndNil = 2
        ''' <summary>
        ''' remove rows that contain only symbols
        ''' </summary>
        ''' <remarks></remarks>
        NoSymbols = 3
        ''' <summary>
        ''' Remove rows that contain only zeros,nils or symbols
        ''' </summary>
        ''' <remarks></remarks>
        NoZeroNilAndSymbol = 4
    End Enum

End Namespace
