'Namespace CommandBar.Plugin
'    Public Class CommandBarPluginShortcut
'        Private _ref As String
'        ''' <summary>
'        ''' Gets or sets the reference to a plugin or saveas control
'        ''' </summary>
'        ''' <value>Reference to a plugin or saveas control</value>
'        ''' <returns>The reference to a plugin or saveas control</returns>
'        ''' <remarks></remarks>
'        Public Property Ref() As String
'            Get
'                Return _ref
'            End Get
'            Set(ByVal value As String)
'                _ref = value
'            End Set
'        End Property


'        Private _uniqueId As String
'        ''' <summary>
'        ''' Gets or sets the UniqueId of the shorcut
'        ''' </summary>
'        ''' <value>UniqueId of the shorcut</value>
'        ''' <returns>The UniqueId of the shorcut</returns>
'        ''' <remarks></remarks>
'        Public Property UniqueRef() As String
'            Get
'                Return _uniqueId
'            End Get
'            Set(ByVal value As String)
'                _uniqueId = value
'            End Set
'        End Property



'        Private _src As String
'        ''' <summary>
'        ''' Gets or sets the path to the shortcuts image
'        ''' </summary>
'        ''' <value>Path to the shortcuts image</value>
'        ''' <returns>The path to the shortcuts image</returns>
'        ''' <remarks></remarks>
'        Public Property Src() As String
'            Get
'                Return _src
'            End Get
'            Set(ByVal value As String)
'                _src = value
'            End Set
'        End Property
'        Private _titleCode As String
'        ''' <summary>
'        ''' Gets or sets the languagecode for the title
'        ''' </summary>
'        ''' <value>Languagecode for the title</value>
'        ''' <returns>The languagecode for the title</returns>
'        ''' <remarks></remarks>
'        Public Property TitleCode() As String
'            Get
'                Return _titleCode
'            End Get
'            Set(ByVal value As String)
'                _titleCode = value
'            End Set
'        End Property

'        Private _altCode As String
'        ''' <summary>
'        ''' Gets or sets the languagecode for the alt text
'        ''' </summary>
'        ''' <value>Languagecode for the alt text</value>
'        ''' <returns>The languagecode for the alt text</returns>
'        ''' <remarks></remarks>
'        Public Property AltCode() As String
'            Get
'                Return _altCode
'            End Get
'            Set(ByVal value As String)
'                _altCode = value
'            End Set
'        End Property

'        Private _placement As CommandBarShortcutPlacementType
'        ''' <summary>
'        ''' Gets or sets the placement of the shortcut
'        ''' </summary>
'        ''' <value>Placement of the shortcut</value>
'        ''' <returns>The placement of the shortcut</returns>
'        ''' <remarks></remarks>
'        Public Property Placement() As CommandBarShortcutPlacementType
'            Get
'                Return _placement
'            End Get
'            Set(ByVal value As CommandBarShortcutPlacementType)
'                _placement = value
'            End Set
'        End Property

'        Private _constraints As New Dictionary(Of String, String)
'        ''' <summary>
'        ''' Gets the properties for the plugin
'        ''' </summary>
'        ''' <value>Properties for the plugin</value>
'        ''' <returns>The properties for the plugin</returns>
'        ''' <remarks></remarks>
'        Public ReadOnly Property Constraints() As Dictionary(Of String, String)
'            Get
'                Return _constraints
'            End Get
'        End Property


'        Public Property FileType() As String
'            Get
'                Return _fileType
'            End Get
'            Set(ByVal value As String)
'                _fileType = value
'            End Set
'        End Property
'        Private _fileType As String

'    End Class

'End Namespace