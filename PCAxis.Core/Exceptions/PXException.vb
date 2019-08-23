Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Base class for all of the exception that should be thrown from PC-Axis
    ''' </summary>
    ''' <remarks>
    ''' All PC-Axis specific exception should inherit this class
    ''' </remarks>
    Public Class PXException
        Inherits System.Exception

#Region "Protected fields"
        Protected m_errorCode As String
        Protected m_params() As Object
#End Region

#Region "Constructors"

        ''' <summary>
        ''' Defalut constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="message">The error message</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="message">The error message</param>
        ''' <param name="errorCode">The error code of the specific error</param>
        ''' <param name="params">parameters to the error message that the error code specifies</param>
        ''' <remarks>The String.Format function is used to insert the params into the error message</remarks>
        Public Sub New(ByVal message As String, ByVal errorCode As String, ByVal params() As Object)
            MyBase.New(message)
            Me.m_errorCode = errorCode
            Me.m_params = params
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="message">the error message</param>
        ''' <param name="ex"> An inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal ex As System.Exception)
            MyBase.New(message, ex)
            Me.m_errorCode = "N/A" 'TODO set it to unknown
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Retrieves the error code
        ''' </summary>
        ''' <value>The error code</value>
        ''' <returns>The error code</returns>
        ''' <remarks>
        ''' The error code is a string that is used to 
        ''' look upp the actuall error message using the 
        ''' localization functionallity
        ''' </remarks>
        Public ReadOnly Property ErrorCode() As String
            Get
                Return m_errorCode
            End Get
        End Property

        ''' <summary>
        ''' Parameters to the error message
        ''' </summary>
        ''' <value>The error message parameters</value>
        ''' <returns>The error message parameters</returns>
        ''' <remarks>
        ''' Error messageas are stored in resource 
        ''' files that are translated to specific languages. 
        ''' The error messages can also contain placeholders for 
        ''' parameters. The parameters will be placed into this 
        ''' placeholders using the string.Format function.
        ''' <example>
        ''' If the error massage is
        ''' <code>The variable {0} could not be found</code>
        ''' then this error message will have one placeholder that will 
        ''' be filled in with the first object in the Paramaters array.
        ''' </example>
        ''' </remarks>
        Public ReadOnly Property Parameters() As Object()
            Get
                Return m_params
            End Get
        End Property

#End Region

    End Class
End Namespace

