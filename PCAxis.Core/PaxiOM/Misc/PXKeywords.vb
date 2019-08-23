Namespace PCAxis.Paxiom

    Public Class PXKeywords
        'Mandantory
        Public Const MATRIX As String = "MATRIX"
        Public Const SUBJECT_CODE As String = "SUBJECT-CODE"
        Public Const SUBJECT_AREA As String = "SUBJECT-AREA"
        Public Const CONTENTS As String = "CONTENTS"
        Public Const UNITS As String = "UNITS"
        Public Const DECIMALS As String = "DECIMALS"
        Public Const TITLE As String = "TITLE"
        Public Const DESCRIPTION As String = "DESCRIPTION"
        Public Const STUB As String = "STUB"
        Public Const HEADING As String = "HEADING"
        Public Const VALUES As String = "VALUES"
        Public Const DATA As String = "DATA"
        'Optional
        Public Const AGGREGALLOWED As String = "AGGREGALLOWED"
        Public Const AUTOPEN As String = "AUTOPEN"
        Public Const AXIS_VERSION As String = "AXIS-VERSION"
        Public Const BASEPERIOD As String = "BASEPERIOD"
        Public Const CELLNOTE As String = "CELLNOTE"
        Public Const CELLNOTEX As String = "CELLNOTEX"
        Public Const CFPRICES As String = "CFPRICES"
        Public Const CHARSET As String = "CHARSET"
        Public Const CODEPAGE As String = "CODEPAGE"
        Public Const CODES As String = "CODES"
        Public Const CONFIDENTIAL As String = "CONFIDENTIAL"
        Public Const CONTACT As String = "CONTACT"
        Public Const CONTVARIABLE As String = "CONTVARIABLE"
        Public Const COPYRIGHT As String = "COPYRIGHT"
        Public Const CREATION_DATE As String = "CREATION-DATE"
        Public Const DATABASE As String = "DATABASE"
        'Public Const DATANOTE As String = "DATANOTE"
        Public Const DATANOTESUM As String = "DATANOTESUM"
        Public Const DATASYMBOL1 As String = "DATASYMBOL1"
        Public Const DATASYMBOL2 As String = "DATASYMBOL2"
        Public Const DATASYMBOL3 As String = "DATASYMBOL3"
        Public Const DATASYMBOL4 As String = "DATASYMBOL4"
        Public Const DATASYMBOL5 As String = "DATASYMBOL5"
        Public Const DATASYMBOL6 As String = "DATASYMBOL6"
        'Public Const DATASYMBOL7 As String = "DATASYMBOL7"
        Public Const DATASYMBOLNIL As String = "DATASYMBOLNIL"
        Public Const DATASYMBOLSUM As String = "DATASYMBOLSUM"
        Public Const DAYADJ As String = "DAYADJ"
        'Public Const PX_DECIMAL As String = "DECIMAL"
        Public Const DESCRIPTIONDEFAULT As String = "DESCRIPTIONDEFAULT"
        Public Const DOMAIN As String = "DOMAIN"
        Public Const DOUBLECOLUMN As String = "DOUBLECOLUMN"
        Public Const ELIMINATION As String = "ELIMINATION"
        Public Const INFOFILE As String = "INFOFILE"
        Public Const KEYS As String = "KEYS"
        Public Const LANGUAGE As String = "LANGUAGE"
        Public Const LANGUAGES As String = "LANGUAGES"
        Public Const LAST_UPDATED As String = "LAST-UPDATED"
        Public Const MAP As String = "MAP"
        Public Const NOTE As String = "NOTE"
        Public Const NOTEX As String = "NOTEX"
        Public Const PARTITIONED As String = "PARTITIONED"
        Public Const PRECISION As String = "PRECISION"
        Public Const PRESTEXT As String = "PRESTEXT"
        Public Const REFPERIOD As String = "REFPERIOD"
        Public Const SEASADJ As String = "SEASADJ"
        Public Const SHOWDECIMALS As String = "SHOWDECIMALS"
        Public Const SOURCE As String = "SOURCE"
        Public Const STOCKFA As String = "STOCKFA"
        Public Const TIMEVAL As String = "TIMEVAL"
        Public Const VALUENOTE As String = "VALUENOTE"
        Public Const VALUENOTEX As String = "VALUENOTEX"
        Public Const UPDATE_FREQUENCY As String = "UPDATE-FREQUENCY"
        Public Const NEXT_UPDATE As String = "NEXT-UPDATE"
        Public Const PX_SERVER As String = "PX-SERVER"
        Public Const DIRECTORY_PATH As String = "DIRECTORY-PATH"
        Public Const INFO As String = "INFO"
        Public Const LINK As String = "LINK"
        Public Const SURVEY As String = "SURVEY"
        Public Const TABLEID As String = "TABLEID"
        Public Const DEFAULT_GRAPH As String = "DEFAULT-GRAPH"
        Public Const VARIABLE_TYPE As String = "VARIABLE-TYPE"
        Public Const HIERARCHIES As String = "HIERARCHIES"
        Public Const HIERARCHYNAMES As String = "HIERARCHYNAMES"
        Public Const HIERARCHYLEVELS As String = "HIERARCHYLEVELS"
        Public Const HIERARCHYLEVELSOPEN As String = "HIERARCHYLEVELSOPEN"
        Public Const DATANOTECELL As String = "DATANOTECELL"
        Public Const ROUNDING As String = "ROUNDING"
        Public Const SYNONYMS As String = "SYNONYMS"
        'EXTRA
        Public Const VARIABLE_CODE As String = "VARIABLECODE"
        Public Const EXTENDED_PROPERTIES As String = "EXTENDEDPROPERTIES"
        Public Const VALUESET_ID As String = "VALUESET-ID"
        Public Const VALUESET_NAME As String = "VALUESET-NAME"
        Public Const GROUPING_ID As String = "GROUPING-ID"
        Public Const GROUPING_NAME As String = "GROUPING-NAME"
        Public Const GROUPING_GROUPPRES As String = "GROUPING-PRESENTATION"
        Public Const REFRENCE_ID As String = "REFERENCE-ID"
        Public Const VALUE_TEXT_OPTION As String = "VALUE-TEXT-OPTION"
        Public Const MAINTABLE As String = "MAINTABLE"
        Public Const OFFICIAL_STATISTICS As String = "OFFICIAL-STATISTICS"
        Public Const ATTRIBUTE_ID As String = "ATTRIBUTE-ID"
        Public Const ATTRIBUTE_TEXT As String = "ATTRIBUTE-TEXT"
        Public Const ATTRIBUTES As String = "ATTRIBUTES"
        Public Const FIRST_PUBLISHED As String = "FIRST-PUBLISHED"
        Public Const DATANOTE As String = "DATANOTE"
        Public Const META_ID As String = "META-ID"

#Region "Class KeywordInfo"
        Public Class KeywordInfo
            Private mMandatory As Boolean
            Private mMultiLingual As Boolean
            Public ReadOnly Property IsMandantory() As Boolean
                Get
                    Return mMandatory
                End Get
            End Property

            Public ReadOnly Property IsMultiLingual() As Boolean
                Get
                    Return mMultiLingual
                End Get
            End Property

            Public Sub New(ByVal mandantory As Boolean, ByVal multiLingual As Boolean)
                mMandatory = mandantory
                mMultiLingual = multiLingual
            End Sub
        End Class
#End Region

        Private Shared mKeywords As Hashtable

        Shared Sub New()
            mKeywords = New Hashtable

            'Mandantory
            mKeywords.Add(MATRIX, New KeywordInfo(True, False))
            mKeywords.Add(SUBJECT_CODE, New KeywordInfo(True, False))
            mKeywords.Add(SUBJECT_AREA, New KeywordInfo(True, True))
            mKeywords.Add(CONTENTS, New KeywordInfo(True, True))
            mKeywords.Add(UNITS, New KeywordInfo(True, True))
            mKeywords.Add(DECIMALS, New KeywordInfo(True, True))
            mKeywords.Add(TITLE, New KeywordInfo(True, True))
            mKeywords.Add(DESCRIPTION, New KeywordInfo(True, True))
            mKeywords.Add(STUB, New KeywordInfo(True, True))
            mKeywords.Add(HEADING, New KeywordInfo(True, True))
            mKeywords.Add(VALUES, New KeywordInfo(True, True))
            mKeywords.Add(DATA, New KeywordInfo(True, False))
            'Optional
            mKeywords.Add(AGGREGALLOWED, New KeywordInfo(False, False))
            mKeywords.Add(AUTOPEN, New KeywordInfo(False, False))
            mKeywords.Add(AXIS_VERSION, New KeywordInfo(False, False))
            mKeywords.Add(BASEPERIOD, New KeywordInfo(False, True))
            mKeywords.Add(CELLNOTE, New KeywordInfo(False, True))
            mKeywords.Add(CELLNOTEX, New KeywordInfo(False, True))
            mKeywords.Add(CFPRICES, New KeywordInfo(False, False))
            mKeywords.Add(CHARSET, New KeywordInfo(False, False))
            mKeywords.Add(CODEPAGE, New KeywordInfo(False, False))
            mKeywords.Add(CODES, New KeywordInfo(False, True))
            mKeywords.Add(CONFIDENTIAL, New KeywordInfo(False, False))
            mKeywords.Add(CONTACT, New KeywordInfo(False, True))
            mKeywords.Add(CONTVARIABLE, New KeywordInfo(False, True))
            mKeywords.Add(COPYRIGHT, New KeywordInfo(False, False))
            mKeywords.Add(CREATION_DATE, New KeywordInfo(False, False))
            mKeywords.Add(DATABASE, New KeywordInfo(False, True))
            'mKeywords.Add(DATANOTE, New KeywordInfo(False, True))
            mKeywords.Add(DATANOTESUM, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL1, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL2, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL3, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL4, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL5, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOL6, New KeywordInfo(False, True))
            'mKeywords.Add(DATASYMBOL7, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOLNIL, New KeywordInfo(False, True))
            mKeywords.Add(DATASYMBOLSUM, New KeywordInfo(False, True))
            mKeywords.Add(DAYADJ, New KeywordInfo(False, True))
            'mKeywords.Add(PX_DECIMAL, New KeywordInfo(False, False))
            mKeywords.Add(DESCRIPTIONDEFAULT, New KeywordInfo(False, False))
            mKeywords.Add(DOMAIN, New KeywordInfo(False, True))
            mKeywords.Add(DOUBLECOLUMN, New KeywordInfo(False, True))
            mKeywords.Add(ELIMINATION, New KeywordInfo(False, True))
            mKeywords.Add(INFOFILE, New KeywordInfo(False, True))
            mKeywords.Add(KEYS, New KeywordInfo(False, True))
            mKeywords.Add(LANGUAGE, New KeywordInfo(False, False))
            mKeywords.Add(LANGUAGES, New KeywordInfo(False, False))
            mKeywords.Add(LAST_UPDATED, New KeywordInfo(False, False))
            mKeywords.Add(MAP, New KeywordInfo(False, True))
            mKeywords.Add(NOTE, New KeywordInfo(False, True))
            mKeywords.Add(NOTEX, New KeywordInfo(False, True))
            mKeywords.Add(PARTITIONED, New KeywordInfo(False, True))
            mKeywords.Add(PRECISION, New KeywordInfo(False, True))
            mKeywords.Add(PRESTEXT, New KeywordInfo(False, True))
            mKeywords.Add(REFPERIOD, New KeywordInfo(False, True))
            mKeywords.Add(SEASADJ, New KeywordInfo(False, True))
            mKeywords.Add(SHOWDECIMALS, New KeywordInfo(False, False))
            mKeywords.Add(SOURCE, New KeywordInfo(False, True))
            mKeywords.Add(STOCKFA, New KeywordInfo(False, True))
            mKeywords.Add(TIMEVAL, New KeywordInfo(False, True))
            mKeywords.Add(VALUENOTE, New KeywordInfo(False, True))
            mKeywords.Add(VALUENOTEX, New KeywordInfo(False, True))
            mKeywords.Add(UPDATE_FREQUENCY, New KeywordInfo(False, False))
            mKeywords.Add(NEXT_UPDATE, New KeywordInfo(False, False))
            mKeywords.Add(PX_SERVER, New KeywordInfo(False, False))
            mKeywords.Add(DIRECTORY_PATH, New KeywordInfo(False, False))
            mKeywords.Add(INFO, New KeywordInfo(False, False))
            mKeywords.Add(LINK, New KeywordInfo(False, False))
            mKeywords.Add(SURVEY, New KeywordInfo(False, False))
            mKeywords.Add(DEFAULT_GRAPH, New KeywordInfo(False, False))
            mKeywords.Add(VARIABLE_TYPE, New KeywordInfo(False, False))
            mKeywords.Add(HIERARCHIES, New KeywordInfo(False, True))
            mKeywords.Add(HIERARCHYNAMES, New KeywordInfo(False, True))
            mKeywords.Add(HIERARCHYLEVELS, New KeywordInfo(False, True))
            mKeywords.Add(HIERARCHYLEVELSOPEN, New KeywordInfo(False, True))
            mKeywords.Add(DATANOTECELL, New KeywordInfo(False, True))
            mKeywords.Add(OFFICIAL_STATISTICS, New KeywordInfo(False, False))
            mKeywords.Add(ATTRIBUTE_ID, New KeywordInfo(False, False))
            mKeywords.Add(ATTRIBUTE_TEXT, New KeywordInfo(False, True))
            mKeywords.Add(ATTRIBUTES, New KeywordInfo(False, False))
            'Extras
            mKeywords.Add(VARIABLE_CODE, New KeywordInfo(False, False))
            mKeywords.Add(EXTENDED_PROPERTIES, New KeywordInfo(False, False))
            mKeywords.Add(FIRST_PUBLISHED, New KeywordInfo(False, False))
            mKeywords.Add(DATANOTE, New KeywordInfo(False, True))
            mKeywords.Add(META_ID, New KeywordInfo(False, True))

        End Sub

        Public Shared ReadOnly Property Keywords(ByVal keyword As String) As KeywordInfo
            Get
                Dim obj As Object = mKeywords(keyword)
                If obj Is Nothing Then
                    Return Nothing
                End If
                Return DirectCast(obj, KeywordInfo)
            End Get
        End Property

    End Class

End Namespace