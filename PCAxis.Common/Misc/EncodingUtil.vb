Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Utility class for handling of encodings
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EncodingUtil

        ''' <summary>
        ''' Gets the encoding object by the name.
        ''' </summary>
        ''' <param name="name">The name of the encoding</param>
        ''' <returns>a Encoding object</returns>
        ''' <remarks>
        ''' If the rigth encoding object is not found then the default encoding will
        ''' be returned
        ''' </remarks>
        Public Shared Function GetEncoding(ByVal name As String) As System.Text.Encoding
            Dim enc As System.Text.Encoding = System.Text.Encoding.Default
            If Not String.IsNullOrEmpty(name) Then
                Try
                    enc = System.Text.Encoding.GetEncoding(name)
                Catch ex As ArgumentException
                    Dim Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(EncodingUtil))
                    Logger.Warn(ex)
                End Try
            End If
            Return enc
        End Function
    End Class

End Namespace
