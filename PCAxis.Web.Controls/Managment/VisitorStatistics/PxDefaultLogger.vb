''' <summary>
''' Default implementation for logging visitor statistics.
'''Logs visitor actions to file using Log4Net.
''' </summary>
''' <remarks></remarks>
Public Class PxDefaultLogger
    Implements IActionLogger

    Private Shared _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PxDefaultLogger))

    Public Sub LogEvent(ByVal context As ActionContext, ByVal userid As String, ByVal lang As String, ByVal database As String, ByVal e As PxActionEventArgs) Implements IActionLogger.LogEvent
        Dim logMess As System.Text.StringBuilder = New System.Text.StringBuilder()

        logMess.Append("Context=" & context.ToString() & ", ")
        logMess.Append("UserId=" & userid & ", ")
        logMess.Append("Language=" & lang & ", ")
        logMess.Append("Database=" & database & ", ")
        logMess.Append("ActionType=" & e.ActionType.ToString() & ", ")
        logMess.Append("ActionName=" & e.ActionName + ", ")
        logMess.Append("TableId=" & e.TableId & ", ")
        logMess.Append("NumberOfCells=" & e.NumberOfCells.ToString() & ", ")
        logMess.Append("NumberOfContents=" & e.NumberOfContents.ToString())
        _logger.Info(logMess.ToString())

    End Sub
End Class

