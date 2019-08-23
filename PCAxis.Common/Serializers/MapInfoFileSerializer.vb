Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Writes a PXModel to file or a stream in CSV format suitable for MapInfo.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MapInfoFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

#Region "Private fields"
        Private csvSerializer As CsvFileSerializer
        Private _originalDoubleColumn As Boolean
#End Region

#Region "Constructors"
        Public Sub New()
            csvSerializer = New CsvFileSerializer
            csvSerializer.Delimiter = ","c
            csvSerializer.DoubleColumn = True
            csvSerializer.Title = False
            csvSerializer.ThousandSeparator = False
        End Sub
#End Region

#Region "IPXModelStreamSerializer Interface members"
        ''' <summary>
        ''' Write a PXModel to a file.
        ''' </summary>
        ''' <param name="model">The PXModel to write.</param>
        ''' <param name="path">The complete file path to write to. <I>path</I> can be a file name.</param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize
            CheckModel(model)
            SetDoubleColumn(model)
            csvSerializer.Serialize(model, path)
            RestoreDoubleColumn(model)
        End Sub

        ''' <summary>
        ''' Write a PXModel to a stream.
        ''' </summary>
        ''' <param name="model">The PXModel to write.</param>
        ''' <param name="stream">The stream to write to.</param>
        ''' <remarks>The caller is responsible of disposing the stream.</remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal stream As System.IO.Stream) Implements IPXModelStreamSerializer.Serialize
            CheckModel(model)
            SetDoubleColumn(model)
            csvSerializer.Serialize(model, stream)
            RestoreDoubleColumn(model)
        End Sub

#End Region

        ''' <summary>
        ''' Verifies that it is possible to create a MapInfo-file from the model
        ''' </summary>
        ''' <param name="model">The PXModel to verify</param>
        ''' <remarks></remarks>
        Private Sub CheckModel(ByVal model As PXModel)
            If model Is Nothing Then Throw New ArgumentNullException("model")

            CheckMap(model)
        End Sub

        ''' <summary>
        ''' Verifies that the MAP-variable is ok
        ''' </summary>
        ''' <param name="model">The PXModel to verify</param>
        ''' <remarks>
        ''' Three requirements must be fulfilled:
        ''' 1. A MAP-variable must exist
        ''' 2. The MAP-variable must be in the stub
        ''' 3. The MAP-variable must be the only variable in the stub
        ''' </remarks>
        Private Sub CheckMap(ByVal model As PXModel)

            For Each var As Variable In model.Meta.Variables
                '1. Verify that a map-variable exists
                If Not String.IsNullOrEmpty(var.Map) Then

                    '2. Verify that the stub only contains one variable
                    If model.Meta.Stub.Count > 1 Then
                        'TODO: Errorcode
                        Throw New PXSerializationException("Export to MapInfo-file requires that the map-variable is the only stubvariable", "")
                    End If

                    '3. Verify that map-variable is the variable in the stub
                    If Not model.Meta.Stub(0).Equals(var) Then
                        'TODO: Errorcode
                        Throw New PXSerializationException("Export to MapInfo-file requires that the map-variable is the only stubvariable", "")
                    End If

                    'Map ok!
                    Exit Sub
                End If
            Next

            'TODO: Errorcode
            Throw New PXSerializationException("Export to MapInfo-file requires a map-variable", "")
        End Sub

        ''' <summary>
        ''' The MapInfo file format requires that the mapvariable (the only stubvariable) is doublecolumn.
        ''' This method explicitly sets doublecolumn = true for this variable.
        ''' </summary>
        ''' <param name="model">The PX-model</param>
        ''' <remarks></remarks>
        Private Sub SetDoubleColumn(ByVal model As PXModel)
            If model.Meta.Stub.Count <> 1 Then
                Throw New PXSerializationException("Export to MapInfo-file requires that there are only one stubvariable", "")
            End If

            _originalDoubleColumn = model.Meta.Stub(0).DoubleColumn

            If model.Meta.Stub(0).Values.Count = 0 Then
                Throw New PXSerializationException("Export to MapInfo-file requires that there are at least one value for the stubvariable", "")
            End If

            If Not model.Meta.Stub(0).Values(0).HasCode Then
                Throw New PXSerializationException("Export to MapInfo-file requires that the values for the stubvariable has codes", "")
            End If

            If Not model.Meta.Stub(0).Values.ValuesHaveCodes Then
                Throw New PXSerializationException("Export to MapInfo-file requires that the values for the stubvariable has codes", "")
            End If

            model.Meta.Stub(0).DoubleColumn = True
        End Sub

        ''' <summary>
        ''' When exporting to MapInfo file format doublecolumn is explicitly set to true in the model
        ''' for the stubvariable.
        ''' This metod restores the doublecolumn value for the stubvariable after export.
        ''' </summary>
        ''' <param name="model">The PX-model</param>
        ''' <remarks></remarks>
        Private Sub RestoreDoubleColumn(ByVal model As PXModel)
            model.Meta.Stub(0).DoubleColumn = _originalDoubleColumn
        End Sub
    End Class

End Namespace
