Imports System.Xml.Serialization

Namespace PCAxis.Paxiom.Localization

    <System.Serializable()> _
        Public Class Language
        Implements ICollection(Of Sentence)

        Private _sentenceDictionary As New Dictionary(Of String, Sentence)()
        Private _languageName As String
        Private _definedSentencesList As New List(Of String)()


        ''' <summary>
        ''' Name of the language
        ''' </summary>
        ''' <value>Language name</value>
        ''' <returns>Language name</returns>
        ''' <remarks></remarks>
        Public Property Name() As String
            Get
                Return Me._languageName
            End Get
            Set(ByVal value As String)
                Me._languageName = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a new Language without a name
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.Name = String.Empty
        End Sub

        ''' <summary>
        ''' Creates a new Langugae with the included name
        ''' </summary>
        ''' <param name="name">Name of the language</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String)
            Me.Name = name
        End Sub

        ''' <summary>
        ''' Create a new Language with the included name and sentences
        ''' </summary>
        ''' <param name="name">Name of the language</param>
        ''' <param name="sentences">Array of sentences</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String, ByVal sentences As Sentence())
            Me.Name = name

            For Each newSentance As Sentence In sentences
                If String.IsNullOrEmpty(newSentance.Name) Then
                    Throw New ArgumentNullException("sentences", _
                                                   "One Sentence.Name is null or empty")
                End If
                Me.Add(newSentance)
                Me._definedSentencesList.Add(newSentance.Name)
            Next

        End Sub

        Default Public Property Item(ByVal sentenceName As String) As Sentence
            Get
                Return Me._sentenceDictionary(sentenceName)
            End Get
            Set(ByVal value As Sentence)
                If String.IsNullOrEmpty(value.Name) Then
                    Throw New ArgumentNullException("value.Name", _
                                                   "Sentence.Name is null or empty")
                End If
                If Not value.Name = sentenceName Then
                    Throw New ArgumentException("Sentence.Name and sentenceName does not match", _
                                              "sentenceName")
                ElseIf Not Me._sentenceDictionary.ContainsKey(sentenceName) Then
                    Me.Add(value)
                Else
                    Me._sentenceDictionary(sentenceName) = value
                End If
            End Set
        End Property

        ReadOnly Property DefinedSentences() As List(Of String)
            Get
                Return Me._definedSentencesList
            End Get
        End Property

        Public Sub Add(ByVal item As Sentence) Implements ICollection(Of Sentence).Add
            If String.IsNullOrEmpty(item.Name) Then
                Throw New ArgumentNullException("item.Name", _
                                               "Sentence.Name is null or empty")
            End If
            If Not Me._sentenceDictionary.ContainsKey(item.Name) Then
                item.Parent = Me
                Me._definedSentencesList.Add(item.Name)
                Me._sentenceDictionary.Add(item.Name, item)
            Else
                Throw New ArgumentException("Sentence is already defined in language", _
                                          "item")
            End If
        End Sub

        Public Sub Clear() Implements ICollection(Of Sentence).Clear
            Me._sentenceDictionary.Clear()
        End Sub

        Public Function Contains(ByVal item As Sentence) As Boolean Implements ICollection(Of Sentence).Contains
            Return Me._sentenceDictionary.ContainsKey(item.Name)
        End Function

        Public Sub CopyTo(ByVal array() As Sentence, ByVal arrayIndex As Integer) Implements ICollection(Of Sentence).CopyTo
            Throw New NotImplementedException("Will be implemented")
        End Sub

        Public ReadOnly Property Count() As Integer Implements ICollection(Of Sentence).Count
            Get
                Return Me._sentenceDictionary.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of Sentence).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(ByVal item As Sentence) As Boolean Implements ICollection(Of Sentence).Remove
            If String.IsNullOrEmpty(item.Name) Then
                Throw New ArgumentNullException("item.Name", _
                                               "Sentence.Name is null or empty")
            End If
            If Me._sentenceDictionary.ContainsKey(item.Name) Then
                Me._sentenceDictionary.Remove(item.Name)
                Me._definedSentencesList.Remove(item.Name)
                Return True
            End If

            Return False
        End Function

        Public Function GetEnumerator() As IEnumerator(Of Sentence) Implements IEnumerable(Of Sentence).GetEnumerator
            Return Me._sentenceDictionary.Values.GetEnumerator()
        End Function

        Private Function GetEnumeratorObject() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me._sentenceDictionary.GetEnumerator()
        End Function
    End Class
End Namespace
