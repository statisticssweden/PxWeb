Imports System.Configuration

Namespace Configuration.Sections
    ''' <summary>
    ''' Represents the PxPages section in the web.config file
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxPageSection
        Inherits ConfigurationSection

        Const CONFIG_PXPAGES As String = "pxpages"

        ''' <summary>
        ''' Gets or sets a collection of available pxpages in web.config
        ''' </summary>
        ''' <value>Collection of pxpages</value>
        ''' <returns>A instance of <see cref="PxPageElementCollection"/></returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_PXPAGES, IsRequired:=False)> _
        Public Property PxPages() As PxPageElementCollection
            Get
                Return CType(Me(CONFIG_PXPAGES), PxPageElementCollection)
            End Get
            Set(ByVal value As PxPageElementCollection)
                Me(CONFIG_PXPAGES) = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' A collection of <see cref="PxPageElement" />
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxPageElementCollection
        Inherits ConfigurationElementCollection

        ''' <summary>
        ''' Returns the collectiontype of this collection
        ''' </summary>
        ''' <value><see cref="ConfigurationElementCollectionType.AddRemoveClearMap" /></value>
        ''' <returns>A instance of <see cref="ConfigurationElementCollectionType.AddRemoveClearMap" /></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.AddRemoveClearMap
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of <see cref="PxPageElement" />
        ''' </summary>
        ''' <returns>A instance of <see cref="PxPageElement" /></returns>
        ''' <remarks></remarks>
        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New PxPageElement
        End Function

        ''' <summary>
        ''' Gets the unique key for a element in the collection
        ''' </summary>
        ''' <param name="element">The <see cref="PxPageElement" /> to return the key for</param>
        ''' <returns>An <see cref="Object" /> representing the language of the <see cref="PxPageElement" /></returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, PxPageElement).Id
        End Function

        ''' <summary>
        ''' Gets the <see cref="PxPageElement" /> associated with the given name 
        ''' </summary>
        ''' <param name="Name">The name of the <see cref="PxPageElement" /> to get</param>
        ''' <value>The <see cref="PxPageElement" /> associated with the name</value>
        ''' <returns>An instance of <see cref="PxPageElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal Name As String) As PxPageElement
            Get
                Return CType(BaseGet(Name), PxPageElement)
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="PxPageElement" /> at the specified index
        ''' </summary>
        ''' <param name="index">The index of the <see cref="PxPageElement" /> to get</param>
        ''' <value>The <see cref="PxPageElement" /> at the specified index</value>
        ''' <returns>An instance of <see cref="PxPageElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As PxPageElement
            Get
                Return CType(BaseGet(index), PxPageElement)
            End Get
        End Property
    End Class


    ''' <summary>
    ''' Represents a PxPage section in the web.config file
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxPageElement
        Inherits ConfigurationSection

        Const CONFIG_ID As String = "id"
        Const CONFIG_DEFAULTPAGE As String = "defaultpage"
        Const CONFIG_PAGES As String = "pages"

        ''' <summary>
        ''' Gets or sets the page id in web.config
        ''' </summary>
        ''' <value>Identifier for PxPage in web.config</value>
        ''' <returns>A <see cref="String" />Id of the PX-page</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_ID, IsRequired:=True)> _
        Public Property Id() As String
            Get
                Return Me(CONFIG_ID).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_ID) = value
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

    End Class
End Namespace
