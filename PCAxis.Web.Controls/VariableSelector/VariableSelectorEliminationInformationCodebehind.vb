Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core

''' <summary>
''' Control that presents information on elimination during variable select.
''' </summary>
''' <remarks></remarks>
Public Class VariableSelectorEliminationInformationCodebehind
    Inherits PaxiomControlBase(Of VariableSelectorEliminationInformationCodebehind, VariableSelectorEliminationInformation)

#Region "Local variables"


#End Region

#Region "Localized strings"

    Private Const LOC_ELIMINATION_LABEL As String = "CtrlVariableSelectorEliminationLabel"
    Private Const LOC_ELIMINATION_TOOLTIP As String = "CtrlVariableSelectorEliminationTooltip"

#End Region

#Region "Controls"
    Protected EliminationTextPlaceholder As PlaceHolder
    Protected EliminationInformationText As Label
    Protected EliminationInformationImage As Image
    Protected EliminationInformationTextEnd As Label
#End Region


    Private Sub VariableSelectorEliminationInformation(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CreateEliminationInformationText()
    End Sub

    Private Sub CreateEliminationInformationText()
        Exit Sub 'Drop the Sub?
        Dim divider As String() = New String() {"{image}"}
        'Get string from language handling and try to split it up
        Dim texts() As String = Me.GetLocalizedString(LOC_ELIMINATION_LABEL).Split(divider, StringSplitOptions.None)

        If texts.Length = 1 Then
            'No {image} divider in string. Don't show the image, only text
            EliminationInformationText.Text = texts(0)
            EliminationInformationTextEnd.Visible = False
            EliminationInformationImage.Visible = False
        Else
            'Show first substring, then the image, and then the second substring.
            EliminationInformationText.Text = texts(0)
            EliminationInformationTextEnd.Text = texts(1)

            EliminationInformationImage.AlternateText = Me.GetLocalizedString(LOC_ELIMINATION_TOOLTIP)
            'EliminationInformationImage.ImageUrl = Marker.EliminationImagePath
            If Not Marker.EliminationImagePath Is Nothing Then
                EliminationInformationImage.ImageUrl = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, Marker.EliminationImagePath)
            End If

        End If

    End Sub

    Private Sub VariableSelectorEliminationInformation_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.LanguageChanged
        CreateEliminationInformationText()
    End Sub

    Private Sub VariableSelectorEliminationInformation_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.PaxiomModelChanged
        CreateEliminationInformationText()
    End Sub


End Class
