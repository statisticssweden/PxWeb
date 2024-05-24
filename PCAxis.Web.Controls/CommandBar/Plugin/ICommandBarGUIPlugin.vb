Namespace CommandBar.Plugin
    ''' <summary>
    ''' Interface for a plugin that has a UI
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommandBarGUIPlugin
        Event Finished As EventHandler(Of CommandBarPluginFinishedEventArgs)
        ''' <summary>
        ''' Gets or sets the properties that the plugin has
        ''' </summary>
        ''' <value><see cref="Dictionary(Of String,String)"></see> containing the keys and values of the properties for the plugin</value>
        ''' <returns>A <see cref="Dictionary(Of String,String)"></see> containing the keys and values of the properties for the plugin</returns>
        ''' <remarks>The properties are added by the <see cref="CommandBarPluginManager"></see> when an instance of the plugin is created</remarks>
        ReadOnly Property Properties() As Dictionary(Of String, String)

        ''' <summary>
        ''' Called by the plugin when it's finished
        ''' </summary>
        ''' <param name="args">A <see cref="CommandBarPluginFinishedEventArgs" /> containing the event data</param>
        ''' <remarks></remarks>
        Sub OnFinished(ByVal args As CommandBarPluginFinishedEventArgs)

        ''' <summary>
        ''' Called by consummers of the plugin to initialize it's properties
        ''' </summary>
        ''' <param name="properties">The properties for the plugin</param>
        ''' <remarks></remarks>
        Sub InitializeProperties(ByVal properties As Dictionary(Of String, String))
    End Interface
End Namespace
