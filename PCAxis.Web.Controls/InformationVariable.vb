Imports PCAxis.Enums

Public Class InformationVariable
    Private _InformationType As InformationType
    Private _Term As String
    Private _Definition As String

    Public Sub New(ByVal informationType As InformationType, ByVal term As String, ByVal definition As String)
        _InformationType = informationType
        _Term = term
        _Definition = definition
    End Sub

    Public ReadOnly Property InformationType() As InformationType
        Get
            Return _InformationType
        End Get
    End Property

    Public ReadOnly Property Term() As String
        Get
            Return _Term
        End Get
    End Property

    Public ReadOnly Property Definition() As String
        Get
            Return _Definition
        End Get
    End Property

    ''' <summary>
    ''' Create a deep copy of me
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateCopy() As InformationVariable
        Dim newObject As InformationVariable

        newObject = CType(Me.MemberwiseClone(), InformationVariable)

        Return newObject
    End Function
End Class
