Namespace PCAxis.Paxiom

    <Serializable()> _
    Public Class Partition
        Implements System.Runtime.Serialization.ISerializable


#Region "Private fields"
        Private mLanguageIndex As Integer = 0

        Private _name() As String
        Private _startIndex As Integer
        Private _length As Integer

#End Region

#Region "Public properties"

        Public Property Name() As String 'Implements IPXItem.Name
            Get
                Return _name(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _name(mLanguageIndex) = value
            End Set
        End Property

        Public Property StartIndex() As Integer
            Get
                Return _startIndex
            End Get
            Set(ByVal value As Integer)
                _startIndex = value
            End Set
        End Property


        Public Property Length() As Integer
            Get
                Return _length
            End Get
            Set(ByVal value As Integer)
                _length = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _name = New String(0) {}
        End Sub

        ''' <summary>
        ''' Constructor for initializing the _name array with internalBufferSize
        ''' </summary>
        ''' <param name="internalBufferSize"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal internalBufferSize As Integer)
            _name = New String(internalBufferSize) {}
        End Sub

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            _name = CType(info.GetValue("Name", GetType(String())), String())
            _startIndex = info.GetInt32("StartIndex")
            _length = info.GetInt32("Length")
        End Sub

        ''' <summary>
        ''' Functionality used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Name", _name) 'array
            info.AddValue("StartIndex", _startIndex)
            info.AddValue("Length", _length)
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Partition
            Dim newObject As Partition

            newObject = CType(Me.MemberwiseClone(), Partition)

            ' Handle reference types
            newObject._name = Nothing
            If Me._name IsNot Nothing Then
                newObject._name = New String(Me._name.Count - 1) {}
                For i As Integer = 0 To Me._name.Count - 1
                    newObject._name(i) = Me._name(i)
                Next
            End If
            Return newObject
        End Function
    End Class

End Namespace
