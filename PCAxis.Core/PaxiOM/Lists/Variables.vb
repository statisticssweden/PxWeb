Namespace PCAxis.Paxiom
    ''' <summary>
    ''' List of variables
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class Variables
        Inherits List(Of Variable)
        Implements System.Runtime.Serialization.ISerializable

        Private _meta As PXMeta

        ''' <summary>
        ''' Add a variable to itself
        ''' </summary>
        ''' <param name="item">the variable to add</param>
        ''' <remarks></remarks>
        Public Overloads Sub Add(ByVal item As Variable)
            MyBase.Add(item)
            item.SetMeta(Me.Meta)
        End Sub

        ''' <summary>
        ''' Inserts a variable to itself into a specific position
        ''' </summary>
        ''' <param name="index">the index that the variable will be inserted to</param>
        ''' <param name="item">the variabel the is inserted</param>
        ''' <remarks></remarks>
        Public Overloads Sub Insert(ByVal index As Integer, ByVal item As Variable)
            MyBase.Insert(index, item)
            item.SetMeta(Me.Meta)
        End Sub

        ''' <summary>
        ''' Inserts a range of variables into a specific position
        ''' </summary>
        ''' <param name="index">index of the position</param>
        ''' <param name="range">the variables that should be added</param>
        ''' <remarks></remarks>
        Public Overloads Sub InsertRange(ByVal index As Integer, ByVal range As IEnumerable(Of Variable))
            MyBase.InsertRange(index, range)
            For Each v As Variable In range
                v.SetMeta(Me.Meta)
            Next
        End Sub

        ''' <summary>
        ''' Looks for the index of the variable having the Code equal to code
        ''' </summary>
        ''' <param name="code">the code</param>
        ''' <returns>return the index of the variable</returns>
        ''' <remarks>
        ''' If no variable with code as Code is found then this function will return -1
        ''' </remarks>
        Public Function GetIndexByCode(ByVal code As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If code = Me.Item(i).Code Then
                    Return i
                End If
            Next

            Return -1
        End Function

        ''' <summary>
        ''' Get a variable by code
        ''' </summary>
        ''' <param name="code">the code that the variable should have</param>
        ''' <returns>a Variable</returns>
        ''' <remarks>
        ''' If no variable with code as Code is found then Nothing/null will be returned
        ''' </remarks>
        Public Function GetByCode(ByVal code As String) As Variable
            For i As Integer = 0 To Me.Count - 1
                If code = Me.Item(i).Code Then
                    Return Me.Item(i)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Searches in the current selected language scope for a variable with the Name
        ''' equal to name and returns that.
        ''' </summary>
        ''' <param name="name">The name to look for</param>
        ''' <returns>A Variable</returns>
        ''' <remarks>
        ''' If no variable with name as the Name is found then Nothing/null will be returned
        ''' </remarks>
        Public Function GetByName(ByVal name As String) As Variable
            For i As Integer = 0 To Me.Count - 1
                If name = Me.Item(i).Name Then
                    Return Me.Item(i)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets a variable by the variable name
        ''' </summary>
        ''' <param name="name">name of the variable</param>
        ''' <param name="allLanguages">
        ''' if it should search for the name in all the languages available
        ''' </param>
        ''' <returns>a variable</returns>
        ''' <remarks>
        ''' If no variable with name as the Name is found then Nothing/null will be returned
        ''' </remarks>
        Public Function GetByName(ByVal name As String, ByVal allLanguages As Boolean) As Variable
            'The current language is searched
            Dim var As Variable = GetByName(name)
            If Not allLanguages Then
                Return var
            End If

            'If we have to continou the search for the variable in other languages
            If var Is Nothing Then
                Dim lc As Integer = 0
                If Me.Count > 0 Then
                    lc = Item(0).m_name.Length - 1
                End If

                'Searches all langauages
                For i As Integer = 0 To Me.Count - 1
                    For j As Integer = 0 To lc
                        If name = Me.Item(i).m_name(j) Then
                            Return Me.Item(i)
                        End If
                    Next
                Next
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets a variable by the variable name
        ''' </summary>
        ''' <param name="name">name of the variable</param>
        ''' <param name="languageIndex">
        ''' a the index of the language index
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetByName(ByVal name As String, ByVal languageIndex As Integer) As Variable
            Dim lc As Integer = 0
            If Me.Count > 0 Then
                lc = Item(0).m_name.Length - 1
            End If

            'If no such language
            If languageIndex > lc Then
                Return Nothing
            End If

            'Search in the specified language
            For i As Integer = 0 To Me.Count - 1
                If name = Me.Item(i).m_name(languageIndex) Then
                    Return Me.Item(i)
                End If
            Next

            Return Nothing
        End Function

        Public Sub New()

        End Sub

        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            Dim v As Variable
            Dim c As Integer
            c = info.GetInt32("NoOfVariables")
            For i As Integer = 1 To c
                v = CType(info.GetValue("Variable" & i, GetType(Variable)), Variable)
                Me.Add(v)
            Next
        End Sub

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            Dim c As Integer
            c = Me.Count

            info.AddValue("NoOfVariables", c)
            For i As Integer = 1 To c
                info.AddValue("Variable" & i, Me(i - 1))
            Next
        End Sub

        ''' <summary>
        ''' Is grouping applied to one or more of the variables
        ''' </summary>
        ''' <returns>True if at least one variabe has grouping applied to it, else false</returns>
        ''' <remarks></remarks>
        Public Function HasGroupingsApplied() As Boolean
            For i As Integer = 0 To Me.Count - 1
                If Not Me.Item(i).CurrentGrouping Is Nothing Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Variables
            Dim newObject As New Variables()

            For i As Integer = 0 To Me.Count - 1
                newObject.Add(Me(i).CreateCopyWithValues())
            Next

            newObject.SetMeta(Nothing)

            Return newObject
        End Function

        Protected Friend Sub SetMeta(ByVal meta As PXMeta)
            Me._meta = meta

            For Each variable As Variable In Me
                variable.SetMeta(meta)
            Next
        End Sub

        ''' <summary>
        ''' The meta object that the list is attached to  
        ''' </summary>
        ''' <value>the meta object that the list is attached to </value>
        ''' <returns>the meta object that the list is attached to </returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Meta() As PXMeta
            Get
                Return _meta
            End Get
        End Property
    End Class

End Namespace
