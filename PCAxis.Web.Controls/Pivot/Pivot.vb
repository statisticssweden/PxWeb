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

''' <summary>
''' Pivot dialog control.
''' </summary>
''' <remarks>
''' After rearainging is done and the user has clicked the continue button.
''' Thenn you need to retrive the PXModel from this control to display the changes.
''' </remarks>
Public Class Pivot
    Inherits CommandBarPluginBase
    Private Const CONTINUE_BUTTON As String = "CtrlPivotContinueButton"
    Private Const STUB_LABEL As String = "CtrlPivotStubLabel"
    Private Const HEADING_LABEL As String = "CtrlPivotHeadingLabel"

#Region " Controls "
    <ControlBinding("HeadingListBox")> _
    Private _headingListBox As ListBox
    <ControlBinding("StubListBox")> _
    Private _stubListBox As ListBox
    <ControlBinding("MoveLeftButton")> _
    Private WithEvents _moveLeftButton As ImageButton
    <ControlBinding("MoveRightButton")> _
    Private WithEvents _moveRightButton As ImageButton
    <ControlBinding("ContinueButton")> _
    Private WithEvents _continueButton As Button

    <ControlBinding("StubLabel")> _
    Private _stubLabel As Label
    <ControlBinding("HeadingLabel")> _
    Private _headingLabel As Label

    <ControlBinding("HeadingUpButton")> _
    Private WithEvents _headingUpButton As ImageButton
    <ControlBinding("HeadingDownButton")> _
    Private WithEvents _headingDownButton As ImageButton
    <ControlBinding("StubUpButton")> _
    Private WithEvents _stubUpButton As ImageButton
    <ControlBinding("StubDownButton")> _
    Private WithEvents _stubDownButton As ImageButton

#End Region

#Region " Properties "

    ''' <summary>
    ''' Datasource for the Left listbox.
    ''' </summary>
    ''' 
    Private _headingListData As List(Of String)

    <Attributes.PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Private Property HeadingListData() As List(Of String)
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

    <Attributes.PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Private Property StubListData() As List(Of String)
        Get
            Return _stubListData
        End Get
        Set(ByVal value As List(Of String))
            _stubListData = value
        End Set
    End Property

    ''' <summary>
    ''' Set the image URL of the up buttons.
    ''' </summary>
    Public Property UpButtonImagePath() As String
        Get
            Return _headingUpButton.ImageUrl
        End Get
        Set(ByVal value As String)
            _headingUpButton.ImageUrl = value
            _stubUpButton.ImageUrl = value
        End Set
    End Property
    ''' <summary>
    ''' Set the image URL of the down buttons.
    ''' </summary>
    Public Property DownButtonImagePath() As String
        Get
            Return _headingDownButton.ImageUrl
        End Get
        Set(ByVal value As String)
            _headingDownButton.ImageUrl = value
            _stubDownButton.ImageUrl = value
        End Set
    End Property
    ''' <summary>
    ''' Set the image URL of the left button.
    ''' </summary>
    Public Property LeftButtonImagePath() As String
        Get
            Return _moveLeftButton.ImageUrl
        End Get
        Set(ByVal value As String)
            _moveLeftButton.ImageUrl = value
        End Set
    End Property
    ''' <summary>
    ''' Set the image URL of the right button.
    ''' </summary>
    Public Property RightButtonImagePath() As String
        Get
            Return _moveRightButton.ImageUrl
        End Get
        Set(ByVal value As String)
            _moveRightButton.ImageUrl = value
        End Set
    End Property

#End Region

    Private Sub Pivot_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
        LoadLanguages()
    End Sub

    Private Sub Pivot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Read viewstate to fill datalists again.
        'Me.HeadingListData = CType(Me.ViewState("HeadingListData"), List(Of String))
        'Me.StubListData = CType(Me.ViewState("StubListData"), List(Of String))

        If ((HeadingListData Is Nothing Or StubListData Is Nothing) Or (Not Me.Page.IsPostBack)) Then
            CreateDataSources()
            FillLists()
            LoadLanguages()
        End If

        'Me.UpButtonImagePath = Me.Properties("")

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
        _headingListBox.DataSource = HeadingListData
        _stubListBox.DataSource = StubListData
        _headingListBox.DataBind()
        _stubListBox.DataBind()
    End Sub

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        AddHandler _moveLeftButton.Click, AddressOf _moveLeftButton_Click
        AddHandler _moveRightButton.Click, AddressOf _moveRightButton_Click
        AddHandler _continueButton.Click, AddressOf _continueButton_Click

        AddHandler _headingUpButton.Click, AddressOf _headingUpButton_Click
        AddHandler _headingDownButton.Click, AddressOf _headingDownButton_Click
        AddHandler _stubUpButton.Click, AddressOf _stubUpButton_Click
        AddHandler _stubDownButton.Click, AddressOf _stubDownButton_Click

        Me.DownButtonImagePath = Me.Properties("DownButtonImagePath")
        Me.LeftButtonImagePath = Me.Properties("LeftButtonImagePath")
        Me.RightButtonImagePath = Me.Properties("RightButtonImagePath")
        Me.UpButtonImagePath = Me.Properties("UpButtonImagePath")

    End Sub

    Private Sub _moveLeftButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _moveLeftButton.Click
        If (_stubListBox.SelectedIndex <> -1) Then
            Dim value As String = _stubListBox.SelectedValue
            StubListData.Remove(value)
            HeadingListData.Add(value)
            FillLists()
        End If
    End Sub

    Private Sub _moveRightButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _moveRightButton.Click
        If (_headingListBox.SelectedIndex <> -1) Then
            Dim value As String = _headingListBox.SelectedValue
            HeadingListData.Remove(value)
            StubListData.Add(value)
            FillLists()
        End If
    End Sub

    Private Sub _continueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _continueButton.Click
        'Fri(pivotering)
        Dim pd As New List(Of PCAxis.Paxiom.Operations.PivotDescription)()

        'Här får du dimensionera upp pd variabeln så att den blir lika stor som antalet variabler i px-modellen
        'där efter får du skapa pxDescription's objekt som säger var variablerna ska vara placerade efter pivotering
        'Exempel med en px-modell med två variabler, kön och län. Efter pivoteringen vill man att kön ska hamna i heading och län i stub
        'Dim pd(1) As PCAxis.Paxiom.Operations.PivotDescription
        'pd(0) =  New PCAxis.Paxiom.Operations.PivotDescription("kön", PCAxis.Paxiom.PlacementType.Heading)
        'pd(1) =  New PCAxis.Paxiom.Operations.PivotDescription("län", PCAxis.Paxiom.PlacementType.Stub)

        For Each value As String In HeadingListData
            pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(value, PCAxis.Paxiom.PlacementType.Heading))
        Next

        For Each value As String In StubListData
            pd.Add(New PCAxis.Paxiom.Operations.PivotDescription(value, PCAxis.Paxiom.PlacementType.Stub))
        Next

        'Därefter utför vi pivoteringen
        Dim p As New PCAxis.Paxiom.Operations.Pivot
        ' Set the new model
        'Me.PaxiomModel = p.Execute(Me.PaxiomModel, pd.ToArray())
        ' raise event
        Me.OnFinished(New PCAxis.Web.Controls.CommandBarPluginFinishedEventArgs(p.Execute(Me.PaxiomModel, pd.ToArray())))
    End Sub

    Private Sub _headingUpButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _headingUpButton.Click
        If (_headingListBox.SelectedIndex <> -1) Then
            Dim value As String = _headingListBox.SelectedValue
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

    Private Sub _headingDownButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _headingDownButton.Click
        If (_headingListBox.SelectedIndex <> -1) Then
            Dim value As String = _headingListBox.SelectedValue
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

    Private Sub _stubUpButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _stubUpButton.Click
        If (_stubListBox.SelectedIndex <> -1) Then
            Dim value As String = _stubListBox.SelectedValue
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

    Private Sub _stubDownButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _stubDownButton.Click
        If (_stubListBox.SelectedIndex <> -1) Then
            Dim value As String = _stubListBox.SelectedValue
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
        _continueButton.Text = Me.GetLocalizedString(CONTINUE_BUTTON)
        _stubLabel.Text = Me.GetLocalizedString(STUB_LABEL)
        _headingLabel.Text = Me.GetLocalizedString(HEADING_LABEL)
    End Sub

End Class
