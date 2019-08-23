

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls

<ToolboxData("<{0}:footnote runat=""server""></{0}:footnote>")> _
Public Class FootnoteCodebehind
    Inherits PaxiomControlBase(Of FootnoteCodebehind, Footnote)
    Private Const LOC_FOOTNOTE_LABEL As String = "CtrlFootnoteFootnoteLabel"
    Private Const LOC_FOOTNOTE_NO_FOOTNOTES_LABEL As String = "CtrlFootnoteNoFootnotesLabel"

    Protected WithEvents FootnoteRepeater As Repeater
    Protected NoFootnotesExist As Label

    Private Const CSS_PREFIX As String = "footnote_"
    Private Const CSS_SUFFIX_HEADER As String = "_header"
    Private Const CSS_SUFFIX_KEY As String = "_key"
    Private Const CSS_SUFFIX_VALUE As String = "_value"
    Private Const CSS_SUFFIX_MANDATORY As String = "footnote_mandatory"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If FootnoteRepeater.DataSource Is Nothing Then
            GetFootNotes()
        End If
    End Sub


    ''' <summary>
    ''' Checks if the language is changed and get footnotes
    ''' </summary>
    Private Sub Footnote_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        GetFootNotes()
    End Sub

    ''' <summary>
    ''' Sets up everything and renders the page.
    ''' </summary>
    Friend Sub GetFootNotes()
        If Me.PaxiomModel IsNot Nothing Then
            Dim footnotes As List(Of FootnoteListItem)

            If Marker.ShowMandatoryOnly Then
                footnotes = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, True)
                FootnoteRepeater.DataSource = footnotes
                If footnotes.Count = 0 Then
                    If Marker.ShowNoFootnotes AndAlso footnotes.Count = 0 Then
                        Me.NoFootnotesExist.Text = GetLocalizedString(LOC_FOOTNOTE_NO_FOOTNOTES_LABEL)
                        Me.NoFootnotesExist.Visible = True
                        Me.FootnoteRepeater.Visible = True
                    Else
                        Me.NoFootnotesExist.Visible = False
                        Me.FootnoteRepeater.Visible = False
                    End If
                End If

            Else
                footnotes = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, False)
                FootnoteRepeater.DataSource = footnotes
                If footnotes.Count = 0 Then
                    If Marker.ShowNoFootnotes Then
                        Me.NoFootnotesExist.Text = GetLocalizedString(LOC_FOOTNOTE_NO_FOOTNOTES_LABEL)
                        Me.NoFootnotesExist.Visible = True
                        Me.FootnoteRepeater.Visible = True
                    Else
                        Me.NoFootnotesExist.Visible = False
                        Me.FootnoteRepeater.Visible = False
                    End If
                End If
            End If

            FootnoteRepeater.DataBind()

        End If
    End Sub


    ''' <summary>
    ''' Gets tag and css class for a specific footnote type.
    ''' </summary>
    Private Function GetTagAndCssClass(ByVal li As FootnoteListItem, ByVal isKey As Boolean) As String
        If li.Header Then
            Return IIf(isKey, "<dt", "<dd").ToString & " class=""" & CSS_PREFIX & li.FootnoteType.ToString().ToLower() & CSS_SUFFIX_HEADER.ToString().ToLower() & " " & IIf(li.Mandatory, CSS_SUFFIX_MANDATORY, "").ToString() & """>"
        Else
            Return IIf(isKey, "<dt", "<dd").ToString & " class=""" & CSS_PREFIX & li.FootnoteType.ToString().ToLower() & IIf(isKey, CSS_SUFFIX_KEY, CSS_SUFFIX_VALUE).ToString() & " " & IIf(li.Mandatory, CSS_SUFFIX_MANDATORY, "").ToString() & """>"
        End If
    End Function

    ''' <summary>
    ''' Helper used to update literal in repeater
    ''' </summary>
    Private Sub SetLiteralText(ByVal e As RepeaterItemEventArgs, ByVal literalId As String, ByVal text As String)
        Dim literal As Literal = DirectCast(e.Item.FindControl(literalId), Literal)        
        literal.Text = text.Replace(vbCrLf, "<br />")
    End Sub

    ''' <summary>
    ''' Updates the page when something is changed in the paxiom model.
    ''' </summary>
    Private Sub Footnote_PaxiomModelChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PaxiomModelChanged
        GetFootNotes()
    End Sub

    ''' <summary>
    ''' Gets tag and css class for a specific footnote type.
    ''' </summary>
    Private Sub FootnoteRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles FootnoteRepeater.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Header
                SetLiteralText(e, "Header", GetLocalizedString(LOC_FOOTNOTE_LABEL))
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim li As FootnoteListItem = DirectCast(e.Item.DataItem, FootnoteListItem)
                SetLiteralText(e, "MainTerm", li.Term)
                SetLiteralText(e, "MainTermTagAndCssClass", GetTagAndCssClass(li, True))
                SetLiteralText(e, "MainDefinition", li.Definition)
                SetLiteralText(e, "MainDefinitionTagAndCssClass", GetTagAndCssClass(li, False))
        End Select
    End Sub

End Class