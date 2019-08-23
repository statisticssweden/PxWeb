Imports System.Globalization

Namespace Interfaces
    ''' <summary>
    ''' Defines a control that supports localization
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ILanguageControl

        ''' <summary>
        ''' Gets or sets the current language
        ''' </summary>
        ''' <value>String representing the current language</value>
        ''' <returns>String representing the current language</returns>
        ''' <remarks></remarks>
        Property CurrentCulture() As CultureInfo
    End Interface
End Namespace
