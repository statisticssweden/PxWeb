Namespace Exceptions

    <Serializable()> _
    Public Class InvalidQuerystringParameterException
        Inherits Exception

        ''' <summary>
        ''' Initializes a new empty instance of the InvalidQuerystringParameterException />
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the exception with the supplied message
        ''' </summary>
        ''' <param name="message">The message for the exception</param>
        ''' <remarks></remarks>
        Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the exception with the supplied message and inner exception
        ''' </summary>
        ''' <param name="message">The message for the exception /></param>
        ''' <param name="innerException">The inner exception for the exception /></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace
