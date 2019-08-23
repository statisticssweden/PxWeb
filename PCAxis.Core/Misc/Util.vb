Imports System.Reflection
Imports PCAxis.Paxiom.ClassAttributes

Namespace PCAxis
    Public Class Util

        ''' <summary>
        ''' Depending on settings in the model, title should be read from different properties.
        ''' </summary>
        ''' <param name="model">The model to get the title from</param>
        ''' <returns>The title of the model</returns>
        ''' <remarks></remarks>
        Public Shared Function GetModelTitle(ByVal model As PCAxis.Paxiom.PXModel) As String
            If (model Is Nothing) Then
                Return ""
            End If

            'If model.Meta.DescriptionDefault And Not String.IsNullOrEmpty(model.Meta.Description) Then
            If Not String.IsNullOrEmpty(model.Meta.Description) Then
                Return model.Meta.Description
            Else
                Return model.Meta.Title
            End If
        End Function

        ''' <summary>
        ''' Resizes all the language dependent field of the given object to the given size
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="size"></param>
        ''' <remarks></remarks>
        Public Shared Sub ResizeLanguageDependentFields(ByVal obj As Object, ByVal size As Integer)
            'Check all the (private) fields of the object
            For Each fi As FieldInfo In obj.GetType.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                Dim attr() As LanguageDependentAttribute = _
                CType(fi.GetCustomAttributes(GetType(LanguageDependentAttribute), True), LanguageDependentAttribute())

                'Is the field language dependent?
                If attr.GetLength(0) > 0 Then
                    'Redim the array
                    Util.RedimArrayField(obj, fi, size)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Used to redim the given 1-dimensional array-field of the given object
        ''' </summary>
        ''' <param name="obj">The object instance that contains the array-field</param>
        ''' <param name="arrayField">FieldInfo object for the array-field to redim</param>
        ''' <param name="size">The new size of the array</param>
        ''' <remarks></remarks>
        Private Shared Sub RedimArrayField(ByVal obj As Object, ByVal arrayField As FieldInfo, ByVal size As Integer)
            'Create array of length size of the proper element type.  
            Dim elementType As Type = arrayField.FieldType.GetElementType()
            Dim newArray As Array = Array.CreateInstance(elementType, size)
            Dim arr() As Object

            arr = CType(arrayField.GetValue(obj), Object())

            If Not arr Is Nothing Then
                If size <= arr.Length Then
                    Array.Copy(arr, newArray, size)
                Else
                    arr.CopyTo(newArray, 0)
                End If

                'Set the object instance's field to the array.  
                'Behind the scenes, there really is no such thing as a "redim".  
                'The old array (if present) is replaced with the new, resized array.  
                arrayField.SetValue(obj, newArray)
            End If
        End Sub

        ''' <summary>
        ''' Switches places for the languages with the given indexes on the given object
        ''' </summary>
        ''' <param name="obj">The object</param>
        ''' <param name="index1">Index of language 1. Will have the index of language 2 when the method is completed</param>
        ''' <param name="index2">Index of language 2. Will have the index of language 1 when the method is completed</param>
        ''' <remarks></remarks>
        Public Shared Sub SwitchLanguages(ByVal obj As Object, ByVal index1 As Integer, ByVal index2 As Integer)
            'Check all the (private) fields of the object
            For Each fi As FieldInfo In obj.GetType.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                Dim attr() As LanguageDependentAttribute = _
                CType(fi.GetCustomAttributes(GetType(LanguageDependentAttribute), True), LanguageDependentAttribute())

                'Is the field language dependent?
                If attr.GetLength(0) > 0 Then
                    Dim arr() As Object
                    Dim t As Object

                    arr = CType(fi.GetValue(obj), Object())

                    If Not arr Is Nothing Then
                        t = arr(index1)
                        arr(index1) = arr(index2)
                        arr(index2) = t
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Handles language bug for chinese in .NET 3.5 
        ''' </summary>
        ''' <param name="lang">Language string</param>
        ''' <returns></returns>
        ''' <remarks>TODO: Remove this method when .Net is upgraded to later version</remarks>
        Public Shared Function GetLanguageForNet3_5(ByVal lang As String) As String
            If String.IsNullOrEmpty(lang) Then
                Return ""
            End If

            'fix for Chinese / Taiwanese bug in net 3.5
#If v3_5 Then
            If String.Compare(lang, "zh", True) = 0 Then
                lang = "zh-TW"
            End If
#End If
            Return lang
        End Function


    End Class
End Namespace
