Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Represents a grouping within a valueset.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Grouping
        Private _id As String
        Private _name As String
        Private _map As String
        Private _groups As New List(Of Group)
        Private _groupPres As GroupingIncludesType

        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Map() As String
            Get
                Return _map
            End Get
            Set(ByVal value As String)
                _map = value
            End Set
        End Property

        Public Property Groups() As List(Of Group)
            Get
                Return _groups
            End Get
            Set(ByVal value As List(Of Group))
                _groups = value
            End Set
        End Property

        ''' <summary>
        ''' How the grouping shall be displayed (aggregated values, single values or both) 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GroupPres() As GroupingIncludesType
            Get
                Return _groupPres
            End Get
            Set(ByVal value As GroupingIncludesType)
                _groupPres = value
            End Set
        End Property


        ''' <summary>
        ''' Creates a deep copy of the Grouping object instance
        ''' </summary>
        ''' <returns>A deep copy of the Grouping object instance</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Grouping
            Dim grouping As Grouping

            grouping = CType(Me.MemberwiseClone(), Grouping)
            grouping._groups = New List(Of Group)
            For Each group As Group In Me.Groups
                grouping.Groups.Add(group.CreateCopy())
            Next

            Return grouping
        End Function
    End Class
End Namespace
