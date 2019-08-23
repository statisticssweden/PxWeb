using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GrandMaster;


namespace generateClassesForQueryLib
{

    class CreateOracleDBScriptMaker : FileMaker
    {

        
        private GrandMaster.GrandMaster myMaster;

        private string outFileName;


        internal CreateOracleDBScriptMaker(string configStubOutFile, GrandMaster.GrandMaster myMaster)
        {
            this.myMaster = myMaster;
            this.outFileName = configStubOutFile;
        }


        /// <summary>
        /// Generates the code to create the database and writes it to file. 
        /// </summary>
        internal void genCreateDatabaseSql()
        {
            outStreamWriter = new StreamWriter(outFileName);
            printLine("--generert create db script for oracle");
            printLine("");
            

            foreach (TableType tt in myMaster.Tables.Table)
            {
                printLine("CREATE TABLE " + tt.modelName + "(");
                this.createColumns(tt);
                this.createPK(tt);
                printLine(");");


                printLine("");
                this.createColumnsComments(tt);
                printLine("");

                printLine("COMMENT ON TABLE " + tt.modelName + " IS 'CNMM:" + tt.Description.Replace("'", "") + "';");

                printLine("");

                if (!tt.modelName.Equals("MetabaseInfo"))
                {
                    createTrigger(tt.modelName);
                }


                 if (tt.hasSL)
                {
                     string language_suffix= "_ENG";

                    printLine("");
                    printLine("-------");
                    printLine("");

                    string tableName = tt.modelName + language_suffix;
                    printLine("CREATE TABLE " + tableName + "(");
                 
                    this.createSLColumns(tt,language_suffix);

                    this.createSLPK(tt,language_suffix);
                    printLine(");");


                    printLine("");
                    this.createSLColumnsComments(tt,language_suffix);
                    printLine("");

                    printLine("COMMENT ON TABLE " + tableName + " IS 'CNMM: SL: " + tt.Description.Replace("'", "") + "';");

                    printLine("");

                    createSLTrigger(tt.modelName, language_suffix);

                }

               
                printLine("");
                printLine("------------------------------------------------------------");
                printLine("");
            }



            printLine("");
            printLine("");
            printLine("");
            printLine("---------------------------------------");
            printLine("-- GRANTS         O_PCAX_ROLE");


            string user = "O_PCAX_ROLE";
            foreach (TableType tt in myMaster.Tables.Table)
            {
                printLine("GRANT SELECT ON " + tt.modelName.ToUpper() + " TO "+user+";");
                if (tt.hasSL)
                {
                    printLine("GRANT SELECT ON " + tt.modelName.ToUpper() + "_ENG TO " + user + ";");
                }
            }
            printLine("");
            printLine("");
            printLine("---------------------------------------");
            printLine("-- GRANTS         PCAX_ROLE");
            user = "PCAX_ROLE";
            foreach (TableType tt in myMaster.Tables.Table)
            {
                printLine("GRANT SELECT ON " + tt.modelName.ToUpper() + " TO " + user + ";");
                if (tt.hasSL)
                {
                    printLine("GRANT SELECT ON " + tt.modelName.ToUpper() + "_ENG TO " + user + ";");
                }
            }
            printLine("");

            outStreamWriter.Close();
            Console.Out.WriteLine("Wrote to file: " + outFileName);


        }

        private void createTrigger(string tableName)
        {
            string triggername = tableName + "_BUPSE";
            printLine("CREATE TRIGGER " + triggername + " BEFORE INSERT OR UPDATE ON " + tableName);
            printLine("FOR EACH ROW");
            printLine("BEGIN");
            printLine(" IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN");
            printLine("   :NEW.USERID := USER;");
            printLine("   :NEW.LOGDATE := systimestamp;");
            printLine("  END IF;");
            printLine("END " + tableName + ";");
            printLine("");
        }


        private void createSLTrigger(string tableName, string language_suffix)
        {
            string triggername = tableName +language_suffix+ "_BUPSE";
            printLine("CREATE TRIGGER " + triggername + " BEFORE INSERT OR UPDATE ON " + tableName + language_suffix);
            printLine("FOR EACH ROW");
            printLine("BEGIN");
            printLine(" IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN");
            printLine("   :NEW.USERID := USER;");
            printLine("   :NEW.LOGDATE := systimestamp;");
            printLine("  END IF;");
            printLine("END " + triggername + ";");
            printLine("");
        }

        private void createColumnsComments(TableType tt)
        {
            foreach (ColumnType ct in tt.Columns)
            {
                printLine("COMMENT ON COLUMN " + tt.modelName + "." + ct.modelName + " IS 'CNMM:" + ct.Description.Replace("'", "") + "';");
            }
        }

        private void createSLColumnsComments(TableType tt, string language_suffix)
        {
            foreach (ColumnType ct in tt.GetAllLanguageRowColumns())
            {
                printLine("COMMENT ON COLUMN " + tt.modelName + language_suffix+"." + ct.modelName + " IS 'CNMM: SL version:" + ct.Description.Replace("'","") + "';");
            }
        }


        private void createColumns(TableType tt)
        {
            string glue ="";
            foreach (ColumnType ct in tt.Columns)
            {
                string outLine = " " + glue + ct.modelName + " "+ getDataType(ct);

                if(ct.mandatory.ToLower().Equals("true")){
                    outLine += " NOT NULL ENABLE";
                }

                printLine(outLine);
                glue = ",";
            }           
        }

        private void createSLColumns(TableType tt, string language_suffix)
        {
            string glue = "";
            foreach (ColumnType ct in tt.GetAllLanguageRowColumns())
            {
                string outLine = " " + glue + ct.modelName + " " + getDataType(ct);

                if (ct.mandatory.ToLower().Equals("true"))
                {
                    outLine += " NOT NULL ENABLE";
                }

                printLine(outLine);
                glue = ",";
            }
        }


        private void createPK(TableType tt)
        {
            string glue = "";
            string outLine = ", CONSTRAINT PK_" + tt.modelName + " PRIMARY KEY (";
            foreach (ColumnType ct in tt.Columns)
            {
                if (ct.pkSpecified)
                {
                    outLine += glue + ct.modelName;
                    glue = ",";
                }
            }
            outLine += " ) ";
            printLine(outLine);
        }

        private void createSLPK(TableType tt, string language_suffix)
        {
            string glue ="";
            string outLine = ", CONSTRAINT PK_"+tt.modelName+language_suffix+" PRIMARY KEY (";
            foreach (ColumnType ct in tt.Columns)
            {
                if (ct.pkSpecified)
                {
                    outLine += glue + ct.modelName;
                    glue = ",";
                }
            }
            outLine += " ) ";
            printLine(outLine);
        }

        private string getDataType(ColumnType ct)
        {
            string myOut = "";
            if (ct.modelName.Equals("LogDate"))
            {
                myOut = "timestamp(9)";
            }
            else if (ct.datatype.Equals("varchar") || ct.datatype.Equals("char"))
            {
                myOut = "VARCHAR2("+ct.length+" CHAR)";
            }
            else if (ct.datatype.Equals("text"))
            {
                myOut = "VARCHAR2(3000 CHAR)";
            }
            else if (ct.datatype.Equals("smallint") || ct.datatype.Equals("int"))
            {
                myOut = "NUMBER";
            }
            else if (ct.datatype.Equals("numeric"))
            {
                myOut = "NUMBER";
            }
            else if (ct.datatype.Equals("smalldatetime"))
            {
                myOut = "DATE";
            }
            else
            {
                throw new System.NotImplementedException("For " + ct.modelName);
            }
            return myOut;

        }
    }
}
