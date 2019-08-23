Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class VariableSelectorMarkingTips
    Inherits MarkerControlBase(Of VariableSelectorMarkingTipsCodebehind, VariableSelectorMarkingTips)

#Region "Local variables"

    Private _markingTipsLinkNavigateUrl As String

#End Region
#Region "Properties"
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MarkingTipsLinkNavigateUrl() As String
        Get
            Return _markingTipsLinkNavigateUrl
        End Get
        Set(ByVal value As String)
            _markingTipsLinkNavigateUrl = value
        End Set
    End Property

#End Region


End Class
