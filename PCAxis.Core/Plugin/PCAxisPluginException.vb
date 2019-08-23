Namespace PCAxis.PlugIn

    ''' <summary>
    '''  This exception should be thrown when a problem with 
    ''' locating och activating a plugin occurs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PCAxisPlugInException
        Inherits PCAxis.Paxiom.PXException

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

        Public Sub New(ByVal msg As String, ByVal ex As Exception)
            MyBase.New(msg, ex)
        End Sub

    End Class

End Namespace