Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class TextSearch
    Inherits MarkerControlBase(Of TextSearchCodebehind, TextSearch)
#Region "Properties"

    <PropertyPersistState(PersistStateType.PerRequest)> _
    Public Shared ReadOnly Property SearchResults() As Dictionary(Of String, SearchResult)
        Get
            Const NAME_SEARCHRESULTS As String = "SearchResults"
            If Not StateProvider.StateProviderFactory.GetStateProvider().Contains(GetType(TextSearch), NAME_SEARCHRESULTS) Then
                StateProvider.StateProviderFactory.GetStateProvider.Add(GetType(TextSearch), NAME_SEARCHRESULTS, New Dictionary(Of String, SearchResult))
            End If
            Return CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(TextSearch), NAME_SEARCHRESULTS), Global.System.Collections.Generic.Dictionary(Of String, SearchResult))
        End Get
    End Property

    Private _searchProvider As ITextSearchProvider
    Public Property SearchProvider() As ITextSearchProvider
        Get
            Return _searchProvider
        End Get
        Set(ByVal value As ITextSearchProvider)
            _searchProvider = value
        End Set
    End Property


    Private _selectionPage As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectionPage() As String
        Get
            Return _selectionPage
        End Get
        Set(ByVal value As String)
            _selectionPage = value
        End Set
    End Property

    Private _downloadPage As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DownloadPage() As String
        Get
            Return _downloadPage
        End Get
        Set(ByVal value As String)
            _downloadPage = value
        End Set
    End Property

    Private _visibleAttributes As New List(Of String)
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property VisibleAttributes() As List(Of String)
        Get
            Return _visibleAttributes
        End Get
        Set(ByVal value As List(Of String))
            _visibleAttributes = value
        End Set

    End Property


    Private _showCommandBar As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowCommandBar() As Boolean
        Get
            Return _showCommandBar
        End Get
        Set(ByVal value As Boolean)
            _showCommandBar = value
        End Set
    End Property



    Private _showDownloadLink As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowDownloadLink() As Boolean
        Get
            Return _showDownloadLink
        End Get
        Set(ByVal value As Boolean)
            _showDownloadLink = value
        End Set
    End Property


    Private _showSelectLink As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSelectLink() As Boolean
        Get
            Return _showSelectLink
        End Get
        Set(ByVal value As Boolean)
            _showSelectLink = value
        End Set
    End Property


    Private _showViewLink As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowViewLink() As Boolean
        Get
            Return _showViewLink
        End Get
        Set(ByVal value As Boolean)
            _showViewLink = value
        End Set
    End Property

    Private _useDefaultValues As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property UseDefaultValues() As Boolean
        Get
            Return _useDefaultValues
        End Get
        Set(ByVal value As Boolean)
            _useDefaultValues = value
        End Set
    End Property



    Private _useDownloadLimit As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property UseDownloadLimit() As Boolean
        Get
            Return _useDownloadLimit
        End Get
        Set(ByVal value As Boolean)
            _useDownloadLimit = value
        End Set
    End Property


    Private _downloadLimit As Integer
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DownloadLimit() As Integer
        Get
            Return _downloadLimit
        End Get
        Set(ByVal value As Integer)
            _downloadLimit = value
        End Set
    End Property





#End Region

End Class
