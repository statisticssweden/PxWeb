Imports Microsoft.VisualBasic

Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Writes a PXModel to file or a stream in HTML format.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class HtmlFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

        Private _writeBorder As Boolean = False
        Private _writeAlignRight As Boolean = True
        Private _subStubValues() As Integer
        Private _fmt As DataFormatter

        'Private _cellNotes As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()

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
            Using writer As System.IO.StreamWriter = New System.IO.StreamWriter(path, False, System.Text.Encoding.GetEncoding(model.Meta.CodePage))
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
            Dim writer As System.IO.StreamWriter = New System.IO.StreamWriter(stream, System.Text.Encoding.GetEncoding(model.Meta.CodePage))

            DoSerialize(model, writer)
        End Sub
#End Region

        ''' <summary>
        ''' Gets or sets a value indicating whether a border is to be written.
        ''' </summary>
        ''' <value>true write a border; false does not write a border.</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CanWriteBorder() As Boolean
            Get
                Return Me._writeBorder
            End Get
            Set(ByVal value As Boolean)
                Me._writeBorder = value
            End Set
        End Property

#Region "Helper functions"
        Private Sub DoSerialize(ByVal model As PXModel, ByVal wr As System.IO.StreamWriter)

            Dim stub As Paxiom.Variables = model.Meta.Stub
            Dim data As Paxiom.PXData = model.Data
            wr.WriteLine("<HTML>")
            wr.WriteLine("<HEAD>")
            wr.WriteLine("<META http-equiv=""content-type"" content=""text/html; charset=" & model.Meta.CodePage & """>")
            wr.Write("<TITLE>")
            wr.Write(model.Meta.Title)
            wr.WriteLine("</TITLE>")
            wr.WriteLine("</HEAD>")
            wr.WriteLine("<BODY>")
            If _writeBorder Then
                wr.WriteLine("<TABLE BORDER>")
            Else
                wr.WriteLine("<TABLE>")
            End If

            ' Write title to the page
            wr.WriteLine("<TR ALIGN=LEFT>")
            Dim tableColspan As Integer = 1

            Dim headings As Variables = model.Meta.Heading

            If Not headings Is Nothing Then
                For i As Integer = 0 To model.Meta.Heading.Count - 1
                    tableColspan = tableColspan * model.Meta.Heading(i).Values.Count
                Next
            End If

            tableColspan = tableColspan + stub.Count


            wr.Write("<TH COLSPAN=")
            wr.Write(tableColspan)
            wr.Write(">")
            wr.Write(model.Meta.Title)
            wr.WriteLine("</TH>")

            If _writeBorder Then
                wr.WriteLine("</TR>")
            End If

            ReDim _subStubValues(stub.Count)
            CalculateSubValues(stub, 0, _subStubValues)

            WriteHeadings(wr, model)


            Dim levels As Integer = stub.Count
            Dim row As Integer = 0 ' Start at row zero

            ' Write the table
            WriteTable(wr, model, levels, 0, model.Meta.ShowDecimals, row)

            ' Write notes
            WriteNotes(wr, model.Meta, tableColspan)

            wr.WriteLine("</TABLE>")
            wr.WriteLine("</BODY>")
            wr.WriteLine("</HTML>")
            wr.Flush()
        End Sub

        Private Sub WriteHeadings(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel)

            Dim heading As Variables = model.Meta.Heading

            If Not heading Is Nothing Then

                Dim subHeadings(1) As Integer

                If heading.Count > 0 Then
                    ReDim subHeadings(heading.Count - 1)
                End If

                CalculateSubValues(heading, 0, subHeadings)

                Dim timesToWrite As Integer = 1 ' This keep track of the number of times the current heading shall be written
                Dim timesWritten As Integer = 0 ' This keep track of the number of times the current heading has been written
                For index As Integer = 0 To heading.Count - 1

                    wr.WriteLine("<TR ALIGN=LEFT>")
                    If model.Meta.Stub.Count > 0 Then
                        If _writeBorder Then
                            wr.Write("<TH COLSPAN=")
                            wr.Write(model.Meta.Stub.Count)
                            wr.WriteLine("> </TH>")
                        Else
                            wr.WriteLine("<TH> </TH>")
                        End If
                    End If

                    ' Write the heading
                    Dim valuesCount As Integer = heading(index).Values.Count
                    For j As Integer = 0 To timesToWrite - 1

                        Dim headingValues As Paxiom.Values = heading(index).Values
                        For ix As Integer = 0 To headingValues.Count - 1

                            If _writeBorder Then
                                wr.Write("<TH VALIGN=TOP COLSPAN=")
                            Else
                                wr.Write("<TH VALIGN COLSPAN=")
                            End If

                            wr.Write(subHeadings(index))
                            wr.Write(">")
                            wr.Write(headingValues(ix).Text)
                            wr.WriteLine("</TH>")

                            timesWritten = timesWritten + 1
                        Next

                    Next

                    timesToWrite = timesWritten
                    timesWritten = 0

                    wr.WriteLine("</TR>")
                Next

            End If

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


        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter, ByVal model As Paxiom.PXModel, ByVal levels As Integer, ByVal level As Integer, ByVal precision As Integer, ByRef row As Integer)

            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture

            _fmt = New DataFormatter(model)

            If level = levels Then
                ' Time to write the data to the file
                WriteDataLine(wr, model, ci, precision, row)

                ' Close this row. The closing tag is not writen if level + 1 < levels, se
                ' the else clause below
                wr.WriteLine("</TR>")

                row = row + 1
            Else
                Dim values As Paxiom.Values = model.Meta.Stub(level).Values
                Dim nextLevel As Integer = level + 1

                For i As Integer = 0 To values.Count - 1

                    If _writeBorder Then

                        ' Med tabellram kan "<TR ALIGN=RIGHT>" bara förekomma för den första
                        ' föspalsvariabeln (annars blir justeringen fel). Denna switch 
                        ' slås på igen i metoden "WriteLine", så att "<TR ALIGN=RIGHT>"
                        ' kommer att skrivas ut för efterföljande förspaltsvariabler.
                        If _writeAlignRight Then
                            wr.WriteLine("<TR ALIGN=RIGHT>")
                            _writeAlignRight = False
                        End If

                        If level + 1 < levels Then
                            wr.Write("<TH ALIGN=LEFT VALIGN=TOP ROWSPAN=")
                            wr.Write(_subStubValues(level))
                            wr.Write(">")
                        Else
                            wr.Write("<TH ALIGN=LEFT>")
                        End If

                    Else
                        wr.WriteLine("<TR ALIGN=RIGHT>")
                        wr.Write("<TH ALIGN=LEFT VALIGN=TOP>")
                    End If

                    'If values(i).HasNotes Then
                    '    For n As Integer = 0 To values(i).Notes.Count - 1
                    '        _cellNotes.Add(values(i).Notes(n).Text)
                    '    Next
                    'End If

                    wr.Write(values(i).Text)
                    wr.WriteLine("</TH>")

                    If Not _writeBorder And level + 1 < levels Then
                        wr.WriteLine("</TR>")
                    End If

                    Dim decimals As Integer = model.Meta.ShowDecimals
                    If values(i).HasPrecision Then
                        decimals = values(i).Precision
                    End If

                    WriteTable(wr, model, levels, nextLevel, decimals, row)
                Next
            End If

        End Sub

        
        Private Sub WriteDataLine(ByVal wr As System.IO.StreamWriter, ByVal model As PCAxis.Paxiom.PXModel, ByVal ci As System.Globalization.CultureInfo, ByVal precision As Integer, ByVal row As Integer)
            Dim value As String
            Dim n As String = String.Empty
            Dim dataNote As String = String.Empty

            Dim data As PCAxis.Paxiom.PXData = model.Data

            For c As Integer = 0 To data.MatrixColumnCount - 1

                wr.Write("<TD>")
                value = _fmt.ReadElement(row, c, n, dataNote)
                wr.Write(value)
                wr.WriteLine("</TD>")
            Next

            ' Turn on the switch to write "<TR ALIGN=RIGHT>"
            _writeAlignRight = True

        End Sub

        Private Sub WriteNotes(ByVal wr As System.IO.StreamWriter, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal tableColspan As Integer)

            Dim stub As PCAxis.Paxiom.Variables = meta.Stub

            wr.WriteLine("<TR ALIGN=LEFT>")
            wr.Write("<TD COLSPAN=")
            wr.Write(tableColspan)
            wr.WriteLine(">")

            For n As Integer = 0 To meta.Notes.Count - 1
                Dim note As Paxiom.Note = meta.Notes(n)
                wr.Write(SplitNoteText(note))
            Next

            For sn As Integer = 0 To stub.Count - 1
                Dim notes As Paxiom.Notes = stub(sn).Notes
                If notes IsNot Nothing Then
                    For n As Integer = 0 To notes.Count - 1
                        wr.Write(SplitNoteText(notes(n)))
                    Next
                End If
            Next

            ' TODO: Write VALUENOTE and VALUENOTEX here
            For n As Integer = 0 To stub.Count - 1
                For v As Integer = 0 To stub(n).Values.Count - 1
                    Dim valueNotes As Notes = stub(n).Values(v).Notes

                    If valueNotes IsNot Nothing Then
                        For vn As Integer = 0 To valueNotes.Count - 1
                            wr.Write(SplitNoteText(valueNotes(vn)))
                        Next
                    End If
                Next
            Next

            Dim cellNotes As CellNotes = meta.CellNotes

            If Not cellNotes Is Nothing Then
                For cn As Integer = 0 To cellNotes.Count - 1
                    wr.Write(SplitNoteText(cellNotes(cn)))
                Next

            End If

            wr.WriteLine("</TD>")
            wr.WriteLine("</TR>")

        End Sub

        ''' <summary>
        ''' Split a note text into several lines.
        ''' </summary>
        ''' <param name="note">The note.</param>
        ''' <returns></returns>
        Private Function SplitNoteText(ByVal note As PCAxis.Paxiom.NoteBase) As String
            Const numberSign As Char = "#"c

            Dim noteText As String = note.Text
            Dim rows As Integer = 1
            Dim charCounter As Integer = 1
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder(noteText.Length)

            For i As Integer = 0 To noteText.Length - 1

                If noteText(i) = numberSign Then
                    sb.Append("<BR>")
                    sb.Append(System.Environment.NewLine)
                Else
                    sb.Append(noteText(i))
                End If

            Next

            sb.Append("<BR>")

            Return sb.ToString()

        End Function

#End Region

    End Class
End Namespace
