Imports System.Configuration
Namespace PCAxis.Paxiom.Configuration.Sections
    Public Class FileGeneratorSection
        Inherits ConfigurationSection

        Const CONFIG_FILEPATH As String = "filepath"

        <ConfigurationProperty(CONFIG_FILEPATH, IsRequired:=True)> _
        Public Property ConfigFilePath() As String
            Get
                Return Me(CONFIG_FILEPATH).ToString()
            End Get
            Set(ByVal value As String)
                Me(CONFIG_FILEPATH) = value
            End Set
        End Property
    
    End Class
End Namespace
