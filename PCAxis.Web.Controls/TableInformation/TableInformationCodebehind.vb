Imports System.Web.UI
Imports System.ComponentModel
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' Component that displays meta information about a table
''' </summary>
''' <remarks>
''' The function SetDescription() decides if the component is displayed or not.
''' It uses information from PaxiomModel
''' </remarks>
Partial Public Class TableInformationCodebehind
    Inherits PaxiomControlBase(Of TableInformationCodebehind, TableInformation)

    Private Const TABLE_LABEL As String = "CtrlTableInformationTableLabel"
    Private _tableTitle As String

    Protected WithEvents lblTableTitle As Literal
    Protected WithEvents lblTableDescription As Label
    Protected WithEvents pnlTableDescription As Panel

    ''' <summary>
    ''' the table title
    ''' </summary>
    ''' <value>table title string</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property TableTitle() As String
        Get
            Return _tableTitle
        End Get
        Set(ByVal value As String)
            _tableTitle = value
            Select Case Marker.TitleTag
                Case TitleTags.None Or Nothing
                    If Marker.TableTitleCssClass = "" OrElse Marker.TableTitleCssClass Is Nothing Then
                        lblTableTitle.Text = "<span>" + _tableTitle + "</span>"
                    Else
                        lblTableTitle.Text = "<span class=""" + Marker.TableTitleCssClass + """>" + _tableTitle + "</span>"
                    End If
                Case Else
                    lblTableTitle.Text = "<" + Marker.TitleTag.ToString() + ">" + _tableTitle + "</" + Marker.TitleTag.ToString() + ">"
            End Select
        End Set
    End Property


    ''' <summary>
    ''' the table description
    ''' </summary>
    ''' <value>table description string</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property TableDescription() As String
        Get
            Return lblTableDescription.Text
        End Get
        Set(ByVal value As String)
            lblTableDescription.Text = value
        End Set
    End Property


    ''' <summary>
    ''' Handle prerender event.
    ''' sets title and description from the information in PaxiomModel
    ''' </summary> 
    Private Sub TableInformation_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            DisplayInformation()
        End If
        lblTableDescription.CssClass = Marker.TableDescriptionCssClass
        'lblTableTitle.CssClass = Marker.TableTitleCssClass
    End Sub

    ''' <summary>
    ''' Displays title and/or description depending on the contents of the paxiom model and 
    ''' the ShowSourcedescription and Type settings of the TableInformation control.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayInformation()
        If Me.PaxiomModel Is Nothing Then
            Return
        End If
        Select Case Me.Marker.Type
            Case TableInformation.TableInformationType.TableView
                If Me.PaxiomModel IsNot Nothing Then
                    If Not Me.PaxiomModel.Meta.DescriptionDefault AndAlso Marker.ShowSourceDescription = True AndAlso Not String.IsNullOrEmpty(Me.PaxiomModel.Meta.Description) Then
                        Me.TableTitle = Me.PaxiomModel.Meta.Description
                        Me.pnlTableDescription.Visible = False
                    Else
                        Me.Visible = False
                    End If
                End If
            Case TableInformation.TableInformationType.PresentationView
                If Not Me.PaxiomModel.Meta.DescriptionDefault Then
                    Me.TableTitle = Me.PaxiomModel.Meta.Title
                    If Not String.IsNullOrEmpty(Me.PaxiomModel.Meta.Description) Then
                        SetDescription()
                    Else
                        Me.pnlTableDescription.Visible = False
                    End If
                Else
                    If Not String.IsNullOrEmpty(Me.PaxiomModel.Meta.Description) Then
                        Me.TableTitle = Me.PaxiomModel.Meta.Description
                    Else
                        Me.Visible = False
                    End If
                End If
            Case Else
                SetTitle()
                Me.pnlTableDescription.Visible = False
                'SetDescription()
        End Select
    End Sub

    ''' <summary>
    ''' Set table title from the information in PaxiomModel
    ''' </summary>    
    Private Sub SetTitle()
        If Me.PaxiomModel IsNot Nothing Then
            Me.TableTitle = PCAxis.Util.GetModelTitle(Me.PaxiomModel)
        End If
    End Sub

    ''' <summary>
    ''' Set table description from the information in PaxiomModel
    ''' </summary>   
    Private Sub SetDescription()
        If Me.PaxiomModel IsNot Nothing Then
            If Not Me.PaxiomModel.Meta.DescriptionDefault AndAlso Marker.ShowSourceDescription = True AndAlso Not String.IsNullOrEmpty(Me.PaxiomModel.Meta.Description) Then
                Me.TableDescription = Me.GetLocalizedString(TABLE_LABEL) & " " & Me.PaxiomModel.Meta.Description
                pnlTableDescription.Visible = True
            Else
                pnlTableDescription.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Handle event language changed
    ''' </summary>
    Private Sub TableInformation_LanguageChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.LanguageChanged
        DisplayInformation()
    End Sub

    ''' <summary>
    ''' Handle event PaxiomModelChanged, ie
    ''' Me.PaxiomModel = PaxiomManager.PaxiomModel
    ''' is executed just before this event handler is triggered
    ''' </summary>
    ''' <param name="sender">not used</param>
    ''' <param name="e">not used</param>
    ''' <remarks></remarks>
    Private Sub TableInformation_PaxiomModelChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles Me.PaxiomModelChanged
        DisplayInformation()
    End Sub


    Public Enum TitleTags
        None
        H1
        H2
        H3
        H4
        H5
        H6
    End Enum
End Class



