
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
''' Delete value control.
''' </summary>
''' <remarks>
''' Deletes selected values. At least one value per variable must remain
''' </remarks>
Public Class DeleteValueCodebehind
    Inherits CommandBarPluginBase(Of DeleteValueCodebehind, DeleteValue)


#Region " Localized strings "

    Private Const DELETE_VALUE_TITLE As String = "CtrlCommandBarFunctionDeleteValue"
    Private Const DELETE_VALUE_HEADER_TEXT As String = "CtrlDeleteValueHeaderLabel"
    Private Const DELETE_VALUE_CONTINUE_BUTTON As String = "CtrlDeleteValueContinueButton"
    Private Const CANCEL_BUTTON As String = "CancelButton"
    Private Const DELETE_VALUE_ERRORDESCRIPTION As String = "CtrlDeleteValueErrorDescription"


#End Region

#Region " Controls "
    Protected WithEvents DeleteValuePanel As Panel
    Protected WithEvents TitleLabel As Label
    Protected WithEvents VariableSelectorValueSelectRepeater As Repeater
    Protected WithEvents ContinueButton As Button
    Protected WithEvents CancelButton As Button
    Protected WithEvents DeleteValueTextLabel As Label
    Protected lblError As Label
    Protected hidInit As HiddenField
#End Region

#Region " Properties "

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteValue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VariableSelectorValueSelectRepeater.DataSource = Me.PaxiomModel.Meta.Variables
        VariableSelectorValueSelectRepeater.DataBind()
        LoadTextsForLanguage()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not hidInit.Value.ToLower.Equals("true") Then
            For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
                Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
                variableSelectorValueSelect.DeselectAllValues()
            Next
            hidInit.Value = "true"
        End If
    End Sub

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteValue_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()
        TitleLabel.Text = GetLocalizedString(DELETE_VALUE_TITLE)
        DeleteValueTextLabel.Text = GetLocalizedString(DELETE_VALUE_HEADER_TEXT)
        ContinueButton.Text = GetLocalizedString(DELETE_VALUE_CONTINUE_BUTTON)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
    End Sub





    ''' <summary>
    ''' Add VariableSelectorValueSelect controll and necessary events for each variable in RepeaterValueSelect
    ''' </summary>   
    Protected Sub RepeaterValueSelect_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles VariableSelectorValueSelectRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        'If Marker.Variable.CurrentGrouping Is Nothing Then

        'End If
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim var As Variable = DirectCast(e.Item.DataItem, Variable)
            Dim ValueSelectPlaceHolder As PlaceHolder = DirectCast(item.FindControl("ValueSelectPlaceHolder"), PlaceHolder)
            If ValueSelectPlaceHolder.Controls.Count = 0 Then
                Dim VariableSelect As New VariableSelectorValueSelect
                VariableSelect.ID = "VariableValueSelect"
                VariableSelect.Variable = var
                VariableSelect.MaxRowsWithoutSearch = Integer.MaxValue 'CInt(Me.Properties("MaxRowsWithoutSearch"))
                VariableSelect.ListSize = CInt(Me.Properties("ListSize"))
                VariableSelect.ShowElimMark = False
                VariableSelect.NumberOfValuesInDefaultView = CInt(Me.Properties("NumberOfValuesInDefaultView"))
                VariableSelect.ShowGroupingDropDown = False
                ValueSelectPlaceHolder.Controls.Add(VariableSelect)
            End If
        End If
    End Sub



    ''' <summary>
    ''' Handles event continuebutton clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click

        Dim p As New PCAxis.Paxiom.Operations.DeleteValue
        Dim sels As New List(Of Selection)

        If SaveSelections() Then
            For Each variable As Variable In Me.PaxiomModel.Meta.Variables
                sels.Add(VariableSelector.SelectedVariableValues(variable.Code))
            Next
            If sels.Count > 0 Then
                Me.OnFinished(New CommandBarPluginFinishedEventArgs(p.Execute(Me.PaxiomModel, sels.ToArray())))
                LogFeatureUsage(OperationConstants.DELETE_VALUE, Me.PaxiomModel.Meta)
                UpdateOperationsTracker(sels.ToArray())
            End If
        Else
            lblError.Visible = True
            lblError.Text = GetLocalizedString(DELETE_VALUE_ERRORDESCRIPTION)
        End If


    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub


    ''' <summary>
    ''' Save selections made in the listboxes for each variable
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveSelections() As Boolean
        VariableSelector.SelectedVariableValues.Clear()

        'Get selected items from each variable
        For Each item As RepeaterItem In VariableSelectorValueSelectRepeater.Items
            Dim variableSelectorValueSelect As VariableSelectorValueSelect = DirectCast(item.FindControl("VariableValueSelect"), VariableSelectorValueSelect)
            Dim selection = variableSelectorValueSelect.Selection
            VariableSelector.SelectedVariableValues.Add(variableSelectorValueSelect.Variable.Code, selection)
        Next

        'If all values for a variable is selected return false
        For Each variable As Variable In Me.PaxiomModel.Meta.Variables
            If (VariableSelector.SelectedVariableValues(variable.Code).ValueCodes.Count) >= variable.Values.Count Then
                Return False
            End If
        Next

        'At least one value per variable remains - return true
        Return True
    End Function

    Private Sub UpdateOperationsTracker(sels As Selection())
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.DELETE_VALUE, sels)
        For Each selection As Selection In sels
            Dim va As Variable = Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(Function(v As Variable) v.Code = selection.VariableCode)
            If va IsNot Nothing AndAlso va.IsTime Then
                If selection.ValueCodes.Count > 0 Then
                    PaxiomManager.OperationsTracker.IsTimeDependent = True
                End If
                Exit For
            End If
        Next
    End Sub





End Class

