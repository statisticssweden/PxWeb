Namespace PCAxis.Paxiom.Localization
   Public Class Sentence
      Implements ICloneable

      Private _sentenceName As String
      Private _sentenceValue As String
      Private _sentenceParent As Language

      ''' <summary>
      ''' Gets or sets the name of the sentence
      ''' </summary>
      ''' <value>The new name for sentence</value>
      ''' <returns>The name of the sentence</returns>
      ''' <remarks></remarks>
      Public Property Name() As String
         Get
            Return _sentenceName
         End Get
         Set(ByVal value As String)
            _sentenceName = value
         End Set
      End Property

      ''' <summary>
      ''' Gets or sets the value of the sentence
      ''' </summary>
      ''' <value>The new value for the sentence</value>
      ''' <returns>The value of the sentence</returns>
      ''' <remarks></remarks>
      Public Property Value() As String
         Get
            Return Me._sentenceValue
         End Get
         Set(ByVal value As String)

            Me._sentenceValue = value

         End Set
      End Property

      ''' <summary>
      ''' Used to mapp a sentence to its containing language
      ''' </summary>
      ''' <value>The language that is the sentences parent</value>
      ''' <returns>The sentences parent</returns>
      ''' <remarks></remarks>
      Friend Property Parent() As Language
         Get
            Return Me._sentenceParent
         End Get
         Set(ByVal value As Language)
            Me._sentenceParent = value
         End Set
      End Property

      ''' <summary>
      ''' Creates a new sentence with the supplied name and value
      ''' </summary>
      ''' <param name="name">Name of the sentence</param>
      ''' <param name="value">Value of the sentence</param>
      ''' <remarks></remarks>
      Public Sub New(ByVal name As String, ByVal value As String)
         Me.Name = name
         Me.Value = value
      End Sub

      ''' <summary>
      ''' Creates a new empty sentence
      ''' </summary>
      ''' <remarks></remarks>
      Public Sub New()
         Me.Name = String.Empty
         Me.Value = String.Empty
      End Sub

      Public Function Clone() As Object Implements ICloneable.Clone
         Return New Sentence(Me.Name, Me.Value)
      End Function
   End Class

End Namespace

