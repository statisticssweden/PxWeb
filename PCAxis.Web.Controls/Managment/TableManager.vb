Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports PCAxis.Web.Core.StateProvider
Imports PCAxis.Chart
Imports PCAxis.Web.Controls

Public NotInheritable Class TableManager

    Const TABLE_SETTINGS As String = "TableSettings"

    Public Shared Property Settings() As TableSettings
        Get
            'Fetch the current model from the stateprovider
            Dim s As TableSettings = CType(StateProviderFactory.GetStateProvider().Item(GetType(TableSettings), TABLE_SETTINGS), TableSettings)
            If s Is Nothing Then
                s = New TableSettings()
                TableManager.Settings = s
                If _initializer IsNot Nothing Then
                    _initializer(s)
                End If
            End If
            Return s
        End Get
        Set(ByVal value As TableSettings)
            StateProviderFactory.GetStateProvider().Add(GetType(TableSettings), TABLE_SETTINGS, value)
        End Set
    End Property

    Public Delegate Sub InitializeSettings(ByVal settings As TableSettings)

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
