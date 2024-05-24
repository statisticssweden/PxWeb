using System.Collections.Generic;

namespace PX.Web.Interfaces.Cache
{
    public interface IPxCacheController
    {
        void Initialize(List<IPxCache> lstCache);
        void Clear();
    }
}
