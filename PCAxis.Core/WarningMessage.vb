Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class for waring messages.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WarningMessage
        Public Code As String
        Public Params() As Object

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
        End Sub


        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="code">the code of the error message</param>
        ''' <remarks>The actual error message is stored in a resource file</remarks>
        Sub New(ByVal code As String)
            Me.Code = code
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="code">the code of the error message</param>
        ''' <param name="params">
        ''' parameters that should be inserted in the error message
        ''' </param>
        ''' <remarks>
        ''' The actual error message is stored in a resource file and 
        ''' the params is used to insert values inte the placeholders
        ''' using the String.Format metod.
        ''' </remarks>
        Sub New(ByVal code As String, ByVal params() As Object)
            Me.Code = code
            Me.Params = params
        End Sub
    End Class
End Namespace