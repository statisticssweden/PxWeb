using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxWeb.Code.Api2.Cache
{
    public interface IPxCache
    {
        void SetCoherenceChecker(Func<bool> coherenceChecker);
        bool IsEnabled();
        void Clear();
        void Disable();
        void Enable();
        T? Get<T>(object key);
        void Set(object key, object value);
        void Set(object key, object value, TimeSpan lifetime);

    }
}