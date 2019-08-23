''' <summary>
''' Specifies if and when a download link will be shown
''' </summary>
''' <remarks></remarks>
Public Enum DownloadLinkVisibilityType
    ''' <summary>
    ''' Never shows the download link
    ''' </summary>
    ''' <remarks></remarks>
    AlwaysHide
    ''' <summary>
    ''' Always shows the download link
    ''' </summary>
    ''' <remarks></remarks>
    AlwaysShow
    ''' <summary>
    ''' Shows the link if the file size is less than a specific size in kb
    ''' </summary>
    ''' <remarks></remarks>
    ShowIfSmallFile
End Enum

