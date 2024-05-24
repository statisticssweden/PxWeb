Imports System.Web
Imports System.Web.Caching
Imports PCAxis.Web.Core.Management

Namespace StateProvider
    ''' <summary>
    ''' Creates instances of stateproviders
    ''' </summary>
    ''' <remarks>
    ''' <see cref="StateProviderFactory" /> caches already create instance of a specific stateprovider
    ''' on a request basis
    ''' </remarks>
    Public NotInheritable Class StateProviderFactory
        Const STATEPROVIDER_KEY As String = "STATEPROVIDER_"
        Public Const REQUEST_ID As String = "rxid"
        Private Const STATPROVIDER_CACHE_KEY As String = "stateprovider"
        Private Shared _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(StateProviderFactory))
        Private Shared _managedHandler As Nullable(Of Boolean)

        Private Sub New()
        End Sub
        ''' <summary>
        ''' Gets the currently active stateprovider instance
        ''' </summary>
        ''' <returns>The currently active stateprovider instance</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStateProvider() As StaterProviderBase
            Return GetStateProvider(True, GetRequestId())
        End Function

        ''' <summary>
        ''' Gets an instance of the specified stateprovider
        ''' </summary>
        ''' <param name="requestId">The request id for the specified stateprovider</param>
        ''' <param name="providerName">the name used in the configuration file for the stateprovider</param>
        ''' <returns>An instance of the specified stateprovider</returns>
        ''' <remarks>If an instance of the specified stateprovider already have been created it will return that instance</remarks>
        Private Shared Function CreateStateProvider(ByVal providerName As String, ByVal requestId As String) As StaterProviderBase
            Dim provider As StaterProviderBase
            'Create a new instance of stateprovider with the supplied providername
            provider = CType(Activator.CreateInstance(Configuration.ConfigurationHelper.StateProviderSection.StateProviders(providerName).Type), StaterProviderBase)
            provider.Load(requestId)
            Return provider
        End Function

        ''' <summary>
        ''' Stores an instance of a stateprovider for the remainder of the asp.net request
        ''' </summary>
        ''' <param name="provider">A stateprovider instance</param>
        ''' <param name="stateProviderName">Name of the stateprovider</param>
        ''' <remarks><paramref name=" stateProviderName "></paramref></remarks>
        Private Shared Sub StoreStateProvider(ByVal provider As StaterProviderBase, ByVal stateProviderName As String)
            HttpContext.Current.Items.Add(GetStateProviderStoreName(stateProviderName), provider)
        End Sub

        ''' <summary>
        ''' Gets a store-compatible string for the <paramref name="stateProviderName" />
        ''' </summary>
        ''' <param name="stateProviderName">The name of the stateprovider</param>
        ''' <returns>A store-compatible string</returns>
        ''' <remarks></remarks>
        Private Shared Function GetStateProviderStoreName(ByVal stateProviderName As String) As String
            Return STATEPROVIDER_KEY + stateProviderName
        End Function

        ''' <summary>
        ''' Checks if the <paramref name="stateProviderName" /> is stored
        ''' </summary>
        ''' <param name="stateProviderName">The name of the stateprovider </param>
        ''' <returns>True if the stateprovider is in the store, false otherwise</returns>
        ''' <remarks></remarks>
        Private Shared Function IsStateProviderStored(ByVal stateProviderName As String) As Boolean
            Return HttpContext.Current.Items.Contains(GetStateProviderStoreName(stateProviderName))
        End Function

        ''' <summary>
        ''' Gets the stored instance of a stateprovider in the store
        ''' </summary>
        ''' <param name="stateProviderName">The name of the stateprovider</param>
        ''' <returns>The instance stored</returns>
        ''' <remarks></remarks>
        Private Shared Function GetStateProviderFromStore(ByVal stateProviderName As String) As StaterProviderBase
            Return CType(HttpContext.Current.Items(GetStateProviderStoreName(stateProviderName)), StaterProviderBase)
        End Function

        ''' <summary>
        ''' Gets an instance of the specified stateprovider
        ''' </summary>
        ''' <param name="requestID">The request id for the specified stateprovider, only used if creating a new instance</param>
        ''' <param name="stateProviderName">The name of the stateprovider</param>
        ''' <returns>A instance of the specified stateprovider</returns>
        ''' <remarks>If an instance of the specified stateprovider already exists in the store it returns that,
        ''' otherwise it creates a new instance, stores it and return that instance</remarks>
        Private Shared Function GetStateProviderByName(ByVal stateProviderName As String, ByVal requestID As String) As StaterProviderBase
            Dim provider As StaterProviderBase

            If HttpContext.Current Is Nothing Then
                'If the current HttpContext is not available there is no 
                'way to cache the instance so skip it and just create the instance
                provider = CreateStateProvider(stateProviderName, requestID)
            Else
                'If current HttpContext is available we can cache the instance
                If IsStateProviderStored(stateProviderName) Then
                    'If an instance have already been cached
                    provider = GetStateProviderFromStore(stateProviderName)
                Else
                    'otherwise create a new instance and store it
                    provider = CreateStateProvider(stateProviderName, requestID)
                    StoreStateProvider(provider, stateProviderName)
                End If
            End If
            Return provider
        End Function

        ''' <summary>
        ''' Gets an instance of the default stateprovider
        ''' </summary>
        ''' <param name="requestId">The requestId for the stateProvider</param>
        ''' <param name="timeout">True if the stateprovider should have timeout on it's content</param>
        ''' <returns>A instance of the default stateprovider</returns>
        ''' <remarks></remarks>
        Private Shared Function GetStateProvider(ByVal timeout As Boolean, ByVal requestId As String) As StaterProviderBase
            Dim provider As StaterProviderBase = Nothing
            Dim stateConfiguration As Configuration.Sections.StateProviderSection = Configuration.ConfigurationHelper.StateProviderSection
            If stateConfiguration IsNot Nothing Then
                With stateConfiguration
                    If String.IsNullOrEmpty(.DefaultStateProvider) Then
                        'If a default stateprovider is not specified return the first
                        provider = GetStateProviderByName(.StateProviders(0).Name, requestId)
                    Else
                        'Otherwise return the defuault stateprovider
                        provider = GetStateProviderByName(.DefaultStateProvider, requestId)
                    End If
                End With

                If timeout Then
                    RegisterTimeout()
                End If
            End If

            Return provider

        End Function


        ''' <summary>
        ''' Gets the unique request id for the stateprovider
        ''' </summary>
        ''' <returns>The unique request id</returns>
        ''' <remarks></remarks>
        Friend Shared Function GetRequestId() As String
            Dim requestId As String = Guid.NewGuid().ToString()

            If HttpContext.Current IsNot Nothing Then
                If HttpContext.Current.Request.Cookies.Get(REQUEST_ID) IsNot Nothing Then
                    requestId = HttpContext.Current.Request.Cookies.Get(REQUEST_ID).Value
                ElseIf Not HttpContext.Current.Response.HeadersWritten Then
                    HttpContext.Current.Response.SetCookie(New HttpCookie(REQUEST_ID, requestId))
                End If

                If HttpContext.Current.Items(REQUEST_ID) Is Nothing Then
                    HttpContext.Current.Items.Add(REQUEST_ID, requestId)
                End If
            End If

            Return requestId
        End Function

        ''' <summary>
        ''' Register the <see cref="StateProviderFactory"/> with the <see cref="Management.LinkManager" />
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub RegisterWithLinkManager()
            Management.LinkManager.RegisterEnsureQueries(New EnsureQueriesEventHandler(AddressOf EnsureQueriesHandler))
        End Sub

        ''' <summary>
        ''' Unregister the <see cref="StateProviderFactory"/> with the <see cref="Management.LinkManager" />
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UnregisterWithLinkManager()
            'RemoveHandler Management.LinkManager.EnsureQueries, AddressOf EnsureQueriesHandler
            Management.LinkManager.UnregisterEnsureQueries(New EnsureQueriesEventHandler(AddressOf EnsureQueriesHandler))
        End Sub

        ''' <summary>
        ''' Adds the request id to the <see cref="Management.LinkManager" />
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub EnsureQueriesHandler(sender As Object, e As EnsureQueriesEventArgs)
            Dim queries As Dictionary(Of String, String) = e.Queries

            'TODO: refactor this out
            'If Not queries.ContainsKey(REQUEST_ID) Then
            '    queries.Add(REQUEST_ID, GetRequestId)
            'End If
        End Sub

        ''' <summary>
        ''' Checks if a request id for the current id exists
        ''' </summary>
        ''' <returns><c>True</c> if the request id exists, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Public Shared Function IsRequestIdAvailable() As Boolean
            If QuerystringManager.GetQuerystringParameter(REQUEST_ID) Is Nothing Then
                If HttpContext.Current.Items(REQUEST_ID) Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        End Function

        ''' <summary>
        ''' Checks if the stateprovider is a managed module (Is declared in the modules section in web.config with preCondition=ManagedHandler and will only be executed for code files like .aspx) 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsManagedHandler() As Boolean
            If Not _managedHandler.HasValue Then
                Dim stateConfiguration As Configuration.Sections.StateProviderSection = Configuration.ConfigurationHelper.StateProviderSection

                If stateConfiguration IsNot Nothing Then
                    _managedHandler = stateConfiguration.ManagedHandler
                Else
                    _managedHandler = False
                End If

            End If

            Return _managedHandler.Value
        End Function

        ''' <summary>
        ''' Registers for timeout using the Cache
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub RegisterTimeout()
            If HttpContext.Current IsNot Nothing Then
                If HttpContext.Current.Cache(GetRequestId()) Is Nothing Then
                    HttpContext.Current.Cache.Add(GetRequestId(), STATPROVIDER_CACHE_KEY, Nothing, Cache.NoAbsoluteExpiration, New TimeSpan(0, Configuration.ConfigurationHelper.StateProviderSection.Timeout, 0), CacheItemPriority.NotRemovable, AddressOf RemoveStateProvider)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Tells the stateprovider for a specified request id to remove it's content if there have been a timeout
        ''' </summary>
        ''' <param name="key">The request id to remove the content for</param>
        ''' <param name="value">The cache stateprovider id</param>
        ''' <param name="reason">The reason for the removal from the cache, if timeout the stateprovider removes the content</param>
        ''' <remarks>Called by the cache</remarks>
        Private Shared Sub RemoveStateProvider(ByVal key As String, ByVal value As Object, ByVal reason As CacheItemRemovedReason)
            If CStr(value) = STATPROVIDER_CACHE_KEY And reason = CacheItemRemovedReason.Expired Then
                GetStateProvider(False, key).Unload(Enums.StateProviderUnloadReason.StateTimeout)
                _logger.DebugFormat("Removing state for request {0} because of timeout", key)
            End If
        End Sub
    End Class
End Namespace
