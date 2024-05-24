Imports System.Configuration
Namespace Configuration.Sections
    ''' <summary>
    ''' Configurationsection for the stateprovidersystem in web.config
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StateProviderSection
        Inherits System.Configuration.ConfigurationSection


        Const CONFIG_STATEPROVIDERS As String = "stateproviders"
        Const CONFIG_DEFAULTSTATEPROVIDER As String = "defaultstateprovider"
        Const CONFIG_TIMEOUT As String = "timeout"
        Const CONFIG_MANAGEDHANDLER As String = "managedhandler"

        ''' <summary>
        ''' Gets or sets a collection of available stateproviders in web.config
        ''' </summary>
        ''' <value>Collection of stateproviders</value>
        ''' <returns>A instance of <see cref="StateProviderElementCollection"/></returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_STATEPROVIDERS, IsRequired:=True)> _
        Public Property StateProviders() As StateProviderElementCollection
            Get
                Return CType(Me(CONFIG_STATEPROVIDERS), StateProviderElementCollection)
            End Get
            Set(ByVal value As StateProviderElementCollection)
                Me(CONFIG_STATEPROVIDERS) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current stateprovider in web.config
        ''' </summary>
        ''' <value>Name of the default stateprovider</value>
        ''' <returns>A <see cref="String" /> representing the name ot the default stateprovider</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_DEFAULTSTATEPROVIDER, IsRequired:=True)> _
        Public Property DefaultStateProvider() As String
            Get
                Return Me(CONFIG_DEFAULTSTATEPROVIDER).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_DEFAULTSTATEPROVIDER) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the timeout in web.config
        ''' </summary>
        ''' <value>Defines the amount of idle time in minutes before a state provider request is removed</value>
        ''' <returns>An <see cref="Integer" /> representing the timeout in minutes</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_TIMEOUT, IsRequired:=True)> _
        Public Property Timeout() As Integer
            Get
                Return CInt(Me(CONFIG_TIMEOUT))
            End Get
            Set(ByVal value As Integer)
                Me(CONFIG_TIMEOUT) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the stateprovider is a managded module or not in web.config
        ''' </summary>
        ''' <value>Defines if the stateprovider is a managed module or not</value>
        ''' <returns>An <see cref="Boolean" /> representing if the stateprovider is a managed module or not</returns>
        ''' <remarks>If se to true the stateprovider module shall be registered in the Web.server - modules section in web.config with precondition set to managedHandler</remarks>
        <ConfigurationProperty(CONFIG_MANAGEDHANDLER, IsRequired:=True)> _
        Public Property ManagedHandler() As Boolean
            Get
                Return CBool(Me(CONFIG_MANAGEDHANDLER))
            End Get
            Set(ByVal value As Boolean)
                Me(CONFIG_MANAGEDHANDLER) = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' A collection of <see cref="StateProviderElement" />
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StateProviderElementCollection
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
        ''' Creates a new instance of <see cref="StateProviderElement" />
        ''' </summary>
        ''' <returns>A instance of <see cref="StateProviderElement" /></returns>
        ''' <remarks></remarks>
        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New StateProviderElement
        End Function

        ''' <summary>
        ''' Gets the unique key for a element in the collection
        ''' </summary>
        ''' <param name="element">The <see cref="StateProviderElement" /> to return the key for</param>
        ''' <returns>An <see cref="Object" /> representing the name of the <see cref="StateProviderElement" /></returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, StateProviderElement).Name
        End Function

        ''' <summary>
        ''' Gets the <see cref="StateProviderElement" /> associated with the given name 
        ''' </summary>
        ''' <param name="Name">The name of the <see cref="StateProviderElement" /> to get</param>
        ''' <value>The <see cref="StateProviderElement" /> associated with the name</value>
        ''' <returns>An instance of <see cref="StateProviderElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal Name As String) As StateProviderElement
            Get
                Return CType(BaseGet(Name), StateProviderElement)
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="StateProviderElement" /> at the specified index
        ''' </summary>
        ''' <param name="index">The index of the <see cref="StateProviderElement" /> to get</param>
        ''' <value>The <see cref="StateProviderElement" /> at the specified index</value>
        ''' <returns>An instance of <see cref="StateProviderElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As StateProviderElement
            Get
                Return CType(BaseGet(index), StateProviderElement)
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents one stateprovider in a config file
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StateProviderElement
        Inherits ConfigurationElement

        Const CONFIG_NAME As String = "name"
        Const CONFIG_TYPE As String = "type"

        ''' <summary>
        ''' Gets or sets the unique name for the stateprovider
        ''' </summary>
        ''' <value>The unique name for the stateprovider</value>
        ''' <returns>A <see cref="String" /> representing the unique name of the provider</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_NAME, IsRequired:=True)> _
        Public Property Name() As String
            Get
                Return Me(CONFIG_NAME).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_NAME) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of the stateprovider
        ''' </summary>
        ''' <value>The type of the stateprovider</value>
        ''' <returns>A <see cref="System.Type" /> of the stateprovider</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_TYPE, IsRequired:=True), System.ComponentModel.TypeConverter(GetType(System.ComponentModel.StringConverter))> _
        Public Property Type() As Type
            Get
                Return Type.GetType(Me(CONFIG_TYPE).ToString)
            End Get
            Set(ByVal value As Type)
                Me(CONFIG_TYPE) = value
            End Set
        End Property

    End Class
End Namespace
