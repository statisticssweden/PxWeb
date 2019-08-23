using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_24;
using PCAxis.PlugIn.Sql;
using PCAxis.Sql.Pxs;
using System.Collections.Specialized;

namespace PCAxis.Sql.Parser_24
{
    public class PXSqlSubTables:Dictionary<string,PXSqlSubTable>
    {
       
        PxsQuery mPxsQuery;


        public PXSqlSubTables(Dictionary<string, SubTableRow> altIBasen, PxsQuery pPxsQuery, PXSqlMeta_24 pxsqlMeta)
            : base() {
            this.mPxsQuery = pPxsQuery;
            PXSqlSubTable mSubTable;
            foreach (SubTableRow subTableRow in altIBasen.Values) {
                mSubTable = new PXSqlSubTable(subTableRow);
                this.Add(mSubTable.SubTable, mSubTable);
            }
            // set selected subtables.
            if (pPxsQuery == null)
            {
                foreach (PXSqlSubTable subTable in this.Values) {
                    subTable.IsSelected = true;
                }
            } else {
                SetSelectedSubTable(pxsqlMeta);
            }
        }

   
  

        /// <summary>
        /// Returns the ids of the subtables marked as selected(All if not a pxs has selected a spesific subtable.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetKeysOfSelectedSubTables()
        {
            List<string> myOut = new List<string>();
            foreach (PXSqlSubTable subTable in this.Values)
            {
                if (subTable.IsSelected)
                {
                    myOut.Add(subTable.SubTable);

                }

            }
            return myOut;
        }

        private void SetSelectedSubTable(PXSqlMeta_24 pxsqlMeta)
        {
            
            Boolean isEqualValueSet;

            foreach (KeyValuePair<string, PCAxis.Sql.Parser_24.PXSqlSubTable> subTable in this)           
            {
                isEqualValueSet = true;
                string subTableName = subTable.Value.SubTable;
                Dictionary <string, SubTableVariableRow> fromDb = pxsqlMeta.MetaQuery.GetSubTableVariableRowskeyVariable(pxsqlMeta.MainTable.MainTable, subTableName,false);
                
                foreach (PQVariable pqvariable in mPxsQuery.Query.Variables)
                {                   
                    string variableName = pqvariable.code;
                    string selectedValueset = pqvariable.SelectedValueset;
                    if (!fromDb.ContainsKey(variableName))
                    {
                        throw new Exception("The variable \"" + variableName + "\" given in the query, does not exist in the database, for the maintable \"" + pxsqlMeta.MainTable.MainTable + "\". Is the query of date? Or the database?");
                    }
                    string valuesetIdFromSubTableVariableRow = fromDb[variableName].ValueSet;
                    if (selectedValueset != valuesetIdFromSubTableVariableRow && selectedValueset != PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)
                    {
                        isEqualValueSet = false;
                        break;
                    }                
                }
                if (isEqualValueSet)
                {
                   
                    this[subTableName].IsSelected = true;
                }

            }

            
        }
    }
}
