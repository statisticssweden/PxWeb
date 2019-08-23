Namespace PCAxis.Paxiom.ClassAttributes

    ''' <summary>
    ''' Tells that the field is language dependent. In other words it is an array where
    ''' each index represents a language. Applies only to fields.
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Field)> _
    Public Class LanguageDependentAttribute
        Inherits Attribute
    End Class

End Namespace
