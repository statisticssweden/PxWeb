Imports System.Globalization

Namespace StateProvider
    ''' <summary>
    ''' Base class for stateproviders 
    ''' </summary>
    ''' <remarks>Stateproviders must inherit from this class</remarks>
    Public MustInherit Class StaterProviderBase

        Private _uniqueId As String

        ''' <summary>
        ''' Gets the request unique id 
        ''' </summary>
        ''' <value>Gets a string with the unique id that the stateprovider should use to separate state</value>
        ''' <returns>A string containing the unique id</returns>
        ''' <remarks>The function <see cref="GetFullName" /> uses this id to create a unique name</remarks>
        Protected ReadOnly Property UniqueId() As String
            Get
                Return _uniqueId
            End Get
        End Property

        ''' <summary>
        ''' Gets a unique name to used for storing an item in the stateprovider
        ''' </summary>
        ''' <param name="type">The <see cref="Type" /> to use as a namespace</param>
        ''' <param name="name">The name of the item</param>
        ''' <returns>A <see cref="String" /> in the form of <see cref="UniqueId" />.<paramref name="type" />.<paramref name="name" /></returns>
        ''' <remarks>Uses the <see cref="UniqueId" /> as part ot the name</remarks>
        Private Function GetFullName(ByVal type As System.Type, ByVal name As String) As String
            Return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", UniqueId, type.FullName, name)
        End Function

        ''' <summary>
        ''' Add an item to the stateprovider 
        ''' </summary>
        ''' <param name="type">The <see cref="Type" /> to use as a namespace</param>
        ''' <param name="name">The name of the item to add</param>
        ''' <param name="data">The item to add</param>
        ''' <remarks></remarks>
        Public Sub Add(ByVal type As System.Type, ByVal name As String, ByVal data As Object)
            OnAdd(GetFullName(type, name), data)
        End Sub

        ''' <summary>
        ''' Gets whether the stateprovider contains an item or not
        ''' </summary>
        ''' <param name="type">The <see cref="Type" /> to use as a namespace</param>
        ''' <param name="name">The name of the item to look for</param>
        ''' <returns><c>True</c> if the stateprovider contains the item, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal type As System.Type, ByVal name As String) As Boolean
            Return OnContains(GetFullName(type, name))
        End Function

        ''' <summary>
        ''' Gets or sets an item in the stateprovider
        ''' </summary>
        ''' <param name="type">The <see cref="Type" /> to use as a namespace</param>
        ''' <param name="name">The name of the item to get or set</param>
        ''' <value>The item to get or set</value>
        ''' <returns>An item of type <see cref="Object" /></returns>
        ''' <remarks>
        ''' A stateprovider does not remove the item if its value is sett to <c>null</c>
        ''' Use <see cref="Remove" /> to do that
        ''' </remarks>
        Public Property Item(ByVal type As System.Type, ByVal name As String) As Object
            Get
                Return OnItemGet(GetFullName(type, name))
            End Get
            Set(ByVal value As Object)
                OnItemSet(GetFullName(type, name), value)
            End Set
        End Property

        ''' <summary>
        ''' Used to initialize the <see cref="StaterProviderBase" /> and is called by the <see cref=" StateProviderFactory" /> after the stateprovider have been created
        ''' </summary>
        ''' <param name="uniqueId">The unique id that the stateprovider should use</param>
        ''' <remarks></remarks>
        Public Sub Load(ByVal uniqueId As String)
            _uniqueId = uniqueId
        End Sub

        ''' <summary>
        ''' Removes an item from the stateprovider
        ''' </summary>
        ''' <param name="type">The <see cref="Type" /> to use as a namespace</param>
        ''' <param name="name">The name of the item to remove</param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal type As System.Type, ByVal name As String)
            OnRemove(GetFullName(type, name))
        End Sub

        ''' <summary>
        ''' Called when the stateprovider should unload it's state
        ''' </summary>
        ''' <param name="reason">The <see cref="Enums.StateProviderUnloadReason" /> for unloading the state</param>
        ''' <remarks>
        ''' </remarks>
        Public Sub Unload(ByVal reason As Enums.StateProviderUnloadReason)
            OnUnLoad(reason)
        End Sub

        ''' <summary>
        ''' Called by <see cref="Add" />
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to add</param>
        ''' <param name="data">The item to add</param>
        ''' <remarks></remarks>
        Protected MustOverride Sub OnAdd(ByVal fullName As String, ByVal data As Object)

        ''' <summary>
        ''' Called by <see cref="Contains" />
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to look for</param>
        ''' <returns><c>True</c> if the stateprovider contains the item, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Protected MustOverride Function OnContains(ByVal fullName As String) As Boolean

        ''' <summary>
        ''' Called by <see cref="Item" /> when getting an item
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to get</param>
        ''' <returns>An item of type <see cref="Object" /></returns>
        ''' <remarks></remarks>
        Protected MustOverride Function OnItemGet(ByVal fullName As String) As Object

        ''' <summary>
        ''' Called by <see cref="Item" /> when setting an item
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to set</param>
        ''' <param name="data">The item to set</param>
        ''' <remarks></remarks>
        Protected MustOverride Sub OnItemSet(ByVal fullName As String, ByVal data As Object)

        ''' <summary>
        ''' Called by <see cref="Remove" />
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to remove</param>
        ''' <remarks></remarks>
        Protected MustOverride Sub OnRemove(ByVal fullName As String)

        ''' <summary>
        ''' Called by <see cref="Unload" />
        ''' </summary>
        ''' <param name="reason">The <see cref="Enums.StateProviderUnloadReason" /> for unloading the state</param>
        ''' <remarks>
        ''' If <paramref name="reason" /> is <see cref="Enums.StateProviderUnloadReason.PageRequestEnded" />
        ''' the stateprovider should save it's state if required. If <paramref name="reason" /> is <see cref=" Enums.StateProviderUnloadReason.StateTimeout" />
        ''' then the stateprovder should remove all state associated with the <see cref="UniqueId" />
        ''' </remarks>
        Protected MustOverride Sub OnUnLoad(ByVal reason As Enums.StateProviderUnloadReason)

        ''' <summary>
        ''' Called by <see cref="Load" />
        ''' </summary>
        ''' <remarks>The stateprovider should initialize any relevant data here</remarks>
        Protected MustOverride Sub OnLoad()

    End Class
End Namespace
