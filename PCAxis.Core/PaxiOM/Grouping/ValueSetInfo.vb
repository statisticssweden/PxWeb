Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Information class for a valueset
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValueSetInfo
        Inherits OperationsInfo

#Region "Constructors"
        ''' <summary>
        ''' Empty Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            MyBase.New()

        End Sub

        ''' <summary>
        ''' Constructor with id as parameter
        ''' </summary>
        ''' <param name="id"></param>
        ''' <remarks></remarks>
        Sub New(ByVal id As String)
            MyBase.New(id)

        End Sub

        ''' <summary>
        ''' Constructor with id and internalBufferSize as parameters
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="internalBufferSize"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal id As String, ByVal internalBufferSize As Integer)
            MyBase.New(id, internalBufferSize)
        End Sub
#End Region

        ''' <summary>
        ''' Creates as deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCopy() As ValueSetInfo
            Dim newObject As ValueSetInfo

            newObject = CType(MyBase.CreateCopy(), ValueSetInfo)

            Return newObject
        End Function

    End Class
End Namespace
