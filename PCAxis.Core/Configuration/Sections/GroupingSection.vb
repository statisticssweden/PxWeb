Imports System.Configuration
Namespace PCAxis.Paxiom.Configuration.Sections
    Public Class GroupingSection
        Inherits ConfigurationSection

        Const CONFIG_FILESPATH As String = "filespath"

        Public Sub New()
            'Set defaultvalues if no config file is present
            Me.FilesPath = ""
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
    End Class
End Namespace
