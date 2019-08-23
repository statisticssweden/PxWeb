using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using PCAxis.Sql.QueryLib_24;
using PCAxis.Paxiom;

using System.Collections;  
using log4net; 


using PCAxis.Sql.Pxs;

namespace PCAxis.Sql.Parser_24
{
    public class PXSqlVariableTime:PXSqlVariable
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlVariableTime));  
           #region props  
           private PXSqlTimeScale mTimeScale;  
 
     
           private StringCollection mTimeVal;  
           public StringCollection TimeVal {  
               get { return mTimeVal; }  
           }  
           private bool mHasTimeVal;  
           public bool HasTimeVal {  
               get { return mHasTimeVal; }  
           }
           //private bool mTableContainsMetaOnly;
           #endregion props  


    #region  contructor
        public PXSqlVariableTime(MainTableVariableRow aTVRow, PXSqlMeta_24 meta)
            :base(aTVRow.Variable,meta,false,true,false)
        {
            mStoreColumnNo = int.Parse(aTVRow.StoreColumnNo);
            if (!meta.ConstructedFromPxs)
            {
                mIndex = mStoreColumnNo;
            }
            SetSelected();
            SetPresText();
            SetDefaultPresTextOption();            
            SetTimeValues();
            mTimeScale = new PXSqlTimeScale(meta.MetaQuery.GetTimeScaleRow(meta.MainTable.TimeScale),meta.Config);
            SetTimeVal();
            PossiblyResetPresText();



        }

    #endregion
        internal void SetSelected()
        {
            this.mIsSelected = false;
            if (meta.ConstructedFromPxs)
            {
                if (meta.PxsFile.Query.Time != null)
                    this.mIsSelected = true;
            }
            else
            {
                this.mIsSelected = true;
            }
        }    
        internal override List<PXSqlValue> GetValuesForParsing()
        {
            if ((meta.inPresentationModus) && meta.ConstructedFromPxs )
            {
                if ((int)meta.PxsFile.Query.Time.TimeOption == 0)
                {
                    return mValues.GetValuesSortedByPxs(mValues.GetValuesForSelectedValueset(selectedValueset));
                }
                else
                {
                    return mValues.GetValuesForSelectedValueset(selectedValueset);
                }
            }
            else
            {
                return mValues.GetValuesSortedByValue(mValues.GetValuesForSelectedValueset(selectedValueset));
            }
        }


        /// <PXKeyword name="TIMEVAL">
        ///   <rule>
        ///     <description> </description>
        ///     <table modelName ="MainTable">
        ///     <column modelName="TimeScale"/>
        ///     </table>
        ///     <table modelName ="TimeScale">
        ///     <column modelName="TimeUnit"/>
        ///     </table>
        ///     <table modelName ="ContentsTime">
        ///     <column modelName="TimePeriod"/>
        ///     </table>
        ///   </rule>
        /// </PXKeyword>
             internal override void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, StringCollection LanguageCodes, string preferredLanguage) {  
               base.ParseMeta(handler, LanguageCodes, preferredLanguage);  
               //log.Debug("PPPPPPPPPPPPPPPPPParsing");  
               //TIMEVAL  
               if (mHasTimeVal) {  
                     
                   string language = null;  
                   string subkey = this.Name;  
                   StringCollection values = this.mTimeVal;  
                   handler(PXKeywords.TIMEVAL, language, subkey, values);  
                   values = null;  
               }  
             
           }  


        private void SetDefaultPresTextOption()
        {
            // if prestextoption not set e.g from Pxs then apply PresTextOption from db
            if (string.IsNullOrEmpty(this.PresTextOption))
                this.PresTextOption = meta.Config.Codes.ValuePresC;
        }



        private void SetTimeVal() {  
               
               
               string timeUnit;  
               if (mTimeScale.TimeUnit == meta.Config.Codes.TimeUnitA)  
                   timeUnit = "A1";  
               else if (mTimeScale.TimeUnit == meta.Config.Codes.TimeUnitH)  
                   timeUnit = "H1";  
               else if (mTimeScale.TimeUnit == meta.Config.Codes.TimeUnitM)  
                   timeUnit = "M1";  
               else if (mTimeScale.TimeUnit == meta.Config.Codes.TimeUnitQ)  
                   timeUnit = "Q1";  
               else if (mTimeScale.TimeUnit == meta.Config.Codes.TimeUnitW)  
                   timeUnit = "W1";  
               else
               {
                   log.Warn("Wrong Timeunit:" + mTimeScale.TimeUnit);
                   timeUnit = mTimeScale.TimeUnit;
               }
               
               mTimeVal = new StringCollection();  
               mTimeVal.Add("TLIST("+timeUnit+")");   
               mHasTimeVal = true;
               if (!meta.MainTable.ContainsOnlyMetaData)
               {
                   string lowestSelectedTime;
                   string highestSelectedTime;
                   ArrayList selectedTimeCodes = new ArrayList();
                   foreach (KeyValuePair<string, PXSqlValue> val in mValues)
                   {
                       selectedTimeCodes.Add(val.Value.ValueCode);
                   }
                   selectedTimeCodes.Sort();
                   lowestSelectedTime = selectedTimeCodes[0].ToString();
                   highestSelectedTime = selectedTimeCodes[selectedTimeCodes.Count - 1].ToString();
                   ArrayList allTimeCodes = TimeValueCodesSortedChronologicaly();
                   int startIndexAllTimes = allTimeCodes.IndexOf(lowestSelectedTime);
                   int endIndexAllTimes = allTimeCodes.IndexOf(highestSelectedTime);
                   for (int i = startIndexAllTimes; i <= endIndexAllTimes; i++)
                   {
                       mTimeVal.Add(allTimeCodes[i].ToString());
                       if (!selectedTimeCodes.Contains(allTimeCodes[i]))
                       {
                           //TODO; Dette gjøres midlertidig for å sørge for at Timeval 
                           //vises i serialiseringen
                           mTimeVal.Clear();
                           mTimeVal.Add("TLIST(" + timeUnit + ")");
                           break;
                       }
                   }
               }
           }


        /**
         * resets PresText from "time" to something reflecting the frequency, like "year", if the Timescale.TimescalePres indicates true. 
         */
        private void PossiblyResetPresText()
        {
            if (mTimeScale.UsePresTextFromTimeScale)
            {
                List<string> keys = new List<string>(this.PresText.Keys); 
               
                foreach (string langCode in keys)
                {
                    this.PresText[langCode] =  mTimeScale.getPresText(langCode);
                }

                
            }
        }


        private ArrayList TimeValueCodesSortedChronologicaly()
        {  
               ArrayList sortedTimeValues = new ArrayList();  
               System.Data.DataSet mTimeInfoTbl = meta.MetaQuery.GetAllTimeValues(meta.MainTable.MainTable, "asc");  
                 
               foreach (System.Data.DataRow row in mTimeInfoTbl.Tables[0].Rows) {

                   sortedTimeValues.Add(row[meta.MetaQuery.DB.ContentsTime.TimePeriodCol.PureColumnName()].ToString());  
               }  
               sortedTimeValues.Sort();  
               return sortedTimeValues;  
           }  


        private void SetTimeValues() {
            // time variable values
            //lage flere overloads databasespørringer avhengig av timeopt
            //mValues = new Dictionary<string, PXSqlValue>();



            PxSqlValues tmpValues = null;

            if (meta.MainTable.ContainsOnlyMetaData)
            {
                PXSqlValue fictiveValue = new PXSqlValue("Time01", meta.LanguageCodes);
                this.mValues.Add("Time01", fictiveValue);
            }

            else
            {

                if (meta.ConstructedFromPxs && ((int)meta.PxsFile.Query.Time.TimeOption != 4))
                {


                    switch ((int)meta.PxsFile.Query.Time.TimeOption)
                    {
                        case 0:
                            Dictionary<string, int> mySelectedValues = new Dictionary<string, int>();
                            int docOrder = 1;
                            TimeTypeTimeValues timevalues = (TimeTypeTimeValues)meta.PxsFile.Query.Time.Item;
                            foreach (BasicValueType time in timevalues.TimeValue)
                            {
                                mySelectedValues.Add(time.code, docOrder++);
                            }

                            tmpValues = meta.MetaQuery.GetTimeValueList(meta.MainTable.MainTable, mySelectedValues);
                            break;

                        case 1:
                            tmpValues = meta.MetaQuery.GetTimeValueList(meta.MainTable.MainTable);
                            break;

                        case 2:
                            int NoOfTimeValues = (int)meta.PxsFile.Query.Time.Item;
                            tmpValues = meta.MetaQuery.GetTimeValueList(meta.MainTable.MainTable, NoOfTimeValues);
                            break;

                        case 3:
                            string StartTimeValue = (string)meta.PxsFile.Query.Time.Item;
                            tmpValues = meta.MetaQuery.GetTimeValueList(meta.MainTable.MainTable, StartTimeValue);
                            break;
                    }





                    this.mValues = tmpValues;
                }
                else
                {
                    this.mValues = meta.MetaQuery.GetAllTimeValuesList(meta.MainTable.MainTable, "desc");

                }
            }
        }
    }
}
