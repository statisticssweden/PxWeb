Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Describes how value texts are created
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ValueTextOptionType
        ''' <summary>
        ''' Normal text exists
        ''' </summary>
        ''' <remarks></remarks>
        NormalText = 0
        ''' <summary>
        ''' Valuetext are missing for all values (use code as text)
        ''' </summary>
        ''' <remarks></remarks>
        ValueTextMissing = 1
        ''' <summary>
        ''' All values in VardeExtra (use code as text)
        ''' </summary>
        ''' <remarks></remarks>
        VardeExtra = 2
    End Enum
End Namespace