using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.QueryLib_24
{

    public partial class MetaQuery
    {

        /// <summary>
        /// Returns the default valueset or an empty string for no default.
        /// </summary>
        /// <param name="maintable"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public string GetValuesetIdDefaultInGui(string maintable, string variable)
        {
            string myOut = "";
            string sqlString = "SELECT DISTINCT " +
            DB.SubTableVariable.ValueSetCol.ForSelect();
            sqlString += " /" + "*** SQLID:  SubTableVariable_03 ***" + "/";
            sqlString += " FROM " + DB.SubTableVariable.GetNameAndAlias();
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is() +
                           " AND " + DB.SubTableVariable.VariableCol.Is() +
                            " AND " + DB.SubTableVariable.DefaultInGuiCol.Is();

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[3];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(maintable);
            parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(variable);
            parameters[2] = DB.SubTableVariable.DefaultInGuiCol.GetStringParameter(DB.Codes.Yes);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);

            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count > 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(48, maintable, variable);
            }

            foreach (DataRow myRow in myRows)
            {
                myOut = myRow[DB.SubTableVariable.ValueSetCol.Label()].ToString();
            }

            return myOut;

        }

        public List<string> GetValuesetIdsFromSubTableVariableOrderedBySortcode(string maintable, string variable)
        {
            string sqlString = "SELECT DISTINCT " +
            DB.SubTableVariable.ValueSetCol.ForSelect() + ", " +
            DB.SubTableVariable.SortCodeCol.ForSelect() ;

            sqlString += " /" + "*** SQLID:  SubTableVariable_02 ***" + "/";
            sqlString += " FROM " + DB.SubTableVariable.GetNameAndAlias() ;

            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is() +
                           " AND " + DB.SubTableVariable.VariableCol.Is();

            sqlString += " ORDER BY " + DB.SubTableVariable.SortCodeCol.Id();

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(maintable);
            parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(variable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);

            DataRowCollection myRows = ds.Tables[0].Rows;

            List<string> myOut = new List<string>();
            foreach (DataRow myRow in myRows)
            {
                string valuesetId = myRow[DB.SubTableVariable.ValueSetCol.Label()].ToString();
                if (myOut.Contains(valuesetId))
                {
                    throw new PCAxis.Sql.Exceptions.DbException(49, maintable, variable,valuesetId);
                }
                myOut.Add(valuesetId);
            }

            return myOut;
        }

        public bool ShouldSubTableVarablesBeSortedAlphabetical(string MainTable, string Variable)
        {
            var sqlString = new StringBuilder();

            sqlString.AppendLine("SELECT COUNT(*) AS SORTCODECOUNT");
            sqlString.AppendLine("FROM " + DB.SubTableVariable.GetNameAndAlias());
            sqlString.AppendLine("WHERE " + DB.SubTableVariable.MainTableCol.Is());
            sqlString.AppendLine("AND " + DB.SubTableVariable.VariableCol.Is());
            sqlString.AppendLine("AND " + DB.SubTableVariable.Alias + "." + DB.SubTableVariable.SortCodeCol.PureColumnName() + " IS NOT NULL");

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(MainTable);
            parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(Variable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString.ToString(), parameters);
            var sortCodeCount = System.Convert.ToInt32(ds.Tables[0].Rows[0]["SORTCODECOUNT"]);

            return sortCodeCount == 0;
        }

    }
}
