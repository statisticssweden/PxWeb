Imports System.Configuration
Imports PCAxis.Web.Controls.Configuration.Sections

Namespace Configuration
    ''' <summary>
    ''' Helperclass for easy access to predefined configurationsections in web.config
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConfigurationHelper
        Const CONFIG_GENERALSETTINGS As String = "pcaxis/web.controls/generalsettings"
        Const CONFIG_PLUGINS As String = "pcaxis/web.controls/plugins"
        Public Const CONFIG_VIEW_TABLE As String = "pcaxis/web.controls/views/table"
        Public Const CONFIG_VIEW_SORTEDTABLE As String = "pcaxis/web.controls/views/sortedtable"
        Public Const CONFIG_VIEW_CHART As String = "pcaxis/web.controls/views/chart"
        Public Const CONFIG_VIEW_FOOTNOTE As String = "pcaxis/web.controls/views/footnote"
        Public Const CONFIG_VIEW_INFORMATION As String = "pcaxis/web.controls/views/information"
        Public Const CONFIG_PXPAGES As String = "pcaxis/web.controls/pxpage"

        Private Sub New()

        End Sub

        ''' <summary>
        ''' Returns the GeneralSettingsSection of the configuration web.config
        ''' </summary>
        ''' <value></value>
        ''' <returns>GeneralSettingsSection</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property GeneralSettingsSection() As GeneralSettingsSection
            Get
                Return CType(ConfigurationManager.GetSection(CONFIG_GENERALSETTINGS), GeneralSettingsSection)
            End Get
        End Property

        ''' <summary>
        ''' Returns the CommandBarSection of the configuration web.config
        ''' </summary>
        ''' <value></value>
        ''' <returns>CommandBarSection</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Plugins() As CommandBarSection
            Get
                Return CType(ConfigurationManager.GetSection(CONFIG_PLUGINS), CommandBarSection)
            End Get
        End Property

        ''' <summary>
        ''' Get page for the given presentation view
        ''' </summary>
        ''' <param name="strView">wanted presentation view</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetViewPage(ByVal strView As String) As String
            Dim view As ViewSection

            view = CType(ConfigurationManager.GetSection(strView), ViewSection)

            If view Is Nothing Then
                Return ""
            End If

            'Check if there is there a special page for the selected language
            For Each page As PageElement In view.Pages
                If page.Language.StartsWith(PCAxis.Web.Core.Management.LocalizationManager.CurrentCulture.TwoLetterISOLanguageName) Then
                    Return page.Url
                End If
            Next

            Return view.DefaultPage
        End Function

        ''' <summary>
        ''' Get page for the specified PX-page
        ''' </summary>
        ''' <param name="strId">Id of the PX-page</param>
        ''' <returns>Page to the specified PX-page</returns>
        ''' <remarks></remarks>
        Public Shared Function GetPxPage(ByVal strId As String) As String
            Dim pxPageSection As PxPageSection

            pxPageSection = CType(ConfigurationManager.GetSection(CONFIG_PXPAGES), PxPageSection)

            If pxPageSection Is Nothing Then
                Return ""
            End If

            For Each pxpage As PxPageElement In pxPageSection.PxPages
                If pxpage.Id.Equals(strId) Then

                    'Check if there is there a special page for the selected language
                    For Each page As PageElement In pxpage.Pages
                        If page.Language.StartsWith(PCAxis.Web.Core.Management.LocalizationManager.CurrentCulture.TwoLetterISOLanguageName) Then
                            Return page.Url
                        End If
                    Next

                    Return pxpage.DefaultPage
                End If
            Next

            Return ""
        End Function

    End Class


    ''' <summary>
    ''' A collection of <see cref="PageElement" />
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PageElementCollection
        Inherits ConfigurationElementCollection

        ''' <summary>
        ''' Returns the collectiontype of this collection
        ''' </summary>
        ''' <value><see cref="ConfigurationElementCollectionType.AddRemoveClearMap" /></value>
        ''' <returns>A instance of <see cref="ConfigurationElementCollectionType.AddRemoveClearMap" /></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.AddRemoveClearMap
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of <see cref="PageElement" />
        ''' </summary>
        ''' <returns>A instance of <see cref="PageElement" /></returns>
        ''' <remarks></remarks>
        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New PageElement
        End Function

        ''' <summary>
        ''' Gets the unique key for a element in the collection
        ''' </summary>
        ''' <param name="element">The <see cref="PageElement" /> to return the key for</param>
        ''' <returns>An <see cref="Object" /> representing the language of the <see cref="PageElement" /></returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, PageElement).Language
        End Function

        ''' <summary>
        ''' Gets the <see cref="PageElement" /> associated with the given name 
        ''' </summary>
        ''' <param name="Name">The name of the <see cref="PageElement" /> to get</param>
        ''' <value>The <see cref="PageElement" /> associated with the name</value>
        ''' <returns>An instance of <see cref="PageElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal Name As String) As PageElement
            Get
                Return CType(BaseGet(Name), PageElement)
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="PageElement" /> at the specified index
        ''' </summary>
        ''' <param name="index">The index of the <see cref="PageElement" /> to get</param>
        ''' <value>The <see cref="PageElement" /> at the specified index</value>
        ''' <returns>An instance of <see cref="PageElement" /></returns>        
        ''' <remarks></remarks>
        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As PageElement
            Get
                Return CType(BaseGet(index), PageElement)
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Represents one PX-Web page in the config file
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PageElement
        Inherits ConfigurationElement

        Const CONFIG_LANGUAGE As String = "language"
        Const CONFIG_URL As String = "url"

        ''' <summary>
        ''' Gets or sets the unique language for the page
        ''' </summary>
        ''' <value>The unique language for the page</value>
        ''' <returns>A <see cref="String" /> representing the unique language of the page</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_LANGUAGE, IsRequired:=True)> _
      Public Property Language() As String
            Get
                Return Me(CONFIG_LANGUAGE).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_LANGUAGE) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the URL for the page
        ''' </summary>
        ''' <value>The URL for the page</value>
        ''' <returns>A <see cref="String" /> representing the URL of the page</returns>
        ''' <remarks></remarks>
        <ConfigurationProperty(CONFIG_URL, IsRequired:=True)> _
      Public Property Url() As String
            Get
                Return Me(CONFIG_URL).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_URL) = value
            End Set
        End Property

    End Class
End Namespace
