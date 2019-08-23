
Namespace CommandBar.Plugin
    <Serializable()> _
    Public Class CommandBarTableLayout2Filter
        Inherits CommandBarFilterNone
        Implements ICommandBarPluginFilter

        Private _pluginBlackList As List(Of String)
        Private _outputFiletypeBlackList As List(Of String)
        Private _outputFormatBlackList As List(Of String)

        ''' <summary>
        ''' Constructor creates list of plugins and outputformats to filter away
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            'Set dropdowns to active/inactive
            _dropDownFileFormatsActive = True
            _dropDownOperationsActive = True
            _dropDownViewsActive = True

            'Add items to filter away eg.: "_pluginBlackList.Add(Plugins.Operations.PIVOT_MANUAL)"
            _pluginBlackList = New List(Of String)()

            'Add items to filter away (filetype buttons) eg.: "_outputFiletypeBlackList.Add(Plugins.FileFormats.CSV_COMMA_WITH_HEADING)
            _outputFiletypeBlackList = New List(Of String)()
            _outputFiletypeBlackList.Add(Plugins.FileFormats.CHART_GIF)
            _outputFiletypeBlackList.Add(Plugins.FileFormats.CHART_JPEG)
            _outputFiletypeBlackList.Add(Plugins.FileFormats.CHART_PNG)



            'Add items to filter (output formats in dropdowns) away eg.: _outputFormatBlackList.Add(Plugins.FileFormats.CSV_COMMA_WITH_HEADING)
            _outputFormatBlackList = New List(Of String)()
            _outputFormatBlackList.Add(Plugins.FileFormats.CHART_GIF)
            _outputFormatBlackList.Add(Plugins.FileFormats.CHART_JPEG)
            _outputFormatBlackList.Add(Plugins.FileFormats.CHART_PNG)

        End Sub

        ''' <summary>
        ''' Checks if rules for the model allows the plugin, then checks if the plugin should be used used with filter applied.
        ''' </summary>
        ''' <param name="plugin">Plugin to check</param>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if plugin should be used used with filter applied</returns>
        Public Overrides Function UsePlugin(ByVal plugin As CommandBarPluginInfo, ByRef model As Paxiom.PXModel, ByVal pluginCategory As String) As Boolean
            Dim dropDownActive As Boolean
            If pluginCategory = Plugins.Categories.OPERATION Then
                dropDownActive = _dropDownOperationsActive
            ElseIf pluginCategory = Plugins.Categories.VIEW Then
                dropDownActive = _dropDownViewsActive
            End If

            If dropDownActive Then
                If MyBase.CheckSpecialConstraints(plugin, model) Then
                    Return MyBase.ApplyFilter(_pluginBlackList, plugin)
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Compare filetype name with the filetype blacklist
        ''' </summary>
        ''' <param name="ftype">filetype to check</param>
        ''' <returns>True if filetype not is in the blacklist</returns>
        ''' <remarks></remarks>
        Public Overrides Function UseFiletype(ByVal ftype As PCAxis.Web.Core.FileType) As Boolean
            If _dropDownFileFormatsActive Then
                Return MyBase.ApplyFilter(_outputFiletypeBlackList, ftype)
            End If
            Return False
        End Function


        ''' <summary>
        ''' Compare fileformat name with the filefomat blacklist
        ''' </summary>
        ''' <param name="outputFormat">outputformat to check</param>
        ''' <returns>True if fileformat not is in the blacklist</returns>
        ''' <remarks></remarks>
        Public Overrides Function UseOutputFormat(ByVal outputFormat As String) As Boolean
            If _dropDownFileFormatsActive Then
                Return MyBase.ApplyFilter(_outputFormatBlackList, outputFormat)
            End If
        End Function


    End Class
End Namespace