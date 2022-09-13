using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public interface IIndex //TODO Should maybe inherit IDisposable
    {
        void BeginWrite(string language);

        void EndWrite(string language);

        void BeginUpdate(string language);

        void EndUpdate(string language);

        void AddEntry(string id, string label, DateTime updated, string[] tags, string category, string source); 

        void UpdateEntry(string id, string label, DateTime updated, string[] tags, string category, string source);

        void RemoveEntry(string id);
    }
}
