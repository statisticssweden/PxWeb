Namespace PCAxis.Paxiom

    ''' <summary>
    ''' A value selection for a variable
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Selection
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="variableCode">The variable code for the selection</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal variableCode As String)
            _variableCode = variableCode

        End Sub

        Private _variableCode As String
        ''' <summary>
        ''' The variable code that the selection applies to
        ''' </summary>
        ''' <value>the variable code</value>
        ''' <returns>the variable code</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VariableCode() As String
            Get
                Return _variableCode
            End Get
        End Property

        Private _values As New System.Collections.Specialized.StringCollection
        ''' <summary>
        ''' List of the value codes that is included in the selection
        ''' </summary>
        ''' <value>List of the value codes that is included in the selection</value>
        ''' <returns>List of the value codes that is included in the selection</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValueCodes() As System.Collections.Specialized.StringCollection
            Get
                Return _values
            End Get
        End Property

#Region "Helper functions"

        ''' <summary>
        ''' Creates a selection for all the values for all the variables in meta
        ''' </summary>
        ''' <param name="meta">the metadata</param>
        ''' <returns>A selection array</returns>
        ''' <remarks></remarks>
        Public Shared Function SelectAll(ByVal meta As PCAxis.Paxiom.PXMeta) As PCAxis.Paxiom.Selection()
            Dim s As PCAxis.Paxiom.Selection()
            s = New PCAxis.Paxiom.Selection(meta.Variables.Count - 1) {}
            Dim v As PCAxis.Paxiom.Variable
            For i As Integer = 0 To meta.Variables.Count - 1
                v = meta.Variables(i)
                s(i) = New PCAxis.Paxiom.Selection(v.Code)
                For j As Integer = 0 To v.Values.Count - 1
                    s(i).ValueCodes.Add(v.Values(j).Code)
                Next
            Next
            Return s
        End Function

        ''' <summary>
        ''' Select all values for the variable
        ''' </summary>
        ''' <param name="variable">the variable</param>
        ''' <returns>a selection object</returns>
        ''' <remarks></remarks>
        Public Shared Function SelectAll(ByVal variable As PCAxis.Paxiom.Variable) As PCAxis.Paxiom.Selection
            Dim s As PCAxis.Paxiom.Selection

            s = New PCAxis.Paxiom.Selection(variable.Code)
            For j As Integer = 0 To variable.Values.Count - 1
                s.ValueCodes.Add(variable.Values(j).Code)
            Next
            Return s
        End Function

#End Region

    End Class

End Namespace
