using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Configuration;

using GrandMaster;

namespace generateClassesForQueryLib
{


    class MetaQueryMaker:FileMaker
    {
        private GrandMaster.GrandMaster myMaster;
        
        private string outDirMetaQ;


        internal MetaQueryMaker(GrandMaster.GrandMaster myMaster, string outDirMetaQ)
        {
            this.myMaster = myMaster;
            this.outDirMetaQ = outDirMetaQ;
        }

        internal void genMetaQuery()
        {

            foreach (TableType tt in myMaster.GetReadTables())
            {
                if (tt.generate.onlyForDbconfig == null)
                {
                    string utFil = outDirMetaQ + "MetaQuery_" + tt.modelName + ".cs";
                    outStreamWriter = new StreamWriter(utFil);

                    String[] myArr = new String[] { "System","System.Data","System.Collections.Generic",
                "System.Collections.Specialized","System.Text","System.Xml.XPath","System.Globalization","",
                "PCAxis.Sql.DbConfig","PCAxis.Sql.Exceptions","",""};
                    String cnmmVersion = myMaster.Tables.CNMMversion;

                    printUsing(myArr);

                    printThisCodeIsGeneratedWarning();
                    printLine("namespace PCAxis.Sql.QueryLib_" + cnmmVersion);
                    printBegin();
                    printLine("public partial class MetaQuery");
                    printBegin();

                    setHelpers(tt);

                    genMetaQueryForTable(tt);

                    printEnd();
                    printEnd();

                    outStreamWriter.Close();
                    Console.Out.WriteLine("Wrote to file: " + utFil);
                }
            }

        }


        private void genMetaQueryForTable(TableType tt)
        {

            if (!String.IsNullOrEmpty(tt.Comment))
            {
                printLine("/*" + tt.Comment + "*/");
            }
            
                if (tt.generate.getByPk != null)
                {
                    allPk(tt);
                }

                if (tt.generate.getAllRows != null)
                {

                    allRows(tt);
                }
                if (tt.generate.Dictionary != null)
                {
                    foreach (TableTypeGenerateDictionary ttgd in tt.generate.Dictionary)
                    {
                        generateDict(ttgd, tt);
                    }
                }
            

            nysqlStringNoWhere(tt);

        }






        private void nysqlStringNoWhere(TableType tt)
        {
            printLine("");


            printLine("private String " + noWhereSqlDef);
            printBegin();
            printLine("//SqlDbConfig dbconf = DB;   ");
            printLine("string sqlString = \"SELECT \";");

            printLine("");
            printLine("");
            // end piv

            printLine("sqlString +=");
            indent++;
            for (int n = 0; n < colsInUse.Length; n++)
            {
                ColumnType ct = colsInUse[n];

                printStartLine("DB." + tabName + "." + ct.modelName + "Col.ForSelect()");
                if (n < colsInUse.Length - 1)
                {
                    printEndLine(" + \", \" +");

                }
            }
            printEndLine(";");
            indent--;

            printLine("");
            ////////  extra langs
            if (!String.IsNullOrEmpty(langCodes))
            {
                string idPreLang2 = dbPre + "Lang2.Alias + DB.GetMetaSuffix(langCode) + \".\" + " + dbPre + "Lang2.";
                string namePreLang2 = dbPre + "Lang2.Alias + DB.GetMetaSuffix(langCode) + \"_\" + " + dbPre + "Lang2.";
                printLine("");
                printLine("foreach (String langCode in mLanguageCodes)");
                printBegin();
                printLine("if (DB.isSecondaryLanguage(langCode))");
                printBegin();
                foreach (ColumnType ct in languageDependentCols)
                {

                    printLine("sqlString += \", \" + DB." + tabName + "Lang2." + ct.modelName + "Col.ForSelectWithFallback(langCode, DB." + tabName + "." + ct.modelName + "Col);");


                }
                printEnd();
                printEnd();
                printLine("");
            }


            //  from clause 
            printLine("sqlString += \" /\" + \"*** SQLID: " + methodName + "_01 ***\" + \"/ \";");
            printLine("sqlString += \" FROM \" + DB." + tabName + ".GetNameAndAlias();");
            // any joins	 
            if (!String.IsNullOrEmpty(langCodes))
            {
                string dbPre = "DB." + tabName;
                printLine("");

                printLine("foreach (String langCode in mLanguageCodes)");
                printBegin();
                printLine("if (DB.isSecondaryLanguage(langCode))");
                printBegin();
                printLine("sqlString += \" LEFT JOIN \"  + " + dbPre + "Lang2.GetNameAndAlias(langCode);");
                printStartLine("sqlString += \" ON \" + ");
                indent = indent + 3;
                for (int n = 0; n < pkCols.Length; n++)
                {
                    print(dbPre + "." + sortedPkCols[n].modelName + "Col.Is(" + dbPre + "Lang2." + sortedPkCols[n].modelName + "Col, langCode)");

                    if (n < pkCols.Length - 1)
                    {
                        printEndLine(" +");
                        printStartLine(" \" AND \" + ");
                    }
                }
                indent = indent - 3;

                printEndLine(";");
                printEnd();
                printEnd();
                printLine("");


            }
            printLine("return sqlString;");
            printEnd();

        }


        


        private void allRows(TableType tt)
        {
            methodName = "Get" + tabName + "AllRows";
            printLine("//returns the all  \"rows\" found in database");
            printLine("public Dictionary<string, " + rowClassName + "> " + methodName + "()");
            printBegin();
            printLine("string sqlString = " + noWhereSql + ";");
            printLine("Dictionary<string, " + rowClassName + "> myOut = new Dictionary<string, " + rowClassName + ">();");

            printLine("");
            printLine("DataSet ds = mSqlCommand.ExecuteSelect(sqlString, null);");
            printLine("DataRowCollection myRows = ds.Tables[0].Rows;");
            printLine("");
            printLine("if (myRows.Count < 1)");
            printBegin();
            printLine("throw new PCAxis.Sql.Exceptions.DbException(44, \"" + tt.modelName + "\", \"" + tt.tableName + "\");");
            printEnd();
            printLine("");
            printLine("foreach (DataRow sqlRow in myRows)");
            printBegin();
            printLine(rowClassName + " outRow = new " + rowClassName + "(sqlRow, DB" + commaLangCodes + ");");
            printLine("myOut.Add(outRow." + tt.generate.getAllRows.key + ", outRow);");

            printEnd();
            printLine("return myOut;");
            printEnd();
            printLine("");

        }


       
        private void generateDict(TableTypeGenerateDictionary ttgd, TableType tt)
        {
            bool allArgsAreStrings = true; 
            methodName = "Get" + tabName + "Rows"  + ttgd.suff;
            string returnObject = "Dictionary<string, " + rowClassName + ">";
            if (ttgd.List)
            {
                returnObject = "Dictionary<string, List<" + rowClassName + ">>";
            }

            printStartLine("public " + returnObject + " " + methodName + "(");
            for (int n = 0; n < ttgd.arg.Length; n++)
            {
                TableTypeGenerateDictionaryArg arg = ttgd.arg[n];
                print(arg.argClass+" a" + arg.id);
                if (n < ttgd.arg.Length - 1)
                {
                    print(", ");
                }
            }
            printEndLine(", bool emptyRowSetIsOK)");
            printBegin();
            printLine(returnObject + " myOut = new "+returnObject+ "();");

            printLine("SqlDbConfig dbconf = DB;");
            printLine("string sqlString = " + noWhereSql + ";");
            printLine("//");
            printStartLine("// WHERE ");
            for (int n = 0; n < ttgd.arg.Length; n++)
            {
                TableTypeGenerateDictionaryArg arg = ttgd.arg[n];

                print(tt.alias + "." + arg.id + " = <\"" + arg.id + " as parameter reference for your db vendor\">");
                if (n < ttgd.arg.Length - 1)
                {
                    printEndLine("");
                    printStartLine("//    AND ");
                }
            }
            printEndLine("");
            printLine("//");

           // Is(mSqlCommand.GetParameterRef("aMainTable")); 

            printStartLine("sqlString += \" WHERE \" + ");
            indent += 3;

            for (int n = 0; n < ttgd.arg.Length; n++)
            {
                TableTypeGenerateDictionaryArg arg = ttgd.arg[n];

                if (arg.argClass.Equals("StringCollection"))
                {
                    if (ttgd.arg.Length != 1)
                    {
                        throw new Exception("oh, dear! StringCollection must be alone.");
                    }
                    allArgsAreStrings = false;
                    print("DB" + "." + tabName + "." + arg.id + "Col.In(mSqlCommand.GetParameterRef(\"a" + arg.id + "\"), a" + arg.id + ".Count)");
                  
                }
                else
                {
                    print("DB" + "." + tabName + "." + arg.id + "Col.Is(mSqlCommand.GetParameterRef(\"a" + arg.id + "\"))");
                    
                }

                if (n < ttgd.arg.Length - 1)
                {
                    printEndLine(" + ");
                    printStartLine(" \" AND \" +");
                }
                else
                {
                    if (ttgd.is_not_null != null)
                    {
                        printEndLine(" + ");
                        printStartLine(" \" AND \" +");
                        print("DB" + "." + tabName + "." + ttgd.is_not_null.columnModelName + "Col.IsNotNULL()");
                    }
                }

            }
            printEndLine(";");
            indent -= 3;
            printLine("");
            printLine("// creating the parameters");
            if (allArgsAreStrings)
            {
                printLine("System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[" + ttgd.arg.Length + "];");
                for (int n = 0; n < ttgd.arg.Length; n++)
                {
                    TableTypeGenerateDictionaryArg arg = ttgd.arg[n];
                    printLine("parameters[" + n + "] = mSqlCommand.GetStringParameter(\"a" + arg.id + "\", a" + arg.id + ");");
                }
            }
            else
            {
                TableTypeGenerateDictionaryArg arg = ttgd.arg[0];
                printLine("System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[a" + arg.id + ".Count];");
                printLine("for (int counter = 1; counter <= a" + arg.id + ".Count; counter++)");
                printLine("{");
                indent += 3;
                printLine("parameters[counter - 1] = mSqlCommand.GetStringParameter(\"a" + arg.id + "\" + counter, a" + arg.id + "[counter - 1]);");

               
                indent -= 3;
                printLine("}");
                printLine("");
            }

            printLine("");
            printLine("");
            printLine("DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);");
            printLine("DataRowCollection myRows = ds.Tables[0].Rows;");
            printLine("");

            printLine("if (myRows.Count < 1 && ! emptyRowSetIsOK)");
            printBegin();

            printStartLine("throw new PCAxis.Sql.Exceptions.DbException(35, ");
            if (allArgsAreStrings)
            {
                for (int n = 0; n < ttgd.arg.Length; n++)
                {
                    TableTypeGenerateDictionaryArg arg = ttgd.arg[n];
                    print(" \" " + arg.id + " = \" + a" + arg.id);
                    if (n < ttgd.arg.Length - 1)
                    {
                        print(" + ");
                    }
                }
            }
            else
            {
                print("\" query, see log. \"");
            }
            printEndLine(");");
            printEnd();
            printLine("");
            printLine("foreach (DataRow sqlRow in myRows)");
            if (ttgd.List)
            {
                printBegin();
                printLine(rowClassName + " outRow = new " + rowClassName + "(sqlRow, DB" + commaLangCodes + ");");
                printLine("if (!myOut.ContainsKey(outRow." + ttgd.key+"))");
                printBegin();
                    printLine("myOut[outRow." + ttgd.key+"] = new List<" + rowClassName + ">();");
                printEnd();
                printLine("myOut[outRow." + ttgd.key+"].Add(outRow);");

                
                printEnd();
            }
            else
            {
                printBegin();
                printLine(rowClassName + " outRow = new " + rowClassName + "(sqlRow, DB" + commaLangCodes + ");");
                printLine("myOut.Add(outRow." + ttgd.key + ", outRow);");
                printEnd();
            }

            printLine("return myOut;");
            printEnd();
        }



        private void allPk(TableType tt)
        {
            methodName = "Get" + tabName + "Row";
            printLine("//returns the single \"row\" found when all PKs are spesified");


            printStartLine("public " + rowClassName + " " + methodName + "(");


            foreach (ColumnType ct in pkCols)
            {
                if (!ct.readBySqlDbConfig)
                {
                    throw new NotSupportedException("Pk må være med");
                }
                print(comma + "string a" + ct.modelName);
                comma = ", ";
            }
            printEndLine(")");
            printBegin();
            printLine("//SqlDbConfig dbconf = DB;");
            printLine("string sqlString = " + noWhereSql + ";");

            printStartLine("sqlString += \" WHERE \" + ");
            comma = "";
            indent = indent + 4;

            for (int n = 0; n < sortedPkCols.Length; n++)
            {
                ColumnType ct = sortedPkCols[n];

                if (ct.upper)
                {
                    print("DB." + tabName + "." + ct.modelName + "Col.IsUppered(mSqlCommand.GetParameterRef(\"a" + ct.modelName + "\"))");
                    

                } else
                {
                    print("DB."+tabName+"."+ ct.modelName + "Col.Is(mSqlCommand.GetParameterRef(\"a" + ct.modelName + "\"))");
                    
                }

                if (n == pkCols.Length - 1)
                {
                    printEndLine(";");
                } else
                {
                    printEndLine(" + ");
                    printStartLine(" \" AND \" +");
                }



            }
            indent = indent - 4;


            printLine("");
            printLine("// creating the parameters");
            printLine("System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[" + sortedPkCols.Length + "];");
            for (int n = 0; n < sortedPkCols.Length; n++)
            {
                ColumnType ct = sortedPkCols[n];
                printLine("parameters[" + n + "] = mSqlCommand.GetStringParameter(\"a" + ct.modelName + "\", a" + ct.modelName + ");");

            }
            printLine("");
            printLine("DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);");
            printLine("DataRowCollection myRows = ds.Tables[0].Rows;");
            printLine("if (myRows.Count != 1)");
            printBegin();
            printStartLine("throw new PCAxis.Sql.Exceptions.DbException(36,");
            comma = "";
            foreach (ColumnType ct in pkCols)
            {
                print(comma + "\" " + ct.modelName + " = \" + a" + ct.modelName);
                comma = " + ";
            }
            printEndLine(");");
            //throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable + " Contents = " + aContents);
            printEnd();
            printLine("");


            //            ContentsRow myOut = new ContentsRow(myRows[0], DB, mLanguageCodes);

            printLine(rowClassName + " myOut = new " + rowClassName + "(myRows[0], DB" + commaLangCodes + "); ");

            printLine("return myOut;");
            
            printEnd();
            printLine("");


        }

        
    }
}
