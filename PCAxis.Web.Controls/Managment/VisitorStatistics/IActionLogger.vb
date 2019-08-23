
''' <summary>
''' Interface for handling visitor statistics
''' </summary>
''' <remarks></remarks>
Public Interface IActionLogger

    ''' <summary>
    ''' Log visitor statistics event to media (db, file...)
    ''' </summary>
    ''' <param name="userid">User id</param>
    ''' <param name="lang">Language</param>
    ''' <param name="database">Database id</param>
    ''' <param name="e">PxActionEvent arguments</param>
    ''' <remarks></remarks>
    Sub LogEvent(ByVal context As ActionContext, ByVal userid As String, ByVal lang As String, ByVal database As String, ByVal e As PxActionEventArgs)

End Interface

