Namespace PCAxis.Paxiom.Operations
    ''' <summary>
    ''' Description class for the SortTimeVariable operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SortTimeVariableDescription

        ''' <summary>
        ''' Enumeration for sorting type
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum SortOrderType
            Ascending
            Descending
            None
        End Enum

        Public SortOrder As SortOrderType

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="sort">Type of sorting</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sort As SortOrderType)
            SortOrder = sort
        End Sub

    End Class
End Namespace
