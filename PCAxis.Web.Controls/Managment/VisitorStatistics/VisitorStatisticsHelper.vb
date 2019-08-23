''' <summary>
''' Helper class for logging visitor statistics
''' </summary>
''' <remarks></remarks>
Public Class VisitorStatisticsHelper

#Region "Private fields"

    ''' <summary>
    ''' Static instance of the logger object
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _logger As IActionLogger
    ' ''' <summary>
    ' ''' Logger type
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Shared _loggerType As System.Type

    ''' <summary>
    ''' Log 4 Net logging
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _log4net As log4net.ILog = log4net.LogManager.GetLogger(GetType(VisitorStatisticsHelper))

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Log action event
    ''' </summary>
    ''' <param name="context">Context for the event was raised</param>
    ''' <param name="userid">User id</param>
    ''' <param name="lang">Language</param>
    ''' <param name="database">Database id</param>
    ''' <param name="e">PxActionEvent arguments</param>
    ''' <remarks></remarks>
    Public Shared Sub LogEvent(ByVal context As ActionContext, ByVal userid As String, ByVal lang As String, ByVal database As String, ByVal e As PxActionEventArgs)
        If _logger Is Nothing Then
            CreateLogger()
        End If

        If Not _logger Is Nothing Then
            Try
                _logger.LogEvent(context, userid, lang, database, e)
            Catch ex As SqlClient.SqlException
                _log4net.ErrorFormat("Failed to log px-event for database {0} : {1}", database, ex.Message)
            End Try
        End If
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Logger object
    ''' </summary>
    ''' <value></value>
    ''' <returns>The logger object</returns>
    ''' <remarks></remarks>
    Public Shared Property Logger() As IActionLogger
        Get
            If _logger Is Nothing Then
                CreateLogger()
            End If

            Return _logger
        End Get
        Set(ByVal value As IActionLogger)
            _logger = value
        End Set
    End Property

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Creates the logger object. Tries to get logger type from config file. If it is not found there the default logger is created.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub CreateLogger()
        Dim loggerTypeStr As String = ""
        Dim loggerType As System.Type

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("visitorStatisticsLogger")) Then
            loggerTypeStr = System.Configuration.ConfigurationManager.AppSettings("visitorStatisticsLogger")
            Try
                loggerType = System.Type.GetType(loggerTypeStr)
                _logger = CType(Activator.CreateInstance(loggerType), IActionLogger)
                _log4net.Info("Visitor statistics logger of type  '" & loggerType.ToString() & "' was created successfully")
            Catch ex As Exception
                _log4net.Info("Unabled to create visitor statistics logger of type  '" & loggerTypeStr & "'")
            End Try
        Else
            _logger = New PxDefaultLogger()
            _log4net.Info("Visitor statistics logger of type 'PxDefaultLogger' was created successfully")
        End If

    End Sub

#End Region

End Class


