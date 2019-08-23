Imports PCAxis.Web.Core
Namespace CommandBar.Plugin
    ''' <summary>
    ''' Baseclass for all markercontrols for plugins that has a UI
    ''' </summary>
    ''' <typeparam name="TControl">The type of the usercontrol</typeparam>
    ''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
    ''' <remarks></remarks>
    <ComponentModel.ToolboxItem(False)> _
    Public MustInherit Class CommandBarMarkerControlBase(Of TControl As CommandBarPluginBase(Of TControl, TMarker), TMarker As CommandBarMarkerControlBase(Of TControl, TMarker))
        Inherits MarkerControlBase(Of TControl, TMarker)
        Implements ICommandBarGUIPlugin

        ''' <summary>
        ''' Fires when the plugin is finished
        ''' </summary>
        ''' <remarks></remarks>
        Event Finished As EventHandler(Of CommandBarPluginFinishedEventArgs) Implements ICommandBarGUIPlugin.Finished

        Private _properties As Dictionary(Of String, String)
        ''' <summary>
        ''' Gets or sets the properties that the plugin has
        ''' </summary>
        ''' <value></value>
        ''' <returns>A <see cref="Dictionary(Of String,String)" /> containing the keys and values of the properties for the plugin</returns>
        ''' <remarks>The properties are added by the <see cref="CommandBarPluginManager" /> when an instance of the plugin is created</remarks>
        Public ReadOnly Property Properties() As Dictionary(Of String, String) Implements ICommandBarGUIPlugin.Properties
            Get
                Return _properties
            End Get
        End Property

        ''' <summary>
        ''' Fires the <see cref="Finished" /> event 
        ''' </summary>
        ''' <param name="args">The <see cref="CommandBarPluginFinishedEventArgs" /> to use when firing the event</param>
        ''' <remarks></remarks>
        Public Sub OnFinished(ByVal args As CommandBarPluginFinishedEventArgs) Implements ICommandBarGUIPlugin.OnFinished
            RaiseEvent Finished(Me, args)
        End Sub

        ''' <summary>
        ''' Used by the <see cref="CommandBarPluginManager" /> to  initialize the plugins properties
        ''' </summary>
        ''' <param name="properties">A <see cref="Dictionary(Of String, String)" /> containing the properties for the plugin</param>
        ''' <remarks></remarks>
        Protected Friend Sub InitializeProperties(ByVal properties As Dictionary(Of String, String)) Implements ICommandBarGUIPlugin.InitializeProperties
            Me._properties = properties
        End Sub


    End Class
End Namespace
