


Imports PCAxis.Web.Core.Enums
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Globalization
Imports System.ComponentModel
Imports PCAxis.Paxiom.Localization
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Core.Management
''' <summary>
''' Displays a <see cref="Paxiom.PXModel" /> as a table
''' </summary>
''' <remarks></remarks>
Public Class TableCodeBehind
    Inherits PaxiomControlBase(Of TableCodeBehind, Table)

    Public Class NumericStringComparer
        Implements IComparer
        ' Calls CaseInsensitiveComparer.Compare 
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim s1, s2 As String
            Dim d1, d2 As Double

            s1 = x.ToString()
            s2 = y.ToString()

            If Double.TryParse(s1, d1) And Double.TryParse(s2, d2) Then
                Return CompareNumbers(d1, d2)
            ElseIf ParseNumber(s1, d1) AndAlso ParseNumber(s2, d2) Then
                Return CompareNumbers(d1, d2)
            Else
                Return New CaseInsensitiveComparer().Compare(s1, s2)
            End If

        End Function 'IComparer.Compare

        ''' <summary>
        ''' Compare which is the bigger value
        ''' </summary>
        ''' <param name="d1">Double value 1</param>
        ''' <param name="d2">Double value 2</param>
        ''' <returns>-1 if d2 is the biggest value, 0 if the values are equal and 1 if d1 is the bigger value</returns>
        ''' <remarks></remarks>
        Private Function CompareNumbers(ByVal d1 As Double, ByVal d2 As Double) As Integer
            If d1 < d2 Then
                Return -1
            ElseIf d1 > d2 Then
                Return 1
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' This method is used to parse the digit part of a value that consists of digits and characters.
        ''' Example: The value "12 years" will result in 12 being returned in the d output parameter.
        ''' </summary>
        ''' <param name="s">Input string</param>
        ''' <param name="d">Output double value</param>
        ''' <returns>True if a double value could be extracted from the input string, else false</returns>
        ''' <remarks>The string must start with a digit to be able to extract a double value</remarks>
        Private Function ParseNumber(ByVal s As String, ByRef d As Double) As Boolean
            Dim i As Integer

            For i = 0 To s.Length - 1
                If Not Char.IsDigit(s.Chars(i)) Then
                    Exit For
                End If
            Next

            If i > 0 Then
                Return Double.TryParse(s.Substring(0, i), d)
            End If

            Return False
        End Function
    End Class




#Region " Constants "

    Private Const FOOTNOTE_LABEL As String = "FootNoteLabel"
    Private Const EMPTY_CELL_VALUE As String = "&nbsp;"
    Private Const DECIMAL_DELIMITER As String = "CtrlTableDecimalDelimiter"
    Private Const THOUSAND_DELIMITER As String = "CtrlTableThousandDelimiter"
    Private Const DEFAULTATTRIBUTES_LABEL As String = "CtrlTableDefaultAttributesLabel"
    Private Const CELLINFORMATION_DIALOGTITLE As String = "CtrlTableCellInformationDialogTitle"
    Private Const CELLINFORMATION_CLOSEBUTTON As String = "PxWebPopupDialogClose"
    Private Const CELLINFORMATION_CELL As String = "CtrlTableCellInformationCell"
    Private Const CELLINFORMATION_ATTRIBUTES As String = "CtrlTableCellInformationAttributes"
    Private Const CELLINFORMATION_NOTES As String = "CtrlTableCellInformationNotes"
    Private Const CSS_HEADER_FIRST As String = "table-header-first"
    Private Const CSS_HEADER_LAST As String = "table-header-last"
    Private Const CSS_HEADER_MIDDLE As String = "table-header-middle"
    Private Const CSS_DATA_FILLED As String = "table-data-filled"
    Private Const CSS_ATTRIBUTE_CELL As String = "attribute-cell"
    Private Const CSS_TITLE As String = "table-title"
    Private Const CSS_STUB As String = "table-stub"
    Private Const LAYOUT1_PREFIX As String = "layout1-"
    Private Const LAYOUT2_PREFIX As String = "layout2-"
    Private Const CSS_LEFT As String = "left"
    Private Const CSS_RIGHT As String = "right"
    Private Const CSS_TABLE_SORT_HEADER As String = "table-sort-header"
    Private Const CSS_TABLE_JQUERYSORT_HEADER As String = "table-jquerysort-header"
    Private Const HEADER_CODE_LENGTH As Integer = 10
#End Region

#Region " Private Variables "

    Protected DataTable As WebControls.Table
    Protected pcaxis_table_defaultattributes As HtmlGenericControl
    Protected lblDefaultAttributes As Label
    Protected rptDefaultAttributes As Repeater
    Protected pxtableCellInformationDialog As HtmlGenericControl
    Protected pxtableCellInformationDialogCloseText As HiddenField
    Private _hierarchyLevelMap As Dictionary(Of String, Integer)
    Dim _formatter As DataFormatter
    Private _sortColumn As Integer
    Private _sortAscending As Boolean
    Private _rowCount As Integer
    Private _isCropped As Boolean
    Private Shared _regex As New System.Text.RegularExpressions.Regex("[^\w\.-:_]")
#End Region



    ''' <summary>
    ''' Gets or sets the column to use to sort the table
    ''' </summary>
    ''' <value>Integer representing the index of the column to sort</value>
    ''' <returns>An integer representing the column to sort</returns>
    ''' <remarks>Used by the sorting function</remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Protected Property SortColumn() As Integer
        Get
            Return _sortColumn
        End Get
        Set(ByVal value As Integer)
            _sortColumn = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether to sort the table ascending or not
    ''' </summary>
    ''' <value>If <c>True</c> the the table is sorted ascending otherwise it's not</value>
    ''' <returns><c>True</c> it the table is sorted ascending, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Protected Property SortAscending() As Boolean
        Get
            Return _sortAscending
        End Get
        Set(ByVal value As Boolean)
            _sortAscending = value
        End Set
    End Property



#Region " Hierarchy "

    ''' <summary>
    ''' Used to create a lookup for all the <see cref="Value" /> in a <see cref="Variable" /> that has a hierarchy
    ''' </summary>
    ''' <param name="hierarchyVariable">The <see cref="Variable" /> to create the lookup for</param>
    ''' <remarks></remarks>
    Private Sub BuildHierarchyMap(ByVal hierarchyVariable As Variable)
        'Create new dictionary to use for lookup, in contains the id the code of the value and the level in the hierarchy it has
        _hierarchyLevelMap = New Dictionary(Of String, Integer)

        'Loop through the hierarchy recursively  
        'For Each hierarchyLevel As HierarchyLevel In hierarchyVariable.Hierarchy.Children
        '    'Begin at level 1 in the hierarchy
        '    BuildHierarchyLevel(hierarchyLevel, 1)
        'Next
        BuildHierarchyLevel(hierarchyVariable.Hierarchy.RootLevel, 1)

    End Sub

    ''' <summary>
    ''' Adds values to the hierarchy lookup
    ''' </summary>
    ''' <param name="hierarchy">The <see cref="HierarchyLevel" /> to process</param>
    ''' <param name="level">The current level in the hierarchy</param>
    ''' <remarks></remarks>
    Private Sub BuildHierarchyLevel(ByVal hierarchy As HierarchyLevel, ByVal level As Integer)
        'Add the current hierarchylevel to the hierarchy lookup
        _hierarchyLevelMap.Add(hierarchy.Code, level)

        For Each hierarchyLevel As HierarchyLevel In hierarchy.Children
            'Build a new hierarchy level from the children of the current hierarchy level
            BuildHierarchyLevel(hierarchyLevel, level + 1)
        Next
    End Sub

#End Region

#Region " Sort "
    ''' <summary>
    ''' Modifies the layout2 table by replacing all multiple rowspan cells with one cell for each row.
    ''' The column is then sorted by the column that the user clicked on.
    ''' </summary>
    ''' <param name="table">The table to transform</param>
    ''' <remarks></remarks>
    Private Sub SortTable(ByVal table As WebControls.Table)
        Dim lastHeaders As List(Of String) = TransformTableToSortLayout(table)

        If SortColumn = GetSortColumn() Then
            SortAscending = Not SortAscending
        Else
            SortAscending = True
        End If
        SortColumn = GetSortColumn()

        'Dim maxLength As Integer = 0
        'Dim isNumber As Boolean = False
        'For Each row As TableRow In table.Rows
        '    Dim key As String = row.Cells(SortColumn).Text
        '    isNumber = isNumber Or IsNumeric(key)
        '    If Len(key) > maxLength Then
        '        maxLength = Len(key)
        '    End If
        'Next
        ' Dim sortedList As New SortedList(Of String, List(Of TableRow))
        Dim sortedList As New SortedList(New NumericStringComparer())
        'Dim sortedList As New SortedList()
        For Each row As TableRow In table.Rows
            Dim key As String = row.Cells(SortColumn).Text
            'If isNumber Then
            '    key = RemoveNonDigitsAndZeroPad(key, maxLength)
            'End If
            If Not sortedList.ContainsKey(key) Then
                Dim list As New List(Of TableRow)
                sortedList.Add(key, list)
            End If
            CType(sortedList(key), List(Of TableRow)).Add(row)
        Next

        table.Rows.Clear()
        If SortAscending Then
            For Each rows As List(Of TableRow) In sortedList.Values
                For Each row As TableRow In rows
                    table.Rows.Add(row)
                Next
            Next
        Else
            For i As Integer = sortedList.Values.Count - 1 To 0 Step -1
                For Each row As TableRow In CType(sortedList.Values(i), List(Of TableRow))
                    table.Rows.Add(row)
                Next
            Next
        End If

        Dim headerRow As New TableRow
        headerRow.CssClass = CSS_TABLE_SORT_HEADER
        headerRow.TableSection = TableRowSection.TableHeader
        Dim nrOfVariables As Integer = PaxiomModel.Meta.Variables.Count - 2
        For i As Integer = 0 To nrOfVariables
            headerRow.Cells.Add(CreateHeaderLink(i, PaxiomModel.Meta.Variables(i).Name))
        Next
        For i As Integer = 0 To lastHeaders.Count - 1
            headerRow.Cells.Add(CreateHeaderLink(nrOfVariables + 1 + i, lastHeaders(i)))
        Next
        table.Rows.AddAt(0, headerRow)



        'From SortTableWithJQuery
        headerRow = New TableRow
        headerRow.CssClass = CSS_TABLE_JQUERYSORT_HEADER
        headerRow.TableSection = TableRowSection.TableHeader
        nrOfVariables = PaxiomModel.Meta.Variables.Count - 2
        For i As Integer = 0 To nrOfVariables
            headerRow.Cells.Add(CreateHeaderCell(i, PaxiomModel.Meta.Variables(i).Name))
        Next
        For i As Integer = 0 To lastHeaders.Count - 1
            headerRow.Cells.Add(CreateHeaderCell(nrOfVariables + 1 + i, lastHeaders(i)))

        Next
        table.Rows.AddAt(0, headerRow)
        'end  from SortTableWithJQuery

    End Sub



    Private Function IsStringInAnyStrings(ByVal Str As String, ByVal ParamArray Strs() As String) As Boolean
        For Each s In Strs
            If Not s Is Nothing Then
                If Str = s Then Return True
            End If
        Next
        Return False
    End Function



    ''' <summary>
    ''' Transforms the table to sort layout
    ''' </summary>
    ''' <param name="table">The table to transform</param>
    ''' <returns>a list of last headers</returns>    
    Private Function TransformTableToSortLayout(ByVal table As System.Web.UI.WebControls.Table) As List(Of String)
        table.Rows.RemoveAt(0)
        table.Rows.RemoveAt(0)
        Dim nrOfColumns As Integer = table.Rows(0).Cells.Count
        For c As Integer = 0 To nrOfColumns - 1
            For r As Integer = 0 To table.Rows.Count - 1
                Dim cell As TableCell = table.Rows(r).Cells(c)
                Dim span As Integer = cell.RowSpan
                table.Rows(r).Cells.RemoveAt(c)
                Dim newCell As New TableCell
                newCell.Text = cell.Text
                table.Rows(r).Cells.AddAt(c, newCell)
                If span > 1 Then
                    For r2 As Integer = r + 1 To r + span - 1
                        Dim cell2 As New TableCell
                        cell2.Text = cell.Text
                        'If table has been cropped this row might not exist...
                        If r2 < table.Rows.Count Then
                            table.Rows(r2).Cells.AddAt(c, cell2)
                        End If
                    Next
                    r = r + span - 1 'Avoid unneccessary loopin                    
                End If
            Next
        Next

        'Move last variable to heading
        Dim lastHeaders As New List(Of String)
        Dim lastCells As New Queue(Of TableCell)
        Dim necessaryRows As New List(Of TableRow)
        If nrOfColumns > 2 Then
            For r As Integer = 0 To table.Rows.Count - 1
                Dim cell As TableCell = table.Rows(r).Cells(nrOfColumns - 2)
                If Not lastHeaders.Contains(cell.Text) Then
                    lastHeaders.Add(cell.Text)
                Else
                    Exit For
                End If
            Next

            For r As Integer = 0 To table.Rows.Count - 1
                Dim cell As TableCell = table.Rows(r).Cells(nrOfColumns - 1)
                cell.CssClass = CSS_RIGHT
                lastCells.Enqueue(cell)
            Next

            For r As Integer = 0 To table.Rows.Count - 1 Step lastHeaders.Count
                table.Rows(r).Cells.RemoveAt(nrOfColumns - 1)
                table.Rows(r).Cells.RemoveAt(nrOfColumns - 2)
                For Each cell As TableCell In table.Rows(r).Cells
                    cell.CssClass = CSS_LEFT
                Next
                necessaryRows.Add(table.Rows(r))
            Next
            table.Rows.Clear()
            For Each row As TableRow In necessaryRows
                For i As Integer = 1 To lastHeaders.Count
                    If lastCells.Count > 0 Then
                        row.Cells.Add(lastCells.Dequeue)
                    End If
                Next
                table.Rows.Add(row)
            Next
        End If

        Return lastHeaders
    End Function


    ''' <summary>
    ''' Creates a header cell
    ''' </summary>
    ''' <param name="column">The column index to create the header for</param>
    ''' <param name="text">The text of the header</param>
    ''' <returns>A <see cref="TableHeaderCell" /> with the column header in it</returns>    
    Private Function CreateHeaderCell(ByVal column As Integer, ByVal text As String) As TableHeaderCell
        Dim headerCell As New TableHeaderCell
        Dim lit As New Literal
        lit.Text = text
        headerCell.Controls.Add(lit)
        Return headerCell
    End Function


    ''' <summary>
    ''' Creates a link for a header using an input button
    ''' </summary>
    ''' <param name="column">The column index to create the button for</param>
    ''' <param name="text">The text of the button</param>
    ''' <returns>A <see cref="TableHeaderCell" /> with the button in it</returns>
    ''' <remarks>Used by the sorting</remarks>
    Private Function CreateHeaderLink(ByVal column As Integer, ByVal text As String) As TableHeaderCell
        Dim headerCell As New TableHeaderCell
        Dim lit As New Literal
        lit.Text = String.Format("<input type=""submit"" id=""TableHeaderButton{0}"" name=""TableHeaderButton{0}"" value=""{1}"" />", column, text)
        headerCell.Controls.Add(lit)
        Return headerCell
    End Function




    ''' <summary>
    ''' Checks which column the user clicked on.
    ''' </summary>
    ''' <returns>The index of the clicked column.</returns>
    Private Function GetSortColumn() As Integer
        Dim sortColumn As Integer = 0
        Try
            For Each key As String In Page.Request.Form.Keys
                If key.StartsWith("TableHeaderButton") Then
                    Dim number As String = key.Replace("TableHeaderButton", "")
                    sortColumn = Integer.Parse(number)
                End If
            Next
        Catch
        End Try
        Return sortColumn
    End Function

    '''' <summary>
    '''' Used to make numbers alphabetical sortable by removing whitespace and pad with zeros in the beginning.
    '''' </summary>
    '''' <param name="number">The string containing the number to transform.</param>
    '''' <param name="maxLength">The longest string in the column. Used to pad the number.</param>
    '''' <returns>The modified string.</returns>
    'Private Function RemoveNonDigitsAndZeroPad(ByVal number As String, ByVal maxLength As Integer) As String
    '    Dim cleanNumber As String = ""
    '    Dim charArray As Char() = number.ToCharArray
    '    For Each c As Char In charArray
    '        If c = "-"c Or c = "0"c Or c = "1"c Or c = "2"c Or c = "3"c Or c = "4"c Or c = "5"c Or c = "6"c Or c = "7"c Or c = "8"c Or c = "9"c Then
    '            cleanNumber = cleanNumber & c
    '        End If
    '    Next
    '    If Len(cleanNumber) < maxLength Then
    '        cleanNumber = StrDup(maxLength - Len(cleanNumber), "0") & cleanNumber
    '    End If
    '    Return cleanNumber
    'End Function

#End Region

#Region " Create Table "

    ''' <summary>
    ''' Renders the table from the <see cref="Paxiom.PXModel" />
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateTable()
        If Me.PaxiomModel IsNot Nothing AndAlso Me.PaxiomModel.IsComplete Then

            'Clear the table
            Me.DataTable.Rows.Clear()

            'Check if we need to transform the <see cref="Paxiom.PXModel" />
            Me.CheckTransformTable()

            'Create a new DataFormatter with the current paxiom model
            Me._formatter = New DataFormatter(Me.PaxiomModel)

            'Set the options on the dataformatter
            Me._formatter.InformationLevel = Me.Marker.InformationLevel
            Me._formatter.DataNotePlacment = Me.Marker.DataNotePlacement
            Me._formatter.ZeroOption = Me.Marker.RemoveRowsOption

            If Marker.TableTransformation = TableTransformationType.Sort Then
                Me._formatter.ThousandSeparator = ""
            End If

            'reset is cropped
            _isCropped = False

            'Calculate the tablemeta need to create the table
            Dim tableMeta As ColumnAndRowMeta = Me.CalculateRowAndColumnMeta()

            'Create all the tables headers
            Dim tableHeaders() As String = Me.CreateHeading(tableMeta)

            'Create the table rows
            Me.CreateRows(tableMeta, tableHeaders)

            'Create the footer
            Me.CreateFooter(tableMeta)

            'Display default attributes of the table
            Me.DisplayDefaultAttributes()

            If _isCropped Then
                Marker.OnPxTableCroppedEvent(New EventArgs())
            End If
        End If

    End Sub

    ''' <summary>
    ''' Creates all the header for the table
    ''' </summary>
    ''' <param name="tableMeta">The <see cref="ColumnAndRowMeta" /> to use to create the headers</param>
    ''' <returns>A array of the string of the ids fot the headers, Used to support accessibility for the table</returns>
    ''' <remarks></remarks>
    Private Function CreateHeading(ByVal tableMeta As ColumnAndRowMeta) As String()

        'Create Title Header
        Dim headerRow As New TableHeaderRow()
        Dim headerCell As New TableHeaderCell()

        'Number of times to add all values for a variable, default to 1 for first header row
        Dim repetitionsCurrentHeaderLevel As Integer = 1

        'Number of columns that a cell should span, default to 1
        Dim columnSpan As Integer = 1

        'Style
        headerCell.CssClass = CSS_TITLE
        headerCell.ColumnSpan = tableMeta.Columns

        If Marker.TitleVisible Then
            'Set Title
            If Me.PaxiomModel.Meta.DescriptionDefault AndAlso Not String.IsNullOrEmpty(Me.PaxiomModel.Meta.Description) Then
                headerCell.Text = Me.PaxiomModel.Meta.Description
            Else
                headerCell.Text = Me.PaxiomModel.Meta.Title
            End If
        End If

        'Set Title Header
        headerRow.TableSection = TableRowSection.TableHeader
        headerRow.Cells.Add(headerCell)
        DataTable.Rows.Add(headerRow)


        'Add Summary(Accessibility)
        DataTable.Attributes.Add("summary", Me.PaxiomModel.Meta.Title)

        'Create stringbuilders to store header ids for accessibility
        Dim columnHeadersBuilder((tableMeta.Columns - tableMeta.ColumnOffset) - 1) As StringBuilder


        Dim columnHeaders((tableMeta.Columns - tableMeta.ColumnOffset) - 1) As String
        For i As Integer = 0 To columnHeaders.Length - 1
            'Initialize each stringbuilder 
            columnHeadersBuilder(i) = New StringBuilder(tableMeta.RowOffset * HEADER_CODE_LENGTH)
        Next


        'Create Variable Header
        headerRow = New TableHeaderRow()
        headerRow.TableSection = TableRowSection.TableHeader
        'If we have any variables in the stub create a empty cell at top 
        'left corner of the table that spans all stub columns
        If Me.PaxiomModel.Meta.Stub.Count > 0 Then
            Dim emptyCell As TableCell = Me.CreateEmptyCell(Me.PaxiomModel.Meta.Heading.Count, tableMeta.ColumnOffset)
            emptyCell.CssClass = CSS_HEADER_FIRST
            headerRow.Cells.Add(emptyCell)
        End If

        'If no variables in the Header, create a empty column
        If Me.PaxiomModel.Meta.Heading.Count <= 0 Then
            headerRow.Cells.Add(Me.CreateEmptyCell(Me.PaxiomModel.Meta.Heading.Count, 1))
            DataTable.Rows.Add(headerRow)
            columnHeaders(0) = String.Empty
        Else
            'Otherwise calculate columnspan start value
            columnSpan = tableMeta.Columns - tableMeta.ColumnOffset
            Dim headingValue As Value
            Dim currColumnSpan As Integer
            Dim totColumnSpan As Integer
            Dim exitLevel As Boolean
            ' loop trough all the variables in the header
            For idxHeadingLevel As Integer = 0 To Me.PaxiomModel.Meta.Heading.Count - 1
                Dim headerOffset As Integer = 0
                'Set row to the tableheader section

                'Set the column span for the header cells for the current row                
                columnSpan = CInt(columnSpan / Me.PaxiomModel.Meta.Heading(idxHeadingLevel).Values.Count)

                currColumnSpan = columnSpan
                totColumnSpan = 0
                exitLevel = False
                'Repeat for number of times in repetion, first time only once
                For idxRepetitionCurrentHeadingLevel As Integer = 1 To repetitionsCurrentHeaderLevel
                    For idxHeadingValue As Integer = 0 To PaxiomModel.Meta.Heading(idxHeadingLevel).Values.Count - 1
                        If (totColumnSpan + currColumnSpan) >= Marker.MaxColumns Then
                            currColumnSpan = Math.Abs(totColumnSpan - Marker.MaxColumns)
                            exitLevel = True
                        Else
                            totColumnSpan += columnSpan
                        End If
                        headingValue = Me.PaxiomModel.Meta.Heading(idxHeadingLevel).Values(idxHeadingValue)

                        'If (val.Selected) Then
                        Dim columnHeaderCell As TableHeaderCell = CreateColumnHeaderCell(Me.PaxiomModel.Meta.Heading(idxHeadingLevel).Code, headingValue, currColumnSpan, idxRepetitionCurrentHeadingLevel, repetitionsCurrentHeaderLevel)
                        'Process cell for styles
                        Me.StyleColumnHeaderCell(columnHeaderCell, idxHeadingLevel, Me.PaxiomModel.Meta.Heading.Count - 1)
                        headerRow.Cells.Add(columnHeaderCell)
                        'Set column header id's for all columns in the current columnspan + header offset
                        For j As Integer = 0 To columnSpan - 1
                            columnHeadersBuilder(j + headerOffset).AppendFormat("{0} ", columnHeaderCell.Attributes("id"))
                        Next j

                        headerOffset += columnSpan
                        If exitLevel Then Exit For
                    Next idxHeadingValue
                    If exitLevel Then Exit For
                Next idxRepetitionCurrentHeadingLevel
                If exitLevel Then _isCropped = True

                DataTable.Rows.Add(headerRow)

                'Set repetiton for the next header variable
                repetitionsCurrentHeaderLevel *= Me.PaxiomModel.Meta.Heading(idxHeadingLevel).Values.Count

                headerRow = New TableHeaderRow()
                headerRow.TableSection = TableRowSection.TableHeader
            Next idxHeadingLevel
        End If

        For index As Integer = 0 To columnHeaders.Count - 1 Step 1
            columnHeaders(index) = columnHeadersBuilder(index).ToString()
        Next index
        Return columnHeaders
    End Function



    ''' <summary>
    ''' Creates ColumnHeaderCell with content
    ''' </summary>
    ''' <param name="variableCode">current Variable.Code</param>
    ''' <param name="val">current Variable.Values(i)</param>
    ''' <param name="columnSpan">tableMeta.Columns - tableMeta.ColumnOffset</param>
    ''' <param name="rep">Number of times all values for a variable are added</param>
    ''' <param name="repetition">Number of times to add all values for a variable</param>
    ''' <returns>TableHeaderCell</returns>
    ''' <remarks>Functionality moved from CreateHeading() and extended with functionality for HeaderPresentationType</remarks>
    Private Function CreateColumnHeaderCell(ByVal variableCode As String, ByVal val As Paxiom.Value, ByVal columnSpan As Integer, ByVal rep As Integer, ByVal repetition As Integer) As TableHeaderCell
        Dim columnHeaderCell As New TableHeaderCell()
        Dim selectedPresentation As HeaderPresentationType = HeaderPresentationType.Text

        If Not String.IsNullOrEmpty(variableCode) Then
            If Marker.VariablePresentationAlternative.ContainsKey(variableCode) Then
                selectedPresentation = Marker.VariablePresentationAlternative(variableCode)
            End If
        End If

        With columnHeaderCell
            .ColumnSpan = columnSpan
            '.Text = Me.ProcessValue(val.Value + " , " + val.Code)
            Select Case selectedPresentation
                Case HeaderPresentationType.Text
                    .Text = Me.ProcessValue(val.Text)
                Case HeaderPresentationType.Code
                    .Text = Me.ProcessValue(val.Code)
                Case HeaderPresentationType.CodeAndText
                    .Text = Me.ProcessValue(val.Code + " " + val.Value)
            End Select
            'Must create id like this so that asp.net don't change the id
            .Attributes.Add("id", String.Format(CultureInfo.InvariantCulture, "H{0}{1}{2}", repetition, val.Code, rep))
            .Attributes.Add("scope", "col")
        End With

        Return columnHeaderCell
    End Function




    ''' <summary>
    ''' Creates an empty <see cref="TableCell" />
    ''' </summary>
    ''' <param name="rowSpan">The number of rows the <see cref="TableCell" /> should span</param>
    ''' <param name="columnSpan">The number of columns the <see cref="TableCell" /> should span</param>
    ''' <returns>A instance of <see cref="TableCell" />that is empty</returns>
    ''' <remarks></remarks>
    Private Function CreateEmptyCell(ByVal rowSpan As Integer, ByVal columnSpan As Integer) As TableCell
        Dim emptyCell As New TableCell()

        With emptyCell
            .RowSpan = rowSpan
            .ColumnSpan = columnSpan
            .Text = EMPTY_CELL_VALUE
        End With

        Return emptyCell
    End Function

    ''' <summary>
    ''' Creates all the rows in the table
    ''' </summary>
    ''' <param name="tableMeta">An instance of <see cref="ColumnAndRowMeta" /> with the meta to use to create the table</param>
    ''' <param name="tableHeaders">The array of ids to use for accessibility</param>
    ''' <remarks></remarks>
    Private Sub CreateRows(ByVal tableMeta As ColumnAndRowMeta, ByVal tableHeaders() As String)

        _rowCount = 0
        If (Me.PaxiomModel.Meta.Stub.Count > 0) Then
            'If we have at least on variable in the stub begin the recursive calls to create all the rows, beginning with the first stub and dataindex
            'and supplying a stringbuilder to add row header ids for accessibility
            Me.CreateRow(0, 0, tableMeta, tableHeaders, New StringBuilder(tableMeta.ColumnOffset * HEADER_CODE_LENGTH), Nothing, tableMeta.Rows - tableMeta.RowOffset, Nothing)

            'If ths table is in sort mode, sort the table after all the rows have been created
            If Marker.TableTransformation = TableTransformationType.Sort Then
                'If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                '    SortTableWithJQuery(DataTable)
                'Else
                SortTable(DataTable)
                'End If
            End If
        Else
            'If there are no variables in ths stub, there is only on large row with all the data
            Dim row As New TableRow()
            'Fill the one row with all the data
            Me.FillData(tableHeaders, String.Empty, tableMeta, row, 0)
            DataTable.Rows.Add(row)
        End If

    End Sub

    ''' <summary>
    ''' Creates a row for the table
    ''' </summary>
    ''' <param name="index">The index of the stub to use to get values</param>
    ''' <param name="dataIndex">The index of data to use to fill</param>
    ''' <param name="tableMeta">The <see cref="ColumnAndRowMeta" /> to use</param>
    ''' <param name="columnHeaders">he array of ids to use for accessibility</param>
    ''' <param name="rowHeaders">A <see cref="StringBuilder" /> to use to add row header ids for accessibility</param>
    ''' <param name="currentRow">The current row to append <see cref="TableCell" /> to</param>
    ''' <param name="rowSpan">The rowspan to use for the cells</param>
    ''' <param name="hierarchy">The current level in the hierarchy, can be null if no hierarchy is used</param>
    ''' <returns>The current dataindex to use</returns>
    ''' <remarks>This methods calls itself recursively to create rows</remarks>
    Private Function CreateRow(ByVal index As Integer, ByVal dataIndex As Integer, ByVal tableMeta As ColumnAndRowMeta, ByVal columnHeaders() As String, ByVal rowHeaders As StringBuilder, ByVal currentRow As TableRow, ByVal rowSpan As Integer, ByVal hierarchy As Nullable(Of Integer)) As Integer

        'Calculate the rowspan for all the cells to add in this call
        rowSpan = CInt((rowSpan) / Me.PaxiomModel.Meta.Stub(index).Values.Count)

        'Loop through all the values in the stub variable
        Dim val As Value
        For i As Integer = 0 To Me.PaxiomModel.Meta.Stub(index).Values.Count - 1
            val = Me.PaxiomModel.Meta.Stub(index).Values(i)
            Dim adjustedRowSpan As Integer

            'If this is a new row
            If (currentRow Is Nothing) Then
                currentRow = New TableRow()
                currentRow.EnableViewState = False
            End If

            'Used to remove rows that contain any data if set by the user
            Select Case Me.Marker.Layout
                Case TableLayoutType.Layout1
                    'Fix the rowspan
                    If rowSpan = 0 Then
                        rowSpan = 1
                    End If

                    'Calculate the number of zero rows, if rowpans and zero rows match this row should not be visible
                    If CalculateZeroRows(dataIndex, (dataIndex + rowSpan)) = rowSpan Then
                        dataIndex += rowSpan
                        Continue For
                    End If
                    adjustedRowSpan = 1

                Case TableLayoutType.Layout2
                    'Calculate the number of zero rows
                    Dim calculateRows As Integer = CalculateZeroRows(dataIndex, rowSpan + dataIndex)
                    'Adjust the rowspan
                    adjustedRowSpan = rowSpan - calculateRows


                    'If we get an zero as adjusted rowspan this row should not be visible
                    If adjustedRowSpan = 0 Then
                        dataIndex += rowSpan
                        Continue For
                    End If
            End Select


            'If we are using a hierarchy
            If (Me.PaxiomModel.Meta.Stub(index).Hierarchy.IsHierarchy) AndAlso tableMeta.UseHierarchy Then
                'Set the level in the hierarchy for the value
                hierarchy = _hierarchyLevelMap(val.Code)
            End If

            'Set the current rows section to body
            currentRow.TableSection = TableRowSection.TableBody

            'Create a new rowheader for the row
            'Dim currentHeaderCell As TableHeaderCell = Me.CreateRowHeaderCell(val, dataIndex, adjustedRowSpan)
            Dim currentHeaderCell As TableHeaderCell = Me.CreateRowHeaderCell(Me.PaxiomModel.Meta.Stub(index).Code, val, dataIndex, adjustedRowSpan)

            'Add the row header to the row
            currentRow.Cells.Add(currentHeaderCell)

            'Style the row ehader
            Me.StyleRowHeaderCell(currentHeaderCell, index, hierarchy)

            'Get the length of the row headers id
            Dim length As Integer = currentHeaderCell.Attributes("id").Length + 1

            'At the row headers is to rowHeaders string builder for accessibility
            rowHeaders.AppendFormat("{0} ", currentHeaderCell.Attributes("id"))

            'If there are more stub variables that need to add headers to this row
            If (Me.PaxiomModel.Meta.Stub.Count > index + 1) Then

                Select Case Me.Marker.Layout
                    Case TableLayoutType.Layout1
                        _rowCount += 1
                        If _rowCount <= Marker.MaxRows Then
                            'If layout one then make the rest of this row empty
                            FillEmpty(tableMeta, currentRow)

                            'Add the row to the table
                            DataTable.Rows.Add(currentRow)
                        Else
                            _isCropped = True
                        End If
                        'Create a new row for the next stub
                        dataIndex = CreateRow(index + 1, dataIndex, tableMeta, columnHeaders, rowHeaders, Nothing, rowSpan, hierarchy)
                    Case TableLayoutType.Layout2
                        'If layout two then add the next stub to the current row
                        dataIndex = CreateRow(index + 1, dataIndex, tableMeta, columnHeaders, rowHeaders, currentRow, rowSpan, hierarchy)
                End Select
                'Set the curret row to nothing
                currentRow = Nothing
            Else
                _rowCount += 1
                If Me.Marker.TableTransformation <> TableTransformationType.Sort Then
                    If _rowCount <= Marker.MaxRows Then
                        'If no more stubs need to add headers then fill the row with data
                        Me.FillData(columnHeaders, rowHeaders.ToString(), tableMeta, currentRow, dataIndex)

                        'Add the complete row to the table
                        DataTable.Rows.Add(currentRow)
                    Else
                        _isCropped = True
                    End If
                Else
                    'Table shall not be cropped when in sort-mode

                    'If no more stubs need to add headers then fill the row with data
                    Me.FillData(columnHeaders, rowHeaders.ToString(), tableMeta, currentRow, dataIndex)

                    'Add the complete row to the table
                    DataTable.Rows.Add(currentRow)
                End If
                'Increase the dataindex
                dataIndex += 1

                'Set the row to nothing so that a new gets created
                currentRow = Nothing
            End If

            'Remove the last header so the ids match for accessibility
            rowHeaders.Remove(rowHeaders.Length - length, length)

            'If _isCropped Then Exit For 'ToDo: optimization check if ok. 
        Next
        Return dataIndex
    End Function

    ''' <summary>
    ''' Create a row header
    ''' </summary>
    ''' <param name="val">The <see cref="Value" /> to use to create the row header</param>
    ''' <param name="dataIndex">The dataindex of use to create the id</param>
    ''' <param name="rowSpan">The rowspan for the row header</param>
    ''' <returns>An instance of <see cref="TableHeaderCell" /></returns>
    ''' <remarks></remarks>
    Private Function CreateRowHeaderCell(ByVal val As Value, ByVal dataIndex As Integer, ByVal rowSpan As Integer) As TableHeaderCell
        Return CreateRowHeaderCell("", val, dataIndex, rowSpan)
    End Function


    ''' <summary>
    ''' Create a row header
    ''' </summary>
    ''' <param name="variableCode">Current Variable.Code</param>
    ''' <param name="val">The <see cref="Value" /> to use to create the row header</param>
    ''' <param name="dataIndex">The dataindex of use to create the id</param>
    ''' <param name="rowSpan">The rowspan for the row header</param>
    ''' <returns>An instance of <see cref="TableHeaderCell" /></returns>
    ''' <remarks>Extends original function CreateRowHeaderCell() with HeaderPresentationType</remarks>
    Private Function CreateRowHeaderCell(ByVal variableCode As String, ByVal val As Value, ByVal dataIndex As Integer, ByVal rowSpan As Integer) As TableHeaderCell
        Dim rowHeaderCell As New TableHeaderCell()
        Dim selectedPresentation As HeaderPresentationType = HeaderPresentationType.Text

        If Not String.IsNullOrEmpty(variableCode) Then
            If Marker.VariablePresentationAlternative.ContainsKey(variableCode) Then
                selectedPresentation = Marker.VariablePresentationAlternative(variableCode)
            End If
        End If

        With rowHeaderCell
            Select Case selectedPresentation
                Case HeaderPresentationType.Text
                    .Text = Me.ProcessValue(val.Text)
                Case HeaderPresentationType.Code
                    .Text = Me.ProcessValue(val.Code)
                Case HeaderPresentationType.CodeAndText
                    .Text = Me.ProcessValue(val.Code + " " + val.Value)
            End Select

            '.Text = Me.ProcessValue(val.Value) '.Text = Me.ProcessValue(val.Value + " ," + val.Code)
            .Scope = TableHeaderScope.Row
            'Must create id like this so that asp.net don't change the id
            .Attributes.Add("id", String.Format(CultureInfo.InvariantCulture, "R{0}{1}", _regex.Replace(val.Code, "_"), dataIndex))
            .RowSpan = rowSpan
        End With

        Return rowHeaderCell
    End Function


    ''' <summary>
    ''' Creates the footer for the table
    ''' </summary>
    ''' <param name="tableMeta">The <see cref="ColumnAndRowMeta" /> to use to create the footer</param>
    ''' <remarks></remarks>
    Private Sub CreateFooter(ByVal tableMeta As ColumnAndRowMeta)

    End Sub

    ''' <summary>
    ''' Displays the default attributes beneath the table
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayDefaultAttributes()
        Dim lst As List(Of KeyValuePair(Of String, String))

        If Marker.DisplayDefaultAttributes Then
            lst = Me.PaxiomModel.Meta.Attributes.GetDefaultAttributes()

            If lst.Count > 0 Then
                Me.lblDefaultAttributes.Text = GetLocalizedString(DEFAULTATTRIBUTES_LABEL)
                Me.rptDefaultAttributes.DataSource = Me.PaxiomModel.Meta.Attributes.GetDefaultAttributes()
                Me.rptDefaultAttributes.DataBind()
            Else
                pcaxis_table_defaultattributes.Visible = False
            End If
        Else
            pcaxis_table_defaultattributes.Visible = False
        End If
    End Sub

#End Region

#Region " Style "

    ''' <summary>
    ''' Styles a columnheader
    ''' </summary>
    ''' <param name="columnHeaderCell">The <see cref="TableHeaderCell" /> to style</param>
    ''' <param name="currentLevel">The current level of the column header</param>
    ''' <param name="totalLevels">The number of levels of columns</param>
    ''' <remarks></remarks>
    Private Sub StyleColumnHeaderCell(ByVal columnHeaderCell As TableHeaderCell, ByVal currentLevel As Integer, ByVal totalLevels As Integer)
        'If this is the first level
        If (currentLevel = 0) Then
            columnHeaderCell.CssClass = CSS_HEADER_FIRST
        ElseIf currentLevel = totalLevels Then
            'If this is the last level
            columnHeaderCell.CssClass = CSS_HEADER_LAST
        Else
            'If this is a level in between
            columnHeaderCell.CssClass = CSS_HEADER_MIDDLE
        End If
    End Sub

    ''' <summary>
    ''' Styles a row header
    ''' </summary>
    ''' <param name="rowHeaderCell">The <see cref="TableHeaderCell" /> to style</param>
    ''' <param name="currentLevel">The current level of the row header</param>
    ''' <param name="hierarchy">The level of the hierarchy if using a hierarchy</param>
    ''' <remarks></remarks>
    Private Sub StyleRowHeaderCell(ByVal rowHeaderCell As TableHeaderCell, ByVal currentLevel As Integer, ByVal hierarchy As Nullable(Of Integer))
        Dim tableRowStyle As String = CSS_STUB

        Select Case Me.Marker.Layout
            Case TableLayoutType.Layout1
                tableRowStyle = LAYOUT1_PREFIX + CSS_STUB
            Case Else
                tableRowStyle = LAYOUT2_PREFIX + CSS_STUB
        End Select

        'If we are using a hierarchy
        If hierarchy.HasValue Then
            'Use a css class with a hierarchy
            rowHeaderCell.CssClass = String.Format(CultureInfo.InvariantCulture, "{0} {0}{1}h{2}", tableRowStyle, currentLevel + 1, hierarchy)
        Else
            'Use a css class with only the level
            rowHeaderCell.CssClass = String.Format(CultureInfo.InvariantCulture, "{0} {0}{1}", tableRowStyle, currentLevel + 1)
        End If
    End Sub

    ''' <summary>
    ''' Styles a datacell 
    ''' </summary>
    ''' <param name="dataCell">The <see cref="TableCell" /> to style</param>
    ''' <param name="row">Index of the cell row</param>
    ''' <param name="column">Index of the cell column</param>
    ''' <param name="hasAttributesOrNotes">If the cell has cell attributes or not</param>
    ''' <remarks></remarks>
    Private Sub StyleDataCell(ByVal dataCell As TableCell, ByVal row As Integer, ByVal column As Integer, ByVal hasAttributesOrNotes As Boolean)
        If hasAttributesOrNotes Then
            dataCell.CssClass = CSS_DATA_FILLED & " " & CSS_ATTRIBUTE_CELL
            'dataCell.Attributes.Add("onclick", "javascript:displayCellInformation('" & row.ToString() & "_" & column.ToString() & "');")
            dataCell.Attributes.Add("data-value", row.ToString() & "_" & column.ToString())


            If Marker.DisplayCellInformationWithoutJavascript Then
                Dim paramChar As String
                If Request.RawUrl.Contains("?") Then
                    paramChar = "&"
                Else
                    paramChar = "?"
                End If

                Dim anchor As New HyperLink
                anchor.Text = dataCell.Text
                anchor.NavigateUrl = Request.RawUrl & paramChar & "cellid=" & row.ToString() & "_" & column.ToString()
                anchor.Target = "_blank"
                dataCell.Text = ""
                dataCell.Controls.Add(anchor)
            End If
        Else
            dataCell.CssClass = CSS_DATA_FILLED
        End If
    End Sub

#End Region

#Region " Data "
    ''' <summary>
    ''' Fills a row with data cells
    ''' </summary>
    ''' <param name="columnHeaders">The array of column header ids to use for accessibility</param>
    ''' <param name="rowHeaders">The array of row header ids to use for accessibility</param>
    ''' <param name="tableMeta">An instance of <see cref="ColumnAndRowMeta" /> to use to create the datacell</param>
    ''' <param name="row">The <see cref="TableRow" /> to add the datacells to</param>
    ''' <param name="dataIndex">The index of the data to use</param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal columnHeaders() As String, ByVal rowHeaders As String, ByVal tableMeta As ColumnAndRowMeta, ByVal row As TableRow, ByVal dataIndex As Integer)
        Dim nrCols As Integer = tableMeta.Columns - (tableMeta.ColumnOffset)
        If nrCols > Marker.MaxColumns Then nrCols = Marker.MaxColumns

        'Loop through cells that need to be added to the row
        For index As Integer = 0 To nrCols - 1

            Dim dataCell As New TableCell()
            Dim hasAttributesOrNotes As Boolean = False

            With dataCell
                'Add the headers for accessibility
                .Attributes.Add("headers", rowHeaders + columnHeaders(index).Trim())
                'Get the data from the dataformatter
                .Text = Me._formatter.ReadElement(dataIndex, index)
            End With

            If Marker.DisplayCellInformation Then
                hasAttributesOrNotes = Me._formatter.HasCellAttributesOrNotes(dataIndex, index)
            End If

            'Style the cell
            Me.StyleDataCell(dataCell, dataIndex, index, hasAttributesOrNotes)
            row.Cells.Add(dataCell)
        Next index

    End Sub

    ''' <summary>
    ''' Fills an row with empty cells
    ''' </summary>
    ''' <param name="tableMeta">An instance of <see cref="ColumnAndRowMeta" /> to use to create the empty cells</param>
    ''' <param name="row">The row to fill</param>
    ''' <remarks></remarks>
    Private Sub FillEmpty(ByVal tableMeta As ColumnAndRowMeta, ByVal row As TableRow)
        'Loop through cells that need to be added to the row
        Dim maxCols As Integer = (tableMeta.Columns - tableMeta.ColumnOffset)
        If maxCols > Marker.MaxColumns Then
            maxCols = Marker.MaxColumns
            _isCropped = True
        End If

        For index As Integer = 0 To maxCols - 1
            row.Cells.Add(Me.CreateEmptyCell(1, 1))
        Next index
    End Sub

    ''' <summary>
    ''' Calculates how many rows that have no data in them in an interval
    ''' </summary>
    ''' <param name="startIndex">The begining of the interval</param>
    ''' <param name="endIndex">The end of the interval</param>
    ''' <returns>The number of zero rows</returns>
    ''' <remarks></remarks>
    Private Function CalculateZeroRows(ByVal startIndex As Integer, ByVal endIndex As Integer) As Integer
        Dim count As Integer = 0
        For i As Integer = startIndex To endIndex - 1
            If _formatter.IsZeroRow(i) Then
                count += 1
            End If
        Next

        Return count
    End Function

    ''' <summary>
    ''' Process a value and makes the first letter uppercase if configured to do so
    ''' </summary>
    ''' <param name="value">The value to process</param>
    ''' <returns>The processed value</returns>
    ''' <remarks></remarks>
    Private Function ProcessValue(ByVal value As String) As String
        If value.Length > 0 Then
            If Me.Marker.UseUpperCase Then
                If Char.IsLower(value(0)) AndAlso Not (Char.IsDigit(value(0))) Then
                    value = Char.ToUpperInvariant(value(0)) + value.Substring(1, value.Length - 1)
                End If

            End If
        End If
        Return value
    End Function
#End Region

#Region " TableEvents "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not QuerystringManager.GetQuerystringParameter("cellid") Is Nothing Then
            DisplayCellInformation()
        End If

        If Not IsPostBack Then
            pxtableCellInformationDialog.Attributes.Add("title", GetLocalizedString(CELLINFORMATION_DIALOGTITLE))
            pxtableCellInformationDialogCloseText.Value = GetLocalizedString(CELLINFORMATION_CLOSEBUTTON)
        End If
    End Sub

    ''' <summary>
    ''' Used to begin the creation to the table
    ''' </summary>
    ''' <param name="sender">The source of the event</param>
    ''' <param name="e">An <see cref="EventArgs" /> that contains no event data</param>
    ''' <remarks>The table is only rendered here to avoid rendering the table many times</remarks>
    Private Sub Table_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me.DataTable.Rows.Count = 0 AndAlso Me.PaxiomModel IsNot Nothing Then
            Me.CreateTable()
        End If
    End Sub

#End Region

#Region " Internal Structures "
    ''' <summary>
    ''' Used to store metadata about the table
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ColumnAndRowMeta
        Private _rows As Integer
        ''' <summary>
        ''' Gets or set the number of rows in the table
        ''' </summary>
        ''' <value>Number of rows in the table</value>
        ''' <returns>The number of rows in the table</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Rows() As Integer
            Get
                Return _rows
            End Get
        End Property

        Private _columns As Integer
        ''' <summary>
        ''' Gets  the number of columns in the table
        ''' </summary>
        ''' <value>Number of columns in the table</value>
        ''' <returns>The number of columns in the table</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Columns() As Integer
            Get
                Return _columns
            End Get

        End Property
        Private _rowOffset As Integer

        ''' <summary>
        ''' Gets  the number of rows that contains headers
        ''' </summary>
        ''' <value>Number of rows that contains headers</value>
        ''' <returns>The number of rows that contains headers</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RowOffset() As Integer
            Get
                Return _rowOffset
            End Get
        End Property

        Private _columnOffset As Integer
        ''' <summary>
        ''' Gets the number of columns that contains headers
        ''' </summary>
        ''' <value>Number of columns that contains headers</value>
        ''' <returns>The number of columns that contains headers</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ColumnOffset() As Integer
            Get
                Return _columnOffset
            End Get
        End Property
        Private _useHierarchy As Boolean
        ''' <summary>
        ''' Gets if the table uses hierarchy
        ''' </summary>
        ''' <value>If the table uses hierarchy</value>
        ''' <returns>If the table uses hierarchy</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UseHierarchy() As Boolean
            Get
                Return _useHierarchy
            End Get
        End Property

        ''' <summary>
        ''' Initialize a new <see cref="ColumnAndRowMeta" />
        ''' </summary>
        ''' <param name="rows">The number of rows</param>
        ''' <param name="columns">The number of columns</param>
        ''' <param name="columnOffset">The number of columns that contains headers</param>
        ''' <param name="rowOffset">The number of rows that contains headers</param>
        ''' <param name="useHierarchy">Whether the table uses hierarchy or not</param>
        ''' <remarks></remarks>
        Sub New(ByVal rows As Integer, ByVal columns As Integer, ByVal columnOffset As Integer, ByVal rowOffset As Integer, ByVal useHierarchy As Boolean)
            Me._columns = columns
            Me._rows = rows
            Me._columnOffset = columnOffset
            Me._rowOffset = rowOffset
            Me._useHierarchy = useHierarchy
        End Sub
    End Class
#End Region

    ''' <summary>
    ''' Checks if the table needs to be tranformed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckTransformTable()
        Dim pivotDescriptionList As List(Of Paxiom.Operations.PivotDescription) = Nothing
        Dim pivotOperation As Paxiom.Operations.Pivot = Nothing

        'If the time variable should be on top if it's the only variable and the only variable is a time variable
        If Marker.TimeOnTopIfSingle = True AndAlso Me.PaxiomModel.Meta.Variables.Count = 1 AndAlso Me.PaxiomModel.Meta.Variables(0).HasTimeValue Then
            'Pivot the table to that variable isn't in the headers
            pivotOperation = New Paxiom.Operations.Pivot
            pivotDescriptionList = New List(Of Paxiom.Operations.PivotDescription)

            pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Me.PaxiomModel.Meta.Variables(0).Name, PlacementType.Heading))

        Else
            Select Case Me.Marker.TableTransformation
                'Do nothing
                Case TableTransformationType.NoTransformation

                    'Pivot the table for sort
                Case TableTransformationType.Sort

                    pivotDescriptionList = New List(Of Paxiom.Operations.PivotDescription)()
                    pivotOperation = New Paxiom.Operations.Pivot()
                    For Each var As Variable In PaxiomModel.Meta.Variables
                        pivotDescriptionList.Add(New PCAxis.Paxiom.Operations.PivotDescription(var.Name, PCAxis.Paxiom.PlacementType.Stub))
                    Next

                    'Pivot the table so variables with only one value comes first
                Case TableTransformationType.SingleValueFirst
                    pivotDescriptionList = New List(Of Paxiom.Operations.PivotDescription)()
                    pivotOperation = New Paxiom.Operations.Pivot()

                    For Each Val As Paxiom.Variable In Me.PaxiomModel.Meta.Variables
                        If Val.Values.Count = 1 Then
                            pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, Val.Placement))
                        End If
                    Next

                    'No need to to a pivot
                    If pivotDescriptionList.Count = 0 Then
                        pivotOperation = Nothing
                        Exit Select
                    End If

                    For Each Val As Paxiom.Variable In Me.PaxiomModel.Meta.Variables
                        If Val.Values.Count > 1 Then
                            pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, Val.Placement))
                        End If
                    Next


                    'Pivot the table so variables with only one value comes first and only on variable with multiples values are in the top
                Case TableTransformationType.SingleValueFirstAndHeaderOnlyOneMultiple
                    pivotDescriptionList = New List(Of Paxiom.Operations.PivotDescription)()
                    pivotOperation = New Paxiom.Operations.Pivot()

                    Dim topFilled As Boolean = False

                    For Each Val As Paxiom.Variable In Me.PaxiomModel.Meta.Variables
                        If Val.Values.Count = 1 Then
                            pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, Val.Placement))
                        End If
                    Next

                    'If pivotDescriptionList.Count = 0 Then
                    '    pivotOperation = Nothing
                    '    Exit Select
                    'End If

                    For Each Val As Paxiom.Variable In Me.PaxiomModel.Meta.Variables
                        If Val.Values.Count > 1 Then
                            If Val.Placement = PlacementType.Heading Then
                                If topFilled Then
                                    pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, PlacementType.Stub))
                                Else
                                    pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, Val.Placement))
                                    topFilled = True
                                End If
                            Else
                                pivotDescriptionList.Add(New Paxiom.Operations.PivotDescription(Val.Name, Val.Placement))
                            End If
                        End If
                    Next

            End Select

        End If

        'If there are any operation
        If pivotOperation IsNot Nothing Then
            Me.PaxiomModel = pivotOperation.Execute(Me.PaxiomModel, pivotDescriptionList.ToArray())
        End If


    End Sub

    ''' <summary>
    ''' Calculate the table meta for the table
    ''' Table meta has information about:
    '''  - number of columns in the table
    '''  - number of rows in the table
    '''  - the number of columns that contains headers
    '''  - the number of rows that contains headers
    ''' </summary>
    ''' <returns>An instance of <see cref="ColumnAndRowMeta" /></returns>
    ''' <remarks></remarks>
    Private Function CalculateRowAndColumnMeta() As ColumnAndRowMeta
        Dim columnCount As Integer = 1
        Dim columnOffset As Integer = 0
        Dim rowCount As Integer = 1
        Dim rowOffset As Integer = Me.PaxiomModel.Meta.Heading.Count
        Dim hierarchyCount As Integer = 0
        Dim hierarchyVariable As Variable = Nothing
        Dim useHierarchy As Boolean = False

        For Each v As Variable In Me.PaxiomModel.Meta.Heading
            columnCount *= v.Values.Count
        Next

        Select Case Me.Marker.Layout
            Case TableLayoutType.Layout1
                'Calulate the number of rows when in layout 1
                For Each var As Variable In Me.PaxiomModel.Meta.Stub
                    rowCount *= var.Values.Count
                    If var.Hierarchy.IsHierarchy Then
                        hierarchyCount += 1
                        hierarchyVariable = var
                    End If
                Next

                'rowOffset += 1
                columnOffset = 1
            Case TableLayoutType.Layout2

                rowCount = 1
                'Calcluate the number of rows when in layout 2
                For Each v As Variable In Me.PaxiomModel.Meta.Stub
                    rowCount *= v.Values.Count
                    If v.Hierarchy.IsHierarchy Then
                        hierarchyCount += 1
                        hierarchyVariable = v
                    End If
                Next
                columnOffset = Me.PaxiomModel.Meta.Stub.Count
        End Select

        rowCount += Me.PaxiomModel.Meta.Heading.Count
        columnCount += columnOffset

        'If only one hierarchy then create a hierarchy
        If hierarchyCount > 0 Then
            useHierarchy = True
            BuildHierarchyMap(hierarchyVariable)
        End If

        Return New ColumnAndRowMeta(rowCount, columnCount, columnOffset, rowOffset, useHierarchy)

    End Function

    ''' <summary>
    ''' Displays information about clicked cell in the table. 
    ''' </summary>
    ''' <remarks>
    ''' Which cell has been clicked is determined by the value of the pxtableHidCellInfo hidden field. 
    ''' When a cell has been clicked the pxtableHidCellInfo hidden field contains the cell identifyer in the 
    ''' format: rowIndex_columnIndex
    ''' Example: 3_5 = The row with index 3 and the column with index 5 has been clicked
    ''' </remarks>
    Private Sub DisplayCellInformation()
        Dim cellInfo As CellInformation

        If Not QuerystringManager.GetQuerystringParameter("cellid") Is Nothing Then
            cellInfo = GetCellInformation(QuerystringManager.GetQuerystringParameter("cellid"))
            DisplayCellInformationDialog(cellInfo)
        End If
    End Sub


    ''' <summary>
    ''' Displays information dialog about the clicked cell
    ''' </summary>
    ''' <param name="cellInfo">Cell information object</param>
    ''' <remarks></remarks>
    Private Sub DisplayCellInformationDialog(ByVal cellInfo As CellInformation)
        Response.ContentType = "text/html"
        Response.Write("<html><head><title>" & GetLocalizedString(CELLINFORMATION_DIALOGTITLE) & "</title></head><body>")
        Response.Write("<span class=""table_cellinformation_cell"">" & GetLocalizedString(CELLINFORMATION_CELL) & "</span>")
        Response.Write("<table>")
        For Each varval As KeyValuePair(Of String, String) In cellInfo.VariableValues
            Response.Write("<tr><td class=""table_cellinformation_text"">" & varval.Key & ":</td><td>&nbsp;</td><td class=""table_cellinformation_text"">" & varval.Value & "</td></tr>")
        Next
        Response.Write("</table>")

        If cellInfo.Attributes.Count > 0 Then
            Response.Write("<br />")
            Response.Write("<span class=""table_cellinformation_attributes"">" & GetLocalizedString(CELLINFORMATION_ATTRIBUTES) & "</span>")
            Response.Write("<table>")
            For Each attr As KeyValuePair(Of String, String) In cellInfo.Attributes
                Response.Write("<tr><td class=""table_cellinformation_text"">" & attr.Key & ":</td><td>&nbsp;</td><td class=""table_cellinformation_text"">" & attr.Value & "</td></tr>")
            Next
            Response.Write("</table>")
        End If

        If cellInfo.CellNotes.Count > 0 Then
            Response.Write("<br />")
            Response.Write("<span class=""table_cellinformation_notes"">" & GetLocalizedString(CELLINFORMATION_NOTES) & "</span>")
            Response.Write("<table>")
            For Each note As String In cellInfo.CellNotes
                Response.Write("<tr><td class=""table_cellinformation_text"">" & note & "</td></tr>")
            Next
            Response.Write("</table>")
        End If

        Response.Write("</body></html>")
        Response.End()
        Response.Flush()

    End Sub
    ''' <summary>
    ''' Get information about the given cell
    ''' </summary>
    ''' <param name="cellId">String identifying the cell. Format: rowIndex_columnIndex</param>
    ''' <returns>CellInformation object</returns>
    ''' <remarks></remarks>
    Private Function GetCellInformation(ByVal cellId As String) As CellInformation
        Dim cellInfo As New CellInformation
        Dim row As Integer
        Dim col As Integer
        Dim attrValues As String() = Nothing
        Dim attr As KeyValuePair(Of String, String)

        If Not GetClickedCellRowCol(cellId, row, col) Then
            Return Nothing
        End If

        If _formatter Is Nothing Then
            Me._formatter = New DataFormatter(Me.PaxiomModel)
        End If

        cellInfo.VariableValues = _formatter.GetCellVariableValues(row, col)

        'Get cell attribute values
        If _formatter.GetCellAttributes(row, col, attrValues) Then
            'There should be one value per attribute
            If Me.PaxiomModel.Meta.Attributes.Identities.Count = attrValues.Length Then
                'Get attribute names and values
                For i As Integer = 0 To Me.PaxiomModel.Meta.Attributes.Identities.Count - 1
                    attr = New KeyValuePair(Of String, String)(Me.PaxiomModel.Meta.Attributes.Names(i), attrValues(i))
                    cellInfo.Attributes.Add(attr)
                Next
            End If
        End If

        cellInfo.CellNotes = _formatter.GetCellNotes(row, col)

        Return cellInfo

    End Function

    ''' <summary>
    ''' Get row and column for the given cell
    ''' </summary>
    ''' <param name="cellId">String identifying the cell. Format: rowIndex_columnIndex</param>
    ''' <param name="row">Contains row index when function returns</param>
    ''' <param name="col">Contains column index when function returns</param>
    ''' <returns>True if row and column was successfully loaded, else false</returns>
    ''' <remarks></remarks>
    Private Function GetClickedCellRowCol(ByVal cellId As String, ByRef row As Integer, ByRef col As Integer) As Boolean
        Dim separator As String() = {"_"}
        Dim rowCol As String() = cellId.Split(separator, StringSplitOptions.RemoveEmptyEntries)

        If rowCol.Length <> 2 Then
            Return False
        End If

        If Not Integer.TryParse(rowCol(0), row) Then
            Return False
        End If

        If Not Integer.TryParse(rowCol(1), col) Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Class representing information about a clicked cell in the table
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CellInformation
        Private _attributes As List(Of KeyValuePair(Of String, String))
        Private _varvals As List(Of KeyValuePair(Of String, String))
        Private _cellNotes As List(Of String)

        Public Sub New()
            _attributes = New List(Of KeyValuePair(Of String, String))
            _varvals = New List(Of KeyValuePair(Of String, String))
            _cellNotes = New List(Of String)
        End Sub

        ''' <summary>
        ''' List of attribute name - attribute value pairs
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributes() As List(Of KeyValuePair(Of String, String))
            Get
                Return _attributes
            End Get
        End Property

        ''' <summary>
        ''' List of variable name - value name pairs identifying the cell
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VariableValues() As List(Of KeyValuePair(Of String, String))
            Get
                Return _varvals
            End Get
            Set(ByVal value As List(Of KeyValuePair(Of String, String)))
                _varvals = value
            End Set
        End Property

        ''' <summary>
        ''' Notes for the cell
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CellNotes() As List(Of String)
            Get
                Return _cellNotes
            End Get
            Set(ByVal value As List(Of String))
                _cellNotes = value
            End Set
        End Property
    End Class

End Class
