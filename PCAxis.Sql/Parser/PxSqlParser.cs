using System;

using PCAxis.Paxiom;
using PCAxis.Sql.Parser;

using log4net;

namespace PCAxis.PlugIn.Sql
{
    public abstract class PXSqlParser : IDisposable, PCAxis.Paxiom.IPXModelParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlParser));


        /// <summary>
        /// Factory method returning a PXSqlParser of the same CMMM-version as that of the PXSqlMeta
        /// </summary>
        /// <param name="inPXSqlMeta"></param>
        /// <returns></returns>

        public static PXSqlParser GetPXSqlParser(PXSqlMeta inPXSqlMeta)
        {
            if (inPXSqlMeta.CNMMVersion.Equals("2.2"))
            {
                return new PCAxis.Sql.Parser_22.PXSqlParser_22((PCAxis.Sql.Parser_22.PXSqlMeta_22)inPXSqlMeta);
            }
            else if (inPXSqlMeta.CNMMVersion.Equals("2.3"))
            {
                return new PCAxis.Sql.Parser_23.PXSqlParser_23((PCAxis.Sql.Parser_23.PXSqlMeta_23)inPXSqlMeta);
            }
            else if (inPXSqlMeta.CNMMVersion.Equals("2.4"))
            {
                return new PCAxis.Sql.Parser_24.PXSqlParser_24((PCAxis.Sql.Parser_24.PXSqlMeta_24)inPXSqlMeta);
            }
            else
            {
                return new PCAxis.Sql.Parser_21.PXSqlParser_21((PCAxis.Sql.Parser_21.PXSqlMeta_21)inPXSqlMeta);
            }
        }

        public abstract void ParseMeta(PCAxis.Paxiom.IPXModelParser.MetaHandler handler, string preferredLanguage);

        public virtual void SetCurrentValueSets(PXMeta pxMeta)
        {
            //Overriden for versions >= 2.4
        }

        public virtual void ApplyElimination(PXMeta pxMeta)
        {
            //Overriden for versions >= 2.4
        }


        /// <summary>
        /// ParseData is handled by a _model.Data.Write in the PxSqlBuilder, this is here just to fullfill IPXModelParser
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="preferredBufferSize"></param>
        public void ParseData(IPXModelParser.DataHandler handler, int preferredBufferSize)
        {
            throw new ApplicationException("BUG");
        }

        public abstract void Dispose();

    }
}

