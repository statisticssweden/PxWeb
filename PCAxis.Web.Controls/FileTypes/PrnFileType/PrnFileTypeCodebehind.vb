
Imports PCAxis.Web.Core
Imports PCAxis.Paxiom
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management

Public Class PrnFileTypeCodebehind
    Inherits FileTypeControlBase(Of PrnFileTypeCodebehind, PrnFileType)

#Region " Localized strings "

    Private Const FILE_WITH_HEADING As String = "CtrlCsvFileTypeFileWithHeading"
    Private Const FILE_WITHOUT_HEADING As String = "CtrlCsvFileTypeFileWithoutHeading"
    Private Const TABULATOR As String = "CtrlCsvFileTypeFileTabulator"
    Private Const COMMA As String = "CtrlCsvFileTypeFileComma"
    Private Const SPACE As String = "CtrlCsvFileTypeFileSpace"
    Private Const SEMICOLON As String = "CtrlCsvFileTypeFileSemicolon"

#End Region

#Region " Controls "

    Protected WithEvents ContinueButton As Button
    Protected WithEvents CancelButton As Button
    Protected FileFormatContainer As PlaceHolder
    Protected HeadingValues As ListBox
    Protected SeparatorValues As ListBox

#End Region

    Private Sub PrnFileType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Marker.ShowUI Then
            Me.HeadingValues.Items.Add(New ListItem(GetLocalizedString(FILE_WITH_HEADING), FILE_WITH_HEADING))
            Me.HeadingValues.Items.Add(New ListItem(GetLocalizedString(FILE_WITHOUT_HEADING), FILE_WITHOUT_HEADING))
            Me.HeadingValues.SelectedIndex = 0

            Me.SeparatorValues.Items.Add(New ListItem(GetLocalizedString(TABULATOR), TABULATOR))
            Me.SeparatorValues.Items.Add(New ListItem(GetLocalizedString(COMMA), COMMA))
            Me.SeparatorValues.Items.Add(New ListItem(GetLocalizedString(SPACE), SPACE))
            Me.SeparatorValues.Items.Add(New ListItem(GetLocalizedString(SEMICOLON), SEMICOLON))
            Me.SeparatorValues.SelectedIndex = 0
            ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton")
            CancelButton.Text = GetLocalizedString("CtrlSaveAsCancelButton")
        Else
            SetSelectedFormat()
            OnFinished()
            Marker.SerializeAndStream()
        End If
    End Sub

    Private Sub continue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        SetSelectedFormat()
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

    ''' <summary>
    ''' Set selected file format
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSelectedFormat()
        'Set default
        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_COMMA_WITH_HEADING

        Select Case HeadingValues.SelectedValue
            Case FILE_WITH_HEADING
                Select Case SeparatorValues.SelectedValue
                    Case TABULATOR
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_TABULATOR_WITH_HEADING
                    Case COMMA
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_COMMA_WITH_HEADING
                    Case SPACE
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_SPACE_WITH_HEADING
                    Case SEMICOLON
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_SEMICOLON_WITH_HEADING
                End Select
            Case FILE_WITHOUT_HEADING
                Select Case SeparatorValues.SelectedValue
                    Case TABULATOR
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_TABULATOR_WITHOUT_HEADING
                    Case COMMA
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_COMMA_WITHOUT_HEADING
                    Case SPACE
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_SPACE_WITHOUT_HEADING
                    Case SEMICOLON
                        Me.Marker.SelectedFormat = Plugins.FileFormats.CSV_SEMICOLON_WITHOUT_HEADING
                End Select
        End Select
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        OnFinished()
    End Sub
End Class
