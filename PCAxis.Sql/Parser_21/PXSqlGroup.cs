namespace PCAxis.Sql.Parser_21
{
using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>Stores A = B + C + D +</summary>
    public class PXSqlGroup
    {
        /// <summary>The code of the Item to which the children belong</summary>
        private string parentCode;

        /// <summary>The list of codes of the children</summary>
        private List<string> childCodes = new List<string>();

        /// <summary>Initializes a new instance of the PXSqlGroup class,  with the given parentCode</summary>
        /// <param name="parentCode">The code of the parent item</param>
        public PXSqlGroup(string parentCode) 
        {
            this.parentCode = parentCode;
        }

        /// <summary>Gets the code of the Item to which the children belong</summary>
        public string ParentCode
        {
            get { return this.parentCode; }
        }

        /// <summary>Gets the list containing B,C,,, </summary>
        public IList<string> ChildCodes
        {
            get { return this.childCodes.AsReadOnly(); }
        }

        /// <summary>Adds a code to the list of codes of the children</summary>
        /// <param name="childCode">The code of child item</param>
        public void AddChildCode(string childCode) 
        {
            this.childCodes.Add(childCode);
        }
    }
}
