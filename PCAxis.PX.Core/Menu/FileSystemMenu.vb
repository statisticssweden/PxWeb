'Imports PCAxis.Menu
'Imports System.IO

'Namespace PCAxis.Paxiom
'    Public Class FileSystemMenu
'        Inherits PxMenuBase

'        ''' <summary>
'        ''' Constructor
'        ''' </summary>
'        ''' <param name="rootPath">The root directory of the menu</param>
'        ''' <param name="language">
'        ''' If specified, only the files in the given language are shown in the menu.
'        ''' If not specified ("") all files are shown.
'        ''' </param>
'        ''' <remarks></remarks>
'        Sub New(ByVal rootPath As String, ByVal language As String)
'            MyBase.Language = language
'            RootItem = New PxMenuItem(System.IO.Path.GetFileName(rootPath), "", "", rootPath, "")
'        End Sub

'        ''' <summary>
'        ''' Set selected item in the menu
'        ''' </summary>
'        ''' <param name="selection"></param>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        Public Overrides Function SetCurrentItemBySelection(ByVal selection As String) As Boolean
'            If MyBase.SetCurrentItemBySelection(selection) Then
'                Dim item As Item

'                item = RootItem.FindSelection(selection)

'                If TypeOf item Is PxMenuItem Then
'                    Dim menuItem As PxMenuItem = CType(item, PxMenuItem)
'                    If menuItem.SubItems.Count = 0 Then
'                        Load(menuItem)
'                    End If
'                End If
'            Else
'                Return False
'            End If

'            Return True
'        End Function

'        ''' <summary>
'        ''' Loads subdirectories and files for the given MenuItem
'        ''' </summary>
'        ''' <param name="item">The MenuItem</param>
'        ''' <remarks></remarks>
'        Protected Sub Load(ByVal item As PxMenuItem)
'            Dim itm As PxMenuItem
'            Dim di As New System.IO.DirectoryInfo(item.Selection)
'            Dim sort As Integer = 0

'            'Add directories
'            For Each d As System.IO.DirectoryInfo In di.GetDirectories()
'                If (d.Attributes And IO.FileAttributes.Hidden) <> IO.FileAttributes.Hidden Then
'                    itm = New PxMenuItem(GetDirectoryName(d), "", "", d.FullName, "")
'                    item.AddSubItem(itm)
'                End If
'            Next

'            'Add files
'            For Each f As FileInfo In di.GetFiles
'                Select Case f.Extension.ToLower
'                    Case ".px"
'                        If AddPxLink(f, item, sort.ToString("00000")) Then
'                            sort = sort + 1
'                        End If
'                    Case ".link"
'                        If AddUrlLink(f, item, sort.ToString("00000")) Then
'                            sort = sort + 1
'                        End If
'                    Case ".pxs"
'                        'item.AddSubItem(New PxMenu.Link(System.IO.Path.GetFileNameWithoutExtension(f.Name), sort.ToString("00000"), PxMenu.Link.LinkTypes.PxsFile, f.FullName))
'                        item.AddSubItem(New TableLink(System.IO.Path.GetFileNameWithoutExtension(f.Name), "", sort.ToString("00000"), f.FullName, "", LinkType.PXS, TableStatus.NotSet, Nothing, "", "", "", PresCategory.Official))
'                        sort = sort + 1
'                End Select
'            Next

'        End Sub

'        ''' <summary>
'        ''' Adds a link for the given PX-file to the specified MenuItem.
'        ''' </summary>
'        ''' <param name="f">FileInfo object for the PX-file</param>
'        ''' <param name="item">MenuItem to add the link to</param>
'        ''' <param name="sortCode">Sortcode for the link</param>
'        ''' <returns>True if the link was created, else false</returns>
'        ''' <remarks></remarks>
'        Private Function AddPxLink(ByVal f As FileInfo, ByVal item As PxMenuItem, ByVal sortCode As String) As Boolean
'            Dim lnkBuilder As New MenuPxLinkBuilder
'            Dim lnk As Link

'            lnk = lnkBuilder.BuildLink(f, Me.Language, sortCode)

'            If Not lnk Is Nothing Then
'                item.AddSubItem(lnk)
'                Return True
'            End If

'            Return False
'        End Function

'        ''' <summary>
'        ''' Adds a link for the given .link-file to the specified MenuItem.
'        ''' </summary>
'        ''' <param name="f">FileInfo object for the .link-file</param>
'        ''' <param name="item">MenuItem to add the link to</param>
'        ''' <param name="sortCode">Sortcode for the link</param>
'        ''' <returns>True if the link was created, else false</returns>
'        ''' <remarks></remarks>
'        Private Function AddUrlLink(ByVal f As FileInfo, ByVal item As PxMenuItem, ByVal sortCode As String) As Boolean
'            Dim lnk As Link
'            Dim prestext As String = ""
'            Dim url As String = ""

'            If ValidUrlLinkFile(f) Then
'                If GetUrlLinkInfo(f, prestext, url) Then
'                    If Not String.IsNullOrEmpty(prestext) And Not String.IsNullOrEmpty(url) Then
'                        'lnk = New PxMenu.Link(prestext, sortCode, PxMenu.Link.LinkTypes.Url, url)
'                        lnk = New Url(prestext, sortCode, url, "", PresCategory.Official, url, LinkPres.Both)
'                        item.AddSubItem(lnk)
'                        Return True
'                    End If
'                End If
'            End If

'            Return False
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
'                    If Me.Language.Length > 0 Then
'                        If Me.Language.Equals(lang.ToLower) Then
'                            'Link-file for my language - Valid file!
'                            Return True
'                        End If
'                    End If
'                End If
'            Else
'                'Fallback language link-file
'                '---------------------------
'                If Me.Language.Length > 0 Then
'                    If File.Exists(Path.Combine(f.DirectoryName, name & "_" & Me.Language & ".link")) Then
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
'        ''' Get the name of the directory
'        ''' </summary>
'        ''' <param name="di">DirectoryInfo object of the directory</param>
'        ''' <returns>The name of the directory</returns>
'        ''' <remarks>The function handles Alias-files if they are present in the directory</remarks>
'        Private Function GetDirectoryName(ByVal di As IO.DirectoryInfo) As String
'            Dim aliasFile As String = ""
'            Dim name As String = ""

'            'Check for Alias.txt for the selected language
'            aliasFile = di.FullName & "\Alias_" & Me.Language & ".txt"
'            If IO.File.Exists(aliasFile) Then
'                name = ReadAliasName(aliasFile)
'            End If

'            'Check for Alias.txt for fallback language
'            If name.Length = 0 Then
'                aliasFile = di.FullName & "\Alias.txt"
'                If IO.File.Exists(aliasFile) Then
'                    name = ReadAliasName(aliasFile)
'                End If
'            End If

'            'Take the directory name
'            If name.Length = 0 Then
'                name = di.Name
'            End If

'            Return name
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

'    End Class
'End Namespace
