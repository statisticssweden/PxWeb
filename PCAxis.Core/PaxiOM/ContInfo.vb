Imports PCAxis.Paxiom.ClassAttributes

Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Contentinfo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class ContInfo
        Implements System.Runtime.Serialization.ISerializable

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            mContact = New String(0) {}
            mUnits = New String(0) {}
            mContact(0) = ""
            mContactInfo = New List(Of Contact)() {}
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="internalBufferSize">number of languages</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal internalBufferSize As Integer)
            mContact = New String(internalBufferSize) {}
            mUnits = New String(internalBufferSize) {}
            mContactInfo = New List(Of Contact)(internalBufferSize) {}
        End Sub

#Region "Public methods"
        ''' <summary>
        ''' Creates a copy of the object instance
        ''' </summary>
        ''' <returns>Returns a copy of the object instance</returns>
        ''' <remarks></remarks>
        Public Function CreateCopy() As ContInfo
            Dim ci As ContInfo
            Dim numberOfLanguages As Integer = mUnits.Length

            ci = CType(Me.MemberwiseClone(), ContInfo)
            ci.mContact = New String(numberOfLanguages - 1) {}
            ci.mUnits = New String(numberOfLanguages - 1) {}

            If Me.mContact.Count() = ci.mContact.Count() AndAlso Me.mContact.Count() = numberOfLanguages Then
                For i As Integer = 0 To numberOfLanguages - 1
                    ci.mContact(i) = Me.mContact(i)
                    ci.mUnits(i) = Me.mUnits(i)
                    ci.mContactInfo(i) = Me.mContactInfo(i)
                Next
            End If

            Return ci
        End Function

        ''' <summary>
        ''' Verifies that the mandatory keyword string UNITS has been set for all languages
        ''' </summary>
        ''' <returns>True if all UNITS strings have been set, else false</returns>
        ''' <remarks>"" is a legal value for UNITS</remarks>
        Public Function CheckUnits() As Boolean
            For Each units As String In mUnits
                If units Is Nothing Then
                    'If the units-string is nothing it means it hasn´t been set - Return false
                    Return False
                End If
            Next

            Return True
        End Function
#End Region

#Region "Private fields"
        '<value>The name of value that the ContInfo is connectet to</value>
        Private mValue As String
        Private mBaseperiod As String
        Private mCFPrices As String
        <LanguageDependent()>
        Private mContact() As String
        Private mContactInfo() As List(Of Contact)
        Private mLastUpdated As String
        Private mRefPeriod As String
        Private mSeasAdj As String
        Private mDayAdj As String
        Private mStockFa As String
        <LanguageDependent()> _
        Private mUnits() As String
        Private mReferenceId As String
        Protected Friend mLanguageIndex As Integer = 0

#End Region

#Region "Public Properties"
        ''' <summary>
        ''' The value 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Value() As String
            Get
                Return Me.mValue
            End Get
            Set(ByVal value As String)
                Me.mValue = value
            End Set
        End Property

        ''' <summary>
        ''' Base period
        ''' </summary>
        ''' <value>Base period</value>
        ''' <returns>Base period</returns>
        ''' <remarks></remarks>
        Public Property Baseperiod() As String
            Get
                Return Me.mBaseperiod
            End Get
            Set(ByVal value As String)
                Me.mBaseperiod = value
            End Set
        End Property

        ''' <summary>
        ''' If current or fixed prices
        ''' </summary>
        ''' <value>If current or fixed prices</value>
        ''' <returns>
        ''' PXConstant.CFPRICES_CURRENT if current and 
        ''' PXConstants.CFPRICES_FIXED if fixed
        ''' </returns>
        ''' <remarks></remarks>
        Public Property CFPrices() As String
            Get
                Return Me.mCFPrices
            End Get
            Set(ByVal value As String)
                Me.mCFPrices = value
            End Set
        End Property

        ''' <summary>
        ''' Contact information
        ''' </summary>
        ''' <value>Contact information</value>
        ''' <returns>Contact information</returns>
        ''' <remarks></remarks>
        Public Property Contact() As String
            Get
                'Return Me.mContact.Replace("#", System.Environment.NewLine)
                If (mContactInfo(mLanguageIndex) IsNot Nothing) Then
                    Return FormatContact(mLanguageIndex)
                Else
                    Return Me.mContact(mLanguageIndex)
                End If
            End Get
            Set(ByVal value As String)
                Me.mContact(mLanguageIndex) = value
            End Set
        End Property
        ''' <summary>
        ''' Contact information
        ''' </summary>
        ''' <value>Contact information</value>
        ''' <returns>Contact information</returns>
        ''' <remarks></remarks>
        Public Property ContactInfo() As List(Of Contact)
            Get
                'Return Me.mContact.Replace("#", System.Environment.NewLine)
                Return Me.mContactInfo(mLanguageIndex)
            End Get
            Set(ByVal value As List(Of Contact))
                Me.mContactInfo(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Last updated
        ''' </summary>
        ''' <value>Last updated</value>
        ''' <returns>Last updated</returns>
        ''' <remarks></remarks>
        Public Property LastUpdated() As String
            Get
                Return Me.mLastUpdated
            End Get
            Set(ByVal value As String)
                Me.mLastUpdated = value
            End Set
        End Property

        ''' <summary>
        ''' Refrence period
        ''' </summary>
        ''' <value>Refrence period</value>
        ''' <returns>Refrence period</returns>
        ''' <remarks></remarks>
        Public Property RefPeriod() As String
            Get
                Return Me.mRefPeriod
            End Get
            Set(ByVal value As String)
                Me.mRefPeriod = value
            End Set
        End Property

        ''' <summary>
        ''' If Season adjusted
        ''' </summary>
        ''' <value>If Season adjusted</value>
        ''' <returns>If Season adjusted</returns>
        ''' <remarks></remarks>
        Public Property SeasAdj() As String
            Get
                Return Me.mSeasAdj
            End Get
            Set(ByVal value As String)
                Me.mSeasAdj = value
            End Set
        End Property

        ''' <summary>
        ''' If day adjusted
        ''' </summary>
        ''' <value>If day adjusted</value>
        ''' <returns>If day adjusted</returns>
        ''' <remarks></remarks>
        Public Property DayAdj() As String
            Get
                Return Me.mDayAdj
            End Get
            Set(ByVal value As String)
                Me.mDayAdj = value
            End Set
        End Property

        ''' <summary>
        ''' If Stock, Flow or Average
        ''' </summary>
        ''' <value>Stock, Flow or Average</value>
        ''' <returns>Stock, Flow, Average or Other</returns>
        ''' <remarks></remarks>
        Public Property StockFa() As String
            Get
                Return Me.mStockFa
            End Get
            Set(ByVal value As String)
                Me.mStockFa = value
            End Set
        End Property

        ''' <summary>
        ''' The unit
        ''' </summary>
        ''' <value>The unit</value>
        ''' <returns>The unit</returns>
        ''' <remarks></remarks>
        Public Property Units() As String
            Get
                Return Me.mUnits(mLanguageIndex)
            End Get
            Set(ByVal value As String)
                Me.mUnits(mLanguageIndex) = value
            End Set
        End Property

        ''' <summary>
        ''' Optinal refrence id could be anything
        ''' </summary>
        ''' <remarks></remarks>
        Public Property RefrenceID() As String
            Get
                Return mReferenceId
            End Get
            Set(ByVal value As String)
                mReferenceId = value
            End Set
        End Property
#End Region

#Region "Protected Set-methods"

        Protected Friend Sub ResizeLanguageVariables(ByVal size As Integer)
            Util.ResizeLanguageDependentFields(Me, size)
        End Sub

        ''' <summary>
        ''' Make the current language the default language
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Sub SetCurrentLanguageDefault()
            If mLanguageIndex > 0 Then
                'Switch text values between current language and the old default language
                Util.SwitchLanguages(Me, 0, mLanguageIndex)
            End If
        End Sub

        Protected Friend Sub SetLanguage(ByVal languageIndex As Integer)
            mLanguageIndex = languageIndex
        End Sub

        ''' <summary>
        ''' This function should only be invoked by internal methods and is not for use by the programmer
        ''' </summary>
        ''' <param name="name">name of the property <see cref="PXKeywords">PXKeywords</see></param>
        ''' <param name="value">the value of the property</param>
        ''' <remarks></remarks>
        Protected Friend Sub SetProperty(ByVal name As String, ByVal value As String)
            Select Case name
                Case PXKeywords.BASEPERIOD
                    Me.mBaseperiod = value
                Case PXKeywords.CFPRICES
                    Me.mCFPrices = value
                Case PXKeywords.CONTACT
                    If (value.Contains("||")) Then
                        Me.mContactInfo(mLanguageIndex) = HandleContacts(value)
                    Else
                        Me.mContact(mLanguageIndex) = value
                    End If
                Case PXKeywords.LAST_UPDATED
                    Me.mLastUpdated = value
                Case PXKeywords.REFPERIOD
                    Me.mRefPeriod = value
                Case PXKeywords.SEASADJ
                    Me.mSeasAdj = value
                Case PXKeywords.DAYADJ
                    Me.mDayAdj = value
                Case PXKeywords.STOCKFA
                    Me.mStockFa = value
                Case PXKeywords.UNITS
                    Me.mUnits(mLanguageIndex) = value
                Case PXKeywords.REFRENCE_ID
                    Me.mReferenceId = value
            End Select
        End Sub

#End Region

        ''' <summary>
        ''' Splits a Contactinfo containing ## into multiple contacts. 
        ''' </summary>
        ''' <param name="inputString">The input contact string</param>
        ''' <returns>A list of contact objects</returns>
        ''' <remarks></remarks>
        Private Function HandleContacts(ByVal inputString As String) As List(Of Contact)
            Dim separator As String() = {"||"}
            Dim arrContacts As String() = inputString.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            Dim contacts As New List(Of Contact)
            mContactInfo(mLanguageIndex) = New List(Of Contact)

            separator = {"#"}
            For Each contact As String In arrContacts
                Dim arrContact As String() = contact.Split(separator, StringSplitOptions.None)
                Dim mContact As Contact = New Contact()
                mContact.Forname = arrContact(0)
                mContact.Surname = arrContact(1)
                mContact.PhonePrefix = arrContact(2)
                mContact.PhoneNo = arrContact(3)
                mContact.Email = arrContact(4)
                mContact.OrganizationName = arrContact(5)
                mContact.Department = arrContact(6)
                mContact.Unit = arrContact(7)
                mContactInfo(mLanguageIndex).Add(mContact)

            Next

            Return mContactInfo(mLanguageIndex)

        End Function
        ''' <summary>
        ''' Formats Contacts as String 
        ''' </summary>
        ''' <returns>Stirn containing formatted contactinfo</returns>
        ''' <remarks></remarks>
        Private Function FormatContact(languageIndex As Integer) As String
            Dim builder As New System.Text.StringBuilder
            For Each contact As Contact In Me.mContactInfo(languageIndex)
                builder.Append(contact.Forname + " " + contact.Surname + ", " + contact.OrganizationName + "#")
                builder.Append(contact.PhonePrefix + " " + contact.PhoneNo + "#")
                builder.Append(contact.Email + "#")
                'builder.Append(PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString("PxcContactPhone") + " " + contact.PhonePrefix + " " + contact.PhoneNo + "#")
                'builder.Append(PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString("PxcContactEMail") + " " + contact.Email + "#")
                builder.Append("#")
            Next
            Return builder.ToString()
        End Function

        ''' <summary>
        ''' Custom serializer
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("Contact", mContact)
            info.AddValue("Baseperiod", mBaseperiod)
            info.AddValue("mCFPrices", mCFPrices)
            info.AddValue("mLastUpdated", mLastUpdated)
            info.AddValue("mRefPeriod", mRefPeriod)
            info.AddValue("mSeasAdj", mSeasAdj)
            info.AddValue("mDayAdj", mDayAdj)
            info.AddValue("mStockFa", mStockFa)
            info.AddValue("mUnits", mUnits)
        End Sub

        ''' <summary>
        ''' Constructor for custom serialization
        ''' </summary>
        ''' <param name="info">SerializationInfo</param>
        ''' <param name="context">StreamingContext</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            mContact = CType(info.GetValue("Contact", GetType(String())), String())
            mBaseperiod = info.GetString("Baseperiod")
            mCFPrices = info.GetString("mCFPrices")
            mLastUpdated = info.GetString("mLastUpdated")
            mRefPeriod = info.GetString("mRefPeriod")
            mSeasAdj = info.GetString("mSeasAdj")
            mDayAdj = info.GetString("mDayAdj")
            mStockFa = info.GetString("mStockFa")
            mUnits = CType(info.GetValue("mUnits", GetType(String())), String())
        End Sub

    End Class
End Namespace
