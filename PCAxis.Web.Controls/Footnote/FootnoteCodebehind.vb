

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls
Imports System.Text

<ToolboxData("<{0}:footnote runat=""server""></{0}:footnote>")>
Public Class FootnoteCodebehind
    Inherits PaxiomControlBase(Of FootnoteCodebehind, Footnote)
    Private Const LOC_FOOTNOTE_LABEL As String = "CtrlFootnoteFootnoteLabel"
    Private Const LOC_FOOTNOTE_NO_FOOTNOTES_LABEL As String = "CtrlFootnoteNoFootnotesLabel"

    Protected WithEvents FootnoteRepeater As Repeater
    Protected WithEvents MandatoryFootnoteRepeater As Repeater
    Protected WithEvents NonMandatoryFootnoteRepeater As Repeater
    Protected NoFootnotesExist As Label

    'Protected FootnoteAccordionStart As Literal
    'Protected FootnoteAccordionEnd As Literal

    Private hasNonMandatoryNoteOnTable As Boolean = False

    Private Const CSS_PREFIX As String = "footnote_"
    Private Const CSS_SUFFIX_HEADER As String = "_header"
    Private Const CSS_SUFFIX_KEY As String = "_key"
    Private Const CSS_SUFFIX_VALUE As String = "_value"
    Private Const CSS_SUFFIX_MANDATORY As String = "footnote_mandatory"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If FootnoteRepeater.DataSource Is Nothing And MandatoryFootnoteRepeater.DataSource Is Nothing Then
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
    ''' 
    ''' Change feb. 2021: "Wcag" for footnotes "on page", not in popup: 
    '''        mandatory footnotes: remove h2 header,
    '''        and the nonmandatory ones in an accordion.
    '''        This is "triggered" by Marker.InAccordionStyle
    ''' </summary>
    Friend Sub GetFootNotes()
        If Me.PaxiomModel IsNot Nothing Then
            If Not Marker.InAccordionStyle Then
                Me.MandatoryFootnoteRepeater.Visible = False
                Me.NonMandatoryFootnoteRepeater.Visible = False

                Dim footnotes As List(Of FootnoteListItem)

                If Marker.ShowMandatoryOnly Then
                    footnotes = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, True)
                Else
                    footnotes = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, False)
                End If

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

                FootnoteRepeater.DataBind()
            Else

                Me.FootnoteRepeater.Visible = False
                Me.GetNoteInAccordionStyle()
            End If
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


    Private Sub GetNoteInAccordionStyle()
        ' Mandatory "footnotes" shall be displayed above the footnotes accoridon
        ' Non-Mandatory "footnotes" shall be displayed inside the footnotes accoridon

        Dim mandatoryFootnotes As List(Of FootnoteListItem) = New List(Of FootnoteListItem)
        Dim nonMandatoryFootnotes As List(Of FootnoteListItem) = New List(Of FootnoteListItem)

        If Marker.ShowMandatoryOnly Then
            mandatoryFootnotes = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, True)

        Else
            Dim tempList As List(Of FootnoteListItem) = FootnoteList.GetFootnoteList(Me.PaxiomModel.Meta, False)

            For Each item As FootnoteListItem In tempList
                If item.Mandatory Then
                    mandatoryFootnotes.Add(item)
                Else
                    nonMandatoryFootnotes.Add(item)
                    If item.FootnoteType = Enums.FootnoteType.Note Then
                        hasNonMandatoryNoteOnTable = True
                    End If
                End If
            Next
        End If

        Dim footnotesCount As Integer = nonMandatoryFootnotes.Count + mandatoryFootnotes.Count
        If footnotesCount = 0 Then
            If Marker.ShowNoFootnotes Then
                Me.NoFootnotesExist.Text = GetLocalizedString(LOC_FOOTNOTE_NO_FOOTNOTES_LABEL)
                Me.NoFootnotesExist.Visible = True
            Else
                Me.NoFootnotesExist.Visible = False
                Me.MandatoryFootnoteRepeater.Visible = False
                Me.NonMandatoryFootnoteRepeater.Visible = False
            End If
        End If

        If Not mandatoryFootnotes.Count = 0 Then
            MandatoryFootnoteRepeater.DataSource = mandatoryFootnotes
            MandatoryFootnoteRepeater.DataBind()
        Else
            Me.MandatoryFootnoteRepeater.Visible = False

        End If
        If Not nonMandatoryFootnotes.Count = 0 Then
            NonMandatoryFootnoteRepeater.DataSource = nonMandatoryFootnotes
            NonMandatoryFootnoteRepeater.DataBind()
        Else
            Me.NonMandatoryFootnoteRepeater.Visible = False
        End If
    End Sub


    Private Sub MandatoryFootnoteRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles MandatoryFootnoteRepeater.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Header
                Dim ObsHeading As String = GetLocalizedString("CtrlFootnoteObsnoteLabel")
                Dim sbHeader As New StringBuilder()
                sbHeader.Append("<div class='obs-notes flex-row flex-wrap' role='region' aria-label='Must read' >")
                sbHeader.Append("<h3 class='obs-notes-h3'>" + "Obs:" + "</h3>")
                sbHeader.Append("<div class='footnote_definitionlist only_mandatory'>")

                SetLiteralText(e, "HeaderMandatory", sbHeader.ToString())
            Case ListItemType.Item, ListItemType.AlternatingItem
                SetLiteralText(e, "MandatoryFootnoteItem", GetFootnoteText(e))
            Case ListItemType.Footer
                SetLiteralText(e, "FooterMandatory", "</div></div>")
        End Select
    End Sub

    Private Sub NonMandatoryFootnoteRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles NonMandatoryFootnoteRepeater.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Header
                Dim headerText As String = GetLocalizedString(LOC_FOOTNOTE_LABEL)
                'Dim HeadingForOnTableNotes As String = GetLocalizedString("CtrlFootnoteFootnoteOnTableHeading")
                Dim sbAccordion As New StringBuilder()
                sbAccordion.Append("<div role='region' aria-label=" + headerText + " class='pxweb-accordion accordion-notes' id='FootnoteAccordion'>")
                sbAccordion.Append("  <button type='button' class='accordion-header closed' onclick='accordionToggle(FootnoteAccordion, this)'>")
                sbAccordion.Append("      <span role='heading' aria-level='2' class='header-text'>")
                sbAccordion.Append(headerText)
                sbAccordion.Append(" </span>")
                sbAccordion.Append("  </button>")
                sbAccordion.Append("<div class='accordion-body closed'>")
                'If Me.hasNonMandatoryNoteOnTable Then
                'sbAccordion.Append(" <h3>" + HeadingForOnTableNotes + "</h3>")
                'End If

                SetLiteralText(e, "FootnoteAccordionStart", sbAccordion.ToString())
            Case ListItemType.Item, ListItemType.AlternatingItem
                SetLiteralText(e, "NonMandatoryFootnoteItem", GetFootnoteText(e))
            Case ListItemType.Footer
                SetLiteralText(e, "FootnoteAccordionEnd", "</div></div>")
        End Select
    End Sub


    'For wcag
    Private Function GetClasses(ByVal li As FootnoteListItem, ByVal isKey As Boolean) As String
        If li.Header Then
            Return "notetype_" & li.FootnoteType.ToString().ToLower() & CSS_SUFFIX_HEADER.ToString().ToLower()
        Else
            Return "notetype_" & li.FootnoteType.ToString().ToLower() & IIf(isKey, CSS_SUFFIX_KEY, CSS_SUFFIX_VALUE).ToString()
        End If
    End Function


    Private Function GetFootnoteText(ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) As String
        Dim li As FootnoteListItem = DirectCast(e.Item.DataItem, FootnoteListItem)
        Dim sbText As New StringBuilder()
        If li.Header Then
            sbText.Append(Me.Heading("h3", li))
            'This occurs only and always for ValueNote, I hope. ? 
        Else
            If Not String.IsNullOrEmpty(li.Term) Then
                If li.FootnoteType = Enums.FootnoteType.ValueNote Then
                    sbText.Append(Me.Heading("h4", li))
                Else
                    sbText.Append(Me.Heading("h3", li))
                End If
            End If
            sbText.Append("<span class='the_long_text " + GetClasses(li, False) + "'>")
            sbText.Append(li.Definition + "</span>")
        End If
        Return sbText.ToString()
    End Function

    Private Function Heading(ByVal hLevel As String, ByVal inLI As FootnoteListItem) As String
        Return "<" + hLevel + " class='" + GetClasses(inLI, True) + "'>" + inLI.Term + "</" + hLevel + ">"
    End Function

End Class
