using PCAxis.Menu;
using PCAxis.Menu.Implementations;
using Px.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Abstractions.DataSource
{
    public class CnmmDataSource : IDataSource
    {
        public PxMenuBase CreateMenu(string id, string language)
        {
            //Create database object to return
            DatamodelMenu retMenu = ConfigDatamodelMenu.Create(language);
            retMenu.RootItem.Sort();

            return retMenu;
        }
    }
}
