
Imports System.Web.UI.WebControls
Imports PCAxis.Paxiom
Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Management


''' <summary>
''' PerPart control.
''' </summary>
''' <remarks>
''' 
''' </remarks>
Public Class PerPartCodebehind
    Inherits CommandBarPluginBase(Of PerPartCodebehind, PerPart)

#Region " Localized strings "

    Private Const PERPART_TITLE As String = "CtrlCommandBarFunctionPerPart"
    Private Const PERPART_CONTINUE_BUTTON As String = "CtrlPerPartContinueButton"
    Private Const PERPART_NEW_VALUE_NAME As String = "CtrlPerPartNewValueName"
    Private Const LOC_ILLEGAL_CHARACTERS_ERROR As String = "PxWebIllegalCharactersErrorMessage"
    Private Const PERPART_KEEPDATA As String = "CtrlPerPartKeepData"
    Private Const PERPART_PERPART_ONLY As String = "CtrlPerPartPerCentOnly"
    Private Const PERPART_SELECTEDVARIABLENAME As String = "CtrlPerPartSelectedVariableNameLabel"
    Private Const PERPART_ONEVARIABLEALLVALUES As String = "CtrlPerPartOneVariableAllValues"
    Private Const PERPART_ONEVARIABLEONEVALUE As String = "CtrlPerPartOneVariableOneValue"
    Private Const PERPART_ONEMATRIXVALUE As String = "CtrlPerPartOneMatrixValue"
    Private Const CANCEL_BUTTON As String = "CancelButton"
    Private Const PERPART_COMPLETE_BUTTON As String = "CtrlPerPartCompleteButton"
#End Region

#Region " Controls "

    Protected NewVariableNameRequired As RequiredFieldValidator
    Protected VariableSelectionRequired As RequiredFieldValidator

    Protected PerPartPanel As Panel
    Protected CalcOptionsPanel As Panel

    'Name and KeepValue
    Protected NewValueNameLabel As Label
    Protected NewValueNameTextBox As TextBox

    Protected KeepValueRadioPanel As Panel
    Protected KeepValueRadio As RadioButtonList
    Protected lblError As Label

    'select option
    Protected SelectOptionPanel As Panel
    Protected SelectOptionRadioPanel As Panel
    Protected SelectOptionRadio As RadioButtonList
    Protected WithEvents SelectOption_ContinueButton As Button

    'select variable (option 1)
    Protected SelectVariableRepeaterPanel As Panel
    Protected WithEvents SelectVariableRepeater As Repeater
    Protected SelectVariablePanel As Panel
    Protected WithEvents SelectVariable_ContinueButton As Button

    'calculate variable value (option 2)
    Protected CalculateOneVariablePanel As Panel
    Protected SelectedVariableNameLabel As Label
    Protected WithEvents CalculateOneVariableListBox As ListBox
    Protected WithEvents CalculateOneVariable_ContinueButton As Button

    'Calculate datacells (option 3)
    Protected CalculateAllVariablesPanel As Panel
    Protected WithEvents CalculateAllVariablesRepeater As Repeater
    Protected WithEvents CalculateAllVariables_ContinueButton As Button
    Protected WithEvents CalculateAllVariablesValuesListBox As ListBox

    'error message
    Protected ErrorMessagePanel As Panel
    Protected ErrorMessageLabel As Label
    Protected InfoMessageLabel As Label

    Protected WithEvents CancelButton1 As Button
    Protected WithEvents CancelButton2 As Button
    Protected WithEvents CancelButton3 As Button
    Protected WithEvents CancelButton4 As Button
    Protected WithEvents CancelButton5 As Button
#End Region

#Region " Properties "
    ''' <summary>
    ''' Datasource for the right listbox.
    ''' </summary>

    Private _nameNewValue As String = "New value"
    Private _keepValue As Boolean = True
    Private _operationType As CalculatePerPartType = CalculatePerPartType.PerCent


    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property NameNewValue() As String
        Get
            Return _nameNewValue
        End Get
        Set(ByVal value As String)
            _nameNewValue = value
        End Set
    End Property

    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property KeepValue() As Boolean
        Get
            Return _keepValue
        End Get
        Set(ByVal value As Boolean)
            _keepValue = value
        End Set
    End Property

    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property OperationType() As CalculatePerPartType
        Get
            Return _operationType
        End Get
        Set(ByVal value As CalculatePerPartType)
            _operationType = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Control_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PerPart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Check that sortvariable not already exists
        If (Not PaxiomModel.Meta.Variables.GetByCode(PXConstant.SORTVARIABLE) Is Nothing) Then
            Me.InfoMessageLabel.Text = GetLocalizedString("PxcPercentUnitVarExists")
            Me.CancelButton5.Text = GetLocalizedString(CANCEL_BUTTON)
            Me.ErrorMessagePanel.Visible = True
            Me.CalcOptionsPanel.Visible = False
            Me.SelectOptionPanel.Visible = False
        Else
            LoadTextsForLanguage()
        End If
    End Sub

    ''' <summary>
    '''  Handles event of language changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PerPart_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub


    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()
        Dim continueButtonText As String = GetLocalizedString(PERPART_CONTINUE_BUTTON)
        NewValueNameTextBox.Text = GetLocalizedString(PERPART_TITLE) ' Also used as Title
        NewValueNameLabel.Text = GetLocalizedString(PERPART_NEW_VALUE_NAME)
        SelectOptionRadio.Items(0).Text = GetLocalizedString(PERPART_ONEVARIABLEALLVALUES)
        SelectOptionRadio.Items(1).Text = GetLocalizedString(PERPART_ONEVARIABLEONEVALUE)
        SelectOptionRadio.Items(2).Text = GetLocalizedString(PERPART_ONEMATRIXVALUE)

        KeepValueRadio.Items(0).Text = GetLocalizedString(PERPART_KEEPDATA)
        KeepValueRadio.Items(1).Text = GetLocalizedString(PERPART_PERPART_ONLY)

        SelectOption_ContinueButton.Text = continueButtonText
        SelectVariable_ContinueButton.Text = GetLocalizedString(PERPART_COMPLETE_BUTTON)
        CalculateOneVariable_ContinueButton.Text = GetLocalizedString(PERPART_COMPLETE_BUTTON)
        CalculateAllVariables_ContinueButton.Text = GetLocalizedString(PERPART_COMPLETE_BUTTON)

        CancelButton1.Text = GetLocalizedString(CANCEL_BUTTON)
        CancelButton2.Text = GetLocalizedString(CANCEL_BUTTON)
        CancelButton3.Text = GetLocalizedString(CANCEL_BUTTON)
        CancelButton4.Text = GetLocalizedString(CANCEL_BUTTON)


        KeepValueRadioPanel.GroupingText = "<span class='font-heading'>" + Me.GetLocalizedString("CtrlPerPartKeepValueRadioLegend") + "</span>"
        SelectOptionRadioPanel.GroupingText = "<span class='font-heading'>" + Me.GetLocalizedString("CtrlPerPartSelectOptionRadioLegend") + "</span>"
        SelectVariableRepeaterPanel.GroupingText = "<span class='font-heading'>" + Me.GetLocalizedString("CtrlChooseVariable") + "</span>"

    End Sub

    Private Function SetProps() As Boolean

        If Not PCAxis.Web.Core.Management.ValidationManager.CheckValue(NewValueNameTextBox.Text) Then
            lblError.Visible = True
            lblError.Text = Me.GetLocalizedString(LOC_ILLEGAL_CHARACTERS_ERROR)
            Return False
        End If

        lblError.Visible = False
        _keepValue = KeepValueRadio.Items(0).Selected
        _nameNewValue = NewValueNameTextBox.Text

        Return True
    End Function

#Region "RepeaterItem binding"

    ''' <summary>
    ''' Add VariableNameRadio and necessary events for each variable in SelectVariableRepeater
    ''' </summary>   
    ''' <remarks>Used for Option 1 and as first step for Option 2. Displays RadioButton for each variable with attribute "VariableCode"</remarks>
    Protected Sub SelectVariableRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles SelectVariableRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim var As Variable = DirectCast(e.Item.DataItem, Variable)
            Dim SelectVariablePanel As Panel = DirectCast(item.FindControl("SelectVariablePanel"), Panel)

            'Radiobutton with text and attributes
            Dim VariableNameRadio As RadioButton = DirectCast(item.FindControl("VariableNameRadio"), RadioButton)
            VariableNameRadio.Text = var.Name
            VariableNameRadio.Attributes.Add("VariableCode", var.Code)
            VariableNameRadio.Attributes.Add("onclick", "SetUniqueRadioButton('SelectVariableRepeater.*VariableSelectionGroup',this)")
            If item.ItemIndex = 0 Then
                VariableNameRadio.Checked = True
            End If
        End If
    End Sub


    ''' <summary>
    ''' Add ValuesListBox with values and attribute "VariableCode" in CalculateAllVariablesRepeater
    ''' </summary>   
    ''' <remarks>Used for Option 3. Displays ListBoxes with values for each variable</remarks>
    Protected Sub CalculateAllVariablesRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles CalculateAllVariablesRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim var As Variable = DirectCast(e.Item.DataItem, Variable)
            Dim ValuesSelectPanel As Panel = DirectCast(item.FindControl("CalculateAllVariablesValuesSelectPanel"), Panel)

            'Title
            Dim VariableNameLabel As Label = DirectCast(item.FindControl("VariableNameLabel"), Label)

            If var.Name.Length > 1 Then
                VariableNameLabel.Text = var.Name.Substring(0, 1).ToUpper() + var.Name.Substring(1)
            Else
                VariableNameLabel.Text = var.Name
            End If

            'Listbox with variablevalues and attribute "VariableCode"
            Dim ValuesListBox As ListBox = DirectCast(item.FindControl("CalculateAllVariablesValuesListBox"), ListBox)
            ValuesListBox.SelectionMode = ListSelectionMode.Single
            ValuesListBox.Rows = CInt(Me.Properties("ListSize"))
            ValuesListBox.Attributes.Add("VariableCode", var.Code)
            ValuesListBox.Attributes.Add("aria-label", var.Name)
            ValuesListBox.DataTextField = "Value"
            ValuesListBox.DataValueField = "Code"

            'Databind
            Dim valuesToShow As Values = var.Values

            ValuesListBox.DataSource = valuesToShow
            ValuesListBox.DataBind()
            If ValuesListBox.Items.Count > 0 Then
                ValuesListBox.Items(0).Selected = True
            End If

        End If
    End Sub


#End Region

#Region "Button_Click eventhandlers"

    ''' <summary>
    ''' Handles event continuebutton clicked for selection of calculation option
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Clicked when one of calculation option 1..3 is selected.</remarks>
    Private Sub SelectOption_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectOption_ContinueButton.Click
        If Not SetProps() Then
            Return
        End If
        Me.SelectOptionPanel.Visible = False
        Select Case Me.SelectOptionRadio.SelectedValue
            Case "1"
                Me.SelectVariablePanel.Visible = True
                Me.SelectVariable_ContinueButton.Attributes.Add("SelectedOption", "1")
                SelectVariableRepeater.DataSource = (Me.PaxiomModel.Meta.Variables)
                SelectVariableRepeater.DataBind()
            Case "2"
                Me.SelectVariablePanel.Visible = True
                Me.SelectVariable_ContinueButton.Attributes.Add("SelectedOption", "2")
                SelectVariableRepeater.DataSource = (Me.PaxiomModel.Meta.Variables)
                SelectVariableRepeater.DataBind()
                SelectVariable_ContinueButton.Text = GetLocalizedString(PERPART_CONTINUE_BUTTON)
            Case "3"
                Me.CalculateAllVariablesPanel.Visible = True
                CalculateAllVariablesRepeater.DataSource = (Me.PaxiomModel.Meta.Variables)
                CalculateAllVariablesRepeater.DataBind()
        End Select
    End Sub



    ''' <summary>
    ''' Handles event continuebutton clicked for Option 1 and as first step for Option 2
    ''' </summary>
    ''' <remarks>
    ''' Clicked when one Variable is selected
    ''' Calculation Option 1: Calculation with selected variable as base is executed
    ''' Calculation Option 2: Values for selected variable is listed and CalculateOneVariablePanel is displayed
    '''</remarks>
    Private Sub SelectVariable_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectVariable_ContinueButton.Click
        If Not SetProps() Then
            Return
        End If
        Select Case SelectVariable_ContinueButton.Attributes("SelectedOption")
            Case "1"
                'Variable is selected and we are ready to calculate
                Dim paxiomOperation As New PCAxis.Paxiom.Operations.CalculatePerPart
                Dim calcPerPartDescription As PCAxis.Paxiom.Operations.CalculatePerPartDescription = CalculateOnSelectedVariable()
                calcPerPartDescription.KeepValue = _keepValue
                calcPerPartDescription.OperationType = _operationType
                calcPerPartDescription.ValueName = _nameNewValue

                'Execute operation
                If calcPerPartDescription.ValueSelection.Count > 0 Then
                    Try
                        Dim model As PXModel = paxiomOperation.Execute(Me.PaxiomModel, calcPerPartDescription)
                        UpdateOperationsTracker(calcPerPartDescription)
                        Me.OnFinished(New CommandBarPluginFinishedEventArgs(model))
                        LogFeatureUsage(OperationConstants.PER_PART, calcPerPartDescription.CalculationVariant.ToString, Me.PaxiomModel.Meta)

                    Catch ex As PXOperationException
                        Me.ErrorMessagePanel.Visible = True
                        Me.ErrorMessageLabel.Text = ex.Message
                    End Try
                End If

            Case "2"
                'Variable is selected, list the values and show the panel for calculation with one value for one variable
                Dim VariableNameRadio As RadioButton
                For Each itm As RepeaterItem In SelectVariableRepeater.Items
                    'Find the selected variable
                    VariableNameRadio = DirectCast(itm.FindControl("VariableNameRadio"), RadioButton)
                    If VariableNameRadio.Checked Then
                        SelectedVariableNameLabel.Text = String.Format(GetLocalizedString(PERPART_SELECTEDVARIABLENAME), VariableNameRadio.Text)

                        'Listbox with variablevalues
                        CalculateOneVariableListBox.SelectionMode = ListSelectionMode.Single
                        CalculateOneVariableListBox.Rows = CInt(Me.Properties("ListSize"))
                        CalculateOneVariableListBox.Attributes.Add("VariableCode", VariableNameRadio.Attributes("VariableCode"))
                        CalculateOneVariableListBox.DataTextField = "Value"
                        CalculateOneVariableListBox.DataValueField = "Code"

                        'Databind
                        Dim var As Paxiom.Variable = Me.PaxiomModel.Meta.Variables.GetByCode(VariableNameRadio.Attributes.Item("VariableCode"))
                        CalculateOneVariableListBox.DataSource = var.Values
                        CalculateOneVariableListBox.DataBind()

                        'Show/Hide panels
                        Me.CalculateOneVariablePanel.Visible = True
                        Me.SelectVariablePanel.Visible = False
                        Exit For
                    End If
                Next
        End Select
    End Sub


    ''' <summary>
    ''' Handles event continuebutton clicked for Option 2
    ''' </summary>
    ''' <remarks>Clicked after one Value for selected Variable is selected</remarks>
    Private Sub CalculateOneVariable_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalculateOneVariable_ContinueButton.Click
        Dim paxiomOperation As New PCAxis.Paxiom.Operations.CalculatePerPart
        Dim calcPerPartDescription As PCAxis.Paxiom.Operations.CalculatePerPartDescription = CalculateOnSelectedValue()
        If Not SetProps() Then
            Return
        End If
        calcPerPartDescription.KeepValue = _keepValue
        calcPerPartDescription.OperationType = _operationType
        calcPerPartDescription.ValueName = _nameNewValue

        'Execute operation
        If calcPerPartDescription.ValueSelection.Count > 0 Then
            Try
                Dim model As PXModel = paxiomOperation.Execute(Me.PaxiomModel, calcPerPartDescription)
                UpdateOperationsTracker(calcPerPartDescription)
                Me.OnFinished(New CommandBarPluginFinishedEventArgs(model))
                LogFeatureUsage(OperationConstants.PER_PART, calcPerPartDescription.CalculationVariant.ToString, Me.PaxiomModel.Meta)
            Catch ex As PXOperationException
                Me.ErrorMessagePanel.Visible = True
                Me.ErrorMessageLabel.Text = ex.Message
            End Try
        End If
    End Sub


    ''' <summary>
    ''' Handles event continuebutton clicked for Option 3
    ''' </summary>
    ''' <remarks>Clicked after one Value for each Variable is selected</remarks>
    Private Sub CalculateAllVariables_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalculateAllVariables_ContinueButton.Click
        Dim paxiomOperation As New PCAxis.Paxiom.Operations.CalculatePerPart
        Dim calcPerPartDescription As PCAxis.Paxiom.Operations.CalculatePerPartDescription = CalculateOnAllVariables()
        If Not SetProps() Then
            Return
        End If
        calcPerPartDescription.KeepValue = _keepValue
        calcPerPartDescription.OperationType = _operationType
        calcPerPartDescription.ValueName = _nameNewValue

        'Execute operation
        If calcPerPartDescription.ValueSelection.Count > 0 Then
            Try
                Dim model As PXModel = paxiomOperation.Execute(Me.PaxiomModel, calcPerPartDescription)
                UpdateOperationsTracker(calcPerPartDescription)
                Me.OnFinished(New CommandBarPluginFinishedEventArgs(model))
                LogFeatureUsage(OperationConstants.PER_PART, calcPerPartDescription.CalculationVariant.ToString, Me.PaxiomModel.Meta)
            Catch ex As PXOperationException
                Me.ErrorMessagePanel.Visible = True
                Me.ErrorMessageLabel.Text = ex.Message
            End Try
        End If


    End Sub


#End Region

#Region "Functions"

    ''' <summary>
    ''' Creates CalculatePerPartDescription for PerPartCalculation of type OneVariableAllValues (All values from one variable = Option 1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CalculateOnSelectedVariable() As PCAxis.Paxiom.Operations.CalculatePerPartDescription
        Dim VariableNameRadio As RadioButton
        Dim paxiomOperation As New PCAxis.Paxiom.Operations.CalculatePerPart
        Dim calcPerPartDescription As New PCAxis.Paxiom.Operations.CalculatePerPartDescription
        Dim selections As List(Of Paxiom.Selection) = New List(Of Paxiom.Selection)
        For Each itm As RepeaterItem In SelectVariableRepeater.Items
            'Find the selected variable
            VariableNameRadio = DirectCast(itm.FindControl("VariableNameRadio"), RadioButton)
            If VariableNameRadio.Checked Then
                Dim variableCode As String = VariableNameRadio.Attributes.Item("VariableCode")
                If Not String.IsNullOrEmpty(variableCode) Then
                    Dim var As Variable = Me.PaxiomModel.Meta.Variables.GetByCode(variableCode)
                    Dim selection = New Paxiom.Selection(var.Code)
                    For Each val As Value In var.Values
                        selection.ValueCodes.Add(val.Code)
                    Next
                    selections.Add(selection)
                    Exit For
                End If
            End If
        Next

        'CalculatePerPartDescription 
        calcPerPartDescription.CalculationVariant = Paxiom.CalculatePerPartSelectionType.OneVariableAllValues
        calcPerPartDescription.ValueSelection = selections.ToArray()

        'Return CalculatePerPartDescription
        Return calcPerPartDescription

    End Function

    ''' <summary>
    ''' Creates CalculatePerPartDescription for  PerPartCalculation of type OneVariableOneValue (One value from one variable = Option 2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CalculateOnSelectedValue() As PCAxis.Paxiom.Operations.CalculatePerPartDescription

        Dim paxiomOperation As New PCAxis.Paxiom.Operations.CalculatePerPart
        Dim calcPerPartDescription As New PCAxis.Paxiom.Operations.CalculatePerPartDescription
        Dim selections As List(Of Paxiom.Selection) = New List(Of Paxiom.Selection)
        Dim variableCode As String = CalculateOneVariableListBox.Attributes("VariableCode")
        Dim variableValue As String = CalculateOneVariableListBox.SelectedValue

        If ((Not String.IsNullOrEmpty(variableCode)) AndAlso (Not String.IsNullOrEmpty(variableValue))) Then
            Dim selection = New Paxiom.Selection(variableCode)
            selection.ValueCodes.Add(variableValue)
            selections.Add(selection)

            'CalculatePerPartDescription 
            calcPerPartDescription.CalculationVariant = Paxiom.CalculatePerPartSelectionType.OneVariableOneValue
            calcPerPartDescription.ValueSelection = selections.ToArray()

        End If

        'Return CalculatePerPartDescription
        Return calcPerPartDescription

    End Function


    ''' <summary>
    ''' Creates CalculatePerPartDescription for  PerPartCalculation of type OneMatrixValue (One value from each variable = Option 3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CalculateOnAllVariables() As PCAxis.Paxiom.Operations.CalculatePerPartDescription
        Dim variableCode As String
        Dim variableValue As String
        Dim valuesListBox As ListBox
        Dim selections As List(Of Paxiom.Selection) = New List(Of Paxiom.Selection)
        Dim calcPerPartDescription As New PCAxis.Paxiom.Operations.CalculatePerPartDescription

        'Get selection
        For Each itm As RepeaterItem In CalculateAllVariablesRepeater.Items
            'VariableCode
            valuesListBox = DirectCast(itm.FindControl("CalculateAllVariablesValuesListBox"), ListBox)
            variableCode = valuesListBox.Attributes.Item("VariableCode")
            If Not String.IsNullOrEmpty(variableCode) Then
                'Selected VariableValue
                Dim selection = New Paxiom.Selection(variableCode)
                variableValue = valuesListBox.SelectedValue
                If Not String.IsNullOrEmpty(variableValue) Then
                    selection.ValueCodes.Add(variableValue)
                    selections.Add(selection)
                End If
            End If
        Next

        'CalculatePerPartDescription 
        calcPerPartDescription.CalculationVariant = Paxiom.CalculatePerPartSelectionType.OneMatrixValue
        calcPerPartDescription.ValueSelection = selections.ToArray()

        'Return CalculatePerPartDescription
        Return calcPerPartDescription

    End Function

#End Region



    Private Sub CalculateOneVariableListBox_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalculateOneVariableListBox.DataBound
        If CalculateOneVariableListBox.Items.Count > 0 Then
            CalculateOneVariableListBox.Items(0).Selected = True
        End If

    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton1.Click, CancelButton2.Click, CancelButton3.Click, CancelButton4.Click, CancelButton5.Click
        NewVariableNameRequired.Enabled = False
        VariableSelectionRequired.Enabled = False

        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub

    Private Sub UpdateOperationsTracker(calcPerPartDescription As CalculatePerPartDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.PER_PART, calcPerPartDescription)
        For Each selection As Selection In calcPerPartDescription.ValueSelection
            Dim va As Variable = Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(Function(v As Variable) v.Code = selection.VariableCode)
            If va IsNot Nothing AndAlso va.IsTime Then
                PaxiomManager.OperationsTracker.IsTimeDependent = True
                Exit For
            End If
        Next
    End Sub


End Class

