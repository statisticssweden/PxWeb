using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_21;
using PCAxis.Paxiom;
using System.Collections.Specialized;
using log4net;

namespace PCAxis.Sql.Parser_21
{
    public class PXSqlMaintable
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlMaintable));

        private PXSqlMeta_21 meta;
        private MainTableRow mtRow;
        private DataStorageRow mDataStorageRow;

        private string mSubjectCode;
        private Dictionary<string, string> mSubjectAreaByLanguage = new Dictionary<string, string>();

        public PXSqlMaintable() { }
        public PXSqlMaintable(MainTableRow mtRow, DataStorageRow dataStorageRow, MenuSelectionRow menuSelectionRow, PXSqlMeta_21 meta)
        {
            this.meta = meta;
            this.mtRow = mtRow;
            this.mDataStorageRow = dataStorageRow;

            this.mSubjectCode = menuSelectionRow.Selection;

            foreach (string langCode in menuSelectionRow.texts.Keys)
            {
                mSubjectAreaByLanguage[langCode] = menuSelectionRow.texts[langCode].PresText;
            }

        }






        //public PXSqlMaintable(MainTableRow mtRow, List<FootnoteRow> footNoteRows)
        //{
        //    this.mtRow = mtRow;
        //    mFootNoteRows = footNoteRows;
        //    // this.FootNotesMainTable = new List<RelevantFootNotesRow>();
        //}

        public string MainTable
        {
            get { return mtRow.MainTable; }
        }
        public string SpecCharExists
        {
            get { return mtRow.SpecCharExists; }
        }
        public bool ContainsOnlyMetaData
        {
            get {
                if (TableStatus.Equals(meta.Config.Codes.StatusMeta) || TableStatus.Equals(meta.Config.Codes.StatusEmpty))
                {
                    return true; 
                }
                else
                {
                    return false;
                }
            }
        } 

        //mMainTable[0][mConfig.MainTable.ContentsVariable].ToString().Length != 0
        internal bool hasContentsVariable(string someLangCode)
        {
            return (mtRow.texts[someLangCode].ContentsVariable.Length != 0);
        }

        public string getPresText(string langCode)
        {
            return mtRow.texts[langCode].PresText;
        }
        public string getPresTextS(string langCode)
        {
            return mtRow.texts[langCode].PresTextS;
        }

        public string getContentsVariable(string langCode)
        {
            return mtRow.texts[langCode].ContentsVariable;
        }


        internal string ProductId { get { return mtRow.ProductId; } }
        internal string SubjectCode { get { return mtRow.SubjectCode; } }
        internal string TableId { get { return mtRow.TableId; } }
        internal string TimeScale { get { return mtRow.TimeScale; } }
        internal string TableStatus { get { return mtRow.TableStatus; } }

       
        //<ships PXKeywords="SUBJECT_AREA">MenuSelection.PresText</ships>




        /// <PXKeyword name="DATABASE">
        ///   <rule>
        ///     <description>The value, which is language dependent, is read from the database config file
        ///     descriptions.description element</description>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="MATRIX">
        ///   <rule>
        ///     <description>The value is set to the Precode of the first selected content</description>
        ///     <table modelName ="Contents">
        ///     <column modelName="PresCode"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="MAINTABLE">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="MainTable"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="TABLEID">
        ///   <rule>
        ///     <description>It's only set if the database column contains av value</description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="TableId"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="SUBJECT_CODE">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="SubjectCode"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="SUBJECT_AREA">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="MenuSelection">
        ///     <column modelName="Prestext"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="INFOFILE">
        ///   <rule>
        ///     <description></description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="ProductId"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="CONTENTS">
        ///   <rule>
        ///     <description>If only one content i selected the value from Contents.PresText is used, otherwise the
        ///     MainTable.PresTexS is used </description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="PresTextS"/>
        ///     </table>
        ///     <table modelName ="Contents">
        ///     <column modelName="PresText"/>
        ///     </table>    
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="UNITS">
        ///   <rule>
        ///     <description>Even if UNITS is specified for each contents Paxiom or more correct PX-Axis expect a UNIT keyword for the whole file.
        ///     we take UNIT for the first keyword.</description>
        ///     <table modelName ="CONTENTS">
        ///     <column modelName="UNIT"/>   
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="STUB">
        ///   <rule>
        ///     <description>If variable type is classificaion or time then MainTableVariable.Variable is used.
        ///     If variable is a contentsvariable then the value of PXSqlMeta.mContVariableCode = "ContentsCode" is used.</description>
        ///     <table modelName ="MainTableVariable">
        ///     <column modelName="Variable"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="HEADING">
        ///   <rule>
        ///     <description>If variable type is classificaion or time then MainTableVariable.Variable is used.
        ///     If variable is a contentsvariable then the value of PXSqlMeta.mContVariableCode = "ContentsCode" is used.</description>
        ///     <table modelName ="MainTableVariable">
        ///     <column modelName="Variable"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="DECIMALS">
        ///   <rule>
        ///     <description>If more contents are selected with different StoreDecimals, then DECIMALS is set to the highest one</description>
        ///     <table modelName ="Contents">
        ///     <column modelName="StoreDecimals"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        /// <PXKeyword name="SHOWDECIMALS">
        ///   <rule>
        ///     <description>If more contents are selected with different PresDecimals, then SHOWDECIMALS is set to the lowest one</description>
        ///     <table modelName ="Contents">
        ///     <column modelName="PresDecimals"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
        public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes)
        {
            string noLanguage = null;
            string subkey = null;
            StringCollection values = new StringCollection();

            // MATRIX
            values.Clear();
            values.Add(meta.Contents[meta.FirstContents].PresCode);
            handler(PXKeywords.MATRIX, noLanguage, subkey, values);


            // MAINTABLE

            values.Clear();
            values.Add(this.MainTable);
            handler(PXKeywords.MAINTABLE, noLanguage, subkey, values);


            // TABLEID

            if (!string.IsNullOrEmpty(this.TableId.Trim(' ')))
            {                
                    values.Clear();
                    values.Add(this.TableId);
                    handler(PXKeywords.TABLEID, noLanguage, subkey, values);
                
            }

            // SUBJECT-CODE

            values.Clear();
            values.Add(this.mSubjectCode);
            handler(PXKeywords.SUBJECT_CODE, noLanguage, subkey, values);

            //SUBJECT-AREA

            foreach (string langCode in mSubjectAreaByLanguage.Keys)
            {
                values.Clear();
                values.Add(mSubjectAreaByLanguage[langCode]);
                handler(PXKeywords.SUBJECT_AREA, langCode, subkey, values);
            }

            // DATABASE
              foreach (string langCode in LanguageCodes)
            {
                values.Clear();
                // values.Add(this.mDataStorageRow.DatabaseName);
                //values.Add(meta.Config.Database.id);  // Was a bug. Before database came from DataStorage.DatabaseName
                values.Add(meta.Config.GetDescription(langCode));// Corrected to description
                handler(PXKeywords.DATABASE, langCode, subkey, values);
            }

            //DESCRIPTION
            // Removed should not be sent to PAXIOM 07.04.2011
            // se Reqtest 319.


            // INFOFILE

            values.Clear();
            values.Add(this.ProductId);
            handler(PXKeywords.INFOFILE, noLanguage, subkey, values);




            //PX_SERVER
            // Removed should not be sent to PAXIOM 07.04.2011
            // se Reqtest 324.


            // CONTENTS
            foreach (string langCode in LanguageCodes)
            {
                values.Clear();
                if (this.meta.PXMetaAdmValues.AllwaysUseMaintablePrestextSInDynamicTitle)
                {
                    values.Add(meta.MainTable.getPresTextS(langCode));
                }
                else if (meta.Contents.Count > 1)
                {
                    // Contents should be set to maintable.PresTexts
                    values.Add(meta.MainTable.getPresTextS(langCode));
                }
                else
                {
                    //values.Add(meta.Contents[meta.FirstContents].PresTextS[langCode]);
                    // bugs reported from sweden, PresTextS is not mandatory, PresText should be used.
                    values.Add(meta.Contents[meta.FirstContents].PresText[langCode]);
                }
                handler(PXKeywords.CONTENTS, langCode, subkey, values);
            }

            //UNITS 
            // even if UNITS is specified for each contents Paxiom or more correct PX-Axis expect a UNIT keyword for the hole file.
            // we take UNIT for the first keyword.
            foreach (string langCode in LanguageCodes)
            {
                values.Clear();
                values.Add(meta.Contents[meta.FirstContents].UNIT[langCode]);
                handler(PXKeywords.UNITS, langCode, null, values);
            }

            //STUB
            values.Clear();
            foreach (PXSqlVariable var in meta.Stubs)
            {
                values.Add(var.Name);
            }
            handler(PXKeywords.STUB, noLanguage, subkey, values);
            //HEADING
            values.Clear();
            foreach (PXSqlVariable var in meta.Headings)
            {
                values.Add(var.Name);
            }
            handler(PXKeywords.HEADING, noLanguage, subkey, values);

            // Decimals stuff

            values.Clear();
            values.Add(meta.DecimalHandler.StoreDecimals.ToString());
            handler(PXKeywords.DECIMALS, noLanguage, subkey, values);

            values.Clear();
            values.Add(meta.DecimalHandler.ShowDecimals.ToString());
            handler(PXKeywords.SHOWDECIMALS, noLanguage, subkey, values);
            values = null;
        }

    }
}

