Namespace Enums
    ''' <summary>
    ''' States the reasons for unloading the stateprovider
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StateProviderUnloadReason
        ''' <summary>
        ''' The stateprovider is unloaded because the current asp.net request has ended
        ''' </summary>
        ''' <remarks>The stateprovider should use this reason to persist the state to it's backing store</remarks>
        PageRequestEnded
        ''' <summary>
        ''' The stateprovider is unloaded because a request has timed out
        ''' </summary>
        ''' <remarks>The stateprovider should use this reason to deleted the state from it's backing store</remarks>
        StateTimeout
    End Enum
End Namespace
