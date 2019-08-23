using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;

namespace PCAxis.Sql
{
    public interface PxSqlCommand : System.IDisposable
    {
        DbConnectionStringBuilder connectionStringBuilder();
        int ExecuteNonQuery(string commandString);
        DataSet ExecuteSelect(string selectString);
        DataSet ExecuteSelect(string selectString, DbParameter[] parameters);
        string getConcatString(params string[] myStrings);
        string getExtraDotForDatatables();
        string getKeywordAfterCreateIndicatingTempTable();
        string getOracleNinja1();
        string getOracleNinja2();
        string GetParameterRef(string propertyName);
        string getPrefixIndicatingTempTable();
        bool getProgramMustTruncAndDropTempTable();
        DbParameter GetStringParameter(string parameterName, string parameterValue);
        string getTempTableCreateCommandEndClause();
        string getUniqueNumber(int lengthOfOtherChars);
        int InsertBulk(StringCollection commandStrings, int numberInBulk);
        string MakeTempTable(string VariableName, string VariableNumber, bool makeGroupFactorCol, bool UseTemporaryTables, StringCollection childCodes, StringCollection parentCodes, List<int> parentCodeCounterList);
        string MakeTempTableJustValues(string VariableName, string VariableNumber, bool UseTemporaryTables, StringCollection valueCodes);
    }
}