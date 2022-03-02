Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.ComponentModel.Design
Imports System.Web.UI.Design
Imports System.Web.Caching
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Core.Exceptions
Imports PCAxis.Web.Core.Configuration
Imports PCAxis.Web.Core.Attributes
Imports System.Web.UI.WebControls
Imports PCAxis.Web.Core.StateProvider
Imports System.Web.UI
Imports System.Web
Imports System.Reflection
Imports PCAxis.Web.Core.Enums
Imports PCAxis.Web.Core.Management.LinkManager
Imports PCAxis.Web.Core.Interfaces

''' <summary>
''' Base class for all usercontrols. Contains methods and properties that are common for all usercontrols
''' </summary>
''' <typeparam name="TControl">The type of the usercontrol</typeparam>
''' <typeparam name="TMarker">The type of the markercontrol</typeparam>
''' <remarks></remarks>
Public MustInherit Class ControlBase(Of TControl As ControlBase(Of TControl, TMarker), TMarker As MarkerControlBase(Of TControl, TMarker))
    Inherits UserControl
    Implements ILanguageControl
    Private Const CACHE_KEY_VIRTUALPATHPROVIDERREGISTRED As String = "PCAxis.Web.Core.IsVirtualPathProviderRegistred"

    Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(ControlBase(Of TControl, TMarker)))

    'Private variables
    Private _currentLanguage As String
    Private _currentCulture As CultureInfo
    Private _persister As StaterProviderBase = StateProviderFactory.GetStateProvider()
    Private _loadingState As Boolean = False

    'Private Events
    Protected Event LanguageChanged As EventHandler


    'Private Properties
    ''' <summary>
    ''' Gets a list of all the properties that should be persisted
    ''' </summary>
    ''' <value>Returns a list of <see cref="PersistentProperty" /> describing the properties that should be persisted</value>
    ''' <returns>An instance of <see cref="List(Of PersistentProperty)"/></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property PersistentProperties() As List(Of PersistentProperty)
        Get
            CheckPersistentProperties()
            Return CType(Cache.Get("PersistentProperties" + Me.Marker.GetType.Name.ToString()), Global.System.Collections.Generic.List(Of Global.PCAxis.Web.Core.ControlBase(Of TControl, TMarker).PersistentProperty))
        End Get
    End Property

    ''' <summary>
    ''' Gets whether the controls state is being loaded or not
    ''' </summary>
    ''' <value>If <c>True</c> then state is being loaded, otherwise state is not being loaded</value>
    ''' <returns><c>True</c> is state is being loaded, otherwise <c>False</c></returns>
    ''' <remarks>Inherited controls should call this in properties that perform tasks when set</remarks>
    Protected Friend ReadOnly Property IsLoadingState() As Boolean
        Get
            Return Me._loadingState
        End Get
    End Property


    Private _marker As TMarker
    ''' <summary>
    ''' Gets a reference to the markercontrol that represents the usercontrol
    ''' </summary>
    ''' <value>A strongly typed reference to the markercontrol that represents the usercontrol</value>
    ''' <returns>The instance of the markercontrol that represent the usercontrol</returns>
    ''' <remarks></remarks>
    Public Property Marker() As TMarker
        Get
            Return _marker
        End Get
        Friend Set(ByVal value As TMarker)
            _marker = value
        End Set
    End Property


    ''' <summary>
    ''' Adds the the control to the static languagecontrols list
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        LocalizationManager.LanguageControls.Add(Me)
        _currentCulture = LocalizationManager.CurrentCulture
    End Sub

    ''' <summary>
    ''' Gets or sets the current language
    ''' </summary>
    ''' <value>String representing the current language</value>
    ''' <returns>String representing the current language</returns>
    ''' <remarks></remarks>
    <Browsable(True),
    Category("Localization")>
    Public Property CurrentCulture() As CultureInfo Implements ILanguageControl.CurrentCulture
        Get
            Return _currentCulture
        End Get
        Set(ByVal value As CultureInfo)
            _currentCulture = value
            If value IsNot Nothing Then
                If Not Me.IsLoadingState = True Then
                    OnLanguageChanged(New EventArgs())
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets a localized string for the supplied key using the controls current culture
    ''' </summary>
    ''' <param name="key">The key to use to retrieve the string</param>
    ''' <returns>A localized string</returns>
    ''' <remarks><seealso cref="LocalizationManager.GetLocalizedString" /></remarks>
    Public Function GetLocalizedString(ByVal key As String) As String
        If Me.DesignMode Then
            Return key
        End If
        Return LocalizationManager.GetLocalizedString(key, _currentCulture)
    End Function

    ''' <summary>
    ''' Raises the LanguageChanged event
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub OnLanguageChanged(ByVal args As EventArgs)
        RaiseEvent LanguageChanged(Me, args)
    End Sub

    ''' <summary>
    ''' Registers the control for controlstate if there are persistent properties
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        If PersistentProperties.Count > 0 Then
            Me.Page.RegisterRequiresControlState(Me)
        End If
        MyBase.OnInit(e)
    End Sub

    ''' <summary>
    ''' Checks if the control has properties that needs to be persisted and caches the result
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckPersistentProperties()
        'If PersistentProperties is nothing then we haven't checked for properties 
        'that should be persisted
        If Cache.Get("PersistentProperties" + Me.Marker.GetType.Name.ToString()) Is Nothing Then
            'Create a new empty list of PersistentProperty in the cache for this type
            SyncLock Me.GetType()
                If Cache.Get("PersistentProperties" + Me.Marker.GetType.Name.ToString()) Is Nothing Then

                    Cache.Add("PersistentProperties" + Me.Marker.GetType.Name.ToString(),
                      New List(Of PersistentProperty), Nothing, Cache.NoAbsoluteExpiration,
                      Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)

                    'Check any public or protected properties in the control for the PropertyPersistStateAttribute            
                    For Each prop As PropertyInfo In Me.GetType.GetProperties(BindingFlags.Instance Or
                                                                        BindingFlags.Public Or BindingFlags.NonPublic)

                        Dim attr() As PropertyPersistStateAttribute =
                        CType(prop.GetCustomAttributes(GetType(PropertyPersistStateAttribute), True),
                        PropertyPersistStateAttribute())

                        If attr.GetLength(0) > 0 Then

                            'Add new instance PersistentProperty if the property has the attribute
                            PersistentProperties.Add(New ControlBase(Of TControl, TMarker).PersistentProperty() _
                            With {.PropertyLocation = ControlBase(Of TControl, TMarker).PropertyLocationType.Control,
                                  .PropertyPersistStateType = attr(0).PersistStateType,
                                  .PropertyName = prop.Name})
                        End If
                    Next

                    'Check any public or protected properties in the markercontrol for the PropertyPersistStateAttribute            
                    For Each prop As PropertyInfo In Me.Marker.GetType.GetProperties(BindingFlags.Instance Or
                                                                     BindingFlags.Public Or BindingFlags.NonPublic)

                        Dim attr() As PropertyPersistStateAttribute =
                        CType(prop.GetCustomAttributes(GetType(PropertyPersistStateAttribute), True),
                        PropertyPersistStateAttribute())
                        If attr.GetLength(0) > 0 Then

                            'Add new instance PersistentProperty if the property has the attribute
                            Dim pp = New ControlBase(Of TControl, TMarker).PersistentProperty() _
                            With {.PropertyLocation = ControlBase(Of TControl, TMarker).PropertyLocationType.Marker,
                                  .PropertyPersistStateType = attr(0).PersistStateType,
                                  .PropertyName = prop.Name}

                            If pp IsNot Nothing Then
                                PersistentProperties.Add(pp)
                            End If
                        End If
                    Next
                End If
            End SyncLock
        End If
    End Sub

    ''' <summary>
    ''' Save the state for the properties that require it
    ''' </summary>
    ''' <returns>A <see cref="List(Of Object)" /> containing the state that should be saved in control state</returns>
    ''' <remarks>Only called if the control has properties that should be persisted</remarks>
    Protected Overrides Function SaveControlState() As Object
        Dim controlState As New List(Of Object)
        Dim row As Integer = 0

        Try


            'For every persistProperty in the PersistentProperty list
            For Each persistProperty As PersistentProperty In Me.PersistentProperties
                Dim prop As PropertyInfo = Nothing
                Dim control As Control = Nothing

                row = 1
                'Check whether the property exists on the control or the markercontrol
                'and set control and prop to the correct values otherwise an exception is thrown            
                Select Case persistProperty.PropertyLocation
                    Case ControlBase(Of TControl, TMarker).PropertyLocationType.Control
                        row = 2
                        prop = Me.GetType.GetProperty(persistProperty.PropertyName, BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Instance)
                        control = Me
                    Case ControlBase(Of TControl, TMarker).PropertyLocationType.Marker
                        row = 3
                        prop = Me.Marker.GetType.GetProperty(persistProperty.PropertyName, BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Instance)
                        row = 4
                        control = Me.Marker
                End Select

                'Saved the state to the correct location            
                Select Case persistProperty.PropertyPersistStateType
                    Case PersistStateType.PerControlAndPage
                        row = 5
                        controlState.Add(prop.GetValue(control, Nothing))
                    Case PersistStateType.PerControlAndRequest
                        row = 6
                        Me._persister.Add(Me.GetType, Me.ID + "." + prop.Name, prop.GetValue(control, Nothing))
                    Case PersistStateType.PerRequest
                        row = 7
                        Me._persister.Add(Me.GetType, prop.Name, prop.GetValue(control, Nothing))
                End Select
            Next


            row = 8
            'Get control state from the base class
            controlState.Add(MyBase.SaveControlState)


        Catch ex As Exception
            Logger.Error(Me.GetType().FullName & " @ row: " & row)
            Throw
        End Try

        Return controlState
    End Function

    ''' <summary>
    ''' Load the state for the properties that require it
    ''' </summary>
    ''' <param name="savedState">The control state to load</param>
    ''' <remarks>Only called if the control has properties that should be persisted</remarks>
    Protected Overrides Sub LoadControlState(ByVal savedState As Object)

        'Set loadingstate to true so properties know if the should react if state is set
        Me._loadingState = True
        Dim controlState As List(Of Object) = CType(savedState, List(Of Object))
        Dim i As Integer = 0

        'For every persistProperty in the PersistentProperty list
        For Each persistProperty As PersistentProperty In Me.PersistentProperties
            Dim prop As PropertyInfo = Nothing
            Dim control As Control = Nothing


            'Check whether the property exists on the control or the markercontrol
            'and set control and prop to the correct values otherwise an exception is thrown            
            Select Case persistProperty.PropertyLocation
                Case ControlBase(Of TControl, TMarker).PropertyLocationType.Control
                    prop = Me.GetType.GetProperty(persistProperty.PropertyName, BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Instance)
                    control = Me
                Case ControlBase(Of TControl, TMarker).PropertyLocationType.Marker
                    prop = Me.Marker.GetType.GetProperty(persistProperty.PropertyName, BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Instance)
                    control = Me.Marker
            End Select

            'Load the state from the correct location            
            Select Case persistProperty.PropertyPersistStateType
                Case PersistStateType.PerControlAndPage
                    prop.SetValue(control, controlState(i), Nothing)
                    i += 1
                Case PersistStateType.PerControlAndRequest
                    prop.SetValue(control, Me._persister.Item(Me.GetType, Me.ID + "." + prop.Name), Nothing)
                Case PersistStateType.PerRequest
                    prop.SetValue(control, Me._persister.Item(Me.GetType, prop.Name), Nothing)
            End Select
        Next

        'Load the control state from the base class
        MyBase.LoadControlState(controlState(controlState.Count - 1))

        Me._loadingState = False
    End Sub


    ''' <summary>
    ''' Replaces # with <br />
    ''' </summary>
    ''' <param name="value"><see cref="String" /> to format</param>
    ''' <returns>A <see cref="String" /> with # replaced with <br /></returns>
    Protected Function FormatString(ByVal value As String) As String
        If Not String.IsNullOrEmpty(value) Then
            value = "<div>" + value + "</div>"
            value = value.Replace("#", "</div><div>")
            'Add an regexp for to find epost@dsdsd.no and adds a mailto
            Dim theRegex As New System.Text.RegularExpressions.Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            Dim matches As System.Text.RegularExpressions.MatchCollection = theRegex.Matches(value)
            Dim email As String
            For Each match As System.Text.RegularExpressions.Match In matches
                email = "<a class=""envelope-icon"" href=""mailto:" + match.Value + """>" + match.Value + "</a>"
                value = value.Replace(match.Value, email)
            Next
        End If
        Return value
    End Function


    ''' <summary>
    ''' Used to enable cachning of the properties that should be persisted
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PersistentProperty

        Private _propertyPersistStateType As Enums.PersistStateType
        ''' <summary>
        ''' Gets or sets how the property is to be persisted
        ''' </summary>
        ''' <value>How the property is to be persisted</value>
        ''' <returns>A <see cref="Enums.PersistStateType" /> value representing the way the property is to persisted</returns>
        ''' <remarks></remarks>
        Public Property PropertyPersistStateType() As Enums.PersistStateType
            Get
                Return _propertyPersistStateType
            End Get
            Set(ByVal value As Enums.PersistStateType)
                _propertyPersistStateType = value
            End Set
        End Property

        Private _propertyLocation As PropertyLocationType
        ''' <summary>
        ''' Gets or sets where the property is located
        ''' </summary>
        ''' <value>Where the property is located</value>
        ''' <returns>A <see cref="PropertyLocationType" /> value representing where the property is located</returns>
        ''' <remarks></remarks>
        Public Property PropertyLocation() As PropertyLocationType
            Get
                Return _propertyLocation
            End Get
            Set(ByVal value As PropertyLocationType)
                _propertyLocation = value
            End Set
        End Property


        Private _propertyName As String
        ''' <summary>
        ''' Gets or sets the name of the property
        ''' </summary>
        ''' <value>Name of the property</value>
        ''' <returns>The name of the property</returns>
        ''' <remarks></remarks>
        Public Property PropertyName() As String
            Get
                Return _propertyName
            End Get
            Set(ByVal value As String)
                _propertyName = value
            End Set
        End Property



    End Class


    ''' <summary>  
    ''' Specifies where a property is located
    ''' </summary>        
    ''' <remarks></remarks>
    Private Enum PropertyLocationType
        ''' <summary>
        ''' The property exists on the markercontrol
        ''' </summary>
        ''' <remarks></remarks>
        Marker
        ''' <summary>
        ''' The property exists on the control
        ''' </summary>
        ''' <remarks></remarks>
        Control
    End Enum


End Class
