Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Exception  that should be thrown when a exception uccurs in the serializer
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PXSerializationException
        Inherits PCAxis.Paxiom.PXException

#Region "Constructors"

        Public Sub New()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="message"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal errorCode As String)
            MyBase.New(message)
            Me.m_errorCode = errorCode
        End Sub

        Public Sub New(ByVal message As String, ByVal ex As System.Exception)
            MyBase.New(message, ex)
            Me.m_errorCode = "N/A" 'TODO set it to unknown
        End Sub
#End Region

    End Class
End Namespace
