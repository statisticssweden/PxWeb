using System;
using System.Collections.Generic;
using System.Text;

namespace GrandMaster
{
     public partial class GrandMaster
    {
         /// <summary>
         /// Returns all tables except those which have readBySqlDbConfig="false"
         /// </summary>
         public List<TableType> GetReadTables(){
             List<TableType> myOut = new List<TableType>();
             foreach (TableType tt in this.Tables.Table)
             {
                 if (tt.readBySqlDbConfig)
                 {
                     
                         myOut.Add(tt);
                    
                 }
             }
             return myOut;
         }
    }

     public partial class TableType
     {
         /// <summary>
         /// Returns all columns except those which have readBySqlDbConfig="false"
         /// </summary>
         public List<ColumnType> GetReadColumns()
         {
             List<ColumnType> myOut = new List<ColumnType>();
             foreach (ColumnType ct in this.Columns)
             {
                 if (ct.readBySqlDbConfig)
                 {
                     myOut.Add(ct);
                 }
             }
             return myOut;
         }

         /// <summary>
         /// Returns the columns that are PK or langDep except those which have readBySqlDbConfig="false"
         /// </summary>
         public List<ColumnType> GetLanguageRowColumns()
         {
             List<ColumnType> myOut = new List<ColumnType>();
             foreach (ColumnType ct in this.Columns)
             {
                 if (ct.readBySqlDbConfig && ct.inLanguageRow())
                 {
                     myOut.Add(ct);
                 }
             }
             return myOut;
         }

         /// <summary>
         /// Returns the all columns in the secondary table row.
         /// </summary>
         public List<ColumnType> GetAllLanguageRowColumns()
         {
             List<ColumnType> myOut = new List<ColumnType>();
             foreach (ColumnType ct in this.Columns)
             {
                 if (ct.inLanguageRow())
                 {
                     myOut.Add(ct);
                 }
             }
             return myOut;
         }


         public bool hasLanguage()
         {
             return this.hasSL;
         }
     }

     public partial class ColumnType
     {
         /// <summary>
         /// is pk or langDep
         /// </summary>
         public bool inLanguageRow()
         {
             return this.pkFieldSpecified || this.hasSL || this.modelName.Equals("LogDate") || this.modelName.Equals("UserId");
         }

         /// <summary>
         /// returns the deviatingDefaultColumnName if given otherwise the uppered modelname
         /// </summary>
         /// <returns></returns>
         public string defaultNameInDatabase()
         {
             if (String.IsNullOrEmpty(this.deviatingDefaultColumnNameField))
             {
                 return this.modelNameField.ToUpper();
             }
             else
             {
                 return this.deviatingDefaultColumnNameField;
             }
             
         }
     }


}
