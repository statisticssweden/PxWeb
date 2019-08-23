Imports System.Runtime.Serialization

Namespace Exceptions
    ''' <summary>
    ''' Thrown by the <see cref=" MarkerControlBase(Of TControl,TMarker)" /> when 
    ''' it fails to properly load the underlying ascx control
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class AscxLoadException
        Inherits Exception

        ''' <summary>
        ''' Initializes a new instance of the <see cref="AscxLoadException" /> with the supplied message
        ''' </summary>
        ''' <param name="message">The message for the <see cref="AscxLoadException" /></param>
        ''' <remarks></remarks>
        Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Initializes a new empty instance of the <see cref="AscxLoadException" />
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="AscxLoadException" /> with the supplied message and inner exception
        ''' </summary>
        ''' <param name="message">The message for the <see cref="AscxLoadException" /></param>
        ''' <param name="innerException">The inner exception for the <see cref="AscxLoadException" /></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        ''' <summary>
        ''' Used to support deserialization of the exception
        ''' </summary>
        ''' <param name="info">The <see cref="SerializationInfo"/> to use to deserialize</param>
        ''' <param name="context">The <see cref="StreamingContext" />to use to deserialize</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class
End Namespace
