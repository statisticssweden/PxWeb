Imports System.Configuration

Namespace Configuration.Sections
    Public Class ViewSection
        Inherits ConfigurationSection

        Const CONFIG_PAGES As String = "pages"
        Const CONFIG_DEFAULTPAGE As String = "defaultpage"

        ''' <summary>
        ''' Gets or sets a collection of available pages in web.config
        ''' </summary>
        ''' <value>Collection of pages</value>
        ''' <returns>A instance of <see cref="PageElementCollection"/></returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_PAGES, IsRequired:=False)> _
        Public Property Pages() As PageElementCollection
            Get
                Return CType(Me(CONFIG_PAGES), PageElementCollection)
            End Get
            Set(ByVal value As PageElementCollection)
                Me(CONFIG_PAGES) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the default page in web.config
        ''' </summary>
        ''' <value>URL of the default page</value>
        ''' <returns>A <see cref="String" /> representing the URL ot the default page</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_DEFAULTPAGE, IsRequired:=True)> _
        Public Property DefaultPage() As String
            Get
                Return Me(CONFIG_DEFAULTPAGE).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_DEFAULTPAGE) = value
            End Set
        End Property

    End Class
End Namespace