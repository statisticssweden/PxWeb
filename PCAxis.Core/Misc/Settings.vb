Imports System.Globalization
Imports System.Collections.Concurrent

Namespace PCAxis.Paxiom

    Public NotInheritable Class Settings
        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(Settings))
        Private Shared _locales As ConcurrentDictionary(Of String, LocaleSettings)

        Public Class Metadata
            Private Shared _removeContent As Boolean = False
            ''' <summary>
            ''' If a content variable should be removed when only one value i selected
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Property RemoveSingleContent() As Boolean
                Get
                    Return _removeContent
                End Get
                Set(ByVal value As Boolean)
                    _removeContent = value
                End Set
            End Property
        End Class

        Shared Sub New()
            _locales = New ConcurrentDictionary(Of String, LocaleSettings)()
        End Sub

        Public Shared Function GetLocale(ByVal language As String) As LocaleSettings

            '            'fix for Chinese / Taiwanese bug in net 3.5
            '#If v3_5 Then
            '            If String.Compare(language, "zh", True) = 0 Then
            '                language = "zh-TW"
            '            End If
            '#End If

            language = Util.GetLanguageForNet3_5(language)

            Dim locale As LocaleSettings
            If _locales.ContainsKey(language) Then
                locale = _locales(language)
            Else
                locale = New LocaleSettings()
                Try
                    Dim ci As CultureInfo
                    ci = New CultureInfo(language)
                    If ci.IsNeutralCulture Then
                        ci = CultureInfo.CreateSpecificCulture(language)
                    End If
                    locale.DecimalSeparator = ci.NumberFormat.NumberDecimalSeparator
                    locale.ThousandSeparator = ci.NumberFormat.NumberGroupSeparator
                    'locale.DateFormat = ci.DateTimeFormat.SortableDateTimePattern
                    locale.DateFormat = ci.DateTimeFormat.ShortDatePattern
                    _locales.GetOrAdd(language, locale)
                Catch ex As Exception
                    Logger.Warn("Could not create Culture for language " + language, ex)
                End Try
            End If

            Return locale
        End Function



#Region "DataSymbols"

        Public Class DataSymbols
            Private Shared _dataSymbols() As String = _
                    {PXConstant.DATASYMBOL_NIL_STRING, _
                     PXConstant.DATASYMBOL_1_STRING, _
                     PXConstant.DATASYMBOL_2_STRING, _
                     PXConstant.DATASYMBOL_3_STRING, _
                     PXConstant.DATASYMBOL_4_STRING, _
                     PXConstant.DATASYMBOL_5_STRING, _
                     PXConstant.DATASYMBOL_6_STRING, _
                     PXConstant.DATASYMBOL_7_STRING}

            Public Shared Property Symbol(ByVal index As Integer) As String
                Get
                    If index > 0 And index < 8 Then
                        Return _dataSymbols(index)
                    Else
                        'TODO grive proper error code
                        Throw New PCAxis.Paxiom.PXException()
                    End If
                End Get
                Set(ByVal value As String)
                    If index > 0 And index < 8 Then
                        _dataSymbols(index) = value
                    Else
                        'TODO grive proper error code
                        Throw New PCAxis.Paxiom.PXException()
                    End If
                End Set
            End Property

            Public Shared Property SymbolNIL() As String
                Get
                    Return _dataSymbols(0)
                End Get
                Set(ByVal value As String)
                    _dataSymbols(0) = value
                End Set
            End Property

        End Class

#End Region

#Region "Datanotes"
        Public Class DataNotes
            Private Shared _placment As DataNotePlacementType = DataNotePlacementType.After

            Public Shared Property Placment() As DataNotePlacementType
                Get
                    Return _placment
                End Get
                Set(ByVal value As DataNotePlacementType)
                    _placment = value
                End Set
            End Property
        End Class
#End Region

#Region "Numbers"

        Public Class Numbers
            Private Shared _roundingRule As MidpointRounding = MidpointRounding.ToEven

            Public Shared Property RoundingRule() As MidpointRounding
                Get
                    Return _roundingRule
                End Get
                Set(ByVal value As MidpointRounding)
                    _roundingRule = value
                End Set
            End Property

            'Private Shared _decimalSeparator As String = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
            'Public Shared Property DecimalSeparator() As String
            '    Get
            '        Return _decimalSeparator
            '    End Get
            '    Set(ByVal value As String)
            '        _decimalSeparator = value
            '    End Set
            'End Property

            'Private Shared _thousandSeparator As String = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator
            'Public Shared Property ThousandSeparator() As String
            '    Get
            '        Return _thousandSeparator
            '    End Get
            '    Set(ByVal value As String)
            '        _thousandSeparator = value
            '    End Set
            'End Property


            Private Shared _secrecy As SecrecyOptionType = SecrecyOptionType.None

            Public Shared Property SecrecyOption() As SecrecyOptionType
                Get
                    Return _secrecy
                End Get
                Set(ByVal value As SecrecyOptionType)
                    _secrecy = value
                End Set
            End Property
        End Class

#End Region

#Region "Locale settings"

        Public Class LocaleSettings

            Sub New()
                _thousandSeparator = ""
                _decimalSeparator = ","
                _dateFormat = "yyyy-MM-dd HH:mm"
            End Sub

            Private _thousandSeparator As String
            Public Property ThousandSeparator() As String
                Get
                    Return _thousandSeparator
                End Get
                Set(ByVal value As String)
                    _thousandSeparator = value
                End Set
            End Property

            Private _decimalSeparator As String
            Public Property DecimalSeparator() As String
                Get
                    Return _decimalSeparator
                End Get
                Set(ByVal value As String)
                    _decimalSeparator = value
                End Set
            End Property

            Private _dateFormat As String
            Public Property DateFormat() As String
                Get
                    Return _dateFormat
                End Get
                Set(ByVal value As String)
                    _dateFormat = value
                End Set
            End Property
        End Class

#End Region

#Region "Files"
        ''' <summary>
        ''' Settings for files (Excel, text)
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Files
            ''' <summary>
            ''' Level of information that will be saved to files (Excel, text)
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared _completeInfoFile As InformationLevelType = InformationLevelType.AllInformation
            Public Shared Property CompleteInfoFile() As InformationLevelType
                Get
                    Return _completeInfoFile
                End Get
                Set(ByVal value As InformationLevelType)
                    _completeInfoFile = value
                End Set
            End Property
            ''' <summary>
            ''' What the filename will start with
            ''' </summary>
            Private Shared _fileBaseName As PCAxis.Paxiom.FileBaseNameType = FileBaseNameType.Matrix
            Public Shared Property FileBaseName() As PCAxis.Paxiom.FileBaseNameType
                Get
                    Return _fileBaseName
                End Get
                Set(ByVal value As PCAxis.Paxiom.FileBaseNameType)
                    _fileBaseName = value
                End Set
            End Property

            ''' <summary>
            ''' How doublecolumn will work when saving files
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared _doubleColumnFile As DoubleColumnType
            Public Shared Property DoubleColumnFile() As DoubleColumnType
                Get
                    Return _doubleColumnFile
                End Get
                Set(ByVal value As DoubleColumnType)
                    _doubleColumnFile = value
                End Set
            End Property
        End Class
#End Region

#Region "Save and Load settings"

        'Public Shared Sub Save(ByVal path As String)
        '    Dim p As String = System.IO.Path.GetDirectoryName(path)
        '    If Not System.IO.Directory.Exists(p) Then
        '        System.IO.Directory.CreateDirectory(p)
        '    End If
        '    Using writer As New System.Xml.XmlTextWriter(path, System.Text.Encoding.Default)
        '        writer.WriteStartDocument()
        '        'DataSymbols
        '        writer.WriteStartElement("Settings")
        '        writer.WriteStartElement("DataSymbols")
        '        writer.WriteStartElement("SymbolNIL")
        '        writer.WriteCData(Settings.DataSymbols.SymbolNIL)
        '        writer.WriteEndElement() 'SymbolNIL

        '        For i As Integer = 1 To 7
        '            writer.WriteStartElement("Symbol" & i.ToString())
        '            writer.WriteCData(Settings.DataSymbols.Symbol(i))
        '            writer.WriteEndElement() 'SymbolX
        '        Next
        '        writer.WriteEndElement() 'DataSymbols

        '        'DataNotes
        '        writer.WriteStartElement("DataNotes")
        '        writer.WriteElementString("Placement", CType(Settings.DataNotes.Placment, Integer).ToString())
        '        writer.WriteEndElement() 'DataNotes

        '        'Numbers
        '        writer.WriteStartElement("Numbers")
        '        If Settings.Numbers.RoundingRule = MidpointRounding.AwayFromZero Then
        '            writer.WriteElementString("RoundingRule", "1")
        '        Else
        '            writer.WriteElementString("RoundingRule", "0")
        '        End If
        '        writer.WriteElementString("DecimalSeparator", Settings.Numbers.DecimalSeparator)
        '        writer.WriteElementString("ThousandSeparator", Settings.Numbers.ThousandSeparator)
        '        writer.WriteEndElement() 'Numbers

        '        'Files
        '        writer.WriteStartElement("Files")
        '        Select Case Settings.Files.CompleteInfoFile
        '            Case InformationLevelType.None
        '                writer.WriteElementString("CompleteInfoFile", "0")
        '            Case InformationLevelType.MandantoryFootnotesOnly
        '                writer.WriteElementString("CompleteInfoFile", "1")
        '            Case InformationLevelType.AllFootnotes
        '                writer.WriteElementString("CompleteInfoFile", "2")
        '            Case InformationLevelType.AllInformation
        '                writer.WriteElementString("CompleteInfoFile", "3")
        '        End Select

        '        Select Case Settings.Files.DoubleColumnFile
        '            Case DoubleColumnType.NoDoubleColumns
        '                writer.WriteElementString("DoubleColumnFile", "0")
        '            Case DoubleColumnType.AlwaysDoubleColumns
        '                writer.WriteElementString("DoubleColumnFile", "1")
        '            Case DoubleColumnType.OnlyDoubleColumnsWhenSpecified
        '                writer.WriteElementString("DoubleColumnFile", "2")
        '        End Select
        '        writer.WriteEndElement() 'Files

        '        writer.WriteEndElement() 'Settings
        '        writer.WriteEndDocument()
        '    End Using
        'End Sub

        'Public Shared Sub Load(ByVal path As String)
        '    Dim doc As New System.Xml.XmlDocument
        '    Dim value As Integer

        '    Try
        '        doc.Load(path)

        '        Dim node As System.Xml.XmlNode

        '        'DataSymbols
        '        node = doc.SelectSingleNode("//Settings/DataSymbols/SymbolNIL")

        '        If node IsNot Nothing Then
        '            Settings.DataSymbols.SymbolNIL = node.InnerText
        '        End If

        '        For i As Integer = 1 To 7
        '            node = doc.SelectSingleNode("//Settings/DataSymbols/Symbol" & i.ToString())

        '            If node IsNot Nothing Then
        '                Settings.DataSymbols.Symbol(i) = node.InnerText
        '            End If
        '        Next

        '        'DataNotes
        '        node = doc.SelectSingleNode("//Settings/DataNotes/Placement")
        '        If node IsNot Nothing Then
        '            If Integer.TryParse(node.InnerText, value) Then
        '                Select Case value
        '                    Case 1
        '                        Settings.DataNotes.Placment = DataNotePlacementType.Before
        '                    Case Else
        '                        Settings.DataNotes.Placment = DataNotePlacementType.After
        '                End Select
        '            Else
        '                Settings.DataNotes.Placment = DataNotePlacementType.After
        '            End If

        '        End If

        '        'Numbers
        '        node = doc.SelectSingleNode("//Settings/Numbers/RoundingRule")
        '        If node IsNot Nothing Then
        '            If Integer.TryParse(node.InnerText, value) Then
        '                If value = 1 Then
        '                    Settings.Numbers.RoundingRule = MidpointRounding.AwayFromZero
        '                Else
        '                    Settings.Numbers.RoundingRule = MidpointRounding.ToEven
        '                End If
        '            Else
        '                Settings.Numbers.RoundingRule = MidpointRounding.ToEven
        '            End If

        '        End If
        '        node = doc.SelectSingleNode("//Settings/Numbers/DecimalSeparator")
        '        If node IsNot Nothing Then
        '            Settings.Numbers.DecimalSeparator = node.InnerText
        '        End If

        '        node = doc.SelectSingleNode("//Settings/Numbers/ThousandSeparator")
        '        If node IsNot Nothing Then
        '            Settings.Numbers.ThousandSeparator = node.InnerText
        '        End If

        '        'Files
        '        node = doc.SelectSingleNode("//Settings/Files/CompleteInfoFile")
        '        If node IsNot Nothing Then
        '            If Integer.TryParse(node.InnerText, value) Then
        '                Select Case value
        '                    Case 0
        '                        Settings.Files.CompleteInfoFile = InformationLevelType.None
        '                    Case 1
        '                        Settings.Files.CompleteInfoFile = InformationLevelType.MandantoryFootnotesOnly
        '                    Case 2
        '                        Settings.Files.CompleteInfoFile = InformationLevelType.AllFootnotes
        '                    Case 3
        '                        Settings.Files.CompleteInfoFile = InformationLevelType.AllInformation
        '                End Select
        '            End If
        '        End If

        '        node = doc.SelectSingleNode("//Settings/Files/DoubleColumnFile")
        '        If node IsNot Nothing Then
        '            If Integer.TryParse(node.InnerText, value) Then
        '                Select Case value
        '                    Case 0
        '                        Settings.Files.DoubleColumnFile = DoubleColumnType.NoDoubleColumns
        '                    Case 1
        '                        Settings.Files.DoubleColumnFile = DoubleColumnType.AlwaysDoubleColumns
        '                    Case 2
        '                        Settings.Files.DoubleColumnFile = DoubleColumnType.OnlyDoubleColumnsWhenSpecified
        '                End Select
        '            End If
        '        End If

        '    Catch ex As Exception
        '        Logger.Warn("Error reading settings file " & path, ex)
        '    End Try
        'End Sub

#End Region


    End Class

End Namespace
