

Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core

Namespace CommandBar
    ''' <summary>
    ''' Used by <see cref="CommandBar" /> when more then one plugin is referenced and <see cref="CommandBarViewMode" /> is <see cref="CommandBarViewMode.ImageButtons" />
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommandBarPluginSelectorCodebehind
        Inherits ControlBase(Of CommandBarPluginSelectorCodebehind, CommandBarPluginSelector)
        Private Const OK_BUTTON_CAPTION As String = "CtrlCommandBarPluginSelectorOkButtonCaption"

#Region "Controls"


        Protected PluginSelectorListBox As ListBox

        Protected WithEvents PluginSelectorOkButton As Button

#End Region




        ''' <summary>
        ''' Used to load <see cref="ListBox" /> the the plugins to show
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub CommandBarPluginSelector_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'If no plugins have yet been loaded
            If PluginSelectorListBox.Items.Count = 0 Then
                For Each plugin As KeyValuePair(Of String, String) In Marker.Plugins
                    PluginSelectorListBox.Items.Add(New ListItem(Me.GetLocalizedString(plugin.Value), plugin.Key))
                Next
                PluginSelectorListBox.Items(0).Selected = True
            End If

            Me.PluginSelectorOkButton.Text = Me.GetLocalizedString(OK_BUTTON_CAPTION)
        End Sub
        ''' <summary>
        ''' Called when a user have selected a plugin and pressed the <see cref="PluginSelectorOkButton" /> button
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub PluginSelectorOkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PluginSelectorOkButton.Click
            'Fire the PluginSelected event
            Marker.OnPluginSelected(PluginSelectorListBox.SelectedValue)
        End Sub

    End Class
End Namespace