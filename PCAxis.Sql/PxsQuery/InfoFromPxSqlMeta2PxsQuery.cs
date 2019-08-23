using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Sql.Pxs
{

    /// <summary>
    /// Class for transfering the information from PxSqlMeta needed by PxsQuery, for each classificationvariable: the SelectedValuesetId and CurrentGroupingId.
    /// May contain PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS.
    /// </summary>
    public class InfoFromPxSqlMeta2PxsQuery
    {

        private Dictionary<string, string> SelectedValuesetIdByVariableId = new Dictionary<string, string>();
        private Dictionary<string, string> CurrentGroupingIdByVariableId = new Dictionary<string, string>();

        /// <summary>
        /// Empty constructor
        /// </summary>
        public InfoFromPxSqlMeta2PxsQuery()
        { 
        }


        /// <summary>
        /// Adds the SelectedValuesetId for the VariableId
        /// </summary>
        /// <param name="VariableId"></param>
        /// <param name="SelectedValuesetId"></param>
        public void AddSelectedValuesetId(string VariableId, string SelectedValuesetId)
        {
            SelectedValuesetIdByVariableId.Add(VariableId, SelectedValuesetId);
        }

        /// <summary>
        /// Adds the CurrentGroupingId for the VariableId.
        /// </summary>
        /// <param name="VariableId"></param>
        /// <param name="CurrentGroupingId"></param>
        public void AddCurrentGroupingId(string VariableId, string CurrentGroupingId)
        {
            CurrentGroupingIdByVariableId.Add(VariableId, CurrentGroupingId);
        }

        /// <summary>
        /// Gets the valuesetId for the variable.
        /// </summary>
        /// <param name="VariableId">the id of the classification variable</param>
        /// <returns></returns>
        internal string GetSelectedValuesetId(string VariableId)
        {
            return SelectedValuesetIdByVariableId[VariableId];
        }


        /// <summary>
        /// Get the CurrentGroupingId for the variable, which may be null
        /// </summary>
        /// <param name="VariableId">the id of the classification variable</param>
        /// <returns></returns>
        internal string GetCurrentGroupingId(string VariableId)
        {
            return CurrentGroupingIdByVariableId[VariableId];
        }

    }
}
