using PCAxis.Sql.DbClient;
using PCAxis.Sql.DbConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Sql.Parser
{
    public class EliminationDataItem
    {
        public string Variable { get; set; }
        public string ValueSet { get; set; }
        public string EliinationMethod { get; set; }
        public string EliinationCode { get; set; }
    
    }

    public class PxSQLEliniationProvider
    {
        public static void ApplyEliminationIfSupported(PCAxis.Paxiom.Selection[] selections, SqlDbConfig config, InfoForDbConnection dbInfo, PCAxis.Paxiom.PXMeta pxMeta)
        {
            /*
            SqlDbConfig_24 DB = config as SqlDbConfig_24;

            if (DB != null && config.MetaModel.Equals("2.4"))
            {
                PxSqlCommand mSqlCommand = new PxSqlCommand(dbInfo.DataBaseType, dbInfo.DataProvider, dbInfo.ConnectionString);
                string eliminationMethodC = "C";

                foreach (var selection in selections.Where(x => x.ValueCodes.Count == 0).ToArray())
                {
                    StringBuilder sqlString = new StringBuilder("");
                    sqlString.AppendLine("SELECT DISTINCT ");
                    sqlString.AppendLine(DB.ValueSet.EliminationCodeCol.ForSelect());
                    sqlString.AppendLine(" FROM " + DB.ValueSet.GetNameAndAlias());
                    sqlString.AppendLine(" INNER JOIN " + DB.SubTableVariable.GetNameAndAlias());
                    sqlString.AppendLine(" ON " + DB.ValueSet.Alias + "." + DB.ValueSet.ValueSetCol.PureColumnName() + " = " + DB.SubTableVariable.Alias + "." + DB.SubTableVariable.ValueSetCol.PureColumnName());
                    sqlString.AppendLine(" WHERE " + DB.SubTableVariable.MainTableCol.Is());
                    sqlString.AppendLine(" AND " + DB.SubTableVariable.VariableCol.Is());
                    sqlString.AppendLine(" AND " + DB.ValueSet.EliminationMethodCol.Is());

                    System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[3];
                    parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(pxMeta.MainTable);
                    parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(selection.VariableCode);
                    parameters[2] = DB.SubTableVariable.VariableCol.GetStringParameter(eliminationMethodC);

                    var ds = mSqlCommand.ExecuteSelect(sqlString.ToString(), parameters);
                    var dt = ds.Tables[0];

                    if(dt.Rows.Count != 1) throw new Exception("Could not resolve eliminination code");

                    var eliminationCode = (string)dt.Rows[0][0];
                    selection.ValueCodes.Add(eliminationCode);
                }
            }
            */
        }
    }
}
