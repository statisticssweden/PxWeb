''' <summary>
''' Serves as the base class for all controls that handles fileformats
''' </summary>
''' <typeparam name="TControl">The type of the usercontrol</typeparam>
''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
''' <remarks></remarks>
Public MustInherit Class FileTypeControlBase(Of TControl As FileTypeControlBase(Of TControl, TMarker), TMarker As FileTypeMarkerControlBase(Of TControl, TMarker))
    Inherits ControlBase(Of TControl, TMarker)

    '''' <summary>
    '''' Streams a file to the browser
    '''' </summary>
    '''' <param name="stream">The stream to send to the browser</param>
    '''' <param name="mimeType">The mimetype to use when sending the stream</param>
    '''' <param name="fileExtension">The file extension to use when sending the stream</param>
    '''' <remarks>The <paramref name="stream" /> is closed by this function</remarks>
    'Protected Sub StreamFile(ByVal stream As Stream, ByVal mimeType As String, ByVal fileExtension As String)
    '    stream.Position = 0
    '    Dim slength As Integer = CInt(stream.Length)

    '    Dim buffer(slength) As Byte
    '    stream.Read(buffer, 0, slength)
    '    stream.Flush()
    '    stream.Close()
    '    ' send response stream to user so they can save the file.
    '    Page.Response.Clear()
    '    Page.Response.ContentType = mimeType
    '    Page.Response.AppendHeader("Content-Length", slength.ToString())
    '    Page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + PaxiomManager.PaxiomModel.Meta.Matrix + "." + fileExtension)
    '    Page.Response.BinaryWrite(buffer)
    '    Page.Response.End()
    'End Sub

    ''' <summary>
    ''' Calls the markercontrols OnFinished method
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub OnFinished()
        Marker.OnFinished()
    End Sub

    '''' <summary>
    '''' When overridden in a derived class, serializes and streams the fileformat to the browser 
    '''' </summary>
    '''' <remarks></remarks>
    'Public MustOverride Sub SerializeAndStream()

End Class
