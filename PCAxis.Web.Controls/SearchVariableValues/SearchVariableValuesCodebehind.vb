

Imports PCAxis.Web.Core
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Enums

Public Class SearchVariableValuesCodebehind
    Inherits PaxiomControlBase(Of SearchVariableValuesCodebehind, SearchVariableValues)

#Region "Localized strings"

    Private Const LOC_ADD_BUTTON As String = "CtrlSearchVariableValuesAddButton"
    Private Const LOC_SEARCH_HEADER As String = "CtrlSearchVariableValuesSearchHeader"
    Private Const LOC_SEARCH_FOR_CODE As String = "CtrlSearchVariableValuesSearchForCode"
    Private Const LOC_SEARCH_FOR_VALUE As String = "CtrlSearchVariableValuesSearchForValue"
    Private Const LOC_SEARCH_TYPE_BEGINNING As String = "CtrlSearchVariableValuesSearchTypeBeginning"
    Private Const LOC_SEARCH_TYPE_ANYWHERE As String = "CtrlSearchVariableValuesSearchTypeAnywhere"
    Private Const LOC_SEARCH As String = "CtrlSearchVariableValuesSearch"
    Private Const LOC_ADD_TO_VARIABLE_SELECTOR As String = "CtrlSearchVariableValuesAddToVariableSelector"

#End Region

#Region " Local variables "

    Protected SearchResult As GridView
    Protected SearchForCode As TextBox
    Protected SearchForValue As TextBox
    Protected SelectedVariableValues As ListBox
    Protected WithEvents Add As Button
    Protected WithEvents Search As Button
    Protected SearchTypeBeginning As RadioButton
    Protected SearchTypeAnywhere As RadioButton
    Protected WithEvents AddToVariableSelector As Button
    Protected SearchHeader As Literal
    Protected SearchForCodeLabel As Label
    Protected SearchForValueLabel As Label
#End Region

    ''' <summary>
    ''' Gets all the selected values.
    ''' </summary>
    ''' <value>All selected values as list of strings</value>
    ''' <returns>List of strings</returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property SelectedValues() As List(Of String)
        Get
            Dim values As New List(Of String)
            For Each li As ListItem In SelectedVariableValues.Items
                values.Add(li.Text)
            Next
            Return values
        End Get
    End Property


    ''' <summary>
    ''' Initializes the control.
    ''' </summary>
    Private Sub SearchVariableValues_Load() Handles Me.Load
        SetLocalizedText()
    End Sub

    ''' <summary>
    ''' Gets new language strings.
    ''' </summary>
    Private Sub SearchVariableValues_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LanguageChanged
        SetLocalizedText()
    End Sub

    ''' <summary>
    ''' Sets up all localized content.
    ''' </summary>
    Private Sub SetLocalizedText()
        Add.Text = Me.GetLocalizedString(LOC_ADD_BUTTON)

        SearchHeader.Text = Me.GetLocalizedString(LOC_SEARCH_HEADER)
        If (Marker.Variable IsNot Nothing) Then
            SearchHeader.Text = SearchHeader.Text + Marker.Variable.Name
        End If

        SearchForCodeLabel.Text = Me.GetLocalizedString(LOC_SEARCH_FOR_CODE)
        SearchForValueLabel.Text = Me.GetLocalizedString(LOC_SEARCH_FOR_VALUE)
        SearchTypeBeginning.Text = Me.GetLocalizedString(LOC_SEARCH_TYPE_BEGINNING)
        SearchTypeAnywhere.Text = Me.GetLocalizedString(LOC_SEARCH_TYPE_ANYWHERE)
        Search.Text = Me.GetLocalizedString(LOC_SEARCH)
        AddToVariableSelector.Text = Me.GetLocalizedString(LOC_ADD_TO_VARIABLE_SELECTOR)
    End Sub

    ''' <summary>
    ''' Add values to be used for selection.
    ''' </summary>
    Protected Sub Add_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Add.Click
        For Each row As GridViewRow In SearchResult.Rows
            Dim cb As CheckBox = CType(row.FindControl("IsSelected"), CheckBox)
            If Not (cb Is Nothing) AndAlso cb.Checked AndAlso row.Visible Then
                Dim code As String = row.Cells(1).Text
                If SelectedVariableValues.Items.FindByValue(code) Is Nothing Then
                    SelectedVariableValues.Items.Add(code)
                End If
                row.Visible = False
            End If
        Next
        If SelectedVariableValues.Items.Count > 1 Then
            SelectedVariableValues.Items.Remove("") 'Remove empty element. XML 1.0 Strict does not validate without an element.
        End If
    End Sub

    ''' <summary>
    ''' Raises an event to signal when value selection is finished.
    ''' </summary>
    Protected Sub AddToVariableSelector_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddToVariableSelector.Click
        Dim selection As New Selection(Marker.Variable.Name)
        For Each item As ListItem In SelectedVariableValues.Items
            selection.ValueCodes.Add(item.Value)
        Next
        VariableSelector.SelectedVariableValues(Marker.Variable.Name) = selection

        Marker.OnSearchVariableValuesAdd(New EventArgs())
    End Sub

    ''' <summary>
    ''' Performs a search in the values based on the search criterias.
    ''' </summary>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Dim v As New List(Of Value)
        Dim _searchForValue As String = SearchForValue.Text.Trim
        Dim _searchForCode As String = SearchForCode.Text.Trim
        If Not String.IsNullOrEmpty(_searchForValue) Then
            If SearchTypeBeginning.Checked Then
                v.AddRange(Marker.Variable.Values.SearchInBeginningOfValue(_searchForValue))
            Else
                v.AddRange(Marker.Variable.Values.SearchInValue(_searchForValue))
            End If
        End If
        If Not String.IsNullOrEmpty(_searchForCode) Then
            If SearchTypeBeginning.Checked Then
                v.AddRange(Marker.Variable.Values.SearchInBeginningOfCode(_searchForCode))
            Else
                v.AddRange(Marker.Variable.Values.SearchInCode(_searchForCode))
            End If
        End If
        SearchResult.DataSource = v
        SearchResult.DataBind()
    End Sub

End Class
