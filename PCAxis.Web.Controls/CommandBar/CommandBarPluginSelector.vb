Imports PCAxis.Web.Core


Namespace CommandBar
    ''' <summary>
    ''' Used to display a list of several plugins for the user to choose from
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class CommandBarPluginSelector
        Inherits MarkerControlBase(Of CommandBarPluginSelectorCodebehind, CommandBarPluginSelector)

        ''' <summary>
        ''' Fires when a plugin have been selected
        ''' </summary>
        ''' <remarks></remarks>
        Public Event PluginSelected As EventHandler(Of CommandBarPluginSelectorPluginSelectedEventArgs)

        ''' <summary>
        ''' Raises the <see cref="PluginSelected" /> event
        ''' </summary>
        ''' <param name="selectedPluginName">The name of the selcted pluign</param>
        ''' <remarks></remarks>
        Protected Friend Sub OnPluginSelected(ByVal selectedPluginName As String)
            RaiseEvent PluginSelected(Me, New CommandBarPluginSelectorPluginSelectedEventArgs(selectedPluginName))
        End Sub

#Region "Properties"
        Private _plugins As New Dictionary(Of String, String)

        ''' <summary>
        ''' Gets a <see cref="Dictionary(Of String,String)" /> of the plugins id and translated string
        ''' </summary>
        ''' <value>A <see cref="Dictionary(Of String,String)" /> where the Key represents the plugins id and 
        ''' the value the translated string</value>
        ''' <returns>A <see cref="Dictionary(Of String,String)" /> of the plugins id and translated string</returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property Plugins() As Dictionary(Of String, String)
            Get
                Return _plugins
            End Get
        End Property

#End Region
    End Class
End Namespace
