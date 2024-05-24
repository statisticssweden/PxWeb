Namespace Enums
    ''' <summary>  
    ''' Specifies how a property is to be persisted    
    ''' </summary>        
    ''' <remarks></remarks>
    Public Enum PersistStateType
        ''' <summary>
        ''' The property is persisted using a stateprovider and the type of the control
        ''' </summary>
        ''' <remarks></remarks>    
        PerRequest
        ''' <summary>
        ''' The property is persisted using a stateprovider and the type and id of the control
        ''' </summary>
        ''' <remarks></remarks>
        PerControlAndRequest
        ''' <summary>
        ''' The property is persisted in controlstate
        ''' </summary>
        ''' <remarks></remarks>
        PerControlAndPage
    End Enum
End Namespace



