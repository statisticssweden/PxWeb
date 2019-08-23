

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Core.Management.LinkManager
Imports PCAxis.Query
Imports PCAxis.Menu

<ToolboxData("<{0}:NavigationFlow runat=""server""></{0}:NavigationFlow>")> _
Public Class NavigationFlowCodebehind
    Inherits PaxiomControlBase(Of NavigationFlowCodebehind, NavigationFlow)

#Region "fields"
    'For language
    Private Const LABEL_FIRSTSTEP As String = "CtrlNavigationFlowStep1"
    Private Const LABEL_SECONDSTEP As String = "CtrlNavigationFlowStep2"
    Private Const LABEL_THIRDSTEP As String = "CtrlNavigationFlowStep3"
    'constants for style
    Private Const ACTIVE_CSSCLAS As String = "active "
    Private Const FUTURE_CSSCLAS As String = "future"
    Private Const PASSIVE_CSSCLAS As String = "passive"
    Private Const COL_CSSCLAS As String = "col"

    Protected panelShowNavigationFlow As Panel
    Protected firstItem, secondItem, thirdItem As Panel
    Protected lblFirstStep As Label
    Protected lblSecondStep As Label
    Protected lblThirdStep As Label

    Protected firstLink As HyperLink
    Protected secondLink As HyperLink

#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblFirstStep.Text = GetLocalizedString(LABEL_FIRSTSTEP)
            lblSecondStep.Text = GetLocalizedString(LABEL_SECONDSTEP)
            lblThirdStep.Text = GetLocalizedString(LABEL_THIRDSTEP)
        End If
    End Sub


#End Region
#Region "Private methods"
    ''' <summary>
    ''' Return path to table in the base format
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Private Function UrlPath(ByVal path As String) As String
    '    If Marker.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX Then
    '        path = path.Replace(PxPathHandler.DIVIDER_STRING, PathHandler.NODE_DIVIDER)
    '    End If
    '    Return path
    'End Function

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

            firstLink.NavigateUrl = LinkManager.CreateLink(Marker.MenuPage, linkItems.ToArray())

        End If
        'Navigat url to the third item in navigation flow
        If mode >= NavigationFlow.NavigationFlowMode.Third Then
            linkItems = New List(Of LinkManager.LinkItem)
            linkItems.Add(New LinkManager.LinkItem(Marker.LayoutParam, ""))
            secondLink.NavigateUrl = LinkManager.CreateLink(Marker.SelectionPage, linkItems.ToArray())
        End If

    End Sub
#End Region

#Region "Public metchods"

    Public Sub UpdateNavigationFlowMode(ByVal mode As NavigationFlow.NavigationFlowMode)
        'Set right css class on the navigation flow items
        Select Case mode
            Case NavigationFlow.NavigationFlowMode.First
                firstItem.CssClass &= ACTIVE_CSSCLAS
                secondItem.CssClass &= FUTURE_CSSCLAS
                thirdItem.CssClass &= FUTURE_CSSCLAS
            Case NavigationFlow.NavigationFlowMode.Second
                firstItem.CssClass &= PASSIVE_CSSCLAS
                secondItem.CssClass &= ACTIVE_CSSCLAS
                thirdItem.CssClass &= FUTURE_CSSCLAS
                'Navigation to menu page
                SetNavigationLink(mode)
            Case NavigationFlow.NavigationFlowMode.Third
                firstItem.CssClass &= PASSIVE_CSSCLAS
                secondItem.CssClass &= PASSIVE_CSSCLAS
                thirdItem.CssClass &= ACTIVE_CSSCLAS
                'Navigation to menu page and selection page
                SetNavigationLink(mode)
            Case Else

        End Select



    End Sub
#End Region
End Class