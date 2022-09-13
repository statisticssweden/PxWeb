using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public interface IIndex
    {
        void BeginWrite(string language);

        void EndWrite(string language);

        void AddEntry(string id); //TODO more params

        void UpdateEntry(string id); //TODO more params

        void RemoveEntry(string id);
    }
}
