Namespace CommandBar.Plugin
    ''' <summary>
    ''' Interface for plugins of the type <see cref="ICommandBarNoGUIPlugin" /> that needs access to the page controls
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommandBarRequireControls

        ''' <summary>
        ''' Gets or sets a <see cref="System.Web.UI.ControlCollection" /> of the page controls
        ''' </summary>
        ''' <value><see cref="System.Web.UI.ControlCollection" /> of the page controls</value>
        ''' <returns>A <see cref="System.Web.UI.ControlCollection" /> of the page controls</returns>
        ''' <remarks></remarks>
        Property PageControls() As System.Web.UI.ControlCollection

    End Interface
End Namespace

