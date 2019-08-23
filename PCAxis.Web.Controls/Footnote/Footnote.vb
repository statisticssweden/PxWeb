Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' </summary>
''' <remarks></remarks>
Public Partial Class Footnote
    Inherits MarkerControlBase(Of FootnoteCodebehind,Footnote)

    Private _ShowMandatoryOnly As Boolean = False
    Private _ShowNoFootnotes As Boolean = True

    ''' <summary>
    ''' Gets or sets if only mandatory footnotes should be visible.
    ''' </summary>
    ''' <value>If <c>True</c> only mandatory footnotes are visible otherwise all footnotes are shown</value>
    ''' <returns><c>True</c> if only mandatory footnotes should be visible otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
Public Property ShowMandatoryOnly() As Boolean
        Get
            Return _ShowMandatoryOnly
        End Get
        Set(ByVal value As Boolean)
            _ShowMandatoryOnly = value
            If Not Me.IsLoadingState Then
                Control.GetFootNotes()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets if the text "No footnotes exists" should be visible when there are no footnotes
    ''' </summary>
    ''' <value>If <c>True</c> the text for no footnotes are shown</value>
    ''' <returns><c>True</c> if the text for no footnotes will be shown otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ShowNoFootnotes() As Boolean
        Get
            Return _ShowNoFootnotes
        End Get
        Set(ByVal value As Boolean)
            _ShowNoFootnotes = value
            If Not Me.IsLoadingState Then
                Control.GetFootNotes()
            End If
        End Set
    End Property
End Class
