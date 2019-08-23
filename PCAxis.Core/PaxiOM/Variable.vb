Imports PCAxis.Paxiom.ClassAttributes
Imports PCAxis.Paxiom.Localization

Namespace PCAxis.Paxiom

    ' Klass som beskriver en variabel
    <Serializable()> _
    Public Class Variable
        Implements System.Runtime.Serialization.ISerializable

#Region "Protected Members"
        Protected Friend mLanguageIndex As Integer = 0

        Private _meta As PXMeta
        <LanguageDependent()> _
        Protected Friend m_name() As String
        Private _time As String
        Private _timeVariable As Boolean = False
        <LanguageDependent()> _
        Private _domain() As String
        Private _elimination As Boolean
        Private _eliminationValue As Value
        Private _map As String
        Private _presentationText As Integer = 1 ' Text is default (0=Code, 1=Text, 2=Code+Text, 3=Text+Code)
        Private _doubleColumn As Boolean = False
        Private _values As Values = New Values
        Private _placement As PlacementType
        <LanguageDependent()> _
        Private _notes() As Notes
        Private _isContentVariable As Boolean = False
        Private _variableType As String
        Private _hierarchy As Hierarchy
        Private _partitions As New Partitions
        Private _code As String
        Private _keys As KeysTypes = KeysTypes.None
        Private _extendedProperties As New ExtendedPropertiesList
        Private _valueSets As New List(Of ValueSetInfo)
        <LanguageDependent()> _
        Private _groupings() As List(Of GroupingInfo)
        Private _currentGrouping As Grouping
        Private _currentValueSet As ValueSetInfo
        Private _TimeScale As TimeScaleType = TimeScaleType.NotSet
        Private _sortVariable As Boolean = False
        Private _valueTextOption As ValueTextOptionType = ValueTextOptionType.NormalText

        <LanguageDependent()> _
        Private _datanote(0) As String
        <LanguageDependent()> _
        Private _metaId(0) As String
        <LanguageDependent()> _
        Private _hasValueMetaId(0) As String

#End Region

        Protected Friend Sub SetMeta(ByVal meta As PXMeta)
            _meta = meta
        End Sub

#Region "Public Properties"
        ''' <summary>
        ''' the meta object that the list is attached to 
        ''' </summary>
        ''' <value>the meta object that the list is attached to </value>
        ''' <returns>the meta object that the list is attached to </returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Meta() As PXMeta
            Get
                Return _meta
            End Get
        End Property

        ''' <summary>
        ''' Check to see if the variable have any notes
        ''' </summary>
        ''' <returns>True if there are any notes in the Notes list</returns>
        ''' <remarks></remarks>
        Public Function HasNotes() As Boolean
            If Me._notes Is Nothing Then
                Return False
            End If

            If Me._notes(mLanguageIndex) Is Nothing Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Check to see if the variable have any groupings
        ''' </summary>
        ''' <returns>True if there are any notes in the Groupings list</returns>
        ''' <remarks></remarks>
        Public Function HasGroupings() As Boolean
            If Me._groupings Is Nothing Then
                Return False
            End If

            If Me._groupings(mLanguageIndex) Is Nothing Then
                Return False
            End If

            If Me._groupings(mLanguageIndex).Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Check to see if the variable have any value sets
        ''' </summary>
        ''' <returns>True if there are any notes in the ValueSets list</returns>
        ''' <remarks></remarks>
        Public Function HasValuesets() As Boolean
            If Me._valueSets Is Nothing Then
                Return False
            End If

            If Me._valueSets.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        '''<summary>Returns the variable code</summary>
        '''<value>Model unique code for the variable</value>
        '''<remarks>
        '''This keyword dosn't exisit in the PC-Axis file format.
        '''At the moment the code is the variable name of the 
        '''default language but this may change in the future
        '''</remarks>
        Public ReadOnly Property Code() As String
            Get
                Return _code
            End Get
        End Property

        '''<summary>Returns the variables name</summary>
        '''<value>Name of the variable</value>
        Public Property Name() As String 'Implements IPXItem.Name
            Get
                'If Me.IsTime Then
                '    Select Case TimeScale
                '        Case TimeScaleType.Annual
                '            Return PxResourceManager.GetResourceManager().GetString("PxcYear", Me.Meta.CurrentLanguage)
                '        Case TimeScaleType.Halfyear
                '            Return PxResourceManager.GetResourceManager().GetString("PxcHalfyear", Me.Meta.CurrentLanguage)
                '        Case TimeScaleType.Quartely
                '            Return PxResourceManager.GetResourceManager().GetString("PxcQuarter", Me.Meta.CurrentLanguage)
                '        Case TimeScaleType.Monthly
                '            Return PxResourceManager.GetResourceManager().GetString("PxcMonth", Me.Meta.CurrentLanguage)
                '        Case TimeScaleType.Weekly
                '            Return PxResourceManager.GetResourceManager().GetString("PxcWeek", Me.Meta.CurrentLanguage)
                '        Case Else
                '            Return Me.m_name(mLanguageIndex)
                '    End Select
                'Else
                Return Me.m_name(mLanguageIndex)
                'End If
            End Get
            Set(ByVal value As String)
                Me.m_name(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Tells if the variable is a time variable or not
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the variable is a time variable, else false</returns>
        ''' <remarks></remarks>
        Public Property IsTime() As Boolean
            Get
                Return _timeVariable
            End Get
            Set(ByVal value As Boolean)
                _timeVariable = value
            End Set
        End Property

        '''<summary>Time value of the variable</summary>
        '''<value></value>
        Public Property TimeValue() As String
            Get
                Return Me._time
            End Get
            Set(ByVal value As String)
                'TODO: Create values if there is none
                Me._time = value
            End Set
        End Property

        ''' <summary>
        ''' Returns what type of keys that should be used when storing the model as
        ''' a PX file in the KEYS format.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Keys() As KeysTypes
            Get
                Return _keys
            End Get
            Set(ByVal value As KeysTypes)
                _keys = value
            End Set
        End Property


        '''<summary>Returns the variable domain</summary>
        '''<value>The doamin of the variable</value>
        Public Property Domain() As String
            Get
                Return Me._domain(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._domain(mLanguageIndex) = value
            End Set
        End Property

        '''<summary>Returns is the variable can be eliminated</summary>
        '''<value>If the variable can be eliminated</value>
        Public Property Elimination() As Boolean
            Get
                Return Me._elimination
            End Get
            Set(ByVal value As Boolean)
                Me._elimination = value
            End Set
        End Property

        '''<summary>Returns the values selected for elimination</summary>
        '''<value>The elimination value if one have been specified</value>
        Public Property EliminationValue() As Value
            Get
                Return Me._eliminationValue
            End Get
            Set(ByVal value As Value)
                Me._eliminationValue = value
            End Set
        End Property

        '''<summary>Returns the variable map</summary>
        '''<value>The map recommend for the varibale</value>
        Public Property Map() As String
            Get
                Return Me._map
            End Get
            Set(ByVal value As String)
                Me._map = value
            End Set
        End Property

        'TODO check if it should be obsolete
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresentationText() As Integer
            Get
                Return Me._presentationText
            End Get
            Set(ByVal value As Integer)
                Me._presentationText = value
            End Set
        End Property

        '''<summary>Returns if the should be separate columns for the codes and the values</summary>
        '''<value>If there should be separet columns for code and values</value>
        Public Property DoubleColumn() As Boolean
            Get
                Return Me._doubleColumn
            End Get
            Set(ByVal value As Boolean)
                Me._doubleColumn = value
            End Set
        End Property

        '''<summary>Returns if the variable is in the stub or in the heading</summary>
        '''<value>The placment of the variable</value>
        Public Property Placement() As PlacementType
            Get
                Return Me._placement
            End Get
            Set(ByVal value As PlacementType)
                Me._placement = value
            End Set
        End Property

        '''<summary>Returns the values for the variable</summary>
        '''<value>The variable values</value>
        Public ReadOnly Property Values() As Values
            Get
                Return Me._values
            End Get
        End Property

        '''<summary>Checks if the variable is a content variable</summary>
        '''<value>Checks if variable is a content variable</value>
        Public Property IsContentVariable() As Boolean
            Get
                Return Me._isContentVariable
            End Get
            Set(ByVal value As Boolean)
                Me._isContentVariable = value
            End Set
        End Property

        '''<summary>Checks if the variable has a timevalue (TIMEVAL)</summary>
        '''<value>Checks if variable is a time variable</value>
        Public ReadOnly Property HasTimeValue() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._time)
            End Get
        End Property

        '''<summary>Returns the variable type</summary>
        '''<value>The variable type</value>
        Public Property VariableType() As String
            Get
                Return _variableType
            End Get
            Set(ByVal value As String)
                _variableType = value
            End Set
        End Property

        '''<summary>Returns the variable hirarchy if the variable is hierarcical</summary>
        '''<value>The hierarchy of the variable</value>
        Public Property Hierarchy() As Hierarchy
            Get
                Return _hierarchy
            End Get
            Set(ByVal value As Hierarchy)
                _hierarchy = value
            End Set
        End Property

        '''<summary>Returns the partiotions of the variable if it is partitioned</summary>
        '''<value>The partitions of the variable</value>
        Public ReadOnly Property Partitions() As Partitions
            Get
                Return _partitions
            End Get
        End Property

        '''<summary>Returns the notes for the variable</summary>
        '''<value>Notes for the variable</value>
        Public ReadOnly Property Notes() As Notes
            Get
                If _notes Is Nothing Then
                    Return Nothing
                End If
                Return _notes(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' Extended properties for the variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExtendedProperties() As ExtendedPropertiesList
            Get
                Return _extendedProperties
            End Get
        End Property

        ''' <summary>
        ''' List of available value setsfor the variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValueSets() As List(Of ValueSetInfo)
            Get
                Return _valueSets
            End Get
        End Property

        ''' <summary>
        ''' List of groupings for the variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Groupings() As List(Of GroupingInfo)
            Get
                Return _groupings(mLanguageIndex)
            End Get
        End Property

        ''' <summary>
        ''' The value set that is currently applied
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CurrentValueSet() As ValueSetInfo
            Get
                Return _currentValueSet
            End Get
            Set(ByVal value As ValueSetInfo)
                _currentValueSet = value
            End Set
        End Property

        ''' <summary>
        ''' The grouping that is currently applied
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CurrentGrouping() As Grouping
            Get
                Return _currentGrouping
            End Get
            Set(ByVal value As Grouping)
                _currentGrouping = value
            End Set
        End Property

        ''' <summary>
        ''' The time scale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TimeScale() As TimeScaleType
            Get
                Return _TimeScale
            End Get
        End Property

        ''' <summary>
        ''' Is this a sortvariable? ($$SORT)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SortVariable() As Boolean
            Get
                Return _sortVariable
            End Get
            Set(ByVal value As Boolean)
                _sortVariable = value
            End Set
        End Property

        ''' <summary>
        ''' Describes how the value texts are created
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValueTextOption() As ValueTextOptionType
            Get
                Return _valueTextOption
            End Get
            Set(ByVal value As ValueTextOptionType)
                _valueTextOption = value
            End Set
        End Property

        ''' <summary>
        ''' Datanote is used to indicate that a note exist for the variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Datanote() As String
            Get
                Return _datanote(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._datanote(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' A Metadata Id for the variable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MetaId() As String
            Get
                Return _metaId(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me._metaId(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Shows if any of the variable values has got a MetaId
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if one or more values has got a MetaId, else false</returns>
        ''' <remarks></remarks>
        Public Property HasValueMetaId() As Boolean
            Get
                If (String.IsNullOrEmpty(_hasValueMetaId(mLanguageIndex))) Then
                    Return False
                Else
                    Return _hasValueMetaId(mLanguageIndex).Equals("true")
                End If
            End Get
            Set(ByVal value As Boolean)
                _hasValueMetaId(mLanguageIndex) = value.ToString().ToLower()
            End Set
        End Property

#End Region

#Region "Constructor"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._values.SetVariable(Me)
            Me.m_name = New String(0) {}
            Me._domain = New String(0) {}
            Me._eliminationValue = Nothing
            Me._code = Guid.NewGuid.ToString()
            Me._groupings = New List(Of GroupingInfo)(0) {}
            Me._hierarchy = New Hierarchy()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="name">name of the variable</param>
        ''' <param name="placment">variable placment</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String, ByVal placment As PlacementType)
            Me._values.SetVariable(Me)
            Me.m_name = New String(0) {}
            Me._domain = New String(0) {}
            Me._eliminationValue = Nothing
            Me.m_name(mLanguageIndex) = name
            Me._placement = placment
            Me._code = name
            Me._groupings = New List(Of GroupingInfo)(0) {}
            Me._hierarchy = New Hierarchy()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="name">name of the variable</param>
        ''' <param name="placment">variable placment</param>
        ''' <param name="internalBufferSize">
        ''' for how many languages that the 
        ''' variable should be dimension for
        ''' </param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String, ByVal placment As PlacementType, ByVal internalBufferSize As Integer)
            Me._values.SetVariable(Me)
            Me.m_name = New String(internalBufferSize) {}
            Me._domain = New String(internalBufferSize) {}
            Me._eliminationValue = Nothing
            Me.m_name(mLanguageIndex) = name
            Me._placement = placment
            Me._code = name
            Me._groupings = New List(Of GroupingInfo)(internalBufferSize) {}
            Me._hierarchy = New Hierarchy(internalBufferSize)
            Me._datanote = New String(internalBufferSize) {}
            Me._metaId = New String(internalBufferSize) {}
            Me._hasValueMetaId = New String(internalBufferSize) {}
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="name">name of the variable</param>
        ''' ''' <param name="code">the variable code</param>
        ''' <param name="placment">variable placment</param>
        ''' <param name="internalBufferSize">
        ''' for how many languages that the 
        ''' variable should be dimension for
        ''' </param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String, ByVal code As String, ByVal placment As PlacementType, ByVal internalBufferSize As Integer)
            Me._values.SetVariable(Me)
            Me.m_name = New String(internalBufferSize) {}
            Me._domain = New String(internalBufferSize) {}
            Me._eliminationValue = Nothing
            Me.m_name(mLanguageIndex) = name
            Me._placement = placment
            Me._code = code
            Me._groupings = New List(Of GroupingInfo)(internalBufferSize) {}
            Me._hierarchy = New Hierarchy(internalBufferSize)
            Me._datanote = New String(internalBufferSize) {}
            Me._metaId = New String(internalBufferSize) {}
            Me._hasValueMetaId = New String(internalBufferSize) {}
        End Sub
#End Region

#Region "SetFunctions"
        ''' <summary>
        ''' Sets the code for the variable
        ''' </summary>
        ''' <param name="code">the code</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetCode(ByVal code As String)
            _code = code
        End Sub

        'TODO: Petros Prio A fixa else om de olika längderna inte är samma.
        ''' <summary>
        ''' Creates new values or just theer names if the values already exist
        ''' </summary>
        ''' <param name="values">name of values</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetValues(ByVal values As String())
            If _values.Count = values.Length Then
                For i As Integer = 0 To _values.Count - 1
                    _values(i).Value = values(i)
                Next
            Else
                Dim size As Integer = Me.m_name.Length - 1
                For Each s As String In values
                    Me._values.Add(New Value(s, size))
                Next
            End If

        End Sub

        ''' <summary>
        ''' If the variable is a timevariable this method creates the timevalues for each of 
        ''' the variables values. 
        ''' </summary>
        ''' <param name="values">
        ''' Array containing the timevalues. The first value in the array shall contain the timescale
        ''' type, for example: TLIST(M1). The rest of the values in the array contains the actual timevalues.
        ''' </param>
        ''' <remarks>
        ''' Timevalues can be described in the ways:
        ''' 1. Interval
        ''' Example:
        ''' TimeValue for the variable: TLIST(M1, 199601-200812)
        ''' 2. Individual values
        ''' Example:
        ''' TimeValue for the variable: TLIST(M1), 200610, 200611, 200612, ...
        ''' </remarks>
        Protected Friend Sub SetTimeValues(ByVal values As String())
            If String.IsNullOrEmpty(Me.TimeValue) Then
                Exit Sub
            End If

            If Not values.Count > 1 Then
                Exit Sub
            End If

            SetTimeScale(values(0))

            'Find out in which way timevalues are described
            If Me.TimeValue.IndexOf("""-""") <> -1 Then
                'Timevalues are described as an interval
                SetTimeValuesFromInterval(values)
            Else
                'Timevalues are described with individual values
                If (_values.Count = values.Length - 1) And values.Length > 1 Then
                    For i As Integer = 0 To _values.Count - 1
                        _values(i).TimeValue = GetTimeValue(values(i + 1))
                    Next
                End If
            End If

        End Sub

        ''' <summary>
        ''' Timevalue shall only contain digits.
        ''' For example January 1993 shall have the timevalue 199301 and not 1993M01
        ''' This method removes any characters in the timevalue
        ''' </summary>
        ''' <param name="val">The timevalue to strip from characters</param>
        ''' <returns>A timevalue without chracters</returns>
        ''' <remarks></remarks>
        Private Function GetTimeValue(ByVal val As String) As String
            Dim timevalue As New Text.StringBuilder

            For i As Integer = 0 To val.Length - 1
                If Char.IsDigit(val.Chars(i)) Then
                    timevalue.Append(val.Chars(i))
                End If
            Next

            Return timevalue.ToString
        End Function

        ''' <summary>
        ''' Generates the timevalues when described as an interval in the px-file
        ''' </summary>
        ''' <param name="values">
        ''' Array with the length of 2. The first value contains timescale and the second value the interval.
        ''' </param>
        ''' <remarks></remarks>
        Private Sub SetTimeValuesFromInterval(ByVal values As String())
            Dim periods As Integer
            Dim arrPeriods As String()
            Dim year As Integer
            Dim period As Integer = 0
            Dim endYear As Integer
            Dim endPeriod As Integer = 0
            Dim timeValues As New Collections.Specialized.StringCollection
            Dim timeValue As String = ""

            'In this case there shall be only two values in the array.
            'The first one contains the timescale type and the second one the interval
            If values.Count = 2 Then
                If values(1).IndexOf("-") <> -1 Then

                    'Set number of periods for this timescale
                    Select Case Me.TimeScale
                        Case TimeScaleType.Annual
                            periods = 0
                        Case TimeScaleType.Halfyear
                            periods = 2
                        Case TimeScaleType.Monthly
                            periods = 12
                        Case TimeScaleType.Quartely
                            periods = 4
                        Case TimeScaleType.Weekly
                            periods = 52
                        Case Else
                            Exit Sub
                    End Select

                    'Find interval...
                    arrPeriods = values(1).Split("-"c)
                    arrPeriods(1) = arrPeriods(1).TrimEnd(")"c)
                    year = CInt(arrPeriods(0).Substring(0, 4))
                    endYear = CInt(arrPeriods(1).Substring(0, 4))
                    If Me.TimeScale <> TimeScaleType.Annual Then
                        period = CInt(arrPeriods(0).Substring(4))
                        endPeriod = CInt(arrPeriods(1).Substring(4))
                    End If

                    While True
                        'Continue?
                        If (year > endYear) Or ((year = endYear) And (period > endPeriod)) Then
                            Exit While
                        End If

                        'Format timevalue...
                        Select Case Me.TimeScale
                            Case TimeScaleType.Annual
                                timeValue = year.ToString
                            Case TimeScaleType.Halfyear
                                timeValue = year.ToString & period.ToString
                            Case TimeScaleType.Quartely
                                timeValue = year.ToString & period.ToString
                            Case TimeScaleType.Monthly
                                timeValue = year.ToString & period.ToString.PadLeft(2, "0"c)
                            Case TimeScaleType.Weekly
                                timeValue = year.ToString & period.ToString.PadLeft(2, "0"c)
                        End Select

                        'Add to collection...
                        timeValues.Add(timeValue)

                        'Increment...
                        If Me.TimeScale = TimeScaleType.Annual Then
                            year = year + 1
                        Else
                            If period = periods Then
                                year = year + 1
                                period = 1
                            Else
                                period = period + 1
                            End If
                        End If
                    End While

                    'Apply timevalues...
                    If _values.Count = timeValues.Count Then
                        For i As Integer = 0 To _values.Count - 1
                            _values(i).TimeValue = timeValues(i)
                        Next
                    End If

                End If
            End If
        End Sub

        ''' <summary>
        ''' If the variable is a timevariable this method sets the timescale of the variable.
        ''' </summary>
        ''' <param name="timeScaleDesc">Contains the timescale part of the variables TimeValue</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetTimeScale(ByVal timeScaleDesc As String)
            Dim start As Integer
            Dim timeScale As String = ""

            If timeScaleDesc.ToLower.IndexOf("tlist(") = -1 Then
                Exit Sub
            End If

            start = timeScaleDesc.IndexOf("(") + 1

            If timeScaleDesc.Length < start + 2 Then
                Exit Sub
            End If

            timeScale = timeScaleDesc.Substring(start, 2)

            Select Case timeScale
                Case "A1"
                    _TimeScale = TimeScaleType.Annual
                Case "H1"
                    _TimeScale = TimeScaleType.Halfyear
                Case "Q1"
                    _TimeScale = TimeScaleType.Quartely
                Case "M1"
                    _TimeScale = TimeScaleType.Monthly
                Case "W1"
                    _TimeScale = TimeScaleType.Weekly
                Case Else
                    Exit Sub
            End Select
        End Sub

        ''' <summary>
        ''' Sets value codes
        ''' </summary>
        ''' <param name="codes">the codes</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetValueCodes(ByVal codes As String())
            'BUG 316: If _values.count = 0 --> Create new values with text and code = code
            If Me._values.Count = 0 Then
                Me.SetValues(codes)
                Me.ValueTextOption = ValueTextOptionType.ValueTextMissing
            End If

            If Me._values.Count <> codes.Length Then
                Throw New PXException("", "PxcErrToManyCodes", New Object() {Me.Name})
            End If
            For index As Integer = 0 To Me._values.Count - 1
                Me._values(index).SetCode(codes(index))
            Next
        End Sub

        ''' <summary>
        ''' Sets the doamin
        ''' </summary>
        ''' <param name="domain">the domain</param>
        ''' <remarks></remarks>
        <Obsolete()> _
        Protected Friend Sub SetDomain(ByVal domain As String)
            Me._domain(mLanguageIndex) = domain
        End Sub

        ''' <summary>
        ''' Sets the Elimination and the EliminationValue
        ''' </summary>
        ''' <param name="elimination">the type of elimination</param>
        ''' <remarks>
        ''' if YES is passed in as a elimination then Elmimination is set to true 
        ''' but no elimination value is set.
        ''' </remarks>
        Public Sub SetElimination(ByVal elimination As String)
            If String.Compare(elimination.ToUpper(), "NO") = 0 Then
                Me._elimination = False
                Exit Sub
            End If

            Me._elimination = True
            If String.Compare(elimination, "YES", True) = 0 Then
                Me._eliminationValue = Nothing
            Else
                Dim v As Value = Values.GetByName(elimination)
                Me._eliminationValue = v
            End If

        End Sub

        ''' <summary>
        ''' Set the elemination value
        ''' </summary>
        ''' <param name="code">code of the value</param>
        ''' <remarks></remarks>
        Public Sub SetEliminationValue(ByVal code As String)
            Me.EliminationValue = Me.Values.GetByCode(code)
        End Sub

        <Obsolete()> _
        Protected Friend Sub SetMap(ByVal map As String)
            Me._map = map
        End Sub

        <Obsolete()> _
        Protected Friend Sub SetPresText(ByVal presText As Integer)
            Me._presentationText = presText
        End Sub

        'TODO PETROS check builder how it is used should be done in meta.
        ''' <summary>
        ''' Marks the variable as a content variable
        ''' </summary>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Protected Friend Sub SetAsContentVariable(ByVal b As Boolean)
            Me._isContentVariable = b
        End Sub

#End Region

#Region "Add functions"
        ''' <summary>
        ''' Adds a note to the variable
        ''' </summary>
        ''' <param name="note">note to add to the Notes list of the variable</param>
        ''' <remarks>
        ''' One should always use this function to add notes since this 
        ''' function reassures that the Notes list is created.
        ''' </remarks>
        Protected Friend Sub AddNote(ByVal note As Note)
            If Me._notes Is Nothing Then
                'Me.mNotes = New NoteCollection
                Me._notes = New Notes(m_name.Length - 1) {}
            End If

            If _notes(mLanguageIndex) Is Nothing Then
                _notes(mLanguageIndex) = New Notes
            End If

            Me._notes(mLanguageIndex).Add(note)
        End Sub

        ''' <summary>
        ''' Adds a grouping to the variable
        ''' </summary>
        ''' <param name="groupingInfo">grouping to add to the Grouping list</param>
        ''' <remarks>
        ''' One should always use this function to add groupings since this 
        ''' function reassures that the Groupings list is created.
        ''' </remarks>
        Public Sub AddGrouping(ByVal groupingInfo As GroupingInfo)
            If Me._groupings Is Nothing Then
                Me._groupings = New List(Of GroupingInfo)(m_name.Length - 1) {}
            End If

            If _groupings(mLanguageIndex) Is Nothing Then
                _groupings(mLanguageIndex) = New List(Of GroupingInfo)
            End If

            If Not Me._groupings(mLanguageIndex).Contains(groupingInfo) Then
                Me._groupings(mLanguageIndex).Add(groupingInfo)
            End If
        End Sub
#End Region

#Region "Public functions"

        ''' <summary>
        ''' Create a deep copy of Me
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Variable.Values is not included</remarks>
        Public Function CreateCopy() As Variable
            Dim newObject As Variable
            newObject = CType(Me.MemberwiseClone(), Variable)
            newObject.SetMeta(Nothing)

            'Handle reference types

            ' Handle Name
            newObject.m_name = Nothing
            If Me.m_name IsNot Nothing Then
                newObject.m_name = New String(Me.m_name.Count - 1) {}
                For i As Integer = 0 To Me.m_name.Count - 1
                    newObject.m_name(i) = Me.m_name(i)
                Next
            End If

            ' Handle Domain
            newObject._domain = Nothing
            If Me._domain IsNot Nothing Then
                newObject._domain = New String(Me._domain.Count - 1) {}
                For i As Integer = 0 To Me._domain.Count - 1
                    newObject._domain(i) = Me._domain(i)
                Next
            End If

            ' Handle Values
            If Me._values IsNot Nothing Then
                newObject._values = Me._values.CreateCopy()
                newObject._values.SetVariable(newObject)
            End If

            ' Handle EliminationValue
            newObject._eliminationValue = Nothing
            If Me.EliminationValue IsNot Nothing Then
                newObject.SetEliminationValue(Me.EliminationValue.Code)
            End If

            'Handle Notes
            newObject._notes = Nothing
            If Me._notes IsNot Nothing Then
                newObject._notes = New Notes(Me._notes.Count - 1) {}
                For i As Integer = 0 To Me._notes.Count - 1
                    If Not Me._notes(i) Is Nothing Then
                        newObject._notes(i) = Me._notes(i).CreateCopy()
                    End If
                Next
            End If

            ' Handle Hierarchy
            newObject._hierarchy = Nothing
            If Me._hierarchy IsNot Nothing Then
                newObject._hierarchy = Me._hierarchy.CreateCopy()
            End If

            ' Handle Partitions
            newObject._partitions = Nothing
            If Me._partitions IsNot Nothing Then
                newObject._partitions = Me._partitions.CreateCopy()
            End If

            ' Handle ExtendedProperties
            newObject._extendedProperties = Nothing
            If Me._extendedProperties IsNot Nothing Then
                newObject._extendedProperties = Me._extendedProperties.CreateCopy()
            End If

            ' Handle ValueSets
            newObject._valueSets = Nothing
            If Me._valueSets IsNot Nothing Then
                newObject._valueSets = New List(Of ValueSetInfo)
                For Each vsi As ValueSetInfo In Me._valueSets
                    newObject._valueSets.Add(vsi.CreateCopy())
                Next
            End If

            ' Handle Groupings
            newObject._groupings = Nothing
            If Me._groupings IsNot Nothing Then
                newObject._groupings = New List(Of GroupingInfo)(Me._groupings.Count - 1) {}
                For i As Integer = 0 To Me._groupings.Count - 1
                    If Me._groupings(i) IsNot Nothing Then
                        newObject._groupings(i) = New List(Of GroupingInfo)
                        For Each gi As GroupingInfo In Me._groupings(i)
                            newObject._groupings(i).Add(gi.CreateCopy())
                        Next
                    End If
                Next
            End If

            ' Handle CurrentGrouping
            newObject._currentGrouping = Nothing
            If Me._currentGrouping IsNot Nothing Then
                newObject._currentGrouping = Me._currentGrouping.CreateCopy()
            End If

            ' Handle CurrentValueSet
            newObject._currentValueSet = Nothing
            If Me._currentValueSet IsNot Nothing Then
                newObject._currentValueSet = Me._currentValueSet.CreateCopy()
            End If

            Return newObject
        End Function

        ''' <summary>
        ''' Create a deep copy of Me including the Variable.Values
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateCopyWithValues() As Variable
            Dim var As Variable = Me.CreateCopy()

            ' Values are copied in the CreateCopy function
            'Dim val As Value

            'For Each value As Value In Me.Values
            '    val = value.CreateCopy
            '    var.Values.Add(val)
            'Next

            Return var
        End Function

        ''' <summary>
        ''' Builds the Timevalues string
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BuildTimeValuesString()
            If Not Me.HasTimeValue Then
                Me.TimeValue = ""
                Exit Sub
            End If

            If Not Me.VerifyTimeValueSeries Then
                Me.TimeValue = ""
                Exit Sub
            End If

            If Me.Values.Count = 0 Then
                Me.TimeValue = ""
                Exit Sub
            End If

            Dim sb As New System.Text.StringBuilder

            sb.Append("TLIST(")

            Select Case Me.TimeScale
                Case TimeScaleType.Annual
                    sb.Append("A1")
                Case TimeScaleType.Halfyear
                    sb.Append("H1")
                Case TimeScaleType.Quartely
                    sb.Append("Q1")
                Case TimeScaleType.Monthly
                    sb.Append("M1")
                Case TimeScaleType.Weekly
                    sb.Append("W1")
                Case Else
                    Me.TimeValue = ""
                    Exit Sub
            End Select

            sb.Append(")")

            For Each value As Value In Me.Values
                sb.Append(",""" & value.TimeValue & """")
            Next

            Me.TimeValue = sb.ToString
        End Sub

        ''' <summary>
        ''' If the variable is a timevariable, this method verifies that the timevalues
        ''' are a valid timeseries (the values must come in succuession after each other)
        ''' </summary>
        ''' <returns>True if timevalues are a valid timeseries, else false</returns>
        ''' <remarks></remarks>
        Private Function VerifyTimeValueSeries() As Boolean
            Dim year1 As Integer
            Dim period1 As Integer
            Dim year2 As Integer
            Dim period2 As Integer

            If Not Me.HasTimeValue Then
                Return False
            End If

            If Me.Values.Count = 0 Then
                Return False
            ElseIf Me.Values.Count = 1 Then
                Return True
            End If

            If String.IsNullOrEmpty(Me.Values(0).TimeValue) Then
                Return False
            End If

            year1 = CInt(Me.Values(0).TimeValue.Substring(0, 4))

            If Me.TimeScale = TimeScaleType.NotSet Then
                Return False
            ElseIf Me.TimeScale = TimeScaleType.Annual Then
                For i As Integer = 1 To Me.Values.Count - 1
                    If String.IsNullOrEmpty(Me.Values(1).TimeValue) Then
                        Return False
                    End If

                    year2 = CInt(Me.Values(i).TimeValue.Substring(0, 4))
                    If Not year2.Equals(year1 + 1) Then
                        Return False
                    End If
                    year1 = year2
                Next
            Else
                period1 = CInt(Me.Values(0).TimeValue.Substring(4))

                For i As Integer = 1 To Me.Values.Count - 1
                    If String.IsNullOrEmpty(Me.Values(1).TimeValue) Then
                        Return False
                    End If

                    year2 = CInt(Me.Values(i).TimeValue.Substring(0, 4))
                    period2 = CInt(Me.Values(i).TimeValue.Substring(4))

                    If year2 < year1 Then
                        Return False
                    End If

                    If Not period2.Equals(period1 + 1) Then
                        If Not year2.Equals(year1 + 1) Then
                            Return False
                        End If

                        If Not period2.Equals(1) Then
                            Return False
                        End If

                        Select Case Me.TimeScale
                            Case TimeScaleType.Halfyear
                                If Not period1.Equals(2) Then
                                    Return False
                                End If
                            Case TimeScaleType.Monthly
                                If Not period1.Equals(12) Then
                                    Return False
                                End If
                            Case TimeScaleType.Quartely
                                If Not period1.Equals(4) Then
                                    Return False
                                End If
                            Case TimeScaleType.Weekly
                                If Not (period1.Equals(52) Or period1.Equals(53)) Then
                                    Return False
                                End If
                        End Select
                    ElseIf Not year2.Equals(year1) Then
                        'If periods in succession the year must be the same
                        Return False
                    End If

                    year1 = year2
                    period1 = period2
                Next
            End If

            Return True
        End Function

#End Region

#Region "Language stuff"

        Protected Friend Sub ResizeLanguageVariables(ByVal size As Integer)
            Util.ResizeLanguageDependentFields(Me, size)

            For i As Integer = 0 To _values.Count - 1
                _values(i).ResizeLanguageVariables(size)
            Next
            For i As Integer = 0 To _valueSets.Count - 1
                _valueSets(i).ResizeLanguageVariables(size)
            Next
            _hierarchy.ResizeLanguageVariables(size)
        End Sub

        ''' <summary>
        ''' Sets the current language as the default language of the model.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Protected Friend Sub SetCurrentLanguageDefault()
            If mLanguageIndex > 0 Then
                'Switch text values between current language and the old default language
                Util.SwitchLanguages(Me, 0, mLanguageIndex)

                For i As Integer = 0 To _values.Count - 1
                    _values(i).SetCurrentLanguageDefault()
                Next
                For i As Integer = 0 To _valueSets.Count - 1
                    _valueSets(i).SetCurrentLanguageDefault()
                Next

                _hierarchy.SetCurrentLanguageDefault()
            End If
        End Sub

        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
            For i As Integer = 0 To _values.Count - 1
                _values(i).SetLanguage(languageIndex)
            Next

            For i As Integer = 0 To _valueSets.Count - 1
                _valueSets(i).SetLanguage(languageIndex)
            Next

            _hierarchy.SetLanguage(languageIndex)
        End Sub

#End Region

        ''' <summary>
        ''' Gets a Valueset by the id
        ''' </summary>
        ''' <param name="Id">id of the valueset</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetValuesetById(ByVal Id As String) As ValueSetInfo
            For i As Integer = 0 To Me.ValueSets.Count - 1
                If Id = Me.ValueSets(i).ID Then
                    Return Me.ValueSets(i)
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Get a GroupingInfo object by its identifier
        ''' </summary>
        ''' <param name="Id">the id of the GroupingInfo object</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGroupingInfoById(ByVal Id As String) As GroupingInfo
            For i As Integer = 0 To Me.Groupings.Count - 1
                If Id = Me.Groupings(i).ID Then
                    Return Me.Groupings(i)
                End If
            Next
            Return Nothing
        End Function


        ''' <summary>
        ''' Creates a new empty collection for the values
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecreateValues()
            _values = New Values()
            _values.SetVariable(Me)
        End Sub

        ''' <summary>
        ''' Constructor used by Serialization
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            m_name = CType(info.GetValue("Name", GetType(String())), String())
            _time = info.GetString("Time")
            _domain = CType(info.GetValue("Domain", GetType(String())), String())
            _map = info.GetString("Map")
            _presentationText = info.GetInt32("PresentationText")
            _doubleColumn = info.GetBoolean("DoubleColumn")
            _placement = CType(info.GetInt32("Placement"), PlacementType)
            _isContentVariable = info.GetBoolean("ContentVariable")
            _variableType = info.GetString("VariableType")
            _values = CType(info.GetValue("Values", GetType(Values)), Values)
            _values.SetVariable(Me)
            _notes = CType(info.GetValue("Notes", GetType(Notes())), Notes())
            _partitions = CType(info.GetValue("Partitions", GetType(Partitions)), Partitions)
            _hierarchy = CType(info.GetValue("Hierarchy", GetType(Hierarchy)), Hierarchy)
            _code = info.GetString("Code")
            _elimination = info.GetBoolean("Elimination")
            Dim c As String = info.GetString("EliminationValue")
            If Not String.IsNullOrEmpty(c) Then
                _eliminationValue = _values.GetByCode(c)
            End If

        End Sub

        ''' <summary>
        ''' Serialization functionality
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Name", m_name) 'array
            info.AddValue("Time", _time)
            info.AddValue("Domain", _domain) 'array
            info.AddValue("Map", _map)
            info.AddValue("PresentationText", _presentationText)
            info.AddValue("DoubleColumn", _doubleColumn)
            info.AddValue("Placement", _placement)
            info.AddValue("ContentVariable", _isContentVariable)
            info.AddValue("VariableType", _variableType)
            info.AddValue("Values", _values)
            info.AddValue("Notes", _notes) 'array
            info.AddValue("Partitions", _partitions)
            info.AddValue("Hierarchy", _hierarchy)
            info.AddValue("Code", _code)
            info.AddValue("Elimination", _elimination)
            If _eliminationValue IsNot Nothing Then
                info.AddValue("EliminationValue", _eliminationValue.Code) 'array
            Else
                info.AddValue("EliminationValue", "") 'array
            End If

        End Sub

    End Class




End Namespace