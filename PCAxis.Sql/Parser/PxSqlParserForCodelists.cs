using System;
using PCAxis.Paxiom;

using PCAxis.Sql.Parser;

using log4net;

namespace PCAxis.PlugIn.Sql {

    /// <summary>
    /// Handles the sending of VALUE and CODE to paxiom 
    /// </summary>
    abstract public class PXSqlParserForCodelists : IDisposable, PCAxis.Paxiom.IPXModelParser {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParserForCodelists));

        /// <summary>
        /// Factorymethod returning the subclass with the CNMM-version of the PXSqlMeta.
        /// </summary>
        /// <param name="inPXSqlMeta"></param>
        /// <param name="variableCode"></param>
        /// <returns></returns>
        public static PXSqlParserForCodelists GetPXSqlParserForCodelists(PXSqlMeta inPXSqlMeta, String variableCode)
        {
            if (inPXSqlMeta.CNMMVersion.Equals("2.2"))
            {
                return new PCAxis.Sql.Parser_22.PXSqlParserForCodelists_22((PCAxis.Sql.Parser_22.PXSqlMeta_22)inPXSqlMeta, variableCode);
            }
            else if (inPXSqlMeta.CNMMVersion.Equals("2.3"))
            {
                return new PCAxis.Sql.Parser_23.PXSqlParserForCodelists_23((PCAxis.Sql.Parser_23.PXSqlMeta_23)inPXSqlMeta, variableCode);
            }
            else if (inPXSqlMeta.CNMMVersion.Equals("2.4"))
            {
                return new PCAxis.Sql.Parser_24.PXSqlParserForCodelists_24((PCAxis.Sql.Parser_24.PXSqlMeta_24)inPXSqlMeta, variableCode);
            }
            else
            {
                return new PCAxis.Sql.Parser_21.PXSqlParserForCodelists_21((PCAxis.Sql.Parser_21.PXSqlMeta_21)inPXSqlMeta, variableCode);
            }
        }


         public abstract void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage);
        
        /// <summary>
         /// The Parser for codelists does not use ParseData, but it has to be here to fullfill the PCAxis.Paxiom.IPXModelParser interface. Throws an exception if called.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="preferredBufferSize"></param>
        public void ParseData(IPXModelParser.DataHandler handler, int preferredBufferSize) {
            throw new ApplicationException("BUG");
        }

        //IDisposable implemenatation:
        public abstract void Dispose();
    }
}


