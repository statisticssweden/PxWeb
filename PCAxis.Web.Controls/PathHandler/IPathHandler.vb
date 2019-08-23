Imports PCAxis.Menu

''' <summary>
''' Interface for handling Menu object ID:s and paths
''' </summary>
''' <remarks></remarks>
Public Interface IPathHandler

    ''' <summary>
    ''' Generate ID string of a ItemSelection object
    ''' </summary>
    ''' <param name="parentId">Id of the parent of the ItemSelection object</param>
    ''' <param name="item">The ItemSelection object to generate ID for</param>
    ''' <returns>Id string of the ItemSelection object</returns>
    ''' <remarks></remarks>
    Function Combine(ByVal parentId As String, ByVal item As ItemSelection) As String

    ''' <summary>
    ''' Get the ItemSelection object with the specified id
    ''' </summary>
    ''' <param name="itemId">Id of the ItemSelection object</param>
    ''' <returns>The ItemSelection object with the specified id</returns>
    ''' <remarks></remarks>
    Function GetSelection(ByVal itemId As String) As ItemSelection

    ''' <summary>
    ''' Get all the of the ItemSelection objects in the path to the ItemSelection object associated with the specified id. 
    ''' </summary>
    ''' <param name="itemId">Id the of the ItemSelection object</param>
    ''' <returns>List of ItemSelection objects that represent the path for the specified ItemSelection object</returns>
    ''' <remarks>
    ''' Example:
    ''' If GetPath is called for the ItemSelection object with the id BE_BE0101_BE0101A a list of 3 ItemSelection objects are returned
    ''' (the ItemSelection objects with the id:s BE, BE0101 and BE0101A)
    ''' </remarks>
    Function GetPath(ByVal itemId As String) As List(Of ItemSelection)

    ''' <summary>
    ''' Get the path string for the specified PxMenuItem
    ''' </summary>
    ''' <param name="menuItem">PxMenuItem to get the path string for</param>
    ''' <returns>Path string</returns>
    ''' <remarks></remarks>
    Function GetPathString(ByVal menuItem As PxMenuItem) As String

    ''' <summary>
    ''' Get the path string for the specified ItemSelection
    ''' </summary>
    ''' <param name="node">ItemSelection to get the path string for</param>
    ''' <returns>Path string</returns>
    ''' <remarks></remarks>
    Function GetPathString(ByVal node As ItemSelection) As String

    ''' <summary>
    ''' Get the table name for the specified ItemSelection
    ''' </summary>
    ''' <param name="item">ItemSelection object</param>
    ''' <returns>Table name</returns>
    ''' <remarks></remarks>
    Function GetTable(ByVal item As ItemSelection) As String

    ''' <summary>
    ''' Generate string to connect to database table
    ''' </summary>
    ''' <param name="db">Database id</param>
    ''' <param name="path">Path to table</param>
    ''' <param name="table">Table name</param>
    ''' <returns>String to connect to table</returns>
    ''' <remarks></remarks>
    Function CombineTable(ByVal db As String, ByVal path As String, ByVal table As String) As String


    ''' <summary>
    ''' Creates a node path with nodeids
    ''' </summary>
    ''' <param name="itemId">the item to create the path for</param>
    ''' <returns></returns>
    Function GetNodeIds(ByVal itemId As String) As List(Of String)


End Interface
