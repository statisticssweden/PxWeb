Imports System.Runtime.Serialization
Imports PCAxis.Enums

Namespace PCAxis.Paxiom
    ''' <summary>
    ''' A model representation of a statistical cube
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class PXModel
        Implements ISerializable, IDisposable


#Region "Private fields"

        Private _meta As PXMeta
        Private _data As PXData
        Private _complete As Boolean = False

#End Region

#Region "Constructor"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _meta = New PXMeta
            _data = New PXData
            _meta.SetModel(Me)
            _data.SetModel(Me)
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="meta">the metadata part</param>
        ''' <param name="data">the data part</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal meta As PXMeta, ByVal data As PXData)
            _meta = meta
            _data = data
            _meta.SetModel(Me)
            _data.SetModel(Me)
            IsComplete = True
        End Sub

#End Region

#Region "Properties"
        ''' <summary>
        ''' The metadata part of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Meta() As PXMeta
            Get
                Return _meta
            End Get
            Set(ByVal value As PXMeta)
                _meta = value
            End Set
        End Property

        ''' <summary>
        ''' The data part of the data cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Data() As PXData
            Get
                Return _data
            End Get
        End Property

        ''' <summary>
        ''' Check if the mode is completely build
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsComplete() As Boolean
            Get
                Return _complete
            End Get
            Set(ByVal value As Boolean)
                _complete = value
            End Set
        End Property
#End Region

        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            _meta = CType(info.GetValue("Meta", GetType(PXMeta)), PXMeta)
            _data = CType(info.GetValue("Data", GetType(PXData)), PXData)
            _meta.SetModel(Me)
            _data.SetModel(Me)
            IsComplete = True
        End Sub

        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) _
            Implements ISerializable.GetObjectData
            info.AddValue("Meta", _meta)
            info.AddValue("Data", _data)
        End Sub

        ''' <summary>
        ''' Validates that all the mandatory data has been set for the PxModel object
        ''' </summary>
        ''' <returns>
        ''' A list with the names of the properties that has not been set.
        ''' If the list is empty everything is ok.
        ''' </returns>
        ''' <remarks></remarks>
        Public Function Validate() As Specialized.StringCollection
            Dim missingList As New Specialized.StringCollection

            If String.IsNullOrEmpty(Me.Meta.Matrix) Then
                missingList.Add("Matrix")
            End If

            If String.IsNullOrEmpty(Me.Meta.SubjectCode) Then
                missingList.Add("SubjectCode")
            End If

            If String.IsNullOrEmpty(Me.Meta.SubjectArea) Then
                missingList.Add("SubjectArea")
            End If

            If String.IsNullOrEmpty(Me.Meta.Contents) Then
                missingList.Add("Contents")
            End If

            If String.IsNullOrEmpty(Me.Meta.ContentInfo.Units) Then
                missingList.Add("Units")
            End If

            'TODO: Decimals

            If String.IsNullOrEmpty(Me.Meta.Title) Then
                missingList.Add("Title")
            End If

            If String.IsNullOrEmpty(Me.Meta.Description) Then
                missingList.Add("Description")
            End If

            If Me.Meta.Stub.Count = 0 And Me.Meta.Heading.Count = 0 Then
                missingList.Add("Stub/Heading")
            End If

            For Each v As Variable In Me.Meta.Stub
                If v.Values.Count = 0 Then
                    missingList.Add("Values(" & v.Name & ")")
                End If
            Next

            For Each v As Variable In Me.Meta.Heading
                If v.Values.Count = 0 Then
                    missingList.Add("Values(" & v.Name & ")")
                End If
            Next

            'TODO: Data

            Return missingList
        End Function

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As PXModel
            Dim newObject As PXModel

            newObject = CType(Me.MemberwiseClone(), PXModel)

            newObject.Meta.SetModel(newObject)

            ' Handle reference types
            newObject.Meta = Me.Meta.CreateCopy()
            newObject._data = New PXData()
            newObject.Data.SetModel(newObject)
            newObject.Data.UseDataCellMatrix = Me.Data.UseDataCellMatrix
            Return newObject
        End Function

        ''' <summary>
        ''' Sets the data object
        ''' </summary>
        ''' <param name="data">The PXData object</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetData(ByVal data As PXData)
            Me._data = data
            Me._data.SetModel(Me)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    _data.Dispose()
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