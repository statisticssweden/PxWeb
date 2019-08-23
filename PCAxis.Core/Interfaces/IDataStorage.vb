Namespace PCAxis.Paxiom
    Public Interface IDataStorage

        ReadOnly Property DataCellMatrixIsFilled As Boolean

        Function Write(ByVal buffer As Double(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer

        ReadOnly Property SupportsDataCellMatrix As Boolean

        Function WriteDataNoteCells(ByVal buffer As String(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer

        Function ReadDataCellNoteElement(ByVal row As Integer, ByVal column As Integer) As String

        Function ReadDataCellNoteElement(ByVal index As Integer) As String

        Sub WriteDataNoteCellElement(index As Integer, value As String)

        Sub WriteDataNoteCellElement(row As Integer, column As Integer, value As String)
        Sub WriteElement(ByVal index As Integer, ByVal value As Double)
        Sub IncrementElement(ByVal index As Integer, ByVal value As Double)
        Function ReadLine(ByVal lineNumber As Integer, ByVal buffer As Double()) As Integer
        Function ReadElement(ByVal row As Integer, ByVal column As Integer) As Double
        Function ReadElement(ByVal index As Integer) As Double

        Sub WriteElement(ByVal row As Integer, ByVal column As Integer, ByVal value As Double)
        Sub IncrementElement(ByVal row As Integer, ByVal column As Integer, ByVal value As Double)

        Function CreateCopy() As IDataStorage

        ReadOnly Property MatrixRowCount() As Integer
        ReadOnly Property MatrixColumnCount() As Integer
        ReadOnly Property MatrixSize() As Integer
        Property CurrentIndex() As Integer



    End Interface

End Namespace
