Namespace PCAxis.Paxiom

    ''' <summary>
    ''' List for storing extended properties
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class ExtendedPropertiesList
        Inherits SerializableDictionary(Of String, String)
        Implements System.Runtime.Serialization.ISerializable

        ''' <summary>
        ''' Empty Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create a deep copy of me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As ExtendedPropertiesList
            Dim newObject As New ExtendedPropertiesList

            For Each kvp As KeyValuePair(Of String, String) In Me
                newObject.Add(kvp.Key, kvp.Value)
            Next

            Return newObject
        End Function

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim c As Integer
            c = CType(info.GetValue("NoOfProperties", GetType(Integer)), Integer)
            If c > 0 Then
                Dim kd As New Dictionary(Of Integer, String)
                Dim key As String
                Dim value As String

                'Reads all keys
                For i As Integer = 1 To c
                    key = CType(info.GetValue("Key" & i, GetType(String)), String)
                    Me.Add(key, Nothing)
                    kd.Add(i, key)
                Next

                'reads all values
                For i As Integer = 1 To c
                    value = CType(info.GetValue("Value" & i, GetType(String)), String)
                    Me(kd(i)) = value
                Next
            End If

        End Sub

        ''' <summary>
        ''' Serialization functionality
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Shadows Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim c As Integer
            c = Me.Count
            info.AddValue("NoOfProperties", c)
            Dim i As Integer
            i = 0
            For Each key As String In Me.Keys
                i += 1
                info.AddValue("Key" & i, key)
            Next

            i = 0
            For Each key As String In Me.Keys
                i += 1
                info.AddValue("Value" & i, Me(key))
            Next
        End Sub
    End Class
End Namespace