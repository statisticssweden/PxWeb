Imports PCAxis.Paxiom.ClassAttributes
Imports System.Collections.Specialized

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Hirearchy
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Hierarchy
        Implements System.Runtime.Serialization.ISerializable

        Protected Friend mLanguageIndex As Integer = 0

#Region "Private fields"
        Private _openLevel As Integer
        Private _levels As Integer = -1
        <LanguageDependent()> _
        Private _names() As StringCollection
        Private _rootLevel As HierarchyLevel
#End Region

#Region "Public properties"
        ''' <summary>
        ''' Number of hierarchies that should be opened by default
        ''' </summary>
        ''' <value>Number of hierarchies that should be opened by default</value>
        ''' <returns>The number of hierarchies that should be opened by default</returns>
        ''' <remarks></remarks>
        Public Property OpenLevel() As Integer
            Get
                Return _openLevel
            End Get
            Set(ByVal value As Integer)
                _openLevel = value
            End Set
        End Property

        ''' <summary>
        ''' The number of levels existing for a symmetrical tree
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Has the default value of -1 if the property is not set</remarks>
        Public Property Levels() As Integer
            Get
                Return _levels
            End Get
            Set(ByVal value As Integer)
                _levels = value
            End Set
        End Property

        ''' <summary>
        ''' Collection of names for the levels in a symmetrical tree
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Names() As StringCollection
            Get
                Return Me._names(mLanguageIndex)
            End Get
            Set(ByVal value As StringCollection)
                Me._names(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Root level of the hierarchy
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RootLevel() As HierarchyLevel
            Get
                Return _rootLevel
            End Get
            Set(ByVal value As HierarchyLevel)
                _rootLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Check i there is a hierarchy
        ''' </summary>
        ''' <value>Check i there is a hierarchy</value>
        ''' <returns>True if there is hierarchylevels defined</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsHierarchy() As Boolean
            Get
                If Me._rootLevel Is Nothing Then
                    Return False
                End If
                Return True
            End Get
        End Property

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._names = New StringCollection(0) {}
            Me._names(0) = New StringCollection
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="internalBufferSize">Number of language the Hierarchy should be dimensioned for</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal internalBufferSize As Integer)
            Me._names = New StringCollection(internalBufferSize) {}
            For i As Integer = 0 To internalBufferSize
                Me._names(i) = New StringCollection
            Next
        End Sub
#End Region

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            _openLevel = info.GetInt32("OpenLevel")
            _levels = info.GetInt32("Levels")
            _names = CType(info.GetValue("Names", GetType(StringCollection())), StringCollection())
            _rootLevel = CType(info.GetValue("RootLevel", GetType(HierarchyLevel)), HierarchyLevel)
        End Sub

        ''' <summary>
        ''' Add object data to SerializationInfo
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("OpenLevel", _openLevel)
            info.AddValue("Levels", _levels)
            info.AddValue("Names", _names)
            info.AddValue("RootLevel", _rootLevel)
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns>A deep clone of it self.</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Hierarchy
            Dim newObject As Hierarchy

            newObject = CType(Me.MemberwiseClone(), Hierarchy)

            newObject._names = Nothing
            If Me._names IsNot Nothing Then
                newObject._names = New StringCollection(Me._names.Count - 1) {}
                For i As Integer = 0 To Me._names.Count - 1
                    newObject._names(i) = New StringCollection
                    If Me._names(i) IsNot Nothing Then
                        For Each name As String In Me._names(i)
                            newObject._names(i).Add(name)
                        Next
                    End If
                Next
            End If

            newObject._rootLevel = Nothing
            If Me._rootLevel IsNot Nothing Then
                newObject._rootLevel = Me._rootLevel.CreateCopy()
            End If

            Return newObject
        End Function

        ''' <summary>
        ''' Clears the hierarchy
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear()
            _openLevel = 0
            _levels = -1
            _rootLevel = Nothing
            For i As Integer = 0 To _names.Count - 1
                _names(i).Clear()
            Next
        End Sub

#Region "Language stuff"

        Protected Friend Sub ResizeLanguageVariables(ByVal size As Integer)
            Util.ResizeLanguageDependentFields(Me, size)
        End Sub

        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
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