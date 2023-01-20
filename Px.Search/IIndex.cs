using PCAxis.Paxiom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public interface IIndex : IDisposable
    {
        void BeginWrite(string language);

        void EndWrite(string language);

        void BeginUpdate(string language);

        void EndUpdate(string language);

        //void AddEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta);
        void AddEntry(TableInformation tbl, PXMeta meta);

        //void UpdateEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta);
        void UpdateEntry(TableInformation tbl, PXMeta meta);

        void RemoveEntry(string id);
    }
}
