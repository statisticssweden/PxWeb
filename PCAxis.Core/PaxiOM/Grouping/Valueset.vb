
Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Represents a valueset
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Valueset

        Private _name As String
        Private _prestext As String
        Private _groupings As New List(Of GroupingInfo)
        Private _values As New Dictionary(Of String, String)

        Public Class ValueSetValue
            Private _code As String
            Private _text As String

            Public Property Code() As String
                Get
                    Return _code
                End Get
                Set(ByVal value As String)
                    _code = value
                End Set
            End Property

            Public Property Text() As String
                Get
                    Return _text
                End Get
                Set(ByVal value As String)
                    _text = value
                End Set
            End Property

            ''' <summary>
            ''' Creates a deep copy of the ValueSetValue object instance
            ''' </summary>
            ''' <returns>A deep copy of the ValueSet object instance</returns>
            ''' <remarks></remarks>
            Public Function CreateCopy() As ValueSetValue
                Dim vsv As ValueSetValue

                vsv = CType(Me.MemberwiseClone(), ValueSetValue)

                Return vsv
            End Function
        End Class

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Prestext() As String
            Get
                Return _prestext
            End Get
            Set(ByVal value As String)
                _prestext = value
            End Set
        End Property

        Public Property Groupings() As List(Of GroupingInfo)
            Get
                Return _groupings
            End Get
            Set(ByVal value As List(Of GroupingInfo))
                _groupings = value
            End Set
        End Property

        Public Property Values() As Dictionary(Of String, String)
            Get
                Return _values
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _values = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a deep copy of the Valueset object instance
        ''' </summary>
        ''' <returns>A deep copy of the Valueset object instance</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Valueset
            Dim valueset As Valueset

            valueset = CType(Me.MemberwiseClone(), Valueset)
            valueset._groupings = New List(Of GroupingInfo)
            For Each gi As GroupingInfo In Me.Groupings
                valueset._groupings.Add(gi.CreateCopy())
            Next

            valueset._values = New Dictionary(Of String, String)
            For Each kvp As KeyValuePair(Of String, String) In Me.Values
                valueset._values.Add(kvp.Key, kvp.Value)
            Next

            Return valueset
        End Function
    End Class
End Namespace
