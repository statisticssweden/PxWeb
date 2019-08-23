Namespace PCAxis.Paxiom

    ''' <summary>
    ''' Paxiom constants
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PXConstant
        ''' <summary>
        ''' Value representation for datasymbol 1
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_1 As Double = -1.0E+19

        ''' <summary>
        ''' Value representation for datasymbol 2
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_2 As Double = -1.0E+20

        ''' <summary>
        ''' Value representation for datasymbol 3
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_3 As Double = -1.0E+21

        ''' <summary>
        ''' Value representation for datasymbol 4
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_4 As Double = -1.0E+22

        ''' <summary>
        ''' Value representation for datasymbol 5
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_5 As Double = -9.9999999991E+22

        ''' <summary>
        ''' Value representation for datasymbol 6
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_6 As Double = -1.0E+24

        ''' <summary>
        ''' Value representation for datasymbol NIL
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_NIL As Double = -1.0E+25

        'TODO Sätt till riktigt värde
        ''' <summary>
        ''' Value representation for datasymbol 7
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_7 As Double = -9999999999999.9

        ''' <summary>
        ''' Default string representation for datasymbol 1
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_1_STRING As String = "."

        ''' <summary>
        ''' Default string representation for datasymbol 2
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_2_STRING As String = ".."

        ''' <summary>
        ''' Default string representation for datasymbol 3
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_3_STRING As String = "..."

        ''' <summary>
        ''' Default string representation for datasymbol 4
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_4_STRING As String = "...."

        ''' <summary>
        ''' Default string representation for datasymbol 5
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_5_STRING As String = "....."

        ''' <summary>
        ''' Default string representation for datasymbol 6
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_6_STRING As String = "......"

        ''' <summary>
        ''' Default string representation for datasymbol 7
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_7_STRING As String = "......."

        ''' <summary>
        ''' Default string representation for datasymbol NIL
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DATASYMBOL_NIL_STRING As String = "-"

        ''' <summary>
        ''' String representation for YES
        ''' </summary>
        ''' <remarks></remarks>
        Public Const YES As String = "YES"

        ''' <summary>
        ''' String representation for datasymbol NO
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NO As String = "NO"

        ''' <summary>
        ''' String representation CURRENT for current or fixed prices
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CFPRICES_CURRENT As String = "C"

        ''' <summary>
        ''' String representation FIXED for current or fixed prices
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CFPRICES_FIXED As String = "F"

        ''' <summary>
        ''' String representation STOCK for stock or flow or avergae
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STOCKFA_STOCK As String = "S"

        ''' <summary>
        ''' String representation FLOW for stock or flow or average
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STOCKFA_FLOW As String = "F"

        ''' <summary>
        ''' String representation AVERAGE for stock or flow or average
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STOCKFA_AVERAGE As String = "A"

        ''' <summary>
        ''' String representation OTHER for stock or flow or average
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STOCKFA_OTHER As String = "X"


        ''' <summary>
        ''' String representation of There are no text, please use codes 
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VALUETEXTOPTION_NOTEXT As String = "S"

        ''' <summary>
        ''' String representation of The texts are very long, please display codes 
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VALUETEXTOPTION_TOOLONG As String = "X"

        ''' <summary>
        ''' String representation of The texts are suitable for display. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VALUETEXTOPTION_NORMAL As String = "N"





        ''' <summary>
        ''' The default representation of a sort variable and also the code for the 
        ''' sort variable
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SORTVARIABLE As String = "$$SORT"

        ''' <summary>
        ''' The Datetime format used in PC-Axis files
        ''' </summary>
        ''' <remarks>Year (4 characters), month (2), day (2), hour (2), colon (:), minute (2)</remarks>
        Public Const PXDATEFORMAT As String = "yyyyMMdd HH:mm"

        ''' <summary>
        ''' Array of all datasymbol values
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ProtectedValues() As Double = New Double() {DATASYMBOL_NIL, DATASYMBOL_1, DATASYMBOL_2, DATASYMBOL_3, DATASYMBOL_4, DATASYMBOL_5, DATASYMBOL_6, DATASYMBOL_7}

        ''' <summary>
        ''' Array of all protected datasymbol NIL
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ProtectedNullValues() As Double = New Double() {DATASYMBOL_NIL}

    End Class

End Namespace
