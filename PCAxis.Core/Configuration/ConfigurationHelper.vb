Imports System.Configuration
Imports PCAxis.Paxiom.Configuration.Sections

Namespace PCAxis.Paxiom.Configuration
    Public Class ConfigurationHelper

        Private Const CONFIG_LOCALIZATION As String = "pcaxis/paxiom/localization"
        Private Const CONFIG_FILEGENERATOR As String = "pcaxis/paxiom/filegenerator"
        Private Const CONFIG_GROUPING As String = "pcaxis/paxiom/grouping"

        Shared ReadOnly Property LocalizationSection() As LocalizationSection
            Get
                If ConfigurationManager.GetSection(CONFIG_LOCALIZATION) Is Nothing Then
                    Return New LocalizationSection
                Else
                    Return CType(ConfigurationManager.GetSection(CONFIG_LOCALIZATION), LocalizationSection)
                End If
            End Get
        End Property

        Shared ReadOnly Property FileGeneratorSection() As FileGeneratorSection
            Get
                Return CType(ConfigurationManager.GetSection(CONFIG_FILEGENERATOR), FileGeneratorSection)
            End Get
        End Property

        Shared ReadOnly Property GroupingSection() As GroupingSection
            Get
                If ConfigurationManager.GetSection(CONFIG_GROUPING) Is Nothing Then
                    Return New GroupingSection
                Else
                    Return CType(ConfigurationManager.GetSection(CONFIG_GROUPING), GroupingSection)
                End If
            End Get
        End Property
    End Class
End Namespace
