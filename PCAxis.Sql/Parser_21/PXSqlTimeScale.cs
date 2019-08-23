using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_21;

namespace PCAxis.Sql.Parser_21
{
    public class PXSqlTimeScale
    {
        private TimeScaleRow tsRow;
       
        private bool doTimeScalePres = false;

        /** True if the timevariable should get its PresText from TIMESCALE,TIMESCALE_ENG ... 
         */
        public Boolean UsePresTextFromTimeScale
        {
            get { return doTimeScalePres; }
        }

        public string TimeScale
        {
            get { return tsRow.TimeScale; }
        }




        
        public string Regular
        {
            get { return tsRow.Regular; }
        }
        public string TimeUnit
        {
            get { return tsRow.TimeUnit; }
        }
        public string Frequency
        {
            get { return tsRow.Frequency; }
        }
        public string StoreFormat
        {
            get { return tsRow.StoreFormat; }
        }

        /**
         * get the PresText for given langCode, check first tsRow.TimeScalePres
         */
        public string getPresText(string langCode)
        {
            
            return tsRow.texts[langCode].PresText;
        }
        
        //Contructor
       
        public PXSqlTimeScale(TimeScaleRow timeScaleRow, DbConfig.SqlDbConfig_21 sqlDbConfig)
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
