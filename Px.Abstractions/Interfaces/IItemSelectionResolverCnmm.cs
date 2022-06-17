using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Menu;

namespace Px.Abstractions.Interfaces
{
    public interface IItemSelectionResolver
    {
        ItemSelection Resolve(string selection);

    }
}
