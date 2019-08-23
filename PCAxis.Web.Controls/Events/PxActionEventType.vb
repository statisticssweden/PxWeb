''' <summary>
''' Describes the type of the PxActionEvent
''' </summary>
''' <remarks></remarks>
Public Enum PxActionEventType
    ''' <summary>
    ''' An operation has been executed on the table
    ''' </summary>
    ''' <remarks></remarks>
    Operation
    ''' <summary>
    ''' Table has been displayed (on screen, as a diagram, ...)
    ''' </summary>
    ''' <remarks></remarks>
    Presentation
    ''' <summary>
    ''' Table has been serialized to a file format
    ''' </summary>
    ''' <remarks></remarks>
    SaveAs
End Enum