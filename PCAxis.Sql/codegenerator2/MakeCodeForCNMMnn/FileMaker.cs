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
    class FileMaker
    {
        protected StreamWriter outStreamWriter;
        protected int indent = 0;
        private string spaces = "                                                                                       ";


        #region helpers
        protected string comma = "";
        protected string tabName = "";
        protected string tabNameSL = "";

        protected string methodName = "";
        protected string langCodes = "";
        protected string commaLangCodes = "";

        protected string noWhereSql = "";
        protected string noWhereSqlDef = "";

        protected string dbPre = "";
        protected string idPre = "";
        protected string namePre = "";
        protected string rowClassName = "";

        protected string textClassName = "";

        protected ColumnType[] pkCols;

        protected ColumnType[] colsInUse;
        protected ColumnType[] languageDependentCols;
        protected ColumnType[] languageInDependentCols;
        protected ColumnType[] sortedPkCols;
        #endregion helpers


        protected FileMaker()
        {

        }


        protected void setHelpers(TableType tt)
        {
            comma = "";
            tabName = tt.modelName;
            tabNameSL = tabName + "Lang2";
            langCodes = "";
            commaLangCodes = "";
            if (tt.hasSL)
            {
                langCodes = " mLanguageCodes";
                commaLangCodes = ", mLanguageCodes";
            }
            noWhereSql = "Get" + tabName + "_SQLString_NoWhere()";
            noWhereSqlDef = "Get" + tabName + "_SQLString_NoWhere()";

            dbPre = "DB." + tt.modelName;
            idPre = dbPre + ".Alias + \".\" + " + dbPre + ".";
            namePre = dbPre + ".Alias + \"_\" + " + dbPre + ".";
            rowClassName = tabName + "Row";
            textClassName = tabName + "Texts";

            pkCols = getPKColumns(tt);
            sortedPkCols = getPKColumnsSortedByPk(tt);

            List<ColumnType> tmpUsedCols = new List<ColumnType>();
            List<ColumnType> tmpLangDepCols = new List<ColumnType>();
            List<ColumnType> tmpLangInDepCols = new List<ColumnType>();
            foreach (ColumnType ct in tt.GetReadColumns())
            {
               

                tmpUsedCols.Add(ct);

                if (ct.hasSL)
                {
                    tmpLangDepCols.Add(ct);
                }
                else
                {
                    tmpLangInDepCols.Add(ct);
                }

            }

            colsInUse = tmpUsedCols.ToArray();

            languageInDependentCols = tmpLangInDepCols.ToArray();
            languageDependentCols = tmpLangDepCols.ToArray();


        }

        /// <summary>
        /// printLine("{"); and indent++;
        /// </summary>
        protected void printBegin()
        {
            printLine("{");
            indent++;
        }

        /// <summary>
        /// indent--; and printLine("}");
        /// </summary>
        protected void printEnd()
        {
            indent--;
            printLine("}");
        }


        protected void printSummary(string summary)
        {
            
            printLine("/// <summary>");
            foreach (string summaryLine in summary.Replace("\r\n", "\n").Split('\n'))
            {
                printLine("/// " + summaryLine.Trim());
            }
            printLine("/// </summary>");
        }
        
       

        protected void printLine(string printString)
        {
            if (printString.Equals(""))
            {
                outStreamWriter.WriteLine("");
            }
            else
            {
                outStreamWriter.WriteLine(spaces.Substring(0, 4 * indent) + printString);
            }
        }

        protected void printStartLine(string printString)
        {
            outStreamWriter.Write(spaces.Substring(0, 4 * indent) + printString);
        }

        protected void print(string printString)
        {
            outStreamWriter.Write(printString);
        }

        protected void printEndLine(string printString)
        {
            outStreamWriter.Write(printString + outStreamWriter.NewLine);
        }

        protected void printUsing(String[] namespaces)
        {
            foreach (string str in namespaces)
            {
                if (str.Equals(""))
                {
                    printLine("");
                }
                else
                {
                    printLine("using " + str + ";");
                }
            }
        }

        protected void printThisCodeIsGeneratedWarning()
        {
            printLine("//This code is generated. ");
            printLine("");
        }



        private ColumnType[] getPKColumns(TableType tt)
        {
            List<ColumnType> pkCols = new List<ColumnType>();
            Dictionary<int, ColumnType> tmpDict = new Dictionary<int, ColumnType>();
            foreach (ColumnType ct in tt.GetReadColumns())
            {
                if (ct.pkSpecified)
                {

                    pkCols.Add(ct);


                }
            }


            return pkCols.ToArray();
        }
        private ColumnType[] getPKColumnsSortedByPk(TableType tt)
        {
            List<ColumnType> pkCols = new List<ColumnType>();
            Dictionary<int, ColumnType> tmpDict = new Dictionary<int, ColumnType>();
            foreach (ColumnType ct in tt.GetReadColumns())
            {
                if (ct.pkSpecified)
                {
                    if (pkCols.Count == 0)
                    {
                        pkCols.Add(ct);
                    }
                    else
                    {
                        //want to place pk=1 before pk=2  
                        if (ct.pk < pkCols[pkCols.Count - 1].pk)
                        {
                            pkCols.Insert(ct.pk - 1, ct);
                        }
                        else
                        {
                            pkCols.Add(ct);
                        }
                    }

                }
            }


            return pkCols.ToArray();
        }



    }
}
