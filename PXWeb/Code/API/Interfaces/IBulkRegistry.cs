using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb.Code.API.Interfaces
{
    public interface IBulkRegistry
    {
        void SetContext(string context);
        bool ShouldTableBeUpdated(string tableId, DateTime lastUpdated);
        void RegisterTableBulkFileUpdated(string tableId, DateTime generationDate);
        void Save();
    }
}
