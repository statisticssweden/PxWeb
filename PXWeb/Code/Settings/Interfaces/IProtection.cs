using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PX.Security;

namespace PXWeb
{
    public interface IProtection
    {
        bool IsProtected { get; }
        IAuthorization AuthorizationMethod { get; }
    }
}
