using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using PCAxis.Paxiom;
using System.Collections.Specialized;

namespace PCAxis.Sql.Parser_22{
    /// <summary> DECIMALS, SHOWDECIMALS and PRECISION all handle decimals in some way.
    /// ParseMeta for PRECISION is in PXsqlContents
    /// 
    /// From the PX-fileformat documentation:
    /// 
    /// DECIMALS : The number of decimals in the table cells. 0 - 15. (0-6 if SHOWDECIMALS is not included). 
    /// Indicates how many decimals will be saved in the PC-Axis file. Written without quotation marks. Compare SHOWDECIMALS.
    /// 
    ///SHOWDECIMALS : The number of decimals to be shown in the table, 0-6. Must be the same or smaller than the number 
    ///stored as indicated by the keyword DECIMALS. If SHOWDECIMALS is not stated in the file the number stated 
    ///by DECIMALS will be used. 
    /// 
    ///PRECISION : Can occur for single values. Determines that the value shall be presented with a number of decimals 
    ///that differs from the keyword SHOWDECIMALS. Is to be written as PRECISION("variable name","value name")=n where 
    ///n is a figure between 1 and 6. The number of decimals for precision must be higher than the number of decimals 
    ///for SHOWDECIMALS to have any effect. 
    /// </summary>
    internal class PXSqlDecimalStuff {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlDecimalStuff));

        internal PXSqlDecimalStuff() { }

        private int mShowDecimals = 6;
        private int mStoreDecimals = 0;
        
        public string ShowDecimals {
            get { return mShowDecimals.ToString(); }
            set {
                int tmpValue = -1;
                try {
                    tmpValue = int.Parse(value);

                } catch (Exception e) {
                    log.Error(e);
                    return;
                }
                if (tmpValue > 6) {
                    throw new ApplicationException("Decimal number cannot be higher than 6. Found " + value);
                } else if (tmpValue < 0) {
                    throw new ApplicationException("Decimal number cannot be lower than 0. Found " + value);
                }
                if (tmpValue < mShowDecimals) {
                    mShowDecimals = tmpValue;
                }
            }
        }


        public string StoreDecimals {

            get { return mStoreDecimals.ToString(); }
            set {
                int tmpValue = -1;
                try {
                    tmpValue = int.Parse(value);

                } catch (Exception e) {
                    log.Error(e);
                    return;
                }
                if (tmpValue > 15) {
                    throw new ApplicationException("Decimal number cannot be higher than 15. Found " + value);
                } else if (tmpValue < 0) {
                    throw new ApplicationException("Decimal number cannot be lower than 0. Found " + value);
                }
                if (tmpValue > mStoreDecimals) {
                    mStoreDecimals = tmpValue;
                }
            }
        }
        // moved to mPXSqlMeta.MainTable.ParseMeta
        //public void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler) {

        //    StringCollection values = new StringCollection();

        //    string noLanguage = null;
        //    string subkey = null;

           
        //    values.Add(mStoreDecimals.ToString());
        //    handler(PXKeywords.DECIMALS, noLanguage, subkey, values);

        //    values.Clear();
        //    values.Add(mShowDecimals.ToString());
        //    handler(PXKeywords.SHOWDECIMALS, noLanguage, subkey, values);
        //}


             
    }
}
