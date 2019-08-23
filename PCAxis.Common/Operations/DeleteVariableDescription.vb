Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Description class for the Delete variable operation implemented in the DeleteVariable class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DeleteVariableDescription
        ''' <summary>
        ''' Code of the variable to delete from the model
        ''' </summary>
        ''' <remarks></remarks>
        Public variableCode As String
        ''' <summary>
        ''' Code of the value to use as the elimination value for the deleted variable
        ''' </summary>
        ''' <remarks></remarks>
        Public valueCode As String
        ''' <summary>
        ''' If the name of the value described by the member valueCode should be added to PXMeta.Contents or not
        ''' </summary>
        ''' <remarks></remarks>
        Public addToContents As Boolean

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="varCode">Code of the variable to delete</param>
        ''' <param name="valCode">Code of the value to use as the elimination value for the deleted variable</param>
        ''' <param name="contentsAdd">If the name of the value described by parameter valCode should be added to PXMeta.Contents or not</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal varCode As String, ByVal valCode As String, ByVal contentsAdd As Boolean)
            variableCode = varCode
            valueCode = valCode
            addToContents = contentsAdd
        End Sub
    End Class

End Namespace
