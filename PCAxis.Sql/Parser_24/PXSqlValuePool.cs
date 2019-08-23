using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_24;
using System.Collections.Specialized;
using PCAxis.Paxiom;

namespace PCAxis.Sql.Parser_24
{
    /// <summary>
    /// Data from the rows of the Valuepool(_*) tables, with some small
    /// </summary>
    public class PXSqlValuepool
    {
        private ValuePoolRow vpRow;

        /// <summary>
        /// The id of the valuepool
        /// </summary>
        public string ValuePool
        {
            get { return vpRow.ValuePool; }
        }

        private String mValueTextOption;

        /// <summary>
        /// PXConstant.VALUETEXTOPTION_NOTEXT or _NORMAL . Derived from ValueTextExists
        /// </summary>
        public String ValueTextOption
        {
            get { return mValueTextOption; }
        }

        private Dictionary<string, string> mPresText;
        /// <summary>
        /// Prestation text for the value pool
        /// </summary>
        public Dictionary<string, string> PresText
        {
            get { return mPresText; }
        }
   

        /// <summary>
        /// Which texts exists (long/short/extra/none)
        /// </summary>
        public string ValueTextExists
        {
            get { return vpRow.ValueTextExists; }
        }
        /// <summary>
        /// Which one ( or two :-) should be presented: Code , short or long text/// </summary>
        public string ValuePres
        {
            get { return vpRow.ValuePres; }
        }
       

        // Depending of if it's a primary or secondary language, Doamin is read from different columns in database table ValuePool.
        // Valuepool if primary and ValuePoolEng(2.0) ValuePoolAlias (later). A solution could be to add a property here called Domain.
        // to be discussed in Ørebro 21.03.2011.
        // versjon 2.3: ValuePoolAlias now exists in the ValuePool table for all languages (also main) 
        private Dictionary<string, string> mDomain;

        /// <summary>
        /// For PXKeywords.DOMAIN
        /// Key is language, value is ValuePoolAlias if not null otherwise ValuePool
        /// </summary>
        public Dictionary<string, string> Domain
        {
            get { return mDomain; }
        }

        public string MetaId
        {
            get { return vpRow.MetaId; }
        }

        /// <summary>Loads the data from a row
        /// </summary>
        /// <param name="inRow">The data row</param>
        /// <param name="meta">For config.codes </param>
        public PXSqlValuepool(QueryLib_24.ValuePoolRow inRow, PXSqlMeta_24 meta)
        {
            
            this.vpRow = inRow;

            if (this.ValueTextExists.Equals(meta.Config.Codes.ValueTextExistsN))
            {
                this.mValueTextOption = PXConstant.VALUETEXTOPTION_NOTEXT;
            }
            else
            {
                this.mValueTextOption = PXConstant.VALUETEXTOPTION_NORMAL;
            }

            mPresText = new Dictionary<string, string>();
            mDomain = new Dictionary<string, string>();
            foreach (string langCode in vpRow.texts.Keys)
            {
                mPresText[langCode] = vpRow.texts[langCode].PresText;
                mDomain[langCode] = vpRow.texts[langCode].ValuePoolAlias;
                if(String.IsNullOrEmpty(mDomain[langCode])) {
                    mDomain[langCode] = vpRow.texts[langCode].ValuePoolAlias;
                }
            }

        }

        
    }
}
