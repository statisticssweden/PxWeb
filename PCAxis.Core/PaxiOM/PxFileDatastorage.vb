Imports System.IO
Imports System.Runtime.InteropServices
Namespace PCAxis.Paxiom

    Public Class PxFileDatastorage
        Implements IDataStorage, IDisposable

        Private _rowCount As Integer
        Private _columnCount As Integer
        Private _matrixSize As Integer
        Private _writer As BinaryWriter
        Private _reader As BinaryReader
        Private _currentIndex As Integer

        Private Shared DOUBLE_SIZE As Integer = Marshal.SizeOf(GetType(Double))

        Private _filename As String

        Public Sub New(rowCount As Integer, columnCount As Integer)
            _rowCount = rowCount
            _columnCount = columnCount
            _matrixSize = _rowCount * _columnCount

            _filename = GeneratePxTempFileName()

            Dim fs As New FileStream(_filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)

            fs.SetLength(MatrixSize * DOUBLE_SIZE)

            _writer = New BinaryWriter(fs)
            _reader = New BinaryReader(fs)
        End Sub

        ''' <summary>
        ''' Generate a filename for a temporary file that can be identified as used by a PX-program.
        ''' </summary>
        ''' <returns>The generated filename</returns>
        ''' <remarks>The filename will have the 'px_' prefix</remarks>
        Private Function GeneratePxTempFileName() As String
            Dim tmp As String
            Dim dir As String
            Dim filename As String
            Dim tmpReturn As String

            tmp = Path.GetTempFileName()
            dir = Path.GetDirectoryName(tmp)
            filename = Path.GetFileName(tmp)
            filename = "px_" + filename
            tmpReturn = Path.Combine(dir, filename)

            Try
                File.Move(tmp, tmpReturn)
                Return tmpReturn
            Catch ex As Exception
                Return tmp
            End Try

        End Function

        ''' <summary>
        ''' Cleanup function that will remove old px-tempfiles that for some reason was not removed in a proper way.
        ''' </summary>
        ''' <remarks></remarks>
        Friend Shared Sub RemoveOldPxTempFiles()
            Dim dir As New DirectoryInfo(Path.GetTempPath())

            For Each f As FileInfo In dir.GetFiles("px_*")
                Dim diff As TimeSpan = DateTime.Now - f.LastWriteTime

                'Try to remove file if it is older than one day
                If diff.Days >= 1 Then
                    Try
                        File.Delete(f.FullName)
                    Catch ex As Exception
                        ' Do nothing
                    End Try
                End If
            Next

        End Sub


        Public Function CreateCopy() As IDataStorage Implements IDataStorage.CreateCopy

            _writer.Flush()

            Dim newObject As PxFileDatastorage

            newObject = CType(Me.MemberwiseClone(), PxFileDatastorage)

            newObject._filename = GeneratePxTempFileName()

            File.Copy(_filename, newObject._filename, True)

            Dim fs As New FileStream(newObject._filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)

            newObject._writer = New BinaryWriter(fs)
            newObject._reader = New BinaryReader(fs)

            Return newObject
        End Function

        Public Property CurrentIndex As Integer Implements IDataStorage.CurrentIndex
            Get
                Return _currentIndex
            End Get
            Set(value As Integer)
                _currentIndex = value
            End Set
        End Property

        Public Sub IncrementElement(index As Integer, value As Double) Implements IDataStorage.IncrementElement
            WriteElement(index, PXData.AddElements(ReadElement(index), value))
        End Sub

        Public Sub IncrementElement(row As Integer, column As Integer, value As Double) Implements IDataStorage.IncrementElement
            WriteElement(row, column, PXData.AddElements(ReadElement(row, column), value))
        End Sub

        Public ReadOnly Property MatrixColumnCount As Integer Implements IDataStorage.MatrixColumnCount
            Get
                Return _columnCount

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

        Public Function ReadElement(index As Integer) As Double Implements IDataStorage.ReadElement
            Dim offset As Integer = (index * DOUBLE_SIZE)

            _reader.BaseStream.Seek(offset, SeekOrigin.Begin)
            Return _reader.ReadDouble()

        End Function

        Public Function ReadElement(row As Integer, column As Integer) As Double Implements IDataStorage.ReadElement
            'Check that the index ar in the 
            If row >= _rowCount Or column >= _columnCount Then Throw New Exception("Index of of bounds")

            Return ReadElement((row * _columnCount) + column)
        End Function

        Function ReadDataCellNoteElement(ByVal row As Integer, ByVal column As Integer) As String Implements IDataStorage.ReadDataCellNoteElement
            Throw New NotSupportedException()
        End Function

        Public Function ReadLine(row As Integer, buffer() As Double) As Integer Implements IDataStorage.ReadLine
            'Check that the index ar in the 
            If row >= _rowCount Then Throw New Exception("Index of of bounds")

            Dim rowBuffer As Byte()
            ReDim rowBuffer(_columnCount * DOUBLE_SIZE)

            Dim offset As Integer = (row * _columnCount) * DOUBLE_SIZE

            _reader.BaseStream.Seek(offset, SeekOrigin.Begin)
            _reader.Read(rowBuffer, 0, rowBuffer.Length)
            System.Buffer.BlockCopy(rowBuffer, 0, buffer, 0, buffer.Length)
            Return _columnCount

        End Function

        Public Function Write(buffer() As Double, startIndex As Integer, stopIndex As Integer) As Integer Implements IDataStorage.Write
            Dim length As Integer = stopIndex - startIndex
            _writer.Write(GetBytes(buffer, startIndex, stopIndex))
            _currentIndex += (startIndex - stopIndex)

            Return length
        End Function

        Public Function WriteDataNoteCells(ByVal buffer As String(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer Implements IDataStorage.WriteDataNoteCells
            Throw New NotSupportedException()
        End Function

        Public ReadOnly Property SupportsDataCellMatrix As Boolean Implements IDataStorage.SupportsDataCellMatrix
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property DataCellMatrixIsFilled As Boolean Implements IDataStorage.DataCellMatrixIsFilled
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public Sub WriteElement(index As Integer, value As Double) Implements IDataStorage.WriteElement
            Dim offset As Integer = index * DOUBLE_SIZE

            _writer.Seek(offset, SeekOrigin.Begin)
            _writer.Write(value)

        End Sub

        Public Sub WriteElement(row As Integer, column As Integer, value As Double) Implements IDataStorage.WriteElement
            'Check that the index ar in the 
            If row >= _rowCount Or column >= _columnCount Then Throw New Exception("Index of of bounds")

            WriteElement((row * _columnCount) + column, value)

        End Sub

        Shared Function GetBytes(values As Double(), startIndex As Integer, stopIndex As Integer) As Byte()
            Dim result As Byte()
            Dim length As Integer = stopIndex - startIndex
            ReDim result(((length + 1) * DOUBLE_SIZE) - 1)
            Buffer.BlockCopy(values, startIndex, result, 0, result.Length)
            Return result
        End Function


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    'Dispose managed state (managed objects).
                    If Not _writer Is Nothing Then
                        _writer.Close()
                    End If

                    If Not _reader Is Nothing Then
                        _reader.Close()
                    End If
                End If

                'Free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.

                If IO.File.Exists(_filename) Then
                    Try
                        File.Delete(_filename)
                    Catch ex As Exception
                        ' Do nothing
                    End Try
                End If

            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Public Function ReadDataCellNoteElement(index As Integer) As String Implements IDataStorage.ReadDataCellNoteElement
            Throw New NotSupportedException()
        End Function

        Public Sub WriteDataNoteCellElement(index As Integer, value As String) Implements IDataStorage.WriteDataNoteCellElement
            Throw New NotSupportedException()
        End Sub
        Public Sub WriteDataNoteCellElement(row As Integer, column As Integer, value As String) Implements IDataStorage.WriteDataNoteCellElement
            Throw New NotSupportedException()
        End Sub
#End Region

    End Class

End Namespace
