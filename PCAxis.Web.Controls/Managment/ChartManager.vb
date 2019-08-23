Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports PCAxis.Web.Core.StateProvider
Imports PCAxis.Chart

Public NotInheritable Class ChartManager

    Const CHART_SETTINGS As String = "CharSettings"

    Public Shared Property Settings() As ChartSettings
        Get
            'Fetch the current model from the stateprovider
            Dim s As ChartSettings = CType(StateProviderFactory.GetStateProvider().Item(GetType(ChartSettings), CHART_SETTINGS), ChartSettings)
            If s Is Nothing Then
                s = New ChartSettings()
                ChartManager.Settings = s
                If _initializer IsNot Nothing Then
                    _initializer(s)
                End If
            End If
            Return s
        End Get
        Set(ByVal value As ChartSettings)
            StateProviderFactory.GetStateProvider().Add(GetType(ChartSettings), CHART_SETTINGS, value)
        End Set
    End Property

    Public Delegate Sub InitializeSettings(ByVal settings As ChartSettings)

    Private Shared _initializer As InitializeSettings

    Public Shared Property SettingsInitializer() As InitializeSettings
        Get
            Return _initializer
        End Get
        Set(ByVal value As InitializeSettings)
            _initializer = value
        End Set
    End Property


End Class
