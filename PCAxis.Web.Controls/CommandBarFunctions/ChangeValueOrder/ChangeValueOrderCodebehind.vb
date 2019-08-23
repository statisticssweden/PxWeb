Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Core.Management
Imports PCAxis.Paxiom

''' <summary>
''' This control makes it possible to rearrange how the variablevalues is displayed.
''' </summary>
Public Class ChangeValueOrderCodebehind
    Inherits CommandBarPluginBase(Of ChangeValueOrderCodebehind, ChangeValueOrder)

#Region "Constants"
    Private Const PROPERTY_LEFTBUTTONIMAGEPATH As String = "LeftButtonImagePath"
    Private Const PROPERTY_RIGHTBUTTONIMAGEPATH As String = "RightButtonImagePath"
    Private Const PROPERTY_DOWNBUTTONIMAGEPATH As String = "DownButtonImagePath"
    Private Const PROPERTY_LISTSIZE As String = "ListSize"
#End Region

#Region "Localized strings"
    Private Const CAPTION As String = "CtrlCommandBarFunctionChangeValueOrder"
    Private Const INSTRUCTION_CHOOSE_VARIABLE As String = "CtrlChangeValueOrderInstructionChooseVariable"
    Private Const INSTRUCTION_CHANGE_ORDER As String = "CtrlChangeValueOrderInstructionChangeOrder"
    Private Const CANCEL_BUTTON As String = "CtrlChangeValueOrderCancelButton"
    Private Const CONTINUE_BUTTON As String = "CtrlChangeValueOrderContinueButton"
    Private Const MOVE_RIGHT As String = "CtrlChangeValueOrderMoveRight"
    Private Const MOVE_LEFT As String = "CtrlChangeValueOrderMoveLeft"
    Private Const MOVE_DOWN As String = "CtrlChangeValueOrderMoveDown"
    Private Const FROM_ORDER As String = "CtrlChangeValueOrderFromOrder"
    Private Const TO_ORDER As String = "CtrlChangeValueOrderToOrder"
#End Region

#Region "Controls"
    Protected CaptionLabel As Label
    Protected Instructions As Literal
    Protected WithEvents CancelButton As Button
    Protected WithEvents ContinueButton As Button
    Protected VariableList As RadioButtonList
    Protected ChooseVariablePanel As Panel
    Protected ChangeOrderPanel As Panel
    Protected WithEvents MoveLeftButton As ImageButton
    Protected WithEvents MoveRightButton As ImageButton
    Protected WithEvents FromOrderDownButton As ImageButton
    Protected WithEvents ToOrderDownButton As ImageButton
    Protected ValuesFromOrder As ListBox
    Protected ValuesToOrder As ListBox
    Protected FromOrderLabel As Label
    Protected ToOrderLabel As Label
#End Region

#Region "Fields"
    Private _selectedVariableCode As String
#End Region


    Private Sub ChangeValueOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetLocalizedTexts()

        VariableList.DataSource = PaxiomModel.Meta.Variables
        VariableList.DataValueField = "Code"
        VariableList.DataTextField = "Name"
        VariableList.CssClass = "commandbar_radioButtonList"
        VariableList.DataBind()

        If VariableList.Items.Count > 0 Then
            VariableList.Items(0).Selected = True
        Else
            ContinueButton.Enabled = False
        End If

        Me.ValuesFromOrder.SelectionMode = ListSelectionMode.Multiple
        Me.ValuesToOrder.SelectionMode = ListSelectionMode.Multiple
    End Sub

    ''' <summary>
    ''' Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any 
    ''' child controls they contain in preparation for posting back or rendering.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        Me.LeftButtonImagePath = Me.Properties(PROPERTY_LEFTBUTTONIMAGEPATH)
        Me.RightButtonImagePath = Me.Properties(PROPERTY_RIGHTBUTTONIMAGEPATH)
        Me.DownButtonImagePath = Me.Properties(PROPERTY_DOWNBUTTONIMAGEPATH)
        Me.ListSize = CInt(Me.Properties(PROPERTY_LISTSIZE))
    End Sub

    ''' <summary>
    ''' Set localized texts
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetLocalizedTexts()
        Me.CaptionLabel.Text = Me.GetLocalizedString(CAPTION)
        If String.IsNullOrEmpty(Instructions.Text) Then
            Instructions.Text = Me.GetLocalizedString(INSTRUCTION_CHOOSE_VARIABLE)
        End If
        Me.CancelButton.Text = Me.GetLocalizedString(CANCEL_BUTTON)
        Me.ContinueButton.Text = Me.GetLocalizedString(CONTINUE_BUTTON)
        Me.MoveLeftButton.ToolTip = Me.GetLocalizedString(MOVE_LEFT)
        Me.MoveLeftButton.AlternateText = Me.GetLocalizedString(MOVE_LEFT)
        Me.MoveRightButton.ToolTip = Me.GetLocalizedString(MOVE_RIGHT)
        Me.MoveRightButton.AlternateText = Me.GetLocalizedString(MOVE_RIGHT)
        Me.FromOrderDownButton.ToolTip = Me.GetLocalizedString(MOVE_DOWN)
        Me.FromOrderDownButton.AlternateText = Me.GetLocalizedString(MOVE_DOWN)
        Me.ToOrderDownButton.ToolTip = Me.GetLocalizedString(MOVE_DOWN)
        Me.ToOrderDownButton.AlternateText = Me.GetLocalizedString(MOVE_DOWN)
        Me.FromOrderLabel.Text = Me.GetLocalizedString(FROM_ORDER)
        Me.ToOrderLabel.Text = Me.GetLocalizedString(TO_ORDER)
    End Sub

    ''' <summary>
    ''' Performs initialization when variable has been selected
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeVariable()
        Dim var As Paxiom.Variable = PaxiomModel.Meta.Variables.GetByCode(VariableList.SelectedValue)
        ValuesFromOrder.DataSource = var.Values
        ValuesFromOrder.DataValueField = "Code"
        ValuesFromOrder.DataTextField = "Value"
        ValuesFromOrder.DataBind()

        SelectedVariableCode = var.Code
        Instructions.Text = GetLocalizedString(INSTRUCTION_CHANGE_ORDER)
    End Sub

#Region "Properties"
    ''' <summary>
    ''' Code of the selected variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Protected Property SelectedVariableCode() As String
        Get
            Return _selectedVariableCode
        End Get
        Set(ByVal value As String)
            _selectedVariableCode = value
        End Set
    End Property

    ''' <summary>
    ''' Set the image URL of the left button.
    ''' </summary>
    Private Property LeftButtonImagePath() As String
        Get
            Return MoveLeftButton.ImageUrl
        End Get
        Set(ByVal value As String)
            MoveLeftButton.ImageUrl = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, value)
        End Set
    End Property
    ''' <summary>
    ''' Set the image URL of the right button.
    ''' </summary>
    Private Property RightButtonImagePath() As String
        Get
            Return MoveRightButton.ImageUrl
        End Get
        Set(ByVal value As String)
            MoveRightButton.ImageUrl = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, value)
        End Set
    End Property
    ''' <summary>
    ''' Set the image URL of the down buttons.
    ''' </summary>
    Private Property DownButtonImagePath() As String
        Get
            Return FromOrderDownButton.ImageUrl
        End Get
        Set(ByVal value As String)
            FromOrderDownButton.ImageUrl = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, value)
            ToOrderDownButton.ImageUrl = System.IO.Path.Combine(PCAxis.Web.Controls.Configuration.Paths.ImagesPath, value)
        End Set
    End Property
    ''' <summary>
    ''' Set the number of rows in the listboxes
    ''' </summary>
    Private Property ListSize() As Integer
        Get
            Return ValuesFromOrder.Rows
        End Get
        Set(ByVal value As Integer)
            ValuesFromOrder.Rows = value
            ValuesToOrder.Rows = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Moves all the selected values in the source listbox to the destination listbox.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="destination"></param>
    ''' <remarks>Selected values are removed from the source listbox</remarks>
    Private Sub MoveValues(ByVal source As ListBox, ByVal destination As ListBox)
        Dim lst As New List(Of ListItem)

        'Iterate from the end to the beginning
        For i As Integer = source.Items.Count - 1 To 0 Step -1
            If source.Items(i).Selected Then
                Dim li As ListItem
                li = New ListItem(source.Items(i).Text, source.Items(i).Value)
                lst.Insert(0, li)
                source.Items.RemoveAt(i)
            End If
        Next

        For Each li As ListItem In lst
            destination.Items.Add(li)
        Next
    End Sub

    ''' <summary>
    ''' Moves the selected items in the listbox to the end of the listbox
    ''' </summary>
    ''' <param name="source"></param>
    ''' <remarks></remarks>
    Private Sub MoveValuesToEnd(ByVal source As ListBox)
        MoveValues(source, source)
    End Sub

    ''' <summary>
    ''' Changes the value order
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChangeValueOrder()
        Dim var As Paxiom.Variable = PaxiomModel.Meta.Variables.GetByCode(SelectedVariableCode)
        Dim modifiedVariableValueWeight(var.Values.Count - 1) As Integer
        Dim newIndex As Integer

        'Find new indexes for the values
        For i As Integer = 0 To var.Values.Count - 1
            newIndex = FindNewValueIndex(var.Values(i).Code)
            If newIndex <> -1 Then
                modifiedVariableValueWeight(i) = newIndex
            End If
        Next

        Dim orderDescription As ChangeValueOrderDescription = New ChangeValueOrderDescription(Me.SelectedVariableCode, modifiedVariableValueWeight)
        Dim changeOrder As PCAxis.Paxiom.Operations.ChangeValueOrder = New PCAxis.Paxiom.Operations.ChangeValueOrder()
        Web.Core.Management.PaxiomManager.PaxiomModel = changeOrder.Execute(PaxiomModel, orderDescription)
        UpdateOperationsTracker(orderDescription)

    End Sub

    ''' <summary>
    ''' Find the new index of the value
    ''' </summary>
    ''' <param name="code">Value code</param>
    ''' <returns>New index of the value. Returns -1 if value code is not found</returns>
    ''' <remarks></remarks>
    Private Function FindNewValueIndex(ByVal code As String) As Integer
        Dim i As Integer = 0

        For Each li As ListItem In Me.ValuesFromOrder.Items
            If li.Value.Equals(code) Then
                Return i
            End If
            i = i + 1
        Next

        For Each li As ListItem In Me.ValuesToOrder.Items
            If li.Value.Equals(code) Then
                Return i
            End If
            i = i + 1
        Next

        Return -1 'Cannot find code
    End Function

    ''' <summary>
    ''' Called when variable is selected and when the "Change value order" operation shall be applied
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        If ChooseVariablePanel.Visible Then
            ChooseVariablePanel.Visible = False
            ChangeOrderPanel.Visible = True
            InitializeVariable()
        Else
            'Apply new value order
            ChangeValueOrder()
            Me.OnFinished(New CommandBarPluginFinishedEventArgs(PaxiomModel))
        End If
    End Sub

    ''' <summary>
    ''' Cancel the "Change value order" operation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(PaxiomModel))
    End Sub

    ''' <summary>
    ''' Move selected values from the "From order" listbox to the "To order" listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MoveRightButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles MoveRightButton.Click
        MoveValues(Me.ValuesFromOrder, Me.ValuesToOrder)
    End Sub

    ''' <summary>
    ''' Move selected values from the "To order" listbox to the "From order" listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MoveLeftButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles MoveLeftButton.Click
        MoveValues(Me.ValuesToOrder, Me.ValuesFromOrder)
    End Sub

    ''' <summary>
    ''' Move the selected values to the end of the "From order" listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FromOrderDownButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles FromOrderDownButton.Click
        MoveValuesToEnd(Me.ValuesFromOrder)
    End Sub

    ''' <summary>
    ''' Move the selected values to the end of the "To order" listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToOrderDownButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ToOrderDownButton.Click
        MoveValuesToEnd(Me.ValuesToOrder)
    End Sub

    Private Sub UpdateOperationsTracker(orderDescription As ChangeValueOrderDescription)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.CHANGE_VALUE_ORDER, orderDescription)
        Dim va As Variable = Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Variables.FirstOrDefault(Function(v As Variable) v.Code = orderDescription.VariableCode)
        If va IsNot Nothing AndAlso va.IsTime Then
            PaxiomManager.OperationsTracker.IsTimeDependent = True
        End If
    End Sub

End Class