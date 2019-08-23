Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class SearchValues
    Inherits MarkerControlBase(Of SearchValuesCodebehind, SearchValues)

    ''' <summary>
    ''' Event used to signal when value selection is finished.
    ''' </summary>
    Public Event SelectionsDone As EventHandler
    ''' <summary>
    ''' Raises an event to signal when value selection is finished.
    ''' </summary>
    Friend Sub OnSearchVariableValuesAdd(ByVal args As EventArgs)
        RaiseEvent SelectionsDone(Me, args)
    End Sub

    Private _variable As Paxiom.Variable
    ''' <summary>
    ''' The variable to search values for.
    ''' </summary>
    <PropertyPersistState(PersistStateType.PerControlAndRequest)> _
    Public Property Variable() As Paxiom.Variable
        Get
            Return _variable
        End Get
        Set(ByVal value As Paxiom.Variable)
            _variable = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets wheather the information link shall be displayed or not
    ''' </summary>
    ''' <remarks></remarks>
    Private _showSearchInformationLink As Boolean = False
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowSearchInformationLink() As Boolean
        Get
            Return _showSearchInformationLink
        End Get
        Set(ByVal value As Boolean)
            _showSearchInformationLink = value
        End Set
    End Property


    ''' <summary>
    ''' Text of the Search information link
    ''' </summary>
    ''' <remarks></remarks>
    Private _searchInformationLinkText As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SearchInformationLinkText() As String
        Get
            Return _searchInformationLinkText
        End Get
        Set(ByVal value As String)
            _searchInformationLinkText = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the URL of the search information link
    ''' </summary>
    ''' <remarks></remarks>
    Private _searchInformationLinkURL As String
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property SearchInformationLinkURL() As String
        Get
            Return _searchInformationLinkURL
        End Get
        Set(ByVal value As String)
            _searchInformationLinkURL = value
        End Set
    End Property


    ''' <summary>
    ''' If the table name shall be displayed or not
    ''' </summary>
    ''' <remarks></remarks>
    Private _showTableName As Boolean = True
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowTableName() As Boolean
        Get
            Return _showTableName
        End Get
        Set(ByVal value As Boolean)
            _showTableName = value
        End Set
    End Property


    Private _showSelectAllAvailableValuesButton As Boolean = False
    ''' <summary>
    ''' If the "Show all available values" button shall be visible or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Warning! Setting this property to true may result in extremely heavy web pages if the variable contains many values (more than 10000)</remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property ShowAllAvailableValuesButton() As Boolean
        Get
            Return _showSelectAllAvailableValuesButton
        End Get
        Set(ByVal value As Boolean)
            _showSelectAllAvailableValuesButton = value
        End Set
    End Property


    ''' <summary>
    ''' Read only property for exposing the Client ID of the search button
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SearchButtonClientID() As String
        Get
            Return Control.SearchButtonClientID
        End Get
    End Property

    Public Sub InitiateSearch()
        Control.InitiateSearch()
    End Sub
End Class
