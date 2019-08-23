Imports PCAxis.Web.Core.Enums

Namespace Attributes
    ''' <summary>
    ''' Used to mark properties that should be persisted between requests
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)> _
    Public Class PropertyPersistStateAttribute
        Inherits Attribute

        Private _persisteStateType As PersistStateType

        ''' <summary>
        ''' Gets how the property is to be persisted
        ''' </summary>
        ''' <value>How the property is to be persisted</value>
        ''' <returns>A <see cref="Enums.PersistStateType" /> value representing the way the property is to persisted</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PersistStateType() As PersistStateType
            Get
                Return Me._persisteStateType
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new PropertyPersistStateAttribute with the specified <see cref="Attributes.PropertyPersistStateAttribute" /> 
        ''' </summary>
        ''' <param name="PersistStateType">A <see cref="Enums.PersistStateType" /> representing how the property is to be persisted</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal PersistStateType As PersistStateType)
            Me._persisteStateType = PersistStateType
        End Sub
    End Class
End Namespace
