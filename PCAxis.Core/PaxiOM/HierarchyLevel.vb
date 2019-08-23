Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Hierarcy level 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class HierarchyLevel
        Implements System.Runtime.Serialization.ISerializable

#Region "Private flieds"

        Private _code As String
        Private _children As New HierarchyLevels

#End Region

#Region "Public properties"

        ''' <summary>
        ''' The value code for the hierarchy level
        ''' </summary>
        ''' <value>The value code for the hierarchy level</value>
        ''' <returns>The value code for the hierarchy level</returns>
        ''' <remarks></remarks>
        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        ''' <summary>
        ''' list of sub hierarchies
        ''' </summary>
        ''' <value>list of sub hierarchies</value>
        ''' <returns>list of sub hierarchies</returns>
        ''' <remarks></remarks>
        Public Property Children() As HierarchyLevels
            Get
                Return _children
            End Get
            Set(ByVal value As HierarchyLevels)
                _children = value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="code">the value code for the level</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal code As String)
            _code = code
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

#End Region

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            _code = info.GetString("Code")
            _children = CType(info.GetValue("Children", GetType(HierarchyLevels)), HierarchyLevels)
        End Sub

        ''' <summary>
        ''' Add object data to SerializationInfo
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Code", _code)
            info.AddValue("Children", _children)
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As HierarchyLevel
            Dim newObject As HierarchyLevel

            newObject = CType(Me.MemberwiseClone(), HierarchyLevel)

            ' Handle reference types
            newObject._children = Nothing
            If Me._children IsNot Nothing Then
                newObject._children = Me._children.CreateCopy()
            End If

            Return newObject
        End Function
    End Class

End Namespace
