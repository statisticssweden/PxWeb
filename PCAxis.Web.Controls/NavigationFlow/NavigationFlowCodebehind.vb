﻿

Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Menu
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management

<ToolboxData("<{0}:NavigationFlow runat=""server""></{0}:NavigationFlow>")> _
Public Class NavigationFlowCodebehind
    Inherits PaxiomControlBase(Of NavigationFlowCodebehind, NavigationFlow)

#Region "fields"
    'For language
    Private Const LABEL_FIRSTSTEP As String = "CtrlNavigationFlowStep1"
    Private Const SCREENREADERTEXT_FIRSTSTEP As String = "CtrlNavigationFlowStep1ScreenReader"
    Private Const LABEL_SECONDSTEP As String = "CtrlNavigationFlowStep2"
    Private Const SCREENREADERTEXT_SECONDSTEP As String = "CtrlNavigationFlowStep2ScreenReader"
    Private Const LABEL_THIRDSTEP As String = "CtrlNavigationFlowStep3"
    Private Const SCREENREADERTEXT_THIRDSTEP As String = "CtrlNavigationFlowStep3ScreenReader"
    'constants for style
    Private Const ACTIVE As String = "active"
    Private Const FUTURE As String = "future"
    Private Const PASSIVE As String = "passive"

    Private Const SVG_FOLDER As String = "~/Resources/Images/svg/NavigationFlow/"

    Protected firstStepLink As HyperLink
    Protected firstStepImage As Image
    Protected firstStepLabel As Label

    Protected secondStepLink As HyperLink
    Protected secondStepImage As Image
    Protected secondStepLabel As Label

    Protected thirdStepLink As HyperLink
    Protected thirdStepImage As Image
    Protected thirdStepLabel As Label

    Protected navHrLeft As Literal
    Protected navHrRight As Literal

    Public SectionAriaLabel As String = "NavigationFlow"
    Protected ExplainAriaLabel As Label



#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SectionAriaLabel = GetLocalizedString("CtrlNavigationFlowSectionScreenReader")

            firstStepLabel.Text = GetLocalizedString(LABEL_FIRSTSTEP)
            firstStepLink.Attributes.Add("aria-label", GetLocalizedString(SCREENREADERTEXT_FIRSTSTEP))

            'firstStepImage.AlternateText = "" ' GetLocalizedString(SCREENREADERTEXT_FIRSTSTEP)

            secondStepLabel.Text = GetLocalizedString(LABEL_SECONDSTEP)
            secondStepLink.Attributes.Add("aria-label", GetLocalizedString(SCREENREADERTEXT_SECONDSTEP))
            'secondStepImage.AlternateText = GetLocalizedString(SCREENREADERTEXT_SECONDSTEP)

            thirdStepLabel.Text = GetLocalizedString(LABEL_THIRDSTEP)
            'thirdStepImage.AlternateText = GetLocalizedString(SCREENREADERTEXT_THIRDSTEP)

            'In case somebody needs to set very spesific margins
            Dim lang As String = LocalizationManager.GetTwoLetterLanguageCode()
            navHrLeft.Text = "<hr aria-hidden=""true"" class=""nav-hr-left " + lang + """/>"
            navHrRight.Text = "<hr aria-hidden=""true"" class=""nav-hr-right " + lang + """/>"

        End If
    End Sub


#End Region
#Region "Private methods"

    Private Sub SetNavigationLink(ByVal mode As NavigationFlow.NavigationFlowMode)
        Dim linkItems As List(Of LinkManager.LinkItem)
        Dim menu As PxMenuBase = Nothing
        Dim pHandler As PathHandler = PathHandlerFactory.Create(Marker.DatabaseType)

        If mode >= NavigationFlow.NavigationFlowMode.Second Then

            If Marker.TablePath.Equals("-") Then
                Marker.TablePath = Marker.DatabaseId
            End If

            linkItems = New List(Of LinkManager.LinkItem)
            linkItems.Add(New LinkManager.LinkItem(Marker.TablePathParam, Marker.TablePath))

            firstStepLink.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())


        End If
        'Navigat url to the third item in navigation flow
        If mode >= NavigationFlow.NavigationFlowMode.Third Then
            linkItems = New List(Of LinkManager.LinkItem)
            linkItems.Add(New LinkManager.LinkItem(Marker.LayoutParam, ""))
            secondStepLink.NavigateUrl = LinkManager.CreateLink(Marker.SelectionPage, linkItems.ToArray())
        Else
            secondStepLink.NavigateUrl = ""

        End If

    End Sub


    Private Sub SetStateOnStep(ByVal stateFirstStep As String, ByVal stateSecondStep As String, ByVal stateThirdStep As String)

        firstStepLabel.CssClass &= stateFirstStep
        firstStepLink.CssClass &= stateFirstStep

        secondStepLabel.CssClass &= stateSecondStep
        secondStepLink.CssClass &= stateSecondStep

        thirdStepLabel.CssClass &= stateThirdStep
        thirdStepLink.CssClass &= stateThirdStep

        firstStepImage.ImageUrl = SVG_FOLDER + "Step1_" + stateFirstStep + ".svg"
        secondStepImage.ImageUrl = SVG_FOLDER + "Step2_" + stateSecondStep + ".svg"
        thirdStepImage.ImageUrl = SVG_FOLDER + "Step3_" + stateThirdStep + ".svg"
    End Sub
#End Region

#Region "Public metchods"

    Public Sub UpdateNavigationFlowMode(ByVal mode As NavigationFlow.NavigationFlowMode)
        'Set right css class on the navigation flow items
        Dim outExplain = GetLocalizedString("CtrlNavigationFlowExplainScreenReader")
        Select Case mode
            Case NavigationFlow.NavigationFlowMode.First
                SetStateOnStep(ACTIVE, FUTURE, FUTURE)
                firstStepLink.Attributes.Add("aria-hidden", "true")
                secondStepLink.Attributes.Add("aria-hidden", "true")
                outExplain += GetLocalizedString(LABEL_FIRSTSTEP)
            Case NavigationFlow.NavigationFlowMode.Second
                SetStateOnStep(PASSIVE, ACTIVE, FUTURE)
                SetNavigationLink(mode)
                secondStepLink.Attributes.Add("aria-hidden", "true")
                outExplain += GetLocalizedString(LABEL_SECONDSTEP)
            Case NavigationFlow.NavigationFlowMode.Third
                SetStateOnStep(PASSIVE, PASSIVE, ACTIVE)
                SetNavigationLink(mode)
                outExplain += GetLocalizedString(LABEL_THIRDSTEP)
            Case Else
        End Select

        ExplainAriaLabel.Attributes.Add("class", "screenreader-only")
        ExplainAriaLabel.Text = outExplain



        thirdStepLink.Attributes.Add("aria-hidden", "true")

    End Sub
#End Region
End Class
