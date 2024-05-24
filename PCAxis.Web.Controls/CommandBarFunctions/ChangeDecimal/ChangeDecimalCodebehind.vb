
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

''' <summary>
''' Change decimals control.
''' </summary>
''' <remarks>
''' Sets Model.Data properties .Decimals and .ShowDecimals to given value.
''' </remarks>
Public Class ChangeDecimalCodebehind
    Inherits CommandBarPluginBase(Of ChangeDecimalCodebehind, ChangeDecimal)



#Region " Localized strings "

    Private Const CHANGE_DECIMAL_TITLE As String = "CtrlChangeDecimalHeading"
    Private Const CHANGE_DECIMAL_HEADER_TEXT As String = "CtrlChangeDecimalHeaderLabel"
    Private Const CHANGE_DECIMAL_NUMBER_TEXT As String = "CtrlChangeDecimalDecimalLabel"
    Private Const CHANGE_DECIMAL_CONTINUE_BUTTON As String = "CtrlChangeDecimalContinueButton"
    Private Const CHANGE_DECIMAL_INVALID_VALUE As String = "CtrlChangeDecimalRangeError"
    Private Const CANCEL_BUTTON As String = "CancelButton"

#End Region

#Region " Controls "
    Protected WithEvents SelectDecimalPanel As Panel
    Protected WithEvents TitleLabel As Label
    Protected WithEvents SelectNumberOfDecimalsLabel As Label
    Protected WithEvents NumberOfDecimalsLabel As Label
    Protected WithEvents RangeLabel As Label
    Protected WithEvents NumberOfDecimalsTextBox As TextBox
    Protected WithEvents ContinueButton As Button
    Protected WithEvents NumberOfDecimalsValidator As RegularExpressionValidator
    Protected WithEvents RequiredInputValidator As RequiredFieldValidator
    Protected WithEvents CancelButton As Button

#End Region

#Region " Properties "

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangeDecimal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadTextsForLanguage()
        NumberOfDecimalsTextBox.Focus()
    End Sub

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangeDecimal_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()
        TitleLabel.Text = GetLocalizedString(CHANGE_DECIMAL_TITLE)
        SelectNumberOfDecimalsLabel.Text = GetLocalizedString(CHANGE_DECIMAL_HEADER_TEXT)
        NumberOfDecimalsLabel.Text = GetLocalizedString(CHANGE_DECIMAL_NUMBER_TEXT)
        ContinueButton.Text = GetLocalizedString(CHANGE_DECIMAL_CONTINUE_BUTTON)
        NumberOfDecimalsValidator.ErrorMessage = GetLocalizedString(CHANGE_DECIMAL_INVALID_VALUE)
        RequiredInputValidator.ErrorMessage = GetLocalizedString(CHANGE_DECIMAL_INVALID_VALUE)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
    End Sub


    ''' <summary>
    ''' Handles event continuebutton clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        If Page.IsValid Then
            Dim numberOfDecimals As Integer = 0
            If IsNumeric(Me.NumberOfDecimalsTextBox.Text) Then
                numberOfDecimals = CInt(Me.NumberOfDecimalsTextBox.Text)
                If ((numberOfDecimals >= 0) AndAlso (numberOfDecimals <= 6)) Then
                    Dim changeDecimals As New PCAxis.Paxiom.Operations.ChangeDecimals()
                    Dim changeDecimalsDescription As New PCAxis.Paxiom.Operations.ChangeDecimalsDescription(CInt(Me.NumberOfDecimalsTextBox.Text))

                    PaxiomManager.PaxiomModel = changeDecimals.Execute(PaxiomModel, changeDecimalsDescription)
                    PaxiomManager.OperationsTracker.AddStep(OperationConstants.CHANGE_DECIMALS, changeDecimalsDescription)

                    Me.OnFinished(New CommandBarPluginFinishedEventArgs(PaxiomManager.PaxiomModel))
                End If
            End If
        End If

    End Sub


    ' Todo - se till att bara gå igenom variabler av decimaltalstyp
    'Public Function GetSmallestPrecision() As Integer
    '    Dim SmallestPrecision As Integer = Integer.MaxValue
    '    Dim var As Variable
    '    For i As Integer = 0 To Me.PaxiomModel.Data.Model.Meta.Variables.Count - 1
    '        var = Me.PaxiomModel.Data.Model.Meta.Variables(i)
    '        For j As Integer = 0 To var.Values.Count - 1
    '            If var.Values(j).Precision < SmallestPrecision Then
    '                SmallestPrecision = var.Values(j).Precision
    '            End If
    '        Next
    '    Next
    '    Return SmallestPrecision
    'End Function



    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        NumberOfDecimalsValidator.Enabled = False
        RequiredInputValidator.Enabled = False
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub


End Class

