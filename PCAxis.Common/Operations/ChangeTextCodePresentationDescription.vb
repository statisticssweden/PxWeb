Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for supporting the ChangeTextCodePresentation operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeTextCodePresentationDescription
        Public PresentationDictionary As Dictionary(Of String, HeaderPresentationType)

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.PresentationDictionary = New Dictionary(Of String, HeaderPresentationType)
        End Sub

        ''' <summary>
        ''' Constructor initializing the parameter
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal presDictionary As Dictionary(Of String, HeaderPresentationType))
            Me.PresentationDictionary = presDictionary
        End Sub
    End Class

End Namespace
