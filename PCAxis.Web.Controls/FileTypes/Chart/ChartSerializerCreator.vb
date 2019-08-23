Imports System.Drawing.Imaging
Imports PCAxis.Chart

Public Class ChartSerializerCreator
    Implements PCAxis.Web.Core.ISerializerCreator

    Public Function Create(fileInfo As String) As Paxiom.IPXModelStreamSerializer Implements Core.ISerializerCreator.Create
        Dim format As ImageFormat

        Select Case fileInfo
            Case "FileTypeChartPng"
                format = ImageFormat.Png
            Case "FileTypeChartGif"
                format = ImageFormat.Gif
            Case "FileTypeChartJpeg"
                format = ImageFormat.Jpeg
            Case Else
                format = ImageFormat.Png
        End Select
        Dim ser As New ChartSerializer()
        ser.Settings = ChartManager.Settings
        ser.Format = format

        Return ser
    End Function
End Class
