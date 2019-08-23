Imports PCAxis.Paxiom.ClassAttributes

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Holds the attributes on cell level for the table
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Attributes

#Region "Private fields"
        Protected Friend mLanguageIndex As Integer = 0
        Private _identities As List(Of String) = New List(Of String)
        <LanguageDependent()> _
        Private _names(0) As List(Of String)
        Private _defaultAttributes(0) As String
        Private _cellAttributes As New Dictionary(Of VariableValuePairs, String())(New VariableValuePairs.EqualityComparer())
#End Region

#Region "Public properties"
        ''' <summary>
        ''' List of attribute identities
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Identities() As List(Of String)
            Get
                Return _identities
            End Get
        End Property

        ''' <summary>
        ''' List of attributes names
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Names() As List(Of String)
            Get
                Return _names(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' Dictionary containing all cell attributes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CellAttributes() As Dictionary(Of VariableValuePairs, String())
            Get
                Return _cellAttributes
            End Get
        End Property
#End Region

#Region "Public methods"
        ''' <summary>
        ''' Create a deep copy of the Attributes object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Attributes
            Dim newObject As Attributes

            newObject = CType(Me.MemberwiseClone(), Attributes)

            ' Handle reference types

            ' Handle identities
            newObject._identities = Nothing
            If Me._identities IsNot Nothing Then
                newObject._identities = New List(Of String)
                For Each identity As String In Me._identities
                    newObject._identities.Add(identity)
                Next
            End If

            ' Handle names
            newObject._names = Nothing
            If Me._names IsNot Nothing Then
                newObject._names = New List(Of String)(Me._names.Count - 1) {}
                For i As Integer = 0 To Me._names.Count - 1
                    newObject._names(i) = New List(Of String)
                    If Me._names(i) IsNot Nothing Then
                        For Each name As String In Me._names(i)
                            newObject._names(i).Add(name)
                        Next
                    End If
                Next
            End If

            ' Handle default attributes
            newObject._defaultAttributes = Nothing
            If Me._defaultAttributes IsNot Nothing Then
                newObject._defaultAttributes = New String(Me._defaultAttributes.Count - 1) {}
                For i As Integer = 0 To Me._defaultAttributes.Count - 1
                    newObject._defaultAttributes(i) = Me._defaultAttributes(i)
                Next
            End If

            ' Handle cell attributes
            newObject._cellAttributes = Nothing
            If Me._cellAttributes IsNot Nothing Then
                newObject._cellAttributes = New Dictionary(Of VariableValuePairs, String())(New VariableValuePairs.EqualityComparer())
                For Each kvp As KeyValuePair(Of VariableValuePairs, String()) In Me._cellAttributes
                    Dim key As VariableValuePairs = kvp.Key.CreateCopy()
                    Dim val As String() = New String(kvp.Value.Count - 1) {}
                    For i As Integer = 0 To kvp.Value.Count - 1
                        val(i) = kvp.Value(i)
                    Next
                    newObject._cellAttributes.Add(key, val)
                Next
            End If

            Return newObject
        End Function

        ''' <summary>
        ''' Add attribute name
        ''' </summary>
        ''' <param name="name">Attribute name</param>
        ''' <remarks></remarks>
        Public Sub AddName(ByVal name As String)
            If _names(mLanguageIndex) Is Nothing Then
                _names(mLanguageIndex) = New List(Of String)
            End If

            _names(mLanguageIndex).Add(name)
        End Sub

        ''' <summary>
        ''' Set the default attributes
        ''' </summary>
        ''' <param name="attributeValues">Collection of default attribute values</param>
        ''' <returns>True if the operation completed successfully else false</returns>
        ''' <remarks>
        ''' Attribute identities must have been added before this call. 
        ''' The number of default attribute values must equal the number of attribute identities
        ''' </remarks>
        Public Function SetDefaultAttributes(ByVal attributeValues As System.Collections.Specialized.StringCollection) As Boolean
            If (_identities.Count = 0) Or (attributeValues.Count = 0) Or (_identities.Count <> attributeValues.Count) Then
                Return False
            End If

            ReDim _defaultAttributes(_identities.Count - 1)

            For i As Integer = 0 To _identities.Count - 1
                _defaultAttributes(i) = attributeValues.Item(i)
            Next

            Return True
        End Function

        ''' <summary>
        ''' Returns list with key-value pairs for the default attributes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefaultAttributes() As List(Of KeyValuePair(Of String, String))
            Dim lst As New List(Of KeyValuePair(Of String, String))
            Dim attr As KeyValuePair(Of String, String)

            For i As Integer = 0 To _identities.Count - 1
                attr = New KeyValuePair(Of String, String)(_names(mLanguageIndex)(i), _defaultAttributes(i))
                lst.Add(attr)
            Next

            Return lst
        End Function

#End Region

#Region "Language stuff"
        ''' <summary>
        ''' Set current language
        ''' </summary>
        ''' <param name="languageIndex">Index of the selected language</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
        End Sub

        ''' <summary>
        ''' Change the number of available languages
        ''' </summary>
        ''' <param name="size">Number of languages</param>
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

#End Region

    End Class

End Namespace
