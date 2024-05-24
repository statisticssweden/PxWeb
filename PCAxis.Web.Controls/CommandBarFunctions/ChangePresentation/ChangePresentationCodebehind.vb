
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
Public Class ChangePresentationCodebehind
    Inherits CommandBarPluginBase(Of ChangePresentationCodebehind, ChangePresentation)

#Region " Localized strings "


    'Change code/text presentation texts
    Private Const CHANGEPRESENTATION_CHANGECODETEXT_TITLE As String = "CtrlCommandBarFunctionChangeCodeTextHeading"
    Private Const CHANGEPRESENTATION_CHANGECODETEXT_TEXT As String = "CtrlChangePresentationChangeCodeTextsText"
    Private Const CHANGEPRESENTATION_CHANGECODETEXT_TEXTPRESENTATION As String = "CtrlChangeCodeTextPresentationText"
    Private Const CHANGEPRESENTATION_CHANGECODETEXT_CODEPRESENTATION As String = "CtrlChangeCodeTextPresentationCode"
    Private Const CHANGEPRESENTATION_CHANGECODETEXT_CODETEXTPRESENTATION As String = "CtrlChangeCodeTextPresentationCodeAndText"
    Private Const CHANGEPRESENTATION_COMPLETE_BUTTON As String = "CtrlChangePresentationCompleteButton"
    Private Const CHANGEPRESENTATION_CONTENTS_TEXT As String = "CtrlChangePresentationContentsHeading"
    Private Const CANCEL_BUTTON As String = "CancelButton"

#End Region

#Region " Controls "
    Protected WithEvents ChangeTextPanel As Panel
    Protected TitleLabel As Label

    'ChangeTexts
    Protected ChangeTextTextLabel As Label
    Protected ContentsTextLabel As Label
    Protected ContentsTextTextBox As TextBox
    Protected UnitsTextLabel As Label
    Protected UnitsTextTextBox As TextBox
    Protected VariablesTextLabel As Label
    Protected WithEvents ChangeTextRepeater As Repeater
    Protected WithEvents ChangeText_ContinueButton As Button
    Protected WithEvents CancelButton As Button

    'Change code/text 
    Protected CodeTextRadio As RadioButtonList
    Protected ChangeCodeTextPanel As Panel
    Protected ChangeCodeTextTextLabel As Label
    Protected WithEvents ChangeCodeTextRepeater As Repeater
    Protected WithEvents ChangeCodeText_CompleteButton As Button
    Protected ChangeCodeTextTable As Table

    'Common
    Protected InfoMessageLabel As Label
    Protected ErrorMessageLabel As Label
    Protected ErrorMessagePanel As Panel

#End Region

#Region " Properties "

    'Private _settingsDictionary As Dictionary(Of String, HeaderPresentationType)
    Private _codeText As String = "Code"
    Private _textText As String = "Text"
    Private _codeAndTextText As String = "Code and Text"
    Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(ChangePresentation))

#End Region

    ''' <summary>
    ''' Control_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangePresentation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadTextsForLanguage()
        ChangeCodeTextPanel.Visible = True
        'Dim table As Table = FindTableControl(Me.Page.Controls)
        '_settingsDictionary = Table.VariablePresentationAlternative
        '_settingsDictionary = New Dictionary(Of String, HeaderPresentationType)

        Dim cantChangeTheseTwo As IEnumerable(Of Variable) = From var In Me.PaxiomModel.Meta.Variables
                                                             Where (var.IsContentVariable Or
                                                         var.IsTime)

        ChangeCodeTextRepeater.DataSource = Me.PaxiomModel.Meta.Variables.Except(cantChangeTheseTwo)


        ChangeCodeTextRepeater.DataBind()
        If ChangeCodeTextRepeater.Items.Count < 1 Then
            InfoMessageLabel.Text = GetLocalizedString("CtrlChangeCodeTextPresentationNoneChangeable")
            ErrorMessagePanel.Visible = True
        End If

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
    Private Sub ChangePresentation_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadTextsForLanguage()
    End Sub

    ''' <summary>
    '''  Fill texts for controles with localized texts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTextsForLanguage()

        Dim buttonText As String = GetLocalizedString(CHANGEPRESENTATION_COMPLETE_BUTTON)

        'Change code/text texts
        _codeText = GetLocalizedString(CHANGEPRESENTATION_CHANGECODETEXT_CODEPRESENTATION)
        _codeAndTextText = GetLocalizedString(CHANGEPRESENTATION_CHANGECODETEXT_CODETEXTPRESENTATION)
        _textText = GetLocalizedString(CHANGEPRESENTATION_CHANGECODETEXT_TEXTPRESENTATION)
        ChangeCodeTextTextLabel.Text = GetLocalizedString(CHANGEPRESENTATION_CHANGECODETEXT_TEXT)
        ChangeCodeText_CompleteButton.Text = buttonText
        'TitleLabel.Text = GetLocalizedString(CHANGEPRESENTATION_CHANGECODETEXT_TITLE)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
    End Sub


#Region "ChangeCodeText"

    ''' <summary>
    ''' Creates three radiobuttons for selection of current variables code/text presentation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Default selection is Text</remarks>
    Protected Sub ChangeCodeTextRepeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles ChangeCodeTextRepeater.ItemDataBound
        Dim item As RepeaterItem = e.Item
        If (item.ItemType = ListItemType.Item) Or (item.ItemType = ListItemType.AlternatingItem) Then
            Dim v As Variable = DirectCast(e.Item.DataItem, Variable)

            Dim ChangeCodeTextOneItemPanel As Panel = DirectCast(item.FindControl("ChangeCodeTextOneItemPanel"), Panel)

            Dim CodeTextRadio As RadioButtonList = DirectCast(item.FindControl("CodeTextRadio"), RadioButtonList)

            Dim variableCode As String = v.Code
            Dim selectedPresentation As HeaderPresentationType = DirectCast(v.PresentationText, HeaderPresentationType)

            CodeTextRadio.Attributes.Add("VariableCode", variableCode)

            'wrap this in <hX> ?
            ChangeCodeTextOneItemPanel.GroupingText = "<span class='commandbar_changepresentation_headertext'>" + Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(v.Name) + "</span>"

            CodeTextRadio.Items.Add(NewRadioButton(_textText, HeaderPresentationType.Text, selectedPresentation.Equals(HeaderPresentationType.Text)))
            CodeTextRadio.Items.Add(NewRadioButton(_codeText, HeaderPresentationType.Code, selectedPresentation.Equals(HeaderPresentationType.Code)))
            CodeTextRadio.Items.Add(NewRadioButton(_codeAndTextText, HeaderPresentationType.CodeAndText, selectedPresentation.Equals(HeaderPresentationType.CodeAndText)))

        End If
    End Sub

    ''' <summary>
    ''' Creates RadioButton control for code/text presentation
    ''' </summary>
    ''' <param name="radioText">Text for RadioButton</param>
    ''' <param name="selectedPresentation">Selected presentation is used as value for RadioButton</param>
    ''' <param name="selected">Is RadioButton selected/not selected</param>
    ''' <returns>New RadioButton</returns>
    Private Function NewRadioButton(ByVal radioText As String, ByVal selectedPresentation As HeaderPresentationType, ByVal selected As Boolean) As ListItem
        Dim btn As ListItem
        btn = New ListItem(radioText, selectedPresentation.ToString())
        btn.Selected = selected
        btn.Attributes.Add("class", "commandbar_changepresentation_radio_item")
        Return btn
    End Function


    ''' <summary>
    ''' Handles event continuebutton clicked for change code/text presentation
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChangeCodeText_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChangeCodeText_CompleteButton.Click
        Dim newModel As PXModel
        'Find tablecontrol
        'Dim table As Table = FindTableControl(Me.Page.Controls)
        Dim presentationType As HeaderPresentationType
        'If table IsNot Nothing Then
        Dim settingsDictionary As Dictionary(Of String, HeaderPresentationType) = New Dictionary(Of String, HeaderPresentationType)
        'Add presentationsettings to dictionary fore each variable listed in repeater
        For Each itm As RepeaterItem In ChangeCodeTextRepeater.Items
            Dim CodeTextRadio As RadioButtonList = DirectCast(itm.FindControl("CodeTextRadio"), RadioButtonList)
            Dim variableCode As String = CodeTextRadio.Attributes.Item("VariableCode")

            If variableCode.ToLower() <> "tid" And variableCode.ToLower() <> "contentscode" Then
                presentationType = CType([Enum].Parse(GetType(HeaderPresentationType), CodeTextRadio.SelectedValue), HeaderPresentationType)
                If Not String.IsNullOrEmpty(variableCode) Then
                    settingsDictionary.Add(variableCode, presentationType)
                End If
            End If
        Next
        'Add dictionary with presentationsettings to table
        'table.VariablePresentationAlternative = settingsDictionary
        newModel = ChangePresentation(settingsDictionary)
        'Me.OnFinished(New CommandBarPluginFinishedEventArgs(newModel))

        'End If

        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Me.PaxiomModel))

        LogFeatureUsage(OperationConstants.CHANGE_TEXT_CODE_PRESENTATION, Me.PaxiomModel.Meta)

    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub

    Private Function ChangePresentation(ByVal presDict As Dictionary(Of String, HeaderPresentationType)) As Paxiom.PXModel

        Dim changePres As New PCAxis.Paxiom.Operations.ChangeTextCodePresentation()
        Dim changePresDescription As New PCAxis.Paxiom.Operations.ChangeTextCodePresentationDescription(presDict)

        PaxiomManager.PaxiomModel = changePres.Execute(PaxiomModel, changePresDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.CHANGE_TEXT_CODE_PRESENTATION, changePresDescription)

        Return PaxiomManager.PaxiomModel

    End Function

#End Region
























    '''''' <summary>
    '''''' Handles event continuebutton clicked
    '''''' </summary>
    '''''' <remarks></remarks>
    ' ''Private Sub ChangeText_ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChangeText_ContinueButton.Click

    ' ''    Dim newModel As PXModel
    ' ''    Dim newMeta As PXMeta
    ' ''    Dim newData As PXData
    ' ''    Dim value As Value
    ' ''    Dim variable As Variable

    ' ''    newMeta = Me.PaxiomModel.Meta.CreateCopy
    ' ''    newData = Me.PaxiomModel.Data.CreateCopy

    ' ''    'Create new model to return
    ' ''    newModel = New PXModel(newMeta, newData)



    ' ''    For Each itm As RepeaterItem In ChangeTextRepeater.Items
    ' ''        'Find the selected variable
    ' ''        Dim tbx As TextBox = DirectCast(itm.FindControl("ValueNameTextbox"), TextBox)

    ' ''        Dim variableCode As String = tbx.Text
    ' ''        Dim var As Variable = Me.PaxiomModel.Meta.Variables.GetByCode(variableCode)













    ' ''        variable = newModel.Meta.Variables.GetByCode(variableCode)

    ' ''        If variable Is Nothing Then
    ' ''            Throw New PXOperationException("Cannot find variable")
    ' ''        End If
    ' ''        'value = variable.Values.GetByCode(rhs.valueCode)
    ' ''        'Dim langs() As String = newModel.Meta.GetAllLanguages()

    ' ''        'If langs Is Nothing Then
    ' ''        '    If langs Is Nothing Then
    ' ''        '        'Only default language exists - Add it to langs
    ' ''        '        ReDim langs(0)
    ' ''        '        langs(0) = newModel.Meta.Language
    ' ''        '    End If
    ' ''        'End If



    ' ''        ''Change contents for all languages
    ' ''        'For i As Integer = 0 To langs.Length - 1
    ' ''        '    newModel.Meta.SetLanguage(i)
    ' ''        newModel.Meta.Contents = newModel.Meta.Contents & ", " & tbx.Text 'value.Value
    ' ''        'Next
    ' ''    Next



    ' ''    Dim selectedLanguageIndex As Integer

    ' ''    'Get selected language index so we can restore it 
    ' ''    selectedLanguageIndex = newModel.Meta.CurrentLanguageIndex

    ' ''    'Restore selected language
    ' ''    newModel.Meta.SetLanguage(selectedLanguageIndex)
    ' ''    Me.OnFinished(New CommandBarPluginFinishedEventArgs(newModel))
    ' ''End Sub






#Region "old"

    '''' <summary>
    '''' Renders one radio button for each variable in table
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub LoadRadioButtonList()

    '    If (VariableToSumRadioButtonList.Items.Count < 1) Then

    '        For Each variable As Variable In Me.PaxiomModel.Meta.Variables
    '            VariableToSumRadioButtonList.Items.Add(variable.Name)
    '        Next
    '        If (VariableToSumRadioButtonList.Items.Count > 0) Then
    '            VariableToSumRadioButtonList.SelectedIndex = 0
    '        End If
    '    End If
    'End Sub


    '''' <summary>
    ''''  Sum all variables
    '''' </summary>    
    '''' <remarks></remarks>
    'Private Sub SumAll()
    '    If Me.PaxiomModel.Meta.Stub.Count > 0 Then
    '        Dim _valuestosum As New List(Of String)
    '        Dim _variable As Variable = Me.PaxiomModel.Meta.Stub(0)
    '        For Each _value As Value In _variable.Values
    '            _valuestosum.Add(_value.Code)
    '        Next

    '        Dim SumDesc As New PCAxis.Paxiom.Operations.SumDescription
    '        SumDesc.VariableCode = _variable.Code
    '        SumDesc.ValueCodes = _valuestosum
    '        SumDesc.NewValueName = TotalVariableName.Text
    '        SumDesc.NewValueCode = Guid.NewGuid().ToString()
    '        SumDesc.KeepValues = True
    '        SumDesc.Operation = _sumOperation

    '        Dim p As New PCAxis.Paxiom.Operations.Sum
    '        Me.OnFinished(New CommandBarPluginFinishedEventArgs(p.Execute(Me.PaxiomModel, SumDesc)))
    '    End If


    'End Sub

    '''' <summary>
    '''' sum selected variabels
    '''' </summary>    
    '''' <remarks></remarks>
    'Private Sub SumSelected()

    '    Dim _selectedvariabletosum As String = VariableToSumRadioButtonList.SelectedValue
    '    Dim _valuestosum As New List(Of String)

    '    For Each _varible As Variable In Me.PaxiomModel.Meta.Variables

    '        If (_varible.Code = _selectedvariabletosum) Then
    '            'If exactly two values in right order are required as input for operation, get them from textboxes
    '            If _twoValueOperation Then
    '                Dim firstValue As String = ""
    '                Dim secondValue As String = ""
    '                Dim foundValues As Integer = 0
    '                For Each _value As Value In _varible.Values
    '                    If FirstOperandTextBox.Text = _value.Value Then
    '                        firstValue = _value.Code
    '                        foundValues += 1
    '                    End If
    '                    If SecondOperandTextBox.Text = _value.Value Then
    '                        secondValue = _value.Code
    '                        foundValues += 1
    '                    End If
    '                    If foundValues = 2 Then
    '                        _valuestosum.Add(firstValue)
    '                        _valuestosum.Add(secondValue)
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                'Get all selected values as input to operation
    '                For Each _value As Value In _varible.Values
    '                    Dim i As ListItem = ValuesToSumListBox.Items.FindByValue(_value.Value)
    '                    If i.Selected Then
    '                        _valuestosum.Add(_value.Code)
    '                    End If
    '                Next
    '            End If

    '            If (Not _twoValueOperation AndAlso _valuestosum.Count > 0) OrElse (_twoValueOperation AndAlso _valuestosum.Count = 2) Then
    '                Dim SumDesc As New PCAxis.Paxiom.Operations.SumDescription
    '                SumDesc.VariableCode = _varible.Code
    '                SumDesc.ValueCodes = _valuestosum
    '                SumDesc.NewValueName = TotalVariableName.Text
    '                SumDesc.NewValueCode = Guid.NewGuid().ToString()
    '                SumDesc.KeepValues = True
    '                SumDesc.Operation = _sumOperation

    '                Dim s As New PCAxis.Paxiom.Operations.Sum
    '                Me.OnFinished(New CommandBarPluginFinishedEventArgs(s.Execute(Me.PaxiomModel, SumDesc)))
    '            End If
    '        End If

    '    Next

    'End Sub

    '''' <summary>
    '''' Populate ValuesDropDown depending on selected variable to sum
    '''' </summary>    
    '''' <remarks></remarks>
    'Private Sub PopulateValuesToSumDropDown(ByVal _variable As Variable)

    '    ValuesToSumListBox.Items.Clear()
    '    ValuesToSumListBox.Visible = True
    '    ValuesToSumListBox.SelectionMode = ListSelectionMode.Multiple
    '    If _twoValueOperation Then
    '        ValuesToSumListBox.Attributes.Add("onchange", "SelectOperands(this.value,'" + FirstOperandTextBox.ClientID + "', '" + SecondOperandTextBox.ClientID + "')")
    '    End If

    '    For Each _value As Value In _variable.Values
    '        ValuesToSumListBox.Items.Add(_value.ToString)
    '    Next

    'End Sub



    '''' <summary>
    ''''  Handles event 
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ContinueButtonSelectVariables_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButtonSelectVariables.Click

    '    Dim _selectedvariabletosum As String = VariableToSumRadioButtonList.SelectedValue

    '    For Each _variable As Variable In Me.PaxiomModel.Meta.Variables
    '        If (_variable.Code = _selectedvariabletosum) Then
    '            PopulateValuesToSumDropDown(_variable)
    '        End If
    '    Next

    '    SelectValuesOptionsPanel.Visible = True
    '    SelectVariableOptionsPanel.Visible = False
    '    If _twoValueOperation Then
    '        SelectOperandsPanel.Visible = True
    '    End If

    'End Sub

    '''' <summary>
    ''''  Handles event 
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ContinueButtonSelectValues_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButtonSelectValues.Click
    '    SumSelected()
    'End Sub


#End Region
End Class

