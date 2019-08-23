Imports System.Configuration
Namespace Configuration.Sections
    Public Class CommandBarSection
        Inherits ConfigurationSection

        Const CONFIG_OPERATIONS_FILEPATH As String = "operationsFilepath"
        Const CONFIG_VIEWS_FILEPATH As String = "viewsFilepath"
        Const CONFIG_FILETYPES_FILEPATH As String = "fileTypesFilepath"
        Const CONFIG_XSDPATH As String = "xsdpath"


        <ConfigurationProperty(CONFIG_OPERATIONS_FILEPATH, DefaultValue:="~/", IsRequired:=False)> _
        Public Property OperationsFilepath() As String
            Get
                Return Me(CONFIG_OPERATIONS_FILEPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_OPERATIONS_FILEPATH) = value
            End Set
        End Property

        <ConfigurationProperty(CONFIG_VIEWS_FILEPATH, DefaultValue:="~/", IsRequired:=False)> _
        Public Property ViewsFilepath() As String
            Get
                Return Me(CONFIG_VIEWS_FILEPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_VIEWS_FILEPATH) = value
            End Set
        End Property

        <ConfigurationProperty(CONFIG_FILETYPES_FILEPATH, DefaultValue:="~/", IsRequired:=False)> _
        Public Property FileTypesFilepath() As String
            Get
                Return Me(CONFIG_FILETYPES_FILEPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_FILETYPES_FILEPATH) = value
            End Set
        End Property

        <ConfigurationProperty(CONFIG_XSDPATH, DefaultValue:="~/", IsRequired:=False)> _
       Public Property Xsdpath() As String
            Get
                Return Me(CONFIG_XSDPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_XSDPATH) = value
            End Set
        End Property

    End Class
End Namespace
