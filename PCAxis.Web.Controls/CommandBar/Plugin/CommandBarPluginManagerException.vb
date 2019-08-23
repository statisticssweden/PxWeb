Imports System.Runtime.Serialization
Namespace CommandBar.Plugin

    ''' <summary>
    ''' Thrown by the <see cref="CommandBarPluginManager" /> whenever an exception occurs
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class CommandBarPluginManagerException
        Inherits Exception

        ''' <summary>
        ''' Initializes a new empty instance of the <see cref="CommandBarPluginManagerException" />
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandBarPluginManagerException" /> with the supplied message
        ''' </summary>
        ''' <param name="message">The message for the <see cref="CommandBarPluginManagerException" /></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandBarPluginManagerException" /> with the supplied message and inner exception
        ''' </summary>
        ''' <param name="message">The message for the <see cref="CommandBarPluginManagerException" /></param>
        ''' <param name="innerException">The inner exception for the <see cref="CommandBarPluginManagerException" /></param>
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
        Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub

    End Class
End Namespace