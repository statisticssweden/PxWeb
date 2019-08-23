Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' List of Value
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Values
        Inherits List(Of Value)
        Implements System.Runtime.Serialization.ISerializable

        Private _variable As Variable


        ''' <summary>
        ''' Sets the varaiable that the list is attached to
        ''' </summary>
        ''' <param name="variable"></param>
        ''' <remarks></remarks>
        Protected Friend Sub SetVariable(ByVal variable As Variable)
            _variable = variable

            For Each value As Value In Me
                value.SetVariable(variable)
            Next
        End Sub

        ''' <summary>
        ''' The variable that the list is attached to
        ''' </summary>
        ''' <value>The variable that the list is attached to</value>
        ''' <returns>The variable that the list is attached to</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Variable() As Variable
            Get
                Return _variable
            End Get
        End Property

        ''' <summary>
        ''' Adds a value to the list
        ''' </summary>
        ''' <param name="item">the value</param>
        ''' <remarks></remarks>
        Public Overloads Sub Add(ByVal item As Value)
            MyBase.Add(item)
            item.SetVariable(Me.Variable)
        End Sub

        ''' <summary>
        ''' Inserts a value to the list
        ''' </summary>
        ''' <param name="index">index of the position where to insert the value</param>
        ''' <param name="item">the value</param>
        ''' <remarks></remarks>
        Public Overloads Sub Insert(ByVal index As Integer, ByVal item As Value)
            MyBase.Insert(index, item)
            item.SetVariable(Me.Variable)
        End Sub

        ''' <summary>
        ''' inserts a values to the list
        ''' </summary>
        ''' <param name="index">index of the position where to insert the values</param>
        ''' <param name="range">the values</param>
        ''' <remarks></remarks>
        Public Overloads Sub InsertRange(ByVal index As Integer, ByVal range As IEnumerable(Of Value))
            MyBase.InsertRange(index, range)
            For Each v As Value In range
                v.SetVariable(Me.Variable)
            Next
        End Sub

        ''' <summary>
        ''' Removes the values from the list
        ''' </summary>
        ''' <param name="item">item to remove</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Remove(ByVal item As Value) As Boolean
            item.SetVariable(Nothing)
            Return MyBase.Remove(item)
        End Function

        ''' <summary>
        ''' Removes a item at a specific psosition
        ''' </summary>
        ''' <param name="index">the index of the item</param>
        ''' <remarks></remarks>
        Public Overloads Sub RemoveAt(ByVal index As Integer)
            Me.Item(index).SetVariable(Nothing)
            MyBase.RemoveAt(index)
        End Sub

        ''' <summary>
        ''' Removes a range of items
        ''' </summary>
        ''' <param name="index">start index of the range</param>
        ''' <param name="count">number of items that should be removed</param>
        ''' <remarks></remarks>
        Public Overloads Sub RemoveRange(ByVal index As Integer, ByVal count As Integer)
            For i As Integer = index To index + count - 1
                Me.Item(i).SetVariable(Nothing)
            Next
            MyBase.RemoveRange(index, count)
        End Sub

        ''' <summary>
        ''' Removes all items from the list
        ''' </summary>
        ''' <param name="match">predicated for removal</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function RemoveAll(ByVal match As Predicate(Of Value)) As Integer
            For Each v As Value In Me
                If match(v) Then
                    v.SetVariable(Nothing)
                End If
            Next
            Return MyBase.RemoveAll(match)
        End Function

        ''' <summary>
        ''' Clears the list
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Clear()
            For Each v As Value In Me
                v.SetVariable(Nothing)
            Next
            MyBase.Clear()
        End Sub

        <Obsolete("Use CreateCopy")> _
        Public Function Clone() As Values
            Dim m As New MemoryStream()
            Dim b As New BinaryFormatter()
            b.Serialize(m, Me)
            m.Position = 0
            Return DirectCast(b.Deserialize(m), Values)
        End Function

        ''' <summary>
        ''' Get a value by value name
        ''' </summary>
        ''' <param name="name">name of the value</param>
        ''' <returns>a value</returns>
        ''' <remarks>if no value is found then Nothing/null will be returned</remarks>
        Public Function GetByName(ByVal name As String) As Value
            For i As Integer = 0 To Me.Count - 1
                If name = Me.Item(i).Value Then
                    Return Me.Item(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function SearchInValue(ByVal stringToSearchFor As String) As List(Of Value)
            Dim fw As New FindWrapper(stringToSearchFor)
            Return Me.FindAll(AddressOf fw.FindInValue)
        End Function

        Public Function SearchInBeginningOfValue(ByVal stringToSearchFor As String) As List(Of Value)
            Dim fw As New FindWrapper(stringToSearchFor)
            Return Me.FindAll(AddressOf fw.FindInBeginningOfValue)
        End Function

        Public Function SearchInCode(ByVal stringToSearchFor As String) As List(Of Value)
            Dim fw As New FindWrapper(stringToSearchFor)
            Return Me.FindAll(AddressOf fw.FindInCode)
        End Function

        Public Function SearchInBeginningOfCode(ByVal stringToSearchFor As String) As List(Of Value)
            Dim fw As New FindWrapper(stringToSearchFor)
            Return Me.FindAll(AddressOf fw.FindInBeginningOfCode)
        End Function

        ''' <summary>
        ''' Gets a value by code
        ''' </summary>
        ''' <param name="code">the code</param>
        ''' <returns>a value</returns>
        ''' <remarks>If no value is found Nothing/null will be returned</remarks>
        Public Function GetByCode(ByVal code As String) As Value
            For i As Integer = 0 To Me.Count - 1
                If code = Me.Item(i).Code Then
                    Return Me.Item(i)
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets index of value with specified code
        ''' </summary>
        ''' <param name="code">the code</param>
        ''' <returns>the index of the value</returns>
        ''' <remarks>If no value is found -1 will be returned</remarks>
        Public Function GetIndexByCode(ByVal code As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If code = Me.Item(i).Code Then
                    Return i
                End If
            Next
            Return -1
        End Function
        ''' <summary>
        ''' Gets index of value with specified name
        ''' </summary>
        ''' <param name="code">the code</param>
        ''' <returns>the index of the value</returns>
        ''' <remarks>If no value is found -1 will be returned</remarks>
        Public Function GetIndexByName(ByVal code As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If code = Me.Item(i).Value Then
                    Return i
                End If
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Creates a deep copy of the collection
        ''' </summary>
        ''' <returns>a deep copy of the collection</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As Values
            Dim newObject As New Values

            ' Cannot do this one for .NET objects
            'newObject = CType(Me.MemberwiseClone(), Values) ' Copy the value types

            ' Handle value types
            newObject._CodeIsFictional = Me._CodeIsFictional

            ' Handle reference types
            For Each v As Value In Me
                newObject.Add(v.CreateCopy())
            Next

            newObject.SetVariable(Nothing)

            Return newObject
        End Function

        '''<summay>
        '''Checks if the values in the collection have codes
        '''</summay>
        '''<returns>Returns True if there is codes for the values otherwise False</returns>
        '''<remark>If the codes are generated fictional then the property will return False. 
        '''The way to determine if the values have codes is by checking if the first value 
        '''in the collection have a code.
        '''</remark>
        Public ReadOnly Property ValuesHaveCodes() As Boolean
            Get
                If _CodeIsFictional Then
                    Return False
                End If

                If Me.Count > 0 Then
                    If String.IsNullOrEmpty(Me.Item(0).Code) Then
                        Return False
                    Else
                        Return True
                    End If
                End If
                'There is no values return false
                Return False
            End Get
        End Property

        Private _CodeIsFictional As Boolean = False

        ''' <summary>
        ''' Returns if the codes are fictinal
        ''' </summary>
        ''' <value>If the codes are fictional/genereted</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsCodesFictional() As Boolean
            Get
                Return _CodeIsFictional
            End Get
        End Property

        '''<summary>
        '''Sets fictional codes to the values
        '''</summary>
        '''<remark>
        '''If the values already contains codes, even fictional nothing is done to the values
        '''</remark>
        Public Sub SetFictionalCodes()
            'The codes is already fictional
            If _CodeIsFictional Then
                ' Check and see if there are any new Values that need codes
                ' Start new ValueCodes from Max + 1
                Dim valuesWithoutCode As New System.Collections.Generic.List(Of Value)
                Dim newCode As Integer = -1
                Dim tmpCode As Integer = -1
                For i As Integer = 0 To Me.Count - 1
                    If String.IsNullOrEmpty(Me.Item(i).Code) Then
                        ' Add to list that we will loop below
                        valuesWithoutCode.Add(Me.Item(i))
                    Else
                        ' Keep track of Max code
                        tmpCode = Convert.ToInt32(Me.Item(i).Code)
                        If tmpCode > newCode Then
                            newCode = tmpCode
                        End If
                    End If
                Next
                ' Add 1 to newCode (Max code + 1 --> next code in sequence)
                newCode = newCode + 1
                ' now loop and set code(s)
                For Each v As Value In valuesWithoutCode
                    v.SetCode(newCode.ToString())
                    newCode = newCode + 1
                Next
            Else

                'Checks to see if there is any values
                If Me.Count > 0 Then
                    'Checks that there is no ordenary codes
                    If String.IsNullOrEmpty(Me.Item(0).Code) Then
                        For i As Integer = 0 To Me.Count - 1
                            Me.Item(i).SetCode(i.ToString())
                        Next
                        _CodeIsFictional = True
                    End If
                End If
            End If
        End Sub

        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("FictionalCodes", _CodeIsFictional)
            Dim c As Integer = Me.Count
            info.AddValue("NoOfValues", c)
            For i As Integer = 1 To c
                info.AddValue("Value" & i, Me(i - 1))
            Next
        End Sub

        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            _CodeIsFictional = info.GetBoolean("FictionalCodes")
            Dim c As Integer = info.GetInt32("NoOfValues")
            Dim v As Value
            For i As Integer = 1 To c
                v = CType(info.GetValue("Value" & i, GetType(Value)), Value)
                Me.Add(v)
            Next
        End Sub

        Public Sub New()
        End Sub

        Public Sub New(ByVal var As Variable)
            Me._variable = var
        End Sub
    End Class

    'TODO PETROS CHECK how this is used
    Public Class FindWrapper
        Public StringToSearchFor As String

        Public Sub New(ByVal stringToSearchFor As String)
            Me.StringToSearchFor = stringToSearchFor
        End Sub

        Public Function FindInValue(ByVal valueToSearchIn As Object) As Boolean
            Dim val As Value = DirectCast(valueToSearchIn, Value)
            Return val.Value.IndexOf(StringToSearchFor, StringComparison.CurrentCultureIgnoreCase) > -1
        End Function

        Public Function FindInBeginningOfValue(ByVal valueToSearchIn As Object) As Boolean
            Dim val As Value = DirectCast(valueToSearchIn, Value)
            Return val.Value.StartsWith(StringToSearchFor, StringComparison.CurrentCultureIgnoreCase)
        End Function

        Public Function FindInCode(ByVal valueToSearchIn As Object) As Boolean
            Dim val As Value = DirectCast(valueToSearchIn, Value)
            Return val.Code.IndexOf(StringToSearchFor, StringComparison.CurrentCultureIgnoreCase) > -1
        End Function

        Public Function FindInBeginningOfCode(ByVal valueToSearchIn As Object) As Boolean
            Dim val As Value = DirectCast(valueToSearchIn, Value)
            Return val.Code.StartsWith(StringToSearchFor, StringComparison.CurrentCultureIgnoreCase)
        End Function
    End Class
End Namespace

