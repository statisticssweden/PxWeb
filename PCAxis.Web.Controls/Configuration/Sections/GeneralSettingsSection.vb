Imports System.Configuration
Namespace Configuration.Sections
    Public Class GeneralSettingsSection
        Inherits ConfigurationSection

        'Const CONFIG_CACHEVIRTUALPATH As String = "cachevirtualpath"
        Const CONFIG_IMAGESPATH As String = "imagespath"


        '<ConfigurationProperty(CONFIG_CACHEVIRTUALPATH, DefaultValue:="~/", IsRequired:=False)> _
        'Public Property CacheVirtualPath() As String
        '    Get
        '        Return Me(CONFIG_CACHEVIRTUALPATH).ToString()
        '    End Get
        '    Set(ByVal value As String)
        '        Me(CONFIG_CACHEVIRTUALPATH) = value
        '    End Set
        'End Property

        <ConfigurationProperty(CONFIG_IMAGESPATH, DefaultValue:="~/", IsRequired:=False)> _
        Public Property ImagesPath() As String
            Get
                Return Me(CONFIG_IMAGESPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_IMAGESPATH) = value
            End Set
        End Property

    End Class
End Namespace
