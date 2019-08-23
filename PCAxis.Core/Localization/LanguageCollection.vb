'Namespace PCAxis.Paxiom.Localization
'   Public Class LanguageCollection
'      Implements IEnumerable(Of KeyValuePair(Of String, Language))
'      Implements ICollection(Of Language)

'      Private _languageDictionary As New Dictionary(Of String, Language)
'      Private _definedSentences As New List(Of String)()

'      Public Function IsLanguageDefined(ByVal languageCode As String) As Boolean
'         If String.IsNullOrEmpty(languageCode) Then
'            Throw New ArgumentNullException("language.Name", _
'                                           "Language.Name is null or empty")
'         End If
'         Return Me._languageDictionary.ContainsKey(languageCode)
'      End Function

'      Private Function IsLanguageComplete(ByVal language As Language) As Boolean
'         For Each sentenceCode As String In Me._definedSentences
'            If Not language.DefinedSentences.Contains(sentenceCode) Then
'               Return False
'            End If
'         Next
'         Return True
'      End Function

'      Public ReadOnly Property DefinedSentences() As List(Of String)
'         Get
'            Return Me._definedSentences
'         End Get
'      End Property

'      Default Public Property Item(ByVal languageCode As String) As Language
'         Get
'            Return Me._languageDictionary(languageCode)
'         End Get
'         Set(ByVal value As Language)
'            If String.IsNullOrEmpty(value.Name) Then
'               Throw New ArgumentNullException("value.Name", _
'                                              "Language.Name is null or empty")
'            End If
'            If Not value.Name = languageCode Then
'               Throw New ArgumentException("Language.Name and languageCode does not match", _
'                                         "languagecode")
'            ElseIf Not Me.IsLanguageDefined(languageCode) Then
'               Me.Add(value)
'            Else
'               If Not Me.IsLanguageComplete(value) Then
'                  Throw New ArgumentException("Language is missing some already defined sentences", _
'                                            "value")
'               End If
'               Me._languageDictionary(languageCode) = value
'            End If
'         End Set
'      End Property

'      Public ReadOnly Property Languages() As Dictionary(Of String, Language).ValueCollection
'         Get
'            Return Me._languageDictionary.Values
'         End Get
'      End Property

'      Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, Language)) Implements IEnumerable(Of KeyValuePair(Of String, Language)).GetEnumerator
'         Return Me._languageDictionary.GetEnumerator()
'      End Function

'      Private Function GetEnumeratorObjects() As IEnumerator Implements IEnumerable.GetEnumerator
'         Return Me._languageDictionary.GetEnumerator()
'      End Function

'      Public Sub Add(ByVal item As Language) Implements ICollection(Of Language).Add
'         If String.IsNullOrEmpty(item.Name) Then
'            Throw New ArgumentNullException("item.Name", _
'                                           "Language.Name is null or empty")
'         End If
'         If _languageDictionary.Count = 0 Then
'            Me._languageDictionary.Add(item.Name, item)
'            Me._definedSentences = item.DefinedSentences
'         ElseIf Not Me._languageDictionary.ContainsKey(item.Name) Then
'            If Not Me.IsLanguageComplete(item) Then
'               Throw New ArgumentException("Language is missing some already defined sentences", _
'                                         "item")
'            End If
'            Me._languageDictionary.Add(item.Name, item)
'         Else
'            Throw New ArgumentException("Language already defined", "item")
'         End If
'      End Sub

'      Public Sub Clear() Implements ICollection(Of Language).Clear
'         Me._languageDictionary.Clear()
'      End Sub

'      Public Function Contains(ByVal item As Language) As Boolean Implements ICollection(Of Language).Contains
'         Me._languageDictionary.ContainsKey(item.Name)
'      End Function

'      Public Sub CopyTo(ByVal array() As Language, ByVal arrayIndex As Integer) Implements ICollection(Of Language).CopyTo
'         Throw New NotImplementedException("Will be implemented")
'      End Sub

'      Public ReadOnly Property Count() As Integer Implements ICollection(Of Language).Count
'         Get
'            Return Me._languageDictionary.Count
'         End Get
'      End Property

'      Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of Language).IsReadOnly
'         Get
'            Return False
'         End Get
'      End Property

'      Public Function Remove(ByVal item As Language) As Boolean Implements ICollection(Of Language).Remove
'         If String.IsNullOrEmpty(item.Name) Then
'            Throw New ArgumentNullException("item.Name", _
'                                           "Language.Name is null or empty")
'         End If
'         If Me._languageDictionary.ContainsKey(item.Name) Then
'            Me._languageDictionary.Remove(item.Name)
'         End If
'      End Function

'      Private Function GetEnumeratorValues() As IEnumerator(Of Language) Implements IEnumerable(Of Language).GetEnumerator
'         Return Me._languageDictionary.Values.GetEnumerator()
'      End Function
'   End Class
'End Namespace
