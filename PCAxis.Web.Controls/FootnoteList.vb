Imports PCAxis.Paxiom

Public Class FootnoteList
    ''' <summary>
    ''' Gets a list with all footnotes. Used by the footnotes webcontrol.
    ''' </summary>
    ''' <param name="showMandatoryOnly">If true only mandatory footnotes is returned. If false all footnotes is returned.</param>
    ''' <returns>All footnotes or if specified only the mandatory.</returns>
    Public Shared Function GetFootnoteList(ByVal meta As PXMeta, ByVal showMandatoryOnly As Boolean) As List(Of FootnoteListItem)
        Dim footnoteList As New List(Of FootnoteListItem)
        'NOTE/X
        AddFootnotes(footnoteList, Enums.FootnoteType.Note, "", meta.Notes, showMandatoryOnly)
        'NOTE/X("Variable")
        For Each variable As Paxiom.Variable In meta.Variables
            AddFootnotes(footnoteList, Enums.FootnoteType.VarNote, variable.Name, variable.Notes, showMandatoryOnly)
        Next
        'VALUENOTE/X("Variable","Value")
        For Each variable As Paxiom.Variable In meta.Variables
            AddVariableHeaders(footnoteList, variable, Enums.FootnoteType.ValueNote, showMandatoryOnly)

            For Each value As Paxiom.Value In variable.Values
                'Dim val As Value = variable.Values.GetByCode(value.Code)

                'AddFootnotes(footnoteList, Enums.FootnoteType.ValueNote, variable.Name & ": " & val.Text, value.Notes, showMandatoryOnly)
                AddFootnotes(footnoteList, Enums.FootnoteType.ValueNote, value.Text, value.Notes, showMandatoryOnly)
            Next
        Next
        'CELLNOTE/X
        For Each note As CellNote In meta.CellNotes
            If (showMandatoryOnly And note.Mandatory) OrElse (Not showMandatoryOnly) Then
                Dim condText As String = GetCellNoteConditionsText(meta, note.Conditions)
                AddFootnote(footnoteList, Enums.FootnoteType.CellNote, condText, note.Text, note.Mandatory)
            End If
        Next
        Return footnoteList
    End Function

    ''' <summary>
    ''' Get the text with conditions (comma separated variabel-value list) for the cellnote
    ''' </summary>
    ''' <param name="meta">The PXMeta object</param>
    ''' <param name="conditions">The VariableValuePairs object containing the conditions</param>
    ''' <returns>The conditions text</returns>
    ''' <remarks>The condition for the content variable is supposed to be first in the list of conditions. If it is not the GetCellNoteConditionsText function handles that case</remarks>
    Private Shared Function GetCellNoteConditionsText(ByVal meta As PXMeta, ByVal conditions As VariableValuePairs) As String
        Dim term As New System.Text.StringBuilder()
        Dim val As Value
        Dim first As Boolean = True

        For Each condition As Paxiom.VariableValuePair In conditions
            Dim v As Variable = meta.Variables.GetByCode(condition.VariableCode)

            If v IsNot Nothing Then
                If v.IsContentVariable And Not first Then
                    'This is wrong! The content variable condition should allways be first of the conditions
                    'Create new conditions in the right order and create the text from this instead!
                    Dim newConditions As New VariableValuePairs
                    Dim contentVariableCode As String = condition.VariableCode 'Remember the code of the content variabel

                    newConditions.Add(condition) 'Add the content variable condition

                    'Add all the other conditions
                    For Each cond As Paxiom.VariableValuePair In conditions
                        If Not cond.VariableCode.Equals(contentVariableCode) Then 'The content variable condition is already added and should not be added again...
                            newConditions.Add(cond)
                        End If
                    Next

                    Return GetCellNoteConditionsText(meta, newConditions)
                End If

                val = v.Values.GetByCode(condition.ValueCode)
                If val IsNot Nothing Then
                    If Not first Then
                        term.Append(", ")
                    End If
                    term.Append(v.Name & ": " & val.Text & " ")
                    first = False
                End If
            End If
        Next

        Return term.ToString.Trim()
    End Function

    Private Shared Sub AddFootnotes(ByVal footnoteList As List(Of FootnoteListItem), ByVal footnoteType As Enums.FootnoteType, ByVal term As String, ByVal notes As Paxiom.Notes, ByVal addMandatoryOnly As Boolean)
        If notes IsNot Nothing Then
            For Each note As Note In notes
                If (addMandatoryOnly And note.Mandantory) OrElse (Not addMandatoryOnly) Then
                    AddFootnote(footnoteList, footnoteType, term, note.Text, note.Mandantory)
                End If
            Next
        End If
    End Sub

    Private Shared Sub AddFootnote(ByVal footnoteList As List(Of FootnoteListItem), ByVal footnoteType As Enums.FootnoteType, ByVal term As String, ByVal definition As String, ByVal mandatory As Boolean)
        Dim li As New FootnoteListItem(footnoteType)
        li.Term = term
        li.Definition = definition
        li.Mandatory = mandatory
        li.Header = False

        If mandatory Then
            'Place the footnote last of the mandatory footnotes of the same type
            For i As Integer = 0 To footnoteList.Count - 1
                If ((footnoteList.Item(i).Mandatory = False) And (footnoteList.Item(i).FootnoteType >= footnoteType)) Then
                    footnoteList.Insert(i, li)
                    Exit Sub
                End If
            Next
            footnoteList.Add(li)
        Else
            footnoteList.Add(li)
        End If
    End Sub

    ''' <summary>
    ''' Add headers to group footnotes of a certain type under a variable name in the graphical presentation
    ''' </summary>
    ''' <param name="footnoteList">List with footnotes</param>
    ''' <param name="variable">The variable to group footnotes under</param>
    ''' <param name="footnoteType">Type of footnote</param>
    ''' <param name="showMandatoryOnly">If only mandatory footnotes shall be added</param>
    ''' <remarks></remarks>
    Private Shared Sub AddVariableHeaders(ByVal footnoteList As List(Of FootnoteListItem), ByVal variable As Variable, ByVal footnoteType As Enums.FootnoteType, ByVal showMandatoryOnly As Boolean)
        Dim mandtoryAdded As Boolean = False
        Dim nonMandtoryAdded As Boolean = False

        For Each val As Value In variable.Values
            If Not val.Notes Is Nothing Then
                For Each note As Note In val.Notes
                    If note.Mandantory And Not mandtoryAdded Then
                        Dim li As New FootnoteListItem(footnoteType)
                        li.Term = variable.Name
                        li.Definition = ""
                        li.Mandatory = True
                        li.Header = True
                        'Place the footnote last of the mandatory footnotes of the same type
                        For i As Integer = 0 To footnoteList.Count - 1
                            If ((footnoteList.Item(i).Mandatory = False) And (footnoteList.Item(i).FootnoteType >= footnoteType)) Then
                                footnoteList.Insert(i, li)
                                Exit Sub
                            End If
                        Next
                        footnoteList.Add(li)
                        mandtoryAdded = True
                    ElseIf Not note.Mandantory And Not nonMandtoryAdded Then
                        If Not showMandatoryOnly Then
                            Dim li As New FootnoteListItem(footnoteType)
                            li.Term = variable.Name
                            li.Definition = ""
                            li.Mandatory = False
                            li.Header = True
                            footnoteList.Add(li)
                            nonMandtoryAdded = True
                        End If
                    End If

                    If mandtoryAdded And nonMandtoryAdded Then
                        Exit Sub
                    End If
                Next
            End If
        Next
    End Sub

End Class
