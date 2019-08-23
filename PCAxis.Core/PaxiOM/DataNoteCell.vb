Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class for holding a cell data note
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class DataNoteCell
        Inherits NoteBase
        Implements System.Runtime.Serialization.ISerializable


#Region "Private fields"


        Private _conditions As New VariableValuePairs

#End Region

#Region "Public properties"
        ''' <summary>
        ''' A set of variable value pairs that makes a condition 
        ''' for which cells is affected by the note
        ''' </summary>
        ''' <value>the coditions</value>
        ''' <returns>the conditions</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Conditions() As VariableValuePairs
            Get
                Return _conditions
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="cond">the condition of the note</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal cond As VariableValuePairs)
            _conditions = cond
        End Sub

#End Region

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCopy() As DataNoteCell
            Dim newObject As DataNoteCell

            newObject = CType(Me.MemberwiseClone(), DataNoteCell)

            If Not Me._conditions Is Nothing Then
                newObject._conditions = Me._conditions.CreateCopy()
            End If

            Return newObject
        End Function

        ''' <summary>
        ''' Constructor for custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            'MyBase.new(info, context)
            Text = info.GetString("Text")
            _conditions = CType(info.GetValue("Conditions", GetType(VariableValuePairs)), VariableValuePairs)

        End Sub

        ''' <summary>
        ''' Custom serializer
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Overloads Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            'MyBase.GetObjectData(info, context)
            info.AddValue("Text", Text)
            info.AddValue("Conditions", _conditions)

        End Sub
    End Class
End Namespace