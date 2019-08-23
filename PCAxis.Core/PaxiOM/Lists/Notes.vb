Namespace PCAxis.Paxiom

    <Serializable()> _
    Public Class Notes
        Inherits List(Of Note)
        Implements System.Runtime.Serialization.ISerializable

        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim c As Integer
            Dim n As Note
            c = info.GetInt32("NoOfNotes")
            For i As Integer = 1 To c
                n = CType(info.GetValue("Note" & c, GetType(Note)), Note)
                Me.Add(n)
            Next
        End Sub

        ''' <summary>
        ''' Functionality used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer = Me.Count
            info.AddValue("NoOfNotes", c)

            For i As Integer = 1 To c
                info.AddValue("Note" & i, Me(i - 1))
            Next
        End Sub

        ''' <summary>
        ''' Get info from all Notes in Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllNotes() As String
            If Me.Count > 0 Then
                Dim builder As New System.Text.StringBuilder

                For i As Integer = 0 To Me.Count - 1
                    builder.Append(Me(i).Text)
                    builder.Append(ControlChars.NewLine)
                Next

                Return builder.ToString()
            End If

            Return String.Empty
        End Function


        ''' <summary>
        ''' Get information from all mandatory Notes in Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllMandantoryNotes() As String
            If Me.Count > 0 Then
                Dim builder As New System.Text.StringBuilder

                For i As Integer = 0 To Me.Count - 1
                    If Me(i).Mandantory Then
                        builder.Append(Me(i).Text)
                        builder.Append(ControlChars.NewLine)
                    End If
                Next

                Return builder.ToString()
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Notes
            Dim newObject As New Notes

            For Each note As Note In Me
                newObject.Add(note.CreateCopy())
            Next

            Return newObject
        End Function

        Public Function GetMandatoryNotesString(ByVal separator As String) As String
            Dim builder As New System.Text.StringBuilder
            Dim first As Boolean = True

            For i As Integer = 0 To Me.Count - 1
                If Me(i).Mandantory Then
                    If Not first Then
                        builder.Append(separator)
                    Else
                        first = False
                    End If
                    builder.Append(Me(i).Text)
                End If
            Next

            Return builder.ToString()

        End Function

        Public Function GetNonMandatoryNotesString(ByVal separator As String) As String
            Dim builder As New System.Text.StringBuilder
            Dim first As Boolean = True

            For i As Integer = 0 To Me.Count - 1
                If Not Me(i).Mandantory Then
                    If Not first Then
                        builder.Append(separator)
                    Else
                        first = False
                    End If
                    builder.Append(Me(i).Text)
                End If
            Next

            Return builder.ToString()
        End Function

    End Class

End Namespace
