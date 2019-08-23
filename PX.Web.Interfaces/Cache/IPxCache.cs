using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Web.Interfaces.Cache
{
    public interface IPxCache
    {
        void SetCoherenceChecker (Func<bool> coherenceChecker);
        bool IsEnabled();
        void Clear();
        void Disable();
        void Enable();
        bool DefaultEnabled { get; }

    }
}
