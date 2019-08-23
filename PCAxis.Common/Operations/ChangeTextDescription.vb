Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for supporting the ChangeText operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeTextDescription

        ''' <summary>
        ''' Table content
        ''' </summary>
        ''' <remarks></remarks>
        Public Content As String

        ''' <summary>
        ''' Units
        ''' </summary>
        ''' <remarks></remarks>
        Public Units As String

        ''' <summary>
        ''' Variable names
        ''' </summary>
        ''' <remarks></remarks>
        Public Variables As List(Of KeyValuePair(Of String, String))

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.Variables = New List(Of KeyValuePair(Of String, String))
        End Sub

        ''' <summary>
        ''' Constructor initializing the parameter
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal content As String, ByVal units As String, ByVal variables As List(Of KeyValuePair(Of String, String)))
            Me.Content = Content
            Me.Units = units
            Me.Variables = variables
        End Sub
    End Class

End Namespace
