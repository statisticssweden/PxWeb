Imports PCAxis.Web.Core.Exceptions
Namespace Management

    ''' <summary>
    ''' Class for checking querystrings
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QuerystringManager

        Private Shared lstIllegalValues As List(Of String)
        ''' <summary>
        ''' Object to control creation of the lstIllegalValues list in a multithreaded environment 
        ''' </summary>
        Private Shared checkValueLock As New Object()

        ''' <summary>
        ''' Get querystring parameter value. 
        ''' The parameter value is checked for illegal characters
        ''' </summary>
        ''' <param name="param">Querystring parameter to get value from</param>
        ''' <returns>The querystring parameter value</returns>
        ''' <remarks>If the parameter contains illegal characters a InvalidQuerystringParameterException is thrown</remarks>
        Public Shared Function GetQuerystringParameter(ByVal param As String) As String
            Return QuerystringManager.GetQuerystringParameter(System.Web.HttpContext.Current, param)
        End Function

        ''' <summary>
        ''' Get querystring parameter value. 
        ''' The parameter value is checked for illegal characters
        ''' </summary>
        ''' <param name="context">An HttpContext object that provides references to the intrinsic server objects</param>
        ''' <param name="param">Querystring parameter to get value from</param>
        ''' <returns>The querystring parameter value</returns>
        ''' <remarks>If the parameter contains illegal characters a InvalidQuerystringParameterException is thrown</remarks>
        Public Shared Function GetQuerystringParameter(ByVal context As System.Web.HttpContext, ByVal param As String) As String
            Dim value As String

            If context Is Nothing Then
                Return Nothing
            End If

            value = context.Request.QueryString(param)

            If value Is Nothing Then
                Return value
            End If

            value = System.Web.HttpUtility.UrlDecode(value)

            If (QuerystringManager.CheckValue(value)) Then
                Return value
            Else
                Return ""
            End If

        End Function

        ''' <summary>
        ''' If the parameter contains illegal characters a InvalidQuerystringParameterException is thrown
        ''' </summary>
        ''' <param name="value">string to check for illegal characters</param>
        ''' <returns>True if the value do not contain any illegal characters, else false</returns>
        ''' <remarks>If the value contains illegal characters a InvalidQuerystringParameterException is thrown</remarks>
        Public Shared Function CheckValue(ByVal value As String) As Boolean
            Dim notValidCharacter As String() = Nothing

            'Check that the parameter do not contain illegal characters
            If Not value Is Nothing Then
                value = System.Web.HttpUtility.UrlDecode(value)


                If lstIllegalValues Is Nothing Then

                    SyncLock checkValueLock

                        If lstIllegalValues Is Nothing Then

                            'Read list of illegal characters
                            lstIllegalValues = New List(Of String)()

                            'Check if the key in web.config exists
                            If Not System.Configuration.ConfigurationManager.AppSettings("characterBlackList") Is Nothing Then
                                notValidCharacter = System.Configuration.ConfigurationManager.AppSettings("characterBlackList").ToString().Split(CChar("|"))
                            End If

                            If Not notValidCharacter Is Nothing Then
                                lstIllegalValues.AddRange(notValidCharacter)
                            End If

                        End If

                    End SyncLock

                End If
                'Check if the value of the parameter contains any of the illegal charecters 
                'If so throw an exception
                If lstIllegalValues.Any(Function(lstIllegalValue) value.ToLower().Contains(lstIllegalValue)) Then
                    Throw New InvalidQuerystringParameterException("Illegal characters in querystring parameter")
                End If
            End If

            Return True
        End Function
    End Class
End Namespace
