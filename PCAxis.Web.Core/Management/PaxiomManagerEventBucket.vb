''' <summary> 
''' Class for connecting handlers with controls listening for Paxiom events.
''' ''' </summary>
''' <remarks></remarks>
Friend Class PaxiomManagerEventBucket
    ''' <summary>
    ''' Event telling that the Paxiom model has changed
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PaxiomModelChanged As EventHandler

    ''' <summary>
    ''' Event telling that the PaxiomModelBuilder is changed
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PaxiomModelBuilderChanged As EventHandler

    ''' <summary>
    ''' Raises event telling that the Paxiom model has changed
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RaisePaxiomModelChangedEvent()
        RaiseEvent PaxiomModelChanged(Nothing, New EventArgs())
    End Sub

    ''' <summary>
    ''' Raises event telling that the Paxiom model builder has changed
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RaisePaxiomModelBuilderChangedEvent()
        RaiseEvent PaxiomModelBuilderChanged(Nothing, New EventArgs())
    End Sub

    Public Event EnsureQueries As EnsureQueriesEventHandler

    Public Sub RaiseEnsureQueriesEvent(ByVal queries As Dictionary(Of String, String))
        RaiseEvent EnsureQueries(Me, New EnsureQueriesEventArgs(queries))
    End Sub


End Class

Public Delegate Sub EnsureQueriesEventHandler(sender As Object, e As EnsureQueriesEventArgs)

Public Class EnsureQueriesEventArgs
    Inherits EventArgs

    Private _queries As Dictionary(Of String, String)
    Public ReadOnly Property Queries() As Dictionary(Of String, String)
        Get
            Return _queries
        End Get
    End Property

    Public Sub New(q As Dictionary(Of String, String))
        _queries = q
    End Sub
End Class
