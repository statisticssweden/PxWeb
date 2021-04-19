
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
''' Pivot dialog control.
''' </summary>
''' <remarks>
''' After rearranging is done and the user has clicked the continue button.
''' Then you need to retrive the PXModel from this control to display the changes.
''' </remarks>
Public Class SumCodebehind
    Inherits CommandBarPluginBase(Of SumCodebehind, Sum)

    Private _sumOperation As Paxiom.SumOperationType 'Type of operation
    Private _twoValueOperation As Boolean = False 'True if operationtype requires exactly two operands
    Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(SumCodebehind))

#Region " Localized strings "

    Private Const SUM_TITLE As String = "CtrlSumHeading"
    Private Const SUM_ALLVALUES_TEXT As String = "CtrlSumAllValuesText"
    Private Const SUM_SELECTEDVALUES_TEXT As String = "CtrlSumSelectedValuesText"
    Private Const SUM_SELECTVARIABLES_TITLE As String = "CtrlSumSelectedVariabelTitle"
    Private Const SUM_SELECTVARIABLEVALUES_TITLE As String = "CtrlSumSelectedVariabelValuesTitle"
    Private Const SUM_CONTINUE_BUTTON As String = "CtrlSumContinueButton"
    Private Const SUM_TOTALVARIABLENAME_TEXT As String = "CtrlSumTotalVariabelNameText"
    Private Const SUM_REQUIREDVARIABLENAME_TEXT As String = "CtrlSumVariableNameRequired"
    Private Const SUM_DIFFERENT_OPERANDS_REQUIRED_TEXT As String = "CtrlSumDifferentOperandsRequired"

    Private Const SUBTRACT_TITLE As String = "CtrlSubtractHeading"
    Private Const SUBTRACT_TOTALVARIABLENAME_TEXT As String = "CtrlSubtractTotalVariabelNameText"
    Private Const SUBTRACT_CONTINUE_BUTTON As String = "CtrlSubtractContinueButton"
    Private Const SUBTRACT_SELECTVARIABLES_TITLE As String = "CtrlSubtractSelectedVariabelTitle"
    Private Const SUBTRACT_SELECTVARIABLEVALUES_TITLE As String = "CtrlSubtractSelectedVariabelValuesTitle"

    Private Const DIVIDE_TITLE As String = "CtrlDivideHeading"
    Private Const DIVIDE_TOTALVARIABLENAME_TEXT As String = "CtrlDivideTotalVariabelNameText"
    Private Const DIVIDE_CONTINUE_BUTTON As String = "CtrlDivideContinueButton"
    Private Const DIVIDE_SELECTVARIABLES_TITLE As String = "CtrlDivideSelectedVariabelTitle"
    Private Const DIVIDE_SELECTVARIABLEVALUES_TITLE As String = "CtrlDivideSelectedVariabelValuesTitle"
    Private Const DIVIDE_SAMEVALUESERROR As String = "CtrlDivideErrorSameVariable"

    Private Const MULTIPLY_TITLE As String = "CtrlMultiplyHeading"
    Private Const MULTIPLY_TOTALVARIABLENAME_TEXT As String = "CtrlMultiplyTotalVariabelNameText"
    Private Const MULTIPLY_CONTINUE_BUTTON As String = "CtrlMultiplyContinueButton"
    Private Const MULTIPLY_SELECTVARIABLES_TITLE As String = "CtrlMultiplySelectedVariabelTitle"
    Private Const MULTIPLY_SELECTVARIABLEVALUES_TITLE As String = "CtrlMultiplySelectedVariabelValuesTitle"

    Private Const SUM_KEEPVALUES_TEXT As String = "CtrlSumKeepValuesText"
    Private Const SUM_OPERATION As String = "SumOperationType"
    Private Const FIRST_OPERAND_TEXT As String = "CtrlFirstOperandLabel"
    Private Const SECOND_OPERAND_TEXT As String = "CtrlSecondOperandLabel"
    Private Const CANCEL_BUTTON As String = "CancelButton"
    Private Const INVALID_VALUENAME As String = "SumInvalidValuename"
    Private Const CHOOSE_VARIABLE As String = "CtrlChooseVariable"
    Private Const SUM_OPTIONS As String = "CtrlSumOptions"
    Private Const SUM_COMPLETE_BUTTON_TEXT As String = "CtrlSumComplete"

    Private Const ILLEGAL_CHARACTERS_ERROR As String = "PxWebIllegalCharactersErrorMessage"

#End Region

#Region " Controls "

    Protected WithEvents TitleLiteral As Literal
    Protected WithEvents TotalVariableNameLabel As Label
    Protected WithEvents TotalVariableName As TextBox
    Protected WithEvents ContinueButton As Button

    Protected WithEvents SelectVariableOptionsPanel As Panel
    Protected WithEvents ChooseVariablePanel As Panel
    Protected WithEvents VariableToSumRadioButtonList As RadioButtonList
    Protected WithEvents ContinueButtonSelectVariables As Button

    Protected WithEvents SelectValuesOptionsPanel As Panel
    Protected WithEvents SelectVariableValuesLabel As Label
    Protected WithEvents ValuesToSumListBox As ListBox
    Protected WithEvents ContinueButtonSelectValues As Button

    Protected SumOptionPanel As Panel
    Protected SumOptionsRadioButtonList As RadioButtonList
    Protected WithEvents TotalVariableNamePanel As Panel
    Protected KeepValuesPanel As Panel
    Protected KeepValuesCheckBox As CheckBox
    Protected KeepValuesLabel As Label
    Protected WithEvents SelectOperandsPanel As Panel
    Protected WithEvents FirstOperandLabel As Label
    Protected WithEvents SecondOperandLabel As Label
    Protected WithEvents FirstOperandDropDown As DropDownList
    Protected WithEvents SecondOperandDropDown As DropDownList
    Protected ErrorMessageLabel As Label
    Protected ErrorMessagePanel As Panel
    Protected TotalVariableNameRequired As RequiredFieldValidator
    Protected OperandsValidator As CompareValidator
    Protected WithEvents CancelButton As Button
    Protected WithEvents CancelButton2 As Button
    Protected WithEvents CancelButton3 As Button
#End Region

#Region " Properties "
    ''' <summary>
    ''' Datasource for the right listbox.
    ''' </summary>
    Private _stubListData As List(Of String)


    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property StubListData() As List(Of String)
        Get
            Return _stubListData
        End Get
        Set(ByVal value As List(Of String))
            _stubListData = value
        End Set
    End Property

    ''' <summary>
    ''' Datasource for the Left listbox.
    ''' </summary>    
    Private _headingListData As List(Of String)

    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property HeadingListData() As List(Of String)
        Get
            Return _headingListData
        End Get
        Set(ByVal value As List(Of String))
            _headingListData = value
        End Set
    End Property


#End Region


    Private Sub Sum_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            _sumOperation = CType([Enum].Parse(GetType(Paxiom.SumOperationType), Me.Properties(SUM_OPERATION)), Paxiom.SumOperationType)
        Catch ex As ArgumentException
            _logger.WarnFormat("Zero operation {0} for Paxiom.ZeroOptionType does not exist.", Me.Properties(SUM_OPERATION))
            Exit Sub
        End Try
        _twoValueOperation = (_sumOperation <> SumOperationType.Addition AndAlso _sumOperation <> SumOperationType.Grouping)

        If _twoValueOperation Then
            SumOptionsRadioButtonList.SelectedValue = "SumSelected"
        End If

        LoadTextsForLanguage()

    End Sub

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Sum_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()
        Dim buttonText As String = "Continue"
        Select Case _sumOperation
            Case SumOperationType.Division
                buttonText = GetLocalizedString(DIVIDE_CONTINUE_BUTTON)
                TitleLiteral.Text = GetLocalizedString(DIVIDE_TITLE)
                TotalVariableNameLabel.Text = GetLocalizedString(DIVIDE_TOTALVARIABLENAME_TEXT)
                ContinueButton.Text = buttonText
                ContinueButtonSelectVariables.Text = buttonText
                SelectVariableValuesLabel.Text = GetLocalizedString(DIVIDE_SELECTVARIABLEVALUES_TITLE)
                ContinueButtonSelectValues.Text = buttonText
            Case SumOperationType.Multiplication
                buttonText = GetLocalizedString(MULTIPLY_CONTINUE_BUTTON)
                TitleLiteral.Text = GetLocalizedString(MULTIPLY_TITLE)
                TotalVariableNameLabel.Text = GetLocalizedString(MULTIPLY_TOTALVARIABLENAME_TEXT)
                ContinueButton.Text = buttonText
                ContinueButtonSelectVariables.Text = buttonText
                SelectVariableValuesLabel.Text = GetLocalizedString(MULTIPLY_SELECTVARIABLEVALUES_TITLE)
                ContinueButtonSelectValues.Text = buttonText
            Case SumOperationType.Subtraction
                buttonText = GetLocalizedString(SUBTRACT_CONTINUE_BUTTON)
                TitleLiteral.Text = GetLocalizedString(SUBTRACT_TITLE)
                TotalVariableNameLabel.Text = GetLocalizedString(SUBTRACT_TOTALVARIABLENAME_TEXT)
                ContinueButton.Text = buttonText
                ContinueButtonSelectVariables.Text = buttonText
                SelectVariableValuesLabel.Text = GetLocalizedString(SUBTRACT_SELECTVARIABLEVALUES_TITLE)
                ContinueButtonSelectValues.Text = buttonText
            Case Else
                buttonText = GetLocalizedString(SUM_CONTINUE_BUTTON)
                TitleLiteral.Text = GetLocalizedString(SUM_TITLE)
                TotalVariableNameLabel.Text = GetLocalizedString(SUM_TOTALVARIABLENAME_TEXT)
                ContinueButton.Text = GetLocalizedString(SUM_COMPLETE_BUTTON_TEXT)
                ContinueButtonSelectVariables.Text = buttonText
                SelectVariableValuesLabel.Text = GetLocalizedString(SUM_SELECTVARIABLEVALUES_TITLE)
                ContinueButtonSelectValues.Text = buttonText
                SumOptionsRadioButtonList.Items(0).Text = GetLocalizedString(SUM_ALLVALUES_TEXT)
                SumOptionsRadioButtonList.Items(1).Text = GetLocalizedString(SUM_SELECTEDVALUES_TEXT)
        End Select

        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
        CancelButton2.Text = GetLocalizedString(CANCEL_BUTTON)
        CancelButton3.Text = GetLocalizedString(CANCEL_BUTTON)
        KeepValuesLabel.Text = GetLocalizedString(SUM_KEEPVALUES_TEXT)
        TotalVariableNameRequired.ErrorMessage = GetLocalizedString(SUM_REQUIREDVARIABLENAME_TEXT)
        OperandsValidator.ErrorMessage = GetLocalizedString(SUM_DIFFERENT_OPERANDS_REQUIRED_TEXT)
        ChooseVariablePanel.GroupingText = "<span class='font-heading'>" + GetLocalizedString(CHOOSE_VARIABLE) + "</span>"
        SumOptionPanel.GroupingText = "<span class='font-heading'>" + GetLocalizedString(SUM_OPTIONS) + "</span>"

        If _twoValueOperation Then
            FirstOperandLabel.Text = GetLocalizedString(FIRST_OPERAND_TEXT)
            SecondOperandLabel.Text = GetLocalizedString(SECOND_OPERAND_TEXT)
        End If

    End Sub

    ''' <summary>
    ''' Renders one radio button for each variable in table
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRadioButtonList()

        If (VariableToSumRadioButtonList.Items.Count < 1) Then

            For Each variable As Variable In Me.PaxiomModel.Meta.Variables
                Dim itm As New ListItem(variable.Name, variable.Code)
                If Not variable.IsContentVariable Then
                    VariableToSumRadioButtonList.Items.Add(itm)
                End If
            Next
            If (VariableToSumRadioButtonList.Items.Count > 0) Then
                VariableToSumRadioButtonList.SelectedIndex = 0
            End If
        End If
    End Sub


    ''' <summary>
    ''' Recursive function used to get a codevalue for a variablevalue that is unique
    ''' </summary>
    ''' <param name="v">Variable to check valuecodes for</param>
    ''' <param name="valueName">Name of the value intended to get the code</param>
    ''' <param name="testCode">Suggested code for the value</param>
    ''' <param name="index">Integer to add at the end of the codevalue if it not is already unique</param>
    ''' <returns>Unique code for the variablevalue</returns>
    ''' <remarks></remarks>
    Private Function GetNewValueCode(ByVal v As Variable, ByVal valueName As String, ByVal testCode As String, ByVal index As Integer) As String

        For Each val As Value In v.Values
            If val.Code = testCode Then
                index += 1
                testCode = GetNewValueCode(v, valueName, valueName + index.ToString(), index)
                Exit For
            End If
        Next
        Return testCode

    End Function



    ''' <summary>
    '''  Sum all values for selected variable
    ''' </summary>    
    ''' <remarks></remarks>
    Private Sub SumAll()
        Dim valuesToSum As New List(Of String)
        Dim SumDesc As New PCAxis.Paxiom.Operations.SumDescription
        Dim newValuenameIsValid As Boolean = True
        Dim newValuecodeIsValid As Boolean = True


        For Each v As Variable In Me.PaxiomModel.Meta.Variables
            If (v.Code = VariableToSumRadioButtonList.SelectedValue) Then
                For Each val As Value In v.Values
                    If val.Text = TotalVariableName.Text Then
                        newValuenameIsValid = False
                        Exit For
                    End If
                    valuesToSum.Add(val.Code)
                Next

                SumDesc.VariableCode = v.Code
                SumDesc.ValueCodes = valuesToSum
                SumDesc.NewValueName = TotalVariableName.Text
                SumDesc.NewValueCode = GetNewValueCode(v, TotalVariableName.Text, TotalVariableName.Text, 0) 'Guid.NewGuid().ToString()
                SumDesc.KeepValues = KeepValuesCheckBox.Checked
                SumDesc.Operation = _sumOperation
                Exit For
            End If
        Next
        If newValuenameIsValid Then
            Try
                Dim s As New PCAxis.Paxiom.Operations.Sum
                Me.OnFinished(New CommandBarPluginFinishedEventArgs(s.Execute(Me.PaxiomModel, SumDesc)))
                LogFeatureUsage(OperationConstants.SUM, "SumAll", Me.PaxiomModel.Meta.TableID)
                UpdateOperationsTracker(SumDesc)
            Catch ex As PXOperationException
                Me.ErrorMessagePanel.Visible = True
                Me.ErrorMessageLabel.Text = ex.Message
            End Try
        Else
            Me.ErrorMessagePanel.Visible = True
            Me.ErrorMessageLabel.Text = GetLocalizedString(INVALID_VALUENAME)
        End If
    End Sub

    ''' <summary>
    ''' sum selected values for selected variable
    ''' </summary>    
    ''' <remarks></remarks>
    Private Sub SumSelected()

        Dim valuesToSum As New List(Of String)
        Dim newValuenameIsValid As Boolean = True

        For Each v As Variable In Me.PaxiomModel.Meta.Variables

            If (v.Code = VariableToSumRadioButtonList.SelectedValue) Then
                'If exactly two values in right order are required as input for operation, get them from textboxes
                If _twoValueOperation Then
                    Dim firstValue As String = ""
                    Dim secondValue As String = ""
                    Dim foundValues As Integer = 0
                    For Each _value As Value In v.Values
                        'If FirstOperandDropDown.Text = _value.Value Then
                        If FirstOperandDropDown.SelectedValue = _value.Code Then
                            firstValue = _value.Code
                            foundValues += 1
                        End If
                        'If SecondOperandDropDown.Text = _value.Value Then
                        If SecondOperandDropDown.SelectedValue = _value.Code Then
                            secondValue = _value.Code
                            foundValues += 1
                        End If
                        If foundValues = 2 Then
                            valuesToSum.Add(firstValue)
                            valuesToSum.Add(secondValue)
                            Exit For
                        End If
                    Next
                Else
                    'Get all selected values as input to operation
                    For Each val As Value In v.Values
                        If val.Text.ToLower() = TotalVariableName.Text.ToLower() Then
                            newValuenameIsValid = False
                            Exit For
                        End If
                        Dim i As ListItem = ValuesToSumListBox.Items.FindByValue(val.Text)
                        If i.Selected Then
                            valuesToSum.Add(val.Code)
                        End If
                    Next
                End If

                If newValuenameIsValid Then
                    If (Not _twoValueOperation AndAlso valuesToSum.Count > 0) OrElse (_twoValueOperation AndAlso valuesToSum.Count = 2) Then
                        Dim SumDesc As New PCAxis.Paxiom.Operations.SumDescription
                        SumDesc.VariableCode = v.Code
                        SumDesc.ValueCodes = valuesToSum
                        SumDesc.NewValueName = TotalVariableName.Text
                        SumDesc.NewValueCode = GetNewValueCode(v, TotalVariableName.Text, TotalVariableName.Text, 0) 'Guid.NewGuid().ToString()
                        SumDesc.KeepValues = KeepValuesCheckBox.Checked
                        SumDesc.Operation = _sumOperation

                        ' Check division by same variable
                        If IsDivisionBySameValue(SumDesc) Then
                            Me.ErrorMessagePanel.Visible = True
                            Me.ErrorMessageLabel.Text = GetLocalizedString(DIVIDE_SAMEVALUESERROR)
                            Return
                        End If

                        Dim s As New PCAxis.Paxiom.Operations.Sum
                        UpdateOperationsTracker(SumDesc)
                        Me.OnFinished(New CommandBarPluginFinishedEventArgs(s.Execute(Me.PaxiomModel, SumDesc)))
                        LogFeatureUsage(OperationConstants.SUM, _sumOperation.ToString, Me.PaxiomModel.Meta.TableID)
                    Else
                        Me.ErrorMessagePanel.Visible = True
                        Me.ErrorMessageLabel.Text = "SumCodebehind.SumSelected():Wrong number of values."
                    End If
                Else
                    Me.ErrorMessagePanel.Visible = True
                    Me.ErrorMessageLabel.Text = GetLocalizedString(INVALID_VALUENAME)
                End If
            End If

        Next

    End Sub


    Private Function IsDivisionBySameValue(ByVal SumDesc As Operations.SumDescription) As Boolean
        If SumDesc.Operation = SumOperationType.Division Then
            If SumDesc.ValueCodes.Count = 2 Then
                Return SumDesc.ValueCodes(0) = SumDesc.ValueCodes(1)
            End If
        End If
        Return False
    End Function



    ''' <summary>
    ''' Populate controls for operandselection depending on selected variable to do operation on
    ''' </summary>    
    ''' <remarks></remarks>
    Private Sub PopulateValuesToSumDropDown(ByVal v As Variable)

        If _twoValueOperation Then
            For Each val As Value In v.Values
                'FirstOperandDropDown.Items.Add(val.ToString)
                'SecondOperandDropDown.Items.Add(val.ToString)
                FirstOperandDropDown.Items.Add(New ListItem(val.ToString(), val.Code))
                SecondOperandDropDown.Items.Add(New ListItem(val.ToString(), val.Code))
            Next
        Else
            ValuesToSumListBox.Items.Clear()
            ValuesToSumListBox.SelectionMode = ListSelectionMode.Multiple
            For Each val As Value In v.Values
                ValuesToSumListBox.Items.Add(val.ToString)
            Next
            If ValuesToSumListBox.Items.Count > 0 Then
                ValuesToSumListBox.Items(0).Selected = True
            End If
        End If



    End Sub



    ''' <summary>
    '''  Handles eventcontinuebutton clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Called when Variable to operate on is selected</remarks>
    Private Sub ContinueButtonSelectVariables_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButtonSelectVariables.Click


        If SumOptionsRadioButtonList.Items(0).Selected Then
            SelectVariableOptionsPanel.Visible = False
            SelectValuesOptionsPanel.Visible = False
            TotalVariableNamePanel.Visible = True
            KeepValuesPanel.Visible = False
            TotalVariableName.Focus()
        Else
            For Each v As Variable In Me.PaxiomModel.Meta.Variables
                If (v.Code = VariableToSumRadioButtonList.SelectedValue) Then
                    PopulateValuesToSumDropDown(v)
                End If
            Next

            SelectValuesOptionsPanel.Visible = True
            SelectVariableOptionsPanel.Visible = False
            If _twoValueOperation Then
                SelectOperandsPanel.Visible = True
            Else
                ValuesToSumListBox.Visible = True
            End If
        End If


    End Sub


    ''' <summary>
    '''  Handles event continuebutton clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Called when values to perform operation on is selected</remarks>
    Private Sub ContinueButtonSelectValues_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButtonSelectValues.Click
        SelectVariableOptionsPanel.Visible = False
        SelectValuesOptionsPanel.Visible = False
        TotalVariableNamePanel.Visible = True
        KeepValuesPanel.Visible = True
        TotalVariableName.Focus()
    End Sub


    ''' <summary>
    '''  Handles event continuebutton clicked. Calls SumAll or SumSelected.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Called when all options for operation is set and and text for the resulting value is given </remarks>
    Private Sub ContinueButton_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(TotalVariableName.Text) Then
            Me.ErrorMessagePanel.Visible = True
            Me.ErrorMessageLabel.Text = GetLocalizedString(ILLEGAL_CHARACTERS_ERROR)
            Return
        ElseIf String.IsNullOrWhiteSpace(TotalVariableName.Text) Then
            Me.ErrorMessagePanel.Visible = True 'Display required field validator
            Return
        End If

        If SumOptionsRadioButtonList.Items(0).Selected Then
            SumAll()
        Else
            SumSelected()
        End If
    End Sub

    ''' <summary>
    ''' Handles event pre render for panel with variables
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Loads list with radiobutton for each variable</remarks>
    Private Sub SelectVariableOptionsPanel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectVariableOptionsPanel.PreRender
        LoadRadioButtonList()
        If _twoValueOperation Then
            SumOptionsRadioButtonList.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click, CancelButton2.Click, CancelButton3.Click
        TotalVariableNameRequired.Enabled = False
        OperandsValidator.Enabled = False
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub

    Private Sub UpdateOperationsTracker(SumDesc As SumDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.SUM, SumDesc)
        Dim va As Variable = Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(Function(v As Variable) v.Code = SumDesc.VariableCode)
        If va IsNot Nothing AndAlso va.IsTime Then
            PaxiomManager.OperationsTracker.IsTimeDependent = True
        End If
    End Sub

End Class

