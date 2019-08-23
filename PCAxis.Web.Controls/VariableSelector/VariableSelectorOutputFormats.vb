Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums


''' <summary>
''' </summary>
''' <remarks></remarks>
Public Partial Class VariableSelectorOutputFormats
    Inherits MarkerControlBase(Of VariableSelectorOutputFormatsCodebehind,VariableSelectorOutputFormats)

    Private _presentationViews As New List(Of String)
    ''' <summary>
    ''' Available presentation views
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property PresentationViews() As List(Of String)
        Get
            Return _presentationViews
        End Get
        Set(ByVal value As List(Of String))
            _presentationViews = value
        End Set
    End Property

    Private _outputFormats As New List(Of String)
    ''' <summary>
    ''' Available file formats
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property OutputFormats() As List(Of String)
        Get
            Return _outputFormats
        End Get
        Set(ByVal value As List(Of String))
            _outputFormats = value
        End Set
    End Property

    Public Sub ShowInSelectedOutputFormat()
        Control.ShowInSelectedOutputFormat()
    End Sub

    Public Function ScreenOutput() As Boolean
        Return Me.Control.ScreenOutput()
    End Function

    ''' <summary>
    ''' Returnes the selected output format
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectedOutput() As String
        Return Me.Control.SelectedOutput()
    End Function
End Class
