
Imports System.Web.UI.WebControls
Imports PCAxis.Paxiom
Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

''' <summary>
''' Change presentation dialog control.
''' </summary>
''' <remarks>
''' 
''' </remarks>
Public Class ChangeTextsCodebehind
    Inherits CommandBarPluginBase(Of ChangeTextsCodebehind, ChangeTexts)




#Region " Localized strings "

    'Change textpresentation texts
    Private Const CHANGETEXTS_TITLE As String = "CtrlChangeTextsHeading"
    Private Const CHANGETEXTS_TEXT As String = "CtrlChangeTextsText"
    Private Const CHANGETEXTS_UNITS As String = "CtrlChangeTextsUnitsHeading"
    Private Const CHANGETEXTS_VARIABLES As String = "CtrlChangeTextsVariablesHeading"
    Private Const CHANGETEXTS_CONTINUE_BUTTON As String = "CtrlChangeTextsContinueButton"
    Private Const CHANGETEXTS_CONTENTS_TEXT As String = "CtrlChangeTextsContentsHeading"
    Private Const CANCEL_BUTTON As String = "CancelButton"
    Private Const ILLEGAL_CHARACTERS_ERROR As String = "PxWebIllegalCharactersErrorMessage"

#End Region

#Region " Controls "
    Protected WithEvents ChangeTextPanel As Panel
    Protected TitleLabel As Label

    'ChangeTexts
    Protected ChangeTextTextLabel As Label
    Protected ContentsTextLabel As Label
    Protected ContentsTextTextBox As TextBox
    Protected lblContentsError As Label
    Protected UnitsTextLabel As Label
    Protected UnitsTextTextBox As TextBox
    Protected lblUnitsError As Label
    Protected VariablesTextLabel As Label
    Protected WithEvents ChangeTextRepeater As Repeater
    Protected WithEvents ChangeText_ContinueButton As Button
    Protected WithEvents CancelButton As Button

    'Common
    Protected ErrorMessageLabel As Label
    Protected ErrorMessagePanel As Panel

#End Region

#Region " Properties "

    Private _settingsDictionary As Dictionary(Of String, HeaderPresentationType)
    Private _codeText As String = "Code"
    Private _textText As String = "Text"
    Private _codeAndTextText As String = "Code and Text"
    Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(ChangeTexts))

#End Region

    ''' <summary>
    ''' Control_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangeTexts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadTextsForLanguage()
        ChangeTextPanel.Visible = True

        ContentsTextTextBox.Text = Me.PaxiomModel.Meta.Contents
        UnitsTextTextBox.Text = Me.PaxiomModel.Meta.ContentInfo.Units

        ChangeTextRepeater.DataSource = Me.PaxiomModel.Meta.Variables
        ChangeTextRepeater.DataBind()
    End Sub


    ' ''' <summary>
    ' ''' Finds the Table control on the current page recursively 
    ' ''' </summary>
    ' ''' <param name="controls">The controlcollection to search through</param>
    ' ''' <returns>The Table control if found, otherwise Nothing(Null)</returns>
    ' ''' <remarks></remarks>
    'Private Function FindTableControl(ByVal controls As System.Web.UI.ControlCollection) As Table
    '    Dim table As Table = Nothing
    '    For Each control As System.Web.UI.Control In controls
    '        If TypeOf (control) Is PCAxis.Web.Controls.Table Then
    '            table = CType(control, Table)
    '            Exit For
    '        End If
    '        table = FindTableControl(control.Controls)
    '        If table IsNot Nothing Then
    '            Exit For
    '        End If
    '    Next
    '    Return table
    'End Function

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangeTexts_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()

        Dim buttonText As String = GetLocalizedString(CHANGETEXTS_CONTINUE_BUTTON)

        'Change Text texts
        ChangeTextTextLabel.Text = GetLocalizedString(CHANGETEXTS_TEXT)
        ContentsTextLabel.Text = GetLocalizedString(CHANGETEXTS_CONTENTS_TEXT)
        UnitsTextLabel.Text = GetLocalizedString(CHANGETEXTS_UNITS)
        VariablesTextLabel.Text = GetLocalizedString(CHANGETEXTS_VARIABLES)
        ChangeText_ContinueButton.Text = buttonText
        TitleLabel.Text = GetLocalizedString(CHANGETEXTS_TITLE)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
    End Sub



#Region "ChangeText"

    ''' <summary>
    ''' 
    ''' </summary>   
    Protected Sub ChangeTextRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles ChangeTextRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim v As Variable = DirectCast(e.Item.DataItem, Variable)
            Dim ValueNameTextbox As TextBox = DirectCast(item.FindControl("ValueNameTextbox"), TextBox)
            ValueNameTextbox.Text = v.Name
            ValueNameTextbox.Attributes.Add("VariableCode", v.Code)
        End If
    End Sub



    ''' <summary>
    ''' Handles event continuebutton clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChangeText_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChangeText_ContinueButton.Click
        Dim newModel As PXModel
        Dim variables As List(Of KeyValuePair(Of String, String))
        Dim contents As String
        Dim units As String
        Dim bError As Boolean = False

        'Variables
        variables = New List(Of KeyValuePair(Of String, String))
        For Each itm As RepeaterItem In ChangeTextRepeater.Items
            Dim tbx As TextBox = DirectCast(itm.FindControl("ValueNameTextbox"), TextBox)
            Dim variableCode As String = tbx.Attributes("VariableCode")
            variables.Add(New KeyValuePair(Of String, String)(variableCode, tbx.Text))

            If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(tbx.Text) Then
                Dim lbl As Label = DirectCast(itm.FindControl("lblError"), Label)
                lbl.Text = GetLocalizedString(ILLEGAL_CHARACTERS_ERROR)
                bError = True
            End If

        Next

        'Contents and Units texts
        contents = ContentsTextTextBox.Text
        units = UnitsTextTextBox.Text

        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(ContentsTextTextBox.Text) Then
            lblContentsError.Text = GetLocalizedString(ILLEGAL_CHARACTERS_ERROR)
            bError = True
        Else
            lblContentsError.Text = ""
        End If

        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(UnitsTextTextBox.Text) Then
            lblUnitsError.Text = GetLocalizedString(ILLEGAL_CHARACTERS_ERROR)
            bError = True
        Else
            lblUnitsError.Text = ""
        End If


        If bError Then
            Return
        End If

        'Finish
        newModel = ChangeTexts(contents, units, variables)
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(newModel))
    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub


    Private Function ChangeTexts(ByVal contents As String, ByVal units As String, ByVal variables As List(Of KeyValuePair(Of String, String))) As Paxiom.PXModel

        Dim changeText As New PCAxis.Paxiom.Operations.ChangeText()
        Dim changeTextDescription As New PCAxis.Paxiom.Operations.ChangeTextDescription(contents, units, variables)

        PaxiomManager.PaxiomModel = changeText.Execute(PaxiomModel, changeTextDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.CHANGE_TEXT, changeTextDescription)

        Return PaxiomManager.PaxiomModel

    End Function


#End Region




End Class

