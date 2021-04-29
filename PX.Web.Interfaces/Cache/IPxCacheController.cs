using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Web.Interfaces.Cache
{
    public interface IPxCacheController
    {
        void Initialize(List<IPxCache> lstCache);
        void Clear();
        void Clear(Type type);
    }
}
