using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.QueryLib_23;

namespace PCAxis.Sql.Parser_23
{
    public class PXSqlSubTable
    {
        public PXSqlSubTable() { }
        //public PXSqlSubTable(SubTableRow row, List<FootnoteRow> footNoteRows)
        //{
        //    mSubTable = row.SubTable;
        //    mFootNoteRows = footNoteRows;     
        //}
        public PXSqlSubTable(SubTableRow row)
        {
            mSubTable = row.SubTable;
            mIsSelected = false;
        }

        private string mSubTable;
        public string SubTable
        {
            get { return mSubTable; }
        }
        //private List<FootnoteRow> mFootNoteRows;
        //public List<FootnoteRow> FootNoteRows
        //{
        //    get { return mFootNoteRows; }
        //}
        private bool mIsSelected;
        public bool IsSelected
        {
            get { return mIsSelected; }
            set { mIsSelected = value; }
        }
    }

}
