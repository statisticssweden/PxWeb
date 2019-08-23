    using System;
using System.Collections.Generic;
using System.Text;

using PCAxis.Sql.QueryLib_24;

namespace PCAxis.Sql.Parser_24
{
    public class PXSqlValueSet
    {
        #region 
        
        private string mValueSet;
        //private string mPresText;
        private Dictionary<string, string> mPresText = new Dictionary<string, string>();
        
        private EliminationAux elimination;
        //private PXSqlValuepool mValuePool;
        private string mValuePoolId;
        private string mValuePres;
        private string mGeoAreaNo;
        private string mMetaId;

       
        private string mSortCodeExists;
        
        private int mNumberOfValues;

        /// <summary>
        /// The list of codes(=ids) of Values groups in the order they should appear in selection or presentation. 
        /// The texts++ is found in PxSqlValues
        /// </summary>
        private List<string> sortedListOfCodes = new List<string>();

        public List<string> SortedListOfCodes
        {
            get { return this.sortedListOfCodes; }
            set { this.sortedListOfCodes = value; }
        }
        #endregion


        public string ValueSet
        {
            get { return this.mValueSet; }
           
        }
        
        public Dictionary<string, string> PresText
        {
            get { return this.mPresText; }
            
        }



        internal string MetaId
        {
            get { return mMetaId; }
        }
   
        public string Elimination
        {
            get { return this.elimination.GetClassicEliminationValue(); }
        }

        internal EliminationAux GetEliminationAux()
        {
            return this.elimination;
        }

        public string ValuePoolId
        {
            get { return this.mValuePoolId; }
        }

        public string ValuePres
        {
            get { return this.mValuePres; }
           
        }
        public string GeoAreaNo
        {
            get { return this.mGeoAreaNo; }
            
        }

        
        public string SortCodeExists
        {
            get { return mSortCodeExists; }
            
        }

        public int NumberOfValues
        {
            get { return mNumberOfValues; }
            set { mNumberOfValues = value; }
        }

        public bool IsDefault { get; set; }

        public PXSqlValueSet() { }
        public PXSqlValueSet(QueryLib_24.ValueSetRow inRow, PXSqlMeta_24 meta, bool isDefaultInGui)
        {
            
            this.mValueSet = inRow.ValueSet;
            this.elimination = new EliminationAux(inRow.EliminationMethod, inRow.EliminationCode, meta.Config.Codes);
            this.mSortCodeExists = inRow.SortCodeExists;
            this.mValuePoolId =  inRow.ValuePool;
            this.mValuePres = inRow.ValuePres;
            this.mGeoAreaNo = inRow.GeoAreaNo;
            this.mMetaId = inRow.MetaId;

            this.IsDefault = isDefaultInGui; 
            


            foreach (string langCode in inRow.texts.Keys) {
               
                //PresText came in version 2.1 and is optional  ...  desciption is up to 200 chars  
                string asPresText = inRow.texts[langCode].PresText;
                if (String.IsNullOrEmpty(asPresText)) {
                    asPresText = inRow.texts[langCode].Description;
                    int gridPosition = asPresText.IndexOf('#');
                    if (gridPosition > 0) {
                        asPresText = asPresText.Substring(0, gridPosition);
                    }
                }
                mPresText[langCode] = asPresText;
               
            }
        //private int mNumberOfValues; is set outside class. Bad thing? Yes
        }

        //for magic all
        public PXSqlValueSet(Dictionary<string, string> presText, string valuePoolId, List<EliminationAux> elims, string sortCodeExists, string valuePres)
        {
            this.mValueSet = PCAxis.PlugIn.Sql.PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS;
            this.mPresText = presText;
            this.mValuePoolId = valuePoolId;
            this.elimination = EliminationAux.MergeEliminations(elims);
            this.mSortCodeExists = sortCodeExists;
            this.mValuePres = valuePres;
            this.mGeoAreaNo = "";
        }
    }
}
