''' <summary>
''' Specifies the type of transformation the table can have
''' </summary>
''' <remarks></remarks>
Public Enum TableTransformationType
    ''' <summary>
    ''' No tranformation of the table
    ''' </summary>
    ''' <remarks></remarks>
    NoTransformation
    ''' <summary>
    ''' Transform the table so that it can be sorted
    ''' </summary>
    ''' <remarks></remarks>
    Sort
    ''' <summary>
    ''' Transform the table so that variable with only one value is put first
    ''' </summary>
    ''' <remarks></remarks>
    SingleValueFirst
    ''' <summary>
    ''' Transform the table so that variable with only one value is put first and there can only be on
    ''' variable with multiple values in the header
    ''' </summary>
    ''' <remarks></remarks>
    SingleValueFirstAndHeaderOnlyOneMultiple
End Enum