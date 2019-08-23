'Imports System.IO
'Imports System.Xml
'Imports System.Collections.Generic
'Imports PCAxis.Menu

'Namespace PCAxis.Paxiom
'    ''' <summary>
'    ''' Class that generates the XML-structure which will be used as input by Menu.
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Class MenuXmlCreator
'#Region "Private classes"

'        ''' <summary>
'        ''' Represents a Language element
'        ''' </summary>
'        ''' <remarks></remarks>
'        Private Class MenuLanguage
'            Public Language As String
'            Public DefaultLanguage As Boolean
'            Public RootMenuItem As PxMenuItem

'            Public Sub New()
'                RootMenuItem = New PxMenuItem
'            End Sub
'        End Class

'        ''' <summary>
'        ''' Contains information about a PX-file
'        ''' </summary>
'        ''' <remarks></remarks>
'        Private Class PxFileInfo
'            Public Title As String
'            Public Size As String
'            Public Modified As String
'            Public Updated As String
'            Public LanguageList As List(Of String)
'            Public VariableList As List(Of VariableInfo)

'            Public Sub New()
'                LanguageList = New List(Of String)
'                VariableList = New List(Of VariableInfo)
'            End Sub
'        End Class

'        ''' <summary>
'        ''' Contains information about a variable
'        ''' </summary>
'        ''' <remarks></remarks>
'        Private Class VariableInfo
'            Public Name As String
'            Public Values As String
'            Public NumberOfValues As Integer
'        End Class
'#End Region

'#Region "Private members"
'        Private _root As DirectoryInfo
'        Private _destination As String
'        Private _xdoc As XmlDocument
'        Private _menuNode As XmlNode
'        Private _selection As Integer = 0
'        Private _multiLanguage As Boolean
'        Private _language As String
'        Private _LanguageRootMenuItem As PxMenuItem
'        Private _Langauges As List(Of MenuLanguage)
'#End Region

'#Region "Constructors"
'        ''' <summary>
'        ''' Constructor
'        ''' </summary>
'        ''' <param name="rootDirectory">The directory that will be the root for the XML-structure</param>
'        ''' <param name="destination">The destination XML-file</param>
'        ''' <param name="multiLanguage">
'        ''' True if multiple languages exists. Only files and folders in the selected language will be added to the XML-structure.
'        ''' False if all files and folders shall be added to the XML-structure.
'        ''' </param>
'        ''' <remarks></remarks>
'        Public Sub New(ByVal rootDirectory As String, ByVal destination As String, ByVal multiLanguage As Boolean)

'            'Verify that the root directory exists
'            If String.IsNullOrEmpty(rootDirectory) Then
'                Throw New DirectoryNotFoundException("Root directory not specified")
'            End If
'            If Not Directory.Exists(rootDirectory) Then
'                Throw New DirectoryNotFoundException("Unknown root directory: " & rootDirectory)
'            End If

'            'Verify that the destination directory exists
'            If String.IsNullOrEmpty(destination) Then
'                Throw New FileNotFoundException("Destination not specified")
'            End If
'            If Not Directory.Exists(Path.GetDirectoryName(destination)) Then
'                Throw New DirectoryNotFoundException("Unknown destination directory: " & Path.GetDirectoryName(destination))
'            End If

'            'Verify that the destination file is specified and correct
'            If String.IsNullOrEmpty(Path.GetFileName(destination)) Then
'                Throw New FileNotFoundException("Destination file not specified")
'            End If
'            If Not Path.HasExtension(destination) Then
'                Throw New FileNotFoundException("Destination file not specified")
'            End If
'            If Not Path.GetExtension(destination).ToLower.Equals(".xml") Then
'                Throw New FileNotFoundException("Illegal destination file type, must be xml")
'            End If

'            'Delete if destination file already exists
'            If File.Exists(destination) Then
'                File.Delete(destination)
'            End If

'            _root = New DirectoryInfo(rootDirectory)
'            _destination = destination
'            _multiLanguage = multiLanguage
'            _xdoc = New XmlDocument
'            _Langauges = New List(Of MenuLanguage)

'            If File.Exists(_destination) Then
'                _xdoc.Load(_destination)
'            Else
'                _xdoc.LoadXml("<?xml version=""1.0"" encoding=""utf-8"" ?><Menu></Menu>")
'            End If

'        End Sub

'        ''' <summary>
'        ''' Adds the Menu object-structure for the specified language 
'        ''' </summary>
'        ''' <param name="language">The language code</param>
'        ''' <param name="defaultLanguage">If the language is the default language or not</param>
'        ''' <remarks>The language object structure is added to the internal list of languages</remarks>
'        Public Sub AddLanguage(ByVal language As String, ByVal defaultLanguage As Boolean)
'            Dim menuLang As New MenuLanguage

'            _language = language.ToLower
'            '_LanguageRootMenuItem = New PxMenu.MenuItem("Databases", "", "0", _selection.ToString)
'            _LanguageRootMenuItem = New PxMenuItem("Databases", "", "0", _selection.ToString(), "")
'            _selection = _selection + 1
'            AppendDirectoryNodes(_root, _LanguageRootMenuItem)

'            menuLang.Language = _language
'            menuLang.DefaultLanguage = defaultLanguage
'            menuLang.RootMenuItem = _LanguageRootMenuItem

'            _Langauges.Add(menuLang)
'        End Sub


'        ''' <summary>
'        ''' Appends the Menu object-structure for the directory, starting at the directory given by "dir".
'        ''' The directory is only appended if at least one valid PX-file exists in the directory
'        ''' or in its subfolders.
'        ''' </summary>
'        ''' <param name="dir">The directory to append in the XML structure</param>
'        ''' <param name="parent">The parent directory (MenuItem) to dir</param>
'        ''' <returns>True if valid PX-files exists in this folder or in its subfolders, else false
'        ''' </returns>
'        ''' <remarks>The function calls itself recurivly to add subdirectories.</remarks>
'        Private Function AppendDirectoryNodes(ByVal dir As DirectoryInfo, ByVal parent As PxMenuItem) As Boolean
'            Dim sort As Integer = 0
'            Dim n As PxMenuItem
'            Dim filesInSubfolders As Boolean = False
'            Dim filesInThisfolder As Boolean = False
'            Dim pxInfo As PxFileInfo

'            'Add directories
'            For Each di As DirectoryInfo In dir.GetDirectories
'                n = AppendDirectoryNode(parent, sort, di)
'                If AppendDirectoryNodes(di, n) = True Then
'                    'Valid PX-files exists in some of my subfolders
'                    filesInSubfolders = True
'                    sort = sort + 1
'                Else
'                    'No valid files exists in the directory - Remove it again!
'                    parent.SubItems.Remove(n)
'                End If
'            Next

'            'Add files
'            For Each f As FileInfo In dir.GetFiles
'                Select Case f.Extension.ToLower
'                    Case ".px"
'                        pxInfo = GetPxFileInfo(f)

'                        If ValidPxFile(pxInfo) Then
'                            If AddPxLink(parent, sort, f, pxInfo) Then
'                                sort = sort + 1
'                                filesInThisfolder = True
'                            End If
'                        End If
'                    Case ".link"
'                        If AddUrlLink(parent, sort, f) Then
'                            sort = sort + 1
'                            filesInThisfolder = True
'                        End If
'                End Select
'            Next

'            If (filesInSubfolders = False) And (filesInThisfolder = False) Then
'                'No valid PX-files exists - the folder shall not be appended to the XML-structure
'                Return False
'            Else
'                'Valid PX-files exists - Append the folder to the XML-structure!
'                Return True
'            End If
'        End Function

'        ''' <summary>
'        ''' Appends the given directory to the Menu object-structure.
'        ''' </summary>
'        ''' <param name="parent">The parent directory (MenuItem) for the dirctory given by "di"</param>
'        ''' <param name="sort">The sortorder of the directory</param>
'        ''' <param name="di">The directory to add</param>
'        ''' <returns>The added MenuItem</returns>
'        ''' <remarks>
'        ''' If Alias-files exists in the directory the function reads the name
'        ''' from the file instead of just taking the directoryname
'        '''  </remarks>
'        Private Function AppendDirectoryNode(ByVal parent As PxMenuItem, ByVal sort As Integer, ByVal di As DirectoryInfo) As PxMenuItem
'            Dim aliasFile As String = ""
'            Dim pres As String = ""
'            Dim desc As String = ""

'            'Check for Alias.txt for the selected language
'            aliasFile = di.FullName & "\Alias_" & _language & ".txt"
'            If File.Exists(aliasFile) Then
'                pres = ReadAliasName(aliasFile)
'            End If

'            'Check for Alias.txt for fallback language
'            If pres.Length = 0 Then
'                aliasFile = di.FullName & "\Alias.txt"
'                If File.Exists(aliasFile) Then
'                    pres = ReadAliasName(aliasFile)
'                End If
'            End If

'            'Take the directory name
'            If pres.Length = 0 Then
'                pres = di.Name
'            End If

'            desc = di.Name

'            Return AddMenuItem(parent, sort, pres, desc)

'        End Function

'        ''' <summary>
'        ''' Adds a MenuItem to the Menu object-structure
'        ''' </summary>
'        ''' <param name="parent">The parent of the MenuItem</param>
'        ''' <param name="sort">The sortorder for the MenuItem</param>
'        ''' <param name="presentation">The presentation text for the MenuItem</param>
'        ''' <param name="description">The description of the MenuItem</param>
'        ''' <returns>The added MenuItem object</returns>
'        ''' <remarks></remarks>
'        Private Function AddMenuItem(ByVal parent As PxMenuItem, _
'                                     ByVal sort As Integer, _
'                                     ByVal presentation As String, _
'                                     ByVal description As String) As PxMenuItem

'            'Dim mi As New PxMenu.MenuItem(presentation, description, sort.ToString("00000"), _selection.ToString)
'            Dim mi As New PxMenuItem(presentation, description, presentation, _selection.ToString, "")
'            _selection = _selection + 1

'            parent.SubItems.Add(mi)
'            Return mi
'        End Function

'        ''' <summary>
'        ''' Adds a link-object for a Px-file (type=table) to the Menu object-structure
'        ''' </summary>
'        ''' <param name="parent">The parent to the link</param>
'        ''' <param name="sort">The sortorder of the link</param>
'        ''' <param name="f">FileInfo for the px-file that the link represents</param>
'        ''' <param name="pxInfo">Information about the PX-file</param>
'        ''' <returns>True if link was added to the XML-structure, else false.</returns>
'        ''' <remarks></remarks>
'        Private Function AddPxLink(ByVal parent As PxMenuItem, _
'                                   ByVal sort As Integer, _
'                                   ByVal f As FileInfo, _
'                                   ByVal pxInfo As PxFileInfo) As Boolean
'            Dim lnk As Link
'            Dim i As Integer = 1

'            If Not pxInfo Is Nothing Then
'                If Not String.IsNullOrEmpty(pxInfo.Title) Then
'                    'lnk = New PxMenu.Link(pxInfo.Title, sort.ToString("00000"), "px", f.Name)
'                    'TODO Fix try parse the dates
'                    lnk = New TableLink(pxInfo.Title, "", sort.ToString("00000"), f.FullName, "", LinkType.PX, TableStatus.AccessibleToAll, Nothing, "", "", "", PresCategory.Official)

'                    'Add attributes for variables/values
'                    For Each varInfo As VariableInfo In pxInfo.VariableList
'                        lnk.SetAttribute("Var" & i.ToString & "Name", varInfo.Name)
'                        lnk.SetAttribute("Var" & i.ToString & "Values", varInfo.Values)
'                        lnk.SetAttribute("Var" & i.ToString & "NumberOfValues", varInfo.NumberOfValues.ToString)
'                        i = i + 1
'                    Next

'                    'Size attribute
'                    lnk.SetAttribute("size", pxInfo.Size)
'                    'Modified attribute
'                    If Not String.IsNullOrEmpty(pxInfo.Modified) Then
'                        lnk.SetAttribute("modified", pxInfo.Modified)
'                    End If
'                    If Not String.IsNullOrEmpty(pxInfo.Updated) Then
'                        lnk.SetAttribute("updated", pxInfo.Updated)
'                    End If
'                    parent.AddSubItem(lnk)

'                    Return True
'                End If
'            End If

'            Return False
'        End Function

'        ''' <summary>
'        ''' Adds a link to an external webpage (type=url) to the Menu object-structure
'        ''' </summary>
'        ''' <param name="parent">The parent MenuItem that the link shall be added to</param>
'        ''' <param name="sort">Sortcode for the link</param>
'        ''' <param name="f">Fileinfo for the link-file</param>
'        ''' <returns>True if the link was added to the Menu object-structure, else false</returns>
'        ''' <remarks></remarks>
'        Private Function AddUrlLink(ByVal parent As PxMenuItem, _
'                                    ByVal sort As Integer, _
'                                    ByVal f As FileInfo) As Boolean
'            Dim lnk As Link
'            Dim prestext As String = ""
'            Dim url As String = ""

'            If ValidUrlLinkFile(f) Then
'                If GetUrlLinkInfo(f, prestext, url) Then
'                    If Not String.IsNullOrEmpty(prestext) And Not String.IsNullOrEmpty(url) Then
'                        'lnk = New PxMenu.Link(prestext, sort.ToString("00000"), "url", url)
'                        lnk = New Url(prestext, sort.ToString("00000"), url, "", PresCategory.Official, url, LinkPres.Both)
'                        parent.AddSubItem(lnk)
'                        Return True
'                    End If
'                End If
'            End If

'            Return False
'        End Function

'        ''' <summary>
'        ''' Reads the alias name from an aliasfile
'        ''' </summary>
'        ''' <param name="path">The aliasfile to read</param>
'        ''' <returns>The alias name read from the file</returns>
'        ''' <remarks>Assumes that there are only one line of text in the aliasfile</remarks>
'        Private Function ReadAliasName(ByVal path As String) As String
'            Dim name As String = ""

'            Using reader As New System.IO.StreamReader(path, System.Text.Encoding.Default)
'                If reader.Peek <> -1 Then
'                    name = reader.ReadLine()
'                End If
'            End Using

'            Return name
'        End Function

'        ''' <summary>
'        ''' Collects information about the given PX-file
'        ''' </summary>
'        ''' <param name="f">FileInfo for the file to get information about</param>
'        ''' <returns>PxFileInfo object containing the information</returns>
'        ''' <remarks></remarks>
'        Private Function GetPxFileInfo(ByVal f As FileInfo) As PXFileInfo
'            Try
'                Dim pxInfo As New PxFileInfo
'                Dim px As New PCAxis.Paxiom.PXFileBuilder()

'                px.SetPath(f.FullName)

'                If Not px.BuildForSelection() Then
'                    Return Nothing
'                End If

'                px.Model.Meta.SetLanguage(_language)

'                If Not String.IsNullOrEmpty(px.Model.Meta.Description) Then
'                    pxInfo.Title = px.Model.Meta.Description
'                Else
'                    pxInfo.Title = px.Model.Meta.Title
'                End If

'                If Not String.IsNullOrEmpty(px.Model.Meta.ContentInfo.LastUpdated) Then
'                    Dim modified As String = px.Model.Meta.ContentInfo.LastUpdated
'                    'px.Model.Meta.ContentInfo.LastUpdated is supposed to have the format "CCYYMMDD hh:mm"
'                    If modified.Length = 14 Then
'                        Try
'                            Dim dtMod As New DateTime(CInt(modified.Substring(0, 4)), _
'                                                      CInt(modified.Substring(4, 2)), _
'                                                      CInt(modified.Substring(6, 2)), _
'                                                      CInt(modified.Substring(9, 2)), _
'                                                      CInt(modified.Substring(12, 2)), _
'                                                      0)
'                            pxInfo.Modified = dtMod.ToShortDateString
'                        Catch ex As Exception
'                            pxInfo.Modified = px.Model.Meta.ContentInfo.LastUpdated
'                        End Try
'                    End If
'                Else
'                    pxInfo.Modified = px.Model.Meta.ContentInfo.LastUpdated
'                End If

'                pxInfo.Updated = f.LastWriteTime.ToShortDateString
'                pxInfo.Size = CStr(CInt(f.Length / 1000)) & " kb"

'                If Not String.IsNullOrEmpty(px.Model.Meta.Language) Then
'                    pxInfo.LanguageList.Add(px.Model.Meta.Language)
'                End If

'                Dim langs() As String = px.Model.Meta.GetAllLanguages

'                If Not langs Is Nothing Then
'                    For Each lang As String In langs
'                        If Not lang.Equals("default") Then
'                            If Not pxInfo.LanguageList.Contains(lang) Then
'                                pxInfo.LanguageList.Add(lang)
'                            End If
'                        End If
'                    Next
'                End If

'                'Add info about variables/values
'                For Each variable As Variable In px.Model.Meta.Variables
'                    Dim varInfo As New VariableInfo
'                    Dim vals As New System.Text.StringBuilder
'                    Dim i As Integer = 1

'                    varInfo.Name = variable.Name

'                    For Each value As Value In variable.Values
'                        If (variable.Values.Count = i And variable.Values.Count > 6) Then
'                            vals.Append("... ")
'                        End If
'                        If (i < 5 Or i = variable.Values.Count) Then
'                            vals.Append(value.Value + ", ")
'                        End If
'                        i = i + 1
'                    Next

'                    varInfo.Values = vals.ToString
'                    varInfo.NumberOfValues = variable.Values.Count

'                    pxInfo.VariableList.Add(varInfo)
'                Next

'                Return pxInfo
'            Catch ex As Exception
'                Return Nothing
'            End Try

'        End Function

'        ''' <summary>
'        ''' Validates if it is an valid PX-file. If the file is valid it shall be added to the XML-structure.
'        ''' </summary>
'        ''' <param name="pxInfo">Information about the PX-file</param>
'        ''' <returns>True if valid, else false</returns>
'        ''' <remarks></remarks>
'        Private Function ValidPxFile(ByVal pxInfo As PXFileInfo) As Boolean
'            If pxInfo Is Nothing Then
'                Return False
'            End If

'            If _multiLanguage Then
'                For Each lang As String In pxInfo.LanguageList
'                    If lang.Equals(_language) Then
'                        Return True
'                    End If
'                Next
'                Return False
'            Else
'                Return True
'            End If
'        End Function

'        ''' <summary>
'        ''' Checks if the specified file shall be added to the Menu object-structure or not
'        ''' </summary>
'        ''' <param name="f">Fileinfo for the file</param>
'        ''' <returns>True if the file shall be added, else false</returns>
'        ''' <remarks></remarks>
'        Private Function ValidUrlLinkFile(ByVal f As FileInfo) As Boolean
'            Dim name As String = ""
'            Dim lang As String = ""
'            Dim index As Integer

'            name = Path.GetFileNameWithoutExtension(f.Name)
'            index = name.IndexOf("_")

'            If index <> -1 Then
'                'Language link-file
'                '------------------
'                If name.Length > index + 1 Then
'                    lang = name.Substring(index + 1, name.Length - (index + 1))
'                    If _language.Length > 0 Then
'                        If _language.Equals(lang.ToLower) Then
'                            'Link-file for my language - Valid file!
'                            Return True
'                        End If
'                    End If
'                End If
'            Else
'                'Fallback language link-file
'                '---------------------------
'                If _language.Length > 0 Then
'                    If File.Exists(Path.Combine(f.DirectoryName, name & "_" & _language & ".link")) Then
'                        'This link-file exists in my language - don´t use this file
'                        Return False
'                    Else
'                        'There are no version of this file in my language - use this file
'                        Return True
'                    End If
'                Else
'                    Return True
'                End If
'            End If

'            Return False
'        End Function

'        ''' <summary>
'        ''' Reads the specified link-file
'        ''' </summary>
'        ''' <param name="f">FileInfo for the file</param>
'        ''' <param name="prestext">Outparameter for the link presentation text</param>
'        ''' <param name="url">Outparameter for the link url</param>
'        ''' <returns>True if presentationtext and url was read successfully, else false</returns>
'        ''' <remarks></remarks>
'        Private Function GetUrlLinkInfo(ByVal f As FileInfo, ByRef prestext As String, ByRef url As String) As Boolean
'            Dim str As String = ""
'            Dim pos1, pos2 As Integer

'            Try
'                Using reader As New System.IO.StreamReader(f.FullName, System.Text.Encoding.Default)
'                    If reader.Peek <> -1 Then
'                        str = reader.ReadLine()
'                        If Not String.IsNullOrEmpty(str) Then
'                            'Read presentation text
'                            pos1 = str.IndexOf("""")
'                            If pos1 <> -1 Then
'                                pos1 = pos1 + 1
'                                pos2 = str.IndexOf("""", pos1)
'                                If pos2 <> -1 Then
'                                    prestext = str.Substring(pos1, pos2 - (pos1))
'                                End If
'                            End If

'                            'Read url
'                            pos1 = str.IndexOf("""", pos2 + 1)
'                            If pos1 <> -1 Then
'                                pos1 = pos1 + 1
'                                pos2 = str.IndexOf("""", pos1)
'                                If pos2 <> -1 Then
'                                    url = str.Substring(pos1, pos2 - (pos1))
'                                End If
'                            End If

'                            Return True

'                        End If
'                    End If
'                End Using

'                Return False

'            Catch ex As Exception
'                Return False
'            End Try

'        End Function

'        ''' <summary>
'        ''' Saves the Menu object-structures for all added languages as XML to the destination file
'        ''' </summary>
'        ''' <remarks></remarks>
'        Public Sub Save()
'            Dim elem As XmlElement
'            Dim ln As XmlNode

'            For Each menuLang As MenuLanguage In _Langauges
'                ln = CreateLanguageTag(menuLang)
'                elem = CType(_xdoc.ReadNode(menuLang.RootMenuItem.GetAsXML(100).CreateReader()), XmlElement)
'                ln.AppendChild(elem)
'                GetMenuNode().AppendChild(ln)
'            Next

'            _xdoc.Save(_destination)
'        End Sub

'        ''' <summary>
'        ''' Creates a language XML-element 
'        ''' </summary>
'        ''' <param name="menuLang">
'        ''' MenuLanguage object representing the language to create an element for
'        ''' </param>
'        ''' <returns>The created language element</returns>
'        ''' <remarks></remarks>
'        Private Function CreateLanguageTag(ByVal menuLang As MenuLanguage) As XmlNode
'            Dim ln As Xml.XmlNode
'            Dim att As Xml.XmlAttribute

'            ln = _xdoc.CreateNode(Xml.XmlNodeType.Element, "Language", "")

'            'lang attribute
'            att = _xdoc.CreateAttribute("lang", "")
'            att.Value = menuLang.Language
'            ln.Attributes.Append(att)

'            'default attribute
'            att = _xdoc.CreateAttribute("default", "")
'            If menuLang.DefaultLanguage = True Then
'                att.Value = "yes"
'            Else
'                att.Value = "no"
'            End If
'            ln.Attributes.Append(att)

'            Return ln
'        End Function

'        ''' <summary>
'        ''' Returns the root (Menu) node
'        ''' </summary>
'        ''' <returns>The Menu node</returns>
'        ''' <remarks></remarks>
'        Private Function GetMenuNode() As XmlNode
'            If _menuNode Is Nothing Then
'                Dim xpath As String = String.Format("//Menu")
'                _menuNode = _xdoc.SelectSingleNode(xpath)
'            End If

'            Return _menuNode
'        End Function

'#End Region


'    End Class
'End Namespace
