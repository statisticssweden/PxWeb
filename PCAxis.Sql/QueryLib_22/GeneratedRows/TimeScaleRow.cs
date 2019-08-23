using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_22
{

    /// <summary>
    /// Holds the attributes for TimeScale. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes the timescales that exist in the macro database.
    /// </summary>
    public class TimeScaleRow
    {
        private String mTimeScale;
        /// <summary>
        /// Name of timescale, i.e.Year, Month, Quarter.\nShould not contain dash (applies for retrievals in PC-AXIS).
        /// </summary>
        public String TimeScale
        {
            get { return mTimeScale; }
        }
        private String mTimeScalePres;
        /// <summary>
        /// Shows if the timescale should be presented in table heading instead of the word Time. Can be:\nY = Yes\nN = No\nAt statistics \n,\n Not yet implemented. Is currently"N" for all timescales.
        /// </summary>
        public String TimeScalePres
        {
            get { return mTimeScalePres; }
        }
        private String mRegular;
        /// <summary>
        /// Shows if timescale is regular or not. Can be:\nY = Yes\nN = No\nAn example of an irregular timescale is an election year.\nData is primarily accompanying information when retrieving statistics to a file.
        /// </summary>
        public String Regular
        {
            get { return mRegular; }
        }
        private String mTimeUnit;
        /// <summary>
        /// Code for TimeUnit. Used as accompanying information when retrieving a statistics file. The following alternatives are possible:\nQ = quarter\nA = academic year\nM = month\nX = 3 years\nS = split year\nY = year
        /// </summary>
        public String TimeUnit
        {
            get { return mTimeUnit; }
        }
        private String mFrequency;
        /// <summary>
        /// Shows how many points in time the relevant timescale contains per calendar year, i.e.:\n1 for timescale year,\n4 for timescale quarter,\n12 for timescale month.\nFor irregular and regular timescales, where points in time do not occur consecutively (i.e. every other year), the field should be NULL.
        /// </summary>
        public String Frequency
        {
            get { return mFrequency; }
        }
        private String mStoreFormat;
        /// <summary>
        /// Description of storage format for the point in time in the timescale. There are the following alternatives: \nyyyy for timescales where TimeUnit = Y,\nyyyy-yyyy for timescales where TimeUnit = T,\nyyyy/yy for timescales where TimeUnit = A,\nyyyy/yyyy for timescales where TimeUnit = P,\nyyyyQq for timescales where TimeUnit = Q,\nyyyyMmm for timescales where TimeUnit = M.\nFor a description of time units, see column TimeUnit in the table TimeScale.
        /// </summary>
        public String StoreFormat
        {
            get { return mStoreFormat; }
        }

        public Dictionary<string, TimeScaleTexts> texts = new Dictionary<string, TimeScaleTexts>();

        public TimeScaleRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mTimeScale = myRow[dbconf.TimeScale.TimeScaleCol.Label()].ToString();
            this.mTimeScalePres = myRow[dbconf.TimeScale.TimeScalePresCol.Label()].ToString();
            this.mRegular = myRow[dbconf.TimeScale.RegularCol.Label()].ToString();
            this.mTimeUnit = myRow[dbconf.TimeScale.TimeUnitCol.Label()].ToString();
            this.mFrequency = myRow[dbconf.TimeScale.FrequencyCol.Label()].ToString();
            this.mStoreFormat = myRow[dbconf.TimeScale.StoreFormatCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new TimeScaleTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for TimeScale  for one language.
    /// The table describes the timescales that exist in the macro database.
    /// </summary>
    public class TimeScaleTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case.\nPresentation text used when selecting time when making a retrieval from databases.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal TimeScaleTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.TimeScaleLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.TimeScale.PresTextCol.Label()].ToString();
            }
        }
    }

}
