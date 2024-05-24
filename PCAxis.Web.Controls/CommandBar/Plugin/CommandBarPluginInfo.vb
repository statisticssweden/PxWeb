Namespace CommandBar.Plugin

    ''' <summary>
    ''' Represents the information for ony plugin entry in the commandbar settingsfile
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommandBarPluginInfo

        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(CommandBarPluginInfo))

        Private _name As String
        ''' <summary>
        ''' Gets or sets the name of the plugin
        ''' </summary>
        ''' <value>Name of the plugin</value>
        ''' <returns>The name of the plugin</returns>
        ''' <remarks></remarks>
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _languageCode As String
        ''' <summary>
        ''' Gets or sets the languagecode for the plugin
        ''' </summary>
        ''' <value>Languagecode for the plugin</value>
        ''' <returns>The languagecode for the plugin</returns>
        ''' <remarks></remarks>
        Public Property NameCode() As String
            Get
                Return _languageCode
            End Get
            Set(ByVal value As String)
                _languageCode = value
            End Set
        End Property


        Private _hasUI As Boolean
        ''' <summary>
        ''' Gets or sets  whether the plugin has a UI
        ''' </summary>
        ''' <value>If <c>True</c> then the plugin has UI, otherwise it is just function</value>
        ''' <returns><c>True</c> if the plugin has a UI, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Public Property HasUI() As Boolean
            Get
                Return _hasUI
            End Get
            Set(ByVal value As Boolean)
                _hasUI = value
            End Set
        End Property

        Private _hideOnMobile As Boolean
        ''' <summary>
        ''' Gets or sets  whether the plugin has a UI
        ''' </summary>
        ''' <value>If <c>True</c> then the plugin has UI, otherwise it is just function</value>
        ''' <returns><c>True</c> if the plugin has a UI, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Public Property HideOnMobile() As Boolean
            Get
                Return _hideOnMobile
            End Get
            Set(ByVal value As Boolean)
                _hideOnMobile = value
            End Set
        End Property

        Private _category As String
        ''' <summary>
        ''' Gets or sets the category for the plugin
        ''' </summary>
        ''' <value>Category for the plugin</value>
        ''' <returns>The category for the plugin</returns>
        ''' <remarks></remarks>
        Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property

        Private _properties As New Dictionary(Of String, String)
        ''' <summary>
        ''' Gets the properties for the plugin
        ''' </summary>
        ''' <value>Properties for the plugin</value>
        ''' <returns>The properties for the plugin</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Properties() As Dictionary(Of String, String)
            Get
                Return _properties
            End Get
        End Property

        Private _constraints As New Dictionary(Of String, String)
        ''' <summary>
        ''' Gets the properties for the plugin
        ''' </summary>
        ''' <value>Properties for the plugin</value>
        ''' <returns>The properties for the plugin</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Constraints() As Dictionary(Of String, String)
            Get
                Return _constraints
            End Get
        End Property



        'Private _visible As Boolean
        '''' <summary>
        '''' Gets or sets whether the plugin is visible
        '''' </summary>
        '''' <value>If <c>True</c> then the plugin should be displayed, otherwise it should not be displayed unless specifically called</value>
        '''' <returns><c>True</c> if the plugin is visible, otherwise <c>False</c></returns>
        '''' <remarks></remarks>
        'Public Property Visible() As Boolean
        '    Get
        '        Return _visible
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _visible = value
        '    End Set
        'End Property

        Private _type As Type
        ''' <summary>
        ''' Gets the <see cref="System.Type" /> of the plugin
        ''' </summary>
        ''' <value><see cref="System.Type" /> of the plugin</value>
        ''' <returns>The <see cref="System.Type" /> of the plugin</returns>
        ''' <remarks></remarks>
        Private ReadOnly Property Type() As Type
            Get
                Return _type
            End Get
        End Property

        Private _image As String
        ''' <summary>
        ''' The image that will be shown for the plugin when CommandBar is in ViewMode = ImageButtons
        ''' </summary>
        ''' <value>Name of the image</value>
        ''' <returns>The name of the image</returns>
        ''' <remarks></remarks>
        Public Property Image() As String
            Get
                Return _image
            End Get
            Set(ByVal value As String)
                _image = value
            End Set
        End Property

        Private _shortcutImage As String
        ''' <summary>
        ''' The image that will be shown for the plugin as a shortcut when CommandBar is in ViewMode = DropDown
        ''' </summary>
        ''' <value>The name of the image</value>
        ''' <returns>The name of the image</returns>
        ''' <remarks></remarks>
        Public Property ShortcutImage() As String
            Get
                Return _shortcutImage
            End Get
            Set(ByVal value As String)
                _shortcutImage = value
            End Set
        End Property
        Private _sortOrder As Integer
        ''' <summary>
        ''' The sort order for the plugin in lists
        ''' </summary>
        ''' <value>Sort order</value>
        ''' <returns>The sort order</returns>
        ''' <remarks></remarks>
        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal value As Integer)
                _sortOrder = value
            End Set
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandBarPluginInfo"  />
        ''' </summary>
        ''' <param name="type">The <see cref="System.Type" /> of the plugin</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal type As String)
            Me._type = System.Type.GetType(type)
        End Sub

        ''' <summary>
        ''' Creates a new instance of the plugin
        ''' </summary>
        ''' <returns>If the plugin has a UI it returns an instance of the plugin, otherwise <c>null</c></returns>
        ''' <remarks>An instance of the plugin or <c>null</c></remarks>
        Public Function GetInstance() As ICommandBarGUIPlugin
            Dim plugin As ICommandBarGUIPlugin = Nothing
            If HasUI Then
                Try
                    'Try to creata an instance of the plugin and cast it to ICommandBarGUIPlugin
                    plugin = CType(Activator.CreateInstance(Me.Type), ICommandBarGUIPlugin)

                    'Initialize the plugins properties
                    plugin.InitializeProperties(Me.Properties)
                Catch e As Exception
                    Logger.Error("Error creating an instance of the plugin " + Me.Name, e)
                End Try
            End If

            Return plugin
        End Function

        ''' <summary>
        ''' Invokes the plugin a a function
        ''' </summary>
        ''' <param name="model">The <see cref="Paxiom.PXModel"/> to work on</param>
        ''' <param name="controls">The <see cref="System.Web.UI.Page.Controls" /> to work with</param>
        ''' <returns>An transformed <see cref="Paxiom.PXModel"/></returns>
        ''' <remarks>Only works if the plugin doesn't have a UI, otherwise does nothing and only returns the unaltered <see cref="Paxiom.PXModel"/>.
        ''' <paramref name="model" /> is not always used, depends on the plugin
        ''' <paramref name="controls" /> is not always used, depends on the plugin</remarks>
        Public Function Invoke(ByVal model As Paxiom.PXModel, ByVal controls As System.Web.UI.ControlCollection) As Paxiom.PXModel
            'Check if we have a UI
            If Not Me.HasUI Then
                'Create an instance of the pluign
                Dim noGUIInstance As Object = Activator.CreateInstance(Me.Type)

                'Make sure it implemnets the ICommandBarNoGUIPlugin interface
                If TypeOf (noGUIInstance) Is ICommandBarNoGUIPlugin Then

                    'Check if it requires a paxiommodel
                    If TypeOf (noGUIInstance) Is ICommandBarRequiresPaxiom Then
                        CType(noGUIInstance, ICommandBarRequiresPaxiom).PaxiomModel = model
                    End If

                    'Check if it requires the control on the current page
                    If TypeOf (noGUIInstance) Is ICommandBarRequireControls Then
                        CType(noGUIInstance, ICommandBarRequireControls).PageControls = controls
                    End If

                    'Execute the plugin with the plugins properties
                    CType(noGUIInstance, ICommandBarNoGUIPlugin).Execute(Me.Properties)

                    'If the plugin used the paxiom model, retrieve the now transformed model
                    If TypeOf (noGUIInstance) Is ICommandBarRequiresPaxiom Then
                        Return CType(noGUIInstance, ICommandBarRequiresPaxiom).PaxiomModel
                    End If

                    'Return the transformed model
                    Return model
                Else
                    'return the unaltered paxiom model
                    Return model
                End If
            End If
            Return model
        End Function

    End Class
End Namespace
