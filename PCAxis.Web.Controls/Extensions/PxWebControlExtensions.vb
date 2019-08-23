Imports System.Runtime.CompilerServices
Imports PCAxis.Web.Core.Management
Imports PCAxis.Paxiom.Extensions
Imports System.Web

Public Module PxWebControlExtensions

    ''' <summary>
    ''' Get a language dependent formatted date
    ''' </summary>
    ''' <param name="dateString">String the date</param>
    ''' <returns>Date formatted correctly according to the date format of the currently selected lnguage</returns>
    ''' <remarks>The dateString must be in the PxDate format. If it is not the function returns the dateString unformatted</remarks>
    <Extension()> _
    Public Function PxDate(ByVal dateString As String, Optional ByVal admin As Boolean = False) As String
        Dim dt As DateTime
        Dim year, month, day, hour, minute As Integer
        Dim lang As String
        Dim format As String

        If dateString.IsPxDate() Then
            year = Integer.Parse(dateString.Substring(0, 4))
            month = Integer.Parse(dateString.Substring(4, 2))
            day = Integer.Parse(dateString.Substring(6, 2))
            hour = Integer.Parse(dateString.Substring(9, 2))
            minute = Integer.Parse(dateString.Substring(12, 2))

            dt = New DateTime(year, month, day, hour, minute, 0)
            If admin Then
                lang = HttpContext.Current.Session("adminlang").ToString()
            Else
                lang = LocalizationManager.CurrentCulture.Name
            End If
            format = PCAxis.Paxiom.Settings.GetLocale(lang).DateFormat

            Return dt.ToString(format, Globalization.CultureInfo.InvariantCulture)
        Else
            Return dateString
        End If
    End Function

End Module
