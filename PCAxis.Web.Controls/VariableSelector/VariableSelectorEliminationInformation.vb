Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class VariableSelectorEliminationInformation
    Inherits MarkerControlBase(Of VariableSelectorEliminationInformationCodebehind, VariableSelectorEliminationInformation)

    Private _eliminationImagePath As String
#Region "Properties"

    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property EliminationImagePath() As String
        Get
            Return _eliminationImagePath
        End Get
        Set(ByVal value As String)
            _eliminationImagePath = value
        End Set
    End Property


#End Region

End Class
