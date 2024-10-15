Imports System.Web
Imports PCAxis.Web.Core
Imports System.Xml.XPath
Imports System.Xml
Imports System.Collections.Concurrent

Namespace CommandBar.Plugin

    ''' <summary>
    ''' Manages the commandbars plugins
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CommandBarPluginManager
        Private Const CACHE_PLUGINS As String = "CommandBarPlugins"

        Private Shared _stopLoadingConfiguration As Boolean = False
        Private Shared _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(CommandBarPluginManager))

#Region "Operation Property"

        Private Shared _operations As ConcurrentDictionary(Of String, CommandBarPluginInfo)

        ''' <summary>
        ''' Gets a dictionary of all loaded plugins
        ''' </summary>
        ''' <value>Dictionary of all the loaded plugins</value>
        ''' <returns>A dictionary of all the loaded plugins</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Operations() As ConcurrentDictionary(Of String, CommandBarPluginInfo)
            Get
                Return _operations
            End Get
        End Property

#End Region

#Region "Views property"

        Private Shared _views As ConcurrentDictionary(Of String, CommandBarPluginInfo)

        ''' <summary>
        ''' Gets a dictionary of all loaded views plugins
        ''' </summary>
        ''' <value>Dictionary of all the loaded plugins</value>
        ''' <returns>A dictionary of all the loaded plugins</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Views() As ConcurrentDictionary(Of String, CommandBarPluginInfo)
            Get
                Return _views
            End Get
        End Property

#End Region

#Region "File format properties"
        Private Shared _fileTypes As ConcurrentDictionary(Of String, FileType)
        Private Shared _fileFormats As ConcurrentDictionary(Of String, String)
        Private Shared _fileFormatType As ConcurrentDictionary(Of String, String)
        Private Shared _fileFormatsSorted As List(Of String)

        Public Shared ReadOnly Property FileTypes() As ConcurrentDictionary(Of String, FileType)
            Get
                Return _fileTypes
            End Get
        End Property

        Public Shared ReadOnly Property FileFormats() As ConcurrentDictionary(Of String, String)
            Get
                Return _fileFormats
            End Get
        End Property

        Public Shared ReadOnly Property FileFormatsSorted() As List(Of String)
            Get
                Return _fileFormatsSorted
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Static constructor that loads the configuration
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            LoadConfiguration()
        End Sub

        ''' <summary>
        ''' Private constructor so that you can't create any instances of the manager
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Loads the configuration files.
        ''' First the default plugins from the embedded configuration file is loaded, next the custom
        ''' configuration file is loaded. The custom configuration file contains extra plugins defined by the
        ''' user (administrator) including links.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub LoadConfiguration()

            'Create new instances of the dictionaries
            _operations = New ConcurrentDictionary(Of String, CommandBarPluginInfo)
            _views = New ConcurrentDictionary(Of String, CommandBarPluginInfo)
            _fileTypes = New ConcurrentDictionary(Of String, FileType)
            _fileFormats = New ConcurrentDictionary(Of String, String)
            _fileFormatType = New ConcurrentDictionary(Of String, String)
            _fileFormatsSorted = New List(Of String)

            _stopLoadingConfiguration = False

            Dim settings As New Xml.XmlReaderSettings()
            'Set up the settings for the xml reader to use schema validation, add the schema, make it close the file after reading and add a handler for the errors
            With settings
                .ValidationType = Xml.ValidationType.Schema
                .Schemas.Add("http://www.pc-axis.scb.se/CommandBarPluginSchema.xsd", Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.CommandBarPluginSchema)))
                .ValidationFlags = Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings
                .CloseInput = True
                AddHandler .ValidationEventHandler, AddressOf XmlValidationCallBack
            End With

            'Load the default plugins (from the embedded configuration file)
            'Dim reader As Xml.XmlReader = Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.CommandBarPlugins), settings)
            'LoadConfigurationFile(reader)
            Dim reader As Xml.XmlReader = Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.CommandBarPluginsOperations), settings)
            LoadConfigurationFile(reader, _operations)
            reader = Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.CommandBarPluginsViews), settings)
            LoadConfigurationFile(reader, _views)
            Dim doc As New XPathDocument(Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.FileConfig)))
            ReadConfigurationFile(doc)

            'Load the custom operations plugins (from the custom configuration file)
            If (Not Configuration.ConfigurationHelper.Plugins Is Nothing) AndAlso (Not String.IsNullOrEmpty(Configuration.ConfigurationHelper.Plugins.OperationsFilepath)) Then
                If IO.File.Exists(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.OperationsFilepath)) Then
                    reader = Xml.XmlReader.Create(New IO.FileStream(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.OperationsFilepath), IO.FileMode.Open, IO.FileAccess.Read), settings)
                    LoadConfigurationFile(reader, _operations)
                End If
            End If

            'Load the custom views plugins (from the custom configuration file)
            If (Not Configuration.ConfigurationHelper.Plugins Is Nothing) AndAlso (Not String.IsNullOrEmpty(Configuration.ConfigurationHelper.Plugins.ViewsFilepath)) Then
                If IO.File.Exists(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.ViewsFilepath)) Then
                    reader = Xml.XmlReader.Create(New IO.FileStream(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.ViewsFilepath), IO.FileMode.Open, IO.FileAccess.Read), settings)
                    LoadConfigurationFile(reader, _views)
                End If
            End If

            'Load the custom file types plugins (from the custom configuration file)
            If (Not Configuration.ConfigurationHelper.Plugins Is Nothing) AndAlso (Not String.IsNullOrEmpty(Configuration.ConfigurationHelper.Plugins.FileTypesFilepath)) Then
                If IO.File.Exists(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.FileTypesFilepath)) Then
                    doc = New XPathDocument(Xml.XmlReader.Create(New System.IO.StreamReader(HttpContext.Current.Server.MapPath(Configuration.ConfigurationHelper.Plugins.FileTypesFilepath))))
                    ReadConfigurationFile(doc)
                End If
            End If

            'If there was an error this will be true
            If _stopLoadingConfiguration Then
                _operations = Nothing
                _views = Nothing
                'Throw exception
                Throw New CommandBarPluginManagerException("Error loading CommandBarPlugins")
            End If

        End Sub

        Private Shared Sub LoadConfigurationFile(ByVal reader As Xml.XmlReader, ByVal list As ConcurrentDictionary(Of String, CommandBarPluginInfo))
            Dim currentPlugin As CommandBarPluginInfo = Nothing

            Using reader

                'While there is still data in the file and xml validates to the schema. 
                'If there is an error with xml file, stop proccesing
                While reader.Read AndAlso Not _stopLoadingConfiguration

                    If reader.NodeType = Xml.XmlNodeType.Element Then
                        'If it is a plugin, create a new plugin for it
                        If reader.Name = "plugin" Then
                            Dim plugin As New CommandBarPluginInfo(reader.Item("type"))
							With plugin
								.HasUI = CBool(reader.Item("hasUI"))
								.NameCode = reader.Item("nameCode")
								.Name = reader.Item("name")
								.Image = reader.Item("Image")
								.ShortcutImage = reader.Item("ShortcutImage")
								.Category = reader.Item("category")
								If Not (Integer.TryParse(reader.Item("SortOrder"), .SortOrder)) Then
									.SortOrder = 0
								End If
							End With
							list.GetOrAdd(plugin.Name, plugin)
                            currentPlugin = plugin
                        End If
                        'If it's a property then currentPlugin is already set so add the property
                        If reader.Name = "property" Then
                            currentPlugin.Properties.Add(reader.Item("key"), reader.Item("value"))
                        End If
                        If reader.Name = "constraint" Then
                            currentPlugin.Constraints.Add(reader.Item("property"), reader.Item("value"))
                        End If
                        'If reader.Name = "category" Then
                        '    currentPlugin.Category = reader.ReadElementString
                        'End If

                    End If
                End While

            End Using
        End Sub


        ''' <summary>
        ''' Reset the loaded plugins
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ResetPlugins()
            _operations.Clear()
            _operations = Nothing
            _views.Clear()
            _views = Nothing
        End Sub


        ''' <summary>
        ''' Fires if there is an error or warning in the xml file
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An <see cref="Xml.Schema.ValidationEventArgs"/> containing the event data</param>
        ''' <remarks></remarks>
        Private Shared Sub XmlValidationCallBack(ByVal sender As Object, ByVal e As Xml.Schema.ValidationEventArgs)
            Select Case e.Severity
                Case Xml.Schema.XmlSeverityType.Error
                    'if there is an error, log and stop processing it
                    _logger.Error(e.Message, e.Exception)
                    _stopLoadingConfiguration = True
                Case Xml.Schema.XmlSeverityType.Warning
                    'If it's only an warning, log it but don't stop processing
                    _logger.Warn(e.Message, e.Exception)
            End Select

        End Sub

        Public Shared Function GetFileType(ByVal fileFormat As String) As FileType
            If _fileFormatType Is Nothing Then
                LoadConfiguration()
            End If
            Dim fileType As FileType = Nothing
            Dim fileTypeType As String = ""
            If _fileFormatType.TryGetValue(fileFormat, fileTypeType) Then
                For Each ft As FileType In _fileTypes.Values
                    If ft.Type = fileTypeType Then
                        fileType = ft
                        Exit For
                    End If
                Next
            End If
            Return fileType
        End Function

        ''' <summary>
        ''' Reads the Configurationfile for filtypes to be available in the control.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub ReadConfigurationFile(ByVal doc As XPathDocument)
            'Dim doc As New XPathDocument(Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.FileConfig)))
            Dim docNav As XPathNavigator
            Dim xmlNI As XPathNodeIterator
            docNav = doc.CreateNavigator()

            Dim manager As New XmlNamespaceManager(docNav.NameTable)
            manager.AddNamespace("ns", "http://www.scb.se")

            xmlNI = docNav.Select("/ns:FileTypeList/ns:FileType", manager)
            While (xmlNI.MoveNext)
                Dim ft As New FileType()

                If (Not Convert.ToBoolean(xmlNI.Current.GetAttribute("Active", ""))) Then
                    Continue While
                End If
                ft.Category = xmlNI.Current.GetAttribute("category", "")
                If (xmlNI.Current.SelectSingleNode("ns:Type", manager).Value IsNot Nothing) Then
                    ft.Type = xmlNI.Current.SelectSingleNode("ns:Type", manager).Value
                    ft.TranslatedText = ft.Type
                End If

                If (xmlNI.Current.SelectSingleNode("ns:AssemblyQualifiedName", manager).Value IsNot Nothing) Then
                    ft.AssemblyQualifiedName = xmlNI.Current.SelectSingleNode("ns:AssemblyQualifiedName", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:Creator", manager).Value IsNot Nothing) Then
                    ft.Creator = xmlNI.Current.SelectSingleNode("ns:Creator", manager).Value
                End If

                Dim fileInfosNode As XPathNavigator = xmlNI.Current.SelectSingleNode("ns:FileInfos", manager)

                If fileInfosNode IsNot Nothing Then
                    Dim fileInfos As XPathNodeIterator = fileInfosNode.Select("ns:FileInfo", manager)
                    While (fileInfos.MoveNext)
                        If fileInfos.Current.Value IsNot Nothing Then
                            Dim format As String = fileInfos.Current.Value
                            'Dim translatedFormat As String = PxResourceManager.GetResourceManager().GetString(format, culture)
                            'ft.FileFormats.Add(format, translatedFormat)
                            '_fileFormats.Add(format, translatedFormat)
                            ft.FileFormats.Add(format, format)
                            _fileFormats.GetOrAdd(format, format)
                            _fileFormatType.GetOrAdd(format, ft.Type)
                            _fileFormatsSorted.Add(format)
                        End If
                    End While
                End If

                If (xmlNI.Current.SelectSingleNode("ns:WebControl", manager).Value IsNot Nothing) Then
                    ft.WebControl = xmlNI.Current.SelectSingleNode("ns:WebControl", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:MimeType", manager).Value IsNot Nothing) Then
                    ft.MimeType = xmlNI.Current.SelectSingleNode("ns:MimeType", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:FileExtension", manager).Value IsNot Nothing) Then
                    ft.FileExtension = xmlNI.Current.SelectSingleNode("ns:FileExtension", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:CssClass", manager).Value IsNot Nothing) Then
                    ft.CssClass = xmlNI.Current.SelectSingleNode("ns:CssClass", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:Image", manager).Value IsNot Nothing) Then
                    ft.Image = xmlNI.Current.SelectSingleNode("ns:Image", manager).Value
                End If

                If (xmlNI.Current.SelectSingleNode("ns:ShortcutImage", manager).Value IsNot Nothing) Then
                    ft.ShortcutImage = xmlNI.Current.SelectSingleNode("ns:ShortcutImage", manager).Value
                End If

                _fileTypes.AddOrUpdate(ft.Type, ft, Function(key, value) ft)
            End While
        End Sub

    End Class

End Namespace