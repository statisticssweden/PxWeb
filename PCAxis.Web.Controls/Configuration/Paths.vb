Namespace Configuration

    ''' <summary>
    ''' Class for handling paths configured from web.config
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Paths
        ''' <summary>
        ''' path to the images folder
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _imagesPath As String = "~/"

        ''' <summary>
        ''' Shared constructor
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            Try
                If PCAxis.Web.Controls.Configuration.ConfigurationHelper.GeneralSettingsSection IsNot Nothing Then
                    If PCAxis.Web.Controls.Configuration.ConfigurationHelper.GeneralSettingsSection.ImagesPath IsNot Nothing Then
                        _imagesPath = PCAxis.Web.Controls.Configuration.ConfigurationHelper.GeneralSettingsSection.ImagesPath
                    End If
                End If

            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Path to the images folder
        ''' </summary>
        ''' <value>Path to the images folder</value>
        ''' <returns>Path to the images folder</returns>
        ''' <remarks>Setting the property will override the web.config setting for the images path</remarks>
        Public Shared Property ImagesPath() As String
            Get
                Return _imagesPath
            End Get
            Set(ByVal value As String)
                _imagesPath = value
            End Set
        End Property

    End Class

End Namespace

