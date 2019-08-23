Namespace PCAxis.Paxiom
    <Serializable()> _
Public Class HierarchyLevels
        Inherits List(Of PCAxis.Paxiom.HierarchyLevel)
        Implements System.Runtime.Serialization.ISerializable

        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim h As HierarchyLevel
            Dim c As Integer
            c = info.GetInt32("NoOfHierarchies")
            For i As Integer = 1 To c
                h = CType(info.GetValue("HierarchLevel" & i, GetType(HierarchyLevel)), HierarchyLevel)
                Me.Add(h)
            Next
        End Sub

        ''' <summary>
        ''' Add object data to SerializationInfo
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer
            c = Me.Count
            info.AddValue("NoOfHierarchies", c)
            For i As Integer = 1 To c
                info.AddValue("HierarchLevel" & i, Me(i - 1))
            Next
        End Sub

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As HierarchyLevels
            Dim newObject As New HierarchyLevels

            For Each hl As HierarchyLevel In Me
                newObject.Add(hl.createcopy())
            Next

            Return newObject
        End Function
    End Class

End Namespace