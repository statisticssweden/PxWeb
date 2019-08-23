Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Represents a group within a grouping
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Group
        Private _groupCode As String
        Public Property GroupCode() As String
            Get
                Return _groupCode
            End Get
            Set(ByVal value As String)
                _groupCode = value
            End Set
        End Property

        Private _groupName As String
        Public Property Name() As String
            Get
                Return _groupName
            End Get
            Set(ByVal value As String)
                _groupName = value
            End Set
        End Property

        'Could also be the same as ValueCode
        Private _children As New List(Of GroupChildValue)
        Public Property ChildCodes() As List(Of GroupChildValue)
            Get
                Return _children
            End Get
            Set(ByVal value As List(Of GroupChildValue))
                _children = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a deep copy of the Group object instance
        ''' </summary>
        ''' <returns>A deep copy of the Group object instance</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Group
            Dim group As Group

            group = CType(Me.MemberwiseClone(), Group)
            group._children = New List(Of GroupChildValue)
            For Each gcv As GroupChildValue In Me.ChildCodes
                group.ChildCodes.Add(gcv.CreateCopy())
            Next

            Return group
        End Function
    End Class

    ''' <summary>
    ''' Represents a value within a group
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GroupChildValue
        Private _code As String
        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a deep copy of the GroupChildValue object instance
        ''' </summary>
        ''' <returns>A deep copy of the object instance</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As GroupChildValue
            Dim gcv As GroupChildValue

            gcv = CType(Me.MemberwiseClone(), GroupChildValue)
            Return gcv
        End Function
    End Class
End Namespace
