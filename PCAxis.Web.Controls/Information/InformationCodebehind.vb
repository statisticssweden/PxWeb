
Imports System.Web.UI
Imports PCAxis.Enums
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports System.Web.UI.WebControls

''' <summary>
''' Showing information
''' </summary>
<ToolboxData("<{0}:information runat=""server""></{0}:information>")> _
Public Class InformationCodebehind
    Inherits PaxiomControlBase(Of InformationCodebehind, Information)

    Private Const INFORMATIONLABEL As String = "CtrlInformationInformationLabel"
    Private Const YESVALUE As String = "CtrlInformationYesValue"
    Private Const NOVALUE As String = "CtrlInformationNoValue"
    Private Const FIXEDPRICESVALUE As String = "CtrlInformationFixedPricesValue"
    Private Const CURRENTPRICESVALUE As String = "CtrlInformationCurrentPricesValue"
    Private Const FLOWVALUE As String = "CtrlInformationFlowValue"
    Private Const AVERAGEVALUE As String = "CtrlInformationAverageValue"
    Private Const STOCKVALUE As String = "CtrlInformationStockValue"



    Protected WithEvents InformationRepeater As Repeater

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If InformationRepeater.DataSource Is Nothing Then
            GetInformation()
        End If
    End Sub

    Private Sub Information_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        GetInformation()

    End Sub

    Private Sub Information_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PaxiomModelChanged
        GetInformation()
    End Sub

    ''' <summary>
    ''' Get information for a table.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub GetInformation()
        If Me.PaxiomModel IsNot Nothing Then            
            If Marker.ShowInformationTypes Is Nothing Then 'Create default which is all information types.
                Marker.ShowInformationTypes = New List(Of InformationType)
                Dim enumValues As Array = System.[Enum].GetValues(GetType(InformationType))
                For Each enumValue As InformationType In enumValues
                    Marker.ShowInformationTypes.Add(enumValue)
                Next
            End If
            If Marker.ShowInformationTypes.Count > 0 Then
                InformationRepeater.DataSource = InformationList.GetInformationList(Me.PaxiomModel.Meta, Marker.ShowInformationTypes, Marker.ContactForEveryContent, Marker.LastUpdatedForEveryContent)
                InformationRepeater.DataBind()
            End If

        End If
    End Sub


    ''' <summary>
    ''' Event used to update values in repeater templates. This is the inner repeater rendering definition list with variables.
    ''' </summary>
    Private Sub VariableRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim ivar As InformationVariable = DirectCast(e.Item.DataItem, InformationVariable)
            SetLiteralText(e, "VariableTerm", ivar.Term & ":")
            SetLiteralText(e, "VariableDefinition", GetDefinition(ivar.InformationType, ivar.Definition))
        End If
    End Sub

    ''' <summary>
    ''' Helper used to update literal in repeater
    ''' </summary>
    Private Sub SetLiteralText(ByVal e As RepeaterItemEventArgs, ByVal literalId As String, ByVal text As String)
        Dim literal As Literal = DirectCast(e.Item.FindControl(literalId), Literal)
        literal.Text = FormatString(text)
    End Sub

    ''' <summary>
    ''' Modifies string and makes a translation an abbreviation is passed.
    ''' </summary>
    ''' <param name="informationType">Information type</param>
    ''' <param name="definition">String containing either a translated text or an abbreviation.</param>
    ''' <returns>String with translated abbreviations if any</returns>
    Private Function GetDefinition(ByVal informationType As InformationType, ByVal definition As String) As String
        Select Case informationType
            Case informationType.DayAdj, informationType.SeasAdj, informationType.Copyright, Enums.InformationType.OfficialStatistics
                definition = CStr(Microsoft.VisualBasic.Switch(definition = "YES", GetLocalizedString(YESVALUE), definition = "NO", GetLocalizedString(NOVALUE), True, String.Empty))
            Case informationType.CFPrices
                definition = CStr(Microsoft.VisualBasic.Switch(definition = "F", GetLocalizedString(FIXEDPRICESVALUE), definition = "C", GetLocalizedString(CURRENTPRICESVALUE), True, String.Empty))
            Case informationType.StockFa
                definition = CStr(Microsoft.VisualBasic.Switch(definition = "F", GetLocalizedString(FLOWVALUE), definition = "A", GetLocalizedString(AVERAGEVALUE), definition = "S", GetLocalizedString(STOCKVALUE), True, String.Empty))
        End Select
        Return definition
    End Function

    ''' <summary>
    ''' Gets a translated text term for a specific Information type. 
    ''' </summary>
    ''' <param name="informationType">Information type</param>
    ''' <returns>Translated text</returns>
    Private Function GetMainTermText(ByVal informationType As InformationType) As String
        Dim key As String = "CtrlInformation" + informationType.ToString & "Label"
        Dim localizedString As String = GetLocalizedString(key)
        Return localizedString
    End Function

    ''' <summary>
    ''' Event used to update values in repeater templates. This is the inner repeater rendering definition list with variables.
    ''' </summary>
    Private Sub InformationRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles InformationRepeater.ItemDataBound
        Select Case e.Item.ItemType
            'Case ListItemType.Header
            '    SetLiteralText(e, "Header", GetLocalizedString(INFORMATIONLABEL))
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim li As InformationListItem = DirectCast(e.Item.DataItem, InformationListItem)
                SetLiteralText(e, "MainTerm", GetMainTermText(li.InformationType))
                If li.InformationVariables.Count > 0 Then
                    Dim variableRepeater As Repeater = DirectCast(e.Item.FindControl("VariableRepeater"), Repeater)
                    AddHandler variableRepeater.ItemDataBound, AddressOf VariableRepeater_ItemDataBound
                    variableRepeater.DataSource = li.InformationVariables
                    variableRepeater.DataBind()
                Else
                    SetLiteralText(e, "MainDefinition", GetDefinition(li.InformationType, li.Definition))
                End If
        End Select
    End Sub

End Class
