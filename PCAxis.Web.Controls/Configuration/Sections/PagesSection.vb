'Imports System.Configuration
'Namespace Configuration.Sections
'    ''' <summary>
'    ''' Configuration section for the web pages used together with the PX web controls
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Class PagesSection
'        Inherits ConfigurationSection

'        Const CONFIG_TABLEPAGE As String = "tablepage"
'        Const CONFIG_SORTEDTABLEPAGE As String = "sortedtablepage"
'        Const CONFIG_CHARTPAGE As String = "chartpage"
'        Const CONFIG_FOOTNOTEPAGE As String = "footnotepage"
'        Const CONFIG_INFORMATIONPAGE As String = "informationpage"

'        ''' <summary>
'        ''' URL to the table web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_TABLEPAGE, DefaultValue:="~/Table.aspx", IsRequired:=False)> _
'        Public Property TablePage() As String
'            Get
'                Return Me(CONFIG_TABLEPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_TABLEPAGE) = value
'            End Set
'        End Property

'        ''' <summary>
'        ''' URL to the sorted table web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_SORTEDTABLEPAGE, DefaultValue:="~/DataSort.aspx", IsRequired:=False)> _
'        Public Property SortedTablePage() As String
'            Get
'                Return Me(CONFIG_SORTEDTABLEPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_SORTEDTABLEPAGE) = value
'            End Set
'        End Property

'        ''' <summary>
'        ''' URL to the chart web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_CHARTPAGE, DefaultValue:="~/Chart.aspx", IsRequired:=False)> _
'        Public Property ChartPage() As String
'            Get
'                Return Me(CONFIG_CHARTPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_CHARTPAGE) = value
'            End Set
'        End Property

'        ''' <summary>
'        ''' URL to the footnote web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_FOOTNOTEPAGE, DefaultValue:="~/Footnotes.aspx", IsRequired:=False)> _
'        Public Property FootnotePage() As String
'            Get
'                Return Me(CONFIG_FOOTNOTEPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_FOOTNOTEPAGE) = value
'            End Set
'        End Property

'        ''' <summary>
'        ''' URL to the information web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_INFORMATIONPAGE, DefaultValue:="~/Information.aspx", IsRequired:=False)> _
'        Public Property InformationPage() As String
'            Get
'                Return Me(CONFIG_INFORMATIONPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_INFORMATIONPAGE) = value
'            End Set
'        End Property
'    End Class


'End Namespace

