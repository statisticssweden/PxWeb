Imports PCAxis.Paxiom
Imports PCAxis.Query


Namespace Management

    ''' <summary>
    ''' Used to mangare the paxiom model for the webcontrols
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class PaxiomManager
        Private Sub New()

        End Sub

        Const PAXIOMMODELID As String = "PaxiomModel"
        Const PAXIOMMODELBUILDERID As String = "PaxiomModelBuilder"
        Const PAXIOMMODELCHANGEDBUCKET As String = "PaxiomModelChangedBucket"
        Const QUERYMODELID As String = "QueryModel"
        Const OPERATIONS_TRACKER As String = "OperationsTracker"
        Const SELECTIONCONTENTID As String = " SelectionContentId"
        Private Shared _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PaxiomManager))

        ''' <summary>
        ''' Gets or sets the <see cref="PXModel" />
        ''' </summary>
        ''' <value>If there is a <see cref="PXModel" /> stored a new instance with the same content is returned, otherwise it returns <c>null</c></value>
        ''' <returns>A new instance of the currently stored <see cref="PXModel" /> or <c>null</c></returns>
        ''' <remarks>When this property is set, all controls that inheirt from <see cref="PaxiomControlBase(Of TControl,TMarker)" /> are automatically updated with the new version</remarks>
        Public Shared Property PaxiomModel() As Paxiom.PXModel
            Get
                'Fetch the current model from the stateprovider
                Dim model As PXModel = CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(PaxiomManager), PAXIOMMODELID), Paxiom.PXModel)
                'FIX: Thought it was wrong that is created a copy of the model everytime you access the property so I changed it.
                'If model IsNot Nothing Then

                '    'Create a new copy of the model
                '    Dim newModel As PXModel = New PXModel(model.Meta.CreateCopy(), model.Data)

                '    Return newModel
                'Else
                '    Return model
                'End If
                Return model
            End Get
            Set(ByVal value As Paxiom.PXModel)
                StateProvider.StateProviderFactory.GetStateProvider().Add(GetType(PaxiomManager), PAXIOMMODELID, value)
                If value IsNot Nothing Then
                    value.Meta.SetPreferredLanguage(LocalizationManager.GetTwoLetterLanguageCode())
                End If
                OnPaxiomModelChanged()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="IPXModelBuilder" />
        ''' </summary>
        ''' <value>If there is a <see cref="IPXModelBuilder" /> stored a new instance with the same content is returned, otherwise it returns <c>null</c></value>
        ''' <returns>A new instance of the currently stored <see cref="IPXModelBuilder" /> or <c>null</c></returns>
        ''' <remarks></remarks>
        Public Shared Property PaxiomModelBuilder() As Paxiom.IPXModelBuilder
            Get
                'Fetch the current model builder from the stateprovider
                Dim builder As Paxiom.IPXModelBuilder = CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(PaxiomManager), PAXIOMMODELBUILDERID), Paxiom.IPXModelBuilder)

                Return builder
            End Get
            Set(ByVal value As Paxiom.IPXModelBuilder)
                StateProvider.StateProviderFactory.GetStateProvider().Add(GetType(PaxiomManager), PAXIOMMODELBUILDERID, value)
                OnPaxiomModelBuilderChanged()
            End Set
        End Property

        ''' <summary>
        ''' Model used by PCAxis.Query to describe the table
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property QueryModel() As TableQuery
            Get
                'Fetch the Query model from the stateprovider
                Dim query As TableQuery = CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(TableQuery), QUERYMODELID), PCAxis.Query.TableQuery)
                Return query
            End Get
            Set(ByVal value As TableQuery)
                StateProvider.StateProviderFactory.GetStateProvider().Add(GetType(TableQuery), QUERYMODELID, value)
            End Set
        End Property

        ''' <summary>
        ''' Tracker for operations
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property OperationsTracker() As OperationsTracker
            Get
                'Fetch the operations tracker
                Dim query As OperationsTracker = CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(OperationsTracker), OPERATIONS_TRACKER), PCAxis.Query.OperationsTracker)
                Return query
            End Get
            Set(ByVal value As OperationsTracker)
                StateProvider.StateProviderFactory.GetStateProvider().Add(GetType(OperationsTracker), OPERATIONS_TRACKER, value)
            End Set
        End Property

        ''' <summary>
        ''' String field used in ShowApiV2URL to store the content variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property SingleContentSelection() As Dictionary(Of String, String)
            Get
                Dim content As Dictionary(Of String, String) = CType(StateProvider.StateProviderFactory.GetStateProvider().Item(GetType(Dictionary(Of String, String)), SELECTIONCONTENTID), Dictionary(Of String, String))
                Return content
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                StateProvider.StateProviderFactory.GetStateProvider().Add(GetType(Dictionary(Of String, String)), SELECTIONCONTENTID, value)
            End Set
        End Property

        Public Shared Sub Clear()
            If StateProvider.StateProviderFactory.GetStateProvider().Contains(GetType(PaxiomManager), PAXIOMMODELBUILDERID) Then
                StateProvider.StateProviderFactory.GetStateProvider().Remove(GetType(PaxiomManager), PAXIOMMODELBUILDERID)
            End If

            If StateProvider.StateProviderFactory.GetStateProvider().Contains(GetType(PaxiomManager), PAXIOMMODELID) Then
                StateProvider.StateProviderFactory.GetStateProvider().Remove(GetType(PaxiomManager), PAXIOMMODELID)
            End If
        End Sub

        ''' <summary>
        ''' Register handler for when the Paxiom model has changed
        ''' </summary>
        ''' <param name="handler"></param>
        ''' <remarks></remarks>
        Public Shared Sub RegisterPaxiomModelChanged(ByVal handler As EventHandler)
            Dim bucket As PaxiomManagerEventBucket

            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)
            Else
                bucket = New PaxiomManagerEventBucket
                System.Web.HttpContext.Current.Items.Add(PAXIOMMODELCHANGEDBUCKET, bucket)
            End If
            AddHandler bucket.PaxiomModelChanged, handler
        End Sub


        ''' <summary>
        ''' Raises the Paxiom model changed event
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub OnPaxiomModelChanged()
            Dim bucket As PaxiomManagerEventBucket = Nothing

            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)
            End If

            If bucket IsNot Nothing Then
                bucket.RaisePaxiomModelChangedEvent()
            End If
            _logger.Debug("PaxiomModel Changed")
        End Sub

        ''' <summary>
        ''' Register handler for when the Paxiom model builder has changed
        ''' </summary>
        ''' <param name="handler"></param>
        ''' <remarks></remarks>
        Public Shared Sub RegisterPaxiomModelBuilderChanged(ByVal handler As EventHandler)
            Dim bucket As PaxiomManagerEventBucket

            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)
            Else
                bucket = New PaxiomManagerEventBucket
                System.Web.HttpContext.Current.Items.Add(PAXIOMMODELCHANGEDBUCKET, bucket)
            End If
            AddHandler bucket.PaxiomModelBuilderChanged, handler
        End Sub

        ''' <summary>
        ''' Raises the Paxiom model builder changed event
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub OnPaxiomModelBuilderChanged()
            Dim bucket As PaxiomManagerEventBucket = Nothing

            If System.Web.HttpContext.Current.Items.Contains(PAXIOMMODELCHANGEDBUCKET) Then
                bucket = CType(System.Web.HttpContext.Current.Items(PAXIOMMODELCHANGEDBUCKET), PaxiomManagerEventBucket)
            End If

            If bucket IsNot Nothing Then
                bucket.RaisePaxiomModelBuilderChangedEvent()
            End If

            _logger.Debug("PaxiomModelBuilder Changed")
        End Sub
    End Class
End Namespace
