[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net_config.xml", Watch = false)]

namespace PCAxis.Sql.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using log4net;
    using DbConfig;

    public abstract class PXSqlData : IDisposable
    {
        /// <summary>The Log</summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlData));
        #region members and propreties

        /// <summary>
        /// Contains the columnnames of the columns needed to construct the id of the cell for DATANOTE CELL/SUM  
        /// </summary>
        private StringCollection DataNoteCellId_Columns = new StringCollection();



        /// <summary>
        /// The datacellnotes are kept in this. Key:celladdress, value: messageCode
        /// </summary>
        protected Dictionary<string, string> _DataNoteCellEntries = new Dictionary<string, string>();

        /// <summary>
        /// The datacellnotes are kept in this. Key:celladdress, value: messageCode
        /// </summary>
        internal Dictionary<string, string> DataNoteCellEntries
        {
            get { return _DataNoteCellEntries; }
        }



        protected int mSize = 0;// = all ValueCount entries multiplied  = the number of cells in the output matrix

        internal int MatrixSize
        {
            get { return mSize; }
        }



        public static PXSqlData GetPXSqlData(PXSqlMeta mPXSqlMeta, SqlDbConfig mSqlDbConfig)
        {

            if (mPXSqlMeta.CNMMVersion.Equals("2.1"))
            {
                return new PCAxis.Sql.Parser_21.PXSqlData_21((PCAxis.Sql.Parser_21.PXSqlMeta_21)mPXSqlMeta);

            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.2"))
            {
                return new PCAxis.Sql.Parser_22.PXSqlData_22((PCAxis.Sql.Parser_22.PXSqlMeta_22)mPXSqlMeta);
            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.3"))
            {
                return new PCAxis.Sql.Parser_23.PXSqlData_23((PCAxis.Sql.Parser_23.PXSqlMeta_23)mPXSqlMeta);
            }
            else if (mPXSqlMeta.CNMMVersion.Equals("2.4"))
            {
                return new PCAxis.Sql.Parser_24.PXSqlData_24((PCAxis.Sql.Parser_24.PXSqlMeta_24)mPXSqlMeta);
            }
            else
            {
                log.Debug("creating PXSqlData_21, but  mPXSqlMeta.CNMMVersion is " + mPXSqlMeta.CNMMVersion);
                return new PCAxis.Sql.Parser_21.PXSqlData_21((PCAxis.Sql.Parser_21.PXSqlMeta_21)mPXSqlMeta);
            }

        }
        #endregion contructors



        /// <summary>
        /// creates and executes the sqlString and returns a double array
        /// </summary>
        /// <returns></returns>
        public abstract double[] CreateMatrix();

        internal abstract string[] DataCellNotes { get; }




        // IDisposable implemenatation

        public abstract void Dispose();

    }
}
