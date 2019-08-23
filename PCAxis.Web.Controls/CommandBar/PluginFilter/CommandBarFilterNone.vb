
Imports System.Reflection

Namespace CommandBar.Plugin
    <Serializable()> _
    Public Class CommandBarFilterNone
        Implements ICommandBarPluginFilter

        Protected _dropDownFileFormatsActive As Boolean = True
        Public ReadOnly Property DropDownFileFormatsActive() As Boolean Implements ICommandBarPluginFilter.DropDownFileFormatsActive
            Get
                Return _dropDownFileFormatsActive
            End Get
        End Property

        Protected _dropDownOperationsActive As Boolean = True
        Public ReadOnly Property DropDownOperationsActive() As Boolean Implements ICommandBarPluginFilter.DropDownOperationsActive
            Get
                Return _dropDownOperationsActive
            End Get
        End Property

        Protected _dropDownViewsActive As Boolean = True
        Public ReadOnly Property DropDownViewsActive() As Boolean Implements ICommandBarPluginFilter.DropDownViewsActive
            Get
                Return _dropDownViewsActive
            End Get
        End Property



        ''' <summary>
        ''' Checks if a plugin should be used used with filter applied
        ''' </summary>
        ''' <param name="plugin">Plugin to check</param>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if plugin should be used used with filter applied</returns>
        ''' <remarks></remarks>
        Public Overridable Function UsePlugin(ByVal plugin As CommandBarPluginInfo, ByRef model As Paxiom.PXModel, ByVal pluginCategory As String) As Boolean Implements ICommandBarPluginFilter.UsePlugin
            Return CheckSpecialConstraints(plugin, model)
        End Function


        Public Overridable Function UseFiletype(ByVal ftype As Core.FileType) As Boolean Implements ICommandBarPluginFilter.UseFiletype
            Return True
        End Function

        Public Overridable Function UseOutputFormat(ByVal outputFormat As String) As Boolean Implements ICommandBarPluginFilter.UseOutputFormat
            Return True
        End Function


        ''' <summary>
        ''' Compare plugin name with the plugin blacklist
        ''' </summary>
        ''' <param name="plugin">plugin to check</param>
        ''' <returns>True if plugin not is in the blacklist</returns>
        ''' <remarks></remarks>
        Protected Function ApplyFilter(ByVal blacklist As List(Of String), ByVal plugin As CommandBarPluginInfo) As Boolean
            If Not blacklist.Contains(plugin.Name) Then
                Return True
            End If
        End Function

        ''' <summary>
        ''' Compare filetype name with the filetype blacklist
        ''' </summary>
        ''' <param name="ftype">filetype to check</param>
        ''' <returns>True if filetype not is in the blacklist</returns>
        ''' <remarks></remarks>
        Protected Function ApplyFilter(ByVal blacklist As List(Of String), ByVal ftype As PCAxis.Web.Core.FileType) As Boolean
            If Not blacklist.Contains(ftype.Type) Then
                Return True
            End If
        End Function

        ''' <summary>
        ''' Compare value name with the associated blacklist
        ''' </summary>
        ''' <param name="blacklist">list wth values to filter away</param>
        ''' <param name="value">value to check</param>
        ''' <returns>True if value not is in the blacklist</returns>
        ''' <remarks></remarks>
        Protected Function ApplyFilter(ByVal blacklist As List(Of String), ByVal value As String) As Boolean
            If Not blacklist.Contains(value) Then
                Return True
            End If
        End Function



        ''' <summary>
        ''' Depending on plugin different constraints due to model rules are checked
        ''' </summary>
        ''' <param name="plugin">Plugin to check</param>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if plugin meets the constraints if there is any, or if there are no special constraints for the plugin</returns>
        ''' <remarks></remarks>
        Protected Function CheckSpecialConstraints(ByVal plugin As CommandBarPluginInfo, ByRef model As Paxiom.PXModel) As Boolean
            Dim returnValue As Boolean = True
            Select Case plugin.Name
                Case Plugins.Operations.SUM
                    returnValue = CheckSumSpecialConstraint(model)
                Case Plugins.Operations.SPLIT_TIME_VARIABLE
                    returnValue = CheckSplitTimeVariableSpecialConstraint(model)
            End Select
            Return returnValue
        End Function



        ''' <summary>
        ''' Checks constraints special for operation Sum
        ''' </summary>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if constraints are fulfilled</returns>
        ''' <remarks></remarks>
        Protected Function CheckSumSpecialConstraint(ByRef model As Paxiom.PXModel) As Boolean
            '<constraint property="AggregAllowed" value="True"/>
            Return CheckProperty("AggregAllowed", "True", model)
        End Function

        ''' <summary>
        ''' Checks constraints special for operation SplitTimeVariable
        ''' </summary>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if constraints are fulfilled</returns>
        ''' <remarks></remarks>
        Protected Function CheckSplitTimeVariableSpecialConstraint(ByRef model As Paxiom.PXModel) As Boolean
            Dim returnValue As Boolean
            '<constraint property="TimeScale" value="Quartely, Monthly, Weekly"/>
            returnValue = CheckProperty("TimeScale", "Quartely, Monthly, Weekly", model)
            '<constraint property="HasTimeValue" value="true"/>
            returnValue = returnValue AndAlso CheckProperty("HasTimeValue", "True", model)
            Return returnValue
        End Function

        ''' <summary>
        ''' Validates that a given property has values allowed for the plugin
        ''' </summary>
        ''' <param name="prop">Property value to check in Model.Meta or Paxiom.Variable</param>
        ''' <param name="propValue">Value as string or values as comma separated string in examined property that is allowed</param>
        ''' <param name="model">Model to use</param>
        ''' <returns>True if constraint is filfilled</returns>
        ''' <remarks>Eg. check that "sum" plugin has the property "AggregAllowed" set to "True"</remarks>
        Protected Function CheckProperty(ByVal prop As String, ByVal propValue As String, ByRef model As Paxiom.PXModel) As Boolean

            Dim property_value As Object

            Dim property_info As Reflection.PropertyInfo = GetType(Paxiom.PXMeta).GetProperty(prop)
            If Not IsNothing(property_info) Then
                'property found under Paxiom.PXMeta (constraint concerns Meta-level)
                property_value = property_info.GetValue(model.Meta, Nothing)
                'Check that constraint is fulfilled
                If Not CheckConstraint(property_value, propValue) Then
                    Return False
                End If
            Else
                'Search for property in Variables (constraint concerns variable level)
                Dim variableFullfillingConstraintFound = False
                Dim variables_property_info As Reflection.PropertyInfo = GetType(Paxiom.PXMeta).GetProperty("Variables")
                Dim vars As Paxiom.Variables = DirectCast(variables_property_info.GetValue(model.Meta, Nothing), Paxiom.Variables)
                For Each var As Paxiom.Variable In vars
                    Dim variable_property_info As Reflection.PropertyInfo = GetType(Paxiom.Variable).GetProperty(prop)
                    If Not IsNothing(variable_property_info) Then
                        property_value = variable_property_info.GetValue(var, Nothing)
                        'Check that constraint is fulfilled
                        If CheckConstraint(property_value, propValue) Then
                            variableFullfillingConstraintFound = True
                            Exit For
                        End If
                    End If
                Next
                'If none of the variables fulfills the constraint, return false
                If Not variableFullfillingConstraintFound Then
                    Return False
                End If
            End If

            'Examined constraints are fulfilled
            Return True
        End Function

        ''' <summary>
        ''' Check if propertyvalue matches constraint for the property
        ''' </summary>
        ''' <param name="property_value">Value of given property</param>
        ''' <param name="constraint">Valid values for the property as commaseparated string</param>
        ''' <returns>True if valid</returns>
        ''' <remarks>Values containing collections are skipped and counted as valid</remarks>
        Protected Function CheckConstraint(ByVal property_value As Object, ByVal constraint As String) As Boolean
            If Not TypeOf property_value Is ICollection Then
                Dim validValues() As String = constraint.Split(CChar(","))
                For Each s As String In validValues
                    If property_value.ToString.ToLower = s.Trim(CChar(" ")).ToLower Then
                        Return True
                    End If
                Next
            Else
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' Check all public fields in Plugins.Operations
        ''' </summary>
        ''' <returns>List with all operations in Plugins.Operations</returns>
        ''' <remarks></remarks>
        Protected Function GetAllRegistredOperations() As List(Of String)
            Dim pluginsClass As Plugins.Operations = New Plugins.Operations()
            Return GetPluginProps(pluginsClass)
        End Function

        ''' <summary>
        ''' Check all public fields in Plugins.Views
        ''' </summary>
        ''' <returns>List with all views in Plugins.Views</returns>
        ''' <remarks></remarks>
        Protected Function GetAllRegistredViews() As List(Of String)
            Dim pluginsClass As Plugins.Views = New Plugins.Views()
            Return GetPluginProps(pluginsClass)
        End Function

        ''' <summary>
        ''' Check all public fields in Plugins.FileFormats
        ''' </summary>
        ''' <returns>List with all file formats in Plugins.FileFormats</returns>
        ''' <remarks></remarks>
        Protected Function GetAllRegistredFileFormats() As List(Of String)
            Dim pluginsClass As Plugins.FileFormats = New Plugins.FileFormats()
            Return GetPluginProps(pluginsClass)
        End Function

        ''' <summary>
        ''' Check all public fields in a given object
        ''' </summary>
        ''' <param name="pluginsClass">Object to examine</param>
        ''' <returns>List with values for all public fields in a given object</returns>
        ''' <remarks></remarks>
        Private Function GetPluginProps(ByVal pluginsClass As Object) As List(Of String)
            Dim lst As List(Of String) = New List(Of String)()
            Dim info() As FieldInfo = pluginsClass.GetType().GetFields()
            For Each pi As FieldInfo In info
                lst.Add(pi.GetValue(pluginsClass).ToString())
            Next
            Return lst
        End Function


    End Class
End Namespace