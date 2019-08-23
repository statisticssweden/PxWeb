Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Management
Imports System.Drawing.Imaging
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Controls.CommandBar.Plugin

Public Class SaveAsChartCodebehind : Inherits FileTypeControlBase(Of SaveAsChartCodebehind, SaveAsChart)

#Region "Controls"
    Protected WithEvents ContinueButton As Button
    Protected WithEvents CancelButton As Button
    Protected ChartFileFormatContainer As PlaceHolder
    Protected ChartFileFormats As ListBox
#End Region


    Private Sub SaveAsChart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Marker.ShowUI Then

            ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton")
            CancelButton.Text = GetLocalizedString("CtrlSaveAsCancelButton")

            If CommandBarPluginManager.FileTypes("chart") IsNot Nothing Then

                Dim fileType As FileType = CommandBarPluginManager.FileTypes("chart")
                ChartFileFormats.Items.Clear()
                For Each kvp As KeyValuePair(Of String, String) In fileType.FileFormats
                    Dim li As New ListItem()
                    li.Text = Me.GetLocalizedString(kvp.Value)
                    li.Value = kvp.Key
                    Me.ChartFileFormats.Items.Add(li)
                Next
                If ChartFileFormats.Items.Count > 0 Then

                    ChartFileFormats.SelectedIndex = 0
                End If
            End If

        Else

            SetSelectedFormat()
            OnFinished()
            Marker.SerializeAndStream()
        End If
    End Sub

    Private Sub SetSelectedFormat()

        If Marker.ShowUI Then
            If (ChartFileFormats.SelectedIndex > -1) Then
                Me.Marker.SelectedFormat = ChartFileFormats.SelectedValue
            Else
                'Set default
                Me.Marker.SelectedFormat = "FileTypeChartPng"
            End If
        End If

    End Sub

    Private Sub Continue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        SetSelectedFormat()
        OnFinished()
        Marker.SerializeAndStream()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        OnFinished()
    End Sub
End Class
