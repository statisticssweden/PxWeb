Imports System.Web.UI
Imports PCAxis.Paxiom
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes


Namespace CommandBar

    ''' <summary>
    ''' Control that displays a commandbar. 
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class CommandBar
        Inherits MarkerControlBase(Of CommandBarCodebehind, CommandBar)

        Private _viewMode As CommandBarViewMode

#Region " Events "

        ''' <summary>
        ''' Signal PX action to listeners
        ''' </summary>
        ''' <remarks></remarks>
        Public Event PxActionEvent As PxActionEventHandler
        Friend Sub OnPxActionEvent(ByVal e As PxActionEventArgs)
            RaiseEvent PxActionEvent(Me, e)
        End Sub

#End Region

        ''' <summary>
        ''' Gets or sets the view mode of an <see cref="CommandBar" />
        ''' The value is initialized from ControlSettings.xml
        ''' </summary>
        ''' <value>View mode of the <see cref="CommandBar" /></value>
        ''' <returns>A <see cref="CommandBarViewMode" /> value representing the view mode of the commandbar </returns>
        ''' <remarks></remarks>
        <PCAxis.Web.Core.Attributes.PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property ViewMode() As CommandBarViewMode
            Get
                Return _viewMode
            End Get
            Set(ByVal value As CommandBarViewMode)
                If Not Me.IsLoadingState AndAlso Not _viewMode = value Then
                    _viewMode = value
                    Control.LoadView()
                Else
                    _viewMode = value
                End If
            End Set
        End Property

        Private _operations As New List(Of String)
        ''' <summary>
        ''' The operations (functions) that will be displayed in the "Function" dropdown when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property Operations() As List(Of String)
            Get
                Return _operations
            End Get
            Set(ByVal value As List(Of String))
                _operations = value
            End Set
        End Property

        Private _outputFormats As New List(Of String)
        ''' <summary>
        ''' The fileformats that will be displayed in the "Save as" dropdown when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property OutputFormats() As List(Of String)
            Get
                Return _outputFormats
            End Get
            Set(ByVal value As List(Of String))
                _outputFormats = value
            End Set
        End Property

        Private _presentationViews As New List(Of String)
        ''' <summary>
        ''' The presentation views that will be displayed in the CommandBar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property PresentationViews() As List(Of String)
            Get
                Return _presentationViews
            End Get
            Set(ByVal value As List(Of String))
                _presentationViews = value
            End Set
        End Property

        Private _operationShortcuts As New List(Of String)
        ''' <summary>
        ''' The shortcuts that will be displayed under the "Function" dropdown when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property OperationShortcuts() As List(Of String)
            Get
                Return _operationShortcuts
            End Get
            Set(ByVal value As List(Of String))
                _operationShortcuts = value
            End Set
        End Property

        Private _fileformatShortcuts As New List(Of String)
        ''' <summary>
        ''' The shortcuts that will be displayed under the "Save as" dropdown when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property FileformatShortcuts() As List(Of String)
            Get
                Return _fileformatShortcuts
            End Get
            Set(ByVal value As List(Of String))
                _fileformatShortcuts = value
            End Set
        End Property

        Private _presentationViewShortcuts As New List(Of String)
        ''' <summary>
        ''' The shortcuts that will be displayed under the "Presentation views" dropdown when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property PresentationViewShortcuts() As List(Of String)
            Get
                Return _presentationViewShortcuts
            End Get
            Set(ByVal value As List(Of String))
                _presentationViewShortcuts = value
            End Set
        End Property

        Private _commandbarShortcuts As New List(Of String)
        ''' <summary>
        ''' The shortcuts that will be displayed to the right of the dropdowns or imagebuttons. 
        ''' Works when CommandBar is in ViewMode = DropDown OR ImageButtons
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property CommandbarShortcuts() As List(Of String)
            Get
                Return _commandbarShortcuts
            End Get
            Set(ByVal value As List(Of String))
                _commandbarShortcuts = value
            End Set
        End Property

        Private _operationButtons As New List(Of String)
        ''' <summary>
        ''' The operation (function) buttons that will be displayed when ComandBar is in ViewMode = ImageButtons
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property OperationButtons() As List(Of String)
            Get
                Return _operationButtons
            End Get
            Set(ByVal value As List(Of String))
                _operationButtons = value
            End Set
        End Property

        Private _filetypeButtons As New List(Of String)
        ''' <summary>
        ''' The filetype buttons that will be displayed when ComandBar is in ViewMode = ImageButtons
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property FiletypeButtons() As List(Of String)
            Get
                Return _filetypeButtons
            End Get
            Set(ByVal value As List(Of String))
                _filetypeButtons = value
            End Set
        End Property

        Private _presentationViewButtons As New List(Of String)
        ''' <summary>
        ''' The presentation view buttons that will be displayed when ComandBar is in ViewMode = ImageButtons
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property PresentationViewButtons() As List(Of String)
            Get
                Return _presentationViewButtons
            End Get
            Set(ByVal value As List(Of String))
                _presentationViewButtons = value
            End Set
        End Property

        'Private _showLinksDropdown As Boolean = True
        '''' <summary>
        '''' Gets or sets the third dropdown list shall be visible or not
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        '<PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        'Public Property ShowLinksDropdown() As Boolean
        '    Get
        '        Return _showLinksDropdown
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _showLinksDropdown = value

        '    End Set
        'End Property

        Private _commandBarFilter As Plugin.ICommandBarPluginFilter = Nothing
        ''' <summary>
        ''' Gets or sets the filter to be used for commandbar views and operations
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Public Property CommandBarFilter() As Plugin.ICommandBarPluginFilter
            Get
                Return _commandBarFilter
            End Get
            Set(ByVal value As Plugin.ICommandBarPluginFilter)
                _commandBarFilter = value
                'Control.LoadView()
            End Set
        End Property


        Private _selectedPresentationView As String
        ''' <summary>
        ''' The active presentation view
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SelectedPresentationView() As String
            Get
                Return _selectedPresentationView
            End Get
            Set(ByVal value As String)
                _selectedPresentationView = value
            End Set
        End Property


        ''' <summary>
        ''' Defines signature of the ScreenMethod method
        ''' </summary>
        ''' <param name="presentationView"></param>
        ''' <remarks></remarks>
        Public Delegate Sub ScreenMethod(ByVal presentationView As String, ByVal model As PXModel)

        Private _screenMethod As ScreenMethod
        ''' <summary>
        ''' Declares the Screen method to use
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScreenOutputMethod() As ScreenMethod
            Get
                If _screenMethod Is Nothing Then
                    _screenMethod = AddressOf DefaultScreenMethod
                End If
                Return _screenMethod
            End Get
            Set(ByVal value As ScreenMethod)
                _screenMethod = value
            End Set
        End Property



        ''' <summary>
        ''' Default implementation for output to screen
        ''' </summary>
        ''' <param name="presentationView"></param>
        ''' <remarks></remarks>
        Protected Sub DefaultScreenMethod(ByVal presentationView As String, ByVal model As PXModel)
            Dim pluginKey As String = presentationView
            Dim plugin As CommandBarPluginInfo = CommandBarPluginManager.Views(pluginKey)

            plugin.Invoke(model, Page.Controls)

            Context.ApplicationInstance.CompleteRequest()

        End Sub
    End Class
End Namespace
