''' <summary>
''' Class for setting properties from Px Model into PxActionEventArgs
''' </summary>
''' <remarks></remarks>
Public Class PxActionEventHelper

    ''' <summary>
    ''' Set PxActionEvent arguments from PX model
    ''' </summary>
    ''' <param name="args">PxActionEventArgs object</param>
    ''' <param name="model">PxModel object</param>
    ''' <remarks></remarks>
    Public Shared Sub SetModelProperties(ByVal args As PxActionEventArgs, ByVal model As PCAxis.Paxiom.PXModel)
        If args Is Nothing Or model Is Nothing Then
            Exit Sub
        End If

        args.TableId = model.Meta.MainTable
        args.NumberOfCells = model.Data.MatrixSize
        If Not model.Meta.ContentVariable Is Nothing Then
            args.NumberOfContents = model.Meta.ContentVariable.Values.Count
        Else
            args.NumberOfContents = 1
        End If
    End Sub

End Class
