''' <summary>
''' Holds information about a specifik fileformat.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class FileType
    Dim _type As String
    ''' <summary>
    ''' Unique id.
    ''' Used to identify the filetype.
    ''' </summary>
    ''' <remarks>This id must be unique for every file type.</remarks>
    Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Dim _translatedText As String
    ''' <summary>
    ''' Translated text used to show in webcontrols.
    ''' </summary>
    <Obsolete()> _
    Property TranslatedText() As String
        Get
            Return _translatedText
        End Get
        Set(ByVal value As String)
            _translatedText = value
        End Set
    End Property

    Dim _assemblyQualifiedName As String
    ''' <summary>
    ''' Contains the qualified name of the class used to serialize the file.
    ''' </summary>
    ''' <remarks>The DLL must be in the <b>bin</b> folder.</remarks>
    Property AssemblyQualifiedName() As String
        Get
            Return _assemblyQualifiedName
        End Get
        Set(ByVal value As String)
            _assemblyQualifiedName = value
        End Set
    End Property

    Dim _fileInfo As String
    ''' <summary>
    ''' Contains the qualified name of the class used to store information about fileformats.
    ''' </summary>
    ''' <remarks>The DLL must be in the <b>bin</b> folder.</remarks>
    Property FileInfo() As String
        Get
            Return _fileInfo
        End Get
        Set(ByVal value As String)
            _fileInfo = value
        End Set
    End Property

    Dim _webControl As String
    ''' <summary>
    ''' Contains the name of the webcontrol that matches the serializer.
    ''' </summary>
    Property WebControl() As String
        Get
            Return _webControl
        End Get
        Set(ByVal value As String)
            _webControl = value
        End Set
    End Property

    Dim _mimeType As String
    ''' <summary>
    ''' MIME-type to be used in the Response stream
    ''' </summary>
    Property MimeType() As String
        Get
            Return _mimeType
        End Get
        Set(ByVal value As String)
            _mimeType = value
        End Set
    End Property

    Dim _fileExtension As String
    ''' <summary>
    ''' MIME-type to be used in the Response stream
    ''' </summary>
    Property FileExtension() As String
        Get
            Return _fileExtension
        End Get
        Set(ByVal value As String)
            _fileExtension = value
        End Set
    End Property

    Dim _cssClass As String
    ''' <summary>
    ''' CssClass used to get graphical representation of the filetype.
    ''' </summary>
    Property CssClass() As String
        Get
            Return _cssClass
        End Get
        Set(ByVal value As String)
            _cssClass = value
        End Set
    End Property

    Dim _fileFormats As New Dictionary(Of String, String)
    ''' <summary>
    ''' Contains all supported file formats with translated text.
    ''' </summary>
    ReadOnly Property FileFormats() As Dictionary(Of String, String)
        Get
            Return _fileFormats
        End Get
    End Property

    Private _image As String
    ''' <summary>
    ''' The image that will be displayed for the file type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Image() As String
        Get
            Return _image
        End Get
        Set(ByVal value As String)
            _image = value
        End Set
    End Property

    Private _shortcutImage As String
    ''' <summary>
    ''' The shortcut image for the file type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShortcutImage() As String
        Get
            Return _shortcutImage
        End Get
        Set(ByVal value As String)
            _shortcutImage = value
        End Set
    End Property

    Private _category As String
    ''' <summary>
    ''' The category for the plugin
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Category() As String
        Get
            Return _category
        End Get
        Set(ByVal value As String)
            _category = value
        End Set
    End Property

    Private _creator As String
    ''' <summary>
    ''' The category for the plugin
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Creator() As String
        Get
            Return _creator
        End Get
        Set(ByVal value As String)
            _creator = value
        End Set
    End Property

End Class
