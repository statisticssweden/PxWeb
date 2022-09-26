using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public interface ISearchBackend
    {
        IIndex GetIndex();
        ISearcher GetSearcher();
    }
}
