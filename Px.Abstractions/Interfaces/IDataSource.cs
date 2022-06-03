using PCAxis.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Abstractions.Interfaces
{
    public interface IDataSource
    {

        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        PxMenuBase CreateMenu(string id, string language);

        ///// <summary>
        ///// Create builder
        ///// </summary>
        ///// <param name="dbi">Database</param>
        ///// <param name="menu">Menu</param>
        ///// <param name="selection">Selection</param>
        ///// <param name="language">Language</param>
        ///// <returns></returns>
        //IPXModelBuilder CreateBuilder(IDatabaseInfo dbi, string menu, string selection, string language);

        //string GetSource(IDatabaseInfo dbi, PCAxis.Paxiom.PXModel model, string language);
    }
}
