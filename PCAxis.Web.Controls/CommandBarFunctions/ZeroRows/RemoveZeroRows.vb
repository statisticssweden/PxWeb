Imports PCAxis.Web.Controls.CommandBar.Plugin

Public Class RemoveZeroRows
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequireControls

    'Private types As New System.Collections.Generic.Dictionary(Of String, Integer)
    Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(RemoveZeroRows))



    ''' <summary>
    ''' Switches the layout of the table on current page
    ''' </summary>
    ''' <param name="properties">"ZeroRowType",Paxiom.ZeroOptionType</param>
    ''' <remarks>If not Table is found, the function does nothing. Default opartaion if something goes wrong is ShowAll</remarks>
    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute

        Dim zeroOperation As Paxiom.ZeroOptionType = Paxiom.ZeroOptionType.ShowAll
        'Get ZeroOperation
        If (Not IsNothing(properties) AndAlso properties.Count > 0) Then
            Try
                zeroOperation = CType([Enum].Parse(GetType(Paxiom.ZeroOptionType), properties("ZeroRowType")), Paxiom.ZeroOptionType)
            Catch ex As ArgumentException
                _logger.WarnFormat("Zero operation {0} for Paxiom.ZeroOptionType does not exist.", properties("ZeroRowType"))
            End Try
        End If

        'Set table layout
        Dim table As Table = FindTableControl(PageControls)
        If table IsNot Nothing Then
            table.RemoveRowsOption = zeroOperation
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
