Imports Microsoft.VisualBasic

Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Writes a PXModel to file or a stream in a simple TXT format.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TextTableFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

#Region "Interface members"
        ''' <summary>
        ''' Write a PXModel to a file.
        ''' </summary>
        ''' <param name="model">The PXModel to write.</param>
        ''' <param name="path">The complete file path to write to. <I>path</I> can be a file name.</param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize

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
        Public Sub Serialize(ByVal model As PXModel, ByVal stream As System.IO.Stream) Implements IPXModelStreamSerializer.Serialize

            If model Is Nothing Then Throw New ArgumentNullException("model")
            If stream Is Nothing Then Throw New ArgumentNullException("stream")

            If Not stream.CanWrite Then Throw New ArgumentException("The stream does not support writing")

            Dim encoding As System.Text.Encoding
            encoding = EncodingUtil.GetEncoding(model.Meta.CodePage)
            Dim writer As System.IO.StreamWriter = New System.IO.StreamWriter(stream, encoding)
            DoSerialize(model, writer)

        End Sub
#End Region

#Region "Helper functions"
        Private Sub DoSerialize(ByVal model As PXModel, ByVal wr As System.IO.StreamWriter)

            ' Start with the headings
            WriteHeadings(wr, model)

            Dim levels As Integer = model.Meta.Stub.Count

            ' Write the table
            If levels > 0 Then
                Dim row As Integer = 0 ' Start at row zero
                WriteTable(wr, model, levels, 0, model.Meta.ShowDecimals, row, String.Empty)
            Else
                WriteTable(wr, model)
            End If

            ' No notes are written for this type...

        End Sub

        Private Sub WriteHeadings(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel)

            Const space As String = " "
            Dim heading As Variables = model.Meta.Heading
            Dim first As Boolean = True

            If Not heading Is Nothing Then

                Dim subHeadings(1) As Integer

                If heading.Count > 0 Then
                    ReDim subHeadings(heading.Count - 1)
                End If

                CalculateSubValues(heading, 0, subHeadings)

                Dim timesToWrite As Integer = 1 ' This keep track of the number of times the current heading shall be written
                Dim timesWritten As Integer = 0 ' This keep track of the number of times the current heading has been written
                For index As Integer = 0 To heading.Count - 1

                    If first Then
                        ' Write stub names
                        For e As Integer = 0 To model.Meta.Stub.Count - 1
                            wr.Write(model.Meta.Stub(e).Name)
                            wr.Write(ControlChars.Tab)
                            If model.Meta.Stub(e).DoubleColumn Then
                                wr.Write(ControlChars.Tab)
                            End If
                        Next
                        first = False
                    Else
                        ' Write space for stubs...
                        For e As Integer = 0 To model.Meta.Stub.Count - 1
                            wr.Write(ControlChars.Tab)
                            If model.Meta.Stub(e).DoubleColumn Then
                                wr.Write(ControlChars.Tab)
                            End If
                        Next
                    End If

                    ' Write the heading
                    Dim valuesCount As Integer = heading(index).Values.Count
                    For j As Integer = 0 To timesToWrite - 1

                        Dim headingValues As Paxiom.Values = heading(index).Values
                        For ix As Integer = 0 To headingValues.Count - 1
                            wr.Write(space)
                            wr.Write(space)
                            wr.Write(headingValues(ix).Value)
                            wr.Write(space)

                            ' Write space after heading (subtract one for this heading)
                            For e As Integer = 1 To subHeadings(index) - 1
                                wr.Write(ControlChars.Tab)
                            Next

                            If ix < headingValues.Count - 1 Then
                                wr.Write(ControlChars.Tab)
                            End If

                            timesWritten = timesWritten + 1
                        Next

                        If j < timesToWrite - 1 Then
                            wr.Write(ControlChars.Tab)
                        End If

                    Next

                    timesToWrite = timesWritten
                    timesWritten = 0

                    wr.WriteLine()
                Next
            End If

        End Sub


        ''' <summary>
        ''' Writes the data in a Paxiom model to a stream.
        ''' </summary>
        ''' <param name="wr">A StreamWriter that encapsulates the stream.</param>
        ''' <param name="model">The Paxiom model to be written.</param>
        ''' <param name="currentStubValue">The current aggregated stub value.</param>
        ''' <param name="levels">The number of stub levels.</param>
        ''' <param name="level">The current level.</param>
        ''' <param name="row">The curent row.</param>
        ''' <remarks>This method handle the case when the model contains stubs.</remarks>
        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As Paxiom.PXModel, ByVal levels As Integer, ByVal level As Integer, ByVal precision As Integer, ByRef row As Integer, ByVal currentStubValue As String)

            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo(&H41D) ' Swedish culture for writing "," as decimal separator
            Dim doubleColumn As Boolean
            If level <> levels Then
                doubleColumn = model.Meta.Stub(level).DoubleColumn
            End If

            If level = 0 Then
                ' This is here for cosmetic and performanse reasons. This avoid a space as a first character.
                ' You could trim the newValue variable in the else clause below, but that would be a rather
                ' expensive solution, I think.
                Dim values As Paxiom.Values = model.Meta.Stub(0).Values

                For i As Integer = 0 To values.Count - 1
                    Dim stubValue As String = values(i).Value

                    If doubleColumn Then
                        stubValue = values(i).Code & ControlChars.Tab & stubValue
                    End If

                    If values(i).HasPrecision Then
                        precision = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, 1, precision, row, stubValue)
                Next

            ElseIf level = levels Then
                ' Write the "stubs" before the corresponding data
                wr.Write(currentStubValue)
                ' Time to write the data to the file
                WriteDataLine(wr, model, ci, precision, row)

                wr.WriteLine()
                row = row + 1
            Else
                Dim values As Paxiom.Values = model.Meta.Stub(level).Values
                Dim nextLevel As Integer = level + 1

                For i As Integer = 0 To values.Count - 1

                    'Dim newValue As String = (currentValue + " " + values(i).Value).TrimStart() ' Alternative solution to get rid of the leading space (only relevant one time)
                    Dim newValue As String
                    If doubleColumn Then
                        newValue = currentStubValue & ControlChars.Tab & values(i).Code & ControlChars.Tab & values(i).Value
                    Else
                        newValue = currentStubValue & ControlChars.Tab & values(i).Value
                    End If


                    Dim currentPrecision As Integer = model.Meta.ShowDecimals
                    If values(i).HasPrecision Then
                        currentPrecision = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, nextLevel, currentPrecision, row, newValue)
                Next
            End If

        End Sub


        ''' <summary>
        ''' Writes the data in a Paxiom model to a stream.
        ''' </summary>
        ''' <param name="wr">A StreamWriter that encapsulates the stream.</param>
        ''' <param name="model">The Paxiom model to be written.</param>
        ''' <remarks>This method handels the case when the model does not contains any stubs.</remarks>
        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As Paxiom.PXModel)

            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo(&H41D) ' Swedish culture for writing "," as decimal separator

            Dim data As PCAxis.Paxiom.PXData = model.Data
            Dim contents As String = model.Meta.Contents()

            For rowIdx As Integer = 0 To data.MatrixRowCount - 1

                wr.Write(contents)
                WriteDataLine(wr, model, ci, model.Meta.ShowDecimals, rowIdx)

            Next

        End Sub


        Private _dataFormatter As DataFormatter = Nothing
        Private Sub WriteDataLine(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel, ByVal ci As System.Globalization.CultureInfo, ByVal precision As Integer, ByVal row As Integer)

            Dim meta As PCAxis.Paxiom.PXMeta = model.Meta
            Dim data As PCAxis.Paxiom.PXData = model.Data

            If _dataFormatter Is Nothing Then
                _dataFormatter = New DataFormatter(model)
                _dataFormatter.DecimalSeparator = "."
            End If

            '' Create the the string for format value output
            'Dim formatString As String = New String("0"c, 1)
            'If precision > 0 Then
            '    formatString = formatString + "."
            '    formatString = String.Concat(formatString, New String("0"c, precision))
            'End If

            Dim value As String
            For c As Integer = 0 To data.MatrixColumnCount - 1
                wr.Write(ControlChars.Tab)
                value = _dataFormatter.ReadElement(row, c)
                wr.Write(value)
            Next

        End Sub

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

#End Region

    End Class
End Namespace
