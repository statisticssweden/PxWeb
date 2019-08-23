Namespace PCAxis.Paxiom.Extensions

    ''' <summary>
    ''' Extension methods for Paxiom
    ''' </summary>
    ''' <remarks></remarks>
    Public Module PxExtensions

        ''' <summary>
        ''' Get a localized string from PxResourceManager
        ''' </summary>
        ''' <param name="obj">Can be called from within all objects</param>
        ''' <param name="key">Key for the string to get</param>
        ''' <returns>The localized string defined by the language of the current thread</returns>
        ''' <remarks></remarks>
        <System.Runtime.CompilerServices.Extension()> _
        Public Function GetLocalizedString(ByVal obj As Object, ByVal key As String) As String
            Return PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString(key)
        End Function

        ''' <summary>
        ''' Get a localized string from PxResourceManager
        ''' </summary>
        ''' <param name="meta">PXMeta object</param>
        ''' <param name="key">Key for the string to get</param>
        ''' <returns>The localized string defined by the current language of the PXMeta object</returns>
        ''' <remarks></remarks>
        <System.Runtime.CompilerServices.Extension()> _
        Public Function GetLocalizedString(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal key As String) As String
            Return PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString(key, meta.CurrentLanguage)
        End Function

    End Module

End Namespace
