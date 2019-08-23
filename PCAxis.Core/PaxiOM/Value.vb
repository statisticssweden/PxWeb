Imports PCAxis.Paxiom.ClassAttributes

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Value class
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Value
        Implements System.Runtime.Serialization.ISerializable

#Region "Private fields"
        Protected Friend mLanguageIndex As Integer = 0
        Private _variable As Variable
        <LanguageDependent()> _
        Private mValue() As String
        Private mCode As String = Nothing
        Private mPrecision As Integer = -1
        'Private mNotes As NoteCollection = Nothing
        <LanguageDependent()> _
        Private mNotes() As Notes
        Private mContentInfo As ContInfo
        Private mTimeValue As String

        <LanguageDependent()> _
        Private _datanote(0) As String
        <LanguageDependent()> _
        Private _metaId(0) As String

#End Region

#Region "Language stuff"

        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
            If mContentInfo IsNot Nothing Then
                mContentInfo.SetLanguage(languageIndex)
            End If
        End Sub

        Protected Friend Sub ResizeLanguageVariables(ByVal size As Integer)
            Util.ResizeLanguageDependentFields(Me, size)

            If mContentInfo IsNot Nothing Then
                mContentInfo.ResizeLanguageVariables(size)
            End If
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

                If mContentInfo IsNot Nothing Then
                    mContentInfo.SetCurrentLanguageDefault()
                End If
            End If
        End Sub
#End Region

        ''' <summary>
        ''' Set which variable the value belongs to
        ''' </summary>
        ''' <param name="variable">The variable</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetVariable(ByVal variable As Variable)
            Me._variable = variable
        End Sub

#Region "Public Properties"
        ''' <summary>
        ''' The variable that the value belongs to
        ''' </summary>
        ''' <value>The variable that the value belongs to</value>
        ''' <returns>The variable that the value belongs to</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Variable() As Variable
            Get
                Return _variable
            End Get
        End Property

        ''' <summary>
        ''' The name/value of the value
        ''' </summary>
        ''' <value>The name/value of the value</value>
        ''' <returns>The name/value of the value</returns>
        ''' <remarks></remarks>
        Public Property Value() As String
            Get
                Return Me.mValue(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me.mValue(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TimeValue() As String
            Get
                Return mTimeValue
            End Get
            Set(ByVal value As String)
                mTimeValue = value
            End Set
        End Property

        ''' <summary>
        ''' The code of the value
        ''' </summary>
        ''' <value>The code of the value</value>
        ''' <returns>The code of the value</returns>
        ''' <remarks>The code could be fictional and should then not be presisted</remarks>
        Public ReadOnly Property Code() As String
            Get
                Return Me.mCode
            End Get
        End Property

        ''' <summary>
        ''' The precision
        ''' </summary>
        ''' <value>The precision</value>
        ''' <returns>The precision</returns>
        ''' <remarks></remarks>
        Public Property Precision() As Integer
            Get
                If Me.mPrecision < 0 Then
                    Return Nothing
                End If
                Return Me.mPrecision
            End Get
            Set(ByVal value As Integer)
                If value > -1 And value < 7 Then
                    Me.mPrecision = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' List of valuenotes
        ''' </summary>
        ''' <value>List of valuenotes</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Notes() As Notes
            Get
                If mNotes Is Nothing Then
                    Return Nothing
                End If
                Return Me.mNotes(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' Content info
        ''' </summary>
        ''' <value>the content info</value>
        ''' <returns>the content info</returns>
        ''' <remarks>This is only set when the values belongs to a content variable</remarks>
        Public Property ContentInfo() As ContInfo
            Get
                Return Me.mContentInfo
            End Get
            Set(ByVal value As ContInfo)
                Me.mContentInfo = value
            End Set
        End Property


        ''' <summary>
        ''' Returns Code, Value or combination of Code and Value depending on PresentationText (PRESTEXT)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Text() As String
            Get
                If Me._variable Is Nothing Then
                    Return Value
                Else
                    Select Case Me._variable.PresentationText
                        Case 0
                            Return Me.Code
                        Case 1
                            Return Me.Value
                        Case 2
                            Return Me.Code & " " & Me.Value
                        Case 3
                            Return Me.Value & " " & Me.Code
                    End Select
                End If
                Return Me.Value
            End Get

        End Property

        ''' <summary>
        ''' Returns Code and Value.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>If codes are fictional only Value is returned</remarks>
        Public ReadOnly Property CodeAndValue() As String
            Get
                If Me._variable Is Nothing Then
                    Return Me.Value
                Else
                    If Me.IsFictional Then
                        Return Me.Value
                    Else
                        Return Me.Code & " " & Me.Value
                    End If
                End If
            End Get
        End Property

        ''' <summary>
        ''' Datanote is used to indicate that a note exist for the value
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Datanote() As String
            Get
                Return _datanote(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._datanote(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' A Metadata Id for the value
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MetaId() As String
            Get
                Return _metaId(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._metaId(mLanguageIndex) = value
            End Set
        End Property
#End Region

#Region "Has-functions"

        ''' <summary>
        ''' Checks to see if a precision has been specified
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasPrecision() As Boolean
            If Me.mPrecision < 1 Then
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Checks to see if the value have any notes.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasNotes() As Boolean
            If Me.mNotes Is Nothing Then
                Return False
            End If

            If Me.mNotes(mLanguageIndex) Is Nothing Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Checks to se if the value have any content info
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasContentInfo() As Boolean
            If Me.mContentInfo Is Nothing Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Checks is the value have any code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasCode() As Boolean
            If Me.mCode Is Nothing Then
                Return False
            End If

            Return True
        End Function
#End Region

#Region "Constructor"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.mValue = New String(0) {}
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="value">value name</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal value As String)
            Me.mValue = New String(0) {}
            Me.mValue(mLanguageIndex) = value
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="value">value name</param>
        ''' <param name="internalBufferSize">
        ''' a size of how many languages that should be allocated for
        ''' </param>
        ''' <remarks></remarks>
        Public Sub New(ByVal value As String, ByVal internalBufferSize As Integer)
            Me.mValue = New String(internalBufferSize) {}
            For i As Integer = 0 To internalBufferSize
                Me.mValue(i) = value
            Next
            Me._metaId = New String(internalBufferSize) {}
            Me._datanote = New String(internalBufferSize) {}
            'Me.mValue(mLanguageIndex) = value
        End Sub

#End Region

#Region "Set functions"

        ''' <summary>
        ''' sets the code
        ''' </summary>
        ''' <param name="code"></param>
        ''' <remarks></remarks>
        Protected Friend Sub SetCode(ByVal code As String)
            Me.mCode = code
        End Sub

        <Obsolete()> _
        Protected Friend Sub SetPrecision(ByVal p As Integer)
            If p > -1 And p < 7 Then
                Me.mPrecision = p
            End If
        End Sub

        ''' <summary>
        ''' Adds a note to the value
        ''' </summary>
        ''' <param name="note">note to add to the Notes list of the value</param>
        ''' <remarks>
        ''' One should always use this function to add notes since this 
        ''' function reassures that the Notes list is created.
        ''' </remarks>
        Public Sub AddNote(ByVal note As Note)
            If Me.mNotes Is Nothing Then
                'Me.mNotes = New NoteCollection
                Me.mNotes = New Notes(mValue.Length - 1) {}
            End If

            If mNotes(mLanguageIndex) Is Nothing Then
                mNotes(mLanguageIndex) = New Notes
            End If

            Me.mNotes(mLanguageIndex).Add(note)
        End Sub

        'TODO PETROS CHECK that the variable is a content variable
        ''' <summary>
        ''' sets the content info
        ''' </summary>
        ''' <param name="name">name of the property in contentinfo</param>
        ''' <param name="value">the value</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetContentInfo(ByVal name As String, ByVal value As String)
            If Me.mContentInfo Is Nothing Then
                Me.mContentInfo = New ContInfo(mValue.Length - 1)
            End If
            Me.mContentInfo.SetProperty(name, value)
        End Sub

#End Region

#Region "Overrides"
        Public Overrides Function ToString() As String
            'Return Me.mValue(mLanguageIndex)
            Return Me.Text
        End Function
#End Region

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Value", mValue)
            info.AddValue("Code", mCode)
            info.AddValue("Precision", mPrecision)
            'info.AddValue("Selected", mSelected)
            info.AddValue("ContentInfo", mContentInfo)
            info.AddValue("Notes", mNotes)
        End Sub

        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            mValue = CType(info.GetValue("Value", GetType(String())), String())
            mCode = info.GetString("Code")
            mPrecision = info.GetInt32("Precision")
            'mSelected = info.GetBoolean("Selected")
            mContentInfo = CType(info.GetValue("ContentInfo", GetType(ContInfo)), ContInfo)
            mNotes = CType(info.GetValue("Notes", GetType(Notes())), Notes())
        End Sub

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Value
            Dim newObject As Value
            newObject = CType(Me.MemberwiseClone(), Value)

            newObject.SetVariable(Nothing)

            ' Handle reference types
            newObject.mValue = Nothing
            If Me.mValue IsNot Nothing Then
                newObject.mValue = New String(Me.mValue.Count - 1) {}
                For i As Integer = 0 To Me.mValue.Count - 1
                    newObject.mValue(i) = Me.mValue(i)
                Next
            End If

            newObject.mNotes = Nothing
            If Me.mNotes IsNot Nothing Then
                newObject.mNotes = New Notes(Me.mNotes.Count - 1) {}
                For i As Integer = 0 To Me.mNotes.Count - 1
                    newObject.mNotes(i) = Me.mNotes(i)
                Next
            End If

            newObject.ContentInfo = Nothing
            If Me.ContentInfo IsNot Nothing Then
                newObject.ContentInfo = Me.ContentInfo.CreateCopy()
            End If

            Return newObject
        End Function

        ''' <summary>
        ''' Checks if the code is fictional
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' The value must belong to a Values list otherwise this 
        ''' function assumes that the values are fictional
        ''' </remarks>
        Public Function IsFictional() As Boolean
            If Me.Variable IsNot Nothing Then
                Return Me.Variable.Values.IsCodesFictional
            End If
            'Assume fictional if the value doesn´t belong to a variable
            Return True
        End Function

    End Class

End Namespace
