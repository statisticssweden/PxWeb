using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.QueryLib_24 {
    /// <summary> 
    /// Holds the attributes for reduced version SubTableVariable.
    /// </summary> 
    public class MainTableVariableRow {

        public static string sqlString(SqlDbConfig_24 DB) {
          
            string currentMethod = "MainTableVariableRow.sqlString";
            SqlDbConfig_24.TblSubTableVariable stv = DB.SubTableVariable;

            return "SELECT DISTINCT " + stv.VariableCol.ForSelect() + ", " +
                                      stv.VariableTypeCol.ForSelect() + "," +
                                      stv.StoreColumnNoCol.ForSelect() +
             " /*** SQLID: " + currentMethod + "_01 ***/ " +
              " FROM " + DB.MetatablesSchema + stv.TableName + " " + stv.Alias +
              " WHERE " + stv.MainTableCol.Is();

            // Ikke  FROM      DB.SubTableVariable.GetNameAndAlias();  ?

            // SELECT DISTINCT STB.Variable, STB.VariableType og StoreColumnNoCol  /*** SQLID: GetMainTablesVariablesById_01 ***/ 
            // FROM MetaData.SubTableVariable STB
            // WHERE STB.MainTable = '<mainTable>'
        }

        private String mVariable;
        public String Variable {
            get { return mVariable; }
        }

        private String mVariableType;
        public String VariableType {
            get { return mVariableType; }
        }

        private String mStoreColumnNo;
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }
        public MainTableVariableRow(DataRow myRow, SqlDbConfig_24 dbconf) {
            this.mVariable = myRow[dbconf.SubTableVariable.VariableCol.Label()].ToString();
            this.mVariableType = myRow[dbconf.SubTableVariable.VariableTypeCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.SubTableVariable.StoreColumnNoCol.Label()].ToString();
        }

    }



}
