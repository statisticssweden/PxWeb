Namespace PCAxis.Paxiom
    
    ''' <summary>
    ''' Class for notes
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Note
        Inherits NoteBase
        Implements System.Runtime.Serialization.ISerializable


#Region "Private fields"

        Private _type As NoteType
        Private _mandatory As Boolean = False

#End Region

#Region "Public Properties"
        ''' <summary>
        ''' The type of note
        ''' </summary>
        ''' <value>The type of the note. In other words where i should be applied</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Type() As NoteType
            Get
                Return Me._type
            End Get
            Set(ByVal value As NoteType)
                Me._type = value
            End Set
        End Property

        ''' <summary>
        ''' If the note is mandantory
        ''' </summary>
        ''' <value>If the note is mandantory</value>
        ''' <returns>True if the note is mandantory otherwise False</returns>
        ''' <remarks></remarks>
        Public Property Mandantory() As Boolean
            Get
                Return Me._mandatory
            End Get
            Set(ByVal value As Boolean)
                Me._mandatory = value
            End Set
        End Property

#End Region

#Region "Constructor(s)"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="text">note text</param>
        ''' <param name="type">note type</param>
        ''' <param name="mandantory">if it should be a mandantory note</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal text As String, ByVal type As NoteType, ByVal mandantory As Boolean)
            Me.Text = text
            Me._type = type
            Me._mandatory = mandantory
        End Sub

#End Region

        ''' <summary>
        ''' Constructor for custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            'MyBase.new(info, context)
            Text = info.GetString("Text")
            _mandatory = info.GetBoolean("Mandatory")
            _type = CType(info.GetInt32("Type"), NoteType)
        End Sub

        ''' <summary>
        ''' custom serializer
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Overloads Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            'MyBase.GetObjectData(info, context)
            info.AddValue("Text", Text)
            info.AddValue("Mandatory", _mandatory)
            info.AddValue("Type", _type)
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns>A deep clone of the note</returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCopy() As Note
            Dim newObject As Note

            newObject = CType(Me.MemberwiseClone(), Note)

            'Handle reference types
            'None at the moment

            Return newObject
        End Function
    End Class


End Namespace