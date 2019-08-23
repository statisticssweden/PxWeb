using System;
using System.Collections.Generic;
using System.Text;
using log4net; 


namespace PCAxis.Sql.Parser_21
{
    public class PxSqlValues:Dictionary<string,PXSqlValue>
    {
        #region contants
        private static readonly ILog log = LogManager.GetLogger(typeof(PxSqlValues));  
        
        #endregion
        internal List<PXSqlValue> GetValuesForSelectedValueset(string selectedValueset)
        {
            List<PXSqlValue> valuesetsValues = new List<PXSqlValue>();
            foreach (PXSqlValue value in this.Values)
            {
                if (selectedValueset == PCAxis.PlugIn.Sql.PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)
                    valuesetsValues.Add(value);
                else
                    if (value.ValueSet == selectedValueset)
                        valuesetsValues.Add(value);
            }
            return valuesetsValues;
        }

        internal List<PXSqlValue> GetValuesSortedByVSValue(List<PXSqlValue> values)
        {
            List<PXSqlValue> sortedValueList = new List<PXSqlValue>();
            foreach (PXSqlValue value in values)
            {
                sortedValueList.Add(value);
            }
            sortedValueList.Sort(PXSqlValue.SortByVsValue());
            return sortedValueList;

        }
        internal List<PXSqlValue> GetValuesSortedByValue(List<PXSqlValue> values)
        {
           
            List<PXSqlValue> sortedValueList = new List<PXSqlValue>();
            foreach (PXSqlValue value in values)
            {
                sortedValueList.Add(value);
            }
            sortedValueList.Sort(PXSqlValue.SortByValue());
            return sortedValueList;

        }

        internal List<PXSqlValue> GetValuesSortedByPxs(List<PXSqlValue> values)
        {
            List<PXSqlValue> sortedValueList = new List<PXSqlValue>();
            foreach (PXSqlValue value in values)
            {
                sortedValueList.Add(value);
            }
            sortedValueList.Sort(PXSqlValue.SortByPxs());
            return sortedValueList;

        }
        // This is the old sorting. Primary sort on Pxs sortorder, secondary on sortorder from database. 
        internal List<PXSqlValue> GetValuesSortedDefault(List<PXSqlValue> values)
        {
            List<PXSqlValue> sortedValueList = new List<PXSqlValue>();
            foreach (PXSqlValue value in values)
            {
                sortedValueList.Add(value);
            }
            sortedValueList.Sort();
            return sortedValueList;

        }
        public PXSqlValue GetValueByContentsCode(string contentsCode)
        {
            foreach (PXSqlValue val in this.Values)
            {
                if (val.ContentsCode == contentsCode)
                {
                    return val;
                }
            }
            return null;
        }
    }
}
