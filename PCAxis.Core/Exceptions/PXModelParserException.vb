Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Exception that should be used when a unrecoverable error occurs in the PXModelParser 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PXModelParserException
        Inherits PCAxis.Paxiom.PXException


#Region "Constructors"

        Public Sub New()

        End Sub


        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal errorCode As String)
            MyBase.New(message)
            Me.m_errorCode = errorCode
        End Sub


        Public Sub New(ByVal message As String, ByVal ex As System.Exception)
            MyBase.New(message, ex)
            Me.m_errorCode = "N/A"
        End Sub

#End Region


    End Class
End Namespace