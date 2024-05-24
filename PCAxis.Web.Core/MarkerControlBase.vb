Imports System.Globalization
Imports System.IO
Imports System.Web.Caching
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.Exceptions
Imports PCAxis.Web.Core.Interfaces

''' <summary>
''' Base class for all marker controls. Marker controls are used to store public properties and 
''' methods and enables user controls to be visible in a toolbox by representing them
''' </summary>
''' <typeparam name="TControl">The type of the usercontrol</typeparam>
''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
''' <remarks></remarks>
Public MustInherit Class MarkerControlBase(Of TControl As ControlBase(Of TControl, TMarker), TMarker As MarkerControlBase(Of TControl, TMarker))
    Inherits CompositeControl
    Implements IMarkerControl

    Private Const CACHE_KEY_VIRTUALPATHPROVIDERREGISTRED As String = "PCAxis.Web.Core.IsVirtualPathProviderRegistred"

    Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(MarkerControlBase(Of TControl, TMarker)))

    'Constants
    Private Const DESIGN_FILE_FORMAT As String = "{0}.ascx"

    'Private variables
    Private _isControlLoaded As Boolean

    ''' <summary>
    ''' Gets or sets whether the control is completely loaded
    ''' </summary>
    ''' <value>If <c>True</c> then the control is completely loaded, otherwise it's not</value>
    ''' <returns><c>True</c> if the control is completely loaded, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property IsControlLoaded() As Boolean
        Get
            Return Me._isControlLoaded
        End Get
    End Property

    'Private Properties

    ''' <summary>
    ''' Gets whether the controls state is being loaded or not
    ''' </summary>
    ''' <value>If <c>True</c> then state is being loaded, otherwise state is not being loaded</value>
    ''' <returns><c>True</c> is state is being loaded, otherwise <c>False</c></returns>
    ''' <remarks>Inherited controls should call this in properties that perform tasks when set</remarks>
    Protected ReadOnly Property IsLoadingState() As Boolean
        Get
            If IsControlLoaded Then
                Return Me.Control.IsLoadingState
            End If
            Return True
        End Get
    End Property

    Private _control As TControl

    ''' <summary>
    ''' Gets a reference to the usercontrol that the markercontrol represents
    ''' </summary>
    ''' <value>A strongly typed reference to the usercontrol that the markercontrol represents</value>
    ''' <returns>The instance of the usercontrol that the markercontrol represents</returns>    
    ''' <remarks></remarks>
    Protected Property Control() As TControl
        Get
            Return _control
        End Get
        Private Set(ByVal value As TControl)
            _control = value
        End Set
    End Property


    ''' <summary>
    ''' Gets a embedded resource the executing assembly
    ''' </summary>
    ''' <param name="resourceName">Name of the embedded resource</param>
    ''' <returns>A string representing the content</returns>
    ''' <remarks></remarks>
    Private Function GetResource(ByVal resourceName As String) As String
        Using reader As New StreamReader(Me.GetType().Assembly.GetManifestResourceStream _
                                        (Me.GetType(), resourceName))
            Return reader.ReadToEnd()
        End Using
    End Function

    ''' <summary>
    ''' Loads the embedded ascx file and displays it
    ''' </summary>    
    ''' <remarks></remarks>
    Private Sub LoadAscxFile()
        If Not _isControlLoaded Then
            Dim ascxControl As ControlBase(Of TControl, TMarker) = Nothing
            'If in visual studio
            If Me.DesignMode Or Me.Page Is Nothing Then

                'Load the content of the ascx file and display it in a LiteralControl
                Me.Controls.Add(New LiteralControl(GetResource(String.Format(CultureInfo.InvariantCulture, _
                                DESIGN_FILE_FORMAT, Me.GetType().Name))))

                Return

            Else

                'Checks to see if we already have registred a AssemblyResourceProvider, otherwise register it
                If Me.Page.Cache.Get(CACHE_KEY_VIRTUALPATHPROVIDERREGISTRED) Is Nothing Then
                    System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(New AssemblyResourceProvider())
                    Me.Page.Cache.Add(CACHE_KEY_VIRTUALPATHPROVIDERREGISTRED, _
                        "True", Nothing, Cache.NoAbsoluteExpiration, _
                        Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
                End If


                Try

                    'Load the ascxcontrol that this marker control represents
                    'using this class assembly and name
                    ascxControl = CType(Me.Page.LoadControl(String.Format( _
                        "~/PCAxis_Web_Controls/{0}.dll/{1}.ascx", _
                        Me.GetType().Assembly.GetName.Name, _
                        Me.GetType.FullName)), ControlBase(Of TControl, TMarker))

                    'Make the ascx control have the same AppRelativeTemplateSourceDirectory as the markercontrol
                    ascxControl.AppRelativeTemplateSourceDirectory = Me.AppRelativeTemplateSourceDirectory



                Catch ex As Exception
                    Logger.Error("Error loading ascx for " + Me.GetType.Name, ex)
                    Throw ex
                End Try


            End If


            'If there is no control loaded, throw an exception
            If (ascxControl Is Nothing) Then
                Throw _
                    New AscxLoadException("Could not load control from ascx")
            End If

            'Set the Control reference to the loaded ascx control
            Me.Control = CType(ascxControl, TControl)

            'Set the ascx controls Marker reference to this markercontrol
            ascxControl.Marker = CType(Me, TMarker)

            'make sure both controls have the same id
            Me.Control.ID = Me.ID

            'Add control to control collection to enable rendering
            Me.Controls.Add(ascxControl)

            _isControlLoaded = True

        End If

    End Sub

    ''' <summary>
    ''' Ensures an id is created for the marker
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.EnsureID()
    End Sub

    ''' <summary>
    ''' Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any 
    ''' child controls they contain in preparation for posting back or rendering.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub CreateChildControls()
        LoadAscxFile()
        MyBase.CreateChildControls()
    End Sub

    ''' <summary>
    ''' Recreates the child controls in the markercontrol
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub RecreateChildControls()
        Me.EnsureChildControls()
    End Sub

    ''' <summary>
    ''' Raises the Init event
    ''' </summary>
    ''' <param name="e">An EventArgs object that contains the event data. </param>
    ''' <remarks>Used to load the ascx control</remarks>
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        Me.EnsureChildControls()
        MyBase.OnInit(e)
    End Sub

    ''' <summary>
    ''' Gets the <see cref="HtmlTextWriterTag" /> value that corresponds to this Web server control
    ''' </summary>
    ''' <value>The <see cref="HtmlTextWriterTag" /> enumeration value to use to render this control</value>
    ''' <returns>One of the <see cref="HtmlTextWriterTag" /> enumeration values</returns>
    ''' <remarks></remarks>
    Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Div
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the programmatic identifier assigned to the server control.
    ''' </summary>
    ''' <value>A programmatic identifier assigned to the control to differate it from the other controls</value>
    ''' <returns>The programmatic identifier assigned to the control.</returns>
    ''' <remarks>Overridden to set the underlying ascx control to the same id</remarks>
    Public Overrides Property ID() As String
        Get
            Return MyBase.ID
        End Get
        Set(ByVal value As String)
            MyBase.ID = value
            If Control IsNot Nothing Then
                Control.ID = value
            End If
        End Set
    End Property
End Class
