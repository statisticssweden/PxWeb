Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Class for handling the Relationtable file format (dBase rel)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RelationtableFileSerializer
        Implements PCAxis.Paxiom.IPXModelStreamSerializer

        ''' <summary>
        ''' Implementation of IPXModelStreamSerializer function
        ''' </summary>
        ''' <param name="model"></param>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal path As String) Implements IPXModelStreamSerializer.Serialize
            Dim csvFileSerializer As New CsvFileSerializer()
            Dim preparedModel As PXModel

            preparedModel = PrepareModel(model)

            SetupCSVSerializer(csvFileSerializer)

            csvFileSerializer.Serialize(preparedModel, path)
        End Sub

        ''' <summary>
        ''' Implementation of IPXModelStreamSerializer function
        ''' </summary>
        ''' <param name="model"></param>
        ''' <param name="stream"></param>
        ''' <remarks></remarks>
        Public Sub Serialize(ByVal model As PXModel, ByVal stream As System.IO.Stream) Implements IPXModelStreamSerializer.Serialize
            Dim csvFileSerializer As New CsvFileSerializer()
            Dim preparedModel As PXModel

            preparedModel = PrepareModel(model)

            SetupCSVSerializer(csvFileSerializer)

            csvFileSerializer.Serialize(preparedModel, stream)
        End Sub

#Region "Private functionality"
        ''' <summary>
        ''' Setup the csv serializer with correct property values for the dBase rel export format
        ''' </summary>
        ''' <param name="csvFileSerializer"></param>
        ''' <remarks></remarks>
        Private Sub SetupCSVSerializer(ByVal csvFileSerializer As PCAxis.Paxiom.CsvFileSerializer)
            csvFileSerializer.Delimiter = CChar(vbTab)
            csvFileSerializer.DoubleColumn = False
            csvFileSerializer.WrapTextWithQuote = False
            csvFileSerializer.ThousandSeparator = False
        End Sub
        ''' <summary>
        ''' Prepare a model for export by pivoting the model to have all variables in the Stub
        ''' </summary>
        ''' <param name="model"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function PrepareModel(ByVal model As PCAxis.Paxiom.PXModel) As PCAxis.Paxiom.PXModel
            Dim pivotOperation As New PCAxis.Paxiom.Operations.Pivot()
            Dim pivotDescriptions As PCAxis.Paxiom.Operations.PivotDescription()
            Dim pv As PCAxis.Paxiom.Operations.PivotDescription
            Dim preparedModel As PCAxis.Paxiom.PXModel

            pivotDescriptions = New PCAxis.Paxiom.Operations.PivotDescription(model.Meta.Variables.Count - 1) {}

            REM Loop Stub and Heading variables and add to pivotDescription
            REM All variables are placed in the Stub to match the export format to use.
            Dim pivotDescriptionIndex As Integer = -1
            For i As Integer = 0 To model.Meta.Stub.Count - 1
                pivotDescriptionIndex += 1
                pv = New PCAxis.Paxiom.Operations.PivotDescription()
                pv.VariableName = model.Meta.Stub(i).Name
                pv.VariablePlacement = PlacementType.Stub
                pivotDescriptions(pivotDescriptionIndex) = pv
            Next
            For i As Integer = 0 To model.Meta.Heading.Count - 1
                pivotDescriptionIndex += 1
                pv = New PCAxis.Paxiom.Operations.PivotDescription()
                pv.VariableName = model.Meta.Heading(i).Name
                pv.VariablePlacement = PlacementType.Stub ' Place all variables in stub
                pivotDescriptions(pivotDescriptionIndex) = pv
            Next

            preparedModel = pivotOperation.Execute(model, pivotDescriptions)

            ' Also ensure that rounding is in the std format
            preparedModel.Meta.Rounding = RoundingType.BankersRounding

            Return preparedModel
        End Function
#End Region
    End Class
End Namespace