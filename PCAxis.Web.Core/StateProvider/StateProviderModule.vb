Imports System.Web
Namespace StateProvider
    Public Class StateProviderModule
        Implements IHttpModule

        Private WithEvents _context As HttpApplication

        ''' <summary>
        '''  You will need to configure this module in the web.config file of your
        '''  web and register it with IIS before being able to use it. For more information
        '''  see the following link: http://go.microsoft.com/?linkid=8101007
        ''' </summary>
#Region "IHttpModule Members"

        Public Sub Dispose() Implements IHttpModule.Dispose

            ' Clean-up code here

        End Sub

        Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
            _context = context
        End Sub

#End Region

        Private Sub _context_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs) Handles _context.BeginRequest
            If ExecutionCheck() Then
                'Register the stateprovider with the linkmanager
                StateProvider.StateProviderFactory.RegisterWithLinkManager()
            End If

        End Sub

        Private Sub _context_EndRequest(ByVal sender As Object, ByVal e As System.EventArgs) Handles _context.EndRequest
            If ExecutionCheck() Then
                'Unloads the stateprovider with the PageRequestEnded so any state that needs to be saved will be saved
                StateProviderFactory.GetStateProvider().Unload(Enums.StateProviderUnloadReason.PageRequestEnded)

                'Unregister the stateprovider with the linkmanager to avoid memory leaks because the LinkManager holds a reference to the stateprovider
                StateProviderFactory.UnregisterWithLinkManager()
            End If
        End Sub

        ''' <summary>
        ''' Check if the module shall run or not
        ''' </summary>
        ''' <returns>True if the module shall run, else false</returns>
        ''' <remarks></remarks>
        Private Function ExecutionCheck() As Boolean
            If StateProvider.StateProviderFactory.IsManagedHandler Then
                'The request is for a managed file (.aspx for example)
                Return True
            End If

            If _context.Request.Path.Contains(".aspx") Then
                Return True
            End If

            If _context.Request.Path.Contains(".") Then
                If _context.Request.Path.Contains(".px") Then
                    'User friendly URL for PX-file table
                    Return True
                Else
                    'Request for file that should not be handled by this module (.gif, .js, .css and so on...)
                    Return False
                End If
            End If

            'User friendly URL without file extension
            Return True
        End Function

    End Class
End Namespace
