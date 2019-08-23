Imports Microsoft.VisualBasic


Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Writes a PXModel to file or a stream in PRN format.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PrnFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

        Const _space As String = ControlChars.Quote + " " + ControlChars.Quote

        Private _writeTitle As Boolean = True
        Private _delimiter As Char = ","c
        Private _writeDataOnly As Boolean = False


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
            writer.Flush()
        End Sub
#End Region

#Region "Public properties"
        Public Property Delimiter() As Char
            Get
                Return _delimiter
            End Get
            Set(ByVal value As Char)
                _delimiter = value
            End Set

        End Property


        Public Property CanWriteTitle() As Boolean
            Get
                Return _writeTitle
            End Get
            Set(ByVal value As Boolean)
                _writeTitle = value
            End Set

        End Property

        Public Property CanWriteDataOnly() As Boolean
            Get
                Return _writeDataOnly
            End Get
            Set(ByVal value As Boolean)
                _writeDataOnly = value
            End Set

        End Property
#End Region

        Private Sub DoSerialize(ByVal model As PXModel, ByVal wr As System.IO.StreamWriter)
            Dim stub As Paxiom.Variables = model.Meta.Stub
            Dim data As Paxiom.PXData = model.Data
            Dim level As Integer = 0
            Dim levels As Integer = stub.Count
            Dim row As Integer = 0 ' Start at row zero

            If _writeDataOnly Then
                WriteTable(wr, model)
                Return
            End If

            If Not _writeTitle Then
                If stub.Count > 0 Then
                    WriteTable(wr, model, levels, 0, model.Meta.ShowDecimals, row, String.Empty)
                Else
                    WriteTable(wr, model)
                End If
                ' Notes are not written in this case
                Return
            End If


            wr.Write(ControlChars.Quote)
            wr.Write(Util.GetModelTitle(model))
            wr.WriteLine(ControlChars.Quote)
            wr.WriteLine()


            ' Write headings
            WriteHeadings(wr, model)

            ' Write the table
            WriteTable(wr, model, levels, 0, model.Meta.ShowDecimals, row)


            wr.WriteLine() ' Put some space between the table and the notes

            ' Write notes
            WriteNotes(wr, model.Meta)

        End Sub

        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As Paxiom.PXModel)
            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture

            Dim data As PCAxis.Paxiom.PXData = model.Data

            For rowIdx As Integer = 0 To data.MatrixRowCount - 1

                WriteDataLine(wr, model, ci, model.Meta.ShowDecimals, rowIdx)

            Next

        End Sub

        ''' <summary>
        ''' Funktion för utskrift av rad i tabellfil. Funktionen är rekursiv med en rekursion per 
        ''' förspaltsvariabel. Om sista förspaltvariabel behandlas skrivs också siffrorna ut.
        ''' </summary>
        ''' <param name="wr"></param>
        ''' <param name="model"></param>
        ''' <param name="levels"></param>
        ''' <param name="level"></param>
        ''' <param name="row"></param>
        ''' <remarks></remarks>
        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel, ByVal levels As Integer, ByVal level As Integer, ByVal precision As Integer, ByRef row As Integer)

            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture

            If level = levels Then
                ' If we have some "stubs", we must write a delimiter to separate 
                ' the last "stub" from its values
                If (levels > 0) Then
                    wr.Write(_delimiter)
                End If

                ' Time to write the data to the file
                WriteDataLine(wr, model, ci, precision, row)

                row = row + 1
            Else
                Dim values As Paxiom.Values = model.Meta.Stub(level).Values
                Dim nextLevel As Integer = level + 1

                For i As Integer = 0 To values.Count - 1
                    ' Write empty for "stubs"
                    For l As Integer = 0 To level - 1
                        wr.Write(_space)
                        wr.Write(_delimiter)
                    Next

                    wr.Write(ControlChars.Quote)
                    wr.Write(values(i).Text)

                    If level = levels - 1 Then
                        ' The data must be on the same line as the "stub"
                        wr.Write(ControlChars.Quote)
                    Else
                        wr.WriteLine(ControlChars.Quote)
                    End If

                    Dim decimals As Integer = model.Meta.ShowDecimals
                    If values(i).HasPrecision Then
                        decimals = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, nextLevel, decimals, row)
                Next
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="wr"></param>
        ''' <param name="model"></param>
        ''' <param name="currentStubValue"></param>
        ''' <param name="levels"></param>
        ''' <param name="level"></param>
        ''' <param name="row"></param>
        ''' <remarks>Without headings the stubs are written on the same row as the data.</remarks>
        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel, ByVal levels As Integer, ByVal level As Integer, ByVal precision As Integer, ByRef row As Integer, ByVal currentStubValue As String)

            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture

            If level = 0 Then
                ' This is here for cosmetic and performanse reasons. This avoid a space as a first character.
                ' You could trim the newValue variable in the else clause below, but that would be a rather
                ' expensive solution, I think.
                Dim values As Paxiom.Values = model.Meta.Stub(0).Values

                For i As Integer = 0 To values.Count - 1
                    Dim stubValue As String = ControlChars.Quote + values(i).Text + ControlChars.Quote

                    If values(i).HasPrecision Then
                        precision = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, 1, precision, row, stubValue)
                Next
            ElseIf level = levels Then

                ' Write the "stubs" before the corresponding data
                wr.Write(currentStubValue)
                ' If we have some "stubs", we must write a delimiter to separate 
                ' the last "stub" from its values
                If (levels > 0) Then
                    wr.Write(_delimiter)
                End If

                ' Time to write the data to the file
                WriteDataLine(wr, model, ci, precision, row)

                row = row + 1
            Else
                Dim values As Paxiom.Values = model.Meta.Stub(level).Values
                Dim nextLevel As Integer = level + 1

                For i As Integer = 0 To values.Count - 1

                    Dim newValue As String = currentStubValue & _delimiter & ControlChars.Quote & values(i).Text & ControlChars.Quote
                    Dim decimals As Integer = model.Meta.ShowDecimals
                    If values(i).HasPrecision Then
                        decimals = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, nextLevel, decimals, row, newValue)
                Next
            End If

        End Sub

        ''' <summary>
        ''' Split a note text into several lines.
        ''' </summary>
        ''' <param name="note">The note.</param>
        ''' <param name="lineLength">The length of the line.</param>
        ''' <returns></returns>
        ''' <remarks>This algoritm is much simpler than the original.</remarks>
        Private Function SplitNoteText(ByVal note As PCAxis.Paxiom.NoteBase, ByVal lineLength As Integer) As String
            Const space As Char = " "c

            Dim noteText As String = note.Text
            Dim charCounter As Integer = 1
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder(noteText.Length)

            sb.Append(ControlChars.Quote)

            For i As Integer = 0 To noteText.Length - 1

                If Char.Equals(noteText(i), ControlChars.Cr) Then
                    'Explicit new line
                    sb.Append(space)
                    sb.Append(ControlChars.Quote)
                    sb.Append(System.Environment.NewLine)
                    sb.Append(ControlChars.Quote)
                    charCounter = 0
                ElseIf charCounter >= lineLength And noteText(i) = space Then
                    sb.Append(noteText(i))
                    sb.Append(ControlChars.Quote)
                    sb.Append(System.Environment.NewLine)
                    sb.Append(ControlChars.Quote)
                    charCounter = 0
                Else
                    If Not Char.Equals(noteText(i), ControlChars.Lf) Then
                        sb.Append(noteText(i))
                    End If
                End If

                charCounter = charCounter + 1
            Next

            sb.Append(ControlChars.Quote)

            Return sb.ToString()

        End Function

        Private _dataFormatter As DataFormatter = Nothing

        Private Sub WriteDataLine(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel, ByVal ci As System.Globalization.CultureInfo, ByVal precision As Integer, ByVal row As Integer)

            Dim meta As PCAxis.Paxiom.PXMeta = model.Meta
            Dim data As PCAxis.Paxiom.PXData = model.Data

            If _dataFormatter Is Nothing Then
                _dataFormatter = New DataFormatter(model)
                _dataFormatter.DecimalSeparator = "."
                _dataFormatter.ThousandSeparator = ""
            End If

            For c As Integer = 0 To data.MatrixColumnCount - 1

                Dim value As String = _dataFormatter.ReadElement(row, c)

                wr.Write(value)

                If (c < data.MatrixColumnCount - 1) Then
                    wr.Write(_delimiter)
                End If
            Next

            wr.WriteLine()

        End Sub

        Private Function CalculateSubHeadings(ByVal vars As Variables, ByVal level As Integer, ByRef subHeadings As Integer()) As Integer

            If vars.Count = 0 Then
                subHeadings(level) = 0
                Return 0
            ElseIf vars.Count - 1 = level Then
                subHeadings(level) = 0
                Return vars(level).Values.Count
            Else
                Dim nextLevel As Integer = level + 1
                Dim ret As Integer = CalculateSubHeadings(vars, nextLevel, subHeadings)
                subHeadings(level) = ret
                Return ret * vars(level).Values.Count
            End If

        End Function

        Private Sub WriteHeadings(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel)

            Dim heading As Variables = model.Meta.Heading

            If Not heading Is Nothing Then

                Dim subHeadings(1) As Integer

                If heading.Count > 0 Then
                    ReDim subHeadings(heading.Count - 1)
                End If

                CalculateSubHeadings(heading, 0, subHeadings)

                Dim timesToWrite As Integer = 1 ' This keep track of the number of times the current heading shall be written
                Dim timesWritten As Integer = 0 ' This keep track of the number of times the current heading has been written
                For index As Integer = 0 To heading.Count - 1

                    ' First write space for stubs...
                    For e As Integer = 0 To model.Meta.Stub.Count - 1
                        wr.Write(_space)
                        wr.Write(_delimiter)
                    Next

                    ' Write the heading
                    Dim valuesCount As Integer = heading(index).Values.Count
                    For j As Integer = 0 To timesToWrite - 1

                        Dim headingValues As Paxiom.Values = heading(index).Values
                        For ix As Integer = 0 To headingValues.Count - 1
                            wr.Write(ControlChars.Quote)
                            wr.Write(headingValues(ix).Text)
                            wr.Write(ControlChars.Quote)

                            ' Write space after heading (subtract one for this heading)
                            For e As Integer = 1 To subHeadings(index) - 1
                                wr.Write(_delimiter)
                                wr.Write(_space)
                            Next

                            If ix < headingValues.Count - 1 Then
                                wr.Write(_delimiter)
                            End If

                            timesWritten = timesWritten + 1
                        Next

                        If j < timesToWrite - 1 Then
                            wr.Write(_delimiter)
                        End If

                    Next

                    timesToWrite = timesWritten
                    timesWritten = 0

                    wr.WriteLine()
                Next
            End If

        End Sub

        Private Sub WriteNotes(ByVal wr As System.IO.StreamWriter, ByVal meta As PCAxis.Paxiom.PXMeta)
            Const NOTE_ROW_LENTH As Integer = 60 ' This value is taken from the original code

            Dim vars As PCAxis.Paxiom.Variables = meta.Variables

            For n As Integer = 0 To meta.Notes.Count - 1
                Dim note As Paxiom.Note = meta.Notes(n)

                wr.Write(SplitNoteText(note, NOTE_ROW_LENTH))
                wr.WriteLine()
                wr.WriteLine()
            Next

            For sn As Integer = 0 To vars.Count - 1
                Dim notes As Paxiom.Notes = vars(sn).Notes
                If notes IsNot Nothing Then
                    For n As Integer = 0 To notes.Count - 1
                        wr.Write(SplitNoteText(notes(n), NOTE_ROW_LENTH))
                        wr.WriteLine()
                        wr.WriteLine()
                    Next
                End If
            Next

            'VALUENOTE and VALUENOTEX
            For n As Integer = 0 To vars.Count - 1
                For v As Integer = 0 To vars(n).Values.Count - 1
                    Dim valueNotes As Notes = vars(n).Values(v).Notes

                    If valueNotes IsNot Nothing Then
                        For vn As Integer = 0 To valueNotes.Count - 1
                            wr.Write(SplitNoteText(valueNotes(vn), NOTE_ROW_LENTH))
                            wr.WriteLine()
                            wr.WriteLine()
                        Next
                    End If
                Next
            Next

            Dim cellNotes As CellNotes = meta.CellNotes

            If Not cellNotes Is Nothing Then
                For cn As Integer = 0 To cellNotes.Count - 1
                    wr.Write(SplitNoteText(cellNotes(cn), NOTE_ROW_LENTH))
                    wr.WriteLine()
                    wr.WriteLine()
                Next

            End If
        End Sub

    End Class

End Namespace

