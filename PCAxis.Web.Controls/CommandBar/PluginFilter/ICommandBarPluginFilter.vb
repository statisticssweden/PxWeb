Imports PCAxis.Web.Core

Namespace CommandBar.Plugin
    ''' <summary>
    ''' Interface for PluginFilter
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommandBarPluginFilter
        ''' <summary>
        ''' Checks if a plugin should be used used with filter applied
        ''' </summary>
        ''' <param name="plugin">Plugin to check</param>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if plugin should be used used with filter applied</returns>
        ''' <remarks>Checks by filter list and logical rules applied on some plugins</remarks>
        Function UsePlugin(ByVal plugin As CommandBarPluginInfo, ByRef model As Paxiom.PXModel, ByVal pluginCategory As String) As Boolean

        Function UseFiletype(ByVal ftype As PCAxis.Web.Core.FileType) As Boolean

        Function UseOutputFormat(ByVal outputFormat As String) As Boolean

        ReadOnly Property DropDownOperationsActive() As Boolean
        ReadOnly Property DropDownFileFormatsActive() As Boolean
        ReadOnly Property DropDownViewsActive() As Boolean


    End Interface
End Namespace
