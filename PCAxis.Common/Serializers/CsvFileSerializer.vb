Imports System.Collections.Specialized


Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Writes a PXModel to file or a stream in CSV format.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CsvFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer


#Region "Private fields"
        Private _model As PCAxis.Paxiom.PXModel
        Private _delimiter As Char = ","c
        Private _decimalSeparator As Char = "."c
        Private _doubleColumn As Boolean = False
        Private _title As Boolean = False
        Private _thosandSeparator As Boolean = False
        Private _wrapTextWithQuote As Boolean = True
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "IPXModelStreamSerializer Interface members"
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

        ''' <summary>
        ''' Serializes the model to the stream in the csv format.
        ''' </summary>
        ''' <param name="model">The model to serialize</param>
        ''' <param name="wr">The stream to serialize to</param>
        ''' <remarks></remarks>
        Private Sub DoSerialize(ByVal model As PXModel, ByVal wr As System.IO.StreamWriter)
            _model = model
            WriteTitle(wr)
            WriteHeading(wr)
            WriteTable(wr)
        End Sub

        ''' <summary>
        ''' Writes the title to the stream if title is set to true
        ''' </summary>
        ''' <param name="wr">The stream to write to</param>
        ''' <remarks></remarks>
        Private Sub WriteTitle(ByVal wr As System.IO.StreamWriter)
            If Me.Title = True Then
                If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                wr.Write(Util.GetModelTitle(_model))
                If Me.WrapTextWithQuote Then wr.WriteLine(ControlChars.Quote)
                wr.WriteLine()
            End If
        End Sub

        ''' <summary>
        ''' Writes the heading (the column namnes separated by comma) to a stream
        ''' </summary>
        ''' <param name="wr">A StreamWriter that encapsulates the stream</param>
        ''' <remarks></remarks>
        Private Sub WriteHeading(ByVal wr As System.IO.StreamWriter)
            ' Write stub variable names 
            For i As Integer = 0 To _model.Meta.Stub.Count - 1
                If i > 0 Then
                    wr.Write(Me.Delimiter)
                End If

                If Me.DoubleColumn Then
                    If _model.Meta.Stub(i).DoubleColumn Then
                        If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                        wr.Write(_model.Meta.Stub(i).Code)
                        If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                        wr.Write(Me.Delimiter)
                    End If
                End If
                If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                wr.Write(_model.Meta.Stub(i).Name)
                If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
            Next

            'Write concatenated heading variable values
            If _model.Meta.Heading.Count > 0 Then
                Dim sc As StringCollection

                wr.Write(Me.Delimiter)

                sc = ConcatHeadingValues(0)
                For i As Integer = 0 To sc.Count - 1
                    If i > 0 Then
                        wr.Write(Me.Delimiter)
                    End If

                    If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                    wr.Write(sc(i))
                    If Me.WrapTextWithQuote Then wr.Write(ControlChars.Quote)
                Next
                wr.WriteLine()
            Else
                'All parameters are in the Stub

                'Add header for the value column
                wr.Write(Me.Delimiter)
                wr.Write(_model.Meta.Contents)
                'We still need a new line
                wr.WriteLine()
            End If

        End Sub

        ''' <summary>
        ''' Creates the heading texts by finding all the possible combinations of the heading variables.
        ''' </summary>
        ''' <param name="headingIndex">The index of the heading variable</param>
        ''' <returns>A stringcollection representing all the concatenated heading texts for the given index</returns>
        ''' <remarks></remarks>
        Private Function ConcatHeadingValues(ByVal headingIndex As Integer) As StringCollection
            Dim sc As New StringCollection
            Dim sc2 As New StringCollection

            If headingIndex < _model.Meta.Heading.Count - 1 Then
                'Call recursivly to get the combinations
                sc2 = ConcatHeadingValues(headingIndex + 1)
                For valueIndex As Integer = 0 To _model.Meta.Heading(headingIndex).Values.Count - 1
                    For j As Integer = 0 To sc2.Count - 1
                        sc.Add(_model.Meta.Heading(headingIndex).Values(valueIndex).Text & " " & sc2(j))
                    Next
                Next
            Else
                For valueIndex As Integer = 0 To _model.Meta.Heading(headingIndex).Values.Count - 1
                    sc.Add(_model.Meta.Heading(headingIndex).Values(valueIndex).Text)
                Next
            End If

            Return sc
        End Function

        ''' <summary>
        ''' Writes the data to a stream
        ''' </summary>
        ''' <param name="wr">The stream to write to</param>
        ''' <remarks></remarks>
        Private Sub WriteTable(ByVal wr As System.IO.StreamWriter)
            'If _model.Meta.Stub.Count > 0 Then
            Dim sc As StringCollection
            Dim df As DataFormatter = New DataFormatter(_model)
            Dim value As String = ""

            df.DecimalSeparator = Me.DecimalSeparator.ToString
            df.ShowDataNotes = False
            If Not Me.ThousandSeparator Then
                df.ThousandSeparator = ""
            End If

            If _model.Meta.Stub.Count > 0 Then

                sc = ConcatStubValues(0)

                'There should be exactly as many items in the stringcollection as 
                'the number of rows in the data.
                If sc.Count <> _model.Data.MatrixRowCount Then
                    'TODO: Errorcode
                    Throw New PXSerializationException("Stubvalues does not match the data", "")
                End If

                For i As Integer = 0 To sc.Count - 1
                    wr.Write(sc(i))
                    For c As Integer = 0 To _model.Data.MatrixColumnCount - 1
                        value = df.ReadElement(i, c)
                        wr.Write(Me.Delimiter)
                        wr.Write(value)
                    Next
                    wr.WriteLine()
                Next
            ElseIf _model.Meta.Heading.Count > 0 Then
                For c As Integer = 0 To _model.Data.MatrixColumnCount - 1
                    value = df.ReadElement(0, c)
                    wr.Write(Me.Delimiter)
                    wr.Write(value)
                Next
            End If
            'End If
        End Sub

        ''' <summary>
        ''' Concatenates the stubvales 
        ''' </summary>
        ''' <param name="stubIndex">The index of the stub variable</param>
        ''' <returns>Strincollection with all the concatenated stubcalues for the given index</returns>
        ''' <remarks></remarks>
        Private Function ConcatStubValues(ByVal stubIndex As Integer) As StringCollection
            Dim sc As New StringCollection
            Dim sc2 As New StringCollection

            If stubIndex < _model.Meta.Stub.Count - 1 Then
                'Call recursivly to get the combinations
                sc2 = ConcatStubValues(stubIndex + 1)
                For valueIndex As Integer = 0 To _model.Meta.Stub(stubIndex).Values.Count - 1
                    For j As Integer = 0 To sc2.Count - 1
                        sc.Add(TableStub(stubIndex, valueIndex) & Me.Delimiter & sc2(j))
                    Next
                Next
            Else
                For valueIndex As Integer = 0 To _model.Meta.Stub(stubIndex).Values.Count - 1
                    sc.Add(TableStub(stubIndex, valueIndex))
                Next
            End If

            Return sc
        End Function

        ''' <summary>
        ''' Get the stub value and code
        ''' </summary>
        ''' <param name="stubIndex">Index of the stubvariable</param>
        ''' <param name="valueIndex">Index of the value</param>
        ''' <returns>
        ''' Returns the value. If the variable has code and doublecolumn is true both code
        ''' and value are returned separated by the delimiter.
        ''' </returns>
        ''' <remarks></remarks>
        Private Function TableStub(ByVal stubIndex As Integer, ByVal valueIndex As Integer) As String
            Dim sb As New System.Text.StringBuilder

            If Me.DoubleColumn Then
                If _model.Meta.Stub(stubIndex).DoubleColumn Then
                    If _model.Meta.Stub(stubIndex).Values(valueIndex).HasCode Then
                        If Me.WrapTextWithQuote Then sb.Append(ControlChars.Quote)
                        sb.Append(_model.Meta.Stub(stubIndex).Values(valueIndex).Code)
                        If Me.WrapTextWithQuote Then sb.Append(ControlChars.Quote)
                        sb.Append(Me.Delimiter)
                    End If
                End If
            End If
            If Me.WrapTextWithQuote Then sb.Append(ControlChars.Quote)
            sb.Append(_model.Meta.Stub(stubIndex).Values(valueIndex).Text)
            If Me.WrapTextWithQuote Then sb.Append(ControlChars.Quote)

            Return sb.ToString
        End Function

#Region "Public properties"
        Public Property Delimiter() As Char
            Get
                Return _delimiter
            End Get
            Set(ByVal value As Char)
                _delimiter = value
            End Set
        End Property

        Public Property DecimalSeparator() As Char
            Get
                Return _decimalSeparator
            End Get
            Set(ByVal value As Char)
                _decimalSeparator = value
            End Set
        End Property

        Public Property DoubleColumn() As Boolean
            Get
                Return _doubleColumn
            End Get
            Set(ByVal value As Boolean)
                _doubleColumn = value
            End Set
        End Property

        Public Property Title() As Boolean
            Get
                Return _title
            End Get
            Set(ByVal value As Boolean)
                _title = value
            End Set
        End Property

        Public Property ThousandSeparator() As Boolean
            Get
                Return _thosandSeparator
            End Get
            Set(ByVal value As Boolean)
                _thosandSeparator = value
            End Set
        End Property


        Public Property WrapTextWithQuote() As Boolean
            Get
                Return _wrapTextWithQuote
            End Get
            Set(ByVal value As Boolean)
                _wrapTextWithQuote = value
            End Set
        End Property

#End Region

    End Class

End Namespace
