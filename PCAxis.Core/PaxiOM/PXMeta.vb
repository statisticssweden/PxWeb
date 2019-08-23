Imports System.Xml
Imports System.IO
Imports System.Reflection
Imports PCAxis.Paxiom.Localization
Imports PCAxis.Paxiom.ClassAttributes
'Imports System.Windows.Forms

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' The metadata for a statistical cube.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class PXMeta
        Implements System.Runtime.Serialization.ISerializable



#Region "Private fields"

        Protected Friend mLanguageIndex As Integer = 0

        Private _model As PXModel
        Private _charset As String
        Private _matrix As String
        Private _subjectCode As String
        <LanguageDependent()> _
        Private _subjectArea(0) As String
        <LanguageDependent()> _
        Private _contents(0) As String
        Private _decimals As Integer = -1
        <LanguageDependent()> _
        Private _title(0) As String
        <LanguageDependent()> _
        Private _description(0) As String
        Private _language As String
        Private _axisVersion As String
        Private _aggregAllowed As Boolean = True
        Private _autoOpen As Boolean = False
        Private _descriptionDefault As Boolean = False
        Private _codePage As String
        Private _creationDate As String
        Private _copyright As Boolean
        Private _showDecimals As Integer = -1
        <LanguageDependent()> _
        Private _source(0) As String
        Private _confidential As Integer = 0
        <LanguageDependent()> _
        Private _database(0) As String
        <LanguageDependent()> _
        Private _infoFile(0) As String
        Private _contentInfo As ContInfo
        Private _updateFrequency As String
        Private _nextUpdate As String
        Private _PXServer As String
        Private _directoryPath As String
        <LanguageDependent()> _
        Private _information(0) As String
        <LanguageDependent()> _
        Private _link(0) As String
        <LanguageDependent()> _
        Private _survey(0) As String
        Private _tableID As String
        Private _defaultGraph As Integer = Integer.MinValue
        <LanguageDependent()> _
        Private _dataNoteSum(0) As String
        <LanguageDependent()> _
        Private _notes(0) As Notes
        Private _variables As Variables = New Variables
        Private _heading As Variables = New Variables
        Private _stub As Variables = New Variables
        <LanguageDependent()> _
        Private _cellNotes(0) As CellNotes
        <LanguageDependent()> _
        Private _dataNoteCells(0) As DataNoteCells
        Private _contentVariable As Variable = Nothing
        Private _rounding As RoundingType = RoundingType.None
        Private _synonyms As String
        Private _currentLanguage As String

        <LanguageDependent()> _
        Private _dataSymbol1(0) As String
        <LanguageDependent()> _
        Private _dataSymbol2(0) As String
        <LanguageDependent()> _
        Private _dataSymbol3(0) As String
        <LanguageDependent()> _
        Private _dataSymbol4(0) As String
        <LanguageDependent()> _
        Private _dataSymbol5(0) As String
        <LanguageDependent()> _
        Private _dataSymbol6(0) As String
        <LanguageDependent()> _
        Private _dataSymbol7(0) As String
        <LanguageDependent()> _
        Private _dataSymbolNIL(0) As String
        <LanguageDependent()> _
        Private _dataSymbolSum(0) As String

        Private _mainTable As String

        Private _extendedProperties As New ExtendedPropertiesList

        Private mLanguages As New Dictionary(Of String, Integer)
        Private _officialStatistics As Boolean = False
        Private _attributes As New Attributes

        Private _preferredLanguage As String = Nothing

        Private _firstPublished As String
        <LanguageDependent()> _
        Private _datanote(0) As String
        <LanguageDependent()> _
        Private _metaId(0) As String

#End Region

#Region "Public properties"
        ''' <summary>
        ''' The model that the metadata belongs to
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Model() As PXModel
            Get
                Return _model
            End Get
        End Property

        ''' <summary>
        '''   preferred Charset
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Charset() As String
            Get
                Return Me._charset
            End Get
            Set(ByVal value As String)
                Me._charset = value
            End Set
        End Property

        ''' <summary>
        ''' The Matrix name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Matrix() As String
            Get
                Return Me._matrix
            End Get
            Set(ByVal value As String)
                Me._matrix = value
            End Set
        End Property

        ''' <summary>
        ''' The subject code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SubjectCode() As String
            Get
                Return Me._subjectCode
            End Get
            Set(ByVal value As String)
                Me._subjectCode = value
            End Set
        End Property

        ''' <summary>
        ''' The subject area
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SubjectArea() As String
            Get
                Return Me._subjectArea(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._subjectArea(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The contents
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contents() As String
            Get
                Return Me._contents(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._contents(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The number of decimals
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Decimals() As Integer
            Get
                Return Me._decimals
            End Get
            Set(ByVal value As Integer)
                Me._decimals = value
            End Set
        End Property

        ''' <summary>
        ''' The title of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Title() As String
            Get
                Return Me._title(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._title(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The description of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description() As String
            Get
                Return Me._description(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._description(mLanguageIndex) = value
            End Set
        End Property


        ''' <summary>
        ''' The PC-Axis version
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AxisVersion() As String
            Get
                Return Me._axisVersion
            End Get
            Set(ByVal value As String)
                Me._axisVersion = value
            End Set
        End Property

        ''' <summary>
        ''' If aggregations are allowed do be done to the data in the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AggregAllowed() As Boolean
            Get
                Return Me._aggregAllowed
            End Get
            Set(ByVal value As Boolean)
                Me._aggregAllowed = value
            End Set
        End Property

        ''' <summary>
        ''' If the cube should automaticlly be opend
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AutoOpen() As Boolean
            Get
                Return Me._autoOpen
            End Get
            Set(ByVal value As Boolean)
                Me._autoOpen = value
            End Set
        End Property

        ''' <summary>
        ''' The description defalut
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DescriptionDefault() As Boolean
            Get
                Return Me._descriptionDefault
            End Get
            Set(ByVal value As Boolean)
                Me._descriptionDefault = value
            End Set
        End Property

        ''' <summary>
        ''' The preferred codepage to use when storing the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodePage() As String
            Get
                Return Me._codePage
            End Get
            Set(ByVal value As String)
                Me._codePage = value
            End Set
        End Property

        ''' <summary>
        ''' Creation date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CreationDate() As String
            Get
                Return Me._creationDate
            End Get
            Set(ByVal value As String)
                Me._creationDate = value
            End Set
        End Property

        ''' <summary>
        ''' Preferred number of decimals that should be used 
        ''' when displaying the measurs of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShowDecimals() As Integer
            Get
                Return Me._showDecimals
            End Get
            Set(ByVal value As Integer)
                Me._showDecimals = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Copyright() As Boolean
            Get
                Return Me._copyright
            End Get
            Set(ByVal value As Boolean)
                Me._copyright = value
            End Set
        End Property

        ''' <summary>
        ''' Source of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source() As String
            Get
                Return Me._source(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._source(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Confidential() As Integer
            Get
                Return Me._confidential
            End Get
            Set(ByVal value As Integer)
                Me._confidential = value
            End Set
        End Property

        ''' <summary>
        ''' The database
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Database() As String
            Get
                Return Me._database(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._database(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Aditional information file location
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InfoFile() As String
            Get
                Return Me._infoFile(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._infoFile(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Variables that are placed in the stub of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Stub() As Variables
            Get
                Return Me._stub
            End Get
        End Property


        ''' <summary>
        ''' List of variables that are placed in the heading of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Heading() As Variables
            Get
                Return Me._heading
            End Get
        End Property

        ''' <summary>
        ''' A list of all variables of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Variables() As Variables
            Get
                Return Me._variables
            End Get
        End Property

        ''' <summary>
        ''' Content information of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContentInfo() As ContInfo
            Get
                Return Me._contentInfo
            End Get
            Set(ByVal value As ContInfo)
                Me._contentInfo = value
            End Set
        End Property

        ''' <summary>
        ''' Notes for the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Notes() As Notes
            Get
                If Me._notes(mLanguageIndex) Is Nothing Then
                    Me._notes(mLanguageIndex) = New Notes
                End If
                Return Me._notes(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' Notes for individual measures
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CellNotes() As CellNotes
            Get
                If Me._cellNotes(mLanguageIndex) Is Nothing Then
                    Me._cellNotes(mLanguageIndex) = New CellNotes
                End If
                Return Me._cellNotes(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' Data notes of individual measures that will be displayed when 
        ''' the measures are displayed
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataNoteCells() As DataNoteCells
            Get
                If Me._dataNoteCells(mLanguageIndex) Is Nothing Then
                    Me._dataNoteCells(mLanguageIndex) = New DataNoteCells
                End If
                Return Me._dataNoteCells(mLanguageIndex)
            End Get
        End Property

        'TODO Serialization fix
        ''' <summary>
        ''' The content variable of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContentVariable() As Variable
            Get
                Return Me._contentVariable
            End Get
            Set(ByVal value As Variable)
                Me._contentVariable = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol1() As String
            Get
                'If _dataSymbol1(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(1)
                'End If
                Return _dataSymbol1(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol1(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol2() As String
            Get
                'If _dataSymbol2(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(2)
                'End If
                Return _dataSymbol2(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol2(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 3
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol3() As String
            Get
                'If _dataSymbol3(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(3)
                'End If
                Return _dataSymbol3(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol3(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 4
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol4() As String
            Get
                'If _dataSymbol4(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(4)
                'End If
                Return _dataSymbol4(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol4(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 5
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol5() As String
            Get
                'If _dataSymbol5(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(5)
                'End If
                Return _dataSymbol5(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol5(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 6
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol6() As String
            Get
                'If _dataSymbol6(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(6)
                'End If
                Return _dataSymbol6(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol6(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NPM 7
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbol7() As String
            Get
                'If _dataSymbol7(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.Symbol(7)
                'End If
                Return _dataSymbol7(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbol7(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for NIL values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbolNIL() As String
            Get
                'If _dataSymbolNIL(mLanguageIndex) Is Nothing Then
                '    Return Settings.DataSymbols.SymbolNIL
                'End If
                Return _dataSymbolNIL(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbolNIL(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The data symbols to use for sum NPM values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSymbolSum() As String
            Get
                Return _dataSymbolSum(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._dataSymbolSum(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' The cubes updated frequency
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpdateFrequency() As String
            Get
                Return _updateFrequency
            End Get
            Set(ByVal value As String)
                _updateFrequency = value
            End Set
        End Property

        ''' <summary>
        ''' Next update 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NextUpdate() As String
            Get
                Return _nextUpdate
            End Get
            Set(ByVal value As String)
                _nextUpdate = value
            End Set
        End Property


        ''' <summary>
        ''' PXServer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PXServer() As String
            Get
                Return _PXServer
            End Get
            Set(ByVal value As String)
                _PXServer = value
            End Set
        End Property

        ''' <summary>
        ''' Directory path
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DirectoryPath() As String
            Get
                Return _directoryPath
            End Get
            Set(ByVal value As String)
                _directoryPath = value
            End Set
        End Property

        ''' <summary>
        ''' Extra information
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Information() As String
            Get
                Return _information(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _information(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Link to more infromation
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Link() As String
            Get
                Return _link(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _link(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Survey
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Survey() As String
            Get
                Return _survey(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _survey(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Table ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TableID() As String
            Get
                Return _tableID
            End Get
            Set(ByVal value As String)
                _tableID = value
            End Set
        End Property

        ''' <summary>
        ''' Default graph
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultGraph() As Integer
            Get
                Return _defaultGraph
            End Get
            Set(ByVal value As Integer)
                If value > 0 And value < 11 Then
                    _defaultGraph = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Data note sum
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataNoteSum() As String
            Get
                Return _dataNoteSum(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                _dataNoteSum(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Which rounding rule that should be applied when doing calculations on the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rounding() As RoundingType
            Get
                Return _rounding
            End Get
            Set(ByVal value As RoundingType)
                _rounding = value
            End Set
        End Property

        ''' <summary>
        ''' Extended properties of the cube
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExtendedProperties() As ExtendedPropertiesList
            Get
                Return _extendedProperties
            End Get
        End Property

        ''' <summary>
        ''' Synomnyms 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Synonyms() As String
            Get
                Return _synonyms
            End Get
            Set(ByVal value As String)
                _synonyms = value
            End Set
        End Property

        ''' <summary>
        ''' Identifies a SQL table
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MainTable() As String
            Get
                Return _mainTable
            End Get
            Set(ByVal value As String)
                _mainTable = value
            End Set
        End Property

        ''' <summary>
        ''' If data is official statistics or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OfficialStatistics() As Boolean
            Get
                Return _officialStatistics
            End Get
            Set(ByVal value As Boolean)
                _officialStatistics = value
            End Set
        End Property

        ''' <summary>
        ''' The attributes at cell level 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributes() As Attributes
            Get
                Return _attributes
            End Get
        End Property

        ''' <summary>
        ''' The date when the data cube was first published in the format CCYYMMDD hh:mm.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirstPublished() As String
            Get
                Return _firstPublished
            End Get
            Set(ByVal value As String)
                _firstPublished = value
            End Set
        End Property

        ''' <summary>
        ''' Datanote is used to indicate that a note exist for the table
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
        ''' A Metadata Id for the table
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

#Region "Language stuff"
        ''' <summary>
        ''' The default language
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal value As String)
                Me._language = value
                Me._preferredLanguage = value
                Dim key As String = value.ToLower()
                If mLanguages.ContainsKey(key) Then
                    mLanguages(key) = 0
                Else
                    'DefaultLaguage hav alway pos 0 in the Language matix
                    mLanguages.Add(key, 0)
                End If
            End Set
        End Property

        ''' <summary>
        ''' The current language
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentLanguage() As String
            Get
                If _currentLanguage Is Nothing Then
                    _currentLanguage = _language
                End If

                Return _currentLanguage
            End Get
        End Property

        ''' <summary>
        ''' Language index of the currently selected language
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentLanguageIndex() As Integer
            Get
                Return mLanguageIndex
            End Get
        End Property

        ''' <summary>
        ''' Number of available languages
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumberOfLanguages() As Integer
            Get
                Return _LanguageCount
            End Get
        End Property

        ''' <summary>
        ''' Changes the current language
        ''' </summary>
        ''' <param name="language">name of the language</param>
        ''' <remarks>
        ''' If the language do not exist then the language will 
        ''' be set to the default language
        ''' </remarks>
        Public Sub SetLanguage(ByVal language As String)
            Dim index As Integer = GetLanguageIndex(language)
            SetLanguage(index)
        End Sub

        ''' <summary>
        ''' Set the language by the language index
        ''' </summary>
        ''' <param name="languageIndex"></param>
        ''' <remarks></remarks>
        Public Sub SetLanguage(ByVal languageIndex As Integer)
            If languageIndex >= _LanguageCount Then
                Throw New IndexOutOfRangeException("Language do not exisist")
            End If

            If mLanguageIndex <> languageIndex Then
                mLanguageIndex = languageIndex

                For i As Integer = 0 To _variables.Count - 1
                    _variables(i).SetLanguage(languageIndex)
                Next

                If Me._contentInfo IsNot Nothing Then
                    Me._contentInfo.SetLanguage(languageIndex)
                End If

                If Me._attributes IsNot Nothing Then
                    Me._attributes.SetLanguage(languageIndex)
                End If

                'Set CurrentLanguage
                For Each kvp As KeyValuePair(Of String, Integer) In mLanguages
                    If Not kvp.Key.Equals("default") Then
                        If languageIndex = kvp.Value Then
                            _currentLanguage = kvp.Key
                            Exit For
                        End If
                    End If
                Next
            End If
        End Sub


        '''<summary>
        '''Gets the index of the language specified.
        '''</summary>
        '''<returns>
        '''The index of the specified language. If the language dosn't exisit 
        '''the index of the default language will be returned.
        '''</returns>
        Public Function GetLanguageIndex(ByVal language As String) As Integer
            If language Is Nothing Then
                Return 0
            End If
            Dim lang As String = language.ToLower()

            If mLanguages.ContainsKey(lang) Then
                Return mLanguages(lang)
            End If

            'Return the default language if the language dosen't exist
            Return 0
        End Function

        ''' <summary>
        ''' returns all available languages for the cube
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllLanguages() As String()

            'Test if multilingual
            If _LanguageCount = 1 Then
                Return Nothing
            End If

            Dim langs(_LanguageCount - 1) As String

            Dim i As Integer = 0
            Dim lang As String = _language.ToLower()
            For Each key As String In Me.mLanguages.Keys
                'Remove the default language
                If key <> "default" Then
                    langs(i) = key
                    i += 1
                End If
            Next
            Return langs
        End Function

        '''<summary>
        '''Checks if the specified language exisit in the model
        '''</summary>
        '''<returns>
        '''Returns True if the specified language exist otherwise it returns False.
        '''</returns>
        Public Function HasLanguage(ByVal language As String) As Boolean
            If language Is Nothing Then
                Return True
            End If
            Return mLanguages.ContainsKey(language.ToLower())
        End Function

        Private _LanguageCount As Integer = 1

        '''<summary>
        '''Adds a language to the model
        '''</summary>
        '''<returns>
        '''Returns the index of the added language.
        '''</returns>
        '''<remark>
        '''If the language already exisit in the model then the existing index will be returned
        '''</remark>
        Public Function AddLanguage(ByVal language As String) As Integer
            Dim lang As String = language.ToLower()
            If mLanguages.ContainsKey(lang) Then
                Return mLanguages(lang)
            End If

            mLanguages.Add(lang, _LanguageCount)
            _LanguageCount += 1
            ResizeLanguageVariables()
            Return mLanguages(lang)
        End Function

        '''<summary>
        '''Adds languages to the model
        '''</summary>
        '''<returns>
        '''Returns the number of languages added
        '''</returns>
        Public Function AddLanguages(ByVal languages() As String) As Integer
            Dim lang As String
            Dim newLangCount As Integer = 0

            For i As Integer = 0 To languages.Length - 1
                lang = languages(i).ToLower()
                If Not mLanguages.ContainsKey(lang) Then
                    'Add the new language
                    mLanguages.Add(lang, _LanguageCount)
                    _LanguageCount += 1

                    newLangCount += 1
                End If
            Next

            'if any laguages has been added resize the arrays
            If newLangCount <> 0 Then
                ResizeLanguageVariables()
            End If

            Return newLangCount
        End Function

        'Resizes the language dependent variables internal arrays
        Protected Sub ResizeLanguageVariables()
            Util.ResizeLanguageDependentFields(Me, _LanguageCount)

            For i As Integer = 0 To _variables.Count - 1
                _variables(i).ResizeLanguageVariables(_LanguageCount)
            Next

            If Not _contentInfo Is Nothing Then
                _contentInfo.ResizeLanguageVariables(_LanguageCount)
            End If

            If Not _attributes Is Nothing Then
                _attributes.ResizeLanguageVariables(_LanguageCount)
            End If
        End Sub


        ''' <summary>
        ''' Sets the current language as the default language of the model.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Public Sub SetCurrentLanguageDefault()
            If mLanguageIndex > 0 Then
                'Switch index values between current language and the old default language
                mLanguages(_language) = mLanguageIndex
                mLanguages(_currentLanguage) = 0

                _language = _currentLanguage

                'Switch text values between current language and the old default language
                Util.SwitchLanguages(Me, 0, mLanguageIndex)

                For i As Integer = 0 To _variables.Count - 1
                    _variables(i).SetCurrentLanguageDefault()
                Next

                If Not _contentInfo Is Nothing Then
                    _contentInfo.SetCurrentLanguageDefault()
                End If

                If Not _attributes Is Nothing Then
                    _attributes.SetCurrentLanguageDefault()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Deletes all languages except the current language from the model
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DeleteAllLanguagesExceptCurrent()
            Dim deleteList As New List(Of String)

            SetCurrentLanguageDefault()

            'Find languages to delete
            For Each kvp As KeyValuePair(Of String, Integer) In mLanguages
                If (kvp.Key <> _currentLanguage) And (kvp.Key.ToLower() <> "default") Then
                    deleteList.Add(kvp.Key)
                End If
            Next

            'Delete the languages
            For Each lang As String In deleteList
                mLanguages.Remove(lang)
            Next

            _LanguageCount = 1

            SetLanguage(0)
            ResizeLanguageVariables()
        End Sub

        Public Sub SetPreferredLanguage(ByVal language As String)
            _preferredLanguage = language
            If (HasLanguage(language)) Then
                SetLanguage(language)
            End If
        End Sub

        Public Function GetPreferredLanguage() As String
            Return _preferredLanguage
        End Function

#End Region

#Region "Count functions"

        Public Function GetRowLength() As Integer
            Dim length As Integer = 1

            For Each v As Variable In Me._heading
                length *= v.Values.Count
            Next

            Return length
        End Function

        Public Function GetRowCount() As Integer
            Dim count As Integer = 1

            For Each v As Variable In Me._stub
                count *= v.Values.Count
            Next

            Return count
        End Function

#End Region

        ''' <summary>
        ''' Creates the title for all languages in the current model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateTitle()
            Dim strBy As String = ""
            Dim strAnd As String = ""
            Dim title As System.Text.StringBuilder
            Dim langs As String()
            Dim langIndex As Integer = 0
            Dim oldCurrentLanguageIndex As Integer
            Dim resourcemanager As PxResourceManager = PxResourceManager.GetResourceManager()

            Dim vars As New List(Of Variable)

            For Each var As Variable In Me.Variables
                If Not (PCAxis.Paxiom.Settings.Metadata.RemoveSingleContent AndAlso var.IsContentVariable AndAlso var.Values.Count = 1) Then
                    'Add all variables except the content variable if it only contains one value
                    vars.Add(var)
                End If
            Next

            'Remember current language
            oldCurrentLanguageIndex = CurrentLanguageIndex

            If Not resourcemanager Is Nothing Then
                langs = Me.GetAllLanguages

                If langs Is Nothing Then
                    'Only default language exists - Add it to langs
                    ReDim langs(0)
                    langs(0) = Me.Language
                End If

                For Each lang As String In langs
                    'Get localized strings
                    strBy = resourcemanager.GetString("PxcMetaTitleBy", lang)
                    strAnd = resourcemanager.GetString("PxcMetaTitleAnd", lang)

                    'Change language on variables
                    For Each var As Variable In vars
                        var.SetLanguage(langIndex)
                    Next

                    'Build the title
                    title = New System.Text.StringBuilder(Me._contents(langIndex))
                    title.Append(" " & strBy & " ")

                    For i As Integer = 0 To vars.Count - 3
                        title.Append(vars(i).Name)
                        title.Append(", ")
                    Next
                    If vars.Count > 1 Then
                        title.Append(vars(vars.Count - 2).Name)
                        title.Append(" " & strAnd & " ")
                        title.Append(vars(vars.Count - 1).Name)
                    Else
                        title.Append(vars(vars.Count - 1).Name)
                    End If
                    'Set title
                    Me._title(langIndex) = title.ToString()

                    langIndex = langIndex + 1
                Next

                'Restore to the selected language
                For Each var As Variable In vars
                    var.SetLanguage(oldCurrentLanguageIndex)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Gets the localized variablename for sortvariable ($$SORT)
        ''' </summary>
        ''' <param name="language">Actual language</param>
        ''' <returns>The localized name of sortvariable</returns>
        ''' <remarks>If language is nothing the defaultlanguage of the model is used</remarks>
        Public Function GetLocalizedSortVariableName(ByVal language As String) As String
            Dim name As String = "Sortvar"
            Dim resourcemanager As PxResourceManager = PxResourceManager.GetResourceManager()

            If String.IsNullOrEmpty(language) Then
                language = Me.Language
            End If

            name = resourcemanager.GetString("PxcSortVariable", language)

            Return name
        End Function

        ''' <summary>
        ''' Adds a variable to the metadata
        ''' </summary>
        ''' <param name="variable">the variable to add</param>
        ''' <remarks>Alway use this method to add variable because this will add the in the rigth collections</remarks>
        Public Sub AddVariable(ByVal variable As Variable)
            If variable.Placement = PlacementType.Heading Then
                Me.Heading.Add(variable)
            Else
                Me.Stub.Add(variable)
            End If

            BuildVariablesCollection()

            variable.SetMeta(Me)
        End Sub

        ''' <summary>
        ''' Inserts a variable into the metadata
        ''' </summary>
        ''' <param name="index">index to place the variable</param>
        ''' <param name="variable">the variable to insert</param>
        ''' <remarks>
        ''' Always use this method to insert the variable into the 
        ''' metadata since this method will place it in the rigth 
        ''' collections and set the variables Meta property. 
        ''' The index parameter is dependent of the placement of the variable. 
        ''' </remarks>
        Public Sub InsertVariable(ByVal index As Integer, ByVal variable As Variable)
            If variable.Placement = PlacementType.Heading Then
                Me.Heading.Insert(index, variable)
            Else
                Me.Stub.Insert(index, variable)
            End If

            BuildVariablesCollection()

            variable.SetMeta(Me)
        End Sub

        ''' <summary>
        ''' Removes a variable from the metadata
        ''' </summary>
        ''' <param name="variable">the variable to remove</param>
        ''' <remarks>
        ''' Always use this function to remove variables from the metadata since
        ''' this method will remove the variable from all the rigth collection and 
        ''' remove the Meta refrence of the variable
        ''' </remarks>
        Public Sub RemoveVariable(ByVal variable As Variable)
            If variable.Placement = PlacementType.Heading Then
                Me.Heading.Remove(variable)
            Else
                Me.Stub.Remove(variable)
            End If

            BuildVariablesCollection()

            variable.SetMeta(Nothing)
        End Sub

        ''' <summary>
        ''' Create a deep copy of the PXMeta object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As PXMeta
            Dim newObject As PXMeta

            newObject = CType(Me.MemberwiseClone(), PXMeta)

            newObject.SetModel(Nothing)

            ' Handle reference types

            ' Handle SubjectArea
            newObject._subjectArea = Nothing
            If Me._subjectArea IsNot Nothing Then
                newObject._subjectArea = New String(Me._subjectArea.Count - 1) {}
                For i As Integer = 0 To Me._subjectArea.Count - 1
                    newObject._subjectArea(i) = Me._subjectArea(i)
                Next
            End If

            ' Handle Contents
            newObject._contents = Nothing
            If Me._contents IsNot Nothing Then
                newObject._contents = New String(Me._contents.Count - 1) {}
                For i As Integer = 0 To Me._contents.Count - 1
                    newObject._contents(i) = Me._contents(i)
                Next
            End If

            ' Handle title
            newObject._title = Nothing
            If Me._title IsNot Nothing Then
                newObject._title = New String(Me._title.Count - 1) {}
                For i As Integer = 0 To Me._title.Count - 1
                    newObject._title(i) = Me._title(i)
                Next
            End If

            ' Handle Description
            newObject._description = Nothing
            If Me._description IsNot Nothing Then
                newObject._description = New String(Me._description.Count - 1) {}
                For i As Integer = 0 To Me._description.Count - 1
                    newObject._description(i) = Me._description(i)
                Next
            End If

            ' Handle Source
            newObject._source = Nothing
            If Me._source IsNot Nothing Then
                newObject._source = New String(Me._source.Count - 1) {}
                For i As Integer = 0 To Me._source.Count - 1
                    newObject._source(i) = Me._source(i)
                Next
            End If

            ' Handle Database
            newObject._database = Nothing
            If Me._database IsNot Nothing Then
                newObject._database = New String(Me._database.Count - 1) {}
                For i As Integer = 0 To Me._database.Count - 1
                    newObject._database(i) = Me._database(i)
                Next
            End If

            ' Handle InfoFile
            newObject._infoFile = Nothing
            If Me._infoFile IsNot Nothing Then
                newObject._infoFile = New String(Me._infoFile.Count - 1) {}
                For i As Integer = 0 To Me._infoFile.Count - 1
                    newObject._infoFile(i) = Me._infoFile(i)
                Next
            End If

            ' Handle ContentInfo
            newObject._contentInfo = Nothing
            If Me._contentInfo IsNot Nothing Then
                newObject._contentInfo = Me._contentInfo.CreateCopy()
            End If

            ' Handle Information
            newObject._information = Nothing
            If Me._information IsNot Nothing Then
                newObject._information = New String(Me._information.Count - 1) {}
                For i As Integer = 0 To Me._information.Count - 1
                    newObject._information(i) = Me._information(i)
                Next
            End If

            ' Handle Link
            newObject._link = Nothing
            If Me._link IsNot Nothing Then
                newObject._link = New String(Me._link.Count - 1) {}
                For i As Integer = 0 To Me._link.Count - 1
                    newObject._link(i) = Me._link(i)
                Next
            End If

            ' Handle Survey
            newObject._survey = Nothing
            If Me._survey IsNot Nothing Then
                newObject._survey = New String(Me._survey.Count - 1) {}
                For i As Integer = 0 To Me._survey.Count - 1
                    newObject._survey(i) = Me._survey(i)
                Next
            End If

            ' Handle DataNoteSum
            newObject._dataNoteSum = Nothing
            If Me._dataNoteSum IsNot Nothing Then
                newObject._dataNoteSum = New String(Me._dataNoteSum.Count - 1) {}
                For i As Integer = 0 To Me._dataNoteSum.Count - 1
                    newObject._dataNoteSum(i) = Me._dataNoteSum(i)
                Next
            End If

            ' Handle Notes
            newObject._notes = Nothing
            If Me._notes IsNot Nothing Then
                newObject._notes = New Notes(Me._notes.Count - 1) {}
                For i As Integer = 0 To Me._notes.Count - 1
                    If Me._notes(i) IsNot Nothing Then
                        newObject._notes(i) = Me._notes(i).CreateCopy()
                    End If
                Next
            End If

            ' Handle Variable collections
            newObject._heading = Me.Heading.CreateCopy() 'New Variables ' 
            newObject._heading.SetMeta(newObject)
            newObject._stub = Me.Stub.CreateCopy() 'New Variables ' 
            newObject._stub.SetMeta(newObject)
            newObject._variables = New Variables
            newObject.BuildVariablesCollection()
            newObject._variables.SetMeta(newObject)

            ' Handle CellNotes
            newObject._cellNotes = Nothing
            If Me._cellNotes IsNot Nothing Then
                newObject._cellNotes = New CellNotes(Me._cellNotes.Count - 1) {}
                For i As Integer = 0 To Me._cellNotes.Count - 1
                    If Me._cellNotes(i) IsNot Nothing Then
                        newObject._cellNotes(i) = Me._cellNotes(i).CreateCopy()
                    End If
                Next
            End If

            ' Handle DataNoteCells
            newObject._dataNoteCells = Nothing
            If Me._dataNoteCells IsNot Nothing Then
                newObject._dataNoteCells = New DataNoteCells(Me._dataNoteCells.Count - 1) {}
                For i As Integer = 0 To Me._dataNoteCells.Count - 1
                    If Me._dataNoteCells(i) IsNot Nothing Then
                        newObject._dataNoteCells(i) = Me._dataNoteCells(i).CreateCopy()
                    End If
                Next
            End If

            ' Handle ContentVariable
            newObject._contentVariable = Nothing
            If Me._contentVariable IsNot Nothing Then
                newObject.SetContentVariable()
            End If

            ' Handle DataSymbol Arrays
            newObject._dataSymbol1 = Nothing
            If Me._dataSymbol1 IsNot Nothing Then
                newObject._dataSymbol1 = New String(Me._dataSymbol1.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol1.Count - 1
                    newObject._dataSymbol1(i) = Me._dataSymbol1(i)
                Next
            End If
            newObject._dataSymbol2 = Nothing
            If Me._dataSymbol2 IsNot Nothing Then
                newObject._dataSymbol2 = New String(Me._dataSymbol2.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol2.Count - 1
                    newObject._dataSymbol2(i) = Me._dataSymbol2(i)
                Next
            End If
            newObject._dataSymbol3 = Nothing
            If Me._dataSymbol3 IsNot Nothing Then
                newObject._dataSymbol3 = New String(Me._dataSymbol3.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol3.Count - 1
                    newObject._dataSymbol3(i) = Me._dataSymbol3(i)
                Next
            End If
            newObject._dataSymbol4 = Nothing
            If Me._dataSymbol4 IsNot Nothing Then
                newObject._dataSymbol4 = New String(Me._dataSymbol4.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol4.Count - 1
                    newObject._dataSymbol4(i) = Me._dataSymbol4(i)
                Next
            End If
            newObject._dataSymbol5 = Nothing
            If Me._dataSymbol5 IsNot Nothing Then
                newObject._dataSymbol5 = New String(Me._dataSymbol5.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol5.Count - 1
                    newObject._dataSymbol5(i) = Me._dataSymbol5(i)
                Next
            End If
            newObject._dataSymbol6 = Nothing
            If Me._dataSymbol6 IsNot Nothing Then
                newObject._dataSymbol6 = New String(Me._dataSymbol6.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol6.Count - 1
                    newObject._dataSymbol6(i) = Me._dataSymbol6(i)
                Next
            End If
            newObject._dataSymbol7 = Nothing
            If Me._dataSymbol7 IsNot Nothing Then
                newObject._dataSymbol7 = New String(Me._dataSymbol7.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbol7.Count - 1
                    newObject._dataSymbol7(i) = Me._dataSymbol7(i)
                Next
            End If
            newObject._dataSymbolNIL = Nothing
            If Me._dataSymbolNIL IsNot Nothing Then
                newObject._dataSymbolNIL = New String(Me._dataSymbolNIL.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbolNIL.Count - 1
                    newObject._dataSymbolNIL(i) = Me._dataSymbolNIL(i)
                Next
            End If
            newObject._dataSymbolSum = Nothing
            If Me._dataSymbolSum IsNot Nothing Then
                newObject._dataSymbolSum = New String(Me._dataSymbolSum.Count - 1) {}
                For i As Integer = 0 To Me._dataSymbolSum.Count - 1
                    newObject._dataSymbolSum(i) = Me._dataSymbolSum(i)
                Next
            End If

            ' End DataSymbol Arrays

            ' Handle ExtendedProperties
            newObject._extendedProperties = Nothing
            If Me._extendedProperties IsNot Nothing Then
                newObject._extendedProperties = Me._extendedProperties.CreateCopy()
            End If

            ' Handle Languages
            newObject.mLanguages = Nothing
            If Me.mLanguages IsNot Nothing Then
                newObject.mLanguages = New Dictionary(Of String, Integer)
                For Each kvp As KeyValuePair(Of String, Integer) In Me.mLanguages
                    newObject.mLanguages.Add(kvp.Key, kvp.Value)
                Next
            End If

            ' Handle Attributes
            newObject._attributes = Nothing
            If Me._attributes IsNot Nothing Then
                newObject._attributes = Me._attributes.CreateCopy()
            End If

            Return newObject
        End Function

        'Public Sub CopyVariablesTo(ByRef newMeta As PXMeta)

        'End Sub
        'Public Sub CopyHeadingTo(ByRef newMeta As PXMeta)

        'End Sub
        'Public Sub CopyStubTo(ByRef newMeta As PXMeta)

        'End Sub

        ''' <summary>
        ''' Copy My CellNotes into parameter Meta object
        ''' </summary>
        ''' <param name="newMeta"></param>
        ''' <remarks></remarks>
        Public Sub CopyCellNotesTo(ByRef newMeta As PXMeta)
            Dim oldVariableValuePair As VariableValuePair
            Dim newVariableValuePairs As VariableValuePairs
            Dim newVariable As Variable

            If newMeta.Variables.Count = 0 Then
                Throw New PXException("New PXMeta does not have any Variables. CopyCellNotes aborted.")
            End If

            For i As Integer = 0 To Me.CellNotes.Count - 1
                newVariableValuePairs = New VariableValuePairs()
                For j As Integer = 0 To Me.CellNotes(i).Conditions.Count - 1
                    oldVariableValuePair = Me.CellNotes(i).Conditions(j)
                    newVariable = newMeta.Variables.GetByCode(oldVariableValuePair.VariableCode)
                    If newVariable IsNot Nothing Then
                        ' Variable was there, now check the value
                        If newVariable.Values.GetByCode(oldVariableValuePair.ValueCode) IsNot Nothing Then
                            newVariableValuePairs.Add(New VariableValuePair(oldVariableValuePair.VariableCode, oldVariableValuePair.ValueCode))
                        End If
                    End If
                Next
                newMeta.CellNotes.Add(New CellNote(newVariableValuePairs))
            Next
        End Sub
        ''' <summary>
        ''' Copy My DataNoteCells into parameter Meta object
        ''' </summary>
        ''' <param name="newMeta"></param>
        ''' <remarks></remarks>
        Public Sub CopyDataNoteCellsTo(ByRef newMeta As PXMeta)
            Dim oldVariableValuePair As VariableValuePair
            Dim newVariableValuePairs As VariableValuePairs
            Dim newVariable As Variable

            If newMeta.Variables.Count = 0 Then
                Throw New PXException("New PXMeta does not have any Variables. CopyDataNoteCells aborted.")
            End If

            For i As Integer = 0 To Me.DataNoteCells.Count - 1
                newVariableValuePairs = New VariableValuePairs()
                For j As Integer = 0 To Me.DataNoteCells(i).Conditions.Count - 1
                    oldVariableValuePair = Me.DataNoteCells(i).Conditions(j)
                    newVariable = newMeta.Variables.GetByCode(oldVariableValuePair.VariableCode)
                    If newVariable IsNot Nothing Then
                        ' Variable was there, now check the value
                        If newVariable.Values.GetByCode(oldVariableValuePair.ValueCode) IsNot Nothing Then
                            newVariableValuePairs.Add(New VariableValuePair(oldVariableValuePair.VariableCode, oldVariableValuePair.ValueCode))
                        End If
                    End If
                Next
                newMeta.DataNoteCells.Add(New DataNoteCell(newVariableValuePairs))
            Next
        End Sub
        ''' <summary>
        ''' Copy My ContentInfo into parameter Meta object
        ''' </summary>
        ''' <param name="newMeta"></param>
        ''' <remarks></remarks>
        Public Sub CopyContentInfoTo(ByRef newMeta As PXMeta)
            If Me.ContentInfo IsNot Nothing Then
                newMeta.ContentInfo = Me.ContentInfo.CreateCopy()
            End If
        End Sub
        '''' <summary>
        '''' Copy My ExtendedProperties into parameter Meta object
        '''' </summary>
        '''' <param name="newMeta"></param>
        '''' <remarks></remarks>
        'Public Sub CopyExtendedPropertiesTo(ByRef newMeta As PXMeta)
        '    With newMeta.ExtendedProperties
        '        For Each kvp As Collections.Generic.KeyValuePair(Of String, String) In Me.ExtendedProperties
        '            .Add(kvp.Key, kvp.Value)
        '        Next
        '    End With
        'End Sub
        ''' <summary>
        ''' Copy My Notes into parameter Meta object
        ''' </summary>
        ''' <param name="newMeta"></param>
        ''' <remarks></remarks>
        Public Sub CopyNotesTo(ByRef newMeta As PXMeta)
            If Me.Notes.Count > 0 Then
                For Each note As Note In Me.Notes
                    newMeta.Notes.Add(New Note(note.Text, note.Type, note.Mandantory))
                Next
            End If
        End Sub

        ''' <summary>
        ''' Sets the content variable
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetContentVariable()
            Me.ContentVariable = Nothing
            For Each variable As Variable In Me.Variables
                If variable.IsContentVariable Then
                    Me.ContentVariable = variable
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _variables.SetMeta(Me)
            _stub.SetMeta(Me)
            _heading.SetMeta(Me)
            _dataSymbolSum(mLanguageIndex) = "" '"#SUM"
            mLanguages.Add("default", 0)
            _notes(0) = New Notes
        End Sub


#Region "Helper fuctions"
        ''' <summary>
        ''' Gets all mandantory notes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllMandantoryNotes() As String
            Dim notes As Notes = _notes(mLanguageIndex)
            Dim var As Variable
            Dim val As Value
            Dim sb As New System.Text.StringBuilder

            'Adds all table notes
            If notes IsNot Nothing Then
                For i As Integer = 0 To notes.Count - 1
                    If notes(i).Mandantory Then
                        sb.Append(notes(i).Text)
                        sb.Append(ControlChars.CrLf)
                        sb.Append(ControlChars.CrLf)
                    End If
                Next
            End If

            'Adds all Variable notes
            For i As Integer = 0 To _variables.Count - 1
                var = _variables(i)
                notes = var.Notes
                If notes IsNot Nothing Then
                    For j As Integer = 0 To notes.Count - 1
                        If notes(j).Mandantory Then
                            sb.Append(var.Name)
                            sb.Append(ControlChars.CrLf)
                            sb.Append(notes(j).Text)
                            sb.Append(ControlChars.CrLf)
                            sb.Append(ControlChars.CrLf)
                        End If
                    Next
                End If
            Next

            'Adds all Value notes
            For i As Integer = 0 To _variables.Count - 1
                var = _variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    val = var.Values(j)
                    notes = val.Notes
                    If notes IsNot Nothing Then
                        For k As Integer = 0 To notes.Count - 1
                            If notes(k).Mandantory Then
                                sb.Append(var.Name)
                                sb.Append("=")
                                sb.Append(val.Value)
                                sb.Append(ControlChars.CrLf)
                                sb.Append(notes(k).Text)
                                sb.Append(ControlChars.CrLf)
                                sb.Append(ControlChars.CrLf)
                            End If
                        Next
                    End If
                Next
            Next

            Return sb.ToString()
        End Function

        Public Function GetAllNotes(ByVal infoLevel As InformationLevelType) As String
            Dim notes As Notes = _notes(mLanguageIndex)
            Dim var As Variable
            Dim val As Value
            Dim sb As New System.Text.StringBuilder

            'Add all table notes
            If notes IsNot Nothing Then
                For i As Integer = 0 To notes.Count - 1
                    If ((notes(i).Mandantory And infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or (infoLevel > InformationLevelType.MandantoryFootnotesOnly)) Then
                        sb.Append(notes(i).Text)
                        sb.Append(ControlChars.CrLf)
                        sb.Append(ControlChars.CrLf)
                    End If
                Next
            End If

            'Add all Variable notes
            For i As Integer = 0 To _variables.Count - 1
                var = _variables(i)
                notes = var.Notes
                If notes IsNot Nothing Then
                    For j As Integer = 0 To notes.Count - 1
                        If ((notes(j).Mandantory And infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or (infoLevel > InformationLevelType.MandantoryFootnotesOnly)) Then
                            sb.Append(var.Name)
                            sb.Append(ControlChars.CrLf)
                            sb.Append(notes(j).Text)
                            sb.Append(ControlChars.CrLf)
                            sb.Append(ControlChars.CrLf)
                        End If
                    Next
                End If
            Next

            'Add all Value notes
            For i As Integer = 0 To _variables.Count - 1
                var = _variables(i)
                For j As Integer = 0 To var.Values.Count - 1
                    val = var.Values(j)
                    notes = val.Notes
                    If notes IsNot Nothing Then
                        For k As Integer = 0 To notes.Count - 1
                            If ((notes(k).Mandantory And infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or (infoLevel > InformationLevelType.MandantoryFootnotesOnly)) Then
                                sb.Append(var.Name)
                                sb.Append("=")
                                sb.Append(val.Value)
                                sb.Append(ControlChars.CrLf)
                                sb.Append(notes(k).Text)
                                sb.Append(ControlChars.CrLf)
                                sb.Append(ControlChars.CrLf)
                            End If
                        Next
                    End If
                Next
            Next

            'Add all cellnotes
            Dim cn As CellNote
            Dim vvp As VariableValuePair
            For i As Integer = 0 To CellNotes.Count - 1
                cn = CellNotes(i)
                If ((cn.Mandatory And infoLevel = InformationLevelType.MandantoryFootnotesOnly) Or (infoLevel > InformationLevelType.MandantoryFootnotesOnly)) Then
                    For j As Integer = 0 To cn.Conditions.Count - 1
                        vvp = cn.Conditions(j)
                        var = _variables.GetByCode(vvp.VariableCode)
                        val = var.Values.GetByCode(vvp.ValueCode)
                        sb.Append(var.Name)
                        sb.Append("=")
                        sb.Append(val.Value)
                        sb.Append(":")
                        sb.Append(ControlChars.CrLf)
                    Next
                    sb.Append(cn.Text)
                    sb.Append(ControlChars.CrLf)
                    sb.Append(ControlChars.CrLf)
                End If
            Next

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' translates symbols constants to symbols strings
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataSymbol(ByVal value As Double) As String
            Select Case value
                Case PXConstant.DATASYMBOL_1
                    Return DataSymbol1
                Case PXConstant.DATASYMBOL_2
                    Return DataSymbol2
                Case PXConstant.DATASYMBOL_3
                    Return DataSymbol3
                Case PXConstant.DATASYMBOL_4
                    Return DataSymbol4
                Case PXConstant.DATASYMBOL_5
                    Return DataSymbol5
                Case PXConstant.DATASYMBOL_6
                    Return DataSymbol6
                Case PXConstant.DATASYMBOL_7
                    Return DataSymbol7
                Case PXConstant.DATASYMBOL_NIL
                    Return DataSymbolNIL
                Case Else
                    Return value.ToString(Globalization.CultureInfo.InvariantCulture)
            End Select
        End Function

#End Region

        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim vars As Variables

            _variables.SetMeta(Me)
            _stub.SetMeta(Me)
            _heading.SetMeta(Me)
            _charset = info.GetString("Charset")
            _matrix = info.GetString("Matrix")
            _subjectCode = info.GetString("SubjectCode")
            _subjectArea = CType(info.GetValue("SubjectArea", GetType(String())), String())
            _contents = CType(info.GetValue("Contents", GetType(String())), String())
            _decimals = info.GetInt32("Decimals")
            _title = CType(info.GetValue("Title", GetType(String())), String())
            _description = CType(info.GetValue("Description", GetType(String())), String())
            _language = info.GetString("Language")
            _axisVersion = info.GetString("AxisVersion")
            _aggregAllowed = info.GetBoolean("AggregAllowed")
            _autoOpen = info.GetBoolean("AutoOpen")
            _descriptionDefault = info.GetBoolean("DescriptionDefault")
            _codePage = info.GetString("CodePage")
            _creationDate = info.GetString("CreationDate")
            _copyright = info.GetBoolean("Copyright")
            _showDecimals = info.GetInt32("ShowDecimals")
            _source = CType(info.GetValue("Source", GetType(String())), String())
            _confidential = info.GetInt32("Confidential")
            _database = CType(info.GetValue("Database", GetType(String())), String())
            _infoFile = CType(info.GetValue("InfoFile", GetType(String())), String())
            _contentInfo = CType(info.GetValue("ContentInfo", GetType(ContInfo)), ContInfo)
            _updateFrequency = info.GetString("UpdateFrequency")
            _nextUpdate = info.GetString("NextUpdate")
            _PXServer = info.GetString("PXServer")
            _directoryPath = info.GetString("DirectoryPath")
            _information = CType(info.GetValue("Information", GetType(String())), String())
            _link = CType(info.GetValue("Link", GetType(String())), String())
            _survey = CType(info.GetValue("Survey", GetType(String())), String())
            _tableID = info.GetString("TableID")
            _defaultGraph = info.GetInt32("DefaultGraph")
            _dataNoteSum = CType(info.GetValue("DataNoteSum", GetType(String())), String())
            _notes = CType(info.GetValue("Notes", GetType(Notes())), Notes())
            _cellNotes = CType(info.GetValue("CellNotes", GetType(CellNotes())), CellNotes())
            _dataNoteCells = CType(info.GetValue("DataNoteCells", GetType(DataNoteCells())), DataNoteCells())
            _dataSymbol1 = CType(info.GetValue("DataSymbol1", GetType(String())), String())
            _dataSymbol2 = CType(info.GetValue("DataSymbol2", GetType(String())), String())
            _dataSymbol3 = CType(info.GetValue("DataSymbol3", GetType(String())), String())
            _dataSymbol4 = CType(info.GetValue("DataSymbol4", GetType(String())), String())
            _dataSymbol5 = CType(info.GetValue("DataSymbol5", GetType(String())), String())
            _dataSymbol6 = CType(info.GetValue("DataSymbol6", GetType(String())), String())
            _dataSymbol7 = CType(info.GetValue("DataSymbol7", GetType(String())), String())
            _dataSymbolNIL = CType(info.GetValue("DataSymbolNIL", GetType(String())), String())
            _dataSymbolSum = CType(info.GetValue("DataSymbolSum", GetType(String())), String())

            vars = CType(info.GetValue("Variables", GetType(Variables)), Variables)

            Dim v As Variable
            For i As Integer = 0 To vars.Count - 1
                v = vars(i)
                If v.IsContentVariable Then
                    Me._contentVariable = v
                End If

                Me.AddVariable(v)
            Next
        End Sub

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Charset", _charset)
            info.AddValue("Matrix", _matrix)
            info.AddValue("SubjectCode", _subjectCode)
            info.AddValue("SubjectArea", _subjectArea) 'array
            info.AddValue("Contents", _contents) 'array
            info.AddValue("Decimals", _decimals)
            info.AddValue("Title", _title) 'array
            info.AddValue("Description", _description) 'array
            info.AddValue("Language", _language)
            info.AddValue("AxisVersion", _axisVersion)
            info.AddValue("AggregAllowed", _aggregAllowed)
            info.AddValue("AutoOpen", _autoOpen)
            info.AddValue("DescriptionDefault", _descriptionDefault)
            info.AddValue("CodePage", _codePage)
            info.AddValue("CreationDate", _creationDate)
            info.AddValue("Copyright", _copyright)
            info.AddValue("ShowDecimals", _showDecimals)
            info.AddValue("Source", _source) 'array
            info.AddValue("Confidential", _confidential)
            info.AddValue("Database", _database) 'array
            info.AddValue("InfoFile", _infoFile) 'array
            info.AddValue("ContentInfo", _contentInfo)
            info.AddValue("UpdateFrequency", _updateFrequency)
            info.AddValue("NextUpdate", _nextUpdate)
            info.AddValue("PXServer", _PXServer)
            info.AddValue("DirectoryPath", _directoryPath)
            info.AddValue("Information", _information) 'array
            info.AddValue("Link", _link) 'array
            info.AddValue("Survey", _survey) 'array
            info.AddValue("TableID", _tableID)
            info.AddValue("DefaultGraph", _defaultGraph)
            info.AddValue("DataNoteSum", _dataNoteSum)
            info.AddValue("Notes", _notes) 'array
            info.AddValue("CellNotes", _cellNotes) 'array
            info.AddValue("DataNoteCells", _dataNoteCells)
            info.AddValue("DataSymbol1", _dataSymbol1) 'array
            info.AddValue("DataSymbol2", _dataSymbol2) 'array
            info.AddValue("DataSymbol3", _dataSymbol3) 'array
            info.AddValue("DataSymbol4", _dataSymbol4) 'array
            info.AddValue("DataSymbol5", _dataSymbol5) 'array
            info.AddValue("DataSymbol6", _dataSymbol6) 'array
            info.AddValue("DataSymbol7", _dataSymbol7) 'array
            info.AddValue("DataSymbolNIL", _dataSymbolNIL) 'array
            info.AddValue("DataSymbolSum", _dataSymbolSum) 'array
            info.AddValue("Variables", _variables)

        End Sub

        ''' <summary>
        ''' Sync the Variables collection with Stub and Heading
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BuildVariablesCollection()
            Me.Variables.Clear()
            For i As Integer = 0 To Me.Stub.Count - 1
                Me.Variables.Add(Me.Stub(i))
            Next

            For i As Integer = 0 To Me.Heading.Count - 1
                Me.Variables.Add(Me.Heading(i))
            Next
        End Sub

        ''' <summary>
        ''' Validate and Prune My objects
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Prune()

            PruneCellNotes()
            PruneDataNoteCells()
            PruneCellAttributes()

            'Checks if the content variable only has one value and if so removes the variable
            'and changes the contents
            If Settings.Metadata.RemoveSingleContent Then
                If Me.ContentVariable IsNot Nothing Then
                    If Me.ContentVariable.Values.Count = 1 Then
                        PruneSingleContent()
                        RemoveVariable(Me.ContentVariable)
                        Me.ContentVariable = Nothing
                        CreateTitle()
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Remove unwanted cellnotes
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PruneCellNotes()
            Dim doRemove As Boolean
            Dim checkVar As Variable
            Dim languageIndex As Integer = mLanguageIndex 'Remember selected language

            'Remove cellnotes for each language
            For lang As Integer = 0 To _LanguageCount - 1
                mLanguageIndex = lang

                ' Validate CellNotes - if Variable or Value is missing, remove the CellNote
                For j As Integer = Me.CellNotes.Count - 1 To 0 Step -1
                    doRemove = False
                    For i As Integer = Me.CellNotes(j).Conditions.Count - 1 To 0 Step -1
                        ' Check the variable for existance
                        checkVar = Me.Variables.GetByCode(Me.CellNotes(j).Conditions(i).VariableCode)
                        ' Check the variable
                        If checkVar Is Nothing Then
                            doRemove = True
                            Exit For
                        Else
                            ' Check the value
                            If checkVar.Values.GetByCode(Me.CellNotes(j).Conditions(i).ValueCode) Is Nothing Then
                                doRemove = True
                                Exit For
                            End If
                        End If
                    Next
                    If doRemove Then
                        Me.CellNotes.RemoveAt(j)
                    End If
                Next
            Next

            'Restore to the selected language
            mLanguageIndex = languageIndex
        End Sub

        ''' <summary>
        ''' Remove unwanted datanotecells
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PruneDataNoteCells()
            Dim doRemove As Boolean
            Dim checkVar As Variable
            Dim languageIndex As Integer = mLanguageIndex 'Remember selected language

            'Remove datanotecells for each language
            For lang As Integer = 0 To _LanguageCount - 1
                mLanguageIndex = lang

                ' Validate DataNoteCells - if Variable or Value is missing, remove the CellNote
                For j As Integer = Me.DataNoteCells.Count - 1 To 0 Step -1
                    doRemove = False
                    For i As Integer = Me.DataNoteCells(j).Conditions.Count - 1 To 0 Step -1
                        ' Check the variable for existance
                        checkVar = Me.Variables.GetByCode(Me.DataNoteCells(j).Conditions(i).VariableCode)
                        ' Check the variable
                        If checkVar Is Nothing Then
                            doRemove = True
                            Exit For
                        Else
                            ' Check the value
                            If checkVar.Values.GetByCode(Me.DataNoteCells(j).Conditions(i).ValueCode) Is Nothing Then
                                doRemove = True
                                Exit For
                            End If
                        End If
                    Next

                    If doRemove Then
                        Me.DataNoteCells.RemoveAt(j)
                    End If
                Next
            Next

            'Restore to the selected language
            mLanguageIndex = languageIndex
        End Sub

        ''' <summary>
        ''' Remove unwanted cell attributes
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PruneCellAttributes()
            Dim checkVar As Variable

            'List with cell attributes to delete
            Dim deleteList As New List(Of VariableValuePairs)

            'Validate attributes at cell level - if Variable or Value is missing, remove the cell attribute
            For Each kvp As KeyValuePair(Of VariableValuePairs, String()) In Me.Attributes.CellAttributes
                For Each vvp As VariableValuePair In kvp.Key
                    'Check variable
                    checkVar = Me.Variables.GetByCode(vvp.VariableCode)

                    If checkVar Is Nothing Then
                        deleteList.Add(kvp.Key)
                        Exit For
                    Else
                        'Check value
                        If checkVar.Values.GetByCode(vvp.ValueCode) Is Nothing Then
                            deleteList.Add(kvp.Key)
                            Exit For
                        End If
                    End If
                Next
            Next

            'Delete unwanted cell attributes...
            For Each vvp As VariableValuePairs In deleteList
                Me.Attributes.CellAttributes.Remove(vvp)
            Next
        End Sub

        ''' <summary>
        ''' Move and remove information from the content variable
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PruneSingleContent()
            Dim languageIndex As Integer = mLanguageIndex 'Remember selected language

            'Copy (overwrite existing) contentinfo from content variable to PXMeta
            If Not Me.ContentVariable.Values(0).ContentInfo Is Nothing Then
                Me.ContentInfo = Me.ContentVariable.Values(0).ContentInfo
            End If

            'Do for each language
            For lang As Integer = 0 To _LanguageCount - 1
                Me.SetLanguage(lang)

                'Setting the contents for each language
                If String.IsNullOrEmpty(Me.Contents) Then
                    Me.Contents = Me.ContentVariable.Values(0).Value
                End If

                'Copy valuenotes from content variable to PXMeta
                If Me.ContentVariable.Values(0).HasNotes Then
                    For Each n As Note In Me.ContentVariable.Values(0).Notes
                        n.Type = NoteType.Table
                        Me.Notes.Add(n)
                    Next
                End If

                'Copy notes from content variable to PXMeta
                If Me.ContentVariable.HasNotes Then
                    For Each n As Note In Me.ContentVariable.Notes
                        n.Type = NoteType.Table
                        Me.Notes.Add(n)
                    Next
                End If

                'Remove Datanotes that belongs to the content variable
                For i As Integer = Me.DataNoteCells.Count - 1 To 0 Step -1
                    Dim datanote As DataNoteCell = Me.DataNoteCells(i)
                    For j As Integer = datanote.Conditions.Count - 1 To 0 Step -1
                        Dim condition As VariableValuePair = datanote.Conditions(j)
                        If condition.VariableCode.Equals(Me.ContentVariable.Code) Then
                            If condition.ValueCode.Equals(Me.ContentVariable.Values(0).Code) Then
                                datanote.Conditions.Remove(condition)
                            Else
                                Me.DataNoteCells.Remove(datanote)
                            End If
                        End If
                    Next
                Next

                'Remove cellnotes that belongs to the content variable
                For i As Integer = Me.CellNotes.Count - 1 To 0 Step -1
                    Dim cellnote As CellNote = Me.CellNotes(i)
                    For j As Integer = cellnote.Conditions.Count - 1 To 0 Step -1
                        Dim condition As VariableValuePair = cellnote.Conditions(j)
                        If condition.VariableCode.Equals(Me.ContentVariable.Code) Then
                            If condition.ValueCode.Equals(Me.ContentVariable.Values(0).Code) Then
                                cellnote.Conditions.Remove(condition)
                            Else
                                Me.CellNotes.Remove(cellnote)
                            End If
                        End If
                    Next
                Next
            Next

            'Restore to the selected language
            Me.SetLanguage(languageIndex)
        End Sub

        Protected Friend Sub SetModel(ByVal model As PXModel)
            _model = model
        End Sub


    End Class

    Public Enum RoundingType
        None = -1
        BankersRounding = 0
        RoundUp = 1
    End Enum
End Namespace
