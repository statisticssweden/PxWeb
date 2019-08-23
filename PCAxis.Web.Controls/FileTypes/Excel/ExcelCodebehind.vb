
Imports PCAxis.Web.Core
Imports PCAxis.Paxiom
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Controls.CommandBar.Plugin

''' <summary>
''' With this component you can make specific settings 
''' for how an Excel file will be created.
''' 
''' For example decide format:
''' Excel file from screen (xlsx)   or
''' Excel file (xlsx) with code and text column
''' </summary>
''' <remarks></remarks>
Public Class ExcelCodebehind
    Inherits FileTypeControlBase(Of ExcelCodebehind, Excel)

#Region "Controls"
    Protected WithEvents ContinueButton As Button
    Protected WithEvents CancelButton As Button
    Protected ExcelFileFormatContainer As PlaceHolder
    Protected ExcelFileFormats As ListBox
#End Region

    ''' <summary>
    ''' Called when the user control is loaded
    ''' </summary>
    ''' <param name="sender">not used</param>
    ''' <param name="e">not used</param>
    ''' <remarks></remarks>
    Private Sub Excel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Marker.ShowUI Then
            ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton")
            CancelButton.Text = GetLocalizedString("CtrlSaveAsCancelButton")

            'Dim fileGenerator As FileGenerator = New FileGenerator(CurrentCulture)
            If CommandBarPluginManager.FileTypes(Plugins.FileFormats.XLS) IsNot Nothing Then
                Dim fileType As FileType = CommandBarPluginManager.FileTypes(Plugins.FileFormats.XLS)
                ExcelFileFormats.Items.Clear()
                For Each kvp As KeyValuePair(Of String, String) In fileType.FileFormats
                    Dim li As New ListItem
                    li.Text = Me.GetLocalizedString(kvp.Value)
                    li.Value = kvp.Key
                    Me.ExcelFileFormats.Items.Add(li)
                Next
                If ExcelFileFormats.Items.Count > -1 Then
                    ExcelFileFormats.SelectedIndex = 0
                End If
            End If
        Else
            SetSelectedFormat()
            OnFinished()
            Marker.SerializeAndStream()
        End If
    End Sub


    ''' <summary>
    ''' Handles Save button click. Creates the Excel file and streams it to the user.
    ''' </summary>
    ''' <param name="sender">not used</param>
    ''' <param name="e">not used</param>
    ''' <remarks></remarks>
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
        Me.Marker.SelectedFormat = Plugins.FileFormats.XLS_EXCEL

        If Marker.ShowUI Then
            If ExcelFileFormats.SelectedIndex > -1 Then
                Me.Marker.SelectedFormat = ExcelFileFormats.SelectedValue
            End If
        End If
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        OnFinished()
    End Sub
End Class
