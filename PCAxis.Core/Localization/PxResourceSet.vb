Imports System.Resources
Imports System.Globalization

Namespace PCAxis.Paxiom.Localization
    ''' <summary>
    ''' Contains all sentences for one language.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxResourceSet
        Inherits ResourceSet

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="basename">
        ''' The basename of the paxiom language file containing the sentences for the language
        ''' </param>
        ''' <param name="culture">
        ''' The culture of the language
        ''' </param>
        ''' <remarks></remarks>
        Public Sub New(ByVal basename As String, ByVal culture As CultureInfo)
            MyBase.New(New PxResourceReader(basename, culture))
        End Sub

        ''' <summary>
        ''' Get a reader for the paxion language files
        ''' </summary>
        ''' <returns>
        ''' The reader for the paxiom language files
        ''' </returns>
        ''' <remarks></remarks>
        Public Overrides Function GetDefaultReader() As Type
            Return GetType(PxResourceReader)
        End Function

        ''' <summary>
        ''' Loads a language into the resourceset.
        ''' </summary>
        ''' <param name="language">The language to be loaded</param>
        ''' <returns>True if the language were succesfully loaded otherwise False.</returns>
        ''' <remarks>
        ''' This function is used to explicitly load a language into the resourceset.
        ''' Existing entries in the resourceset will be replaced and new ones will be added.
        ''' </remarks>
        Public Function LoadLanguage(ByVal language As Language) As Boolean
            Try
                For Each sentence As Sentence In language
                    If Me.Table.Contains(sentence.Name) Then
                        Me.Table(sentence.Name) = sentence.Value
                    Else
                        Me.Table.Add(sentence.Name, sentence.Value)
                    End If
                Next

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class
End Namespace
