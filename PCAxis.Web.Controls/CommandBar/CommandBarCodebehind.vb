Imports PCAxis.Web.Core
Imports PCAxis.Paxiom.Localization
Imports PCAxis.Web.Core.Attributes
Imports System.Web.UI.WebControls
Imports System.Collections.ObjectModel
Imports System.Web.UI
Imports PCAxis.Web.Controls.CommandBar.Plugin
Imports PCAxis.Web.Core.Management
Imports PCAxis.Web.Core.Management.LinkManager
Imports PCAxis.Web.Core.Interfaces
Imports System.Web.UI.HtmlControls
Imports System.Collections.Concurrent

Namespace CommandBar
    ''' <summary>
    ''' Host plugins for manipulation and serialization  on the selected <see cref="PCAxis.Paxiom.PXModel" /> and other functions
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommandBarCodebehind
        Inherits PaxiomControlBase(Of CommandBarCodebehind, CommandBar)
        Private Const EDIT_AND_CALULATE_CAPTION As String = "CtrlCommandBarEditAndCalulateCaption"
        Private Const SAVE_AS_CAPTION As String = "CtrlCommandBarSaveAsCaption"
        Private Const GRAPHS_AND_MAPS_CAPTION As String = "CtrlCommandBarGraphsAndMapsCaption"
        Private Const FILE_DOWNLOAD_LINK As String = "CtrlCommandBarFileDownLoadLink"
        Private Const COMMAND_PLUGIN_IMAGES As String = "PluginImages"
        Private Const COMMAND_PLUGIN_SHORTCUT As String = "PluginShortCut"
        Private Const ID_PLUGINCONTROL As String = "PluginControl"
        Private Const FORMAT_IMAGEBUTTON As String = "ImageButton{0}"
        Private Const FORMAT_SHORTCUT_OPERATION As String = "ShortcutOperation{0}"
        Private Const FORMAT_SHORTCUT_LINK As String = "ShortcutLink{0}"
        Private Const FORMAT_SHORTCUT_FILE As String = "ShortcutFile{0}"
        Private Const FORMAT_COMMANDBARSHORTCUT As String = "CommandBarShortcut{0}"
        Private Const ID_PLUGINSELECTOR As String = "PluginSelector"

        Private Const ACCORDION_OPERATIONS_TITLE As String = "CtrlCommandBarAccordionOperatationsTitle"
        Private Const ACCORDION_SAVE_AS_TITLE As String = "CtrlCommandBarAccordionSaveAsTitle"
        Private Const ACCORDION_SHOW_AS_TITLE As String = "CtrlCommandBarAccordionShowAsTitle"
        Private Const BUTTON_SAVE_AS As String = "CtrlCommandBarSaveAsButton"
        Private Const BUTTON_SHOW_AS As String = "CtrlCommandBarShowAsButton"


        Private _isImageButtonsLoaded As Boolean = False
        Private _isPluginIdChanged As Boolean = False        


#Region "Controls"

        Protected CommandBarPanel As Panel
        Protected WithEvents FunctionDropDownList As DropDownList
        Protected WithEvents SaveAsDropDownList As DropDownList
        Protected WithEvents PresentationViewsDropDownList As DropDownList
        Protected WithEvents ShowAsRadioButtonList As RadioButtonList
        Protected SaveAsRadioButtonList As RadioButtonList
        Protected AccordionPanel As Panel
        Protected OperationsButtonsPanel As Panel
        Protected SaveAsPanel As Panel
        Protected SaveAsHeaderButton As HtmlButton
        Protected ShowResultAsPanel As Panel
        Protected OperationsPanel As Panel
        Protected SaveFilePanel As Panel
        Protected SaveFileLink As HyperLink
        Protected ShowResultLabel As Label
        Protected SaveAsLabel As Label
        Protected OperationsLabel As Label
        Protected WithEvents SaveAsBtn As Button
        Protected WithEvents ShowAsBtn As Button
        Protected ShortcutButtonPanel As Panel
        Protected PluginControlHolder As Panel
        Protected ShowResultAsBody As Panel
        Protected ShowResultAsHeader As HtmlButton
        Protected OptionsBody As Panel
        Protected OperationsHeaderButton As HtmlButton
        Protected lblLegendShow As Label
        Protected lblLegendSave As Label

#End Region

#Region " Properties "
        Private _pluginID As String

        ''' <summary>
        ''' Gets or sets the id of the plugin that is currently loaded
        ''' </summary>
        ''' <value>If a plugin is loaded that is is returned, otherwise the id is <c>null</c></value>
        ''' <returns>The id of the currently loaded plugin or <c>null</c></returns>
        ''' <remarks>This is used to recreate the plugin at postback and to change which plugin to load</remarks>
        <Attributes.PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Protected Property PluginID() As String
            Get
                Return _pluginID
            End Get
            Set(ByVal value As String)
                _pluginID = value
                If Not Me.IsLoadingState Then
                    _isPluginIdChanged = True
                End If
            End Set
        End Property



        Private _isDropDownsLoaded As Boolean = False
        ''' <summary>
        ''' Gets or sets whether the dropdowns have been loaded
        ''' </summary>
        ''' <value>If <c>True</c> then there is no need to populate the dropdowns, otherwise the dropdowns need to be populated</value>
        ''' <returns><c>True</c> if the dropdowns are loaded, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        <Attributes.PropertyPersistState(Core.Enums.PersistStateType.PerControlAndPage)> _
        Protected Property IsDropDownsLoaded() As Boolean
            Get
                Return _isDropDownsLoaded
            End Get
            Set(ByVal value As Boolean)
                _isDropDownsLoaded = value
            End Set
        End Property

        ''Private _fileGenerator As FileGenerator
        '''' <summary>
        '''' Gets an instance of <see cref="FileGenerator" /> with the <see cref="CurrentCulture" />
        '''' </summary>
        '''' <value>Instance of <see cref="FileGenerator" /> with the <see cref="CurrentCulture" /></value>
        '''' <returns>An instance of <see cref="FileGenerator" /> with the <see cref="CurrentCulture" /></returns>
        '''' <remarks></remarks>
        'Private ReadOnly Property FileGenerator() As FileGenerator
        '    Get
        '        If _fileGenerator Is Nothing Then
        '            _fileGenerator = New FileGenerator(CurrentCulture)
        '        End If
        '        Return _fileGenerator
        '    End Get
        'End Property

#End Region

#Region " Events"
        ''' <summary>
        ''' Loads clientscripts, recreates the plugin if needed and load the correct view
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub CommandBar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If (QuerystringManager.GetQuerystringParameter("commandbar") = "false") Then
                CommandBarPanel.Visible = False
            ElseIf QuerystringManager.GetQuerystringParameter("downloadfile") IsNot Nothing Then
                DownloadFile(QuerystringManager.GetQuerystringParameter("downloadfile"))
            Else
                'LoadScripts()

                'If there is a pluign that has a UI and it needs to be recreated,
                'this will make sure it is loaded so it can handle any events connected to it
                If Not String.IsNullOrEmpty(Me.PluginID) Then
                    HandlePlugin(PluginID)
                End If

                LoadView()
            End If

        End Sub


        ''' <summary>
        ''' Used to display a plugin if one was selected
        ''' </summary>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            If (Not _isDropDownsLoaded) And (Not _isImageButtonsLoaded) Then
                LoadView()
            End If

            'If the pluginId has been changed and it's not null or empty
            If _isPluginIdChanged And Not String.IsNullOrEmpty(Me.PluginID) Then
                HandlePlugin(Me.PluginID)
            End If

            MyBase.OnPreRender(e)
        End Sub

        ''' <summary>
        ''' Changes the language of the <see cref="DropDownList"/> and <see cref="ImageButton" /> in the <see cref="CommandBar" />
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub CommandBar_LanguageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LanguageChanged
            'Create a new filegenerator with the new language
            '_fileGenerator = New FileGenerator(CurrentCulture)

            'Make CommandBar reload itself during PreRender
            _isDropDownsLoaded = False
            _isImageButtonsLoaded = False
        End Sub
#End Region


        ''' <summary>
        ''' Loads the commandbars UI with <see cref="DropDownList" /> or <see cref="ImageButton" /> depending on the <see cref="CommandBar.ViewMode" />
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Sub LoadView()

            Dim shortCutPanel = DirectCast(Me.Parent.Parent.FindControl("CommandBarShortCuts"), Panel)

            AccordionPanel.Visible = False
            shortcutPanel.Visible = False

            'Ensure there is a CommandBarPlugin filter
            If Marker.CommandBarFilter Is Nothing Then
                Marker.CommandBarFilter = CommandBarFilterFactory.GetFilter(CommandBarPluginFilterType.None.ToString())
            End If

            'Check the viewmode
            Select Case Me.Marker.ViewMode
                Case CommandBarViewMode.DropDown
                    FillDropDowns()
                    AccordionPanel.Visible = True
                    FillShortcutButtons(shortCutPanel)
                    shortcutPanel.Visible = True
                Case CommandBarViewMode.ImageButtons
                    FillButtons(shortCutPanel)
                    shortcutPanel.Visible = True
            End Select

            If shortCutPanel.Controls.Count > 0 Then
                'Set this to ture so that we don't try and create the imagebuttons more than once
                _isImageButtonsLoaded = True
            End If

            Me.SaveFilePanel.Visible = False

        End Sub

        Private Function IsJavascriptEnabled() As Boolean
            Return System.Web.HttpContext.Current.Request.Browser.EcmaScriptVersion.Major >= 1
        End Function

        ''' <summary>
        ''' Fills the dropdowns in the commandbar with plugins and adds shortcuts under them, used when <see cref="CommandBar.ViewMode" /> is <see cref="CommandBarViewMode.DropDown" />
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillDropDowns()
            Dim plugin As CommandBarPluginInfo
            Dim li As ListItem

            'Set DropDowns Enabled/Disabled
            SaveAsHeaderButton.Disabled = Not Marker.CommandBarFilter.DropDownFileFormatsActive
            OperationsHeaderButton.Disabled = Not Marker.CommandBarFilter.DropDownOperationsActive
            ShowResultAsPanel.Enabled = Marker.CommandBarFilter.DropDownViewsActive

            'Add plugins to the operations dropdown
            For Each op As String In Marker.Operations
                plugin = CommandBarPluginManager.Operations(op)

                If Not plugin Is Nothing Then
                    'Add item to dropdownlist if filter allows it
                    If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.OPERATION) Then
                        Dim button As Button = CreatePluginButton(plugin, FORMAT_IMAGEBUTTON, COMMAND_PLUGIN_IMAGES, Plugins.Categories.OPERATION)
                        OperationsButtonsPanel.Controls.Add(button)
                    End If
                End If
            Next

            'Only do this if the dropdowns haven't already been loaded
            If IsDropDownsLoaded = False Then

                SaveAsRadioButtonList.Items.Clear()
                ShowAsRadioButtonList.Items.Clear()

                'Add a localized text for accordions
                ShowResultLabel.Text = GetLocalizedString(ACCORDION_SHOW_AS_TITLE)
                SaveAsLabel.Text = GetLocalizedString(ACCORDION_SAVE_AS_TITLE)
                OperationsLabel.Text = GetLocalizedString(EDIT_AND_CALULATE_CAPTION)
                SaveAsBtn.Text = GetLocalizedString(BUTTON_SAVE_AS)
                SaveAsBtn.Attributes.Add("aria-label", GetLocalizedString("CtrlCommandBarSaveAsButtonScreenReader"))
                ShowAsBtn.Text = GetLocalizedString(BUTTON_SHOW_AS)
                ShowAsBtn.Attributes.Add("aria-label", GetLocalizedString("CtrlCommandBarShowAsButtonScreenReader"))
                lblLegendShow.Text = GetLocalizedString("CtrlCommandBarShowAsLegend")
                lblLegendSave.Text = GetLocalizedString("CtrlCommandBarSaveAsLegend")

                'Add fileformats to the "Save as" dropdown
                For Each outputFormat As String In Marker.OutputFormats
                    'Add item to dropdownlist if filter allows it
                    If Marker.CommandBarFilter.UseOutputFormat(outputFormat) Then
                        Dim downLoadUrl = Request.RawUrl & "?downloadfile=" & outputFormat

                        If Request.QueryString.Count > 0 Then
                            downLoadUrl = Request.RawUrl & "&downloadfile=" & outputFormat
                        End If

                        SaveAsRadioButtonList.Items.Add(New ListItem(GetLocalizedString(outputFormat), downLoadUrl))
                    End If
                Next

                If SaveAsRadioButtonList.Items.Count > 0 Then
                    SaveAsRadioButtonList.Items(0).Selected = true
                End If

                'Add plugins to the presentation views dropdown
                For Each op As String In Marker.PresentationViews
                    plugin = CommandBarPluginManager.Views(op)

                    If Not plugin Is Nothing Then
                        'Add item to dropdownlist if filter allows it
                        If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.VIEW) Then
                            li = New ListItem(Me.GetLocalizedString(plugin.NameCode), plugin.Name)
                            If li.Value.Equals(Marker.SelectedPresentationView) Then
                                li.Selected = True
                            End If
                            ShowAsRadioButtonList.Items.Add(li)
                        End If

                    End If
                Next

                IsDropDownsLoaded = True
            End If
        End Sub

        Private Sub FillShortcutButtons(ByVal shortCutPanel As Panel)
            Dim plugin As CommandBarPluginInfo
            Dim filetype As FileType
            Dim li As ListItem

            'Operations
            For Each op As String In Marker.OperationShortcuts
                plugin = CommandBarPluginManager.Operations(op)

                If Not plugin Is Nothing Then
                    If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.OPERATION) Then
                        Dim button As Button = CreatePluginButton(plugin, FORMAT_IMAGEBUTTON, COMMAND_PLUGIN_IMAGES, Plugins.Categories.OPERATION)
                        shortCutPanel.Controls.Add(button)
                    End If
                End If
            Next

            'Presentations
            For Each s As String In Marker.PresentationViewShortcuts
                plugin = CommandBarPluginManager.Views(s)

                If Not plugin Is Nothing Then
                    If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.VIEW) Then
                        Dim button As Button = CreatePluginButton(plugin, FORMAT_IMAGEBUTTON, COMMAND_PLUGIN_IMAGES, Plugins.Categories.VIEW)
                        shortCutPanel.Controls.Add(button)
                    End If
                End If
            Next

            'Add shortcut buttons for file formats
            For Each fileformat As String In Marker.FileformatShortcuts
                filetype = CommandBarPluginManager.GetFileType(fileformat)

                If Not filetype Is Nothing Then
                    'Add item to dropdownlist if filter allows it
                    If Marker.CommandBarFilter.UseFiletype(filetype) Then
                        Dim button As Button = CreateFileformatShortcutButton(filetype, fileformat)
                        shortCutPanel.Controls.Add(button)
                    End If
                End If
            Next
        End Sub

        Private Sub FillButtons(ByVal shortCutPanel As Panel)
            Dim plugin As CommandBarPluginInfo
            Dim filetype As FileType
            Dim li As ListItem


            If Not _isImageButtonsLoaded Then
                shortCutPanel.Controls.Clear()

                'Operations
                For Each op As String In Marker.OperationButtons
                    plugin = CommandBarPluginManager.Operations(op)

                    If Not plugin Is Nothing Then
                        If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.OPERATION) Then
                            Dim button As Button = CreatePluginButton(plugin, FORMAT_IMAGEBUTTON, COMMAND_PLUGIN_IMAGES, Plugins.Categories.OPERATION)
                            shortCutPanel.Controls.Add(button)
                        End If
                    End If
                Next

                'Presentations
                For Each s As String In Marker.PresentationViewButtons
                    plugin = CommandBarPluginManager.Views(s)

                    If Not plugin Is Nothing Then
                        If Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, Plugins.Categories.VIEW) Then
                            Dim button As Button = CreatePluginButton(plugin, FORMAT_IMAGEBUTTON, COMMAND_PLUGIN_IMAGES, Plugins.Categories.VIEW)
                            shortCutPanel.Controls.Add(button)
                        End If
                    End If
                Next

                'Add shortcut buttons for file formats
                For Each fileformat As String In Marker.FiletypeButtons
                    filetype = CommandBarPluginManager.GetFileType(fileformat)

                    If Not filetype Is Nothing Then
                        'Add item to dropdownlist if filter allows it
                        If Marker.CommandBarFilter.UseFiletype(filetype) Then
                            Dim button As Button = CreateFileformatShortcutButton(filetype, fileformat)
                            shortCutPanel.Controls.Add(button)
                        End If
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Creates a Operations button for the given plugin
        ''' </summary>
        ''' <param name="plugin">Plugin to create imagebutton for</param>
        ''' <param name="idString">String used togheter with the plugin name for creating the ID of the imagebutton</param>
        ''' <param name="commandName">CommandName of the imagebutton</param>
        ''' <returns>The created imagebutton</returns>
        ''' <remarks></remarks>
        Private Function CreatePluginButton(ByVal plugin As CommandBarPluginInfo, ByVal idString As String, ByVal commandName As String, ByVal pluginCategory As String) As Button
            Dim button As Button = New Button()

            If plugin Is Nothing Then
                Throw New System.ArgumentNullException()
            End If
            With button
                .ID = String.Format(idString, plugin.Name, Globalization.CultureInfo.InvariantCulture)
                .CommandArgument = plugin.Name
                .ToolTip = Me.GetLocalizedString(plugin.NameCode)
                If (plugin.Name = "pivotCCW" Or plugin.Name = "pivotCW") Then
                    .CssClass = $"pxweb-btn icon-placement pxweb-buttons {plugin.Name}"
                Else
                    .CssClass = $"pxweb-btn icon-placement pxweb-buttons pxweb-open-tools-button {plugin.Name}"
                End If
                .CommandName = commandName
                .Text = Me.GetLocalizedString(plugin.NameCode)
                AddHandler .Command, AddressOf Button_Command

                If Not Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, pluginCategory) Then
                    .Visible = False
                End If
            End With

            Return Button
        End Function

        Private Function CreateFileformatShortcutButton(ByVal filetype As FileType, ByVal fileformat As String) As Button
            Dim button As Button = New Button()

            If filetype Is Nothing Then
                Throw New System.ArgumentNullException()
            End If
            If String.IsNullOrEmpty(fileformat) Then
                Throw New System.ArgumentNullException()
            End If

            With button
                .ID = String.Format(FORMAT_SHORTCUT_FILE, fileformat, Globalization.CultureInfo.InvariantCulture)
                .CommandArgument = fileformat
                .ToolTip = Me.GetLocalizedString(fileformat)
                .CommandName = COMMAND_PLUGIN_SHORTCUT
                .CssClass = $"pxweb-btn icon-placement pxweb-buttons {fileformat}"
                If IsJavascriptEnabled() And (Not String.IsNullOrEmpty(fileformat)) Then
                    Dim downLoadUrl = Request.RawUrl & "?downloadfile=" & fileformat

                    If Request.QueryString.Count > 0 Then
                        downLoadUrl = Request.RawUrl & "&downloadfile=" & fileformat
                    End If
                    .OnClientClick = "commandbarDownloadFile('" + downLoadUrl + "'); return false;"
                End If
                AddHandler .Command, AddressOf Button_Command
            End With

            Return button
        End Function




        ''' <summary>
        ''' Loads a plugin using the supplied <paramref name="pluginKey" />
        ''' </summary>
        ''' <param name="pluginKey">The unique key of the plugin</param>
        ''' <remarks>Loads plugins with or without UI and fileformats</remarks>
        Private Sub HandlePlugin(ByVal pluginKey As String, Optional ByVal shortcut As Boolean = False)


            'Validate input
            If String.IsNullOrEmpty(pluginKey) Then
                Return
            End If

            Dim c As ConcurrentDictionary(Of String, Plugin.CommandBarPluginInfo)
            Dim pluginCategory As String
            If CommandBarPluginManager.Operations.ContainsKey(pluginKey) Then
                c = CommandBarPluginManager.Operations
                pluginCategory = Plugins.Categories.OPERATION
            Else
                c = CommandBarPluginManager.Views
                pluginCategory = Plugins.Categories.VIEW
            End If

            If c.ContainsKey(pluginKey) Then
                'If the plugins is a commandbar plugin the get it from the plugins collection
                Dim plugin As CommandBarPluginInfo = c(pluginKey)

                If pluginCategory = Plugins.Categories.OPERATION Then
                    HandleOperation(plugin, pluginKey)
                ElseIf pluginCategory = Plugins.Categories.VIEW Then
                    SignalAction(PxActionEventType.Presentation, pluginKey)
                    Marker.ScreenOutputMethod.Invoke(pluginKey, Me.PaxiomModel)
                End If


            ElseIf CommandBarPluginManager.GetFileType(pluginKey) IsNot Nothing Then
                'If the plugin is a fileformat (for example FileTypeExcelDoubleColumn) 
                'load it from filegenerator
                Dim ft As FileType = CommandBarPluginManager.GetFileType(pluginKey)
                LoadFileTypeControl(pluginKey, ft, pluginKey, False)
            Else
                'If the plugin key is a filetype (for example xls)
                Dim fileType As FileType = CommandBarPluginManager.FileTypes(pluginKey)
                Dim showUI As Boolean

                If shortcut Then
                    'Never show UI when shortcut...
                    showUI = False
                Else
                    showUI = True
                End If

                'if the filetype only have one fileformat
                If fileType.FileFormats.Count = 1 Then
                    LoadFileTypeControl(fileType.Type, fileType, fileType.FileFormats.First.Key, showUI)
                Else
                    LoadFileTypeControl(fileType.Type, fileType, String.Empty, showUI)
                End If
            End If
        End Sub

        Private Sub HandleOperation(ByVal plugin As CommandBarPluginInfo, ByVal pluginKey As String)
            'If the control has a UI load it and add it the commandbars plugin container
            If plugin.HasUI Then
                Dim control As ICommandBarGUIPlugin = Nothing
                control = plugin.GetInstance()

                CType(control, Control).ID = ID_PLUGINCONTROL
                AddHandler control.Finished, AddressOf Plugin_Finished

                ShowPlugin(pluginKey, CType(control, Control))
            Else
                'Otherwise execute the plugin immediately and set the result
                SignalAction(PxActionEventType.Operation, pluginKey)
                PaxiomManager.PaxiomModel = plugin.Invoke(Me.PaxiomModel, Page.Controls)
                UpdatePluginVisibility(plugin, Plugins.Categories.OPERATION)
                ClearPlugin()
            End If
        End Sub

        ''' <summary>
        ''' If the plugin have constraints the operation may have made that the plugin shall no longer be visible to the user.
        ''' This method checks the constraints of the plugin and updates it´s visibility
        ''' </summary>
        ''' <param name="plugin">The plugin to update visibility for</param>
        ''' <remarks></remarks>
        Private Sub UpdatePluginVisibility(ByVal plugin As CommandBarPluginInfo, ByVal pluginCategory As String)
            If plugin.Constraints.Count > 0 Then
                'If Not CheckPropertyValues(plugin.Constraints()) Then
                If Not Marker.CommandBarFilter.UsePlugin(plugin, Me.PaxiomModel, pluginCategory) Then
                    'The plugins do not fullfill constraints anymore - Hide the plugin!

                    If Marker.ViewMode = CommandBarViewMode.ImageButtons Then
                        'Hide image button
                        HidePluginImageButton(String.Format(FORMAT_IMAGEBUTTON, plugin.Name))
                    ElseIf Marker.ViewMode = CommandBarViewMode.DropDown Then
                        'Remove plugin from operations dropdown
                        HidePluginDropDown(FunctionDropDownList, plugin.Name)

                        'Remove plugin from links dropdown
                        HidePluginDropDown(PresentationViewsDropDownList, plugin.Name)

                        'Hide operation shortcut button
                        HidePluginImageButton(String.Format(FORMAT_SHORTCUT_OPERATION, plugin.Name))

                        'Hide links shortcut button
                        HidePluginImageButton(String.Format(FORMAT_SHORTCUT_LINK, plugin.Name))
                    End If

                    'Hide CommandBar shortcut button
                    HidePluginImageButton(String.Format(FORMAT_COMMANDBARSHORTCUT, plugin.Name))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Hides a imagebutton
        ''' </summary>
        ''' <param name="id">Id of the image button</param>
        ''' <remarks></remarks>
        Private Sub HidePluginImageButton(ByVal id As String)
            Dim btn As ImageButton

            btn = CType(Me.FindControl(id), ImageButton)
            If Not btn Is Nothing Then
                btn.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' Removes a plugin from a dropdown list
        ''' </summary>
        ''' <param name="dropdown">The dropdown list to remove plugin from</param>
        ''' <param name="pluginId">Id of the plugin to remove</param>
        ''' <remarks></remarks>
        Private Sub HidePluginDropDown(ByVal dropdown As DropDownList, ByVal pluginId As String)
            For Each li As ListItem In dropdown.Items
                If li.Value.Equals(pluginId) Then
                    dropdown.Items.Remove(li)
                    Exit Sub
                End If
            Next
        End Sub

        ''' <summary>
        ''' Loads a filetypes webcontrol
        ''' </summary>
        ''' <param name="pluginKey">The key to use to make sure the control gets displayed again</param>
        ''' <param name="ft">The <see cref="FileType" /> of the control</param>
        ''' <param name="fileFormat">The fileformat to use. Can be empty</param>
        ''' <remarks></remarks>
        Private Sub LoadFileTypeControl(ByVal pluginKey As String, ByVal ft As FileType, ByVal fileFormat As String, ByVal showUI As Boolean)
            'Load the filetypes webcontrol
            Dim fileTypeControl As IFileTypeControl = CType(Activator.CreateInstance(Type.GetType(ft.WebControl)), IFileTypeControl)

            With fileTypeControl
                CType(fileTypeControl, Control).ID = pluginKey
                .SelectedFormat = If(fileFormat Is Nothing, String.Empty, fileFormat)
                .SelectedFileType = ft
                .ShowUI = showUI
            End With

            AddHandler fileTypeControl.Finished, AddressOf FileType_Finished

            'Show the filetypes webcontrol
            ShowPlugin(pluginKey, CType(fileTypeControl, Control))
        End Sub
        ''' <summary>
        ''' Makes sure the supplied <paramref name="pluginControl" /> is displayed
        ''' </summary>
        ''' <param name="pluginKey">The key to use to make sure the control gets displayed again</param>
        ''' <param name="pluginControl">The control to display</param>
        ''' <remarks></remarks>
        Private Sub ShowPlugin(ByVal pluginKey As String, ByVal pluginControl As Control)
            Me._pluginID = pluginKey

            With PluginControlHolder
                .Visible = True
                .Controls.Clear()
                .Controls.Add(pluginControl)
                .Focus()
            End With
        End Sub

        Private Sub ShowAsBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowAsBtn.Click
            If ShowAsRadioButtonList.SelectedValue IsNot Nothing Then
                Me.PluginID = ShowAsRadioButtonList.SelectedValue
            End If
        End Sub

        Private Sub SaveAsBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveAsBtn.Click
            If SaveAsRadioButtonList.SelectedValue IsNot Nothing Then
                Dim selectedValue As String
                selectedValue = SaveAsRadioButtonList.SelectedValue
                'If only one parameter the whole url is set as key
                If System.Web.HttpUtility.ParseQueryString(selectedValue).Count > 1
                    DownloadFile(System.Web.HttpUtility.ParseQueryString(selectedValue).Get("downloadfile"))
                Else 
                    DownloadFile(System.Web.HttpUtility.ParseQueryString(selectedValue).Get(0))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Operation selected
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub DropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FunctionDropDownList.SelectedIndexChanged

            Dim dropdown As DropDownList = TryCast(sender, DropDownList)

            If dropdown IsNot Nothing Then
                If dropdown.SelectedIndex > 0 Then
                    'Doesn't load the plugin directly but sets the PluginId instead 
                    'This is done so that when we download a file selected in dropdown and the dropdown isn't reset
                    'and we press a shortcut later the dropdown doesn't override the buttons plugin
                    Me.PluginID = dropdown.SelectedItem.Value
                    dropdown.SelectedIndex = 0
                End If
            End If

        End Sub

        ''' <summary>
        ''' Presentation view selected
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub PresentationViewsDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PresentationViewsDropDownList.SelectedIndexChanged

            Dim dropdown As DropDownList = TryCast(sender, DropDownList)

            If dropdown IsNot Nothing Then
                'Doesn't load the plugin directly but sets the PluginId instead 
                'This is done so that when we download a file selected in dropdown and the dropdown isn't reset
                'and we press a shortcut later the dropdown doesn't override the buttons plugin
                Me.PluginID = dropdown.SelectedItem.Value
                dropdown.SelectedIndex = 0
            End If

        End Sub

        ''' <summary>
        ''' Used by the file download <see cref="DropDownList" /> in the commandbar
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub SaveAsDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveAsDropDownList.SelectedIndexChanged
            Dim selectedValue As String

            selectedValue = SaveAsDropDownList.SelectedValue
            SaveAsDropDownList.SelectedIndex = 0
            DownloadFile(System.Web.HttpUtility.ParseQueryString(selectedValue)("downloadfile"))
        End Sub

        ''' <summary>
        ''' 1. Show download link for the selected file format.
        ''' 2. Reset the commandbar
        ''' 3. If javascript is enabled download is done automatically
        ''' </summary>
        ''' <param name="pluginID">File format</param>
        ''' <remarks></remarks>
        Private Sub CreateDownloadLink(ByVal pluginID As String)
            If String.IsNullOrEmpty(pluginID) Then
                'Cancel button clicked
                ClearPlugin()
                Exit Sub
            Else
                Dim downLoadUrl = Request.RawUrl & "?downloadfile=" & pluginID

                If Request.QueryString.Count > 0 Then
                    downLoadUrl = Request.RawUrl & "&downloadfile=" & pluginID
                End If

                Me.SaveFileLink.NavigateUrl = downLoadUrl
                Me.SaveFilePanel.Visible = True
                'Set localized text on the download link
                SaveFileLink.Text = GetLocalizedString(FILE_DOWNLOAD_LINK)
                ClearPlugin()
                'Call script that automatically starts the download
                Page.ClientScript.RegisterClientScriptBlock(GetType(Page), "download", "jQuery(document).ready(function(){automaticFileDownload('" & SaveFileLink.ClientID & "')});", True)
            End If
        End Sub

        ''' <summary>
        ''' Downloads a file in the selected file format
        ''' </summary>
        ''' <param name="pluginID">File format</param>
        ''' <remarks></remarks>
        Private Sub DownloadFile(ByVal pluginID As String)
            If CommandBarPluginManager.GetFileType(pluginID) IsNot Nothing Then
                Dim ft As FileType = CommandBarPluginManager.GetFileType(pluginID)
                Dim fileTypeControl As IFileTypeControl = CType(Activator.CreateInstance(Type.GetType(ft.WebControl)), IFileTypeControl)

                With fileTypeControl
                    CType(fileTypeControl, Control).ID = pluginID
                    .SelectedFormat = pluginID
                    .SelectedFileType = ft
                    .ShowUI = False
                End With

                SignalAction(PxActionEventType.SaveAs, pluginID)

                fileTypeControl.SerializeAndStream()
            End If
        End Sub

        ''' <summary>
        ''' Called by all <see cref="ImageButton" /> in the commandbar to make the clicked <see cref="ImageButton" /> plugin to be loaded
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">A CommandEventArgs that contains the event data</param>
        ''' <remarks>If the <see cref="ImageButton" /> has more than one plugin connected to it, a <see cref="CommandBarPluginSelector"/> is shown</remarks>
        Private Sub Button_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)

            'If this is a plugin
            If e.CommandName = COMMAND_PLUGIN_IMAGES Then
                Me.PluginID = e.CommandArgument.ToString

            ElseIf e.CommandName = COMMAND_PLUGIN_SHORTCUT Then
                'if this is shortcut, use the commandargument as a plugin key and HandlePlugin will get the correct pluign to load
                'Shortcuts can only have one plugin
                Me.PluginID = e.CommandArgument.ToString
            End If

        End Sub

        ''' <summary>
        ''' Called when a file format has been selected for the active file type user control
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An EventArgs that contains no event data</param>
        ''' <remarks></remarks>
        Private Sub FileType_Finished(ByVal sender As Object, ByVal e As EventArgs)
            'CreateDownloadLink(CType(Me.PluginControlHolder.Controls(0), IFileTypeControl).SelectedFormat)

            SignalAction(PxActionEventType.SaveAs, PluginID)

            ClearPlugin()
        End Sub

        ''' <summary>
        ''' Called when a plugin with a UI is finished
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An <see cref="CommandBarPluginFinishedEventArgs" /> that contains the event data</param>
        ''' <remarks></remarks>
        Private Sub Plugin_Finished(ByVal sender As Object, ByVal e As CommandBarPluginFinishedEventArgs)

            If e.PaxiomModel IsNot Nothing Then
                PaxiomManager.PaxiomModel = e.PaxiomModel
                SignalAction(PxActionEventType.Operation, PluginID)
            End If
            ClearPlugin()
        End Sub

        ''' <summary>
        ''' Called when the <see cref="CommandBarPluginSelector" /> has selected a plugin
        ''' </summary>
        ''' <param name="sender">The source of the event</param>
        ''' <param name="e">An <see cref="CommandBarPluginSelectorPluginSelectedEventArgs" /> that contains the event data</param>
        ''' <remarks></remarks>
        Private Sub PluginSelector_PluginSelected(ByVal sender As Object, ByVal e As CommandBarPluginSelectorPluginSelectedEventArgs)
            HandlePlugin(e.SelectedPluginName)
        End Sub

        ''' <summary>
        ''' Removes the active plugin from being displayed
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearPlugin()
            'Clear the id so the control doesn't get recreate on postback
            Me.PluginID = Nothing

            'Clear the pluginholder
            With PluginControlHolder
                .Controls.Clear()
                .Visible = False
            End With
            OperationsPanel.Focus()
        End Sub


        ''' <summary>
        ''' Signal PX action to listeners
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SignalAction(ByVal actionType As PxActionEventType, ByVal actionName As String)
            Dim args As New PxActionEventArgs(actionType, actionName)
            PxActionEventHelper.SetModelProperties(args, PaxiomManager.PaxiomModel)
            Marker.OnPxActionEvent(args)
        End Sub
    End Class

End Namespace
