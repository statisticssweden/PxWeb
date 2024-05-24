Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Web.UI

Namespace Management
    ''' <summary>
    ''' Used to create links
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class LinkManager

        ''' <summary>
        ''' Defines signature of the Create link method
        ''' </summary>
        ''' <param name="page"></param>
        ''' <param name="formatHtmlEntities"></param>
        ''' <param name="links"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Delegate Function LinkMethod(ByVal page As String, ByVal formatHtmlEntities As Boolean, <[ParamArray]()> ByVal links As LinkItem()) As String

        Private Shared _linkMethod As LinkMethod
        ''' <summary>
        ''' Declares the Create link method to use
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property CreateLinkMethod() As LinkMethod
            Get
                If _linkMethod Is Nothing Then
                    _linkMethod = AddressOf DefaultCreateLink
                End If
                Return _linkMethod
            End Get
            Set(ByVal value As LinkMethod)
                _linkMethod = value
            End Set
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            CreateLinkMethod = AddressOf DefaultCreateLink
        End Sub

        ''' <summary>        
        ''' Allows others to automatically append their own querystrings into 
        ''' the links create by the linkmanager by subscribing to this event
        ''' </summary>
        ''' <param name="queries">The <see cref="Dictionary(Of String,String)" /> to append your own querystrings to</param>
        ''' <remarks>Subscribers should always check that key they use are not already added</remarks>
        '''Public Shared Event EnsureQueries(ByVal queries As Dictionary(Of String, String))

        'Public Delegate Sub EnsureQueriesEventHandler(sender As Object, e As EnsureQueriesEventArgs)
        Const PAXIOMMODELCHANGEDBUCKET As String = "PaxiomModelChangedBucket"

        Public Shared Sub RegisterEnsureQueries(ByVal handler As EnsureQueriesEventHandler)
            Dim bucket As PaxiomManagerEventBucket


            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)
            Else
                bucket = New PaxiomManagerEventBucket
                System.Web.HttpContext.Current.Items.Add(PAXIOMMODELCHANGEDBUCKET, bucket)
            End If

            AddHandler bucket.EnsureQueries, handler
        End Sub

        Public Shared Sub UnregisterEnsureQueries(ByVal handler As EnsureQueriesEventHandler)
            Dim bucket As PaxiomManagerEventBucket

            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)

                RemoveHandler bucket.EnsureQueries, handler
            End If

        End Sub


        ''' <summary>
        ''' Gets a link to the supplied page with the supplied querystrings
        ''' </summary>
        ''' <param name="page">The <see cref="Page" /> to navigate to</param>
        ''' <param name="links">A array of zero or more <see cref="LinkItem" /></param>
        ''' <returns>A <see cref="String" /> containing the complete link to navigate to</returns>
        ''' <remarks>Does not encode html entities</remarks>
        Public Shared Function CreateLink(ByVal page As Page, ByVal ParamArray links As LinkItem()) As String
            Return CreateLink(page, False, links)
        End Function

        ''' <summary>
        ''' Create a link to the supplied page with the supplied querystrings
        ''' </summary>
        ''' <param name="page">The <see cref="Page" /> to navigate to</param>
        ''' <param name="formatHtmlEntities">A <see cref="Boolean" /> that sets wheter or not to encode html entites</param>
        ''' <param name="links">A array of zero or more <see cref="LinkItem" /></param>
        ''' <returns>A <see cref="String" /> containing the complete link to navigate to</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateLink(ByVal page As Page, ByVal formatHtmlEntities As Boolean, ByVal ParamArray links As LinkItem()) As String
            Return CreateLink(Path.GetFileNameWithoutExtension(page.Request.Path) & ".aspx", formatHtmlEntities, links)
        End Function

        ''' <summary>
        ''' Create a link to the supplied page with the supplied querystrings
        ''' </summary>
        ''' <param name="page">The name of the page to navigate to</param>
        ''' <param name="links">A array of zero or more <see cref="LinkItem" /></param>
        ''' <returns>A <see cref="String" /> containing the complete link to navigate to</returns>
        ''' <remarks>Does not encode html entities</remarks>
        Public Shared Function CreateLink(ByVal page As String, ByVal ParamArray links As LinkItem()) As String
            Return CreateLink(page, False, links)
        End Function

        ''' <summary>
        ''' Create a link to the supplied page with the supplied querystrings
        ''' </summary>
        ''' <param name="page">The name of the page to navigate to</param>
        ''' <param name="formatHtmlEntities">A <see cref="Boolean" /> that sets wheter or not to encode html entites</param>
        ''' <param name="links">A array of zero or more <see cref="LinkItem" /></param>
        ''' <returns>A <see cref="String" /> containing the complete link to navigate to</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateLink(ByVal page As String, ByVal formatHtmlEntities As Boolean, ByVal ParamArray links As LinkItem()) As String
            Return LinkManager.CreateLinkMethod(page, formatHtmlEntities, links)
        End Function

        ''' <summary>
        ''' Default implementation to create a link to the supplied page with the supplied querystrings
        ''' </summary>
        ''' <param name="page">The name of the page to navigate to</param>
        ''' <param name="formatHtmlEntities">A <see cref="Boolean" /> that sets wheter or not to encode html entites</param>
        ''' <param name="links">A array of zero or more <see cref="LinkItem" /></param>
        ''' <returns>A <see cref="String" /> containing the complete link to navigate to</returns>
        ''' <remarks></remarks>
        Public Shared Function DefaultCreateLink(ByVal page As String, ByVal formatHtmlEntities As Boolean, ByVal ParamArray links As LinkItem()) As String

            Dim pageLink As New StringBuilder()
            Dim first As Boolean = True
            Dim queries As New Dictionary(Of String, String)

            If Not page Is Nothing Then
                If Not page.Contains("~") Then
                    pageLink.Append("~/")
                End If
            End If

            pageLink.Append(page)

            'Calls all subscribers to let them add their querystrings to the link
            'Used by Stateprovider
            OnEnsureQueries(queries)

            'Adds all querystrings supplied to the function
            For Each link As LinkItem In links
                With link
                    If queries.ContainsKey(.Key) Then
                        queries.Item(.Key) = .Value
                    Else
                        queries.Add(.Key, .Value)
                    End If
                End With
            Next

            'Sort keys in a list so we get rxid last
            Dim lst As New List(Of KeyValuePair(Of String, String))
            For Each query As KeyValuePair(Of String, String) In queries
                If query.Key.Equals(StateProvider.StateProviderFactory.REQUEST_ID) Then
                    lst.Add(query) 'Add rxid last
                Else
                    lst.Insert(0, query) 'Insert first
                End If
            Next

            'Creates the link
            For Each query As KeyValuePair(Of String, String) In lst

                If first Then
                    pageLink.Append("?")
                    first = False
                Else
                    If formatHtmlEntities Then
                        pageLink.Append("&amp;")
                    Else
                        pageLink.Append("&")
                    End If

                End If

                pageLink.Append(String.Format( _
                                               CultureInfo.InvariantCulture, _
                                               "{0}={1}", _
                                               query.Key, _
                                               HttpUtility.UrlPathEncode(query.Value)))

            Next


            Return HttpUtility.UrlPathEncode(pageLink.ToString())
        End Function
        ''' <summary>
        ''' Raises the EnsureQueries event
        ''' </summary>
        ''' <param name="queries"></param>
        ''' <remarks></remarks>
        Private Shared Sub OnEnsureQueries(ByVal queries As Dictionary(Of String, String))
            Dim bucket As PaxiomManagerEventBucket
            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)

                bucket.RaiseEnsureQueriesEvent(queries)
            End If
        End Sub

        ''' <summary>
        ''' Use this to get querystring items when the default Create link method is not used
        ''' </summary>
        ''' <returns>Dictionary containing querstring key-value pairs</returns>
        ''' <remarks></remarks>
        Public Shared Function GetQueries() As Dictionary(Of String, String)
            Dim queries As New Dictionary(Of String, String)

            'Calls all subscribers to let them add their querystrings to the link
            'Used by Stateprovider
            OnEnsureQueries(queries)

            Return queries
        End Function

        ''' <summary>
        ''' Represents a querystring
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure LinkItem

            Private _key As String
            ''' <summary>
            ''' Gets or sets the key for the querystring
            ''' </summary>
            ''' <value>The <see cref="Key" /> is used as the key for the querystring</value>
            ''' <returns>A <see cref="String" /></returns>
            ''' <remarks></remarks>
            Public Property Key() As String
                Get
                    Return _key
                End Get
                Set(ByVal value As String)
                    _key = value
                End Set
            End Property


            Private _value As String
            ''' <summary>
            ''' Gets or sets the value for the querystring
            ''' </summary>
            ''' <value>The <see cref="Value" /> is used as the value for the querystring</value>
            ''' <returns>A <see cref="String" /></returns>
            ''' <remarks></remarks>
            Public Property Value() As String
                Get
                    Return _value
                End Get
                Set(ByVal value As String)
                    _value = value
                End Set
            End Property

            ''' <summary>
            ''' A constructor that takes both the key and the value of the <see cref="LinkItem" />
            ''' </summary>
            ''' <param name="key">The <see cref="Key" /> of the <see cref="LinkItem" /></param>
            ''' <param name="value">The <see cref="Value" /> of the <see cref="LinkItem" /></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal key As String, ByVal value As String)
                Me.Value = value
                Me.Key = key
            End Sub

        End Structure
    End Class


End Namespace
