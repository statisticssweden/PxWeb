Imports System.Configuration
Imports PCAxis.Web.Core.Configuration.Sections

Namespace Configuration
    ''' <summary>
    ''' Helper class for easy access to predefined configurationsections in web.config
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConfigurationHelper
        Const CONFIG_STATEPROVIDER As String = "pcaxis/web.core/stateprovider"
        Const CONFIG_GENERALSETTINGS As String = "pcaxis/web.core/generalsettings"

        Private Sub New()

        End Sub

        ''' <summary>
        ''' Gets the StateProviderConfigurationSection of the configuration web.config
        ''' </summary>
        ''' <value>Configuration for the stateprovider system</value>
        ''' <returns>An instance of <see cref="Sections.StateProviderSection" /></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property StateProviderSection() As StateProviderSection
            Get
                Return CType(ConfigurationManager.GetSection(CONFIG_STATEPROVIDER), StateProviderSection)
            End Get
        End Property


        ''' <summary>
        ''' Gets the GeneralSettingsSection of the configuration web.config
        ''' </summary>
        ''' <value>General configurations</value>
        ''' <returns>An instance of <see cref="Sections.GeneralSettingsSection" /></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property GeneralSettingsSection() As GeneralSettingsSection
            Get
                Return CType(ConfigurationManager.GetSection(CONFIG_GENERALSETTINGS), GeneralSettingsSection)
            End Get
        End Property
    End Class
End Namespace
