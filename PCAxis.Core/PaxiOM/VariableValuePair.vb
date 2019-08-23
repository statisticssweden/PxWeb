Namespace PCAxis.Paxiom

    <Serializable()> _
    Public Class VariableValuePair
        Implements System.Runtime.Serialization.ISerializable

#Region "Private fields"

        Private _variable As String
        Private _value As String

#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="variableCode">The variable code</param>
        ''' <param name="valueCode">The value code</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal variableCode As String, ByVal valueCode As String)
            _variable = variableCode
            _value = valueCode
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The variable code
        ''' </summary>
        ''' <value>The variable code</value>
        ''' <returns>The variable code</returns>
        ''' <remarks></remarks>
        Public Property VariableCode() As String
            Get
                Return _variable
            End Get
            Set(ByVal value As String)
                _variable = value
            End Set
        End Property

        ''' <summary>
        ''' The value code
        ''' </summary>
        ''' <value>The value code</value>
        ''' <returns>The value code</returns>
        ''' <remarks></remarks>
        Public Property ValueCode() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

#End Region

        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            _variable = info.GetString("Variable")
            _value = info.GetString("Value")
        End Sub

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Variable", _variable)
            info.AddValue("Value", _value)
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As VariableValuePair
            Dim vp As VariableValuePair

            ' Only Value types right now
            vp = CType(Me.MemberwiseClone(), VariableValuePair)

            ' Handle Reference types
            ' None at this moment

            Return vp
        End Function
    End Class
End Namespace