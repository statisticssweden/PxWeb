Namespace PCAxis.PlugIn

    Public Interface IPlugIn
        ReadOnly Property Id() As Guid
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
        Sub Initialize(ByVal host As IPluginHost, ByVal configuration As Dictionary(Of String, String))
        Sub Terminate()
    End Interface

End Namespace