Imports System.Resources
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Namespace PCAxis.Paxiom.Localization

    ''' <summary>
    ''' Manager for the paxiom language files
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PxResourceManager
        Inherits ResourceManager

        Private Shared _resourcemanagers As ConcurrentDictionary(Of String, PxResourceManager)
        Private _basename As String = ""
        Private _resources As Hashtable

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="basename">The name of the xml-file to use as the fallback language</param>
        ''' <remarks>
        ''' Constructor is protected because PxResourceManager is implemented with the 
        ''' Singelton design pattern
        ''' </remarks>
        Protected Sub New(ByVal basename As String)
            _basename = basename
            'ResourceSets = New Hashtable()
            _resources = New Hashtable()
        End Sub

        ''' <summary>
        ''' Gets the resourcemanager for the given basename
        ''' </summary>
        ''' <param name="basename">The basename (filename) for the desired resourcemanager</param>
        ''' <returns>The resourcemanager for the given basename</returns>
        ''' <remarks></remarks>
        Public Shared Function GetResourceManager(ByVal basename As String) As PxResourceManager
            Dim resman As PxResourceManager = Nothing

            If _resourcemanagers Is Nothing Then
                _resourcemanagers = New ConcurrentDictionary(Of String, PxResourceManager)
            End If

            If _resourcemanagers.ContainsKey(basename) Then
                resman = _resourcemanagers.Item(basename)
            Else
                resman = New PxResourceManager(basename)
                _resourcemanagers.GetOrAdd(basename, resman)
            End If

            Return resman
        End Function

        ''' <summary>
        ''' Gets the default resourcemanager 
        ''' </summary>      
        ''' <returns>The default resourcemanager</returns>
        ''' <remarks></remarks>
        Public Shared Function GetResourceManager() As PxResourceManager
            Dim resman As PxResourceManager = Nothing
            Dim basename As String = Configuration.ConfigurationHelper.LocalizationSection.BaseFile

            If _resourcemanagers Is Nothing Then
                _resourcemanagers = New ConcurrentDictionary(Of String, PxResourceManager)
            End If

            If _resourcemanagers.ContainsKey(basename) Then
                resman = _resourcemanagers.Item(basename)
            Else
                resman = New PxResourceManager(basename)
                _resourcemanagers.GetOrAdd(basename, resman)
            End If

            Return resman
        End Function

        ''' <summary>
        ''' Removes the default resourcemanager and adds it again. 
        ''' New or changed sentences will then be accessible.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ResetResourceManager()
            Dim basename As String = Configuration.ConfigurationHelper.LocalizationSection.BaseFile
            Dim resman As PxResourceManager = Nothing

            If _resourcemanagers Is Nothing Then
                _resourcemanagers = New ConcurrentDictionary(Of String, PxResourceManager)
            End If

            If _resourcemanagers.ContainsKey(basename) Then
                resman = New PxResourceManager(basename)
                _resourcemanagers.TryRemove(basename, resman)
            End If

            resman = New PxResourceManager(basename)
            _resourcemanagers.GetOrAdd(basename, resman)
        End Sub

        'Protected Overrides Function InternalGetResourceSet(ByVal culture As CultureInfo, _
        '                                                    ByVal createIfNotExists As Boolean, _
        '                                                    ByVal tryParents As Boolean) As ResourceSet
        '    Dim rs As PxResourceSet = Nothing

        '    If ResourceSets.Contains(culture.Name) Then
        '        rs = CType(ResourceSets(culture.Name), PxResourceSet)
        '    Else
        '        rs = New PxResourceSet(_basename, culture)
        '        ResourceSets.Add(culture.Name, rs)
        '    End If

        '    Return rs
        'End Function
        Protected Overrides Function InternalGetResourceSet(ByVal culture As CultureInfo, _
                                                            ByVal createIfNotExists As Boolean, _
                                                            ByVal tryParents As Boolean) As ResourceSet
            Dim rs As PxResourceSet = Nothing

            If _resources.Contains(culture.Name) Then
                rs = CType(_resources(culture.Name), PxResourceSet)
            Else
                SyncLock _resources
                    If Not _resources.Contains(culture.Name) Then
                        rs = New PxResourceSet(_basename, culture)
                        _resources.Add(culture.Name, rs)
                    Else
                        rs = CType(_resources(culture.Name), PxResourceSet)
                    End If
                End SyncLock
                
            End If

            Return rs
        End Function


        ''' <summary>
        ''' Loads a language into the resource manager.
        ''' </summary>
        ''' <param name="language">The language to be loaded</param>
        ''' <returns>True if the language were succesfully loaded otherwise False.</returns>
        ''' <remarks>This function is used to explicitly load a language into the resource manager.</remarks>
        Public Function LoadLanguage(ByVal language As Language) As Boolean
            Try
                Dim culture As New CultureInfo(language.Name)
                Dim rs As PxResourceSet = Nothing

                rs = CType(InternalGetResourceSet(culture, True, True), PxResourceSet)
                rs.LoadLanguage(language)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Returns a sentece
        ''' </summary>
        ''' <param name="name">
        ''' Key for the sentence to return
        ''' </param>
        ''' <returns>
        ''' The sentence associated with the name parameter in the language defined by the 
        ''' current thread.
        ''' </returns>
        ''' <remarks>
        ''' If the sentence is not defined in the language of the current thread the
        ''' fallback sentence is returned.
        ''' </remarks>
        Public Overrides Function GetString( _
            ByVal name As String _
        ) As String

            Dim value As String = ""

            value = MyBase.GetString(name)


            If String.IsNullOrEmpty(value) Then
                Return name
            Else
                Return value
            End If
        End Function

        ''' <summary>
        ''' Returns a sentence
        ''' </summary>
        ''' <param name="name">
        ''' Key for the sentence to return
        ''' </param>
        ''' <param name="culture">
        ''' Return the sentence in the language defined by culture
        ''' </param>
        ''' <returns>
        ''' The sentence associated with the name parameter in the language defined by
        ''' the culture parameter
        ''' </returns>
        ''' <remarks>
        ''' If the sentence is not defined in the language given by culture the fallback
        ''' sentence. If the sentence is not the fallback language either the key value
        ''' is returned.
        ''' </remarks>
        Public Overrides Function GetString(ByVal name As String, ByVal culture As CultureInfo) As String

            Dim value As String = ""

            value = MyBase.GetString(name, culture)

            If String.IsNullOrEmpty(value) Then
                Return name
            Else
                Return value
            End If
        End Function

        ''' <summary>
        ''' Returns a sentence
        ''' </summary>
        ''' <param name="name">Key for the sentence to return</param>
        ''' <param name="lang">Return the sentence in the language defined by lang</param>
        ''' <returns>
        ''' The sentence associated with the name parameter in the language defined by
        ''' the culture parameter
        ''' </returns>
        ''' <remarks>
        ''' This overload of GetString hides the creation of CultureInfo needed to get the sentence.
        ''' If lang has a value of a not supported language the default language is used.
        ''' </remarks>
        Public Overloads Function GetString(ByVal name As String, ByVal lang As String) As String
            Dim ci As System.Globalization.CultureInfo

            If String.IsNullOrEmpty(name) Then
                Return String.Empty
            End If

            If String.IsNullOrEmpty(lang) Then
                Return GetString(name)
            End If

            Try
                ci = New System.Globalization.CultureInfo(lang)
            Catch ex As Exception
                ci = Nothing
            End Try

            If Not ci Is Nothing Then
                Return GetString(name, ci)
            Else
                Return GetString(name)
            End If
        End Function

    End Class
End Namespace
