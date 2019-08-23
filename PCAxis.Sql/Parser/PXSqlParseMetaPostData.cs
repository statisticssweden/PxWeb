namespace PCAxis.Sql.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using PCAxis.PlugIn.Sql;

    using log4net;


    public abstract class PXSqlParseMetaPostData : IDisposable
    {
        /// <summary>The Log</summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParseMetaPostData));



        public static PCAxis.Paxiom.IPXModelParser GetPXSqlParseMetaPostData(PXSqlMeta mPXSqlMeta,PXSqlData mPXSqlData)
        {

            if (mPXSqlMeta.CNMMVersion.Equals("2.1"))
            {
                PCAxis.Sql.Parser_21.PXSqlData_21 mPXSqlData_21 = (PCAxis.Sql.Parser_21.PXSqlData_21)mPXSqlData;
                PCAxis.Sql.Parser_21.PXSqlMeta_21 mPXSqlMeta_21 = (PCAxis.Sql.Parser_21.PXSqlMeta_21)mPXSqlMeta;
                return new PCAxis.Sql.Parser_21.PXSqlParseMetaPostData_21(mPXSqlData_21.DataNoteCellEntries,mPXSqlMeta_21);

            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.2"))
            {
                PCAxis.Sql.Parser_22.PXSqlData_22 mPXSqlData_22 = (PCAxis.Sql.Parser_22.PXSqlData_22)mPXSqlData;
                PCAxis.Sql.Parser_22.PXSqlMeta_22 mPXSqlMeta_22 = (PCAxis.Sql.Parser_22.PXSqlMeta_22)mPXSqlMeta;
                return new PCAxis.Sql.Parser_22.PXSqlParseMetaPostData_22(mPXSqlData_22.DataNoteCellEntries, mPXSqlMeta_22);
            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.3"))
            {
                PCAxis.Sql.Parser_23.PXSqlData_23 mPXSqlData_23 = (PCAxis.Sql.Parser_23.PXSqlData_23)mPXSqlData;
                PCAxis.Sql.Parser_23.PXSqlMeta_23 mPXSqlMeta_23 = (PCAxis.Sql.Parser_23.PXSqlMeta_23)mPXSqlMeta;
                return new PCAxis.Sql.Parser_23.PXSqlParseMetaPostData_23(mPXSqlData_23.DataNoteCellEntries, mPXSqlData_23.AttributesEntries,mPXSqlData_23.DefaultAttributes,mPXSqlMeta_23.Attributes,mPXSqlMeta_23.LanguageCodes, mPXSqlData_23.UsedNPMCharacters, mPXSqlMeta_23);
            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.4"))
            {
                PCAxis.Sql.Parser_24.PXSqlData_24 mPXSqlData_24 = (PCAxis.Sql.Parser_24.PXSqlData_24)mPXSqlData;
                PCAxis.Sql.Parser_24.PXSqlMeta_24 mPXSqlMeta_24 = (PCAxis.Sql.Parser_24.PXSqlMeta_24)mPXSqlMeta;
                return new PCAxis.Sql.Parser_24.PXSqlParseMetaPostData_24(mPXSqlData_24.DataNoteCellEntries, mPXSqlData_24.AttributesEntries, mPXSqlData_24.DefaultAttributes, mPXSqlMeta_24.Attributes, mPXSqlMeta_24.LanguageCodes, mPXSqlData_24.UsedNPMCharacters, mPXSqlMeta_24);
            }
            else
            {
                log.Warn("creating PXSqlParseMetaPostData_21, but  mPXSqlMeta.CNMMVersion is " + mPXSqlMeta.CNMMVersion);
                PCAxis.Sql.Parser_21.PXSqlData_21 mPXSqlData_21 = (PCAxis.Sql.Parser_21.PXSqlData_21)mPXSqlData;
                PCAxis.Sql.Parser_21.PXSqlMeta_21 mPXSqlMeta_21 = (PCAxis.Sql.Parser_21.PXSqlMeta_21)mPXSqlMeta;
                return new PCAxis.Sql.Parser_21.PXSqlParseMetaPostData_21(mPXSqlData_21.DataNoteCellEntries, mPXSqlMeta_21);
            }

        }



        /// <summary>
        /// creates and executes the sqlString and returns a double array
        /// </summary>
        /// <returns></returns>
        public abstract double[] CreateMatrix();





        // IDisposable implemenatation

        public abstract void Dispose();

    }
}
