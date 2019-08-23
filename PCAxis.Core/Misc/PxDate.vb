Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Runtime.CompilerServices

Namespace PCAxis.Paxiom.Extensions

    ''' <summary>
    ''' Class representing a PX date
    ''' </summary>
    ''' <remarks>
    ''' Dates in PC-Axis shall have the following format:
    ''' yyyyMMdd HH:mm
    ''' Year (4 characters), month (2), day (2), hour (2), colon (:), minute (2)
    ''' </remarks>
    Public Module PxDate
        '        Private ReadOnly _regex As Regex = New Regex("^\d{8} \d{2}:\d{2}$", RegexOptions.Compiled)
        'Shall have format yyyyMMdd hh:mm
        Private ReadOnly _regex As Regex = New Regex("^(19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01]) (([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$", RegexOptions.Compiled)

        ''' <summary>
        ''' Checks if a string follows the pattern of a PX date
        ''' </summary>
        ''' <param name="dateString">Date string to check</param>
        ''' <returns>True if it is a legal PX date, else false</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function IsPxDate(ByVal dateString As String) As Boolean
            If String.IsNullOrEmpty(dateString) Then
                Return False
            End If

            Return _regex.IsMatch(dateString)
        End Function

        <Extension()>
        Public Function PxDateStringToDateTime(ByVal value As String) As DateTime
            If Not value.IsPxDate() Then
                Return DateTime.MaxValue
            End If

            Dim dt As DateTime

            If Not DateTime.TryParseExact(value, PXConstant.PXDATEFORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, dt) Then
                Return DateTime.MaxValue
            End If

            Return dt

        End Function

        <Extension()>
        Public Function DateTimeToPxDateString(ByVal value As DateTime) As String
            If value = DateTime.MinValue Then Return ""

            Return value.ToString("yyyyMMdd HH:mm")
        End Function
    End Module
End Namespace