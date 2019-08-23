Imports PCAxis.Web.Core.Enums
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Globalization
Imports System.ComponentModel
Imports PCAxis.Paxiom.Localization
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports PCAxis.Web.Core.Management.LinkManager

Public Enum SearchResultViewMode
    SelectAndDownload
    SelectAndDownloadIfSmaller
    SelectOnly
    DownloadOnly
    ViewOnly
    ViewOnlyDefaultValues
    ViewOnlyDefaultValuesCommandBar
End Enum
Public Class TextSearchCodebehind
    Inherits PaxiomControlBase(Of TextSearchCodebehind, TextSearch)

#Region "Constants"

    Private Const FOOTNOTE_LABEL As String = "FootNoteLabel"
    Private Const EMPTY_CELL_VALUE As String = "&nbsp;"
    Private Const DECIMAL_DELIMITER As String = "CtrlTableDecimalDelimiter"
    Private Const THOUSAND_DELIMITER As String = "CtrlTableThousandDelimiter"
    Private Const CSS_HEADER_FIRST As String = "table-header-first"
    Private Const CSS_HEADER_LAST As String = "table-header-last"
    Private Const CSS_HEADER_MIDDLE As String = "table-header-middle"
    Private Const CSS_DATA_FILLED As String = "table-data-filled"
    Private Const CSS_TITLE As String = "table-title"
    Private Const CSS_STUB As String = "table-stub"
    Private Const CSS_LEFT As String = "left"
    Private Const CSS_TABLE_SORT_HEADER As String = "table-sort-header"
    Private Const LOC_SEARCHRESULTTEXT As String = "CtrlTextSearchResultText"
    Private Const LOC_SEARCHOPTIONALL As String = "CtrlTextSearchOptionAll"
    Private Const LOC_SEARCHOPTIONAND As String = "CtrlTextSearchOptionAnd"
    Private Const LOC_SEARCHOPTIONOR As String = "CtrlTextSearchOptionOr"
    Private Const LOC_SEARCHBUTTONTEXT As String = "CtrlTextSearchButtonText"
    Private Const CTRL_RESULTATTRIBUTESREPEATER As String = "ResultAttributesRepeater"
    Private Const PROP_ATTRIBUTES As String = "Attributes"
#End Region

#Region "Private Variables"

    Protected WithEvents ResultRepeater As WebControls.Repeater
    Protected SearchTextBox As TextBox
    Protected WithEvents SearchButton As Button
    Protected SearchLibrariesListBox As ListBox
    Protected SearchUIPanel As Panel
    Protected ResultUIPanel As Panel
    Protected ResultHeader As Literal
    Protected SearchOption As RadioButtonList
    Protected SearchTextBoxRequiredFieldValidator As RequiredFieldValidator

#End Region


    Private _searchText As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Protected Property SearchText() As String
        Get
            Return _searchText
        End Get
        Set(ByVal value As String)
            _searchText = value
        End Set
    End Property



    Private Sub TextSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        

        SearchTextBoxRequiredFieldValidator.ErrorMessage = Me.GetLocalizedString("CtrlTextSearchErrorTextBox")
        If TextSearch.SearchResults.Count > 0 Then
            ShowSearchResults(TextSearch.SearchResults.Values.ToList())
        Else
            If Not Me.Page.IsPostBack AndAlso Marker.SearchProvider IsNot Nothing AndAlso Marker.SearchProvider.Libraries.Count > 0 Then
                SearchLibrariesListBox.DataSource = Marker.SearchProvider.Libraries.Values
                SearchLibrariesListBox.DataBind()
                SearchLibrariesListBox.Items(0).Selected = True

                SearchOption.DataSource = New Object() {New With {.Key = GetLocalizedString(LOC_SEARCHOPTIONALL), .Value = "0"}, _
                                                         New With {.Key = GetLocalizedString(LOC_SEARCHOPTIONAND), .Value = "1"}, _
                                                         New With {.Key = GetLocalizedString(LOC_SEARCHOPTIONOR), .Value = "2"}}
                SearchOption.DataBind()
                SearchOption.Items(0).Selected = True

            End If

            ResultUIPanel.Visible = False
            SearchUIPanel.Visible = True

            Me.SearchButton.Text = GetLocalizedString(LOC_SEARCHBUTTONTEXT)
        End If
    End Sub

    Private Function GetTextSearchType(ByVal searchOption As RadioButtonList) As TextSearchType
        Select Case searchOption.SelectedValue
            Case "0"
                Return TextSearchType.All
            Case "1"
                Return TextSearchType.And
            Case "2"
                Return TextSearchType.Or
            Case Else
                Return TextSearchType.All
        End Select
    End Function
    Private Function GetSearchText(ByVal dividedText As String(), ByVal languageCode As String) As String
        Dim dividerText As String = String.Format(CultureInfo.InvariantCulture, " {0} ", GetLocalizedString(languageCode)).ToLower

        Return String.Format(CultureInfo.InvariantCulture, "{0} {1}", _
                             GetLocalizedString(LOC_SEARCHRESULTTEXT), String.Concat(dividedText.Select( _
                            Function(text, index) If(index < dividedText.Length - 1, text + dividerText, text)) _
                            .ToArray()))
    End Function

    Public Function ShowHeading(ByVal size As Integer) As Boolean
        Dim visibleLinks As Integer = 0

        If (Marker.ShowDownloadLink And Marker.UseDownloadLimit = False) Or (Marker.ShowDownloadLink And Marker.UseDownloadLimit And size <= Marker.DownloadLimit) Then
            visibleLinks += 1
        End If

        If Marker.ShowSelectLink Then
            visibleLinks += 1
        End If

        If Marker.ShowViewLink Then
            visibleLinks += 1
        End If

        If visibleLinks = 0 Or visibleLinks > 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ShowDownloadLink(ByVal size As Integer) As Boolean
        If (Marker.ShowDownloadLink And Marker.UseDownloadLimit = False) Or (Marker.ShowDownloadLink And Marker.UseDownloadLimit And size <= Marker.DownloadLimit) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetLink(ByVal selection As String, ByVal mode As String) As String

        Dim linkItems As New List(Of LinkItem)

        linkItems.Add(New LinkItem("px", selection))

        Select Case mode
            Case "Select"
                Return Management.LinkManager.CreateLink(Marker.SelectionPage, linkItems.ToArray())
            Case "Download"
                Return Management.LinkManager.CreateLink(Marker.DownloadPage, linkItems.ToArray())
            Case "View"
                linkItems.Add(New LinkItem("action", CStr(IIf(Marker.UseDefaultValues, "selectdefault", "selectall"))))
                linkItems.Add(New LinkItem("commandbar", Marker.ShowCommandBar.ToString().ToLower))
                Return Management.LinkManager.CreateLink(Marker.SelectionPage, linkItems.ToArray())
        End Select
        Return selection
    End Function


    Private Sub ResultRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ResultRepeater.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim attributeRepeater As Repeater = CType(e.Item.FindControl(CTRL_RESULTATTRIBUTESREPEATER), Repeater)
                attributeRepeater.DataSource = DataBinder.GetPropertyValue(e.Item.DataItem, PROP_ATTRIBUTES)
                attributeRepeater.DataBind()
        End Select

    End Sub

    Private Sub ShowSearchResults(ByVal results As List(Of SearchResult))
        Dim resultQuery = From resultItem In results _
                                      Let Attributes = (From attribute In resultItem.Attributes _
                                                        Where Marker.VisibleAttributes.Contains(attribute.Key) _
                                                        Select Key = Me.GetLocalizedString(attribute.Key), attribute.Value) _
                                      Select resultItem.PresentationText, _
                                      Selection = resultItem.MenuSelection, _
                                      resultItem.Size, _
                                      Attributes


        ResultRepeater.DataSource = resultQuery
        ResultRepeater.DataBind()


        Select Case GetTextSearchType(SearchOption)
            Case TextSearchType.All
                ResultHeader.Text = Me.Page.Server.HtmlEncode(String.Format(CultureInfo.InvariantCulture, "{0} {1}", GetLocalizedString(LOC_SEARCHRESULTTEXT), SearchText))
            Case TextSearchType.And
                ResultHeader.Text = Me.Page.Server.HtmlEncode(GetSearchText(GetDividedText, LOC_SEARCHOPTIONAND))
            Case TextSearchType.Or
                ResultHeader.Text = Me.Page.Server.HtmlEncode(GetSearchText(GetDividedText, LOC_SEARCHOPTIONOR))
        End Select

        ResultUIPanel.Visible = True
        SearchUIPanel.Visible = False
    End Sub
    Private Function GetDividedText() As String()
        Return SearchText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
    End Function
    Private Sub SearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Me.Page.Validate()
        If Me.Page.IsValid() Then

            SearchText = SearchTextBox.Text

            Dim dividedText As String() = GetDividedText()

            Dim results As List(Of SearchResult) = Marker.SearchProvider.Search(dividedText, GetTextSearchType(SearchOption), Marker.SearchProvider.Libraries(SearchLibrariesListBox.SelectedValue))


            TextSearch.SearchResults.Clear()
            For Each result As SearchResult In results
                If Not TextSearch.SearchResults.ContainsKey(result.MenuSelection) Then
                    TextSearch.SearchResults.Add(result.MenuSelection, result)
                End If
            Next


            ShowSearchResults(results)

        End If
    End Sub
End Class