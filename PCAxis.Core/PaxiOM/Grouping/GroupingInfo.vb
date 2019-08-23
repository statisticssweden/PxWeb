Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Identifies a grouping
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GroupingInfo
        Inherits OperationsInfo

#Region "Constructors"

        ''' <summary>
        ''' Defines how the grouping shall be presented (aggregated values, single values or both)
        ''' </summary>
        ''' <remarks></remarks>
        Private _groupPres As GroupingIncludesType
        Public Property GroupPres() As GroupingIncludesType
            Get
                Return _groupPres
            End Get
            Set(ByVal value As GroupingIncludesType)
                _groupPres = value
            End Set
        End Property

        Sub New()
            MyBase.New()

        End Sub

        Sub New(ByVal id As String)
            MyBase.New(id)

        End Sub

        Public Sub New(ByVal id As String, ByVal internalBufferSize As Integer)
            MyBase.New(id, internalBufferSize)
        End Sub

        'TODO: Readonly properties...

        ''' <summary>
        ''' Creates as deep copy of the GroupingInfo object instance
        ''' </summary>
        ''' <returns>A deep copy of the GroupingInfo object instance</returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCopy() As GroupingInfo
            Dim newObject As GroupingInfo

            newObject = CType(MyBase.CreateCopy(), GroupingInfo)

            Return newObject
        End Function

#End Region
    End Class
End Namespace
