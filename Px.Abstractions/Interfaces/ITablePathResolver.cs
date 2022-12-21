using PCAxis.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Abstractions.Interfaces
{
    public interface ITablePathResolver
    {
        string Resolve(string language, string id, out bool selectionExists);
    }
}
