Namespace PCAxis.Paxiom

    Public Class DataIndexer

        Private _pmStub() As Integer
        Private _pmHeading() As Integer
        Private _weightHeading() As Integer
        Private _weightStub() As Integer
        Private _meta As PXMeta


        Public ReadOnly Property StubIndecies() As Integer()
            Get
                Return _weightStub
            End Get
        End Property

        Public ReadOnly Property HeadingIndecies() As Integer()
            Get
                Return _weightHeading
            End Get
        End Property

        Sub New(ByVal meta As PXMeta)
            _meta = meta
            If meta.Heading.Count = 0 Then
                _weightHeading = Nothing
            Else
                _weightHeading = New Integer(meta.Heading.Count - 1) {}
            End If
            If meta.Stub.Count = 0 Then
                _weightStub = Nothing
            Else
                _weightStub = New Integer(meta.Stub.Count - 1) {}
            End If
            _pmStub = CreatePMatrix(meta.Stub)
            _pmHeading = CreatePMatrix(meta.Heading)
        End Sub


        Public Sub SetContext(ByVal rowIndex As Integer, ByVal columnIndex As Integer)
            CalcHeadingWeights(columnIndex, _meta.Heading)
            CalcStubWeights(rowIndex, _meta.Stub)
        End Sub

        Private Function CreatePMatrix(ByVal variables As Variables) As Integer()
            Dim size As Integer = variables.Count

            Dim pMatrix As Integer()
            'Check that there is at least one variable in the Heading
            If size > 0 Then
                pMatrix = New Integer(size) {}

                For index As Integer = 0 To size - 1
                    pMatrix(index) = 1
                    For j As Integer = index To size - 1
                        pMatrix(index) *= variables(j).Values.Count
                    Next
                Next
                pMatrix(size) = 1
            Else
                pMatrix = Nothing
            End If

            Return pMatrix
        End Function

        Private Sub CalcHeadingWeights(ByVal columnIndex As Integer, ByVal headingVariables As Variables)
            If _weightHeading Is Nothing Then
                Exit Sub
            End If

            Dim size As Integer = _weightHeading.Length

            For index As Integer = 0 To size - 1
                _weightHeading(index) = Convert.ToInt32(Math.Floor((columnIndex / _pmHeading(index + 1)))) Mod headingVariables(index).Values.Count
            Next
        End Sub

        Private Sub CalcStubWeights(ByVal rowIndex As Integer, ByVal stubVariables As Variables)
            If _weightStub Is Nothing Then
                Exit Sub
            End If

            Dim size As Integer = _weightStub.Length

            For index As Integer = 0 To size - 1
                _weightStub(index) = Convert.ToInt32(Math.Floor(rowIndex / _pmStub(index + 1))) Mod stubVariables(index).Values.Count
            Next
        End Sub

    End Class

End Namespace
