Namespace StateProvider.StateProviders
    'Public Class FileStateProvider
    '    Implements IStateProvider


    '    Private _uniqueId As String
    '    Private _path As String
    '    Private _states As Dictionary(Of String, Object) = Nothing

    '    Public Sub Add(ByVal name As String, ByVal data As Object) Implements IStateProvider.Add
    '        If _states.ContainsKey(name) Then
    '            _states(name) = data
    '        Else
    '            _states.Add(name, data)
    '        End If

    '    End Sub

    '    Public Function Contains(ByVal name As String) As Boolean Implements IStateProvider.Contains
    '        _states.ContainsKey(name)
    '    End Function

    '    Public Property Item(ByVal name As String) As Object Implements IStateProvider.Item
    '        Get
    '            If _states.ContainsKey(name) Then
    '                Return _states(name)
    '            Else
    '                Return Nothing
    '            End If
    '        End Get
    '        Set(ByVal value As Object)
    '            _states(name) = value
    '        End Set
    '    End Property

    '    Public Sub Remove(ByVal name As String) Implements IStateProvider.Remove
    '        _states.Remove(name)
    '    End Sub

    '    Public Sub Load(ByVal uniqueId As String) Implements IStateProvider.Load
    '        _uniqueId = uniqueId
    '        _path = System.Web.Hosting.HostingEnvironment.MapPath(Configuration.ConfigurationHelper.GeneralSettingsSection.CacheVirtualPath)
    '        _path = IO.Path.Combine(_path, uniqueId + ".tmp")
    '        If IO.File.Exists(_path) Then
    '            Dim formater As Runtime.Serialization.Formatters.Binary.BinaryFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
    '            Using fs As IO.FileStream = IO.File.Open(_path, IO.FileMode.Open)
    '                _states = CType(formater.Deserialize(fs), Dictionary(Of String, Object))
    '            End Using
    '        Else
    '            _states = New Dictionary(Of String, Object)
    '        End If

    '    End Sub

    '    Public Sub Unload(ByVal reason As Enums.StateProviderUnloadReason) Implements IStateProvider.Unload
    '        Select Case reason
    '            Case Enums.StateProviderUnloadReason.PageRequestEnded
    '                Dim formater As Runtime.Serialization.Formatters.Binary.BinaryFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
    '                Using fs As IO.FileStream = IO.File.Open(_path, IO.FileMode.Create)
    '                    formater.Serialize(fs, _states)
    '                End Using

    '            Case Enums.StateProviderUnloadReason.StateTimeout
    '                If IO.File.Exists(_path) Then
    '                    IO.File.Delete(_path)
    '                End If
    '        End Select

    '    End Sub

    'End Class
End Namespace
