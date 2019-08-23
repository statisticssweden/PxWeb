Imports PCAxis.Web.Core
Imports PCAxis.Paxiom.Localization
Imports PCAxis.Web.Core.Attributes
Imports System.Web.UI.WebControls
Imports System.Collections.ObjectModel

Namespace CommandBar
    ''' <summary>
    ''' Provides data for the <see cref="CommandBarPluginSelector.PluginSelected" /> event
    ''' </summary>    
    ''' <remarks></remarks>
    Public Class CommandBarPluginSelectorPluginSelectedEventArgs
        Inherits EventArgs

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandBarPluginSelectorPluginSelectedEventArgs"  />
        ''' </summary>
        ''' <param name="selectedPluginName">The id of the selected plugin</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal selectedPluginName As String)
            Me._selectedPluginName = selectedPluginName
        End Sub

        Private _selectedPluginName As String
        ''' <summary>
        ''' Gets the id of the selected plugin
        ''' </summary>
        ''' <value>Id of the selected plugin</value>
        ''' <returns>The id of the selected plugin</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SelectedPluginName() As String
            Get
                Return _selectedPluginName
            End Get

        End Property

    End Class
End Namespace
