Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' Component that displays meta information about a table
''' </summary>
''' <remarks>
''' </remarks>
Public Partial Class TableInformation
    Inherits MarkerControlBase(Of TableInformationCodebehind, TableInformation)

    ''' <summary>
    ''' Type of tableinformation
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TableInformationType
        ''' <summary>
        ''' For TableInformationType = Normal the following is true:
        ''' 
        ''' If the Paxiom model contains DESCRIPTIONDEFAULT then the DESCRIPTION of the model is displayed 
        ''' in the main headline and the text area is hidden.
        ''' 
        ''' If the Paxiom model does not contain DESCRIPTIONDEFAULT the TITLE of the Paxiom model is displayed 
        ''' in the main headline. If the Table Information webcontrol is configured with SourceDescription = True 
        ''' the DESCRIPTION of the model is also displayed in the text area
        ''' </summary>
        ''' <remarks></remarks>
        Normal
        ''' <summary>
        ''' For TableInformationType = TableView the following is true:
        ''' 
        ''' If the Paxiom model does not contain DESCRIPTIONDEFAULT and the Table Information webcontrol is 
        ''' configured with SourceDescription = true the DESCRIPTION of the Paxiom model is displayed in the 
        ''' main headline. For all other cases the whole webcontrol is hidden.
        ''' </summary>
        ''' <remarks></remarks>
        TableView
        ''' <summary>
        ''' Show dynamically generated title as main headline.
        ''' If the Table Information webcontrol is configured with SourceDescription = true then also show description (if it exist) in text area
        ''' </summary>
        ''' <remarks></remarks>
        PresentationView
    End Enum

    Private _showSourceDescription As Boolean
    Private _type As TableInformationType = TableInformationType.Normal
    Private _tableTitleCssClass As String = "tableinformation_title"
    Private _tableDescriptionCssClass As String = "tableinformation_description"
    Private _titleTag As TableInformationCodebehind.TitleTags

    ''' <summary>
    ''' Gets or sets whether to show the text from <see cref="Paxiom.PXMeta.Description" />
    ''' </summary>
    ''' <value>If <c>True</c> the text from <see cref="Paxiom.PXMeta.Description" /> is shown, oterwise it is not shown</value>
    ''' <returns><c>True</c> if the text from <see cref="Paxiom.PXMeta.Description" /> is to be shown, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ShowSourceDescription() As Boolean
        Get
            Return _showSourceDescription
        End Get
        Set(ByVal value As Boolean)
            _showSourceDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets type of presentation-behaviour of the TableInformation webcontrol
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property Type() As TableInformationType
        Get
            Return _type
        End Get
        Set(ByVal value As TableInformationType)
            _type = value
        End Set
    End Property

    ''' <summary>
    ''' Sets/Get the html tag for the title in the Title control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>None means that a Span tag with css-class will be used. All other cases will not use the defined css-class for the title.</remarks>
    <Bindable(True), Browsable(True), Category("Default"), DefaultValue(""), Description("Set/Get titletag."), PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property TitleTag() As TableInformationCodebehind.TitleTags
        Get
            Return _titleTag
        End Get
        Set(ByVal value As TableInformationCodebehind.TitleTags)
            _titleTag = value
        End Set
    End Property

    ''' <summary>
    ''' Sets/Get the CSS class on the Title control.
    ''' </summary>
    ''' <value>This is currently "hierarchical_tableinformation_title" (defined in hierarchical.css)</value>
    ''' <returns></returns>
    ''' <remarks>used by lblTableTitle</remarks>
    <Bindable(True), Browsable(True), Category("Default"), DefaultValue(""), Description("Set/Get CSS class."), PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property TableTitleCssClass() As String
        Get
            Return _tableTitleCssClass
        End Get
        Set(ByVal value As String)
            _tableTitleCssClass = value
        End Set
    End Property

    ''' <summary>
    ''' Sets/Get the CSS class on the Description control.
    ''' </summary>
    ''' <value>This is currently "hierarchical_tableinformation_description" (defined in hierarchical.css)</value>
    ''' <returns></returns>
    ''' <remarks>Used by lblTableDescription</remarks>
    <Bindable(True), Browsable(True), Category("Default"), DefaultValue(""), Description("Set/Get CSS class."), PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property TableDescriptionCssClass() As String
        Get
            Return _tableDescriptionCssClass
        End Get
        Set(ByVal value As String)
            _tableDescriptionCssClass = value
        End Set
    End Property

End Class
