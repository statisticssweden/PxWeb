Namespace PCAxis.Paxiom.Operations

    ''' <summary>
    ''' Class for supporting the ChangeDecimals operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeDecimalsDescription
        ''' <summary>
        ''' Desired number of decimals 
        ''' </summary>
        ''' <remarks></remarks>
        Public Decimals As Integer

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor initializing the decimals parameter
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal decimals As Integer)
            Me.Decimals = decimals
        End Sub

    End Class

End Namespace
