Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Operation that sort the timevariable ascending or descending
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SortTimeVariable
        Implements PCAxis.Paxiom.IPXOperation

        ''' <summary>
        ''' Operation for sorting time variable
        ''' </summary>
        ''' <param name="model">The PX model object to sort</param>
        ''' <param name="rhs">Operation description object</param>
        ''' <returns>A PX model with sorted time variable</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal model As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
            If Not TypeOf rhs Is SortTimeVariableDescription Then
                Throw New PXOperationException("SortTimeVariableDescription required")
            End If

            Return Execute(model, CType(rhs, SortTimeVariableDescription))
        End Function

        ''' <summary>
        ''' Operation for sorting time variable
        ''' </summary>
        ''' <param name="model">The PX model object to sort</param>
        ''' <param name="rhs">SortTimeVariableDescription object</param>
        ''' <returns>A PX model with sorted time variable</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal model As PXModel, ByVal rhs As SortTimeVariableDescription) As PXModel
            For Each variable As Variable In model.Meta.Variables
                If variable.IsTime Then
                    model = SortTimeValues(model, variable, rhs)
                    Exit For
                End If
            Next

            Return model
        End Function

        Private Function SortTimeValues(ByVal model As PXModel, ByVal variable As Variable, ByVal rhs As SortTimeVariableDescription) As PXModel
            Dim vals As System.Linq.IOrderedEnumerable(Of Value) = Nothing
            Dim indexes As Integer() = New Integer(variable.Values.Count - 1) {}
            Dim i As Integer
            Dim j As Integer
            Dim orderDesc As ChangeValueOrderDescription
            Dim op As ChangeValueOrder

            Select Case rhs.SortOrder
                Case SortTimeVariableDescription.SortOrderType.Ascending
                    vals = From v In variable.Values Select v Order By v.TimeValue
                Case SortTimeVariableDescription.SortOrderType.Descending
                    vals = From v In variable.Values Select v Order By v.TimeValue Descending
            End Select

            i = 0
            For Each val As Value In variable.Values
                j = 0
                For Each v As Value In vals
                    If v.Equals(val) Then
                        indexes(i) = j
                        Exit For
                    End If
                    j = j + 1
                Next
                i = i + 1
            Next

            orderDesc = New ChangeValueOrderDescription(variable.Code, indexes)
            op = New ChangeValueOrder
            model = op.Execute(model, orderDesc)

            Return model
        End Function

    End Class
End Namespace