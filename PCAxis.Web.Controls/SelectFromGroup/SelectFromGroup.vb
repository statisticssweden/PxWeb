Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums


''' <summary>
''' Arguments for the SelectionDone event
''' </summary>
''' <remarks></remarks>
Public Class SelectFromGroupEventArgs
    Inherits System.EventArgs

    Public VariableCode As String
    Public Aggregation As String
    Public Includes As PCAxis.Paxiom.GroupingIncludesType
End Class

''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class SelectFromGroup
    Inherits MarkerControlBase(Of SelectFromGroupCodebehind, SelectFromGroup)

    ''' <summary>
    ''' Event used to signal when value selection is finished.
    ''' </summary>
    '''Public Event SelectionDone As EventHandler
    Public Event SelectionDone(ByVal sender As Object, ByVal e As SelectFromGroupEventArgs)
    ''' <summary>
    ''' Raises an event to signal when value selection is finished.
    ''' </summary>
    Friend Sub OnSelectionDone(ByVal args As SelectFromGroupEventArgs)
        RaiseEvent SelectionDone(Me, args)
    End Sub

    Private _variable As Paxiom.Variable
    ''' <summary>
    ''' The variable to search values for.
    ''' </summary>
    <PropertyPersistState(PersistStateType.PerControlAndRequest)> _
    Public Property Variable() As Paxiom.Variable
        Get
            Return _variable
        End Get
        Set(ByVal value As Paxiom.Variable)
            _variable = value
        End Set
    End Property

    Public Sub InitiateSearch()
        Control.InitiateSearch()
    End Sub

End Class
