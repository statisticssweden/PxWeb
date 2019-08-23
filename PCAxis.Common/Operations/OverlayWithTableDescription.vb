Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Class for supporting the OverlayWithTable operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OverlayWithTableDescription
        Public OverlayModel As PXModel
        Public OverlayVariable As String
        Public OverlayCode1 As String
        Public OverlayCode2 As String
        Public OverlayValue1 As String
        Public OverlayValue2 As String

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Cunstructor for initializing all public parameters
        ''' </summary>
        ''' <param name="overlayModel"></param>
        ''' <param name="overlayVariable"></param>
        ''' <param name="overlayCode1"></param>
        ''' <param name="overlayCode2"></param>
        ''' <param name="overlayValue1"></param>
        ''' <param name="overlayValue2"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal overlayModel As PXModel, _
                            ByVal overlayVariable As String, _
                            ByVal overlayCode1 As String, _
                            ByVal overlayCode2 As String, _
                            ByVal overlayValue1 As String, _
                            ByVal overlayValue2 As String)
            Me.OverlayModel = overlayModel
            Me.OverlayVariable = overlayVariable
            Me.OverlayCode1 = overlayCode1
            Me.OverlayCode2 = overlayCode2
            Me.OverlayValue1 = overlayValue1
            Me.OverlayValue2 = overlayValue2
        End Sub

    End Class
End Namespace