'TODO Move to plugin.core
Imports PCAxis.Paxiom

Namespace PCAxis.Paxiom.Parsers

    Public Class PXFileParser
        Implements IDisposable, PCAxis.PlugIn.IPlugIn, PCAxis.Paxiom.IPXModelParser

        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PXFileParser))
        Private _keepRunning As Boolean

#Region "Members"

        Private theStream As System.IO.StreamReader
        Private mHasParsedMeta As Boolean = False
        Private mHasParsedData As Boolean = False
        Private InFnutt As Boolean
        'Internal state of the parser
        Private state As ParserState
        'For interal use only
        Protected disposed As Boolean = False
        Private DecimalSeparator As String = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
        Public Const DefaultDecimalSeparator As String = "."

        Private _inKeysFormat As Boolean = False
        Private _stubVariables As New Dictionary(Of String, ValueCodeContainer)
        Private _stubVariableList As New List(Of String)

        Private _encoding As System.Text.Encoding
        Private _path As String
        Private _position As Long

#End Region

#Region "Public properties"

        Public Sub SetPath(ByVal path As String)
            If theStream IsNot Nothing Then
                Try
                    theStream.Close()
                Catch ex As Exception
                    Logger.Error("", ex)
                End Try
            End If
            Try
                Me._encoding = GetEncoding(path)
            Catch ex As Exception
                Me._encoding = System.Text.Encoding.Default
            End Try

            _path = path
            'Me.theStream = New IO.StreamReader(New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 2048), _encoding)
            Me.state = ParserState.ReadKeyword
        End Sub

        Public Shared Function GetEncoding(ByVal path As String) As System.Text.Encoding

            If System.IO.Path.GetExtension(path).ToLower.Equals(".px") Then
                Using tr As System.IO.TextReader = New System.IO.StreamReader(path)
                    Dim lineCount = 1
                    Dim nextLine As String = tr.ReadLine()

                    While lineCount <= 100 And nextLine IsNot Nothing And Not nextLine.ToUpper().StartsWith("DATA=")
                        If nextLine.ToUpper().StartsWith("CODEPAGE=") Then
                            nextLine = nextLine.Substring(nextLine.IndexOf("""") + 1)
                            nextLine = nextLine.Substring(0, nextLine.LastIndexOf(""""))
                            Return System.Text.Encoding.GetEncoding(nextLine)
                        End If

                        lineCount += 1
                        nextLine = tr.ReadLine()
                    End While
                End Using
            End If

            Dim cs As String

            Dim BUFFER_SIZE As Integer = 1024
            Dim buffer(BUFFER_SIZE - 1) As Byte
            Dim size As Integer
            Using fs As System.IO.FileStream = System.IO.File.OpenRead(path)
                Dim det As Ude.ICharsetDetector
                det = New Ude.CharsetDetector
                Dim fi As New System.IO.FileInfo(path)
                size = Math.Min(BUFFER_SIZE, Convert.ToInt32(fi.Length))
                size = fs.Read(buffer, 0, size)
                det.Feed(buffer, 0, size)
                det.DataEnd()

                cs = det.Charset
            End Using

            If Logger.IsDebugEnabled Then
                Logger.Debug("Character set = " & cs)
            End If

            If cs Is Nothing Then
                Return System.Text.Encoding.Default
            End If

            'Fix fix it is ASCII it is probably the codepage of the machine
            If String.Compare(cs, "ASCII", True) = 0 Then
                Return System.Text.Encoding.Default
            End If

            Return System.Text.Encoding.GetEncoding(cs)
        End Function

#End Region

#Region "Constants"

		'The default value of the data buffer size
		Private Const DEFAULT_DATA_BUFFER_SIZE As Integer = 256
		Private Const START_SUBKEY As Char = "("c
		Private Const END_SUBKEY As Char = ")"c
		Private Const START_VALUES As Char = "="c
		Private Const END_VALUES As Char = ";"c
		Private Const FNUTT As Char = """"c
		Private Const COMMA As Char = ","c
		Private Const COLON As Char = ":"c
		Private Const START_LANGUAGE As Char = "["c
		Private Const END_LANGUAGE As Char = "]"c
		Private Const MINUS As Char = "-"c

		Public Const FILENAME As String = "Filename"

#End Region

#Region "Enum(s)"

		Private Enum ParserState
			ReadKeyword
			ReadLanguage
			ReadSubkeys
			ReadValues
		End Enum

		Private Enum KeysStates
			TrimWhiteSpaceBeforeKey
			ReadKey
			TrimWhiteSpaceAfterKey
			TrimWiteSpaceSurroundValue
			ReadValue
		End Enum

		Private Enum ValueType
			NotSet
			FnuttValue
			NoFnuttValue
			ErrorValue
		End Enum

#End Region

#Region "ParseMeta"


		Public Sub ParseMeta(ByVal handler As Paxiom.IPXModelParser.MetaHandler, ByVal preferredLanguage As String) Implements Paxiom.IPXModelParser.ParseMeta
			'Om man kontroll om man readan har lästin all metadata
			If mHasParsedMeta Then
				Exit Sub
			End If

			'Checks that the object is still alive
			If Me.disposed Then
				Throw New ObjectDisposedException(Me.GetType().ToString, _
				 "This object has been disposed.")
			End If

            Me.theStream = New IO.StreamReader(New System.IO.FileStream(_path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 2048), _encoding)

			Dim keyword As String = Nothing
			Dim language As String = Nothing
			Dim subkey As String = Nothing
			Dim values As System.Collections.Specialized.StringCollection = Nothing
			Dim hasLanguage As Boolean = False
			Dim hasSubkeys As Boolean = False
			'Dim keepRunning As Boolean = True
			Dim sb As System.Text.StringBuilder
			'Dim ws As Boolean
			Dim n As Integer
			Dim ch As Char
			Dim currentRow As Integer = 0
			InFnutt = False
            Dim valueType As ValueType
            Dim charCount = 0

			_keepRunning = True

			If _encoding IsNot Nothing Then
				values = New System.Collections.Specialized.StringCollection()
				values.Add(_encoding.WebName)
				handler(PXKeywords.CODEPAGE, Nothing, Nothing, values)
				values = Nothing
			End If

            n = theStream.Read()
            charCount += 1
			sb = New System.Text.StringBuilder
			While n > -1 And _keepRunning
                ch = Convert.ToChar(n)
				'Testar om man är inom dubellfnuttar 
				If ch = FNUTT Then
					InFnutt = Not InFnutt
				End If
				'Går igenom en sektion
				Select Case state
					Case ParserState.ReadKeyword
						Select Case ch
							Case FNUTT
								Exit Select
							Case START_LANGUAGE
								'Om vi redan har läst in subkeys så kastar vi ett fel
								If hasSubkeys Then
									Throw New PlugIn.PCAxisPlugInException("Invalid PX file")
								End If
								keyword = sb.ToString()
								sb = New System.Text.StringBuilder
								state = ParserState.ReadLanguage
								hasLanguage = True
							Case START_SUBKEY
								If Not hasLanguage Then
									keyword = sb.ToString()
								End If
								sb = New System.Text.StringBuilder
								state = ParserState.ReadSubkeys
								hasSubkeys = True
							Case START_VALUES
								If Not (hasSubkeys Or hasLanguage) Then
									keyword = sb.ToString()
								End If
								values = New System.Collections.Specialized.StringCollection
								sb = New System.Text.StringBuilder
								state = ParserState.ReadValues
								If keyword = "DATA" Then
                                    _keepRunning = False
                                    Exit While
								End If
								valueType = PXFileParser.ValueType.NotSet
							Case END_VALUES, END_LANGUAGE, END_SUBKEY
								Throw New PlugIn.PCAxisPlugInException("Invalid PX file")
							Case Else
								If InFnutt Then
									sb.Append(ch)
								Else
									If Not Char.IsWhiteSpace(ch) Then
										sb.Append(ch)
									End If
								End If
						End Select
					Case ParserState.ReadLanguage
						If Char.IsWhiteSpace(ch) Then
							Throw New PlugIn.PCAxisPlugInException("Invalid PX file")
						End If
						Select Case ch
							Case START_VALUES, START_SUBKEY, START_LANGUAGE, END_VALUES, END_SUBKEY, FNUTT
								Throw New PlugIn.PCAxisPlugInException("Invalid PX file")
							Case END_LANGUAGE
								language = sb.ToString()
								sb = New System.Text.StringBuilder
								state = ParserState.ReadKeyword
							Case Else
								sb.Append(ch)
						End Select
					Case ParserState.ReadSubkeys
						Select Case ch
							'Case START_VALUES, START_SUBKEY, START_LANGUAGE, END_VALUES, END_LANGUAGE
							'Throw New FileParserException("Invalid PX file")
							Case FNUTT
								' Include these characters to subkey stringbuilder
								' we will remove these before calling the handler further down
								sb.Append(ch)
								'Exit Select
							Case END_SUBKEY
								If InFnutt Then
									sb.Append(ch)
								Else
									subkey = sb.ToString()
									sb = New System.Text.StringBuilder
									state = ParserState.ReadKeyword
								End If
							Case Else
								If InFnutt Then
									sb.Append(ch)
								Else
									If Not Char.IsWhiteSpace(ch) Then
										sb.Append(ch)
									End If
								End If
						End Select
					Case ParserState.ReadValues
						If valueType = PXFileParser.ValueType.ErrorValue Then
							'There are errors in values -> Read to ; and send error
							If ch = END_VALUES Then
								values = New System.Collections.Specialized.StringCollection
								values.Add(keyword)
								values.Add(language)
								values.Add(subkey)

								_keepRunning = handler("KEYWORD_ERROR", Nothing, Nothing, values)

								keyword = Nothing
								language = Nothing
								subkey = Nothing
								values = Nothing
								hasLanguage = False
								hasSubkeys = False
								sb = New System.Text.StringBuilder
								state = ParserState.ReadKeyword
								InFnutt = False
							End If
						Else
							If InFnutt Then
								Select Case valueType
									Case PXFileParser.ValueType.NotSet
										valueType = PXFileParser.ValueType.FnuttValue
										If Not ch = FNUTT Then
											sb.Append(ch)
										End If
									Case PXFileParser.ValueType.FnuttValue
										If Not ch = FNUTT Then
											sb.Append(ch)
										End If
									Case PXFileParser.ValueType.NoFnuttValue
										'No fnutts allowed in NoFnuttValues!
										valueType = PXFileParser.ValueType.ErrorValue
								End Select
							Else
								If Char.IsWhiteSpace(ch) Or ch = FNUTT Then
									Exit Select
								End If
								Select Case ch
									Case START_LANGUAGE, START_VALUES, END_LANGUAGE
										Throw New Exception("Invalid PX file")
									Case COMMA
										values.Add(sb.ToString())
										sb = New System.Text.StringBuilder
										'Some keywords (e.g. TIMEVAL) may have values of both type 
										'FnuttValue and NoFnuttValue. Reset ValueType between values 
										'to handle this
										valueType = PXFileParser.ValueType.NotSet
									Case COLON
										'Hiearchyvalue - Add colon...
										sb.Append(ch)
									Case MINUS
										Select Case keyword
											Case "TIMEVAL"
												'Timeval in interval format
												sb.Append(ch)
												'Read past the next "-character
                                                n = theStream.Read()
                                                charCount += 1
												If n <> 34 Then	'34 = "
													valueType = PXFileParser.ValueType.ErrorValue
												End If
												InFnutt = True
											Case Else
												valueType = PXFileParser.ValueType.ErrorValue
										End Select
									Case END_SUBKEY
										Select Case keyword
											Case "TIMEVAL"
												'Timeval in interval format
												sb.Append(ch)
											Case Else
												valueType = PXFileParser.ValueType.ErrorValue
										End Select
									Case END_VALUES

										values.Add(sb.ToString())

										' Remove start/ending FNUTT
										If subkey IsNot Nothing AndAlso subkey.Length > 0 Then
											If subkey.Substring(0, 1) = FNUTT Then subkey = subkey.Substring(1)
											If subkey.Length > 0 AndAlso subkey.Substring(subkey.Length - 1, 1) = FNUTT Then subkey = subkey.Substring(0, subkey.Length - 1)
										End If

										'Storing information if the data is in KEYS format
										If String.IsNullOrEmpty(language) Then
											Select Case keyword
												Case PXKeywords.STUB 'Adding info about the value 
													For Each variable As String In values
														_stubVariables.Add(variable, New ValueCodeContainer())
														_stubVariableList.Add(variable)
													Next
												Case PXKeywords.VALUES 'Storing info about the values for a variable in the stub
													If _stubVariables.ContainsKey(subkey) Then
														_stubVariables(subkey).Values = values
													End If
												Case PXKeywords.CODES 'Storing info about the codes for a variable in the stub
													If _stubVariables.ContainsKey(subkey) Then
														_stubVariables(subkey).Codes = values
													End If
												Case PXKeywords.KEYS 'Checks if the data is in KEYS format
													_inKeysFormat = True
													If _stubVariables.ContainsKey(subkey) Then
														_stubVariables(subkey).Key = values(0)
													End If
											End Select
										End If

										_keepRunning = handler(keyword, language, subkey, values)
										keyword = Nothing
										language = Nothing
										subkey = Nothing
										values = Nothing
										hasLanguage = False
										hasSubkeys = False
										sb = New System.Text.StringBuilder
										state = ParserState.ReadKeyword
									Case Else 'TODO 20050810 För att få med sånt som står utan fnuttar i values
										''Enbart bett värde utan fnuttar är tillåtet annars är det ett fel i nyckelordet.
										'If values.Count = 0 Then
										'    sb.Append(ch)
										'Else
										'    'TODO FIXA bugg
										'    'Signalera att det är ett fel i keywordet
										'    'Läs till EOL eller semikolon
										'End If

										Select Case valueType
											Case PXFileParser.ValueType.NotSet
												valueType = PXFileParser.ValueType.NoFnuttValue
												If values.Count = 0 Then
													sb.Append(ch)
												End If
											Case PXFileParser.ValueType.NoFnuttValue
												If values.Count = 0 Then
													sb.Append(ch)
												End If
											Case PXFileParser.ValueType.FnuttValue
												'This is wrong...
												valueType = PXFileParser.ValueType.ErrorValue
										End Select
								End Select
							End If
						End If
				End Select
                n = theStream.Read()
                charCount += 1
			End While

			If n = -1 Then
                'Throw New PXModelParserException(Localization.PxResourceManager.GetResourceManager.GetString(ErrorCodes.DATA_MISSING))
                Throw New PXModelParserException(ErrorCodes.DATA_MISSING)
            Else
                mHasParsedMeta = True
			End If

            _position = charCount 'GetPosition(theStream)
            theStream.Close()
		End Sub

		''' <summary>
		''' Stop parsing
		''' </summary>
		''' <remarks></remarks>
		Public Sub StopParsing()
			_keepRunning = False
		End Sub
#End Region

#Region "Helper functions"

		Private Function CheckWhiteSpace(ByVal ch As Char) As Boolean
			If Char.IsWhiteSpace(ch) Then
				Return True
			End If
			Return False
		End Function

#End Region

        Private Sub FastForward(ByVal s As System.IO.StreamReader, ByVal charCount As Long)

            For index As Long = 1 To charCount
                s.Read()
            Next

        End Sub

#Region "ParseData"

		Public Sub ParseData(ByVal handler As PCAxis.Paxiom.IPXModelParser.DataHandler, ByVal preferredBufferSize As Integer) Implements Paxiom.IPXModelParser.ParseData

			'Checks that the object is still alive
			If Me.disposed Then
				Throw New ObjectDisposedException(Me.GetType().ToString, _
				 "This object has been disposed.")
			End If

			'Check that the metadata has been read
			If Not mHasParsedMeta Then
				Exit Sub
			End If
			If mHasParsedData Then
				Exit Sub 'Data has aready been read.
			End If

            Me.theStream = New IO.StreamReader(New System.IO.FileStream(_path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 2048), _encoding)
            'theStream.BaseStream.Seek(_position, IO.SeekOrigin.Begin)
            'theStream.DiscardBufferedData()
            FastForward(theStream, _position)


			If _inKeysFormat Then
				ParseKeysData(handler, preferredBufferSize)
			Else
				ParseMatrixData(handler, preferredBufferSize)
			End If

            mHasParsedData = True
            theStream.Close()
		End Sub

		Private Sub ParseKeysData(ByVal handler As PCAxis.Paxiom.IPXModelParser.DataHandler, ByVal preferredBufferSize As Integer)
			Dim DBG As Boolean = Logger.IsDebugEnabled

			'Contains a empty row
			Dim clearBuffer(preferredBufferSize - 1) As Double
			'The buffer that will be used for the parsed values
			Dim buffer(preferredBufferSize - 1) As Double

			'Number of variables in the stub
			Dim noOfVariables As Integer = _stubVariables.Keys.Count
			'The number of rows in the entire matrix
			Dim rowSize As Integer = 1
			'Matrix containing weights for variables in the stub
			Dim sMatrix(noOfVariables - 1) As Integer
			For i As Integer = 0 To noOfVariables - 1
				sMatrix(i) = 1
				rowSize *= _stubVariables(_stubVariableList(i)).Values.Count
				For j As Integer = i + 1 To noOfVariables - 1
					sMatrix(i) *= _stubVariables(_stubVariableList(j)).Values.Count
				Next
			Next
			'The current row index
			Dim currentRow As Integer = 0

			Dim keyCount As Integer = 0
			Dim valueCount As Integer = 0
			Dim columnSize As Integer = preferredBufferSize
			Dim columnCount As Integer = 0

			Dim keyRowIndex As Integer = 0

			For i As Integer = 0 To preferredBufferSize - 1
				clearBuffer(i) = 0.0
			Next

			Dim ch As Char
			Dim n As Integer
			Dim sb As New System.Text.StringBuilder()
			Dim data As String
			Dim done As Boolean = False
			Dim currentIndex As Integer = 0
			Dim stopReading As Boolean = False
			Dim state As KeysStates = KeysStates.TrimWhiteSpaceBeforeKey
			Dim reEval As Boolean = False
			n = theStream.Read()
			While n > -1 And Not done
				ch = Convert.ToChar(n)
				reEval = True

				While reEval
					reEval = False

					Select Case state
						Case KeysStates.TrimWhiteSpaceBeforeKey
							If keyCount = noOfVariables Then
								state = KeysStates.TrimWiteSpaceSurroundValue
								columnCount = 0
								reEval = True
							ElseIf ch = FNUTT Then
								state = KeysStates.ReadKey
								sb = New System.Text.StringBuilder
							ElseIf Char.IsWhiteSpace(ch) Then
								'Continue remove white space
							Else
								'TODO ERROR
							End If
						Case KeysStates.ReadKey
							If ch = FNUTT Then
								state = KeysStates.TrimWhiteSpaceAfterKey

								data = sb.ToString()

								'Calculates the index of the roow in the entire matrix
								Dim list As System.Collections.Specialized.StringCollection
								If _stubVariables.ContainsKey(_stubVariableList(keyCount)) Then
									Dim vcc As ValueCodeContainer = _stubVariables(_stubVariableList(keyCount))
									If String.Compare(vcc.Key, "CODES", True) = 0 Then
										list = vcc.Codes
									Else
										list = vcc.Values
									End If

									Dim vIndex As Integer
									vIndex = list.IndexOf(data)
									If vIndex > -1 Then
										keyRowIndex += sMatrix(keyCount) * vIndex
										keyCount += 1

										If DBG Then
											Logger.Debug(data)
										End If

									Else
										Throw New PXModelParserException(String.Format("Value {0} dose not exist for variable {1}", data, _stubVariableList(keyCount)))
									End If
								End If
							Else
								sb.Append(ch)
							End If
						Case KeysStates.TrimWhiteSpaceAfterKey
							If ch = COMMA Then
								state = KeysStates.TrimWhiteSpaceBeforeKey
								'---TODO reset
								'reEval = True
							ElseIf Char.IsWhiteSpace(ch) Then
								'Continue reading white spaces
							Else
								'TODO ERROR
							End If
						Case KeysStates.TrimWiteSpaceSurroundValue
							If Char.IsWhiteSpace(ch) Then
								'Keep reading white spaces
							Else
								sb = New System.Text.StringBuilder()
								reEval = True
								state = KeysStates.ReadValue
							End If
						Case KeysStates.ReadValue
							If Char.IsWhiteSpace(ch) Then
								data = sb.ToString()
								'Create a new empty buffer to store the next value
								sb = New System.Text.StringBuilder()
								buffer(columnCount) = StringToDouble(data)
								columnCount += 1
								If columnCount = columnSize Then
									'The hole line of data is read
									SignalNewData(handler, preferredBufferSize, clearBuffer, buffer, currentRow, keyCount, keyRowIndex, done, stopReading, state)
								End If

							ElseIf ch = END_VALUES Then
								data = sb.ToString()
								buffer(columnCount) = StringToDouble(data)
								columnCount += 1
								If columnCount = columnSize Then
									'Last line of data has been read
									SignalNewData(handler, preferredBufferSize, clearBuffer, buffer, currentRow, keyCount, keyRowIndex, done, stopReading, state)
								End If
								'writes the rest of the zeros
								If Not done Then
									Dim zeroCount As Integer
									zeroCount = rowSize - currentRow
									For i As Integer = 0 To zeroCount - 1
										handler(clearBuffer, 0, preferredBufferSize - 1, stopReading)
										If stopReading Then
											done = True
											Exit For
										End If
										currentRow += 1
									Next
								End If
								done = True
							Else
								sb.Append(ch)
							End If
					End Select
				End While

				n = theStream.Read()
			End While
		End Sub

		Private Shared Sub SignalNewData(ByVal handler As PCAxis.Paxiom.IPXModelParser.DataHandler, ByVal preferredBufferSize As Integer, ByVal clearBuffer As Double(), ByVal buffer As Double(), ByRef currentRow As Integer, ByRef keyCount As Integer, ByRef keyRowIndex As Integer, ByRef done As Boolean, ByVal stopReading As Boolean, ByRef state As KeysStates)
			'Fills up with zeros
			Dim zeroCount As Integer
			zeroCount = keyRowIndex - currentRow
			For i As Integer = 0 To zeroCount - 1
				handler(clearBuffer, 0, preferredBufferSize - 1, stopReading)
				If stopReading Then
					done = True
					Exit For
				End If
				currentRow += 1
			Next
			'writes the data row
			handler(buffer, 0, preferredBufferSize - 1, stopReading)
			If stopReading Then
				done = True
			End If
			currentRow += 1

			state = KeysStates.TrimWhiteSpaceBeforeKey
			keyCount = 0
			keyRowIndex = 0
		End Sub

		Private Sub ParseMatrixData(ByVal handler As PCAxis.Paxiom.IPXModelParser.DataHandler, ByVal preferredBufferSize As Integer)
			Dim DBG As Boolean = Logger.IsDebugEnabled
			Dim ch As Char
			Dim n As Integer
			Dim sb As System.Text.StringBuilder
			Dim data As String
			Dim buffer As Double()
			Dim done As Boolean = False
			Dim bufferSize As Integer
			Dim currentIndex As Integer = 0
			Dim stopReading As Boolean = False
			'Dim dc As String = PCAxis.AxiOM.PXModel.DecimalSeparator


			'Sets the databuffer size
			If preferredBufferSize > 0 Then
				bufferSize = preferredBufferSize
			Else
				bufferSize = PXFileParser.DEFAULT_DATA_BUFFER_SIZE
			End If
			'Creates the databuffer
			ReDim buffer(bufferSize - 1)

			sb = New System.Text.StringBuilder
			n = theStream.Read()
			While n > -1 And Not done
				ch = Convert.ToChar(n)

				'If we are at
				If ch = END_VALUES Then
					done = True
					data = sb.ToString()
					sb = New System.Text.StringBuilder
					If data Is Nothing Or data = String.Empty Then
						Throw New PXModelParserException("Data in KEYS format is missing or corrupt", ErrorCodes.DATA_MISSING.ToString())
					Else
						'handler(Double.Parse(data))
						buffer(currentIndex) = StringToDouble(data)
						currentIndex += 1
					End If
                ElseIf Char.IsWhiteSpace(ch) Then
                    data = sb.ToString()
                    sb = New System.Text.StringBuilder
                    If DBG Then
                        Logger.Debug(data)
                    End If

                    If data Is Nothing Or data = "" Then
                        'Throw New PXModelParserException("Data in matrix format is missing or corrupt", ErrorCodes.DATA_MISSING)
                    Else
                        buffer(currentIndex) = StringToDouble(data)
                        currentIndex += 1

                    End If
				Else
					sb.Append(ch)
				End If

				'Check if buffer is full
				If currentIndex >= bufferSize Or done Then
					handler(buffer, 0, currentIndex - 1, stopReading)
					If stopReading Then
						Exit While
					End If
					'IF we are done we do not reallocate any space
					If Not done Then
						ReDim buffer(bufferSize)
						currentIndex = 0
					End If
				End If

				n = theStream.Read()
			End While
		End Sub

#End Region

#Region "CovertFunctions"

		Private Shared _NumberFormat As System.Globalization.NumberFormatInfo

		Shared Sub New()
			Try
				_NumberFormat = CType(System.Globalization.CultureInfo.InstalledUICulture.NumberFormat.Clone(), System.Globalization.NumberFormatInfo)
				_NumberFormat.NumberDecimalSeparator = "."
			Catch ex As Exception
				Logger.Error("Could not create NumberFormat", ex)
			End Try

		End Sub


		Private Function StringToDouble(ByVal data As String) As Double
			Dim d As Double
			Dim dd As String '= data.Trim()
			If Not Double.TryParse(data, Globalization.NumberStyles.Number, _NumberFormat, d) Then
				dd = data.Trim()
				If dd.StartsWith("""") Then
					dd = dd.Substring(1)
				End If
				If dd.EndsWith("""") Then
					dd = dd.Substring(0, dd.Length - 1)
				End If
				Select Case dd
					Case PXConstant.DATASYMBOL_1_STRING
						d = PXConstant.DATASYMBOL_1
					Case PXConstant.DATASYMBOL_2_STRING
						d = PXConstant.DATASYMBOL_2
					Case PXConstant.DATASYMBOL_3_STRING
						d = PXConstant.DATASYMBOL_3
					Case PXConstant.DATASYMBOL_4_STRING
						d = PXConstant.DATASYMBOL_4
					Case PXConstant.DATASYMBOL_5_STRING
						d = PXConstant.DATASYMBOL_5
					Case PXConstant.DATASYMBOL_6_STRING
						d = PXConstant.DATASYMBOL_6
					Case PXConstant.DATASYMBOL_7_STRING
						d = PXConstant.DATASYMBOL_7
					Case PXConstant.DATASYMBOL_NIL_STRING
						d = PXConstant.DATASYMBOL_NIL
					Case Else
						'TODO change to TryParse
						'If DecimalSeparator <> DefaultDecimalSeparator Then
						'    d = Double.Parse(data.Replace(".", DecimalSeparator))
						'Else
						'    d = Double.Parse(data)
						'End If
						d = Double.Parse(data, _NumberFormat)

				End Select

			End If

			Return d
		End Function

#End Region

#Region "Constructor(s)"

		Public Sub New()

		End Sub

		Public Sub New(ByVal path As String)
			_encoding = GetEncoding(path)
			Me.theStream = New IO.StreamReader(New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 2048), _encoding)
			Me.state = ParserState.ReadKeyword
		End Sub

#End Region

#Region "IPlugIn implementation"

		Public ReadOnly Property Id() As System.Guid Implements PCAxis.PlugIn.IPlugIn.Id
			Get
				Return New Guid("c6fd3c04-c14d-42ce-8271-e151a0bfdc8d")
			End Get
		End Property

		Public ReadOnly Property Name() As String Implements PCAxis.PlugIn.IPlugIn.Name
			Get
				Return "PX-file parser"
			End Get
		End Property

		Public ReadOnly Property Description() As String Implements PCAxis.PlugIn.IPlugIn.Description
			Get
				Return "This is the default plugin which reads a classical PC-Axis file"
			End Get
		End Property

		Public Sub Initialize(ByVal host As PCAxis.PlugIn.IPlugInHost, ByVal configuration As Dictionary(Of String, String)) Implements PCAxis.PlugIn.IPlugIn.Initialize
			Dim filename As String
			'Checks that there is a filename 
			If Not configuration.ContainsKey(PXFileParser.FILENAME) Then
				Throw New PCAxis.Paxiom.PXModelParserException("No file was given")
			End If
			filename = configuration(PXFileParser.FILENAME)

			'Dispose the previous stream if there was one
			If Not theStream Is Nothing Then
				theStream.Dispose()
			End If

			'Opens a stream to the file
			_encoding = GetEncoding(filename)
			Me.theStream = New System.IO.StreamReader(New System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 2048), _encoding)
			Me.state = ParserState.ReadKeyword
		End Sub

		Public Sub Terminate() Implements PCAxis.PlugIn.IPlugIn.Terminate
			Me.Dispose()
		End Sub

#End Region

#Region "IDisposable implementation"

		Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposed Then
				If disposing Then
					If Not theStream Is Nothing Then
						theStream.Dispose()
					End If
				End If
				' Add code here to release the unmanaged resource.
				theStream = Nothing
				' Note that this is not thread safe.
			End If
			Me.disposed = True
		End Sub


		' Do not change or add Overridable to these methods.
		' Put cleanup code in Dispose(ByVal disposing As Boolean).
		Public Overloads Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Protected Overrides Sub Finalize()
			Dispose(False)
			MyBase.Finalize()
		End Sub
#End Region

#Region "Helper classes"

		Private Class ValueCodeContainer
			Public Values As System.Collections.Specialized.StringCollection
			Public Codes As System.Collections.Specialized.StringCollection
			Public Key As String
		End Class

#End Region

	End Class
End Namespace
