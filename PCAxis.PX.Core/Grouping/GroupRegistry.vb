Imports System.IO
Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Registry for all groupings
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GroupRegistry

#Region "IniReader Class"
        ''' <summary>
        ''' Ini-file reader. Used by the groupregistry when reading from the .vs and .agg files
        ''' </summary>
        ''' <remarks></remarks>
        Private Class IniReader
            Private Const BUFFER_SIZE As Integer = 1024
#Region "API Calls"
            Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
            Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, _
            ByVal lpKeyName As String, ByVal lpDefault As String, _
            ByVal lpReturnedString As String, ByVal nSize As Int32, _
            ByVal lpFileName As String) As Int32
#End Region
#Region "Read overloads"
            Public Overloads Function Read(ByVal path As String, _
                                           ByVal sectionName As String, _
                                           ByVal keyName As String) As String
                ' overload 1 assumes zero-length default
                Return Read(path, sectionName, keyName, "")
            End Function

            Public Overloads Function Read(ByVal path As String, _
                                           ByVal sectionName As String) As String
                ' overload 2 returns all keys in a given section of the given file
                Return Read(path, sectionName, Nothing, "")
            End Function

            Public Overloads Function Read(ByVal path As String) As String
                ' overload 3 returns all section names given just path
                Return Read(path, Nothing, Nothing, "")
            End Function
#End Region

#Region "Public methods"

            Public Overloads Function Read(ByVal path As String, _
                                           ByVal sectionName As String, _
                                           ByVal keyName As String, _
                                           ByVal defaultValue As String) As String
                Dim n As Integer
                Dim bufferSize As Integer = BUFFER_SIZE
                Dim data As String

                data = New String(" "c, bufferSize)
                n = GetPrivateProfileString(sectionName, keyName, defaultValue, data, data.Length, path)

                If n > 0 Then

                    While n >= (bufferSize - 2)
                        'Buffer was to small. Grow buffer and read again
                        bufferSize = bufferSize + BUFFER_SIZE
                        data = New String(" "c, bufferSize)
                        n = GetPrivateProfileString(sectionName, keyName, defaultValue, data, data.Length, path)
                    End While

                    Return data.Substring(0, n)
                Else
                    Return ""
                End If
            End Function

            Public Function GetAllKeysInSection(ByVal path As String, ByVal section As String) As System.Collections.Specialized.StringCollection
                Dim col As New System.Collections.Specialized.StringCollection
                Dim value As String
                Dim keys() As String

                value = Read(path, section) ' get all keys in section
                value = value.Replace(ControlChars.NullChar, "|"c) ' change embedded NULLs to pipe chars
                keys = value.Split("|"c)

                For i As Integer = 0 To keys.Length - 1
                    If Not String.IsNullOrEmpty(keys(i)) Then
                        col.Add(keys(i))
                    End If
                Next

                Return col
            End Function
#End Region
        End Class
#End Region


#Region "GroupingInfoMeta class"
        ''' <summary>
        ''' Holds metadata about a specific GroupingInfo-object
        ''' </summary>
        ''' <remarks></remarks>
        Private Class GroupingInfoMeta
            ''' <summary>
            ''' The Valueset the GroupInfo object belong to
            ''' </summary>
            ''' <remarks></remarks>
            Public Valueset As Valueset
            ''' <summary>
            ''' Path to the .agg-file 
            ''' </summary>
            ''' <remarks></remarks>
            Public Path As String
            ''' <summary>
            ''' If the GroupInfo belongs to the default grouping directory or not
            ''' </summary>
            ''' <remarks></remarks>
            Public DefaultDirectory As Boolean
        End Class
#End Region

        Private Shared _groupRegistry As GroupRegistry
        Private _groupingsLoaded As Boolean = False
        Private _groupingDir As String
        Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(GroupRegistry))

        ''' <summary>
        ''' Contains loaded valuesets. Used to determine if a valueset file has been loaded into the registry or not.
        ''' Key = Full path to the valueset file (directory path + filename)
        ''' Value = The valueset
        ''' </summary>
        ''' <remarks></remarks>
        Private _valuesets As New Dictionary(Of String, Valueset)

        ''' <summary>
        ''' Tells which valueset a domain belongs to
        ''' Key = Domain name
        ''' Value = Dictionary of valuesets. Key = Path without filename to the valueset. Value = The valueset.
        ''' </summary>
        ''' <remarks></remarks>
        Private _domains As New Dictionary(Of String, Dictionary(Of String, Valueset))

        ''' <summary>
        ''' Tells which GroupingInfoMeta that belongs to the GroupingInfo object
        ''' Key = GroupingInfo object
        ''' Value = Corresponding GroupingInfoMeta object
        ''' </summary>
        ''' <remarks></remarks>
        Private _groupingInfoMeta As New Dictionary(Of GroupingInfo, GroupingInfoMeta)

        ''' <summary>
        ''' Path to the groupings files
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _groupingsPath As String = "~/"

        ''' <summary>
        ''' If strict check will be done for if aggregations belongs to the current valueset or not 
        ''' </summary>
        ''' <remarks></remarks>
        Private _strict As Boolean = True

        ''' <summary>
        ''' Shared constructor
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            Try
                'Get path to the groupings files if it is defined in web.config
                If PCAxis.Paxiom.Configuration.ConfigurationHelper.GroupingSection IsNot Nothing Then
                    If PCAxis.Paxiom.Configuration.ConfigurationHelper.GroupingSection.FilesPath IsNot Nothing Then
                        _groupingsPath = PCAxis.Paxiom.Configuration.ConfigurationHelper.GroupingSection.FilesPath
                    End If
                End If

            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Path to the groupings files folder
        ''' </summary>
        ''' <value>Path to the groupings files folder</value>
        ''' <returns>Path to the groupings files folder</returns>
        ''' <remarks>Setting the property will override the web.config setting for the groupings files path</remarks>
        Public Shared Property GroupingsPath() As String
            Get
                Return _groupingsPath
            End Get
            Set(ByVal value As String)
                _groupingsPath = value
            End Set
        End Property

        ''' <summary>
        ''' If strict check will be performed on aggregations before they are added to a valueset or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Strict() As Boolean
            Get
                Return _strict
            End Get
            Set(ByVal value As Boolean)
                _strict = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the GroupRegistry object
        ''' </summary>
        ''' <returns>The GroupRegistry object</returns>
        ''' <remarks></remarks>
        Public Shared Function GetRegistry() As GroupRegistry
            If _groupRegistry Is Nothing Then
                _groupRegistry = New GroupRegistry
            End If

            Return _groupRegistry
        End Function

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks>
        ''' Constructor is protected because GroupRegistry is implemented with the 
        ''' Singelton design pattern
        ''' </remarks>
        Protected Sub New()
            _logger.InfoFormat("GroupRegistry created")

            If IO.Path.IsPathRooted(_groupingsPath) Then
                _groupingDir = _groupingsPath
            Else
                _groupingDir = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _groupingsPath)
            End If

            If Not Directory.Exists(_groupingDir) Then
                _logger.WarnFormat("Groupingdirectory does not exists {0}", _groupingDir)
                _groupingDir = String.Empty
            End If

        End Sub

        ''' <summary>
        ''' Loads all available groupings into the registry.
        ''' </summary>
        ''' <remarks>
        '''Groupings are loaded from the directory specified in the config-file
        ''' </remarks>
        Public Sub LoadGroupings()
            LoadGroupings(Nothing)
        End Sub


        ''' <summary>
        ''' Loads available groupings asynchronously
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadGroupingsAsync()
            If Not System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf LoadGroupings)) Then
                'The task could not be queued...
                _logger.ErrorFormat("Error when queing work to load groupings from {0}", _groupingDir)
            End If
        End Sub

        ''' <summary>
        ''' Load Groupings
        ''' </summary>
        ''' <param name="stateInfo"></param>
        ''' <remarks></remarks>
        Private Sub LoadGroupings(ByVal stateInfo As Object)
            Dim dir As DirectoryInfo

            Try
                If _groupingDir.Equals(String.Empty) Then
                    Exit Sub
                End If

                _logger.InfoFormat("Start loading groupings from {0}", _groupingDir)

                dir = New DirectoryInfo(_groupingDir)


                For Each f As FileInfo In dir.GetFiles("*", SearchOption.AllDirectories)
                    'Get avaliable groupings from the vs-files in the directory
                    If f.Extension.ToLower.Equals(".vs") Then
                        ReadValuesetFile(f, _valuesets, _domains, _groupingInfoMeta, True)
                    End If
                Next

                _groupingsLoaded = True
                _logger.InfoFormat("Finished loading groupings from {0}", _groupingDir)

            Catch ex As Exception
                _logger.ErrorFormat("Error when loading groupings from {0} : {1}", _groupingDir, ex.Message)
                Exit Sub
            End Try
        End Sub

        ''' <summary>
        ''' The registry are emptied and the (default) grouping directory are reloaded
        ''' All other groupings that may have been in the registry are deleted from the registry.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReloadGroupings()
            ReloadGroupings(Nothing)
        End Sub

        ''' <summary>
        ''' The registry are emptied and the (default) grouping directory are reloaded asynchronously
        ''' All other groupings that may have been in the registry are deleted from the registry.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReloadGroupingsAsync()
            If Not System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf ReloadGroupings)) Then
                'The task could not be queued...
                _logger.ErrorFormat("Error when queing work to reload groupings from {0}", _groupingDir)
            End If
        End Sub

        ''' <summary>
        ''' The registry are emptied and the (default) grouping directory are reloaded
        ''' All other groupings that may have been in the registry are deleted from the registry.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ReloadGroupings(ByVal stateInfo As Object)
            Dim dir As DirectoryInfo

            _groupingsLoaded = False

            Try
                'Temporary dictionaries
                Dim valuesets As New Dictionary(Of String, Valueset)
                Dim domains As New Dictionary(Of String, Dictionary(Of String, Valueset))
                Dim groupingInfoMeta As New Dictionary(Of GroupingInfo, GroupingInfoMeta)

                If _groupingDir.Equals(String.Empty) Then
                    Exit Sub
                End If

                dir = New DirectoryInfo(_groupingDir)

                For Each f As FileInfo In dir.GetFiles
                    'Get avaliable groupings from the vs-files in the directory
                    If f.Extension.ToLower.Equals(".vs") Then
                        ReadValuesetFile(f, valuesets, domains, groupingInfoMeta, True)
                    End If
                Next

                'Assign new dictionaries to members
                _valuesets = valuesets
                _domains = domains
                _groupingInfoMeta = groupingInfoMeta

                _groupingsLoaded = True
                _logger.InfoFormat("Groupings are reloaded successfully from {0}", _groupingDir)

            Catch ex As Exception
                _logger.ErrorFormat("Error when loading groupings from {0} : {1}", _groupingDir, ex.Message)
                Exit Sub
            End Try

        End Sub

        ''' <summary>
        ''' Loads the groupings in the specified path that belongs to the specified domain into the registry
        ''' </summary>
        ''' <param name="path">The path where to look for groupings</param>
        ''' <param name="domain">The domain that the groupings to load shall belong to</param>
        ''' <remarks></remarks>
        Public Sub LoadGroupingsForPathAndDomain(ByVal path As String, ByVal domain As String)
            Dim dir As DirectoryInfo

            If Not Directory.Exists(path) Then
                Exit Sub
            End If

            If domain Is Nothing Then
                Exit Sub
            End If

            dir = New DirectoryInfo(path)

            For Each f As FileInfo In dir.GetFiles
                'Get avaliable groupings from the vs-files in the directory
                If f.Extension.ToLower.Equals(".vs") Then
                    If CheckDomain(f, domain) Then
                        ReadValuesetFile(f, _valuesets, _domains, _groupingInfoMeta, False)
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' Reads and loads the specified valueset (.vs) file into the GroupRegistry.
        ''' All groupings (.agg) files in the valueset file are also loaded into the GroupRegistry.
        ''' </summary>
        ''' <param name="f">FileInfo object for the Valueset (.vs) file</param>
        ''' <param name="valuesetDict">Dictionary of valuesets</param>
        ''' <param name="domainsDict">Dictionary that tells which valueset a domain belongs to</param>
        ''' <param name="groupingInfoMetaDict">Dictionary that tells which GroupingInfoMeta that belongs to a specific GroupingInfo object</param>
        ''' <param name="defaultDirectory">Tells if the .vs file is in the (default) grouping directory or not</param>
        ''' <remarks></remarks>
        Private Sub ReadValuesetFile(ByVal f As FileInfo, _
                                     ByVal valuesetDict As Dictionary(Of String, Valueset), _
                                     ByVal domainsDict As Dictionary(Of String, Dictionary(Of String, Valueset)), _
                                     ByVal groupingInfoMetaDict As Dictionary(Of GroupingInfo, GroupingInfoMeta), _
                                     ByVal defaultDirectory As Boolean)
            Dim vs As Valueset
            Dim ini As New IniReader
            Dim col1, col2 As System.Collections.Specialized.StringCollection
            Dim code As String
            Dim value As String
            Dim domain As String
            Dim vsDict As Dictionary(Of String, Valueset)
            Dim vsPath As String
            Dim grouping As String
            Dim gi As GroupingInfo
            Dim groupingPath As String = ""
            Dim valuesetName As String
            Dim groupingName As String
            Dim giMeta As GroupingInfoMeta

            If Not File.Exists(f.FullName) Then
                _logger.ErrorFormat("Valueset file does not exist {0}", f.FullName)
                Exit Sub
            End If

            Try
                'Is the valueset with this path already added to the registry?
                If Not valuesetDict.ContainsKey(f.FullName) Then
                    vs = New Valueset()

                    '1. Set Valueset metadata
                    '--------------------------
                    vs.Name = ini.Read(f.FullName, "descr", "name").ToLower()
                    vs.Prestext = ini.Read(f.FullName, "descr", "prestext")

                    '2. Add values to valueset
                    '---------------------------
                    col1 = ini.GetAllKeysInSection(f.FullName, "valuecode")
                    col2 = ini.GetAllKeysInSection(f.FullName, "valuetext")

                    If col1.Count <> col2.Count Then
                        Throw New PXException("Valuecode and valuetext section must contain the same number of entries")
                    End If

                    For i As Integer = 0 To col1.Count - 1
                        code = ini.Read(f.FullName, "valuecode", col1(i))
                        If (i.Equals(999) And code.Equals("****")) Then
                            'Handle .vsc and .vsn files
                            ReadLargeValueset(f.FullName, vs)
                            Exit For
                        Else
                            value = ini.Read(f.FullName, "valuetext", col2(i))
                            If Not vs.Values.ContainsKey(code) Then
                                vs.Values.Add(code, value)
                            End If
                        End If
                    Next

                    '3. Add groupings to valueset
                    '---------------------------
                    'Get all keys in aggreg section
                    col1 = ini.GetAllKeysInSection(f.FullName, "aggreg")

                    For i As Integer = 0 To col1.Count - 1
                        grouping = ini.Read(f.FullName, "aggreg", col1(i))
                        groupingPath = Path.Combine(f.DirectoryName, grouping)
                        If System.IO.File.Exists(groupingPath) Then
                            valuesetName = ini.Read(groupingPath, "aggreg", "valueset").ToLower()
                            'Verify that the aggregation belongs to the valueset (or if not check shall be done...)
                            If (_strict = False) Or (valuesetName.Equals(vs.Name)) Then
                                'Get name from the .agg file
                                groupingName = ini.Read(groupingPath, "aggreg", "name")
                                gi = New GroupingInfo(grouping)
                                gi.Name = groupingName
                                'Always use aggregated values for PX-file groupings
                                gi.GroupPres = GroupingIncludesType.AggregatedValues
                                vs.Groupings.Add(gi)

                                giMeta = New GroupingInfoMeta
                                giMeta.Valueset = vs
                                giMeta.Path = groupingPath
                                giMeta.DefaultDirectory = defaultDirectory

                                'Add to groupingInfoMeta dictionary
                                groupingInfoMetaDict.Add(gi, giMeta)
                            End If
                        End If
                    Next

                    '4. Add valueset to the valueset dictionary, full path to .vs-file is used as key
                    '--------------------------------------------------------------------------------
                    valuesetDict.Add(f.FullName, vs)

                    '5. Add the domain(s) to the domain dictionary
                    '-------------------------------------------
                    'Get all keys in domain section
                    col1 = ini.GetAllKeysInSection(f.FullName, "domain")

                    For i As Integer = 0 To col1.Count - 1
                        domain = ini.Read(f.FullName, "domain", col1(i)).ToLower()

                        If defaultDirectory Then
                            vsPath = ""
                        Else
                            vsPath = System.IO.Path.GetDirectoryName(groupingPath)
                        End If

                        If domainsDict.ContainsKey(domain) Then
                            vsDict = domainsDict(domain)
                            If Not vsDict.ContainsKey(vsPath) Then
                                vsDict.Add(vsPath, vs)
                            Else
                                _logger.Info(domain & " already loaded for " & vsPath)
                            End If
                        Else
                            vsDict = New Dictionary(Of String, Valueset)
                            vsDict.Add(vsPath, vs)
                            domainsDict.Add(domain, vsDict)
                        End If
                    Next
                End If
            Catch ex As Exception
                _logger.ErrorFormat("Error loading the valueset file: {0}, Error message: {1}", f.FullName, ex.Message)
                Exit Sub
            End Try

        End Sub

        ''' <summary>
        ''' If the valueset contains more than 1000 values only the first 999 values are located in the .vs file.
        ''' The rest of the values are located in two other files .vsc (codes) and .vsn (value texts).
        ''' This method loads codes and value texts from these files into the valueset
        ''' </summary>
        ''' <param name="valuesetFile">Path of the .vs file</param>
        ''' <param name="vs">Valueset object</param>
        ''' <remarks></remarks>
        Private Sub ReadLargeValueset(ByVal valuesetFile As String, ByVal vs As Valueset)
            If File.Exists(valuesetFile & "c") Then
                '.vsc file exists

                Dim vsnExist As Boolean = File.Exists(valuesetFile & "n") 'If .vsn file exists or not
                Dim values As New List(Of String)
                Dim value As String
                Dim code As String
                Dim i As Integer = 0

                Try
                    If vsnExist Then
                        Using vsn As New StreamReader(valuesetFile & "n")
                            'Add all valuetexts in .vsn file to list
                            Do While vsn.Peek() >= 0
                                values.Add(vsn.ReadLine().Trim())
                            Loop
                        End Using
                    End If

                    Using vsc As New StreamReader(valuesetFile & "c")
                        Do While vsc.Peek() >= 0
                            'Get code from .vsc file
                            code = vsc.ReadLine().Trim()

                            If vsnExist Then
                                value = values(i)
                            Else
                                value = code
                            End If

                            'Add code and value to the valueset
                            vs.Values.Add(code, value)
                            i = i + 1
                        Loop
                    End Using
                Catch e As Exception
                    _logger.ErrorFormat("Error reading large valueset for file {0}", valuesetFile)
                    Exit Sub
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Checks that the given valueset file (.vs) has the specified domain
        ''' </summary>
        ''' <param name="f">The (.vs) file to check</param>
        ''' <param name="domain">The wanted domain</param>
        ''' <returns>True if the valueset belongs to the specified domain, else false</returns>
        ''' <remarks></remarks>
        Private Function CheckDomain(ByVal f As FileInfo, ByVal domain As String) As Boolean
            Dim ini As New IniReader
            Dim domains As System.Collections.Specialized.StringCollection
            Dim name As String

            If Not File.Exists(f.FullName) Then
                Return False
            End If

            'Get all keys in domain section
            domains = ini.GetAllKeysInSection(f.FullName, "domain")

            For i As Integer = 0 To domains.Count - 1
                name = ini.Read(f.FullName, "domain", domains(i))
                If domain.ToLower() = name.ToLower() Then
                    Return True
                End If
            Next

            Return False
        End Function

        ''' <summary>
        ''' Get all default groupings for the specified domain
        ''' </summary>
        ''' <param name="domain">The name of the domain to get groupings for</param>
        ''' <returns>List of GroupingInfo-objects that the describes the available groupings</returns>
        ''' <remarks></remarks>
        Public Function GetDefaultGroupings(ByVal domain As String) As List(Of GroupingInfo)
            Return GetGroupingsFromPath(domain, "")
        End Function

        ''' <summary>
        ''' Get all groupings from the specified path (not belonging to the default path)
        ''' </summary>
        ''' <param name="domain">The name of the domain to get groupings for</param>
        ''' <param name="path">Path of the groupings</param>
        ''' <returns>List of GroupingInfo-objects that the describes the available groupings</returns>
        ''' <remarks></remarks>
        Public Function GetGroupingsFromPath(ByVal domain As String, ByVal path As String) As List(Of GroupingInfo)
            If Not String.IsNullOrEmpty(domain) Then
                domain = domain.ToLower()
                If _domains.ContainsKey(domain) Then
                    Dim lst As New List(Of GroupingInfo)

                    If _domains(domain).ContainsKey(path) Then
                        For Each gi As GroupingInfo In _domains(domain)(path).Groupings
                            lst.Add(gi)
                        Next
                    End If

                    Return lst

                End If
            End If

            'Return empty list
            Return New List(Of GroupingInfo)
        End Function


        ''' <summary>
        ''' Get the specified grouping. 
        ''' </summary>
        ''' <param name="gi">
        ''' The GroupingInfo object that represents the Grouping
        ''' Reads the .agg file specified by gi.
        ''' </param>
        ''' <returns>The specified grouping</returns>
        ''' <remarks></remarks>
        Public Function GetGrouping(ByVal gi As GroupingInfo) As Grouping
            Dim grouping As New Grouping
            Dim ini As New IniReader
            Dim path As String
            Dim groupCodeKeys As System.Collections.Specialized.StringCollection
            Dim childCodeKeys As System.Collections.Specialized.StringCollection
            Dim group As Group
            Dim giMeta As GroupingInfoMeta

            If Not _groupingInfoMeta.ContainsKey(gi) Then
                Return Nothing
            End If

            giMeta = _groupingInfoMeta(gi)

            If giMeta Is Nothing Then
                Return Nothing
            End If

            If giMeta.Valueset Is Nothing Then
                Return Nothing
            End If

            If String.IsNullOrEmpty(giMeta.Path) Then
                Return Nothing
            End If

            path = giMeta.Path

            If Not File.Exists(path) Then
                Throw New PCAxis.Paxiom.PXException("Could not load grouping")
            End If

            grouping.ID = gi.ID
            grouping.Name = ini.Read(path, "aggreg", "name")
            grouping.Map = ini.Read(path, "aggreg", "map")

            'Add groups to the grouping
            groupCodeKeys = ini.GetAllKeysInSection(path, "aggtext")
            For Each groupCodeKey As String In groupCodeKeys
                group = New Group
                group.GroupCode = ini.Read(path, "aggreg", groupCodeKey)
                group.Name = ini.Read(path, "aggtext", groupCodeKey)

                'Add childcodes to the group
                childCodeKeys = ini.GetAllKeysInSection(path, group.GroupCode)
                For Each childCodeKey As String In childCodeKeys
                    Dim child As New GroupChildValue
                    child.Code = ini.Read(path, group.GroupCode, childCodeKey)
                    If giMeta.Valueset.Values.TryGetValue(child.Code, child.Name) Then
                        group.ChildCodes.Add(child)
                    End If
                Next

                grouping.Groups.Add(group)
            Next

            Return grouping
        End Function

        ''' <summary>
        ''' Are groupings loaded or not?
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GroupingsLoaded() As Boolean
            Get
                Return _groupingsLoaded
            End Get
        End Property
    End Class
End Namespace
