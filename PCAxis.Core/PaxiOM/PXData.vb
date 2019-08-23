Namespace PCAxis.Paxiom

    <Serializable()> _
    Public Class PXData
        Implements System.Runtime.Serialization.ISerializable, IDisposable


#Region "Private fields"

        'The internal DataBuffer
        'Private _matrix As Double()
        'Private _currentIndex As Integer
        'Private _maxMatrixSize As Integer
        'Private _rowSize As Integer
        'Private _rowCount As Integer
        'Private _matrixSize As Integer
        Private _model As PXModel

        Private _dataStorage As IDataStorage
        Private _useDataCellMatrix As Boolean = False

#End Region

#Region "Properties"
        ''' <summary>
        ''' The model that the data belongs to 
        ''' </summary>
        ''' <value>The model that the data belongs to </value>
        ''' <returns>The model that the data belongs to </returns>
        ''' <remarks></remarks>
        Public Property Model() As PXModel
            Get
                Return _model
            End Get
            Set(ByVal value As PXModel)
                _model = value
            End Set
        End Property

        ''' <summary>
        ''' Number of rows in the data matrix
        ''' </summary>
        ''' <value>Number of rows in the data matrix</value>
        ''' <returns>Number of rows in the data matrix</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MatrixRowCount() As Integer
            Get
                Return _dataStorage.MatrixRowCount
            End Get
        End Property

        ''' <summary>
        ''' Number of columns in the data matrix
        ''' </summary>
        ''' <value>Number of columns in the data matrix</value>
        ''' <returns>Number of columns in the data matrix</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MatrixColumnCount() As Integer
            Get
                Return _dataStorage.MatrixColumnCount
            End Get
        End Property

        ''' <summary>
        ''' The size of the data matrix
        ''' </summary>
        ''' <value>The size of the data matrix</value>
        ''' <returns>The size of the data matrix</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MatrixSize() As Integer
            Get
                Return _dataStorage.MatrixSize
            End Get
        End Property




        ''' <summary>
        ''' Check to see if all the data ahve been loaded
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsMatrixLoaded() As Boolean
            Get
                If _dataStorage.MatrixSize = _dataStorage.CurrentIndex Then
                    Return True
                End If
                Return False
            End Get
        End Property

        ''' <summary>
        ''' One dimensional representation of the matrix
        ''' </summary>
        ''' <returns>The matrix</returns>
        Public ReadOnly Property Matrix As Double()
            Get
                Return New Double() {1} '_matrix
            End Get
        End Property
#End Region

#Region "Constructor(s)"

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '_currentIndex = 0
            'TODO REMOVE just for test
            SetMatrixSize(4, 4)
        End Sub

#End Region


#Region "Public functions"
        ''' <summary>
        ''' Sets the model property 
        ''' </summary>
        ''' <param name="model">The model that the data should belong to</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetModel(ByVal model As PXModel)
            Me._model = model
        End Sub

        Public Property CurrentIndex As Integer
            Get
                Return Me._dataStorage.CurrentIndex
            End Get
            Set(value As Integer)
                Me._dataStorage.CurrentIndex = value
            End Set
        End Property

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As PXData
            Dim newObject As PXData

            newObject = CType(Me.MemberwiseClone(), PXData)
            newObject._model = Nothing

            'TODO fix
            'newObject._matrix = New Double(Me._matrix.Count - 1) {}
            'System.Array.Copy(Me._matrix, newObject._matrix, Me._matrix.Length)
            newObject._dataStorage = _dataStorage.CreateCopy()
            Return newObject
        End Function

        ''' <summary>
        ''' Writes data sequentially to the date note cells matrix
        ''' </summary>
        ''' <param name="buffer">data buffer to write</param>
        ''' <param name="startIndex">start index of the data in the buffer</param>
        ''' <param name="stopIndex">stop index of the data in the buffer</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function WriteDataNoteCells(ByVal buffer As String(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer
            Return _dataStorage.WriteDataNoteCells(buffer, startIndex, stopIndex)
        End Function

        ''' <summary>
        ''' Writes data sequentially to the matrix
        ''' </summary>
        ''' <param name="buffer">data buffer to write</param>
        ''' <param name="startIndex">start index of the data in the buffer</param>
        ''' <param name="stopIndex">stop index of the data in the buffer</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Write(ByVal buffer As Double(), ByVal startIndex As Integer, ByVal stopIndex As Integer) As Integer
            Return _dataStorage.Write(buffer, startIndex, stopIndex)

        End Function

        ''' <summary>
        ''' Writes a singel value into the the data matrix
        ''' </summary>
        ''' <param name="index">index of the value in the data matrix</param>
        ''' <param name="value">the value</param>
        ''' <remarks></remarks>
        Public Sub WriteElement(ByVal index As Integer, ByVal value As Double)
            _dataStorage.WriteElement(index, value)
        End Sub

        Public Sub WriteDataNoteCellElement(index As Integer, value As String)
            If _dataStorage.SupportsDataCellMatrix Then
                _dataStorage.WriteDataNoteCellElement(index, value)
            End If
        End Sub

        Public Sub WriteDataNoteCellElement(ByVal row As Integer, ByVal column As Integer, ByVal value As String)
            If _dataStorage.SupportsDataCellMatrix Then
                _dataStorage.WriteDataNoteCellElement(row, column, value)
            End If
        End Sub

        ''' <summary>
        ''' Increments a value in the datamatrix by the amount specified in value
        ''' </summary>
        ''' <param name="index">the index of the value</param>
        ''' <param name="value">the amount</param>
        ''' <remarks></remarks>
        Public Sub IncrementElement(ByVal index As Integer, ByVal value As Double)
            _dataStorage.IncrementElement(index, value)
        End Sub

        ''' <summary>
        ''' Re-dimensions the data matrix
        ''' </summary>
        ''' <param name="rowCount">the new number of rows</param>
        ''' <param name="rowSize">the new number of columns</param>
        ''' <remarks></remarks>
        Public Sub SetMatrixSize(ByVal rowCount As Integer, ByVal rowSize As Integer)
            Dim size As Integer = (rowCount * rowSize)
            Dim memoryTreshold As Integer = Integer.MaxValue
            Dim memSetting As Integer

            'Is the MaxCellsInMemory threshold value specified?
            If Not String.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings("MaxCellsInMemory")) Then
                If Integer.TryParse(System.Configuration.ConfigurationManager.AppSettings("MaxCellsInMemory"), memSetting) Then
                    memoryTreshold = memSetting
                End If
            End If


            If (_dataStorage IsNot Nothing And TypeOf (_dataStorage) Is IDisposable) Then
                CType(_dataStorage, IDisposable).Dispose()
            End If

            If (size < memoryTreshold) Then
                _dataStorage = New PxMemoryDatastorage(rowCount, rowSize)

            Else
                _dataStorage = New PxFileDatastorage(rowCount, rowSize)
            End If

        End Sub

        ''' <summary>
        ''' Reads a row into a buffer
        ''' </summary>
        ''' <param name="lineNumber">the row that should be read</param>
        ''' <param name="buffer">the buffer that will store the values</param>
        ''' <returns>the number of elements read</returns>
        ''' <remarks>Make sure that the buffer is equla or larger then the row size</remarks>
        Public Function ReadLine(ByVal lineNumber As Integer, ByVal buffer As Double()) As Integer
            Return _dataStorage.ReadLine(lineNumber, buffer)
        End Function


        ''' <summary>
        ''' Reads a element in the data matrix
        ''' </summary>
        ''' <param name="row">the row position</param>
        ''' <param name="column">the column position</param>
        ''' <returns>the value of the specified position</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal row As Integer, ByVal column As Integer) As Double
            Return _dataStorage.ReadElement(row, column)
        End Function

        Public Function ReadDataCellNoteElement(ByVal row As Integer, ByVal column As Integer) As String
            Return _dataStorage.ReadDataCellNoteElement(row, column)
        End Function

        Public Property UseDataCellMatrix As Boolean
            Get
                Return (_dataStorage.SupportsDataCellMatrix And _useDataCellMatrix)
            End Get
            Set(value As Boolean)
                _useDataCellMatrix = value
            End Set
        End Property

        Public ReadOnly Property DataCellMatrixIsFilled As Boolean
            Get
                Return _dataStorage.DataCellMatrixIsFilled
            End Get
        End Property

        ''' <summary>
        ''' Reads a element in the data matrix
        ''' </summary>
        ''' <param name="index">the index of the value</param>
        ''' <returns>the value of the specified index</returns>
        ''' <remarks></remarks>
        Public Function ReadElement(ByVal index As Integer) As Double
            Return _dataStorage.ReadElement(index)
        End Function


        Function ReadDataCellNoteElement(ByVal index As Integer) As String
            If _dataStorage.SupportsDataCellMatrix Then
                Return _dataStorage.ReadDataCellNoteElement(index)
            End If

            Return ""
        End Function

        ''' <summary>
        ''' Writes to a element in the data matrix
        ''' </summary>
        ''' <param name="row">row position</param>
        ''' <param name="column">column position</param>
        ''' <param name="value">value that should be written</param>
        ''' <remarks></remarks>
        Public Sub WriteElement(ByVal row As Integer, ByVal column As Integer, ByVal value As Double)
            _dataStorage.WriteElement(row, column, value)
        End Sub

        ''' <summary>
        ''' Increments a value in the datamatrix by the amount specified in value
        ''' </summary>
        ''' <param name="row">row position</param>
        ''' <param name="column">column position</param>
        ''' <param name="value">the amount</param>
        ''' <remarks></remarks>
        Public Sub IncrementElement(ByVal row As Integer, ByVal column As Integer, ByVal value As Double)
            _dataStorage.IncrementElement(row, column, value)
        End Sub

        ''' <summary>
        ''' Cleanup method that removes temporary files that have been created by PX and for some reasons have not been deleted before
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub RemovePxTempFiles()
            PxFileDatastorage.RemoveOldPxTempFiles()
        End Sub

#End Region

        ''' <summary>
        ''' Constructor for serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContex</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            '_rowSize = info.GetInt32("RowSize")
            '_rowCount = info.GetInt32("RowCount")
            '_matrixSize = info.GetInt32("MatrixSize")
            '_matrix = CType(info.GetValue("Matrix", GetType(Double())), Double())
        End Sub

        ''' <summary>
        ''' for custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContex</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            'info.AddValue("RowSize", _rowSize)
            'info.AddValue("RowCount", _rowCount)
            'info.AddValue("MatrixSize", _matrixSize)
            'info.AddValue("Matrix", _matrix)
        End Sub

        ' ''' <summary>
        ' ''' Adds a element with a value using the addiative rulze of PC-Axis
        ' ''' </summary>
        ' ''' <param name="lhs">left hand side</param>
        ' ''' <param name="rhs">right hand side</param>
        ' ''' <returns>the sum of lhs and rhs</returns>
        ' ''' <remarks></remarks>


        ''' <summary>
        ''' Hepler function to create a row buffer with the rigth size
        ''' that can be used to read rows.
        ''' </summary>
        ''' <returns>A row buffer</returns>
        ''' <remarks></remarks>
        Public Function CreateRowBuffer() As Double()
            Return New Double(_dataStorage.MatrixColumnCount - 1) {}
        End Function

        Public Shared Function AddElements(ByVal lhs As Double, ByVal rhs As Double) As Double
            If Array.IndexOf(PXConstant.ProtectedValues, lhs) >= 0 And _
               Array.IndexOf(PXConstant.ProtectedValues, rhs) >= 0 Then
                If Double.Equals(lhs, rhs) Then
                    Return lhs
                ElseIf Array.IndexOf(PXConstant.ProtectedNullValues, lhs) >= 0 Then
                    Return rhs
                ElseIf Array.IndexOf(PXConstant.ProtectedNullValues, rhs) >= 0 Then
                    Return lhs
                Else
                    Return PXConstant.DATASYMBOL_7
                End If
            ElseIf Array.IndexOf(PXConstant.ProtectedValues, lhs) >= 0 Then
                If lhs = PXConstant.DATASYMBOL_NIL Then
                    Return rhs
                Else
                    Return lhs
                End If

            ElseIf Array.IndexOf(PXConstant.ProtectedValues, rhs) >= 0 Then
                If rhs = PXConstant.DATASYMBOL_NIL Then
                    Return lhs
                Else
                    Return rhs
                End If
            Else
                Return lhs + rhs
            End If

        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    'Dispose managed state (managed objects).
                    If TypeOf (_dataStorage) Is IDisposable Then
                        CType(_dataStorage, IDisposable).Dispose()
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace
