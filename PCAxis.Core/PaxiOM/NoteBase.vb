Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Base class for all notes
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class NoteBase
        ' Implements System.Runtime.Serialization.ISerializable

        Private _text As String

        ''' <summary>
        ''' the Text of the note
        ''' </summary>
        ''' <value>the Text of the note</value>
        ''' <returns>the Text of the note</returns>
        ''' <remarks></remarks>
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As NoteBase
            Dim newObject As NoteBase

            newObject = CType(Me.MemberwiseClone(), NoteBase)

            Return newObject
        End Function
    End Class

End Namespace

