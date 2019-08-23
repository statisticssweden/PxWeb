Namespace PCAxis.Paxiom

    ''' <summary>
    ''' List of cell notes
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class CellNotes
        Inherits List(Of CellNote)
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
        ''' <returns> Create deep copy of me</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As CellNotes
            Dim newObject As New CellNotes

            For Each cn As CellNote In Me
                newObject.Add(cn.CreateCopy())
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
            Dim note As CellNote
            Dim c As Integer
            c = info.GetInt32("NoOfCellNotes")

            For i As Integer = 1 To c
                note = CType(info.GetValue("CellNote" & i, GetType(CellNote)), CellNote)
            Next

        End Sub

        ''' <summary>
        ''' Custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer
            info.AddValue("NoOfCellNotes", c)
            For i As Integer = 1 To c
                info.AddValue("CellNote" & i, Me(i - 1))
            Next
        End Sub

    End Class
End Namespace

