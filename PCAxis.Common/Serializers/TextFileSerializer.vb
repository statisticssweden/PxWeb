Imports System.Text
Imports System.Collections.Generic

Namespace PCAxis.Paxiom

    'SPECIFICATION
    'The Textfile format has the following requirements:
    '
    '1. The stubtext (rowheader) cannot be wider than 40 characters. Longer texts must be split into 2 or more rows.
    '2. If a stub variable has DOUBLECOLUMN=Yes in the px-file both the code and value of the variable must be 
    '   shown in the stubtext.
    '3. The columnwidth equals the number of characters in the value width the most digits (not necessary the 
    '   biggest value), or the number of characters in the longest heading-text. 
    '4. A pagebreak can only occur on a row that has values.
    '5. After a pagebreak all preceeding stubtexts must be written on the new page before the first value of the 
    '   new page is written (A value cannot be written without the stubtexts accociated with it).

    Public Class TextFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

#Region "Constants"
        Private SPACE As Char = " "c
        Private STUB_INDENT As Integer = 2  'Number of spaces for indentation of nested stubs
        Private TABLE_INDENT As Integer = 2 'Number of spaces between the longest stubvalue-text and the first heading
        Private MAX_STUBLENGTH As Integer = 40 'The maximum number of characters for stubs
        Private DOUBLECOLUMN_SPACING As Integer = 3 'Number of spaces between stub-code and stub-value if doublecolumn
#End Region

#Region "Private fields"
        Private _model As PCAxis.Paxiom.PXModel
        Private _wr As System.IO.StreamWriter
        Private _rowLength As Integer
        Private _pageLength As Integer
        Private _margin As Integer
        Private _completeInfo As PCAxis.Paxiom.CompleteInfoType
        Private _pageNumber As Integer
        Private _pageSubNumber As Integer
        Private _stubWidth As Integer 'The length of the longest stubvalue-text
        Private _row As Integer 'Row within data
        Private _pageRow As Integer 'Row whitin the current page
        Private _columnWidth As Integer 'The columnwidth
        'Private _stubLevels As Integer
        'Private _stubTexts As New List(Of String)
        'Private _lastStubLevel As Integer
        Private _stubLevels As New List(Of StubLevel)
#End Region

        Private Class StubLevel
            Public Text As String
            Public Items As Integer
            Public CompletedItems As Integer
        End Class

#Region "Properties"
        Public Property RowLength() As Integer
            Get
                Return _rowLength
            End Get
            Set(ByVal value As Integer)
                _rowLength = value
            End Set
        End Property

        Public Property PageLength() As Integer
            Get
                Return _pageLength
            End Get
            Set(ByVal value As Integer)
                _pageLength = value
            End Set
        End Property

        Public Property Margin() As Integer
            Get
                Return _margin
            End Get
            Set(ByVal value As Integer)
                _margin = value
            End Set
        End Property

        Public Property CompleteInfo() As PCAxis.Paxiom.CompleteInfoType
            Get
                Return _completeInfo
            End Get
            Set(ByVal value As PCAxis.Paxiom.CompleteInfoType)
                _completeInfo = value
            End Set
        End Property
#End Region

#Region "Interface members"
        ''' <summary>
        ''' Write a PXModel to a file.
        ''' </summary>
        ''' <param name="model">The PXModel to write.</param>
        ''' <param name="path">The complete file path to write to. <I>path</I> can be a file name.</param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal path As String) Implements PCAxis.Paxiom.IPXModelStreamSerializer.Serialize
            If model Is Nothing Then Throw New ArgumentNullException("model")

            ' Let the StreamWriter verify the path argument
            Dim encoding As System.Text.Encoding
            encoding = EncodingUtil.GetEncoding(model.Meta.CodePage)
            Using writer As System.IO.StreamWriter = New System.IO.StreamWriter(path, False, encoding)
                DoSerialize(model, writer)
            End Using
        End Sub

        ''' <summary>
        ''' Write a PXModel to a stream.
        ''' </summary>
        ''' <param name="model">The PXModel to write.</param>
        ''' <param name="stream">The stream to write to.</param>
        ''' <remarks>The caller is responsible of disposing the stream.</remarks>
        Public Sub Serialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal stream As System.IO.Stream) Implements PCAxis.Paxiom.IPXModelStreamSerializer.Serialize
            If model Is Nothing Then Throw New ArgumentNullException("model")
            If stream Is Nothing Then Throw New ArgumentNullException("stream")

            If Not stream.CanWrite Then Throw New ArgumentException("The stream does not support writing")

            Dim encoding As System.Text.Encoding
            encoding = EncodingUtil.GetEncoding(model.Meta.CodePage)
            Dim writer As System.IO.StreamWriter = New System.IO.StreamWriter(stream, encoding)
            DoSerialize(model, writer)
        End Sub

#End Region

        Private Sub DoSerialize(ByVal model As PXModel, ByVal wr As System.IO.StreamWriter)
            Initialize(model, wr)
            _wr.WriteLine()
            WritePageHead()
            WriteTable(0)
        End Sub

        Private Sub Initialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal wr As System.IO.StreamWriter)
            _model = model
            _wr = wr
            _row = 0
            _pageRow = 1
            _pageNumber = 1
            _pageSubNumber = 1
            _stubWidth = CalculateStubWidth()
            _columnWidth = CalculateColumnWidth()
            '_stubLevels = model.Meta.Stub.Count
        End Sub

        Private Sub WritePageHead()
            _wr.WriteLine(PageNumberIndent() & _pageNumber.ToString & "." & _pageSubNumber.ToString)
            _wr.WriteLine()
            _wr.Write(New String(SPACE, Me.Margin))
            _wr.Write(_model.Meta.Title)
            _wr.WriteLine()
            _wr.WriteLine()
            _pageRow = 5
            WriteHeadings()
        End Sub

        Private Function PageNumberIndent() As String
            Dim indent As Integer

            indent = _rowLength - 14

            If indent < 31 Then
                indent = 31
            End If

            Return New String(SPACE, indent)
        End Function

        Private Function CalculateStubWidth() As Integer
            Dim stub As Variables = _model.Meta.Stub
            Dim length As Integer
            Dim stubvalue As String = ""
            Dim longest As Integer = 0

            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                Dim stubValues As Paxiom.Values = _model.Meta.Stub(i).Values

                For j As Integer = 0 To stubValues.Count - 1

                    If _model.Meta.Stub(i).DoubleColumn Then
                        '*****************
                        '* Requirement 2 *
                        '*****************
                        stubvalue = stubValues(j).Code & _
                        New String(SPACE, DOUBLECOLUMN_SPACING) & _
                        stubValues(j).Value
                    Else
                        stubvalue = stubValues(j).Value
                    End If

                    '*****************
                    '* Requirement 1 *
                    '*****************
                    If stubvalue.Length > MAX_STUBLENGTH Then
                        Return MAX_STUBLENGTH
                    End If

                    length = stubvalue.Length + (i * STUB_INDENT)

                    If length > longest Then
                        longest = length
                    End If
                Next
            Next

            longest = longest + TABLE_INDENT

            Return longest
        End Function

        Private Function CalculateColumnWidth() As Integer
            Dim buffer(_model.Data.MatrixColumnCount - 1) As Double
            Dim value As String
            Dim longestValue As Integer

            longestValue = 0

            '*****************
            '* Requirement 3 *
            '*****************

            'Loop through all the values and find the one with most digits...
            For i As Integer = 0 To _model.Data.MatrixRowCount - 1
                _model.Data.ReadLine(i, buffer)

                For j As Integer = 0 To buffer.Length - 1
                    'TODO: Handle number of decimals, thousands, symbols...
                    value = buffer(j).ToString

                    If value.Length > longestValue Then
                        longestValue = value.Length
                    End If
                Next
            Next

            'Loop through all headings and find if heading name is longer...
            For i As Integer = 0 To _model.Meta.Heading.Count - 1
                Dim headingValues As Paxiom.Values = _model.Meta.Heading(i).Values
                For j As Integer = 0 To headingValues.Count - 1
                    If headingValues(j).Value.Length > longestValue Then
                        longestValue = headingValues(j).Value.Length
                    End If
                Next
            Next

            longestValue = longestValue + 1

            Return longestValue
        End Function

        Private Function CalculateSubValues(ByVal vars As Variables, ByVal level As Integer, ByRef subValues As Integer()) As Integer

            If vars.Count = 0 Then
                subValues(level) = 1
                Return 0
            ElseIf vars.Count - 1 = level Then
                subValues(level) = 1
                Return vars(level).Values.Count
            Else
                Dim nextLevel As Integer = level + 1
                Dim ret As Integer = CalculateSubValues(vars, nextLevel, subValues)
                subValues(level) = ret
                Return ret * vars(level).Values.Count
            End If

        End Function

        Private Sub WriteHeadings()
            Dim heading As Variables = _model.Meta.Heading

            If Not heading Is Nothing Then

                Dim subHeadings(1) As Integer

                If heading.Count > 0 Then
                    ReDim subHeadings(heading.Count - 1)
                End If

                CalculateSubValues(heading, 0, subHeadings)

                Dim timesToWrite As Integer = 1 ' This keep track of the number of times the current heading shall be written
                Dim timesWritten As Integer = 0 ' This keep track of the number of times the current heading has been written
                For index As Integer = 0 To heading.Count - 1

                    'Write margin
                    _wr.Write(New String(SPACE, Me.Margin))

                    'Write spaces for the stubs
                    _wr.Write(New String(SPACE, _stubWidth))

                    ' Write the heading
                    Dim valuesCount As Integer = heading(index).Values.Count
                    For j As Integer = 0 To timesToWrite - 1

                        Dim headingValues As Paxiom.Values = heading(index).Values
                        For ix As Integer = 0 To headingValues.Count - 1
                            'Write heading text
                            _wr.Write(headingValues(ix).Value)
                            'Fill the rest of the column with spaces
                            _wr.Write(New String(SPACE, _columnWidth - headingValues(ix).Value.Length))

                            'Fill subheadings with spaces
                            For e As Integer = 1 To subHeadings(index) - 1
                                _wr.Write(New String(SPACE, _columnWidth))
                            Next

                            timesWritten = timesWritten + 1
                        Next

                    Next

                    timesToWrite = timesWritten
                    timesWritten = 0

                    _wr.WriteLine()
                Next
                _wr.WriteLine()
                _pageRow = _pageRow + heading.Count + 1
            End If

        End Sub

        Private Sub WriteTable(ByVal level As Integer)
            Dim stubValues As Paxiom.Values = _model.Meta.Stub(level).Values
            Dim stubText As String = ""
            Dim stubLevel As New StubLevel

            'stubLevel.Items = stubValues.Count
            'stubLevel.CompletedItems = 0

            For j As Integer = 0 To stubValues.Count - 1
                'Write margin
                _wr.Write(New String(SPACE, Me.Margin))
                stubText = CreateStubText(level, stubValues(j))

                If Integer.Equals(level, _model.Meta.Stub.Count - 1) Then
                    _wr.Write(stubText)
                    'Fill rest of stub with spaces
                    _wr.Write(New String(SPACE, _stubWidth - stubText.ToString.Length))
                    'Write data
                    WriteDataLine()

                    _wr.WriteLine()
                    _pageRow = _pageRow + 1

                    If _pageRow >= PageLength Then
                        _pageNumber = _pageNumber + 1
                        WritePageHead()

                        If j < stubValues.Count - 1 Then
                            For i As Integer = 0 To _stubLevels.Count - 1
                                _wr.WriteLine(_stubLevels(i).text)
                            Next
                            _pageRow = _pageRow + _stubLevels.Count
                        End If
                    End If

                Else
                    _wr.Write(stubText)
                    _wr.WriteLine()
                    _pageRow = _pageRow + 1
                End If

                If Not Integer.Equals(level, _model.Meta.Stub.Count - 1) Then
                    stubLevel = New StubLevel
                    stubLevel.text = stubText
                    'stubLevel.Completed = False

                    If _stubLevels.Count = level + 1 Then
                        _stubLevels(level) = stubLevel
                    Else
                        _stubLevels.Add(stubLevel)
                    End If
                End If

                'Write stubtext recursivly
                If level < _model.Meta.Stub.Count - 1 Then
                    WriteTable(level + 1)
                End If
            Next

            If Not Integer.Equals(level, _model.Meta.Stub.Count - 1) Then
                _stubLevels.RemoveAt(level)
            End If
        End Sub


        Private Function CreateStubText(ByVal level As Integer, ByVal value As Paxiom.Value) As String
            Dim stubText As New StringBuilder

            stubText.Append(New String(SPACE, level * STUB_INDENT))

            If _model.Meta.Stub(level).DoubleColumn Then
                '*****************
                '* Requirement 2 *
                '*****************
                stubText.Append(value.Code & _
                New String(SPACE, DOUBLECOLUMN_SPACING) & _
                value.Value)
            Else
                stubText.Append(value.Value)
            End If

            Return stubText.ToString
        End Function


        Private Sub WriteDataLine()
            Dim value As Double
            Dim strValue As String

            For c As Integer = 0 To _model.Data.MatrixColumnCount - 1
                value = _model.Data.ReadElement(_row, c)

                Select Case value
                    Case PXConstant.DATASYMBOL_1
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol1 Is Nothing, PXConstant.DATASYMBOL_1_STRING, _model.Meta.DataSymbol1), String)
                    Case PXConstant.DATASYMBOL_2
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol2 Is Nothing, PXConstant.DATASYMBOL_2_STRING, _model.Meta.DataSymbol2), String)
                    Case PXConstant.DATASYMBOL_3
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol3 Is Nothing, PXConstant.DATASYMBOL_3_STRING, _model.Meta.DataSymbol3), String)
                    Case PXConstant.DATASYMBOL_4
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol4 Is Nothing, PXConstant.DATASYMBOL_4_STRING, _model.Meta.DataSymbol4), String)
                    Case PXConstant.DATASYMBOL_5
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol5 Is Nothing, PXConstant.DATASYMBOL_5_STRING, _model.Meta.DataSymbol5), String)
                    Case PXConstant.DATASYMBOL_6
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol6 Is Nothing, PXConstant.DATASYMBOL_6_STRING, _model.Meta.DataSymbol6), String)
                    Case PXConstant.DATASYMBOL_7
                        strValue = DirectCast(IIf(_model.Meta.DataSymbol7 Is Nothing, PXConstant.DATASYMBOL_7_STRING, _model.Meta.DataSymbol7), String)
                    Case PXConstant.DATASYMBOL_NIL
                        strValue = DirectCast(IIf(_model.Meta.DataSymbolNIL Is Nothing, PXConstant.DATASYMBOL_NIL_STRING, _model.Meta.DataSymbolNIL), String)
                    Case Else
                        strValue = value.ToString
                End Select

                'TODO: Handle thousands, precision, decimal

                'Right-aligned values
                _wr.Write(New String(SPACE, _columnWidth - strValue.Length))
                _wr.Write(strValue)
            Next

            _row = _row + 1
        End Sub

    End Class
End Namespace
