using System.Xml.Serialization;
using System.Xml;
using System.Configuration;

using CommentsInModel;
using GrandMaster;
using System.Collections.Specialized;
using System;

namespace Merge_Comments_and_oldMaster
{
    class Merger
    {

        private CommentsInModel.CommentsInModel myComments;
        private GrandMaster.GrandMaster myOut;
        private GrandMaster.GrandMaster myMaster;
        private string root = ConfigurationManager.AppSettings["outroot"];
        private string versjon = "24";

        /// <summary>
        /// Brukes til å merge commentar fil i ny versjon med master fra gammel til ny master
        /// </summary>
        public Merger()
        {
            Console.WriteLine(String.Format("Starter med versjon {0} på root {1}",versjon,root));
            Console.WriteLine("Press Enter for å kjøre eller stopp VS.");
            Console.ReadLine();

            myOut = new GrandMaster.GrandMaster();
            string mergeDir = root + @"\PCAxis.Sql\codegenerator2\Merge_Comments_and_oldMaster\";

            string file = mergeDir + "old_master_"+versjon+".xml";
            string commentsFile = mergeDir + "CommentsInModel_" + versjon + ".xml";
            string utFil = mergeDir + "new_master_" + versjon + ".xml";

            System.IO.StreamWriter outFile = new System.IO.StreamWriter(utFil);

            XmlTextReader xmlReader = new XmlTextReader(file);
            XmlTextReader xmlReader3 = new XmlTextReader(commentsFile);

            XmlSerializer oldMasterSerial = new XmlSerializer(typeof(GrandMaster.GrandMaster));
            XmlSerializer commentsSerial = new XmlSerializer(typeof(CommentsInModel.CommentsInModel));

            XmlSerializer grandSerial = new XmlSerializer(typeof(GrandMaster.GrandMaster));

            myComments = (CommentsInModel.CommentsInModel)commentsSerial.Deserialize(xmlReader3);
            myMaster = (GrandMaster.GrandMaster)oldMasterSerial.Deserialize(xmlReader);

           
            StringCollection lang1Tabs = new StringCollection();

            //codes
            int tmpi=0;

            //myOut.Codes = myMaster.Codes;
            /*
            myOut.Codes = new GrandMasterCode[myMaster.Codes.Length];
            foreach (MasterCode mc in myMaster.Codes)
            {
                GrandMasterCode gg = new GrandMasterCode();
                gg.codeName = mc.codeName;
                gg.defaultCodeValue = mc.defaultCodeValue;

                myOut.Codes[tmpi] = gg;
                tmpi++;
            }
            //keywords
            tmpi = 0;
            myOut.Keywords = new GrandMasterKeyword[myMaster.Keywords.Length];
            foreach (MasterKeyword mc in myMaster.Keywords)
            {
                GrandMasterKeyword gg = new GrandMasterKeyword();
                gg.modelName = mc.modelName;
                if (mc.optionalSpecified)
                {
                    gg.optionalSpecified = mc.optionalSpecified;
                    gg.optional = mc.optional;
                } 
                if(!String.IsNullOrEmpty(mc.defaultLocalName))
                {
                    gg.defaultLocalName = mc.defaultLocalName;
                }
                myOut.Keywords[tmpi] = gg;
                tmpi++;
            }
            */

            //comments fila bestemmer hvilke tabeller som skal med
            myOut.Tables = new GrandMasterTables();
            myOut.Tables.Table = new TableType[myComments.Tables.Table.Length];
            tmpi = 0;
            
            foreach (Table commTable in myComments.Tables.Table) 
            {
                TableType tt = new TableType();
                
                tt.modelName = commTable.name;
               
                tt.Description = commTable.Description;
                
                TableType oldMasterTable = getOldMaster(tt.modelName);
                bool hasOldMaster = (!(oldMasterTable == null));
                
                tt.Columns = this.getColumns(commTable.Columns, oldMasterTable);
               
                if (hasOldMaster)
                {
                    tt.alias = oldMasterTable.alias;
                    tt.Comment = oldMasterTable.Comment;
                    
                    tt.defaultAlias_lang2 = oldMasterTable.defaultAlias_lang2;

                    #region tt.generate
                    tt.generate = oldMasterTable.generate;

                    #endregion tt.generate

                    //tt.langType fixes this later, together with the new ones
                    //tt.modelName already in place;
                    tt.readBySqlDbConfig = oldMasterTable.readBySqlDbConfig;
                    //tt.tableName fixes this later, together with the new ones
                   
                }

                myOut.Tables.Table[tmpi] = tt;
                tmpi++;
            }

            ///setter tabellenes langType og tableName
            ///
            foreach (TableType tt in myOut.Tables.Table)
            {
                tt.tableName = tt.modelName.ToUpper();

                bool minstEnKolHasSL = false;
                foreach(ColumnType ct in tt.Columns){
                    if (ct.hasSL)
                    {
                        minstEnKolHasSL = true;
                    }
                }
                tt.hasSL = minstEnKolHasSL;
            }

            //outFile.WriteLine("11111sdfsdfsfds");

            grandSerial.Serialize(outFile, myOut);
            //outFile.WriteLine("22222sdfsdfsfds");
            //outFile.Flush();
            Console.Out.WriteLine("Wrote to file: " + utFil);
            Console.Out.WriteLine("OOOOOOOOOOOOOOOBS   KODER og KEYWORDS må legges inn for hånd!!!");
        }

        

        /// <summary>
        /// finner den gamle tabellen eller null
        /// </summary>
        /// <param name="aModelName">finn denne</param>
        /// <returns>tabellen eller null</returns>
        private TableType getOldMaster(string aModelName)
        {
            
            foreach (TableType tab in myMaster.Tables.Table)
            {
                if (tab.modelName.Equals(aModelName))
                {
                    return tab;
                } 
            }
            return null;
            
        }

        private ColumnType[] getColumns(TableColumns tableColumns, TableType oldMaster)
        {
            int outColIndex = 0;
            

            ColumnType[] retVal = new ColumnType[tableColumns.Column.Length];
            foreach (Column commentC in tableColumns.Column)
            {
                ColumnType outCol = new ColumnType();
                outCol.modelName = commentC.colname;
                outCol.datatype = commentC.datatype;
                outCol.Description = commentC.Description;
                outCol.foreignkey = commentC.foreignkey;
                outCol.length = commentC.length;
                outCol.mandatory = commentC.mandatory;
                outCol.pkSpecified = commentC.primarykey.Equals("True");
                outCol.pk = 0;  //kommer ikke med i serializering hvis ikke outCol.pkSpecified

                ColumnType oldC = getColumns(oldMaster, outCol.modelName);
                if (oldC != null)
                {
                    outCol.DateTimeRoundTrip = oldC.DateTimeRoundTrip;
                    outCol.DateTimeRoundTripSpecified = oldC.DateTimeRoundTripSpecified;
                    outCol.deviatingDefaultColumnName = oldC.deviatingDefaultColumnName;
                    if (oldC.hasSL)
                    {
                        outCol.hasSL = oldC.hasSL;
                    }
                    outCol.pkSpecified = oldC.pkSpecified;
                    outCol.pk = oldC.pk;
                    if (!oldC.readBySqlDbConfig)
                    {
                        outCol.readBySqlDbConfig = oldC.readBySqlDbConfig;
                    }
                    outCol.upper = oldC.upper;
                    outCol.upperSpecified = oldC.upperSpecified;
                    

                }
                retVal[outColIndex] = outCol;
                outColIndex++;
            }
            return retVal;
        }

        private ColumnType getColumns(TableType oldMaster, string aModelName)
        {
            if (oldMaster == null)
            {
                return null;
            }
            ColumnType[] oldMasterCs = oldMaster.Columns; 
            foreach (ColumnType col in oldMasterCs)
            {
                if (col.modelName.Equals(aModelName))
                {
                    return col;
                }
            }
            return null;
        }

    }
}
