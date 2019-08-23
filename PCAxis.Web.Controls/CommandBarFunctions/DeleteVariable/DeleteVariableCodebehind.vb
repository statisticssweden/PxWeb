
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel

Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Text
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management
Imports PCAxis.Paxiom.Operations

''' <summary>
''' Delete variable control.
''' </summary>
''' <remarks>
''' 
''' </remarks>
Public Class DeleteVariableCodebehind
    Inherits CommandBarPluginBase(Of DeleteVariableCodebehind, DeleteVariable)



#Region " Localized strings "

    Private Const DELETE_VARIABLE_TITLE As String = "CtrlCommandBarFunctionDeleteVariable"
    Private Const DELETE_VARIABLE_HEADER_TEXT As String = "CtrlDeleteVariableHeaderLabel"
    Private Const DELETE_VARIABLE_TEXT As String = "CtrlDeleteVariableTextLabel"
    Private Const DELETE_VARIABLE_ADDTOTITLE_TEXT As String = "CtrlDeleteVariableAddToTitleLabel"
    Private Const DELETE_VARIABLE_CONTINUE_BUTTON As String = "CtrlDeleteVariableContinueButton"
    Private Const CANCEL_BUTTON As String = "CancelButton"
    Private Const DELETE_VARIABLE_ERRORDESCRIPTION As String = "CtrlDeleteVariableErrorDescription"
    Private Const DELETE_VARIABLE_ONE_VARIABLE As String = "CtrlDeleteVariableOneVariable"

#End Region

#Region " Controls "
    Protected WithEvents DeleteValuePanel As Panel
    Protected WithEvents TitleLabel As Label
    Protected WithEvents VariableSelectorValueSelectRepeater As Repeater
    Protected WithEvents ContinueButton As Button
    Protected WithEvents DeleteVariableTextLabel As Label
    Protected WithEvents DeleteVariableAddToTitle As Label
    Protected AddToTitleCheckbox As CheckBox
    Protected ErrorMessagePanel As Panel
    Protected ErrorMessageLabel As Label
    Protected WithEvents CancelButton As Button


#End Region

#Region " Properties "

#End Region

    ''' <summary>
    ''' Control_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteVariable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VariableSelectorValueSelectRepeater.DataSource = Me.PaxiomModel.Meta.Variables
        VariableSelectorValueSelectRepeater.DataBind()
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteVariable_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()
        TitleLabel.Text = GetLocalizedString(DELETE_VARIABLE_TITLE)
        DeleteVariableTextLabel.Text = GetLocalizedString(DELETE_VARIABLE_HEADER_TEXT)
        DeleteVariableAddToTitle.Text = GetLocalizedString(DELETE_VARIABLE_ADDTOTITLE_TEXT)
        ContinueButton.Text = GetLocalizedString(DELETE_VARIABLE_CONTINUE_BUTTON)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
    End Sub




    ''' <summary>
    ''' Add ValuesListBox, VariableNameRadio and necessary events for each variable in RepeaterValueSelect
    ''' </summary>   
    Protected Sub RepeaterValueSelect_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles VariableSelectorValueSelectRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim var As Variable = DirectCast(e.Item.DataItem, Variable)
            Dim ValuesSelectPanel As Panel = DirectCast(item.FindControl("ValuesSelectPanel"), Panel)

            'Radiobutton with text and attributes
            Dim VariableNameRadio As RadioButton = DirectCast(item.FindControl("VariableNameRadio"), RadioButton)
            VariableNameRadio.Text = var.Name
            VariableNameRadio.Attributes.Add("VariableCode", var.Code)
            VariableNameRadio.Attributes.Add("onclick", "SetUniqueRadioButton('VariableSelectorValueSelectRepeater.*VariableSelectionGroup',this)")

            'Listbox with variablevalues
            Dim ValuesListBox As ListBox = DirectCast(item.FindControl("ValuesListBox"), ListBox)
            ValuesListBox.SelectionMode = ListSelectionMode.Single
            ValuesListBox.Rows = CInt(Me.Properties("ListSize"))
            ValuesListBox.DataTextField = "Value"
            ValuesListBox.DataValueField = "Code"

            'Databind
            Dim valuesToShow As Values = var.Values
            ValuesListBox.DataSource = valuesToShow
            ValuesListBox.DataBind()
            ValuesSelectPanel.Controls.Add(ValuesListBox)

        End If
    End Sub




    ''' <summary>
    ''' Handles event continuebutton clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click

        Dim VariableNameRadio As RadioButton
        Dim ValuesListBox As ListBox
        Dim paxiomOperation As New PCAxis.Paxiom.Operations.DeleteVariable
        Dim deleteDescription As PCAxis.Paxiom.Operations.DeleteVariableDescription = Nothing

        'Cannot delete all variables. At least 1 variable must exist
        If VariableSelectorValueSelectRepeater.Items.Count = 1 Then
            Me.ErrorMessagePanel.Visible = True
            Me.ErrorMessageLabel.Text = GetLocalizedString(DELETE_VARIABLE_ONE_VARIABLE)
            Exit Sub
        End If

        For Each itm As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            'Find the selected variable
            VariableNameRadio = DirectCast(itm.FindControl("VariableNameRadio"), RadioButton)
            If VariableNameRadio.Checked Then
                Dim variableCode As String = VariableNameRadio.Attributes.Item("VariableCode")
                If Not String.IsNullOrEmpty(variableCode) Then
                    ValuesListBox = DirectCast(itm.FindControl("ValuesListBox"), ListBox)
                    Dim variableValue As String = ValuesListBox.SelectedValue
                    If Not String.IsNullOrEmpty(variableValue) Then
                        'Create DeleteVariableDescription for selected variable
                        deleteDescription = New PCAxis.Paxiom.Operations.DeleteVariableDescription(variableCode, variableValue, AddToTitleCheckbox.Checked)
                    End If
                End If
                Exit For
            End If
        Next

        'Execute operation
        If Not IsNothing(deleteDescription) Then
            Try
                Dim model As PXModel = paxiomOperation.Execute(Me.PaxiomModel, deleteDescription)
                UpdateOperationsTracker(deleteDescription)
                Me.OnFinished(New CommandBarPluginFinishedEventArgs(model))
            Catch ex As PXOperationException
                Me.ErrorMessagePanel.Visible = True
                Me.ErrorMessageLabel.Text = ex.Message
            End Try
        Else
            Me.ErrorMessagePanel.Visible = True
            Me.ErrorMessageLabel.Text = GetLocalizedString(DELETE_VARIABLE_ERRORDESCRIPTION)
        End If
    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub

    Private Sub UpdateOperationsTracker(deleteDescription As DeleteVariableDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.DELETE_VARIABLE, deleteDescription)
        Dim va As Variable = Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(Function(v As Variable) v.Code = deleteDescription.variableCode)
        If va IsNot Nothing AndAlso va.IsTime Then
            PaxiomManager.OperationsTracker.IsTimeDependent = True
        End If
    End Sub

End Class

