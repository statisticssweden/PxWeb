Imports System.Resources
Imports System.IO
Imports System.Globalization
Imports System.Xml
Imports System.Xml.Schema

Namespace PCAxis.Paxiom.Localization

    ''' <summary>
    ''' Reader of paxiom language files
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxResourceReader
        Implements IResourceReader


        Private Shared _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PxResourceReader))

        Private _culture As String = ""
        Private _basename As String = ""
        Private _validXML As Boolean = True

        Private Shared _languagePath As String = ""

        Shared Sub New()
            _languagePath = Configuration.ConfigurationHelper.LocalizationSection.FilesPath
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="basename">
        ''' The basename of the paxiom language file
        ''' </param>
        ''' <param name="culture">
        ''' The culture of the paxiom language file
        ''' </param>
        ''' <remarks>
        ''' The basename combined with the culture gives which file to read.
        ''' </remarks>
        ''' 
        Public Sub New(ByVal basename As String, ByVal culture As CultureInfo)
            _culture = culture.ToString
            _basename = basename
        End Sub

        ''' <summary>
        ''' Path to the folder with the language files
        ''' </summary>
        ''' <value>Path to the folder with the language files</value>
        ''' <returns>Path to the folder with the language files</returns>
        ''' <remarks>Use this property to override the setting specified in web.config</remarks>
        Public Shared Property LanguagePath() As String
            Get
                Return _languagePath
            End Get
            Set(ByVal value As String)
                _languagePath = value
            End Set
        End Property

#Region "IResourceReader implementation"
        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me.GetEnumerator1
        End Function

        Public Sub Close() Implements System.Resources.IResourceReader.Close

        End Sub

        ''' <summary>
        ''' Reads the paxiom language file
        ''' </summary>
        ''' <returns>
        ''' Iterator for the language
        ''' </returns>
        ''' <remarks></remarks>
        Public Function GetEnumerator1() As System.Collections.IDictionaryEnumerator Implements System.Resources.IResourceReader.GetEnumerator
            Dim htLanguage As New Hashtable
            Dim filename As String = ""
            Dim path As String = ""
            Dim xmldoc As New XmlDocument
            Dim schema As Xml.Schema.XmlSchema
            Dim eventHandler As ValidationEventHandler
            Dim xmlnodes As XmlNodeList
            Dim key As String = ""
            Dim value As String = ""

            If String.IsNullOrEmpty(_culture) Then
                filename = _basename & ".xml"
            Else
                filename = _basename & "." & _culture & ".xml"
            End If

            Try
                'If IO.Path.IsPathRooted(_languagePath) Then
                '    path = _languagePath
                'Else
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _languagePath.TrimStart("\"c, "/"c))
                'End If

                path = System.IO.Path.Combine(path, filename)

                If File.Exists(path) Then
                    Dim file As New FileStream(path, FileMode.Open, FileAccess.Read)
                    xmldoc.Load(file)
                    file.Close()
                    _validXML = True

                    Using s As New StringReader(My.Resources.LanguageSchema)
                        eventHandler = New ValidationEventHandler(AddressOf ValidationEventHandler)

                        schema = XmlSchema.Read(s, eventHandler)
                        xmldoc.Schemas.Add(schema)
                        xmldoc.Validate(eventHandler)

                        If _validXML Then
                            _logger.InfoFormat("Loading language from {0}", path)
                            xmlnodes = xmldoc.GetElementsByTagName("sentence")

                            For Each node As XmlNode In xmlnodes
                                key = node.Attributes("name").Value()
                                value = node.Attributes("value").Value()
                                htLanguage.Add(key, value)
                            Next
                        End If
                    End Using
                Else
                    '_logger.WarnFormat("The Language file '{0}' does not exist", path)
                End If

            Catch ex As Exception
            End Try

            Return htLanguage.GetEnumerator
        End Function

        ''' <summary>
        ''' Validates that the xml-file conforms with the xsd-schema
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ValidationEventHandler(ByVal sender As Object, ByVal e As ValidationEventArgs)
            Select Case e.Severity
                Case XmlSeverityType.Error
                    _logger.ErrorFormat("Validating language XML-file: " & e.Message)
                Case XmlSeverityType.Warning
                    _logger.WarnFormat("Validating language XML-file: " & e.Message)
            End Select
            _validXML = False
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
#End Region


    End Class
End Namespace
