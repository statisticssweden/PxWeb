Namespace PCAxis.Paxiom

    '<summary>
    'Interface which ...TODO
    '</summary>
    Public Interface IPXModelParser
        '<summary>
        'The PXModel will call this function to retrieve all the meta information for the PXFile
        'The implementor should call the MetaHandler for each new metadata
        '<seealso cref="MetaHandler">See the NewMeta Event for more information</seealso>
        '</summary>
        Sub ParseMeta(ByVal handler As MetaHandler, ByVal preferredLanguage As String)

        '<summary>
        'The PXModel will call this function to retrieve all the data for the PXFile.
        'The implementor should call the DataHandler delegate function for all the data.
        '</summary>
        Sub ParseData(ByVal handler As DataHandler, ByVal preferredBufferSize As Integer)

        '<summary>
        'Delegate function that should be invoked for each data value and in the same order as the metadata was fetched. 
        'The data value should be in a string format.
        '</summary>
        '</remark>
        'If the datamatrix contains NPM-values then the proper constant should be returned.
        '</remark>
        Delegate Sub DataHandler(ByVal dataBuffer As Double(), ByVal startIndex As Integer, ByVal stopIndex As Integer, ByRef stopReading As Boolean)

        '<summary>
        'Delegate which must be called for each new metadata value.
        '<example>
        'This example show how to set the PC-Axis version value for the default language
        '<code>
        'Dim values As New System.Collections.Specialized.StringCollection
        'values.Add("2000")
        'NewMeta("AXIS-VERSION", nothing, nothing, values)
        '</code>
        '</example>
        '</summary>
        Delegate Function MetaHandler(ByVal keyword As String, ByVal language As String, ByVal subkey As String, ByVal values As System.Collections.Specialized.StringCollection) As Boolean

    End Interface

End Namespace
