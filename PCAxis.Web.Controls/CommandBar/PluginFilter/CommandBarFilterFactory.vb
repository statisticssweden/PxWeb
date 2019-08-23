Namespace CommandBar.Plugin
    Public Module CommandBarFilterFactory
        Private _chartFilter As CommandBarChartFilter
        Private _footnoteFilter As CommandBarFootNoteFilter
        Private _informationFilter As CommandBarInformationFilter
        Private _noFilter As CommandBarFilterNone
        Private _sortFilter As CommandBarSortFilter
        Private _tableLayout1Filter As CommandBarTableLayout1Filter
        Private _tableLayout2Filter As CommandBarTableLayout2Filter
        Private _tableInformationFilter As CommandBarInformationFilter

        Public Function GetFilter(ByVal filterType As String) As ICommandBarPluginFilter
            Select Case filterType
                Case CommandBarPluginFilterType.Chart.ToString()
                    If _chartFilter Is Nothing Then
                        _chartFilter = New CommandBarChartFilter()
                    End If
                    Return _chartFilter
                Case CommandBarPluginFilterType.Footnoote.ToString()
                    If _footnoteFilter Is Nothing Then
                        _footnoteFilter = New CommandBarFootNoteFilter()
                    End If
                    Return _footnoteFilter
                Case CommandBarPluginFilterType.Information.ToString()
                    If _informationFilter Is Nothing Then
                        _informationFilter = New CommandBarInformationFilter()
                    End If
                    Return _informationFilter
                Case CommandBarPluginFilterType.Sort.ToString()
                    If _sortFilter Is Nothing Then
                        _sortFilter = New CommandBarSortFilter()
                    End If
                    Return _sortFilter
                Case CommandBarPluginFilterType.TableLayout1.ToString()
                    If _tableLayout1Filter Is Nothing Then
                        _tableLayout1Filter = New CommandBarTableLayout1Filter()
                    End If
                    Return _tableLayout1Filter
                Case CommandBarPluginFilterType.TableLayout2.ToString()
                    If _tableLayout2Filter Is Nothing Then
                        _tableLayout2Filter = New CommandBarTableLayout2Filter()
                    End If
                    Return _tableLayout2Filter
                Case Else
                    If _noFilter Is Nothing Then
                        _noFilter = New CommandBarFilterNone()
                    End If
                    Return _noFilter
            End Select
        End Function
    End Module
End Namespace