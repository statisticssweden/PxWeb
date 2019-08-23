Namespace PCAxis.Paxiom

    Public Class PaxiomUtil
        Public Shared Sub SetCode(ByVal value As Value, ByVal code As String)
            value.SetCode(code)
        End Sub

        Public Shared Sub SetData(ByVal model As PXModel, ByVal data As PXData)
            model.SetData(data)
        End Sub

        Public Shared Sub SetLanguage(ByVal variable As Variable, ByVal languageIndex As Integer)
            variable.SetLanguage(languageIndex)
        End Sub

        Public Shared Sub ResizeLanguageVariables(ByVal variable As Variable, ByVal size As Integer)
            variable.ResizeLanguageVariables(size)
        End Sub


    End Class

End Namespace
