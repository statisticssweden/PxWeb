Imports PCAxis.Paxiom.Extensions
Imports PCAxis.Paxiom.Localization
Imports System.Globalization

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Serializer that stores the model as a xml file that 
    ''' Excel 2003 and later versions understand
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ExcelFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

#Region "Private fields"
        Private model As PXModel

        Private _infoLevel As InformationLevelType = InformationLevelType.None
        Private writer As PXExcelTextWriter
        Private _showDataNoteCells As Boolean = False
        Private _modelDataNotePlacement As DataNotePlacementType
        Private _doubleColumn As DoubleColumnType = DoubleColumnType.NoDoubleColumns

#End Region

#Region "Public properties"

        Public Property InformationLevel() As InformationLevelType
            Get
                Return _infoLevel
            End Get
            Set(ByVal value As InformationLevelType)
                _infoLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Show code and text in separate columns?
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DoubleColumn() As DoubleColumnType
            Get
                Return _doubleColumn
            End Get
            Set(ByVal value As DoubleColumnType)
                _doubleColumn = value
            End Set
        End Property
#End Region

        Public Sub Serialize(ByVal model As PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize
            Using stream As New System.IO.FileStream(path, IO.FileMode.Create)
                Serialize(model, stream)
            End Using
        End Sub


        Public Sub Serialize(ByVal model As PXModel, ByVal stream As IO.Stream) Implements IPXModelStreamSerializer.Serialize
            MyClass.model = model
            Dim w As New PXExcelTextWriter(stream, EncodingUtil.GetEncoding(model.Meta.CodePage))
            writer = w

#If DEBUG Then
            writer.Indentation = 1
            writer.Formatting = Xml.Formatting.Indented
#Else
                writer.Formatting = Xml.Formatting.None
#End If

            'XML <?xml version="1.0"?>
            writer.WriteAllNecessaryStartElements(model.Meta.Matrix)
            'WriteTable(writer)
            WriteTable()

            writer.WriteEndDocument()
            writer.Flush()
        End Sub

        Private Sub WriteTable()

            Dim cell As Cell
            Dim cells As List(Of Cell)

            'TITLE
            cells = New List(Of Cell)
            cell = New Cell
            cell.Value = model.Meta.Title
            cell.DataType = "String"
            cells.Add(cell)
            WriteRow(cells)

            'SPACE ROWS
            WriteRow(Nothing)
            WriteRow(Nothing)

            Dim fmt As New DataFormatter(model)
            fmt.ThousandSeparator = ""
            fmt.InformationLevel = _infoLevel

            ' Keep track of if the model has datanotecells
            _showDataNoteCells = (model.Meta.DataNoteCells.Count > 0)
            _modelDataNotePlacement = fmt.DataNotePlacment

            If _modelDataNotePlacement = DataNotePlacementType.None Then
                ' Make sure we do not show any datanotecells
                _showDataNoteCells = False
            End If

            'HEADING
            WriteHeading()


            'DATA
            'Dim buffer(model.Data.MatrixColumnCount - 1) As Double
            Dim n As String = String.Empty
            Dim dataNote As String = String.Empty
            Dim dataNoteCell As Cell
            Dim valL As Long
            Dim valD As Double

            For i As Integer = 0 To model.Data.MatrixRowCount - 1
                cells.Clear()
                'model.Data.ReadLine(i, buffer)
                For k As Integer = 0 To model.Meta.Stub.Count - 1
                    If Me.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns Then
                        If Not model.Meta.Stub(k).Values.IsCodesFictional Then
                            cells.Add(GetStubCell(k, i, True))
                        End If
                    ElseIf Me.DoubleColumn = DoubleColumnType.OnlyDoubleColumnsWhenSpecified Then
                        If Not model.Meta.Stub(k).Values.IsCodesFictional Then
                            If model.Meta.Stub(k).DoubleColumn = True Then
                                cells.Add(GetStubCell(k, i, True))
                            End If
                        End If
                    End If
                    cells.Add(GetStubCell(k, i))
                Next
                For j As Integer = 0 To model.Data.MatrixColumnCount - 1
                    cell = New Cell
                    cell.Value = fmt.ReadElement(i, j, n, dataNote)
                    If Long.TryParse(cell.Value, valL) Then
                        cell.DataType = "Number"
                    Else
                        If Double.TryParse(cell.Value, valD) Then
                            cell.Value = cell.Value.Replace(",", ".") ' Must use . as decimal separator in Excel...
                            cell.DataType = "Number"
                        Else
                            cell.DataType = "String"
                        End If
                    End If
                    cell.Comment = n
                    cell.StyleName = "d1"

                    dataNoteCell = New Cell
                    dataNoteCell.Value = dataNote
                    dataNoteCell.DataType = "String"
                    If _showDataNoteCells AndAlso _modelDataNotePlacement = DataNotePlacementType.Before Then
                        cells.Add(dataNoteCell)
                    End If
                    cells.Add(cell)
                    If _showDataNoteCells AndAlso _modelDataNotePlacement = DataNotePlacementType.After Then
                        cells.Add(dataNoteCell)
                    End If
                Next
                WriteRow(cells)
            Next

            'Writes the information
            If _infoLevel > InformationLevelType.None Then
                WriteFootnotes()
                If _infoLevel = InformationLevelType.AllInformation Then
                    WriteTableInformation()
                End If
            End If

        End Sub

#Region "WriteFootnotes"

        Private Sub WriteFootnotes()
            If Not _infoLevel > InformationLevelType.None Then
                Exit Sub
            End If

            writer.WriteEmptyRow()

            Dim n As Note
            'Writes mandantory table notes
            For i As Integer = 0 To model.Meta.Notes.Count - 1
                n = model.Meta.Notes(i)
                If (n.Mandantory And _infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or _
                        _infoLevel > InformationLevelType.MandantoryFootnotesOnly Then

                    writer.WriteEmptyRow()
                    writer.WriteRowString(n.Text)
                End If
            Next

            'Writes mandantory variable notes
            Dim var As Variable
            For i As Integer = 0 To model.Meta.Variables.Count - 1
                var = model.Meta.Variables(i)
                If var.HasNotes Then
                    For j As Integer = 0 To var.Notes.Count - 1
                        n = var.Notes(j)
                        If (n.Mandantory And _infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or _
                                _infoLevel > InformationLevelType.MandantoryFootnotesOnly Then

                            writer.WriteRowString("")
                            writer.WriteRowString(var.Name & ":")
                            writer.WriteRowString(n.Text)
                        End If
                    Next
                End If
            Next

            'Writes mandantory value notes
            Dim val As Value
            For i As Integer = 0 To model.Meta.Variables.Count - 1
                var = model.Meta.Variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    val = var.Values(j)
                    If val.HasNotes Then
                        For k As Integer = 0 To val.Notes.Count - 1
                            n = val.Notes(k)
                            If (n.Mandantory And _infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or _
                                    _infoLevel > InformationLevelType.MandantoryFootnotesOnly Then

                                writer.WriteEmptyRow()
                                writer.WriteRowString(var.Name & ":")
                                writer.WriteRowString(val.Value & ":")
                                writer.WriteRowString(n.Text)
                            End If
                        Next
                    End If
                Next
            Next

            'Writes mandantory cellnotes 
            Dim cn As CellNote
            Dim vvp As VariableValuePair
            For i As Integer = 0 To model.Meta.CellNotes.Count - 1
                cn = model.Meta.CellNotes(i)
                If (cn.Mandatory And _infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or _
                        _infoLevel > InformationLevelType.MandantoryFootnotesOnly Then

                    writer.WriteEmptyRow()
                    For j As Integer = 0 To cn.Conditions.Count - 1
                        vvp = cn.Conditions(j)
                        var = model.Meta.Variables.GetByCode(vvp.VariableCode)
                        val = var.Values.GetByCode(vvp.ValueCode)
                        writer.WriteRowString(var.Name & ":")
                        writer.WriteRowString(val.Value & ":")
                    Next
                    writer.WriteRowString(cn.Text)
                End If
            Next

        End Sub

#End Region

        ''' <summary>
        ''' Writes table information to the file. If a content variable exist information are 
        ''' written for all the values of the content variable. Íf no content variable exists
        ''' information is taken directlty from PXMeta.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub WriteTableInformation()
            Dim contvar As Boolean = False
            Dim var As Variable
            Dim info As PCAxis.Paxiom.ContInfo
            Dim value As String
            Dim lastUpdated As New Dictionary(Of String, String)
            Dim contact As New Dictionary(Of String, String)
            Dim units As New Dictionary(Of String, String)
            Dim stockfa As New Dictionary(Of String, String)
            Dim refperiod As New Dictionary(Of String, String)
            Dim baseperiod As New Dictionary(Of String, String)
            Dim cfprices As New Dictionary(Of String, String)
            Dim dayadj As New Dictionary(Of String, String)
            Dim seasadj As New Dictionary(Of String, String)

            With model.Meta
                If .ContentVariable IsNot Nothing AndAlso .ContentVariable.Values.Count > 0 Then
                    contvar = True
                    var = model.Meta.ContentVariable

                    '1. Collect information for all the values
                    '-----------------------------------------
                    For i As Integer = 0 To var.Values.Count - 1
                        info = var.Values(i).ContentInfo
                        value = var.Values(i).Text

                        If info IsNot Nothing Then
                            'LAST-UPDATED
                            If Not String.IsNullOrEmpty(info.LastUpdated) Then
                                lastUpdated.Add(value, info.LastUpdated)
                            End If

                            'CONTACT
                            If Not String.IsNullOrEmpty(info.Contact) Then
                                contact.Add(value, info.Contact)
                            End If

                            'UNITS
                            If Not String.IsNullOrEmpty(info.Units) Then
                                units.Add(value, info.Units)
                            End If

                            'STOCKFA
                            If Not String.IsNullOrEmpty(info.StockFa) Then
                                stockfa.Add(value, info.StockFa)
                            End If

                            'REFPERIOD
                            If Not String.IsNullOrEmpty(info.RefPeriod) Then
                                refperiod.Add(value, info.RefPeriod)
                            End If

                            'BASEPERIOD
                            If Not String.IsNullOrEmpty(info.Baseperiod) Then
                                baseperiod.Add(value, info.Baseperiod)
                            End If

                            'CFPRICES
                            If Not String.IsNullOrEmpty(info.CFPrices) Then
                                cfprices.Add(value, info.CFPrices)
                            End If

                            'DAYADJ
                            If Not String.IsNullOrEmpty(info.DayAdj) Then
                                If info.DayAdj.ToUpper.Equals("YES") Then
                                    dayadj.Add(value, info.DayAdj)
                                End If
                            End If

                            'SEASADJ
                            If Not String.IsNullOrEmpty(info.SeasAdj) Then
                                If info.SeasAdj.ToUpper.Equals("YES") Then
                                    seasadj.Add(value, info.SeasAdj)
                                End If
                            End If
                        End If
                    Next
                End If

                '2. Write the collected information
                '----------------------------------

                'LAST-UPDATED
                If contvar Then
                    If lastUpdated.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordLastUpdated") & ":")

                        For Each kvp As KeyValuePair(Of String, String) In lastUpdated
                            writer.WriteKeyValueRowString(kvp.Key & ":", kvp.Value)
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.LastUpdated) Then
                            writer.WriteEmptyRow()
                            writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordLastUpdated") & ":", .ContentInfo.LastUpdated)
                        End If
                    End If
                End If

                'SOURCE
                If Not String.IsNullOrEmpty(.Source) Then
                    writer.WriteEmptyRow()
                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordSource") & ":", .Source)
                End If

                'CONTACT
                If contvar Then
                    If contact.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordContact") & ":")
                        For Each kvp As KeyValuePair(Of String, String) In contact
                            Dim str() As String

                            writer.WriteRowString(kvp.Key & ":")
                            str = kvp.Value.Split("#"c)

                            For i As Integer = 0 To str.Length - 1
                                writer.WriteRowString(str(i))
                            Next
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.Contact) Then
                            Dim str() As String

                            writer.WriteEmptyRow()
                            writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordContact") & ":")

                            str = .ContentInfo.Contact.Split("#"c)
                            For i As Integer = 0 To str.Length - 1
                                writer.WriteRowString(str(i))
                            Next
                        End If
                    End If
                End If

                'COPYRIGHT
                If .Copyright Then
                    writer.WriteEmptyRow()
                    writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordCopyright"))
                End If

                'UNITS
                If contvar Then
                    If units.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordUnits") & ":")
                        For Each kvp As KeyValuePair(Of String, String) In units
                            writer.WriteKeyValueRowString(kvp.Key & ":", kvp.Value)
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.Units) Then
                            writer.WriteEmptyRow()
                            writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordUnits") & ":", .ContentInfo.Units)
                        End If
                    End If
                End If

                'STOCKFA
                If contvar Then
                    If stockfa.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordStockfa") & ":")

                        For Each kvp As KeyValuePair(Of String, String) In stockfa
                            Select Case kvp.Value.ToUpper
                                Case "S"
                                    writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock"))
                                Case "F"
                                    writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow"))
                                Case "A"
                                    writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage"))
                            End Select
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.StockFa) Then
                            writer.WriteEmptyRow()
                            Select Case .ContentInfo.StockFa.ToUpper
                                Case "S"
                                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordStockfa") & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueStock"))
                                Case "F"
                                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordStockfa") & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueFlow"))
                                Case "A"
                                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordStockfa") & ":", model.Meta.GetLocalizedString("PxcKeywordStockfaValueAverage"))
                            End Select
                        End If
                    End If
                End If

                'REFPERIOD
                If contvar Then
                    If refperiod.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordRefPeriod") & ":")
                        For Each kvp As KeyValuePair(Of String, String) In refperiod
                            writer.WriteKeyValueRowString(kvp.Key & ":", kvp.Value)
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.RefPeriod) Then
                            writer.WriteEmptyRow()
                            writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordRefPeriod") & ":", .ContentInfo.RefPeriod)
                        End If
                    End If
                End If

                'BASEPERIOD
                If contvar Then
                    If baseperiod.Count > 0 Then
                        writer.WriteEmptyRow()
                        writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordBasePeriod") & ":")
                        For Each kvp As KeyValuePair(Of String, String) In baseperiod
                            writer.WriteKeyValueRowString(kvp.Key & ":", kvp.Value)
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.Baseperiod) Then
                            writer.WriteEmptyRow()
                            writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordBasePeriod") & ":", .ContentInfo.Baseperiod)
                        End If
                    End If
                End If

                'CFPRICES
                If contvar Then
                    If cfprices.Count > 0 Then
                        writer.WriteEmptyRow()
                        For Each kvp As KeyValuePair(Of String, String) In cfprices
                            Select Case kvp.Value.ToUpper
                                Case "C"
                                    writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent"))
                                Case "F"
                                    writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed"))
                            End Select
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.CFPrices) Then
                            writer.WriteEmptyRow()
                            Select Case .ContentInfo.CFPrices.ToUpper
                                Case "C"
                                    writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordCFPricesValueCurrent"))
                                Case "F"
                                    writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordCFPricesValueFixed"))
                            End Select
                        End If
                    End If
                End If

                'DAYADJ
                If contvar Then
                    If dayadj.Count > 0 Then
                        writer.WriteEmptyRow()
                        For Each kvp As KeyValuePair(Of String, String) In dayadj
                            writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordDayAdj"))
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.DayAdj) Then
                            If .ContentInfo.DayAdj.ToUpper.Equals("YES") Then
                                writer.WriteEmptyRow()
                                writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordDayAdj"))
                            End If
                        End If
                    End If
                End If

                'SEASADJ
                If contvar Then
                    If seasadj.Count > 0 Then
                        writer.WriteEmptyRow()
                        For Each kvp As KeyValuePair(Of String, String) In seasadj
                            writer.WriteKeyValueRowString(kvp.Key & ":", model.Meta.GetLocalizedString("PxcKeywordSeasAdj"))
                        Next
                    End If
                Else
                    If .ContentInfo IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ContentInfo.SeasAdj) Then
                            If .ContentInfo.SeasAdj.ToUpper.Equals("YES") Then
                                writer.WriteEmptyRow()
                                writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordSeasAdj"))
                            End If
                        End If
                    End If
                End If

                'OFFICIAL STATISTICS
                'If the statistics are official, insert information about that in the file 
                'Reqtest error report #406
                If Not String.IsNullOrEmpty(CStr(.OfficialStatistics)) Then
                    writer.WriteEmptyRow()
                    writer.WriteRowString(model.Meta.GetLocalizedString("PxcKeywordOfficialStatistics"))

                End If

                'DATABASE
                If Not String.IsNullOrEmpty(.Database) Then
                    writer.WriteEmptyRow()
                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordDatabase") & ":", .Database)
                End If

                'MATRIX
                If Not String.IsNullOrEmpty(.Matrix) Then
                    writer.WriteEmptyRow()
                    writer.WriteKeyValueRowString(model.Meta.GetLocalizedString("PxcKeywordMatrix") & ":", .Matrix)
                End If
            End With

        End Sub

        Private Sub WriteHeading()
            Dim Cell As Cell
            Dim cells As List(Of Cell)
            Dim si As New List(Of Cell)
            Dim intv As New List(Of Cell)
            Dim p As Integer

            'Calc left indention
            For k As Integer = 0 To model.Meta.Stub.Count - 1
                If Me.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns Then
                    If Not model.Meta.Stub(k).Values.IsCodesFictional Then
                        si.Add(New Cell)
                    End If
                ElseIf Me.DoubleColumn = DoubleColumnType.OnlyDoubleColumnsWhenSpecified Then
                    If Not model.Meta.Stub(k).Values.IsCodesFictional Then
                        If model.Meta.Stub(k).DoubleColumn = True Then
                            si.Add(New Cell)
                        End If
                    End If
                End If
                si.Add(New Cell)
            Next

            'HEADING
            For i As Integer = 0 To model.Meta.Heading.Count - 1
                cells = New List(Of Cell)
                'STUB INDENTION
                cells.AddRange(si)

                'INTERVAL
                intv.Clear()
                For k As Integer = 0 To CalcHeadingInterval(i) - 1
                    intv.Add(New Cell)
                Next

                'HEADING
                If i > 0 Then
                    p = model.Meta.Heading(i - 1).Values.Count - 1
                Else
                    p = 0
                End If

                For l As Integer = 0 To p
                    For j As Integer = 0 To model.Meta.Heading(i).Values.Count - 1
                        Cell = New Cell
                        Cell.Value = model.Meta.Heading(i).Values(j).Text
                        Cell.DataType = "String"

                        cells.Add(Cell)
                        cells.AddRange(intv)
                        If _showDataNoteCells Then
                            ' Add empty cells to get the aligning correct
                            For ii As Integer = 0 To CalcHeadingInterval(i)
                                cells.Add(New Cell)
                            Next
                        End If
                    Next
                Next

                WriteRow(cells)
            Next
        End Sub



        Private Sub WriteRow(ByVal cells As List(Of Cell))
            writer.WriteStartRow() 'Row
            If cells IsNot Nothing Then
                Dim cell As Cell
                For i As Integer = 0 To cells.Count - 1
                    cell = cells(i)
                    writer.WriteDataCell(cell.Value, cell.Comment, cell.StyleName, cell.DataType)
                Next
            End If
            writer.WriteEndElement() 'Row
        End Sub

        Private Function CalcHeadingInterval(ByVal headingLevel As Integer) As Integer
            Dim interv As Integer = 1
            'Intervall
            If headingLevel < model.Meta.Heading.Count - 1 Then
                For i As Integer = model.Meta.Heading.Count - 1 To headingLevel + 1 Step -1
                    interv *= model.Meta.Heading(i).Values.Count
                Next
                interv -= 1
            Else
                interv = 0
            End If

            Return interv
        End Function


        ''' <summary>
        ''' Returns a cell containing the value (or code) for the given stub-variable and row
        ''' </summary>
        ''' <param name="StubNr">Ordernumber for the variable in the stub</param>
        ''' <param name="RowNr">Row number</param>
        ''' <param name="Code">If true return code, else return value</param>
        ''' <returns>A cell containing the value or code</returns>
        ''' <remarks></remarks>
        Private Function GetStubCell(ByVal StubNr As Integer, _
                                     ByVal RowNr As Integer, _
                                     Optional ByVal Code As Boolean = False) As Cell
            Dim Interval As Integer
            Dim count As Integer = model.Meta.Stub(StubNr).Values.Count

            If StubNr < model.Meta.Stub.Count - 1 Then
                Interval = CalcStubInterval(StubNr + 1)
            Else
                Interval = 1
            End If

            Dim val As Value
            If RowNr Mod Interval = 0 Then
                Dim Cell As New Cell

                val = model.Meta.Stub(StubNr).Values((RowNr \ Interval) Mod count)
                If Code = True Then
                    Cell.Value = val.Code
                Else
                    Cell.Value = val.Text
                End If
                If _infoLevel > InformationLevelType.None And val.HasNotes Then
                    If _infoLevel = InformationLevelType.MandantoryFootnotesOnly Then
                        Cell.Comment = val.Notes.GetAllMandantoryNotes()
                    Else
                        Cell.Comment = val.Notes.GetAllNotes()
                    End If
                End If

                Cell.DataType = "String"
                Return Cell
            Else
                Return New Cell
            End If
        End Function

        Private Function CalcStubInterval(ByVal StubChildNr As Integer) As Integer
            Dim interv As Integer = 1
            'Intervall
            For i As Integer = model.Meta.Stub.Count - 1 To StubChildNr Step -1
                interv *= model.Meta.Stub(i).Values.Count
            Next
            Return interv
        End Function


        Private Structure Cell
            Public Value As String
            Public DataType As String
            Public StyleName As String
            Public Comment As String
        End Structure
    End Class



    Friend Class PXExcelTextWriter
        Inherits System.Xml.XmlTextWriter

#Region "Constructors"

        Sub New(ByVal writer As System.IO.TextWriter)
            MyBase.New(writer)

        End Sub

        Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
            MyBase.New(stream, encoding)

        End Sub

        Sub New(ByVal path As String, ByVal encoding As System.Text.Encoding)
            MyBase.New(path, encoding)

        End Sub

#End Region

        Public Sub WriteAllNecessaryStartElements(ByVal worksheetName As String)
            WriteStartDocument()
            WriteStartWorkbook()
            WriteDocumentProperties()
            WriteStyles()
            WriteStartWorksheet(worksheetName)
            WriteStartTable()
        End Sub


        Public Overrides Sub WriteStartDocument()
            MyBase.WriteStartDocument()
            'XML PROCESSINSRTUCTIONS <?mso-application progid="Excel.Sheet"?>
            WriteProcessingInstruction("mso-application", "progid=""Excel.Sheet""")
        End Sub

        Public Sub WriteStartWorkbook()
            WriteStartElement("Workbook")
            'Writes namespaces
            WriteAttributeString("xmlns", Nothing, "urn:schemas-microsoft-com:office:spreadsheet")
            WriteAttributeString("xmlns", "o", Nothing, "urn:schemas-microsoft-com:office:office")
            WriteAttributeString("xmlns", "x", Nothing, "urn:schemas-microsoft-com:office:excel")
            WriteAttributeString("xmlns", "ss", Nothing, "urn:schemas-microsoft-com:office:spreadsheet")
            WriteAttributeString("xmlns", "html", Nothing, "http://www.w3.org/TR/REC-html40")
        End Sub

        Private Sub WriteDocumentProperties()
            WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office")
            WriteElementString("Author", "")
            WriteElementString("LastAuthor", "")
            WriteElementString("Created", "")
            WriteElementString("LastSaved", "")
            WriteElementString("Company", "")
            WriteElementString("Version", "")
            WriteEndElement() 'DocumentProperties
        End Sub

        Private Sub WriteStyles()
            WriteStartElement("Styles")
            'Default style
            WriteStartElement("Style")
            WriteAttributeString("ss", "ID", Nothing, "Default")
            WriteAttributeString("ss", "Name", Nothing, "Normal")
            WriteElementString("Alignment", "")
            WriteElementString("Borders", "")
            WriteElementString("Font", "")
            WriteElementString("Interior", "")
            WriteElementString("NumberFormat", "")
            WriteElementString("Protection", "")
            WriteEndElement() 'Default style
            'Data style
            WriteStartElement("Style")
            WriteAttributeString("ss", "ID", Nothing, "d1")
            WriteStartElement("Alignment")
            WriteAttributeString("ss", "Horizontal", Nothing, "Right")
            WriteAttributeString("ss", "Vertical", Nothing, "Bottom")
            WriteEndElement() 'Alignment
            WriteEndElement() 'Data style
            WriteEndElement() 'Styles
        End Sub


        Public Sub WriteStartWorksheet(ByVal name As String)
            WriteStartElement("Worksheet")
            WriteAttributeString("ss", "Name", Nothing, name)
        End Sub

        Public Sub WriteStartTable()
            WriteStartElement("Table")
        End Sub

        Public Sub WriteStartRow()
            WriteStartElement("Row")
        End Sub

        Public Sub WriteStringCell(ByVal data As String, ByVal comment As String, ByVal styleID As String)
            WriteStartElement("Cell")
            If Not String.IsNullOrEmpty(styleID) Then
                WriteAttributeString("ss", "StyleID", Nothing, styleID)
            End If
            If Not String.IsNullOrEmpty(data) Then
                WriteStartElement("Data")
                WriteAttributeString("ss", "Type", Nothing, "String")
                WriteString(data)
                WriteEndElement() ' Data
                If Not String.IsNullOrEmpty(comment) Then
                    WriteStartElement("Comment")
                    WriteStartElement("ss", "Data", Nothing)
                    WriteString(comment)
                    WriteEndElement() ' ss:Data
                    WriteEndElement() ' Comment
                End If
            End If
            WriteEndElement() ' Cell
        End Sub

        Public Sub WriteDataCell(ByVal data As String, ByVal comment As String, ByVal styleID As String, ByVal dataType As String)
            WriteStartElement("Cell")
            If Not String.IsNullOrEmpty(styleID) Then
                WriteAttributeString("ss", "StyleID", Nothing, styleID)
            End If
            If Not String.IsNullOrEmpty(data) Then
                WriteStartElement("Data")
                WriteAttributeString("ss", "Type", Nothing, dataType)
                WriteString(data)
                WriteEndElement() ' Data
                If Not String.IsNullOrEmpty(comment) Then
                    WriteStartElement("Comment")
                    WriteStartElement("ss", "Data", Nothing)
                    WriteString(comment)
                    WriteEndElement() ' ss:Data
                    WriteEndElement() ' Comment
                End If
            End If
            WriteEndElement() ' Cell
        End Sub

        Public Sub WriteEmptyCell()
            WriteStartElement("Cell")
            WriteStartElement("Data")
            WriteAttributeString("ss", "Type", Nothing, "String")
            WriteString("")
            WriteEndElement() ' Data
            WriteEndElement() ' Cell
        End Sub

        Public Sub WriteEmptyRow()
            WriteStartRow()
            WriteEmptyCell()
            WriteEndElement() 'Row
        End Sub

        Public Sub WriteRowString(ByVal row As String)
            WriteStartElement("Row")
            WriteStartElement("Cell")
            WriteStartElement("Data")
            WriteAttributeString("ss", "Type", Nothing, "String")
            WriteString(row)
            WriteEndElement() ' Data
            WriteEndElement() ' Cell
            WriteEndElement() ' Row
        End Sub

        Public Sub WriteKeyValueRowString(ByVal key As String, ByVal value As String)
            WriteStartElement("Row")
            WriteStartElement("Cell")
            WriteStartElement("Data")
            WriteAttributeString("ss", "Type", Nothing, "String")
            WriteString(key)
            WriteEndElement() ' Data
            WriteEndElement() ' Cell
            WriteStartElement("Cell")
            WriteStartElement("Data")
            WriteAttributeString("ss", "Type", Nothing, "String")
            WriteString(value)
            WriteEndElement() ' Data
            WriteEndElement() ' Cell
            WriteEndElement() ' Row
        End Sub

    End Class
End Namespace
