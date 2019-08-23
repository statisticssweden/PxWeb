
''' <summary>
''' Delegate for handling PxActionEvent
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
''' <remarks></remarks>
Public Delegate Sub PxActionEventHandler(ByVal sender As Object, ByVal e As PxActionEventArgs)

''' <summary>
''' Arguments to the PxActionEvent
''' </summary>
''' <remarks></remarks>
Public Class PxActionEventArgs
    Inherits EventArgs

    Public Const PRESENTATION_SCREEN As String = "screen"

    Private _actionType As PxActionEventType
    Private _actionName As String
    Private _tableId As String
    Private _numberOfCells As Integer
    Private _numberOfContents As Integer

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="actionType">Type of action</param>
    ''' <param name="actionName">Name of action</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal actionType As PxActionEventType, ByVal actionName As String)
        _actionType = actionType
        _actionName = actionName
    End Sub

    ''' <summary>
    ''' Type of action
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ActionType() As PxActionEventType
        Get
            Return _actionType
        End Get
    End Property

    ''' <summary>
    ''' Name of the action
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ActionName() As String
        Get
            Return _actionName
        End Get
    End Property

    ''' <summary>
    ''' Table the action has been performed upon
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TableId() As String
        Get
            Return _tableId
        End Get
        Set(ByVal value As String)
            _tableId = value
        End Set
    End Property

    ''' <summary>
    ''' Number of cells in the table the action has been performed upon
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumberOfCells() As Integer
        Get
            Return _numberOfCells
        End Get
        Set(ByVal value As Integer)
            _numberOfCells = value
        End Set
    End Property

    ''' <summary>
    ''' Number of contents (number of values in the content variable) in the table the action has been performed upon
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumberOfContents() As Integer
        Get
            Return _numberOfContents
        End Get
        Set(ByVal value As Integer)
            _numberOfContents = value
        End Set
    End Property
End Class
