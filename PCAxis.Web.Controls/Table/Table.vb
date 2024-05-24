Imports System.ComponentModel
Imports PCAxis.Paxiom
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes
Imports PCAxis.Web.Core.Enums



''' <summary>
''' Control that displays a table
''' </summary>
''' <remarks></remarks>
Partial Public Class Table
    Inherits MarkerControlBase(Of TableCodeBehind, Table)

    Private _useUpperCase As Boolean
    Private _layout As TableLayoutType = TableLayoutType.Layout1
    Private _dataNotePlacement As DataNotePlacementType = DataNotePlacementType.None
    Private _informationLevel As InformationLevelType = InformationLevelType.AllFootnotes
    Private _removeRowsOption As ZeroOptionType = ZeroOptionType.ShowAll
    'Private _decimalAdjust As DecimalAdjustType = DecimalAdjustType.RoundUpOnOddNumber
    Private _transformTable As TableTransformationType = TableTransformationType.NoTransformation
    Private _promptForMandatoryFootnotes As Boolean = False
    Private _timeOnTopIfSingle As Boolean = False
    Private _variablePresentationAlternative As Dictionary(Of String, HeaderPresentationType)
    'Private _thousandSeparator As String = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator
    'Private _decimalSeparator As String = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
    'Private _roundingRule As MidpointRounding = MidpointRounding.ToEven
    Private _displayCellInformation As Boolean = False
    Private _displayCellInformationWithoutJavascript As Boolean = False
    Private _displayDefaultAttributes As Boolean = False

#Region " Properties "

    ''' <summary>
    ''' Gets or sets the layout of the control
    ''' </summary>
    ''' <value>Layout of the <see cref="Table" /></value>
    ''' <returns>A <see cref="TableLayoutType" /> value representing the layout of the <see cref="Table" /></returns>
    ''' <remarks></remarks>
    <Category("Layout"), _
    PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property Layout() As TableLayoutType
        Get
            Return _layout
        End Get
        Set(ByVal value As TableLayoutType)
            If Not _layout = value Then
                If Me.TableTransformation = TableTransformationType.Sort AndAlso value = TableLayoutType.Layout1 Then

                Else
                    _layout = value
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether to used uppercase for the first letter in a <see cref="Paxiom.Value" />
    ''' </summary>
    ''' <value>If <c>True</c> the first letter in every <see cref="Paxiom.Value" /> in a 
    ''' <see cref="Paxiom.Variable" /> is uppercased, otherwise the first letter retains it's current case</value>
    ''' <returns><c>True</c> if uppercase is used, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property UseUpperCase() As Boolean
        Get
            Return _useUpperCase
        End Get
        Set(ByVal value As Boolean)
            _useUpperCase = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets where datanotes are to be placed in a cell
    ''' </summary>
    ''' <value>The placement of datanotes in the <see cref="Table" /></value>
    ''' <returns>A <see cref="DataNotePlacementType" /> value representing the datanote placement in the <see cref="Table" /></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DataNotePlacement() As DataNotePlacementType
        Get
            Return _dataNotePlacement
        End Get
        Set(ByVal value As DataNotePlacementType)
            _dataNotePlacement = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the <see cref="InformationLevelType" /> of the <see cref="Table" />
    ''' </summary>
    ''' <value>The level of information to show in the <see cref="Table" /></value>
    ''' <returns>A <see cref="InformationLevelType" /> value representing the level of information to show in the <see cref="Table" /></returns>
    ''' <remarks>
    ''' In the current version the informationlevel is used only for the internal dataformatter.
    ''' This allows the table to display footnotes in the cells of the table. However this feature
    ''' is not used, so setting this property does not result in any changes to the table.
    ''' </remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property InformationLevel() As Paxiom.InformationLevelType
        Get
            Return _informationLevel
        End Get
        Set(ByVal value As InformationLevelType)
            _informationLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the <see cref="ZeroOptionType" /> of the <see cref="Table" />
    ''' </summary>
    ''' <value>The types of rows to automatically remove from the table</value>
    ''' <returns>A <see cref="ZeroOptionType" /> value representing the types of rows to automatically remove from the <see cref="Table" /></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property RemoveRowsOption() As ZeroOptionType
        Get
            Return _removeRowsOption
        End Get
        Set(ByVal value As ZeroOptionType)
            _removeRowsOption = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the type of transformation the <see cref="Table" /> should use
    ''' </summary>
    ''' <value>The type of transformation the <see cref="Table" /> should use</value>
    ''' <returns>A <see cref="TableTransformationType" /> value representing the type of transformation for the table <see cref="Table" /></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property TableTransformation() As TableTransformationType
        Get
            Return _transformTable
        End Get
        Set(ByVal value As TableTransformationType)
            _transformTable = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets wheter to prompt for mandator footnotes
    ''' </summary>
    ''' <value>If <c>True</c> then a prompt is shown, otherwise nothing is show</value>
    ''' <returns><c>True</c> if a prompt should be shown, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property PromptForMandatoryFootnotes() As Boolean
        Get
            Return _promptForMandatoryFootnotes
        End Get
        Set(ByVal value As Boolean)
            _promptForMandatoryFootnotes = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether to put the time variable in the header if it is the only available variable
    ''' </summary>
    ''' <value>If <c>True</c> and time variable is the only available variable, it is moved to the header otherwise nothing is changed</value>
    ''' <returns><c>True</c> if the time will be placed on top if it is the only available variable, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property TimeOnTopIfSingle() As Boolean
        Get
            Return _timeOnTopIfSingle
        End Get
        Set(ByVal value As Boolean)
            _timeOnTopIfSingle = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the text/code representation of variable in table header
    ''' </summary>
    ''' <value>List of Variable.Code and HeaderPresentationType</value>
    ''' <returns>List of Variable.Code and HeaderPresentationType if set, else empty list</returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property VariablePresentationAlternative() As Dictionary(Of String, HeaderPresentationType)
        Get
            Return _variablePresentationAlternative
        End Get
        Set(ByVal value As Dictionary(Of String, HeaderPresentationType))
            _variablePresentationAlternative = value
        End Set
    End Property


    ''' <summary>
    ''' Max columns to show, if not set, the default value is 1000
    ''' </summary>
    ''' <remarks></remarks>
    Private _maxColumns As Integer = 1000
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MaxColumns() As Integer
        Get
            Return _maxColumns
        End Get
        Set(ByVal value As Integer)
            _maxColumns = value
        End Set
    End Property

    ''' <summary>
    ''' Max rows to show, if not set, the default value is 1000
    ''' </summary>
    ''' <remarks></remarks>
    Private _maxRows As Integer = 1000
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property MaxRows() As Integer
        Get
            Return _maxRows
        End Get
        Set(ByVal value As Integer)
            _maxRows = value
        End Set
    End Property

    ''' <summary>
    ''' Show or hide table title
    ''' </summary>
    ''' <remarks></remarks>
    Private _titleVisible As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property TitleVisible() As Boolean
        Get
            Return _titleVisible
        End Get
        Set(ByVal value As Boolean)
            _titleVisible = value
        End Set
    End Property

    ''' <summary>
    ''' Show or hide table title
    ''' </summary>
    ''' <remarks></remarks>
    Private _newTitleLayout As Boolean
    <PropertyPersistState(PersistStateType.PerControlAndPage)>
    Public Property NewTitleLayout() As Boolean
        Get
            Return _newTitleLayout
        End Get
        Set(ByVal value As Boolean)
            _newTitleLayout = value
        End Set
    End Property

    ''' <summary>
    ''' If cell information shall be displayed in the table or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DisplayCellInformation() As Boolean
        Get
            Return _displayCellInformation
        End Get
        Set(ByVal value As Boolean)
            _displayCellInformation = value
        End Set
    End Property

    ''' <summary>
    ''' If cell information shall be displayed in the table or not when javascript is disabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DisplayCellInformationWithoutJavascript() As Boolean
        Get
            Return _displayCellInformationWithoutJavascript
        End Get
        Set(ByVal value As Boolean)
            _displayCellInformationWithoutJavascript = value
        End Set
    End Property

    ''' <summary>
    ''' If default attributes shall be displayed beneath the table or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(PersistStateType.PerControlAndPage)> _
    Public Property DisplayDefaultAttributes() As Boolean
        Get
            Return _displayDefaultAttributes
        End Get
        Set(ByVal value As Boolean)
            _displayDefaultAttributes = value
        End Set
    End Property
#End Region

#Region "Events"
    ''' <summary>
    ''' Signal PX action to listeners
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PxTableCroppedEvent As Eventhandler
    Friend Sub OnPxTableCroppedEvent(ByVal e As EventArgs)
        RaiseEvent PxTableCroppedEvent(Me, e)
    End Sub

#End Region

    Public Sub New()
        _variablePresentationAlternative = New Dictionary(Of String, HeaderPresentationType)
    End Sub
End Class
