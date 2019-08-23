Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Operations interface which is used by implementers that wishes to create there own operation
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IPXOperation

        ''' <summary>
        ''' Exceutes the operation on the model.
        ''' </summary>
        ''' <param name="lhs">the model which the operation should be applied to.</param>
        ''' <param name="rhs">operations parameters that controls the execution of the operation</param>
        ''' <returns>A new PXModel instans where the operation have been applied</returns>
        ''' <remarks>
        ''' The operation must be nondestructive to the lhs parameter. 
        ''' That is to the model that comes in as a parameter should only 
        ''' be read and not modified instead a copy of that should be modified and returned.
        ''' <example>
        ''' This example illustrates a operation that capitalizes the variable names
        ''' <code>
        ''' Public Class VariableNamesToUpper
        '''    Implements IPXOperation
        '''    Public Function Execute(ByVal lhs As PXModel, ByVal rhs As Object) As PXModel Implements IPXOperation.Execute
        '''        Dim newModel As PXModel
        '''        newModel = lhs.CreateCopy()
        '''        Dim languages() As String = newModel.Meta.GetAllLanguages()
        '''        'loops through all the available languages
        '''        For Each lang As String In languages
        '''
        '''            'loops through all the variables
        '''            For Each v As Variable In newModel.Meta.Variables
        '''                v.Name = v.Name.ToUpper()
        '''            Next
        '''        Next
        '''
        '''        Return newModel
        '''    End Function
        ''' End Class
        ''' </code> 
        ''' </example>
        ''' </remarks>
        Function Execute(ByVal lhs As PCAxis.Paxiom.PXModel, ByVal rhs As Object) As PCAxis.Paxiom.PXModel

    End Interface




End Namespace