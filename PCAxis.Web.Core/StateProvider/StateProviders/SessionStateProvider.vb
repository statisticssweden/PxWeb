Imports System.Web
Namespace StateProvider.StateProviders

    ''' <summary>
    ''' Used to store state in session
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SessionStateProvider
        Inherits StaterProviderBase

        ''' <summary>
        ''' Adds an item to session
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to add</param>
        ''' <param name="data">The item to add</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnAdd(ByVal fullName As String, ByVal data As Object)
            If HttpContext.Current IsNot Nothing And HttpContext.Current.Session IsNot Nothing Then
                HttpContext.Current.Session.Add(fullName, data)
            End If
        End Sub

        ''' <summary>
        ''' Gets whether an item exists in session
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to look for</param>
        ''' <returns><c>True</c> if the session contains the item, otherwise <c>False</c></returns>
        ''' <remarks></remarks>
        Protected Overrides Function OnContains(ByVal fullName As String) As Boolean
            If HttpContext.Current IsNot Nothing And HttpContext.Current.Session IsNot Nothing Then
                If HttpContext.Current.Session.Item(fullName) IsNot Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Gets an item from session
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to get</param>        
        ''' <returns>An item of type <see cref="Object" /></returns>
        ''' <remarks></remarks>
        Protected Overrides Function OnItemGet(ByVal fullName As String) As Object
            If HttpContext.Current IsNot Nothing Then
                Return HttpContext.Current.Session(fullName)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Sets an item in session
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to set</param>
        ''' <param name="data">The item to set</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnItemSet(ByVal fullName As String, ByVal data As Object)
            If HttpContext.Current IsNot Nothing Then
                HttpContext.Current.Session(fullName) = data
            End If
        End Sub

        ''' <summary>
        ''' Removes an item from session
        ''' </summary>
        ''' <param name="fullName">The unique name of the item to remove</param>        
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemove(ByVal fullName As String)
            If HttpContext.Current IsNot Nothing Then
                HttpContext.Current.Session.Remove(fullName)
            End If
        End Sub

        ''' <summary>
        ''' Unloads the session state provider
        ''' </summary>
        ''' <param name="reason">The <see cref="Enums.StateProviderUnloadReason" /> for unloading the state</param>
        ''' <remarks>Does nothing</remarks>
        Protected Overrides Sub OnUnLoad(ByVal reason As Enums.StateProviderUnloadReason)
            If reason = Enums.StateProviderUnloadReason.StateTimeout Then
            End If
        End Sub

        ''' <summary>
        ''' Load the session stateprovider
        ''' </summary>
        ''' <remarks>Does nothing</remarks>
        Protected Overrides Sub OnLoad()

        End Sub
    End Class
End Namespace
