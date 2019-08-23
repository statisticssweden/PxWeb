Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Class for handling the Aremos file format
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AremosFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

        Private _model As PCAxis.Paxiom.PXModel
        Private _weightHeading As Integer()
        Private _weightStub As Integer()
        Private _oldPMHeading As Integer()
        Private _oldPMStub As Integer()

        ''' <summary>
        ''' Implementation of IPXModelStreamSerializer function
        ''' </summary>
        ''' <param name="model"></param>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize
            If model Is Nothing Then Throw New ArgumentNullException("model")

            Using writer As System.IO.StreamWriter = New System.IO.StreamWriter(path, False, EncodingUtil.GetEncoding(model.Meta.CodePage))
                WriteModelToStream(model, writer)
            End Using
        End Sub

        ''' <summary>
        ''' Implementation of IPXModelStreamSerializer function
        ''' </summary>
        ''' <param name="model"></param>
        ''' <param name="stream"></param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal stream As System.IO.Stream) Implements IPXModelStreamSerializer.Serialize
            If model Is Nothing Then Throw New ArgumentNullException("model")
            If stream Is Nothing Then Throw New ArgumentNullException("stream")

            If Not stream.CanWrite Then Throw New ArgumentException("The stream does not support writing")

            Dim encoding As System.Text.Encoding
            encoding = EncodingUtil.GetEncoding(model.Meta.CodePage)
            Dim writer As System.IO.StreamWriter = New System.IO.StreamWriter(stream, encoding)
            WriteModelToStream(model, writer)
        End Sub

        Private Sub WriteModelToStream(ByVal model As PXModel, ByRef wr As System.IO.StreamWriter)
            _model = model

            WriteMatrix(wr)
        End Sub


        ''' <summary>
        ''' Writes information for the stub
        ''' </summary>
        ''' <param name="rowNumber">Number of the row</param>
        ''' <param name="wr">The writer</param>
        ''' <remarks></remarks>
        Private Sub WriteStubInfo(ByVal rowNumber As Integer, ByRef wr As System.IO.StreamWriter)
            Dim sb As New System.Text.StringBuilder
            Dim str As String
            Dim varLength As Integer
            Dim valLength As Integer

            str = CutString(_model.Meta.Contents, 20)
            sb.Append(String.Format("{0,-20}", str))
            sb.Append("  ")
            ' Loop stub variables and write variable/value info
            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                If i > 0 Then
                    sb.Append(" ") 'variable/value separator
                End If
                sb.Append(CutString(_model.Meta.Stub(i).Code + "=" + _model.Meta.Stub(i).Values(_weightStub(i)).Value, 42))
            Next

            If sb.ToString.Length <= 64 Then
                'The string is not to long, write it!
                wr.Write(String.Format("{0,-16}", _model.Meta.Matrix + CStr(rowNumber)))
                wr.WriteLine(sb.ToString)
                Exit Sub
            End If

            'The string is to long! Rebuild it...
            '------------------------------------
            If _model.Meta.Stub.Count < 3 Then
                'Dont cut names if less than 3 variables in the stub
                varLength = 0
                valLength = 0
            ElseIf _model.Meta.Stub.Count = 3 Then
                '10 chars when 3 variables in the stub
                varLength = 10
                valLength = 10
            Else
                'More than 3 variables in the stub...
                varLength = 6
                valLength = 7
            End If

            sb = New System.Text.StringBuilder
            'Contents max 11 chars
            sb.Append(CutString(_model.Meta.Contents, 11))
            sb.Append(" ")
            ' Loop stub variables and write variable/value info
            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                If i > 0 Then
                    sb.Append(" ") 'variable/value separator
                End If
                If _model.Meta.Stub.Count < 3 Then
                    'Don´t cut names of variables and values
                    sb.Append(_model.Meta.Stub(i).Code)
                    sb.Append("=")
                    sb.Append(_model.Meta.Stub(i).Values(_weightStub(i)).Value)
                Else
                    'Cut the names...
                    sb.Append(CutString(_model.Meta.Stub(i).Code, varLength))
                    sb.Append("=")
                    sb.Append(CutString(_model.Meta.Stub(i).Values(_weightStub(i)).Value, valLength))
                End If
            Next

            wr.Write(String.Format("{0,-16}", _model.Meta.Matrix + CStr(rowNumber)))
            'Total string max 64 chars
            wr.WriteLine(CutString(sb.ToString, 64))

        End Sub

        ''' <summary>
        ''' Writes information for the heading
        ''' </summary>
        ''' <param name="wr">The writer</param>
        ''' <remarks></remarks>
        Private Sub WriteHeadInfo(ByRef wr As System.IO.StreamWriter)
            Dim firstValue As String = String.Empty
            Dim lastValue As String = String.Empty
            Dim timescale As String = String.Empty

            If _model.Meta.Heading.Count > 1 Then
                Throw New PXOperationException("Only the timevariable can be in the heading when saving to aremos file format")
            End If

            If Not _model.Meta.Heading(0).HasTimeValue Then
                Throw New PXOperationException("", "ERROR_MISSING_TIMEVALUE")
            End If

            wr.Write(String.Format("{0,-16}", ""))
            wr.Write(String.Format("{0,-28}", CutString(_model.Meta.Source, 16)))

            ' Only one variable is allowed in head for aremos format
            firstValue = FormatTimeVal(_model.Meta.Heading(0).Values(0).TimeValue, _model.Meta.Heading(0).TimeScale)
            lastValue = FormatTimeVal(_model.Meta.Heading(0).Values(_model.Meta.Heading(0).Values.Count - 1).TimeValue, _model.Meta.Heading(0).TimeScale)

            Select Case _model.Meta.Heading(0).TimeScale
                Case TimeScaleType.Annual
                    timescale = "A"
                Case TimeScaleType.Halfyear
                    timescale = "H"
                Case TimeScaleType.Monthly
                    timescale = "M"
                Case TimeScaleType.Quartely
                    timescale = "Q"
                Case TimeScaleType.Weekly
                    timescale = "W"
                Case Else
                    Throw New PXOperationException("Timescale not set")
            End Select

            timescale = timescale & _model.Meta.ShowDecimals.ToString

            wr.WriteLine(firstValue + "  " + lastValue + "  " + timescale)

        End Sub

        ''' <summary>
        ''' Cuts a string
        ''' </summary>
        ''' <param name="str">The string to cut</param>
        ''' <param name="length">The maximum length of the string</param>
        ''' <returns>The new cutted string</returns>
        ''' <remarks></remarks>
        Private Function CutString(ByVal str As String, ByVal length As Integer) As String
            Dim ret As String

            If String.IsNullOrEmpty(str) Then
                Return ""
            End If

            If str.Length > length Then
                ret = str.Substring(0, length)
            Else
                ret = str
            End If

            Return ret
        End Function

        Private Function FormatTimeVal(ByVal value As String, ByVal timeScaleType As PCAxis.Paxiom.TimeScaleType) As String
            Dim year As String = ""
            Dim timescale As String = ""

            year = value.Substring(0, 4)

            If timeScaleType = Paxiom.TimeScaleType.Annual Then
                timescale = "01"
            Else
                timescale = value.Substring(4)
                timescale = timescale.PadLeft(2, "0"c)
            End If

            Return String.Concat(year, timescale)
        End Function

        Private Sub WriteMatrix(ByVal wr As System.IO.StreamWriter)
            If _model.Meta.Stub.Count > 0 Then
                Dim value As String
                Dim df As DataFormatter = New DataFormatter(_model)

                'Creates pMatrix
                _oldPMHeading = CreatePMatrix(_model.Meta.Heading)
                _oldPMStub = CreatePMatrix(_model.Meta.Stub)

                ' Init _weight arrays
                If _model.Meta.Heading.Count = 0 Then
                    _weightHeading = Nothing
                Else
                    _weightHeading = New Integer(_model.Meta.Heading.Count - 1) {}
                End If
                If _model.Meta.Stub.Count = 0 Then
                    _weightStub = Nothing
                Else
                    _weightStub = New Integer(_model.Meta.Stub.Count - 1) {}
                End If


                For row As Integer = 0 To _model.Data.MatrixRowCount - 1
                    CalcStubWeights(row, _model.Meta.Stub)
                    WriteStubInfo(row + 1, wr)
                    WriteHeadInfo(wr)
                    ' Only five columns per row
                    Dim colCounter As Integer = 0
                    For col As Integer = 0 To _model.Data.MatrixColumnCount - 1
                        If colCounter = 5 Then
                            wr.WriteLine()
                            colCounter = 0
                        End If

                        df.DecimalSeparator = "."
                        df.DataFormat = "0.000000e+00"
                        value = df.ReadElement(row, col)

                        wr.Write("   ")
                        wr.Write(value)

                        colCounter += 1
                    Next
                    wr.WriteLine()
                Next
            End If
        End Sub

        ''' <summary>
        ''' Creates the helpmatrixes that are used to identify which variable values that corresponds
        ''' to a given row or column in the datamatrix
        ''' </summary>
        ''' <param name="variables">The variables (Stub or Heading)</param>
        ''' <returns>An Integer array</returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Identifies value position in the heading variables for the incoming columnIndex
        ''' </summary>
        ''' <param name="columnIndex"></param>
        ''' <param name="headingVariables"></param>
        ''' <remarks></remarks>
        Private Sub CalcHeadingWeights(ByVal columnIndex As Integer, ByVal headingVariables As Variables)
            Dim size As Integer = _weightHeading.Length

            For index As Integer = 0 To size - 1
                _weightHeading(index) = Convert.ToInt32(Math.Floor((columnIndex / _oldPMHeading(index + 1)))) Mod headingVariables(index).Values.Count
            Next
        End Sub

        ''' <summary>
        ''' Identifies value position in the stub variables for the incoming rowIndex
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <param name="stubVariables"></param>
        ''' <remarks></remarks>
        Private Sub CalcStubWeights(ByVal rowIndex As Integer, ByVal stubVariables As Variables)
            Dim size As Integer = _weightStub.Length

            For index As Integer = 0 To size - 1
                _weightStub(index) = Convert.ToInt32(Math.Floor(rowIndex / _oldPMStub(index + 1))) Mod stubVariables(index).Values.Count
            Next
        End Sub

    End Class


End Namespace