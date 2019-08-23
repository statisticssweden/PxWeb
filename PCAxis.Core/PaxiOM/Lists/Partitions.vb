Namespace PCAxis.Paxiom
    <Serializable()> _
Public Class Partitions
        Inherits List(Of Partition)
        Implements System.Runtime.Serialization.ISerializable

        Public Function FindByValues(ByVal startIndex As Integer, ByVal length As Integer) As Partition
            For i As Integer = 0 To Me.Count - 1
                If Me.Item(i).StartIndex = startIndex And Me.Item(i).Length = length Then
                    Return Me.Item(i)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim p As Partition
            Dim c As Integer
            c = info.GetInt32("NoOfPartitions")
            For i As Integer = 1 To c
                p = CType(info.GetValue("Partition" & i, GetType(Partition)), Partition)
                Me.Add(p)
            Next
        End Sub

        ''' <summary>
        ''' Functionality used by Serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer = Me.Count
            info.AddValue("NoOfPartitions", c)
            For i As Integer = 1 To c
                info.AddValue("Partition" & i, Me(i - 1))
            Next
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Partitions
            Dim newObject As New Partitions

            For i As Integer = 0 To Me.Count - 1
                newObject.Add(Me(i).CreateCopy())
            Next

            Return newObject
        End Function
    End Class
End Namespace
