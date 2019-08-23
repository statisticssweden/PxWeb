Imports PCAxis.Web.Core.Exceptions
Namespace Management

    ''' <summary>
    ''' Class for validating input values
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValidationManager

        ''' <summary>
        ''' Verifies that the given value contains no illegal characters or strings
        ''' </summary>
        ''' <param name="value">string to check</param>
        ''' <returns>True if the string do not contain any illegal characters, else false</returns>
        ''' <remarks>Uses the QuerystringManager to perform the actual check</remarks>
        Public Shared Function CheckValue(ByVal value As String) As Boolean
            Try
                If QuerystringManager.CheckValue(value) Then
                    Return True
                End If
            Catch ex As InvalidQuerystringParameterException
                Return False
            Catch ex As Exception
                Return False
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Get the given value. Checks that the value contains no illegal characters.
        ''' </summary>
        ''' <param name="value">Value to get</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetValue(ByVal value As String) As String
            If QuerystringManager.CheckValue(value) Then
                Return value
            Else
                Return ""
            End If
        End Function

    End Class

End Namespace
