Namespace PCAxis.Paxiom

    ''' <summary>
    ''' The builder interface
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPXModelBuilder

        ''' <summary>
        ''' The instance of the PXModel that the builder is constructing
        ''' </summary>
        ''' <value>A instance of the PXModel</value>
        ''' <returns>A instance of the PXModel</returns>
        ''' <remarks>
        ''' It is possible that the instance of the Model could change 
        ''' during the different stages of the build process
        ''' </remarks>
        ReadOnly Property Model() As PCAxis.Paxiom.PXModel

        ''' <summary>
        ''' Path to the table that is the source of the builder model
        ''' </summary>
        ''' <value>Path to the table of the builder model</value>
        ''' <returns>Path to the table of the builder model</returns>
        ''' <remarks></remarks>
        ReadOnly Property Path() As String

        ''' <summary>
        ''' Builds the metadata part of the PXModel with all the necessary 
        ''' data for a user to make a selection.
        ''' </summary>
        ''' <remarks></remarks>
        Function BuildForSelection() As Boolean

        ''' <summary>
        ''' Builds the final version of the model with all metadata and data.
        ''' </summary>
        ''' <param name="selection">
        ''' A selection of variable and values that should be included in the 
        ''' final version of the model. The size of the selection array must 
        ''' be equal to the number of variables i the model</param>
        ''' <remarks></remarks>
        Function BuildForPresentation(ByVal selection() As Selection) As Boolean

        ''' <summary>
        ''' Sets the path or address to the data cube 
        ''' that should function as the data source.
        ''' </summary>
        ''' <param name="path">the path to the data source</param>
        ''' <remarks>
        ''' It is up to the implementer to determine how the 
        ''' path string should look like.</remarks>
        Sub SetPath(ByVal path As String)

        ''' <summary>
        ''' Sets the preferred language that should be read. If the language 
        ''' do not exisit then the default language should be used.
        ''' </summary>
        ''' <param name="language">the language code for the language that should be read</param>
        ''' <remarks></remarks>
        Sub SetPreferredLanguage(ByVal language As String)

        ''' <summary>
        ''' If all available languages should be read into the model
        ''' </summary>
        ''' <value>If all available languages should be read into the model</value>
        ''' <returns>
        ''' True if all languages should be read and False if only 
        ''' the preferred language should be read.
        ''' </returns>
        ''' <remarks></remarks>
        Property ReadAllLanguages() As Boolean

        ''' <summary>
        ''' Set the credentials if they are required 
        ''' </summary>
        ''' <param name="userName">The user name</param>
        ''' <param name="password">The user password</param>
        ''' <remarks></remarks>
        Sub SetUserCredentials(ByVal userName As String, ByVal password As String)

        ''' <summary>
        ''' Applies the specified value set to the specified variable
        ''' </summary>
        ''' <param name="variableCode">
        ''' The code of the variable that the value set should be applied to
        ''' </param>
        ''' <param name="valueSet">
        ''' The value set to apply
        ''' </param>
        ''' <remarks></remarks>
        Sub ApplyValueSet(ByVal variableCode As String, ByVal valueSet As ValueSetInfo)

        ''' <summary>
        ''' Applies the specified valuesets for a given subTable
        ''' </summary>
        ''' <param name="subTable">
        ''' The subtable for which valuesets should be applied
        ''' </param>
        ''' <remarks></remarks>
        Sub ApplyValueSet(ByVal subTable As String)

        ''' <summary>
        ''' Applies the specified grouing to the specified variable
        ''' </summary>
        ''' <param name="variableCode">
        ''' The code of the variable that the grouping should be applied to
        ''' </param>
        ''' <param name="groupingInfo">
        ''' The grouping to apply
        ''' </param>
        ''' <remarks></remarks>
        Sub ApplyGrouping(ByVal variableCode As String, ByVal groupingInfo As GroupingInfo, ByVal include As GroupingIncludesType)

        ''' <summary>
        ''' List of non fatal errors that occured during the build process. Implementers should add 
        ''' any non fatal errors that occurs when reading the data source. However, these 
        ''' errors should not be of the kind that prevent the user from continuing 
        ''' to display the data cube
        ''' </summary>
        ''' <value>List of warnings</value>
        ''' <returns>List of warings that occured during the build process</returns>
        ''' <remarks></remarks>
        ReadOnly Property Warnings() As List(Of BuilderMessage)
        ''' <summary>
        ''' List of fatal errors that occured during the build process. Implementers should add 
        ''' any fatal errors that occurs when reading the data source. These 
        ''' errors should be of the kind that prevent the user from continuing 
        ''' to display the data cube
        ''' </summary>
        ''' <value>List of errors</value>
        ''' <returns>List of errors that occured during the build process</returns>
        ''' <remarks></remarks>
        ReadOnly Property Errors() As List(Of BuilderMessage)

        ''' <summary>
        ''' State of the builder. Tells if BuildForSelection/BuildForPresentation is done or not. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property BuilderState() As ModelBuilderStateType

        ''' <summary>
        ''' Gets or Sets if the builder should not try to set the current valueset. Default is false. 
        ''' This is set to true when used in API because CurrentValueSet property is only relevant for the GUI
        ''' </summary>
        ''' <returns></returns>
        Property DoNotApplyCurrentValueSet() As Boolean
    End Interface

End Namespace
