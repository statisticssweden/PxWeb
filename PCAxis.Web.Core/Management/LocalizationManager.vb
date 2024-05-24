Imports System.Globalization
Imports System.Web
Imports PCAxis.Paxiom.Localization
Imports PCAxis.Web.Core.Interfaces
Imports PCAxis.Web.Core.StateProvider

Namespace Management
    ''' <summary>
    ''' Handles the localization for webcontrols
    ''' </summary>
    ''' <remarks>Uses the <see cref="PCAxis.Paxiom.Localization.PxResourceManager"/> for managing the localization</remarks>
    Public NotInheritable Class LocalizationManager
        'Constants
        Private Const KEY_CURRENTCULTURE As String = "CurrentCulture"
        Private Const LANGUAGE_CONTROLS_COLLECTION As String = "LanguageControlsCollection"

        'Default language is set to English
        Private Shared _defaultLanguage As String = "en"

        Private Sub New()

        End Sub

        Shared Sub New()
            Try
                'Get default language from web.config if the setting is present
                If PCAxis.Web.Core.Configuration.ConfigurationHelper.GeneralSettingsSection IsNot Nothing Then
                    If PCAxis.Web.Core.Configuration.ConfigurationHelper.GeneralSettingsSection.DefaultLanguage IsNot Nothing Then
                        _defaultLanguage = PCAxis.Web.Core.Configuration.ConfigurationHelper.GeneralSettingsSection.DefaultLanguage
                    End If
                End If

            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' The default language
        ''' </summary>
        ''' <value>The default language</value>
        ''' <returns>The default language</returns>
        ''' <remarks>Setting the property will override the web.config setting for default language</remarks>
        Public Shared Property DefaultLanguage() As String
            Get
                Return _defaultLanguage
            End Get
            Set(ByVal value As String)
                _defaultLanguage = value
            End Set
        End Property




        ''' <summary>
        ''' Gets a <see cref="List(Of ILanguageControl)" /> of all the webcontrols currently loaded
        ''' for this request that implements <see cref="ILanguageControl" />
        ''' </summary>
        ''' <value>A a <see cref="List(Of ILanguageControl)" /> webcontrols currently loaded
        ''' for this request that implements <see cref="ILanguageControl" /></value>
        ''' <returns>A <see cref="List(Of ILanguageControl)" /></returns>
        ''' <remarks></remarks>
        Friend Shared ReadOnly Property LanguageControls() As List(Of ILanguageControl)
            Get
                If HttpContext.Current IsNot Nothing Then
                    If HttpContext.Current.Items(LANGUAGE_CONTROLS_COLLECTION) IsNot Nothing Then
                        Return CType(HttpContext.Current.Items(LANGUAGE_CONTROLS_COLLECTION), List(Of ILanguageControl))
                    Else
                        HttpContext.Current.Items(LANGUAGE_CONTROLS_COLLECTION) = New List(Of ILanguageControl)()
                        Return (CType(HttpContext.Current.Items(LANGUAGE_CONTROLS_COLLECTION), List(Of ILanguageControl)))
                    End If
                Else
                    Return New List(Of ILanguageControl)()
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the current culture for the <see cref="LocalizationManager" />
        ''' </summary>
        ''' <value>The <see cref="CultureInfo" /> of the current culture for the <see cref="LocalizationManager" /></value>
        ''' <returns>A <see cref="CultureInfo" /> representing the current culture</returns>
        ''' <remarks></remarks>
        Public Shared Property CurrentCulture() As CultureInfo
            Get
                If StateProviderFactory.GetStateProvider().Contains(GetType(LocalizationManager), KEY_CURRENTCULTURE) _
                AndAlso StateProviderFactory.GetStateProvider().Item(GetType(LocalizationManager), KEY_CURRENTCULTURE) IsNot Nothing Then
                    Return CType(StateProviderFactory.GetStateProvider().Item(GetType(LocalizationManager), KEY_CURRENTCULTURE), CultureInfo)
                End If

                'Dim defaultLanguage As String = PCAxis.Web.Core.Configuration.ConfigurationHelper.GeneralSettingsSection.DefaultLanguage
                Dim Culture As CultureInfo = CultureInfo.CreateSpecificCulture(DefaultLanguage)
                If StateProviderFactory.GetStateProvider().Contains(GetType(LocalizationManager), KEY_CURRENTCULTURE) Then
                    StateProviderFactory.GetStateProvider().Item(GetType(LocalizationManager), KEY_CURRENTCULTURE) = Culture
                Else
                    StateProviderFactory.GetStateProvider().Add(GetType(LocalizationManager), KEY_CURRENTCULTURE, Culture)
                End If

                Return Culture
            End Get
            Private Set(ByVal value As CultureInfo)
                If StateProviderFactory.GetStateProvider().Contains(GetType(LocalizationManager), KEY_CURRENTCULTURE) Then
                    StateProviderFactory.GetStateProvider().Item(GetType(LocalizationManager), KEY_CURRENTCULTURE) = value
                Else
                    StateProviderFactory.GetStateProvider().Add(GetType(LocalizationManager), KEY_CURRENTCULTURE, value)
                End If
            End Set
        End Property


        ''' <summary>
        ''' Changes the language for all controls based on ControlBase
        ''' </summary>
        ''' <param name="language">The name of the language to  hancge to</param>
        ''' <remarks></remarks>
        Public Shared Sub ChangeLanguage(ByVal language As String)
            'ChangeLanguage(CultureInfo.CreateSpecificCulture(language))
            Dim ci As CultureInfo

            Try
                ci = New CultureInfo(language)
            Catch ex As Exception
                Exit Sub
            End Try

            ChangeLanguage(New CultureInfo(language))
        End Sub


        ''' <summary>
        ''' Changes the language for all controls based on ControlBase
        ''' </summary>
        ''' <param name="culture">A <see cref="CultureInfo" /> representing the language to change to</param>
        ''' <remarks></remarks>
        Public Shared Sub ChangeLanguage(ByVal culture As CultureInfo)
            CurrentCulture = culture
            For Each control As ILanguageControl In LocalizationManager.LanguageControls
                control.CurrentCulture = culture
            Next
        End Sub

        ''' <summary>
        ''' Gets the localized string for the supplied key and culture
        ''' </summary>
        ''' <param name="key">The key to retrieve a localized string for</param>
        ''' <param name="culture">The <see cref="CultureInfo" /> to use when retrieving the localized string</param>
        ''' <returns>A <see cref="String" /> with the localized content</returns>
        ''' <remarks>Returns the <paramref name="key" /> if there is no localized content</remarks>
        Public Shared Function GetLocalizedString(ByVal key As String, ByVal culture As CultureInfo) As String
            Return GetResourceManager().GetString(key, culture)
        End Function

        ''' <summary>
        ''' Gets the localized string for the supplied key using the <see cref="CurrentCulture" /> of the <see cref="LocalizationManager" />
        ''' </summary>
        ''' <param name="key">The key to retrieve a localized string for</param>        
        ''' <returns>A <see cref="String" /> with the localized content</returns>
        ''' <remarks>Returns the <paramref name="key" /> if there is no localized content</remarks>
        Public Shared Function GetLocalizedString(ByVal key As String) As String
            Return GetLocalizedString(key, CurrentCulture)
        End Function
        ''' <summary>
        ''' Enables to programmatically add a language to the <see cref="PxResourceManager" />
        ''' </summary>
        ''' <param name="language">The <see cref="Language" /> to add</param>
        ''' <remarks></remarks>
        'Public Shared Sub LoadLanguage(ByVal language As Language)
        '    GetResourceManager().LoadLanguage(language)
        'End Sub

        ''' <summary>
        ''' Gets an instance <see cref="PxResourceManager" />
        ''' </summary>
        ''' <returns>An instance of <see cref="PxResourceManager" /></returns>
        ''' <remarks></remarks>
        Private Shared Function GetResourceManager() As PxResourceManager
            Return PxResourceManager.GetResourceManager()
        End Function

        ''' <summary>
        ''' Resets the <see cref="PxResourceManager" /> and enables reloading of the languages
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ResetResourceManager()
            PxResourceManager.ResetResourceManager()
        End Sub

        'Public Shared Function GetTwoLetterLanguageCode(ByVal Culture As CultureInfo) As String
        '    If Culture.Name = "bs-Latn-BA" Then Return "bs-Latn-BA"
        '    If Culture.Name = "hr-BA" Then Return "hr-BA"
        '    If Culture.Name = "sr-Cyrl-BA" Then Return "sr-Cyrl-BA"
        '    If Culture.TwoLetterISOLanguageName = "iv" Then Return ""
        '    If Culture.TwoLetterISOLanguageName = "zh" Then Return "zh-TW" 'Fix for handling Chinese Taiwan in .Net 3.5
        '    Return Culture.TwoLetterISOLanguageName
        'End Function

        Public Shared Function GetTwoLetterLanguageCode() As String
            'Return GetTwoLetterLanguageCode(CurrentCulture)
            Return CurrentCulture.Name
        End Function

    End Class
End Namespace
