Namespace PCAxis.Paxiom
    Public Class PXFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

        'The serializer allways produces a px-file of the 2010 axis-version
        Private Const AXIS_VERSION As String = "2010"

        Private _SerializeInKeysFormat As Boolean = False
        ''' <summary>
        ''' Controls if the model will be serialized in KEYS format or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerializeInKeysFormat() As Boolean
            Get
                Return _SerializeInKeysFormat
            End Get
            Set(ByVal value As Boolean)
                _SerializeInKeysFormat = value
            End Set
        End Property


        Public Sub Serialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize
            Using fs As System.IO.FileStream = New System.IO.FileStream(path, IO.FileMode.Create)
                Serialize(model, fs)
            End Using

        End Sub


        Public Sub Serialize(ByVal model As PXModel, ByVal stream As System.IO.Stream) Implements IPXModelStreamSerializer.Serialize
            Dim meta As PCAxis.Paxiom.PXMeta = model.Meta

            Dim var As PCAxis.Paxiom.Variable
            Dim val As PCAxis.Paxiom.Value
            Dim info As PCAxis.Paxiom.ContInfo = Nothing
            Dim writer As PXFileWriter = Nothing
            Dim lang As String = Nothing
            Dim selectedLanguageIndex As Integer

            'Get selected language index so we can restore it after serialization
            selectedLanguageIndex = model.Meta.CurrentLanguageIndex

            model.Meta.SetLanguage("default")

            Try
                writer = New PXFileWriter(stream, EncodingUtil.GetEncoding(model.Meta.CodePage))
                'CHARSET
                writer.PXWrite(PXKeywords.CHARSET, "ANSI", lang)
                'AXIS-VERSION
                If Not String.IsNullOrEmpty(AXIS_VERSION) Then
                    writer.PXWrite(PXKeywords.AXIS_VERSION, AXIS_VERSION, lang)
                End If
                'CODEPAGE
                If Not String.IsNullOrEmpty(writer.Encoding.WebName) Then
                    writer.PXWrite(PXKeywords.CODEPAGE, writer.Encoding.WebName, lang)
                End If
                'LANGUAGE
                If Not String.IsNullOrEmpty(meta.Language) Then
                    writer.PXWrite(PXKeywords.LANGUAGE, meta.Language, lang)
                End If
                'LANGUAGES
                'Saves all other languages
                Dim langs() As String = model.Meta.GetAllLanguages()
                If langs IsNot Nothing Then
                    writer.BeginPXLine(PXKeywords.LANGUAGES, lang)
                    writer.WriteValue(meta.Language)

                    For i As Integer = 0 To langs.Length - 1
                        If Not langs(i).Equals(meta.Language) Then
                            writer.Write(",")
                            writer.WriteValue(langs(i))
                        End If
                    Next

                    writer.EndPXLine()
                End If
                'CREATION-DATE
                If Not String.IsNullOrEmpty(meta.CreationDate) Then
                    writer.PXWrite(PXKeywords.CREATION_DATE, meta.CreationDate, lang)
                End If
                'DECIMALS
                writer.PXWrite(PXKeywords.DECIMALS, meta.Decimals, lang)
                'SHOWDECIMALS
                If meta.ShowDecimals <> -1 Then
                    writer.PXWrite(PXKeywords.SHOWDECIMALS, meta.ShowDecimals, lang)
                End If
                'ROUNDING
                If meta.Rounding <> RoundingType.None Then
                    writer.PXWrite(PXKeywords.ROUNDING, CType(meta.Rounding, Integer), lang)
                End If
                'MATRIX
                writer.PXWrite(PXKeywords.MATRIX, meta.Matrix, lang)
                'AGGREGALLOWED
                If Not meta.AggregAllowed Then
                    writer.PXWrite(PXKeywords.AGGREGALLOWED, meta.AggregAllowed, lang)
                End If
                'AUTOOPEN
                If meta.AutoOpen Then
                    writer.PXWrite(PXKeywords.AUTOPEN, meta.AutoOpen, lang)
                End If
                'CONFIDENTIAL
                If meta.Confidential <> 0 Then
                    writer.PXWrite(PXKeywords.CONFIDENTIAL, meta.Confidential, lang)
                End If
                'COPYRIGHT
                writer.PXWrite(PXKeywords.COPYRIGHT, meta.Copyright, lang)

                'SUBJECT-CODE
                writer.PXWrite(PXKeywords.SUBJECT_CODE, meta.SubjectCode, lang)
                '*SUBJECT-AREA
                writer.PXWrite(PXKeywords.SUBJECT_AREA, meta.SubjectArea, lang)
                '*DESCRIPTION
                If Not String.IsNullOrEmpty(meta.Description) Then
                    writer.PXWrite(PXKeywords.DESCRIPTION, meta.Description, lang)
                    'DESCRIPTIONDEFAULT 
                    'TODO Check with Louise that it is ok to change place between DecriptionDefault and Title
                    'If meta.DescriptionDefault Then
                    '    writer.PXWrite(PXKeywords.DESCRIPTIONDEFAULT, meta.DescriptionDefault, lang)
                    'End If
                End If
                '*TITLE
                If String.IsNullOrEmpty(meta.Title) Then
                    meta.CreateTitle()
                End If
                writer.PXWrite(PXKeywords.TITLE, meta.Title, lang)
                '*CONTENTS
                writer.PXWrite(PXKeywords.CONTENTS, meta.Contents, lang)
                ''*UNITS
                'writer.WriteLine(String.Format("{0}=""{1}"";", PXKeywords.UNITS, meta.Units))
                '*STUB
                If meta.Stub.Count > 0 Then


                    writer.BeginPXLine(PXKeywords.STUB, lang)

                    For i As Integer = 0 To meta.Stub.Count - 2
                        writer.WriteValue(GetVariableName(meta.Stub(i)))
                        writer.WriteNextValue(GetVariableName(meta.Stub(i + 1)).Length)
                    Next
                    writer.WriteValue(GetVariableName(meta.Stub(meta.Stub.Count - 1)))
                    writer.EndPXLine()
                End If


                '*HEADING
                If meta.Heading.Count > 0 Then
                    writer.BeginPXLine(PXKeywords.HEADING, lang)

                    For i As Integer = 0 To meta.Heading.Count - 2
                        writer.WriteValue(GetVariableName(meta.Heading(i)))
                        writer.WriteNextValue(GetVariableName(meta.Heading(i + 1)).Length)
                    Next
                    writer.WriteValue(GetVariableName(meta.Heading(meta.Heading.Count - 1)))
                    writer.EndPXLine()
                End If

                '*CONTENTVARIABLE
                If meta.ContentVariable IsNot Nothing Then
                    writer.PXWrite(PXKeywords.CONTVARIABLE, GetVariableName(meta.ContentVariable), lang)
                End If
                '*VALUES
                For j As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(j)
                    writer.BeginPXLine(PXKeywords.VALUES, GetVariableName(var), lang)

                    For i As Integer = 0 To var.Values.Count - 2
                        writer.WriteValue(var.Values(i).Value)
                        writer.WriteNextValue(var.Values(i + 1).Value.Length)
                    Next
                    writer.WriteValue(var.Values(var.Values.Count - 1).Value)
                    writer.EndPXLine()

                Next
                '*TIMEVAL
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.HasTimeValue Then
                        writer.BeginPXLine(PXKeywords.TIMEVAL, GetVariableName(var), lang)
                        writer.Write(var.TimeValue)
                        writer.EndPXLine()
                        Exit For
                    End If
                Next

                '*CODES
                For j As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(j)
                    If (Not var.Values.ValuesHaveCodes) Then
                        Continue For
                    End If

                    writer.BeginPXLine(PXKeywords.CODES, GetVariableName(var), lang)

                    For i As Integer = 0 To var.Values.Count - 2
                        writer.WriteValue(var.Values(i).Code)
                        writer.WriteNextValue(var.Values(i + 1).Code.Length)
                    Next
                    writer.WriteValue(var.Values(var.Values.Count - 1).Code)
                    writer.EndPXLine()
                Next

                '*HIERARCHYLEVELSOPEN
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Hierarchy Is Nothing Then
                        Continue For
                    End If
                    If Not var.Hierarchy.IsHierarchy Then
                        Continue For
                    End If

                    writer.BeginPXLine(PXKeywords.HIERARCHYLEVELSOPEN, GetVariableName(var), lang)
                    writer.Write(var.Hierarchy.OpenLevel.ToString)
                    writer.EndPXLine()
                Next

                '*HIERARCHYLEVELS
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Hierarchy Is Nothing Then
                        Continue For
                    End If
                    If Not var.Hierarchy.IsHierarchy Then
                        Continue For
                    End If

                    If var.Hierarchy.Levels <> -1 Then
                        writer.BeginPXLine(PXKeywords.HIERARCHYLEVELS, GetVariableName(var), lang)
                        writer.Write(var.Hierarchy.Levels.ToString)
                        writer.EndPXLine()
                    End If
                Next

                '*HIERARCHYNAMES
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Hierarchy Is Nothing Then
                        Continue For
                    End If
                    If Not var.Hierarchy.IsHierarchy Then
                        Continue For
                    End If

                    If var.Hierarchy.Names.Count > 0 Then
                        Dim first As Boolean = True
                        writer.BeginPXLine(PXKeywords.HIERARCHYNAMES, GetVariableName(var), lang)
                        For Each name As String In var.Hierarchy.Names
                            If first Then
                                writer.WriteValue(name)
                                first = False
                            Else
                                writer.Write(",")
                                writer.WriteValue(name)
                            End If
                        Next
                        writer.EndPXLine()
                    End If
                Next

                '*HIERARCHIES
                For j As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(j)
                    If var.Hierarchy Is Nothing Then
                        Continue For
                    End If
                    If Not var.Hierarchy.IsHierarchy Then
                        Continue For
                    End If

                    writer.BeginPXLine(PXKeywords.HIERARCHIES, GetVariableName(var), lang)
                    writer.WriteValue(var.Hierarchy.RootLevel.Code)
                    WriteHieararchiesChildren(writer, var.Hierarchy.RootLevel)
                    writer.EndPXLine()
                Next

                '*DOUBLECOLUMN
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.DoubleColumn Then
                        writer.PXWrite(PXKeywords.DOUBLECOLUMN, GetVariableName(var), var.DoubleColumn, lang)
                    End If
                Next
                '*PRESTEXT
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.PresentationText <> -1 And var.PresentationText <> 1 Then
                        writer.PXWrite(PXKeywords.PRESTEXT, GetVariableName(var), var.PresentationText, lang)
                    End If
                Next
                '*DOMAIN
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If Not String.IsNullOrEmpty(var.Domain) Then
                        writer.PXWrite(PXKeywords.DOMAIN, GetVariableName(var), var.Domain, lang)
                    End If
                Next
                '*MAP
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If Not String.IsNullOrEmpty(var.Map) Then
                        writer.PXWrite(PXKeywords.MAP, GetVariableName(var), var.Map, lang)
                    End If
                Next
                '*PARTITIONED
                Dim part As Partition
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    For j As Integer = 0 To var.Partitions.Count - 1
                        part = var.Partitions(j)
                        If part.Length = Integer.MaxValue Then
                            writer.PXWrite(PXKeywords.PARTITIONED, GetVariableName(var), """ & part.Name&""," & part.StartIndex, lang)
                        Else
                            writer.PXWrite(PXKeywords.PARTITIONED, GetVariableName(var), """ & part.Name&""," & part.StartIndex & "," & part.Length, lang)
                        End If
                    Next

                Next
                '*ELIMINATION
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Elimination Then
                        If var.EliminationValue Is Nothing Then
                            writer.PXWrite(PXKeywords.ELIMINATION, GetVariableName(var), True, lang)
                        Else
                            writer.PXWrite(PXKeywords.ELIMINATION, GetVariableName(var), var.EliminationValue.Value, lang)
                        End If
                    End If
                Next
                '*PRECISION
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    For j As Integer = 0 To var.Values.Count - 1
                        val = var.Values(j)
                        If val.Precision <> 0 Then
                            writer.PXWrite(PXKeywords.PRECISION, GetVariableName(var), val.Value, val.Precision, lang)
                        End If
                    Next
                Next
                If meta.ContentInfo IsNot Nothing Then
                    info = meta.ContentInfo
                    '*LAST-UPDATED
                    If Not String.IsNullOrEmpty(info.LastUpdated) Then
                        writer.PXWrite(PXKeywords.LAST_UPDATED, info.LastUpdated, lang)
                    End If
                    'STOCKFA
                    If Not String.IsNullOrEmpty(info.StockFa) Then
                        writer.PXWrite(PXKeywords.STOCKFA, info.StockFa, lang)
                    End If
                    '*CFPRICES
                    If Not String.IsNullOrEmpty(info.CFPrices) Then
                        writer.PXWrite(PXKeywords.CFPRICES, info.CFPrices, lang)
                    End If
                    '*DAYADJ
                    If Not String.IsNullOrEmpty(info.DayAdj) Then
                        writer.PXWrite(PXKeywords.DAYADJ, info.DayAdj, lang, False)
                    End If
                    '*SEASADJ
                    If Not String.IsNullOrEmpty(info.SeasAdj) Then
                        writer.PXWrite(PXKeywords.SEASADJ, info.SeasAdj, lang, False)
                    End If
                    'UNITS
                    'TODO Ask Louise about the correct placment of Units
                    If Not String.IsNullOrEmpty(info.Units) Then
                        writer.PXWrite(PXKeywords.UNITS, info.Units, lang)
                    Else
                        writer.PXWrite(PXKeywords.UNITS, "", lang)
                    End If
                    '*REFPERIOD
                    If Not String.IsNullOrEmpty(info.RefPeriod) Then
                        writer.PXWrite(PXKeywords.REFPERIOD, info.RefPeriod, lang)
                    End If
                Else
                    writer.PXWrite(PXKeywords.UNITS, "", lang)
                End If

                If info IsNot Nothing Then
                    '*CONTACT
                    If Not String.IsNullOrEmpty(info.Contact) Then
                        writer.PXWrite(PXKeywords.CONTACT, info.Contact, lang)
                    End If
                    '*BASEPERIOD
                    If Not String.IsNullOrEmpty(info.Baseperiod) Then
                        writer.PXWrite(PXKeywords.BASEPERIOD, info.Baseperiod, lang)
                    End If

                End If

                If meta.ContentVariable IsNot Nothing Then
                    var = meta.ContentVariable
                    For i As Integer = 0 To var.Values.Count - 1
                        info = var.Values(i).ContentInfo
                        Dim value As String = var.Values(i).Value

                        If info IsNot Nothing Then
                            'LAST-UPDATED
                            If Not String.IsNullOrEmpty(info.LastUpdated) Then
                                writer.PXWrite(PXKeywords.LAST_UPDATED, value, info.LastUpdated, lang)
                            End If
                            '*STOCKFA
                            If Not String.IsNullOrEmpty(info.StockFa) Then
                                writer.PXWrite(PXKeywords.STOCKFA, value, info.StockFa, lang)
                            End If
                            'CFPRICES
                            If Not String.IsNullOrEmpty(info.CFPrices) Then
                                writer.PXWrite(PXKeywords.CFPRICES, value, info.CFPrices, lang)
                            End If
                            'DAYADJ
                            If Not String.IsNullOrEmpty(info.DayAdj) Then
                                writer.PXWrite(PXKeywords.DAYADJ, value, info.DayAdj, lang, False)
                            End If
                            'SEASADJ
                            If Not String.IsNullOrEmpty(info.SeasAdj) Then
                                writer.PXWrite(PXKeywords.SEASADJ, value, info.SeasAdj, lang, False)
                            End If
                            'REFPERIOD
                            If Not String.IsNullOrEmpty(info.RefPeriod) Then
                                writer.PXWrite(PXKeywords.REFPERIOD, value, info.RefPeriod, lang)
                            End If
                            'UNITS
                            'TODO Ask Louise about the correct placment of Units
                            If Not String.IsNullOrEmpty(info.Units) Then
                                writer.PXWrite(PXKeywords.UNITS, value, info.Units, lang)
                            Else
                                writer.PXWrite(PXKeywords.UNITS, value, "?", lang)
                            End If
                            'CONTACT
                            If Not String.IsNullOrEmpty(info.Contact) Then
                                writer.PXWrite(PXKeywords.CONTACT, value, info.Contact, lang)
                            End If
                            'BASEPERIOD
                            If Not String.IsNullOrEmpty(info.Baseperiod) Then
                                writer.PXWrite(PXKeywords.BASEPERIOD, value, info.Baseperiod, lang)
                            End If

                        End If
                    Next
                End If
                '*FIRST-PUBLISHED
                If Not String.IsNullOrEmpty(meta.FirstPublished) Then
                    writer.PXWrite(PXKeywords.FIRST_PUBLISHED, meta.FirstPublished, lang)
                End If
                '*DATABASE
                If Not String.IsNullOrEmpty(meta.Database) Then
                    writer.PXWrite(PXKeywords.DATABASE, meta.Database, lang)
                End If
                '*SOURCE
                If Not String.IsNullOrEmpty(meta.Source) Then
                    writer.PXWrite(PXKeywords.SOURCE, meta.Source, lang)
                End If
                '*INFOFILE
                If Not String.IsNullOrEmpty(meta.InfoFile) Then
                    writer.PXWrite(PXKeywords.INFOFILE, meta.InfoFile, lang)
                End If
                '*NOTEX
                Dim note As String
                note = meta.Notes.GetMandatoryNotesString("##")
                If Not String.IsNullOrEmpty(note) Then
                    writer.PXWrite(PXKeywords.NOTEX, note, lang)
                End If
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Notes Is Nothing Then
                        Continue For
                    End If
                    note = var.Notes.GetMandatoryNotesString("##")
                    If Not String.IsNullOrEmpty(note) Then
                        writer.PXWrite(PXKeywords.NOTEX, GetVariableName(var), note, lang)
                    End If
                Next
                'For i As Integer = 0 To meta.Notes.Count - 1
                '    If meta.Notes(i).Mandantory Then
                '        writer.PXWrite(PXKeywords.NOTEX, meta.Notes(i).Text, lang)
                '    End If
                'Next
                'For i As Integer = 0 To meta.Variables.Count - 1
                '    var = meta.Variables(i)
                '    If var.Notes Is Nothing Then
                '        Continue For
                '    End If
                '    For j As Integer = 0 To var.Notes.Count - 1
                '        If var.Notes(j).Mandantory Then
                '            writer.PXWrite(PXKeywords.NOTEX, GetVariableName(var), var.Notes(j).Text, lang)
                '        End If
                '    Next
                'Next

                '*NOTE
                note = meta.Notes.GetNonMandatoryNotesString("##")
                If Not String.IsNullOrEmpty(note) Then
                    writer.PXWrite(PXKeywords.NOTE, note, lang)
                End If
                
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    If var.Notes Is Nothing Then
                        Continue For
                    End If
                    note = var.Notes.GetNonMandatoryNotesString("##")
                    If Not String.IsNullOrEmpty(note) Then
                        writer.PXWrite(PXKeywords.NOTE, GetVariableName(var), note, lang)
                    End If
                Next
                'For i As Integer = 0 To meta.Notes.Count - 1
                '    If Not meta.Notes(i).Mandantory Then
                '        writer.PXWrite(PXKeywords.NOTE, meta.Notes(i).Text, lang)
                '    End If
                'Next
                'For i As Integer = 0 To meta.Variables.Count - 1
                '    var = meta.Variables(i)
                '    If var.Notes Is Nothing Then
                '        Continue For
                '    End If
                '    For j As Integer = 0 To var.Notes.Count - 1
                '        If Not var.Notes(j).Mandantory Then
                '            writer.PXWrite(PXKeywords.NOTE, GetVariableName(var), var.Notes(j).Text, lang)
                '        End If
                '    Next
                'Next
                '*VALUENOTEX
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    For k As Integer = 0 To var.Values.Count - 1
                        val = var.Values(k)
                        If val.Notes Is Nothing Then
                            Continue For
                        End If
                        note = val.Notes.GetMandatoryNotesString("##")
                        If Not String.IsNullOrEmpty(note) Then
                            writer.PXWrite(PXKeywords.VALUENOTEX, GetVariableName(var), val.Value, note, lang)
                        End If
                    Next
                Next
                'For i As Integer = 0 To meta.Variables.Count - 1
                '    var = meta.Variables(i)
                '    For k As Integer = 0 To var.Values.Count - 1
                '        val = var.Values(k)
                '        If val.Notes Is Nothing Then
                '            Continue For
                '        End If
                '        For j As Integer = 0 To val.Notes.Count - 1
                '            If val.Notes(j).Mandantory Then
                '                writer.PXWrite(PXKeywords.VALUENOTEX, GetVariableName(var), val.Value, val.Notes(j).Text, lang)
                '            End If
                '        Next
                '    Next
                'Next
                '*VALUENOTE
                For i As Integer = 0 To meta.Variables.Count - 1
                    var = meta.Variables(i)
                    For k As Integer = 0 To var.Values.Count - 1
                        val = var.Values(k)
                        If val.Notes Is Nothing Then
                            Continue For
                        End If
                        note = val.Notes.GetNonMandatoryNotesString("##")
                        If Not String.IsNullOrEmpty(note) Then
                            writer.PXWrite(PXKeywords.VALUENOTE, GetVariableName(var), val.Value, note, lang)
                        End If
                    Next
                Next
                'For i As Integer = 0 To meta.Variables.Count - 1
                '    var = meta.Variables(i)
                '    For k As Integer = 0 To var.Values.Count - 1
                '        val = var.Values(k)
                '        If val.Notes Is Nothing Then
                '            Continue For
                '        End If
                '        For j As Integer = 0 To val.Notes.Count - 1
                '            If Not val.Notes(j).Mandantory Then
                '                writer.PXWrite(PXKeywords.VALUENOTE, GetVariableName(var), val.Value, val.Notes(j).Text, lang)
                '            End If
                '        Next
                '    Next
                'Next
                '*META-ID
                If Not String.IsNullOrEmpty(meta.MetaId) Then
                    writer.PXWrite(PXKeywords.META_ID, meta.MetaId, lang)
                End If
                For Each var In meta.Variables
                    If Not String.IsNullOrEmpty(var.MetaId) Then
                        writer.PXWrite(PXKeywords.META_ID, var.Name, var.MetaId, lang)
                    End If
                Next
                For Each var In meta.Variables
                    For Each val In var.Values
                        If Not String.IsNullOrEmpty(val.MetaId) Then
                            writer.PXWrite(PXKeywords.META_ID, var.Name, val.Value, val.MetaId, lang)
                        End If
                    Next
                Next
                '*DATANOTE
                If Not String.IsNullOrEmpty(meta.Datanote) Then
                    writer.PXWrite(PXKeywords.DATANOTE, meta.Datanote, lang)
                End If
                For Each var In meta.Variables
                    If Not String.IsNullOrEmpty(var.Datanote) Then
                        writer.PXWrite(PXKeywords.DATANOTE, var.Name, var.Datanote, lang)
                    End If
                Next
                For Each var In meta.Variables
                    For Each val In var.Values
                        If Not String.IsNullOrEmpty(val.Datanote) Then
                            writer.PXWrite(PXKeywords.DATANOTE, var.Name, val.Value, val.Datanote, lang)
                        End If
                    Next
                Next
                '*CELLNOTEEX
                SerializeCellNotes(meta, True, Nothing, writer)
                '*CELLNOTE
                SerializeCellNotes(meta, False, Nothing, writer)
                '*DATASYMBOL1
                If Not String.IsNullOrEmpty(meta.DataSymbol1) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL1, meta.DataSymbol1, lang)
                End If
                '*DATASYMBOL2
                If Not String.IsNullOrEmpty(meta.DataSymbol2) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL2, meta.DataSymbol2, lang)
                End If
                '*DATASYMBOL3
                If Not String.IsNullOrEmpty(meta.DataSymbol3) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL3, meta.DataSymbol3, lang)
                End If
                '*DATASYMBOL4
                If Not String.IsNullOrEmpty(meta.DataSymbol4) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL4, meta.DataSymbol4, lang)
                End If
                '*DATASYMBOL5
                If Not String.IsNullOrEmpty(meta.DataSymbol5) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL5, meta.DataSymbol5, lang)
                End If
                '*DATASYMBOL6
                If Not String.IsNullOrEmpty(meta.DataSymbol6) Then
                    writer.PXWrite(PXKeywords.DATASYMBOL6, meta.DataSymbol6, lang)
                End If
                ''*DATASYMBOL7
                'If Not String.IsNullOrEmpty(meta.DataSymbol7) Then
                '    writer.PXWrite(PXKeywords.DATASYMBOL7, meta.DataSymbol7, lang)
                'End If
                '*DATASYMBOLSUM
                If Not String.IsNullOrEmpty(meta.DataSymbolSum) Then
                    writer.PXWrite(PXKeywords.DATASYMBOLSUM, meta.DataSymbolSum, lang)
                End If
                '*DATASYMBOLNIL
                If Not String.IsNullOrEmpty(meta.DataSymbolNIL) Then
                    writer.PXWrite(PXKeywords.DATASYMBOLNIL, meta.DataSymbolNIL, lang)
                End If
                '*DATANOTECELL
                SerializeDataNoteCell(meta, Nothing, writer)
                '*DATANOTESUM
                If Not String.IsNullOrEmpty(meta.DataNoteSum) Then
                    writer.PXWrite(PXKeywords.DATANOTESUM, meta.DataNoteSum, lang)
                End If
                'DEFAULT-GRAPH
                If meta.DefaultGraph <> Integer.MinValue Then
                    writer.PXWrite(PXKeywords.DEFAULT_GRAPH, meta.DefaultGraph, lang)
                End If
                'DIRECTORY-PATH
                If Not String.IsNullOrEmpty(meta.DirectoryPath) Then
                    writer.PXWrite(PXKeywords.DIRECTORY_PATH, meta.DirectoryPath, lang)
                End If
                '*INFO
                If Not String.IsNullOrEmpty(meta.Information) Then
                    writer.PXWrite(PXKeywords.INFO, meta.Information, lang)
                End If
                '*LINK
                If Not String.IsNullOrEmpty(meta.Link) Then
                    writer.PXWrite(PXKeywords.LINK, meta.Link, lang)
                End If
                'NEXT-UPDATE
                If Not String.IsNullOrEmpty(meta.NextUpdate) Then
                    writer.PXWrite(PXKeywords.NEXT_UPDATE, meta.NextUpdate, lang)
                End If
                'PX-SERVER
                If Not String.IsNullOrEmpty(meta.PXServer) Then
                    writer.PXWrite(PXKeywords.PX_SERVER, meta.PXServer, lang)
                End If
                '*SURVEY
                If Not String.IsNullOrEmpty(meta.Survey) Then
                    writer.PXWrite(PXKeywords.SURVEY, meta.Survey, lang)
                End If
                'TABLEID
                If Not String.IsNullOrEmpty(meta.TableID) Then
                    writer.PXWrite(PXKeywords.TABLEID, meta.TableID, lang)
                End If
                'UPDATE_FREQUENCY
                If Not String.IsNullOrEmpty(meta.UpdateFrequency) Then
                    writer.PXWrite(PXKeywords.UPDATE_FREQUENCY, meta.UpdateFrequency, lang)
                End If
                '*VARIABLE-TYPE
                For i As Integer = 0 To meta.Variables.Count - 1
                    Dim type As String
                    type = meta.Variables(i).VariableType
                    If Not String.IsNullOrEmpty(type) Then
                        'writer.PXWrite(PXKeywords.VARIABLE_TYPE, GetVariableName(meta.Variables(i)), meta.UpdateFrequency, lang)
                        writer.PXWrite(PXKeywords.VARIABLE_TYPE, GetVariableName(meta.Variables(i)), type, lang)
                    End If
                Next

                'ATTRIBUTE-ID
                If meta.Attributes.Identities.Count > 0 Then
                    writer.BeginPXLine(PXKeywords.ATTRIBUTE_ID, lang)

                    For i As Integer = 0 To meta.Attributes.Identities.Count - 2
                        writer.WriteValue(meta.Attributes.Identities(i))
                        writer.WriteNextValue(meta.Attributes.Identities(i + 1).Length)
                    Next
                    writer.WriteValue(meta.Attributes.Identities(meta.Attributes.Identities.Count - 1))
                    writer.EndPXLine()
                End If

                'ATTRIBUTE-TEXT
                If Not meta.Attributes.Names Is Nothing Then
                    If meta.Attributes.Names.Count > 0 Then
                        writer.BeginPXLine(PXKeywords.ATTRIBUTE_TEXT, lang)

                        For i As Integer = 0 To meta.Attributes.Names.Count - 2
                            writer.WriteValue(meta.Attributes.Names(i))
                            writer.WriteNextValue(meta.Attributes.Names(i + 1).Length)
                        Next
                        writer.WriteValue(meta.Attributes.Names(meta.Attributes.Names.Count - 1))
                        writer.EndPXLine()
                    End If
                End If

                'ATTRIBUTES

                'Default attributes
                Dim defLst As List(Of KeyValuePair(Of String, String)) = meta.Attributes.GetDefaultAttributes()
                If defLst.Count > 0 Then
                    writer.BeginPXLine(PXKeywords.ATTRIBUTES, lang)
                    For i As Integer = 0 To defLst.Count - 2
                        writer.WriteValue(defLst(i).Value)
                        writer.WriteNextValue(defLst(i + 1).Value.Length)
                    Next
                    writer.WriteValue(defLst(defLst.Count - 1).Value)
                    writer.EndPXLine()
                End If

                'Cell attributes
                SerializeCellAttributes(meta, writer)

                If langs IsNot Nothing Then
                    For langIndex As Integer = 0 To langs.Length - 1
                        If Not langs(langIndex).Equals(meta.Language) Then
                            SerializeLanguage(model, langs(langIndex), writer)
                        End If
                    Next
                End If

                '*DESCRIPTION
                If Not String.IsNullOrEmpty(meta.Description) Then
                    'DESCRIPTIONDEFAULT 
                    If meta.DescriptionDefault Then
                        writer.PXWrite(PXKeywords.DESCRIPTIONDEFAULT, meta.DescriptionDefault, lang)
                    End If
                End If

                'DATA
                Dim nfi As New System.Globalization.NumberFormatInfo
                nfi.NumberDecimalSeparator = "."
                nfi.NumberDecimalDigits = Math.Max(meta.Decimals, 0)
                nfi.NumberGroupSeparator = ""

                Dim variablesInKeysFormat As List(Of Variable) = GetVariablesInKeysFormat(model)
                'Dim headVariablesInKeysFormat As New List(Of Variable)
                Dim stubVariablesInKeysFormat As New List(Of Variable)

                If variablesInKeysFormat.Count > 0 Then
                    ' Write the KEYS info
                    For Each checkVar As Variable In variablesInKeysFormat
                        ' Only stub variables is to be handled in keys format
                        If checkVar.Placement = PlacementType.Stub Then
                            If checkVar.Keys = KeysTypes.Code Then
                                writer.WriteLine(PXKeywords.KEYS + "(""" + checkVar.Code + """)=CODES;")
                            ElseIf checkVar.Keys = KeysTypes.Value Then
                                writer.WriteLine(PXKeywords.KEYS + "(""" + checkVar.Name + """)=VALUES;")
                            End If

                            'If checkVar.Placement = PlacementType.Heading Then
                            '    headVariablesInKeysFormat.Add(checkVar)
                            'Else
                            stubVariablesInKeysFormat.Add(checkVar)
                            'End If
                        End If
                    Next
                End If

                writer.WriteLine("DATA=")
                'Dim data As PCAxis.Paxiom.PXData
                'data = model.Data

                Dim dfmt As New DataFormatter(model)

                dfmt.DataSymbolNIL = PXConstant.DATASYMBOL_NIL_STRING
                dfmt.DataSymbols(1) = PXConstant.DATASYMBOL_1_STRING
                dfmt.DataSymbols(2) = PXConstant.DATASYMBOL_2_STRING
                dfmt.DataSymbols(3) = PXConstant.DATASYMBOL_3_STRING
                dfmt.DataSymbols(4) = PXConstant.DATASYMBOL_4_STRING
                dfmt.DataSymbols(5) = PXConstant.DATASYMBOL_5_STRING
                dfmt.DataSymbols(6) = PXConstant.DATASYMBOL_6_STRING
                dfmt.DataSymbols(7) = PXConstant.DATASYMBOL_7_STRING

                dfmt.DecimalSeparator = "."
                dfmt.ThousandSeparator = ""
                dfmt.DataNotePlacment = DataNotePlacementType.None
                dfmt.DecimalPrecision = GetNumberOfDecimals(meta)
                dfmt.ZeroOption = ZeroOptionType.NoZero


                Dim theValue As Double
                Dim protectedValuesIndex As Integer
                If variablesInKeysFormat.Count > 0 Then
                    ' Keys format
                    ' loopa och plocka ut de värden som är markerade som keys
                    Dim matrixHelper As Operations.PMatrixHelper = New Operations.PMatrixHelper(model)

                    For row As Integer = 0 To model.Data.MatrixRowCount - 1
                        matrixHelper.CalcStubWeights(row)

                        ' Only rows with data shall be written
                        If dfmt.IsZeroRow(row) = False Then
                            For col As Integer = 0 To model.Data.MatrixColumnCount - 1
                                matrixHelper.CalcHeadingWeights(col)

                                theValue = model.Data.ReadElement(row, col)
                                ' write the stub variable(s) code or value
                                If stubVariablesInKeysFormat.Count > 0 And col = 0 Then
                                    For i As Integer = 0 To matrixHelper.StubVariablesValueIndex.Length - 1
                                        If stubVariablesInKeysFormat(i).Keys = KeysTypes.Code Then
                                            writer.Write("""" + stubVariablesInKeysFormat(i).Values(matrixHelper.StubVariablesValueIndex(i)).Code + """")
                                        Else
                                            writer.Write("""" + stubVariablesInKeysFormat(i).Values(matrixHelper.StubVariablesValueIndex(i)).Value + """")
                                        End If

                                        If i < matrixHelper.StubVariablesValueIndex.Length - 1 Then
                                            writer.Write(", ")
                                        End If
                                    Next

                                    writer.Write(",")
                                End If
                                ' write the formatted data value
                                protectedValuesIndex = Array.IndexOf(PXConstant.ProtectedValues, theValue)
                                If protectedValuesIndex >= 0 Then
                                    ' Its a protected value - the displayvalue needs to be wrapped with "
                                    writer.Write(" """ + dfmt.ReadElement(row, col) + """")
                                Else
                                    writer.Write(" " + dfmt.ReadElement(row, col))
                                End If
                            Next
                            writer.WriteLine("")
                        End If
                    Next
                Else
                    ' Std px file format
                    For rowIndex As Integer = 0 To model.Data.MatrixRowCount - 1
                        For columnIndex As Integer = 0 To model.Data.MatrixColumnCount - 1
                            theValue = model.Data.ReadElement(rowIndex, columnIndex)
                            protectedValuesIndex = Array.IndexOf(PXConstant.ProtectedValues, theValue)
                            'writer.Write(dfmt.ReadElement(rowIndex, columnIndex))
                            If protectedValuesIndex >= 0 Then
                                ' Its a protected value - the displayvalue needs to be wrapped with "
                                writer.Write("""" + dfmt.ReadElement(rowIndex, columnIndex) + """")
                            Else
                                writer.Write(dfmt.ReadElement(rowIndex, columnIndex))
                            End If

                            'If columnIndex <> (model.Data.MatrixColumnCount - 1) Then
                            writer.Write(" ")
                            'End If
                        Next
                        writer.WriteLine("")
                    Next
                End If

                writer.Write(";")
            Finally
                If writer IsNot Nothing Then
                    writer.Flush()
                End If

                'Restore selected language
                model.Meta.SetLanguage(selectedLanguageIndex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the children of the given hiearchy-level to the file
        ''' </summary>
        ''' <param name="writer">Writer-object</param>
        ''' <param name="level">Level to write children for</param>
        ''' <remarks>WriteHieararchiesChildren is called recursively for each of the children of the given level</remarks>
        Private Sub WriteHieararchiesChildren(ByVal writer As PXFileWriter, ByVal level As PCAxis.Paxiom.HierarchyLevel)
            If level.Children.Count > 0 Then
                For Each child As HierarchyLevel In level.Children
                    writer.Write(",")
                    writer.WriteValue(level.Code)
                    writer.Write(":")
                    writer.WriteValue(child.Code)
                    WriteHieararchiesChildren(writer, child)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Check the Stub for Variables in Keys format
        ''' </summary>
        ''' <param name="model"></param>
        ''' <returns>List of variables that are in Keys format</returns>
        ''' <remarks>Only Stub variables are checked</remarks>
        Private Function GetVariablesInKeysFormat(ByVal model As PXModel) As List(Of Variable)
            Dim variables As New List(Of Variable)

            If Me.SerializeInKeysFormat Then
                For Each v As Variable In model.Meta.Stub ' should only be present in stub
                    If v.Values.IsCodesFictional Then
                        v.Keys = KeysTypes.Value
                    Else
                        ' Only set the type if it is not allready set
                        If v.Keys = KeysTypes.None Then v.Keys = KeysTypes.Code
                    End If
                    'If v.Keys <> KeysTypes.None Then variables.Add(v)
                    variables.Add(v)
                Next
            End If
            Return variables
        End Function


        Private Sub SerializeLanguage(ByVal model As PCAxis.Paxiom.PXModel, ByVal lang As String, ByVal writer As PXFileWriter)
            Dim meta As PCAxis.Paxiom.PXMeta = model.Meta

            Dim var As PCAxis.Paxiom.Variable
            Dim val As PCAxis.Paxiom.Value
            Dim info As PCAxis.Paxiom.ContInfo = Nothing
            'Sets the language to the specific language
            model.Meta.SetLanguage(lang)

            '*SUBJECT-AREA
            writer.PXWrite(PXKeywords.SUBJECT_AREA, meta.SubjectArea, lang)
            '*DESCRIPTION
            If Not String.IsNullOrEmpty(meta.Description) Then
                writer.PXWrite(PXKeywords.DESCRIPTION, meta.Description, lang)
            End If
            '*TITLE
            writer.PXWrite(PXKeywords.TITLE, meta.Title, lang)
            '*CONTENTS
            writer.PXWrite(PXKeywords.CONTENTS, meta.Contents, lang)
            ''*UNITS
            'writer.WriteLine(String.Format("{0}[{2}]=""{1}"";", PXKeywords.UNITS, meta.Units, lang))
            '*STUB
            If meta.Stub.Count > 0 Then
                writer.BeginPXLine(PXKeywords.STUB, lang)

                For i As Integer = 0 To meta.Stub.Count - 2
                    writer.WriteValue(GetVariableName(meta.Stub(i)))
                    writer.WriteNextValue(GetVariableName(meta.Stub(i + 1)).Length)
                Next
                writer.WriteValue(GetVariableName(meta.Stub(meta.Stub.Count - 1)))
                writer.EndPXLine()
            End If

            '*HEADING
            If meta.Heading.Count > 0 Then
                writer.BeginPXLine(PXKeywords.HEADING, lang)

                For i As Integer = 0 To meta.Heading.Count - 2
                    writer.WriteValue(GetVariableName(meta.Heading(i)))
                    writer.WriteNextValue(GetVariableName(meta.Heading(i + 1)).Length)
                Next
                writer.WriteValue(GetVariableName(meta.Heading(meta.Heading.Count - 1)))
                writer.EndPXLine()
            End If

            '*CONTENTVARIABLE
            If meta.ContentVariable IsNot Nothing Then
                writer.PXWrite(PXKeywords.CONTVARIABLE, GetVariableName(meta.ContentVariable), lang)
            End If
            '*VALUES
            For j As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(j)
                writer.BeginPXLine(PXKeywords.VALUES, GetVariableName(var), lang)

                For i As Integer = 0 To var.Values.Count - 2
                    writer.WriteValue(var.Values(i).Value)
                    writer.WriteNextValue(var.Values(i + 1).Value.Length)
                Next
                writer.WriteValue(var.Values(var.Values.Count - 1).Value)
                writer.EndPXLine()

            Next

            '*TIMEVAL
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.HasTimeValue Then
                    writer.BeginPXLine(PXKeywords.TIMEVAL, GetVariableName(var), lang)
                    writer.Write(var.TimeValue)
                    writer.EndPXLine()
                    Exit For
                End If
            Next
            '*CODES
            For j As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(j)
                If (Not var.Values.ValuesHaveCodes) Or var.IsTime Then
                    Continue For
                End If

                writer.BeginPXLine(PXKeywords.CODES, GetVariableName(var), lang)

                For i As Integer = 0 To var.Values.Count - 2
                    writer.WriteValue(var.Values(i).Code)
                    writer.WriteNextValue(var.Values(i).Code.Length)
                Next
                writer.WriteValue(var.Values(var.Values.Count - 1).Code)
                writer.EndPXLine()
            Next

            '*HIERARCHYLEVELSOPEN
            For j As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(j)
                If var.Hierarchy Is Nothing Then
                    Continue For
                End If
                If Not var.Hierarchy.IsHierarchy Then
                    Continue For
                End If

                writer.BeginPXLine(PXKeywords.HIERARCHYLEVELSOPEN, GetVariableName(var), lang)
                writer.Write(var.Hierarchy.OpenLevel.ToString)
                writer.EndPXLine()
            Next

            '*HIERARCHYLEVELS
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.Hierarchy Is Nothing Then
                    Continue For
                End If
                If Not var.Hierarchy.IsHierarchy Then
                    Continue For
                End If

                If var.Hierarchy.Levels <> -1 Then
                    writer.BeginPXLine(PXKeywords.HIERARCHYLEVELS, GetVariableName(var), lang)
                    writer.Write(var.Hierarchy.Levels.ToString)
                    writer.EndPXLine()
                End If
            Next

            '*HIERARCHYNAMES
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.Hierarchy Is Nothing Then
                    Continue For
                End If
                If Not var.Hierarchy.IsHierarchy Then
                    Continue For
                End If

                If var.Hierarchy.Names.Count > 0 Then
                    Dim first As Boolean = True
                    writer.BeginPXLine(PXKeywords.HIERARCHYNAMES, GetVariableName(var), lang)
                    For Each name As String In var.Hierarchy.Names
                        If first Then
                            writer.WriteValue(name)
                            first = False
                        Else
                            writer.Write(",")
                            writer.WriteValue(name)
                        End If
                    Next
                    writer.EndPXLine()
                End If
            Next

            '*HIERARCHIES
            For j As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(j)
                If var.Hierarchy Is Nothing Then
                    Continue For
                End If
                If Not var.Hierarchy.IsHierarchy Then
                    Continue For
                End If

                writer.BeginPXLine(PXKeywords.HIERARCHIES, GetVariableName(var), lang)
                writer.WriteValue(var.Hierarchy.RootLevel.Code)
                WriteHieararchiesChildren(writer, var.Hierarchy.RootLevel)
                writer.EndPXLine()
            Next

            '*DOUBLECOLUMN
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.DoubleColumn Then
                    writer.PXWrite(PXKeywords.DOUBLECOLUMN, GetVariableName(var), var.DoubleColumn, lang)
                End If
            Next
            '*PRESTEXT
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.PresentationText <> -1 And var.PresentationText <> 1 Then
                    writer.PXWrite(PXKeywords.PRESTEXT, GetVariableName(var), var.PresentationText, lang)
                End If
            Next
            '*DOMAIN
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If Not String.IsNullOrEmpty(var.Domain) Then
                    writer.PXWrite(PXKeywords.DOMAIN, GetVariableName(var), var.Domain, lang)
                End If
            Next
            '*MAP
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If Not String.IsNullOrEmpty(var.Map) Then
                    writer.PXWrite(PXKeywords.MAP, GetVariableName(var), var.Map, lang)
                End If
            Next
            '*PARTITIONED
            Dim part As Partition
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                For j As Integer = 0 To var.Partitions.Count - 1
                    part = var.Partitions(j)
                    If part.Length = Integer.MaxValue Then
                        writer.PXWrite(PXKeywords.PARTITIONED, GetVariableName(var), """ & part.Name&""," & part.StartIndex, lang)
                    Else
                        writer.PXWrite(PXKeywords.PARTITIONED, GetVariableName(var), """ & part.Name&""," & part.StartIndex & "," & part.Length, lang)
                    End If
                Next

            Next
            '*ELIMINATION
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.Elimination Then
                    If var.EliminationValue Is Nothing Then
                        writer.PXWrite(PXKeywords.ELIMINATION, GetVariableName(var), True, lang)
                    Else
                        writer.PXWrite(PXKeywords.ELIMINATION, GetVariableName(var), var.EliminationValue.Value, lang)
                    End If
                End If
            Next
            '*PRECISION
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    val = var.Values(j)
                    If val.Precision <> 0 Then
                        writer.PXWrite(PXKeywords.PRECISION, GetVariableName(var), val.Value, val.Precision, lang)
                    End If
                Next
            Next
            If meta.ContentInfo IsNot Nothing Then
                info = meta.ContentInfo
                '*LAST-UPDATED
                If Not String.IsNullOrEmpty(info.LastUpdated) Then
                    writer.PXWrite(PXKeywords.LAST_UPDATED, info.LastUpdated, lang)
                End If
                'STOCKFA
                If Not String.IsNullOrEmpty(info.StockFa) Then
                    writer.PXWrite(PXKeywords.STOCKFA, info.StockFa, lang)
                End If
                '*CFPRICES
                If Not String.IsNullOrEmpty(info.CFPrices) Then
                    writer.PXWrite(PXKeywords.CFPRICES, info.CFPrices, lang)
                End If
                '*DAYADJ
                If Not String.IsNullOrEmpty(info.DayAdj) Then
                    writer.PXWrite(PXKeywords.DAYADJ, info.DayAdj, lang, False)
                End If
                '*SEASADJ
                If Not String.IsNullOrEmpty(info.SeasAdj) Then
                    writer.PXWrite(PXKeywords.SEASADJ, info.SeasAdj, lang, False)
                End If
                '*UNITS
                'TODO Ask Louise about the correct placment of Units
                If Not String.IsNullOrEmpty(info.Units) Then
                    writer.PXWrite(PXKeywords.UNITS, info.Units, lang)
                Else
                    writer.PXWrite(PXKeywords.UNITS, "?", lang)
                End If
                '*REFPERIOD
                If Not String.IsNullOrEmpty(info.RefPeriod) Then
                    writer.PXWrite(PXKeywords.REFPERIOD, info.RefPeriod, lang)
                End If
            Else
                writer.PXWrite(PXKeywords.UNITS, "?", lang)
            End If

            If info IsNot Nothing Then
                '*CONTACT
                If Not String.IsNullOrEmpty(info.Contact) Then
                    writer.PXWrite(PXKeywords.CONTACT, info.Contact, lang)
                End If
                '*BASEPERIOD
                If Not String.IsNullOrEmpty(info.Baseperiod) Then
                    writer.PXWrite(PXKeywords.BASEPERIOD, info.Baseperiod, lang)
                End If

            End If

            If meta.ContentVariable IsNot Nothing Then
                var = meta.ContentVariable
                For i As Integer = 0 To var.Values.Count - 1
                    info = var.Values(i).ContentInfo
                    Dim value As String = var.Values(i).Value

                    If info IsNot Nothing Then
                        '*LAST-UPDATED
                        If Not String.IsNullOrEmpty(info.LastUpdated) Then
                            writer.PXWrite(PXKeywords.LAST_UPDATED, value, info.LastUpdated, lang)
                        End If
                        '*STOCKFA
                        If Not String.IsNullOrEmpty(info.StockFa) Then
                            writer.PXWrite(PXKeywords.STOCKFA, value, info.StockFa, lang)
                        End If
                        '*CFPRICES
                        If Not String.IsNullOrEmpty(info.CFPrices) Then
                            writer.PXWrite(PXKeywords.CFPRICES, value, info.CFPrices, lang)
                        End If
                        '*DAYADJ
                        If Not String.IsNullOrEmpty(info.DayAdj) Then
                            writer.PXWrite(PXKeywords.DAYADJ, value, info.DayAdj, lang, False)
                        End If
                        '*SEASADJ
                        If Not String.IsNullOrEmpty(info.SeasAdj) Then
                            writer.PXWrite(PXKeywords.SEASADJ, value, info.SeasAdj, lang, False)
                        End If
                        '*REFPERIOD
                        If Not String.IsNullOrEmpty(info.RefPeriod) Then
                            writer.PXWrite(PXKeywords.REFPERIOD, value, info.RefPeriod, lang)
                        End If
                        '*UNITS
                        'TODO Ask Louise about the correct placment of Units
                        If Not String.IsNullOrEmpty(info.Units) Then
                            writer.PXWrite(PXKeywords.UNITS, value, info.Units, lang)
                        Else
                            writer.PXWrite(PXKeywords.UNITS, value, "?", lang)
                        End If
                        '*CONTACT
                        If Not String.IsNullOrEmpty(info.Contact) Then
                            writer.PXWrite(PXKeywords.CONTACT, value, info.Contact, lang)
                        End If
                        '*BASEPERIOD
                        If Not String.IsNullOrEmpty(info.Baseperiod) Then
                            writer.PXWrite(PXKeywords.BASEPERIOD, value, info.Baseperiod, lang)
                        End If

                    End If
                Next
            End If
            '*META-ID
            If Not String.IsNullOrEmpty(meta.MetaId) Then
                writer.PXWrite(PXKeywords.META_ID, meta.MetaId, lang)
            End If
            For Each var In meta.Variables
                If Not String.IsNullOrEmpty(var.MetaId) Then
                    writer.PXWrite(PXKeywords.META_ID, var.Name, var.MetaId, lang)
                End If
            Next
            For Each var In meta.Variables
                For Each val In var.Values
                    If Not String.IsNullOrEmpty(val.MetaId) Then
                        writer.PXWrite(PXKeywords.META_ID, var.Name, val.Value, val.MetaId, lang)
                    End If
                Next
            Next
            '*DATANOTE
            If Not String.IsNullOrEmpty(meta.Datanote) Then
                writer.PXWrite(PXKeywords.DATANOTE, meta.Datanote, lang)
            End If
            For Each var In meta.Variables
                If Not String.IsNullOrEmpty(var.Datanote) Then
                    writer.PXWrite(PXKeywords.DATANOTE, var.Name, var.Datanote, lang)
                End If
            Next
            For Each var In meta.Variables
                For Each val In var.Values
                    If Not String.IsNullOrEmpty(val.Datanote) Then
                        writer.PXWrite(PXKeywords.DATANOTE, var.Name, val.Value, val.Datanote, lang)
                    End If
                Next
            Next
            '*DATABASE
            If Not String.IsNullOrEmpty(meta.Database) Then
                writer.PXWrite(PXKeywords.DATABASE, meta.Database, lang)
            End If
            '*SOURCE
            If Not String.IsNullOrEmpty(meta.Source) Then
                writer.PXWrite(PXKeywords.SOURCE, meta.Source, lang)
            End If
            '*INFOFILE
            If Not String.IsNullOrEmpty(meta.InfoFile) Then
                writer.PXWrite(PXKeywords.INFOFILE, meta.InfoFile, lang)
            End If
            '*NOTEX
            Dim note As String
            note = meta.Notes.GetMandatoryNotesString("##")
            If Not String.IsNullOrEmpty(note) Then
                writer.PXWrite(PXKeywords.NOTEX, note, lang)
            End If
            'For i As Integer = 0 To meta.Notes.Count - 1
            '    If meta.Notes(i).Mandantory Then
            '        writer.PXWrite(PXKeywords.NOTEX, meta.Notes(i).Text, lang)
            '    End If
            'Next
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.Notes Is Nothing Then
                    Continue For
                End If
                note = var.Notes.GetMandatoryNotesString("##")
                If Not String.IsNullOrEmpty(note) Then
                    writer.PXWrite(PXKeywords.NOTEX, GetVariableName(var), note, lang)
                End If
            Next
            'For i As Integer = 0 To meta.Variables.Count - 1
            '    var = meta.Variables(i)
            '    If var.Notes Is Nothing Then
            '        Continue For
            '    End If
            '    For j As Integer = 0 To var.Notes.Count - 1
            '        If var.Notes(j).Mandantory Then
            '            writer.PXWrite(PXKeywords.NOTEX, GetVariableName(var), var.Notes(j).Text, lang)
            '        End If
            '    Next
            'Next
            '*NOTE
            note = meta.Notes.GetNonMandatoryNotesString("##")
            If Not String.IsNullOrEmpty(note) Then
                writer.PXWrite(PXKeywords.NOTE, note, lang)
            End If
            'For i As Integer = 0 To meta.Notes.Count - 1
            '    If Not meta.Notes(i).Mandantory Then
            '        writer.PXWrite(PXKeywords.NOTE, meta.Notes(i).Text, lang)
            '    End If
            'Next
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                If var.Notes Is Nothing Then
                    Continue For
                End If
                note = var.Notes.GetNonMandatoryNotesString("##")
                If Not String.IsNullOrEmpty(note) Then
                    writer.PXWrite(PXKeywords.NOTE, GetVariableName(var), note, lang)
                End If
            Next
            'For i As Integer = 0 To meta.Variables.Count - 1
            '    var = meta.Variables(i)
            '    If var.Notes Is Nothing Then
            '        Continue For
            '    End If
            '    For j As Integer = 0 To var.Notes.Count - 1
            '        If Not var.Notes(j).Mandantory Then
            '            writer.PXWrite(PXKeywords.NOTE, GetVariableName(var), var.Notes(j).Text, lang)
            '        End If
            '    Next
            'Next
            '*VALUENOTEX
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                For k As Integer = 0 To var.Values.Count - 1
                    val = var.Values(k)
                    If val.Notes Is Nothing Then
                        Continue For
                    End If
                    note = val.Notes.GetMandatoryNotesString("##")
                    If Not String.IsNullOrEmpty(note) Then
                        writer.PXWrite(PXKeywords.VALUENOTEX, GetVariableName(var), val.Value, note, lang)
                    End If
                Next
            Next
            'For i As Integer = 0 To meta.Variables.Count - 1
            '    var = meta.Variables(i)
            '    For k As Integer = 0 To var.Values.Count - 1
            '        val = var.Values(k)
            '        If val.Notes Is Nothing Then
            '            Continue For
            '        End If
            '        For j As Integer = 0 To val.Notes.Count - 1
            '            If val.Notes(j).Mandantory Then
            '                writer.PXWrite(PXKeywords.VALUENOTEX, GetVariableName(var), val.Value, val.Notes(j).Text, lang)
            '            End If
            '        Next
            '    Next
            'Next
            '*VALUENOTE
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                For k As Integer = 0 To var.Values.Count - 1
                    val = var.Values(k)
                    If val.Notes Is Nothing Then
                        Continue For
                    End If
                    note = val.Notes.GetNonMandatoryNotesString("##")
                    If Not String.IsNullOrEmpty(note) Then
                        writer.PXWrite(PXKeywords.VALUENOTE, GetVariableName(var), val.Value, note, lang)
                    End If
                Next
            Next
            'For i As Integer = 0 To meta.Variables.Count - 1
            '    var = meta.Variables(i)
            '    For k As Integer = 0 To var.Values.Count - 1
            '        val = var.Values(k)
            '        If val.Notes Is Nothing Then
            '            Continue For
            '        End If
            '        For j As Integer = 0 To val.Notes.Count - 1
            '            If Not val.Notes(j).Mandantory Then
            '                writer.PXWrite(PXKeywords.VALUENOTE, GetVariableName(var), val.Value, val.Notes(j).Text, lang)
            '            End If
            '        Next
            '    Next
            'Next
            '*CELLNOTEEX
            SerializeCellNotes(meta, True, lang, writer)
            '*CELLNOTE
            SerializeCellNotes(meta, False, lang, writer)
            '*DATASYMBOL1
            If Not String.IsNullOrEmpty(meta.DataSymbol1) Then
                If Not meta.DataSymbol1 = PCAxis.Paxiom.PXConstant.DATASYMBOL_1_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL1, meta.DataSymbol1, lang)
                End If
            End If
            '*DATASYMBOL2
            If Not String.IsNullOrEmpty(meta.DataSymbol2) Then
                If Not meta.DataSymbol2 = PCAxis.Paxiom.PXConstant.DATASYMBOL_2_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL2, meta.DataSymbol2, lang)
                End If
            End If
            '*DATASYMBOL3
            If Not String.IsNullOrEmpty(meta.DataSymbol3) Then
                If Not meta.DataSymbol3 = PCAxis.Paxiom.PXConstant.DATASYMBOL_3_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL3, meta.DataSymbol3, lang)
                End If
            End If
            '*DATASYMBOL4
            If Not String.IsNullOrEmpty(meta.DataSymbol4) Then
                If Not meta.DataSymbol4 = PCAxis.Paxiom.PXConstant.DATASYMBOL_4_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL4, meta.DataSymbol4, lang)
                End If
            End If
            '*DATASYMBOL5
            If Not String.IsNullOrEmpty(meta.DataSymbol5) Then
                If Not meta.DataSymbol5 = PCAxis.Paxiom.PXConstant.DATASYMBOL_5_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL5, meta.DataSymbol5, lang)
                End If
            End If
            '*DATASYMBOL6
            If Not String.IsNullOrEmpty(meta.DataSymbol6) Then
                If Not meta.DataSymbol6 = PCAxis.Paxiom.PXConstant.DATASYMBOL_6_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOL6, meta.DataSymbol6, lang)
                End If
            End If
            ''*DATASYMBOL7
            'If Not String.IsNullOrEmpty(meta.DataSymbol7) Then
            '    If Not meta.DataSymbol7 = PCAxis.Paxiom.PXConstant.DATASYMBOL_7_STRING Then
            '        writer.PXWrite(PXKeywords.DATASYMBOL7, meta.DataSymbol7, lang)
            '    End If
            'End If
            '*DATASYMBOLSUM
            If Not String.IsNullOrEmpty(meta.DataSymbolSum) Then
                writer.PXWrite(PXKeywords.DATASYMBOLSUM, meta.DataSymbolSum, lang)
            End If
            '*DATASYMBOLNIL
            If Not String.IsNullOrEmpty(meta.DataSymbolNIL) Then
                If Not meta.DataSymbolNIL = PCAxis.Paxiom.PXConstant.DATASYMBOL_NIL_STRING Then
                    writer.PXWrite(PXKeywords.DATASYMBOLNIL, meta.DataSymbolNIL, lang)
                End If
            End If
            '*DATANOTECELL
            SerializeDataNoteCell(meta, lang, writer)
            '*DATANOTESUM
            If Not String.IsNullOrEmpty(meta.DataNoteSum) Then
                writer.PXWrite(PXKeywords.DATANOTESUM, meta.DataNoteSum, lang)
            End If
            'DEFAULT-GRAPH
            If meta.DefaultGraph <> Integer.MinValue Then
                writer.PXWrite(PXKeywords.DEFAULT_GRAPH, meta.DefaultGraph, lang)
            End If
            'DIRECTORY-PATH
            If Not String.IsNullOrEmpty(meta.DirectoryPath) Then
                writer.PXWrite(PXKeywords.DIRECTORY_PATH, meta.DirectoryPath, lang)
            End If
            '*INFO
            If Not String.IsNullOrEmpty(meta.Information) Then
                writer.PXWrite(PXKeywords.INFO, meta.Information, lang)
            End If
            '*LINK
            If Not String.IsNullOrEmpty(meta.Link) Then
                writer.PXWrite(PXKeywords.LINK, meta.Link, lang)
            End If
            '*SURVEY
            If Not String.IsNullOrEmpty(meta.Survey) Then
                writer.PXWrite(PXKeywords.SURVEY, meta.Survey, lang)
            End If
            '*VARIABLE-TYPE
            For i As Integer = 0 To meta.Variables.Count - 1
                Dim type As String
                type = meta.Variables(i).VariableType
                If Not String.IsNullOrEmpty(type) Then
                    'writer.PXWrite(PXKeywords.VARIABLE_TYPE, GetVariableName(meta.Variables(i)), meta.UpdateFrequency, lang)
                    writer.PXWrite(PXKeywords.VARIABLE_TYPE, GetVariableName(meta.Variables(i)), type, lang)
                End If
            Next
            'ATTRIBUTE-TEXT
            If Not meta.Attributes.Names Is Nothing Then
                If meta.Attributes.Names.Count > 0 Then
                    writer.BeginPXLine(PXKeywords.ATTRIBUTE_TEXT, lang)

                    For i As Integer = 0 To meta.Attributes.Names.Count - 2
                        writer.WriteValue(meta.Attributes.Names(i))
                        writer.WriteNextValue(meta.Attributes.Names(i + 1).Length)
                    Next
                    writer.WriteValue(meta.Attributes.Names(meta.Attributes.Names.Count - 1))
                    writer.EndPXLine()
                End If
            End If

        End Sub

        Private Shared Sub SerializeCellNotes(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal mandantory As Boolean, ByVal language As String, ByVal writer As System.IO.StreamWriter)
            Dim n As CellNote

            Dim vvp As VariableValuePair
            For i As Integer = 0 To meta.CellNotes.Count - 1
                n = meta.CellNotes(i)
                If n.Mandatory = mandantory Then

                    If mandantory Then
                        writer.Write(PXKeywords.CELLNOTEX)
                    Else
                        writer.Write(PXKeywords.CELLNOTE)
                    End If
                    If language IsNot Nothing Then
                        writer.Write("[")
                        writer.Write(language)
                        writer.Write("]")
                    End If

                    writer.Write("(")
                    For j As Integer = 0 To meta.Stub.Count - 1
                        writer.Write("""")
                        'Checks if there is a constraint for the variable
                        vvp = n.Conditions.FindByVariableCode(meta.Stub(j).Code)
                        If vvp Is Nothing Then
                            writer.Write("*") 'If no constraint for the variable
                        Else
                            writer.Write(meta.Stub(j).Values.GetByCode(vvp.ValueCode).Value)
                        End If
                        writer.Write("""")
                        If meta.Heading.Count > 0 Then
                            writer.Write(",")
                        End If
                    Next
                    For j As Integer = 0 To meta.Heading.Count - 1
                        writer.Write("""")
                        'Checks if there is a constraint for the variable
                        vvp = n.Conditions.FindByVariableCode(meta.Heading(j).Code)
                        If vvp Is Nothing Then
                            writer.Write("*") 'If no constraint for the variable
                        Else
                            writer.Write(meta.Heading(j).Values.GetByCode(vvp.ValueCode).Value)
                        End If
                        writer.Write("""")
                        If j <> (meta.Heading.Count - 1) Then
                            writer.Write(",")
                        End If
                    Next
                    writer.Write(")=")
                    writer.Write(SerializeCellNoteText(n.Text))
                    writer.WriteLine(""";")

                End If
            Next
        End Sub

        ''' <summary>
        ''' Serialize the cellnote(x) text
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function SerializeCellNoteText(ByVal text As String) As String
            Dim separator As String() = {vbCrLf}
            Dim arr As String() = text.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            Dim sb As New System.Text.StringBuilder()
            Dim i As Integer = 0

            sb.Append(vbCrLf)

            For Each part As String In arr
                sb.Append("""")
                sb.Append(SplitLongCellnote(part))
                If i < arr.Length - 1 Then
                    sb.Append("#""")
                    sb.Append(vbCrLf)
                End If
                i = i + 1
            Next

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Split cellnote to rows of 256 characters (start " and end " characters included)
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function SplitLongCellnote(ByVal text As String) As String

            If text.Length > 254 Then
                Dim sb As New System.Text.StringBuilder()
                Dim index As Integer = 0

                While index < text.Length
                    If index > 0 Then
                        sb.Append("""")
                    End If
                    If text.Length >= index + 254 Then
                        sb.Append(text.Substring(index, 254))
                        sb.Append("""")
                        sb.Append(vbCrLf)
                    Else
                        sb.Append(text.Substring(index))
                    End If
                    index = index + 254
                End While

                Return sb.ToString()
            Else
                Return text
            End If
        End Function

        ''' <summary>
        ''' Write attributes at cell level to the stream
        ''' </summary>
        ''' <param name="meta">PXMeta object</param>
        ''' <param name="writer">Stream writer</param>
        ''' <remarks></remarks>
        Private Shared Sub SerializeCellAttributes(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal writer As System.IO.StreamWriter)
            Dim first As Boolean
            Dim var As Variable
            Dim val As Value
            Dim fictionalCodes As Boolean
            For Each kvp As KeyValuePair(Of VariableValuePairs, String()) In meta.Attributes.CellAttributes
                first = True
                writer.Write(PXKeywords.ATTRIBUTES)
                writer.Write("(")
                For Each vvp As VariableValuePair In kvp.Key
                    fictionalCodes = False
                    var = meta.Variables.GetByCode(vvp.VariableCode)

                    If Not first Then
                        writer.Write(",")
                    End If
                    writer.Write("""")
                    If var.Values.IsCodesFictional Then
                        val = var.Values.GetByCode(vvp.ValueCode)
                        writer.Write(val.Text)
                    Else
                        writer.Write(vvp.ValueCode)
                    End If
                    writer.Write("""")
                    first = False
                Next
                writer.Write(")=")
                first = True
                For Each attr As String In kvp.Value
                    If Not first Then
                        writer.Write(",")
                    End If
                    writer.Write("""")
                    writer.Write(attr)
                    writer.Write("""")
                    first = False
                Next
                writer.WriteLine(";")
            Next

        End Sub

        Private Shared Sub SerializeDataNoteCell(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal language As String, ByVal writer As System.IO.StreamWriter)
            Dim n As DataNoteCell

            Dim vvp As VariableValuePair
            For i As Integer = 0 To meta.DataNoteCells.Count - 1
                n = meta.DataNoteCells(i)

                writer.Write(PXKeywords.DATANOTECELL)

                If language IsNot Nothing Then
                    writer.Write("[")
                    writer.Write(language)
                    writer.Write("]")
                End If

                writer.Write("(")
                For j As Integer = 0 To meta.Stub.Count - 1
                    writer.Write("""")
                    'Checks if there is a constraint for the variable
                    vvp = n.Conditions.FindByVariableCode(meta.Stub(j).Code)
                    If vvp Is Nothing Then
                        writer.Write("*") 'If no constraint for the variable
                    Else
                        If meta.Stub(j).Values.ValuesHaveCodes Then 'If codes exist use them otherview use value
                            writer.Write(meta.Stub(j).Values.GetByCode(vvp.ValueCode).Code)
                        Else
                            writer.Write(meta.Stub(j).Values.GetByCode(vvp.ValueCode).Value)
                        End If
                    End If
                    writer.Write("""")
                    If meta.Heading.Count > 0 Then
                        writer.Write(",")
                    End If
                Next
                For j As Integer = 0 To meta.Heading.Count - 1
                    writer.Write("""")
                    'Checks if there is a constraint for the variable
                    vvp = n.Conditions.FindByVariableCode(meta.Heading(j).Code)
                    If vvp Is Nothing Then
                        writer.Write("*") 'If no constraint for the variable
                    Else
                        If meta.Heading(j).Values.ValuesHaveCodes Then 'If codes exist use them otherview use value
                            writer.Write(meta.Heading(j).Values.GetByCode(vvp.ValueCode).Code)
                        Else
                            writer.Write(meta.Heading(j).Values.GetByCode(vvp.ValueCode).Value)
                        End If
                    End If
                    writer.Write("""")
                    If j <> (meta.Heading.Count - 1) Then
                        writer.Write(",")
                    End If
                Next
                writer.Write(")=""")
                writer.Write(n.Text)
                writer.WriteLine(""";")
            Next

        End Sub

        Private Function GetNumberOfDecimals(ByVal meta As PXMeta) As Integer
            Dim dec As Integer = 0

            dec = Math.Max(meta.Decimals, dec)

            Dim var As Variable
            Dim val As Value
            For i As Integer = 0 To meta.Variables.Count - 1
                var = meta.Variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    val = var.Values(j)
                    dec = Math.Max(val.Precision, dec)
                Next
            Next

            Return dec
        End Function

        ''' <summary>
        ''' Returns the name of the variable. 
        ''' </summary>
        ''' <param name="variable">The variable to get the name for</param>
        ''' <returns>The name of the variable. If the variable is a sortvariable $$SORT is returned.</returns>
        ''' <remarks></remarks>
        Private Function GetVariableName(ByVal variable As Variable) As String
            If variable.SortVariable Then
                Return PXConstant.SORTVARIABLE
            Else
                Return variable.Name
            End If
        End Function

    End Class


    Public Class PXFileWriter
        Inherits System.IO.StreamWriter

#Region "Constructors"

        Sub New(ByVal stream As System.IO.Stream)
            MyBase.New(stream)
        End Sub

        Sub New(ByVal path As String)
            MyBase.New(path)
        End Sub

        Sub New(ByVal path As String, ByVal append As Boolean)
            MyBase.New(path, append)
        End Sub

        Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
            MyBase.New(stream, encoding)
        End Sub

        Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding, ByVal bufferSize As Integer)
            MyBase.New(stream, encoding, bufferSize)
        End Sub

        Sub New(ByVal path As String, ByVal append As Boolean, ByVal encoding As System.Text.Encoding)
            MyBase.New(path, append, encoding)
        End Sub

        Sub New(ByVal path As String, ByVal append As Boolean, ByVal encoding As System.Text.Encoding, ByVal bufferSize As Integer)
            MyBase.New(path, append, encoding, bufferSize)
        End Sub

#End Region

        'KEYWORD[LANGUAGE]="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal value As String, ByVal language As String)
            PXWrite(keyword, value, language, True)
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal value As String, ByVal language As String)
            PXWrite(keyword, subkey, value, language, True)
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY, SUBKEY2)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal subkey2 As String, ByVal value As String, ByVal language As String)
            PXWrite(keyword, subkey, subkey2, value, language, True)
        End Sub

        'KEYWORD[LANGUAGE]="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal value As String, ByVal language As String, ByVal useQuote As Boolean)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("=")
            WriteValue(value, useQuote)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal value As String, ByVal language As String, ByVal useQuote As Boolean)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If
            Write("(""")
            Write(subkey)
            Write(""")=")
            WriteValue(value, useQuote)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY, SUBKEY2)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal subkey2 As String, ByVal value As String, ByVal language As String, ByVal useQuote As Boolean)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("(""")
            Write(subkey)
            Write(""",""")
            Write(subkey2)
            Write(""")=")
            WriteValue(value, useQuote)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE]="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal value As Integer, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("=")
            Write(value)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal value As Integer, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If
            Write("(""")
            Write(subkey)
            Write(""")=")
            Write(value)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY, SUBKEY2)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal subkey2 As String, ByVal value As Integer, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("(""")
            Write(subkey)
            Write(""",""")
            Write(subkey2)
            Write(""")=")
            Write(value)
            WriteLine(";")
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE]="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal value As Boolean, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            If value Then
                WriteLine("=YES;")
            Else
                WriteLine("=NO;")
            End If
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal value As Boolean, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If
            Write("(""")
            Write(subkey)
            If value Then
                WriteLine(""")=YES;")
            Else
                WriteLine(""")=NO;")
            End If
            _curentLineSize = 0
        End Sub

        'KEYWORD[LANGUAGE](SUBKEY, SUBKEY2)="VALUE";
        Public Sub PXWrite(ByVal keyword As String, ByVal subkey As String, ByVal subkey2 As String, ByVal value As Boolean, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("(""")
            Write(subkey)
            Write(""",""")
            Write(subkey2)
            If value Then
                Write(""")=YES;")
            Else
                Write(""")=NO;")
            End If
            _curentLineSize = 0
        End Sub

        Private _lineSize As Integer = 256
        Private _curentLineSize As Integer = 0

        Public Sub BeginPXLine(ByVal keyword As String, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("=")
            _curentLineSize = 0

        End Sub

        Public Sub BeginPXLine(ByVal keyword As String, ByVal subkey As String, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If
            Write("(""")
            Write(subkey)
            Write(""")=")
            _curentLineSize = 0

        End Sub

        Public Sub BeginPXLine(ByVal keyword As String, ByVal subkey As String, ByVal subkey2 As String, ByVal language As String)
            Write(keyword)

            If Not String.IsNullOrEmpty(language) Then
                Write("[")
                Write(language)
                Write("]")
            End If

            Write("(""")
            Write(subkey)
            Write(""",""")
            Write(subkey2)
            Write(""")=")
            _curentLineSize = 0
        End Sub

        Public Sub WriteValue(ByVal value As String)
            WriteValue(value, True)
        End Sub

        Public Sub WriteValue(ByVal value As String, ByVal useQuote As Boolean)
            Dim val As String
            If value.IndexOf(ControlChars.NewLine) > -1 Then
                val = value.Replace(ControlChars.NewLine, "#")
            Else
                val = value
            End If


            While val.Length + _curentLineSize + 2 > _lineSize
                If useQuote Then
                    Write("""")
                End If

                _curentLineSize += 1

                Dim cIndex As Integer = _lineSize - (_curentLineSize + 1)
                Write(val.Substring(0, cIndex))
                val = val.Substring(cIndex)
                If useQuote Then
                    WriteLine("""")
                End If
                _curentLineSize = 0
            End While

            If useQuote Then
                Write("""")
            End If

            Write(val)

            If useQuote Then
                Write("""")
            End If

            _curentLineSize += val.Length + 2


        End Sub

        Public Sub WriteNextValue(size As Integer)
            If _curentLineSize + size + 3 > _lineSize Then ' + 3 is for the " " characters and the comma
                WriteLine(",")
                _curentLineSize = 0
            Else
                Write(",")
                _curentLineSize += 1
            End If
        End Sub

        Public Sub EndPXLine()
            WriteLine(";")
            _curentLineSize = 0
        End Sub

    End Class

End Namespace
