Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Exception that should be thrown when a operation fails to execute
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PXOperationException
        Inherits PCAxis.Paxiom.PXException

#Region "Constructors"

        Public Sub New()

        End Sub

        '''<summary>
        '''</summary>
        '''<param name="message">The error message</param>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        
        Public Sub New(ByVal message As String, ByVal errorCode As String)
            MyBase.New(message)
            Me.m_errorCode = errorCode
        End Sub


        Public Sub New(ByVal message As String, ByVal ex As System.Exception)
            MyBase.New(message, ex)
            Me.m_errorCode = ErrorCode
        End Sub

#End Region


    End Class
End Namespace

