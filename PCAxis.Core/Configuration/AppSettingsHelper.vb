Namespace PCAxis.Paxiom.Configuration

    Public Module AppSettingsHelper

        Public Function GetAppSettingsPath(setting As String) As String
            Dim p As String = System.Configuration.ConfigurationManager.AppSettings(setting)

            If setting Is Nothing Then
                Return Nothing
            End If

            If System.IO.Path.IsPathRooted(p) Then
                Return p
            End If
            p = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p)

            Return System.IO.Path.GetFullPath((New Uri(p)).LocalPath)
        End Function

    End Module

End Namespace
