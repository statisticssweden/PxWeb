Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums
Imports PCAxis.Web.Core.Interfaces
Imports System.IO
Imports PCAxis.Web.Core.Management

''' <summary>
''' Serves as the base class for all markercontrols that handles fileformats
''' </summary>
''' <typeparam name="TControl">The type of the usercontrol</typeparam>
''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
''' <remarks></remarks>
<ComponentModel.ToolboxItem(False)> _
Public Class FileTypeMarkerControlBase(Of TControl As FileTypeControlBase(Of TControl, TMarker), TMarker As FileTypeMarkerControlBase(Of TControl, TMarker))
    Inherits MarkerControlBase(Of TControl, TMarker)
    Implements IFileTypeControl


    Private _selectedFormat As String
    Private _selectedFileType As FileType
    Private _showUI As Boolean

    ''' <summary>
    ''' Raised when the control has finished streaming the fileformat to the browser
    ''' </summary>
    ''' <remarks>Never fires in the current implementation</remarks>
    Public Event Finished As EventHandler Implements IFileTypeControl.Finished

    ''' <summary>
    ''' Gets or sets the currently selected fileformat
    ''' </summary>
    ''' <value>Name of the currently selected fileformat</value>
    ''' <returns>The currently selcted fileformat</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedFormat() As String Implements IFileTypeControl.SelectedFormat
        Get
            Return _selectedFormat
        End Get
        Set(ByVal value As String)
            _selectedFormat = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the selected filetype
    ''' </summary>
    ''' <value>The selected filetype</value>
    ''' <returns>An instance of <see cref="FileType" /> with the selected filetype</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SelectedFileType() As FileType Implements IFileTypeControl.SelectedFileType
        Get
            Return _selectedFileType
        End Get
        Set(ByVal value As FileType)
            _selectedFileType = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets if user interface will be shown or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowUI() As Boolean Implements IFileTypeControl.ShowUI
        Get
            Return _showUI
        End Get
        Set(ByVal value As Boolean)
            _showUI = value
        End Set
    End Property

    ''' <summary>
    ''' Raises the Finished event
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub OnFinished()
        RaiseEvent Finished(Me, New EventArgs())
    End Sub

    '''' <summary>
    '''' Calls the <see cref="Control" /> SerializeAndStream method
    '''' </summary>
    '''' <remarks></remarks>
    'Protected Sub SerializeAndStream() Implements IFileTypeControl.SerializeAndStream
    '    Control.SerializeAndStream()
    'End Sub

    Public Overridable Sub SerializeAndStream() Implements IFileTypeControl.SerializeAndStream
    End Sub

    ''' <summary>
    ''' Streams a file to the browser
    ''' </summary>
    ''' <param name="stream">The stream to send to the browser</param>
    ''' <param name="mimeType">The mimetype to use when sending the stream</param>
    ''' <param name="fileExtension">The file extension to use when sending the stream</param>
    ''' <remarks>The <paramref name="stream" /> is closed by this function</remarks>
    Protected Sub StreamFile(ByVal stream As Stream, ByVal mimeType As String, ByVal fileExtension As String)
        stream.Position = 0
        Dim slength As Integer = CInt(stream.Length)

        Dim buffer(slength - 1) As Byte
        stream.Read(buffer, 0, slength)
        stream.Flush()
        stream.Close()
        ' send response stream to user so they can save the file.
        Dim r As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
        r.Clear()
        r.ContentType = mimeType
        r.AppendHeader("Content-Length", slength.ToString())
        ''Dim a As String = PaxiomManager.PaxiomModel.Meta.Matrix
        'a.en(System.Text.Encoding.GetEncoding(model.Meta.CodePage))
        r.HeaderEncoding = (System.Text.Encoding.GetEncoding(PaxiomManager.PaxiomModel.Meta.CodePage))
        r.AppendHeader("Content-Disposition", "attachment; filename=" + FileBaseName() + "." + fileExtension)
        r.BinaryWrite(buffer)
        'r.End()

        r.Flush()
        r.SuppressContent = True
        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest()

        'Page.Response.Clear()
        'Page.Response.ContentType = mimeType
        'Page.Response.AppendHeader("Content-Length", slength.ToString())
        'Page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + PaxiomManager.PaxiomModel.Meta.Matrix + "." + fileExtension)
        'Page.Response.BinaryWrite(buffer)
        'Page.Response.End()
    End Sub

    ''' <summary>
    ''' Create filename from selection in the settings-file
    ''' </summary>
    ''' <returns></returns>
    Private Function FileBaseName() As String

        If Settings.Files.FileBaseName.Equals(PCAxis.Paxiom.FileBaseNameType.Matrix) AndAlso Not String.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.Matrix) Then
            Return PaxiomManager.PaxiomModel.Meta.Matrix + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss")

        ElseIf Settings.Files.FileBaseName.Equals(PCAxis.Paxiom.FileBaseNameType.TableID) AndAlso Not String.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.TableID) Then
            Return PaxiomManager.PaxiomModel.Meta.TableID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss")

        Else
            Return PaxiomManager.PaxiomModel.Meta.Matrix + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss")
        End If


    End Function
End Class
