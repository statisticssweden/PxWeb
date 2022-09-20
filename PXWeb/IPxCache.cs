using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxWeb
{
    public interface IPxCache<S, T>
    {
        void SetCoherenceChecker(Func<bool> coherenceChecker);
        bool IsEnabled();
        void Clear();
        void Disable();
        void Enable();
        T Get(S key);
        void Set(S key, T value);
        bool DefaultEnabled { get; }

    }
}