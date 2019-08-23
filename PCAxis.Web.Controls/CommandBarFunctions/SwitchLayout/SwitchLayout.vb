Imports PCAxis.Web.Controls.CommandBar.Plugin

Public Class SwitchLayout
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequireControls

    ''' <summary>
    ''' Switches the layout of the table on current page
    ''' </summary>
    ''' <param name="properties">Not used</param>
    ''' <remarks>If not Table is found, the function does nothing</remarks>
    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        Dim table As Table = FindTableControl(PageControls)
        If table IsNot Nothing Then
            Select Case table.Layout
                Case TableLayoutType.Layout1
                    table.Layout = TableLayoutType.Layout2
                Case TableLayoutType.Layout2
                    table.Layout = TableLayoutType.Layout1
                Case Else
                    table.Layout = TableLayoutType.Layout1
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Finds the Table control on the current page recursively 
    ''' </summary>
    ''' <param name="controls">The controlcollection to search through</param>
    ''' <returns>The Table control if found, otherwise Nothing(Null)</returns>
    ''' <remarks></remarks>
    Private Function FindTableControl(ByVal controls As System.Web.UI.ControlCollection) As Table
        Dim table As Table = Nothing
        For Each control As System.Web.UI.Control In controls
            If TypeOf (control) Is Table Then
                table = CType(control, Table)
                Exit For
            End If
            table = FindTableControl(control.Controls)
            If table IsNot Nothing Then
                Exit For
            End If
        Next
        Return table
    End Function


    Private _pageControls As System.Web.UI.ControlCollection
    Public Property PageControls() As System.Web.UI.ControlCollection Implements ICommandBarRequireControls.PageControls
        Get
            Return _pageControls
        End Get
        Set(ByVal value As System.Web.UI.ControlCollection)
            _pageControls = value
        End Set
    End Property
End Class
