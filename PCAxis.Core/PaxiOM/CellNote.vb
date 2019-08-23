Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class holding information about a cell notes in a PC Axis file
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class CellNote
        Inherits NoteBase
        Implements System.Runtime.Serialization.ISerializable

#Region "Private fields"

        ''<value>A string which in the same format as in the PC Axis file</value>
        'Private mLocation As String
        '<value>The note text</value>
        'Private mText As String
        '<value>Flag if the cell note is mandantory or not</value>
        Private _mandatory As Boolean = False
        Private _conditions As New VariableValuePairs

#End Region

#Region "Public properties"

        ''' <summary>
        ''' Condition that specifies which cell are effected by the cell note
        ''' </summary>
        ''' <value>The conditions</value>
        ''' <returns>The conditions</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Conditions() As VariableValuePairs
            Get
                Return _conditions
            End Get
        End Property


        ''' <summary>
        ''' Flag if the cell note is mandantory or not
        ''' </summary>
        ''' <value>If the cell note is mandantory or not</value>
        ''' <returns>If the cell note is mandantory or not</returns>
        ''' <remarks></remarks>
        Public Property Mandatory() As Boolean
            Get
                Return Me._mandatory
            End Get
            Set(ByVal value As Boolean)
                Me._mandatory = value
            End Set
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
        ''' <param name="cond">condition for the cell note</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal cond As VariableValuePairs)
            _conditions = cond
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
            _conditions = CType(info.GetValue("Conditions", GetType(VariableValuePairs)), VariableValuePairs)

        End Sub

        ''' <summary>
        ''' Custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Overloads Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            'MyBase.GetObjectData(info, context)
            info.AddValue("Text", Text)
            info.AddValue("Mandatory", _mandatory)
            info.AddValue("Conditions", _conditions)

        End Sub

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns>A deep copy of me</returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCopy() As CellNote
            Dim newObject As CellNote

            newObject = CType(Me.MemberwiseClone(), CellNote)

            If Not Me._conditions Is Nothing Then
                newObject._conditions = Me._conditions.CreateCopy()
            End If

            Return newObject
        End Function
    End Class

End Namespace