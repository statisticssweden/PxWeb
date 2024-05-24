Namespace Interfaces

    ''' <summary>
    ''' Defines the interface to a filetype control
    ''' </summary>
    ''' <remarks>All controls that handles a filetype must implement the IFileTypeControl interface. Usually, a controls inherits from <see cref="FileTypeMarkerControlBase(Of TControl,TMarker)" /></remarks>
    Public Interface IFileTypeControl
        ''' <summary>
        ''' Raised when the control has finished streaming the fileformat to the browser
        ''' </summary>
        ''' <remarks></remarks>
        Event Finished As EventHandler

        ''' <summary>
        ''' Gets or sets the currently selected fileformat
        ''' </summary>
        ''' <value>Name of the currently selected fileformat</value>
        ''' <returns>The currently selcted fileformat</returns>
        ''' <remarks></remarks>
        Property SelectedFormat() As String

        ''' <summary>
        ''' Gets or sets the selected filetype
        ''' </summary>
        ''' <value>The selected filetype</value>
        ''' <returns>An instance of <see cref="FileType" /> with the selected filetype</returns>
        ''' <remarks></remarks>
        Property SelectedFileType() As FileType

        ''' <summary>
        ''' Gets or sets if user interface should be shown or not
        ''' </summary>
        ''' <value>If true user interface will be shown, else no user interface will be shown</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property ShowUI() As Boolean

        ''' <summary>
        ''' Called by hosts to save a fileformat
        ''' </summary>
        ''' <remarks></remarks>
        Sub SerializeAndStream()
    End Interface
End Namespace
