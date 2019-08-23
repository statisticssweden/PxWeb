Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Class for holding cell data notes
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class DataNoteCells
        Inherits List(Of DataNoteCell)
        Implements System.Runtime.Serialization.ISerializable

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As DataNoteCells
            Dim newObject As New DataNoteCells

            For Each dnc As DataNoteCell In Me
                newObject.Add(dnc.CreateCopy())
            Next

            Return newObject
        End Function

        ''' <summary>
        ''' Constructor for custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim note As DataNoteCell
            Dim c As Integer
            c = info.GetInt32("NoOfDataNoteCells")

            For i As Integer = 1 To c
                note = CType(info.GetValue("DataNoteCell" & i, GetType(DataNoteCell)), DataNoteCell)
            Next

        End Sub

        ''' <summary>
        ''' Custom serializer
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer
            info.AddValue("NoOfDataNoteCells", c)
            For i As Integer = 1 To c
                info.AddValue("DataNoteCell" & i, Me(i - 1))
            Next
        End Sub
    End Class

End Namespace