using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;
using PCAxis.Sql.QueryLib_22;
using System.Collections.Specialized;
using PCAxis.Paxiom;
using log4net;

namespace PCAxis.Sql.Parser_22
{
 public   class PXSqlContent:IComparable<PXSqlContent>
    {
     private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlContent));

     #region value to be used if row is missing

     /// <summary>
     /// must be 0, 2 or 3. 0 indicates presCellsZero = yes 
     /// </summary>
     private string mCategoryOfCellsInMissingRows = "0";
     public string CategoryOfCellsInMissingRows {
         get { return mCategoryOfCellsInMissingRows; }
     }

     /// <summary>
     /// the magicNumber (or 0 when category = 0) 
     /// </summary>
     private double mValueOfCellsInMissingRows = 0;
     public double ValueOfCellsInMissingRows {
         get { return mValueOfCellsInMissingRows; }
     }
     #endregion value to be used if row is missing

     private ContentsRow mRow;

     /// <summary>
     /// Contact is per maintable in the database, but per Content in paxiom. Thus we have to send the same contact for all Content.
     /// </summary>
     private PXSqlContact mContact; 



     public PXSqlContent() { }
     public PXSqlContent(ContentsRow row, PXSqlMeta_22 pxsqlMeta, SqlDbConfig_22 config, PXSqlContact contact) {
         mRow = row;
         mContact = contact;
         //pxsqlMeta.MetaQuery.DB.Codes.Copyright1;
         mContents = row.Contents;
         //mFootnoteContents = row.FootnoteContents;
         //mFootnoteTime = row.FootnoteTime;
         //mFootnoteValue = row.FootnoteValue;
         //mFootnoteVariable = row.FootnoteVariable;
         mPresText = new Dictionary<string, string>();
         mPresTextS = new Dictionary<string, string>();
         mPresCode = row.PresCode;

         mBasePeriod = new Dictionary<string, string>();
         mRefPeriod = new Dictionary<string, string>();
         mUnit = new Dictionary<string, string>();


         foreach (string langCode in pxsqlMeta.LanguageCodes)
         {
             mPresText[langCode] = row.texts[langCode].PresText;
             mPresTextS[langCode] = row.texts[langCode].PresTextS;
             mBasePeriod[langCode] = row.texts[langCode].BasePeriod;
             mRefPeriod[langCode] = row.texts[langCode].RefPeriod;
             mUnit[langCode] = row.texts[langCode].Unit;
         }

         mPresDecimals = mRow.PresDecimals;
         pxsqlMeta.DecimalHandler.ShowDecimals = mPresDecimals;
         pxsqlMeta.DecimalHandler.StoreDecimals = mRow.StoreDecimals;
         
         mSeasAdj = mRow.SeasAdj.Equals(config.Codes.Yes);
         mDayAdj = mRow.DayAdj.Equals(config.Codes.Yes);
         mLastUpdatet = mRow.LastUpdated;
         mStockFA = PaxiomifyStockFA(mRow.StockFA, config);
         mCFPrices = PaxiomifyCFPrices(mRow.CFPrices, config);

         
         mAggregPossible = ! mRow.AggregPossible.Equals(config.Codes.No); //not notPossible since yes is default
        
         #region mCategoryOfCellsInMissingRows and mValueOfCellsInMissingRows 
         if( pxsqlMeta.inPresentationModus &&  mRow.PresCellsZero.Equals(config.Codes.No)){
            
             // both 2.1 and 2.2 uses a mRow.PresMissingLine from the SpecialCharacter.CharacterType( which is the primary key)
             // or if mRow.PresMissingLine is missing: the default

                 mValueOfCellsInMissingRows = pxsqlMeta.mPxsqlNpm.DefaultCodeMissingLineMagic;
                 if (!(  String.IsNullOrEmpty(mRow.PresMissingLine)))
                 {
                     mValueOfCellsInMissingRows = pxsqlMeta.mPxsqlNpm.DataSymbolNMagic(mRow.PresMissingLine);
                 }
                 mCategoryOfCellsInMissingRows = pxsqlMeta.mPxsqlNpm.GetCategory(mValueOfCellsInMissingRows).ToString();

         }
         #endregion mCategoryOfCellsInMissingRows and mValueOfCellsInMissingRows 
     }

    

    private string mContents;
	public string Contents
	{
		get { return mContents;}
		set { mContents = value;}
	}

     private int mSortOrder;
     public int SortOrder
     {
         get { return mSortOrder; }
         set { mSortOrder = value; }
     }

     private Dictionary<string,string> mPresText;
     public Dictionary<string, string> PresText
        {
            get { return mPresText; }
            set { mPresText = value; }
        }

     private Dictionary<string, string> mPresTextS;
     public Dictionary<string, string> PresTextS
        {
            get { return mPresTextS; }
            set { mPresTextS = value; }
        }

     
     private string mPresCode;
        public string PresCode
        {
            get { return mPresCode; }
            set { mPresCode = value; }
        }
        private Dictionary<string, string> mUnit;
        public Dictionary<string, string> UNIT
        {
            get { return mUnit; }
            set { mUnit = value; }
        }



     //Ikke i bruk

     //private string mProducer;
     //   public string Producer
     //   {
     //       get { return mProducer; }
     //       set { mProducer = value; }
     //   }


        #region ContentInfo in Paxiom 

        private Dictionary<string, string> mRefPeriod;
       // private Dictionary<string, string> mUnit;
        private Dictionary<string, string> mBasePeriod;

        private string mLastUpdatet;
        private string mCFPrices;
        private bool mDayAdj;
        private bool mSeasAdj;
        private string mStockFA;

        #endregion ContentInfo in Paxiom


        //Ikke i bruk
        //private string mPublished;
        //public string Published
        //{
        //    get { return mPublished; }
        //    set { mPublished = value; }
        //}

     private string mPresDecimals;
  

        public string PresCellsZero {
            get { return mRow.PresCellsZero; }
        }


        public string PresMissingLine {
            get { return mRow.PresMissingLine; }
        }

     private bool mAggregPossible;
        public bool AggregPossible
        {
            get { return mAggregPossible; }
        }

   

     private string mStoreColumnNo;
        public string StoreColumnNo
        {
            get { return mStoreColumnNo; }
            set { mStoreColumnNo = value; }
        }

     private string mStoreFormat;
        public string StoreFormat
        {
            get { return mStoreFormat; }
            set { mStoreFormat = value; }
        }

     private string mStoreNoChar;
        public string StoreNoChar
        {
            get { return mStoreNoChar; }
            set { mStoreNoChar = value; }
        }

     //private string mStoreDecimals;
     //   public string StoreDecimals
     //   {
     //       get { return mStoreDecimals; }
     //       set { mStoreDecimals = value; }
     //   }

     private string mUserid;
        public string Userid
        {
            get { return mUserid; }
            set { mUserid = value; }
        }

     private string mLogDate;
        public string LogDate
        {
            get { return mLogDate; }
            set { mLogDate = value; }
     
        }
     
    
     public int CompareTo(PXSqlContent compPXSqlContent)
     {
         return mSortOrder.CompareTo(compPXSqlContent.mSortOrder);
     }

     /// <PXKeyword name="BASEPERIOD">
     ///   <rule>
     ///     <description>Language dependent.</description>
     ///     <table modelName ="Contents">
     ///     <column modelName="ValueCode"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="CONTACT">
     ///   <rule>
     ///     <description>Language dependent.</description>
     ///     <table modelName ="Person">
     ///     <column modelName="ForeName"/>
     ///     <column modelName="SureName"/>
     ///     <column modelName="PhonePrefix"/>     
     ///     <column modelName="PhoneNo"/>  
     ///     <column modelName="FaxNo"/>      
     ///     <column modelName="Email"/>      
     ///     </table>
     ///     <table modelName ="Organization">
     ///     <column modelName="OrganizationName"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="REFPERIOD">
     ///   <rule>
     ///     <description>Language dependent.</description>
     ///     <table modelName ="Contents">
     ///     <column modelName="RefPeriod"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="UNITS">
     ///   <rule>
     ///     <description>Language dependent.</description>
     ///     <table modelName ="Contents">
     ///     <column modelName="Unit"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="CFPRICES">
     ///   <rule>
     ///     <description>If value in database equals to CFPricesC code in db config file then set to PXConstant.CFPRICES_CURRENT.
     ///     If value in database equals to CFPricesF code in db config file then set to PXConstant.CFPRICES_FIXED. </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="CFPrices"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="LAST_UPDATED">
     ///   <rule>
     ///     <description> </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="LastUpdated"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="SEASADJ">
     ///   <rule>
     ///     <description> </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="SeasAdj"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="DAYADJ">
     ///   <rule>
     ///     <description> </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="DayAdj"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="STOCKFA">
     ///   <rule>
     ///     <description>Language dependent. Maps the code in dbconfig file to PX PXConstant
     ///      StockFAS-->STOCKFA_STOCK, StockFAF-->STOCKFA_FLOW,StockFAA-->STOCKFA_AVERAGE. </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="StockFa"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="PRECISION">
     ///   <rule>
     ///     <description> </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="PresDecimals"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     /// <PXKeyword name="REFRENCE_ID">
     ///   <rule>
     ///     <description>Holds the Contents.Contents from database so it could be referenced back to from paxiom  </description>
     ///     <table modelName ="Contents">
     ///     <column modelName="Contents"/>
     ///     </table>
     ///   </rule>
     /// </PXKeyword>
     public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes)
     {

         StringCollection values = new StringCollection();

         //string subkey = this.Contents;
         string subkey = this.PresCode; //prescode should be Code i Paxiom. Contents is sent as Reference_is an ouse as code i PXS.

         foreach (string langCode in LanguageCodes) {

             // BASEPERIOD
             values.Clear();
             values.Add(this.mBasePeriod[langCode]);
             handler(PXKeywords.BASEPERIOD, langCode, subkey, values);


             // CONTACT
             values.Clear();
             values.Add(this.mContact.GetBigFatContactString(langCode));
             handler(PXKeywords.CONTACT, langCode, subkey, values);


             // REFPERIOD
             values.Clear();
             values.Add(this.mRefPeriod[langCode]);
             handler(PXKeywords.REFPERIOD, langCode, subkey, values);


             //  UNITS
             values.Clear();
             values.Add(this.mUnit[langCode]);
             handler(PXKeywords.UNITS, langCode, subkey, values);
                
         }

         string noLanguage = null;

         // CFPRICES
         values.Clear();
         values.Add(this.mCFPrices);
         handler(PXKeywords.CFPRICES, noLanguage, subkey, values);

        
         // LAST_UPDATED
         values.Clear();
         values.Add(this.mLastUpdatet);
         handler(PXKeywords.LAST_UPDATED, noLanguage, subkey, values);

         //SEASADJ
         values.Clear();
         if (this.mSeasAdj) {
             values.Add(PXConstant.YES);
         } else {
             values.Add(PXConstant.NO);
         }
         handler(PXKeywords.SEASADJ, noLanguage, subkey, values);

         //DAYADJ
         values.Clear();

         if (this.mDayAdj) {
             values.Add(PXConstant.YES);
         } else {
             values.Add(PXConstant.NO);
         }
         handler(PXKeywords.DAYADJ, noLanguage, subkey, values);

         //STOCKFA
         values.Clear();
         values.Add(this.mStockFA);
         handler(PXKeywords.STOCKFA, noLanguage, subkey, values);

         //PRECISION
         if (!String.IsNullOrEmpty(this.mPresDecimals))
         {
             values.Clear();
             values.Add(this.mPresDecimals);
             String myKey = PXSqlMeta_22.mContVariableCode + "\",\"" + subkey;//for PXModelBuilder.SplittString   A","B","C 
                                                                          // not "A","B","C" 
             log.Debug("Sender precision for " + myKey);
             handler(PXKeywords.PRECISION, noLanguage, myKey, values);
         }

         

         //REFRENCE_ID  Used to hold unique Id of Content in Paxiom. This will be used in pxs to identify selected contents
         // The resason why contents is placed in Code fro the value which represent the contents is that this is occupied by PresCode.
         values.Clear();
         values.Add(this.Contents);
         handler(PXKeywords.REFRENCE_ID, noLanguage, subkey, values);
     }




     internal void AdjustPresDecimalsToCommonDecimals(string Decimals) {
         log.Debug(Decimals.Equals(mPresDecimals));
         if(Decimals.Equals(mPresDecimals)){
             
             mPresDecimals = String.Empty;
         }
     }

     /**
      * Translates database code to Paxiom code for StockFA.
      */
     private string PaxiomifyStockFA(string StockFA,SqlDbConfig_22 config) {
         string myOut = "";
         if (! String.IsNullOrEmpty(StockFA)  ) {
             if (StockFA.Equals(config.Codes.StockFAS)) {
                 myOut = PXConstant.STOCKFA_STOCK;
             } else if (StockFA.Equals(config.Codes.StockFAA)) {
                 myOut = PXConstant.STOCKFA_AVERAGE;
             } else if (StockFA.Equals(config.Codes.StockFAF)) {
                 myOut = PXConstant.STOCKFA_FLOW;
             } else {
                 myOut = PXConstant.STOCKFA_OTHER;
                 //throw new ApplicationException("Unknown StockFA code:\"" + StockFA + "\"");
             }
         }
         return myOut;
     }
     /**Translates database code to Paxiom code for CFPrices*/
     private string PaxiomifyCFPrices(string CFPrices, SqlDbConfig_22 config) {
          string myOut = "";
          if (! String.IsNullOrEmpty(CFPrices)) {
              if (CFPrices.Equals(config.Codes.CFPricesC)) {
                  myOut = PXConstant.CFPRICES_CURRENT;
              } else if (CFPrices.Equals(config.Codes.CFPricesF)) {
                  myOut = PXConstant.CFPRICES_FIXED;
              } else {
                  throw new ApplicationException("Unknown CFPrices code:\"" + CFPrices + "\"");
              }
          }
         return myOut;
     }
 }



	
}
