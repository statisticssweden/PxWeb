Imports PCAxis.Paxiom.Operations
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management

Public Class SplitTimeVariable

    Implements ICommandBarNoGUIPlugin
    Implements ICommandBarRequiresPaxiom


    Private _logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(SplitTimeVariable))

#Region "Properties"
    Private _pageControls As System.Web.UI.ControlCollection
    Private _model As PCAxis.Paxiom.PXModel



    Public Property PaxiomModel() As PCAxis.Paxiom.PXModel Implements CommandBar.Plugin.ICommandBarRequiresPaxiom.PaxiomModel
        Get
            Return _model
        End Get
        Set(ByVal value As PCAxis.Paxiom.PXModel)
            _model = value
        End Set
    End Property
#End Region


    ''' <summary>
    ''' Splits timevariable
    ''' </summary>
    ''' <param name="properties">Not used</param>
    ''' <remarks></remarks>
    Public Sub Execute(ByVal properties As System.Collections.Generic.Dictionary(Of String, String)) Implements ICommandBarNoGUIPlugin.Execute

        Try
            Dim opSplit As New PCAxis.Paxiom.Operations.SplitTimevariable
            _model = opSplit.Execute(_model, Nothing)
            UpdateOperationsTracker()
        Catch ex As PCAxis.Paxiom.PXOperationException
        Catch ex As Exception
        End Try
    End Sub

    Private Sub UpdateOperationsTracker()
        PaxiomManager.OperationsTracker.AddStep(OperationConstants.SPLIT_TIME, Nothing)
        PaxiomManager.OperationsTracker.IsTimeDependent = True
    End Sub



End Class
