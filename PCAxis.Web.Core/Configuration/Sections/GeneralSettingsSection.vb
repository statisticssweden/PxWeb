Imports System.Configuration
Namespace Configuration.Sections
    ''' <summary>
    ''' Configurationsection for the generalsettings in web.config
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GeneralSettingsSection
        Inherits ConfigurationSection

        Const CONFIG_DEFAULTLANGUAGE As String = "defaultlanguage"

        ''' <summary>
        ''' Gets or sets the default language in web.config
        ''' </summary>
        ''' <value>Two letter abbreviation of the language. i.e. 'sv' or 'en'</value>
        ''' <returns>A <see cref="String" /> representing the default language</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_DEFAULTLANGUAGE, IsRequired:=True)> _
        Public Property DefaultLanguage() As String
            Get
                Return Me(CONFIG_DEFAULTLANGUAGE).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_DEFAULTLANGUAGE) = value
            End Set
        End Property

    End Class
End Namespace
