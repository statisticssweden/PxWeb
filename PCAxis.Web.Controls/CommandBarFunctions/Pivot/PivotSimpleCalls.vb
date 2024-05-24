Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

Public Class PivotSimpleCalls
    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequiresPaxiom

    Private Shared FeatureUsageLogger As log4net.ILog = log4net.LogManager.GetLogger("FeatureUsage")

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
        Dim cubeId As String
        Dim logFormatString As String
        If Me.PaxiomModel.Meta.TableID Is Nothing Then
            cubeId = Me.PaxiomModel.Meta.Matrix
            logFormatString = LogFormat.FEATURE_USAGE_LOG_FORMAT_PXFILE
        Else
            cubeId = Me.PaxiomModel.Meta.TableID
            logFormatString = LogFormat.FEATURE_USAGE_LOG_FORMAT_CNMM
        End If

        Select Case properties(PROPERTY_DIRECTION)
            Case DIRECTION_CW
                Me.PaxiomModel = pivotFunctions.PivotCW(PaxiomModel)
                PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT_CW, Nothing)
                FeatureUsageLogger.InfoFormat(logFormatString, OperationConstants.PIVOT_CW, "Null", cubeId)
            Case DIRECTION_CCW
                Me.PaxiomModel = pivotFunctions.PivotCCW(PaxiomModel)
                PaxiomManager.OperationsTracker.AddStep(OperationConstants.PIVOT_CCW, Nothing)
                FeatureUsageLogger.InfoFormat(logFormatString, OperationConstants.PIVOT_CCW, "Null", cubeId)
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
