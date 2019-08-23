Imports PCAxis.Paxiom.ClassAttributes

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Base class for other operations
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class OperationsInfo

        Protected Friend mLanguageIndex As Integer = 0
        Protected m_id As String
        <LanguageDependent()> _
        Private _name As String()

        ''' <summary>
        ''' The name
        ''' </summary>
        ''' <value>The name</value>
        ''' <returns>The name</returns>
        ''' <remarks></remarks>
        Public Property Name() As String
            Get
                Return _name(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _name(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The identity of the operation
        ''' </summary>
        ''' <value>The identity of the operation</value>
        ''' <returns>The identity of the operation</returns>
        ''' <remarks></remarks>
        Public Property ID() As String
            Get
                Return m_id
            End Get
            Set(ByVal value As String)
                m_id = value
            End Set
        End Property

#Region "Language stuff"

        ''' <summary>
        ''' Resizes language secific values
        ''' </summary>
        ''' <param name="size">number of languages</param>
        ''' <remarks></remarks>
        Protected Friend Sub ResizeLanguageVariables(ByVal size As Integer)
            Util.ResizeLanguageDependentFields(Me, size)
        End Sub

        ''' <summary>
        ''' Sets the current language as the default language of the model.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Protected Friend Sub SetCurrentLanguageDefault()
            If mLanguageIndex > 0 Then
                'Switch text values between current language and the old default language
                Util.SwitchLanguages(Me, 0, mLanguageIndex)
            End If
        End Sub

        ''' <summary>
        ''' Sets the current language
        ''' </summary>
        ''' <param name="languageIndex"></param>
        ''' <remarks></remarks>
        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
        End Sub

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _name = New String(0) {}

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="id">identity of the operation</param>
        ''' <remarks></remarks>
        Sub New(ByVal id As String)
            m_id = id
            _name = New String(0) {}

        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="id">identity of the operation</param>
        ''' <param name="internalBufferSize">number of languages</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal id As String, ByVal internalBufferSize As Integer)
            Me._name = New String(internalBufferSize) {}
            m_id = id
        End Sub

#End Region

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As OperationsInfo
            Dim newObject As OperationsInfo

            newObject = CType(Me.MemberwiseClone(), OperationsInfo)

            ' Handle reference types
            newObject._name = New String(Me._name.Count - 1) {}
            For i As Integer = 0 To Me._name.Count - 1
                newObject._name(i) = Me._name(i)
            Next

            Return newObject
        End Function
    End Class

End Namespace