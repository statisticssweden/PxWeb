Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' </summary>
''' <remarks></remarks>
Public Partial Class Footnote
    Inherits MarkerControlBase(Of FootnoteCodebehind, Footnote)

    Private _ShowMandatoryOnly As Boolean = False
    Private _ShowNoFootnotes As Boolean = True
    Private _InAccordionStyle As Boolean = False

    ''' <summary>
    ''' Gets or sets if only mandatory footnotes should be visible.
    ''' </summary>
    ''' <value>If <c>True</c> only mandatory footnotes are visible otherwise all footnotes are shown</value>
    ''' <returns><c>True</c> if only mandatory footnotes should be visible otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)>
    Public Property ShowMandatoryOnly() As Boolean
        Get
            Return _ShowMandatoryOnly
        End Get
        Set(ByVal value As Boolean)
            _ShowMandatoryOnly = value
            If Not Me.IsLoadingState Then
                'Mergeparty: Or do we need these?
                '           Control.GetFootNotes()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets if the text "No footnotes exists" should be visible when there are no footnotes
    ''' </summary>
    ''' <value>If <c>True</c> the text for no footnotes are shown</value>
    ''' <returns><c>True</c> if the text for no footnotes will be shown otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)>
    Public Property ShowNoFootnotes() As Boolean
        Get
            Return _ShowNoFootnotes
        End Get
        Set(ByVal value As Boolean)
            _ShowNoFootnotes = value
            If Not Me.IsLoadingState Then
                '          Control.GetFootNotes()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets if the text "No footnotes exists" should be visible when there are no footnotes
    ''' </summary>
    ''' <value>If <c>True</c> the text for no footnotes are shown</value>
    ''' <returns><c>True</c> if the text for no footnotes will be shown otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)>
    Public Property InAccordionStyle() As Boolean
        Get
            Return _InAccordionStyle
        End Get
        Set(ByVal value As Boolean)
            'TODO chech setting

            _InAccordionStyle = value

            If Not Me.IsLoadingState Then
                '  Control.GetFootNotes()
            End If
        End Set
    End Property

End Class
