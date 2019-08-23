Public MustInherit Class TextSearchProviderBase
    Implements ITextSearchProvider

    Protected MustOverride Function GetLibraries() As IDictionary(Of String, SearchLibrary)

    Protected MustOverride Function DoSearch(ByVal keywords As IEnumerable(Of String), ByVal searchType As TextSearchType, ByVal library As SearchLibrary) As List(Of SearchResult)

    Protected MustOverride ReadOnly Property CacheLibrary() As Boolean

    Private _libraries As IDictionary(Of String, SearchLibrary)

    Public ReadOnly Property Libraries() As IDictionary(Of String, SearchLibrary) Implements ITextSearchProvider.Libraries
        Get
            If Me.CacheLibrary AndAlso _libraries IsNot Nothing Then
                Return _libraries
            ElseIf Me.CacheLibrary Then
                _libraries = GetLibraries()
                Return _libraries
            Else
                Return GetLibraries()
            End If
        End Get
    End Property

    Public Function Search(ByVal keywords As System.Collections.Generic.IEnumerable(Of String), ByVal searchType As TextSearchType, ByVal library As SearchLibrary) As System.Collections.Generic.List(Of SearchResult) Implements ITextSearchProvider.Search
        Dim s As String = String.Empty

        Select Case searchType
            Case TextSearchType.All
                For Each keyword As String In keywords
                    s += keyword + " "
                Next
                Return DoSearch(s.Trim().Split(New String() {""}, StringSplitOptions.RemoveEmptyEntries), searchType, library)
        End Select

        Return DoSearch(keywords, searchType, library)
    End Function
End Class
