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
    class SqlDbConfig_nnMaker:FileMaker
    {


        private GrandMaster.GrandMaster myMaster;
        private string outFileSqlDbConfig;


        internal SqlDbConfig_nnMaker(GrandMaster.GrandMaster myMaster, string outFileSqlDbConfig)
        {
            this.myMaster = myMaster;
            this.outFileSqlDbConfig = outFileSqlDbConfig;
        }



        internal void genSqlDbCongigSubclass()
        {
            String[] myArr = new String[] { "System", "System.Globalization",
                "System.Xml","System.Xml.XPath","log4net"};
            String className = "SqlDbConfig_" + myMaster.Tables.CNMMversion;
            String fileName =  outFileSqlDbConfig;

            outStreamWriter = new StreamWriter(fileName);
            printUsing(myArr);
            printLine("");

            printThisCodeIsGeneratedWarning();

            printLine("namespace PCAxis.Sql.DbConfig");
            printBegin();
            printLine("public class " + className + " : SqlDbConfig");
            printLine("{");

            indent++;
            printLine("private static readonly ILog log = LogManager.GetLogger(typeof(" + className + "));");

            printLine("");
            printLine("public Ccodes Codes;");

            printLine("public DbKeywords Keywords;");

            printLine("");
            printLine("#region Fields");

            foreach (TableType tt in myMaster.GetReadTables())
            {

                setHelpers(tt);
                genDbCongigPartialForTableFields(tt);

            }
            printLine("#endregion Fields");
            printLine("");
            printLine("private void initStructs()");
            printLine("{");
            printLine("");
            indent++;
            foreach (TableType tt in myMaster.GetReadTables())
            {

                setHelpers(tt);

                genDbCongigPartialForTableInitClasses(tt);

            }
            printEnd();
            printLine("");
            printLine("public " + className + "(XmlReader xmlReader, XPathNavigator nav)");
            printLine(": base(xmlReader, nav)");
            printBegin();
            printLine("log.Info(\"" + className + " called\");");
            printLine("");
            printLine("this.initStructs();");
            printLine("this.initCodesAndKeywords();");
            printEnd();
            printLine("");






            printLine("#region  structs");
            printLine("");
            printLine(" ");
            printLine("");

            foreach (TableType tt in myMaster.GetReadTables())
            {

                setHelpers(tt);


                genDbCongigPartialForTableClasses(tt, className);

                if (tt.hasSL)
                {
                    genDbCongigPartialForTableClassesLang2(tt, className);
                }
            }
            printLine("#endregion  structs");




            printLine("");
            printLine("");
            printLine("private void initCodesAndKeywords()");
            printBegin();
            #region Codes
            printLine("");
            printLine("#region Codes");
            printLine("");
            printLine("Codes = new Ccodes();");
            printLine("");
            foreach (GrandMasterCode kode in myMaster.Codes)
            {
                printLine("Codes." + kode.codeName + " = ExtractCode(\"" + kode.codeName + "\", \"" + kode.defaultCodeValue + "\");");
            }
            printLine("");
            printLine("#endregion Codes");
            printLine("");
            #endregion Codes

            #region Keywords
            printLine("");
            printLine("#region Keywords");
            printLine("");
            printLine("Keywords = new DbKeywords();");
            printLine("");
            foreach (GrandMasterKeyword keyword in myMaster.Keywords)
            {
                if (keyword.optionalSpecified && keyword.optional)
                {
                    printLine("if (FileHasKeyword(\"" + keyword.modelName + "\"))");
                    printBegin();
                    printLine("Keywords.Optional_" + keyword.modelName + " = ExtractKeyword(\"" + keyword.modelName + "\");");
                    printEnd();
                }
                else
                {
                    printLine("Keywords." + keyword.modelName + " = ExtractKeyword(\"" + keyword.modelName + "\", \"" + keyword.defaultLocalName + "\");");
                }
            }
            printLine("");
            printLine("#endregion Keywords");
            printLine("");
            #endregion Keywords
            printEnd();
            printLine("");


            printLine("public struct Ccodes");
            printBegin();
            foreach (GrandMasterCode kode in myMaster.Codes)
            {
                printLine("public String " + kode.codeName + ";");
            }
            printEnd();
            printLine("");
            printLine("public struct DbKeywords");
            printBegin();
            foreach (GrandMasterKeyword keyword in myMaster.Keywords)
            {
                if (keyword.optionalSpecified && keyword.optional)
                {

                    printLine("public String Optional_" + keyword.modelName + ";");

                }
                else
                {
                    printLine("public String " + keyword.modelName + ";");
                }
            }
            printEnd();
            printEnd();
            printEnd();

            outStreamWriter.Close();
            Console.Out.WriteLine("Wrote to file " + fileName);
        }





        private void genDbCongigPartialForTableClasses(TableType tt, String className)
        {


            printSummary(tt.Description);

            printLine("public class Tbl" + tabName + " : Tab");
            printBegin();

            printLine("");
            foreach (ColumnType ct in tt.GetReadColumns())
            {
              
                printSummary(ct.Description);
                printLine("public Column4Parameterized " + ct.modelName + "Col;");
            }
            printLine("");
            printLine("internal Tbl" + tabName + "(" + className + " config)");
            printLine(": base(config.ExtractAliasName(\"" + tabName + "\",\"" + tt.alias + "\"), config.ExtractTableName(\"" + tabName + "\",\"" + tt.tableName + "\"), config.MetaOwner)");
            printBegin();
            printLine("string tmpColumnName =\"\";");
            foreach (ColumnType ct in tt.GetReadColumns())
            {

                printLine("tmpColumnName = config.ExtractColumnName(\"" + tabName + "\", \"" + ct.modelName + "\", \"" + ct.defaultNameInDatabase() + "\");");
                printLine("this." + ct.modelName + "Col = new Column4Parameterized(tmpColumnName, this.Alias,\"" + ct.modelName + "\",config.GetDataProvider());");
                
            }
            printLine("");
            printEnd();
            printLine("");
            printEnd();
            printLine("");
        }

        private void genDbCongigPartialForTableClassesLang2(TableType tt, String className)
        {
            //JFI ta med denne: printSummary(myComments.getTabellBeskrivelse(tt.modelName.Replace("Lang2","_Eng")));
            printLine("public class Tbl" + tabNameSL + " : Lang2Tab");
            printBegin();


            printLine("");
            foreach (ColumnType ct in tt.GetLanguageRowColumns())
            {
//                printSummary(myComments.getKolonneBeskrivelse(tt.modelName.Replace("Lang2", "_Eng"), ct.modelName));
                printSummary(ct.Description);
                printLine("public Lang2Column4Parameterized " + ct.modelName + "Col;");
                printLine("");
            }



            printLine("internal Tbl" + tabNameSL + "(" + className + " config)");
            printLine(": base(config.ExtractAliasName(\"" + tabNameSL + "\",\"" + tt.defaultAlias_lang2 + "\"), config.ExtractTableName(\"" + tabNameSL + "\",\"" + tt.tableName + "_\"), config.MetaOwner, config.MetaSuffixByLanguage)");


            printBegin();
            printLine("string tmpColumnName =\"\";");
            foreach (ColumnType ct in tt.GetLanguageRowColumns())
            {
                printLine("tmpColumnName = config.ExtractColumnName(\"" + tabNameSL + "\", \"" + ct.modelName + "\"  , \"" + ct.defaultNameInDatabase() + "\");");
                printLine("this." + ct.modelName + "Col = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);");
            }
            printEnd();
            printLine("");

            printEnd();
            printLine("");
            printLine("");
        }



        private void genDbCongigPartialForTableInitClasses(TableType tt)
        {
            printLine("");
            printLine(tabName + " = new Tbl" + tabName + "(this);");

            if (tt.hasSL)
            {
                printLine("");
                printLine(tabNameSL + " = new Tbl" + tabNameSL + "(this);");
            }

        }


        private void genDbCongigPartialForTableFields(TableType tt)
        {
            //printLine("");
            //tja her var summary kommentert ut printSummary(myComments.getTabellBeskrivelse(tt.modelName));
            printLine("public Tbl" + tabName + " " + tabName + ";");
            if (tt.hasSL)
            {
                printLine("public Tbl" + tabNameSL + " " + tabNameSL + ";");
            }
        }

 




    }
}
