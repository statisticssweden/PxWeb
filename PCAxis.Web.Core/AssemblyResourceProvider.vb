Imports System.Web
Imports System.Web.Hosting
Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Used to reroute virtual paths to assembly resources
''' </summary>
''' <remarks>Reroutes calls to ~/PCAxis_Web_Controls/ to return emedded resources
''' Uses the format of ~/PCAxis_Web_Controls/AssemblyName.dll/EmbeddedResourceName
''' </remarks>
Public Class AssemblyResourceProvider
    Inherits System.Web.Hosting.VirtualPathProvider


    Public Sub New()

    End Sub

    ''' <summary>
    ''' Gets whether a virtual path contains ~/PCAxis_Web_Controls/
    ''' </summary>
    ''' <param name="virtualPath">The virtual path to check</param>
    ''' <returns><c>True</c> if the path contains ~/PCAxis_Web_Controls/, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    Private Function IsAppResourcePath(ByVal virtualPath As String) As Boolean

        Dim checkPath As String = VirtualPathUtility.ToAppRelative(virtualPath)
        Return checkPath.StartsWith("~/PCAxis_Web_Controls/", StringComparison.InvariantCultureIgnoreCase)
    End Function

    ''' <summary>
    ''' Gets whether a file exists or not at the given virtual path
    ''' </summary>
    ''' <param name="virtualPath">The virtual path to the file to check for</param>
    ''' <returns><c>True</c> if the files exists, otherwise <c>False</c></returns>
    ''' <remarks></remarks>
    Public Overrides Function FileExists(ByVal virtualPath As String) As Boolean
        'This is overridden from the baseclass so if the IsAppResourcePath returns false
        'maybe the baseclass won't so we need to check both
        Return (IsAppResourcePath(virtualPath) OrElse MyBase.FileExists(virtualPath))
    End Function

    ''' <summary>
    ''' Gets a file from the given virtual path
    ''' </summary>
    ''' <param name="virtualPath">The virtual path to the file to get</param>
    ''' <returns>An new instance of <see cref="VirtualFile" /></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFile(ByVal virtualPath As String) As VirtualFile

        'Check if the virtual path should be rerouted
        If (IsAppResourcePath(virtualPath)) Then            
            Return New AssemblyResourceVirtualFile(virtualPath)
        Else
            Return MyBase.GetFile(virtualPath)
        End If
    End Function

    ''' <summary>
    ''' Creates a cache dependency based on the specified virtual paths
    ''' </summary>
    ''' <param name="virtualPath">The virtual path to create a cache dependency for</param>
    ''' <param name="virtualPathDependencies">An array of virtual paths required by the primary virtual path</param>
    ''' <param name="utcStart">The UTC time at which the virtual path was requested</param>
    ''' <returns>A <see cref="Caching.CacheDependency" /> object for the specified virtual path</returns>
    ''' <remarks></remarks>
    Public Overrides Function GetCacheDependency(ByVal virtualPath As String, ByVal virtualPathDependencies As System.Collections.IEnumerable, ByVal utcStart As DateTime) As System.Web.Caching.CacheDependency

        If (IsAppResourcePath(virtualPath)) Then
            'Can't return a cache dependency for a embedded resource
            Return Nothing
        Else
            Return MyBase.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart)
        End If
    End Function
End Class

''' <summary>
''' Represents an embedded resource as an <see cref="VirtualFile" />
''' </summary>
''' <remarks></remarks>
Public Class AssemblyResourceVirtualFile
    Inherits VirtualFile

    Private _path As String

    ''' <summary>
    ''' Initializes a new instance of <see cref=" AssemblyResourceVirtualFile" />
    ''' </summary>
    ''' <param name="virtualPath">The virtual path to the embedded resource</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal virtualPath As String)
        MyBase.New(virtualPath)
        _path = VirtualPathUtility.ToAppRelative(virtualPath)
    End Sub

    ''' <summary>
    ''' Gets a read-only stream to the embedded resource
    ''' </summary>
    ''' <returns>A read-only stream to the embedded resource</returns>
    ''' <remarks></remarks>
    Public Overrides Function Open() As System.IO.Stream
        'Split the virtual path into it's components
        Dim parts As String() = _path.Split(CChar("/"))
        Dim assemblyName As String = parts(2)
        Dim resourceName As String = parts(3)

        'Create a fullpath to the assembly that contains the embedded resource
        Dim assemblyFullName As String = Path.Combine(HttpRuntime.BinDirectory, assemblyName)

        'Load the assembly
        Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFile(assemblyFullName)

        If (assembly IsNot Nothing) Then
            'Return the embedded resource as a stream
            Return assembly.GetManifestResourceStream(resourceName)
        End If
        Return Nothing
    End Function


End Class
