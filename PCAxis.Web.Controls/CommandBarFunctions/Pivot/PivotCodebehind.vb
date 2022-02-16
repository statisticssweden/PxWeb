

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
''' After rearranging is done and the user has clicked the continue button you
''' need to retrive the PXModel from this control to display the changes.
''' </remarks>
Public Class PivotCodebehind
    Inherits CommandBarPluginBase(Of PivotCodebehind, Pivot)
    Private Const COMPLETE_BUTTON As String = "CtrlPivotCompleteButton"
    Private Const STUB_LABEL As String = "CtrlPivotStubLabel"
    Private Const HEADING_LABEL As String = "CtrlPivotHeadingLabel"
    Private Const CANCEL_BUTTON As String = "CtrlPivotCancelButton"
    Private Const MOVE_RIGHT_LABEL As String = "CtrlCommandBarFunctionPivotManualMoveRight"
    Private Const MOVE_LEFT_LABEL As String = "CtrlCommandBarFunctionPivotManualMoveLeft"
    Private Const MOVE_UP_LABEL As String = "CtrlCommandBarFunctionPivotManualMoveUp"
    Private Const MOVE_DOWN_LABEL As String = "CtrlCommandBarFunctionPivotManualMoveDown"


#Region " Controls "

    Protected HeadingListBox As ListBox
    Protected StubListBox As ListBox
    Protected WithEvents MoveLeftButton As Button
    Protected WithEvents MoveRightButton As Button
    Protected WithEvents ContinueButton As Button
    Protected StubLabel As Label
    Protected HeadingLabel As Label
    Protected WithEvents HeadingUpButton As Button
    Protected WithEvents HeadingDownButton As Button
    Protected WithEvents StubUpButton As Button
    Protected WithEvents StubDownButton As Button
    Protected WithEvents CancelButton As Button

#End Region

#Region " Properties "

    ''' <summary>
    ''' Datasource for the Left listbox.
    ''' </summary>
    ''' 
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



#End Region


    Private Sub Pivot_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadLanguages()
    End Sub

    Private Sub Pivot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If ((HeadingListData Is Nothing Or StubListData Is Nothing) Or (Not Me.Page.IsPostBack)) Then
            CreateDataSources()
            FillLists()
            LoadLanguages()
        End If

        Dim scriptSwitch As String = "return PivotManual_Switch('{0}', '{1}');"
        Dim scriptReorder As String = "return PivotManual_Reorder('{0}', '{1}');"

        MoveRightButton.OnClientClick = String.Format(scriptSwitch, StubListBox.ClientID, HeadingListBox.ClientID)
        MoveLeftButton.OnClientClick = String.Format(scriptSwitch, HeadingListBox.ClientID, StubListBox.ClientID)
        ContinueButton.OnClientClick = String.Format("PivotManual_Submit('{0}','{1}');", HeadingListBox.ClientID, StubListBox.ClientID)

        StubDownButton.OnClientClick = String.Format(scriptReorder, StubListBox.ClientID, "bottom")
        StubUpButton.OnClientClick = String.Format(scriptReorder, StubListBox.ClientID, "top")

        HeadingDownButton.OnClientClick = String.Format(scriptReorder, HeadingListBox.ClientID, "bottom")
        HeadingUpButton.OnClientClick = String.Format(scriptReorder, HeadingListBox.ClientID, "top")

        'MoveRightButton



    End Sub

    ''' <summary>
    ''' Read info from the PXModel to fill the lists in this control.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateDataSources()
        HeadingListData = New List(Of String)
        StubListData = New List(Of String)
        For Each val As PCAxis.Paxiom.Variable In Me.PaxiomModel.Meta.Heading
            HeadingListData.Add(val.Name)
        Next

        For Each val As PCAxis.Paxiom.Variable In Me.PaxiomModel.Meta.Stub
            StubListData.Add(val.Name)
        Next
    End Sub

    ''' <summary>
    ''' Bind the datasource to the controls.
    ''' </summary>
    Private Sub FillLists()
        HeadingListBox.DataSource = HeadingListData
        StubListBox.DataSource = StubListData
        HeadingListBox.DataBind()
        StubListBox.DataBind()
        HeadingListBox.Rows = CInt(Me.Properties("ListSize"))
        StubListBox.Rows = CInt(Me.Properties("ListSize"))
    End Sub


    Private Sub MoveLeftButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MoveLeftButton.Click
        If (HeadingListBox.SelectedIndex <> -1) Then
            'Dim value As String = HeadingListBox.SelectedValue
            For Each index As Integer In HeadingListBox.GetSelectedIndices()
                Dim value As String = HeadingListBox.Items(index).Value
                HeadingListData.Remove(value)
                StubListData.Add(value)
            Next

            FillLists()
        End If
    End Sub

    Private Sub MoveRightButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MoveRightButton.Click
        If (StubListBox.SelectedIndex <> -1) Then
            For Each index As Integer In StubListBox.GetSelectedIndices()
                Dim value As String = StubListBox.Items(index).Value
                StubListData.Remove(value)
                HeadingListData.Add(value)
            Next
            FillLists()
        End If
    End Sub

    Private Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        'Fri(pivotering)
        Dim pd As New List(Of PCAxis.Paxiom.Operations.PivotDescription)()

        'Här får du dimensionera upp pd variabeln så att den blir lika stor som antalet variabler i px-modellen
        'där efter får du skapa pxDescription's objekt som säger var variablerna ska vara placerade efter pivotering
        'Exempel med en px-modell med två variabler, kön och län. Efter pivoteringen vill man att kön ska hamna i heading och län i stub     

        Dim hiddenStub As String = Me.Page.Request.Form("PivotManualStubList")
        Dim hiddenHeading As String = Me.Page.Request.Form("PivotManualHeadingList")

        If Not (String.IsNullOrEmpty(hiddenStub) AndAlso String.IsNullOrEmpty(hiddenHeading)) Then
            For Each s As String In hiddenStub.Split(CChar("@"))
                If Not String.IsNullOrEmpty(s) Then
                    pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(s, PCAxis.Paxiom.PlacementType.Stub))
                End If
            Next
            For Each s As String In hiddenHeading.Split(CChar("@"))
                If Not String.IsNullOrEmpty(s) Then
                    pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(s, PCAxis.Paxiom.PlacementType.Heading))
                End If
            Next
        Else

            For Each item As ListItem In HeadingListBox.Items
                pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(item.Value, PCAxis.Paxiom.PlacementType.Heading))
            Next

            For Each item As ListItem In StubListBox.Items
                pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(item.Value, PCAxis.Paxiom.PlacementType.Stub))
            Next
        End If

        'Därefter utför vi pivoteringen
        Dim p As New PCAxis.Paxiom.Operations.Pivot
        ' Set the new model
        'Me.PaxiomModel = p.Execute(Me.PaxiomModel, pd.ToArray())
        ' raise event
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(p.Execute(Me.PaxiomModel, pd.ToArray())))
        LogFeatureUsage(OperationConstants.PIVOT, Me.PaxiomModel.Meta)
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT, pd.ToArray())
    End Sub

    Private Sub HeadingUpButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HeadingUpButton.Click
        If (HeadingListBox.SelectedIndex <> -1) Then
            Dim value As String = HeadingListBox.SelectedValue
            Dim index As Integer = 0
            For Each key As String In HeadingListData
                If (key = value) Then
                    Exit For
                End If

                index += 1
            Next

            If (index <> 0) Then
                HeadingListData.Remove(value)
                index -= 1
                HeadingListData.Insert(index, value)
            End If
            FillLists()
        End If
    End Sub

    Private Sub HeadingDownButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HeadingDownButton.Click
        If (HeadingListBox.SelectedIndex <> -1) Then
            Dim value As String = HeadingListBox.SelectedValue
            Dim index As Integer = 0
            For Each key As String In HeadingListData
                If (key = value) Then
                    Exit For
                End If

                index += 1
            Next

            index += 1
            If (index < HeadingListData.Count) Then
                HeadingListData.Remove(value)
                HeadingListData.Insert(index, value)
            End If
            FillLists()
        End If
    End Sub

    Private Sub StubUpButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StubUpButton.Click
        If (StubListBox.SelectedIndex <> -1) Then
            Dim value As String = StubListBox.SelectedValue
            Dim index As Integer = 0
            For Each key As String In StubListData
                If (key = value) Then
                    Exit For
                End If

                index += 1
            Next

            If (index <> 0) Then
                StubListData.Remove(value)
                index -= 1
                StubListData.Insert(index, value)
            End If
            FillLists()
        End If
    End Sub

    Private Sub StubDownButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StubDownButton.Click
        If (StubListBox.SelectedIndex <> -1) Then
            Dim value As String = StubListBox.SelectedValue
            Dim index As Integer = 0
            For Each key As String In StubListData
                If (key = value) Then
                    Exit For
                End If

                index += 1
            Next
            index += 1
            If (index < StubListData.Count) Then
                StubListData.Remove(value)
                StubListData.Insert(index, value)
            End If
            FillLists()
        End If
    End Sub

    Private Sub LoadLanguages()
        ContinueButton.Text = Me.GetLocalizedString(COMPLETE_BUTTON)
        StubLabel.Text = Me.GetLocalizedString(STUB_LABEL)
        HeadingLabel.Text = Me.GetLocalizedString(HEADING_LABEL)
        CancelButton.Text = GetLocalizedString(CANCEL_BUTTON)
        MoveRightButton.Text = GetLocalizedString(MOVE_RIGHT_LABEL)
        MoveLeftButton.Text = GetLocalizedString(MOVE_LEFT_LABEL)
        StubUpButton.Text = GetLocalizedString(MOVE_UP_LABEL)
        StubDownButton.Text = GetLocalizedString(MOVE_DOWN_LABEL)
        HeadingUpButton.Text = GetLocalizedString(MOVE_UP_LABEL)
        HeadingDownButton.Text = GetLocalizedString(MOVE_DOWN_LABEL)
        StubListBox.Attributes.Add("aria-label", GetLocalizedString("CtrlCommandBarFunctionPivotManualListboxRowScreenReader"))
        HeadingListBox.Attributes.Add("aria-label", GetLocalizedString("CtrlCommandBarFunctionPivotManualListboxColumnScreenReader"))
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        For Each var As Variable In Me.PaxiomModel.Meta.Variables
            Page.ClientScript.RegisterForEventValidation(StubListBox.UniqueID, var.Name)
            Page.ClientScript.RegisterForEventValidation(HeadingListBox.UniqueID, var.Name)
            Page.ClientScript.RegisterHiddenField("PivotManualStubList", "")
            Page.ClientScript.RegisterHiddenField("PivotManualHeadingList", "")
        Next

        MyBase.Render(writer)
    End Sub

    ''' <summary>
    ''' Handles event cancel button clicked
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.OnFinished(New CommandBarPluginFinishedEventArgs(Nothing))
    End Sub

End Class
