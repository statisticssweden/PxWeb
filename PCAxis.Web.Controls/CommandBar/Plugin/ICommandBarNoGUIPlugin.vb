Namespace CommandBar.Plugin
    ''' <summary>
    ''' Interface for plugins that dont's have a UI and only executes as a function
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommandBarNoGUIPlugin

        ''' <summary>
        ''' Executes the plugin using the supplied <paramref name="properties" />
        ''' </summary>
        ''' <param name="properties">The properties the plugins needs to execut</param>
        ''' <remarks></remarks>
        Sub Execute(ByVal properties As Dictionary(Of String, String))


    End Interface
End Namespace
