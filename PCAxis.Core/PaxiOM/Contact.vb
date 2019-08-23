

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Holds the attributes on cell level for the table
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Class Contact

#Region "Private fields"
        Private mForName As String
        Private mSurName As String
        Private mPhonePrefix As String
        Private mPhoneNo As String
        Private mEmail As String



        Protected Friend mLanguageIndex As Integer = 0

        'Private mOrganizationName(0) As List(Of String)
        'Private mDepartment(0) As List(Of String)
        'Private mUnit As List(Of String)
        Private mOrganizationName As String
        Private mDepartment As String
        Private mUnit As String



#End Region
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            mForName = ""
            mSurName = ""
            mPhonePrefix = ""
            mPhoneNo = ""
            mEmail = ""
            mOrganizationName = ""
            mDepartment = ""
            mUnit = ""
        End Sub
#Region "Public properties"
        ''' <summary>
        ''' Forname
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Forname As String
            Get
                Return mForName
            End Get
            Set(ByVal value As String)
                mForName = value
            End Set
        End Property

        ''' <summary>
        ''' Surname
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Surname As String
            Get
                Return mSurName
            End Get
            Set(ByVal value As String)
                mSurName = value
            End Set
        End Property

        ''' <summary>
        ''' PhonePrefix
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PhonePrefix As String
            Get
                Return mPhonePrefix
            End Get
            Set(ByVal value As String)
                mPhonePrefix = value
            End Set
        End Property

        ''' <summary>
        ''' PhoneNo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PhoneNo As String
            Get
                Return mPhoneNo
            End Get
            Set(ByVal value As String)
                mPhoneNo = value
            End Set
        End Property


        ''' <summary>
        ''' Email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Email As String
            Get
                Return mEmail
            End Get
            Set(ByVal value As String)
                mEmail = value
            End Set
        End Property



        ''' <summary>
        ''' List of OrganizationName
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OrganizationName() As String
            Get
                Return mOrganizationName
            End Get
            Set(ByVal value As String)
                mOrganizationName = value
            End Set
        End Property

        ''' <summary>
        ''' List of Department
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Department() As String
            Get
                Return mDepartment
            End Get
            Set(ByVal value As String)
                mDepartment = value
            End Set
        End Property
        ''' <summary>
        ''' List of Unit
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Unit() As String
            Get
                Return mUnit
            End Get
            Set(ByVal value As String)
                mUnit = value
            End Set
        End Property


#End Region

    End Class
End Namespace
