

Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Paxiom.Localization
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath
Imports PCAxis.Web.Core.Interfaces
Imports PCAxis.Web.Controls.CommandBar.Plugin

''' <summary>
''' Save dialog control.
''' </summary>
''' <remarks>
''' To use this control you also need a config file that uses the schema FileSchema.xsd.
''' <br />
''' Don't forget to set the PxModel.
''' </remarks>
Public Class SaveAsCodebehind
    Inherits PaxiomControlBase(Of SaveAsCodebehind, SaveAs)

    Private _fileFormats As ListControl
    'Private _fileGenerator As FileGenerator

#Region "Constants"
    Private Const DECIMAL_DELIMITER As String = "CtrlTableDecimalDelimiter"
    Private Const THOUSAND_DELIMITER As String = "CtrlTableThousandDelimiter"
#End Region



#Region " Controls "
    Protected WithEvents ContinueButton As Button
    Protected Footer As PlaceHolder
    Protected FileFormatDropDownList As DropDownList
    Protected FileFormatListBox As ListBox
#End Region

    Private Sub CreateFileTypeControl()
        If Footer.Controls.Count = 0 Then
            Dim selectedValue As String = _fileFormats.SelectedValue
            If Not String.IsNullOrEmpty(selectedValue) Then
                Dim ft As FileType = CommandBarPluginManager.GetFileType(selectedValue)
                Dim fileType As IFileTypeControl = CType(Activator.CreateInstance(Type.GetType(ft.WebControl)), IFileTypeControl)
                CType(fileType, Control).ID = selectedValue
                fileType.SelectedFormat = selectedValue
                fileType.SelectedFileType = ft
                Footer.Controls.Add(CType(fileType, Control))
            End If
        End If
    End Sub

    Private Sub SaveAs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton")

        '_fileGenerator = New FileGenerator(CurrentCulture)
        CreateListControl()

        If Footer.Visible Then
            CreateFileTypeControl()
        End If
    End Sub

    Public Sub CreateListControl()
        _fileFormats = DirectCast(IIf(Marker.ShowDropdowns, FileFormatDropDownList, FileFormatListBox), ListControl)
        If Not Page.IsPostBack Then
            DirectCast(IIf(Marker.ShowDropdowns, FileFormatListBox, FileFormatDropDownList), ListControl).Visible = False
            _fileFormats.DataSource = CommandBarPluginManager.FileFormats
            _fileFormats.DataTextField = "value"
            _fileFormats.DataValueField = "key"
            _fileFormats.DataBind()
        End If
    End Sub

    Private Sub _continue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click
        CreateFileTypeControl()
        Footer.Visible = True
    End Sub
End Class
