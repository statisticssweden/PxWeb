using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query
{
    /// <summary>
    /// Data access interface for saved queries 
    /// </summary>
    public interface ISavedQueryDatabaseAccessor
    {
        SavedQuery Load(int id);

        int Save(SavedQuery query, int? id = null);

        bool MarkAsRunned(int name);

        bool MarkAsFailed(int name);
    }
}
