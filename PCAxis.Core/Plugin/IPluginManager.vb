Namespace PCAxis.PlugIn

    ''' <summary>
    ''' A manager for loading an activating plugins
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPlugInManager

        ''' <summary>
        ''' Loads a plugin by id
        ''' </summary>
        ''' <param name="id">the identity of the plugin</param>
        ''' <returns>A new instans of the plugin</returns>
        ''' <remarks></remarks>
        Function GetPlugIn(ByVal id As Guid) As IPlugIn

        ''' <summary>
        ''' Loads a plugin by id
        ''' </summary>
        ''' <param name="id">the identity of the plugin which should be a guid</param>
        ''' <returns>A new instans of the plugin</returns>
        ''' <remarks></remarks>
        Function GetPlugIn(ByVal id As String) As IPlugIn

        ''' <summary>
        ''' Gets all plugins that implements a specific interface.
        ''' </summary>
        ''' <param name="interface">name of the interface that the plugin should implement</param>
        ''' <returns>and array of new plugin instances</returns>
        ''' <remarks></remarks>
        Function GetPlugIns(ByVal [interface] As String) As IPlugIn()

    End Interface

End Namespace