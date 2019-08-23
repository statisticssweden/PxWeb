using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_24;

namespace PCAxis.Sql.Parser_24
{
    public class PXSqlTimeScale
    {
        private TimeScaleRow tsRow;
       
        private bool doTimeScalePres = false;

        /// <summary>
        /// True if the timevariable should get its PresText from TIMESCALE,TIMESCALE_ENG ... 
        /// </summary>
        public Boolean UsePresTextFromTimeScale
        {
            get { return doTimeScalePres; }
        }

        /*
        public string TimeScale
        {
            get { return tsRow.TimeScale; }
        }
         */



        /*
        
        public string Regular
        {
            get { return tsRow.Regular; }
        }
         */


        public string TimeUnit
        {
            get { return tsRow.TimeUnit; }
        }
        /*
        public string Frequency
        {
            get { return tsRow.Frequency; }
        }
         */

        /*
        public string StoreFormat
        {
            get { return tsRow.StoreFormat; }
        }
         */

        /// <summary>
        /// get the PresText for given langCode, check first tsRow.TimeScalePres
        /// </summary>
        public string getPresText(string langCode)
        {
            
            return tsRow.texts[langCode].PresText;
        }
        
        
       
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="timeScaleRow">the row to create from</param>
        /// <param name="sqlDbConfig">current config</param>
        public PXSqlTimeScale(TimeScaleRow timeScaleRow, DbConfig.SqlDbConfig_24 sqlDbConfig)
        {
            //TODO; Complete member initialization
            this.tsRow = timeScaleRow;
            if (tsRow.TimeScalePres != null)
            {
                doTimeScalePres = tsRow.TimeScalePres.Equals(sqlDbConfig.Codes.Yes);
            }
        }
    }
}
