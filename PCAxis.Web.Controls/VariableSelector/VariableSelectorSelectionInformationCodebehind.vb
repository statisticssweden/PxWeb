

Imports PCAxis.Paxiom.Localization
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Enums
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Core.Management

''' <summary>
''' Control that presents information on limits for selecttion and selections done.
''' </summary>
''' <remarks></remarks>
Public Class VariableSelectorSelectionInformationCodebehind
    Inherits PaxiomControlBase(Of VariableSelectorSelectionInformationCodebehind, VariableSelectorSelectionInformation)

#Region "Local variables"



#End Region

#Region "Localized strings"
    Private Const LOC_SELECTION_LIMITS_ROWCOLUMN_LABEL As String = "CtrlVariableSelectorSelectionLimitsRowsColumnsLabel"
    Private Const LOC_SELECTION_LIMITS_CELLS_LABEL As String = "CtrlVariableSelectorSelectionLimitsCellsLabel"
    Private Const LOC_SELECTED_ROWS_LABEL As String = "CtrlVariableSelectorSelectedRowsLabel"
    Private Const LOC_SELECTED_COLUMNS_LABEL As String = "CtrlVariableSelectorSelectedColumnsLabel"
    Private Const LOC_SELECTED_CELLS_LABEL As String = "CtrlVariableSelectorSelectedCellsLabel"
    Private Const LOC_SELECTED_CELLS_LIMIT_LABEL As String = "CtrlVariableSelectorSelectedCellsLimitLabel"
#End Region

#Region "Controls"
    Protected SelectionLimitsInformationPlaceHolder As PlaceHolder
    Protected RowColSelectionInformationPlaceHolder As PlaceHolder
    Protected CellSelectionInformationPlaceHolder As PlaceHolder
    Protected SelectionLimitationLabel As Label
    Protected SelectionMadeInformationPlaceHolder As PlaceHolder
    Protected SelectedRowsLabel As Label
    Protected SelectedColumnsLabel As Label
    Protected SelectedCellsLabel As Label
    Protected SelectedRowsLabelSelected As Label
    Protected SelectedColumnsLabelSelected As Label
    Protected SelectedCellsLabelSelected As Label
    Protected SelectedCellsNumberLabel As Label
    Protected SelectedCellsLimitLabel As Label
    Protected SelectionLimitation As HtmlControls.HtmlInputHidden
    Protected NumberFormat As HtmlControls.HtmlInputHidden
#End Region


    Private Sub VariableSelectorSelectionInformation(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CreateSelectionLimitsInformationText()
        If Not IsPostBack Then
            SetNumberFormat()
        End If
    End Sub

    Private Sub CreateSelectionLimitsInformationText()
        Dim selectionLimitedByRowsCols As Boolean = (Marker.LimitSelectionsBy = "RowsColumns")

        'Show/hide selection limits
        If (Marker.ShowSelectionLimits) Then
            SelectionLimitsInformationPlaceHolder.Visible = True
        Else
            SelectionLimitsInformationPlaceHolder.Visible = False
        End If

        'Select what selection count that is used Rows-cols or Cells
        If (selectionLimitedByRowsCols) Then
            RowColSelectionInformationPlaceHolder.Visible = True
            CellSelectionInformationPlaceHolder.Visible = False
            Dim rows As String = DataFormatter.NumericToString(Marker.SelectedRowsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode())
            Dim cols As String = DataFormatter.NumericToString(Marker.SelectedColumnsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode())
            SelectionLimitationLabel.Text = String.Format(Me.GetLocalizedString(LOC_SELECTION_LIMITS_ROWCOLUMN_LABEL), rows, cols)
            SelectionLimitation.Value = Marker.SelectedRowsLimit.ToString() & "," & Marker.SelectedColumnsLimit.ToString()
        Else 'Marker.LimitSelectionsBy = "Cells"
            RowColSelectionInformationPlaceHolder.Visible = False
            CellSelectionInformationPlaceHolder.Visible = True
            'SelectionLimitationLabel.Text = String.Format(Me.GetLocalizedString(LOC_SELECTION_LIMITS_CELLS_LABEL), DataFormatter.NumericToString(Marker.CellLimitScreen, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            Dim rows As String = DataFormatter.NumericToString(Marker.SelectedRowsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode())
            Dim cols As String = DataFormatter.NumericToString(Marker.SelectedColumnsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode())
            SelectionLimitationLabel.Text = String.Format(Me.GetLocalizedString(LOC_SELECTION_LIMITS_ROWCOLUMN_LABEL), rows, cols)
            SelectionLimitation.Value = Marker.SelectedTotalCellsLimit.ToString()
        End If

        If (Marker.ShowSelectionsMade) Then
            SelectionMadeInformationPlaceHolder.Visible = True
            If (selectionLimitedByRowsCols) Then
                SelectedRowsLabel.Text = Me.GetLocalizedString(LOC_SELECTED_ROWS_LABEL)
                SelectedColumnsLabel.Text = Me.GetLocalizedString(LOC_SELECTED_COLUMNS_LABEL)
            Else 'Marker.LimitSelectionsBy = "Cells"
                SelectedCellsLabel.Text = Me.GetLocalizedString(LOC_SELECTED_CELLS_LABEL)
                SelectedCellsLimitLabel.Text = String.Format(Me.GetLocalizedString(LOC_SELECTED_CELLS_LIMIT_LABEL), DataFormatter.NumericToString(Marker.SelectedTotalCellsLimit, 0, LocalizationManager.GetTwoLetterLanguageCode()))
            End If
        Else
            SelectionMadeInformationPlaceHolder.Visible = False
        End If

    End Sub

    Private Sub SetNumberFormat()
        Dim lang As String = LocalizationManager.CurrentCulture.Name

        NumberFormat.Value = "#" & PCAxis.Paxiom.Settings.GetLocale(lang).ThousandSeparator & "##0" & PCAxis.Paxiom.Settings.GetLocale(lang).DecimalSeparator & "####"
    End Sub

    Private Sub VariableSelectorEliminationInformation_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.LanguageChanged
        CreateSelectionLimitsInformationText()
        SetNumberFormat()
    End Sub

    Private Sub VariableSelectorEliminationInformation_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.PaxiomModelChanged
        CreateSelectionLimitsInformationText()
    End Sub


End Class
