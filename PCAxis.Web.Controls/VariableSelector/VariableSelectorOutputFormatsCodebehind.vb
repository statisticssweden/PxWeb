Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core

''' <summary>
''' Control that presents information on limits for selecttion and selections done.
''' </summary>
''' <remarks></remarks>
Public Class VariableSelectorOutputFormatsCodebehind

    Inherits PaxiomControlBase(Of VariableSelectorOutputFormatsCodebehind, VariableSelectorOutputFormats)

#Region "Local variables"


#End Region

#Region "Localized strings"

    Private Const CSS_VARIABLESELECTOR_CONTINUE_FORMATS_DROPDOWN As String = "variableselector_continue_formats_dropdown"

#End Region

#Region "Controls"
    Protected OutputFormatPlaceHolder As PlaceHolder
    Protected OutputFormatDropDownList As DropDownList
    Protected FileTypeControlHolder As Panel
#End Region

#Region "Properties"



#End Region

    Private Sub VariableSlectorMarketingTips(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FillOutputFormatsControls()
        'TODO: Ska synligheten på kontrollen styras av någon parameter? finns ingen spec på denna
    End Sub

    ''' <summary>
    ''' Fills the dropdown list with presentation views and file formats
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillOutputFormatsControls()
        Dim li As ListItem
        Dim plugin As CommandBarPluginInfo

        For Each presView As String In Marker.PresentationViews
            plugin = CommandBarPluginManager.Views(presView)
            If Not plugin Is Nothing Then
                li = New ListItem(GetLocalizedString(plugin.NameCode), plugin.Name)
                If Not OutputFormatDropDownList.Items.Contains(li) Then
                    OutputFormatDropDownList.Items.Add(li)
                End If
            End If
        Next
        For Each outputFormat As String In Marker.OutputFormats
            li = New ListItem(GetLocalizedString(outputFormat), outputFormat)
            If Not OutputFormatDropDownList.Items.Contains(li) Then
                OutputFormatDropDownList.Items.Add(li)
            End If
        Next
    End Sub

    Private Sub VariableSelectorEliminationInformation_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.LanguageChanged
        FillOutputFormatsControls()
    End Sub

    Private Sub VariableSelectorEliminationInformation_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.PaxiomModelChanged
        FillOutputFormatsControls()
    End Sub

    ''' <summary>
    ''' Is screen output selected in the dropdown or not?
    ''' </summary>
    ''' <returns>True if screen output is selected, else false</returns>
    ''' <remarks></remarks>
    Public Function ScreenOutput() As Boolean
        If Marker.PresentationViews.Contains(OutputFormatDropDownList.SelectedValue) Then
            Return True
        Else
            Return False
        End If

        'If OutputFormatDropDownList.SelectedValue.Equals("CtrlVariableSelectorOutputFormatsOutputScreen") Then
        '    Return True
        'Else
        '    Return False
        'End If

    End Function

    ''' <summary>
    ''' Returns the selected output format
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectedOutput() As String
        If Me.OutputFormatDropDownList.SelectedIndex <> -1 Then
            Return OutputFormatDropDownList.SelectedValue
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Creates a file of selected type (in the dropdown list) containing the model and opens it
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowInSelectedOutputFormat()
        Dim outputFormat As String = OutputFormatDropDownList.SelectedValue
        'Dim FileGenerator As New FileGenerator(CurrentCulture)
        Dim ft As FileType = CommandBarPluginManager.GetFileType(outputFormat)

        If ft Is Nothing Then
            Exit Sub
        End If

        'Load the filetypes webcontrol
        Dim fileTypeControl As PCAxis.Web.Core.Interfaces.IFileTypeControl = CType(Activator.CreateInstance(Type.GetType(ft.WebControl)), PCAxis.Web.Core.Interfaces.IFileTypeControl)

        If fileTypeControl Is Nothing Then
            Exit Sub
        End If

        With fileTypeControl
            CType(fileTypeControl, Control).ID = outputFormat
            .SelectedFormat = If(outputFormat Is Nothing, String.Empty, outputFormat)
            .SelectedFileType = ft
        End With

        'With FileTypeControlHolder
        '    .Controls.Clear()
        '    .Controls.Add(CType(fileTypeControl, Control))
        '    .Visible = True
        'End With

        fileTypeControl.SerializeAndStream()

    End Sub

End Class