

Imports PCAxis.Paxiom.Localization
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Enums
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.UI.HtmlControls

''' <summary>
''' Control that presents information on limits for selecttion and selections done.
''' </summary>
''' <remarks></remarks>
<ToolboxData("<{0}:VariableSelectorMarkingTips runat=""server""></{0}:VariableSelectorMarkingTips>")> _
Public Class VariableSelectorMarkingTipsCodebehind
    Inherits PaxiomControlBase(Of VariableSelectorMarkingTipsCodebehind, VariableSelectorMarkingTips)


#Region "Localized strings"

    Private Const LOC_INSTRUCTION_LABEL As String = "CtrlVariableSelectorInstructionLabel"
    Private Const LOC_MARKING_TIPS_LINK As String = "CtrlVariableSelectorMarkingTipsLink"

#End Region

#Region "Controls"
    Protected MarkingTipsPlaceHolder As PlaceHolder
    Protected MarkingTipsLabel As Label
    Protected MarkingTipsLink As HyperLink

#End Region


    Private Sub VariableSlectorMarketingTips(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CreateMarkingTips()
    End Sub

    Private Sub CreateMarkingTips()
        MarkingTipsLabel.Text = Me.GetLocalizedString(LOC_INSTRUCTION_LABEL) + ". "
        MarkingTipsLink.Text = Me.GetLocalizedString(LOC_MARKING_TIPS_LINK)

        If Marker.MarkingTipsLinkNavigateUrl IsNot Nothing Then
            If Marker.MarkingTipsLinkNavigateUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) Then
                MarkingTipsLink.NavigateUrl = Marker.MarkingTipsLinkNavigateUrl
            Else
                MarkingTipsLink.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Marker.MarkingTipsLinkNavigateUrl)
            End If
        End If

    End Sub

    Private Sub VariableSelectorEliminationInformation_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.LanguageChanged
        CreateMarkingTips()
    End Sub

    Private Sub VariableSelectorEliminationInformation_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.PaxiomModelChanged
        CreateMarkingTips()
    End Sub


End Class