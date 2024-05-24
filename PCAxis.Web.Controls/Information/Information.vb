Imports PCAxis.Enums
Imports PCAxis.Web.Core
Imports PCAxis.Web.Core.Attributes



''' <summary>
''' </summary>
''' <remarks></remarks>
Partial Public Class Information
    Inherits MarkerControlBase(Of InformationCodebehind, Information)

    Private _informationTypes As List(Of InformationType) = Nothing
    Private _contactForEveryContent As Boolean = True
    Private _lastUpdatedForEveryContent As Boolean = True

    ''' <summary>
    ''' Gets or sets a list of the <see cref="PCAxis.Enums.InformationType" /> that should be visible
    ''' </summary>
    ''' <value>All the <see cref="PCAxis.Enums.InformationType" /> that are visible</value>
    ''' <returns>A list of <see cref="PCAxis.Enums.InformationType" /></returns>
    ''' <remarks></remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ShowInformationTypes() As List(Of InformationType)
        Set(ByVal value As List(Of InformationType))
            _informationTypes = value
            If Not Me.IsLoadingState Then
                Control.GetInformation()
            End If
        End Set
        Get
            Return _informationTypes
        End Get
    End Property

    ''' <summary>
    ''' If contact information shall be displayed for each content when there are more than one content or not.
    ''' If set to false the contact information from the first content will be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This setting must be set before any call to ShowInformationTypes to have any effect</remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property ContactForEveryContent() As Boolean
        Get
            Return _contactForEveryContent
        End Get
        Set(ByVal value As Boolean)
            _contactForEveryContent = value
        End Set
    End Property

    ''' <summary>
    ''' If last updated information shall be displayed for each content when there are more than one content or not.
    ''' If set to false the last updated information from the first content will be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This setting must be set before any call to ShowInformationTypes to have any effect</remarks>
    <PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
    Public Property LastUpdatedForEveryContent() As Boolean
        Get
            Return _lastUpdatedForEveryContent
        End Get
        Set(ByVal value As Boolean)
            _lastUpdatedForEveryContent = value
        End Set
    End Property

End Class
