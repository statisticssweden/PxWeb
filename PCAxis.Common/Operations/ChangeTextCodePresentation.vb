Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for handling Change text/code presentation Operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeTextCodePresentation
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Implementation of IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(lhs As PXModel, rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is ChangeTextCodePresentationDescription Then
                Throw New PXOperationException("Parameter is not supported")
            End If

            Return Execute(lhs, CType(rhs, ChangeTextCodePresentationDescription))
        End Function

        ''' <summary>
        ''' Local Execute function handling the typeCasted rhs
        ''' </summary>
        ''' <param name="oldModel"></param>
        ''' <param name="rhs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(oldModel As PXModel, rhs As ChangeTextCodePresentationDescription) As PXModel
            Dim newModel As PXModel
            Dim newMeta As PXMeta
            Dim newData As PXData
            Dim v As Variable

            newMeta = oldModel.Meta.CreateCopy
            newData = oldModel.Data.CreateCopy

            'Create new model to return
            newModel = New PXModel(newMeta, newData)

            'Variables
            For Each itm As KeyValuePair(Of String, HeaderPresentationType) In rhs.PresentationDictionary
                v = newModel.Meta.Variables.GetByCode(itm.Key)
                v.PresentationText = itm.Value
            Next

            ''Contents and Units
            'Dim selectedLanguageIndex As Integer
            'Dim langs() As String = newModel.Meta.GetAllLanguages()

            'If langs Is Nothing Then
            '    If langs Is Nothing Then
            '        'Only default language exists - Add it to langs
            '        ReDim langs(0)
            '        langs(0) = newModel.Meta.Language
            '    End If
            'End If

            ''Get selected language index so we can restore it 
            'selectedLanguageIndex = newModel.Meta.CurrentLanguageIndex

            ''Change contents for all languages
            'For i As Integer = 0 To langs.Length - 1
            '    newModel.Meta.SetLanguage(i)
            '    newModel.Meta.Contents = rhs.Content
            '    newModel.Meta.ContentInfo.Units = rhs.Units
            'Next

            ''Restore selected language
            'newModel.Meta.SetLanguage(selectedLanguageIndex)

            'newModel.Meta.DeleteAllLanguagesExceptCurrent()

            'Save
            'newModel.Meta.CreateTitle()
            'newModel.Meta.Prune()
            newModel.IsComplete = True

            Return newModel
        End Function
    End Class

End Namespace
