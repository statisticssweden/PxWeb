using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxWeb
{
    public interface IPxCache
    {
        void SetCoherenceChecker(Func<bool> coherenceChecker);
        bool IsEnabled();
        void Clear();
        void Disable();
        void Enable();
        T? Get<T>(Object key);
        void Set(Object key, Object value);
        bool DefaultEnabled { get; }

    }
}