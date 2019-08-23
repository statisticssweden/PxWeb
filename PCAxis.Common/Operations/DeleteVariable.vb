Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Operation that deletes a variable from a PXModel.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DeleteVariable
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Implementation of the IPXOperation Execute function
        ''' </summary>
        ''' <param name="lhs">The input model</param>
        ''' <param name="rhs">Description about the operation</param>
        ''' <returns>A new PXModel where the variable has been deleted</returns>
        ''' <remarks>Throws an exception if the description parameter rhs is not of the type DeleteVariableDescription</remarks>
        Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is DeleteVariableDescription Then
                Throw New PXOperationException("DeleteVariableDescription required")
            End If

            Return Execute(lhs, CType(rhs, DeleteVariableDescription))
        End Function

        ''' <summary>
        ''' Executes the variable delete operation
        ''' </summary>
        ''' <param name="oldModel">The input model</param>
        ''' <param name="rhs">Description of which variable to delete</param>
        ''' <returns>A new PXModel where the variable has been deleted</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal oldModel As PXModel, ByVal rhs As DeleteVariableDescription) As PXModel
            Dim newModel As PXModel
            Dim newMeta As PXMeta
            Dim newData As PXData
            Dim variable As Variable
            Dim value As Value
            Dim elims As New List(Of Operations.EliminationDescription)
            Dim elimDesc As Operations.EliminationDescription
            Dim elim As Operations.Elimination

            newMeta = oldModel.Meta.CreateCopy
            newData = oldModel.Data.CreateCopy

            'Create new model to return
            newModel = New PXModel(newMeta, newData)

            variable = newModel.Meta.Variables.GetByCode(rhs.variableCode)

            If variable Is Nothing Then
                Throw New PXOperationException("Cannot find variable")
            End If

            value = variable.Values.GetByCode(rhs.valueCode)

            If value Is Nothing Then
                Throw New PXOperationException("Cannot find value")
            End If

            If rhs.addToContents Then
                Dim selectedLanguageIndex As Integer
                Dim langs() As String = newModel.Meta.GetAllLanguages()

                If langs Is Nothing Then
                    If langs Is Nothing Then
                        'Only default language exists - Add it to langs
                        ReDim langs(0)
                        langs(0) = newModel.Meta.Language
                    End If
                End If

                'Get selected language index so we can restore it 
                selectedLanguageIndex = newModel.Meta.CurrentLanguageIndex

                'Change contents for all languages
                For i As Integer = 0 To langs.Length - 1
                    newModel.Meta.SetLanguage(i)
                    newModel.Meta.Contents = newModel.Meta.Contents & ", " & value.Value
                Next

                'Restore selected language
                newModel.Meta.SetLanguage(selectedLanguageIndex)
            End If

            variable.EliminationValue = value
            variable.Elimination = True

            elimDesc = New Operations.EliminationDescription(variable.Code, True)
            elims.Add(elimDesc)
            elim = New Operations.Elimination

            newModel = elim.Execute(newModel, elims.ToArray)

            newModel.Meta.CreateTitle()
            newModel.Meta.Prune()
            newModel.IsComplete = True
            Return newModel
        End Function

    End Class

End Namespace


