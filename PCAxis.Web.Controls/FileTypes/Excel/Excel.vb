Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core.Management



''' <summary>
''' With this component you can make specific settings 
''' for how an Excel file will be created.
''' 
''' For example decide format:
''' Excel file from screen (xlsx)   or
''' Excel file (xlsx) with code and text column
''' </summary>
''' <remarks></remarks>
Partial Public Class Excel
    Inherits FileTypeMarkerControlBase(Of ExcelCodebehind, Excel)
    ''' <summary>
    ''' Creates an Excel file and sends it to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SerializeAndStream()
        Using stream As IO.MemoryStream = New IO.MemoryStream()
            Dim excelSerializer As New ExcelFileSerializer

            If Me.SelectedFormat.Equals(Plugins.FileFormats.XLS_DOUBLE_COLUMN) Then
                excelSerializer.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns
            Else
                'Get doublecolumn from application setting
                excelSerializer.DoubleColumn = Settings.Files.DoubleColumnFile
            End If

            'Get information level from application setting
            excelSerializer.InformationLevel = Settings.Files.CompleteInfoFile
            'End If

            excelSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
            StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        End Using
    End Sub

End Class
