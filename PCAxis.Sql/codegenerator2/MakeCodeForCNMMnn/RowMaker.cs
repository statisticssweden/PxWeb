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
    class RowMaker:FileMaker
    {


        private GrandMaster.GrandMaster myMaster;
        string outDirRows;


        internal RowMaker(GrandMaster.GrandMaster myMaster, string outDirRows)
        {
            this.myMaster = myMaster;
                this.outDirRows = outDirRows;
        }


        internal void genRowsClasses()
        {


            foreach (TableType tt in myMaster.GetReadTables())
            {
                

                if (!(tt.generate.dontMakeRowClasses != null || tt.generate.onlyForDbconfig != null))
                {
                    setHelpers(tt);

                    string utFil = this.outDirRows + rowClassName + ".cs";
                    outStreamWriter = new StreamWriter(utFil);

                    String[] myArr = new String[] { "System","System.Data","System.Collections.Generic",
                "System.Collections.Specialized","System.Text","System.Xml.XPath","System.Globalization","",
                "PCAxis.Sql.DbConfig",""};
                    String cnmmVersion = myMaster.Tables.CNMMversion;

                    printUsing(myArr);

                    printThisCodeIsGeneratedWarning();
                    printLine("namespace PCAxis.Sql.QueryLib_" + cnmmVersion);
                    printBegin();




                    if (!String.IsNullOrEmpty(tt.Comment))
                    {
                        printLine("/*" + tt.Comment + "*/");
                    }



                    if (tt.hasSL)
                    {
                        genRowLangIndep(tt, cnmmVersion);
                        genRowLangDep(tt, cnmmVersion);
                    }
                    else
                    {
                        genRowLangNone(tt, cnmmVersion);
                    }


                    printEnd();
                    outStreamWriter.Close();
                    Console.Out.WriteLine("Wrote to file: " + utFil);
                }
            }
        }



        private void genRowLangIndep(TableType tt, String cnmmVersion)
        {
            printLine("");
            printSummary("Holds the attributes for " + tabName + ". The language dependent parts are stored in the texts dictionary which is indexed by language code.\n"+ tt.Description);
            printLine("public class " + rowClassName + "");
            printBegin();
            foreach (ColumnType ct in languageInDependentCols)
            {
                fieldNproperty(ct, tt.modelName);
            }
            printLine("");
            printLine("public Dictionary<string, " + textClassName + "> texts = new Dictionary<string, " + textClassName + ">();");
            printLine("");
            //<!--contrucktor -->
            printLine("public " + rowClassName + "(DataRow myRow, SqlDbConfig_" + cnmmVersion + " dbconf, StringCollection languageCodes)");
            printBegin();
            foreach (ColumnType ct in languageInDependentCols)
            {
                constructorLine(ct);
            }
            printLine("");
            printLine("foreach (string languageCode in languageCodes)");
            printBegin();
            printLine("texts.Add(languageCode, new " + textClassName + "(myRow, dbconf, languageCode));");
            printEnd();
            printLine("");
            printEnd();
            printEnd();
        }

        private void genRowLangDep(TableType tt, String cnmmVersion)
        {

            printLine("");
            printSummary("Holds the language dependent attributes for " + tabName + "  for one language.\n"+ tt.Description);
            printLine("public class " + textClassName);
            printBegin();
            foreach (ColumnType ct in languageDependentCols)
            {
                fieldNproperty(ct, tt.modelName);
            }
            printLine("");

            printLine("");

            //    <!--conctrutor -->
            printLine("internal " + textClassName + "(DataRow myRow, SqlDbConfig_" + cnmmVersion + " dbconf, String languageCode)");
            printBegin();
            printLine("if (dbconf.isSecondaryLanguage(languageCode))");
            printBegin();
            foreach (ColumnType ct in languageDependentCols)
            {
                printLine(String.Format("this.m{0} = myRow[dbconf.{1}Lang2.{0}Col.Label(languageCode)].ToString();", ct.modelName, tabName));
            }
            printEnd();

            printLine("else");
            printBegin();
            foreach (ColumnType ct in languageDependentCols)
            {
                constructorLine(ct);
            }

            printEnd();
            printEnd();
            printEnd();
            printLine("");
        }



        private void genRowLangNone(TableType tt, String cnmmVersion)
        {

            printLine("");
            printSummary("Holds the attributes for " + tabName + ". (This entity is language independent.) \n"+ tt.Comment +"\n"+ tt.Description);
            printLine("public class " + rowClassName);
            printBegin();
            foreach (ColumnType ct in tt.GetReadColumns())
            {
                fieldNproperty(ct, tt.modelName);
            }
            printLine("");

            //    <!--conctrutor -->
            printLine("public " + rowClassName + "(DataRow myRow, SqlDbConfig_" + cnmmVersion + " dbconf)");
            printBegin();
            foreach (ColumnType ct in tt.GetReadColumns())
            {
                constructorLine(ct);
            }
            printEnd();
            printEnd();

        }




        private void constructorLine(ColumnType ct)
        {
           
            if (ct.DateTimeRoundTripSpecified)
            {
                printLine("this.m" + ct.modelName + " = myRow[dbconf." + tabName + "." + ct.modelName + "Col.Label()] == DBNull.Value ? \"\" : Convert.ToDateTime(myRow[dbconf.Contents.LastUpdatedCol.Label()]).ToString(\"yyyyMMdd HH:mm\");");

            }
            else
            {

                printLine("this.m" + ct.modelName + " = myRow[dbconf." + tabName + "." + ct.modelName + "Col.Label()].ToString();");
            }
        }

        private void fieldNproperty(ColumnType ct, string tableName)
        {
            
            printLine("private String m" + ct.modelName + ";");

            printSummary(ct.Description);
            printLine("public String " + ct.modelName);
            printBegin();
            printLine("get { return m" + ct.modelName + "; }");
            printEnd();
        }

    }
}
