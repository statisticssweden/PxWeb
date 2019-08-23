'Namespace CommandBar.Plugin
'    ''' <summary>
'    ''' Represents an imagegroup
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Class CommandBarPluginImageGroup

'        Private _name As String
'        ''' <summary>
'        ''' Gets or sets the name of the imagegroup
'        ''' </summary>
'        ''' <value>Name of the imagegroup</value>
'        ''' <returns>The name of the imagegroup</returns>
'        ''' <remarks></remarks>
'        Public Property Name() As String
'            Get
'                Return _name
'            End Get
'            Set(ByVal value As String)
'                _name = value
'            End Set
'        End Property


'        Private _src As String
'        ''' <summary>
'        ''' Gets or sets the path to the imagegroup image
'        ''' </summary>
'        ''' <value>Path to the imagegroup image</value>
'        ''' <returns>The path to the imagegroup image</returns>
'        ''' <remarks></remarks>
'        Public Property Src() As String
'            Get
'                Return _src
'            End Get
'            Set(ByVal value As String)
'                _src = value
'            End Set
'        End Property


'        Private _filetype As String
'        Public Property FileType() As String
'            Get
'                Return _filetype
'            End Get
'            Set(ByVal value As String)
'                _filetype = value
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


'        Private _pluginsRefs As New List(Of String)
'        ''' <summary>
'        ''' Gets a list of the names of plugins and fileformats that the imagegroup contains
'        ''' </summary>
'        ''' <value>List of the names of plugins and fileformats that the imagegroup contains</value>
'        ''' <returns>A list of the names of plugins and fileformats that the imagegroup contains</returns>
'        ''' <remarks></remarks>
'        Public ReadOnly Property PluginRefs() As List(Of String)
'            Get
'                Return _pluginsRefs
'            End Get
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


'    End Class

'End Namespace