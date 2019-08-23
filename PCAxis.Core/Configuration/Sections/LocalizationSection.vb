Imports System.Configuration
Namespace PCAxis.Paxiom.Configuration.Sections
    Public Class LocalizationSection
        Inherits ConfigurationSection

        Const CONFIG_FILESPATH As String = "filespath"
        Const CONFIG_BASEFILE As String = "basefile"

        Public Sub New()
            'Set defaultvalues if no config file is present
            Me.FilesPath = ""
            Me.BaseFile = "pxlang"
        End Sub

        <ConfigurationProperty(CONFIG_FILESPATH, IsRequired:=True)> _
      Public Property FilesPath() As String
            Get
                Return Me(CONFIG_FILESPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_FILESPATH) = value
            End Set
        End Property

        <ConfigurationProperty(CONFIG_BASEFILE, IsRequired:=True)> _
      Public Property BaseFile() As String
            Get
                Return Me(CONFIG_BASEFILE).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_BASEFILE) = value
            End Set
        End Property

    End Class
End Namespace
