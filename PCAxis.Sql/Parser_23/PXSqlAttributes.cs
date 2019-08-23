using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using PCAxis.Sql.QueryLib_23;

namespace PCAxis.Sql.Parser_23
{
    class PXSqlAttributes
    {
        private bool mHasAttributes;
        internal bool HasAttributes
        {
            get { return mHasAttributes; }
        }


        private bool mAllAttributesInOneColumn;
        internal bool AllAttributesInOneColumn
        {
            get { return mAllAttributesInOneColumn; }
        }

        private SortedList<string, AttributeRow> mSortedAttributes;
        internal SortedList<string, AttributeRow> SortedAttributes
        {
            get { return mSortedAttributes; }
        }

        Dictionary<string, AttributeRow> mAvailableAttributes;
        internal Dictionary<string, AttributeRow> AvailableAttributes
        {
            get { return mAvailableAttributes; }
        }

        string  mTheOneAndOnlyAttributeColumn;
        internal string TheOneAndOnlyAttributeColumn
        {
            get { return mTheOneAndOnlyAttributeColumn; }
        }

        private PXSqlMeta_23 mMeta;

        //protected Dictionary<string, StringCollection> _AttributesEntries;

        internal PXSqlAttributes( PXSqlMeta_23 meta) 
        {
            this.mMeta = meta;
            mAvailableAttributes = mMeta.MetaQuery.GetAttributeRows(meta.MainTable.MainTable, true);
            if (mAvailableAttributes.Count > 0)
            {
                mHasAttributes = true;
                mSortedAttributes = getAttributesSorted();
                mAllAttributesInOneColumn = CheckIfMoreAttributesInOneColumn();
                if (AllAttributesInOneColumn)
                {
                    mTheOneAndOnlyAttributeColumn = mAvailableAttributes.First().Value.AttributeColumn;
                }
            }
        }

        /// <summary>
        /// Returns attributes order by sequenceNo
        /// </summary>
        private SortedList<string,AttributeRow> getAttributesSorted()
        {
            SortedList<string, AttributeRow> sortedAttributes = new SortedList<string,AttributeRow>();
            foreach (AttributeRow myAttr in this.mAvailableAttributes.Values)
            {
                sortedAttributes.Add(myAttr.SequenceNo, myAttr);
            }
            return sortedAttributes;
        }
        private bool CheckIfMoreAttributesInOneColumn()
        {
            int NumberOfAttrRow = SortedAttributes.Count;
            int NumberOfDistinctAttrColumns = mMeta.MetaQuery.GetNumberOfDistinctAttributeColumns(mMeta.MainTable.MainTable);
            if (NumberOfDistinctAttrColumns == NumberOfAttrRow)
            {
                return false;
            }
            else
            {
                if (NumberOfDistinctAttrColumns > 1)
                {
                    throw new ApplicationException("If more than one attributecolumn is defined there must be one column for each attribute");
                }
                else
                {

                    return true;
                }
            }
        }

    }
}
