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
    class ConfigStubMaker : FileMaker
    {


        private GrandMaster.GrandMaster myMaster;
        //private CommentsInModel myComments;

        private string outFileName;


        internal ConfigStubMaker(string configStubOutFile, GrandMaster.GrandMaster myMaster)
        {
            this.myMaster = myMaster;
           
            this.outFileName = configStubOutFile;
        }



        internal void genConfigStub()
        {
            outStreamWriter = new StreamWriter(outFileName);
            printLine("<Codes>");
            foreach (GrandMasterCode kode in myMaster.Codes)
            {
                printLine("<!--Code codeName=\"" + kode.codeName + "\" codeValue=\"" + kode.defaultCodeValue + "\"/-->");
            }
            printLine("</Codes>");


            printLine("<Keywords>");
            foreach (GrandMasterKeyword keyword in myMaster.Keywords)
            {
                if (keyword.optionalSpecified && keyword.optional)
                {

                    printLine("<!-- " + keyword.modelName + " represent an optional property, which has a default property value ( the others have a default property name ). -->");
                    printLine("<!--Keyword modelName=\"" + keyword.modelName + "\" keywordName=\"" + keyword.modelName.ToUpper() + "\"/-->");
                }
                else
                {

                    printLine("<!--Keyword modelName=\"" + keyword.modelName + "\" keywordName=\"" + keyword.defaultLocalName + "\"/-->");
                }
            }
            printLine("</Keywords>");

            printLine("<Tables>");
            printLine("<!-- To uncomment a column you need to uncomment the table as well -->");
            foreach (TableType tt in myMaster.GetReadTables())
            {



                printLine("<!--Table modelName=\"" + tt.modelName + "\" tableName=\"" + tt.tableName + "\" alias=\"" + tt.alias + "\">");
                printLine("<Columns-->");
                foreach (ColumnType ct in tt.GetReadColumns())
                {
                    printLine("<!--Column modelName=\"" + ct.modelName + "\" columnName=\"" + ct.defaultNameInDatabase() + "\"/-->");
                }
                printLine("<!--/Columns>");
                printLine("</Table-->");

                if (tt.hasSL)
                {
                    //omigjen for lang2
                    printLine("<!--Table modelName=\"" + tt.modelName + "Lang2\" tableName=\"" + tt.tableName + "_\" alias=\"" + tt.defaultAlias_lang2 + "\">");
                    printLine("<Columns-->");
                    foreach (ColumnType ct in tt.Columns)
                    {
                        if (!ct.readBySqlDbConfig)
                        {
                            continue;
                        }
                        if (ct.hasSL || ct.pkSpecified)
                        {
                            printLine("<!--Column modelName=\"" + ct.modelName + "\" columnName=\"" + ct.defaultNameInDatabase() + "\"/-->");
                        }
                    }
                    printLine("<!--/Columns>");
                    printLine("</Table-->");


                }


            }
            printLine("</Tables>");
            outStreamWriter.Close();
            Console.Out.WriteLine("Wrote to file " + outFileName);
        }
    }
}
