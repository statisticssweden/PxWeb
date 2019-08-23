Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class for handling variables that do not belong to a PXMeta object (special case!!!)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VariableHelper

        ''' <summary>
        ''' Delete alla languages from variable except the language with the given language index
        ''' </summary>
        ''' <param name="var">Variable object</param>
        ''' <param name="langIndex">Index of language to keep</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteAllLanguagesExceptOne(ByVal var As Variable, ByVal langIndex As Integer)
            If langIndex > var.m_name.Length - 1 Then
                'Index out of range - exit sub
                Exit Sub
            End If

            var.mLanguageIndex = langIndex
            var.SetCurrentLanguageDefault()
            var.SetLanguage(0)
            var.ResizeLanguageVariables(1)
        End Sub

    End Class

End Namespace
