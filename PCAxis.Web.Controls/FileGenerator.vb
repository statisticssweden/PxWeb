'Imports System.Xml
'Imports System.Xml.XPath
'Imports System.Globalization
'Imports PCAxis.Paxiom.Localization
'Imports PCAxis.Web.Core

'<Obsolete()> _
'    Public Class FileGenerator
'    Private _fileTypes As New Dictionary(Of String, FileType)
'    Private _fileFormats As New Dictionary(Of String, String)
'    Private _fileFormatType As New Dictionary(Of String, String)

'    Public ReadOnly Property FileTypes() As Dictionary(Of String, FileType)
'        Get
'            Return _fileTypes
'        End Get
'    End Property

'    Public ReadOnly Property FileFormats() As Dictionary(Of String, String)
'        Get
'            Return _fileFormats
'        End Get
'    End Property

'    Private Sub New()
'    End Sub
'    'Public Sub New(ByVal culture As CultureInfo)
'    '    'MyClass.New(IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PCAxis.Paxiom.Configuration.ConfigurationHelper.FileGeneratorSection.ConfigFilePath), culture)
'    'End Sub

'    'Public Sub New(ByVal fullName As String, ByVal culture As CultureInfo)
'    '    ReadConfigurationFile(fullName, culture)
'    'End Sub

'    Public Sub New(ByVal culture As CultureInfo)
'        ReadConfigurationFile(culture)
'    End Sub

'    Public Function GetFileType(ByVal fileFormat As String) As FileType
'        Dim fileType As FileType = Nothing
'        Dim fileTypeType As String = ""
'        If _fileFormatType.TryGetValue(fileFormat, fileTypeType) Then
'            For Each ft As FileType In _fileTypes.Values
'                If ft.Type = fileTypeType Then
'                    fileType = ft
'                    Exit For
'                End If
'            Next
'        End If
'        Return fileType
'    End Function

'    ''' <summary>
'    ''' Reads the Configurationfile for filtypes to be available in the control.
'    ''' </summary>
'    ''' <remarks></remarks>
'    Private Sub ReadConfigurationFile(ByVal culture As CultureInfo)
'        Dim doc As New XPathDocument(Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.FileConfig)))
'        Dim docNav As XPathNavigator
'        Dim xmlNI As XPathNodeIterator
'        docNav = doc.CreateNavigator()

'        Dim manager As New XmlNamespaceManager(docNav.NameTable)
'        manager.AddNamespace("ns", "http://www.scb.se")

'        xmlNI = docNav.Select("/ns:FileTypeList/ns:FileType", manager)
'        While (xmlNI.MoveNext)
'            Dim ft As New FileType()

'            If (Not Convert.ToBoolean(xmlNI.Current.GetAttribute("Active", ""))) Then
'                Continue While
'            End If
'            ft.Category = xmlNI.Current.GetAttribute("category", "")
'            If (xmlNI.Current.SelectSingleNode("ns:Type", manager).Value IsNot Nothing) Then
'                ft.Type = xmlNI.Current.SelectSingleNode("ns:Type", manager).Value
'                ft.TranslatedText = PxResourceManager.GetResourceManager().GetString(ft.Type, culture)
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:AssemblyQualifiedName", manager).Value IsNot Nothing) Then
'                ft.AssemblyQualifiedName = xmlNI.Current.SelectSingleNode("ns:AssemblyQualifiedName", manager).Value
'            End If

'            Dim fileInfosNode As XPathNavigator = xmlNI.Current.SelectSingleNode("ns:FileInfos", manager)

'            If fileInfosNode IsNot Nothing Then
'                Dim fileInfos As XPathNodeIterator = fileInfosNode.Select("ns:FileInfo", manager)
'                While (fileInfos.MoveNext)
'                    If fileInfos.Current.Value IsNot Nothing Then
'                        Dim format As String = fileInfos.Current.Value
'                        Dim translatedFormat As String = PxResourceManager.GetResourceManager().GetString(format, culture)
'                        ft.FileFormats.Add(format, translatedFormat)
'                        _fileFormats.Add(format, translatedFormat)
'                        _fileFormatType.Add(format, ft.Type)
'                    End If
'                End While
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:WebControl", manager).Value IsNot Nothing) Then
'                ft.WebControl = xmlNI.Current.SelectSingleNode("ns:WebControl", manager).Value
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:MimeType", manager).Value IsNot Nothing) Then
'                ft.MimeType = xmlNI.Current.SelectSingleNode("ns:MimeType", manager).Value
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:FileExtension", manager).Value IsNot Nothing) Then
'                ft.FileExtension = xmlNI.Current.SelectSingleNode("ns:FileExtension", manager).Value
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:CssClass", manager).Value IsNot Nothing) Then
'                ft.CssClass = xmlNI.Current.SelectSingleNode("ns:CssClass", manager).Value
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:Image", manager).Value IsNot Nothing) Then
'                ft.Image = xmlNI.Current.SelectSingleNode("ns:Image", manager).Value
'            End If

'            If (xmlNI.Current.SelectSingleNode("ns:ShortcutImage", manager).Value IsNot Nothing) Then
'                ft.ShortcutImage = xmlNI.Current.SelectSingleNode("ns:ShortcutImage", manager).Value
'            End If

'            _fileTypes.Add(ft.Type, ft)
'        End While
'    End Sub
'End Class
