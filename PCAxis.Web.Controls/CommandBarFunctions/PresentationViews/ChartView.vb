Imports PCAxis.Web.Controls.Table
Imports PCAxis.Web.Core.StateProvider
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Management
Imports PCAxis.Paxiom
Imports PCAxis.Paxiom.Operations

Public Class ChartView
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequireControls

    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        Dim strPage As String
        Dim linkItems As New List(Of LinkManager.LinkItem)

        ' Find web page
        strPage = Configuration.ConfigurationHelper.GetViewPage(Configuration.ConfigurationHelper.CONFIG_VIEW_CHART)

        If String.IsNullOrEmpty(strPage) Then
            strPage = "Chart.aspx"
        End If

        If Not PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel Is Nothing Then
            'PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = PivotTimeVariableToAloneInHead(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel)
            Dim pivotTimeToHead As New PCAxis.Paxiom.Operations.PivotTimeToHeading
            Dim pivotTimeToHeadDescription As New PCAxis.Paxiom.Operations.PivotTimeToHeadingDescription()

            PaxiomManager.PaxiomModel = pivotTimeToHead.Execute(PaxiomManager.PaxiomModel, pivotTimeToHeadDescription)
            PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT_TIME_TO_HEADING, pivotTimeToHeadDescription)
        End If

        ' Create URL and redirect
        If (properties.Count > 0) Then
            For Each kvp As KeyValuePair(Of String, String) In properties
                linkItems.Add(New LinkManager.LinkItem(kvp.Key, kvp.Value))
            Next

            System.Web.HttpContext.Current.Response.Redirect(Core.Management.LinkManager.CreateLink(strPage, linkItems.ToArray))
        Else
            System.Web.HttpContext.Current.Response.Redirect(Core.Management.LinkManager.CreateLink(strPage))
        End If

    End Sub

    Private _pageControls As System.Web.UI.ControlCollection
    Public Property PageControls() As System.Web.UI.ControlCollection Implements ICommandBarRequireControls.PageControls
        Get
            Return _pageControls
        End Get
        Set(ByVal value As System.Web.UI.ControlCollection)
            _pageControls = value
        End Set
    End Property

    ' ''' <summary>
    ' ''' Pivots a PXModel instance so that the time variable is the only variable in the head of the table
    ' ''' </summary>
    ' ''' <param name="px">PX model object</param>
    ' ''' <returns>PX model with the time variable as the only variable in the head</returns>
    ' ''' <remarks></remarks>
    'Private Function PivotTimeVariableToAloneInHead(ByVal px As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel
    '    Dim piv As PCAxis.Paxiom.Operations.Pivot = New PCAxis.Paxiom.Operations.Pivot()

    '    Dim q = From v In px.Meta.Variables _
    '        Select New PivotDescription(v.Name, CType(IIf(v.IsTime, PlacementType.Heading, PlacementType.Stub), PlacementType))

    '    Return piv.Execute(px, q.ToArray())
    'End Function

End Class
