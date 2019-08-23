Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Interface which presist a PXModel to a data source
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPXModelStreamSerializer
        
        ''' <summary>
        ''' Persist the model to the data storage
        ''' </summary>
        ''' <param name="model">model to persist</param>
        ''' <param name="path">path to det data store</param>
        ''' <remarks></remarks>
        Sub Serialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal path As String)

        ''' <summary>
        ''' Persist the model to a stream
        ''' </summary>
        ''' <param name="model">model to persist</param>
        ''' <param name="stream">stream that the model will be persisted to</param>
        ''' <remarks></remarks>
        Sub Serialize(ByVal model As PCAxis.Paxiom.PXModel, ByVal stream As System.IO.Stream)
    End Interface
End Namespace
