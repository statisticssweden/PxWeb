Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' </summary>
''' <remarks></remarks>
Public Partial Class SaveAs
    Inherits MarkerControlBase(Of SaveAsCodebehind, SaveAs)

    Private _showDropdowns As Boolean = False

    ''' <summary>
    ''' Change default view to dropdowns.
    ''' </summary>
    ''' <value>If <c>True</c> dropdowns is used to render the control</value>
    ''' <returns><c>True</c> if the control uses dropdowns, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Property ShowDropdowns() As Boolean
        Get
            Return _showDropdowns
        End Get
        Set(ByVal value As Boolean)
            _showDropdowns = value
        End Set
    End Property
End Class
