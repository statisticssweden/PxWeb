Namespace PCAxis.Paxiom

    Public Class PxMemoryDatastorage
        Implements IDataStorage


        Private _matrix As Double()
        Private _dataNoteCellMatrix As String()
        Private _currentIndex As Integer
        Private _rowSize As Integer
        Private _rowCount As Integer
        Private _matrixSize As Integer
        Private _writeDataNoteCellsHasBeenUsed As Boolean = False
        Private _supportsDataCellMatrix As Boolean


        Public Sub New(rowCount As Integer, rowSize As Integer)
            SetMatrixSize(rowCount, rowSize)
            _currentIndex = 0
        End Sub

        Public Sub SetMatrixSize(ByVal rowCount As Integer, ByVal rowSize As Integer)
            ReDim _matrix((rowCount * rowSize) - 1)
            ReDim _dataNoteCellMatrix((rowCount * rowSize) - 1)
            Me._rowSize = rowSize
            Me._rowCount = rowCount
            Me._matrixSize = Me._rowSize * Me._rowCount

            _writeDataNoteCellsHasBeenUsed = False
        End Sub

        Public Sub IncrementElement(index As Integer, value As Double) Implements IDataStorage.IncrementElement
            _matrix(index) = PXData.AddElements(_matrix(index), value)
        End Sub

        Public Sub IncrementElement(row As Integer, column As Integer, value As Double) Implements IDataStorage.IncrementElement
            If column >= _rowSize Or row > _rowCount Then
                Throw New IndexOutOfRangeException()
            End If

            Dim index As Integer = row * Me._rowSize + column

            _matrix(index) = PXData.AddElements(_matrix(index), value)
        End Sub

        Public Function ReadElement(index As Integer) As Double Implements IDataStorage.ReadElement
            Return _matrix(index)
        End Function

        Public Function ReadDataCellNoteElement(index As Integer) As String Implements IDataStorage.ReadDataCellNoteElement
            Return _dataNoteCellMatrix(index)
        End Function

        Public Function ReadElement(row As Integer, column As Integer) As Double Implements IDataStorage.ReadElement
            If column >= _rowSize Or row > _rowCount Then
                Throw New IndexOutOfRangeException()
            End If

            Dim index As Integer = row * Me._rowSize + column

            Return _matrix(index)
        End Function

        Function ReadDataCellNoteElement(ByVal row As Integer, ByVal column As Integer) As String Implements IDataStorage.ReadDataCellNoteElement
            If column >= _rowSize Or row > _rowCount Then
                Throw New IndexOutOfRangeException()
            End If

            Dim index As Integer = row * Me._rowSize + column

            Return _dataNoteCellMatrix(index)
        End Function

        Public Function ReadLine(lineNumber As Integer, buffer() As Double) As Integer Implements IDataStorage.ReadLine
            'Checks that the line numbr is within the bounds 
            If lineNumber < 0 Or lineNumber > _rowCount Then
                Throw New IndexOutOfRangeException("lineNumber differs the row count of the data matrix")
            End If

            'Checks that the destination buffer has the right size
            If buffer.Length < _rowSize Then
                'TODO Consider throw exception
                Return 0
            End If

            'Copys the data to the buffer
            System.Array.Copy(_matrix, _rowSize * lineNumber, buffer, 0, _rowSize)

            Return _rowSize
        End Function

        Private _currentIndexBeforeLastWrite As Integer = 0

        Public Function Write(buffer() As Double, startIndex As Integer, stopIndex As Integer) As Integer Implements IDataStorage.Write
            _currentIndexBeforeLastWrite = _currentIndex
            Dim length As Integer = stopIndex - startIndex + 1
            System.Array.Copy(buffer, startIndex, _matrix, _currentIndex, length)
            _currentIndex += length

            Return length
        End Function

        Public Function WriteDataNoteCells(ByVal buffer As String(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer Implements IDataStorage.WriteDataNoteCells
            Dim length As Integer = stopIndex - startIndex + 1
            System.Array.Copy(buffer, startIndex, _dataNoteCellMatrix, _currentIndexBeforeLastWrite, length)
            _writeDataNoteCellsHasBeenUsed = True

            Return length
        End Function

        Public ReadOnly Property SupportsDataCellMatrix As Boolean Implements IDataStorage.SupportsDataCellMatrix
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property DataCellMatrixIsFilled As Boolean Implements IDataStorage.DataCellMatrixIsFilled
            Get
                Return _writeDataNoteCellsHasBeenUsed
            End Get
        End Property

        Public Sub WriteElement(index As Integer, value As Double) Implements IDataStorage.WriteElement
            _matrix(index) = value
        End Sub

        Public Sub WriteDataNoteCellElement(index As Integer, value As String) Implements IDataStorage.WriteDataNoteCellElement
            _dataNoteCellMatrix(index) = value
            _writeDataNoteCellsHasBeenUsed = True
        End Sub

        Public Sub WriteElement(row As Integer, column As Integer, value As Double) Implements IDataStorage.WriteElement
            If column >= _rowSize Or row > _rowCount Then
                Throw New IndexOutOfRangeException()
            End If

            Dim index As Integer = row * Me._rowSize + column

            _matrix(index) = value
        End Sub




        Public Sub WriteDataNoteCellElement(row As Integer, column As Integer, value As String) Implements IDataStorage.WriteDataNoteCellElement
            If column >= _rowSize Or row > _rowCount Then
                Throw New IndexOutOfRangeException()
            End If

            Dim index As Integer = row * Me._rowSize + column
            _dataNoteCellMatrix(index) = value
            _writeDataNoteCellsHasBeenUsed = True
        End Sub



        Public Property CurrentIndex As Integer Implements IDataStorage.CurrentIndex
            Get
                Return _currentIndex
            End Get
            Set(value As Integer)
                _currentIndex = value
            End Set
        End Property

        Public ReadOnly Property MatrixColumnCount As Integer Implements IDataStorage.MatrixColumnCount
            Get
                Return _rowSize
            End Get
        End Property

        Public ReadOnly Property MatrixRowCount As Integer Implements IDataStorage.MatrixRowCount
            Get
                Return _rowCount
            End Get
        End Property

        Public ReadOnly Property MatrixSize As Integer Implements IDataStorage.MatrixSize
            Get
                Return _matrixSize
            End Get
        End Property

        Public Function CreateCopy() As IDataStorage Implements IDataStorage.CreateCopy

            Dim newObject As PxMemoryDatastorage

            newObject = CType(Me.MemberwiseClone(), PxMemoryDatastorage)

            newObject._matrix = New Double(Me._matrix.Count - 1) {}
            System.Array.Copy(Me._matrix, newObject._matrix, Me._matrix.Length)
            newObject._writeDataNoteCellsHasBeenUsed = _writeDataNoteCellsHasBeenUsed
            newObject._supportsDataCellMatrix = _supportsDataCellMatrix

            Return newObject
        End Function


    End Class
End Namespace
