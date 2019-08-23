Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class SearchVariableValues
    Inherits MarkerControlBase(Of SearchVariableValuesCodebehind, SearchVariableValues)

    ''' <summary>
    ''' Event used to signal when value selection is finished.
    ''' </summary>
    Public Event SelectionsDone As EventHandler
    ''' <summary>
    ''' Raises an event to signal when value selection is finished.
    ''' </summary>
    Friend Sub OnSearchVariableValuesAdd(ByVal args As EventArgs)
        RaiseEvent SelectionsDone(Me, args)
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

End Class
