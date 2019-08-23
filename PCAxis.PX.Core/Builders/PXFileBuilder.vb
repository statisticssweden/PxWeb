Namespace PCAxis.Paxiom

    Public Class PXFileBuilder
        Inherits PCAxis.Paxiom.PXFileModelBuilderAdpater
        Implements PCAxis.PlugIn.IPlugIn

        Private Shared Logger As log4net.ILog = log4net.LogManager.GetLogger(GetType(PXFileBuilder))

#Region "Private members"
        Private _host As PCAxis.PlugIn.IPlugInHost
        Private _originalGroupedVariables As Dictionary(Of String, Variable)
        Private _groupingIncludes As Dictionary(Of String, GroupingIncludesType)
        'Private _groupingInclude As GroupingIncludesType
#End Region

        ''' <summary>
        ''' Set the path to the PX-file to build PX-model from
        ''' </summary>
        ''' <param name="path">The path to the PX-file</param>
        ''' <remarks></remarks>
        Public Overrides Sub SetPath(ByVal path As String)
            If Logger.IsDebugEnabled Then
                Logger.Debug("SetPath=" & path.ToString())
            End If

            m_path = path

            Dim plugin As PCAxis.PlugIn.IPlugIn
            plugin = New PCAxis.Paxiom.Parsers.PXFileParser

            DirectCast(plugin, PCAxis.Paxiom.Parsers.PXFileParser).SetPath(path.ToString())
            m_parser = DirectCast(plugin, PCAxis.Paxiom.Parsers.PXFileParser)

        End Sub

        Protected Overrides Sub SetMeta(ByVal keyword As String, ByVal subkey As String, ByVal values As System.Collections.Specialized.StringCollection, ByVal meta As PXMeta, ByVal isDefaultLanguage As Boolean)
            Select Case keyword
                'Case PXKeywords.NOTE, _
                '        PXKeywords.NOTEX, _
                '        PXKeywords.VALUENOTE, _
                '        PXKeywords.VALUENOTEX, _
                '        PXKeywords.CELLNOTE, _
                '        PXKeywords.CELLNOTEX
                '
                'Dim val As String = values(0).Replace("#", ControlChars.CrLf)
                'values.Clear()
                'values.Add(val)
                'MyBase.SetMeta(keyword, subkey, values, meta, isDefaultLanguage)
                Case PXKeywords.PRESTEXT
                    'Only allow PRESTEXT=0, PRESTEXT=2 and PRESTEXT=3 when AXIS-VERSION is 2010 or higher
                    If (values(0).Equals("0")) Or (values(0).Equals("2")) Or (values(0).Equals("3")) Then
                        Dim version As Integer

                        If Not Integer.TryParse(meta.AxisVersion, version) Then
                            Errors.Add(New BuilderMessage("PxcErr162"))
                            Exit Sub
                        Else
                            If version < 2010 Then
                                Errors.Add(New BuilderMessage("PxcErr162"))
                                Exit Sub
                            End If
                        End If
                    End If

                    MyBase.SetMeta(keyword, subkey, values, meta, isDefaultLanguage)
                Case Else
                    MyBase.SetMeta(keyword, subkey, values, meta, isDefaultLanguage)
            End Select

        End Sub

        ''' <summary>
        ''' Build PX model meta part
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function BuildForSelection() As Boolean
            Me.m_builderState = ModelBuilderStateType.BuildingForSelection

            Try
                If Not MyBase.BuildForSelection() Then
                    Return False
                End If
            Catch ex As PXModelParserException
                Me.Errors.Add(New BuilderMessage(ex.Message))
                Return False
            End Try
            

            Me.m_model.Meta.MainTable = m_path

            Dim f As IO.FileInfo = New IO.FileInfo(m_path)
            Dim dirPath As String = System.IO.Path.GetDirectoryName(m_path)

            'Load groupings for variables
            Dim groupReg As GroupRegistry = GroupRegistry.GetRegistry
            If groupReg.GroupingsLoaded Then

                Dim currentLanguageIndex As Integer = Model.Meta.CurrentLanguageIndex

                For i As Integer = 0 To Model.Meta.NumberOfLanguages - 1
                    Model.Meta.SetLanguage(i)

                    For Each variable As Variable In Model.Meta.Variables
                        'Load groupings from this directory to the Group registry
                        groupReg.LoadGroupingsForPathAndDomain(f.DirectoryName, variable.Domain)

                        'Add groupings from this directory to the variable
                        For Each groupInfo As GroupingInfo In groupReg.GetGroupingsFromPath(variable.Domain, dirPath)
                            variable.AddGrouping(groupInfo)
                        Next

                        'Groupings from the local directory overrides groupings from the default directory
                        '==> Only add groupings from the default directory if no groupings are previously
                        '    added to the variable
                        If Not variable.HasGroupings Then
                            'Add default groupings to the variable
                            For Each groupInfo As GroupingInfo In groupReg.GetDefaultGroupings(variable.Domain)
                                variable.AddGrouping(groupInfo)
                            Next
                        End If
                    Next
                Next

                Model.Meta.SetLanguage(currentLanguageIndex)

            End If

            If Not m_preferredLanguage Is Nothing Then
                Model.Meta.SetPreferredLanguage(m_preferredLanguage)
            End If

            Me.m_builderState = ModelBuilderStateType.BuildForSelection
            Return True
        End Function

        ''' <summary>
        ''' Applies the selected grouping on the selected variable
        ''' </summary>
        ''' <param name="variableCode">Code of the variable</param>
        ''' <param name="groupingInfo">Grouping to apply</param>
        ''' <param name="include">Type of values that will be included (grouped values, single values)</param>
        ''' <remarks></remarks>
        Public Overrides Sub ApplyGrouping(ByVal variableCode As String, ByVal groupingInfo As GroupingInfo, ByVal include As GroupingIncludesType)
            Dim grouping As Grouping = GroupRegistry.GetRegistry.GetGrouping(groupingInfo)

            'Set how the grouping shall be displayed (always aggregated values for PX-file groupings) 
            grouping.GroupPres = include

            Dim var As Variable = Me.Model.Meta.Variables.GetByCode(variableCode)
            Dim valueCodes As New List(Of String) 'List of added values. Asserts that two values with the same code is not added. All value codes must be unique!

            If grouping Is Nothing Then
                Exit Sub
            End If

            'If var.CurrentGrouping Is Nothing Then


            'Create a copy of the original variable and store it. It will be used when the aggregated 
            'values are calculated in BuildForPresentation.
            Dim varOriginal As Variable = var.CreateCopyWithValues
            StoreOriginalGroupedVariable(varOriginal, include)

            var.CurrentGrouping = grouping

            'Create the new values
            var.RecreateValues()
            For Each group As Group In grouping.Groups
                Dim val As Value

                If include = GroupingIncludesType.AggregatedValues Or include = GroupingIncludesType.All Then
                    If Not valueCodes.Contains(group.GroupCode) Then
                        valueCodes.Add(group.GroupCode)
                        val = New Value
                        PaxiomUtil.SetCode(val, group.GroupCode)
                        val.Value = group.Name
                        var.Values.Add(val)
                    End If
                End If

                If include = GroupingIncludesType.SingleValues Or include = GroupingIncludesType.All Then
                    For Each child As GroupChildValue In group.ChildCodes
                        If Not valueCodes.Contains(child.Code) Then
                            valueCodes.Add(child.Code)
                            val = New Value
                            PaxiomUtil.SetCode(val, child.Code)
                            val.Value = child.Name
                            var.Values.Add(val)
                        End If
                    Next
                End If

            Next
            'Remove hiearchy after grouping
            If var.Hierarchy.IsHierarchy Then
                var.Hierarchy.RootLevel = Nothing
            End If

            'Remove elimination for variable 
            var.Elimination = False
            var.EliminationValue = Nothing


            'End If

            If Me.Model.Meta.NumberOfLanguages > 1 Then
                'Model no longer multilingual after grouping has been selected

                Dim lang As Integer = Me.Model.Meta.CurrentLanguageIndex

                'Remove languages from model
                Me.Model.Meta.DeleteAllLanguagesExceptCurrent()

                'Also remove languages from original variables stored in builder
                For Each v As Variable In _originalGroupedVariables.Values
                    PCAxis.Paxiom.VariableHelper.DeleteAllLanguagesExceptOne(v, lang)
                Next
            End If

        End Sub

        Public Overrides Function BuildForPresentation(ByVal selection() As Selection) As Boolean
            Dim performGrouping As Boolean = False
            Dim variable As Variable
            Dim groupedVariables As Variables = Nothing
            Dim varOriginal As Variable
            Dim selectedValues As System.Collections.Specialized.StringCollection
            Dim grouping As Grouping
            Dim sel As Selection = Nothing
            Dim missingValues As Integer = 0

            Me.m_builderState = ModelBuilderStateType.BuildingForPresentation

            'Contains the values for the grouped variables
            'Key = variable code
            'Value list of values
            Dim groupingValues As New Dictionary(Of String, List(Of Value))

            Me.Errors.Clear()
            Me.Warnings.Clear()

            If Model.Meta.Variables.HasGroupingsApplied Then
                'Grouping is applied to at least one of the variables.
                performGrouping = True

                groupedVariables = New Variables

                For i As Integer = 0 To Model.Meta.Variables.Count - 1
                    variable = Model.Meta.Variables(i)

                    If Not variable.CurrentGrouping Is Nothing Then

                        '1. Substitue the grouped variable with the stored original variable
                        '-------------------------------------------------------------------

                        'Get the stored original variable - This object instance is the resulting variable object
                        varOriginal = _originalGroupedVariables(variable.Code)

                        'Apply some grouping rules on the variable:
                        '1. Remove DOMAIN for the variable - cannot be grouped as before
                        varOriginal.Domain = Nothing
                        '2. Remove/change MAP
                        If Not String.IsNullOrEmpty(variable.CurrentGrouping.Map) Then
                            varOriginal.Map = variable.CurrentGrouping.Map
                        Else
                            varOriginal.Map = Nothing
                        End If

                        'Substitute variables
                        SubstituteVariablesInModel(variable, varOriginal)

                        'Add the substituted grouped variable to the collection
                        groupedVariables.Add(variable)


                        '2. Remove unused values in the grouped variable 
                        '   (have to do this to get the SumGrouping-operation to work correctly)
                        '-----------------------------------------------------------------------

                        'Find selection for the variable
                        For Each s As Selection In selection
                            If s.VariableCode.Equals(variable.Code) Then
                                sel = s
                                Exit For
                            End If
                        Next

                        If sel Is Nothing Then
                            Throw New PXException("Selection missing for variable")
                        End If

                        'Save values so we can restore them to the variable if somethings goes wrong during grouping
                        Dim lst As New List(Of Value)
                        For j As Integer = 0 To variable.Values.Count - 1
                            'originalValues.Add(variable.Values(j))
                            lst.Add(variable.Values(j))
                        Next
                        groupingValues.Add(variable.Code, lst)

                        Dim usedValue As Boolean
                        For j As Integer = variable.Values.Count - 1 To 0 Step -1
                            usedValue = False
                            For Each selectedValue As String In sel.ValueCodes
                                If variable.Values(j).Code.Equals(selectedValue) Then
                                    usedValue = True
                                    Exit For
                                End If
                            Next

                            If Not usedValue Then
                                variable.Values.RemoveAt(j)
                            End If
                        Next

                        '3. Rebuild the selection to contain all the childcodes for the selected groups
                        '   plus all the single values
                        '------------------------------------------------------------------------------

                        If sel.VariableCode.Equals(variable.Code) Then
                            selectedValues = New System.Collections.Specialized.StringCollection
                            grouping = variable.CurrentGrouping

                            Dim added As Boolean
                            For Each selectedValue As String In sel.ValueCodes
                                added = False

                                'Is it a group?
                                For Each group As Group In grouping.Groups
                                    If selectedValue.Equals(group.GroupCode) Then
                                        For Each child As GroupChildValue In group.ChildCodes
                                            If Not varOriginal.Values.IsCodesFictional Then
                                                If Not selectedValues.Contains(child.Code) Then
                                                    'Check that the childvalue really exist in the original variable
                                                    If Not varOriginal.Values.GetByCode(child.Code) Is Nothing Then
                                                        selectedValues.Add(child.Code)
                                                    Else
                                                        missingValues = missingValues + 1
                                                    End If
                                                End If
                                            Else
                                                'Translate codes of values in the group to the fictional codes of the variable
                                                Dim tmpVal As Value = varOriginal.Values.GetByName(child.Code)
                                                If Not tmpVal Is Nothing Then
                                                    selectedValues.Add(tmpVal.Code)
                                                Else
                                                    missingValues = missingValues + 1
                                                End If
                                            End If
                                            added = True
                                        Next
                                        Exit For
                                    End If
                                Next

                                'Not a group - add a single value
                                If Not added Then
                                    If Not selectedValues.Contains(selectedValue) Then
                                        selectedValues.Add(selectedValue)
                                    End If
                                End If
                            Next

                            sel.ValueCodes.Clear()
                            For Each selectedValue As String In selectedValues
                                sel.ValueCodes.Add(selectedValue)
                            Next

                            If sel.ValueCodes.Count = 0 Then
                                'The grouped variable contains no values 
                                'Restore values for all grouped variables and create error message
                                For Each var As Variable In groupedVariables
                                    'Substitute variables
                                    SubstituteVariablesInModel(_originalGroupedVariables(var.Code), var)

                                    'Restore the grouped values to the variable
                                    var.Values.Clear()
                                    For j As Integer = 0 To groupingValues(var.Code).Count - 1
                                        var.Values.Add(groupingValues(var.Code)(j))
                                    Next
                                Next

                                Errors.Add(New BuilderMessage("PxcGroupingVariableWithNoValues", New Object() {variable.Name}))
                                Exit For
                            End If
                        End If
                    End If
                Next

                If missingValues > 0 Then
                    Warnings.Add(New BuilderMessage("PxcGroupedValuesMissing", New Object() {missingValues}))
                End If
            End If

            If Me.Errors.Count > 0 Then
                Logger.WarnFormat("Fatal error when building for presentation")
                Me.m_builderState = ModelBuilderStateType.BuildForSelection
                Return False
            End If

            If Not MyBase.BuildForPresentation(selection) Then
                Me.m_builderState = ModelBuilderStateType.BuildForSelection
                Return False
            End If

            'Execute grouping
            If performGrouping Then
                Dim sumGroupOp As New Operations.SumGrouping
                Dim sumGroupDescription As New Operations.SumGroupingDescription
                Dim groupingIncludes As New List(Of GroupingIncludesType)

                sumGroupDescription.GroupVariables = groupedVariables

                For Each var As Variable In groupedVariables
                    groupingIncludes.Add(Me._groupingIncludes(var.Code))
                Next

                sumGroupDescription.KeepValues = groupingIncludes
                m_model = sumGroupOp.Execute(Model, sumGroupDescription)
            End If

            Me.m_builderState = ModelBuilderStateType.BuildForPresentation

            If Me.m_parser IsNot Nothing AndAlso GetType(IDisposable).IsAssignableFrom(m_parser.GetType()) Then
                CType(Me.m_parser, IDisposable).Dispose()
            End If

            Return True
        End Function


         ''' <summary>
        ''' Substitutes variables in variables collection
        ''' </summary>
        ''' <param name="variables">The variable collection</param>
        ''' <param name="oldVar">The old variable in the collection that will be replaced</param>
        ''' <param name="newVar">The new variable</param>
        ''' <remarks></remarks>
        Private Sub SubstituteVariables(ByVal variables As Variables, _
                                        ByVal oldVar As Variable, _
                                        ByVal newVar As Variable)
            Dim index As Integer

            index = variables.IndexOf(oldVar)
            variables.RemoveAt(index)
            variables.Insert(index, newVar)
        End Sub

        Private Sub SubstituteVariablesInModel(ByVal oldVar As Variable, ByVal newVar As Variable)
            'Substitute variables
            If oldVar.Placement = PlacementType.Heading Then
                SubstituteVariables(Model.Meta.Heading, oldVar, newVar)
            Else
                SubstituteVariables(Model.Meta.Stub, oldVar, newVar)
            End If

            SubstituteVariables(Model.Meta.Variables, oldVar, newVar)
        End Sub

        ''' <summary>
        ''' Adds the variable to the internal dictionary containing the original versions 
        ''' (before they where grouped) of the grouped variables.
        ''' </summary>
        ''' <param name="variable">The original variable</param>
        ''' <remarks></remarks>
        Private Sub StoreOriginalGroupedVariable(ByVal variable As Variable, ByVal include As GroupingIncludesType)
            If _originalGroupedVariables Is Nothing Then
                _originalGroupedVariables = New Dictionary(Of String, Variable)
            End If
            If Not _originalGroupedVariables.ContainsKey(variable.Code) Then
                _originalGroupedVariables.Add(variable.Code, variable)
            End If

            If _groupingIncludes Is Nothing Then
                _groupingIncludes = New Dictionary(Of String, GroupingIncludesType)
            End If
            If Not _groupingIncludes.ContainsKey(variable.Code) Then
                _groupingIncludes.Add(variable.Code, include)
            End If
        End Sub

#Region "Plugin implementation"

        Public ReadOnly Property Description() As String Implements plugin.IPlugIn.Description
            Get
                Return "Model builder from the PX-file format"
            End Get
        End Property

        Public ReadOnly Property Id() As System.Guid Implements plugin.IPlugIn.Id
            Get
                Return New Guid("B8ACBF13-4512-48a0-981B-B55A25A46E80")
            End Get
        End Property

        Public Sub Initialize(ByVal host As PlugIn.IPlugInHost, ByVal configuration As System.Collections.Generic.Dictionary(Of String, String)) Implements plugin.IPlugIn.Initialize
            _host = host
        End Sub

        Public ReadOnly Property Name() As String Implements plugin.IPlugIn.Name
            Get
                Return "PXFileBuilder"
            End Get
        End Property

        Public Sub Terminate() Implements plugin.IPlugIn.Terminate

        End Sub

#End Region

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
