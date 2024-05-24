Imports System.Drawing.Imaging
Imports PCAxis.Chart
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Management

Partial Public Class SaveAsChart : Inherits FileTypeMarkerControlBase(Of SaveAsChartCodebehind, SaveAsChart)

    Public Overrides Sub SerializeAndStream()

        Using stream As New System.IO.MemoryStream()

            Dim mimeType As String
            Dim fileExtension As String
            Dim format As ImageFormat

            Select Case Me.SelectedFormat
                Case "FileTypeChartPng"
                    mimeType = "image/png"
                    fileExtension = "png"
                    format = ImageFormat.Png
                Case "FileTypeChartGif"
                    mimeType = "image/gif"
                    fileExtension = "gif"
                    format = ImageFormat.Gif
                Case "FileTypeChartJpeg"
                    mimeType = "image/jpeg"
                    fileExtension = "jpg"
                    format = ImageFormat.Jpeg
                Case Else
                    mimeType = "image/png"
                    fileExtension = "png"
                    format = ImageFormat.Png
            End Select
            Dim ser As New ChartSerializer()
            ser.Settings = ChartManager.Settings
            ser.Format = format
            ser.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, mimeType, fileExtension)
        End Using
    End Sub

    'private int GetTextboxValue(string textboxName, int defaultValue)
    '{

    '    System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

    '    if (page == null) return defaultValue;
    '    System.Web.UI.WebControls.TextBox txt = (System.Web.UI.WebControls.TextBox)page.FindControl(textboxName);

    '    if (txt == null) return defaultValue;

    '    int value;
    '    return int.TryParse(txt.Text, out value) ? value : defaultValue;
    '}

End Class
