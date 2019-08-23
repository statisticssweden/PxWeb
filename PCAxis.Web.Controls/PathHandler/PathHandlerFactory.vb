Imports PCAxis.Web.Core.Enums

''' <summary>
''' Class for creating PathHandler objects 
''' </summary>
''' <remarks></remarks>
Public Class PathHandlerFactory

    ''' <summary>
    ''' Create PathHandler object of the right type
    ''' </summary>
    ''' <param name="databaseType">Type of database</param>
    ''' <returns>PathHandler object</returns>
    ''' <remarks></remarks>
    Public Shared Function Create(ByVal databaseType As DatabaseType) As PathHandler
        Select Case databaseType
            Case databaseType.PX
                Return New PxPathHandler()
            Case databaseType.CNMM
                Return New CnmmPathHandler()
            Case Else
                Return Nothing
        End Select
    End Function
End Class
