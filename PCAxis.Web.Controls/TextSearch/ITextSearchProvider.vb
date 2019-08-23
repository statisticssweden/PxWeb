Public Structure SearchLibrary

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property


    Private _Value As String
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Sub New(ByVal name As String, ByVal value As String)
        Me.Value = value
        Me.Name = name
    End Sub

End Structure



Public Class SearchResult

    Private _menuSelection As String
    Public Property MenuSelection() As String
        Get
            Return _menuSelection
        End Get
        Set(ByVal value As String)
            _menuSelection = value
        End Set
    End Property


    Private _presentationText As String
    Public Property PresentationText() As String
        Get
            Return _presentationText
        End Get
        Set(ByVal value As String)
            _presentationText = value
        End Set
    End Property



    Private _paxiomSelection As New List(Of Paxiom.Selection)
    Public ReadOnly Property PaxiomSelection() As List(Of Paxiom.Selection)
        Get
            Return _paxiomSelection
        End Get

    End Property


    Private _size As Integer
    Public Property Size() As Integer
        Get
            Return _size
        End Get
        Set(ByVal value As Integer)
            _size = value
        End Set
    End Property


    Private _attributes As New Dictionary(Of String, String)
    Public ReadOnly Property Attributes() As Dictionary(Of String, String)
        Get

            Return _attributes
        End Get
    End Property



End Class


Public Interface ITextSearchProvider

    Function Search(ByVal keywords As IEnumerable(Of String), ByVal searchType As TextSearchType, ByVal library As SearchLibrary) As List(Of SearchResult)

    ReadOnly Property Libraries() As IDictionary(Of String, SearchLibrary)


End Interface
