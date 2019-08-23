Imports PCAxis.Paxiom.Extensions

Namespace PCAxis.Paxiom
    ''' <summary>
    ''' Adapter class for IPXModelAdapeter making it simpler to implement a model builder
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class PXModelBuilderAdapter
        Implements IPXModelBuilder


        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PXModelBuilderAdapter))

#Region "Private members"

        Protected m_path As String
        Protected m_parser As PCAxis.Paxiom.IPXModelParser
        Protected m_selectedLanguage As String = Nothing
        Protected m_readAllLanguages As Boolean
        Protected m_preferredLanguage As String
        Protected m_builderState As ModelBuilderStateType = ModelBuilderStateType.Created

#End Region

#Region "IPXModelBuilder implementation"

        ''' <summary>
        ''' The model which is created by the builder
        ''' </summary>
        ''' <remarks>this could be modiefied and recreated during diffrent phases of the builder</remarks>
        Protected m_model As New PCAxis.Paxiom.PXModel

        ''' <summary>
        ''' The model which is created by the builder
        ''' </summary>
        ''' <value>The model which is created by the builder</value>
        ''' <returns>The model which is created by the builder</returns>
        ''' <remarks>The model object will contain diffrent amount of information during the diffrent phases of the build</remarks>
        Public ReadOnly Property Model() As PXModel Implements IPXModelBuilder.Model
            Get
                Return m_model
            End Get
        End Property

        Private _DoNotApplyCurrentValueSet As Boolean = False

        Public Property DoNotApplyCurrentValueSet() As Boolean Implements IPXModelBuilder.DoNotApplyCurrentValueSet
            Get
                Return _DoNotApplyCurrentValueSet
            End Get
            Set(ByVal value As Boolean)
                _DoNotApplyCurrentValueSet = value
            End Set
        End Property

        ''' <summary>
        ''' Path to the table of the builder model
        ''' </summary>
        ''' <value>Path to the table of the builder model</value>
        ''' <returns>Path to the table of the builder model</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Path() As String Implements IPXModelBuilder.Path
            Get
                If String.IsNullOrEmpty(m_path) Then
                    Return ""
                Else
                    Return m_path
                End If
            End Get
        End Property

        ''' <summary>
        ''' Sets the path of the data cube. Implementors must override this method
        ''' </summary>
        ''' <param name="path">the identification/url necessary so that the builder 
        ''' should be able to find the right data source</param>
        ''' <remarks>The path may vary between diffrent builder implementations 
        ''' because these often requier diffrent information</remarks>
        Public Overridable Sub SetPath(ByVal path As String) Implements IPXModelBuilder.SetPath

        End Sub

        ''' <summary>
        ''' Builds the model according to the specified selection. 
        ''' Implementors must overrides this method
        ''' </summary>
        ''' <param name="selection">The selection of values that should be in the 
        ''' resulting model after the build</param>
        ''' <remarks></remarks>
        Public MustOverride Function BuildForPresentation(ByVal selection() As Selection) As Boolean Implements IPXModelBuilder.BuildForPresentation


        'TODO: Petros Prio A _Preferred Language
        ''' <summary>
        ''' Reads in necessary metadata so that the user can do a selection.
        ''' </summary>
        ''' <remarks>This implemntation adds codes to the values if they where not 
        ''' suplied by the builder</remarks>
        Public Overridable Function BuildForSelection() As Boolean Implements IPXModelBuilder.BuildForSelection
            BeginBuildForSelection()

            m_parser.ParseMeta(AddressOf Me.SetMeta, "") '_PreferredLanguage)

            If Errors.Count > 0 Then
                Logger.WarnFormat("Error in file {0}, Error type = {1}", m_path, GetLocalizedString(Errors(0).Code))
                EndBuildForSelection()
                Return False
            End If

            'TITLE or DESCRIPTION must exist
            With Me.Model.Meta
                If String.IsNullOrEmpty(.Title) And String.IsNullOrEmpty(.Description) Then
                    .CreateTitle()
                    If String.IsNullOrEmpty(.Title) Then
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.TITLE_OR_DESCRIPTION_MISSING))
                    End If
                End If
            End With

            CheckMeta()

            'Adds fictional codes for all values if codes is missing.
            For i As Integer = 0 To Me.Model.Meta.Variables.Count - 1
                If Not Me.Model.Meta.Variables(i).Values.ValuesHaveCodes Then
                    Me.Model.Meta.Variables(i).Values.SetFictionalCodes()
                End If
            Next

            EndBuildForSelection()

            If Me.Errors.Count > 0 Then
                Logger.WarnFormat("Fatal error in file {0}", m_path)
                Return False
            End If


            Return True

        End Function

        ''' <summary>
        ''' Performs basic checks that Meta is ok. 
        ''' For fatal errors, errors are added to the builder.
        ''' For non-fatal errors, warnings are added to the builder.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CheckMeta()

            With Me.Model.Meta
                'At least one variable must exist
                If Me.m_model.Meta.Variables.Count = 0 Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.STUB_AND_HEADING_MISSING))
                End If

                'Every variable must have at least one value
                For Each variable As Variable In .Variables
                    If variable.Values.Count = 0 Then
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.VALUES))
                    End If
                Next

                'Check mandatory keywords...

                'TITLE + DESCRIPTION
                If String.IsNullOrEmpty(.Title) AndAlso String.IsNullOrEmpty(.Description) Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.TITLE_OR_DESCRIPTION_MISSING))
                End If

                'MATRIX
                If String.IsNullOrEmpty(.Matrix) Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.MATRIX_MISSING))
                End If

                'SUBJECT-AREA
                If .SubjectArea Is Nothing Then '"" is a valid value for SUBJECT_AREA
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.SUBJECT_AREA_MISSING))
                End If

                'SUBJECT-CODE
                If String.IsNullOrEmpty(.SubjectCode) Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.SUBJECT_CODE_MISSING))
                End If

                'CONTENTS
                If String.IsNullOrEmpty(.Contents) Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.CONTENTS_MISSING))
                End If

                'DECIMALS
                If .Decimals.Equals(-1) Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.DECIMALS_MISSING))
                End If

                'UNITS
                If .ContentInfo Is Nothing Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.UNITS_MISSING))
                Else
                    If Not .ContentInfo.CheckUnits Then
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.UNITS_MISSING))
                    End If
                End If

                'Additional checks...

                'Cannot have DESCRIPTIONDEFAULT if there is no DESCRIPTION
                If .DescriptionDefault And String.IsNullOrEmpty(.Description) Then
                    .DescriptionDefault = False
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.DESCRIPTIONDEFAULT))
                End If
            End With

        End Sub

        ''' <summary>
        ''' This methods is called before the parser ReadMeta methods is called.
        ''' Implementors can override this method if they want to do something before 
        ''' ReadMeta is called
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub BeginBuildForSelection()
            Me.m_builderState = ModelBuilderStateType.BuildingForSelection
            Logger.Debug("Start BuildForSelection at " & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"))
        End Sub

        ''' <summary>
        ''' This methods is called after the parser ReadMeta methods is called.
        ''' Implementors can override this method if they want to do something after 
        ''' ReadMeta is called
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub EndBuildForSelection()
            If Not String.IsNullOrEmpty(m_preferredLanguage) Then
                If Model.Meta.HasLanguage(m_preferredLanguage) Then
                    Model.Meta.SetLanguage(m_preferredLanguage)
                End If
            End If
            Me.m_builderState = ModelBuilderStateType.BuildForSelection
            Logger.Debug("End BuildForSelection at " & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"))
        End Sub

#End Region


#Region "Build Meta"


        ''' <summary>
        ''' Callback function passed to the parser
        ''' </summary>
        ''' <param name="keyword">The keyword</param>
        ''' <param name="language">The language selector should be Nothing/null for the default language</param>
        ''' <param name="subkey">The subkey. Should be Nothing/null if no subkey is present</param>
        ''' <param name="values">The values for the specified key</param>
        ''' <returns>True if keyword was succesfully added otherwise False.</returns>
        ''' <remarks></remarks>
        Private Function SetMeta(ByVal keyword As String, ByVal language As String, ByVal subkey As String, ByVal values As System.Collections.Specialized.StringCollection) As Boolean

            'Changes the language if 
            If language <> m_selectedLanguage Then
                m_selectedLanguage = language
                If Not m_model.Meta.HasLanguage(language) Then
                    Return False
                End If
                m_model.Meta.SetLanguage(language)
            End If
            Dim isDefaultLanguage As Boolean = language Is Nothing
            SetMeta(keyword, subkey, values, m_model.Meta, isDefaultLanguage)

            If Me.Errors.Count > 0 Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Creates the right data structures i the meta part of the model by using
        ''' the parameters supplied
        ''' </summary>
        ''' <param name="keyword">The keyword where often the same keyword that also 
        ''' exisit in the PX file format</param>
        ''' <param name="subkey">this is often the name of the variable</param>
        ''' <param name="values">the values that should be set for the given keyword</param>
        ''' <param name="meta">the PXMeta object to modify</param>
        ''' <param name="isDefaultLanguage"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Sub SetMeta(ByVal keyword As String, ByVal subkey As String, ByVal values As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDefaultLanguage As Boolean)
            'TODO Check some of the keywords
            'They should only be set if it is the default language.
            If Logger.IsDebugEnabled Then
                Logger.Debug(String.Format("KEYWORD={0}, SUBKEY={1}", keyword, subkey))
            End If
            Select Case keyword
                Case PXKeywords.HEADING
                    CreateHeadingVariables(values, meta)
                Case PXKeywords.STUB
                    CreateStubVariables(values, meta, isDefaultLanguage)
                Case PXKeywords.VALUES
                    CreateValues(subkey, values, meta)
                Case PXKeywords.CODES
                    SetValueCodes(subkey, values, meta)
                Case PXKeywords.DOMAIN
                    SetDomain(subkey, values(0), meta)
                Case PXKeywords.MAP
                    SetMap(subkey, values(0), meta)
                Case PXKeywords.ELIMINATION
                    SetElimination(subkey, values(0), meta)
                Case PXKeywords.CHARSET
                    meta.Charset = values(0)
                Case PXKeywords.AXIS_VERSION
                    meta.AxisVersion = values(0)
                Case PXKeywords.COPYRIGHT
                    If Not GetBoolean(values(0), meta.Copyright) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.COPYRIGHT_IS_WRONG))
                    End If
                Case PXKeywords.CREATION_DATE
                    meta.CreationDate = values(0)
                Case PXKeywords.SUBJECT_AREA
                    If Not values(0) Is Nothing Then '"" is a valid value for SUBJECT_AREA 
                        meta.SubjectArea = values(0)
                    Else
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.SUBJECT_AREA_MISSING))
                    End If
                Case PXKeywords.SUBJECT_CODE
                    If Not String.IsNullOrEmpty(values(0)) Then
                        meta.SubjectCode = values(0)
                    Else
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.SUBJECT_CODE_MISSING))
                    End If
                Case PXKeywords.MATRIX
                    If Not String.IsNullOrEmpty(values(0)) Then
                        meta.Matrix = values(0)
                    Else
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.MATRIX_MISSING))
                    End If
                Case PXKeywords.DESCRIPTION
                    meta.Description = values(0)
                Case PXKeywords.DESCRIPTIONDEFAULT
                    If Not GetBoolean(values(0), meta.DescriptionDefault) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.DESCRIPTIONDEFAULT))
                    End If
                Case PXKeywords.TITLE
                    meta.Title = values(0)
                Case PXKeywords.CONTENTS
                    If Not String.IsNullOrEmpty(values(0)) Then
                        meta.Contents = values(0)
                    Else
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.CONTENTS_MISSING))
                    End If
                Case PXKeywords.UNITS
                    If Not values(0) Is Nothing Then
                        CreateContentInfo(subkey, keyword, values(0), meta)
                    Else
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.UNITS_MISSING))
                    End If
                Case PXKeywords.DECIMALS
                    If Not Integer.TryParse(values(0), meta.Decimals) Then
                        'Mandatory keyword - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.DECIMALS_MISSING))
                    End If
                Case PXKeywords.SHOWDECIMALS
                    Dim decimals As Integer

                    If Not Integer.TryParse(values(0), decimals) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.SHOWDECIMALS))
                    Else
                        If (decimals < 0) Or (decimals > 6) Then
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.SHOWDECIMALS))
                        Else
                            If meta.Decimals < decimals Then
                                'SHOWDECIMALS cannot be bigger than DECIMALS
                                Me.Warnings.Add(New BuilderMessage(ErrorCodes.SHOWDECIMALS))
                            Else
                                meta.ShowDecimals = decimals
                            End If
                        End If
                    End If
                Case PXKeywords.SOURCE
                    meta.Source = values(0)
                Case PXKeywords.DATABASE
                    meta.Database = values(0)
                Case PXKeywords.DOUBLECOLUMN
                    SetDoubleColumn(subkey, values(0), meta)
                Case PXKeywords.AUTOPEN
                    If Not GetBoolean(values(0), meta.AutoOpen) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.AUTOPEN))
                    End If
                Case PXKeywords.INFOFILE
                    meta.InfoFile = values(0)
                Case PXKeywords.FIRST_PUBLISHED
                    meta.FirstPublished = values(0)
                Case PXKeywords.BASEPERIOD, PXKeywords.CFPRICES, PXKeywords.CONTACT, PXKeywords.LAST_UPDATED, PXKeywords.REFPERIOD, PXKeywords.SEASADJ, PXKeywords.DAYADJ, PXKeywords.STOCKFA, PXKeywords.UNITS, PXKeywords.REFRENCE_ID
                    CreateContentInfo(subkey, keyword, values(0), meta)
                Case PXKeywords.DATANOTE
                    SetDatanote(subkey, values(0), meta)
                Case PXKeywords.META_ID
                    SetMetaId(subkey, values(0), meta)
                Case PXKeywords.NOTE
                    For Each footnote As String In HandleFootnotes(values(0))
                        CreateNote(subkey, footnote, False, meta)
                    Next
                Case PXKeywords.NOTEX
                    For Each footnote As String In HandleFootnotes(values(0))
                        CreateNote(subkey, footnote, True, meta)
                    Next
                Case PXKeywords.VALUENOTE
                    For Each footnote As String In HandleFootnotes(values(0))
                        CreateValueNote(subkey, footnote, False, meta)
                    Next
                Case PXKeywords.VALUENOTEX
                    For Each footnote As String In HandleFootnotes(values(0))
                        CreateValueNote(subkey, footnote, True, meta)
                    Next
                Case PXKeywords.CELLNOTE
                    For Each footnote As String In HandleFootnotes(values(0))
                        SetCellNote(subkey, footnote, False, meta)
                    Next
                Case PXKeywords.CELLNOTEX
                    For Each footnote As String In HandleFootnotes(values(0))
                        SetCellNote(subkey, footnote, True, meta)
                    Next
                Case PXKeywords.CONTVARIABLE
                    SetContentVariable(values(0), meta)
                Case PXKeywords.PRECISION
                    SetPrecision(subkey, values(0), meta)
                Case PXKeywords.KEYS
                    SetKeys(subkey, values(0), meta)
                Case PXKeywords.TIMEVAL
                    If isDefaultLanguage Then
                        SetTimeValue(subkey, values, meta)
                    End If
                    CreateTimeValues(subkey, values, meta)
                Case PXKeywords.DATASYMBOL1
                    meta.DataSymbol1 = values(0)
                Case PXKeywords.DATASYMBOL2
                    meta.DataSymbol2 = values(0)
                Case PXKeywords.DATASYMBOL3
                    meta.DataSymbol3 = values(0)
                Case PXKeywords.DATASYMBOL4
                    meta.DataSymbol4 = values(0)
                Case PXKeywords.DATASYMBOL5
                    meta.DataSymbol5 = values(0)
                Case PXKeywords.DATASYMBOL6
                    meta.DataSymbol6 = values(0)
                    'Case PXKeywords.DATASYMBOL7
                    '    meta.DataSymbol7 = values(0)
                Case PXKeywords.DATASYMBOLNIL
                    meta.DataSymbolNIL = values(0)
                Case PXKeywords.DATASYMBOLSUM
                    meta.DataSymbolSum = values(0)
                Case PXKeywords.UPDATE_FREQUENCY
                    meta.UpdateFrequency = values(0)
                Case PXKeywords.NEXT_UPDATE
                    meta.NextUpdate = values(0)
                Case PXKeywords.PX_SERVER
                    meta.PXServer = values(0)
                Case PXKeywords.DIRECTORY_PATH
                    meta.DirectoryPath = values(0)
                Case PXKeywords.INFO
                    meta.Information = values(0)
                Case PXKeywords.LINK
                    meta.Link = values(0)
                Case PXKeywords.SURVEY
                    meta.Survey = values(0)
                Case PXKeywords.TABLEID
                    meta.TableID = values(0)
                Case PXKeywords.DEFAULT_GRAPH
                    Dim graph As Integer
                    If Integer.TryParse(values(0), graph) Then
                        meta.DefaultGraph = graph
                    End If
                Case PXKeywords.PRESTEXT
                    SetPresentationText(subkey, values(0), meta)
                Case PXKeywords.VARIABLE_TYPE
                    SetVariableType(subkey, values(0), meta)
                Case PXKeywords.DATANOTESUM
                    meta.DataNoteSum = values(0)
                Case PXKeywords.LANGUAGE
                    meta.Language = values(0)
                Case PXKeywords.AGGREGALLOWED
                    If Not GetBoolean(values(0), meta.AggregAllowed) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.AGGREGALLOWED))
                    End If
                Case PXKeywords.PARTITIONED
                    SetPartitioned(subkey, values, meta, isDefaultLanguage)
                Case PXKeywords.CONFIDENTIAL
                    If Not Integer.TryParse(values(0), meta.Confidential) Then
                        meta.Confidential = 0
                    End If
                Case PXKeywords.LANGUAGES
                    Dim langs(values.Count - 1) As String
                    values.CopyTo(langs, 0)
                    meta.AddLanguages(langs)
                Case PXKeywords.HIERARCHYLEVELSOPEN
                    SetHierarchyLevelOpen(subkey, values(0), meta, isDefaultLanguage)
                Case PXKeywords.HIERARCHIES
                    SetHierarchy(subkey, values, meta, isDefaultLanguage)
                Case PXKeywords.HIERARCHYNAMES
                    SetHierarchyNames(subkey, values, meta)
                Case PXKeywords.HIERARCHYLEVELS
                    SetHierarchyLevels(subkey, values(0), meta, isDefaultLanguage)
                Case PXKeywords.DATANOTECELL
                    SetDataNoteCell(subkey, values(0), meta)
                Case PXKeywords.VARIABLE_CODE
                    SetVariableCode(subkey, values(0), meta)
                Case PXKeywords.CODEPAGE
                    meta.CodePage = values(0)
                Case PXKeywords.VALUESET_ID
                    CreateValueSets(values, subkey, meta)
                Case PXKeywords.VALUESET_NAME
                    SetValueSetNames(values, subkey, meta)
                Case PXKeywords.ROUNDING
                    SetRounding(values(0), meta)
                Case PXKeywords.SYNONYMS
                    meta.Synonyms = values(0)
                Case PXKeywords.VALUE_TEXT_OPTION
                    SetValueTextOption(subkey, values(0), meta)
                Case PXKeywords.MAINTABLE
                    meta.MainTable = values(0)
                Case PXKeywords.OFFICIAL_STATISTICS
                    If Not GetBoolean(values(0), meta.OfficialStatistics) Then
                        'Optional keyword - Add warning
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.OFFICIAL_STATISTICS))
                    End If
                Case PXKeywords.ATTRIBUTE_ID
                    CreateAttributeIDs(values, meta)
                Case PXKeywords.ATTRIBUTE_TEXT
                    CreateAttributeNames(values, meta)
                Case PXKeywords.ATTRIBUTES
                    SetAttributes(subkey, values, meta)
                Case "KEYWORD_ERROR"
                    'Something was wrong with the keyword
                    HandleKeywordError(values(0))
                Case Else
                    Logger.Debug("Unknown keyword: " & keyword & " created extended property")
                    CreateExtendedProperty(keyword, values(0), subkey, meta)

            End Select

        End Sub

        ''' <summary>
        ''' Splits a footnote text containing ## into two different footnotes. The character # is translated to CrLf.
        ''' </summary>
        ''' <param name="inputString">The input footnote string</param>
        ''' <returns>A list of footnote string</returns>
        ''' <remarks></remarks>
        Private Function HandleFootnotes(ByVal inputString As String) As List(Of String)
            Dim separator As String() = {"##"}
            Dim arrFootnotes As String() = inputString.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            Dim footnotes As New List(Of String)

            For Each footnote As String In arrFootnotes
                footnotes.Add(footnote.Replace("#", ControlChars.CrLf))
            Next

            Return footnotes
        End Function

        ''' <summary>
        ''' Handles the error when "KEYWORD_ERROR is returned from the parser"
        ''' Adds a Error- or warning message depending on the the type of error.
        ''' </summary>
        ''' <param name="keyword">
        ''' The keyword 
        ''' </param>
        ''' <remarks></remarks>
        Private Sub HandleKeywordError(ByVal keyword As String)
            Dim info As PXKeywords.KeywordInfo

            If Not String.IsNullOrEmpty(keyword) Then
                info = PXKeywords.Keywords(keyword)

                If info Is Nothing Then
                    'Unknown keyword...
                    Errors.Add(New BuilderMessage("PxcKeywordError", New Object() {keyword}))
                    Exit Sub
                End If

                If info.IsMandantory Then
                    Select Case keyword
                        Case PXKeywords.DESCRIPTION
                            Warnings.Add(New BuilderMessage("PxcKeywordError", New Object() {keyword}))
                        Case Else
                            'Mandatory keyword - Add error
                            Errors.Add(New BuilderMessage("PxcKeywordError", New Object() {keyword}))
                    End Select
                Else
                    Select Case keyword
                        Case PXKeywords.CODES
                            Errors.Add(New BuilderMessage("PxcKeywordError", New Object() {keyword}))
                        Case Else
                            'Optional keyword - Add warning
                            Warnings.Add(New BuilderMessage("PxcKeywordError", New Object() {keyword}))
                    End Select
                End If

            End If
        End Sub

        ''' <summary>
        ''' Helper function to convert YES and NO values to booleans
        ''' </summary>
        ''' <param name="value">the value to convert</param>
        ''' <param name="defaultValue">the default value to return if value is not YES or NO</param>
        ''' <returns>Returns True if value is YES and False if value is NO</returns>
        ''' <remarks>the comparison ignores case</remarks>
        Public Shared Function ConvertToBoolean(ByVal value As String, ByVal defaultValue As Boolean) As Boolean
            If String.Compare(value, "YES", True) = 0 Then
                Return True
            End If

            If String.Compare(value, "NO", True) = 0 Then
                Return True
            End If

            Return defaultValue
        End Function


#Region "Set stuff"
        ''' <summary>
        ''' Sets the rounding rule for the cube
        ''' </summary>
        ''' <param name="value">the rounding rule</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetRounding(ByVal value As String, ByVal meta As PXMeta)
            Dim i As Integer

            If Integer.TryParse(value, i) Then
                If i = 0 Or i = 1 Then
                    meta.Rounding = CType(i, RoundingType)
                Else
                    Logger.Warn(value & ", No such rounding rule using default rule")
                    meta.Rounding = RoundingType.None
                End If

            Else
                Logger.Warn(value & ", No such rounding rule using default rule")
                meta.Rounding = RoundingType.None
            End If
        End Sub

        ''' <summary>
        ''' Creates the variables in the Heading collection
        ''' </summary>
        ''' <param name="variableNames">A collection of variable names that will be created and placed in the Heading</param>
        ''' <param name="meta"></param>
        ''' <remarks>If there already is variables created in the stub no variables will be created only there names will be set.</remarks>
        Private Sub CreateHeadingVariables(ByVal variableNames As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            If variableNames.Count = meta.Heading.Count Then
                For i As Integer = 0 To meta.Heading.Count - 1
                    If variableNames(i).Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                        'Sortvariable - Get name from languagemanager
                        meta.Heading(i).Name = meta.GetLocalizedSortVariableName(m_selectedLanguage)
                    Else
                        meta.Heading(i).Name = variableNames(i)
                    End If
                Next
            ElseIf meta.Heading.Count > 0 Then
                'Variable exists probably a diffrent language
                Logger.Error("Number of variables do not match existing amount")
            Else
                For Each n As String In variableNames
                    If String.IsNullOrEmpty(n) Then
                        'Invalid HEADING - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.HEADING))
                    End If

                    If n.Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                        'Sortvariable - Get name from languagemanager
                        v = New Variable(meta.GetLocalizedSortVariableName(m_model.Meta.Language), PXConstant.SORTVARIABLE, PlacementType.Heading, m_model.Meta.NumberOfLanguages - 1)
                        v.SortVariable = True
                    Else
                        v = New Variable(n, PlacementType.Heading, m_model.Meta.NumberOfLanguages - 1)
                    End If
                    meta.AddVariable(v)
                Next
            End If

        End Sub

        ''' <summary>
        ''' Creates the variables in the Stub collection 
        ''' </summary>
        ''' <param name="variableNames">A collection of variable names that will be created and placed in the Heading</param>
        ''' <param name="meta"></param>
        ''' <remarks>If there already is variables created in the stub no variables will be created only there names will be set.</remarks>
        Private Sub CreateStubVariables(ByVal variableNames As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDefaultLanguage As Boolean)
            Dim v As Variable

            If variableNames.Count = meta.Stub.Count And (Not isDefaultLanguage) Then
                For i As Integer = 0 To meta.Stub.Count - 1
                    If variableNames(i).Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                        'Sortvariable - Get name from languagemanager
                        meta.Stub(i).Name = meta.GetLocalizedSortVariableName(m_selectedLanguage)
                    Else
                        meta.Stub(i).Name = variableNames(i)
                    End If
                Next
            Else
                For Each n As String In variableNames
                    If String.IsNullOrEmpty(n) Then
                        'Invalid STUB - Add error
                        Me.Errors.Add(New BuilderMessage(ErrorCodes.STUB))
                    End If

                    If n.Equals(PCAxis.Paxiom.PXConstant.SORTVARIABLE) Then
                        'Sortvariable - Get name from languagemanager
                        v = New Variable(meta.GetLocalizedSortVariableName(m_model.Meta.Language), PXConstant.SORTVARIABLE, PlacementType.Stub, m_model.Meta.NumberOfLanguages - 1)
                        v.SortVariable = True
                    Else
                        v = New Variable(n, PlacementType.Stub, m_model.Meta.NumberOfLanguages - 1)
                    End If
                    meta.AddVariable(v)
                Next
            End If
        End Sub

        Protected MustOverride Function FindVariable(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal findId As String) As Variable
        Protected MustOverride Function FindVariable(ByVal meta As PCAxis.Paxiom.PXMeta, ByVal findId As String, ByVal lang As Integer) As Variable
        Protected MustOverride Function FindValue(ByVal variable As PCAxis.Paxiom.Variable, ByVal findId As String) As Value


        'TODO Multilang handeling
        ''' <summary>
        ''' Adds a extended property to the rigth property collection
        ''' </summary>
        ''' <param name="prop">The name of the extended property</param>
        ''' <param name="value">The value of the extended property</param>
        ''' <param name="variablename">The variable that the extenden property is connected to. If the property is for the hole table then this value should be set to null or a empty string</param>
        ''' <param name="meta"></param>
        ''' <remarks>The property is added either on a ExtendedProperties collection on a variable or in the Extended Properties collection of the model depending if a valid variablename was passed as a argument</remarks>
        Private Sub CreateExtendedProperty(ByVal prop As String, ByVal value As String, ByVal variablename As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            If String.IsNullOrEmpty(variablename) Then
                meta.ExtendedProperties.Add(prop, value)
            Else
                Dim v As Variable
                v = FindVariable(meta, variablename)
                If v Is Nothing Then
                    Exit Sub
                End If

                v.ExtendedProperties.Add(prop, value)
            End If

        End Sub

        ''' <summary>
        ''' Sets the variable code for a existing variable placed in the stub or the heading
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="code">the code of the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Protected Sub SetVariableCode(ByVal variablename As String, ByVal code As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Exit Sub
            End If

            v.SetCode(code)

        End Sub

        ''' <summary>
        ''' Sets the variable keys for a existing variable placed in the stub or the heading
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="keys">the code of the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetKeys(ByVal variablename As String, ByVal keys As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.KEYS))
                Exit Sub
            End If

            If String.Compare("VALUES", keys) = 0 Then
                v.Keys = KeysTypes.Value
            ElseIf String.Compare("CODES", keys) = 0 Then
                v.Keys = KeysTypes.Code
            Else
                v.Keys = KeysTypes.None
            End If

        End Sub

        ''' <summary>
        ''' Sets the variable code for a existing variable placed in the stub or the heading
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="doubleColumn">the code of the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetDoubleColumn(ByVal variablename As String, ByVal doubleColumn As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.DOUBLECOLUMN))
                Exit Sub
            End If

            v.DoubleColumn = ConvertToBoolean(doubleColumn, False)

        End Sub

        ''' <summary>
        ''' Creates values for an existing variable.
        ''' </summary>
        ''' <param name="variablename">The name of the variable</param>
        ''' <param name="valueNames">A collection of value names</param>
        ''' <param name="meta"></param>
        ''' <remarks>If the variable already have values it will only change there names according to the new names provided</remarks>
        Private Sub CreateValues(ByVal variablename As String, ByVal valueNames As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.VALUES))
                Exit Sub
            End If

            'Check value names
            For Each name As String In valueNames
                If String.IsNullOrEmpty(name) Then
                    'Invalid VALUE - Add error
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.VALUES))
                End If
            Next

            Dim vs(valueNames.Count - 1) As String
            valueNames.CopyTo(vs, 0)
            v.SetValues(vs)

        End Sub

        ''' <summary>
        ''' Set valuecodes for an existing variable
        ''' </summary>
        ''' <param name="variablename">Name of the variable</param>
        ''' <param name="codes">Collection of codes</param>
        ''' <param name="meta">Meta part of the model</param>
        ''' <remarks></remarks>
        Private Sub SetValueCodes(ByVal variablename As String, ByVal codes As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.CODES))
                Exit Sub
            End If

            Dim vs(codes.Count - 1) As String
            codes.CopyTo(vs, 0)
            v.SetValueCodes(vs)

        End Sub

        ''' <summary>
        ''' Sets the domain for a selected variable
        ''' </summary>
        ''' <param name="variablename">Name of the variable that the domain will be set for</param>
        ''' <param name="domain">The domain of the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetDomain(ByVal variablename As String, ByVal domain As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.DOMAIN))
                Exit Sub
            End If
            v.Domain = domain

        End Sub

        ''' <summary>
        ''' Sets the Map for a existing variable
        ''' </summary>
        ''' <param name="variablename">The name of the variable</param>
        ''' <param name="map">The Map</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetMap(ByVal variablename As String, ByVal map As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.MAP))
                Exit Sub
            End If
            v.Map = map

        End Sub

        ''' <summary>
        ''' Sets the presentation text for a existing variable
        ''' </summary>
        ''' <param name="variablename">The variable name</param>
        ''' <param name="presentationText">the presentation text for the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetPresentationText(ByVal variablename As String, ByVal presentationText As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRESTEXT))
                Exit Sub
            End If

            Dim i As Integer = 1
            If Integer.TryParse(presentationText, i) Then
                v.PresentationText = i
            Else
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRESTEXT))
                Logger.Warn("No prestext conversion error")
            End If
        End Sub

        ''' <summary>
        ''' Sets the variable type for a exisiting variable
        ''' </summary>
        ''' <param name="variablename">the variable name</param>
        ''' <param name="variableType">the variable type for the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetVariableType(ByVal variablename As String, ByVal variableType As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Exit Sub
            End If
            v.VariableType = variableType
        End Sub

        ''' <summary>
        ''' Set the elimination value for a existing variable
        ''' </summary>
        ''' <param name="variablename"></param>
        ''' <param name="elimination"></param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetElimination(ByVal variablename As String, ByVal elimination As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.ELIMINATION))
                Exit Sub
            End If
            v.SetElimination(elimination)

        End Sub


        ''' <summary>
        ''' Creates the content info for the table or for the contentvariable values
        ''' </summary>
        ''' <param name="valuename">name of the value in the content variable</param>
        ''' <param name="key">content info variable name</param>
        ''' <param name="value">content info variable value</param>
        ''' <param name="meta"></param>
        ''' <remarks>If the content info already exisit then the conentent info will not be created instead the content info variable will be set to the value provided</remarks>
        Public Sub CreateContentInfo(ByVal valuename As String, ByVal key As String, ByVal value As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            If valuename Is Nothing Then 'ContInfo för PX-filen
                If meta.ContentInfo Is Nothing Then
                    meta.ContentInfo = New ContInfo(m_model.Meta.NumberOfLanguages - 1)
                End If
                meta.ContentInfo.SetProperty(key, value)
                'If key = PXKeywords.UNITS Then
                '    meta.Units = value
                'End If
                'TODO(Fixa)
            Else 'ContInfo för ett visst värde för innehållsvariabeln
                If meta.ContentVariable Is Nothing Then
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.KEYWORD_DEPENDENT_ON_CONTVARIABLE_FOUND_BUT_NO_CONTENSTS_VARIABLE))
                    Exit Sub
                End If

                Dim v As Value
                v = FindValue(meta.ContentVariable, valuename)

                If v Is Nothing Then
                    Select Case key
                        Case "LAST-UPDATED"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.LAST_UPDATED_IS_WRONG))
                        Case "STOCKFA"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.STOCKFA_IS_WRONG))
                        Case "CFPRICES"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.CFPRICES_IS_WRONG))
                        Case "DAYADJ"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.DAYADJ_IS_WRONG))
                        Case "SEASADJ"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.SEASADJ_IS_WRONG))
                        Case "UNITS"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.UNITS))
                        Case "REFPERIOD"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.REFPERIOD_IS_WRONG))
                        Case "BASEPERIOD"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.BASEPERIOD_IS_WRONG))
                        Case "CONTACT"
                            Me.Warnings.Add(New BuilderMessage(ErrorCodes.CONTACT_IS_WRONG))
                    End Select
                    Exit Sub
                Else
                    v.SetContentInfo(key, value)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Creates a table or variable note
        ''' </summary>
        ''' <param name="variabelname">the name of the variable</param>
        ''' <param name="text">the note</param>
        ''' <param name="mandatory">if the note is mandatory</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub CreateNote(ByVal variabelname As String, ByVal text As String, ByVal mandatory As Boolean, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim n As Note

            If variabelname Is Nothing Then 'Table note
                n = New Note(text, NoteType.Table, mandatory)
                meta.Notes.Add(n)
            Else 'Variabel note
                Dim variable As Variable

                variable = FindVariable(meta, variabelname)

                If variable Is Nothing Then
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.NOTE_OR_NOTEX_FOR_VARIABLE))
                Else
                    n = New Note(text, NoteType.Variable, mandatory)
                    variable.AddNote(n)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Create a value note or value notex
        ''' </summary>
        ''' <param name="variableValueString">String containing variable and value</param>
        ''' <param name="text">Text of the note</param>
        ''' <param name="mandantory">If the note is mandatory</param>
        ''' <param name="meta">Meta part of the model</param>
        ''' <remarks></remarks>
        Private Sub CreateValueNote(ByVal variableValueString As String, ByVal text As String, ByVal mandantory As Boolean, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim sc As System.Collections.Specialized.StringCollection
            sc = SplittString(variableValueString)
            If sc.Count <> 2 Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.VALUENOTE))
                Exit Sub
            End If
            CreateValueNote(sc(0), sc(1), text, mandantory, meta)
        End Sub

        ''' <summary>
        ''' Creates a value note or value notex
        ''' </summary>
        ''' <param name="variableName">The variable name for the value</param>
        ''' <param name="valueName">The name of the variable</param>
        ''' <param name="text">The note</param>
        ''' <param name="mandantory">If the note is mandantory</param>
        ''' <param name="meta">Meta part of the model</param>
        ''' <remarks></remarks>
        Private Sub CreateValueNote(ByVal variableName As String, ByVal valueName As String, ByVal text As String, ByVal mandantory As Boolean, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim note As Note = New Note(text, NoteType.Value, mandantory)
            Dim variable As Variable
            Dim value As Value

            variable = FindVariable(meta, variableName)

            If variable Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.VALUENOTE))
            Else
                value = FindValue(variable, valueName)
                If value Is Nothing Then
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.VALUENOTE))
                Else
                    value.AddNote(note)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Set the content variable
        ''' </summary>
        ''' <param name="name">The name of the variable which will be set as contentvariable</param>
        ''' <param name="meta"></param>
        ''' <remarks>The variable with the given name must exist</remarks>
        Private Sub SetContentVariable(ByVal name As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim variable As Variable

            variable = FindVariable(meta, name)

            If variable Is Nothing Then
                meta.ContentVariable = Nothing
                Me.Errors.Add(New BuilderMessage(ErrorCodes.CONTVARIABLE_OR_DEPENDAENT_KEYWORD_ERROR))
            Else
                meta.ContentVariable = variable
                meta.ContentVariable.SetAsContentVariable(True)
            End If
        End Sub

        ''' <summary>
        ''' Set precision for a specifik value
        ''' </summary>
        ''' <param name="subkey">String identifying variable and value</param>
        ''' <param name="precision">Number of decimals wanted</param>
        ''' <param name="meta">The meta part of the model</param>
        ''' <remarks></remarks>
        Private Sub SetPrecision(ByVal subkey As String, ByVal precision As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim p As Integer = 0
            Dim sc As System.Collections.Specialized.StringCollection
            Dim variable As Variable
            Dim value As Value

            If Not Integer.TryParse(precision, p) Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRECISION))
                Exit Sub
            End If

            sc = SplittString(subkey)
            If sc.Count <> 2 Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRECISION))
                Exit Sub
            End If

            variable = FindVariable(meta, sc(0))
            If variable Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRECISION))
                Exit Sub
            End If

            value = FindValue(variable, sc(1))
            If value Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PRECISION))
                Exit Sub
            End If

            value.Precision = p

        End Sub


        ''' <summary>
        ''' Set the time value for the time variable
        ''' </summary>
        ''' <param name="variableName"></param>
        ''' <param name="values"></param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetTimeValue(ByVal variableName As String, ByVal values As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim sb As New System.Text.StringBuilder()
            Dim variable As Variable

            'If values.Count = 2 AndAlso values(1).Contains("""-""") Then
            If values.Count = 2 AndAlso values(1).Contains("-") Then
                Dim startYear As String
                Dim endYear As String
                Dim index As Integer = values(1).IndexOf("-")

                'startYear = values(1).Substring(index - 4, 4)
                startYear = values(1).Substring(0, 4)
                endYear = values(1).Substring(index + 1, 4)

                'TIMEVAL in interval format
                sb.Append(values(0))
                sb.Append(",")
                sb.Append("""")
                sb.Append(startYear)
                sb.Append("""-""")
                sb.Append(endYear)
                sb.Append("""")
                sb.Append(")")
            Else
                sb.Append(values(0))
                For i As Integer = 1 To values.Count - 1
                    sb.Append(",""")
                    sb.Append(values(i))
                    sb.Append("""")
                Next
            End If

            variable = FindVariable(meta, variableName)

            If variable Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.TIMEVAL_IS_WRONG))
            Else
                variable.TimeValue = sb.ToString()
                variable.IsTime = True
            End If
        End Sub

        Private Sub CreateTimeValues(ByVal variablename As String, ByVal valueNames As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Exit Sub
            End If

            If valueNames.Count > 1 Then
                Dim vs(valueNames.Count - 1) As String
                valueNames.CopyTo(vs, 0)
                v.SetTimeValues(vs)
            End If

        End Sub

        ''' <summary>
        ''' Sets the CellNotes
        ''' </summary>
        ''' <param name="region">a string representation for the region that the cellnote is
        '''  applicable for see PX file format</param>
        ''' <param name="text">the note</param>
        ''' <param name="mandantory">if the note should be mandantory</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetCellNote(ByVal region As String, ByVal text As String, ByVal mandantory As Boolean, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim regions As System.Collections.Specialized.StringCollection = SplittString(region)
            Dim value As String
            Dim var As Variable
            Dim val As Value
            Dim note As CellNote
            Dim conds As New VariableValuePairs
            Dim cond As VariableValuePair

            If regions.Count <> Me.m_model.Meta.Variables.Count Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.CELLNOTE))
                Exit Sub
            End If

            For i As Integer = 0 To regions.Count - 1
                value = regions(i).Trim()
                If value <> "*" Then
                    If i < meta.Stub.Count Then
                        var = meta.Stub(i)
                    Else
                        var = meta.Heading(i - meta.Stub.Count)
                    End If
                    'If not codes create fictional
                    If Not var.Values.ValuesHaveCodes Then
                        var.Values.SetFictionalCodes()
                    End If
                    val = FindValue(var, value)

                    If val Is Nothing Then
                        'Value does not exist - do not add cellnote
                        'Warnings.Add(New WarningMessage("PxcErrCellNoteValueMissing", New Object() {value}))
                        Warnings.Add(New BuilderMessage(ErrorCodes.CELLNOTE))
                        Exit Sub
                    Else
                        cond = New VariableValuePair(var.Code, val.Code)
                        conds.Add(cond)
                    End If
                End If
            Next
            note = New CellNote(conds)
            note.Text = text
            note.Mandatory = mandantory
            meta.CellNotes.Add(note)
        End Sub

        ''' <summary>
        ''' Sets the datanotecell
        ''' </summary>
        ''' <param name="dimensionValues">a string containing the variable values that identifies the cell</param>
        ''' <param name="text">the note</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetDataNoteCell(ByVal dimensionValues As String, ByVal text As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            ''Dim regions() As String
            ''regions = region.Split(New Char() {","c}, 17, StringSplitOptions.RemoveEmptyEntries)
            'Dim regions As System.Collections.Specialized.StringCollection = SplittString(region)
            'Dim value As String
            'Dim var As Variable
            'Dim val As Value
            Dim note As DataNoteCell
            'Dim conds As New VariableValuePairs
            'Dim cond As VariableValuePair
            'Dim byCode As Boolean = True
            ''For i As Integer = 0 To regions.Length - 1
            'For i As Integer = 0 To regions.Count - 1
            '    value = regions(i).Trim()
            '    If value <> "*" Then
            '        If i < meta.Stub.Count Then
            '            var = meta.Stub(i)
            '        Else
            '            var = meta.Heading(i - meta.Stub.Count)
            '        End If
            '        'If not codes create fictional
            '        If Not var.Values.ValuesHaveCodes Then
            '            var.Values.SetFictionalCodes()
            '        End If
            '        If var.Values.IsCodesFictional Then
            '            val = var.Values.GetByName(value)
            '        Else
            '            val = var.Values.GetByCode(value)
            '            'Some times the rule that if a code exisit the code is used is ignored
            '            If val Is Nothing Then
            '                val = var.Values.GetByName(value)
            '            End If
            '        End If

            '        If val Is Nothing Then
            '            'Value dose not exist - do not add cellnote
            '            Warnings.Add(New BuilderMessage(ErrorCodes.DATANOTE))
            '            Exit Sub
            '        Else
            '            cond = New VariableValuePair(var.Code, val.Code)
            '            conds.Add(cond)
            '        End If
            '    End If
            'Next
            Dim conds As VariableValuePairs

            conds = GetVariableValuePairs(dimensionValues, meta, ErrorCodes.DATANOTE)

            If Not conds Is Nothing Then
                note = New DataNoteCell(conds)
                note.Text = text
                meta.DataNoteCells.Add(note)
            End If

            'Dim note As CellNote = New CellNote
            'note.SetText(text)
            'note.SetMandantory(mandantory)
            ''note.SetRegion(region)
            'meta.CellNotes.Add(note)
        End Sub

        ''' <summary>
        ''' Get Variable-Value pairs for identifying a cell
        ''' </summary>
        ''' <param name="dimensionValues">
        ''' String containing the values identifying the cell. 
        ''' Example: '"0126","2","2007"'
        ''' </param>
        ''' <param name="meta">PXMeta object</param>
        ''' <param name="strErrorCode">The error code to display if something goes wrong</param>
        ''' <returns>The VariableValuePairs collection identifying the cell</returns>
        ''' <remarks></remarks>
        Private Function GetVariableValuePairs(ByVal dimensionValues As String, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal strErrorCode As String) As VariableValuePairs
            Dim dimensions As System.Collections.Specialized.StringCollection = SplittString(dimensionValues)
            Dim value As String
            Dim var As Variable
            Dim val As Value
            Dim conds As New VariableValuePairs
            Dim cond As VariableValuePair
            Dim byCode As Boolean = True

            For i As Integer = 0 To dimensions.Count - 1
                value = dimensions(i).Trim()
                If value <> "*" Then
                    If i < meta.Stub.Count Then
                        var = meta.Stub(i)
                    Else
                        var = meta.Heading(i - meta.Stub.Count)
                    End If
                    'If not codes create fictional
                    If Not var.Values.ValuesHaveCodes Then
                        var.Values.SetFictionalCodes()
                    End If
                    If var.Values.IsCodesFictional Then
                        val = var.Values.GetByName(value)
                    Else
                        val = var.Values.GetByCode(value)
                        'Some times the rule that if a code exisit the code is used is ignored
                        If val Is Nothing Then
                            val = var.Values.GetByName(value)
                        End If
                    End If

                    If val Is Nothing Then
                        'Value dose not exist - return nothing
                        Warnings.Add(New BuilderMessage(strErrorCode))
                        Return Nothing
                    Else
                        cond = New VariableValuePair(var.Code, val.Code)
                        conds.Add(cond)
                    End If
                End If
            Next

            Return conds
        End Function

        ''' <summary>
        ''' Sets the variable partion information
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="partition">partition iformation</param>
        ''' <param name="meta"></param>
        ''' <param name="isDafultLanguage"></param>
        ''' <remarks></remarks>
        Private Sub SetPartitioned(ByVal variablename As String, ByVal partition As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDafultLanguage As Boolean)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PARTITION))
                Exit Sub
            End If

            'Check that name and startindex is given
            If partition.Count < 2 Then
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.PARTITION))
                Exit Sub
            End If

            Dim value As Integer
            Dim startIndex As Integer
            Dim length As Integer
            If Integer.TryParse(partition(1), value) Then
                startIndex = value
                If partition.Count = 3 Then
                    If Integer.TryParse(partition(2), value) Then
                        length = value
                    End If
                Else
                    length = Integer.MaxValue
                End If
            End If

            Dim p As Partition
            If isDafultLanguage Then
                'TODO check if it already contains more then 3 partionons 
                p = New Partition(m_model.Meta.NumberOfLanguages - 1)
                p.StartIndex = startIndex
                p.Length = length
                v.Partitions.Add(p)
            Else
                p = v.Partitions.FindByValues(startIndex, length)
                If p Is Nothing Then
                    'TODO check if it already contains more then 3 partionons 
                    p = New Partition(m_model.Meta.NumberOfLanguages - 1)
                    p.StartIndex = startIndex
                    p.Length = length
                    v.Partitions.Add(p)
                End If
            End If

            p.Name = partition(0)

        End Sub

        ''' <summary>
        ''' Sets the information about the open leveels of a hierarchy
        ''' </summary>
        ''' <param name="variablename">name of the hierarcical variable</param>
        ''' <param name="level">the level number that should be opened</param>
        ''' <param name="meta"></param>
        ''' <param name="isDefaultLanguage"></param>
        ''' <remarks></remarks>
        Private Sub SetHierarchyLevelOpen(ByVal variablename As String, ByVal level As String, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDefaultLanguage As Boolean)
            'Only sets it for the default language
            If Not isDefaultLanguage Then
                Exit Sub
            End If

            'Get the variable 
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.HIERARCHYLEVELSOPEN))
                Exit Sub
            End If

            Dim levelNo As Integer
            If Not Integer.TryParse(level, levelNo) Then
                levelNo = 0
            End If

            v.Hierarchy.OpenLevel = levelNo
        End Sub

        ''' <summary>
        ''' Sets the hierarchy of a variable
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="codes">codes of the hierarchy</param>
        ''' <param name="meta"></param>
        ''' <param name="isDafultLanguage"></param>
        ''' <remarks></remarks>
        Private Sub SetHierarchy(ByVal variablename As String, ByVal codes As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDafultLanguage As Boolean)
            'Only sets it for the default language
            If Not isDafultLanguage Then
                Exit Sub
            End If

            'Get the variable 
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.HIERARCHIES))
                Exit Sub
            End If

            Dim levels As New Dictionary(Of String, HierarchyLevel)
            Dim rootLevels As New List(Of HierarchyLevel)

            Dim parent As String
            Dim child As String
            Dim pc() As String
            Dim delimiters() As Char = New Char() {":"c}
            Dim level As HierarchyLevel
            Dim parentLevel As HierarchyLevel

            For i As Integer = 0 To codes.Count - 1
                pc = codes(i).Split(delimiters)

                If pc.Length = 1 Then
                    parent = pc(0)
                    If parent.StartsWith("""") Then
                        parent = parent.Substring(1, parent.Length - 2)
                    End If
                    level = New HierarchyLevel(parent)
                    rootLevels.Add(level)
                    levels.Add(parent, level)
                ElseIf pc.Length = 2 Then
                    parent = pc(0)
                    If parent.StartsWith("""") Then
                        parent = parent.Substring(1, parent.Length - 2)
                    End If
                    child = pc(1)
                    If child.StartsWith("""") Then
                        child = child.Substring(1, child.Length - 2)
                    End If
                    If levels.ContainsKey(parent) Then
                        parentLevel = levels(parent)
                    Else
                        'parent level dose not exist exit
                        Exit Sub
                    End If
                    level = New HierarchyLevel(child)
                    parentLevel.Children.Add(level)
                    levels.Add(child, level)
                Else
                    'Somthing is wrong exit 
                    Me.Errors.Add(New BuilderMessage(ErrorCodes.HIERARCHIES))
                    Exit Sub
                End If

            Next

            'There should be exactly one rootlevel
            If rootLevels.Count = 1 Then
                v.Hierarchy.RootLevel = rootLevels(0)
            End If
        End Sub

        ''' <summary>
        ''' Set names for the hierarchies levels
        ''' </summary>
        ''' <param name="variablename">name of the hierarchical variables</param>
        ''' <param name="names">names of the hierarchies</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetHierarchyNames(ByVal variablename As String, ByVal names As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            'Get the variable 
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.HIERARCHYNAMES))
                Exit Sub
            End If

            If Not v.Hierarchy Is Nothing Then
                CreateHierarchyNames(v.Hierarchy, names)
            End If
        End Sub

        'Set number of hierarchy levels in a symmetrical tree
        Private Sub SetHierarchyLevels(ByVal variablename As String, ByVal levels As String, ByVal meta As PCAxis.Paxiom.PXMeta, ByVal isDefaultLanguage As Boolean)
            'Only sets it for the default language (only needs to be set once)
            If Not isDefaultLanguage Then
                Exit Sub
            End If

            'Get the variable 
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.HIERARCHYLEVELS))
                Exit Sub
            End If

            If Not v.Hierarchy Is Nothing Then
                Dim levelsNo As Integer
                If Not Integer.TryParse(levels, levelsNo) Then
                    levelsNo = 0
                End If

                'Check if HIERARCHYNAMES has been set. If so the number of levels must match
                If v.Hierarchy.Names.Count > 0 Then
                    If v.Hierarchy.Names.Count <> levelsNo Then
                        Me.Warnings.Add(New BuilderMessage(ErrorCodes.HIERARCHYLEVELS))
                    End If
                End If

                v.Hierarchy.Levels = levelsNo
            End If
        End Sub

        ''' <summary>
        ''' Set names for the levels in a symmetrical tree
        ''' </summary>
        ''' <param name="hierarchy">Hierarchy object</param>
        ''' <param name="names">String collection containing the names</param>
        ''' <remarks></remarks>
        Protected Overridable Sub CreateHierarchyNames(ByVal hierarchy As Hierarchy, ByVal names As System.Collections.Specialized.StringCollection)
            'check if HIEARARCHYLEVELS has been set. If so the number of levels must match.
            If hierarchy.Levels > -1 Then
                If hierarchy.Levels <> names.Count Then
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.HIERARCHYNAMES))
                End If
            End If

            hierarchy.Names.Clear()
            For Each name As String In names
                hierarchy.Names.Add(name)
            Next
        End Sub

        ''' <summary>
        ''' Creates ValuesSetInfo's for the cube
        ''' </summary>
        ''' <param name="valueSetIDs">id for the valueset</param>
        ''' <param name="variablename">name of the variable that has 
        ''' the given valueset</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub CreateValueSets(ByVal valueSetIDs As System.Collections.Specialized.StringCollection, ByVal variablename As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Exit Sub
            End If

            Dim vs As ValueSetInfo
            For i As Integer = 0 To valueSetIDs.Count - 1
                vs = New ValueSetInfo(valueSetIDs(i), m_model.Meta.NumberOfLanguages - 1)
                v.ValueSets.Add(vs)
            Next

        End Sub

        ''' <summary>
        ''' Sets the valueset names for a variable
        ''' </summary>
        ''' <param name="valueSetNames">names of the value sets</param>
        ''' <param name="variablename">name of the variable that has 
        ''' the given valueset</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetValueSetNames(ByVal valueSetNames As System.Collections.Specialized.StringCollection, ByVal variablename As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename, 0)
            If v Is Nothing Then
                Exit Sub
            End If

            If v.ValueSets.Count <> valueSetNames.Count Then
                Logger.Debug("Number of names differ from number of value sets")
                Exit Sub
            End If

            For i As Integer = 0 To valueSetNames.Count - 1
                v.ValueSets(i).Name = valueSetNames(i)
            Next

        End Sub

        ''' <summary>
        ''' Sets the value-text-option for a existing variable placed in the stub or the heading
        ''' </summary>
        ''' <param name="variablename">name of the variable</param>
        ''' <param name="valueTextOption">the value text option of the variable</param>
        ''' <param name="meta"></param>
        ''' <remarks></remarks>
        Private Sub SetValueTextOption(ByVal variablename As String, ByVal valueTextOption As String, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim v As Variable
            v = FindVariable(meta, variablename)
            If v Is Nothing Then
                Me.Errors.Add(New BuilderMessage(ErrorCodes.VALUETEXTOPTION))
                Exit Sub
            End If

            If String.Compare(PXConstant.VALUETEXTOPTION_NOTEXT, valueTextOption) = 0 Then
                v.ValueTextOption = ValueTextOptionType.ValueTextMissing
            ElseIf String.Compare(PXConstant.VALUETEXTOPTION_TOOLONG, valueTextOption) = 0 Then
                v.ValueTextOption = ValueTextOptionType.VardeExtra
            Else
                v.ValueTextOption = ValueTextOptionType.NormalText
            End If

        End Sub

        ''' <summary>
        ''' Create id:s for attributes at cell level
        ''' </summary>
        ''' <param name="attributeIDs">A collection of attribute ids</param>
        ''' <param name="meta">PXMeta object</param>
        ''' <remarks></remarks>
        Private Sub CreateAttributeIDs(ByVal attributeIDs As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            For Each id As String In attributeIDs
                If String.IsNullOrEmpty(id) Then
                    'Invalid ATTRIBUTE-ID - Add Warning (not mandatory keyword)
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.ATTRIBUTE_ID))
                    Exit Sub
                End If

                meta.Attributes.Identities.Add(id)
            Next
        End Sub

        ''' <summary>
        ''' Create names for the attributes at cell level
        ''' </summary>
        ''' <param name="attributeNames">Collection of attribute names</param>
        ''' <param name="meta">PXMeta object</param>
        ''' <remarks></remarks>
        Private Sub CreateAttributeNames(ByVal attributeNames As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)

            If meta.Attributes.Identities.Count <> attributeNames.Count Then
                'Number of attribute names must equal the number of attribute identities - Add Warning (not mandatory keyword)
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.ATTRIBUTE_TEXT))
                Exit Sub
            End If

            For Each name As String In attributeNames
                If String.IsNullOrEmpty(name) Then
                    'Invalid ATTRIBUTE-TEXT - Add Warning (not mandatory keyword)
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.ATTRIBUTE_TEXT))
                    Exit Sub
                End If

                meta.Attributes.AddName(name)
            Next
        End Sub

        ''' <summary>
        ''' Set attributes at cell level
        ''' </summary>
        ''' <param name="dimensions">Dimension values identifying the cell</param>
        ''' <param name="attributeValues">The attribute values</param>
        ''' <param name="meta">PXMeta object</param>
        ''' <remarks>dimensions = Nothing means it is the default attributes</remarks>
        Private Sub SetAttributes(ByVal dimensions As String, ByVal attributeValues As System.Collections.Specialized.StringCollection, ByVal meta As PCAxis.Paxiom.PXMeta)
            Dim dimensionValues As VariableValuePairs
            Dim attributeValuesArray(attributeValues.Count - 1) As String

            'The number of attribute values must equal the number of attribute identifiers
            If (attributeValues.Count = 0) Or (meta.Attributes.Identities.Count = 0) Or (attributeValues.Count <> meta.Attributes.Identities.Count) Then
                'Invalid ATTRIBUTES - Add Warning (not mandatory keyword)
                Me.Warnings.Add(New BuilderMessage(ErrorCodes.ATTRIBUTES))
                Exit Sub
            End If

            If dimensions Is Nothing Then
                'If no dimensions are defined the attributes are the default attributes
                If Not meta.Attributes.SetDefaultAttributes(attributeValues) Then
                    'Invalid ATTRIBUTES - Add Warning (not mandatory keyword)
                    Me.Warnings.Add(New BuilderMessage(ErrorCodes.ATTRIBUTES))
                    Exit Sub
                End If
            Else
                'Set attributes at cell level
                dimensionValues = GetVariableValuePairs(dimensions, meta, ErrorCodes.ATTRIBUTES)
                If Not dimensionValues Is Nothing Then
                    attributeValues.CopyTo(attributeValuesArray, 0)
                    If Not meta.Attributes.CellAttributes.ContainsKey(dimensionValues) Then
                        meta.Attributes.CellAttributes.Add(dimensionValues, attributeValuesArray)
                    End If
                End If
            End If

        End Sub

#End Region

#End Region

#Region "Helper functions"

        ''' <summary>
        ''' Splitts a string in the PX file format into multiple values. Uses comma character as a separator
        ''' </summary>
        ''' <param name="s">The string to splitt</param>
        ''' <returns>A collection of strings</returns>
        ''' <remarks>quotation marks and white spaces surrounding the strings/tokens will be removed</remarks>
        Protected Shared Function SplittString(ByVal s As String) As System.Collections.Specialized.StringCollection
            Dim sc As System.Collections.Specialized.StringCollection = New System.Collections.Specialized.StringCollection
            'Dim strs As String() = s.Split(New Char() {","c})
            'Dim ws As String
            'For i As Integer = 0 To strs.Length - 1
            '    ws = strs(i).Trim()
            '    If ws.StartsWith("""") And ws.EndsWith("""") Then
            '        ws = ws.Substring(1)
            '        ws = ws.Substring(0, ws.Length - 2)
            '    End If
            '    sc.Add(ws)
            'Next

            ' Loop characters in string and parse
            Dim previousCharacter As String = String.Empty
            Dim currentCharacter As String = String.Empty
            Dim anItem As New System.Text.StringBuilder()
            Dim inString As Boolean = True
            Dim addCharacter As Boolean = True
            Dim addToCollection As Boolean = False
            For i As Integer = 0 To s.Length - 1
                currentCharacter = s.Substring(i, 1)


                Select Case currentCharacter
                    Case """"
                        ' Change status of inString
                        inString = Not inString
                        addCharacter = False ' we do not add these characters
                    Case ","
                        ' If we are in a string the comma is part of it and not a separator
                        addCharacter = inString
                    Case Else
                        addCharacter = True
                End Select

                ' If we are at the last character we are not in a string anymore and we should add to collection
                If i = s.Length - 1 Then
                    inString = False
                    addToCollection = True
                ElseIf previousCharacter = """" And Not inString Then
                    addToCollection = True
                End If

                If addCharacter Then
                    anItem.Append(currentCharacter)
                End If

                If addToCollection Then
                    ' Add an item to the collection
                    sc.Add(anItem.ToString())
                    ' Clear anItem for next round
                    anItem = New System.Text.StringBuilder()

                    ' A comma is expected to be the current character
                    ' If not treat as garbage - keep previousCharacter as is to get back in this code
                    If currentCharacter = "," Then
                        previousCharacter = currentCharacter
                    End If

                    ' Reset
                    addToCollection = False
                Else
                    previousCharacter = currentCharacter
                End If
            Next

            Return sc
        End Function

        ''' <summary>
        ''' Function that converts PX File booleans to .NET booleans i.e. YES and NO to True and False
        ''' </summary>
        ''' <param name="value">The value to convert</param>
        ''' <param name="result">Out parameter for the converted boolean</param>
        ''' <returns>True if convertion succeeded, else false</returns>
        ''' <remarks></remarks>
        Protected Shared Function GetBoolean(ByVal value As String, ByRef result As Boolean) As Boolean
            If String.Compare(value, "YES", True) = 0 Then
                result = True
                Return True
            ElseIf String.Compare(value, "NO", True) = 0 Then
                result = False
                Return True
            Else
                Return False
            End If
        End Function
#End Region

        ''' <summary>
        ''' If all languages should be read into the PXModel
        ''' </summary>
        ''' <value>True if all language should be read into the PXModel object otherwise False</value>
        ''' <returns>True if all language should be read into the PXModel object otherwise False</returns>
        ''' <remarks></remarks>
        Public Overridable Property ReadAllLanguages() As Boolean Implements IPXModelBuilder.ReadAllLanguages
            Get
                Return m_readAllLanguages
            End Get
            Set(ByVal value As Boolean)
                m_readAllLanguages = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the preferred language to read from the data source
        ''' </summary>
        ''' <param name="language">id of the language</param>
        ''' <remarks>if not all language should be read then preferred language should be read 
        ''' is it exist otherwise the default language should be read</remarks>
        Public Overridable Sub SetPreferredLanguage(ByVal language As String) Implements IPXModelBuilder.SetPreferredLanguage
            m_preferredLanguage = language
        End Sub

        ''' <summary>
        ''' Sets the user credentials if they are needed
        ''' </summary>
        ''' <param name="userName"></param>
        ''' <param name="password"></param>
        ''' <remarks></remarks>
        Public Overridable Sub SetUserCredentials(ByVal userName As String, ByVal password As String) Implements IPXModelBuilder.SetUserCredentials

        End Sub

        ''' <summary>
        ''' Applies the values set to a given variable
        ''' </summary>
        ''' <param name="variableCode">the code of the variable which the value set
        ''' will be applied to</param>
        ''' <param name="valueSet">the value set to apply</param>
        ''' <remarks></remarks>
        Public Overridable Sub ApplyValueSet(ByVal variableCode As String, ByVal valueSet As ValueSetInfo) Implements IPXModelBuilder.ApplyValueSet
            Throw New NotImplementedException()
        End Sub

        ''' <summary>
        ''' Applies the valuesets for a given subtable
        ''' </summary>
        ''' <param name="subTable">the subtable for which the valuesets will be applied
        ''' </param>
        ''' <remarks></remarks>
        Public Overridable Sub ApplyValueSet(ByVal subTable As String) Implements IPXModelBuilder.ApplyValueSet
            Throw New NotImplementedException()
        End Sub

        ''' <summary>
        ''' Applies a grouping to a given variable
        ''' </summary>
        ''' <param name="variableCode">the code of the variable</param>
        ''' <param name="groupingInfo">the grouing to apply</param>
        ''' <param name="include"></param>
        ''' <remarks></remarks>
        Public Overridable Sub ApplyGrouping(ByVal variableCode As String, ByVal groupingInfo As GroupingInfo, ByVal include As GroupingIncludesType) Implements IPXModelBuilder.ApplyGrouping
            Throw New NotImplementedException()
        End Sub

        Protected m_warnings As New List(Of BuilderMessage)
        ''' <summary>
        ''' A list of warnings that occured when building the model and that
        ''' should be shown to the user when displaying the cube.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Warnings() As System.Collections.Generic.List(Of BuilderMessage) Implements IPXModelBuilder.Warnings
            Get
                Return m_warnings
            End Get
        End Property

        Private _errors As New List(Of BuilderMessage)
        ''' <summary>
        ''' A list of fatal errors that occured when building the model. 
        ''' These errors are of the kind the cube cannot be displayed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Errors() As System.Collections.Generic.List(Of BuilderMessage) Implements IPXModelBuilder.Errors
            Get
                Return _errors
            End Get
        End Property

        ''' <summary>
        ''' State of the model builder
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BuilderState() As ModelBuilderStateType Implements IPXModelBuilder.BuilderState
            Get
                Return m_builderState
            End Get
        End Property

        Private Sub SetDatanote(subkey As String, datanote As String, meta As PXMeta)
            If String.IsNullOrEmpty(subkey) Then
                meta.Datanote = datanote
            Else
                Dim sc As System.Collections.Specialized.StringCollection
                sc = SplittString(subkey)
                If sc.Count = 1 Then
                    'TODO find variable
                    Dim var As Variable
                    var = FindVariable(meta, sc(0))
                    If var IsNot Nothing Then
                        var.Datanote = datanote
                    Else
                        'TODO Log warning
                    End If
                ElseIf sc.Count = 2 Then
                    Dim var As Variable
                    var = FindVariable(meta, sc(0))
                    If var IsNot Nothing Then
                        Dim val As Value
                        val = FindValue(var, sc(1))
                        If val IsNot Nothing Then
                            val.Datanote = datanote
                        Else
                            'TODO log warning
                        End If
                    Else
                        'TODO Log warning
                    End If
                Else
                    'TODO Log warning
                End If

            End If

        End Sub

        Private Sub SetMetaId(subkey As String, metaId As String, meta As PXMeta)
            If String.IsNullOrEmpty(subkey) Then
                meta.MetaId = metaId
            Else
                Dim sc As System.Collections.Specialized.StringCollection
                sc = SplittString(subkey)
                If sc.Count = 1 Then
                    'TODO find variable
                    Dim var As Variable
                    var = FindVariable(meta, sc(0))
                    If var IsNot Nothing Then
                        var.MetaId = metaId
                    Else
                        'TODO Log warning
                    End If
                ElseIf sc.Count = 2 Then
                    Dim var As Variable
                    var = FindVariable(meta, sc(0))
                    If var IsNot Nothing Then
                        Dim val As Value
                        val = FindValue(var, sc(1))
                        If val IsNot Nothing Then
                            val.MetaId = metaId
                            var.HasValueMetaId = True
                        Else
                            'TODO log warning
                        End If
                    Else
                        'TODO Log warning
                    End If
                Else
                    'TODO Log warning
                End If

            End If

        End Sub
    End Class

End Namespace
