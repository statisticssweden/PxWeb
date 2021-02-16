Imports PCAxis.Web.Core
Namespace CommandBar.Plugin
    ''' <summary>
    ''' Base class for all <see cref="CommandBar" /> plugins that has a user interface
    ''' </summary>
    ''' <typeparam name="TControl">The type of the usercontrol</typeparam>
    ''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
    ''' <remarks></remarks>
    Public MustInherit Class CommandBarPluginBase(Of TControl As CommandBarPluginBase(Of TControl, TMarker), TMarker As CommandBarMarkerControlBase(Of TControl, TMarker))
        Inherits PaxiomControlBase(Of TControl, TMarker)

        Private Shared FeatureUsageLogger As log4net.ILog = log4net.LogManager.GetLogger("FeatureUsage")

        Private _properties As Dictionary(Of String, String)
        ''' <summary>
        ''' Gets or sets the properties that the plugin has
        ''' </summary>
        ''' <value></value>
        ''' <returns>A <see cref="Dictionary(Of String,String)" /> containing the keys and values of the properties for the plugin</returns>
        ''' <remarks>The properties are added by the <see cref="CommandBarPluginManager" /> when an instance of the plugin is created</remarks>
        Public ReadOnly Property Properties() As Dictionary(Of String, String)
            Get
                Return Marker.Properties
            End Get
        End Property

        ''' <summary>
        ''' Fires the <see cref="CommandBarMarkerControlBase(Of TControl,TMarker).Finished" /> event 
        ''' </summary>
        ''' <param name="args">The <see cref="CommandBarPluginFinishedEventArgs" /> to use when firing the event</param>
        ''' <remarks></remarks>
        Protected Sub OnFinished(ByVal args As CommandBarPluginFinishedEventArgs)
            Marker.OnFinished(args)
        End Sub

        Protected Sub LogFeatureUsage(ByVal featureType As String, ByVal featureVariant As String, ByVal maintable As String)
            FeatureUsageLogger.InfoFormat(LogFormat.FEATURE_USAGE_LOG_FORMAT, featureType, featureVariant, maintable)
        End Sub

        Protected Sub LogFeatureUsage(ByVal featureType As String, ByVal maintable As String)
            LogFeatureUsage(featureType, "Null", maintable)
        End Sub
    End Class

    Public Class LogFormat
        Public Const FEATURE_USAGE_LOG_FORMAT As String = "Feature={0}, Variant={1}, maintable={2}"
    End Class


End Namespace



