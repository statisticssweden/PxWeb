'Imports System.Configuration
'Namespace Configuration.Sections
'    ''' <summary>
'    ''' Configuration section for the CommandBar Sort plugin
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Class SortSection
'        Inherits ConfigurationSection

'        Const CONFIG_SORTPAGE As String = "sortpage"

'        ''' <summary>
'        ''' URL to the sort web page
'        ''' </summary>
'        ''' <value></value>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        <ConfigurationProperty(CONFIG_SORTPAGE, DefaultValue:="~/DataSort.aspx", IsRequired:=False)> _
'        Public Property SortPage() As String
'            Get
'                Return Me(CONFIG_SORTPAGE).ToString()
'            End Get
'            Set(ByVal value As String)
'                Me(CONFIG_SORTPAGE) = value
'            End Set
'        End Property

'    End Class
'End Namespace
