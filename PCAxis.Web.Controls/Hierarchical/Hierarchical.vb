Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Hierarchical
    Inherits MarkerControlBase(Of HierarchicalCodebehind, Hierarchical)
    Public Event SelectionsDone(ByVal sender As Object, ByVal e As EventArgs, ByVal hierarchicalVariableName As String)

    Friend Sub OnSelectionsDone()
        RaiseEvent SelectionsDone(Me, New EventArgs(), Variable.Name)
    End Sub

#Region " Properties "

    ''' <summary>
    ''' Gets or sets the variable this controll should present
    ''' </summary>
    ''' <remarks></remarks>
    Protected _variable As Paxiom.Variable
    <PropertyPersistState(PersistStateType.PerControlAndRequest)> _
    Public Property Variable() As Variable
        Get
            Return _variable
        End Get
        Set(ByVal value As Variable)
            _variable = value
        End Set
    End Property


    Protected _showbuttonlabels As Boolean = False
    Public Property ShowButtonLabels() As Boolean
        Get
            Return _showbuttonlabels
        End Get
        Set(ByVal value As Boolean)
            _showbuttonlabels = value
        End Set
    End Property


    Protected _hierachylevelsopen As Integer
    Public Property HierarchyLevelsOpen() As Integer
        Get
            Return _hierachylevelsopen
        End Get
        Set(ByVal value As Integer)
            _hierachylevelsopen = value
        End Set
    End Property

#End Region

    Public Sub RenderHierarchicalTree()
        Control.RenderHierarchicalTree()
    End Sub

End Class
