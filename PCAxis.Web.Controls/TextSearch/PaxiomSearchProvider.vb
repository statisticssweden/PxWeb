'Public Class PaxiomSearchProvider
'    Inherits TextSearchProviderBase


'    Private _libraries As New Dictionary(Of String, SearchLibrary)
'    Private _menu As PxMenu.PxMenuBase
'    'Private _menuHelper As MenuHelper
'    'Public Sub New(ByVal rootPath As String)
'    '    _libraries.Add(New SearchLibrary(IO.Path.GetFileName(rootPath), rootPath))
'    '    For Each Dir As String In IO.Directory.GetDirectories(rootPath, "*", IO.SearchOption.AllDirectories)
'    '        _libraries.Add(New SearchLibrary(IO.Path.GetFileName(Dir), Dir))
'    '    Next
'    'End Sub

'    Public Sub New(ByVal menu As PxMenu.PxMenuBase)
'        BuildLibrary(menu.RootItem)
'        _menu = menu
'        _menuHelper = New MenuHelper(menu)
'    End Sub


'    Public Sub BuildLibrary(ByVal menuItem As PxMenu.MenuItem)
'        _libraries.Add(menuItem.Selection, New SearchLibrary(GetLevelString(menuItem.LevelNumber) + menuItem.Text, menuItem.Selection))
'        For Each item As PxMenu.Item In menuItem.SubItems
'            If TypeOf (item) Is PxMenu.MenuItem Then
'                Dim childMenuItem As PxMenu.MenuItem = CType(item, PxMenu.MenuItem)
'                BuildLibrary(childMenuItem)
'            End If
'        Next
'    End Sub

'    Private Function GetLevelString(ByVal levelNumber As Integer) As String
'        Dim s As String = String.Empty
'        For i As Integer = 0 To levelNumber - 2
'            s += "..\"
'        Next
'        Return s
'    End Function




'    Private Sub PerformSearch(ByVal menuItem As PxMenu.MenuItem, ByVal keywords As IEnumerable(Of String), ByVal searchType As TextSearchType, ByVal results As List(Of SearchResult))
'        Dim t As Paxiom.PXFileBuilder = Nothing

'        'Dim count As Integer = GetCount(keywords) - 1

'        For Each item As PxMenu.Item In menuItem.SubItems
'            If TypeOf (item) Is PxMenu.MenuItem Then
'                PerformSearch(CType(item, PxMenu.MenuItem), keywords, searchType, results)
'            ElseIf TypeOf (item) Is PxMenu.Link Then
'                Dim link As PxMenu.Link = CType(item, PxMenu.Link)

'                If link.Type = PxMenu.LinkType.PX Then

'                    Try
'                        Dim path As String = _menuHelper.GetFullPath(link)
'                        If IO.File.Exists(path) Then
'                            t = New Paxiom.PXFileBuilder()

'                            With t
'                                .SetPath(path)
'                                .SetPreferredLanguage(Core.Management.LocalizationManager.GetTwoLetterLanguageCode())
'                                .BuildForSelection()
'                            End With


'                            Dim search = (From keyword In keywords _
'                                     From variable In t.Model.Meta.Variables _
'                                     Let variableValues = _
'                                        (From value In variable.Values _
'                                        Where value.Value.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > -1 _
'                                        Select ValueCode = value.Code) _
'                                     Where variable.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > -1 Or variableValues.Count > 0 _
'                                     Group variable.Code, variableValues By keyword Into VariableGroups = Group)

'                            search.ToString()

'                            If search.Count > 0 AndAlso Not (searchType = TextSearchType.And AndAlso search.Count <> keywords.Count) Then
'                                Dim fileInfo As IO.FileInfo = New IO.FileInfo(path)
'                                Dim searchResultItem As New SearchResult With {.MenuSelection = link.Selection, .PresentationText = link.Text, .Size = CInt(fileInfo.Length / 1024)}

'                                searchResultItem.Attributes.Add("CtrlTextSearchAttributePath", _menuHelper.GetRelativePath(link).Replace("~", ".."))
'                                searchResultItem.Attributes.Add("CtrlTextSearchAttributeModified", fileInfo.LastWriteTime.ToString())
'                                searchResultItem.Attributes.Add("CtrlTextSearchAttributeSize", (fileInfo.Length / 1024).ToString("N") + "kb")


'                                Dim variables = From variableGroup In search _
'                                                From variable In variableGroup.VariableGroups _
'                                                Select variable.Code Distinct

'                                For Each var In variables
'                                    Dim variabelCode = var
'                                    Dim values = From variableGroup In search _
'                                                 From variable In variableGroup.VariableGroups _
'                                                 From value In variable.variableValues _
'                                                 Where variable.Code = variabelCode _
'                                                 Select value Distinct

'                                    Dim sel As New Paxiom.Selection(var)
'                                    sel.ValueCodes.AddRange(values.ToArray)

'                                    searchResultItem.PaxiomSelection.Add(sel)
'                                Next


'                                results.Add(searchResultItem)

'                            End If


'                        End If
'                    Catch ex As Exception
'                        ex.ToString()
'                    Finally
'                        If t IsNot Nothing Then
'                            t = Nothing
'                        End If
'                    End Try
'                End If
'            End If
'        Next
'    End Sub


'    Protected Overrides ReadOnly Property CacheLibrary() As Boolean
'        Get
'            Return False
'        End Get
'    End Property

'    Protected Overrides Function DoSearch(ByVal keywords As System.Collections.Generic.IEnumerable(Of String), ByVal searchType As TextSearchType, ByVal library As SearchLibrary) As System.Collections.Generic.List(Of SearchResult)
'        Dim result As New List(Of SearchResult)
'        _menu.SetCurrentItemBySelection(library.Value)
'        If TypeOf (_menu.CurrentItem) Is PxMenu.MenuItem Then
'            PerformSearch(CType(_menu.CurrentItem, PxMenu.MenuItem), keywords, searchType, result)
'        End If
'        Return result
'    End Function

'    Protected Overrides Function GetLibraries() As System.Collections.Generic.IDictionary(Of String, SearchLibrary)
'        Return _libraries
'    End Function
'End Class
