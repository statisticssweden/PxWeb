Namespace PCAxis.Paxiom

    ''' <summary>
    ''' List of variable and value pairs
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class VariableValuePairs
        Inherits List(Of VariableValuePair)
        Implements System.Runtime.Serialization.ISerializable

        ''' <summary>
        ''' Looks for a VariableValuePair with the specified variable code
        ''' </summary>
        ''' <param name="variableCode">the variable code</param>
        ''' <returns>
        ''' The first instance of a VariableValuePair having the 
        ''' variable code variableCode
        ''' </returns>
        ''' <remarks>
        ''' If no VariableValuePair whit the given variable code then function
        ''' will return Nothing/null
        ''' </remarks>
        Public Function FindByVariableCode(ByVal variableCode As String) As VariableValuePair
            For i As Integer = 0 To Me.Count - 1
                If Item(i).VariableCode = variableCode Then
                    Return Item(i)
                End If
            Next

            Return Nothing
        End Function

        Public Sub New()

        End Sub

        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim pair As VariableValuePair
            Dim c As Integer
            c = info.GetInt32("NoOfPairs")
            For i As Integer = 1 To c
                pair = CType(info.GetValue("Pair" & i, GetType(VariableValuePair)), VariableValuePair)
                Me.Add(pair)
            Next
        End Sub

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer
            c = Me.Count
            info.AddValue("NoOfPairs", c)
            For i As Integer = 1 To c
                info.AddValue("Pair" & i, Me(i - 1))
            Next
        End Sub

        ''' <summary>
        ''' Create a Deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As VariableValuePairs
            Dim newObject As New VariableValuePairs()

            For i As Integer = 0 To Me.Count - 1
                newObject.Add(Me(i).CreateCopy())
            Next

            Return newObject
        End Function

        ''' <summary>
        ''' Class for comparing equality between VariableValuePairs-objects
        ''' </summary>
        ''' <remarks></remarks>
        Public Class EqualityComparer
            Implements IEqualityComparer(Of VariableValuePairs)

            Public Overloads Function Equals(ByVal x As VariableValuePairs, ByVal y As VariableValuePairs) As Boolean Implements IEqualityComparer(Of VariableValuePairs).Equals
                Dim found As Boolean

                If x.Count <> y.Count Then
                    Return False
                End If

                For i As Integer = 0 To x.Count - 1
                    found = False

                    For j As Integer = 0 To y.Count - 1
                        If x.Item(i).VariableCode.Equals(y.Item(j).VariableCode) And x.Item(i).ValueCode.Equals(y.Item(j).ValueCode) Then
                            found = True
                            Exit For
                        End If
                    Next

                    If Not found Then
                        Return False
                    End If
                Next

                Return True
            End Function

            Public Overloads Function GetHashCode(ByVal obj As VariableValuePairs) As Integer Implements IEqualityComparer(Of VariableValuePairs).GetHashCode
                Dim hCode As Integer = 1

                'Some algorithm to create a bucket as unique as possible...
                For Each x As VariableValuePair In obj
                    hCode = hCode * x.VariableCode.Length * x.ValueCode.Length
                Next
                Return hCode.GetHashCode
            End Function
        End Class

    End Class

End Namespace
