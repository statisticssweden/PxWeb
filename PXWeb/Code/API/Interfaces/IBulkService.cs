using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb.Code.API.Interfaces
{
    public interface IBulkService
    {
        bool CreateBulkFilesForDatabase(string database, string language);
    }
}
