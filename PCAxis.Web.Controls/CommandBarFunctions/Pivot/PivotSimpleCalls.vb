Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management
Imports PCAxis.Paxiom.Operations

Public Class PivotSimpleCalls
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequiresPaxiom

    Private Const DIRECTION_CCW As String = "CCW"
    Private Const DIRECTION_CW As String = "CW"
    Private Const PROPERTY_DIRECTION As String = "Direction"

    ''' <summary>
    ''' Pivots the supplied PaxiomModel clockwise och counter-clockwise
    ''' depending on the direction needed
    ''' </summary>
    ''' <param name="properties">Determines in what direction to pivot</param>
    ''' <remarks>Requires that the key "direction" be set in <paramref name="properties" /></remarks>
    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute
        Dim pivotFunctions As New PCAxis.Paxiom.Operations.Pivot()

        Select Case properties(PROPERTY_DIRECTION)
            Case DIRECTION_CW
                Me.PaxiomModel = pivotFunctions.PivotCW(PaxiomModel)
                PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT_CW, Nothing)
            Case DIRECTION_CCW
                Me.PaxiomModel = pivotFunctions.PivotCCW(PaxiomModel)
                PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT_CCW, Nothing)
        End Select


    End Sub

    Private _paxiomModel As Paxiom.PXModel

    Public Property PaxiomModel() As Paxiom.PXModel Implements ICommandBarRequiresPaxiom.PaxiomModel
        Get
            Return _paxiomModel
        End Get
        Set(ByVal value As Paxiom.PXModel)
            _paxiomModel = value
        End Set
    End Property
End Class
